using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class EnemyAI : MonoBehaviour {

    public List<RangeData> ranges = new List<RangeData>();

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
    public enum Range
    {
        Short,
        Medium,
        Long,
        Extreme,
        None
    }

    public enum MovementType
    {
        Stationary,
        Ground,
        Flying,
        Other
    }

    public RangeData targetRangeLock;
    public RangeData targetCurrentRange;
    public bool aiActive = false;
    public bool targetLocked = false;

    public MovementType moveType;
    public Creature myTarget;
    private Rigidbody2D myBody;
    private Vector2 movementVector;
    public float speed;
    public Vector2 lastTrackedPosition;
    public bool isMoving;
    public bool preferenceSet = false;


    public List<AIAbility> myAbilities= new List<AIAbility>();










    // Use this for initialization
    protected virtual void Start () {
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

        foreach (AIAbility component in GetComponents<AIAbility>())
        {
            myAbilities.Add(component);
        }
        UsePercentCalculator(Range.Short);
    }
	
	// Update is called once per frame
	protected virtual void FixedUpdate () {
        if (aiActive)
        {
            if (myTarget != null)
            {
                RangeFinder(myTarget);
                RangeLock(targetCurrentRange);
                TrackTarget();
                AIMovementTracker();
            }
            if (myTarget == null)
            {
                FindTarget();
            }
        }
        if (!aiActive)
        {
            Debug.Log("AI DEACTIVATED");
        }
    }

    protected virtual void RangeFinder(Creature currentTarget)
    {
        Debug.Log("Rangefinder ACTIVE");
        float tempDistance = Vector2.Distance(transform.position, currentTarget.transform.position);
        if (tempDistance > longRange.rangeMaxDistance)
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

    protected virtual void RangeLock(RangeData rangeLocker)
    {
        if (targetLocked == true)
        {
            Debug.Log("Target locked at:" + targetRangeLock.range);
            if (targetRangeLock.currentPreference <= 0)
            {
                Debug.Log("RANGE PREFERENCE DEPLETED");
                UnlockTarget();
            }
            if (targetRangeLock.currentPreference > 0)
            {
                if (rangeLocker.currentPreference > targetRangeLock.currentPreference)
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

    protected virtual void TrackTarget()
    {
        if (targetLocked)
        {
            if (targetRangeLock != targetCurrentRange)
            {
                if (myTarget.transform.localPosition.x > transform.position.x)
                {
                    if (targetRangeLock.rangeMaxDistance > targetCurrentRange.rangeMaxDistance)
                    {
                        AvoidTarget();
                    }
                    if (targetRangeLock.rangeMaxDistance < targetCurrentRange.rangeMaxDistance)
                    {
                        ChaseTarget();
                    }
                }
                if (myTarget.transform.localPosition.x < transform.position.x)
                {
                    if (targetRangeLock.rangeMaxDistance > targetCurrentRange.rangeMaxDistance)
                    {
                        AvoidTarget();
                    }
                    if (targetRangeLock.rangeMaxDistance < targetCurrentRange.rangeMaxDistance)
                    {
                        ChaseTarget();
                    }
                }
            }
        }
    }
    /*--Input Direction is the TARGET'S direction, not the direction of movement--*/
    protected virtual void ChaseTarget()
    {
        Debug.Log("CHASING TARGET");

        switch (moveType)
        {
            case MovementType.Ground:
                {
                    break;
                }
            case MovementType.Flying:
                {
                    break;
                }
            case MovementType.Other:
                {
                    break;
                }
            case MovementType.Stationary:
                {
                    Debug.Log("AI IS STATIONARY");
                    break;
                }
        }
    }
    protected virtual void AvoidTarget()
    {
        Debug.Log("AVOIDING TARGET");
        switch (moveType)
        {
            case MovementType.Ground:
                {
                    break;
                }
            case MovementType.Flying:
                {
                    break;
                }
            case MovementType.Other:
                {
                    break;
                }
            case MovementType.Stationary:
                {
                    Debug.Log("AI IS STATIONARY");
                    break;
                }
        }
    }
    protected virtual void AIActivator()
    {
        //Debug.Log("Enemy Activated:"+ this.gameObject.name);
        aiActive = true;
    }

    protected virtual void AIMovementTracker()
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

        if (isMoving == false)
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

    protected virtual void FindTarget()
    {
        /*--Use to find player or other faction enemies--*/
        Debug.Log("FINDING TARGET");
        List<Collider2D> hitColliders = new List<Collider2D>();
        List<Creature> possibleTargets = new List<Creature>();
        if (Physics2D.OverlapCircleAll(transform.position, longRange.rangeMaxDistance, 9).Length != 0)
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
        foreach (Creature target in possibleTargets)
        {
            if (Vector2.Distance(transform.position, target.transform.position) < tempDistance)
            {
                tempDistance = Vector2.Distance(transform.position, target.transform.position);
                myTarget = target;
            }
        }
        Debug.Log("TARGET FOUND");
    }

    protected virtual void UnlockTarget()
    {
        targetRangeLock = null;
        targetLocked = false;
    }

    protected virtual void AIReset()
    {
        myTarget = null;
        foreach (RangeData range in ranges)
        {
            range.currentPreference = 0;
        }
    }








    protected virtual void UsePercentCalculator(Range rangeToCalcFor)
    {
        //Debug.Log("PercentChecker Activated");
        float numberPool = 0f;
        List<AIAbility> possibleAbilities = new List<AIAbility>();
        foreach(AIAbility ability in myAbilities)
        {
            if (ability.combatUseRanges.Contains(rangeToCalcFor))
            {
                possibleAbilities.Add(ability);
            }
        }
        foreach(AIAbility ability in possibleAbilities)
        {
            //Debug.Log(ability.abilityName);
            if (ability.charges >= 1)
            {
                numberPool += ability.useNumber;
                //Debug.Log(numberPool + "" + ability.abilityName);
            }
        }
        float tempPercent = 0;
        foreach (AIAbility ability in possibleAbilities)
        {
            ability.usePercent = ability.useNumber / numberPool;

            ability.lowerLimit = 0 + tempPercent;
            tempPercent += ability.usePercent;
            if(ability.lowerLimit + ability.usePercent <= 1)
            {
                ability.upperLimit = ability.lowerLimit + ability.usePercent;
            }
            if(ability.lowerLimit + ability.usePercent > 1)
            {
                ability.upperLimit = 1;
            }
            
        }
        tempPercent = Random.value;
        //Debug.Log(tempPercent);
        foreach(AIAbility ability in possibleAbilities)
        {
            if (tempPercent > ability.upperLimit && tempPercent < ability.lowerLimit)
            {
                possibleAbilities.Remove(ability);
            }
            if (tempPercent <= ability.upperLimit && tempPercent >= ability.lowerLimit)
            {
                Debug.Log(ability.abilityName);
            }
        }
        //Debug.Log(numberPool);        
    }
}
