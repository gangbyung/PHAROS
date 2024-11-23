using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : MonoBehaviour
{
    public static AIMovement Instance { get; private set; } // 싱글톤 인스턴스

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

    private float runAnimationTime = 0f; // Run 애니메이션 시간 추적
    public float maxRunTime = 1f; // 최대 7초 동안 Run 애니메이션 허용
    private Transform tigerTransform; // 하위 Tiger 오브젝트 참조

    private void Awake()
    {
        // 싱글톤 인스턴스를 설정하고, 다른 인스턴스가 있을 경우 파괴
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // 이미 인스턴스가 존재하면 새로운 오브젝트는 파괴
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
        

        // 씬 로드 후 초기화
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

        // 플레이어와의 거리 체크 레이캐스트 시각화
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

                    // 선택된 방향을 시각화
                    Debug.DrawRay(transform.position, bestDirection * aiMoveDistance, Color.green, 1f);
                }
            }

            if (hasMoved)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

                if (Vector3.Distance(transform.position, targetPosition) >= 0.1f)
                {
                    avatarAnimator.SetTrigger("Run");
                    runAnimationTime += Time.deltaTime; // Run 애니메이션 시간 누적
                    HandleRunAnimation();
                }

                if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
                {
                    hasMoved = false;
                    isIdle = true;
                    runAnimationTime = 0f; // 이동이 끝났으므로 시간 초기화
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

        // Raycast 시작 위치를 높임
        Vector3 adjustedPosition = new Vector3(transform.position.x, 2f, transform.position.z);

        // 벽을 감지하는 Raycast 체크
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

        // 네 방향의 안전 여부를 각각 확인
        bool isForwardSafe = !Physics.Raycast(position, transform.TransformDirection(Vector3.forward), rayLength, wallLayer);
        bool isBackSafe = !Physics.Raycast(position, transform.TransformDirection(Vector3.back), rayLength, wallLayer);
        bool isLeftSafe = !Physics.Raycast(position, transform.TransformDirection(Vector3.left), rayLength, wallLayer);
        bool isRightSafe = !Physics.Raycast(position, transform.TransformDirection(Vector3.right), rayLength, wallLayer);

        // 디버깅 시각화
        Debug.DrawRay(position, transform.TransformDirection(Vector3.forward) * rayLength, isForwardSafe ? Color.green : Color.red, 0.2f);
        Debug.DrawRay(position, transform.TransformDirection(Vector3.back) * rayLength, isBackSafe ? Color.green : Color.red, 0.2f);
        Debug.DrawRay(position, transform.TransformDirection(Vector3.left) * rayLength, isLeftSafe ? Color.green : Color.red, 0.2f);
        Debug.DrawRay(position, transform.TransformDirection(Vector3.right) * rayLength, isRightSafe ? Color.green : Color.red, 0.2f);

        // 네 방향 모두 안전해야 안전한 지점으로 간주
        return isForwardSafe && isBackSafe && isLeftSafe && isRightSafe;
    }

    // ForceIdleAndResetRotation 함수 추가
    void ForceIdleAndResetRotation()
    {
        hasMoved = false;
        isIdle = true;
        avatarAnimator.SetTrigger("Idle");
        runAnimationTime = 0f; // 시간 초기화

        // Tiger의 현재 Y축 회전 값을 0, 90, 180, 270 중 가장 가까운 값으로 설정
        if (tigerTransform != null)
        {
            Vector3 tigerRotation = tigerTransform.eulerAngles;
            float closestRotation = GetClosestRotation(tigerRotation.y);
            tigerTransform.rotation = Quaternion.Euler(tigerRotation.x, closestRotation, tigerRotation.z);
        }

        Debug.Log("Run 애니메이션이 7초 이상 지속되어 강제로 Idle 상태로 전환되었습니다.");
    }

    // 현재 Y축 회전 값에 가장 가까운 0, 90, 180, 270 중 하나를 반환하는 함수
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
