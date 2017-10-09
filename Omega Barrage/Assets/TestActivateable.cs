using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestActivateable : Activateable {

    public string activatedBy;
    public GameObject myGameManager;


    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == activatedBy)
        {
            //Debug.Log("can be activated by " + activatedBy);
            other.GetComponent<ActivateObject>().thingToActivate = this.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        //Debug.Log("can no longer be activated by " + activatedBy);
        if(other != null)
        {
            if(other.GetComponent<ActivateObject>() != null)
            {
                other.GetComponent<ActivateObject>().thingToActivate = null;
            }
        }
    }
    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public override void ActivationFunction()
    {
        myGameManager.GetComponent<EnemySpawnManager>().spawnersActive = true;
    }
}
