using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Example : MonoBehaviour
{
    public Color _CSColor, _SniperColor, _AssaultColor;
    public Material[] environmentMaterials;

    private Color _color;

    void OnEnable()
    {
        switch (LevelSelectionNew.modeSelection)
        {
            case LevelSelectionNew.modeType.SNIPER:
            case LevelSelectionNew.modeType.MULTIPLAYER:
            case LevelSelectionNew.modeType.FREEFORALL:
            case LevelSelectionNew.modeType.BR:
                _color = _SniperColor;
                break;
            case LevelSelectionNew.modeType.ASSAULT:
                _color = _AssaultColor;
                break;
            case LevelSelectionNew.modeType.COVERSTRIKE:
                _color = _CSColor;
                break;
        }

        for (int i = 0; i < environmentMaterials.Length; i++)
        {
            environmentMaterials[i].color = _color;
            RenderSettings.ambientLight = _color;
        }
    }
}
