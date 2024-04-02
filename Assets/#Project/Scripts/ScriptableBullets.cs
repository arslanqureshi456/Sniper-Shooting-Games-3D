using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "ScriptableObjects/Bullets")]
public class ScriptableBullets : ScriptableObject
{
    public string bulletName = "bull 1";
    public float goldPrice = 10;
    public float spPrice = 0;
    public int PrefIndex = 0;
}
