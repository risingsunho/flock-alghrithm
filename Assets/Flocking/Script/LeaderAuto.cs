using UnityEngine;
using System.Collections;

public class LeaderAuto : MonoBehaviour {
    public Vector3[] wp;
    internal Identification iden;
    private GameObject Cam;
    public Vector3 tspeed;
    public int i=0;
    public float speed = 1f;
	// Use this for initialization
	void Start () {
        wp = new Vector3[100];
        Cam = GameObject.FindGameObjectWithTag("Cam");
        iden = Cam.GetComponent<Identification>();
        System.Random r = new System.Random();
        for (int j = 0; j < wp.Length; j++)
        {
            wp[j] = new Vector3(40+2 * j, 2, 50);           
        }
    }
	
	// Update is called once per frame
	void Update () {
      /*  if (iden.turnnelseen) speed = 2f;
        else speed = 1f;*/
        transform.LookAt(wp[i]);
        //transform.Translate(wp[i]*Time.deltaTime*0.1f);
        tspeed = transform.forward;
        tspeed=tspeed * speed;
        //GetComponent<Rigidbody>().AddForce(tspeed);
        GetComponent<Rigidbody>().velocity = tspeed;
        if (Vector3.Distance(transform.position, wp[i]) < 0.2f)
        {           
            i++;
        }
	}
}
