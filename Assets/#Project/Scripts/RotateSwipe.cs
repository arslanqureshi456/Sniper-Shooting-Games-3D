using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RotateSwipe : MonoBehaviour
{
    public float manualRotateSpeed = 5f;
    public float rotateSpeed = 0.5f;
    public bool onlyManualRotation = true;
    public bool x_Axis, y_Axis, z_Axis;

    public GameObject[] UIElements;

    private float xMin, yMin, xMax, yMax;
    private Touch touch;
    private Rect _rect;
    private Quaternion _rotation;
    public Vector3 defaultScale;
    private bool isScaled = false;
    Vector3 lastPos = Vector3.zero;
    float val;
    Vector3 startRot = Vector3.one;
    private void OnEnable()
    {
        startRot = transform.eulerAngles;
        xMin = Screen.width / 6.5f;
        yMin = Screen.height / 5;
        xMax = Screen.width / 1.5f;
        yMax = Screen.height / 2;
        _rect = new Rect(xMin, yMin, xMax, yMax);
        //transform.localScale = transform.localScale * 0.7f;
        defaultScale = this.transform.localScale;
    }
    private void OnDisable()
    {
        transform.eulerAngles = startRot;
    }
    void LateUpdate()
    {
        if (Input.touchCount > 0 && !onlyManualRotation)
        {
            touch = Input.GetTouch(0);
            if(_rect.Contains(touch.position))
            {
                if (touch.phase == TouchPhase.Moved)
                {
                    val = touch.deltaPosition.x * rotateSpeed;
                    Rotate(val);
                    HideUIElements();
                }
            }
        }
        else if (Input.GetMouseButton(0) && !onlyManualRotation)
        {
            if (_rect.Contains(Input.mousePosition))
            {
                if ((Input.mousePosition - lastPos).magnitude > 0.0015f)
                {
                    val = (-(Input.mousePosition - lastPos).sqrMagnitude * rotateSpeed);
                    Rotate(val);
                    lastPos = Input.mousePosition;
                    HideUIElements();
                }
            }
        }
        else
        {
            if (x_Axis)
                transform.Rotate(manualRotateSpeed * Time.deltaTime, 0f, 0f);
            else if(y_Axis)
                transform.Rotate(0f, manualRotateSpeed * Time.deltaTime, 0f);
            else if(z_Axis)
                transform.Rotate(0f, 0f, manualRotateSpeed * Time.deltaTime);

            ShowUIElements();
        }
    }
    void Rotate(float val)
    {
        if (x_Axis)
            _rotation = Quaternion.Euler(transform.eulerAngles.x - val, transform.eulerAngles.y, transform.eulerAngles.z);
        else if (y_Axis)
            _rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y - val, transform.eulerAngles.z);
        else if (z_Axis)
            _rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z -val);

        transform.rotation = _rotation;
    }
    private void ShowUIElements()
    {
        if (UIElements.Equals(null))
            return;

        foreach (GameObject go in UIElements)
        {
            go.transform.localScale = Vector3.one;
            //go.SetActive(true);
        }

        isScaled = false;
        this.transform.localScale = defaultScale;
    }

    private void HideUIElements()
    {
        if (UIElements.Equals(null))
            return;

        foreach (GameObject go in UIElements)
        {
            go.transform.localScale = Vector3.zero;
            //go.SetActive(false);
        }

        if(!isScaled)
        {
            isScaled = true;
            this.transform.localScale *= 1.06f;
        }
    }
}