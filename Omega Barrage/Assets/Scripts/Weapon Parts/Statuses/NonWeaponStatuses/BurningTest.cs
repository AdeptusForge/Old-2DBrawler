using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BurningTest : NonWeaponStatus
{

    private float initialSpeed;

    protected override void Start()
    {
        statusName = "Burning";
        base.Start();
    }
    protected override void Update()
    {
        base.Update();
    }

    protected override void StatusText()
    {
        //Color32 color = new Color32(255, 99, 71, 255);
        //entity.FloatingText(statusName, color);
    }

    protected override void ApplyIntervalEffect()
    {
        PeriodicDamage();
    }

    protected override void CleanUpAndDestroy()
    {

        base.CleanUpAndDestroy();
    }

    protected override StatusEffect GetTargetStatus(Creature target)
    {
        return target.GetComponent<BurningTest>();
    }

    protected override void AddToEntity(Creature target)
    {
        target.gameObject.AddComponent<BurningTest>();
        BurningTest activeBurn = target.gameObject.GetComponent<BurningTest>();
        activeBurn.maxStackCount = maxStackCount;
        activeBurn.maxDuration = maxDuration;
        activeBurn.duration = duration;
        activeBurn.damageValue = damageValue;
        activeBurn.effectInterval = effectInterval;
        activeBurn.effectParticle = effectParticle;
    }


}
