using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ShaderControl;

/*Applies damage and force to all colliders within a given radius.*/

namespace TacticalAI
{
    public class ExplosionScript : MonoBehaviour
    {
    	public string damageMethodName = "Damage";
    
        public float explosionRadius = 5.0f;
        public float explosionPower = 10.0f;
        public float upwardsPower = 10.0f;
        public float damage = 200.0f;
        public LayerMask layerMask;
        public float explosionTime = 5;
        public bool scaleDamageByDistance = true;

        public bool showBlastRadius = false;

        public bool shouldDoSingleHitboxDamage = false;

        void Awake()
        {
            try
            {
                if(gameObject.activeInHierarchy)
                StartCoroutine(Go());
            }
            catch { }
        }

        IEnumerator Go()
        {
            //ABDUL
            ExplosionEffect(this.transform.position);

            Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
            Collider hit;
            List<Rigidbody> hitBodies = new List<Rigidbody>();
            int i = 0;
            float damageThisTime = damage;

            for (i = 0; i < colliders.Length; i++)
            {
                hit = colliders[i];
                //Make sure we have line of sight to the collider
                if (!Physics.Linecast(transform.position, hit.transform.position, layerMask))
                {
                    //Make damage fall off if further away.  Scaling is linear
                    if(scaleDamageByDistance){
                        damageThisTime = damage * Vector3.Distance(transform.position , hit.transform.position) / explosionRadius;
                    }

                    if (hit.GetComponent<Collider>().gameObject.GetComponent<FPSPlayer>())
                    {
                        hit.GetComponent<Collider>().gameObject.GetComponent<FPSPlayer>().ApplyDamage(damage);
                    }
                 
                    //Ideally, you should use single hitbox damage, but non-paragon AI scripts may not support it.
                    //if (shouldDoSingleHitboxDamage)
                    //{
                    //    hit.GetComponent<Collider>().SendMessage("SingleHitBoxDamage", damageThisTime, SendMessageOptions.DontRequireReceiver);
                    //}
                    //else
                    //{
                    //    //Will damage the same agent once for each collider
                    //    hit.GetComponent<Collider>().SendMessage(damageMethodName, damageThisTime, SendMessageOptions.DontRequireReceiver);
                    //}

                    if (hit.GetComponent<Rigidbody>())
                        hitBodies.Add(hit.GetComponent<Rigidbody>());
                }
            }

            //Make sure things are dead.
            yield return null;
            for (i = 0; i < hitBodies.Count; i++)
            {
                //Make sure the rigid body still exists so we don't get an error (it may be destroyed if the target is killed)
                if (hitBodies[i])
                    hitBodies[i].AddExplosionForce(explosionPower, transform.position, explosionRadius, upwardsPower, ForceMode.Impulse);
            }

            //Destroy the explosion object, given some time afterwards to let the special effect play out.
            Destroy(gameObject, explosionTime);
        }

        void OnDrawGizmos()
        {
            if (showBlastRadius)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(transform.position, explosionRadius);
            }
        }

        //ABDUL
        public GameObject explosion;

        private GameObject explosionObj;
        private ParticleSystem partSys;
        public void ExplosionEffect(Vector3 position)
        {
            if(explosion == null)
            {
                explosion = GameObject.Find("FPS Effects").transform.GetChild(5).gameObject;
            }
            explosionObj = explosion;
            explosionObj.transform.position = position;
            foreach (Transform child in explosionObj.transform)
            {//emit all particles in the particle effect game object group stored in impactObj var
                if (child.GetComponent<ParticleSystem>())
                {
                    partSys = child.GetComponent<ParticleSystem>();
                    partSys.Emit(Mathf.RoundToInt(partSys.emission.rateOverTime.constant));//emit the particle(s)
                }
            }
        }

        private void OnDisable()
        {
            StopCoroutine("Go");
        }
    }
}
