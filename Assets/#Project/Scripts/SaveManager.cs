using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    public SaveState state;

    public int Session; // create this int for session ads

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        Load();
        
    }

    public void Save()
    {
        PlayerPrefs.SetString("save", Helper.Encrypt(Helper.Serialize<SaveState>(state)));
    }

    public void Load()
    {
        if(PlayerPrefs.HasKey("save"))
        {
            state = Helper.Deserialize<SaveState>(Helper.Decrypt(PlayerPrefs.GetString("save")));
        }
        else
        {
            state = new SaveState();
            Save();
        }
    }

    public bool isWeaponUnlocked(int index)
    {
        return (state.weaponUnlocked & (1 << index)) != 0;
    }

    public void UnlockWeapon(int index)
    {
        state.weaponUnlocked |= 1 << index;
    }

    public void ResetSave()
    {
        PlayerPrefs.DeleteKey("save");
    }
}