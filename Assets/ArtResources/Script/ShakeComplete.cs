//=========================================================================
//	Created Date:   2016-06-22
//	Author: Jinzhou.Shi
//	Email:shijinzhou@gmail.com
//	Summary:
//
//
//
//
//=========================================================================

using UnityEngine;
public class ShakeComplete : MonoBehaviour
{
    private Animator animator;
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void RePlay()
    {
        animator.SetBool("bShake",true);
    }

    public void ArenaPlay()
    {
        animator.SetBool("bArena", true);
    }

    public void Stop()
    {
//        animator.SetBool("bIdle", true);
        animator.CrossFade("Idle",0.2f);
    }


    public void OnShakeComplete()
    {

    }
}
