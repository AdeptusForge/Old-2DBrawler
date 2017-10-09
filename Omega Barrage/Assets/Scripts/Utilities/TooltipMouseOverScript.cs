using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipMouseOverScript : MonoBehaviour {

    TooltipManager selector;
    public Sprite selecteeTooltip;



	// Use this for initialization
	void Start ()
    {
        selector = GameObject.FindGameObjectWithTag("Tooltip Manager").GetComponent<TooltipManager>();
	}
	
	// Update is called once per frame
	void OnMouseOver ()
    {
        selector.tooltip = selecteeTooltip;
	}

    void OnMouseExit()
    {
        selector.tooltip = null;
    }
}
