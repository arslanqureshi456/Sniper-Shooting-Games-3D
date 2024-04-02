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
//namespace EModules


/*
 * You can use version control systems.

   To maintain compatibility between people who do not use the plug-in hierarchy, you may copy few *.cs files to other machines.

   You should copy 'DescriptionHelper.cs', 'DescriptionRegistrator.cs' and 'Hierarchy (MAYA outliner) Plugin.dll' from ../Resource folder.
 */









namespace EModules.EModulesInternal

{

internal partial class SETUPROOT {









    float[] caclCACHE = new float[10];
    
    int GET_STATE()
    {   //if ( A.DISABLE_DES() ) return 2;
        if (A.IS_PROJECT()) return 0;
        if (Hierarchy_GUI.Instance( A ).SaveToScriptableObject == "FOLDER") return 2;
        if (Hierarchy_GUI.Instance( A ).GETUSE_REGISTRATOR) return 0;
        return 1;
    }
    
    
    
    void ADD_CacheGIT(float start_X, float wOffset, ref float outY, bool drawOnlyCache)
    {   HR();
    
        if (!drawOnlyCache)
        {   // Space( 27 );
            HelpBox( "If you want to share the current project settings, copy 'Library/EModules' to another machine", MessageType.None );
            R = GetControlRect( 40 );
            
            HR();
            
            /*if (A.IS_HIERARCHY()) HelpBox( "If you want to share the current project settings, copy '" + A.PluginInternalFolder + "/" + A.pluginname + "PluginSettings.asset'", MessageType.Info );
            else
            {   HelpBox( "You may share current project settings", MessageType.Info );
                HelpBox( "'ProjectDescriptionHelper.asset' stores Objects information", MessageType.Info );
                HelpBox( "'ProjectSettings.asset' stores plugin's settings of the current project", MessageType.Info );
            }*/
            
            // Space( EditorGUIUtility.singleLineHeight );
            
            
            // Label( "You can use version control systems." );
            DRAW_HELP_TEXTURE( "NEW_BOTTOM_GITHELP2" );
            
            
            HelpBox( "You can save Auto Highlighter and Icons settings local to Library, or to Assets and push it to other developers", MessageType.None );
            DrawNew();
            BEGIN_PADDING(15, 15);
            R = GetControlRect( EditorGUIUtility.singleLineHeight );
            var nv2 =  A. TOOGLE_POP(ref R, "<b>Auto-HighLighter</b> Filters", A.par.SaveSettingsHighLighterToLibrary ? 0 : 1, "Save To Library (Local)", "Save To Asset (Public)") == 0;
            END_PADDING();
            Space(R.y + R.height - Y);
            if (nv2 != A.par.SaveSettingsHighLighterToLibrary)
            {
            
                var c = nv2;
                var compex = UnityEditor.EditorUtility.DisplayDialogComplex("Auto-HighLighter Settings", "Do you want to remove old settings file?", "Yes", "Cancel", "No");
                if (compex == 0 || compex == 2) EModulesHierarchy_ProjectSettings_HighLighter.SwitchLibraryMode(A, c, compex == 2);
                
            }
            
            if (A.IS_HIERARCHY())
            {
            
                DrawNew();
                BEGIN_PADDING(15, 15);
                R = GetControlRect( EditorGUIUtility.singleLineHeight );
                var nv3 =  A. TOOGLE_POP(ref R, "<b>Components Icons</b>", A.par.SaveSettingsCustomIconsToLibrary ? 0 : 1, "Save To Library (Local)", "Save To Asset (Public)") == 0;
                END_PADDING();
                Space(R.y + R.height - Y);
                if (nv3 != A.par.SaveSettingsCustomIconsToLibrary)
                {
                
                    var c = nv3;
                    var compex = UnityEditor.EditorUtility.DisplayDialogComplex("Components Icons Settings", "Do you want to remove old settings file?", "Yes", "Cancel", "No");
                    if (compex == 0 || compex == 2) EModulesHierarchy_ProjectSettings_ComponentsIcons.SwitchLibraryMode(A, c, compex == 2);
                    
                    /* if ()
                     {
                    
                     }
                     A.RepaintAllViews();*/
                }
            }
            
            
            
            if (A.IS_HIERARCHY())
            {
            
                Space( EditorGUIUtility.singleLineHeight );
                var R = GetControlRect(EditorGUIUtility.singleLineHeight * 1);
                LABEL(R, "<i>Compatibility variants with those who do not have this asset:</i>" );
                LABEL(GetControlRect(EditorGUIUtility.singleLineHeight * 1), "- Use <b>Cache in Folder</b> mode (OpenSource Only)" );
                var c =  new GUIContent("- Copy <b>\"" + A.PluginInternalFolder  + "/SharedFolder/\"</b>\n to other machines, which does not have plugin");
                var h = GUI.skin.label.CalcHeight(c, R.width);
                LABEL(GetControlRect(h), c.text  );
                
                /*    HelpBox( "To maintain compatibility between people who do not use the plugin hierarchy, you may save data in the ScriptableObject files in the \"_ SAVE DATA\" folder within the plugin folder \"" +
                             A.PluginInternalFolder + "\"", MessageType.Info );
                    HelpBox( "Also, you may copy \"" + A.PluginInternalFolder  + "/SharedFolder/\" to other machines.", MessageType.Info );*/
                // Space( 10 );
                
                // DRAW_HELP_TEXTURE( "NEW_BOTTOM_GITHELP1" );
                // Space( 10 );
                
                // MessageType.Info );
                
            }
            
            
            HR();
            
            
            R = GetControlRect( 40 );
            LABEL(GetControlRect( EditorGUIUtility.singleLineHeight ), "<i>Editor Settings:</i>" );
            R = GetControlRect( 40 );
            var bR = R;
            bR.width /= 2;
            if (GUI.Button( bR, "Import Editor Settings" ))
            {   var lastPath = EditorPrefs.GetString("Hierarchy/LastPath", Adapter. UNITY_SYSTEM_PATH);
                var path = EditorUtility.OpenFilePanel("Import Settings", lastPath, "settings");
                if (path.Length != 0)
                {   EditorPrefs.SetString( "Hierarchy/LastPath", path );
                    var load = File.ReadAllText(path, System.Text.Encoding.Unicode);
                    if (!A.SettingsFromString( load ))
                    {   EditorUtility.DisplayDialog( "Import Settings", "Wrong format", "Ok" );
                    }
                }
            }
            bR.x += bR.width;
            if (GUI.Button( bR, "Export Editor Settings" ))
            {   var path = EditorUtility.SaveFilePanel("Export Settings", Adapter.UNITY_SYSTEM_PATH, A.pluginname + ".settings", "settings");
                if (path.Length != 0)
                {   EditorPrefs.SetString( "Hierarchy/LastPath", path );
                    File.WriteAllText( path, A.SettingsToString(), System.Text.Encoding.Unicode );
                }
            }
            
            R = GetControlRect( EditorGUIUtility.singleLineHeight );
            bR.x = R.x;
            bR.y = R.y + 5;
            bR.height = R.height;
            if (GUI.Button( bR, "Default Editor Settings" ))
            {   if (EditorUtility.DisplayDialog( "Default Editor Settings", "Do you want to reset editor's editor settings", "Ok", "Cancel" ))
                {   A.ResetSettings();
                }
            }
            
        }
        
        
        //**  ****  **//
        //**  ****  **//
        //**  ****  **//
        if (GetControlRect( 0 ).y > outY) outY = GetControlRect( 0 ).y;
        Y = start_X;
        X += wOffset;
        //**  ****  **//
        //**  ****  **//
        //**  ****  **//
        
        
        
        
        var I = 5;
        
        
        if (A.IS_HIERARCHY())
        {   // Space(30);
            //  DrawNew(42);
            BEGIN_CATEGORY( ref caclCACHE[I], true, new Color32( 0, 0, 0, 40 ) );
            DrawHeader( "STORED DATA" );
            DRAW_WIKI_BUTTON( "Other", "Cache and Data Storage" );
            
            
            /*var dd = Hierarchy_GUI.Initialize( Hierarchy.adapter );
            if (dd)
            {
              DrawNew();
              var r = GetControlRect( 30 );
              Adapter.DrawRect( SHRINK( r, -3 ), new Color( 0.4f, 0.1f, 0.1f, 0.3f ) );
              A.par.ALWAYS_SAVEDATA_QUESTION = A.TOGGLE_LEFT( r, "Ask the cache storage location every time you im-\nport the plugin", A.par.ALWAYS_SAVEDATA_QUESTION );
              if (!A.par.ALWAYS_SAVEDATA_QUESTION)
              {
                var default_cache =new[] {"Save in scene - Add registrator", "Save in scene - (Default)", "Save in folder - No data in scene" };
                var rect = GetControlRect(20);
                var newState = EditorGUI.Popup( rect,"Default storage behavior", A.par.BAKED_DEFAULT_STATE, default_cache );
                if (newState != A.par.BAKED_DEFAULT_STATE)
                {
                  A.par.BAKED_DEFAULT_STATE = newState;
                  A.SavePrefs();
                }
              }
            
            }*/
            var R1 = new Rect();
            var R2 = new Rect();
            var R3 = new Rect();
            
            DrawNew();
            //  HelpBox( "Prefab Settings", MessageType.Info );
            Y -= 5;
            LABEL(GetControlRect(EditorGUIUtility.singleLineHeight), "<b>Prefab Settings:</b>");
            BeginHorizontal();
            R1 = GetControlRect( (40), 2 );
            var oldE = GUI.enabled ;
            GUI.enabled
                = GET_STATE() != 2;
            if (GUI.Button( R1, "Separate Instances" ) && Hierarchy_GUI.Instance(A).PrefabIDMode != Hierarchy_GUI.PrefabIDModeEnum.SeparateInstances)
            {   if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                {   if (EditorUtility.DisplayDialog( "Enable Separate Prefabs Instances", "This mode will store data for each instance separately?", "Yes", "Cancel" ))
                    {   Hierarchy_GUI.Instance(A).PrefabIDMode = Hierarchy_GUI.PrefabIDModeEnum.SeparateInstances;
                        Hierarchy_GUI.SetDirtyObject( A );
                        Adapter.RequestScriptReload();
                    }
                }
            }
            GUI.enabled = oldE;
            if (Hierarchy_GUI.Instance(A).PrefabIDMode == Hierarchy_GUI.PrefabIDModeEnum.SeparateInstances) GUI.DrawTexture( R1, A.GetIcon( "BUTBLUE" ) );
            
            R2 = R1;
            R2.x += R2.width;
            if (GUI.Button( R2, "Merged Instances" ) && Hierarchy_GUI.Instance(A).PrefabIDMode != Hierarchy_GUI.PrefabIDModeEnum.MergedInstances)
            {   if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                {   if (EditorUtility.DisplayDialog( "Enable Merged Prefabs Instances", "This mode will store data for all instance in sync?", "Yes", "Cancel" ))
                    {   Hierarchy_GUI.Instance(A).PrefabIDMode = Hierarchy_GUI.PrefabIDModeEnum.MergedInstances;
                        Hierarchy_GUI.SetDirtyObject( A );
                        Adapter.RequestScriptReload();
                    }
                }
            }
            if (Hierarchy_GUI.Instance(A).PrefabIDMode == Hierarchy_GUI.PrefabIDModeEnum.MergedInstances) GUI.DrawTexture( R2, A.GetIcon( "BUTBLUE" ) );
            EndHorizontal();
            HelpBox( "Merged means all data for all instances of one corresponding object will stored in sync. Changes applied to one instance are automatically applied to others.", MessageType.None );
            
            HR();
            // Space( 30 );
            DrawNew();
            //HelpBox( "Data Settings", MessageType.Info );
            LABEL(GetControlRect(EditorGUIUtility.singleLineHeight), "<b>Data Settings:</b>");
            
            if (A.tempAdapterDisableCache)
                HelpBox( "Please Fix Compile Errors To Enable Cache", MessageType.Error );
                
            /*   var R = GET_OFFSETRECT(EditorGUIUtility.singleLineHeight);
            if (Event.current.type == EventType.repaint && GUI.enabled) SETUP_GREENLINE_HORISONTAL.Draw(new Rect(0, R.y + 7, 20 + PAD, 6), false, false, false, false);
            var LAST_GREEN = BEGIN_GREENLINE(R, Hierarchy.par.ENABLE_LEFTDOCK_FIX);
            Hierarchy.par.ENABLE_LEFTDOCK_FIX = TOOGLE_LEFT(R, "Enable Left Panel", Hierarchy.par.ENABLE_LEFTDOCK_FIX);*/
            
            //  DrawNew(40);
            BeginHorizontal();
            
            R1 = GetControlRect( (40), 2 );
            var on = GUI.enabled;
            /*  GUI.enabled = false;
              if (GUI.Button( R1, "Save in scene\nAdd registrator" ) && GET_STATE() != 0)
              {   if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                  {   if (EditorUtility.DisplayDialog( "Enable Registrator", "This mode will add scripts with a cache for GameObjects, are you sure?", "Yes", "Cancel" ))
                      {   Hierarchy_GUI.Instance( A ).SaveToScriptableObject = "SCENE";
                          Hierarchy_GUI.Instance( A ).USE_REGISTRATOR = true;
                          Hierarchy_GUI.SetDirtyObject( A );
                          Adapter.RequestScriptReload();
                      }
                  }
              }
              if (GET_STATE() == 0) GUI.DrawTexture( R1, A.GetIcon( "BUTBLUE" ) );
              GUI.enabled = on;
            
              R2 = R1;
              R2.x += R2.width;*/
            R2 = R1;
            if (GUI.Button( R2, "Cache in Scenes\n(Default)" ) && GET_STATE() != 1)
            {   if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                {   if (EditorUtility.DisplayDialog( "Save in scene", "Do you want to enable scene caching mode? ", "Yes", "Cancel" ))
                    {   //  A.par.DISABLE_DES = false;
                        Adapter.SWITCH_TO_SCENE_MODE(A);
                        
                    }
                }
            }
            if (GET_STATE() == 1) GUI.DrawTexture( R2, A.GetIcon( "BUTBLUE" ) );
            
            R3 = R2;
            R3.x += R3.width;
            if (GUI.Button( R3, "Cache in Folder\n(Experimental)" ) && GET_STATE() != 2)
            {   //EditorUtility.DisplayDialog( "Save in folder", "this function is not yet available it is in the test, it will be added to version 18.2 in the near future", "Cancel", "Cancel" );
            
#pragma warning disable
                if (Adapter.ALLOW_FOLDER_SAVER)
                {   if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    {   if (EditorUtility.DisplayDialog( "Save in folder",
                                                         "Every scene will save params in separate scriptableobject files, if you will need to copy data to another scene you will need to use copy/paste interface in inspector for scriptableobject file",
                                                         "Yes", "Cancel" ))
                        {   // A.par.DISABLE_DES = true;
                            Hierarchy_GUI.Instance( A ).SaveToScriptableObject = "FOLDER";
                            Hierarchy_GUI.Instance( A ).GETUSE_REGISTRATOR = false;
                            Hierarchy_GUI.SetDirtyObject( A );
                            Adapter.RequestScriptReload();
                        }
                    }
                }
                else
                {   EditorUtility.DisplayDialog( "Warning!",
                                                 "To use this feature you need to install \"Hierarchy PRO OpenSource\" version",
                                                 "Ok");
                }
#pragma warning restore
                
            }
            if (GET_STATE() == 2) GUI.DrawTexture( R3, A.GetIcon( "BUTBLUE" ) );
            
            EndHorizontal();
            
            
            var C1 = new GUIContent(
                @"- The plugin will automatically add scripts with the cache to the objects, if necessary.

- You can 'Duplicate' or create 'Instantiate' without losing the parameters of the hierarchy plugin, you can also save the settings for the prefab.

- Costs of this mode, are that you will have many scripts associated with the plugin.");

            var C2 = new GUIContent(@"- In default mode, in each scene will be created an object with plugin settings, you can hide or show it.

- You can 'Duplicate' object without losing the parameters of the hierarchy plugin, the plugin will store the entire cache in one place (DescriptionHelper object).

- The downside of this mode - presence of an object in the scene. If you send the scene to another machine which does not have plugin, you will have to send 'SharedFolder' also.
");

            var C3 = new GUIContent(@"- If you save data in ScriptableObject files, your data will be stored apart from the scenes, inside the plugin folder in the """ + A.PluginInternalFolder + @"/_ SAVED DATA""

- Benefits of this mode, are that you can keep clean scenes, and send it to those who do not have hierarchy plugin.

- The downside is, you will have to fix files names with parameters if you rename or move or duplicate scenes. Another shortcoming is that the initialization will take a little longer, although it takes milliseconds, you still have to know about it.

"

                                   );
                                   
                                   
            R1.y += R1.height;
            R2.y += R2.height;
            R3.y += R3.height;
            var startY = R1.y;
            
            
            R1.height = EditorStyles.helpBox.CalcHeight( C1, R1.width );
            R2.height = EditorStyles.helpBox.CalcHeight( C2, R2.width );
            R3.height = EditorStyles.helpBox.CalcHeight( C3, R3.width );
            /*   R1.height = 240;
            R2.height = 150;
            R3.height = 200;*/
            
            /* var TD = GUI.enabled;
              GUI.enabled = GET_STATE() == 0;
              EditorGUI.HelpBox( R1, C1.text, MessageType.None );
              R1.y += R1.height + 5;
              R1.height = EditorGUIUtility.singleLineHeight;
              GUI.Label( R1, "DescriptionHelper" );
              R1.y += R1.height;
              R1.height = 20;
              var d = A.TOGGLE_LEFT(R1, "Hide Helper", A.par.HIDE_DES);
              if (d != A.par.HIDE_DES)
              {   A.par.HIDE_DES = d;
                  #if !PROJECT
                  Hierarchy.M_Descript.UpdateFlags();
                  #endif
                  Adapter. RequestScriptReload();
              }
              R1.y += R1.height;
            
              GUI.enabled = TD;*/
            
            
            
            var  TD = GUI.enabled;
            GUI.enabled = GET_STATE() == 1;
            EditorGUI.HelpBox( R2, C2.text, MessageType.None );
            R2.y += R2.height + 5;
            R2.height = EditorGUIUtility.singleLineHeight;
            GUI.Label( R2, "DescriptionHelper" );
            R2.y += R2.height;
            R2.height = 20;
            var  d = A.TOGGLE_LEFT( R2, "Hide Helper", A.par.HIDE_DES );
            if (d != A.par.HIDE_DES)
            {   A.par.HIDE_DES = d;
                #if !PROJECT
                Hierarchy.M_Descript.UpdateFlags();
                #endif
                Adapter.RequestScriptReload();
            }
            R2.y += R2.height;
            
            GUI.enabled = TD;
            
            
            
            TD = GUI.enabled;
            // GUI.enabled &= GET_STATE() == 2;
            GUI.enabled = GET_STATE() == 2;
            EditorGUI.HelpBox( R3, C3.text, MessageType.None );
            R3.y += R3.height;
            
            /*
             R3.height = 40;
             if (GUI.Button( R3, "Cache Manager", A.SETUP_BUTTON ))
             {
               ScenesScanner.Init( A );
             }
             R3.y += R3.height;
             R3.height = 60;
             EditorGUI.HelpBox( R3, @"If you have already used the hierarchy plugin and saved some data in scenes, you can clear the created cache.", MessageType.None );
             R3.y += R3.height;*/
            GUI.enabled = TD;
            
            
            
            Space( Math.Max( Math.Max( R1.y, R2.y ), R3.y ) - startY );
            
            
            Space( 10 );
            Adapter.DrawRect( GetControlRect( 15 ), new Color( 0.4f, 0.1f, 0.1f, 0.3f ) );
            
            R3 = GetControlRect( 40 );
            R3.width = R1.width * 2 / 3;
            if (GUI.Button( R3, "Cache Manager", A.SETUP_BUTTON ))
            {   _S___ScenesScannerWindow.Init( A );
            }
            R3.x += R3.width;
            R3.width = R3.width * 2;
            EditorGUI.HelpBox( R3, @"If you have already used the hierarchy plugin and saved some data in scenes, you can clear the created cache.", MessageType.None );
            
            Space( 30 );
            /*  Hierarchy.par.ENABLE_REGISTRATORT = TOOGLE_LEFT("ENABLE_REGISTRATORT", Hierarchy.par.ENABLE_REGISTRATORT);
            Hierarchy.DISABLE_DES() = TOOGLE_LEFT("ENABLE_REGISTRATORT", Hierarchy.DISABLE_DES());*/
            
            HelpBox( "Any data will not be included in the build", MessageType.Info );
            
            
            
            END_CATEGORY( ref caclCACHE[I] );
            
            
            
            if (GetControlRect( 0 ).y > outY) outY = GetControlRect( 0 ).y;
            
        }
        
        
        
        
        
        //**  ****  **//
        //**  ****  **//
        //**  ****  **//
        if (GetControlRect( 0 ).y > outY) outY = GetControlRect( 0 ).y;
        
    }//ADD_Cache/GIT
    
}
}
