using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

    public HitBoxSpecifics[] myHitBoxes;
    private PlayerMovement player;
    
    /*--PLAYER VARIABLES--*/    
    public float shortJumpForce;
    public float fullJumpForce;
    public float maxRiseSpeed;
    public float gravityForce;

    public float fallSpeed;
    public float fastFallSpeed;
    public float airStrafeForce;
    public float maxAirStrafeSpeed;
    public float runForce;
    public float maxRunSpeed;

    public int maxDashNumber;
    public float dashRegenRate;
    public Vector2 dashDestUp;
    public Vector2 dashDestDown;
    public Vector2 dashDestForward;
    public Vector2 dashDestBackward;



    public float airDrag;
    public float groundDrag;


    // Use this for initialization
    void Awake () {
        player = GetComponentInParent<PlayerMovement>();
        player.shortJumpForce = shortJumpForce;
        player.fullJumpForce = fullJumpForce;
        player.maxRiseSpeed = maxRiseSpeed;
        player.fallSpeed = fallSpeed;
        player.fastFallSpeed = fastFallSpeed;
        player.airStrafeForce = airStrafeForce;
        player.maxAirStrafeSpeed = maxAirStrafeSpeed;
        player.runForce = runForce;
        player.maxRunSpeed = maxRunSpeed;
        player.maxDashNumber = maxDashNumber;
        player.dashRegenRate = dashRegenRate;
        player.maxDashNumber = maxDashNumber;
        player.airDrag = airDrag;
        player.groundDrag = groundDrag;
        player.myGravityForce = gravityForce;        
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
