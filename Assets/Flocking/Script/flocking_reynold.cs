using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class flocking_reynold : MonoBehaviour
{
    internal BoidControllerTest controller;
    internal LeaderAuto lauto;
    internal Identification iden;
    internal TopDownAgent tagent;

    //public float NEIGHBOUR_RADIUS = 5f; //주변보이드의 무게중심으로 이동
    public List<GameObject> neighborhood = new List<GameObject>();




    private GameObject Controller;
    private GameObject Leader;
    private GameObject Cam;


    public float collisionRange = 3f;
    public float isolationRange = 7f;
    public float idRange = 5f;

    public Vector3 CohVector;
    public Vector3 ArrVector;
    public Vector3 SepVector;
    public Vector3 direction;
    private Vector3 currentdirection;


    public float Cohfactor = 1f;
    public float Sepfactor = 1f;
    public float Arrfactor = 1f;
    public float speed = 1;

    public bool close = false;
    public bool far = false;
    public bool normal = false;
    public bool turnel = false;

    private bool manual = false;

    public string state;
    public int cnt;

     IEnumerator Start()
     {
         Leader = GameObject.FindGameObjectWithTag("leader");
         lauto = Leader.GetComponent<LeaderAuto>();
         Controller = GameObject.FindGameObjectWithTag("Controller");
         controller = Controller.GetComponent<BoidControllerTest>();
         Cam = GameObject.FindGameObjectWithTag("Cam");
         iden = Cam.GetComponent<Identification>();
         tagent = Leader.GetComponent<TopDownAgent>();
         while (true)
         {
             Identification();
             SituationAwareness();
             Planning();
             Pathplanning();            
            // float waitTime = Random.Range(0.1f, 0.2f);
             yield return new WaitForSeconds(0.1f);
         }
         
     }
    /*void Start()
    {
        Leader = GameObject.FindGameObjectWithTag("leader");
        lauto = Leader.GetComponent<LeaderAuto>();
        Controller = GameObject.FindGameObjectWithTag("Controller");
        controller = Controller.GetComponent<BoidControllerTest>();
        Cam = GameObject.FindGameObjectWithTag("Cam");
        iden = Cam.GetComponent<Identification>();
        tagent = Leader.GetComponent<TopDownAgent>();

    }
    void Update()
    {
        //idRange = Vector3.Distance(transform.position,Leader.transform.position)+2f;
        if (Input.GetKeyDown(KeyCode.Space))
        {           
            idRange = 100;
            Sepfactor = 1;
            Cohfactor = 0.1f;
            Arrfactor = 0.5f;
            collisionRange = 100f;
            isolationRange = 1000f;
            manual = true;
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            idRange = 5;
            Sepfactor = 1;
            Cohfactor = 1f;
            Arrfactor = 1f;
            collisionRange = 3f;
            isolationRange = 7f;
            manual = false;
        }
        Identification();
        SituationAwareness();
        Planning();
        Pathplanning();

    }*/
    Vector3 Alignmnt()
    {
        Vector3 arrVec = Vector3.zero;
        arrVec = Leader.GetComponent<Rigidbody>().velocity;
        if (neighborhood.Count != 0)
        {
            foreach (GameObject boid in neighborhood)
            {
                arrVec += boid.GetComponent<Rigidbody>().velocity;
            }
            arrVec /= neighborhood.Count;
        }
        return arrVec;

    }
    Vector3 Seperation()
    {
        Dictionary<int, float> tmp = new Dictionary<int, float>();
        Vector3 sep = new Vector3();
        int index = 0;
        if (neighborhood.Count != 0)
        {
            foreach (GameObject boid in neighborhood)
            {
                tmp.Add(index, Vector3.Distance(transform.position, boid.transform.position + boid.GetComponent<Rigidbody>().velocity));
                //tmp.Add(index, Vector3.Distance(transform.position, boid.transform.position));
                index++;
            } //이웃한 boid들을 <인덱스, 거리>로 저장
            var rank = tmp.OrderBy(num => num.Value);

            foreach (KeyValuePair<int, float> t in tmp)
            {
               // sep += ((neighborhood[t.Key].transform.position + neighborhood[t.Key].GetComponent<Rigidbody>().velocity) - transform.position);
                sep += (neighborhood[t.Key].transform.position - transform.position);
            } //충돌범위 내에 있는 보이드들의 벡터를 더한다.
            rank = tmp.OrderByDescending(num => num.Value);
            foreach (KeyValuePair<int, float> t in tmp) //거리에 반비례하여 곱
            {
                sep *= t.Value;
            }

            return -((sep / neighborhood.Count));
        }
        else {
            return Vector3.zero;
        }
    }
    Vector3 Cohesion()
    {
        Vector3 coh = Vector3.zero;
        if (neighborhood.Count != 0)
        {
            foreach (GameObject boid in neighborhood)
            {
                //coh += (boid.transform.position + boid.GetComponent<Rigidbody>().velocity);
                coh += boid.transform.position;
            }
            coh /= neighborhood.Count;
            coh = coh - transform.position;
            return coh;
        }
        else
        {
            return Vector3.zero;
        }
    }


    void Identification()
    {
        Dictionary<int, float> tmp = new Dictionary<int, float>();
        neighborhood.Clear();
        tmp.Clear();
        //////////////인식범위 설정/////////////
        foreach (GameObject go in controller.boids) //인식범위 내
        {
            if (Vector3.Distance(go.transform.position, transform.position) < idRange)
            {
                if (this.name != go.name) neighborhood.Add(go);
            }            
        }   
        if(Vector3.Distance(transform.position,Leader.transform.position)<idRange)
        {
            neighborhood.Add(Leader);
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
        if(iden.turnnelseen)
        {
            state = "종대";
        }
        else if(iden.obstacleseen)
        {
            state = "횡대";
        }
        else
        {
            if(neighborhood.Count==0)
            {
                state = "follow Leader";
            }
            else if(close)
            {
                state = "close";
            }
            else if(far)
            {
                state = "far";                
            }
            else if(normal)
            {
                state = "normal";
            }
        }
    }
    void Planning()
    {
        speed = lauto.speed;        
        if (!iden.turnnelseen && !iden.obstacleseen && !manual)
        {
            if (neighborhood.Count == 0)
            {
                neighborhood.Add(Leader);
                Sepfactor = 0;
                Cohfactor = 1f;
                Arrfactor = 0f;
                speed = speed + 0.3f;
            }
            else
            {
                Sepfactor = 1f;
                Cohfactor = 1f;
                Arrfactor = 1f;
            }
        }
        else if (iden.turnnelseen || iden.obstacleseen || manual)
        {
            if (manual)
            {
                Sepfactor = 1;
                Cohfactor = 0.1f;
                Arrfactor = 0.5f;
            }
            else
            {
                Arrfactor = 1.0f;
                Cohfactor = 0f;
                Sepfactor = 0f;
            }
        }
        if (!iden.turnnelseen && !iden.obstacleseen)
        {
            ArrVector = Alignmnt();
        }
        CohVector = Cohesion();
        SepVector = Seperation();

        direction = (Arrfactor * ArrVector + Cohfactor * CohVector + Sepfactor * SepVector);
        direction.y = 0;
        if (iden.turnnelseen || iden.obstacleseen)
        {
            if (direction.magnitude > speed)
            {
                speed = speed * 2f;
                direction = direction.normalized * speed;
            }
            else
            {
                //direction 그대로
            }
        }
        else
        {
            direction = direction.normalized * speed;
        }      

    }
    void Pathplanning()
    {
        currentdirection = transform.position + direction;
        transform.LookAt(currentdirection);     
        GetComponent<Rigidbody>().velocity = direction;
        
    }
}
