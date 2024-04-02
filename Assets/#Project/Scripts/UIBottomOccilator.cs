using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBottomOccilator : MonoBehaviour
{
    float SPEED = 0.012f;
    float target = 0, lerpAmount, start = 0, SizeAmount = 0;
    Rect rec;
    RectTransform trans;
    bool Direction = true;
    void Start()
    {
        enabled = false;
        Invoke("Delay", 0.02f);
    }
    void Delay()
    {
        rec = transform.parent.GetComponent<RectTransform>().rect;
        trans = GetComponent<RectTransform>();
        trans.localPosition = new Vector2(rec.xMin, rec.yMin);
        //trans.localScale = new Vector2(1, 0.05f);
        Reset();
        start = rec.xMin;
        target = rec.xMax;
        enabled = true;
    }
    private void Reset()
    {
        Direction = true;
        lerpAmount = 0;
        trans.offsetMax = new Vector2(rec.xMin, trans.offsetMax.y);
        trans.offsetMin = new Vector2(rec.xMin, trans.offsetMin.y);
        enabled = false;
        Invoke("DelayedEnable", 1.1f);
    }
    void DelayedEnable()
    {
        enabled = true;
    }
    void Update()
    {
        if(rec != null && trans != null)
        {
            lerpAmount += SPEED;
            SizeAmount = 0.75f * (Mathf.Abs(lerpAmount - 0.5f) / 0.5f);
            if (Direction)
            {
                trans.offsetMax = new Vector2(Mathf.Lerp(rec.xMin, target, lerpAmount), trans.offsetMax.y);
                if (lerpAmount >= 1)
                {
                    Direction = false;
                    lerpAmount = 0;
                }
            }
            else
            {
                trans.offsetMin = new Vector2(Mathf.Lerp(rec.xMin, target, lerpAmount), trans.offsetMin.y);
                if (lerpAmount >= 1)
                {
                    Reset();
                }
            }
        }
    }

    private void OnDisable()
    {
        CancelInvoke("Delay");
        CancelInvoke("DelayedEnable");
    }
}
