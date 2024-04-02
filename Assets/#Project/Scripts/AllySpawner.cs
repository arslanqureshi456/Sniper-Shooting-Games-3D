using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllySpawner : MonoBehaviour
{
    public Transform patrolPoint;
    public GameObject AllyPrefab;
    TacticalAI.BaseScript baseScript;
    TacticalAI.TargetScript tScript;
    void Start()
    {
        AllyPrefab.GetComponent<TacticalAI.HealthScript>().teamID = 2;
        baseScript = Instantiate(AllyPrefab, transform.position, transform.rotation).GetComponent<TacticalAI.BaseScript>();
        tScript = baseScript.GetComponent<TacticalAI.TargetScript>();
        TacticalAI.TargetScript.commonTarget = new TacticalAI.Target(0, tScript.GetUniqueID(), baseScript.transform, tScript);
        //baseScript.canEngage = false;
        baseScript.SetMoveTarget(patrolPoint);
    }
}
