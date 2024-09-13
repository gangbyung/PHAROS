using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAIMovement : MonoBehaviour
{
    public Transform player; // 플레이어의 위치
    public float moveDistance = 10f; // AI의 이동 거리
    public float moveSpeed = 5f; // AI의 이동 속도
    public float boundaryLimit = 100f; // 경계 제한
    public LayerMask wallLayer; // 벽이 있는 레이어
    private Vector3 targetPosition; // AI가 이동할 목표 위치
    private bool isMoving = false; // AI가 이동 중인지 여부
    private Animator animator; // 애니메이션 컨트롤러

    void Start()
    {
        targetPosition = transform.position; // 처음에는 현재 위치를 목표로 설정
        animator = GetComponentInChildren<Animator>(); // AI의 애니메이션 컴포넌트 가져오기
    }

    void Update()
    {
        // 플레이어가 일정 거리 이상 이동했을 때 AI가 움직임
        if (Vector3.Distance(player.position, transform.position) >= moveDistance && !isMoving)
        {
            Vector3 moveDirection = GetMoveDirection(); // 이동할 방향 계산
            if (moveDirection != Vector3.zero)
            {
                targetPosition = transform.position + moveDirection * moveDistance;
                isMoving = true;
                animator.SetBool("isRun", true); // 달리는 애니메이션 실행
            }
        }

        // AI가 목표 위치로 이동
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                isMoving = false;
                animator.SetBool("isRun", false); // 이동이 끝나면 멈추는 애니메이션 실행
            }
        }
    }

    // AI가 벽에 부딪히지 않고 움직일 수 있는 방향 계산
    Vector3 GetMoveDirection()
    {
        Vector3[] directions = { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };
        foreach (Vector3 direction in directions)
        {
            Vector3 newPosition = transform.position + direction * moveDistance;
            if (IsValidPosition(newPosition)) // 벽과 충돌하지 않는 위치일 때 이동 방향 설정
            {
                return direction;
            }
        }
        return Vector3.zero; // 이동할 수 없는 경우
    }

    // AI가 벽에 부딪히지 않는지 확인
    bool IsValidPosition(Vector3 position)
    {
        if (Mathf.Abs(position.x) > boundaryLimit || Mathf.Abs(position.z) > boundaryLimit)
        {
            return false; // 경계 밖이면 이동 불가
        }

        if (Physics.Raycast(transform.position, position - transform.position, moveDistance, wallLayer))
        {
            return false; // 벽이 있으면 이동 불가
        }

        return true; // 이동 가능
    }
}
