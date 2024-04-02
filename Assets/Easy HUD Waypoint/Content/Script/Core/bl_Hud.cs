using UnityEngine;

public class bl_Hud : MonoBehaviour {

    /// <summary>
    /// 
    /// </summary>
    public bl_HudInfo HudInfo;

    /// <summary>
    /// Instantiate a new Hud
    /// add hud to hud manager in start
    /// </summary>
    void Start()
    {
        if (bl_HudManager.instance != null)
        {
            if (HudInfo.m_Target == null)
            {
                HudInfo.m_Target = transform;
            }
            if (HudInfo.m_Target == null) { HudInfo.m_Target = this.GetComponent<Transform>(); }
            if (HudInfo.ShowDynamically) { HudInfo.Hide = true; }
            //if (LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.FREEFORALL ||
            //    LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.BR)
            //{
            //    HudInfo.m_Icon = null;
            //}
            bl_HudManager.instance.CreateHud(this.HudInfo);
        }
        else
        {
#if UNITY_EDITOR
            Debug.LogError("Need have a Hud Manager in scene");
#endif
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public void Show()
    {
        if (bl_HudManager.instance != null)
        {
            bl_HudManager.instance.HideStateHud(HudInfo, false);
        }
        else
        {
#if UNITY_EDITOR
            Debug.LogWarning("the instance of bl_HudManager in scene wasn't found.");
#endif
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public void Hide()
    {
        if (bl_HudManager.instance != null)
        {
            bl_HudManager.instance.HideStateHud(HudInfo, true);
        }
        else
        {
#if UNITY_EDITOR
            Debug.LogWarning("the instance of bl_HudManager in scene wasn't found.");
#endif
        }
    }

    private void OnDestroy()
    {
        Hide();
    }
}