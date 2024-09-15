using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Rotation : MonoBehaviour
{
    public Animator avatarAnimator;

    void Start()
    {
        if (avatarAnimator == null)
        {
            Debug.LogError("Avatar Animator is not assigned!");
        }
    }

    public void SetRotationForDirection(Vector3 direction)
    {
        if (direction == transform.TransformDirection(Vector3.forward))
        {
            Debug.Log("앞으로 회전");
            avatarAnimator.SetTrigger("Idle");  // Idle 상태를 기본으로 유지
        }
        else if (direction == transform.TransformDirection(Vector3.back))
        {
            Debug.Log("뒤로 회전");
            avatarAnimator.SetTrigger("Turn180");  // 180도 회전 애니메이션
        }
        else if (direction == transform.TransformDirection(Vector3.left))
        {
            Debug.Log("왼쪽으로 회전");
            avatarAnimator.SetTrigger("Turn90 Left");  // 90도 왼쪽 회전
        }
        else if (direction == transform.TransformDirection(Vector3.right))
        {
            Debug.Log("오른쪽으로 회전");
            avatarAnimator.SetTrigger("Turn90 Right");  // 90도 오른쪽 회전
        }
    }
}
