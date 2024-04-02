using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteSwap : MonoBehaviour
{
    public float Delay;
    public Sprite[] sprites;
    int index;
    Image img;
    void OnEnable()
    {
        img = GetComponent<Image>();
        StartCoroutine(SwapRoutine());
    }

    IEnumerator SwapRoutine()
    {
        while(true)
        {
            index++;
            img.sprite = sprites[index % sprites.Length];
            yield return new WaitForSeconds(Delay);
        }
    }

    void OnDisable()
    {
        StopCoroutine("SwapRoutine");
    }
}
