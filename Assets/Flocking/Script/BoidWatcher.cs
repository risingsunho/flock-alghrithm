using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoidWatcher : MonoBehaviour
{
    private Vector3 offset;
    public GameObject tget;
    public GameObject landposition;
    private int state = 2;
    private bool finished = false;
    void Start()
    {
        offset = (transform.position - tget.transform.position)*1.5f;
        offset.y = transform.position.y + 0.1f;
    }
	void Update()
	{
        //transform.LookAt(tget.transform.position);
        if(Input.GetKey(KeyCode.F5))
        {
            finished = true;
            state = 1;
        }
        else if (Input.GetKey(KeyCode.F6))
        {
            state = 2;
        }       
        if (state==1)
        {
            FollowingState();
        }
        else if(state==2)
        {
            LandingState();
        }
       
        
    }
    void FollowingState()
    {
        if (finished)
        {
            transform.LookAt(tget.transform.position);
            finished = false;
        }
        Vector3 desiredPosition = tget.transform.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, 1f);
    }
    void LandingState()
    {
        transform.position = landposition.transform.position;
        transform.LookAt(tget.transform.position);
         
    }
    
}