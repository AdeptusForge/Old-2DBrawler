using UnityEngine;
using System.Collections;

public abstract class WeaponStatus : StatusEffect{

    public float damageMultiplyer;
    public float damageBonus;
    public bool isPermenantBuff;
    public float speedMultiplyer;

    protected override void Start()
    {
        base.Start();
        if (isPermenantBuff)
        {
            applyActive = true;
            Apply(GetComponentInParent<CreaturePlayer>());
        }
    }
    protected override bool IsExpired()
    {
        bool results = false;
        if (isPermenantBuff)
        {
            results = maxDuration != -1f;
        }
        if (!isPermenantBuff)
        {
            results = duration <= 0;
        }
        return results;
    }
}
