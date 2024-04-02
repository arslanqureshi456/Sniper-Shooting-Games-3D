using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelCompleteScoreFetch : MonoBehaviour
{
    public enum EndingParameters
    {
        kills,
        headshots,
        progress,
        ratio,
        fillamount,
        gold,
        sp,
        barrel,
        grenade,
        chopper,
        minhealth,
        minbullets,
        accuracy,
        drop,
        sniper,
        ratioCurrent
    }
    public EndingParameters parameter;
    void OnEnable()
    {
        Delayed();
        //Invoke("Delayed", 1.0f);
        
    }
    void Delayed()
    {
        gameObject.SetActive(true);
        switch (parameter)
        {
            case EndingParameters.gold:
                GetComponent<Text>().text = GameManager.Instance.gold + "";
                break;
            case EndingParameters.sp:
                GetComponent<Text>().text = GameManager.Instance.sp + "";
                break;
            case EndingParameters.progress:
                GetComponent<Text>().text = GameManager.Instance.currentProgress + "%";
                break;
            case EndingParameters.ratio:
                GetComponent<Text>().text = GameManager.Instance.levelRatio;
                break;
            case EndingParameters.ratioCurrent:
                GetComponent<Text>().text = GameManager.Instance.levelRatioCurrentLevel;
                break;
            case EndingParameters.fillamount:
                GetComponent<Image>().fillAmount = GameManager.Instance.currentProgress / 100;
                break;
        }
    }
    private void OnDisable()
    {
        switch(parameter)
        {
            case EndingParameters.progress:
            case EndingParameters.fillamount:
            case EndingParameters.gold:
            case EndingParameters.sp:
            case EndingParameters.ratio:
            case EndingParameters.ratioCurrent:

                break;
            default:
                gameObject.SetActive(false);
                break;
        }
      //  CancelInvoke("Delayed");
    }
}
