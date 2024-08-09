using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    public float touchSensitivity = 0.1f;
    public Transform cameraTransform; // 카메라의 Transform
    public Transform playerTransform; // 플레이어의 Transform
    public float sensitivity = 0.1f;

    private float xRotation = 0f;
    private float yRotation = 0f;
    public float yRotationLimit = 40f; // 좌우 회전 각도 제한

    void Start()
    {
        // 초기 회전 값 설정
        Vector3 initialRotation = cameraTransform.localRotation.eulerAngles;
        xRotation = initialRotation.x;
        yRotation = initialRotation.y;
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                float touchX = touch.deltaPosition.x * sensitivity;
                float touchY = touch.deltaPosition.y * sensitivity;

                xRotation -= touchY;
                xRotation = Mathf.Clamp(xRotation, -40f, 40f);

                yRotation += touchX;
                yRotation = Mathf.Clamp(yRotation, -yRotationLimit, yRotationLimit);

                // 플레이어의 방향을 기준으로 카메라의 시선 방향 설정
                cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
                playerTransform.localRotation = Quaternion.Euler(0f, yRotation, 0f);
            }
        }
    }
}
