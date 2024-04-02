using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class BulletSpecificationTouch : MonoBehaviour
{
    bool began = true;
    float currentXRotation, currentYRotation;
    Vector2 startTouchPos;
    void OnEnable()
    {
        transform.localEulerAngles = new Vector3(0, 0, 3.918f);
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.touchCount > 0 && !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
        //{
        //    if (began)
        //    {
        //        began = false;
        //        startTouchPos = Input.GetTouch(0).position;
        //    }
        //    else if ((Input.GetTouch(0).position - startTouchPos).sqrMagnitude > 0.001f)
        //    {
        //        currentXRotation += (Input.GetTouch(0).position.x - startTouchPos.x) * 0.005f;
        //        currentYRotation += (Input.GetTouch(0).position.y - startTouchPos.y) * 0.005f;
        //        currentYRotation = Mathf.Clamp(currentYRotation, -9, 9);
        //        currentXRotation = Mathf.Clamp(currentXRotation, -6, 13);
        //        startTouchPos = Input.GetTouch(0).position;
        //        RotateCamera();
        //    }
        //}
        //else
        //{
        //    if (!began)
        //    {
        //        began = true;

        //    }
        //}

        if (Input.GetMouseButton(0))
        {
            if (began)
            {
                //Debug.Log(" in begin");
                began = false;
                startTouchPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            }
            else if ((new Vector2(Input.mousePosition.x, Input.mousePosition.y) - startTouchPos).sqrMagnitude > 0.001f)
            {
                currentXRotation += (Input.mousePosition.x - startTouchPos.x) * 0.1f;
                currentYRotation += (Input.mousePosition.y - startTouchPos.y) * 0.1f;
                startTouchPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                RotateCamera();
            }
        }
        else
        {
            if (!began)
            {
                began = true;

            }
        }
    }
    void RotateCamera()
    {
        //Debug.Log(" rotating ");
        transform.localEulerAngles = new Vector3(0, currentXRotation, currentYRotation);// rotation(0, 0, 0);
    }
}
