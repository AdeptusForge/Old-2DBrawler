using UnityEngine;
using System.Collections;

public abstract class AttackHit : MonoBehaviour {




	// Use this for initialization
	void Start ()
    {

	}


    protected abstract void AttackOnHit(string attackID);

}
