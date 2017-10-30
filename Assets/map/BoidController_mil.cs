using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidController_mil : MonoBehaviour {

    public List<GameObject> boids = new List<GameObject>();
    public GameObject prefab;
    public int flockSize = 8;

    public GameObject leader;

    void Start()
    {
        //float[] xvalue = { 34.8207f, 34.1304f, 30.4286f, 29.1294f, 29.4085f, 23.8651f, 24.3990f, 24.9876f };
        //float[] zvalue = { 54f, 47.44f, 50.8292f, 56.5772f, 44.342f, 48.8732f, 43.8941f, 53.7578f };
        float x = 1;
        float y = 2;
        float z = 1;

        System.Random r = new System.Random();
        for (int i = 0; i < flockSize; i++)
        {
            Vector3 v = leader.transform.position;
            Vector3 startpos;
            if(i==0)
            {
                x = v.x - 12;
                y = v.y;
                z = v.z + 12;
            }
            else if (i == 1)
            {
                x = v.x - 20;
                y = v.y;
                z = v.z + 12;
            }
            else if (i==2)
            {
                x = v.x - 12;
                y = v.y;
                z = v.z;
            }
            else if (i == 3)
            {
                x = v.x - 20;
                y = v.y;
                z = v.z;
            }
            else if (i == 4)
            {
                x = v.x - 12;
                y = v.y;
                z = v.z - 12;
            }
            else if (i == 5)
            {
                x = v.x - 20;
                y = v.y;
                z = v.z -12;
            }
            else if (i == 6)
            {
                x = v.x - 28;
                y = v.y;
                z = v.z + 8;
            }
            else if (i == 7)
            {
                x = v.x - 28;
                y = v.y;
                z = v.z - 8;
            }
            startpos = new Vector3(x, y, z);
            GameObject boid = Instantiate(prefab, startpos, transform.rotation) as GameObject;

            boid.name = i.ToString();
            boid.tag = "boid";
            boids.Add(boid);
        }
    }
}
