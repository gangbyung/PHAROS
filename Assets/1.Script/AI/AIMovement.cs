using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAIMovement : MonoBehaviour
{
    public Transform player; // �÷��̾��� ��ġ
    public float moveDistance = 10f; // AI�� �̵� �Ÿ�
    public float moveSpeed = 5f; // AI�� �̵� �ӵ�
    public float boundaryLimit = 100f; // ��� ����
    public LayerMask wallLayer; // ���� �ִ� ���̾�
    private Vector3 targetPosition; // AI�� �̵��� ��ǥ ��ġ
    private bool isMoving = false; // AI�� �̵� ������ ����
    private Animator animator; // �ִϸ��̼� ��Ʈ�ѷ�

    void Start()
    {
        targetPosition = transform.position; // ó������ ���� ��ġ�� ��ǥ�� ����
        animator = GetComponentInChildren<Animator>(); // AI�� �ִϸ��̼� ������Ʈ ��������
    }

    void Update()
    {
        // �÷��̾ ���� �Ÿ� �̻� �̵����� �� AI�� ������
        if (Vector3.Distance(player.position, transform.position) >= moveDistance && !isMoving)
        {
            Vector3 moveDirection = GetMoveDirection(); // �̵��� ���� ���
            if (moveDirection != Vector3.zero)
            {
                targetPosition = transform.position + moveDirection * moveDistance;
                isMoving = true;
                animator.SetBool("isRun", true); // �޸��� �ִϸ��̼� ����
            }
        }

        // AI�� ��ǥ ��ġ�� �̵�
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                isMoving = false;
                animator.SetBool("isRun", false); // �̵��� ������ ���ߴ� �ִϸ��̼� ����
            }
        }
    }

    // AI�� ���� �ε����� �ʰ� ������ �� �ִ� ���� ���
    Vector3 GetMoveDirection()
    {
        Vector3[] directions = { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };
        foreach (Vector3 direction in directions)
        {
            Vector3 newPosition = transform.position + direction * moveDistance;
            if (IsValidPosition(newPosition)) // ���� �浹���� �ʴ� ��ġ�� �� �̵� ���� ����
            {
                return direction;
            }
        }
        return Vector3.zero; // �̵��� �� ���� ���
    }

    // AI�� ���� �ε����� �ʴ��� Ȯ��
    bool IsValidPosition(Vector3 position)
    {
        if (Mathf.Abs(position.x) > boundaryLimit || Mathf.Abs(position.z) > boundaryLimit)
        {
            return false; // ��� ���̸� �̵� �Ұ�
        }

        if (Physics.Raycast(transform.position, position - transform.position, moveDistance, wallLayer))
        {
            return false; // ���� ������ �̵� �Ұ�
        }

        return true; // �̵� ����
    }
}
