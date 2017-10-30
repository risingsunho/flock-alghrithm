using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class BoidFlockingTest : MonoBehaviour
{

    internal BoidControllerTest controller;
    internal Identification iden;
    public List<GameObject> neighborhood = new List<GameObject>();    

    public Dictionary<int, float> tmp = new Dictionary<int, float>();


    public Vector3 direction;
    private GameObject Controller;
    private GameObject Cam;

    public Vector3 CohVector;
    public Vector3 ArrVector;
    public Vector3 SepVector;

    public Vector3 center = new Vector3();

    public float Cohfactor = 0.333f;
    public float Sepfactor = 0.333f;
    public float Arrfactor = 0.333f;
    public float speed = 1;

   // public float IdentificationRange = 6f;
    public float isolationRange = 4;
    public float collisionRange = 3f;

    public bool close = false;
    public bool far = false;
    public bool normal = false;
    public bool turnel = false;



    IEnumerator Start()
     {
         Controller = GameObject.FindGameObjectWithTag("Controller");
         controller = Controller.GetComponent<BoidControllerTest>();
        Cam = GameObject.FindGameObjectWithTag("Cam");
        iden = Cam.GetComponent<Identification>();
        while (true)
        {
            if (!iden.turnnelseen)
            {
                Identification();
            }
             SituationAwareness();
             Planning();
             Pathplanning();
             //neighborhood.Clear();
             float waitTime = Random.Range(0.1f,0.3f);
             yield return new WaitForSeconds(waitTime);
         }

     }
    /*void Start()
    {
        Controller = GameObject.FindGameObjectWithTag("Controller");
        Cam = GameObject.FindGameObjectWithTag("Cam");
        iden = Cam.GetComponent<Identification>();
        controller = Controller.GetComponent<BoidControllerTest>();
    }
    void Update()
    {
        Identification();
        SituationAwareness();
        Planning();
        Pathplanning();        
    }*/


    Vector3 CalArrVector(List<GameObject> nearBoid)
    {
        Vector3 arrVec = Vector3.zero;
        return controller.leader.GetComponent<Rigidbody>().velocity;
       /* foreach (GameObject boid in nearBoid)
        {
            arrVec += boid.GetComponent<Rigidbody>().velocity;           
        }
        arrVec += controller.leader.GetComponent<Rigidbody>().velocity;

        return (arrVec / (nearBoid.Count+1)).normalized;*/

    }
    Vector3 CalSepVector(List<GameObject> nearBoid)
    {
        Vector3 sepVec = Vector3.zero;
        foreach (GameObject boid in nearBoid)
        {
            if (boid.name == "Obstacle")
            {
                sepVec += (boid.GetComponent<Rigidbody>().velocity) * (-1000);
            }
            else
            {
                sepVec += transform.position - boid.transform.position;
            }
        }
        if (nearBoid.Count != 0)
        {
            sepVec = sepVec / nearBoid.Count;
            return sepVec.normalized;
        }
        else
        {
            return Vector3.zero;
        }
    }
    Vector3 CalCohVector(List<GameObject> nearBoid)
    {
        Vector3 cohVec = new Vector3();
        foreach (GameObject boid in nearBoid)
        {
                cohVec += boid.transform.position;        
        }        
        if (nearBoid.Count != 0)
        {
            center = cohVec / nearBoid.Count;            
            return (center - transform.position).normalized;
        }
        else
        { 
            return Vector3.zero;
        }
    }
    void Identification() // 가장 근접 3~4개 인식
    {       
        neighborhood.Clear();
        tmp.Clear();

        for(int i=0;i<controller.boids.Count;i++)
        {
            tmp.Add(i, Vector3.Distance(transform.position, controller.boids[i].transform.position));           
        }

        var rank = tmp.OrderBy(num => num.Value);
        int k = 0;
        foreach (var r in rank)
        {
            if (this.name != controller.boids[r.Key].name)
            {               
                neighborhood.Add(controller.boids[r.Key]);                
            }
            k++;
            if (k==4)break;       
        }        
    }
    void SituationAwareness()
    {
        close = false;
        far = false;
        normal = false;
        turnel = false;
        foreach (GameObject boid in neighborhood)
        {
            if (Vector3.Distance(transform.position, boid.transform.position) <= collisionRange)
            {
                close = true;
                far = false;
                normal = false;
                break;
            }
        }
        if (close == false)
        {
            foreach (GameObject boid in neighborhood)
            {
                if (Vector3.Distance(transform.position, boid.transform.position) >= isolationRange)
                {
                    close = false;
                    far = true;
                    normal = false;
                    break;
                }
            }
        }
        if (!far && !close)
        {
            close = false;
            far = false;
            normal = true;
        }

    }
    void Planning()
    {
        CohVector = CalCohVector(neighborhood);//모든 boid의 위치의 평균-나의 위치= 모든 boid의 중심으로 방향벡터----> 응집
        ArrVector = CalArrVector(neighborhood);
        SepVector = CalSepVector(neighborhood); // 나와 주변의 uav와의 방향벡터의 평균 --> 분리     
        if(far)
        {
            Arrfactor = 1f;
            Cohfactor = 1f;
            Sepfactor = 0.5f;
            speed = controller.leader.GetComponent<LeaderAuto>().speed+0.3f;
        }   
        else if(close)
        {
            Arrfactor = 1f;
            Cohfactor = 0.5f;
            Sepfactor = 1f;
            speed = 1f;
        }
        else if(normal)
        {
            Arrfactor = 1f;
            Cohfactor = 1f;
            Sepfactor = 1f;
            speed = controller.leader.GetComponent<LeaderAuto>().speed;
        }
        if(iden.turnnelseen) //터널 발견 노멀 상황
        {
                Arrfactor = 1f;
                Cohfactor = 1f;
                Sepfactor = 0.5f;
                speed =controller.leader.GetComponent<LeaderAuto>().speed;
            if (close) //터널 발견 했으나 충돌위기
            {
                Arrfactor = 1f;
                Cohfactor = 0f;
                Sepfactor = 1f;
                speed = controller.leader.GetComponent<LeaderAuto>().speed-0.5f;
            }
            else if(far) //터널 발견 했지만 가까이 붙어야하는 상황
            {
                speed = controller.leader.GetComponent<LeaderAuto>().speed + 0.5f;
            }

        }
        direction = (Arrfactor * ArrVector + Cohfactor * CohVector + Sepfactor * SepVector).normalized * speed;        
    }
    void Pathplanning()
    {
        if (!iden.turnnelseen)
        {
            transform.LookAt(controller.leader.transform.position);
            GetComponent<Rigidbody>().velocity = direction;
        }
    }
}