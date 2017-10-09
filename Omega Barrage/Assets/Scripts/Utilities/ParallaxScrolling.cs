using UnityEngine;
using System.Collections;

public class ParallaxScrolling : MonoBehaviour {

    public Transform[] backgrounds;
    private float[] parallaxScales;
    public float smoothing = 1f;

    private Transform camTransform;
    private Vector3 previousCamPos;


    void Awake ()
    {
        camTransform = Camera.main.transform;
    }

	// Use this for initialization
	void Start () {
        previousCamPos = camTransform.position;
        parallaxScales = new float[backgrounds.Length];

        for (int i = 0; i < backgrounds.Length; i++)
        {
            parallaxScales[i] = backgrounds[i].position.z * -1;
        }
	}
	
	// Update is called once per frame
	void Update () {

        for (int i = 0; i < backgrounds.Length; i ++)
        {
            float parallax = (previousCamPos.x - camTransform.position.x) * parallaxScales[i];
            float backgroundTargetPosX = backgrounds[i].position.x + parallax;
            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.y);

            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.fixedDeltaTime);
        }

        previousCamPos = camTransform.position;
	}
}
