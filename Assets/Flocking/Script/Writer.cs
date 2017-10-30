using UnityEngine;
using System.IO;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
public class Writer : MonoBehaviour {
    private StreamWriter writer;
    public GameObject[] tmp = new GameObject[10];
    public GameObject lder;
    private bool startflag = true;
    private bool endflag=false;
    // Use this for initialization
	string setData()
    {       
        string line="";
        line += lder.transform.position.ToString() + ",";
        for(int i=0;i<=7;i++)
        {
            line += tmp[i].transform.position.ToString()+",";
            if(i==6)
            {
                i++;
                line += tmp[i].transform.position.ToString();
                break;
            }
        }
        return line;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            endflag = true;
            print("end");
            writer.Close();
        }
        if (!endflag)
        {
            writer.WriteLine(Time.time.ToString());
            writer.WriteLine(setData());
        }
    }
    void Start()
    {
        if (startflag)
        {
            writer = new StreamWriter("log.txt");
            tmp = GameObject.FindGameObjectsWithTag("boid");
            startflag = false;
        }      
    }
}
