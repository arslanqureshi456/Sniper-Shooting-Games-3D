using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletsGlide : MonoBehaviour
{
    const float MINANLGE = 0.01f, MAXANGLE = 0.03f, DIVIDER = 6.3f, LERPFACTOR = 0.001f;
    float angleX = 0,angleY = 0, angleZ = 0;
    RectTransform rect;
    Vector3 startPos;
    void Start()
    {
        angleX = Random.Range(0, 360);
        angleY = Random.Range(0, 360);
        angleZ = Random.Range(0, 360);
        //enabled = false;
        //Invoke("Delay", 2.5f);
        rect = GetComponent<RectTransform>();
    }
    void Update()
    {
        angleX += Random.Range(MINANLGE, MAXANGLE);
        angleY += Random.Range(MINANLGE, MAXANGLE);
        angleZ += Random.Range(MINANLGE, MAXANGLE);
        rect.anchoredPosition = new Vector3(Mathf.Sin(angleX) * DIVIDER,Mathf.Sin(angleY) * DIVIDER,Mathf.Sin(angleZ) * DIVIDER);
    }
    IEnumerator IncrementAngle()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(2.1f, 2.7f));
        }
    }
    void Delay()
    {
        startPos = transform.position;
        enabled = true;
        //StartCoroutine(IncrementAngle());
    }
}
