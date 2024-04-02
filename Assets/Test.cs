using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject _objectToInstantiate;

    private void OnEnable()
    {
        Instantiate(_objectToInstantiate);
    }
}
