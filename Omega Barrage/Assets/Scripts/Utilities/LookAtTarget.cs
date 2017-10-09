using UnityEngine;
using System.Collections;

public class LookAtTarget : MonoBehaviour {

    public GameObject tempTarget;
    public Vector3 target;
    public float rotato;
    public float iAmError = 0f;


    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.rotation = RotateToTarget.SmoothRotation(tempTarget.transform.position, transform, rotato, iAmError);
	}
}
