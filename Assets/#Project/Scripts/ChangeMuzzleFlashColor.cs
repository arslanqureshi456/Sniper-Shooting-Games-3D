using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMuzzleFlashColor : MonoBehaviour
{
    public Color _mpColor, _csmColor, _m4Color, _akColor;
    public Material[] _materials;

    private Color _color;

    void FixedUpdate()
    {
        switch (PlayerPrefs.GetInt("selectedWeaponIndex"))
        {
            case 0:
            case 1:
            case 2:
            case 3:
            case 4:
                _color = _mpColor;
                break;
            case 7:
            case 9:
            case 11:
            case 13:
            case 16:
            case 18:
                _color = _m4Color;
                break;
            case 5:
            case 8:
            case 12:
            case 15:
                _color = _akColor;
                break;
            case 6:
            case 10:
            case 14:
            case 17:
                _color = _csmColor;
                break;
        }

        for (int i = 0; i < _materials.Length; i++)
        {
            _materials[i].SetColor("_TintColor", _color);
        }
    }
}
