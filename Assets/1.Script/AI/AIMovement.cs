using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : MonoBehaviour
{
    public static AIMovement Instance { get; private set; } // �̱��� �ν��Ͻ�

    public Transform player;
    public float aiMoveDistance = 10f;
    public float playerMoveDistance = 5f;
    public float rayDistance = 10f;
    public double boundaryLimit = 100000000f;
    public LayerMask wallLayer;
    public float moveSpeed = 5f;
    public float playerPredictionFactor = 2f;
    public int futureSteps = 3;

    private Vector3 lastPlayerPosition;
    private Vector3 targetPosition;
    private bool hasMoved = false;
    private bool hasRedPotionEffect = false;
    private bool isIdle = true;
    private Animator avatarAnimator;
    public float safeDistance = 2;

    private float runAnimationTime = 0f; // Run �ִϸ��̼� �ð� ����
    public float maxRunTime = 1f; // �ִ� 7�� ���� Run �ִϸ��̼� ���
    private Transform tigerTransform; // ���� Tiger ������Ʈ ����

    private void Awake()
    {
        // �̱��� �ν��Ͻ��� �����ϰ�, �ٸ� �ν��Ͻ��� ���� ��� �ı�
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // �̹� �ν��Ͻ��� �����ϸ� ���ο� ������Ʈ�� �ı�
        }
    }

    void Start()
    {
        GameObject player1 = GameObject.FindWithTag("Player");
        player = player1.transform;
        lastPlayerPosition = player.position;
        targetPosition = transform.position;

        avatarAnimator = GetComponentInChildren<Animator>();
        tigerTransform = transform.Find("Tiger");

        if (avatarAnimator == null)
        {
            Debug.LogError("Avatar Animator is not assigned and could not be found in children!");
        }

        if (tigerTransform == null)
        {
            Debug.LogError("Tiger object not found!");
        }
        

        // �� �ε� �� �ʱ�ȭ
        //ResetAIState();
    }

    void ResetAIState()
    {
        lastPlayerPosition = player.position;
        targetPosition = transform.position;
        isIdle = true;
        hasMoved = false;
        runAnimationTime = 0f;
    }

    void Update()
    {
        playerMoveDistance = hasRedPotionEffect ? 38f : 4.8f;

        // �÷��̾���� �Ÿ� üũ ����ĳ��Ʈ �ð�ȭ
        Debug.DrawRay(transform.position, (player.position - transform.position).normalized * rayDistance, Color.red);

        if (Vector3.Distance(player.position, lastPlayerPosition) >= playerMoveDistance)
        {
            if (isIdle)
            {
                Vector3 bestDirection = GetBestFleeDirection();
                if (bestDirection != Vector3.zero)
                {
                    targetPosition = transform.position + bestDirection * (hasRedPotionEffect ? aiMoveDistance / 6f : aiMoveDistance);
                    lastPlayerPosition = player.position;
                    hasMoved = true;
                    isIdle = false;
                    runAnimationTime = 0f;

                    // ���õ� ������ �ð�ȭ
                    Debug.DrawRay(transform.position, bestDirection * aiMoveDistance, Color.green, 1f);
                }
            }

            if (hasMoved)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

                if (Vector3.Distance(transform.position, targetPosition) >= 0.1f)
                {
                    avatarAnimator.SetTrigger("Run");
                    runAnimationTime += Time.deltaTime; // Run �ִϸ��̼� �ð� ����
                    HandleRunAnimation();
                }

                if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
                {
                    hasMoved = false;
                    isIdle = true;
                    runAnimationTime = 0f; // �̵��� �������Ƿ� �ð� �ʱ�ȭ
                    avatarAnimator.SetTrigger("Idle");

                    Vector3 bestDirection = GetBestFleeDirection();
                    if (bestDirection != Vector3.zero)
                    {
                        var rotationComponent = GetComponentInChildren<AI_Rotation>();
                        if (rotationComponent != null)
                        {
                            rotationComponent.SetRotationForDirection(bestDirection);
                        }
                    }
                }
            }
        }
    }


    void HandleRunAnimation()
    {
        AnimatorStateInfo stateInfo = avatarAnimator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("Run") && stateInfo.loop && stateInfo.normalizedTime >= 3f)
        {
            ForceIdleAndResetRotation();
        }

        if (runAnimationTime >= maxRunTime)
        {
            ForceIdleAndResetRotation();
        }
    }

    Vector3 GetBestFleeDirection()
    {
        Vector3[] possibleDirections = {
        transform.TransformDirection(Vector3.forward),
        transform.TransformDirection(Vector3.back),
        transform.TransformDirection(Vector3.left),
        transform.TransformDirection(Vector3.right)
    };

        Vector3 bestDirection = Vector3.zero;
        float maxSafeScore = float.MinValue;

        List<Vector3> validDirections = new List<Vector3>(possibleDirections);

        // Raycast ���� ��ġ�� ����
        Vector3 adjustedPosition = new Vector3(transform.position.x, 2f, transform.position.z);

        // ���� �����ϴ� Raycast üũ
        foreach (Vector3 direction in possibleDirections)
        {
            if (Physics.Raycast(adjustedPosition, direction, rayDistance, LayerMask.GetMask("Player")))
            {
                validDirections.Remove(direction);
                Debug.DrawRay(adjustedPosition, direction * rayDistance, Color.red, 0.5f);
            }
        }

        foreach (Vector3 direction in validDirections)
        {
            Vector3 tempTargetPosition = adjustedPosition + direction * aiMoveDistance;

            if (Mathf.Abs(tempTargetPosition.x) < boundaryLimit && Mathf.Abs(tempTargetPosition.z) < boundaryLimit)
            {
                if (!Physics.Raycast(adjustedPosition, direction, rayDistance, wallLayer))
                {
                    Debug.DrawRay(adjustedPosition, direction * rayDistance, Color.cyan, 0.5f);

                    if (IsSafeFromWalls(tempTargetPosition))
                    {
                        float safeScore = CalculateFutureSafety(tempTargetPosition);
                        if (safeScore > maxSafeScore)
                        {
                            maxSafeScore = safeScore;
                            bestDirection = direction;
                        }
                    }
                }
            }
        }

        return bestDirection;
    }

    float CalculateFutureSafety(Vector3 aiPosition)
    {
        float totalSafeScore = 0f;
        Vector3 simulatedPlayerPosition = player.position;

        for (int step = 0; step < futureSteps; step++)
        {
            Vector3 predictedPlayerPosition = simulatedPlayerPosition + player.forward * playerPredictionFactor;
            float distanceToAI = Vector3.Distance(aiPosition, predictedPlayerPosition);
            totalSafeScore += distanceToAI;

            simulatedPlayerPosition = predictedPlayerPosition;
        }

        return totalSafeScore;
    }

    bool IsSafeFromWalls(Vector3 position)
    {
        float rayLength = safeDistance;

        // �� ������ ���� ���θ� ���� Ȯ��
        bool isForwardSafe = !Physics.Raycast(position, transform.TransformDirection(Vector3.forward), rayLength, wallLayer);
        bool isBackSafe = !Physics.Raycast(position, transform.TransformDirection(Vector3.back), rayLength, wallLayer);
        bool isLeftSafe = !Physics.Raycast(position, transform.TransformDirection(Vector3.left), rayLength, wallLayer);
        bool isRightSafe = !Physics.Raycast(position, transform.TransformDirection(Vector3.right), rayLength, wallLayer);

        // ����� �ð�ȭ
        Debug.DrawRay(position, transform.TransformDirection(Vector3.forward) * rayLength, isForwardSafe ? Color.green : Color.red, 0.2f);
        Debug.DrawRay(position, transform.TransformDirection(Vector3.back) * rayLength, isBackSafe ? Color.green : Color.red, 0.2f);
        Debug.DrawRay(position, transform.TransformDirection(Vector3.left) * rayLength, isLeftSafe ? Color.green : Color.red, 0.2f);
        Debug.DrawRay(position, transform.TransformDirection(Vector3.right) * rayLength, isRightSafe ? Color.green : Color.red, 0.2f);

        // �� ���� ��� �����ؾ� ������ �������� ����
        return isForwardSafe && isBackSafe && isLeftSafe && isRightSafe;
    }

    // ForceIdleAndResetRotation �Լ� �߰�
    void ForceIdleAndResetRotation()
    {
        hasMoved = false;
        isIdle = true;
        avatarAnimator.SetTrigger("Idle");
        runAnimationTime = 0f; // �ð� �ʱ�ȭ

        // Tiger�� ���� Y�� ȸ�� ���� 0, 90, 180, 270 �� ���� ����� ������ ����
        if (tigerTransform != null)
        {
            Vector3 tigerRotation = tigerTransform.eulerAngles;
            float closestRotation = GetClosestRotation(tigerRotation.y);
            tigerTransform.rotation = Quaternion.Euler(tigerRotation.x, closestRotation, tigerRotation.z);
        }

        Debug.Log("Run �ִϸ��̼��� 7�� �̻� ���ӵǾ� ������ Idle ���·� ��ȯ�Ǿ����ϴ�.");
    }

    // ���� Y�� ȸ�� ���� ���� ����� 0, 90, 180, 270 �� �ϳ��� ��ȯ�ϴ� �Լ�
    float GetClosestRotation(float currentYRotation)
    {
        float[] possibleRotations = { 0f, 90f, 180f, 270f };
        float closestRotation = possibleRotations[0];
        float minDifference = Mathf.Abs(currentYRotation - closestRotation);

        foreach (float rotation in possibleRotations)
        {
            float difference = Mathf.Abs(currentYRotation - rotation);
            if (difference < minDifference)
            {
                closestRotation = rotation;
                minDifference = difference;
            }
        }

        return closestRotation;
    }

    public void SetRedPotionEffect(bool isActive)
    {
        hasRedPotionEffect = isActive;
    }
}
