
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



public class _6__BottomWindow_HyperGraphWindow : EditorWindow {
    // internal  Action OnGUIV;
    _6__BottomWindow_HyperGraphWindow hyperwindow
    {   get { return adapter.bottomInterface.hyperGraph.editorWindow as _6__BottomWindow_HyperGraphWindow; }
        set { adapter.bottomInterface.hyperGraph.editorWindow = value; }
        
    }
    
    
    Adapter.MOUSE_RAW_UP_HELPER  mouse_uo_helper = new Adapter.MOUSE_RAW_UP_HELPER();
    internal void PUSH_ONMOUSEUP( Action ac )
    {   mouse_uo_helper.PUSH_ONMOUSEUP( ac, this );
    }
    
    
    internal static void ShowW(Adapter adapter)
    {   foreach (var w in Resources.FindObjectsOfTypeAll<_6__BottomWindow_HyperGraphWindow>().Where( w => w.adapter == adapter )) w.Close();
        var wd = EditorWindow.GetWindow<_6__BottomWindow_HyperGraphWindow>( "HyperGraph - " + adapter.pluginname);
        wd.adapter = adapter;
        adapter.bottomInterface.hyperGraph.editorWindow = wd;
        /* hyperwindow = EditorWindow.CreateInstance<HyperWindow>();
         hyperwindow.Show();*/
        //var window = GetWindow<SelectTargetGoWindow<T>>();
        // window.ShowUtility();
        //hyperwindow.
        wd.ShowAuxWindow();
        wd.wasInit = false;
    }
    bool wasInit = true;
    Adapter __adapter;
    Adapter adapter
    {   get
        {   if (__adapter == null) __adapter = Initializator.AdaptersByName.First( a => titleContent.text.EndsWith( a.Key ) ).Value;
            return __adapter;
        }
        set { __adapter = value; }
    }
    /*    float scroll;*/
    //   float offset;
    float? oldHeight;
    float? oldWidth;
    
    void OnDestroy()
    {   adapter.bottomInterface.hyperGraph.Repaint();
    }
    
    internal static void CHECK_MOUSE_UP(Adapter adapter)
    {
    
    
        foreach ( var w in adapter.bottomInterface.WindowController )
        {   var asd =  w.REFERENCE_WINDOW as _6__BottomWindow_BottomInterfaceWindow;
            if ( asd) asd.EVENT_MOUSE_UP_CLEAR();
        }
        
        
        
        if (adapter.bottomInterface.hyperGraph.WindowHyperController.currentAction != null)
        {   adapter.bottomInterface.hyperGraph.WindowHyperController.currentAction( true, adapter.deltaTime );
            adapter.bottomInterface.ClearAction();
            GUIUtility.hotControl = 0;
            Adapter.EventUse();
            adapter.bottomInterface.hyperGraph.Repaint();
        }
        if ( adapter.bottomInterface.favorGraph.WindowFavorController.currentAction != null )
        {   adapter.bottomInterface.favorGraph.WindowFavorController.currentAction( true, adapter.deltaTime );
            adapter.bottomInterface.ClearAction();
            // MonoBehaviour.print( "ASD" );
            GUIUtility.hotControl = 0;
            Adapter.EventUse();
            adapter.bottomInterface.hyperGraph.Repaint();
        }
        
    }
    bool mayscroll;
    public void OnGUI()
    {
    
    
    
    
    
        if (!hyperwindow)
        {   hyperwindow = this;
            if (!hyperwindow) return;
            adapter.bottomInterface.hyperGraph.CHECK_SCAN();
        }
        
        
        if (!adapter.par.ENABLE_ALL || !adapter.par.ENABLE_BOTTOMDOCK)
        {   GUI.Label( new Rect( 0, 0, position.width, position.height ), "Cache Disabled!" );
            return;
        }
        
        mouse_uo_helper.Invoke();
        
        
        if (Event.current.type == EventType.ScrollWheel && new Rect( 0, 0, hyperwindow.position.width, hyperwindow.position.height ).Contains( Event.current.mousePosition ))
        {   if (mayscroll)
            {   if (adapter.OnScroll != null) adapter.OnScroll( Adapter.ScrollType.HyperGraphScroll_Window, Event.current.delta.y );
                mayscroll = false;
            }
        }
        if (Event.current.type == EventType.Repaint)
        {   mayscroll = true;
        }
        
        
        
        
        if (!oldHeight.HasValue) oldHeight = hyperwindow.position.height;
        if (oldHeight.Value != hyperwindow.position.height)
        {   var oldH = oldHeight.Value;
            oldHeight = hyperwindow.position.height;
            //  controller.HEIGHT = (startHeight + (startPos.y - p.y));
            //  CHECK_HEIGHT();
            adapter.bottomInterface.hyperGraph.WindowHyperController.scrollPos.y -= (oldH - adapter.bottomInterface.hyperGraph.WindowHyperController.HEIGHT) / 2;
            // Hierarchy.BottomInterface.HyperGraph.RESET_SMOOTH_HEIGHT();
        }
        
        
        if (!oldWidth.HasValue) oldWidth = hyperwindow.position.width;
        if (oldWidth.Value != hyperwindow.position.width)
        {   var oldW = oldWidth.Value;
            oldWidth = hyperwindow.position.width;
            //  controller.HEIGHT = (startHeight + (startPos.y - p.y));
            //  CHECK_HEIGHT();
            adapter.bottomInterface.hyperGraph.WindowHyperController.scrollPos.x -= (oldW - adapter.bottomInterface.hyperGraph.WindowHyperController.WIDTH) / 2;
            // Hierarchy.BottomInterface.HyperGraph.RESET_SMOOTH_HEIGHT();
        }
        
        
        
        if (adapter.bottomInterface.hyperGraph.WindowHyperController.currentAction != null)
        {   if (!adapter.bottomInterface.hyperGraph.WindowHyperController.currentAction( false, adapter.deltaTime ))
                adapter.bottomInterface.hyperGraph.Repaint();
        }
        
        
        if (Event.current.rawType == EventType.MouseUp)
        {   CHECK_MOUSE_UP( adapter );
        }
        
        
        
        
        
        
        
        
        
        
        if (!wasInit)
        {   if (Event.current.type == EventType.Repaint)
            {   // MonoBehaviour.print(hyperwindow.position);
                if (hyperwindow.position.x < 15 && hyperwindow.position.y < 50)
                {   var p = hyperwindow.position;
                    p.x =  Adapter.MAX_WINDOW_WIDTH.x + (Adapter.MAX_WINDOW_WIDTH.y - p.width) / 2;
                    p.y = Adapter.MAX_WINDOW_HEIGHT.x +  (Adapter.MAX_WINDOW_HEIGHT.y - p.height) / 2;
                    hyperwindow.position = p;
                }
                wasInit = true;
            }
            // return;
            
        }
        
        adapter.ChangeGUI();
        try
        {   adapter.bottomInterface.hyperGraph.EXTERNAL_HYPER_DRAWER( new Rect( 0, 0, hyperwindow.position.width, hyperwindow.position.height ),
                    adapter.bottomInterface.hyperGraph.WindowHyperController, this);
        }
        catch (Exception ex)
        {   adapter.logProxy.LogError( "HyperWindow - " + ex.Message + " " + ex.StackTrace );
        }
        adapter.RestoreGUI();
        
    }
}

}