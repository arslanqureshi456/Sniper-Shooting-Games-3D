using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeLoadingReward : MonoBehaviour
{
    public static FakeLoadingReward instance;
    public GameObject FakeLoadingCanvas;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }
}
