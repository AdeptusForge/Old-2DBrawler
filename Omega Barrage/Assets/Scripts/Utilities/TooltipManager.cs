using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TooltipManager : MonoBehaviour {

    Vector3 pointer;
    public Sprite tooltip;


    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        pointer = Input.mousePosition;
        transform.position = new Vector3(Camera.main.ScreenToWorldPoint(pointer).x, Camera.main.ScreenToWorldPoint(pointer).y, -9);

        if(tooltip != null)
        {
            //Debug.Log("Displaying tooltip");
            GetComponent<Image>().sprite = tooltip;
            GetComponent<Image>().color = Color.white;
        }
        if (tooltip == null)
        {
            //Debug.Log("No tooltip to display");
            GetComponent<Image>().sprite = null;
            GetComponent<Image>().color = Color.clear;
        }
    }
}
