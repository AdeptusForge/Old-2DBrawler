using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAITestScript : MonoBehaviour {

    private List<RangeData> ranges = new List<RangeData>();

    public RangeData extremeRange;
    public RangeData longRange;
    public RangeData mediumRange;
    public RangeData shortRange;
    public RangeData nonerange;

    [System.Serializable]
    public class RangeData
    {
        public Range range;
        public float rangeMaxDistance;
        public float rangeMaxPreference;
        public float currentPreference;
    }

    private enum Direction
    {
        Right,
        Left
    }


    /*--VARIABLES--*/
    public enum Range
    {
        Short,
        Medium,
        Long,
        Extreme,
        None
    }

    public RangeData targetRangeLock;
    public RangeData targetCurrentRange;
    public bool aiActive = false;
    public bool targetLocked = false;

    public Creature myTarget;
    private Rigidbody2D myBody;
    private Vector2 movementVector;
    public float speed;
    public Vector2 lastTrackedPosition;
    public bool isMoving;
    public bool preferenceSet = false;




    // Use this for initialization
    void Start ()
    {
        ranges.Add(longRange);
        ranges.Add(mediumRange);
        ranges.Add(shortRange);


        targetCurrentRange = nonerange;
        targetRangeLock = nonerange;
        myBody = GetComponent<Rigidbody2D>();
        AIReset();
        longRange.currentPreference = longRange.rangeMaxPreference;
        mediumRange.currentPreference = mediumRange.rangeMaxPreference;
        shortRange.currentPreference = shortRange.rangeMaxPreference;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        //Debug.Log(targetCurrentRange);
        if (aiActive)
        {
            if(myTarget != null)
            {
                RangeFinder(myTarget);
                RangeLock(targetCurrentRange);
                TrackTarget();
                AIMovementTracker();
            }
            if(myTarget == null)
            {
                FindTarget();
            }
        }
        if (!aiActive)
        {
            Debug.Log("AI DEACTIVATED");
        }
    }

    public void AIActivator()
    {
        //Debug.Log("Enemy Activated:"+ this.gameObject.name);
        aiActive = true;
    }

    /*--Determines the target's location in relation to the enemy in question*/
    private void RangeFinder(Creature currentTarget)
    {
        //Debug.Log("Rangefinder ACTIVE");
        float tempDistance = Vector2.Distance(transform.position, currentTarget.transform.position);
        if(tempDistance > longRange.rangeMaxDistance)
        {
            /*--Extreme Range--*/
            targetCurrentRange = extremeRange;
        }
        else if (tempDistance <= longRange.rangeMaxDistance && tempDistance > mediumRange.rangeMaxDistance)
        {
            /*--Long Range--*/
            targetCurrentRange = longRange;
        }
        else if (tempDistance <= mediumRange.rangeMaxDistance && tempDistance > shortRange.rangeMaxDistance)
        {
            /*--Medium Range--*/
            targetCurrentRange = mediumRange;
        }
        else if (tempDistance <= shortRange.rangeMaxDistance && tempDistance > 0)
        {
            /*--Short Range--*/
            targetCurrentRange = shortRange;
        }
        else
        {
            /*--None Range--*/
            targetCurrentRange = nonerange;
        }
    }
    
    /*--Locks player range. Deteremined by range preference--*/
    private void RangeLock(RangeData rangeLocker)
    {
        if (targetLocked == true)
        {
            Debug.Log("Target locked at:" + targetRangeLock.range);
            if (targetRangeLock.currentPreference <= 0)
            {
                Debug.Log("RANGE PREFERENCE DEPLETED");
                UnlockTarget();
            }
            if(targetRangeLock.currentPreference > 0)
            {
                if(rangeLocker.currentPreference > targetRangeLock.currentPreference)
                {
                    if (rangeLocker.rangeMaxPreference > targetRangeLock.rangeMaxPreference)
                    {
                        Debug.Log("TARGET LOCK SWITCH TO HIGHER PRFERENCE");
                        targetRangeLock = rangeLocker;
                    }
                }
            }
        }
        if (targetLocked == false)
        {
            Debug.Log("Target not locked");
            targetRangeLock = rangeLocker;
            targetLocked = true;
        }        
    }

    private void UnlockTarget()
    {
        targetRangeLock = null;
        targetLocked = false;
    }

    /*--Determines if the player is too close or too far from the perferred range--*/
    public void TrackTarget()
    {
        if (targetLocked)
        {
            if (targetRangeLock != targetCurrentRange)
            {
                if (myTarget.transform.localPosition.x > transform.position.x)
                {
                    if (targetRangeLock.rangeMaxDistance > targetCurrentRange.rangeMaxDistance)
                    {
                        AvoidTarget(Direction.Right);
                    }
                    if (targetRangeLock.rangeMaxDistance < targetCurrentRange.rangeMaxDistance)
                    {
                        ChaseTarget(Direction.Right);
                    }
                }
                if (myTarget.transform.localPosition.x < transform.position.x)
                {
                    if (targetRangeLock.rangeMaxDistance > targetCurrentRange.rangeMaxDistance)
                    {
                        AvoidTarget(Direction.Left);
                    }
                    if (targetRangeLock.rangeMaxDistance < targetCurrentRange.rangeMaxDistance)
                    {
                        ChaseTarget(Direction.Left);
                    }
                }
            }
        }
    }
    /*--Input Direction is the TARGET'S direction, not the direction of movement--*/
    private void ChaseTarget(Direction dir)
    {
        Debug.Log("CHASING TARGET ON THE " + dir.ToString());
        if(dir == Direction.Right)
        {
        }
        if (dir == Direction.Left)
        {
        }
    }
    private void AvoidTarget(Direction dir)
    {
        Debug.Log("AVOIDING TARGET ON THE " + dir.ToString());
        if (dir == Direction.Right)
        {
        }
        if (dir == Direction.Left)
        {
        }
    }

    private void AIMovementTracker()
    {
        
        if (myBody.velocity.x >= -0.01 && myBody.velocity.x <= 0.01)
        {
            myBody.velocity = new Vector2(0, myBody.velocity.y);
        }
        if (myBody.velocity.y >= -0.01 && myBody.velocity.y <= 0.01)
        {
            myBody.velocity = new Vector2(myBody.velocity.x, 0);
        }

        if (myBody.gameObject.GetComponent<CreatureActions>().isHitStunned || myBody.velocity == Vector2.zero)
        {
            isMoving = false;
        }
        if (myBody.velocity != Vector2.zero && !GetComponent<CreatureActions>().isHitStunned)
        {
            isMoving = true;
        }

        if(isMoving == false)
        {
            lastTrackedPosition = transform.position;
            Debug.DrawRay(Vector3.zero, lastTrackedPosition);
        }
        if (isMoving == true)
        {
            float currentPreferenceChange = 0f;
            currentPreferenceChange = Vector3.Distance(transform.position, lastTrackedPosition);
            //Debug.Log(currentPreferenceChange);
        }
    }

    void FindTarget()
    {
        /*--Use to find player or other faction enemies--*/
        Debug.Log("FINDING TARGET");
        List<Collider2D> hitColliders = new List<Collider2D>();
        List<Creature> possibleTargets = new List<Creature>();
        if(Physics2D.OverlapCircleAll(transform.position, longRange.rangeMaxDistance, 9).Length != 0)
        {
            foreach (Collider2D thing in Physics2D.OverlapCircleAll(transform.position, longRange.rangeMaxDistance, 9))
            {
                if (thing.GetComponent<Creature>() != null)
                {
                    if (GetComponent<Creature>().myEnemies.Contains(thing.GetComponent<Creature>().myFaction))
                    {
                        possibleTargets.Add(thing.GetComponent<Creature>());
                    }
                }
            }
        }
        else
        {
            Debug.Log("NO TARGETS IN RANGE");
        }
        float tempDistance = 1000f;
        foreach(Creature target in possibleTargets)
        {
            if(Vector2.Distance(transform.position, target.transform.position) < tempDistance)
            {
                tempDistance = Vector2.Distance(transform.position, target.transform.position);
                myTarget = target;
            }
        }
        Debug.Log("TARGET FOUND");
    }

    void AIReset()
    {
        myTarget = null;
        foreach(RangeData range in ranges)
        {
            range.currentPreference = 0;
        }
    }
}