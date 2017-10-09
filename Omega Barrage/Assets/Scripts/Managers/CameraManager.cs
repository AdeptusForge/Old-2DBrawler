using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

    public enum CameraMoveType
    {
        SetVelocityToward,
        LerpFollowToward,
        AddForceToward,
        TranslateDirectly,
        TransformEdit,
        TransformMove,
    }
    public enum HoriFocusShift
    {
        LeftFocus,
        RightFocus,
        NoFocus
    }
    public enum VertFocusShift
    {
        UpFocus,
        DownFocus,
        NoFocus
    }

    public GameObject upperThreshold;
    public GameObject lowerThreshold;
    public GameObject leftThreshold;
    public GameObject rightThreshold;
    public CameraMoveType moveType;
    public HoriFocusShift horiFocus;
    public VertFocusShift vertFocus;
    //public Transform target;
    public float trackSpeed;
    public float focusSwapModifier;
    public Rigidbody2D myBody;
    private GameObject myPlayer;
    private Vector3 horiOffset;
    private Vector3 vertOffset;
    public int cameraHoriMode;
    public int cameraVertMode;
    /*--1 = gameplay mode, 2 = cinematic mode, 3 = debug mode(REMOVE BEFORE FINAL RELEASE)--*/
    public Vector3 horiFocusPoint;
    public Vector3 vertFocusPoint;
    public Vector3 destination;
    public float minDistance;
    Camera cam;
    float originalSize;
    public float zoomFactor = 1f;
    public float zoomSpeed = 5f;
    float timePassed = 0f;


    // Use this for initialization
    void Start () {
        cam = GetComponent<Camera>();
        myBody = GetComponent<Rigidbody2D>();
        myPlayer = GameObject.FindGameObjectWithTag("Player");
        //Debug.Log(cam.pixelRect);
        originalSize = cam.orthographicSize;
        vertFocus = VertFocusShift.NoFocus;
        horiFocus = HoriFocusShift.NoFocus;
    }
	
	// Update is called once per frame
	void FixedUpdate () {

        switch (cameraHoriMode)
        {
            case 1:
                {
                    DualForwardThresholdFocus();
                    break;
                }
            case 2:
                {
                    HoriPositionLock();
                    break;
                }
            case 3:
                {
                    break;
                }
        }
        switch (cameraVertMode)
        {
            case 1:
                {
                    //DualForwardThresholdFocus();
                    break;
                }
            case 2:
                {
                    VertPositionLock();
                    break;
                }
            case 3:
                {
                    break;
                }
        }

        float targetSize = originalSize * zoomFactor;
        if (targetSize != cam.orthographicSize)
        {
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, Time.deltaTime * zoomSpeed);
        }
    }

    void MoveCameraToDestination()
    {
        if (Vector3.Distance(cam.transform.position, destination) <= minDistance)
        {
            return;
        }
        MoveCamera(CameraMoveType.TransformMove, destination, trackSpeed * focusSwapModifier);
    }

    private Vector3 GenerateDestination()
    {
        Vector3 moveVector = Vector3.zero;
        switch (cameraHoriMode)
        {
            case 1:
                {
                    //DualForwardThresholdFocus();
                    break;
                }
            case 2:
                {
                    //HoriPositionLock();
                    break;
                }
            case 3:
                {
                    break;
                }
        }
        switch (cameraVertMode)
        {
            case 1:
                {
                    //DualForwardThresholdFocus();
                    break;
                }
            case 2:
                {
                    //VertPositionLock();
                    break;
                }
            case 3:
                {
                    break;
                }
        }
        return moveVector;
    }

    public void LockCameraAxis(int lockValue)
    {
        switch (lockValue)
        {
            case 0:
                Debug.Log("Camera Unfrozen");
                myBody.constraints = RigidbodyConstraints2D.None;
                break;
            case 1:
                Debug.Log("Camera FreezePositionX");
                myBody.constraints = myBody.constraints | RigidbodyConstraints2D.FreezePositionX;
                break;
            case 2:
                Debug.Log("Camera FreezePositionY");
                myBody.constraints =  myBody.constraints | RigidbodyConstraints2D.FreezePositionY;
                break;
            case 3:
                Debug.Log("Camera FreezeRotation");
                myBody.constraints = myBody.constraints | RigidbodyConstraints2D.FreezeRotation;
                break;
            case 4:
                Debug.Log("Camera FreezePosition");
                myBody.constraints = myBody.constraints | RigidbodyConstraints2D.FreezePosition;
                break;
            case 5:
                Debug.Log("Camera FreezeAll");
                myBody.constraints = RigidbodyConstraints2D.FreezeAll;
                break;
            default:
                Debug.Log("LockCameraAxis incorrect input");
                break;

        }
    }

    public IEnumerator MoveCamera(CameraMoveType moveTypeEnum, Vector3 direction, float speed)
    {
        {
            switch (moveTypeEnum)
            {
                case CameraMoveType.SetVelocityToward:
                    {
                        myBody.velocity = new Vector2(direction.x - transform.position.x, direction.y - transform.position.y) * speed;
                        break;
                    }
                case CameraMoveType.LerpFollowToward:
                    {
                        float newX = Mathf.Lerp(transform.position.x, direction.x, Time.deltaTime * speed);
                        float newY = Mathf.Lerp(transform.position.y, direction.y, Time.deltaTime * speed);
                        transform.position = new Vector3(newX, newY, transform.position.z);
                        break;
                    }
                case CameraMoveType.AddForceToward:
                    /*--Not working properly--*/

                    {
                        myBody.AddForce(new Vector2(direction.x - transform.position.x, direction.y - transform.position.y) * speed);
                        break;
                    }
                case CameraMoveType.TranslateDirectly:
                    {
                        transform.Translate(new Vector2(direction.x - transform.position.x, direction.y - transform.position.y) * speed);
                        break;
                    }
                case CameraMoveType.TransformEdit:
                    {
                        transform.position = new Vector3(direction.x, direction.y, transform.position.z);
                        break;
                    }
                case CameraMoveType.TransformMove:
                    {
                        float step = speed * Time.deltaTime;
                        transform.position = Vector3.MoveTowards(transform.position, direction, step);
                        break;
                    }             
                default:
                    Debug.Log("Unusable CameraMoveType");
                    break;
            }
        }
        return null;
    }

    public void ZoomCamera(float zoomFactor)
    {
        this.zoomFactor = zoomFactor;
    }

    public Vector3 GenerateOffset()
    {
        Vector3 totalOffset = Vector3.zero;
        Vector3 vertOffset = Vector3.zero;
        Vector3 horiOffset = Vector3.zero;

        switch (cameraHoriMode)
        {
            case 1:
                {
                    /*--Horizontal Threshold Focus--*/
                    switch (horiFocus)
                    {
                        case HoriFocusShift.NoFocus:
                            {
                                break;
                            }
                        case HoriFocusShift.RightFocus:
                            {
                                //totalOffset += horiFocusPoint;
                                horiOffset = horiFocusPoint;
                                break;
                            }
                        case HoriFocusShift.LeftFocus:
                            {
                                //totalOffset += -horiFocusPoint;
                                horiOffset = -horiFocusPoint;
                                break;
                            }
                    }
                    break;
                }
            case 2:
                {
                    /*--Horizontal Position Lock--*/
                    break;
                }
            case 3:
                {
                    /*--Horizontal Camera Window--*/
                    break;
                }
        }
        switch (cameraVertMode)
        {
            case 1:
                {
                    /*--Vertical Threshold Focus--*/
                    switch (vertFocus)
                    {
                        case VertFocusShift.NoFocus:
                            {
                                break;
                            }
                        case VertFocusShift.UpFocus:
                            {
                                //totalOffset += vertFocusPoint;
                                vertOffset = vertFocusPoint;
                                break;
                            }
                        case VertFocusShift.DownFocus:
                            {
                                //totalOffset -= vertFocusPoint;
                                vertOffset = -vertFocusPoint;
                                break;
                            }
                    }
                    break;
                }
            case 2:
                {
                    /*--Vertical Position Lock--*/
                    break;
                }
            case 3:
                {
                    /*--Horizontal Camera Window--*/
                    break;
                }
        }

        totalOffset = horiOffset + vertOffset;
        return totalOffset;
    }

    void DualForwardThresholdFocus()
    {
        if (myPlayer.GetComponent<CreatureMovement>().isHitStunned == false)
        {
            if (rightThreshold.GetComponent<CameraThreshold>().playerTriggeringThreshold)
            {
                horiFocus = HoriFocusShift.RightFocus;
                horiOffset = horiFocusPoint;
            }
            if (leftThreshold.GetComponent<CameraThreshold>().playerTriggeringThreshold)
            {
                horiFocus = HoriFocusShift.LeftFocus;
                horiOffset = -horiFocusPoint;
            }
            destination = new Vector3(myPlayer.transform.localPosition.x, myPlayer.transform.position.y, cam.transform.position.z) + GenerateOffset();
            if (horiFocus == HoriFocusShift.RightFocus)
            {
                if (myPlayer.GetComponent<Rigidbody2D>().velocity.x > 0f)
                {
                    //Debug.Log("Moving toward focus");
                    if (Vector3.Distance(cam.transform.position, destination) > minDistance)
                    {
                        //Debug.Log("Out of Focus");
                        //InvokeRepeating("MoveCameraToDestination", 0f, 0.1666667f);
                        if (!(cam.transform.position.x > destination.x))
                        {
                            MoveCameraToDestination();
                        }
                    }
                    if (Vector3.Distance(cam.transform.position, destination) <= minDistance)
                    {
                        //Debug.Log("In Focus");
                    }
                    if (!(cam.transform.position.x > destination.x))
                    {
                        MoveCamera(CameraMoveType.TransformMove, destination, trackSpeed);
                    }
                }
                if (myPlayer.GetComponent<Rigidbody2D>().velocity.x < 0f)
                {
                    //Debug.Log("Moving away from focus");
                    if (Vector3.Distance(cam.transform.position, destination) > minDistance)
                    {
                        //Debug.Log("Out of Focus");
                    }
                    if (Vector3.Distance(cam.transform.position, destination) <= minDistance)
                    {
                        //Debug.Log("In Focus");
                    }
                }
            }
            if (horiFocus == HoriFocusShift.LeftFocus)
            {
                if (myPlayer.GetComponent<Rigidbody2D>().velocity.x < 0f)
                {
                    //Debug.Log("Moving toward focus");
                    if (Vector3.Distance(cam.transform.position, destination) > minDistance)
                    {
                        //Debug.Log("Out of Focus");
                        if (!(cam.transform.position.x < destination.x))
                        {
                            MoveCameraToDestination();
                        }
                    }
                    if (Vector3.Distance(cam.transform.position, destination) <= minDistance)
                    {
                        //Debug.Log("In Focus");
                    }
                    if (!(cam.transform.position.x < destination.x))
                    {
                        MoveCamera(CameraMoveType.TransformMove, destination, trackSpeed);
                    }
                }
                if (myPlayer.GetComponent<Rigidbody2D>().velocity.x > 0f)
                {
                    //Debug.Log("Moving away from focus");
                    if (Vector3.Distance(cam.transform.position, destination) > minDistance)
                    {
                        //Debug.Log("Out of Focus");
                    }
                    if (Vector3.Distance(cam.transform.position, destination) <= minDistance)
                    {
                        //Debug.Log("In Focus");
                    }
                }
            }
        }
        if (myPlayer.GetComponent<CreatureMovement>().isHitStunned == true)
        {
            /*--Knockback Camera Mode 1--*/
        }
    }

    void HoriPositionLock()
    {
        destination = new Vector3(myPlayer.transform.localPosition.x, cam.transform.position.y, cam.transform.position.z);
        MoveCamera(CameraMoveType.TransformMove, destination, trackSpeed);
    }
    void VertPositionLock()
    {
        destination = new Vector3(cam.transform.position.x, myPlayer.transform.localPosition.y, destination.z);
        MoveCamera(CameraMoveType.TransformMove, destination, trackSpeed);
    }

}
