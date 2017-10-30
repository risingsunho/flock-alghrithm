using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class TopDownAgent_mil : MonoBehaviour
{
    internal Identification iden;
    internal BoidController_mil controller;
    internal flocking_mil f;
    internal LAuto_Mil la;
    public Dictionary<int, float> boidlists = new Dictionary<int, float>();


    private GameObject Cam;
    private GameObject Controller;
    public List<GameObject> tmplist = new List<GameObject>();

    public float gap = 30f;
    private GameObject[] boids;
    private GameObject[] spreads;

    private bool kkkk = true;
    private bool kkk = true;
    public float boidspeed = 0;

    // Use this for initialization
    void Start()
    {
        Controller = GameObject.FindGameObjectWithTag("Controller");
        Cam = GameObject.FindGameObjectWithTag("Cam");
        iden = Cam.GetComponent<Identification>();
        controller = Controller.GetComponent<BoidController_mil>();
        la = GetComponent<LAuto_Mil>();
        boids = GameObject.FindGameObjectsWithTag("boid");
        spreads= GameObject.FindGameObjectsWithTag("destination");

    }
    void Update()
    {
        if (la.i == 3)
        {
            iden.turnnelseen = true;
        }
        else if(la.i==4)
        {
            iden.turnnelseen = false;
        }
        else if (la.i == 6)
        {            
            iden.obstacleseen = true;
        }
        else if (la.i == 7)
        {
            iden.obstacleseen = false;                        
        }
        else if(la.i==8)
        {
           
        }
        else if (la.i == 9)
        {
            Spread();

            foreach (GameObject boid in boids)
            {
                flocking_mil fm = boid.GetComponent<flocking_mil>();
                fm.manual = true;
                /* fm.idRange = 10;
                 fm.Sepfactor = 0;
                 fm.Cohfactor = 1f;
                 fm.Arrfactor = 0f;
                 fm.collisionRange = 8f;
                 fm.isolationRange = 10f;
                 fm.manual = false;
                 fm.speed = fm.speed * 1.5f;
                 fm.gapwithleader = 5f;*/
            }
            
                       
        }
        else if(la.i==10)
        {
            foreach (GameObject boid in boids)
            {
                flocking_mil fm = boid.GetComponent<flocking_mil>();
                fm.manual = false;
                /* fm.idRange = 100;
                 fm.Sepfactor = 1;
                 fm.Cohfactor = 0f;
                 fm.Arrfactor = 0f;
                 fm.collisionRange = 100f;
                 fm.isolationRange = 1000f;
                 fm.manual = true;
                 fm.gapwithleader = 30f;*/
            }
            /* iden.obstacleseen = false;
             foreach (GameObject boid in boids)
             {
                 flocking_mil fm = boid.GetComponent<flocking_mil>();
                 fm.idRange = 15;
                 fm.Sepfactor = 0;
                 fm.Cohfactor = 1f;
                 fm.Arrfactor = 0f;
                 fm.collisionRange = 10f;
                 fm.isolationRange = 15f;
                 fm.manual = false;
                 fm.gapwithleader = 15f;
             }*/
        }
        else if (la.i == 12)
        {
            iden.turnnelseen = true;
        }
        else if (la.i == 13)
        {
            iden.turnnelseen = false;
        }
        if (iden.turnnelseen)
        {
            rowPositioning();
        }
        else if (iden.obstacleseen)
        {
            Circle();                        
        }

    }
    void Spread()
    {
        boidlists.Clear();
        tmplist.Clear();
        if (kkk)
        {
            boidlists.Clear();
            tmplist.Clear();
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
            kkk = false;
        }
        
        int index = 0;
        foreach (GameObject boid in tmplist) //리더와 가까운 순서대로
        {
            f = boid.GetComponent<flocking_mil>();
            f.neighborhood.Clear();
            Vector3 temppos=Vector3.zero;
            temppos = spreads[index].transform.position;         
            f.ArrVector = (temppos - boid.transform.position);
            index++;
        }
    }

    void Circle()
    {
        //tmplist = new List<GameObject>();
        
        if (kkkk)
        {
            boidlists.Clear();
            tmplist.Clear();
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
            f = boid.GetComponent<flocking_mil>();
            f.neighborhood.Clear();
            Vector3 temppos;
            if (first) //리더왼쪽
            {
                temppos = controller.leader.transform.position + (controller.leader.transform.right * -1 * h * gap);
                f.ArrVector = (temppos - boid.transform.position);
                //f.CohVector = Vector3.zero;
               // f.speed = f.ArrVector.magnitude;
                //f.SepVector = Vector3.zero;
                k++;
                h++;

                first = false;
            }
            else if (!first) //리더 오른쪽 gap만큼 간격으로 정렬
            {
                temppos = controller.leader.transform.position + (controller.leader.transform.right * j * gap);
                f.ArrVector = (temppos - boid.transform.position);
                //f.CohVector = Vector3.zero;
               // f.speed = f.ArrVector.magnitude;
                //f.SepVector = Vector3.zero;
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
       // tmplist = new List<GameObject>();
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
        foreach (GameObject boid in tmplist) //리더와 가까운 순서대로
        {
            Vector3 temppos;
            f = boid.GetComponent<flocking_mil>();
            temppos = controller.leader.transform.position + (-controller.leader.transform.forward * k * gap);
            f.ArrVector = (temppos - boid.transform.position);
            //f.CohVector = Vector3.zero;
            //f.SepVector = Vector3.zero;
            //f.speed = f.ArrVector.magnitude;
            k++;

            if (k == tmplist.Count + 1)
            {
                break;
            }
        }
    }
    
}
