﻿using UnityEngine;
using System.Collections.Generic;

public class bl_HudManager : MonoBehaviour
{

    /// <summary>
    /// All huds in scene
    /// </summary>
    [Tooltip("Hud list manager, you can add a new hud directly here.")]
    public List<bl_HudInfo> Huds = new List<bl_HudInfo>();
    /// <summary>
    /// Player transform
    /// </summary>

    [Tooltip("You can use MainCamera or the root of your player")]
    public Transform LocalPlayer = null;

    public float clampBorder = 12;
    public bool useGizmos = true;
    [Header("Global Settings")]
    [Range(1, 50)] public float IconSize = 50;
    [Range(1, 50)] public float OffScreenIconSize = 25;

    [Header("GUI Scaler")]
    [Tooltip("The resolution the UI layout is designed for. If the screen resolution is larger, the GUI will be scaled up, and if it's smaller, the GUI will be scaled down. This is done in accordance with the Screen Match Mode.")]
    public Vector2 m_ReferenceResolution = new Vector2(800f, 600f);
    [Range(0f, 1f), Tooltip("Determines if the scaling is using the width or height as reference, or a mix in between."), SerializeField]
    public float m_MatchWidthOrHeight;
    [Tooltip("Select Reference Resolution automatically in run time.")]
    public bool AutoScale = true;
    [Header("Style")]
    public GUIStyle TextStyle;

    float distance,temp;
    int[] active = new int[3];
    private static bl_HudManager _instance;
    public float showDelay = 9999999;
    //Get singleton
    public static bl_HudManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<bl_HudManager>();
            }
            return _instance;
        }
    }

    private void Start()
    {
        foreach(bl_HudInfo _huds in Huds)
        {
            if(_huds != null)
                _huds.Arrow.ArrowSize = Screen.width / 25;
        }
        Invoke("ForceDelay", 70);
    }

    /// <summary>
    /// 
    /// </summary>
    void OnDestroy()
    {
        _instance = null;
    }
    /// <summary>
    /// 
    /// </summary>
    void Update() 
    {
        if(showDelay > 0)
            showDelay -= Time.deltaTime;
        if (AutoScale)
        { 
            m_ReferenceResolution.x = Screen.width; m_ReferenceResolution.y = Screen.height;
        } 
    }

    /// <summary>
    /// Draw a icon for each in list
    /// </summary>
    void OnGUI()
    {
        if (bl_HudUtility.mCamera == null)
            return;
        if (LocalPlayer == null)
            return;
        switch (LevelSelectionNew.modeSelection)
        {
            case LevelSelectionNew.modeType.ASSAULT:
            case LevelSelectionNew.modeType.SNIPER:
                for (int i = 0; i < Huds.Count; i++)
                {
                    if (!Huds[i].Hide)
                    {
                        if (Huds[i].HideOnCloseDistance > 0 && GetHudDistance(i) < Huds[i].HideOnCloseDistance) { continue; }
                        if (Huds[i].HideOnLargeDistance > 0 && GetHudDistance(i) > Huds[i].HideOnLargeDistance) { continue; }
                        OnScreen(i);
                        OffScreen(i);
                    }
                }
                break;
            case LevelSelectionNew.modeType.FREEFORALL:
                if (showDelay > 0)
                    return;
                if (Huds.Count <= 0)
                    return;
                for (int i = 0; i < 3; i++)
                {
                    if (active[i] >= Huds.Count)
                        continue;
                    if ((Huds[active[i]].HideOnCloseDistance > 0 && distance < Huds[active[i]].HideOnCloseDistance)) { return; }
                    //OnScreen(active[i]);
                    OffScreen(active[i]);
                }
                break;
            default:
                enabled = false;
                break;
        }
        
        
    }
    public void ForceDelay()
    {
        showDelay = 15;//40
        for(int i=0;i<3;i++)
        {
            active[i] = Random.Range(0, Huds.Count);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="i"></param>
    void OnScreen(int i)
    {
        //if transform destroy, then remove from list
        if (Huds[i].m_Target == null)
        {
            Huds.Remove(Huds[i]);
            return;
        }

        //Check target if OnScreen
        if (bl_HudUtility.isOnScreen(bl_HudUtility.ScreenPosition(Huds[i].m_Target), Huds[i].m_Target))
        {
            //Calculate Position of target
            Vector3 RelativePosition = Huds[i].m_Target.position + Huds[i].Offset;
            if ((Vector3.Dot(this.LocalPlayer.forward, RelativePosition - this.LocalPlayer.position) > 0f))
            {
                //Calculate the 2D position of the position where the icon should be drawn
                Vector3 point = bl_HudUtility.mCamera.WorldToViewportPoint(RelativePosition);

                //The viewportPoint coordinates are between 0 and 1, so we have to convert them into screen space here
                Vector2 drawPosition = new Vector2(point.x * Screen.width, Screen.height * (1 - point.y));

                if (!Huds[i].Arrow.ShowArrow)
                {
                    //Clamp the position to the edge of the screen in case the icon would be drawn outside the screen
                    drawPosition.x = Mathf.Clamp(drawPosition.x, clampBorder, Screen.width - clampBorder);
                    drawPosition.y = Mathf.Clamp(drawPosition.y, clampBorder, Screen.height - clampBorder);
                }
                //Calculate distance from player to way point
                float Distance = Vector3.Distance(this.LocalPlayer.position, RelativePosition);
                //Cache distance
                float CompleteDistance = Distance;

                //Max Hud Increment 
                if (Distance > Huds[i].m_MaxSize) // if more than "50" no increase more
                {
                    Distance = 50;
                }
                float n = IconSize;
                //Calculate depend of type 
                if (Huds[i].m_TypeHud == TypeHud.Decreasing)
                {
                    n = (((50 + Distance) / (25)) * 0.9f) + 0.1f;
                }
                else if (Huds[i].m_TypeHud == TypeHud.Increasing)
                {
                    n = (((50 - Distance) / (25)) * 0.9f) + 0.1f;
                }
                //Calculate Size of Hud
                float sizeX = 0, sizeY = 0;
                if (Huds[i].m_Icon)
                {
                     sizeX = Huds[i].m_Icon.width * n;
                    if (sizeX >= Huds[i].m_MaxSize)
                    {
                        sizeX = Huds[i].m_MaxSize;
                    }
                     sizeY = Huds[i].m_Icon.height * n;
                    if (sizeY >= Huds[i].m_MaxSize)
                    {
                        sizeY = Huds[i].m_MaxSize;
                    }
                }
                
                float TextUperIcon = sizeY / 2 + 5;

                //palpating effect
                if (Huds[i].isPalpitin)
                {
                    Palpating(Huds[i]);
                }

                //Draw Huds
                GUI.color = Huds[i].m_Color;
                if(Huds[i].m_Icon)
                    GUI.DrawTexture(new Rect(drawPosition.x - (sizeX / 2), drawPosition.y - (sizeY / 2), sizeX, sizeY), Huds[i].m_Icon);
                if (!Huds[i].ShowDistance)
                {
                    if (!string.IsNullOrEmpty(Huds[i].m_Text))
                    {
                        Vector2 size = TextStyle.CalcSize(new GUIContent(Huds[i].m_Text));
                        GUI.Label(new Rect(drawPosition.x - (size.x / 2) + 10, (drawPosition.y - (size.y / 2)) - TextUperIcon, size.x, size.y), Huds[i].m_Text, TextStyle);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(Huds[i].m_Text))
                    {
                        string text = Huds[i].m_Text + "\n<color=whitte>[" + string.Format("{0:N0}m", CompleteDistance) + "]</color>";
                        Vector2 size = TextStyle.CalcSize(new GUIContent(text));
                        GUI.Label(new Rect(drawPosition.x - (size.x / 2) + 10, (drawPosition.y - (size.y / 2)) - TextUperIcon, size.x, size.y), text, TextStyle);
                    }
                    else
                    {
                        string text = "<color=whitte>[" + string.Format("{0:N0}m", CompleteDistance) + "]</color>";
                        Vector2 size = TextStyle.CalcSize(new GUIContent(text));
                        GUI.Label(new Rect(drawPosition.x - (size.x / 2) + 10, ((drawPosition.y - (size.y / 2)) - TextUperIcon), size.x, size.y), text, TextStyle);
                    }
                }
            }
        }
    }

    /// <summary>
    /// When the target if not in Player sight 
    /// </summary>
    /// <param name="i"></param>
    void OffScreen(int i)
    {
        //if transform destroy, then remove from list
        if (Huds[i].m_Target == null)
        {
            Huds.Remove(Huds[i]);
            return;
        }

        if (Huds[i].Arrow.ArrowIcon != null && Huds[i].Arrow.ShowArrow)
        {

            //Check target if OnScreen
            if (!bl_HudUtility.isOnScreen(bl_HudUtility.ScreenPosition(Huds[i].m_Target), Huds[i].m_Target))
            {
                //Get the relative position of arrow
                Vector3 ArrowPosition = Huds[i].m_Target.position + Huds[i].Arrow.ArrowOffset;
                Vector3 pointArrow = bl_HudUtility.mCamera.WorldToScreenPoint(ArrowPosition);

                //pointArrow.x = pointArrow.x / bl_HudUtility.mCamera.pixelWidth;
                //pointArrow.y = pointArrow.y / bl_HudUtility.mCamera.pixelHeight;

                Vector3 mForward = Huds[i].m_Target.position - bl_HudUtility.mCamera.transform.position;
                Vector3 mDir = bl_HudUtility.mCamera.transform.InverseTransformDirection(mForward);
                mDir = mDir.normalized / 5;
                pointArrow.x = 0.5f + mDir.x * 20f / bl_HudUtility.mCamera.aspect;
                pointArrow.y = 0.5f + mDir.y * 20f;

                //if (pointArrow.z < 0)
                //{
                //    pointArrow *= -1f;
                //    pointArrow *= -1f;
                //}
                //Arrow
                GUI.color = Huds[i].m_Color;

                float Xpos = bl_HudUtility.mCamera.pixelWidth * pointArrow.x;
                float Ypos = bl_HudUtility.mCamera.pixelHeight * (1f - pointArrow.y);

                //palpating effect
                if (Huds[i].isPalpitin)
                {
                    Palpating(Huds[i]);
                }

                //Calculate area to rotate guis
                float mRot = bl_HudUtility.GetRotation(bl_HudUtility.mCamera.pixelWidth / (2), bl_HudUtility.mCamera.pixelHeight / (2), Xpos, Ypos);
                //Get pivot from area
                Vector2 mPivot = bl_HudUtility.GetPivot(0, 0, Screen.width);
                //Arrow
                Matrix4x4 matrix = GUI.matrix;
                GUIUtility.RotateAroundPivot(mRot, mPivot);
                if(Huds[i].Arrow != null)
                    GUI.DrawTexture(new Rect(mPivot.x - bl_HudUtility.HalfSize(Huds[i].Arrow.ArrowSize - Screen.width / 4), mPivot.y - bl_HudUtility.HalfSize(Huds[i].Arrow.ArrowSize), Huds[i].Arrow.ArrowSize, Huds[i].Arrow.ArrowSize), Huds[i].Arrow.ArrowIcon);
                GUI.matrix = matrix;

                float ClampedX = Mathf.Clamp(mPivot.x, 20, (Screen.width - OffScreenIconSize) - 20);
                float ClampedY = Mathf.Clamp(mPivot.y, 20, (Screen.height - OffScreenIconSize) - 20);
                if(Huds[i].m_Icon && LevelSelectionNew.modeSelection != LevelSelectionNew.modeType.FREEFORALL &&
                    LevelSelectionNew.modeSelection != LevelSelectionNew.modeType.BR)
                    GUI.DrawTexture(bl_HudUtility.ScalerRect(new Rect(ClampedX, ClampedY, OffScreenIconSize, OffScreenIconSize)), Huds[i].m_Icon);

                Vector2 ClampedTextPosition = mPivot;
                //Icons and Text
                if (!Huds[i].ShowDistance)
                {
                    if (!string.IsNullOrEmpty(Huds[i].m_Text))
                    {
                        Vector2 size = TextStyle.CalcSize(new GUIContent(Huds[i].m_Text));
                        ClampedTextPosition.x = Mathf.Clamp(ClampedTextPosition.x, (size.x + OffScreenIconSize) + 30, ((Screen.width - OffScreenIconSize) - 10) - size.x);
                        ClampedTextPosition.y = Mathf.Clamp(ClampedTextPosition.y, (size.y + OffScreenIconSize) + 35, ((Screen.height - size.y) - OffScreenIconSize) - 20);
                        GUI.Label(bl_HudUtility.ScalerRect(new Rect(ClampedTextPosition.x - (size.x / 2), ClampedTextPosition.y - (size.y / 2), size.x, size.y)), Huds[i].m_Text, TextStyle);
                    }
                }
                else
                {
                    float Distance = Vector3.Distance(LocalPlayer.position, Huds[i].m_Target.position);
                    if (!string.IsNullOrEmpty(Huds[i].m_Text))
                    {
                        string text = Huds[i].m_Text + "\n <color=whitte>[" + string.Format("{0:N0}m", Distance) + "]</color>";
                        Vector2 size = TextStyle.CalcSize(new GUIContent(text));
                        ClampedTextPosition.x = Mathf.Clamp(ClampedTextPosition.x, (size.x + OffScreenIconSize) + 30, ((Screen.width - OffScreenIconSize) - 10) - size.x);
                        ClampedTextPosition.y = Mathf.Clamp(ClampedTextPosition.y, (size.y + OffScreenIconSize) + 35, ((Screen.height - size.y) - OffScreenIconSize) - 20);
                        GUI.Label(bl_HudUtility.ScalerRect(new Rect(ClampedTextPosition.x - (size.x / 2), (ClampedTextPosition.y - (size.y / 2)), size.x, size.y)), text, TextStyle);
                    }
                    else
                    {
                        string text = "<color=whitte>[" + string.Format("{0:N0}m", Distance) + "]</color>";
                        Vector2 size = TextStyle.CalcSize(new GUIContent(text));
                        ClampedTextPosition.x = Mathf.Clamp(ClampedTextPosition.x, (size.x + OffScreenIconSize) + 30, ((Screen.width - OffScreenIconSize) - 10) - size.x);
                        ClampedTextPosition.y = Mathf.Clamp(ClampedTextPosition.y, (size.y + OffScreenIconSize) + 35, ((Screen.height - size.y) - OffScreenIconSize) - 20);
                        GUI.Label(bl_HudUtility.ScalerRect(new Rect(ClampedTextPosition.x - (size.x / 2), (ClampedTextPosition.y - (size.y / 2)), size.x, size.y)), text, TextStyle);
                    }
                }
                // GUI.DrawTexture(bl_HudUtility.ScalerRect(new Rect(mPivot.x + marge.x,(mPivot.y + ((!Huds[i].ShowDistance) ? 10 : 20)) + marge.y, 25, 25)), Huds[i].m_Icon);
            }
            GUI.color = Color.white;
        }
    }

    //Add a new Huds from instance to the list
    public void CreateHud(bl_HudInfo info)
    {
        Huds.Add(info);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="i"></param>
    public void RemoveHud(int i)
    {
        Huds.RemoveAt(i);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="hud"></param>
    public void RemoveHud(bl_HudInfo hud)
    {
        if (Huds.Contains(hud))
        {
            Huds.Remove(hud);
        }
        else
        {
#if UNITY_EDITOR
            Debug.Log("Huds list don't contain this hud!");
#endif
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="i">Id of hud in list</param>
    public void HideStateHud(int i, bool hide = false)
    {
        if (Huds[i] != null)
        {
            Huds[i].Hide = hide;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="i">Id of hud in list</param>
    public void HideStateHud(bl_HudInfo hud, bool hide = false)
    {
        if (Huds.Contains(hud))
        {
            for (int i = 0; i < Huds.Count; i++)
            {
                if (Huds[i] == hud)
                {
                    Huds[i].Hide = hide;
                }
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private float GetHudDistance(int i)
    {
        //if transform destroy, then remove from list
        if (Huds[i] == null || Huds[i].m_Target == null)
        {
            Huds.Remove(Huds[i]);
            return 0;
        }
        //Calculate Position of target
        Vector3 RelativePosition = Huds[i].m_Target.position + Huds[i].Offset;
        float Distance = Vector3.Distance(this.LocalPlayer.position, RelativePosition);
        return Distance;
    }

    private void Palpating(bl_HudInfo hud)
    {
        if (hud.m_Color.a <= 0)
        {
            hud.tip = false;
        }
        else if (hud.m_Color.a >= 1)
        {
            hud.tip = true;
        }
        //Create a loop
        if (hud.tip == false)
        {
            hud.m_Color.a += Time.deltaTime * 0.5f;
        }
        else
        {
            hud.m_Color.a -= Time.deltaTime * 0.5f;
        }
    }
    /// <summary>
    /// Visual Debug of all way points in scene
    /// </summary>
    void OnDrawGizmosSelected()
    {
        if (!useGizmos)
            return;

        for (int i = 0; i < Huds.Count; i++)
        {
            if (Huds[i].m_Target != null)
            {
                Gizmos.color = new Color(0, 0.35f, 0.9f, 0.9f);
                Gizmos.DrawWireSphere(Huds[i].m_Target.position, 3);
                Gizmos.color = new Color(0, 0.35f, 0.9f, 0.3f);
                Gizmos.DrawSphere(Huds[i].m_Target.position, 3);
                if (i < Huds.Count - 1)
                {
                    Gizmos.DrawLine(Huds[i].m_Target.position, Huds[i + 1].m_Target.position);
                }
                else
                {
                    Gizmos.DrawLine(Huds[i].m_Target.position, Huds[0].m_Target.position);
                }
            }
        }
    }

}