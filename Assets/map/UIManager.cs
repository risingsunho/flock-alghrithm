using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    internal Identification iden;
    internal BoidController_mil controller;
    internal flocking_mil f;
    internal LAuto_Mil la;
    public Dropdown dd;
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
        spreads = GameObject.FindGameObjectsWithTag("destination");

    }
    void Update()
    {
        if (dd.value==0) //종대
        {
            iden.obstacleseen = true;
            iden.turnnelseen = false;
            rowPositioning();
        }
        else if (dd.value == 1) //횡대
        {
            iden.obstacleseen = false;
            iden.turnnelseen = true;
            Lining();
        }
        else if (dd.value == 2) //임무수행
        {
            iden.obstacleseen = false;
            iden.turnnelseen = false;
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
        else if (la.i == 3) //원래대로
        {
            iden.obstacleseen = false;
            iden.turnnelseen = false;
            foreach (GameObject boid in boids)
            {
                flocking_mil fm = boid.GetComponent<flocking_mil>();
                fm.manual = false;              
            }
           
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
            Vector3 temppos = Vector3.zero;
            temppos = spreads[index].transform.position;
            f.ArrVector = (temppos - boid.transform.position);
            index++;
        }
    }

    void Lining()
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
