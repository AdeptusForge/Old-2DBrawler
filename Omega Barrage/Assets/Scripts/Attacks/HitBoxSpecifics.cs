using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class HitBoxSpecifics : MonoBehaviour {

    /*--On-Hit Variables--*/
    public float comboPower;
    public float damage;
    private float currentDamage;
    public int armorDamage;
    public int shieldDamage;
    public bool damAfterResistRemove;

    public string attackID;
    private string enemyHitName;
    public bool attackIDReset = false;

    /*--knockback variables--*/
    public float knockBackDirection;
    public float knockBackPower;
    public float knockBackDuration;
    public float attackHitStunDuration;
    private int myFacing;
    private Vector3 knockBackVector;

    private CreatureMovement myCreature;
    public List<GameObject> enemiesHit = new List<GameObject>();
    public string attackHits;
    public Weapon parentWeapon;
    public HitBoxManager myManager;
    public ComboGaugeManager myComboGauge;

    void Awake()
    {
        if(gameObject.transform.root.tag == "Player")
        {
            parentWeapon = GetComponentInParent<Weapon>();
        }
        myManager = GetComponentInParent<HitBoxManager>();
        myManager.allHitBoxScripts.Add(this);
    }
	// Use this for initialization
	void Start ()
    {
        myCreature = GetComponentInParent<CreatureMovement>();       
    }
	
	// Update is called once per frame
	void Update ()
    {
        DamageBuff();
	}
    
    //checks what/if anything was hit and whether it's affected by damage
    void OnTriggerEnter2D(Collider2D other)
    {

        if(other.gameObject.GetComponentInParent<Creature>() == null)
        {
            //Debug.Log("whatever");
        }
        if (FactionChecker(other.gameObject) == true)
        {
            if(attackID != other.gameObject.GetComponentInParent<Creature>().affectedByAttack)
            {
                //Debug.Log(other.gameObject.transform.root.GetComponent<CreatureEnemy>().affectedByAttack);
                other.gameObject.GetComponentInParent<Creature>().affectedByAttack = attackID;
                //Debug.Log("Attack succeeded." + other.gameObject.GetComponent<CreatureEnemy>().affectedByAttack + " " + other.gameObject.name + " " + gameObject.name);
                enemiesHit.Add(other.gameObject.transform.root.gameObject);
                AttackOnHit(attackID, enemyHitName, other);               
            }

            if (attackID == other.gameObject.GetComponentInParent<Creature>().affectedByAttack)
            {
            //Debug.Log("Attack failed." + other.gameObject.GetComponent<CreatureEnemy>().affectedByAttack + " " + other.gameObject.name + " " + gameObject.name);
            }
        }
        foreach (GameObject enemy in enemiesHit)
        {
            //Debug.Log(enemy.name);
            //checks to see if an enemy is inside hitbox and then runs the attack function
            //if (other.gameObject.tag == whatIsMyEnemy /*&& attackID != other.gameObject.GetComponent<CreatureEnemy>().affectedByAttack*/)
            //{
            //    Debug.Log("Attack succeeded." + other.gameObject.GetComponent<CreatureEnemy>().affectedByAttack + " " + other.gameObject.name + " " + gameObject.name);
            //    AttackOnHit(attackID, enemyHitName, other);

            //}
            //tells if hitboxes were within hit range, but did not connect because they were redundant
            //else if (other.gameObject.tag == whatIsMyEnemy && attackID == other.gameObject.GetComponent<CreatureEnemy>().affectedByAttack)
            //{
            //    Debug.Log("Attack failed." + other.gameObject.GetComponent<CreatureEnemy>().affectedByAttack + " " + other.gameObject.name + " " + gameObject.name);
            //}
        }
    }
    //damages any enemies hit by a hitbox
    private void AttackOnHit(string attackID, string enemyHitName, Collider2D other)
    {
        //Debug.Log("hit occured on " + other.gameObject.name);
        //Debug.Log(other.gameObject.transform.root.name + "added to enemieshit");
        if (damAfterResistRemove == false)
        {
            if(other.gameObject.GetComponent<Creature>() != null)
            {
                other.gameObject.GetComponent<Creature>().AdjustHealth(currentDamage);
            }
            else
            {
                other.gameObject.GetComponentInParent<Creature>().AdjustHealth(currentDamage);
            }

            if (other.gameObject.GetComponent<CreaturePlayer>() == null)
            {
                if(other.gameObject.GetComponent<Creature>() != null)
                {
                    other.gameObject.GetComponent<Creature>().AdjustArmor(armorDamage);
                    other.gameObject.GetComponent<Creature>().AdjustShield(shieldDamage);
                }
            }
        }
        else
        {
            if(other.gameObject.GetComponent<CreaturePlayer>() == null)
            {
                other.gameObject.GetComponentInParent<Creature>().AdjustArmor(armorDamage);
                other.gameObject.GetComponentInParent<Creature>().AdjustShield(shieldDamage);
            }            
            other.gameObject.GetComponentInParent<Creature>().AdjustHealth(currentDamage);
        }
        

        other.gameObject.GetComponentInParent<Pushable>().Push(knockBackPower, CalcKnockBackVector(knockBackDirection), knockBackDuration * 0.01666667f);
        other.gameObject.GetComponentInParent<Creature>().hitStunDuration = attackHitStunDuration;
        other.gameObject.GetComponentInParent<CreatureMovement>().hitStunTimer = attackHitStunDuration;
        ComboGaugeManager.AddHit();
        if(parentWeapon != null)
        {
            if (parentWeapon.gameObject.GetComponent<NonWeaponStatus>() != null)
            {
                NonWeaponStatus[] onHitStatuses = parentWeapon.GetComponents<NonWeaponStatus>();
                foreach (NonWeaponStatus status in onHitStatuses)
                {
                    status.Apply(other.gameObject.GetComponentInParent<Creature>());
                }
            }
            if (parentWeapon.gameObject.transform.root.GetComponent<NonWeaponStatus>() == null)
            {
            }
        }       
        if(this.gameObject.transform.root.GetComponent<PlayerAttack>() != null)
        {
            PlayerAttack.curComboGauge += comboPower;
            if (PlayerAttack.curComboGauge >= PlayerAttack.maxComboGauge)
            {
                PlayerAttack.curComboGauge = PlayerAttack.maxComboGauge;
            }
            Debug.Log(PlayerAttack.curComboGauge);
        }
    }
    //calculates the knockback vector on any attack that hits an enemy
    private Vector3 CalcKnockBackVector(float degree)
    {
        degree = (-degree * (myCreature.currentFacing / Mathf.Abs(myCreature.currentFacing))) + 90f;
        float sinOfDegree = Mathf.Sin((degree * Mathf.PI) / 180);
        if(sinOfDegree > -0.0001f && sinOfDegree < 0.0001f)
        {
            sinOfDegree = 0f;
        }
        float cosOfDegree = Mathf.Cos((degree * Mathf.PI) / 180);
        if (cosOfDegree > -0.0001f && cosOfDegree < 0.0001f)
        {
            cosOfDegree = 0f;
        }
        knockBackVector = new Vector3(cosOfDegree, sinOfDegree, 0f);
        return knockBackVector;
    }
    //resets the attackID so that the same attack doesn't hit multiple times
    public void AttackIDReset()
    {
        foreach (GameObject enemy in enemiesHit)
        {
            //Debug.Log("cleared attack ID");
            if(enemy != null && enemy.GetComponent<Creature>() != null)
            {
                enemy.GetComponent<Creature>().affectedByAttack = "";
            }
            else if(enemy != null && enemy.GetComponentInParent<Creature>() != null)
            {
                enemy.GetComponentInParent<Creature>().affectedByAttack = "";

            }
        }
        //Debug.Log("cleared enemy hit list" + this.gameObject);
        enemiesHit.Clear();
    }
    //buffs damage from weapon status effects
    private void DamageBuff()
    {
        if(GetComponentInParent<WeaponStatus>() != null)
        {
            currentDamage = (damage *(1 + GetComponentInParent<WeaponStatus>().damageMultiplyer)) + GetComponentInParent<WeaponStatus>().damageBonus;
        }
        else
        {
            currentDamage = damage;
        }
    }
    private bool FactionChecker(GameObject toBeChecked)
    {
        if(toBeChecked.GetComponent<Creature>() == null && toBeChecked.GetComponentInParent<Creature>() != null)
        {
            return GetComponentInParent<Creature>().myEnemies.Contains(toBeChecked.gameObject.GetComponentInParent<Creature>().myFaction);
        }
        else if(toBeChecked.GetComponent<Creature>() != null)
        {
            return GetComponent<Creature>().myEnemies.Contains(toBeChecked.gameObject.GetComponentInParent<Creature>().myFaction);
        }
        else
        {
            return false;
        }
    }
}