using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PreSplash : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("LoadPreloader", 1);
    }
    
    void LoadPreloader()
    {
        SceneManager.LoadScene("Preloader");
    }

    private void OnDestroy()
    {
        CancelInvoke("LoadPreloader");
    }

    private void OnDisable()
    {
        CancelInvoke("LoadPreloader");
    }
}
