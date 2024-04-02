#if UNITY_EDITOR
    #define HIERARCHY
    #define PROJECT
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


namespace EModules.EModulesInternal {

public class _S___Hierarchy_GUI2 : EditorWindow {
    public void OnGUI()
    {   if ( Adapter.HierAdapter == null ) return;
        Adapter.HierAdapter.Legacy_SetDraver( this );
    }
    
    
    internal const int P = 2000;
    internal static void showWindow()
    {   EditorWindow.GetWindow<_S___Hierarchy_GUI2>();
    }
}




partial class Adapter {
    int legacyIndex = 0;
    string legacySearchContex;
    GUIStyle __emptyStyle ;
    GUIStyle emptyStyle
    {   get
        {   return __emptyStyle ?? (__emptyStyle = new GUIStyle()
            {   active = new GUIStyleState() { background = GUI.skin.box.normal.background ?? GUI.skin.box.normal.scaledBackgrounds[0] },
                clipping = TextClipping.Overflow,
                alignment = TextAnchor.MiddleLeft,
                border = new RectOffset( 5, 5, 5, 5 )
            });
        }
    }
    void LegacyButton( string t, int ind )
    {   var R = EditorGUILayout.GetControlRect(GUILayout.Height(EditorGUIUtility.singleLineHeight * 1.5f));
        R.width *= 0.8f;
        /*R.x += 16 * t.Count( c => c == '/' );
        if ( t.IndexOf( '/' ) != -1 ) t = t.Substring( t.LastIndexOf( '/' ) + 1 );
        t = "└ " + t;*/
        if ( GUI.Button( R, "", emptyStyle ) )
        {   legacyIndex = ind;
            legacyScrollMenu = Vector2.zero;
        }
        if ( ind == legacyIndex && Event.current.type == EventType.Repaint ) GUI.skin.box.Draw( R, true, true, true, true );
        GUI.Label( R, t.ToUpper() );
        EditorGUIUtility.AddCursorRect( R, MouseCursor.Link );
    }
    Vector2 legacyScrollMenu;
    public void Legacy_SetDraver( EditorWindow win )
    {
    
        var w = EditorGUILayout.GetControlRect( GUILayout.Height( 0 ) ).width;
        
        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical( GUILayout.Width( w * 0.2f ) );
        GUILayout.Space( 40 );
        GUILayout.BeginVertical( GUI.skin.box );
        /*  if ( pluginID == 0 )
          {   LegacyButton( "Hierarchy PRO", 0 );
              LegacyButton( "Hierarchy PRO/Hierarchy HighLighter", 1 );
              LegacyButton( "Hierarchy PRO/Hierarchy Bottom Bar", 3 );
              // LegacyButton( "Hierarchy PRO/Hierarchy Bottom Bar/Quick Help" , 4 );
              LegacyButton( "Hierarchy PRO/Hierarchy Search Window", 5 );
              LegacyButton( "Hierarchy PRO/Hierarchy Custom Click Menu", 6 );
              LegacyButton( "Hierarchy PRO/Hierarchy Cache", 7 );
              LegacyButton( "Hierarchy PRO/Hierarchy Right Bar", 8 );
              LegacyButton( "Hierarchy PRO/Hierarchy Right Bar/Display of Functions and Vars", 9 );
              LegacyButton( "Hierarchy PRO/Hierarchy Right Bar/PlayMode Data Keeper", 10 );
              LegacyButton( "Hierarchy PRO/Hierarchy Right Bar/Memory Optimizer", 11 );
              LegacyButton( "Hierarchy PRO/Hierarchy Right Bar/Custom Modules", 12 );
          }
          else
          {   LegacyButton( "Hierarchy PRO/☰ Project ORG", 0 );
              LegacyButton( "Hierarchy PRO/☰ Project ORG/Project HighLighter", 1 );
              LegacyButton( "Hierarchy PRO/☰ Project ORG/Project Bottom Bar", 3 );
              LegacyButton( "Hierarchy PRO/☰ Project ORG/Project Search Window", 5 );
              LegacyButton( "Hierarchy PRO/☰ Project ORG/Project Custom Click Menu", 6 );
              LegacyButton( "Hierarchy PRO/☰ Project ORG/Project Cache", 7 );
              LegacyButton( "Hierarchy PRO/☰ Project ORG/Project Right Bar", 8 );
          }*/
        //   if ( pluginID == 0 )
        {   LegacyButton( "Hierarchy PRO", 0 );
            LegacyButton( "HighLighter", 1 );
            LegacyButton( "Bottom Bar", 3 );
            // LegacyButton( "Bottom Bar/Quick Help" , 4 );
            LegacyButton( "Search Window", 5 );
            LegacyButton( "Custom Click Menu", 6 );
            LegacyButton( "Cache", 7 );
            LegacyButton( "Right Bar", 8 );
            LegacyButton( "-Display Vars/Functions", 9 );
            LegacyButton( "-PlayMode Data Keeper", 10 );
            LegacyButton( "-Memory Optimizer", 11 );
            LegacyButton( "-Custom Modules", 12 );
        }
        //    else
        #if UNITY_EDITOR
        {   GUILayout.Label("-");
            LegacyButton( "☰ Project ORG", 13 );
            /* LegacyButton( "☰ Project HighLighter", 14 );
             LegacyButton( "☰ Project Bottom Bar", 15 );
             LegacyButton( "☰ Project Search Window", 16 );
             LegacyButton( "☰ Project Custom Click Menu", 17 );
             LegacyButton( "☰ Project Cache", 18 );
             LegacyButton( "☰ Project Right Bar", 19 );*/
        }
        #endif
        GUILayout.EndVertical();
        
        GUILayout.EndVertical();
        GUILayout.BeginVertical();
        
        
        
        GUILayout.BeginHorizontal();
        GUILayout.Label( "Search:" );
        legacySearchContex = EditorGUILayout.TextField( legacySearchContex );
        GUILayout.EndHorizontal();
        GUILayout.Space( 16 );
        
        legacyScrollMenu = GUILayout.BeginScrollView( legacyScrollMenu, EditorStyles.helpBox );
        __prefWindow = win;
        if ( legacyIndex <= 12 ) HierarchySettingGUI( legacySearchContex, legacyIndex );
        else ProjectSettingGUI( legacySearchContex, legacyIndex );
        GUILayout.EndScrollView();
        
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
    }
}







}
