﻿//namespace EModules
#if UNITY_EDITOR
    #define HIERARCHY
    // #define PROJECT
#endif

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;
#if PROJECT
    using EModules.Project;
#endif
//namespace EModules




namespace EModules.EModulesInternal

{
/* internal partial class Adapter
 {





     internal partial class BottomInterface
     {*/

internal class _W__ModulesWindow : _W___IWindow {


    internal static _W___IWindow Init(MousePos rect, Adapter adapter)
    {   if (rect.type != MousePos.Type.ModulesListWindow_380_700)
        {   Debug.LogWarning("Mismatch type");
            rect.SetType(MousePos.Type.ModulesListWindow_380_700);
        }
        if (adapter.DEFAUL_SKIN == null) adapter.DEFAUL_SKIN = Adapter.GET_SKIN();
        
        // rect.y -= rect.height;
        
        var w = (_W__ModulesWindow)private_Init( rect, typeof( _W__ModulesWindow ), adapter, "Right Modules", utils: true);
        
        w.SET_NEW_HEIGHT( adapter, rect.Height );
        
        return w;
    }
    static Vector2 sp;
    
    protected override void OnGUI()
    {   if (_inputWindow == null) return;
    
        if (adapter == null)
        {   CloseThis();
            return;
        }
        
        
        base.OnGUI();
        
        
        adapter.InitializeStyles();
        
        GUILayout.  Space(EditorGUIUtility.singleLineHeight);
        
        GUILayout.Label("Default Modules:");
        
        //Space( 4 );
        var HH = 95;
        var R = EditorGUILayout.GetControlRect(GUILayout.Height(HH));
        
        var en = GUI.enabled;
        if (adapter.modules == null)adapter.MOI.InitModules();
        adapter.ChangeGUI(false);
        DRAW_MODULES(adapter, R, 3, 3, true );
        adapter.RestoreGUI();
        GUI.enabled = en;
        
        
        
        GUILayout.  Space(EditorGUIUtility.singleLineHeight);
        
        R = EditorGUILayout.GetControlRect();
        var newM = adapter.TOGGLE_LEFT( R, "Use Custom Modules:", adapter.par.USE_CUSTOMMODULES );
        if (newM != adapter.par.USE_CUSTOMMODULES)
        {   adapter.par.USE_CUSTOMMODULES = newM;
            adapter.SavePrefs();
            InternalEditorUtility.RepaintAllViews();
        }
        
        EditorGUILayout.HelpBox( "You should inherit the interface to create menu item", MessageType.None );
        
#pragma warning disable
        HH = 95;
        R = EditorGUILayout.GetControlRect(GUILayout.Height(HH));
        GUILayout.  Space(EditorGUIUtility.singleLineHeight);
        var H3 = 325;
        var R2 = EditorGUILayout.GetControlRect(GUILayout.MinHeight(H3), GUILayout.Width(position.width - 10 ), GUILayout.ExpandHeight(true));
#pragma warning restore
        
        DrawCustomModules1( adapter,  R, R2);
        EditorGUILayout.HelpBox( "Source code of default modules are placed into: '" + adapter.PluginInternalFolder + "/CustomModule_Example.cs'", MessageType.None );
        DrawCustomModules2( adapter,  R, R2);
        
        
        
        
        
        adapter.RestoreGUI();
    }
    
    static  bool enddd3;
    internal static void DrawCustomModules1(Adapter adapter, Rect R, Rect R2 )
    {   enddd3 = GUI.enabled;
        GUI.enabled &= !Adapter.LITE;
        
        GUI.enabled &= adapter.par.USE_CUSTOMMODULES;
        
        adapter.ChangeGUI();
        DRAW_MODULES(adapter, R, 3, 3, false );
        adapter.RestoreGUI();
        
        
        
        // GUILayout.  Space(EditorGUIUtility.singleLineHeight);
        
        
    }
    internal static void DrawCustomModules2(Adapter adapter, Rect R, Rect R2 )
    {
    
    
    
        R = R2;
        //R = EditorGUILayout.GetControlRect(GUILayout.MinHeight(H3), GUILayout.Width(position.width - 10 ), GUILayout.ExpandHeight(true));
        
        // sp = GUILayout.BeginScrollView( sp, GUILayout.Width(position.width), false);
        sp = GUI.BeginScrollView( R, sp, new Rect( 0, 0, 500, R.height - GUI.skin.verticalScrollbar.fixedWidth ), true, false);
        if (adapter.IS_HIERARCHY()) EditorGUI.TextArea( new Rect( 0, 0, 500, R.height - GUI.skin.verticalScrollbar.fixedWidth ), HIERARCHY_MODULE_HELP );
        else EditorGUI.TextArea( new Rect( 0, 0, 500, R.height - GUI.skin.verticalScrollbar.fixedWidth ), PROJECT_MODULE_HELP );
        //EditorGUILayout.GetControlRect(GUILayout.Height(H3 - GUI.skin.verticalScrollbarDownButton.fixedWidth));
        GUI.EndScrollView();
        
        /* if (adapter.IS_HIERARCHY()) HelpBox( "To add your own module, inherit the slot class (HierarchyExtensions.CustomModule_Slot1 / 2 / 3) anywhere in your code", MessageType.None );
         else HelpBox( "To add your own module, inherit the slot class (ProjectExtensions.CustomModule_Slot1 / 2 / 3) anywhere in your code", MessageType.None );*/
        //  END_PADDING();
        
        GUI.enabled = enddd3;
        
        
        
    }
    
    
    
    
    
    static string HIERARCHY_MODULE_HELP = @"
    class MyModule : HierarchyExtensions.CustomModule_Slot3
    {
        public override string NameOfModule { get { return ""MyModule""; } }
    
        // In this method, you can display information and buttons
        public override void Draw(Rect drawRect, GameObject o)
        {
            // You can invoke different built-in methods for changing variables
            //        if (GUI.Button(drawRect,""string"")) SHOW_StringInput(...
            //        if (GUI.Button(drawRect,""int"")) SHOW_IntInput(...
            //        if (GUI.Button(drawRect,""dropdown"")) SHOW_DropDownMenu(...
        }
    
        // ToString(...) method is used for the search box
        public override string ToString(GameObject o)
        {
            return null;
        }
    }";

    static string PROJECT_MODULE_HELP = @"
    class MyModule : ProjectExtensions.CustomModule_Slot1
    {
        public override string NameOfModule { get { return ""MyModule""; } }
    
        // In this method, you can display information and buttons
        public override void Draw(Rect drawRect, string assetPath, string assetGuid, int instanceId, bool isFolder, bool isMainAsset)
        {
          // You can invoke different built-in methods for changing variables
          //        if (GUI.Button(drawRect,""string"")) SHOW_StringInput(...
          //        if (GUI.Button(drawRect,""int"")) SHOW_IntInput(...
          //        if (GUI.Button(drawRect,""dropdown"")) SHOW_DropDownMenu(...
        }
    
        // ToString(...) method is used for the search box
        public override string ToString(string assetPath, string assetGuid, int instanceId, bool isFolder, bool isMainAsset)
        {
          return null;
        }
    }";


    internal static void DRAW_MODULES(Adapter adapter, Rect RECT, int CELLS, int LINES, bool drawMain)
    {   var HH = 25;
        // var SH = 3;
        // int LINECOUNT = 3;
        int WIDTH = 78 * 4 / CELLS;
        var PADDING = 5;
        // DrawNew(HH + PADDING);
        //   var RECT = GET_OFFSETRECT( (HH) * LINES + PADDING * 2/*, new[] { Width(WIDTH * 3) }*/, -5 );
        var or = RECT;
        RECT.x += PADDING;
        RECT.y += PADDING;
        RECT.height -= PADDING * 2;
        RECT.width -= PADDING * 2;
        var SX = (RECT.width - WIDTH * CELLS) / (CELLS - 1);
        //RECT.width = WIDTH * 3;
        int interato = 0;
        
        var res = GUI.enabled;
        
        var Y = 0;
        var m = drawMain ? adapter.modules.Reverse() : adapter.modules;
        foreach (var source in m)
        {   if (drawMain && (source.sibbildPos == -1
                         #if HIERARCHY || PROJECT
                             || (
                             #if HIERARCHY
                                 adapter.IS_HIERARCHY() && source is EModules.EModulesInternal.Hierarchy.M_UserModulesRoot
                             #if PROJECT
                                 ||
                             #endif
                             #endif
                                 
                             #if PROJECT
                                 source is EModules.EProjectInternal.Project.M_UserModulesRoot
                             #endif
                             )
                         #endif
                             
                            )) continue;
            if (!drawMain
                #if HIERARCHY || PROJECT
                    && !(
                    #if HIERARCHY
                        adapter.IS_HIERARCHY() && source is EModules.EModulesInternal.Hierarchy.M_UserModulesRoot
                    #if PROJECT
                        ||
                    #endif
                    #endif
                    #if PROJECT
                        source is EModules.EProjectInternal.Project.M_UserModulesRoot
                    #endif
                    )
                #endif
               ) continue;
               
            if (interato == CELLS)
            {   Y++;
                interato = 0;
            }
            
            var drawRect = new Rect( RECT.x + (WIDTH + SX) * interato, RECT.y + (HH + 3) * Y, WIDTH, HH );
            /* Label("", Width(width), Height(25));
             lastRect = GetLastRect();*/
            GUI.enabled = res & source.enableOverride();
            
            if (source.enable)
            {   /* Hierarchy.colorText11.SetPixel(0, 0, new Color(0.6f, 0.3f, 0.1f, 1));
                 Hierarchy.colorText11.Apply();*/
                if (Event.current.type.Equals( EventType.Repaint ))
                {   var asd = GUI.color;
                    //GUI.color *= new Color( 0.6f, 0.3f, 0.1f, 1 );
                    // GUI.DrawTexture(drawRect, redTTexure);
                    var glowR = drawRect;
                    glowR.y -= 4;
                    glowR.height += 8;
                    GUI.DrawTexture( glowR, adapter.STYLE_DEFBUTTON.active.background );
                    /* GUI.DrawTexture( glowR, adapter.GetIcon( "HIPERUI_BUTTONGLOW" ) );
                     GUI.DrawTexture( glowR, adapter.GetIcon( "HIPERUI_BUTTONGLOW" ) );
                     GUI.DrawTexture( glowR, adapter.GetIcon( "HIPERUI_BUTTONGLOW" ) );
                     GUI.DrawTexture( glowR, adapter.GetIcon( "HIPERUI_BUTTONGLOW" ) );
                     GUI.DrawTexture( glowR, adapter.GetIcon( "HIPERUI_BUTTONGLOW" ) );*/
                    /* if (Event.current.type == EventType.repaint)
                     {
                         var t = Adapter.GET_SKIN().box.normal.background;
                         var b = Adapter.GET_SKIN().box.border;
                         var B = Mathf.RoundToInt(Hierarchy.GetIcon("HIPERUI_BUTTONGLOW").width * 0.4f);
                         Adapter.GET_SKIN().box.border = new RectOffset(B, B, B, B);
                         Adapter.GET_SKIN().box.normal.background = Hierarchy.GetIcon("HIPERUI_BUTTONGLOW");
                         Adapter.GET_SKIN().box.Draw(drawRect, false, false, false, false);
                         Adapter.GET_SKIN().box.Draw(drawRect, false, false, false, false);
                         Adapter.GET_SKIN().box.Draw(drawRect, false, false, false, false);
                         Adapter.GET_SKIN().box.Draw(drawRect, false, false, false, false);
                         Adapter.GET_SKIN().box.normal.background = t;
                         Adapter.GET_SKIN().box.border = b;
                     }*/
                    GUI.color = asd;
                }
            }
            GUIContent content = new GUIContent();
            if (source.HeaderTexture2D != null) content.image = adapter.GetIcon( source.HeaderTexture2D );
            else content.text = source.HeaderText;
            content.tooltip = "Enable/Disalbe";
            if (drawMain && !source.enableOverride()) content.tooltip += " (Pro Only)";
            if (!drawMain)
            {   content.tooltip = source.ContextHelper;
                if (!source.enableOverride()) content.tooltip += source.enableOverrideMessage();
            }
            if (adapter.Button( drawRect, content ))
            {   source.CreateUndo();
                source.enable = !source.enable;
                source.SetDirty();
                
                InternalEditorUtility.RepaintAllViews();
                
            }
            EditorGUIUtility.AddCursorRect( drawRect, MouseCursor.Link );
            interato++;
        }
        
        
        GUI.enabled = res;
        
        or.height += 3;
        Adapter. INTERNAL_BOX( or, "" );
    }
    
}
}
