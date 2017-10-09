using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class InputManager : MonoBehaviour {

    public class BufferableActions
    {
        public enum Action : int
        {
            Jump = 1,
            ComboAttack = 2,
            Attack = 3,
            Dash = 4,
            Pivot = 5,
            WeaponAbility = 6
        }
        public enum AttackType
        {
            Light,
            Medium,
            Heavy,
            None
        }
        public enum DashDirection
        {
            Up,
            Down,
            Forward,
            Backward,
            None
        }
        public enum WeaponAbility
        {
            Active1,
            Active2,
            None
        }
        public Action performedAction;
        public AttackType? performedAttackType1;
        public AttackType? performedAttackType2;
        public DashDirection? performedDashDirection;
        public WeaponAbility? performedWeaponAbility;

        public BufferableActions (Action action, AttackType attackType1, AttackType attackType2, DashDirection dashDirection, WeaponAbility weaponAbility)
        {
            performedAction = action;
            performedAttackType1 = attackType1;
            performedAttackType2 = attackType2;
            performedDashDirection = dashDirection;
            performedWeaponAbility = weaponAbility;
            if(performedAction == Action.Attack)
            {
                performedAttackType2 = null;
                performedDashDirection = null;
                performedWeaponAbility = null;
            }
            if(performedAction == Action.ComboAttack)
            {
                performedDashDirection = null;
                performedWeaponAbility = null;
            }
            if(performedAction == Action.Dash)
            {
                performedAttackType1 = null;
                performedAttackType2 = null;
                performedWeaponAbility = null;
            }
            if(performedAction == Action.Jump || performedAction == Action.Pivot)
            {
                performedAttackType1 = null;
                performedAttackType2 = null;
                performedDashDirection = null;
                performedWeaponAbility = null;
            }
            if(performedAction == Action.WeaponAbility)
            {
                performedAttackType1 = null;
                performedAttackType2 = null;
                performedDashDirection = null;
            }
        }
    }


    private List<BufferableActions> bufferedActions = new List<BufferableActions>();
    public Action bufferedAction;
    public string bufferedActionString;
    public PlayerMovement playerMovement;
    public float maxCheckerTimer;
    public bool checkerTimerActive = false;
    public float currentCheckerTimer = 0f;
    public int previousBufferedAttack = 0;

    public List<KeyCode> validInputs = new List<KeyCode>();

    /*---NON-REBINDABLE INPUTS--*/
    public KeyCode menu;
    /*--ALL REBINDABLE INPUTS--*/
    public KeyCode jump;
    public KeyCode attackLight;
    public KeyCode attackMedium;
    public KeyCode attackHeavy;
    public KeyCode combo;
    public KeyCode dash;
    public KeyCode up;
    public KeyCode down;
    public KeyCode left;
    public KeyCode right;
    public KeyCode weaponActive1;
    public KeyCode weaponActive2;
    public KeyCode consumable;
    public KeyCode activate;


    // Use this for initialization
    void Start () {
        playerMovement = GetComponent<PlayerMovement>();
        menu = KeyCode.Escape;
        jump = KeyCode.Space;
        attackLight = KeyCode.P;
        attackMedium = KeyCode.O;
        attackHeavy = KeyCode.I;
        combo = KeyCode.U;
        dash = KeyCode.LeftShift;
        up = KeyCode.W;
        down = KeyCode.S;
        left = KeyCode.A;
        right = KeyCode.D;
        weaponActive1 = KeyCode.Escape;
        weaponActive2 = KeyCode.Escape;
        consumable = KeyCode.Escape;
        activate = KeyCode.E;
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        InputCheckTimer();
	}
    //public void DetectInputsDuringBuffer()
    //{
    //    if (playerMovement.canBuffer)
    //    {
    //        if (Input.anyKeyDown)
    //        {
    //            if (Input.GetKeyDown(jump) && !Input.GetKey(dash))
    //            {
    //                bufferedActions.Add(new BufferableActions(BufferableActions.Action.Jump, BufferableActions.AttackType.None, BufferableActions.AttackType.None, BufferableActions.DashDirection.None, BufferableActions.WeaponAbility.None));
    //            }
    //            if (Input.GetKey(dash))
    //            {
    //                if (Input.GetKeyDown(up))
    //                {
    //                    bufferedActions.Add(new BufferableActions(BufferableActions.Action.Dash, BufferableActions.AttackType.None, BufferableActions.AttackType.None, BufferableActions.DashDirection.Up, BufferableActions.WeaponAbility.None));
    //                }
    //                if (Input.GetKeyDown(down))
    //                {
    //                    bufferedActions.Add(new BufferableActions(BufferableActions.Action.Dash, BufferableActions.AttackType.None, BufferableActions.AttackType.None, BufferableActions.DashDirection.Down, BufferableActions.WeaponAbility.None));
    //                }
    //                if (Input.GetKeyDown(left))
    //                {
    //                    if(playerMovement.currentFacing == 1)
    //                    {
    //                        bufferedActions.Add(new BufferableActions(BufferableActions.Action.Dash, BufferableActions.AttackType.None, BufferableActions.AttackType.None, BufferableActions.DashDirection.Backward, BufferableActions.WeaponAbility.None));
    //                    }
    //                    if (playerMovement.currentFacing == -1)
    //                    {
    //                        bufferedActions.Add(new BufferableActions(BufferableActions.Action.Dash, BufferableActions.AttackType.None, BufferableActions.AttackType.None, BufferableActions.DashDirection.Forward, BufferableActions.WeaponAbility.None));
    //                    }
    //                }
    //                if (Input.GetKeyDown(right))
    //                {
    //                    if (playerMovement.currentFacing == 1)
    //                    {
    //                        bufferedActions.Add(new BufferableActions(BufferableActions.Action.Dash, BufferableActions.AttackType.None, BufferableActions.AttackType.None, BufferableActions.DashDirection.Forward, BufferableActions.WeaponAbility.None));
    //                    }
    //                    if (playerMovement.currentFacing == -1)
    //                    {
    //                        bufferedActions.Add(new BufferableActions(BufferableActions.Action.Dash, BufferableActions.AttackType.None, BufferableActions.AttackType.None, BufferableActions.DashDirection.Backward, BufferableActions.WeaponAbility.None));
    //                    }
    //                }
    //            }
    //            if (Input.GetKeyDown(attackLight))
    //            {
    //                if (!checkerTimerActive)
    //                {
    //                    bufferedActions.Add(new BufferableActions(BufferableActions.Action.Attack, BufferableActions.AttackType.Light, BufferableActions.AttackType.None, BufferableActions.DashDirection.None, BufferableActions.WeaponAbility.None));
    //                    checkerTimerActive = true;
    //                    previousBufferedAttack = 1;
    //                }
    //                if (checkerTimerActive)
    //                {
    //                    switch (previousBufferedAttack)
    //                    {
    //                        case 1:
    //                            bufferedActions.Add(new BufferableActions(BufferableActions.Action.ComboAttack, BufferableActions.AttackType.Light, BufferableActions.AttackType.Light, BufferableActions.DashDirection.None, BufferableActions.WeaponAbility.None));
    //                            previousBufferedAttack = 0;
    //                            break;
    //                        case 2:
    //                            bufferedActions.Add(new BufferableActions(BufferableActions.Action.ComboAttack, BufferableActions.AttackType.Medium, BufferableActions.AttackType.Light, BufferableActions.DashDirection.None, BufferableActions.WeaponAbility.None));
    //                            previousBufferedAttack = 0;
    //                            break;
    //                        case 3:
    //                            bufferedActions.Add(new BufferableActions(BufferableActions.Action.ComboAttack, BufferableActions.AttackType.Heavy, BufferableActions.AttackType.Light, BufferableActions.DashDirection.None, BufferableActions.WeaponAbility.None));
    //                            previousBufferedAttack = 0;
    //                            break;
    //                        default:
    //                            break;
    //                    }
    //                }
    //            }
    //            if (Input.GetKeyDown(attackMedium))
    //            {
    //                if (!checkerTimerActive)
    //                {
    //                    bufferedActions.Add(new BufferableActions(BufferableActions.Action.Attack, BufferableActions.AttackType.Medium, BufferableActions.AttackType.None, BufferableActions.DashDirection.None, BufferableActions.WeaponAbility.None));
    //                    checkerTimerActive = true;
    //                    previousBufferedAttack = 2;
    //                }
    //                if (checkerTimerActive)
    //                {
    //                    switch (previousBufferedAttack)
    //                    {
    //                        case 1:
    //                            bufferedActions.Add(new BufferableActions(BufferableActions.Action.ComboAttack, BufferableActions.AttackType.Light, BufferableActions.AttackType.Medium, BufferableActions.DashDirection.None, BufferableActions.WeaponAbility.None));
    //                            previousBufferedAttack = 0;
    //                            break;
    //                        case 2:
    //                            bufferedActions.Add(new BufferableActions(BufferableActions.Action.ComboAttack, BufferableActions.AttackType.Medium, BufferableActions.AttackType.Medium, BufferableActions.DashDirection.None, BufferableActions.WeaponAbility.None));
    //                            previousBufferedAttack = 0;
    //                            break;
    //                        case 3:
    //                            bufferedActions.Add(new BufferableActions(BufferableActions.Action.ComboAttack, BufferableActions.AttackType.Heavy, BufferableActions.AttackType.Medium, BufferableActions.DashDirection.None, BufferableActions.WeaponAbility.None));
    //                            previousBufferedAttack = 0;
    //                            break;
    //                        default:
    //                            break;
    //                    }
    //                }
    //            }
    //            if (Input.GetKeyDown(attackHeavy))
    //            {
    //                if (!checkerTimerActive)
    //                {
    //                    bufferedActions.Add(new BufferableActions(BufferableActions.Action.Attack, BufferableActions.AttackType.Heavy, BufferableActions.AttackType.None, BufferableActions.DashDirection.None, BufferableActions.WeaponAbility.None));
    //                    checkerTimerActive = true;
    //                    previousBufferedAttack = 3;
    //                }
    //                if (checkerTimerActive)
    //                {
    //                    switch (previousBufferedAttack)
    //                    {
    //                        case 1:
    //                            bufferedActions.Add(new BufferableActions(BufferableActions.Action.ComboAttack, BufferableActions.AttackType.Light, BufferableActions.AttackType.Heavy, BufferableActions.DashDirection.None, BufferableActions.WeaponAbility.None));
    //                            previousBufferedAttack = 0;
    //                            break;
    //                        case 2:
    //                            bufferedActions.Add(new BufferableActions(BufferableActions.Action.ComboAttack, BufferableActions.AttackType.Medium, BufferableActions.AttackType.Heavy, BufferableActions.DashDirection.None, BufferableActions.WeaponAbility.None));
    //                            previousBufferedAttack = 0;
    //                            break;
    //                        case 3:
    //                            bufferedActions.Add(new BufferableActions(BufferableActions.Action.ComboAttack, BufferableActions.AttackType.Heavy, BufferableActions.AttackType.Heavy, BufferableActions.DashDirection.None, BufferableActions.WeaponAbility.None));
    //                            previousBufferedAttack = 0;
    //                            break;
    //                        default:
    //                            break;
    //                    }
    //                }
    //            }
    //            if (Input.GetKeyDown(left))
    //            {
    //                if (playerMovement.currentFacing == 1)
    //                {
    //                    bufferedActions.Add(new BufferableActions(BufferableActions.Action.Pivot, BufferableActions.AttackType.None, BufferableActions.AttackType.None, BufferableActions.DashDirection.None, BufferableActions.WeaponAbility.None));
    //                }
    //            }
    //            if (Input.GetKeyDown(right))
    //            {
    //                if (playerMovement.currentFacing == -1)
    //                {
    //                    bufferedActions.Add(new BufferableActions(BufferableActions.Action.Pivot, BufferableActions.AttackType.None, BufferableActions.AttackType.None, BufferableActions.DashDirection.None, BufferableActions.WeaponAbility.None));
    //                }
    //            }
    //        }
    //    }
    //}

    public void CheckListForEnumValues()
    {
        int bufferActionValue = 10;       
        foreach (BufferableActions action in bufferedActions)
        {
            if ((int)action.performedAction < bufferActionValue)
            {
                bufferActionValue = (int)action.performedAction;
                bufferedActionString = action.performedAction.ToString() + action.performedAttackType1.ToString() + action.performedAttackType2.ToString() +action.performedDashDirection.ToString() + action.performedWeaponAbility.ToString();
            }
            Debug.Log(bufferedActionString);
        }
        bufferedActions.Clear();
    }
    private void InputCheckTimer()
    {
        if (playerMovement.canBuffer)
        {
            Debug.Log("can buffer");
            if (checkerTimerActive == true)
            {
                if (currentCheckerTimer < maxCheckerTimer)
                {
                    currentCheckerTimer = currentCheckerTimer + Time.fixedDeltaTime;
                    Debug.Log(currentCheckerTimer);
                }
                if (currentCheckerTimer > maxCheckerTimer)
                {
                    currentCheckerTimer = maxCheckerTimer;
                    Debug.Log("timer went over");
                }
                if(currentCheckerTimer == maxCheckerTimer)
                {
                    currentCheckerTimer = 0f;
                    checkerTimerActive = false;
                }
            }
        }
        if (!playerMovement.canBuffer)
        {
            currentCheckerTimer = 0f;
            checkerTimerActive = false;
        }
    }
}
