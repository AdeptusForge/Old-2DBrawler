using UnityEngine;
using System.Collections;

public class EnemyActivator : MonoBehaviour {

    public Creature activator;
    public Creature[] activatees = new Creature[20];


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnTriggerEnter2D(Collider2D other)
    {
        foreach(Creature activatee in activatees)
        {
            if(activatee != null)
            {
                activatee.GetComponent<EnemyAITestScript>().AIActivator();
            }
        }
        //Debug.Log("player activated enemies");
    }
}
