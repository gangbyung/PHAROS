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
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (direction == transform.TransformDirection(Vector3.back))
        {
            Debug.Log("�ڷ� ȸ��");
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (direction == transform.TransformDirection(Vector3.left))
        {
            Debug.Log("�������� ȸ��");
            avatarAnimator.SetTrigger("Turn90 Right");
            transform.rotation = Quaternion.Euler(0, -90, 0);
        }
        else if (direction == transform.TransformDirection(Vector3.right))
        {
            Debug.Log("���������� ȸ��");
            avatarAnimator.SetTrigger("Turn90 Left");
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }
    }
}
