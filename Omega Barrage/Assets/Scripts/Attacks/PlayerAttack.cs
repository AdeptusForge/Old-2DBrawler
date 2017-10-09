using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAttack : CreatureAttack {

    public enum Combo
    {
        DamageFast,
        DamageModerate,
        DamageSlow,
        DamageNone,
        ArmorFast,
        ArmorModerate,
        ArmorSlow,
        ArmorNone,
        ShieldFast,
        ShieldModerate,
        ShieldSlow,
        ShieldNone,
        NoneFast,
        NoneModerate,
        NoneSlow,
        NoneNone
    }
    Combo[] armorArray = { Combo.ArmorFast, Combo.ArmorModerate, Combo.ArmorSlow, Combo.ArmorNone };
    Combo[] damageArray = { Combo.DamageFast, Combo.DamageModerate, Combo.DamageSlow, Combo.DamageNone };
    Combo[] shieldArray = { Combo.ShieldFast, Combo.ShieldModerate, Combo.ShieldSlow, Combo.ShieldNone };
    Combo[] fastArray = { Combo.ArmorFast, Combo.DamageFast, Combo.ShieldFast, Combo.NoneFast };
    Combo[] moderateArray = { Combo.ArmorModerate, Combo.DamageModerate, Combo.ShieldModerate, Combo.NoneModerate };
    Combo[] slowArray = { Combo.ArmorSlow, Combo.DamageSlow, Combo.ShieldSlow, Combo.NoneSlow };
    Combo[] noneArray = { Combo.NoneFast, Combo.NoneModerate, Combo.NoneSlow, Combo.NoneNone };

    public List<Combo> armorCombos;
    public List<Combo> damageCombos;
    public List<Combo> shieldCombos;
    public List<Combo> slowCombos;
    public List<Combo> moderateCombos;
    public List<Combo> fastCombos;
    public List<Combo> noneCombos;


    public Combo currentCombo;
    public bool canAttack = true;

    public bool canCombo = false;

    public static float maxComboGauge = 50f;
    public static float curComboGauge;
    public PlayerMovement playerMovement;
    private InputManager playerInputs;

    protected override void Start()
    {
        armorCombos.AddRange(armorArray);
        damageCombos.AddRange(damageArray);
        shieldCombos.AddRange(shieldArray);
        slowCombos.AddRange(slowArray);
        moderateCombos.AddRange(moderateArray);
        fastCombos.AddRange(fastArray);
        noneCombos.AddRange(noneArray);

        currentCombo = Combo.NoneNone;
        base.Start();
        playerMovement = GetComponent<PlayerMovement>();
        playerInputs = GetComponent<InputManager>();
    }

    // Update is called once per frame
    void Update ()
    {
        if (!playerMovement.isHitStunned)
        {
            //attack
            if (Input.GetKeyDown(playerInputs.attackLight))
            {
                if (canAttack && (playerMovement.grounded || playerMovement.platformed))
                {
                    Attack("AttackLight");
                    ComboMaker("Shield", "Fast");
                }
                if (canAttack && !playerMovement.grounded && !playerMovement.platformed)
                {
                    Attack("AerialLight");
                    ComboMaker("Shield", "Fast");
                }
            }
            if (Input.GetKeyDown(playerInputs.attackMedium))
            {
                if (canAttack && (playerMovement.grounded || playerMovement.platformed))
                {
                    Attack("AttackMedium");
                    ComboMaker("Armor", "Moderate");

                }
                if (canAttack && !playerMovement.grounded && !playerMovement.platformed)
                {
                    Attack("AerialMedium");
                    ComboMaker("Armor", "Moderate");
                }
            }

            if (Input.GetKeyDown(playerInputs.attackHeavy))
            {
                if (canAttack && (playerMovement.grounded || playerMovement.platformed))
                {
                    Attack("AttackHeavy");
                    ComboMaker("Damage", "Slow");
                }
                if (canAttack && !playerMovement.grounded && !playerMovement.platformed)
                {
                    Attack("AerialHeavy");
                    ComboMaker("Damage", "Slow");
                }
            }

            if (Input.GetKeyDown(playerInputs.combo))
            {
                if (curComboGauge == maxComboGauge && canCombo)
                {
                    Debug.Log("Combo?");
                    if(!noneCombos.Contains(currentCombo))
                    {
                        ComboAttack(currentCombo);
                    }
                }
            }
        }
    }

    public void ComboMaker(string comboType, string comboSpeed)
    {
        if(curComboGauge == maxComboGauge)
        {
            if (currentCombo != Combo.NoneNone)
            {
                if (shieldCombos.Contains(currentCombo))
                {
                    switch (comboSpeed)
                    {
                        case "Fast":
                            {
                                currentCombo = Combo.ShieldFast;
                                break;
                            }
                        case "Moderate":
                            {
                                currentCombo = Combo.ShieldModerate;
                                break;
                            }
                        case "Slow":
                            {
                                currentCombo = Combo.ShieldSlow;
                                break;
                            }
                    }
                }
                if (armorCombos.Contains(currentCombo))
                {
                    switch (comboSpeed)
                    {
                        case "Fast":
                            {
                                currentCombo = Combo.ArmorFast;
                                break;
                            }
                        case "Moderate":
                            {
                                currentCombo = Combo.ArmorModerate;
                                break;
                            }
                        case "Slow":
                            {
                                currentCombo = Combo.ArmorSlow;
                                break;
                            }
                    }
                }
                if (damageCombos.Contains(currentCombo))
                {
                    switch (comboSpeed)
                    {
                        case "Fast":
                            {
                                currentCombo = Combo.DamageFast;
                                break;
                            }
                        case "Moderate":
                            {
                                currentCombo = Combo.DamageModerate;
                                break;
                            }
                        case "Slow":
                            {
                                currentCombo = Combo.DamageSlow;
                                break;
                            }
                    }
                }
            }
            if (currentCombo == Combo.NoneNone)
            {
                switch (comboType)
                {
                    case "Shield":
                        {
                            currentCombo = Combo.ShieldNone;
                            break;
                        }
                    case "Armor":
                        {
                            currentCombo = Combo.ArmorNone;
                            break;
                        }
                    case "Damage":
                        {
                            currentCombo = Combo.DamageNone;
                            break;
                        }
                }
            }
        }        
    }


    public void ComboAttack(Combo combo)
    {
        curComboGauge = 0;
        Debug.Log("ComboAttack" + combo.ToString());
        myAnimator.PlayInFixedTime("ComboAttack" + combo.ToString());
        currentCombo = Combo.NoneNone;
    }
}
