using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class flocking_mil : MonoBehaviour
{
    internal BoidController_mil controller;
    internal LAuto_Mil lauto;
    internal Identification iden;
    internal TopDownAgent_mil tagent;

    //public float NEIGHBOUR_RADIUS = 5f; //주변보이드의 무게중심으로 이동
    public List<GameObject> neighborhood = new List<GameObject>();




    private GameObject Controller;
    private GameObject Leader;
    private GameObject Cam;


    public float collisionRange = 30f;
    public float isolationRange = 45f;
    public float idRange = 50f;

    public Vector3 CohVector;
    public Vector3 ArrVector;
    public Vector3 SepVector;
    public Vector3 direction;
    private Vector3 currentdirection;


    public float Cohfactor = 1f;
    public float Sepfactor = 1f;
    public float Arrfactor = 1f;
    public float speed = 1;
    public float gapwithleader = 15f;

    public bool close = false;
    public bool far = false;
    public bool normal = false;
    public bool turnel = false;
    public bool manual = false;
    public bool farfromleader = false;
    public bool Topdown = false;
    public float dist = 0;


    public string state;
    public int cnt;

     IEnumerator Start()
     {
         Leader = GameObject.FindGameObjectWithTag("leader");
         lauto = Leader.GetComponent<LAuto_Mil>();
         Controller = GameObject.FindGameObjectWithTag("Controller");
         controller = Controller.GetComponent<BoidController_mil>();
         Cam = GameObject.FindGameObjectWithTag("Cam");
         iden = Cam.GetComponent<Identification>();
         tagent = Leader.GetComponent<TopDownAgent_mil>();
         while (true)
         {
             Identification();
             SituationAwareness();
            DecisionMaking();
             Planning();
             Pathplanning();            
             float waitTime = Random.Range(0.1f, 0.5f);
             yield return new WaitForSeconds(waitTime);
         }
         
     }
    /* void Start()
     {
         Leader = GameObject.FindGameObjectWithTag("leader");
         lauto = Leader.GetComponent<LAuto_Mil>();
         Controller = GameObject.FindGameObjectWithTag("Controller");
         controller = Controller.GetComponent<BoidController_mil>();
         Cam = GameObject.FindGameObjectWithTag("Cam");
         iden = Cam.GetComponent<Identification>();
         tagent = Leader.GetComponent<TopDownAgent_mil>();
         
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
          DecisionMaking();
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
        return arrVec.normalized;

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
                sep += ((neighborhood[t.Key].transform.position + neighborhood[t.Key].GetComponent<Rigidbody>().velocity) - transform.position);
                //sep += (neighborhood[t.Key].transform.position - transform.position);
            } //충돌범위 내에 있는 보이드들의 벡터를 더한다.
            rank = tmp.OrderByDescending(num => num.Value);
            foreach (KeyValuePair<int, float> t in tmp) //거리에 반비례하여 곱
            {
                sep *= t.Value;
            }

            return -((sep / neighborhood.Count)).normalized;
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
                coh += (boid.transform.position + boid.GetComponent<Rigidbody>().velocity);
                //coh += boid.transform.position;
            }
            coh /= neighborhood.Count;
            coh = coh - transform.position;
            return coh.normalized;
        }
        else
        {
            return Vector3.zero;
        }
    }


    void Identification()
    {
        
        neighborhood.Clear();
        
        //////////////인식범위 설정/////////////
        foreach (GameObject go in controller.boids) //인식범위 내
        {
            if(manual)
            {
                if(go.name=="leader")
                {

                }
                else
                {
                    neighborhood.Add(go);
                }
            }
            else 
            {
                if (Vector3.Distance(go.transform.position, transform.position) < idRange)
                {
                    if (this.name != go.name) neighborhood.Add(go);
                }                    
            }
        }
        
        if (Vector3.Distance(transform.position, Leader.transform.position) < idRange)
        {
            neighborhood.Add(Leader);
        }
        if(neighborhood.Count==0)
        {
            neighborhood.Add(Leader);
        }
    }




    void SituationAwareness()
    {
        Dictionary<int, float> tmp = new Dictionary<int, float>();
        List<GameObject> boidlists=new List<GameObject>();
        tmp.Clear();
        boidlists.Clear();
        Vector3 cent=new Vector3();
        if(neighborhood.Count==1)
        {
            cent=(transform.position+neighborhood[0].transform.position)/ 2;
        }
        else
        {
            foreach (GameObject boid in neighborhood)
            {
                cent += (boid.transform.position + boid.GetComponent<Rigidbody>().velocity);
                //coh += boid.transform.position;
            }
        }
        
        cent /= neighborhood.Count;


        for (int i = 0; i < controller.boids.Count; i++)
        {
            tmp.Add(i, Vector3.Distance(Leader.transform.position, controller.boids[i].transform.position)); //리더와 각 보이드들간의 거리
        }

        var rank = tmp.OrderBy(num => num.Value);        
        foreach (var r in rank)
        {
            if (this.name != controller.boids[r.Key].name)
            {
                boidlists.Add(controller.boids[r.Key]); //리더와의 거리 순으로 boid들을 정렬                               
            }
        }
        close = false;
        far = false;
        normal = false;
        turnel = false;
        dist = Vector3.Distance(Leader.transform.position, cent);
        if (dist>gapwithleader)
        {
            farfromleader = true;           
        }
        else
        {
            farfromleader = false;
        }
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

        if (iden.turnnelseen)
        {
            state = "종대";
        }
        else if (iden.obstacleseen)
        {
            state = "횡대";
        }
        else
        {
            if (neighborhood.Count == 0)
            {
                state = "follow Leader";
            }
            else if (close)
            {
                state = "close";
            }
            else if (far)
            {
                state = "far";
            }
            else if (normal)
            {
                state = "normal";
            }
        }
    }
    void DecisionMaking()
    {
        if(iden.turnnelseen || iden.obstacleseen || manual)
        {
            Topdown = true;
        }
        else if(!iden.turnnelseen && !iden.obstacleseen && !manual)
        {
            Topdown = false;
        }
    }
    void Planning()
    {
        speed = lauto.speed;
         if(farfromleader && !manual)
         {
             if (!close)
             {
                 neighborhood.Clear();
                 neighborhood.Add(Leader);
             }
             Sepfactor = 0.2f;
             Cohfactor = 1f;
             Arrfactor = 0.5f;

         }
         if(Topdown)
         {
             if (manual)
             {
                 Sepfactor = 3;
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
         else if (!Topdown)
         {
             ArrVector = Alignmnt();
             CohVector = Cohesion();
             SepVector = Seperation();

             if (far)
             {
                 Arrfactor = 0f;
                 Cohfactor = 1f;
                 Sepfactor = 0.5f;
                 speed = speed * 1.5f;
             }
             else if (close)
             {
                 Arrfactor = 0f;
                 Cohfactor = 0f;
                 Sepfactor = 1f;
                 //speed = 1f;
             }
             else if (normal)
             {
                 Arrfactor = 1f;
                 Cohfactor = 1f;
                 Sepfactor = 1f;
                 //speed = lauto.speed;
             }
         }

        if (Topdown)
        {
            if(manual)
            {
                speed = lauto.speed*3;
            }           
            if (farfromleader && !manual)
            {
                speed = lauto.speed * 2f;
            }
            else if (dist > idRange && !farfromleader)
            {
                speed = lauto.speed * 1.5f;
            }
            else if (dist <= idRange && !farfromleader)
            {
                speed = lauto.speed;
            }
        }
        if (farfromleader && !manual)
        {
            speed = lauto.speed * 2.3f;
            foreach(string name in iden.names)
            {
                if(this.name==name)
                {
                   // speed = 0;
                }
            }
        }
        if(iden.obstacleseen)
        {
            speed = lauto.speed * 2.3f;
        }
       

        direction = (Arrfactor * ArrVector + Cohfactor * CohVector + Sepfactor * SepVector);        
        direction = direction.normalized * speed;
        //direction.y = 0;
     /*   if ((transform.position.y - Leader.transform.position.y) >= 10)
        {
            direction.y = -1f;
        }
        else if((transform.position.y - Leader.transform.position.y) >= 10)
        {
            direction.y = 1f;
        }*/
      

    }
    void Pathplanning()
    {
        currentdirection = transform.position + direction;
        Vector3 lookpoint = currentdirection;
        lookpoint.y = transform.position.y;
        transform.LookAt(lookpoint);
        //transform.LookAt(currentdirection);
        if(Time.time<=5)
        {
            direction = (Vector3.up *direction.magnitude)*0.5f;
        }
        //GetComponent<Rigidbody>().velocity = direction;
        GetComponent<Rigidbody>().AddForce(direction);
    }
}
