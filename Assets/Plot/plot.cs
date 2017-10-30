using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
public class plot : MonoBehaviour {

    // Use this for initialization
    public GameObject prefab;
    public GameObject leaderprefab;
    public int numberOfObjects = 20;


    private float time;
    private Vector3 [] position;
    private string[] strpos;
    private StreamReader reader = new StreamReader("log.txt");


    private bool flag = true;
    void ParsePosition()
    {
        string str = "";
        float t = 0;
        if (flag)
        {
            for (int j = 0; j <= 8;)
            {
                str= reader.ReadLine();
                time = Convert.ToSingle(str);
                if (str != null)
                {
                    str = reader.ReadLine();                  
                    str = str.Replace("(", "");
                    str = str.Replace(")", "");
                    str = str.Replace(" ", "");
                    strpos = Regex.Split(str, ",");
                    for (int i = 0; i < strpos.Length; i = i + 3)
                    {
                        t = Convert.ToSingle(strpos[i]);// float.Parse(strpos[i],System.Globalization.CultureInfo.InvariantCulture);
                        position[j].x = t;
                        t = Convert.ToSingle(strpos[i + 1]);// float.Parse(strpos[i],System.Globalization.CultureInfo.InvariantCulture);
                        position[j].y = t;
                        t = Convert.ToSingle(strpos[i + 2]);// float.Parse(strpos[i],System.Globalization.CultureInfo.InvariantCulture);
                        position[j].z = t;
                        j++;
                    }
                }
                else
                {
                    print("end");
                    flag = false;
                    break;
                }
            }
        }
    }
    /*void Update()
    {
        
    }*/
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
      //  {
            position = new Vector3[10];
            if (flag)
            {
                ParsePosition();
                // if (time > 30 && time <= 30.5)
                // {
                for (int i = 0; i < position.Length; i++)
                {
                    if (position[i].x != 0f)
                    {
                        if (i == 0)
                        {
                            leaderprefab.tag = "plot";
                            Instantiate(leaderprefab, position[i], Quaternion.identity);
                        }
                        else
                        {
                            prefab.tag = "plot";
                            Instantiate(prefab, position[i], Quaternion.LookRotation(position[i + 1]));
                        }
                    }

                }           
           // }
        }
        
    }

}

