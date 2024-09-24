using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AnimTigers : MonoBehaviour
{
    public Animator animator;
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }
    public void IdelAnim()
    {
        animator.SetTrigger("Idle2");
    }

}
