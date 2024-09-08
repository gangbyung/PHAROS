using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LangE : MonoBehaviour
{
    public Transform player;
    public float aiMoveDistance = 10f; // AI�� �� ĭ �̵� �Ÿ�
    public float playerMoveDistance = 5f; // �÷��̾��� �̵� �Ÿ�
    public float rayDistance = 10f; // ����ĳ��Ʈ �Ÿ�
    public float safeDistance = 2f; // ������ ���� �Ÿ�
    public float boundaryLimit = 100f; // ��� ����
    public LayerMask wallLayer;
    public float moveSpeed = 5f; // AI�� �̵� �ӵ�

    private Vector3 lastPlayerPosition;
    private Vector3 targetPosition;
    private bool hasMoved = false; // AI�� �̵��ߴ��� ���θ� ����
    private bool hasRedPotionEffect = false; // ���� ���� ȿ�� ����

    void Start()
    {
        lastPlayerPosition = player.position;
        targetPosition = transform.position; // �ʱ� ��ǥ ��ġ�� ���� ��ġ�� ����
    }

    void Update()
    {
        if (Vector3.Distance(player.position, lastPlayerPosition) >= playerMoveDistance)
        {
            if (!hasMoved)
            {
                // AI�� �̵��� ���� �ĺ��� (4����)
                Vector3[] possibleDirections = {
                    Vector3.forward,
                    Vector3.back,
                    Vector3.left,
                    Vector3.right
                };

                Vector3 bestDirection = Vector3.zero;
                float maxDistance = float.MinValue;

                // �� �������� ����ĳ��Ʈ�� ����
                foreach (Vector3 direction in possibleDirections)
                {
                    Vector3 tempTargetPosition = transform.position + direction * (hasRedPotionEffect ? aiMoveDistance / 4f : aiMoveDistance);

                    // ��� ���� �˻�
                    if (Mathf.Abs(tempTargetPosition.x) < boundaryLimit && Mathf.Abs(tempTargetPosition.z) < boundaryLimit)
                    {
                        // �� ����
                        if (!Physics.Raycast(transform.position, direction, rayDistance, wallLayer))
                        {
                            // ���� �Ÿ� �˻�
                            if (IsSafeFromWalls(tempTargetPosition))
                            {
                                // �÷��̾�κ��� ���� �� ���� ����
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

                // ���� �� �������� ��ǥ ��ġ ����
                if (bestDirection != Vector3.zero)
                {
                    targetPosition = transform.position + bestDirection * (hasRedPotionEffect ? aiMoveDistance / 4f : aiMoveDistance);
                    lastPlayerPosition = player.position;
                    hasMoved = true;
                }
            }

            // ��ǥ ��ġ�� õõ�� �̵�
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // ��ǥ ��ġ�� �����ߴ��� Ȯ���ϰ� �̵� �÷��� ����
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
