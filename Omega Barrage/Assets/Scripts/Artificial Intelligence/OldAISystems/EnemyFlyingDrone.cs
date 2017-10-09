using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlyingDrone : MonoBehaviour {

    public GameObject target;
    public float flightSpeed;
    public bool isActive;
    public float deactiveTime;
    public float curDeactiveTime;
    public Vector3 flightPath;
    public Rigidbody2D myBody;

    // Use this for initialization
    void Start () {
        myBody = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player");
        GetComponent<Creature>().myFactionManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<Factions>();
	}
	
	// Update is called once per frame
	void Update () {
        if(target != null)
        {
            flightPath = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z);
        }
        //Debug.DrawLine(transform.position, flightPath);
        if (isActive)
        {
            transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), flightPath, 1 * flightSpeed);
        }

        if (curDeactiveTime == 0f)
        {
            isActive = true;
            GetComponent<HitBoxSpecifics>().AttackIDReset();
        }
    }
    void FixedUpdate()
    {
        if(curDeactiveTime > 0)
        {
            isActive = false;
            curDeactiveTime -= Time.deltaTime;
        }

        if ((curDeactiveTime < -0.001f || curDeactiveTime > -0.001f) && curDeactiveTime < 0.001f)
        {
            curDeactiveTime = 0f;
        }
        if(GetComponent<CreatureEnemy>().knockBackDuration > 0f)
        {
            curDeactiveTime = GetComponent<CreatureEnemy>().knockBackDuration;
        }
        if(GetComponent<CreatureEnemy>().knockBackDuration == 0 && curDeactiveTime == 0 && GetComponent<Rigidbody2D>().velocity != Vector2.zero)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            curDeactiveTime = deactiveTime/2;
        }
        if (flightPath.x  > transform.position.x)
        {
            GetComponent<CreatureMovement>().currentFacing = 1;
        }
        if (flightPath.x < transform.position.x)
        {
            GetComponent<CreatureMovement>().currentFacing = -1;
        }

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject == target)
        {
            if (isActive)
            {
                curDeactiveTime = deactiveTime;
            }
        }
    }    
}
