using UnityEngine;
using System.Collections;

public class DamageBuffTest : WeaponStatus{


    protected override void Start()
    {
        statusName = "Damage Buff";
        base.Start();
    }
    protected override void Update()
    {
        base.Update();
    }
    protected override void StatusText()
    {
    }
    protected override void ApplyIntervalEffect()
    {
    }
    protected override void CleanUpAndDestroy()
    {

        base.CleanUpAndDestroy();
    }
    protected override StatusEffect GetTargetStatus(Creature target)
    {
        return target.GetComponent<DamageBuffTest>();
    }
    protected override void AddToEntity(Creature target)
    {
        target.gameObject.AddComponent<DamageBuffTest>();
        DamageBuffTest activeBuff = target.gameObject.GetComponent<DamageBuffTest>();
        activeBuff.maxStackCount = maxStackCount;
        activeBuff.maxDuration = maxDuration;
        activeBuff.duration = duration;
        activeBuff.damageValue = damageValue;
        activeBuff.effectInterval = effectInterval;
        activeBuff.effectParticle = effectParticle;
        activeBuff.isPermenantBuff = isPermenantBuff;
        activeBuff.damageBonus = damageBonus;
        activeBuff.damageMultiplyer = damageMultiplyer;
        activeBuff.speedMultiplyer = speedMultiplyer;
    }
}
