using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour {


    public enum AbilityName
    {
        TestAbility1,
        TestAbility2,
        TestAbility3,
        TestAbility4,

    }




    public AbilityName abilityName;
    public float maxCooldown;
    public float cooldown;
    public int maxCharges;
    public int charges;
    public GameObject myUser;

    protected virtual void Awake()
    {
        myUser = this.gameObject;
        cooldown = 0f;
        charges = maxCharges;
    }



    protected virtual void Start()
    {

    }

    protected virtual void FixedUpdate()
    {
    }

    protected virtual void Update()
    {

    }

    protected virtual void ActivateAbility()
    {
        charges -= 1;
    }
    protected virtual void RegenerateCharges(int chargeAmount)
    {
        if (charges < maxCharges)
        {
            if ((charges + chargeAmount) >= maxCharges)
            {
                charges = maxCharges;
            }
            else if ((charges + chargeAmount) < maxCharges)
            {
                charges += chargeAmount;
            }
        }
    }
}
