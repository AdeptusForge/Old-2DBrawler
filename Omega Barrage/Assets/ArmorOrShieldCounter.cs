using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorOrShieldCounter : MonoBehaviour {

    public bool Armor;

    public Sprite[] armorSprites= new Sprite[6];
    public Sprite[] shieldSprites = new Sprite[6];

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if (Armor == true)
        {
            GetComponent<SpriteRenderer>().sprite = armorSprites[GetComponentInParent<CreatureEnemy>().curArmorStacks];
        }

        if (Armor == false)
        {
            GetComponent<SpriteRenderer>().sprite = shieldSprites[GetComponentInParent<CreatureEnemy>().curShieldStacks];
        }

    }
}
