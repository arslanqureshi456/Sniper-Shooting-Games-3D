//#define CLOSE_AFTERICON
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine.SceneManagement;
//namespace EModules
#if PROJECT
    using EModules.Project;
#endif

namespace EModules.EModulesInternal {

public partial class M_Colors_Window : _W___IWindow {


    /*   internal static Rect DrawColorAligmentSettingsLine(Rect lineRect, Adapter adapter)
        {   Adapter. LABEL(lineRect, "<i>Default Background Color Aligment:</i>");
            lineRect.y += lineRect.height;
            var rect = lineRect;
            var WW = lineRect.width;
            rect.width = WW * 0.15f;
            Label(rect, "LEFT:");
            rect.x += rect.width;
            rect.width = WW * 0.35f;
            var oldLeftAligment = adapter.BgAligmentToLeft(adapter._S_bgColorDefaultAligment);
            var newLeftAligment = (Adapter.BgAligmentLeft)EditorGUI.Popup(rect, (int)oldLeftAligment, Enum.GetNames(typeof(Adapter.BgAligmentLeft)));
            rect.x += rect.width;
            rect.width = WW * 0.15f;
            Label(rect, "RIGHT:");
            rect.x += rect.width;
            rect.width = WW * 0.35f;
            var oldRightAligment = adapter.BgAligmentToRight(adapter._S_bgColorDefaultAligment);
            var newRightAligment = (Adapter.BgAligmentRight)EditorGUI.Popup(rect, (int)oldRightAligment, Enum.GetNames(typeof(Adapter.BgAligmentRight)));
            if (oldLeftAligment != newLeftAligment || oldRightAligment != newRightAligment)
            {   adapter._S_bgColorDefaultAligment = adapter.BgLeftToAligment(adapter._S_bgColorDefaultAligment, newLeftAligment);
                adapter._S_bgColorDefaultAligment = adapter.BgRightToAligment(adapter._S_bgColorDefaultAligment, newRightAligment);
    
            }
            return lineRect;
        }*/
    internal static Rect DrawIconAligmentSettingsLine(Rect lineRect, Adapter adapter)
    {
    
    
        var nv =  adapter. TOOGLE_POP_INVERCE(ref lineRect, "Icons Placement", adapter._S_bgIconsPlace, "Next to 'Label'", "Next to 'Foldout'", "Align 'Left'");
        if (nv != adapter._S_bgIconsPlace)
        {   adapter._S_bgIconsPlace = nv;
            adapter.RepaintAllViews();
        }
        
        /*Adapter. LABEL(lineRect, "<i>Icons Placement:</i>");
        lineRect.y += lineRect.height;
        var new_S_bgIconPlace = EditorGUI.Popup(lineRect, adapter._S_bgIconsPlace, new[] { "Next to 'Label'", "Next to 'Foldout' Button", "Align 'Left'"});
        if (new_S_bgIconPlace != adapter._S_bgIconsPlace )
        {   adapter._S_bgIconsPlace = new_S_bgIconPlace ;
            adapter.RepaintAllViews();
        }*/
        return lineRect;
    }
    internal static Rect DrawHoverPlaceSettingLine(Rect lineRect, Adapter adapter)
    {
#pragma warning disable
        int nv = 0;
        if (adapter.IS_PROJECT())
            nv =  adapter. TOOGLE_POP(ref lineRect, "<b>HighLighter Window Button</b> Placement", adapter._S_bgButtonForIconsPlace - 1,  "-Icon-") + 1;
        else if (Adapter.USE2018_3)
            nv =  adapter. TOOGLE_POP(ref lineRect, "<b>HighLighter Window Button</b> Placement", adapter._S_bgButtonForIconsPlace, "<Left", "-Icon-", "<Left and -Icon-");
        else
            nv =  adapter. TOOGLE_POP(ref lineRect, "<b>HighLighter Window Button</b> Placement", adapter._S_bgButtonForIconsPlace, "<Left");
#pragma warning restore
            
        if (nv != adapter._S_bgButtonForIconsPlace)
        {   adapter._S_bgButtonForIconsPlace = nv;
            adapter.RepaintAllViews();
        }
        
        
        
        lineRect .y += lineRect.height;
        
        
        var on = GUI.enabled;
        GUI.enabled = Adapter.USE2018_3;
        
        var nv2 =  adapter. TOOGLE_POP(ref lineRect, "<b>Red Dot</b> at HighLighter Button", adapter._S_hoverState, "None", "Window Open Only", "Window and Hover");
        if (nv2 != adapter._S_hoverState)
        {   adapter._S_hoverState = nv2;
            adapter.RepaintAllViews();
        }
        GUI.enabled = on;
        
        
        
        /*
        Adapter. LABEL(lineRect, "<i>Hover Red Marker:</i>");
        lineRect.y += lineRect.height;
        var new_S_hoverState = EditorGUI.Popup(lineRect, adapter._S_hoverState, new[] { "Disable", "If Color Window is Open", "Show Always"});
        if (new_S_hoverState != adapter._S_hoverState )
        {   adapter._S_hoverState = new_S_hoverState ;
            adapter.RepaintAllViews();
        }*/
        return lineRect;
    }
    
    
    
    
    void DrawDettings(Rect inputrect)
    {   inputrect.width -= 4;
        var lineRect  = inputrect;
        lineRect.height = EditorGUIUtility.singleLineHeight;
        lineRect.x += 20;
        lineRect.width -= 40;
        
        /* lineRect.y += lineRect.height;
        adapter._S_USE_HIGLIGHT_IN_BOTTOM = adapter.TOGGLE_LEFT(lineRect, "Colors are displayed on the bottom panel", adapter._S_USE_HIGLIGHT_IN_BOTTOM);*/
        
        adapter.InitializeStyles();
        
        
        EditorGUI.BeginChangeCheck();
        
        /*  GUI.enabled = adapter.par.USE_HIGLIGHT;
          lineRect =   DrawColorAligmentSettingsLine(lineRect, adapter);
          GUI.enabled = true;*/
        // adapter.ENABLE_RICH();
        lineRect.y += lineRect.height;
        adapter.par.highligterOpacity = Mathf.Clamp( EditorGUI.FloatField(lineRect, "HighLighter Opacity:", adapter.par.highligterOpacity ), 0, 1 );
        lineRect.y += lineRect.height;
        adapter._S_BottomPaddingForBgColor = Mathf.Clamp( EditorGUI.IntField(lineRect, "Vertical Padding '1':", adapter._S_BottomPaddingForBgColor ), 0, 16 );
        lineRect.y += lineRect.height;
        // adapter.DISABLE_RICH();
        
        var on2 = GUI.enabled;
        GUI.enabled = !adapter.IS_PROJECT();
        //  EditorGUI.BeginDisabledGroup(true);
        lineRect.y += lineRect.height;
        lineRect =  DrawHoverPlaceSettingLine(lineRect, adapter);
        lineRect.y += (EditorGUIUtility.singleLineHeight);
        lineRect.y += (EditorGUIUtility.singleLineHeight);
        GUI.enabled = on2;
        //  EditorGUI.EndDisabledGroup();
        
        lineRect.y += lineRect.height;
        adapter.ENABLE_RICH();
        adapter.par.COLOR_ICON_SIZE = Mathf.Clamp( EditorGUI.IntField(lineRect, "<b>Custom Icons</b> size '" + EditorGUIUtility.singleLineHeight + "'", adapter.par.COLOR_ICON_SIZE ), 10, 30 );
        adapter.DISABLE_RICH();
        lineRect.y += lineRect.height;
        adapter._S_USEdefaultIconSize = adapter.TOGGLE_LEFT(lineRect, "<i>Use Default Icons size:</i>", adapter._S_USEdefaultIconSize);
        lineRect.y += lineRect.height;
        var  on = GUI.enabled;
        GUI.enabled &= adapter._S_USEdefaultIconSize;
        adapter.ENABLE_RICH();
        adapter._S_defaultIconSize = Mathf.Clamp( EditorGUI.IntField(lineRect, "<b>Defaul Iconst</b> size '" + EditorGUIUtility.singleLineHeight + "'", adapter._S_defaultIconSize ), 10, 30 );
        adapter.DISABLE_RICH();
        GUI.enabled = on;
        
        lineRect.y += lineRect.height;
        lineRect =  DrawIconAligmentSettingsLine(lineRect, adapter);
        
        lineRect.y += lineRect.height;
        
        
        
        
        on = GUI.enabled;
        GUI.enabled = adapter.HAS_LABEL_ICON();
        var nv =  adapter. TOOGLE_POP(ref lineRect, "Draw yellow marks next to the assigned icons", adapter.par.BottomParams.DRAW_FOLDER_STARMARK ? 1 : 0, "Disable", "Enable") == 1;
        if (nv != adapter.par.BottomParams.DRAW_FOLDER_STARMARK)
        {   adapter.par.BottomParams.DRAW_FOLDER_STARMARK = nv;
            adapter.RepaintAllViews();
        }
        /* lineRect.y += lineRect.height;
         adapter.par.BottomParams.DRAW_FOLDER_STARMARK = adapter.TOGGLE_LEFT( lineRect, "Draw yellow marks next to the assigned icons", adapter.par.BottomParams.DRAW_FOLDER_STARMARK );*/
        GUI.enabled = on;
        
        lineRect.y += (EditorGUIUtility.singleLineHeight);
        lineRect.y += (EditorGUIUtility.singleLineHeight);
        
        
        
        if (adapter.IS_HIERARCHY())
        {
        
            EditorGUI.BeginChangeCheck();
            lineRect.y += lineRect.height;
            adapter.par.SHOW_NULLS = adapter.TOGGLE_LEFT( lineRect, "Show Locator for Object without Component", adapter.par.SHOW_NULLS );
            
            lineRect.y += lineRect.height;
            adapter.par.SHOW_PREFAB_ICON = adapter.TOGGLE_LEFT( lineRect, "Show Prefab icon", adapter.par.SHOW_PREFAB_ICON );
            
            lineRect.y += lineRect.height;
            adapter.par.SHOW_MISSINGCOMPONENTS = adapter.TOGGLE_LEFT( lineRect, "Show Warning if Object has missing Component", adapter.par.SHOW_MISSINGCOMPONENTS );
            if (EditorGUI.EndChangeCheck())
            {   if (adapter.OnClearObjects != null) adapter.OnClearObjects();
            }
        }
        /*  if (adapter.IS_HIERARCHY())
           {
        
               lineRect.y += lineRect.height;
               adapter.par.SHOW_NULLS = adapter.TOGGLE_LEFT( lineRect, "Show 'Locator' icon for empty GameObjects", adapter.par.SHOW_NULLS );
        
               lineRect.y += lineRect.height;
               adapter.par.SHOW_PREFAB_ICON = adapter.TOGGLE_LEFT( lineRect, "Show 'Prefab' icon", adapter.par.SHOW_PREFAB_ICON );
        
               lineRect.y += lineRect.height;
               adapter.par.SHOW_MISSINGCOMPONENTS = adapter.TOGGLE_LEFT( lineRect, "Show 'Warning' icon if component is missing", adapter.par.SHOW_MISSINGCOMPONENTS );
           }*/
        
        
        
        if (EditorGUI.EndChangeCheck() )
        {   adapter.SavePrefs();
            adapter.RepaintAllViews();
        }
        
        
    }
}
}
