using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Identification : MonoBehaviour
{
    // public GameObject obstacle;
    // public GameObject turnnel;

    private GameObject[] boids;

    public bool obstacleseen = false;
    public bool turnnelseen = false;
    public bool boidforward = false;
    public bool command = false;

    public float Coh = 0.333f;
    public float Sep = 0.333f;
    public float Arr = 0.333f;

    public List<string> names;
    Camera camera;

    void Start()
    {        
        camera = GetComponent<Camera>();
        boids = GameObject.FindGameObjectsWithTag("boid");
        names = new List<string>();
    }

    void Update()
    {
        names.Clear();
        if(Input.GetKeyDown(KeyCode.A))
        {
            turnnelseen = true;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            turnnelseen = false;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            obstacleseen = true;
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            obstacleseen = false;            
        }
        foreach(GameObject boid in boids)
        {
            Vector3 boidviewpos = camera.WorldToViewportPoint(boid.transform.position);
            if (boidviewpos.z > 0F && boidviewpos.z < 50F)
            {
                boidforward = true;
                names.Add(boid.name);
            }
            else 
            {
                boidforward = false;
            }            
        }

    }
}