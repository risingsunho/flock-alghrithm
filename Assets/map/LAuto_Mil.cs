using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class LAuto_Mil : MonoBehaviour {
    public GameObject[] wps;
    public Vector3[] wp;
    internal Identification iden;
    private GameObject Cam;
    public Vector3 tspeed;
    public int i = 0;
    public float speed = 10f;
    public int wpsize = 8;
    public float time = 0;
    
    // Use this for initialization
    void Start()
    {
      /*  wp = new Vector3[wpsize];
        wps = new GameObject[wpsize];
        Cam = GameObject.FindGameObjectWithTag("Cam");
        iden = Cam.GetComponent<Identification>();       
                
        for (int j=0;j<wps.Length;j++)
        {
            string str = "wp" + j.ToString();
            wps[j] = GameObject.FindGameObjectWithTag(str);            
        }
       
        for (int j = 0; j < wp.Length; j++)
        {
            wp[j] = wps[j].transform.position;           
        }*/
        
    }

    // Update is called once per frame
    void Update()
    {
       /* Vector3 lookpoint = wp[i];
        lookpoint.y = transform.position.y;
        transform.LookAt(lookpoint);
        

        tspeed = wp[i] - transform.position;
        //GetComponent<Rigidbody>().velocity = tspeed.normalized * speed;
        if (i == 9)
        {
            if(Time.time-time>20)
            {
                GetComponent<Rigidbody>().AddForce(tspeed.normalized * speed);
            }
        }
        else
        {
            GetComponent<Rigidbody>().AddForce(tspeed.normalized * speed);
            time = Time.time;
        }
        if (Vector3.Distance(transform.position, wp[i]) < 2f && i!=wp.Length+1)
        {
            i++;
        }    */    
        
    }
}
