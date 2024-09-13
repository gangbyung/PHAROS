using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AnimTigers : MonoBehaviour
{
    public Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void IdelAnim()
    {
        animator.SetTrigger("Idle");
    }

}
