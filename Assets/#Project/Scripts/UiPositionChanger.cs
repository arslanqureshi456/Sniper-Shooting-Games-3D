using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UiPositionChanger : MonoBehaviour
{
    [Serializable]
    public class DualArrays
    {
        public Transform[] Objects;
    }
    public GameObject PopUp;
    public bool isCustom = false, isMulti = false;
    public static UiPositionChanger Instance;
    public enum ButtonNames
    {
        FireBTNLeft,
        FireBTNRight
    }
    public RectTransform[] Buttons, Corners;
    public RectTransform AddsContainer;
    public class UIPositionContainer
    {
        public Vector2[] Positions;
        public Vector3[] Scales;
        public UIPositionContainer(int i)
        {
            Positions = new Vector2[i];
            Scales = new Vector3[i];
        }
    }
    Vector2 lastSafePosition;
    float lastSafeSize;
    public Slider sizeSlider;
    UIPositionContainer pos;
    int currentButton = -1, lastButton = -1;
    void OnEnable()
    {
        Instance = this;
        isCustom = false;
        if (!isMulti)
        {
            if (PlayerPrefs.HasKey("UIPositions"))
            {
                SetButtons();
            }
        }
        else
        {
            if (PlayerPrefs.HasKey("UIPositionsMulti"))
            {
                SetButtons();
            }
        }
        ResetBoundaries();
    }
    void SetButtons()
    {
        if (!isMulti)
        {
            pos = JsonUtility.FromJson<UIPositionContainer>(PlayerPrefs.GetString("UIPositions"));
        }
        else
        {
            pos = JsonUtility.FromJson<UIPositionContainer>(PlayerPrefs.GetString("UIPositionsMulti"));
        }
        isCustom = true;
    }
    void SetButtonsPos()
    {
        for (int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].position = pos.Positions[i];
            Buttons[i].localScale = pos.Scales[i];
        }
    }
    void Update()
    {
        try
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                if (Input.touchCount == 1 && currentButton > -1)
                {
                    Vector2 touch = Input.GetTouch(0).position;
                    pos.Positions[currentButton] = touch;
                    Buttons[currentButton].position = touch;
                    if (IsNotOverLapping(currentButton))
                    {
                        lastSafePosition = Buttons[currentButton].position;
                    }
                }
            }
            else
            {
                if (Input.GetMouseButton(0) && currentButton > -1)
                {
                    Vector2 touch = Input.mousePosition;
                    pos.Positions[currentButton] = touch;
                    Buttons[currentButton].position = touch;
                    if (IsNotOverLapping(currentButton))
                    {
                        lastSafePosition = Buttons[currentButton].position;
                    }
                }
            }
        }
        catch { }
    }
    void ResetBoundaries()
    {
        for (int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].transform.GetChild(0).gameObject.SetActive(false);
        }
        for (int i = 0; i < Corners.Length; i++)
        {
            Corners[i].transform.GetChild(0).gameObject.SetActive(false);
        }
        AddsContainer.GetChild(0).gameObject.SetActive(false);
    }
    bool IsNotOverLapping(int index)
    {
        Rect rec, rec1;
        Vector3[] corners = new Vector3[4];
        Buttons[index].GetWorldCorners(corners);
        rec = new Rect(corners[0].x, corners[0].y, corners[2].x - corners[0].x, corners[2].y - corners[0].y);
        for (int i = 0; i < Buttons.Length; i++)
        {
            if (i != index)
            {
                Buttons[i].GetWorldCorners(corners);
                rec1 = new Rect(corners[0].x, corners[0].y, corners[2].x - corners[0].x, corners[2].y - corners[0].y);

                if (rec.Overlaps(rec1))
                {
                    Buttons[i].GetChild(0).gameObject.SetActive(true);
                    return false;
                }
                else
                {
                    Buttons[i].GetChild(0).gameObject.SetActive(false);
                }
            }
        }
        for (int i = 0; i < Corners.Length; i++)
        {

            Corners[i].GetWorldCorners(corners);
            rec1 = new Rect(corners[0].x, corners[0].y, corners[2].x - corners[0].x, corners[2].y - corners[0].y);

            if (rec.Overlaps(rec1))
            {
                Corners[i].GetChild(0).gameObject.SetActive(true);
                return false;
            }
            else
            {
                Corners[i].GetChild(0).gameObject.SetActive(false);
            }
        }
        AddsContainer.GetWorldCorners(corners);
        rec1 = new Rect(corners[0].x, corners[0].y, corners[2].x - corners[0].x, corners[2].y - corners[0].y);

        if (rec.Overlaps(rec1))
        {
            AddsContainer.GetChild(0).gameObject.SetActive(true);
            return false;
        }
        else
        {
            AddsContainer.GetChild(0).gameObject.SetActive(false);
        }
        if (Buttons[index].position.x < Corners[3].position.x)
            return false;
        else if (Buttons[index].position.x > Corners[1].position.x)
            return false;
        else if (Buttons[index].position.y > Corners[0].position.y)
            return false;
        else if (Buttons[index].position.y < Corners[2].position.y)
            return false;
        return true;
    }
    public void ResetButton(int i)
    {
        if (currentButton == i)
        {
            ResetBoundaries();
            Buttons[currentButton].position = lastSafePosition;
            Buttons[currentButton].localScale = new Vector3(lastSafeSize, lastSafeSize, lastSafeSize);
            pos.Positions[currentButton] = Buttons[currentButton].position;
            pos.Scales[currentButton] = Buttons[currentButton].localScale;
            currentButton = -1;
        }
    }
    public void SetButton(int i)
    {
        if (currentButton == -1)
        {
            lastButton = currentButton = i;
            sizeSlider.value = lastSafeSize = Buttons[currentButton].localScale.x;
            lastSafePosition = Buttons[currentButton].position;
        }
    }
    public Vector3 GetScale(int i)
    {
            return pos.Scales[i];
    }
    public Vector2 GetPosition(int i)
    {
        return pos.Positions[i];
    }
    public void Reset()
    {
        AudioManager.instance.NormalClick();
        if (!isMulti)
        {
            PlayerPrefs.SetString("UIPositions", PlayerPrefs.GetString("UIPositionsDefault"));
            pos = JsonUtility.FromJson<UIPositionContainer>(PlayerPrefs.GetString("UIPositions"));
        }
        else
        {
            PlayerPrefs.SetString("UIPositionsMulti", PlayerPrefs.GetString("UIPositionsDefaultMulti"));
            pos = JsonUtility.FromJson<UIPositionContainer>(PlayerPrefs.GetString("UIPositionsMulti"));
        }
        SetButtonsPos();
    }
    public void Save()
    {
        AudioManager.instance.NormalClick();
        PopUp.SetActive(false);
        PopUp.SetActive(true);
        if (!isMulti)
            PlayerPrefs.SetString("UIPositions", JsonUtility.ToJson(pos));
        else
            PlayerPrefs.SetString("UIPositionsMulti", JsonUtility.ToJson(pos));
    }
    public void Exit()
    {
        AudioManager.instance.NormalClick();
        pos = JsonUtility.FromJson<UIPositionContainer>(PlayerPrefs.GetString("UIPositions"));
        GameManager.Instance.ControlsReturn();
    }
    public void SetFirst()
    {
        if (!isMulti)
        {
            if (!PlayerPrefs.HasKey("UIPositions"))
            {
                pos = new UIPositionContainer(Buttons.Length);
                for (int i = 0; i < Buttons.Length; i++)
                {
                    pos.Positions[i] = Buttons[i].position;
                    pos.Scales[i] = new Vector3(0.7f, 0.7f, 0.7f);
                }
                PlayerPrefs.SetString("UIPositions", JsonUtility.ToJson(pos));
                pos = JsonUtility.FromJson<UIPositionContainer>(PlayerPrefs.GetString("UIPositions"));
                PlayerPrefs.SetString("UIPositionsDefault", JsonUtility.ToJson(pos));
                for (int i = 0; i < Buttons.Length; i++)
                {
                    Buttons[i].position = pos.Positions[i];
                }
                isCustom = true;
            }
            else
                SetButtonsPos();
        }
        else
        {
            if (!PlayerPrefs.HasKey("UIPositionsMulti"))
            {
                pos = new UIPositionContainer(Buttons.Length);
                for (int i = 0; i < Buttons.Length; i++)
                {
                    pos.Positions[i] = Buttons[i].position;
                    pos.Scales[i] = new Vector3(0.7f, 0.7f, 0.7f);
                }
                PlayerPrefs.SetString("UIPositionsMulti", JsonUtility.ToJson(pos));
                pos = JsonUtility.FromJson<UIPositionContainer>(PlayerPrefs.GetString("UIPositionsMulti"));
                PlayerPrefs.SetString("UIPositionsDefaultMulti", JsonUtility.ToJson(pos));
                for (int i = 0; i < Buttons.Length; i++)
                {
                    Buttons[i].position = pos.Positions[i];
                }
                isCustom = true;
            }
            else
                SetButtonsPos();
        }
    }
    public void ChangeSize(float val)
    {
        if (lastButton > -1)
        {
            pos.Scales[lastButton] = new Vector3(sizeSlider.value, sizeSlider.value, sizeSlider.value);
            Buttons[lastButton].localScale = pos.Scales[lastButton];
            if (IsNotOverLapping(lastButton))
            {
                lastSafeSize = Buttons[lastButton].localScale.x;
            }
        }
    }
    public void ResetScale()
    {
        if (lastButton > -1)
        {
            Buttons[lastButton].localScale = new Vector3(lastSafeSize, lastSafeSize, lastSafeSize);
            pos.Scales[lastButton] = Buttons[lastButton].localScale;
        }
    }
}
