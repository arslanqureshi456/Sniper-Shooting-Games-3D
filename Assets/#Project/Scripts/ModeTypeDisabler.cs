using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeTypeDisabler : MonoBehaviour
{
    public LevelSelectionNew.modeType mode;
    private void OnEnable()
    {
        if (LevelSelectionNew.modeSelection != mode)
            gameObject.SetActive(false);
    }
}
