using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraThreshold : MonoBehaviour {

    public bool playerTriggeringThreshold;




	// Use this for initialization
	void Start () {
        playerTriggeringThreshold = false;

    }
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            playerTriggeringThreshold = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerTriggeringThreshold = false;
        }
    }
}
