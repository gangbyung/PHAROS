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
        // 현재 오브젝트의 forward 벡터와 목표 방향 간의 dot product 계산
        float forwardDot = Vector3.Dot(transform.forward, direction);
        float rightDot = Vector3.Dot(transform.right, direction);

        if (forwardDot > 0.9f) // 거의 정면
        {
            Debug.Log("앞으로 회전");
            StartCoroutine(Animcor());
        }
        else if (forwardDot < -0.9f) // 거의 뒤
        {
            Debug.Log("뒤로 회전");
            StartCoroutine(AnimcorBack());
        }
        else if (rightDot > 0.9f) // 거의 오른쪽
        {
            Debug.Log("오른쪽으로 회전");
            StartCoroutine(AnimcorRight());
        }
        else if (rightDot < -0.9f) // 거의 왼쪽
        {
            Debug.Log("왼쪽으로 회전");
            StartCoroutine(AnimcorLeft());
        }
    }
    IEnumerator Animcor()
    {
        yield return new WaitForSeconds(0.3f);
        avatarAnimator.SetTrigger("Idle");  // Idle 상태를 기본으로 유지
        yield return null;
    }
    IEnumerator AnimcorLeft()
    {
        yield return new WaitForSeconds(0.15f);
        avatarAnimator.SetTrigger("Turn90 Left");  // Idle 상태를 기본으로 유지
        yield return null;

    }
    IEnumerator AnimcorRight()
    {
        yield return new WaitForSeconds(0.15f);
        avatarAnimator.SetTrigger("Turn90 Right");  // Idle 상태를 기본으로 유지
        yield return null;

    }
    IEnumerator AnimcorBack()
    {
        yield return new WaitForSeconds(0.15f);
        avatarAnimator.SetTrigger("Turn180");  // Idle 상태를 기본으로 유지
        yield return null;

    }
}
