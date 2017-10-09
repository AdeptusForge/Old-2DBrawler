using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Creature : MonoBehaviour {

    public Factions myFactionManager;
    public Factions.AllGameFactions myFaction;
    public List<Factions.AllGameFactions> myEnemies;
    public List<Factions.AllGameFactions> myAllies;
    protected SpriteRenderer mySprite;
    protected Rigidbody2D myBody;
    public List<StatusEffect> activeStatusEffects = new List<StatusEffect>();
    public string affectedByAttack;
    public float hitStunDuration;
    public float knockBackDuration;

    public float maxHealth;
    public float curHealth;
    public int maxArmorOrShields;
    public int curArmorStacks;
    public int curShieldStacks;
    private float totalResistances;
    public bool isDead = false;
    public float damageAmp;


    // Use this for initialization
    protected virtual void Start ()
    {
        mySprite = GetComponent<SpriteRenderer>();
        myBody = GetComponent<Rigidbody2D>();
        FullHealth();
        myFactionManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<Factions>();
        myFactionManager.GetMyEnemiesAndAllies(myFaction, this.gameObject);
    }
	
	// Update is called once per frame
	protected virtual void Update () {
        UpdateHitStunAndKnockbackTime();
        HealthCheck();
        totalResistances = curArmorStacks*20f + curShieldStacks*20f;
        if (totalResistances > 100f)
        {
            totalResistances = 100f;
        }
        if (knockBackDuration > 0f)
        {
            gameObject.layer = 16;
        }
        else
        {
            gameObject.layer = 0;
        }
    }
    protected virtual void UpdateHitStunAndKnockbackTime()
    {
        if(hitStunDuration > 0f)
        {
            hitStunDuration -= Time.deltaTime;
        }
        if(hitStunDuration < 0f)
        {
            hitStunDuration = 0f;
        }

        if (knockBackDuration > 0f)
        {
            knockBackDuration -= Time.deltaTime;
        }
        if (knockBackDuration < 0f)
        {
            knockBackDuration = 0f;
        }
    }
    protected virtual bool IsExpired()
    {
        return hitStunDuration <= 0;
    }

    protected float healthDifference;

    public virtual void FullHealth()
    {
        curHealth = maxHealth;
    }

    public virtual void AdjustHealth(float healthAdj)
    {
        //Debug.Log(healthAdj +"health adjusted on " + this.gameObject);
        if (healthAdj > 0)
        {
            if(damageAmp > 1)
            {
                healthAdj *= 1 + (damageAmp * 0.01f);
            }
            if (curArmorStacks > 0 || curShieldStacks > 0)
            {
                healthAdj *= 1 - (totalResistances * 0.01f);
            }

            if (healthAdj < 0.1f && healthAdj > 0.01f)
            {
                healthAdj = 0.1f;
            }
            if(healthAdj < 0.01f)
            {
                healthAdj = 0f;
            }
        }

        curHealth -= healthAdj;

        if (curHealth <= 0)
        {
            curHealth = 0;
        }
        if (curHealth > maxHealth)
        {
            curHealth = maxHealth;
        }

        healthDifference = curHealth / maxHealth;
    }

    public virtual void AdjustArmor(int armorAdj)
    {
        curArmorStacks -= armorAdj;

        if (curArmorStacks <= 0)
        {
            curArmorStacks = 0;
        }
        if (curArmorStacks > maxArmorOrShields)
        {
            curArmorStacks = maxArmorOrShields;
        }
    }

    public virtual void AdjustShield(int shieldAdj)
    {
        curShieldStacks -= shieldAdj;

        if (curShieldStacks <= 0)
        {
            curShieldStacks = 0;
        }
        if (curShieldStacks > maxArmorOrShields)
        {
            curShieldStacks = maxArmorOrShields;
        }
    }

    protected virtual void HealthCheck()
    {
        if (curHealth <= 0)
        {
            if (!isDead)
            {
                CreatureDying();
            }
            else
            {
                CreatureDead();
            }
        }
    }

    protected virtual void CreatureDying()
    {
        isDead = true;
    }

    protected virtual void CreatureDead()
    {
        Destroy(gameObject);
    }

    public virtual void KillCreature()
    {
        curHealth = 0;
    }

}
