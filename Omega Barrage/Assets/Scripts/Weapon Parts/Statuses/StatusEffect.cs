using UnityEngine;
using System.Collections;
//using System;
using UnityEngine.UI;

[RequireComponent(typeof(Transform))]
public abstract class StatusEffect : MonoBehaviour
{
    public float maxDuration;
    public float duration;
    public float effectInterval;
    protected float intervalCounter;
    public float startDelay;
    public int stackCount = 1;
    public int maxStackCount = -1;

    public string statusName;

    public int damageValue;
    public GameObject effectParticle;
    protected GameObject effectParticleInstance;
    public Creature creature;
    public bool applyActive;
    //TODO: WHATEVER THE FUCK I WANT

    protected int initialDamage;
    protected float initialDuration;

    protected virtual void Start()
    {

        creature = this.gameObject.GetComponent<Creature>();
        //weaponCheck = this.gameObject.GetComponent<Weapon>();
        initialDamage = damageValue;
        initialDuration = duration;

        Transform myTransform = GetComponent<Transform>();

        //if (effectParticle != null)
        //{

        //    if (weaponCheck == null)
        //    {

        //        effectParticleInstance = Instantiate(effectParticle, myTransform.position, myTransform.rotation) as GameObject;
        //        effectParticleInstance.gameObject.transform.parent = myTransform;
        //    }else{

        //        foreach (Transform t in weaponCheck.effectParticleLocations)
        //        {
        //            if (t.name == "EffectParticleLocation")
        //            {

        //                GameObject myEffect2 = Instantiate(effectParticle, t.position, t.rotation) as GameObject;
        //                myEffect2.transform.parent = t;
        //                myEffect2.transform.position = t.position;
        //            }
        //        }
        //    }
        //}

        if (creature != null)
        {
            StatusText();
        }

    }

    protected virtual void Update()
    {

        if (creature != null)
        {

            UpdateTime();

            if (IsExpired())
            {
                Debug.Log("Status expired");
                CleanUpAndDestroy();
            }
            else if (IsIntervalTime())
            {

                ApplyIntervalEffect();
                ResetIntervalTime();
            }
            if(maxDuration != -1 && duration > maxDuration)
            {
                duration = maxDuration;
            }
        }
    }//End of Update


    protected abstract void ApplyIntervalEffect();
    protected abstract StatusEffect GetTargetStatus(Creature target);
    protected virtual void AddToEntity(Creature target)
    {
        target.activeStatusEffects.Add(this);
    }
    protected abstract void StatusText();

    protected virtual void UpdateTime()
    {
        if (startDelay > 0)
            startDelay -= Time.deltaTime;
        if (duration > 0)
            duration -= Time.deltaTime;
        intervalCounter -= Time.deltaTime;
    }

    protected virtual bool IsExpired()
    {
        return duration <= 0;
    }

    protected virtual void CleanUpAndDestroy()
    {
        //Destroy(effectParticleInstance);
        if(GetComponent<Creature>() != null)
        {
            GetComponent<Creature>().activeStatusEffects.Remove(this);
        }
        Destroy(this);
    }

    protected virtual void ResetIntervalTime()
    {
        intervalCounter = effectInterval;
    }

    protected virtual bool IsIntervalTime()
    {
        return intervalCounter <= 0;
    }

    public virtual void Apply(Creature target)
    {
        if (applyActive == true)
        {
            StatusEffect status = GetTargetStatus(target);
            //Debug.Log("stack added to " + target.gameObject.name);

            if (status != null)
            {
                status.StackEffect();
            }
            else
            {
                AddToEntity(target);
            }
        }
    }

    public virtual void StackEffect()
    {
        if (maxDuration != -1 && duration < maxDuration)
        {
            duration = maxDuration;
        }
        else if (maxDuration == -1)
        {
            duration += initialDuration;
        }
        if(maxStackCount != -1 && stackCount < maxStackCount)
        {
            stackCount++;
            damageValue += initialDamage;
        }
    }

    protected void PeriodicDamage()
    {
        if (startDelay <= 0)
        {
            if (creature != null)
                creature.GetComponent<Creature>().AdjustHealth(damageValue);
        }
    }
}
