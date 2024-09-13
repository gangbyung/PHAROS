using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : MonoBehaviour
{
    public Transform player;
    public float aiMoveDistance = 10f; // AI의 한 칸 이동 거리
    public float playerMoveDistance = 5f; // 플레이어의 이동 거리
    public float rayDistance = 10f; // 레이캐스트 거리
    public float boundaryLimit = 100f; // 경계 제한
    public LayerMask wallLayer;
    public float moveSpeed = 5f; // AI의 이동 속도

    private Vector3 lastPlayerPosition;
    private Vector3 targetPosition;
    private bool hasMoved = false;
    private bool hasRedPotionEffect = false;
    private bool isIdle = true;
    private Animator avatarAnimator;

    void Start()
    {
        lastPlayerPosition = player.position;
        targetPosition = transform.position; // 초기 목표 위치를 현재 위치로 설정
        avatarAnimator = GetComponentInChildren<Animator>();
        if (avatarAnimator == null)
        {
            Debug.LogError("Avatar Animator is not assigned and could not be found in children!");
        }
    }

    void Update()
    {
        if (Vector3.Distance(player.position, lastPlayerPosition) >= playerMoveDistance)
        {
            if (isIdle)
            {
                Vector3 bestDirection = GetBestFleeDirection();
                if (bestDirection != Vector3.zero)
                {
                    targetPosition = transform.position + bestDirection * (hasRedPotionEffect ? aiMoveDistance / 4f : aiMoveDistance);
                    lastPlayerPosition = player.position;
                    hasMoved = true;
                    isIdle = false;

                    // Set rotation in a different class
                    // Assumes there is a component named AI_Rotation attached to the child object
                    var rotationComponent = GetComponentInChildren<AI_Rotation>();
                    if (rotationComponent != null)
                    {
                        rotationComponent.SetRotationForDirection(bestDirection);
                    }
                }
            }

            if (hasMoved)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
                {
                    hasMoved = false;
                    isIdle = true;
                }
            }
        }

        avatarAnimator.SetBool("isIdle", isIdle);
        avatarAnimator.SetBool("isRun", !isIdle);
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
        float maxDistance = float.MinValue;

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
            Vector3 tempTargetPosition = transform.position + direction * (hasRedPotionEffect ? aiMoveDistance / 4f : aiMoveDistance);

            if (Mathf.Abs(tempTargetPosition.x) < boundaryLimit && Mathf.Abs(tempTargetPosition.z) < boundaryLimit)
            {
                if (!Physics.Raycast(transform.position, direction, rayDistance, wallLayer))
                {
                    if (IsSafeFromWalls(tempTargetPosition))
                    {
                        float distanceToPlayer = Vector3.Distance(tempTargetPosition, player.position);
                        if (distanceToPlayer > maxDistance)
                        {
                            maxDistance = distanceToPlayer;
                            bestDirection = direction;
                        }
                    }
                }
            }
        }

        return bestDirection;
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

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * rayDistance);
        Gizmos.DrawRay(transform.position, transform.TransformDirection(Vector3.back) * rayDistance);
        Gizmos.DrawRay(transform.position, transform.TransformDirection(Vector3.left) * rayDistance);
        Gizmos.DrawRay(transform.position, transform.TransformDirection(Vector3.right) * rayDistance);
    }
}
