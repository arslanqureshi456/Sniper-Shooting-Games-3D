using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentInstantiate : MonoBehaviour
{
    public GameObject InstantiatePrefab;
    private void Awake()
    {
        Instantiate(InstantiatePrefab, InstantiatePrefab.transform.position, InstantiatePrefab.transform.rotation);
    }
}
