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
            Debug.Log("������ ȸ��");
            StartCoroutine(Animcor());
            //avatarAnimator.SetTrigger("Idle");  // Idle ���¸� �⺻���� ����
        }
        else if (direction == transform.TransformDirection(Vector3.back))
        {
            Debug.Log("�ڷ� ȸ��");  // 180�� ȸ�� �ִϸ��̼�
            StartCoroutine(AnimcorBack());
        }
        else if (direction == transform.TransformDirection(Vector3.left))
        {
            Debug.Log("�������� ȸ��");
            StartCoroutine(AnimcorLeft());
            // 90�� ���� ȸ��
        }
        else if (direction == transform.TransformDirection(Vector3.right))
        {
            Debug.Log("���������� ȸ��");
            StartCoroutine(AnimcorRight());
            // 90�� ������ ȸ��
        }
    }
    IEnumerator Animcor()
    {
        yield return new WaitForSeconds(0.5f);
        avatarAnimator.SetTrigger("Idle");  // Idle ���¸� �⺻���� ����
        yield return null;
    }
    IEnumerator AnimcorLeft()
    {
        yield return new WaitForSeconds(0.3f);
        avatarAnimator.SetTrigger("Turn90 Left");  // Idle ���¸� �⺻���� ����
        yield return null;

    }
    IEnumerator AnimcorRight()
    {
        yield return new WaitForSeconds(0.3f);
        avatarAnimator.SetTrigger("Turn90 Right");  // Idle ���¸� �⺻���� ����
        yield return null;

    }
    IEnumerator AnimcorBack()
    {
        yield return new WaitForSeconds(0.3f);
        avatarAnimator.SetTrigger("Turn180");  // Idle ���¸� �⺻���� ����
        yield return null;

    }
}
