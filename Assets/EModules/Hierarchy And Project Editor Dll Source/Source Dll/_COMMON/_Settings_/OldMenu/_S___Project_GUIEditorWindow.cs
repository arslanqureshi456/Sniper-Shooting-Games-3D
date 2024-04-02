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
#if PROJECT
    using EModules.Project;
#endif
//namespace EModules

namespace EModules.EModulesInternal {





public class _S___Project_GUIEditorWindow : EditorWindow {
    const string PLUGINNAME = Initializator.PROJECT_NAME;
    
    
    public void Update__()
    {   PROJ_SETUP.module.editor_updated();
    }
    
    private void OnEnable()
    {   Undo.undoRedoPerformed -= repaint;
        Undo.undoRedoPerformed += repaint;
        EditorApplication.update -= Update__;
        EditorApplication.update += Update__;
    }
    private void OnDestroy()
    {   Undo.undoRedoPerformed -= repaint;
        EditorApplication.update -= Update__;
    }
    void repaint()
    {   if (this) Repaint();
    }
    
    static _S___Project_GUIEditorWindow w;
    
    
    public void OnGUI()
    {   //  Initializator.Adapters[PLUGINNAME].drawGUIcaller( false , module );
        if (PROJ_SETUP.module == null) return;
        GUI.Box(new Rect(0, 0, position.width, position.height), "");
        PROJ_SETUP.module.OnGUI( this, PROJ_SETUP.module );
    }
    
    
    
    internal const int P = 2000;
    
    #if PROJECT
    //   [MenuItem( "Window/" + PLUGINNAME + " Plugin/", false, P + 5 )]
    //  [MenuItem( "Window/" + PLUGINNAME + " Plugin/Old Settings Window", false, P + 5 )]
    // [MenuItem("as",false,1,")]
    internal static void _showWindow()
    {   if (!Initializator.AdaptersByName.ContainsKey( PLUGINNAME )) return;
    
        Hierarchy_GUI.Instance( Initializator.AdaptersByName[PLUGINNAME] );
        try
        {   foreach (var w1 in Resources.FindObjectsOfTypeAll<_S___Project_GUIEditorWindow>()) ((EditorWindow)w1).Close();
            w = EditorWindow.GetWindow<_S___Project_GUIEditorWindow>( false, PLUGINNAME + " Plugin", true );
            w.minSize = new Vector2( PROJ_SETUP.W  + 30, Adapter.MAX_WINDOW_HEIGHT.y / 3 );
            w.maxSize = new Vector2( PROJ_SETUP.W * 3 + 30, Adapter.MAX_WINDOW_HEIGHT.y );
            // w = SETUPROOTWindow.GetWindow( Initializator.HIERARCHY_NAME , typeof( SETUPROOTWindow ) );
            w.Show();
            
            if (!EditorPrefs.GetBool( PLUGINNAME + "/WSinit", false ))
            {   var p = w.position;
                p.height = Adapter.MAX_WINDOW_HEIGHT.y / 1.5f;
                p.width = PROJ_SETUP.W * 3 + 30;
                p.x =  Adapter.MAX_WINDOW_WIDTH.x + Adapter.MAX_WINDOW_WIDTH.y / 2 - p.width / 2;
                p.y = Adapter.MAX_WINDOW_HEIGHT.x + Adapter.MAX_WINDOW_HEIGHT.y / 2 - p.height / 2;
                w.position = p;
                EditorPrefs.SetBool( PLUGINNAME + "/WSinit", true );
            }
        }
        catch { }
    }
    
    /* [PreferenceItem( PLUGINNAME )]
     internal void drawGUI()
     {   GUILayout.Label( "Project Plugin" );
         GUILayout.Label( "- Window/Project Plugin/Settings" );
         if (GUILayout.Button( "Open Plugin Settings" ))
         {   showWindow();
         }
         // OnGUI();
         //  if (PROJ_SETUP.module == null) return;
    
         // PROJ_SETUP.module.drawGUIcaller(true, PROJ_SETUP.module);
     }*/
    #endif
}


}
