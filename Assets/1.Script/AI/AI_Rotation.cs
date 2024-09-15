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
            avatarAnimator.SetTrigger("Idle");  // Idle ���¸� �⺻���� ����
        }
        else if (direction == transform.TransformDirection(Vector3.back))
        {
            Debug.Log("�ڷ� ȸ��");
            avatarAnimator.SetTrigger("Turn180");  // 180�� ȸ�� �ִϸ��̼�
        }
        else if (direction == transform.TransformDirection(Vector3.left))
        {
            Debug.Log("�������� ȸ��");
            avatarAnimator.SetTrigger("Turn90 Left");  // 90�� ���� ȸ��
        }
        else if (direction == transform.TransformDirection(Vector3.right))
        {
            Debug.Log("���������� ȸ��");
            avatarAnimator.SetTrigger("Turn90 Right");  // 90�� ������ ȸ��
        }
    }
}
