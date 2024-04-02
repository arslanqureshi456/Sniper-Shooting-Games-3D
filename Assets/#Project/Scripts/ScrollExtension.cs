using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollExtension : MonoBehaviour
{
    [Range(1, 10)]
    public float delay;
    [Range(0, 1)]
    public float from;
    [Range(0, 1)]
    public float to;
    public enum Direction
    {
        Horizontal,
        Vertical
    }
    public Direction direction;

    private Coroutine coroutine;

    private void Start()
    {
        ScrollRect scrollRect = GetComponent<ScrollRect>();
        if (scrollRect != null)
        {
            if (coroutine != null) 
                StopCoroutine(coroutine);
            coroutine = StartCoroutine(AnimateOverTime(delay, scrollRect));
        }
    }

    IEnumerator AnimateOverTime(float seconds, ScrollRect sr)
    {
        float animationTime = 0f;
        while (animationTime < seconds)
        {
            animationTime += Time.deltaTime;
            float lerpValue = animationTime / seconds;
            if(direction == Direction.Vertical)
                sr.verticalNormalizedPosition = Mathf.Lerp(from, to, lerpValue);
            else
                sr.horizontalNormalizedPosition = Mathf.Lerp(from, to, lerpValue);
            yield return null;
        }
    }

    void OnDisable()
    {
        StopCoroutine("AnimateOverTime");
    }
}
