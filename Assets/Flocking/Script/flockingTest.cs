using UnityEngine;
using System.Collections;

public class flockingTest : MonoBehaviour {
    public GameObject[] boids;
    private int size=3;
    private Rigidbody[] rb;
    private Vector3[] velos;
    private Vector3 directionVector;
    private string names;
	// Use this for initialization
	void Start () {
        boids = new GameObject[size];
        rb = new Rigidbody[size];
        velos = new Vector3[size];     
        for(int i=0;i< size;i++)
        {
            names = i.ToString();
            boids[i] = GameObject.FindGameObjectWithTag(names);
        }   	   
	}
	
	// Update is called once per frame
	void Update () {
        directionVector=calDirection();
        GetComponent<Rigidbody>().velocity = directionVector;
    }
    Vector3 calDirection()
    {
        for (int i = 0; i < size; i++)
        {
            rb[i] = boids[i].GetComponent<Rigidbody>();
            velos[i] = rb[i].velocity;
        }
        Vector3 dirvec = new Vector3(0, 0, 0);
        for(int i=0;i< size;i++)
        {
            dirvec.x += velos[i].x;
            dirvec.y += velos[i].y;
            dirvec.z += velos[i].z;            
        }
        dirvec.x = dirvec.x / size;
        dirvec.y = dirvec.y / size;
        dirvec.z = dirvec.z / size;
        return dirvec;
    }
}
