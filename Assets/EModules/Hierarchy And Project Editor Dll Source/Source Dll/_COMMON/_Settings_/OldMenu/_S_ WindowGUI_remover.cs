using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
#if PROJECT
    using EModules.Project;
#endif
//namespace EModules



    namespace EModules.EModulesInternal

{

    internal partial class SETUPROOT {
    
        internal void DrawRemove()
        
        {   if (A.IS_HIERARCHY()) DrawRemoveHierarchy();
            else DrawRemoveProject();
        }
        
        internal void DrawRemoveProject()
        {   //DrawHeader( "Just remove folder " + A.PluginInternalFolder );
        
        }
        
        internal void DrawRemoveHierarchy()
        {   //  Space( 35 );
        
            var c = GUI.color;
            GUI.color *= new Color( 1, 1, 1, 1f );
            GUI.enabled = true;
            
            
            BEGIN_CATEGORY( ref calcREMOVE, false, new Color32( 0, 0, 0, 40 ) );
            DrawHeader( "Variants to remove the plugin", true);
            // DrawNew();
            
            LABEL(GetControlRect(EditorGUIUtility.singleLineHeight * 2), "If you used <B>Cache in Folder</b> mode you can simply\n remove the plugin folder");
            HR();
            LABEL(GetControlRect(EditorGUIUtility.singleLineHeight * 2), "If you used <B>Cache in Scenes</b> you can choose\n one of these methods:");
            // var R = GET_OFFSETRECT(16);
            
            
            
            // Label("Ways to remove the plugin");
            var R = GetControlRect( 40 );
            A.ChangeGUI();
            EditorGUIUtility.AddCursorRect(R, MouseCursor.Link);
            var b = GUI.Button( R, "Save Remove Partially" );
            A.RestoreGUI();
            if (b)
            {   REMOVER.CreateWizard( A, 0 );
            }
            HelpBox( "This option saves 'SharedFolder' with DLL library if you have a cache in any scene.", MessageType.None );
            // HelpBox("This method will not affect one library that stores settings for scenes", MessageType.Warning);
            R = GetControlRect( 40 );
            A.ChangeGUI();
            EditorGUIUtility.AddCursorRect(R, MouseCursor.Link);
            b = GUI.Button( R, "Remove and Create 'ClearDataHelper.cs'" );
            A.RestoreGUI();
            if (b)
            {   REMOVER.CreateWizard( A, 1 );
            }
            HelpBox( "This option create a script that will automatically remove the cache in opened scenes, if you have a cache in any scene.", MessageType.None );
            R = GetControlRect( 40 );
            A.ChangeGUI();
            EditorGUIUtility.AddCursorRect(R, MouseCursor.Link);
            b = GUI.Button( R, "Clean Cache and Remove Immediately" );
            A.RestoreGUI();
            if (b)
            {   if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                {   A.par.ENABLE_ALL = false;
                    OnEnableChange();
                    _S___ScenesScannerWindow.Init( A, () =>
                    {   REMOVER.CreateWizard( A, 3 );
                    } );
                }
            }
            HelpBox( "This will remove all hierarchy data that was saved is scenes using Clear Cache Manager", MessageType.None );
            // HelpBox("This will cause warnings \"missing component\" in the scenes in which the hierarchy data was stored", MessageType.Error);
            // HelpBox("If you want to permanently remove the plug-in, it's better to use the second way", MessageType.None);
            
            END_CATEGORY( ref calcREMOVE );
            
            
            GUI.color = c;
        }
        
        
      internal  class REMOVER : ScriptableObject {
            public static void CreateWizard(Adapter adapter, int mode)
            {   if (EditorUtility.DisplayDialog( "Remove Hierarchy Plugin", "Do you want to delete Hierarchy Plugin ?", "Yes", "No" ))
                {   if (mode == 3)
                    {   //if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
                    }
                    
                    adapter.Destroy();
                    
                    if (mode == 3)
                    {   /*var lastScenes = EditorSceneManager.GetActiveScene();
                        foreach (var path in AssetDatabase.GetAllAssetPaths().Where(p => p.EndsWith(".unity")))
                        {
                            try
                            {
                                var loaded = EditorSceneManager.OpenScene(path, OpenSceneMode.Single);
                                foreach (var t in loaded.GetRootGameObjects().Where(t => t.name == "DescriptionHelperObject"))
                                    UnityEngine.Object.DestroyImmediate(t.gameObject, true);
                                EditorSceneManager.SaveScene(loaded);
                        #pragma warning disable
                                EditorSceneManager.UnloadScene(loaded);
                        #pragma warning restore
                            }
                            catch
                            {
                            }
                        }
                        if (!string.IsNullOrEmpty(lastScenes.path)) EditorSceneManager.OpenScene(lastScenes.path, OpenSceneMode.Single);*/
                    }
                    
                    //string FOLDER = "Plugins/EModules/Hierarchy";
                    
                    
                    if (Directory.Exists( adapter.PluginExternalFolder ))
                    {   foreach (var file in Directory.GetFiles( adapter.PluginExternalFolder, "*.*", SearchOption.AllDirectories ))
                        {   if (mode == 0 && file.EndsWith( "Hierarchy (MAYA outliner) Plugin.dll" )) continue;
                            if (mode == 0 && file.EndsWith( "Hierarchy (MAYA outliner) Plugin.dll.meta" )) continue;
                            if (mode == 0 && file.EndsWith( "DescriptionHelper.cs" )) continue;
                            if (mode == 0 && file.EndsWith( "DescriptionHelper.cs.meta" )) continue;
                            if (mode == 0 && file.EndsWith( "DescriptionRegistrator.cs" )) continue;
                            if (mode == 0 && file.EndsWith( "DescriptionRegistrator.cs.meta" )) continue;
                            File.Delete( file );
                            File.Delete( file + ".meta" );
                        }
                        var dirOrdered = Directory.GetDirectories( adapter.PluginExternalFolder, "*.*", SearchOption.AllDirectories ).OrderByDescending( d => d.Length );
                        foreach (var dir in dirOrdered)
                        {   if (Directory.GetFiles( dir, "*.*", SearchOption.AllDirectories ).Length == 0)
                            {   Directory.Delete( dir );
                                File.Delete( dir + ".meta" );
                            }
                        }
                        
                        
                    }
                    
                    
                    
                    if (mode == 1)
                    {   Adapter.CHECK_FOLDERS( adapter );
                        File.WriteAllText( adapter.PluginExternalFolder + "/HierarchyClearSavedDataHelper.cs",
                                           @"using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;


[InitializeOnLoad]
public  class HierarchyClearSavedDataHelper
{
     HierarchyClearSavedDataHelper()
    {
        EditorApplication.update += Update;
    }
     string ScenePath;
    private  void Update()
    {
        if (Application.isPlaying) return;
        if (ScenePath != EditorSceneManager.GetActiveScene().path)
        {
            ScenePath = EditorSceneManager.GetActiveScene().path;
            foreach (var t in Resources.FindObjectsOfTypeAll<Transform>().Where(t => t.name == ""DescriptionHelperObject"" && t.gameObject.scene.IsValid() && t.GetComponents<Component>().Any(c => !c)))
                UnityEngine.Object.DestroyImmediate(t.gameObject, true);
        }
    }
}
" );
                    }
                    
                    
                    var internalFolder = adapter.PluginInternalFolder + '/';
                    var externalFolder = adapter.PluginExternalFolder + '/';
                    
                    
                    do
                    {   internalFolder = internalFolder.Remove( internalFolder.LastIndexOf( '/' ) );
                        externalFolder = externalFolder.Remove( externalFolder.LastIndexOf( '/' ) );
                        
                        if (Directory.Exists( externalFolder )
                                && Directory.GetFiles( externalFolder, "*.*", SearchOption.AllDirectories ).Length == 0)
                        {   FileUtil.DeleteFileOrDirectory( internalFolder );
                            FileUtil.DeleteFileOrDirectory( internalFolder + ".meta" );
                        }
                        
                    } while (internalFolder.Contains( '/' ));
                    
                    
                    
                    
                    
                    
                    
                    /*   if (Directory.Exists(Application.dataPath + "/Plugins/EModules")
                           && Directory.GetFiles(Application.dataPath + "/Plugins/EModules", "*.*", SearchOption.AllDirectories).Length == 0)
                       {
                           // MonoBehaviour.print("ASD");
                           FileUtil.DeleteFileOrDirectory("Assets/Plugins/EModules");
                           FileUtil.DeleteFileOrDirectory("Assets/Plugins/emem.storeta");
                       }
                       if (Directory.Exists(Application.dataPath + "/Plugins")
                       && Directory.GetFiles(Application.dataPath + "/Plugins", "*.*", SearchOption.AllDirectories).Length == 0)
                       {
                           FileUtil.DeleteFileOrDirectory("Assets/Plugins");
                           FileUtil.DeleteFileOrDirectory("Assets/Plugins.meta");
                       }*/
                    /* if (Directory.Exists(Application.dataPath + "/Editor")
                     && Directory.GetFiles(Application.dataPath + "/Editor").Length == 0)
                     {
                         FileUtil.DeleteFileOrDirectory("Assets/Plugins");
                         FileUtil.DeleteFileOrDirectory("Assets/Plugins.meta");
                     }*/
                    
                    // AssetDatabase.StopAssetEditing();
                    AssetDatabase.Refresh();
                    
                    var pref = Resources.FindObjectsOfTypeAll( typeof( EditorWindow ) ).FirstOrDefault( w => w.GetType().FullName == "UnityEditor.PreferencesWindow" );
                    var hierWin = pref as EditorWindow;
                    if (hierWin != null) hierWin.Close();
                }
                
            }
        }
        
        
    }
    
    
}
