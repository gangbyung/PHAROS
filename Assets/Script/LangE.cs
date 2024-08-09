using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LangE : MonoBehaviour
{
    public Transform player;
    public float moveDistance = 10f; // 한 칸 이동 거리
    public float rayDistance = 10f; // 레이캐스트 거리
    public float safeDistance = 2f; // 벽과의 안전 거리
    public float boundaryLimit = 100f; // 경계 제한
    public LayerMask wallLayer;
    public float moveSpeed = 2f; // AI의 이동 속도

    private Vector3 lastPlayerPosition;
    private Vector3 targetPosition;

    void Start()
    {
        lastPlayerPosition = player.position;
        targetPosition = transform.position; // 초기 목표 위치를 현재 위치로 설정
    }

    void Update()
    {
        // 플레이어의 이동 감지
        if (Vector3.Distance(player.position, lastPlayerPosition) >= moveDistance)
        {
            // 플레이어가 이동한 방향 계산
            Vector3 playerMoveDirection = (player.position - lastPlayerPosition).normalized;

            // AI가 이동할 방향 후보들
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
                Vector3 tempTargetPosition = transform.position + direction * moveDistance;

                // 경계 제한 검사
                if (Mathf.Abs(tempTargetPosition.x) < boundaryLimit && Mathf.Abs(tempTargetPosition.z) < boundaryLimit)
                {
                    // 벽 감지
                    if (!Physics.Raycast(transform.position, direction, rayDistance, wallLayer))
                    {
                        // 안전 거리 검사
                        if (IsSafeFromWalls(tempTargetPosition, direction))
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
                targetPosition = transform.position + bestDirection * moveDistance;
            }

            // 마지막 플레이어 위치 업데이트
            lastPlayerPosition = player.position;
        }

        // 목표 위치로 천천히 이동
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    bool IsSafeFromWalls(Vector3 position, Vector3 direction)
    {
        RaycastHit hit;
        // 벽과의 안전 거리를 유지하기 위해 레이캐스트 발사
        if (Physics.Raycast(position, direction, out hit, safeDistance, wallLayer))
        {
            return false;
        }
        return true;
    }

    void OnDrawGizmos()
    {
        // 디버그 용도로 레이캐스트를 시각화
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.forward * rayDistance);
        Gizmos.DrawRay(transform.position, Vector3.back * rayDistance);
        Gizmos.DrawRay(transform.position, Vector3.left * rayDistance);
        Gizmos.DrawRay(transform.position, Vector3.right * rayDistance);
    }
}
