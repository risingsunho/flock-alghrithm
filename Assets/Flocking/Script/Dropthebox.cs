using UnityEngine;
using System.Collections;

public class Dropthebox : MonoBehaviour {
    public int drop=0;
    private Vector3 stop;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            drop++;
        }
        if(drop==1)
        {
           // this.gameObject.AddComponent<BoxCollider>();
            this.gameObject.AddComponent<Rigidbody>();
            
           //this.GetComponent<Rigidbody>().useGravity = false;
            this.GetComponent<Rigidbody>().mass = 1000f;
            this.GetComponent<BoxCollider>().size = new Vector3(1, 1, 1);
            //this.GetComponent<Rigidbody>().AddForce(Vector3.down);
            transform.parent = null;
            drop++;           
        }
        

    }
  
}
