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
        StartCoroutine(Idle());
    }
    IEnumerator Idle()
    {
        yield return new WaitForSeconds(.1f);
        animator.SetTrigger("Idle");

        yield return null;
    }
}
