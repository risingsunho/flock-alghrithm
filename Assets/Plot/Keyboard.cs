using UnityEngine;
using System.Collections;

public class Keyboard : MonoBehaviour {
    // Update is called once per frame
    Vector3 curr=new Vector3();
    Vector3 next = new Vector3();
    float speed = 2;
    void Update () {
	    if(Input.GetKey(KeyCode.W))
        {
            curr = transform.position;
            next = curr;
            next.y = curr.y + 0.1f;
            GetComponent<Rigidbody>().AddForce(Vector3.up*speed);
            
        }
        else if(Input.GetKey(KeyCode.A))
        {
            curr = transform.position;
            next = curr;
            next.x = curr.x - 0.1f;
            //transform.position = next;
            GetComponent<Rigidbody>().AddForce(Vector3.left * speed);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            curr = transform.position;
            next = curr;
            next.x = curr.x + 0.1f;
            //transform.position = next;
            GetComponent<Rigidbody>().AddForce(Vector3.right * speed);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            curr = transform.position;
            next = curr;
            next.y = curr.y - 0.1f;
            //transform.position = next;
            GetComponent<Rigidbody>().AddForce(Vector3.down * speed);
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            GetComponent<Rigidbody>().Sleep();
        }
    }
}
