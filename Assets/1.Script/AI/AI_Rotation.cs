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
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (direction == transform.TransformDirection(Vector3.back))
        {
            Debug.Log("뒤로 회전");
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (direction == transform.TransformDirection(Vector3.left))
        {
            Debug.Log("왼쪽으로 회전");
            avatarAnimator.SetTrigger("Turn90 Right");
            transform.rotation = Quaternion.Euler(0, -90, 0);
        }
        else if (direction == transform.TransformDirection(Vector3.right))
        {
            Debug.Log("오른쪽으로 회전");
            avatarAnimator.SetTrigger("Turn90 Left");
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }
    }
}
