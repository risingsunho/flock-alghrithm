using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamPositioning : MonoBehaviour {
    GameObject Cam;
    internal Identification iden;
    internal LAuto_Mil lauto;
    public Camera world1;
    public GameObject Leader;


    Vector3 cam1pos;
    Vector3 cam2pos;
    Vector3 cam3pos;
    Vector3 cam4pos;
    Vector3 cam5pos;


    Vector3 cam1rot;
    Vector3 cam2rot;
    Vector3 cam3rot;
    Vector3 cam4rot;
    Vector3 cam5rot;

    Quaternion cam1qua;
    Quaternion cam2qua;
    Quaternion cam3qua;
    Quaternion cam4qua;
    Quaternion cam5qua;

    // Use this for initialization
    void Start () {
        Cam = GameObject.FindGameObjectWithTag("Cam");
        iden = Cam.GetComponent<Identification>();
        Leader = GameObject.FindGameObjectWithTag("leader");
        lauto = Leader.GetComponent<LAuto_Mil>();


        cam1pos = new Vector3(0,-0.33f,-8f);
        cam1rot = new Vector3(0,0,0);
        cam1qua = Quaternion.Euler(cam1rot);

        cam2pos = new Vector3(2.67f, 1.86f, 2.1f);
        cam2rot = new Vector3(20, 220, 0);
        cam2qua = Quaternion.Euler(cam2rot);

        cam3pos = new Vector3(0, 5.28f, -10.92f);
        cam3rot = new Vector3(20, 0, 0);
        cam3qua = Quaternion.Euler(cam3rot);

        cam4pos = new Vector3(0, 4.34f, 8.26f);
        cam4rot = new Vector3(20, -180, 0);
        cam4qua = Quaternion.Euler(cam4rot);

        cam5pos = new Vector3(0, 5f, 0f);
        cam5rot = new Vector3(90f, 354.3f, 0);
        cam5qua = Quaternion.Euler(cam5rot);


    }

    // Update is called once per frame
    void FixedUpdate () {
        
        if (lauto.i==0 || lauto.i == 3) //이글
        {
            world1.transform.localPosition = Vector3.Lerp(world1.transform.localPosition, cam2pos, Time.fixedDeltaTime);
            world1.transform.localRotation = Quaternion.Slerp(world1.transform.localRotation, cam2qua, Time.fixedDeltaTime);
        }
        else if (lauto.i==12)//뒤 위
        {
            world1.transform.localPosition = Vector3.Lerp(world1.transform.localPosition, cam3pos, Time.fixedDeltaTime);
            world1.transform.localRotation = Quaternion.Slerp(world1.transform.localRotation, cam3qua, Time.fixedDeltaTime);
        }
        else if (lauto.i==6) // 앞 위
        {
            world1.transform.localPosition = Vector3.Lerp(world1.transform.localPosition, cam4pos, Time.fixedDeltaTime);
            world1.transform.localRotation = Quaternion.Slerp(world1.transform.localRotation, cam4qua, Time.fixedDeltaTime);
        }
        else if (lauto.i == 8 || lauto.i==9 || lauto.i==10) //위
        {
            world1.transform.localPosition = Vector3.Lerp(world1.transform.localPosition, cam5pos, Time.fixedDeltaTime);
            world1.transform.localRotation = Quaternion.Slerp(world1.transform.localRotation, cam5qua, Time.fixedDeltaTime);
        }
        else //뒤
        {
            world1.transform.localPosition = Vector3.Lerp(world1.transform.localPosition, cam1pos, Time.deltaTime);
            world1.transform.localRotation = Quaternion.Slerp(world1.transform.localRotation, cam1qua, Time.deltaTime);
        }
    }
}




