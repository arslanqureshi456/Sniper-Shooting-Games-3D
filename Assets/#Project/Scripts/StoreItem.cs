using UnityEngine;
using UnityEngine.UI;

public class StoreItem : MonoBehaviour
{
    float TargetX = 0;
    RectTransform rect;
    RectTransform ParentRect = null;
    float tempDiff = 0;
    Transform Gun;
    public bool isCard = false;
    void Start()
    {
        Invoke("Delay", 0.1f);
        enabled = false;
    }
    private void Update()
    {
        if(ParentRect != null)
        {
            tempDiff = TargetX + ParentRect.anchoredPosition.x;
            rect.localPosition = new Vector3(rect.localPosition.x, 0, Mathf.Lerp(150, -35, 1 - Mathf.Clamp(Mathf.Abs(tempDiff) / 1050, 0, 1)));
            if (!isCard)
            {
                if (tempDiff > 0)
                    Gun.localEulerAngles = new Vector3(Gun.localEulerAngles.x, Mathf.Lerp(50, 90, 1 - Mathf.Clamp(Mathf.Abs(tempDiff) / 1500, 0, 1)), Gun.localEulerAngles.z);
                else
                    Gun.localEulerAngles = new Vector3(Gun.localEulerAngles.x, Mathf.Lerp(130, 90, 1 - Mathf.Clamp(Mathf.Abs(tempDiff) / 1500, 0, 1)), Gun.localEulerAngles.z);
            }
            else
            {
                if (tempDiff > 0)
                    Gun.localEulerAngles = new Vector3(Gun.localEulerAngles.x, Mathf.Lerp(-30, 0, 1 - Mathf.Clamp(Mathf.Abs(tempDiff) / 1500, 0, 1)), Gun.localEulerAngles.z);
                else
                    Gun.localEulerAngles = new Vector3(Gun.localEulerAngles.x, Mathf.Lerp(30, 0, 1 - Mathf.Clamp(Mathf.Abs(tempDiff) / 1500, 0, 1)), Gun.localEulerAngles.z);
            }
        }
    }
    void Delay()
    {
        rect = GetComponent<RectTransform>();
        ParentRect = transform.parent.GetComponent<RectTransform>();
        GridLayoutGroup g = transform.parent.GetComponent<GridLayoutGroup>();
        TargetX = transform.GetSiblingIndex() * (g.cellSize.x + g.spacing.x);
        Gun = transform.GetChild(0);
        enabled = true;
    }

    private void OnDisable()
    {
        CancelInvoke("Delay");
    }
}
