using UnityEngine;
using System.Collections;

public class CreaturePlayer : Creature {

    public GameObject myWeapon;

    // Use this for initialization
    protected override void Start ()
    {
        base.Start();
        myWeapon = GameObject.FindGameObjectWithTag("Weapon");
	}
	
	// Update is called once per frame
	protected override void Update ()
    {
        base.Update();
	}
    
}
