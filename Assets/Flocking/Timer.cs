using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour {
    private GameObject leader;
    private GameObject [] followers;
    private string info;
	// Use this for initialization
	void Start () {
        followers = new GameObject[10];
        leader = GameObject.FindGameObjectWithTag("leader");
        followers = GameObject.FindGameObjectsWithTag("boid");
    }
	
	// Update is called once per frame
	void Update () {
        info= "현재시간 : " + Time.time.ToString() + "\n\n" + "leader의 위치 : " + leader.transform.position.ToString();
        foreach(GameObject follower in followers)
        {
            info += "\n\n" + "follower" + follower.name + "의 상태 : " + follower.GetComponent<flocking_reynold>().state + "\nNeighborhood = ";
            foreach (GameObject go in follower.GetComponent<flocking_reynold>().neighborhood)
            {
                info += go.name + ",";
            }
            info += "\n" + "나의 위치 = " + follower.transform.position.ToString()+ "\n"+"나의 방향 = "+ follower.GetComponent<Rigidbody>().velocity;
            info += "\n" + "Ka = "+follower.GetComponent<flocking_reynold>().Arrfactor 
                + ", Kc = "+ follower.GetComponent<flocking_reynold>().Cohfactor 
                + ", Ks = "+ follower.GetComponent<flocking_reynold>().Sepfactor;
            info += "\n" + "다음 위치 = " + follower.GetComponent<flocking_reynold>().direction;



        }
        this.GetComponent<TextMesh>().text = info;
    }
}
