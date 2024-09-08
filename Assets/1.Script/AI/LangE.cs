using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LangE : MonoBehaviour
{
    public Transform player;
    public float aiMoveDistance = 10f; // AI의 한 칸 이동 거리
    public float playerMoveDistance = 5f; // 플레이어의 이동 거리
    public float rayDistance = 10f; // 레이캐스트 거리
    public float safeDistance = 2f; // 벽과의 안전 거리
    public float boundaryLimit = 100f; // 경계 제한
    public LayerMask wallLayer;
    public float moveSpeed = 5f; // AI의 이동 속도

    private Vector3 lastPlayerPosition;
    private Vector3 targetPosition;
    private bool hasMoved = false; // AI가 이동했는지 여부를 추적
    private bool hasRedPotionEffect = false; // 빨간 포션 효과 여부

    void Start()
    {
        lastPlayerPosition = player.position;
        targetPosition = transform.position; // 초기 목표 위치를 현재 위치로 설정
    }

    void Update()
    {
        if (Vector3.Distance(player.position, lastPlayerPosition) >= playerMoveDistance)
        {
            if (!hasMoved)
            {
                // AI가 이동할 방향 후보들 (4방향)
                Vector3[] possibleDirections = {
                    Vector3.forward,
                    Vector3.back,
                    Vector3.left,
                    Vector3.right
                };

                Vector3 bestDirection = Vector3.zero;
                float maxDistance = float.MinValue;

                // 각 방향으로 레이캐스트를 보냄
                foreach (Vector3 direction in possibleDirections)
                {
                    Vector3 tempTargetPosition = transform.position + direction * (hasRedPotionEffect ? aiMoveDistance / 4f : aiMoveDistance);

                    // 경계 제한 검사
                    if (Mathf.Abs(tempTargetPosition.x) < boundaryLimit && Mathf.Abs(tempTargetPosition.z) < boundaryLimit)
                    {
                        // 벽 감지
                        if (!Physics.Raycast(transform.position, direction, rayDistance, wallLayer))
                        {
                            // 안전 거리 검사
                            if (IsSafeFromWalls(tempTargetPosition))
                            {
                                // 플레이어로부터 가장 먼 방향 선택
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

                // 가장 먼 방향으로 목표 위치 설정
                if (bestDirection != Vector3.zero)
                {
                    targetPosition = transform.position + bestDirection * (hasRedPotionEffect ? aiMoveDistance / 4f : aiMoveDistance);
                    lastPlayerPosition = player.position;
                    hasMoved = true;
                }
            }

            // 목표 위치로 천천히 이동
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // 목표 위치에 도달했는지 확인하고 이동 플래그 리셋
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                hasMoved = false;
            }
        }
    }

    bool IsSafeFromWalls(Vector3 position)
    {
        if (Physics.Raycast(position, Vector3.forward, safeDistance, wallLayer) ||
            Physics.Raycast(position, Vector3.back, safeDistance, wallLayer) ||
            Physics.Raycast(position, Vector3.left, safeDistance, wallLayer) ||
            Physics.Raycast(position, Vector3.right, safeDistance, wallLayer))
        {
            return false;
        }
        return true;
    }

    public void SetRedPotionEffect(bool isActive)
    {
        hasRedPotionEffect = isActive;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.forward * rayDistance);
        Gizmos.DrawRay(transform.position, Vector3.back * rayDistance);
        Gizmos.DrawRay(transform.position, Vector3.left * rayDistance);
        Gizmos.DrawRay(transform.position, Vector3.right * rayDistance);
    }
}
