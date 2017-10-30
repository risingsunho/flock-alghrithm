using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeGrid : MonoBehaviour {
    public GameObject xgrid;
    public GameObject ygrid;
    public int width;
    public int height;
    public Vector3 v;
	// Use this for initialization
	
	// Update is called once per frame
	void Start () {
        for (int i = 0; i < height; i++)
        {
            v = new Vector3(500,0,10*i);
            GameObject boid = Instantiate(ygrid, v, transform.rotation) as GameObject;
            boid.transform.parent = transform;

        }
        for (int j = 0; j < width; j++)
        {
            v = new Vector3(10*j, 0, 500);
            GameObject boid = Instantiate(xgrid, v, transform.rotation) as GameObject;
            boid.transform.parent = transform;
        }

    }
}
