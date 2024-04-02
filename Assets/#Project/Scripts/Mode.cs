using UnityEngine;

[CreateAssetMenu(fileName = "New Mode", menuName = "ScriptableObjects/Mode")]
public class Mode : ScriptableObject
{
    [Space(5)]
    public int index;
    [Space(5)]
    [Header("Currency")]
    public int goldPrice;
    public int spPrice;
    [Space(5)]
    [Header("In App")]
    public string discountedPriceID;
    public string realPriceID;
    [Space(5)]
    [Header("Video AD")]
    public int adCount;
}
