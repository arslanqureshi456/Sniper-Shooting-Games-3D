using DG.Tweening.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuHeli : MonoBehaviour
{
    public float Xmax, Xmin;
    Vector3 TargetX;
    public static Vector3 BulletTarget;
    int FireState = 0;
    float TurnFactor;
    Quaternion TargetRot;
    public GameObject Rocket;
    public Transform GunPoint, GunPoint1;
    void Start()
    {
        transform.position = new Vector3(Xmax, transform.position.y, transform.position.z);
        transform.localEulerAngles = new Vector3(12, 242, 4);
        TargetX = new Vector3(Xmin, transform.position.y, transform.position.z);
        //StartCoroutine(Occilator());
        StartCoroutine(Fire());
    }

    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, TargetX, 0.001f);
        if (transform.position.x > 1200 && TargetX.x == Xmax)
        {
            TargetX = new Vector3(Xmin, transform.position.y, transform.position.z);
            transform.localEulerAngles = new Vector3(12, 242, 4);
        } 
        else if(transform.position.x < -1200 && TargetX.x == Xmin)
        {
            TargetX = new Vector3(Xmax, transform.position.y, transform.position.z);
            transform.localEulerAngles = new Vector3(12, 242, 4);
            transform.localEulerAngles = new Vector3(12, 114, 4);
        }
        //if (FireState == 1)
        //{
        //    TurnFactor += 0.07f;
        //    transform.rotation = Quaternion.Lerp(transform.rotation, TargetRot, TurnFactor);
        //    if (TurnFactor >= 1)
        //    {
        //        FireState = 0;

            //    }
            //}
    }
    IEnumerator Fire()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(1.78f, 3.35f));
            //Vector3 relativePos = BulletTarget - transform.position;
            //TargetRot = Quaternion.LookRotation(relativePos, Vector3.up);
            //FireState = 1;
            //TurnFactor = 0;
            //if((TargetX.x == Xmax && transform.position.x < -130 && transform.position.x > -550) || (TargetX.x == Xmin && transform.position.x > 130 && transform.position.x < 550))
            //{
            
            float b = Random.Range(-180, 180);
            if (b > 0 && b < 20)
                b = 20;
            else if (b < 0 && b < -20)
                b = -20;
            float a = b * -1.5f;
            if (a < 0)
                a = -a;
            BulletTarget = new Vector3(b, (-6 - a / 3), a);
            GameObject.Instantiate(Rocket, BulletTarget, Quaternion.identity);
            //Invoke("DelayedTrauma", 0.32f);
            //}
        }
    }

    void OnDisable()
    {
        StopCoroutine("Fire");
    }
   
}
