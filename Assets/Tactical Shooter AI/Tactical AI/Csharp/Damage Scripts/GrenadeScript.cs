﻿using UnityEngine;
using System.Collections;

/*
 * This script aims the grenade and launches it.
 *  It will also warn other agents of itself when it lands, usually prompting them to move out of the way.
 * */

namespace TacticalAI
{
    public class GrenadeScript : MonoBehaviour
    {
        //Timer starts after the grenade hits the ground
        public float timeTilExplode = 3;
        public GameObject explosion;

        private Vector3 target;

        //This was to give the grenade consistant force instead of adjusting the force and throwing at a 45 degree angle
        //var alterAngle : boolean = false;	
        public float warningRadius = 7;
        public float timeUntilWarningCanBeGiven = 1;

        private bool hasTarget = false;


        //public bool makeSureWeDontGoThroughObjects = true;
        //private Vector3 lastPos;
        public LayerMask layerMask;

        private bool warned = false;
        private bool canBeWarnedYet = false;

        private Rigidbody myRigidBody;

        public float runAwayBuffer = 3;

        void Go()
        {
            float throwForce = 0;
            //Aims grenade at Target
            if (hasTarget)
            {
                float xDist = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(target.x, target.z));
                float yDist = -(transform.position.y - target.y);

                //Calculate force required
                throwForce = xDist / (Mathf.Sqrt(Mathf.Abs((yDist - xDist) / (0.5f * (-Physics.gravity.y)))));
                throwForce = 1.414f * throwForce * GetComponent<Rigidbody>().mass;

                //Always fire on a 45 degree angle, this makes it easier to calculate the force required.
                transform.Rotate(-45, 0, 0);
            }
            myRigidBody = GetComponent<Rigidbody>();
            myRigidBody.AddForce(transform.forward * throwForce, ForceMode.Impulse);

            //Start the time on the grenade
            try
            {
                StartCoroutine(StartDetonationTimer());
            }
            catch { }
            //Because the grenade may skim the colliders of the agent before detonating, we want to wait a moment or two before being able to "warn" agents of the grenade
            //Warning will cause surrounding agents to attempt to escape from the grenade.
            StartCoroutine(SetTimeUntilWarning()); 	
        }

        void Warn()
        {
            //Only send out one warning
            if(!warned)
                {
                    warningRadius = warningRadius*warningRadius;
                    TacticalAI.Target[] targets = GameObject.FindGameObjectWithTag("AI Controller").GetComponent<TacticalAI.ControllerScript>().GetCurrentTargets();
				
                    for(int i = 0; i < targets.Length; i++)
                        {
                            //If the grenade is close enough and has clear line of sight (ie: they are not on the other side of a wall), then warn them
                            if(Vector3.SqrMagnitude(myRigidBody.position - targets[i].transform.position) < warningRadius && !Physics.Linecast(targets[i].transform.position, myRigidBody.position, layerMask))
                                {
                                    targets[i].targetScript.WarnOfGrenade(transform, warningRadius + runAwayBuffer);
                                }
                        }
				
				
                    warned = true;
                }	
        }

        void DetonateGrenade()
        {
            try
            {
                if (explosion)
                    Instantiate(explosion, transform.position, transform.rotation);
                else
                {
#if UNITY_EDITOR
                    Debug.LogWarning("No explosion object assigned to grenade!");
#endif
                }
            }
            catch { }

            Destroy(gameObject);
        }

        IEnumerator StartDetonationTimer()
        {
            yield return new WaitForSeconds(timeTilExplode);
            DetonateGrenade();
        }

        IEnumerator SetTimeUntilWarning()
        {
            yield return new WaitForSeconds(timeUntilWarningCanBeGiven);
            canBeWarnedYet = true;
        }

        void OnCollisionEnter(Collision collision)
        {
            if (canBeWarnedYet)
                 Warn();
        }

        void Awake()
        {
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Game")
            {
                if (GameManager.isFirstNade)
                {
                    AudioManager.instance.PlayNadeTakeCover();
                    GameManager.isFirstNade = false;
                }
                else if (Random.Range(0f, 1f) > 0.42f)
                {
                    AudioManager.instance.PlayNadeTakeCover();
                }
            }
            gameObject.GetComponent<Rigidbody>().useGravity = false;
            if (PlayerPrefs.GetInt("GrenadeCutScene").Equals(1))
                StartCoroutine(EnableCamera());
            //if (!PlayerPrefs.GetInt("CanThrowGrenade").Equals(1) && (GameManager.Instance.levelIndex.Equals(4) ||
            //    GameManager.Instance.levelIndex.Equals(5) || PlayerPrefs.GetInt("GrenadeCutScene").Equals(1)))
            //    StartCoroutine(EnableCamera());
        }

        public void SetTarget(Vector3 pos)
        {
            target = pos;
            Debug.DrawLine(transform.position, pos);
            pos.y = transform.position.y;
            transform.LookAt(pos);
            gameObject.GetComponent<Rigidbody>().useGravity = true;
            hasTarget = true;
            Go();
        }

        public void DropGrenade()
        {
            gameObject.GetComponent<Rigidbody>().useGravity = true;
            transform.parent = null;
            try
            {
                StartCoroutine(StartDetonationTimer());
            }
            catch { }
        }

        private IEnumerator EnableCamera()
        {
            //PlayerPrefs.SetInt("CanThrowGrenade", 1);
            PlayerPrefs.SetInt("GrenadeCutScene", 0);
            GetComponent<TrailRenderer>().enabled = false;
            yield return new WaitForSeconds(0.3f);
            FindObjectOfType<GrenadeCutScene>().target_1 = this.transform;
            FindObjectOfType<GrenadeCutScene>().enabled = true;
            FindObjectOfType<GrenadeCutScene>().gameObject.GetComponent<Camera>().enabled = true;
            GetComponent<TrailRenderer>().enabled = true;
            Time.timeScale = 0;
        }
    }
}
