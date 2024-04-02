using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillBarPrefs : MonoBehaviour
{
    public bool Assault, Sniper, Cover;

    // Start is called before the first frame update
    void Start()
    {
        if (Assault)
        {
            if(PlayerPrefs.GetInt("Assault"+transform.name) != 1)
            {
                Invoke("EnableAnimator", 3);
                PlayerPrefs.SetInt("Assault" + transform.name, 1);
            }
            else
            {
                GetComponent<Image>().fillAmount = 1;
                GetComponent<Animator>().enabled = false;
            }
        }
       else  if (Sniper)
        {
            if (PlayerPrefs.GetInt("Sniper" + transform.name) != 1)
            {
                Invoke("EnableAnimator", 3);
                PlayerPrefs.SetInt("Sniper" + transform.name, 1);
            }
            else
            {
                GetComponent<Image>().fillAmount = 1;
                GetComponent<Animator>().enabled = false;
            }
        }
        else if (Cover)
        {
            if (PlayerPrefs.GetInt("Cover" + transform.name) != 1)
            {
                Invoke("EnableAnimator", 3);
                PlayerPrefs.SetInt("Cover" + transform.name, 1);
            }
            else
            {
                GetComponent<Image>().fillAmount = 1;
                GetComponent<Animator>().enabled = false;
            }
        }
    }

    void EnableAnimator()
    {
        GetComponent<Animator>().enabled = true;
    }

    private void OnDisable()
    {
        CancelInvoke("EnableAnimator");
    }

}
