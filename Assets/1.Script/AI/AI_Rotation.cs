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
        // ���� ������Ʈ�� forward ���Ϳ� ��ǥ ���� ���� dot product ���
        float forwardDot = Vector3.Dot(transform.forward, direction);
        float rightDot = Vector3.Dot(transform.right, direction);

        if (forwardDot > 0.9f) // ���� ����
        {
            Debug.Log("������ ȸ��");
            StartCoroutine(Animcor());
        }
        else if (forwardDot < -0.9f) // ���� ��
        {
            Debug.Log("�ڷ� ȸ��");
            StartCoroutine(AnimcorBack());
        }
        else if (rightDot > 0.9f) // ���� ������
        {
            Debug.Log("���������� ȸ��");
            StartCoroutine(AnimcorRight());
        }
        else if (rightDot < -0.9f) // ���� ����
        {
            Debug.Log("�������� ȸ��");
            StartCoroutine(AnimcorLeft());
        }
    }
    IEnumerator Animcor()
    {
        yield return new WaitForSeconds(0.3f);
        avatarAnimator.SetTrigger("Idle");  // Idle ���¸� �⺻���� ����
        yield return null;
    }
    IEnumerator AnimcorLeft()
    {
        yield return new WaitForSeconds(0.15f);
        avatarAnimator.SetTrigger("Turn90 Left");  // Idle ���¸� �⺻���� ����
        yield return null;

    }
    IEnumerator AnimcorRight()
    {
        yield return new WaitForSeconds(0.15f);
        avatarAnimator.SetTrigger("Turn90 Right");  // Idle ���¸� �⺻���� ����
        yield return null;

    }
    IEnumerator AnimcorBack()
    {
        yield return new WaitForSeconds(0.15f);
        avatarAnimator.SetTrigger("Turn180");  // Idle ���¸� �⺻���� ����
        yield return null;

    }
}
