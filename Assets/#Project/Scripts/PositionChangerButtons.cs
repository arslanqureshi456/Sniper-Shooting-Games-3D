using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PositionChangerButtons : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public int number;
    public void OnPointerDown(PointerEventData data)
    {
        UiPositionChanger.Instance.SetButton(number);
    }
    public void OnPointerUp(PointerEventData data)
    {
        UiPositionChanger.Instance.ResetButton(number);
    }
}
