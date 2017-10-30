using UnityEngine;
using System.Collections;

public class ManualControl : MonoBehaviour {

    // Parameters for manually-controlled motion.
    public float forceMultiplier = 20.0f;  // Controls forward / backward force in manual mode.
    public float upDownFactor = 0.75f;  // Up / down force relative to forward / back.
    public float leftRightFactor = 0.5f;   // Left / right force relative to forward / back.
    public float rotateFactor = 1f;
    public float currspeed;
    public int score;

	void Start()
	{

	}
	// Using FixedUpdate() because there are some physics calculations.
	void Update()
	{

		float dr  = Input.GetAxis ("DroneRotate");
		float dfb = Input.GetAxis ("DroneForwardBack");
		float dud = Input.GetAxis ("DroneUpDown");
		float drl = Input.GetAxis ("DroneRightLeft");
        if(dr<=0.2&&dr>=-0.2)
        {
            dr = 0;
        }
        if (dfb <= 0.2 && dfb >= -0.2)
        {
            dfb = 0;
        }
        if (dud <= 0.2 && dud >= -0.2)
        {
            dud = 0;
        }
        if (drl <= 0.2 && drl >= -0.2)
        {
            drl = 0;
        }
        GetComponent<Rigidbody>().AddForce(dfb * transform.forward * forceMultiplier *0.5f);
		GetComponent<Rigidbody>().AddForce(dud * transform.up      * forceMultiplier * upDownFactor * 0.5f);
		GetComponent<Rigidbody>().AddForce(drl * transform.right   * forceMultiplier * leftRightFactor * 0.5f);
		GetComponent<Rigidbody>().AddTorque(dr * transform.up      * forceMultiplier * rotateFactor );
        currspeed = GetComponent<Rigidbody>().velocity.magnitude;

    }	
}
