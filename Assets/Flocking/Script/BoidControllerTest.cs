using UnityEngine;
using System.Collections.Generic;
public class BoidControllerTest : MonoBehaviour
{
    public List<GameObject> boids = new List<GameObject>();
    public GameObject prefab;
    public int flockSize =8;

    public GameObject leader;

    void Start()
    {
        float[] xvalue = { 34.8207f, 34.1304f, 30.4286f, 29.1294f, 29.4085f, 23.8651f, 24.3990f, 24.9876f};// -4.09f };
        float[] zvalue = { 54f, 47.44f, 50.8292f, 56.5772f, 44.342f, 48.8732f, 43.8941f, 53.7578f };//, -9f };
        float x=1;
        float y = 2;
        float z = 1;

        System.Random r = new System.Random();
        for (int i = 0; i < flockSize; i++)
        {
             /*x = r.Next(15, 35);
             y = 2;
             z = r.Next(40, 60);*/
            Vector3 v = new Vector3(xvalue[i],y,zvalue[i]);  
            GameObject boid = Instantiate(prefab, v, transform.rotation) as GameObject;
          
            boid.name = i.ToString();
            boid.tag = "boid";
           // boid.controller = this;
            boids.Add(boid);
           /* x -= 1.2f;
            z -= 1;*/
        }
       // boids.Add(target);
    }
}