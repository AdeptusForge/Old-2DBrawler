using UnityEngine;
using System.Collections;

public class PlayerMovement : CreatureMovement {


    /*--SPEED VARIABLES--*/
    public float maxRunSpeed;
    public float currentRunSpeed;
    public float runForce;
    public float maxAirStrafeSpeed;
    public float currentAirStrafeSpeed;
    public float airStrafeForce;
    public float fallSpeed;
    public float fastFallSpeed;
    public float currentFallSpeed;
    private float currentMaxFallSpeed;
    private bool isFastFalling;
    

    /*--JUMP VARIABLES--*/
    public float minJumpTime;
    public float maxJumpTime;
    public float jumpTimer;
    public float shortJumpForce;
    public float fullJumpForce;
    public float maxRiseSpeed;
    public float currentRiseSpeed;
    public float jumpCheckTime;
    public bool jumpCheck = false;

    /*--DASH VARIABLES--*/
    public float maxDashNumber;
    public float dashRegenRate;
    public float curDashNumber;
    public bool canDash;
    public GameObject dashObjectForward;
    public GameObject dashObjectBackward;
    public GameObject dashObjectUp;
    public GameObject dashObjectDown;
    //public RaycastHit2D dashGroundRayHitRight;
    //public RaycastHit2D dashGroundRayHitLeft;
    //public RaycastHit2D dashGroundRayHitUp;
    //public RaycastHit2D dashGroundRayHitDown;


    /*--OTHER VARIABLES--*/
    public bool canPivot;
    public bool canMove;
    public Weapon myWeapon;
    public float groundDrag;
    public float airDrag;
    public float techTimer;
    public float maxTechTimer;
    public InputManager myInputs;
    public bool canBuffer;
    public float walkAngle;
    public Vector2 slopeWalkVector;

    public enum Direction
    {
        Left,
        Right
    }

    void Awake()
    {
        myWeapon = GetComponentInChildren<Weapon>();
        myInputs = GetComponent<InputManager>();
    }

    protected override void Start()
    {
        base.Start();
        isFastFalling = false;
        canDash = true;
        curDashNumber = maxDashNumber;
    }

    protected override void FixedUpdate()
    {
        CalcWalkVector();
        base.FixedUpdate();
        DragChanger();
        /*--SPEED LIMITERS--*/
        //RUN LIMITER
        if (currentRunSpeed > maxRunSpeed)
        {
            myBody.velocity = new Vector2(maxRunSpeed, myBody.velocity.y);
        }
        if (currentRunSpeed < -maxRunSpeed)
        {
            myBody.velocity = new Vector2(-maxRunSpeed, myBody.velocity.y);
        }
        if (myBody.velocity.x != 0f && (grounded || platformed))
        {
            currentRunSpeed = myBody.velocity.x;
            currentAirStrafeSpeed = 0f;
        }
        else if (myBody.velocity.x == 0f && (grounded || platformed))
        {
            currentRunSpeed = 0f;
        }
        //AIR STRAFE LIMITER
        if (currentAirStrafeSpeed > maxAirStrafeSpeed)
        {
            myBody.velocity = new Vector2(maxAirStrafeSpeed, myBody.velocity.y);
        }
        if (currentAirStrafeSpeed < -maxAirStrafeSpeed)
        {
            myBody.velocity = new Vector2(-maxAirStrafeSpeed, myBody.velocity.y);
        }
        if (myBody.velocity.x != 0f && !grounded && !platformed)
        {
            currentAirStrafeSpeed = myBody.velocity.x;
            currentRunSpeed = 0f;
        }
        else if (myBody.velocity.x == 0f && !grounded && !platformed)
        {
            currentAirStrafeSpeed = 0f;
        }
        //FALLING LIMITER
        if (myBody.velocity.y <= 0f)
        {
            currentFallSpeed = myBody.velocity.y;
        }
        else
        {
            currentFallSpeed = 0f;
        }
        if (Mathf.Abs(currentFallSpeed) >= FallSpeed(isFastFalling))
        {
            myBody.velocity = new Vector2(myBody.velocity.x, -FallSpeed(isFastFalling));
        }
        //RISING LIMITER
        if (myBody.velocity.y >= 0f)
        {
            currentRiseSpeed = myBody.velocity.y;
        }
        else
        {
            currentRiseSpeed = 0f;
        }
        if (currentRiseSpeed >= maxRiseSpeed)
        {
            myBody.velocity = new Vector2(myBody.velocity.x, maxRiseSpeed);
        }

        /*--END OF SPEED LIMITERS--*/

        /*--Dash Regenerator--*/
        if(dashRegenRate > 0f && curDashNumber < maxDashNumber && curDashNumber != maxDashNumber)
        {
            curDashNumber += dashRegenRate * Time.deltaTime;
            //Debug.Log(curDashNumber);
        }
        if(curDashNumber > maxDashNumber)
        {
            curDashNumber = maxDashNumber;
        }


        /*--Jump Check Timer--*/
        if(jumpCheck == true && jumpCheckTime > 0)
        {
            jumpCheckTime -= Time.deltaTime;
        }
        if(jumpCheckTime < 0)
        {
            jumpCheckTime = 0;
            jumpCheck = false;
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        //Debug.Log(jumpNumber +""+ grounded +""+platformed + ""+ isJumping);
        //Debug.Log(grounded +"" + platformed);
        //myInputs.DetectInputsDuringBuffer();
        //myInputs.CheckListForEnumValues();
        base.Update();
        
        if (!impactedGround && canMove)
        {
            if (!isHitStunned)
            {
                if (jumpNumber >= 1f && isJumping == false)
                {
                    /*--JUMP INPUT TIMER--*/
                    if (Input.GetKey(myInputs.jump) && jumpNumber >= 1f && jumpTimer < maxJumpTime)
                    {
                        canPivot = false;
                        if (grounded || platformed)
                        {
                            myBody.velocity = new Vector3(myBody.velocity.x, 0);
                        }
                        if (!grounded && !platformed)
                        {
                            myBody.velocity = new Vector3(myBody.velocity.x, 0);
                        }
                        jumpTimer += Time.deltaTime;
                    }
                    if ((Input.GetKeyUp(myInputs.jump) || jumpTimer > maxJumpTime))
                    {
                        Jump();
                        jumpTimer = 0f;
                    }
                }

                /*--FAST FALL ACTIVATOR/DEACTIVATOR--*/
                if (Input.GetKeyDown(myInputs.down) && !Input.GetKey(myInputs.dash))
                {
                    FastFall();
                }
                if (grounded == true || platformed == true)
                {
                    isFastFalling = false;
                }
                if (Input.GetKey(myInputs.dash))
                {
                    canPivot = false;
                }
                else
                {
                    canPivot = true;
                }


                /*--RUN ACTIVATOR--*/
                if (grounded || platformed)
                {
                    if (Input.GetKey(myInputs.right))
                    {
                        Run(Direction.Right);
                        if(Input.GetKey(myInputs.dash))
                        {
                            DashStart("DashForward");
                        }
                    }
                    if (Input.GetKey(myInputs.left))
                    {
                        Run(Direction.Left);
                        if (Input.GetKey(myInputs.dash))
                        {
                            DashStart("DashForward");
                        }
                    }
                    if (currentFacing == 1f && Input.GetKeyDown(myInputs.left) && canPivot)
                    {
                        Debug.Log("Traditional Pivot");
                        Pivot(currentFacing);
                    }
                    if (currentFacing == -1f && Input.GetKeyDown(myInputs.right) && canPivot)
                    {
                        Debug.Log("Traditional Pivot");
                        Pivot(currentFacing);
                    }
                }

                /*--AIRSTRAFE ACTIVATOR--*/
                if (!grounded && !platformed)
                {
                    if (Input.GetKey(myInputs.right))
                    {
                        Airstrafe(Direction.Right);
                        if (Input.GetKey(myInputs.dash))
                        {
                            if (currentFacing == 1)
                            {
                                Debug.Log("dash input FORWARDS");
                                DashStart("DashForward");
                            }
                            if (currentFacing == -1)
                            {
                                Debug.Log("dash input BACKWARDS");
                                DashStart("DashBackward");
                            }
                        }
                    }
                    if (Input.GetKey(myInputs.left))
                    {
                        Airstrafe(Direction.Left);
                        if (Input.GetKey(myInputs.dash))
                        {
                            if (currentFacing == 1)
                            {
                                Debug.Log("dash input BACKWARDS");
                                DashStart("DashBackward");
                            }
                            if (currentFacing == -1)
                            {
                                Debug.Log("dash input FORWARDS");
                                DashStart("DashForward");
                            }
                        }
                    }
                }
                if (Input.GetKey(myInputs.dash))
                {
                    if (canDash && curDashNumber >= 1f)
                    {
                        if (Input.GetKeyDown(myInputs.left) )
                        {
                            if (currentFacing == 1)
                            {
                                Debug.Log("dash input BACKWARDS");
                                DashStart("DashBackward");
                            }
                            if (currentFacing == -1)
                            {
                                Debug.Log("dash input FORWARDS");
                                DashStart("DashForward");
                            }
                        }
                        if (Input.GetKeyDown(myInputs.right))
                        {
                            if(currentFacing == 1)
                            {
                                Debug.Log("dash input FORWARDS");
                                DashStart("DashForward");
                            }
                            if(currentFacing == -1)
                            {
                                Debug.Log("dash input BACKWARDS");
                                DashStart("DashBackward");
                            }
                        }
                        if (Input.GetKeyDown(myInputs.down))
                        {
                            Debug.Log("dash input DOWN");
                            DashStart("DashDownward");
                        }
                        if (Input.GetKeyDown(myInputs.up))
                        {
                            Debug.Log("dash input UP");
                             DashStart("DashUpward");
                        }
                    }
                }
                if (platformed && Input.GetKey(myInputs.down))
                {
                    isFallingThrough = true;
                    Fallthrough(isFallingThrough);
                }
                else
                {
                    isFallingThrough = false;
                    Fallthrough(isFallingThrough);
                }
            }
            if (isHitStunned)
            {
                if (Input.GetKeyDown(myInputs.dash) && canTech)
                {
                    techTimer = maxTechTimer;
                    canTech = false;
                }
            }   
        }//end of movement activators
        if (impactedGround)
        {
            if (Input.anyKeyDown)
            {
                GetUp();
            }
        }

        /*--FUNCTION INPUT TEST. KEEP UNTIL EVERYTHING WORKS--*/
        if (techTimer > 0f)
        {
            techTimer -= Time.deltaTime;
            isTeching = true;
        }
        if(techTimer > -0.001f && techTimer < 0.001f)
        {
            techTimer = 0f;
        }
        if(techTimer == 0f)
        {
            isTeching = false;
        }

        
    }//end of FixedUpdate


    private void Jump()
    {
        jumpCheck = true;
        jumpCheckTime = 0.3f;
        if(jumpNumber >= 1f)
        {
            if ((grounded || platformed) && !isJumping && !Input.GetKey(myInputs.dash))
            {
                //Debug.Log("Jump Check");
                jumpNumber -= 1f;
                if (jumpTimer <= minJumpTime)
                {
                    isJumping = true;
                    //Debug.Log("shorthop" + isJumping);
                    Move(MoveType.AddForce, new Vector2(0f, shortJumpForce), 1f);
                }
                if (jumpTimer > minJumpTime)
                {
                    isJumping = true;
                    //Debug.Log("fullhop" + isJumping);
                    Move(MoveType.AddForce, new Vector2(0f, fullJumpForce), 1f);
                }
            }
            if (!grounded && !platformed && !sloped && !isJumping)
            {
                //Debug.Log("jump analysis");
                jumpNumber -= 1f;
                StopAllMovement();
                if (jumpTimer <= minJumpTime)
                {
                    isJumping = true;
                    //Debug.Log("shorthop");
                    Move(MoveType.AddForce, new Vector2(0f, shortJumpForce), 1f);
                }
                if (jumpTimer > minJumpTime)
                {
                    isJumping = true;
                    //Debug.Log("fullhop");
                    Move(MoveType.AddForce, new Vector2(0f, fullJumpForce), 1f);
                }
            }
        }
        //Debug.Log(jumpNumber);
        //Debug.Log("jump ended early");
    }

    public float FallSpeed(bool isFastFalling)
    {

        if (!isFastFalling)
        {
            currentMaxFallSpeed = fallSpeed;
        }
        if(isFastFalling)
        {
            currentMaxFallSpeed = fastFallSpeed;
        }
        return currentMaxFallSpeed;
    }
    private void FastFall()
    {
        if (!isFallingThrough)
        {
            isFastFalling = true;
            Move(MoveType.SetVelocity, new Vector2(myBody.velocity.x, -fastFallSpeed), 1f);
        }
    }
    public void DashStart(string dashDirection)
    {
        Debug.Log(dashDirection);
        myAnimator.PlayInFixedTime(dashDirection);
    }
    public void Dash(string dashDirection)
    {
        curDashNumber -= 1;
        if (dashDirection == "Up")
        {
            Debug.Log("dash upward" + dashObjectUp.GetComponent<DashChecker>().dashModiDest);
            Move(MoveType.TransformEdit, dashObjectUp.GetComponent<DashChecker>().dashModiDest, 0f);
        }
        if (dashDirection == "Down")
        {
            Debug.Log("dash downward" + dashObjectDown.GetComponent<DashChecker>().dashModiDest);
            Move(MoveType.TransformEdit, dashObjectDown.GetComponent<DashChecker>().dashModiDest, 0f);
        }
        if (dashDirection == "Forward")
        {
            Debug.Log("dash forward" + dashObjectBackward.GetComponent<DashChecker>().dashModiDest);
            if (currentFacing == 1)
            {
                Move(MoveType.TransformEdit,  dashObjectForward.GetComponent<DashChecker>().dashModiDest, 0f);
            }
            if (currentFacing == -1)
            {
                Move(MoveType.TransformEdit, dashObjectForward.GetComponent<DashChecker>().dashModiDest, 0f);
            }
        }
        if (dashDirection == "Backward")
        {
            if(currentFacing == 1)
            {
                Move(MoveType.TransformEdit, dashObjectBackward.GetComponent<DashChecker>().dashModiDest, 0f);
            }
            if (currentFacing == -1)
            {
                Move(MoveType.TransformEdit, dashObjectBackward.GetComponent<DashChecker>().dashModiDest, 0f);
            }
            Debug.Log("dash backward" + dashObjectBackward.GetComponent<DashChecker>().dashModiDest);
        }
    }
    public void CheckWeapon()
    {
        myWeapon = GetComponentInChildren<Weapon>();
    }
    private void DragChanger()
    {
        if(grounded || platformed)
        {
            myBody.drag = groundDrag;
        }
        if(!grounded && !platformed)
        {
            myBody.drag = airDrag;
        }
    }

    private void Run(Direction dir)
    {
        if(dir == Direction.Left)
        {
            if (currentFacing != -1 && canPivot && !Input.GetKey(myInputs.right))
            {
                Pivot(currentFacing);
            }
            if (currentFacing == -1)
            {
                Move(MoveType.AddForce, Vector2.left, runForce);
            }
        }
        else if (dir == Direction.Right)
        {
            if (currentFacing != 1 && canPivot && !Input.GetKey(myInputs.left))
            {
                Pivot(currentFacing);
            }
            if (currentFacing == 1)
            {
                Move(MoveType.AddForce, Vector2.right, runForce);
            }
        }
    }

    private void Airstrafe(Direction dir)
    {
        if(dir == Direction.Left)
        {
            if (!Input.GetKey(myInputs.right))
            {
                Move(MoveType.AddForce, Vector2.left, airStrafeForce);
            }
            else if (Input.GetKey(myInputs.right))
            {
                Debug.Log("Left -> Right");
                Move(MoveType.AddForce, Vector2.right, airStrafeForce);
            }
        }
        if (dir == Direction.Right)
        {
            if (!Input.GetKey(myInputs.left))
            {
                Move(MoveType.AddForce, Vector2.right, airStrafeForce);
            }
            else if (Input.GetKey(myInputs.left))
            {
                Debug.Log("Right -> Left");
                Move(MoveType.AddForce, Vector2.left, airStrafeForce);
            }
        }
    }




    public void PerformBufferedAction()
    {
        myInputs.CheckListForEnumValues();
        myAnimator.PlayInFixedTime(myInputs.bufferedActionString);
    }
    public void CalcWalkVector()
    {
        walkAngle = GetComponent<CreatureMovement>().degree + 90f;

        float sinOfDegree = Mathf.Sin((walkAngle * Mathf.PI) / 180);
        if (sinOfDegree > -0.0001f && sinOfDegree < 0.0001f)
        {
            sinOfDegree = 0f;
        }
        float cosOfDegree = Mathf.Cos((walkAngle * Mathf.PI) / 180);
        if (cosOfDegree > -0.0001f && cosOfDegree < 0.0001f)
        {
            cosOfDegree = 0f;
        }

        slopeWalkVector = new Vector2(cosOfDegree, sinOfDegree);
        //Debug.DrawRay(this.gameObject.transform.position, slopeWalkVector);
    }
}
