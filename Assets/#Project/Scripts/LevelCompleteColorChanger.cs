using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelCompleteColorChanger : MonoBehaviour
{
    private void OnEnable()
    {
        if (GameManager.Instance.isLevelFailed)
            if(GetComponent<Image>())
                GetComponent<Image>().color = Color.gray;
        else if(GetComponent<Text>())
                GetComponent<Text>().color = Color.gray;
    }
}
