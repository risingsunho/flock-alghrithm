using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class TopDownAgent : MonoBehaviour {
    internal Identification iden;
    internal BoidControllerTest controller;
    internal flocking_reynold f;
        
    public Dictionary<int, float> boidlists = new Dictionary<int, float>();


    private GameObject Cam;
    private GameObject Controller;
    public List<GameObject> tmplist = new List<GameObject>();


    private float collisionRange = 1f;
    public float isolationRange = 5f;

    private bool kkkk = true;
  

    // Use this for initialization
    void Start () {
        Controller = GameObject.FindGameObjectWithTag("Controller");
        Cam = GameObject.FindGameObjectWithTag("Cam");
        iden = Cam.GetComponent<Identification>();
        controller = Controller.GetComponent<BoidControllerTest>();    
   
    }  
    void Update()
    {
        if (iden.turnnelseen) rowPositioning();
        else if (iden.obstacleseen) Circle();
                
    }
    void Circle()
    {
        //tmplist = new List<GameObject>();
       // boidlists.Clear();
        //tmplist.Clear();
        if (kkkk)
        {
            for (int i = 0; i < controller.boids.Count; i++)
            {
                boidlists.Add(i, Vector3.Distance(controller.leader.transform.position, controller.boids[i].transform.position)); //리더와 각 보이드들간의 거리
            }

            var rank = boidlists.OrderBy(num => num.Value);

            foreach (var r in rank)
            {
                if (this.name != controller.boids[r.Key].name)
                {
                    tmplist.Add(controller.boids[r.Key]); //리더와의 거리 순으로 boid들을 정렬                               
                }
            }
            kkkk = false;
        }        
        bool first = true;
        int h = 1;
        int k = 0;
        int j = 1;
        foreach (GameObject boid in tmplist) //리더와 가까운 순서대로
        {
            f = boid.GetComponent<flocking_reynold>();
            f.neighborhood.Clear();
            Vector3 temppos;
            if (first) //리더왼쪽
            {
                temppos = controller.leader.transform.position + (controller.leader.transform.right * -1 * h * f.collisionRange);
                f.ArrVector = (temppos - boid.transform.position);
                f.CohVector = Vector3.zero;
                f.SepVector = Vector3.zero;               
                k++;
                h++;

                first = false;          
            }
            else if(!first) //리더 오른쪽
            {
                temppos = controller.leader.transform.position + (controller.leader.transform.right * j * f.collisionRange);
                f.ArrVector = (temppos - boid.transform.position);
                f.CohVector = Vector3.zero;
                f.SepVector = Vector3.zero;
                k++;
                j++;
                first = true;
            }
            if (k == tmplist.Count)
            {
                break;
            }
        } 
    }
    void rowPositioning()
    {
        tmplist = new List<GameObject>();
        boidlists.Clear();
        tmplist.Clear();
        for (int i = 0; i < controller.boids.Count; i++)
        {
            boidlists.Add(i, Vector3.Distance(controller.leader.transform.position, controller.boids[i].transform.position)); //리더와 각 보이드들간의 거리
        }

        var rank = boidlists.OrderBy(num => num.Value);
        int k = 0;
        foreach (var r in rank)
        {
            if (this.name != controller.boids[r.Key].name)
            {
                tmplist.Add(controller.boids[r.Key]); //리더와의 거리 순으로 boid들을 정렬                               
            }            
        }
        k = 1;
        bool first = true;
        foreach (GameObject boid in tmplist) //리더와 가까운 순서대로
        {
            Vector3 temppos;
            f = boid.GetComponent<flocking_reynold>();
            temppos = controller.leader.transform.position + (-controller.leader.transform.forward * k * 3f);
            f.ArrVector = (temppos - boid.transform.position);
            f.CohVector = Vector3.zero;
            f.SepVector = Vector3.zero;
            k++;
                
            /* if (first) // 리더랑 가장 가까이 있는 보이드는 리더를 따라가게
             {
                 //f.neighborhood.Add(controller.leader);
                 f.ArrVector = ((controller.leader.transform.position-2*controller.transform.forward)-boid.transform.position).normalized;
                 f.CohVector = Vector3.zero;
                 f.SepVector = Vector3.zero;
                 first = false;
             }
             else if (!first) // 두번째는 첫번째
             {
                 //f.neighborhood.Add(tmplist[k]);
                 f.ArrVector = ((tmplist[k].transform.position-2*tmplist[k].transform.forward) - boid.transform.position).normalized;
                 f.SepVector = Vector3.zero;
                 f.CohVector = Vector3.zero;
                 k++;
             }*/
            if (k == tmplist.Count+1)
            {
                break;
            }
        }
    }

}
