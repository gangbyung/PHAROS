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
            StartCoroutine(Animcor());
            //avatarAnimator.SetTrigger("Idle");  // Idle 상태를 기본으로 유지
        }
        else if (direction == transform.TransformDirection(Vector3.back))
        {
            Debug.Log("뒤로 회전");  // 180도 회전 애니메이션
            StartCoroutine(AnimcorBack());
        }
        else if (direction == transform.TransformDirection(Vector3.left))
        {
            Debug.Log("왼쪽으로 회전");
            StartCoroutine(AnimcorLeft());
            // 90도 왼쪽 회전
        }
        else if (direction == transform.TransformDirection(Vector3.right))
        {
            Debug.Log("오른쪽으로 회전");
            StartCoroutine(AnimcorRight());
            // 90도 오른쪽 회전
        }
    }
    IEnumerator Animcor()
    {
        yield return new WaitForSeconds(0.5f);
        avatarAnimator.SetTrigger("Idle");  // Idle 상태를 기본으로 유지
        yield return null;
    }
    IEnumerator AnimcorLeft()
    {
        yield return new WaitForSeconds(0.3f);
        avatarAnimator.SetTrigger("Turn90 Left");  // Idle 상태를 기본으로 유지
        yield return null;

    }
    IEnumerator AnimcorRight()
    {
        yield return new WaitForSeconds(0.3f);
        avatarAnimator.SetTrigger("Turn90 Right");  // Idle 상태를 기본으로 유지
        yield return null;

    }
    IEnumerator AnimcorBack()
    {
        yield return new WaitForSeconds(0.3f);
        avatarAnimator.SetTrigger("Turn180");  // Idle 상태를 기본으로 유지
        yield return null;

    }
}
