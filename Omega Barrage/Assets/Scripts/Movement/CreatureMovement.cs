using UnityEngine;
using System.Collections;
using System;

public abstract class CreatureMovement : CreatureActions {

    public enum MoveType
    {
        SetVelocity,
        AddForce,
        TranslateDirectly,
        TransformEdit,
    }
    
    /*-- GROUND CHECKER VARIABLES --*/
    public GameObject groundCheck;
    public LayerMask whatIsGround;
    public LayerMask whatIsPlatform;
    public bool ignorePlatforms;
    public bool grounded;
    public bool platformed;
    public bool sloped;
    protected bool impactedGround;
    protected bool isTeching;

    public float jumpNumber;
    protected bool isJumping;
    public static float myGravity;
    public float myGravityForce;
    private Vector2 gravityDirection;
    private float gravityForce;
    protected MoveType moveType;
    protected Rigidbody2D myBody;
    public float currentFacing;

    protected bool isFallingThrough;
    public float hitStunTimer;
    protected bool canTech = true;
    public float degree = 0f;

    // Use this for initialization
    protected override void Start ()
    {
        base.Start();
        impactedGround = false;
        isTeching = false;
        myBody = GetComponent<Rigidbody2D>();
        myGravity = GetComponent<Rigidbody2D>().gravityScale;
        currentFacing = transform.localScale.x;
        GravityReset();
    }
	
    protected virtual void FixedUpdate()
    {
        Gravity();

        /*--GROUND AND PLATFORM CHECKER--*/
        if(groundCheck != null)
        {
            if (!groundCheck.GetComponent<Collider2D>().IsTouchingLayers(whatIsGround))
            {
                //Debug.Log("IS NOT TOUCHING GROUND");
                if (groundCheck.GetComponent<Collider2D>().IsTouchingLayers(whatIsPlatform))
                {
                    //Debug.Log("IS TOUCHING PLATFORM");
                    if (!isHitStunned)
                    {
                        if (gameObject.GetComponent<PlayerMovement>() != null && gameObject.GetComponent<PlayerMovement>().jumpCheckTime <= 0)
                        {
                            platformed = true;
                            //Debug.Log("jumps reset");
                            jumpNumber = 1f;
                            isJumping = false;
                        }
                        if (gameObject.GetComponent<PlayerMovement>() == null)
                        {
                        }
                    }
                    if (isHitStunned)
                    {
                    }
                }
            }
            if (groundCheck.GetComponent<Collider2D>().IsTouchingLayers(whatIsGround))
            {
                //Debug.Log("IS TOUCHING GROUND");
                if (!isHitStunned)
                {
                    if (gameObject.GetComponent<PlayerMovement>() != null && gameObject.GetComponent<PlayerMovement>().jumpCheckTime <= 0)
                    {
                        grounded = true;
                        //Debug.Log("jumps reset");
                        jumpNumber = 1f;
                        isJumping = false;
                        gameObject.GetComponent<PlayerMovement>().jumpCheck = false;
                    }
                    if (gameObject.GetComponent<PlayerMovement>() == null)
                    {

                    }
                }
                if (isHitStunned)
                {
                    if (GetComponent<Creature>().knockBackDuration <= 0f)
                    {
                        if (!isTeching)
                        {
                            OnTheGround();
                        }
                        if (isTeching)
                        {
                            FastGetUp();
                        }
                    }
                    if (GetComponent<Creature>().knockBackDuration > 0f)
                    {
                    }
                }
            }
            if (!groundCheck.GetComponent<Collider2D>().IsTouchingLayers(whatIsGround) && !groundCheck.GetComponent<Collider2D>().IsTouchingLayers(whatIsPlatform))
            {
                //Debug.Log("NOT TOUCHING ANYTHING");
                grounded = false;
                platformed = false;
            }
        }

        hitStunTimer -= Time.deltaTime;
        if (hitStunTimer > 0f)
        {
            isHitStunned = true;
        }

        if (hitStunTimer < 0f)
        {
            hitStunTimer = 0f;
        }
        if (hitStunTimer == 0f)
        {
            isHitStunned = false;
        }
        if (hitStunTimer > -0.001f && hitStunTimer < 0.001f)
        {
            hitStunTimer = 0f;
        }
    }

    // Update is called once per frame
    protected virtual void Update ()
    {}


    public virtual void Move(MoveType moveTypeEnum, Vector2 direction, float speed)
    {
        switch (moveTypeEnum)
        {
            case MoveType.SetVelocity:
                myBody.velocity = direction * speed;
                break;
            case MoveType.AddForce:
                myBody.AddForce(direction * speed);
                break;
            case MoveType.TranslateDirectly:
                transform.Translate(direction * speed);
                break;
            case MoveType.TransformEdit:
                transform.position = new Vector3(transform.position.x + direction.x, transform.position.y + direction.y, transform.position.z);
                break;
            default:
                Debug.Log("Unusable Movetype");
                break;
        }
    }

    //fallthrough platforms
    protected void Fallthrough(bool ignore)
    {
        Physics2D.IgnoreLayerCollision(gameObject.layer, 10, ignore);
    }

    public void Pivot(float facing)
    {
        facing = currentFacing * -1f;
        currentFacing = -currentFacing;
        transform.localScale = new Vector3(facing, myBody.transform.localScale.y, myBody.transform.localScale.z);
        if (!sloped)
        {
        }
        if (sloped)
        {
        }
    }

    public void StopAllMovement()
    {
        myBody.velocity = Vector3.zero;
    }
    protected void OnTheGround()
    {
        if (!impactedGround)
        {
            impactedGround = true;
        }
    }
    public void FastGetUp()
    {
        Debug.Log("you just teched broh");
        impactedGround = false;
        hitStunTimer = 0f;
    }
    public void GetUp()
    {
        Debug.Log("has gotten up");
        impactedGround = false;
        hitStunTimer = 0f;
    }
    public void Gravity()
    {
        if (GetComponent<Creature>().knockBackDuration > 0f)
        {
            //Debug.Log("Gravity should be off");
            //gravityForce = 0f;
        }
        if (GetComponent<Creature>().knockBackDuration != -1320f)
        {
            gravityForce = myGravityForce;
        }

        Move(MoveType.AddForce, gravityDirection, gravityForce);
    }
    private void CalcGravityVector(GameObject slope)
    {

        if (slope != null)
        {
            degree = slope.GetComponent<Slope>().slopeAngle;
        }
        else
        {
            degree = 0f;
        }
        degree -= 90f;
        float sinOfDegree = Mathf.Sin((degree * Mathf.PI) / 180);
        if (sinOfDegree > -0.0001f && sinOfDegree < 0.0001f)
        {
            sinOfDegree = 0f;
        }
        float cosOfDegree = Mathf.Cos((degree * Mathf.PI) / 180);
        if (cosOfDegree > -0.0001f && cosOfDegree < 0.0001f)
        {
            cosOfDegree = 0f;
        }
        gravityDirection = new Vector2(cosOfDegree, sinOfDegree);
        //Debug.DrawRay(this.gameObject.transform.position, gravityDirection);
    }
    public void GravityReset()
    {
        gravityDirection = Vector2.down;
    }
}
