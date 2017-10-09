using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public abstract class CreatureActions : MonoBehaviour {


    protected Animator myAnimator;
    protected RuntimeAnimatorController myAnimationController;
    public bool isHitStunned;

    // Use this for initialization
    protected virtual void Start ()
    {
        myAnimator = GetComponent<Animator>();
        myAnimationController = myAnimator.runtimeAnimatorController;
    }
    private void HitStunChecker()
    {
        if(GetComponent<Creature>().hitStunDuration > 0)
        {
            isHitStunned = true;
        }
        else
        {
            isHitStunned = false;
        }
    }
}
