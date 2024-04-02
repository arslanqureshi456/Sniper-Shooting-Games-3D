using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SoldierScene : MonoBehaviour
{
    public bool isPatrol;
    public Transform Target;
    void OnEnable()
    {
        if (isPatrol)
        {
            GetComponent<Animator>().SetTrigger("isWalkAnim");
        }else
        {
            GetComponent<Animator>().SetTrigger("isSitAnim");
        }
    }
    private void Start()
    {
        //transform.parent.GetComponent<NavMeshAgent>().SetDestination(Target.position);
    }
}
