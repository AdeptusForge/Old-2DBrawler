using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HitBoxManager : MonoBehaviour {

    public List<HitBoxSpecifics> allHitBoxScripts = new List<HitBoxSpecifics>();
	
    void Start(){

    }
	// Update is called once per frame
	void Update () {
	
	}

    public void ResetAllAttackID(string animatorReset)
    {
        //Debug.Log("animator reset");
        foreach (HitBoxSpecifics hitBoxScript in allHitBoxScripts)
        {
            //Debug.Log(hitBoxScript.attackHits);
            if (hitBoxScript.attackHits == animatorReset)
            {
                hitBoxScript.AttackIDReset();
            }
        }
    }

}
