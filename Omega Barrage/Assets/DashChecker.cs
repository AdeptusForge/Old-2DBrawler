using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashChecker : MonoBehaviour {

    public enum dashDirection
    {
        Up,
        Down,
        Forward,
        Backward
    };
    public enum dashAdjustment
    {
        Closer,
        Further,
        Adjacent
    };

    public dashDirection checkDirection;
    public Vector2 dashInitDest;
    public Vector2 dashModiDest;
    private Vector3 dashModifier;
    public Weapon myWeapon;
    public PlayerMovement myPlayerMovement;
    private ContactPoint2D modiPoint;
    public ColliderDistance2D collDist;
    public float totalDashDist;
    public float maxDashDist;
    public float minDashDist;
    public Collider2D myCollider;

    public bool canDashUnhindered;
    public bool CanDashUnhindered
    {
        get { return canDashUnhindered; }
    }
    public bool canDashHindered;
    public bool CanDashHindered
    {
        get { return canDashHindered; }
    }
    private Vector3 pointA;
    private Vector3 pointB;
    private Vector3 pointC;
    private Vector3 pointD;
    public Bounds collBounds;
    public List<dashAdjustment> possibleDashAdjusts;
    private dashAdjustment dashAdjusterDirection;



    // Use this for initialization
    void Start() {
        //Debug.Log(gameObject.name);
        canDashUnhindered = true;
        canDashHindered = true;
        myCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void FixedUpdate() {
        //if (DashCheck() == false && DashCheck() == false && DashCheck() == false)
        //{
        //    canDashHindered = false;
        //}
        pointA = new Vector3(transform.position.x + 0.5f, transform.position.y, 0f);
        pointB = new Vector3(transform.position.x - 0.5f, transform.position.y, 0f);
        pointC = new Vector3(transform.position.x, transform.position.y + 1f, 0f);
        pointD = new Vector3(transform.position.x, transform.position.y - 1f, 0f);

        //Debug.Log(dashInitDest);
        //Debug.Log(dashModiDest);
        //Debug.Log(collDist.isOverlapped + this.gameObject.name);
        //Debug.DrawLine(myPlayerMovement.transform.position, new Vector3(myPlayerMovement.transform.position.x +dashModiDest.x, myPlayerMovement.transform.position.y + dashModiDest.y), Color.red);
        switch (checkDirection)
        {
            case dashDirection.Up:
                {
                    //Debug.Log(myWeapon.dashDestUp);
                    transform.localPosition = myWeapon.dashDestUp;
                    dashInitDest = myWeapon.dashDestUp;
                    break;
                }
            case dashDirection.Down:
                {
                    //Debug.Log(myWeapon.dashDestDown);
                    transform.localPosition = myWeapon.dashDestDown;
                    dashInitDest = myWeapon.dashDestDown;
                    break;
                }
            case dashDirection.Forward:
                {
                    //Debug.Log(myWeapon.dashDestForward);
                    transform.localPosition = myWeapon.dashDestForward;
                    dashInitDest = myWeapon.dashDestForward;
                    break;
                }
            case dashDirection.Backward:
                {
                    //Debug.Log(myWeapon.dashDestBackward);
                    transform.localPosition = myWeapon.dashDestBackward;
                    dashInitDest = myWeapon.dashDestBackward;
                    break;
                }
        }



        //Debug.Log(canDashUnhindered + this.gameObject.name);
        if (canDashUnhindered == true)
        {
            dashModiDest = new Vector2(dashInitDest.x * myPlayerMovement.currentFacing, dashInitDest.y);
        }

        totalDashDist = Vector3.Distance(myPlayerMovement.gameObject.transform.position, new Vector3(dashModiDest.x + myPlayerMovement.gameObject.transform.position.x, dashModiDest.y + myPlayerMovement.gameObject.transform.position.y));
        //Debug.Log(totalDashDist + this.gameObject.name);
        if (totalDashDist > maxDashDist)
        {
        }



        if (totalDashDist < minDashDist || totalDashDist > maxDashDist)
        {
            canDashHindered = false;
        }
        if(totalDashDist > minDashDist && totalDashDist < maxDashDist)
        {
            canDashHindered = true;
        }
        if (canDashHindered == false && canDashUnhindered == false)
        {
            //Debug.Log("can't dash that way moron" + this.gameObject.name);
        }
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        //Debug.Log(other + this.gameObject.name);
        if (other == null)
        {
            Debug.Log("is working" + this.gameObject.name);
            canDashUnhindered = true;
        }
        if (other != null)
        {
            if (other.gameObject.layer == 8)
            {
                canDashUnhindered = false;
            }
            else if (other.gameObject.layer != 8)
            {
                //Debug.Log("" + other.gameObject.layer + "" + this.gameObject.name);
                canDashUnhindered = true;
            }
        }
    }
    void OnCollisionExit2D(Collision2D other)
    {
        //Debug.Log("collider exit");
        canDashUnhindered = true;
        dashModifier = Vector3.zero;
        possibleDashAdjusts = new List<dashAdjustment>();
    }

    void OnCollisionStay2D(Collision2D other)
    {
        collDist = gameObject.GetComponent<Collider2D>().Distance(other.collider);
        collDist.pointA = transform.position;
        collBounds = other.collider.bounds;

        bool pointAOverlap = other.collider.OverlapPoint(pointA);
        bool pointBOverlap = other.collider.OverlapPoint(pointB);
        bool pointCOverlap = other.collider.OverlapPoint(pointC);
        bool pointDOverlap = other.collider.OverlapPoint(pointD);

        //Debug.Log(pointAOverlap + "" + pointBOverlap + pointCOverlap + pointDOverlap + this.gameObject.name);

        switch (pointAOverlap)
        {
            case true:
                {
                    switch (pointBOverlap)
                    {
                        case true:
                            {
                                switch (pointCOverlap)
                                {
                                    case true:
                                        {
                                            switch (pointDOverlap)
                                            {
                                                case true:
                                                    {
                                                        /*--Overlap All--*/
                                                        DashChange(dashAdjustment.Further, dashAdjustment.Closer);
                                                        break;
                                                    }
                                                case false:
                                                    {
                                                        /*--Overlap All But Bottom--*/
                                                        switch (checkDirection)
                                                        {
                                                            case dashDirection.Forward:
                                                                {
                                                                    DashChange(dashAdjustment.Further, dashAdjustment.Closer);
                                                                    break;
                                                                }
                                                            case dashDirection.Backward:
                                                                {
                                                                    DashChange(dashAdjustment.Further, dashAdjustment.Closer);
                                                                    break;
                                                                }
                                                            case dashDirection.Up:
                                                                {
                                                                    DashChange(dashAdjustment.Further, dashAdjustment.Closer);
                                                                    break;
                                                                }
                                                            case dashDirection.Down:
                                                                {
                                                                    DashChange(dashAdjustment.Further, dashAdjustment.Closer);
                                                                    break;
                                                                }
                                                        }
                                                        break;
                                                    }

                                            }
                                            break;
                                        }
                                    case false:
                                        {
                                            switch (pointDOverlap)
                                            {
                                                case true:
                                                    {
                                                        /*--Overlap All But Top--*/
                                                        switch (checkDirection)
                                                        {
                                                            case dashDirection.Forward:
                                                                {
                                                                    DashChange(dashAdjustment.Adjacent, dashAdjustment.Further, dashAdjustment.Closer);
                                                                    break;
                                                                }
                                                            case dashDirection.Backward:
                                                                {
                                                                    DashChange(dashAdjustment.Adjacent, dashAdjustment.Further, dashAdjustment.Closer);
                                                                    break;
                                                                }
                                                            case dashDirection.Up:
                                                                {
                                                                    DashChange(dashAdjustment.Further, dashAdjustment.Closer);
                                                                    break;
                                                                }
                                                            case dashDirection.Down:
                                                                {
                                                                    DashChange(dashAdjustment.Further, dashAdjustment.Closer);
                                                                    break;
                                                                }
                                                        }
                                                        break;
                                                    }
                                                case false:
                                                    {
                                                        /*--Overlap All But Top & Bottom--*/
                                                        switch (checkDirection)
                                                        {
                                                            case dashDirection.Forward:
                                                                {
                                                                    DashChange(dashAdjustment.Further, dashAdjustment.Closer);
                                                                    break;
                                                                }
                                                            case dashDirection.Backward:
                                                                {
                                                                    DashChange(dashAdjustment.Further, dashAdjustment.Closer);
                                                                    break;
                                                                }
                                                            case dashDirection.Up:
                                                                {
                                                                    DashChange(dashAdjustment.Further, dashAdjustment.Closer);
                                                                    break;
                                                                }
                                                            case dashDirection.Down:
                                                                {
                                                                    DashChange(dashAdjustment.Further, dashAdjustment.Closer);
                                                                    break;
                                                                }
                                                        }
                                                        break;
                                                    }
                                            }
                                            break;
                                        }
                                }
                                break;
                            }
                        case false:
                            {
                                switch (pointCOverlap)
                                {
                                    case true:
                                        {
                                            switch (pointDOverlap)
                                            {
                                                case true:
                                                    {
                                                        /*--Overlap All But Back--*/
                                                        switch (checkDirection)
                                                        {
                                                            case dashDirection.Forward:
                                                                {
                                                                    DashChange(dashAdjustment.Further, dashAdjustment.Closer);
                                                                    break;
                                                                }
                                                            case dashDirection.Backward:
                                                                {
                                                                    DashChange(dashAdjustment.Further, dashAdjustment.Closer);
                                                                    break;
                                                                }
                                                            case dashDirection.Up:
                                                                {
                                                                    DashChange(dashAdjustment.Further, dashAdjustment.Closer);
                                                                    break;
                                                                }
                                                            case dashDirection.Down:
                                                                {
                                                                    DashChange(dashAdjustment.Further, dashAdjustment.Closer);
                                                                    break;
                                                                }
                                                        }
                                                        break;
                                                    }
                                                case false:
                                                    {
                                                        /*--Overlap All But Back & Bottom--*/
                                                        switch (checkDirection)
                                                        {
                                                            case dashDirection.Forward:
                                                                {
                                                                    DashChange(dashAdjustment.Further, dashAdjustment.Closer);
                                                                    break;
                                                                }
                                                            case dashDirection.Backward:
                                                                {
                                                                    DashChange(dashAdjustment.Further, dashAdjustment.Closer);
                                                                    break;
                                                                }
                                                            case dashDirection.Up:
                                                                {
                                                                    DashChange(dashAdjustment.Further, dashAdjustment.Closer);
                                                                    break;
                                                                }
                                                            case dashDirection.Down:
                                                                {
                                                                    DashChange(dashAdjustment.Further, dashAdjustment.Closer);
                                                                    break;
                                                                }
                                                        }
                                                        break;
                                                    }
                                            }
                                            break;
                                        }
                                    case false:
                                        {
                                            switch (pointDOverlap)
                                            {
                                                case true:
                                                    {
                                                        /*--Overlap All But Back & Top--*/
                                                        switch (checkDirection)
                                                        {
                                                            case dashDirection.Forward:
                                                                {
                                                                    DashChange(dashAdjustment.Further, dashAdjustment.Closer);
                                                                    break;
                                                                }
                                                            case dashDirection.Backward:
                                                                {
                                                                    DashChange(dashAdjustment.Further, dashAdjustment.Closer);
                                                                    break;
                                                                }
                                                            case dashDirection.Up:
                                                                {
                                                                    DashChange(dashAdjustment.Further, dashAdjustment.Closer);
                                                                    break;
                                                                }
                                                            case dashDirection.Down:
                                                                {
                                                                    DashChange(dashAdjustment.Further, dashAdjustment.Closer);
                                                                    break;
                                                                }
                                                        }
                                                        break;
                                                    }
                                                case false:
                                                    {
                                                        /*--Overlap Front Only--*/
                                                        switch (checkDirection)
                                                        {
                                                            case dashDirection.Forward:
                                                                {
                                                                    DashChange(dashAdjustment.Further, dashAdjustment.Closer);
                                                                    break;
                                                                }
                                                            case dashDirection.Backward:
                                                                {
                                                                    DashChange(dashAdjustment.Further, dashAdjustment.Closer);
                                                                    break;
                                                                }
                                                            case dashDirection.Up:
                                                                {
                                                                    DashChange(dashAdjustment.Adjacent, dashAdjustment.Closer, dashAdjustment.Further);
                                                                    break;
                                                                }
                                                            case dashDirection.Down:
                                                                {
                                                                    DashChange(dashAdjustment.Adjacent, dashAdjustment.Closer, dashAdjustment.Further);
                                                                    break;
                                                                }
                                                        }
                                                        break;
                                                    }
                                            }
                                            break;
                                        }
                                }
                            }
                            break;
                    }
                    break;
                }
            case false:
                {
                    switch (pointBOverlap)
                    {
                        case true:
                            {
                                switch (pointCOverlap)
                                {
                                    case true:
                                        {
                                            switch (pointDOverlap)
                                            {
                                                case true:
                                                    {
                                                        /*--Overlap All But Front--*/
                                                        switch (checkDirection)
                                                        {
                                                            case dashDirection.Forward:
                                                                {
                                                                    DashChange(dashAdjustment.Further, dashAdjustment.Closer);
                                                                    break;
                                                                }
                                                            case dashDirection.Backward:
                                                                {
                                                                    DashChange(dashAdjustment.Further, dashAdjustment.Closer);
                                                                    break;
                                                                }
                                                            case dashDirection.Up:
                                                                {
                                                                    DashChange(dashAdjustment.Further, dashAdjustment.Closer);
                                                                    break;
                                                                }
                                                            case dashDirection.Down:
                                                                {
                                                                    DashChange(dashAdjustment.Further, dashAdjustment.Closer);
                                                                    break;
                                                                }
                                                        }
                                                        break;
                                                    }
                                                case false:
                                                    {
                                                        /*--Overlap All But Front & Bottom--*/
                                                        switch (checkDirection)
                                                        {
                                                            case dashDirection.Forward:
                                                                {
                                                                    DashChange(dashAdjustment.Further, dashAdjustment.Closer);
                                                                    break;
                                                                }
                                                            case dashDirection.Backward:
                                                                {
                                                                    DashChange(dashAdjustment.Further, dashAdjustment.Closer);
                                                                    break;
                                                                }
                                                            case dashDirection.Up:
                                                                {
                                                                    DashChange(dashAdjustment.Further, dashAdjustment.Closer);
                                                                    break;
                                                                }
                                                            case dashDirection.Down:
                                                                {
                                                                    DashChange(dashAdjustment.Further, dashAdjustment.Closer);
                                                                    break;
                                                                }
                                                        }
                                                        break;
                                                    }
                                            }
                                            break;
                                        }
                                    case false:
                                        {
                                            switch (pointDOverlap)
                                            {
                                                case true:
                                                    {
                                                        /*--Overlap All But Front & Top--*/
                                                        switch (checkDirection)
                                                        {
                                                            case dashDirection.Forward:
                                                                {
                                                                    DashChange(dashAdjustment.Further, dashAdjustment.Closer);
                                                                    break;
                                                                }
                                                            case dashDirection.Backward:
                                                                {
                                                                    DashChange(dashAdjustment.Further, dashAdjustment.Closer);
                                                                    break;
                                                                }
                                                            case dashDirection.Up:
                                                                {
                                                                    DashChange(dashAdjustment.Further, dashAdjustment.Closer);
                                                                    break;
                                                                }
                                                            case dashDirection.Down:
                                                                {
                                                                    DashChange(dashAdjustment.Further, dashAdjustment.Closer);
                                                                    break;
                                                                }
                                                        }
                                                        break;
                                                    }
                                                case false:
                                                    {
                                                        /*--Overlap Back Only--*/
                                                        switch (checkDirection)
                                                        {
                                                            case dashDirection.Forward:
                                                                {
                                                                    DashChange(dashAdjustment.Further, dashAdjustment.Closer);
                                                                    break;
                                                                }
                                                            case dashDirection.Backward:
                                                                {
                                                                    DashChange(dashAdjustment.Further, dashAdjustment.Closer);
                                                                    break;
                                                                }
                                                            case dashDirection.Up:
                                                                {
                                                                    DashChange(dashAdjustment.Adjacent, dashAdjustment.Closer, dashAdjustment.Further);
                                                                    break;
                                                                }
                                                            case dashDirection.Down:
                                                                {
                                                                    DashChange(dashAdjustment.Adjacent, dashAdjustment.Closer, dashAdjustment.Further);
                                                                    break;
                                                                }
                                                        }
                                                        break;
                                                    }
                                            }
                                            break;
                                        }
                                }
                                break;
                            }
                        case false:
                            {
                                switch (pointCOverlap)
                                {
                                    case true:
                                        {
                                            switch (pointDOverlap)
                                            {
                                                case true:
                                                    {
                                                        /*--Overlap All But Front & Back--*/
                                                        switch (checkDirection)
                                                        {
                                                            case dashDirection.Forward:
                                                                {
                                                                    DashChange(dashAdjustment.Further, dashAdjustment.Closer);
                                                                    break;
                                                                }
                                                            case dashDirection.Backward:
                                                                {
                                                                    DashChange(dashAdjustment.Further, dashAdjustment.Closer);
                                                                    break;
                                                                }
                                                            case dashDirection.Up:
                                                                {
                                                                    DashChange(dashAdjustment.Further, dashAdjustment.Closer);
                                                                    break;
                                                                }
                                                            case dashDirection.Down:
                                                                {
                                                                    DashChange(dashAdjustment.Further, dashAdjustment.Closer);
                                                                    break;
                                                                }
                                                        }
                                                        break;
                                                    }
                                                case false:
                                                    {
                                                        /*--Overlap Top Only--*/
                                                        switch (checkDirection)
                                                        {
                                                            case dashDirection.Forward:
                                                                {
                                                                    DashChange(dashAdjustment.Adjacent, dashAdjustment.Closer, dashAdjustment.Further);                                                                   
                                                                    break;
                                                                }
                                                            case dashDirection.Backward:
                                                                {
                                                                    DashChange(dashAdjustment.Adjacent, dashAdjustment.Closer, dashAdjustment.Further);
                                                                    break;
                                                                }
                                                            case dashDirection.Up:
                                                                {
                                                                    DashChange(dashAdjustment.Further, dashAdjustment.Closer);
                                                                    break;
                                                                }
                                                            case dashDirection.Down:
                                                                {
                                                                    DashChange(dashAdjustment.Further, dashAdjustment.Closer);
                                                                    break;
                                                                }
                                                        }
                                                        break;
                                                    }
                                            }
                                            break;
                                        }
                                    case false:
                                        {
                                            switch (pointDOverlap)
                                            {
                                                case true:
                                                    {
                                                        /*--Overlap Bottom Only--*/
                                                        switch (checkDirection)
                                                        {
                                                            case dashDirection.Forward:
                                                                {
                                                                    DashChange(dashAdjustment.Adjacent, dashAdjustment.Closer, dashAdjustment.Further);
                                                                    break;
                                                                }
                                                            case dashDirection.Backward:
                                                                {
                                                                    DashChange(dashAdjustment.Adjacent, dashAdjustment.Closer, dashAdjustment.Further);
                                                                    break;
                                                                }
                                                            case dashDirection.Up:
                                                                {
                                                                    DashChange(dashAdjustment.Further, dashAdjustment.Closer);
                                                                    break;
                                                                }
                                                            case dashDirection.Down:
                                                                {
                                                                    DashChange(dashAdjustment.Further, dashAdjustment.Closer);
                                                                    break;
                                                                }
                                                        }
                                                        break;
                                                    }
                                                case false:
                                                    {
                                                        /*--Collision without Overlap--*/
                                                        //Debug.Log("How the fuck is there a collision without an overlap?" + this.gameObject.name);
                                                        DashChange();
                                                        break;
                                                    }
                                            }
                                            break;
                                        }
                                }
                                break;
                            }
                    }
                    break;
                }
        }
    }

    /*--Start of Dash Change--*/
    /*--DashChange basically just takes in a certain number of possible dash adjustments from the switch nest above and converts them into a readable list. The only overflow is for additional possible adjustments--*/
    void DashChange()
    {
        possibleDashAdjusts = new List<dashAdjustment>();
    }
    void DashChange(dashAdjustment possible1)
    {
        possibleDashAdjusts = new List<dashAdjustment>();
        possibleDashAdjusts.Add(possible1);
        dashModiDest = new Vector3(DashAdjuster(checkDirection, collBounds, false).x - myPlayerMovement.transform.position.x, DashAdjuster(checkDirection, collBounds, false).y - myPlayerMovement.transform.position.y, 0f);
    }
    void DashChange(dashAdjustment possible1, dashAdjustment possible2)
    {
        possibleDashAdjusts = new List<dashAdjustment>();
        possibleDashAdjusts.Add(possible1);
        possibleDashAdjusts.Add(possible2);
        dashModiDest = new Vector3(DashAdjuster(checkDirection, collBounds, false).x - myPlayerMovement.transform.position.x, DashAdjuster(checkDirection, collBounds, false).y - myPlayerMovement.transform.position.y, 0f);
    }
    void DashChange(dashAdjustment possible1, dashAdjustment possible2, dashAdjustment possible3)
    {
        possibleDashAdjusts = new List<dashAdjustment>();
        possibleDashAdjusts.Add(possible1);
        possibleDashAdjusts.Add(possible2);
        possibleDashAdjusts.Add(possible3);
        dashModiDest = new Vector3(DashAdjuster(checkDirection, collBounds, false).x - myPlayerMovement.transform.position.x, DashAdjuster(checkDirection, collBounds, false).y - myPlayerMovement.transform.position.y, 0f);
    }
    
     /*--End of Dash Change--*/




    Vector3 DashAdjuster(dashDirection dir, Bounds vertexes, bool dashCheckerBypass)
    {
        //Debug.Log(dashAdjusterDirection + this.gameObject.name);
        dashAdjusterDirection = dashAdjustment.Closer;
        if(dashCheckerBypass == false)
        {
            if (possibleDashAdjusts.Contains(dashAdjustment.Adjacent))
            {
                if(Vector3.Distance(transform.position, DashAdjustFurther(checkDirection, collBounds)) < Vector3.Distance(transform.position, DashAdjustAdjacent(checkDirection, collBounds)))
                {
                    if (Vector3.Distance(transform.position, DashAdjustFurther(checkDirection, collBounds)) < Vector3.Distance(transform.position, DashAdjustCloser(checkDirection, collBounds)) && !(Vector3.Distance(transform.position, DashAdjustFurther(checkDirection, collBounds)) > maxDashDist) && possibleDashAdjusts.Contains(dashAdjustment.Further))
                    {
                        dashAdjusterDirection = dashAdjustment.Further;
                    }
                }
                if (Vector3.Distance(transform.position, DashAdjustCloser(checkDirection, collBounds)) < Vector3.Distance(transform.position, DashAdjustAdjacent(checkDirection, collBounds)))
                {
                    if (Vector3.Distance(transform.position, DashAdjustFurther(checkDirection, collBounds)) > Vector3.Distance(transform.position, DashAdjustCloser(checkDirection, collBounds)) && !(Vector3.Distance(transform.position, DashAdjustCloser(checkDirection, collBounds)) < minDashDist) && possibleDashAdjusts.Contains(dashAdjustment.Closer))
                    {
                        dashAdjusterDirection = dashAdjustment.Closer;
                    }
                }
                if (Vector3.Distance(transform.position, DashAdjustFurther(checkDirection, collBounds)) > Vector3.Distance(transform.position, DashAdjustAdjacent(checkDirection, collBounds)) && Vector3.Distance(transform.position, DashAdjustCloser(checkDirection, collBounds)) > Vector3.Distance(transform.position, DashAdjustAdjacent(checkDirection, collBounds)))
                {
                    dashAdjusterDirection = dashAdjustment.Adjacent;
                }
            }
            if(!possibleDashAdjusts.Contains(dashAdjustment.Adjacent))
            {
                if (Vector3.Distance(transform.position, DashAdjustFurther(checkDirection, collBounds)) < Vector3.Distance(transform.position, DashAdjustCloser(checkDirection, collBounds)) && !(Vector3.Distance(transform.position, DashAdjustFurther(checkDirection, collBounds)) > maxDashDist) && possibleDashAdjusts.Contains(dashAdjustment.Further))
                {
                    dashAdjusterDirection = dashAdjustment.Further;
                }
                if (Vector3.Distance(transform.position, DashAdjustFurther(checkDirection, collBounds)) > Vector3.Distance(transform.position, DashAdjustCloser(checkDirection, collBounds)) && !(Vector3.Distance(transform.position, DashAdjustCloser(checkDirection, collBounds)) < minDashDist) && possibleDashAdjusts.Contains(dashAdjustment.Closer))
                {
                    dashAdjusterDirection = dashAdjustment.Closer;
                }
            }
        }

        /*--DASH CHECKER BYPASS FUNCTION--*/
        if (dashCheckerBypass == true)
        {
            dashAdjusterDirection = dashAdjustment.Closer;
        }

        if (possibleDashAdjusts.Contains(dashAdjusterDirection))
        {
            switch (dashAdjusterDirection)
            {
                case dashAdjustment.Further:
                    {
                        dashModifier = DashAdjustFurther(checkDirection, collBounds);
                        break;
                    }
                case dashAdjustment.Adjacent:
                    {
                        dashModifier = DashAdjustAdjacent(checkDirection, collBounds);
                        break;
                    }
                case dashAdjustment.Closer:
                    {
                        dashModifier = DashAdjustCloser(checkDirection, collBounds);
                        break;
                    }
            }
        }
        return dashModifier;
    }








    //bool DashCheck(Vector3 exitPoint)
    //{
    //    //Debug.Log(exitPoint + this.gameObject.name);
    //    //Debug.DrawLine(exitPoint, new Vector2(exitPoint.x + myCollider.bounds.min.x, exitPoint.y + myCollider.bounds.min.y), Color.cyan);
    //    //Debug.DrawLine(exitPoint, new Vector2(exitPoint.x + myCollider.bounds.max.x, exitPoint.y + myCollider.bounds.max.y), Color.cyan);

    //    switch (checkDirection)
    //    {
    //        case dashDirection.Forward:
    //            {
    //                if ()
    //                {

    //                }
    //                break;
    //            }
    //        case dashDirection.Backward:
    //            {
    //                if ()
    //                {

    //                }
    //                break;
    //            }
    //        case dashDirection.Up:
    //            {
    //                if ()
    //                {

    //                }
    //                break;
    //            }
    //        case dashDirection.Down:
    //            {
    //                if ()
    //                {

    //                }
    //                break;
    //            }
    //    }
    //    Debug.Log("fell out of switch inside DashCheck");
    //    return false;
    //}









    Vector3 DashAdjustCloser(dashDirection dir, Bounds vertexes)
    {
        Vector3 dashModClose;
        /*--Bottom-Left--*/
        Vector3 boundsVertex1 = vertexes.min;
        /*--Top-Right--*/
        Vector3 boundsVertex2 = vertexes.max;
        /*--Bottom-Right--*/
        Vector3 boundsVertex3 = new Vector3(vertexes.max.x, vertexes.min.y, vertexes.max.z);
        /*--Top-Left--*/
        Vector3 boundsVertex4 = new Vector3(vertexes.min.x, vertexes.max.y, vertexes.max.z);
        Vector3 pointOnEdge = Vector3.zero;
        switch (dir)
        {
            case dashDirection.Forward:
                {
                    if (myPlayerMovement.currentFacing == 1)
                    {
                        pointOnEdge = new Vector3(vertexes.min.x, transform.position.y, 0f);
                        dashModClose = new Vector3(pointOnEdge.x - 0.5f, pointOnEdge.y, 0f);
                        //Debug.Log(dashModClose + "close" + this.gameObject.name);

                        return dashModClose;
                    }
                    if (myPlayerMovement.currentFacing == -1)
                    {
                        pointOnEdge = new Vector3(boundsVertex3.x, transform.position.y, 0f);
                        dashModClose = new Vector3(pointOnEdge.x + 0.5f, pointOnEdge.y, 0f);
                        //Debug.Log(dashModClose + "close" + this.gameObject.name);
                        return dashModClose;
                    }
                    break;
                }
            case dashDirection.Backward:
                {
                    if (myPlayerMovement.currentFacing == 1)
                    {
                        pointOnEdge = new Vector3(boundsVertex3.x, transform.position.y, 0f);
                        dashModClose = new Vector3(pointOnEdge.x + 0.5f, pointOnEdge.y, 0f);
                        //Debug.Log(dashModClose + "close" + this.gameObject.name);
                        return dashModClose/* * myPlayerMovement.currentFacing*/;
                    }
                    if (myPlayerMovement.currentFacing == -1)
                    {
                        pointOnEdge = new Vector3(vertexes.min.x, transform.position.y, 0f);
                        dashModClose = new Vector3(pointOnEdge.x - 0.5f, pointOnEdge.y, 0f);
                        //Debug.Log(dashModClose + "close" + this.gameObject.name);
                        return dashModClose/* * myPlayerMovement.currentFacing*/;
                    }
                    break;
                }
            case dashDirection.Up:
                {
                    pointOnEdge = new Vector3(transform.position.x, vertexes.min.y, 0f);
                    dashModClose = new Vector3(pointOnEdge.x, pointOnEdge.y - 1f, 0f);
                    //Debug.Log(dashModClose + "close" + this.gameObject.name);
                    //if (DashCheck(dashModClose) == true)
                    //{
                    //    return dashModClose;
                    //}
                    return dashModClose;
                    //break;
                }
            case dashDirection.Down:
                {
                    pointOnEdge = new Vector3(transform.position.x, vertexes.max.y, 0f);
                    dashModClose = new Vector3(pointOnEdge.x, pointOnEdge.y + 1f, 0f);
                    //Debug.Log(dashModClose + "close" + this.gameObject.name);
                    //if (DashCheck(dashModClose) == true)
                    //{
                    //    return dashModClose;
                    //}
                    return dashModClose;
                    //break;
                }
        }
        Debug.Log("fell out of dashadjust close switch");
        return pointOnEdge;
    }
    Vector3 DashAdjustFurther(dashDirection dir, Bounds vertexes)
    {
        Vector3 dashModFar;
        /*--Bottom-Left--*/
        Vector3 boundsVertex1 = vertexes.min;
        /*--Top-Right--*/
        Vector3 boundsVertex2 = vertexes.max;
        /*--Bottom-Right--*/
        Vector3 boundsVertex3 = new Vector3(vertexes.max.x, vertexes.min.y, vertexes.max.z);
        /*--Top-Left--*/
        Vector3 boundsVertex4 = new Vector3(vertexes.min.x, vertexes.max.y, vertexes.max.z);
        Vector3 pointOnEdge = Vector3.zero;
        switch (dir)
        {
            case dashDirection.Forward:
                {
                    if (myPlayerMovement.currentFacing == 1)
                    {
                        pointOnEdge = new Vector3(vertexes.max.x, transform.position.y, 0f);
                        dashModFar = new Vector3(pointOnEdge.x + 0.5f, pointOnEdge.y, 0f);
                        //if (DashCheck(dashModFar) == true)
                        //{
                        //    return dashModFar;
                        //}
                        return dashModFar;

                    }
                    if (myPlayerMovement.currentFacing == -1)
                    {
                        pointOnEdge = new Vector3(vertexes.min.x, transform.position.y, 0f);
                        dashModFar = new Vector3(pointOnEdge.x - 0.5f, pointOnEdge.y, 0f);
                        //if (DashCheck(dashModFar) == true)
                        //{
                        //    return dashModFar;
                        //}
                        return dashModFar;
                    }
                    break;
                }
            case dashDirection.Backward:
                {
                    if (myPlayerMovement.currentFacing == 1)
                    {
                        pointOnEdge = new Vector3(vertexes.min.x, transform.position.y, 0f);
                        dashModFar = new Vector3(pointOnEdge.x - 0.5f, pointOnEdge.y, 0f);
                        return dashModFar;
                    }
                    if (myPlayerMovement.currentFacing == -1)
                    {
                        pointOnEdge = new Vector3(vertexes.max.x, transform.position.y, 0f);
                        dashModFar = new Vector3(pointOnEdge.x + 0.5f, pointOnEdge.y, 0f);
                        //if (DashCheck(dashModFar) == true)
                        //{
                        //    return dashModFar;
                        //}
                        return dashModFar;
                    }
                    break;
                }
            case dashDirection.Up:
                {
                    pointOnEdge = new Vector3(transform.position.x, vertexes.max.y, 0f);
                    dashModFar = new Vector3(pointOnEdge.x, pointOnEdge.y + 1f, 0f);
                    //if (DashCheck(dashModFar) == true)
                    //{
                    //    return dashModFar;
                    //}
                    return dashModFar;
                    //break;
                }
            case dashDirection.Down:
                {
                    pointOnEdge = new Vector3(transform.position.x, vertexes.max.y, 0f);
                    dashModFar = new Vector3(pointOnEdge.x, pointOnEdge.y + 1f, 0f);
                    //if (DashCheck(dashModFar) == true)
                    //{
                    //    return dashModFar;
                    //}
                    return dashModFar;
                    //break;
                }
        }
        //Debug.Log("fell out of dashAdjust far switch");
        return pointOnEdge;
    }
    Vector3 DashAdjustAdjacent(dashDirection dir, Bounds vertexes)
    {
        Vector3 dashModAdj;
        /*--Bottom-Left--*/
        Vector3 boundsVertex1 = vertexes.min;
        /*--Top-Right--*/
        Vector3 boundsVertex2 = vertexes.max;
        /*--Bottom-Right--*/
        Vector3 boundsVertex3 = new Vector3(vertexes.max.x, vertexes.min.y, vertexes.max.z);
        /*--Top-Left--*/
        Vector3 boundsVertex4 = new Vector3(vertexes.min.x, vertexes.max.y, vertexes.max.z);
        Vector3 pointOnEdgeRH = Vector3.zero;
        Vector3 pointOnEdgeLH = Vector3.zero;
        switch (dir)
        {
            case dashDirection.Forward:
                {
                    pointOnEdgeLH = new Vector3(transform.position.x, boundsVertex2.y, vertexes.max.z);
                    pointOnEdgeRH = new Vector3(transform.position.x, boundsVertex1.y, vertexes.max.z);
                    if (Vector3.Distance(transform.position, pointOnEdgeLH) < Vector3.Distance(transform.position, pointOnEdgeRH))
                    {
                        dashModAdj = new Vector3(pointOnEdgeLH.x, pointOnEdgeLH.y + 1f, pointOnEdgeLH.z);    
                        return dashModAdj;
                    }
                    if (Vector3.Distance(transform.position, pointOnEdgeLH) > Vector3.Distance(transform.position, pointOnEdgeRH))
                    {
                        dashModAdj = new Vector3(pointOnEdgeRH.x, pointOnEdgeRH.y - 1f, pointOnEdgeRH.z);
                        return dashModAdj;
                    }
                    break;
                }
            case dashDirection.Backward:
                {
                    pointOnEdgeLH = new Vector3(transform.position.x, boundsVertex1.y, vertexes.max.z);
                    pointOnEdgeRH = new Vector3(transform.position.x, boundsVertex2.y, vertexes.max.z);
                    if (Vector3.Distance(transform.position, pointOnEdgeLH) < Vector3.Distance(transform.position, pointOnEdgeRH))
                    {
                        dashModAdj = new Vector3(pointOnEdgeLH.x, pointOnEdgeLH.y + 1f, pointOnEdgeLH.z);
                        return dashModAdj;
                    }
                    if (Vector3.Distance(transform.position, pointOnEdgeLH) > Vector3.Distance(transform.position, pointOnEdgeRH))
                    {
                        dashModAdj = new Vector3(pointOnEdgeRH.x, pointOnEdgeRH.y - 1f, pointOnEdgeRH.z);
                        return dashModAdj;
                    }
                    break;
                }
            case dashDirection.Up:
                {
                    pointOnEdgeLH = new Vector3(boundsVertex2.x, transform.position.y, vertexes.max.z);
                    pointOnEdgeRH = new Vector3(boundsVertex1.x, transform.position.y, vertexes.max.z);
                    if (Vector3.Distance(transform.position, pointOnEdgeLH) < Vector3.Distance(transform.position, pointOnEdgeRH))
                    {
                        dashModAdj = new Vector3(pointOnEdgeLH.x - 0.5f, pointOnEdgeLH.y, pointOnEdgeLH.z);
                        return dashModAdj;
                    }
                    if (Vector3.Distance(transform.position, pointOnEdgeLH) > Vector3.Distance(transform.position, pointOnEdgeRH))
                    {
                        dashModAdj = new Vector3(pointOnEdgeRH.x + 0.5f, pointOnEdgeRH.y, pointOnEdgeRH.z);
                        return dashModAdj;
                    }
                    break;
                }
            case dashDirection.Down:
                {
                    pointOnEdgeLH = new Vector3(transform.position.x, boundsVertex2.y, vertexes.max.z);
                    pointOnEdgeRH = new Vector3(transform.position.x, boundsVertex1.y, vertexes.max.z);
                    if (Vector3.Distance(transform.position, pointOnEdgeLH) < Vector3.Distance(transform.position, pointOnEdgeRH))
                    {
                        dashModAdj = new Vector3(pointOnEdgeRH.x + 0.5f, pointOnEdgeRH.y, pointOnEdgeRH.z);
                        return dashModAdj;
                    }
                    if (Vector3.Distance(transform.position, pointOnEdgeLH) > Vector3.Distance(transform.position, pointOnEdgeRH))
                    {
                        dashModAdj = new Vector3(pointOnEdgeLH.x - 0.5f, pointOnEdgeLH.y, pointOnEdgeLH.z);
                        return dashModAdj;
                    }
                    break;
                }
        }
        return Vector3.zero;
    }
}