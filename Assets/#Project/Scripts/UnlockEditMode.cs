
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
public class UnlockEditMode : MonoBehaviour
{
    [MenuItem("Unlocker/UnlockAll")]
    static void UnlockAllModesAndLevels()
    {
        PlayerPrefs.SetInt("CoverStrikeCurrentLevel", 10);
        PlayerPrefs.SetInt("CoverStrikeNextLevel", 11);

        PlayerPrefs.SetInt("currentLevel", 80);
        PlayerPrefs.SetInt("nextLevel", 81);
        PlayerPrefs.SetInt("LevelCount", 80);

        PlayerPrefs.SetInt("sniperCurrentLevel", 16);
        PlayerPrefs.SetInt("sniperNextLevel", 17);

        for (int i=0;i<80;i++)
        {
            PlayerPrefs.SetInt("levelUnlocked-" + i, 1);
        }
        for (int i = 0; i < 16; i++)
        {
            PlayerPrefs.SetInt("sniperLevelUnlocked-" + i, 1);
        }
        for (int i = 0; i < 10; i++)
        {
            PlayerPrefs.SetInt("CoverStrikeLevelUnlocked-" + i, 1);
        }
    }
}
#endif
