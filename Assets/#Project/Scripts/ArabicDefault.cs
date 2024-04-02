using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ArabicSupport;

public class ArabicDefault : MonoBehaviour
{
    public string Text;
    void OnEnable()
    {
        GetComponent<Text>().text = ArabicFixer.Fix(Text);
    }
}
