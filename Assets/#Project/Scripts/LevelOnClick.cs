using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelOnClick : MonoBehaviour
{
    int sibling;
    bool isUnlocked = true, isReady = false;

    private void OnEnable()
    {
        if (isReady)
        {
            Invoke("DelayedEnable", 0.15f);
        }
    }
    void DelayedEnable()
    {
        if ((sibling + 1) == LevelSelectionNew.Instance.currentUnlocked)
        {
            LevelSelectionNew.Instance.lastLevel = DeSelect;
        }
    }
    private void Start()
    {
        //ABDUL
        GetComponent<Button>().onClick.AddListener(OnClick);
        sibling = (transform.GetSiblingIndex());
        isReady = true;
    }
   
    private void OnDisable()
    {
        transform.GetChild(2).gameObject.SetActive(false);
        CancelInvoke("DelayedEnable");
    }
    void OnClick()
    {
        AudioManager.instance.NormalClick();
        if (LevelSelectionNew.Instance.lastLevel != null)
        {
            LevelSelectionNew.Instance.lastLevel();
            LevelSelectionNew.Instance.lastLevel = null;
        }
        if (isUnlocked)
        {
            transform.GetChild(2).gameObject.SetActive(true);
            LevelSelectionNew.Instance.lastLevel = DeSelect;
        }
#if UNITY_EDITOR
        print("transform.GetChild(3).GetComponent<Text>().text : " + transform.GetChild(3).GetComponent<Text>().text);
#endif
        int level = int.Parse(transform.GetChild(3).GetComponent<Text>().text);
            LevelSelectionNew.Instance.OnLevelSelect(level-1);
            AssaultLevelsUpdation.instance.DisableCurrentLevelFirst();
    }


    void DeSelect()
    {
        transform.GetChild(2).gameObject.SetActive(false);//4
    }
}
