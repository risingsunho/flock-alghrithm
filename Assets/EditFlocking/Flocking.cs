using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Flocking : MonoBehaviour
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
    public float idRange = 8f;

    public Vector3 CohVector;
    public Vector3 ArrVector;
    public Vector3 SepVector;
    public Vector3 direction;

    public float Cohfactor = 1f;
    public float Sepfactor = 1f;
    public float Arrfactor = 1f;
    public float speed = 1;

    public bool close = false;
    public bool far = false;
    public bool normal = false;
    public bool turnel = false;

    private bool manual = false;
    private bool hasleader = false;


    public int cnt;


    /* IEnumerator Start()
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
             float waitTime = Random.Range(0.1f, 0.3f);
             yield return new WaitForSeconds(waitTime);
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

    }*/
    void Update()
    {
        //idRange = Vector3.Distance(transform.position,Leader.transform.position)+2f;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            collisionRange = 2;
            isolationRange = 100;
            Sepfactor = 1;
            Cohfactor = 0.1f;
            Arrfactor = 0.5f;
            manual = true;
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            collisionRange = 3;
            isolationRange = 7;
            Sepfactor = 1;
            Cohfactor = 1f;
            Arrfactor = 1f;
            manual = false;
        }
        /*Identification();
        SituationAwareness();
        Planning();
        Pathplanning(); */

    }
    Vector3 Alignmnt()
    {
        Vector3 arrVec = Vector3.zero;
        arrVec = Leader.GetComponent<Rigidbody>().velocity;
         foreach (GameObject boid in neighborhood)
          {
              arrVec += boid.GetComponent<Rigidbody>().velocity;           
          }
        // arrVec = arrVec+ Leader.GetComponent<Rigidbody>().velocity*5;
         arrVec /= neighborhood.Count;

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
                index++;
            } //이웃한 boid들을 <인덱스, 거리>로 저장
            var rank = tmp.OrderBy(num => num.Value);

            foreach (KeyValuePair<int, float> t in tmp)
            {
                sep += ((neighborhood[t.Key].transform.position + neighborhood[t.Key].GetComponent<Rigidbody>().velocity) - transform.position);
            } //충돌범위 내에 있는 보이드들의 벡터를 더한다.
            /*rank = tmp.OrderByDescending(num => num.Value);
            foreach (KeyValuePair<int, float> t in tmp) //거리에 반비례하여 곱
            {
                sep *= t.Value;
            }*/

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
                coh += (boid.transform.position + boid.GetComponent<Rigidbody>().velocity);
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
        hasleader = false;
        Dictionary<int, float> tmp = new Dictionary<int, float>();
        neighborhood.Clear();
        tmp.Clear();
        //////////////인식범위 설정/////////////
        foreach (GameObject go in controller.boids)
        {
            if (Vector3.Distance(go.transform.position, transform.position) < idRange)
            {
                if (this.name != go.name) neighborhood.Add(go);
            }
        }
        if (!iden.turnnelseen)
        {
            if (neighborhood.Count == 0)
            {
               // neighborhood.Add(Leader);

                speed = lauto.speed * 1.5f;
                Sepfactor = 0;
                Cohfactor = 1f;
                Arrfactor = 0.5f;
            }
            else
            {
                // idRange =5f;
                speed = lauto.speed;
                Sepfactor = 1f;
                Cohfactor = 1f;
                Arrfactor = 1f;
            }
        }
    }




    void SituationAwareness()
    {
       /* close = false;
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
        }*/

    }
    void Planning()
    {
        if (!iden.turnnelseen)
        {
            ArrVector = Alignmnt();
        }
        CohVector = Cohesion();
        SepVector = Seperation();
       /* if (!manual)
        {
            if (iden.turnnelseen)
            {
                if (far)
                {
                    Arrfactor = 1f;
                    Cohfactor = 0f;
                    Sepfactor = 0.5f;
                    //collisionRange = 4;
                    // isolationRange = 7;
                    idRange = 8f;
                    speed = lauto.speed + 0.3f;
                }
                else if (close)
                {
                    Arrfactor = 0.1f;
                    Cohfactor = 0.1f;
                    Sepfactor = 1f;
                    collisionRange = 2;
                    isolationRange = 7;
                    idRange = 8f;
                    speed = lauto.speed;
                }
                else if (normal)
                {
                    Arrfactor = 1f;
                    Cohfactor = 0f;
                    Sepfactor = 0f;
                    //collisionRange = 3;
                    //isolationRange = 5;
                    idRange = 8f;
                    speed = lauto.speed;
                }

            }
            else {
                if (far)
                {
                    Arrfactor = 1f;
                    Cohfactor = 1f;
                    Sepfactor = 0.1f;
                    //collisionRange = 4;
                    // isolationRange = 7;
                    idRange = 10f;
                    speed = lauto.speed + 0.3f;
                    if (close)
                    {
                        Arrfactor = 0.5f;
                        Cohfactor = 0.1f;
                        Sepfactor = 1f;
                        //collisionRange = 4;
                        //isolationRange = 6;
                        idRange = 5f;
                        speed = lauto.speed;
                    }
                }
                else if (close)
                {
                    Arrfactor = 0.5f;
                    Cohfactor = 0.1f;
                    Sepfactor = 1f;
                    // collisionRange = 4;
                    // isolationRange = 6;
                    idRange = 5f;
                    speed = lauto.speed;
                }
                else if (normal)
                {
                    Arrfactor = 1f;
                    Cohfactor = 1f;
                    Sepfactor = 1f;
                    // collisionRange = 3;
                    // isolationRange = 5;
                    idRange = 8f;
                    speed = lauto.speed;
                }
            }
        }*/

        direction = (Arrfactor * ArrVector + Cohfactor * CohVector + Sepfactor * SepVector).normalized * speed;
        direction.y = 0;
    }
    void Pathplanning()
    {
        transform.LookAt(controller.leader.transform.position);
        GetComponent<Rigidbody>().velocity = direction;
    }
}
