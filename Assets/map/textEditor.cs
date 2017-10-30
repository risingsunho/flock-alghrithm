using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class textEditor : MonoBehaviour {
    private string info;
    internal BoidController_mil controller;
    internal flocking_mil flock;
    GameObject boidcontroller;
    // Use this for initialization
    void Start () {
        boidcontroller = GameObject.FindGameObjectWithTag("Controller");
        controller = boidcontroller.GetComponent<BoidController_mil>();
        
	}
	
	// Update is called once per frame
	void Update () {
        flock = controller.boids[0].GetComponent<flocking_mil>();
        if(flock.iden.turnnelseen) //일렬종대
        {
            info = "Formation Flight (Column)";
        }
        else if(flock.iden.obstacleseen)//횡대
        {
            info = "Formation Flight (Line)";
        }
        else if(flock.manual)
        {
            info = "Mission Flight (hovering)";
        }
        else
        {            
            info = "Flocking Flight";
        }
        if (Time.time >= 4f)
        {
            this.GetComponent<TextMesh>().text = info;
        }
        else
        {
            info = "Take Off ...";
            this.GetComponent<TextMesh>().text = info;
        }
    }
}
