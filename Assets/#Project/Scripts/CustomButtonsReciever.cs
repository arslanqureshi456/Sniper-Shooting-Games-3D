using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomButtonsReciever : MonoBehaviour
{
    public int index;
    bool isFirst = true;
    public bool isEnabled = false;
    void OnEnable()
    {
        //if (isEnabled)
            Invoke("DelayedPositions", 1f);
        //else
            //CheckButtons();
    }
   
    void DelayedPositions()
    {
        try
        {
            if (UiPositionChanger.Instance != null && UiPositionChanger.Instance.isCustom)
            {
                GetComponent<RectTransform>().localScale = UiPositionChanger.Instance.GetScale(index);
                GetComponent<RectTransform>().position = UiPositionChanger.Instance.GetPosition(index);
            }
        }
        catch { }
    }

    private void OnDisable()
    {
        CancelInvoke("DelayedPositions");
    }
}
