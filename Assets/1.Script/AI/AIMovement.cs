using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : MonoBehaviour
{
    public Transform player;
    public float aiMoveDistance = 10f; // AI�� �� ĭ �̵� �Ÿ�
    public float playerMoveDistance = 5f; // �÷��̾��� �̵� �Ÿ�
    public float rayDistance = 10f; // ����ĳ��Ʈ �Ÿ�
    public float boundaryLimit = 100f; // ��� ����
    public LayerMask wallLayer;
    public float moveSpeed = 5f; // AI�� �̵� �ӵ�

    private Vector3 lastPlayerPosition;
    private Vector3 targetPosition;
    private bool hasMoved = false;
    private bool hasRedPotionEffect = false;
    private bool isIdle = true;
    private Animator avatarAnimator;
    public float safeDistance = 2;

    void Start()
    {
        lastPlayerPosition = player.position;
        targetPosition = transform.position; // �ʱ� ��ǥ ��ġ�� ���� ��ġ�� ����
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

                // �̹� �̵��� ������ ����صΰ� ��ǥ �������� �̵� ����
            }
        }

        if (hasMoved)
        {
            // ��ǥ �������� �̵�
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // �̵� ���� �� Run �ִϸ��̼� ����
            if (Vector3.Distance(transform.position, targetPosition) >= 0.1f)
            {
                avatarAnimator.SetTrigger("Run");
            }
                else
                {
                    avatarAnimator.ResetTrigger("Run");
                    avatarAnimator.SetTrigger("Idle");
                }
            // ��ǥ ������ �������� ��
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                hasMoved = false;
                isIdle = true;

                // ��ǥ ������ �������ڸ��� ȸ�� �ִϸ��̼� ����
                Vector3 bestDirection = GetBestFleeDirection();
                if (bestDirection != Vector3.zero)
                {
                    // ȸ�� ���⿡ ���� ȸ�� �ִϸ��̼� ����
                    var rotationComponent = GetComponentInChildren<AI_Rotation>();
                    if (rotationComponent != null)
                    {
                        rotationComponent.SetRotationForDirection(bestDirection);
                    }
                }
                avatarAnimator.ResetTrigger("Run");
                avatarAnimator.SetTrigger("Idle"); // ��ǥ ������ �����ϸ� Idle �ִϸ��̼� ����
            }
        }
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