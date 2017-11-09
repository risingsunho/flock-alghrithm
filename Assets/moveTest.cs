using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveTest : MonoBehaviour {
    public Vector3 position;
    public Vector3 rotation;
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 startposition = transform.position;
        Vector3 destination = new Vector3(position.x, position.y, position.z);
        Quaternion startrotation = transform.rotation;
        Quaternion desiredrotation = Quaternion.Euler(position.x, position.y, position.z);

        transform.position = Vector3.Slerp(startposition, destination, Time.fixedDeltaTime);
        transform.rotation = Quaternion.Slerp(startrotation, desiredrotation, Time.fixedDeltaTime);

        destination += Vector3.up;
        
    }
}
