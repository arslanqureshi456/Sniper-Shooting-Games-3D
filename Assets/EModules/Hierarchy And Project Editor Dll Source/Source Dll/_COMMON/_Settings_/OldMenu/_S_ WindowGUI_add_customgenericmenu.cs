using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;
#if PROJECT
    using EModules.Project;
#endif



    namespace EModules.EModulesInternal

{




    internal partial class SETUPROOT {
    
    
    
        void DRAW_CUSTOMOBJECTSMENU(float start_X, float wOffset, ref float outY)
        {
#pragma warning disable
            if (Adapter.LITE) return;
#pragma warning restore
            
            
            BEGIN_CATEGORY( ref calcGEN, false );
            DrawHeader( "Custom Generic Menu (Right Click)" );
            DRAW_WIKI_BUTTON( "Other", "Custom Extensible Menu" );
            
            
            R = GetControlRect( EditorGUIUtility.singleLineHeight );
            LABEL(R, "For which windows does Menu's HotKeys work?");
            var htks = A.CUSTOMMENU_HOTKEYS_WINDOWS;
            //  internal string[] AllWindows = { "SceneView", "Inspector", "GameView", "SceneHierarchy"/*, "ProjectBrowser"*/};
            R = GetControlRect( EditorStyles.toolbarButton.fixedHeight );
            GUI.enabled = false;
            if (A.IS_HIERARCHY())
            {   GUI.Toolbar(R, 1, new[] {   "No", "Scene Hierarchy"}, EditorStyles.toolbarButton) ;
            }
            else
            {   GUI.Toolbar(R, 1, new[] {   "No", "Project Browser"}, EditorStyles.toolbarButton) ;
            }
            GUI.enabled = true;
            R = GetControlRect( EditorStyles.toolbarButton.fixedHeight );
            var oldw1 = htks.ContainsKey("SceneView");
            var w1 =  GUI.Toolbar(R, oldw1 ? 1 : 0, new[] {   "No", "SceneView"}, EditorStyles.toolbarButton) == 1;
            R = GetControlRect( EditorStyles.toolbarButton.fixedHeight );
            var oldw2 = htks.ContainsKey("GameView");
            var w2 =  GUI.Toolbar(R, oldw2 ? 1 : 0, new[] {   "No", "GameView"}, EditorStyles.toolbarButton) == 1;
            R = GetControlRect( EditorStyles.toolbarButton.fixedHeight );
            var oldw3 = htks.ContainsKey("Inspector");
            var w3 =  GUI.Toolbar(R, oldw3 ? 1 : 0, new[] {   "No", "Inspector"}, EditorStyles.toolbarButton) == 1;
            if (w1 != oldw1 || w2 != oldw2 || w3 != oldw3)
            {   var res = new Dictionary<string, bool>();
                if (w1) res.Add("SceneView", true);
                if (w2) res.Add("GameView", true);
                if (w3) res.Add("Inspector", true);
                A.CUSTOMMENU_HOTKEYS_WINDOWS = res;
            }
            
            
            
            HelpBox( "Source code of default functions are placed into: '" + A.PluginInternalFolder + "/RightClickObjectLeftMenu_Example.cs'", MessageType.None );
            
            
            Space(EditorGUIUtility.singleLineHeight);
            // Label("Context Menu Extension");
            HelpBox( "Right-CLICK to the left of the object", MessageType.None );
            
            
            
            DRAW_HELP_TEXTURE( A.IS_HIERARCHY() ? "HELP_RIGHTMENU" : "HELP_RIGHTMENU PROJECT", height : 60, DDD : 1 );
            Space( 5 );
            
            
            // HelpBox(@"""Code Example inside 'RightClickObjectLeftMenu_Example.cs""
            /*  if (A.IS_HIERARCHY())
                  HelpBox( @"- ""Group/Ungroup""
            - ""Duplicate Next To Object""
            - ""Expand Selection/Collapse Selection""
            - ""Select Only Top/Select All Children""", MessageType.Info );
              Space( 10 );*/
            // GUI.enabled = false;
            
            var en = GUI.enabled;
            GUI.enabled = !Adapter.LITE;
            
            
            
            
            
            Space(EditorGUIUtility.singleLineHeight);
            /*  if (A.IS_HIERARCHY()) HelpBox( "You can inherit the interface \n'HierarchyExtensions.IGenericMenu'\n anywhere in your project", MessageType.None );
              else HelpBox( "You can inherit the interface \n'ProjectExtensions.IGenericMenu'\n anywhere in your project", MessageType.None );*/
            HelpBox( "You should inherit the interface to create menu item", MessageType.None );
            var rect = GET_OFFSETRECT(221);
            sp = GUI.BeginScrollView( rect, sp, new Rect( 0, 0, 850, 185 ) );
            if (A.IS_HIERARCHY()) EditorGUI.TextArea( new Rect( 0, 0, 850, 185 ), HIERARCHY_MENU_HELP );
            else EditorGUI.TextArea( new Rect( 0, 0, 850, 185 ), PROJECT_MENU_HELP );
            GUI.EndScrollView();
            // if (A.IS_HIERARCHY()) DRAW_HELP_TEXTURE( "HELP_RIGHTMENU_CODE" );
            
            GUI.enabled = en;
            
            END_CATEGORY( ref calcGEN );
            
            
            
            
            //**  ****  **//
            //**  ****  **//
            //**  ****  **//
            /* if (GetControlRect(0).y > outY) outY = GetControlRect(0).y;
             Y = start_X;
             X += wOffset;*/
            //**  ****  **//
            //**  ****  **//
            //**  ****  **//
            
            
            
            
            
            
            
            //**  ****  **//
            //**  ****  **//
            //**  ****  **//
            if (GetControlRect( 0 ).y > outY) outY = GetControlRect( 0 ).y;
            
        }//ADD_CustomGenericMenu
        
        
        
        
        
    }
}
