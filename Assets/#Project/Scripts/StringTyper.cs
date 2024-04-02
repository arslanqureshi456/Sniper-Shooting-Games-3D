using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StringTyper : MonoBehaviour
{
    public bool isLast = false;
    bool isBlink = false;
    public string str = "";
    int index = 0,blinkCount = 0;
    Text text;
    float delay;
    public GameObject second = null;
    void Start()
    {
        text = GetComponent<Text>();
        Delay();
        //Invoke("Delay", 0.01f);
        
    }
    void Delay()
    {
        if (text.text == "")
        {
            str = PlayerPrefs.GetString("Name");
        }
        else
            str = text.text;
        text.text = "";
        delay = 1.2f /(float)str.Length;
        StartCoroutine(Typer());
    }
    IEnumerator Typer()
    {

        while(index < str.Length)
        {
            index++;
            text.text = str.Substring(0, index);
            yield return new WaitForSecondsRealtime(delay);
        }
        /*while (blinkCount < 10)
        {
            index++;
            text.text = str.Substring(0, index);
            if (index == str.Length - 1)
                isBlink = true;
            if (isBlink)
            {
                if(isLast)
                {
                    blinkCount++;
                    yield return new WaitForSeconds(0.1f);
                    if (index == str.Length)
                        index = str.Length - 2;
                }
                else
                {
                    blinkCount++;
                    index = str.Length - 1;
                }
            }else
            { 
                AudioManager.instance.TypeName(); 
            }
            
            yield return new WaitForSeconds(0.11f);
        }
        if (!isLast && second != null)
            second.SetActive(true);
        else
        {
            yield return new WaitForSeconds(0.9f);
            MainMenuManager.Instance.ShowWelcome();
        }*/
    }

    void OnDisable()
    {
        StopCoroutine("Typer");
    }
}
