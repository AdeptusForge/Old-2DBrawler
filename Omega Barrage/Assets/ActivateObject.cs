using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateObject : MonoBehaviour {

    public GameObject thingToActivate;
    public InputManager myInputs;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(myInputs.activate) && thingToActivate != null)
        {
            thingToActivate.GetComponent<Activateable>().ActivationFunction();
        }
	}
}
