using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Name : MonoBehaviour {
    private string info;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        info = this.transform.parent.name;
        this.GetComponent<TextMesh>().text = info;
    }
}
