using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionSizeToggle : MonoBehaviour
{
    enum state
    {
        shrinked,
        streching,
        streched,
        shrinking,
        focusing
    }
    state currentState = state.shrinked;
    float realWidth = 0;
    float lerpAmount = 0;
    float step = 0.045f;
    //int index;
    RectTransform rect, rectParent;
    //ContentSizeFitter filter;
    //float contentTarget = 0;
    float focusTarget = 0;
    public RectTransform selectionContent;
    //Transform sibling;


    void Start()
    {
        rect = GetComponent<RectTransform>();
        realWidth = rect.sizeDelta.x;
        rect.sizeDelta = new Vector2(0, rect.sizeDelta.y);
        //filter = transform.parent.GetComponent<ContentSizeFitter>();
        //index = transform.GetSiblingIndex();
        //contentTarget = (-(index  - 1) * 147);
        //sibling = filter.transform.GetChild(index - 1);
        //rectParent = filter.GetComponent<RectTransform>();
    }
    void FixedUpdate()
    {
        //filter.enabled = false;
        switch(currentState)
        {
            case state.streching:
                //rectParent.anchoredPosition = new Vector3(Mathf.Lerp(rectParent.anchoredPosition.x, contentTarget, lerpAmount), rectParent.anchoredPosition.y, 0);
                lerpAmount += step;
                //rect.sizeDelta = new Vector2(Mathf.Lerp(0, realWidth, lerpAmount), rect.sizeDelta.y);
                if (selectionContent)
                    selectionContent.anchoredPosition = new Vector2(0, Mathf.Lerp(selectionContent.anchoredPosition.y, focusTarget, lerpAmount));
                if (lerpAmount >= 1)
                {
                    lerpAmount = 0;
                    if(focusTarget == -99999)
                        currentState = state.streched;
                    else
                        currentState = state.focusing;
                }
                break;
            case state.shrinking:
                lerpAmount += step;
                //rect.sizeDelta = new Vector2(Mathf.Lerp(rect.sizeDelta.x, 0, lerpAmount), rect.sizeDelta.y);
                if (lerpAmount >= 1)
                {
                    lerpAmount = 0;
                    currentState = state.shrinked;
                }
                break;
            case state.focusing:
                lerpAmount += step * 3;
                if (selectionContent)
                    selectionContent.anchoredPosition = new Vector2(0, Mathf.Lerp(selectionContent.anchoredPosition.y, focusTarget, lerpAmount));
                if (lerpAmount >= 1)
                {
                    lerpAmount = 0;
                    currentState = state.streched;
                }
                break;
        }
        //filter.enabled = true;
    }
    void CloseView()
    {
        //sibling.GetChild(1).gameObject.SetActive(false);
        currentState = state.shrinking;
        lerpAmount = 0;
    }
    public void OnClick()
    {
        if (MainMenuManager.LastLevelToggle != null && MainMenuManager.LastLevelToggle != CloseView)
            MainMenuManager.LastLevelToggle();
        if(currentState != state.shrinked && currentState != state.shrinking)
        {
            currentState = state.shrinking;
            //sibling.GetChild(1).gameObject.SetActive(false);
            MainMenuManager.LastLevelToggle = null;
        }
        else
        {
            //sibling.GetChild(1).gameObject.SetActive(true);
            if(selectionContent)
            {
                selectionContent.gameObject.SetActive(true);
                selectionContent.anchoredPosition = new Vector2(selectionContent.anchoredPosition.x, 0);
            }
                
            currentState = state.streching;
            MainMenuManager.LastLevelToggle = CloseView;
        }
        lerpAmount = 0;
    }
    public void Focus(int row)
    {
        focusTarget = (row * 108) + 10;
        currentState = state.focusing;
        lerpAmount = 0;
    }
    public void SetFocusTarget(int row)
    {
        focusTarget = (row * 108) + 10;
    }
    private void OnDisable()
    {
        MainMenuManager.LastLevelToggle -= CloseView;
    }
}
