using UnityEngine;
using System.Collections;

public class PlatformManager : MonoBehaviour {

    private bool playerPresent;
    public Collider2D parentObject;
    private PlayerController player;

	// Use this for initialization
	void Start ()
        {
        player = FindObjectOfType<PlayerController>();
	    }
	
	// Update is called once per frame
	void Update ()
        {
	    
	    }

    void OnTriggerStay2D(Collider2D other)
        {
        if(other.name == "Player")
            {
            Debug.Log("derp");
            playerPresent = true;
            }
        }
}
