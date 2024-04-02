using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NadeScene : MonoBehaviour
{
    public GameObject HandNade, ThrowNade;
    public Transform nadeLookTarget;
    public LookAt lookScript;
    void Start()
    {
        
    }
    public void ShowHandNade()
    {
        HandNade.SetActive(true);
    }
    public void HideHandNade()
    {
        HandNade.SetActive(false);
    }
}
