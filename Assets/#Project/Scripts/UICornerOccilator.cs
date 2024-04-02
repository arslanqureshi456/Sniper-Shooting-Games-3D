using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICornerOccilator : MonoBehaviour
{
    int Movement = 1;
    float SPEED = 0.035f;
    float target = 0,lerpAmount,start = 0,SizeAmount = 0;
    Rect rec;
    RectTransform trans;
    public int Direction = 0;
    public float Intensity = 1;
    Image Img;
    void Start()
    {
        Img = GetComponent<Image>();
        rec = transform.parent.GetComponent<RectTransform>().rect;
        trans = GetComponent<RectTransform>();
        enabled = false;
        trans.localScale = new Vector2(0, 0);
        Invoke("Delay", Random.Range(0.5f,0.1f));
    }
    void Delay()
    {
        Intensity *= 0.085f;
        if(Direction == 0)
        {
            trans.localPosition = new Vector2(rec.xMin, rec.yMax);
            trans.localScale = new Vector2(0.15f, 0.15f);
            start = rec.yMax;
            target = rec.yMin;
        }
        else
        {
            trans.localPosition = new Vector2(rec.xMax, rec.yMin);
            trans.localScale = new Vector2(0.15f, 0.15f);
            start = rec.yMin;
            target = rec.yMax;
        }
        //if (Random.Range(0, 2) > 0)
        //    Movement = 1;
        //else
        //    Movement = -1;

        enabled = true;
    }
    void FixedUpdate()
    {
        lerpAmount += SPEED;
        SizeAmount = 0.55f - Mathf.Abs(lerpAmount - 0.5f);
        Img.color = new Color(Img.color.r, Img.color.g, Img.color.b, SizeAmount + Intensity);
        switch(Mathf.Abs(Direction) % 4){
            case 0:
                trans.localPosition = new Vector2(trans.localPosition.x, Mathf.Lerp(start, target, lerpAmount));
                trans.localScale = new Vector2(0.015f, SizeAmount);
                if (lerpAmount >= 1)
                {
                    lerpAmount = 0;
                    Direction += Movement;
                    start = rec.xMin;
                    target = rec.xMax;
                }
                break;
            case 1:
                trans.localPosition = new Vector2(Mathf.Lerp(start, target, lerpAmount), trans.localPosition.y);
                trans.localScale = new Vector2(SizeAmount,0.012f);
                if (lerpAmount >= 1)
                {
                    lerpAmount = 0;
                    Direction += Movement;
                    start = rec.yMin;
                    target = rec.yMax;
                }
                break;
            case 2:
                trans.localPosition = new Vector2(trans.localPosition.x, Mathf.Lerp(start, target, lerpAmount));
                trans.localScale = new Vector2(0.015f,SizeAmount);
                if (lerpAmount >= 1)
                {
                    lerpAmount = 0;
                    Direction += Movement;
                    start = rec.xMax;
                    target = rec.xMin;
                }
                break;
            case 3:
                trans.localPosition = new Vector2(Mathf.Lerp(start, target, lerpAmount), trans.localPosition.y);
                trans.localScale = new Vector2(SizeAmount,0.012f);
                if (lerpAmount >= 1)
                {
                    lerpAmount = 0;
                    Direction += Movement;
                    start = rec.yMax;
                    target = rec.yMin;
                }
                break;
        }
    }

    private void OnDisable()
    {
        CancelInvoke("Delay");
    }
}
