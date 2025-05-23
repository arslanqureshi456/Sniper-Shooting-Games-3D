﻿using UnityEngine;
using System.Collections;

/*
 * This script takes damage from various body parts, multiplies it, and passes it onto the Health Script.
 * 
 * */

namespace TacticalAI
{
    public class HitBox : MonoBehaviour
    {
        public float damageMultiplyer = 1;
        private Vector3 addForceVector;

        private Rigidbody myRigidBody;
        public TacticalAI.HealthScript myScript;
        public bool canDoSingleHealthBoxDamage = true;

        [HideInInspector]
        public float damageTakenThisFrame = 0;
        //public bool storeDamage = false;
        public int teamID;
        bool onceHeadshot = true;

        public GameObject damageTextPrefab;
        public AudioClip headShot;

        void Awake()
        {
            if(GetComponentInParent<TacticalAI.HealthScript>())
                teamID = GetComponentInParent<TacticalAI.HealthScript>().teamID;
            myRigidBody = gameObject.GetComponent<Rigidbody>();
        }

        //ABDUL
        public void Damage(float damage, RaycastHit hit)
        {
            try
            {
                if (myScript)
                {
                    //ABDUL
                    if (GameManager.Instance && damageMultiplyer > 1)
                    {
                        //  Invoke("DelayHeadshotSound", 0.3f);
                     //   GameManager.Instance.ShowHeadShotAnim();

                        //GameManager.Instance.headShot++;
                        //GameManager.Instance.ShowHeadShotText();
                        //GameManager.Instance.ShowHeadShotScoreText();
                    }
                    //else if (GameManager.Instance)
                    //{
                    //    GameManager.Instance.ShowHeadShotScoreText();
                    //}
                    damage = damage * damageMultiplyer;

                    //GameObject damageTextInstance = Instantiate(damageTextPrefab, this.transform.position, this.transform.rotation);
                    //damageTextInstance.transform.GetChild(0).GetComponent<TextMesh>().text = ((int)damage).ToString();

                    StartCoroutine("StoreDamageTakenRecently", damage);
                    if (myScript && myScript.health > 0)
                        myScript.Damage(damage, hit);
                }
            }
            catch { }
        }

        void DelayHeadshotSound()
        {
            PlayAudioAtPos.PlayClipAt(headShot, this.transform.position, 1.0f, 0.0f);
        }

        public void Damage(float damage)
        {
                if (myScript)
                {
                    //ABDUL
                    //if (damageMultiplyer > 1)
                    //{
                    //    GameManager.Instance.headShot++;
                    //    GameManager.Instance.ShowHeadShotText();
                    //}

                    //Use the multiplier to take differing amounts of damage depending on where the AI is hit
                    damage = damage * damageMultiplyer;

                    //GameObject damageTextInstance = Instantiate(damageTextPrefab, this.transform.position, this.transform.rotation);
                    //damageTextInstance.transform.GetChild(0).GetComponent<TextMesh>().text = ((int)damage).ToString();

                    //Store the amount of damage taken for the dismemberment sript
                    StartCoroutine("StoreDamageTakenRecently", damage);

                    if (myScript)
                        myScript.Damage(damage);
                }
            
        }

        //public void Damage(float damage, float force, Vector3 dir)
        //{
        //    if (myScript)
        //    {
        //        //Use the multiplier to take differing amounts of damage depending on where the AI is hit
        //        damage = damage * damageMultiplyer;

        //        StartCoroutine(AddForceVector(force*dir));

        //        //Store the amount of damage taken for the dismemberment sript
        //        StartCoroutine("StoreDamageTakenRecently", damage);

        //        if (myScript)
        //            myScript.Damage(damage);
        //    }
        //}

        
        //}

        //Use for explosives
        public void SingleHitBoxDamage(float damage)
        {
            //We don't do the damage multiplier here because this is used for explosions, and we  don't want to leave it up to RNG which hitbox is used first
            if (myScript)
            {
                    //StartCoroutine("StoreDamageTakenRecently", damage);

                if (canDoSingleHealthBoxDamage)
                    StartCoroutine(myScript.SingleHitBoxDamage(damage));
                else
                    myScript.Damage(damage);
            }
        }

        public IEnumerator StoreDamageTakenRecently(float d)
        {
            //Only store the damage this frame.  That way only a single strong damage source will trigger the dismemberment script.
            damageTakenThisFrame += d;
            yield return 0;
            damageTakenThisFrame -= d;
        }

        //Do I even need this?
        public IEnumerator AddForceVector(Vector3 fv)
        {
            yield return null;
            if (myRigidBody)
                {
                    myRigidBody.AddForce(fv, ForceMode.Impulse);
                }
        }
    }
}
