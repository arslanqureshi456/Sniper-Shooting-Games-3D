using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisablePanel : MonoBehaviour
{
    void DiablePanel()
    {
        this.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        Invoke("DiablePanel", 3f);
    }

    private void OnDisable()
    {
        CancelInvoke("DiablePanel");
    }
}
