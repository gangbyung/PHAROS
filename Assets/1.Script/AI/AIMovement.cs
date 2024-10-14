using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : MonoBehaviour
{
    public Transform player;
    public float aiMoveDistance = 10f;
    public float playerMoveDistance = 5f;
    public float rayDistance = 10f;
    public float boundaryLimit = 100f;
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
    public float maxRunTime = 7f; // 최대 7초 동안 Run 애니메이션 허용
    private Transform tigerTransform; // 하위 Tiger 오브젝트 참조

    void Start()
    {
        lastPlayerPosition = player.position;
        targetPosition = transform.position;
        avatarAnimator = GetComponentInChildren<Animator>();
        tigerTransform = transform.Find("Tiger"); // Tiger 오브젝트 찾기

        if (avatarAnimator == null)
        {
            Debug.LogError("Avatar Animator is not assigned and could not be found in children!");
        }

        if (tigerTransform == null)
        {
            Debug.LogError("Tiger object not found!");
        }
    }

    void Update()
    {
        playerMoveDistance = hasRedPotionEffect ? 38f : 4.8f;

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
                    runAnimationTime = 0f; // 이동 시작 시 애니메이션 시간 초기화
                }
            }

            if (hasMoved)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

                // 목표 지점에 도달하지 않은 경우
                if (Vector3.Distance(transform.position, targetPosition) >= 0.1f)
                {
                    avatarAnimator.SetTrigger("Run");
                    runAnimationTime += Time.deltaTime; // Run 애니메이션 시간 누적

                    // 7초가 지나면 강제로 Idle 상태로 전환하고 회전 초기화
                    if (runAnimationTime >= maxRunTime)
                    {
                        ForceIdleAndResetRotation();
                    }
                }

                // 목표 지점에 도달한 경우
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

    // Run 애니메이션 시간이 7초 이상되면 Idle 상태로 전환하고 Tiger 오브젝트의 회전을 0, 90, 180, 270으로 변경
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

        foreach (Vector3 direction in possibleDirections)
        {
            if (Physics.Raycast(transform.position, direction, rayDistance, LayerMask.GetMask("Player")))
            {
                validDirections.Remove(direction);
            }
        }

        foreach (Vector3 direction in validDirections)
        {
            Vector3 tempTargetPosition = transform.position + direction * aiMoveDistance;

            if (Mathf.Abs(tempTargetPosition.x) < boundaryLimit && Mathf.Abs(tempTargetPosition.z) < boundaryLimit)
            {
                if (!Physics.Raycast(transform.position, direction, rayDistance, wallLayer))
                {
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
        return !(Physics.Raycast(position, transform.TransformDirection(Vector3.forward), safeDistance, wallLayer) ||
                 Physics.Raycast(position, transform.TransformDirection(Vector3.back), safeDistance, wallLayer) ||
                 Physics.Raycast(position, transform.TransformDirection(Vector3.left), safeDistance, wallLayer) ||
                 Physics.Raycast(position, transform.TransformDirection(Vector3.right), safeDistance, wallLayer));
    }

    public void SetRedPotionEffect(bool isActive)
    {
        hasRedPotionEffect = isActive;
    }
}
