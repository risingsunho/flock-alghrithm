using UnityEngine;
using System.Collections;

public class MovingObstacle : MonoBehaviour
{
    public Vector3[] wp;
    public Vector3 tspeed;
    public int i = 0;
    public float speed = 2f;
    // Use this for initialization
    void Start()
    {
        wp = new Vector3[100];
        System.Random r = new System.Random();
        for (int j = 100; j >0; j--)
        {
            wp[100-j] = new Vector3(j, 2, 30);            
        }
    }

    // Update is called once per frame
    void Update()
    {
            transform.LookAt(wp[i]);
            tspeed = transform.forward.normalized;
            tspeed = tspeed * speed;
            GetComponent<Rigidbody>().velocity = tspeed;
       
        if (Vector3.Distance(transform.position, wp[i]) < 0.2f)
        {
            if(i<99)
            {
                i++;
            }
            
        }
        
    }
}
