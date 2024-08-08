using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    public GameObject targetObject;
    public Button LeftrotateButton; // 버튼
    public Button RightrotateButton;
    public float rotationSpeed = 90f;
    private Quaternion targetRotation;
    private bool LeftshouldRotate = false;
    private bool RightshouldRotate = false;

    public float moveSpeed = 2f; // 이동 속도
    public float maxDistance = 10f; // 최대 이동 거리
    private bool isMovingForward = false; // 이동 상태 추적

    private Vector3 startPosition; // 이동 시작 위치
    private void Awake()
    {
        LeftrotateButton.onClick.AddListener(OnLeftRotateButtonClick);
        RightrotateButton.onClick.AddListener(OnRightRotateButtonClick);
    }
    private void Start()
    {
        //if (LeftrotateButton != null)
        //{
        //    LeftrotateButton.onClick.AddListener(OnLeftRotateButtonClick);
        //}
        //if (RightrotateButton != null)
        //{
        //    RightrotateButton.onClick.AddListener(OnRightRotateButtonClick);
        //}
        if (targetObject != null)
        {
            // 현재 회전 상태로 목표 회전 초기화
            targetRotation = targetObject.transform.rotation;
        }
        else
        {
            Debug.LogWarning("타겟 오브젝트가 null입니다.");
        }
    }
    void Update()
    {
        if (isMovingForward) //앞으로 이동하는 로직
        {
            // 이동
            Vector3 move = transform.forward * moveSpeed * Time.deltaTime;
            transform.Translate(move, Space.World);

            // 이동한 거리 계산
            float distanceTravelled = Vector3.Distance(startPosition, transform.position);
            if (distanceTravelled >= maxDistance)
            {
                StopMovingForward();
            }
        }

        if (LeftshouldRotate && targetObject != null) //왼쪽으로 회전하는 로직
        {
            // 현재 회전과 목표 회전 사이를 부드럽게 보간
            targetObject.transform.rotation = Quaternion.RotateTowards(
                targetObject.transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );

            // 목표 회전에 도달하면 회전을 멈춥니다.
            if (Quaternion.Angle(targetObject.transform.rotation, targetRotation) < 0.1f)
            {
                LeftshouldRotate = false;
            }
        }
        if (RightshouldRotate && targetObject != null) //왼쪽으로 회전하는 로직
        {
            // 현재 회전과 목표 회전 사이를 부드럽게 보간
            targetObject.transform.rotation = Quaternion.RotateTowards(
                targetObject.transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );

            // 목표 회전에 도달하면 회전을 멈춥니다.
            if (Quaternion.Angle(targetObject.transform.rotation, targetRotation) < 0.1f)
            {
                RightshouldRotate = false;
            }
        }
    }

    // 버튼이 눌렸을 때 호출될 메서드
    public void StartMovingForward()
    {
        isMovingForward = true;
        startPosition = transform.position; // 이동 시작 위치 저장
    }

    // 버튼이 놓였을 때 또는 이동이 멈출 때 호출될 메서드
    public void StopMovingForward()
    {
        isMovingForward = false;
    }
    void OnLeftRotateButtonClick() //왼쪽 버튼클릭 이벤트
    {
        if (targetObject != null)
        {
            // 현재 회전 상태를 기준으로 목표 회전을 -90도씩 증가시킴
            targetRotation = Quaternion.Euler(
                targetObject.transform.eulerAngles.x,
                targetObject.transform.eulerAngles.y - 90f,
                targetObject.transform.eulerAngles.z
            );

            LeftshouldRotate = true;
        }
    }
    void OnRightRotateButtonClick() //오른쪽 버튼 클릭 이벤트
    {
        if (targetObject != null)
        {
            // 현재 회전 상태를 기준으로 목표 회전을 +90도씩 증가시킴
            targetRotation = Quaternion.Euler(
                targetObject.transform.eulerAngles.x,
                targetObject.transform.eulerAngles.y + 90f,
                targetObject.transform.eulerAngles.z
            );

            RightshouldRotate = true;
        }
    }
}
