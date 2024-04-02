
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
internal partial class Adapter {

    internal static string STR_DLL( Adapter adapter ) { return adapter.pluginID == Initializator.HIERARCHY_ID ? "Hierarchy (MAYA outliner)" : "Project"; }
    
    static bool wasEMInternal = false;
    static  bool[] wasAdapterInternal = new bool[10];
    internal static void CHECK_FOLDERS( Adapter adapter )
    {
    
    
    
    
    
        if ( !wasAdapterInternal[adapter.pluginID] )
        {   wasAdapterInternal[adapter.pluginID] = true;
        
            string PAT = STR_DLL( adapter ) + " Plugin.dll";
            
            var sf = adapter.pluginID == 0 ? "/SharedFolder/" : "/Source/";
            
            if ( !File.Exists( adapter.PluginInternalFolder + sf + PAT ) )
            {
            
            
            
                var candidate = AssetDatabase.GetAllAssetPaths().Where(p => !string.IsNullOrEmpty(p) && p[p.Length - 1] == 'l').FirstOrDefault( a => a.EndsWith( PAT ) );
                if ( candidate != null )
                {   candidate = candidate.Replace( '\\', '/' );
                    candidate = candidate.Remove( candidate.LastIndexOf( '/' ) );
                    candidate = candidate.Remove( candidate.LastIndexOf( '/' ) );
                    adapter.PluginInternalFolder = candidate;
                }
                
                if ( adapter.PluginInternalFolder == null ) adapter.PluginInternalFolder = "Assets/Plugins/EModules/" + adapter.pluginname;
                adapter.PluginExternalFolder = UNITY_SYSTEM_PATH + adapter.PluginInternalFolder;
                CreateFolders( adapter.PluginInternalFolder );
            }
            
            
            
            if ( !wasEMInternal )
            {   wasEMInternal = true;
                EMInternalFolder = adapter.PluginInternalFolder.Remove( adapter.PluginInternalFolder.LastIndexOf( '/' ) );
            }
        }
        
        
    }
    
    
    
    
    internal static void CreateFolders( string path )
    {   var folder = "";
        var tempF = UNITY_SYSTEM_PATH;
        foreach ( var s in path.Split( '/' ) )
        {   if ( folder == "" )
            {   folder = s;
                continue;
            }
            // if ( !AssetDatabase.IsValidFolder( folder + '/' + s ) ) AssetDatabase.CreateFolder( folder , s );
            folder += '/' + s;
            if ( !Directory.Exists( tempF + folder ) ) Directory.CreateDirectory( tempF + folder );
        }
    }
    
    static bool wasChecked = false;
    internal static string CheckFiles( Adapter adapter )
    {
    
        if ( wasChecked ) return null;
        
        if ( !Directory.Exists( adapter.PluginExternalFolder + "/Source" ) ) Directory.CreateDirectory( adapter.PluginExternalFolder + "/Source" );
        // if (adapter.pluginID == Initializator.HIERARCHY_ID)
        {   if ( !Directory.Exists( adapter.PluginExternalFolder + "/SharedFolder" ) ) Directory.CreateDirectory( adapter.PluginExternalFolder + "/SharedFolder" );
        }
        
        
        
        var SAT = STR_DLL( adapter );
        
        //var result = false;
        
        string FFF = adapter.pluginID == Initializator.HIERARCHY_ID ? "SharedFolder" : "Source" ;
        
        //             {
        //                 result |= TRY_MOVE_FILE( adapter , "/Resources/DescriptionHelper.cs" , "/" + FFF + "/DescriptionHelper.cs" );
        //                 result |= TRY_MOVE_FILE( adapter , "/Resources/DescriptionRegistrator.cs" , "/" + FFF + "/DescriptionRegistrator.cs" );
        //                 result |= TRY_MOVE_FILE( adapter , "/Resources/" + SAT + " Plugin.dll" , "/" + FFF + "/" + SAT + " Plugin.dll" );
        //                 result |= TRY_MOVE_FILE( adapter , "/Resources/" + SAT + " Handler.dll" , "/" + FFF + "/" + SAT + " Handler.dll" );
        //                 result |= TRY_MOVE_FILE( adapter , "/Resources/" + SAT + " Editor.dll" , "/Source/" + SAT + " Editor.dll" );
        //             }
        //
        //             {
        //                 result |= TRY_REMOVE_FILE( adapter , "/Resources/DescriptionHelper.cs" );
        //                 result |= TRY_REMOVE_FILE( adapter , "/Resources/DescriptionRegistrator.cs" );
        //                 result |= TRY_REMOVE_FILE( adapter , "/Resources/" + SAT + " Plugin.dll" );
        //                 result |= TRY_REMOVE_FILE( adapter , "/Resources/" + SAT + " Handler.dll" );
        //                 result |= TRY_REMOVE_FILE( adapter , "/Resources/" + SAT + " Editor.dll" );
        //                 result |= TRY_REMOVE_FILE( adapter , "/Resources/" + STR_DATA( adapter ) );
        //             }
        
        
        
        
        
        /*HIER BEGIN*/
        if ( adapter.pluginID == Initializator.HIERARCHY_ID )
        {   if ( !File.Exists( adapter.PluginExternalFolder + "/SharedFolder/DescriptionHelper.cs" ) ) return adapter.PluginInternalFolder + "/SharedFolder/DescriptionHelper.cs";
            var loadedH = AssetDatabase.LoadAssetAtPath<MonoScript>( adapter.PluginInternalFolder + "/SharedFolder/DescriptionHelper.cs" );
            if ( loadedH == null || loadedH.GetClass() == null || !typeof( IHashProperty ).IsAssignableFrom( loadedH.GetClass() ) ) return adapter.PluginInternalFolder + "/SharedFolder/DescriptionHelper.cs";
            
            if ( !File.Exists( adapter.PluginExternalFolder + "/SharedFolder/DescriptionRegistrator.cs" ) ) return adapter.PluginInternalFolder + "/SharedFolder/DescriptionRegistrator.cs";
            loadedH = AssetDatabase.LoadAssetAtPath<MonoScript>( adapter.PluginInternalFolder + "/SharedFolder/DescriptionRegistrator.cs" );
            if ( loadedH == null || loadedH.GetClass() == null
                    || !typeof( IDescriptionRegistrator ).IsAssignableFrom( loadedH.GetClass() ) ) return adapter.PluginInternalFolder + "/SharedFolder/DescriptionRegistrator.cs";
        }
        /*HIER END*/
        
        
        
        
        #if !UNITY_EDITOR
        if (!File.Exists(adapter.PluginExternalFolder + "/Source/" + SAT + " Editor.dll")) return adapter.PluginInternalFolder + "/Source/" + SAT + " Editor.dll";
        #endif
        
        if ( adapter.pluginID == Initializator.HIERARCHY_ID )
        {   if ( !File.Exists( adapter.PluginExternalFolder + "/" + FFF + "/" + SAT + " Handler.dll" ) ) return adapter.PluginInternalFolder + "/" + FFF + "/" + SAT + " Handler.dll";
            if ( !File.Exists( adapter.PluginExternalFolder + "/SharedFolder/" + SAT + " Plugin.dll" ) ) return adapter.PluginInternalFolder + "/SharedFolder/" + SAT + " Plugin.dll";
        }
        else
        {   // if ( !File.Exists( adapter.PluginExternalFolder + "/Source/" + SAT + " Plugin.dll" ) ) return adapter.PluginInternalFolder + "/Source/" + SAT + " Plugin.dll";
        }
        
        //  if ( result ) RequestScriptReload();
        
        wasChecked = true;
        return null;
    }
    
    
    static bool TRY_MOVE_FILE( Adapter adapter, string SOURCE, string DESTINATION )
    {   if ( File.Exists( adapter.PluginExternalFolder + DESTINATION ) ) return false;
        if ( !File.Exists( adapter.PluginExternalFolder + SOURCE ) ) return false;
        
        // FileUtil.DeleteFileOrDirectory( adapter.PluginInternalFolder + "/Resources/" + STR_DATA( adapter ) );
        
        // FileUtil.DeleteFileOrDirectory( adapter.PluginInternalFolder + DESTINATION );
        //File.Delete( adapter.PluginExternalFolder + DESTINATION );
        // FileUtil.MoveFileOrDirectory( adapter.PluginInternalFolder + SOURCE , adapter.PluginInternalFolder + DESTINATION );
        File.Move( adapter.PluginExternalFolder + SOURCE, adapter.PluginExternalFolder + DESTINATION );
        
        // File.Move( adapter.PluginExternalFolder + SOURCE , adapter.PluginExternalFolder + DESTINATION );
        
        
        
        if ( File.Exists( adapter.PluginExternalFolder + SOURCE + ".meta" ) )     //  FileUtil.DeleteFileOrDirectory( adapter.PluginInternalFolder + DESTINATION + ".meta" );
        {   if ( File.Exists( adapter.PluginExternalFolder + DESTINATION + ".meta" ) ) File.Delete( adapter.PluginExternalFolder + DESTINATION + ".meta" );
            // FileUtil.MoveFileOrDirectory( adapter.PluginInternalFolder + SOURCE + ".meta" , DESTINATION + ".meta" );
            File.Move( adapter.PluginExternalFolder + SOURCE + ".meta", adapter.PluginExternalFolder + DESTINATION + ".meta" );
        }
        return true;
    }
    
    
    static bool TRY_REMOVE_FILE( Adapter adapter, string filename )
    {   if ( File.Exists( adapter.PluginExternalFolder + filename ) )     //  FileUtil.DeleteFileOrDirectory( adapter.PluginInternalFolder + filename );
        {   File.Delete( adapter.PluginExternalFolder + filename );
            if ( File.Exists( adapter.PluginExternalFolder + filename + ".meta" ) )     // FileUtil.DeleteFileOrDirectory( adapter.PluginInternalFolder + filename + ".meta" );
            {   File.Delete( adapter.PluginExternalFolder + filename + ".meta" );
                RequestScriptReload();
            }
            return true;
        }
        return false;
    }
}
}
