using UnityEngine;
using System.Collections;

public class Agent : MonoBehaviour
{
   /* private GameObject cam;
    private GameObject Controller;
    private BoidController bc;
    private Identification a;

    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("Cam");
        Controller = GameObject.FindGameObjectWithTag("Controller");
        a = cam.GetComponent<Identification>();
        bc = Controller.GetComponent<BoidController>();
       
    }

    void Update()
    {
        if(a.obstacleseen)
        {
            bc.Sepfactor = 1.5f;
            bc.Cohfactor = 0.5f;
            bc.Arrfactor = 5f;
        }
        else if(a.turnnelseen)
        {
            bc.Sepfactor = 0.5f;
            bc.Cohfactor = 1.5f;
            bc.Arrfactor = 5f;
        }
        ////////////아래는 주석///////////
        else if(!a.seen)
        {
            bc.Sepfactor = 0.5f;
            bc.Cohfactor = 0.5f;
            bc.Arrfactor = 0.5f;
        }
    }*/
}