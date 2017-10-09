using UnityEngine;
using System.Collections;

public abstract class CreatureAttack : CreatureActions {



    protected virtual void Attack(string attackName)
    {
        myAnimator.Play(attackName);
    }
}
