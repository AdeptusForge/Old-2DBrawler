using UnityEngine;
using System.Collections;

public class WeaponSwap : MonoBehaviour {

    public int weaponID;
    public RuntimeAnimatorController[] weaponAnimationControllerList = new RuntimeAnimatorController[2];
    private Animator myAnimator;
    private RuntimeAnimatorController myAnimationController;
    private GameObject currentWeapon;
    public GameObject weaponToSwapWith;
    private GameObject holster;
    private PlayerMovement player;

    public void Start()
    {
        myAnimator = GetComponent<Animator>();
        myAnimationController = myAnimator.runtimeAnimatorController;
        holster = gameObject.transform.GetChild(0).gameObject;
        currentWeapon = holster.transform.GetChild(0).gameObject;
        player = GetComponent<PlayerMovement>();
    }

    /*--used in swapping out later down the line--*/
    public void AnimationSetSwap(int weaponID)
    {
        Debug.Log("This should work");
        myAnimator.runtimeAnimatorController = weaponAnimationControllerList[weaponID];
        myAnimator.PlayInFixedTime("Idle");
    }
    public void WeaponHitBoxSwap()
    {
        Destroy(currentWeapon);
        currentWeapon = Instantiate(weaponToSwapWith);
        currentWeapon.transform.parent = holster.transform;
        player.CheckWeapon();
    }
   void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            WeaponHitBoxSwap();
            foreach(RuntimeAnimatorController weapon in weaponAnimationControllerList)
            {
                Debug.Log(weapon);
            }
            //AnimationSetSwap(1);
        }
    }
}
