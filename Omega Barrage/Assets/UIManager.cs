using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public GameObject infoUI;
    public GameObject comboUI;
    public GameObject comboSymbol1;
    public GameObject comboSymbol2;


    public GameObject myPlayer;

    public Sprite comboDamage;
    public Sprite comboArmor;
    public Sprite comboShield;
    public Sprite comboFast;
    public Sprite comboModerate;
    public Sprite comboSlow;



    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(comboSymbol2.GetComponent<Image>().sprite);

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            infoUI.SetActive(false);
        }
        if (PlayerAttack.curComboGauge == PlayerAttack.maxComboGauge)
        {
            comboUI.SetActive(true);
        }
        else
        {
            comboUI.SetActive(false);
        }
        if (comboUI.activeSelf == true)
        {

            Debug.Log("Combo UI Active");
            comboSymbol1.GetComponent<Image>().sprite = null;
            comboSymbol1.GetComponent<Image>().color = Color.clear;
            comboSymbol2.GetComponent<Image>().sprite = null;
            comboSymbol2.GetComponent<Image>().color = Color.clear;

            if (myPlayer.GetComponent<PlayerAttack>().armorCombos.Contains(myPlayer.GetComponent<PlayerAttack>().currentCombo))
            {
                comboSymbol1.GetComponent<Image>().sprite = comboArmor;
                comboSymbol1.GetComponent<Image>().color = Color.white;
            }
            else if (myPlayer.GetComponent<PlayerAttack>().damageCombos.Contains(myPlayer.GetComponent<PlayerAttack>().currentCombo))
            {
                comboSymbol1.GetComponent<Image>().sprite = comboDamage;
                comboSymbol1.GetComponent<Image>().color = Color.white;
            }
            else if (myPlayer.GetComponent<PlayerAttack>().shieldCombos.Contains(myPlayer.GetComponent<PlayerAttack>().currentCombo))
            {
                comboSymbol1.GetComponent<Image>().sprite = comboShield;
                comboSymbol1.GetComponent<Image>().color = Color.white;
            }
            else
            {
                comboSymbol1.GetComponent<Image>().sprite = null;
                comboSymbol1.GetComponent<Image>().color = Color.clear;
            }


            if (myPlayer.GetComponent<PlayerAttack>().fastCombos.Contains(myPlayer.GetComponent<PlayerAttack>().currentCombo))
            {
                comboSymbol2.GetComponent<Image>().sprite = comboFast;
                comboSymbol2.GetComponent<Image>().color = Color.white;
            }
            else if (myPlayer.GetComponent<PlayerAttack>().moderateCombos.Contains(myPlayer.GetComponent<PlayerAttack>().currentCombo))
            {
                comboSymbol2.GetComponent<Image>().sprite = comboModerate;
                comboSymbol2.GetComponent<Image>().color = Color.white;
            }
            else if (myPlayer.GetComponent<PlayerAttack>().slowCombos.Contains(myPlayer.GetComponent<PlayerAttack>().currentCombo))
            {
                comboSymbol2.GetComponent<Image>().sprite = comboSlow;
                comboSymbol2.GetComponent<Image>().color = Color.white;
            }
            else
            {
                comboSymbol2.GetComponent<Image>().sprite = null;
                comboSymbol2.GetComponent<Image>().color = Color.clear;
            }

        }
    }
}
