
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


/*
    internal partial class Hierarchy
    {
        internal partial class BottomInterface
        {
*/


public class _6__BottomWindow_FavoritWindow : EditorWindow {

    _6__BottomWindow_FavoritWindow favorwindow
    {   get
        {   return adapter.bottomInterface.favorGraph.editorWindow as _6__BottomWindow_FavoritWindow;
        }
        set
        {   adapter.bottomInterface.favorGraph.editorWindow = value;
        }
        
    }
    
    
    Adapter.MOUSE_RAW_UP_HELPER  mouse_uo_helper = new Adapter.MOUSE_RAW_UP_HELPER();
    internal void PUSH_ONMOUSEUP( Action ac )
    {   mouse_uo_helper.PUSH_ONMOUSEUP( ac, this );
    }
    
    
    internal void ReloadScript()
    {   wasInit = false;
    }
    
    internal static void ShowW(Adapter adapter)
    {   foreach (var w in Resources.FindObjectsOfTypeAll<_6__BottomWindow_FavoritWindow>().Where( w => w.adapter == adapter )) w.Close();
        var wd = EditorWindow.GetWindow<_6__BottomWindow_FavoritWindow>( "≡ " + adapter.pluginname + "Fav - " + adapter.pluginname);
        wd.adapter = adapter;
        adapter.bottomInterface.favorGraph.editorWindow = wd;
        
        wd.ShowAuxWindow();
        wd.wasInit = false;
    }
    bool wasInit = true;
    Adapter __adapter;
    Adapter adapter
    {   get
        {   if (__adapter == null)
            {
            
                var c = titleContent;
                var adapterName = c.text.Substring(c.text.LastIndexOf(' ') + 1);
                if (!Initializator.AdaptersByName.ContainsKey( adapterName )) return null;
                adapter = Initializator.AdaptersByName[adapterName];
                // __adapter = Initializator.Adapters.First( a => titleContent.text.EndsWith( a.Key ) ).Value;
                
                __adapter.onDidReloadScript -= ReloadScript;
                __adapter.onDidReloadScript += ReloadScript;
            }
            
            return __adapter;
        }
        set { __adapter = value; }
    }
    
    /*    float scroll;*/
    //   float offset;
    float? oldHeight;
    float? oldWidth;
    
    void OnDestroy()
    {   adapter.bottomInterface.favorGraph.Repaint();
    }
    
    internal static void CHECK_MOUSE_UP(Adapter adapter)
    {   if (adapter.bottomInterface.favorGraph.WindowFavorController.currentAction != null)
        {   adapter.bottomInterface.favorGraph.WindowFavorController.currentAction( true, adapter.deltaTime );
            adapter.bottomInterface.ClearAction();
            
            GUIUtility.hotControl = 0;
            Adapter.EventUse();
            
            adapter.bottomInterface.favorGraph.Repaint();
        }
    }
    bool mayscroll;
    private Adapter.BottomInterface.FavorControllerWindow current_controller;
    
    
    
    
    public void OnGUI()
    {
    
    
    
        if (adapter == null) return;
        
        if (!favorwindow)
        {   favorwindow = this;
            if (!favorwindow) return;
            adapter.onDidReloadScript -= ReloadScript;
            adapter.onDidReloadScript += ReloadScript;
        }
        
        
        
        
        
        if (!adapter.par.ENABLE_ALL || !adapter.par.ENABLE_BOTTOMDOCK)
        {   GUI.Label( new Rect( 0, 0, position.width, position.height ), "Cache Disabled!" );
            return;
        }
        
        
        
        
        
        
        
        /*  if (!bottomInterfaceWindow)
          {
              bottomInterfaceWindow = this;
              if (!bottomInterfaceWindow) return;
        
        
        
              // Hierarchy.BottomInterface.HyperGraph.CHECK_SCAN();
          }*/
        
        
        if (current_controller == null)
        {   adapter.bottomInterface.FavoritControllers.RemoveAll( w => !w.REFERENCE_WINDOW || w.REFERENCE_WINDOW == this );
            //  current_controller =  new BottomControllerWindow( adapter ) { REFERENCE_WINDOW = this };
            current_controller = adapter.bottomInterface.favorGraph.WindowFavorController;
            current_controller.REFERENCE_WINDOW = this;
            adapter.bottomInterface.FavoritControllers.Add( current_controller );
        }
        
        
        
        
        
        
        
        if (Event.current.type == EventType.ScrollWheel && new Rect( 0, 0, favorwindow.position.width, favorwindow.position.height ).Contains( Event.current.mousePosition ))
        {   if (mayscroll)
            {   if (adapter.OnScroll != null) adapter.OnScroll( Adapter.ScrollType.FavorGraphScroll_Window, Event.current.delta.y );
                mayscroll = false;
            }
        }
        if (Event.current.type == EventType.Repaint)
        {   mayscroll = true;
        }
        
        
        
        
        /* if (!oldHeight.HasValue) oldHeight = favorwindow.position.height;
         if (oldHeight.Value != favorwindow.position.height)
         {
           var oldH = oldHeight.Value;
           oldHeight = favorwindow.position.height;
           //  controller.HEIGHT = (startHeight + (startPos.y - p.y));
           //  CHECK_HEIGHT();
           adapter.bottomInterface.favorGraph.WindowFavorController.scrollPos.y -= (oldH - adapter.bottomInterface.favorGraph.WindowFavorController.HEIGHT) / 2;
           // Hierarchy.BottomInterface.favorGraph.RESET_SMOOTH_HEIGHT();
         }
        
        
         if (!oldWidth.HasValue) oldWidth = favorwindow.position.width;
         if (oldWidth.Value != favorwindow.position.width)
         {
           var oldW = oldWidth.Value;
           oldWidth = favorwindow.position.width;
           //  controller.HEIGHT = (startHeight + (startPos.y - p.y));
           //  CHECK_HEIGHT();
           adapter.bottomInterface.favorGraph.WindowFavorController.scrollPos.x -= (oldW - adapter.bottomInterface.favorGraph.WindowFavorController.WIDTH) / 2;
           // Hierarchy.BottomInterface.favorGraph.RESET_SMOOTH_HEIGHT();
         }*/
        
        
        
        if (adapter.bottomInterface.favorGraph.WindowFavorController.currentAction != null)
        {   if (!adapter.bottomInterface.favorGraph.WindowFavorController.currentAction( false, adapter.deltaTime ))
                adapter.bottomInterface.favorGraph.Repaint();
                
                
        }
        
        
        if (Event.current.rawType == EventType.MouseUp)
        {   CHECK_MOUSE_UP( adapter );
        }
        
        
        
        
        
        
        
        
        
        
        if (!wasInit)
        {   if (Event.current.type == EventType.Repaint)
            {   if (favorwindow.position.x < 15 && favorwindow.position.y < 50)
                {   var p = favorwindow.position;
                    p.x =  Adapter.MAX_WINDOW_WIDTH.x + (Adapter.MAX_WINDOW_WIDTH.y - p.width) / 2;
                    p.y = Adapter.MAX_WINDOW_HEIGHT.x + (Adapter.MAX_WINDOW_HEIGHT.y - p.height) / 2;
                    favorwindow.position = p;
                }
                wasInit = true;
                Hierarchy.HierarchyAdapterInstance.need_ClearHierarchyObjects1 = true;
                //  adapter.ClearHierarchyObjects();
                adapter.RepaintAllViews();
            }
            // return;
            
        }
        
        adapter.ChangeGUI();
        try
        {   if (Event.current.type == EventType.Repaint) Adapter.GET_SKIN().textArea.Draw( new Rect( 0, -50, favorwindow.position.width,
                        favorwindow.position.height + 50 ), /*new GUIContent("Navigator"),*/ false, false, false, false );
            //  Adapter.FadeRect( new Rect( 0, 0, favorwindow.position.width, favorwindow.position.height ), 1 );
            adapter.bottomInterface.favorGraph.FAVORIT_GUI( new Rect( 0, 0, favorwindow.position.width, favorwindow.position.height ), adapter.bottomInterface.favorGraph.WindowFavorController );
        }
        catch (Exception ex)
        {   adapter.logProxy.LogError( "Favorites Manager - " + ex.Message + " " + ex.StackTrace );
        }
        adapter.RestoreGUI();
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        mouse_uo_helper.Invoke();
        
        
        
        
        
        
    }
    /*
    
    
        class EmptyCLass : EditorWindow
        {
    
    
    
          // internal  Action OnGUIV;
          //internal  BottomInterfaceWindow bottomInterfaceWindow;
    
          internal enum TYPE { CUSTOM = 0, LAST = 1, HIER = 2 }
    
          internal class FavoritesController_Window : Adapter.BottomInterface.BottomController
          {
            static GUIContent c;
    
            internal FavoritesController_Window(Adapter adapter, Action repaintAction) : base( adapter )
            {
              this.repaintAction = repaintAction;
            }
    
            internal override bool CheckCategoryIndex(Scene scene) { throw new NotImplementedException(); }
            internal override int GetCategoryIndex(Scene scene) { throw new NotImplementedException(); }
            internal override string GetCurerentCategoryName() { throw new NotImplementedException(); }
            internal override void SetCategoryIndex(int index, Scene scene) { throw new NotImplementedException(); }
    
            Action repaintAction;
            public override void REPAINT(Adapter adapter)
            {
              if (repaintAction != null) repaintAction();
            }
          }
    
    
    
          internal static void ShowW(Adapter adapter, TYPE type, string categoryName)
          {
            //foreach (var w in Resources.FindObjectsOfTypeAll<BottomInterfaceWindow>().Where(w => w.current_type == type)) w.Close();
    
            var currentIndex = Resources.FindObjectsOfTypeAll<FavoritWindow>().Length  + 1;
    
            FavoritWindow currentWindow = null;
            currentWindow = (FavoritWindow)EditorWindow.CreateInstance<FavoritWindow>();
    
    
    
            //  bottomInterfaceWindow.mi
            currentWindow.current_type = type;
            currentWindow.adapter = adapter;
            currentWindow.wasInit = false;
    
            var c = currentWindow.titleContent;
            c.text = adapter.pluginname + " Favorites " + currentIndex;
            currentWindow.titleContent = c;
    
    
            adapter.bottomInterface.FavoritControllers.RemoveAll( w => !w.REFERENCE_WINDOW );
            currentWindow.current_controller = new FavoritesController_Window( adapter, currentWindow.REPAINT ) { REFERENCE_WINDOW = currentWindow };
            adapter.bottomInterface.FavoritControllers.Add( currentWindow.current_controller );
    
            currentWindow.minSize = new Vector2( 50, 50 );
    
            currentWindow.Show();
    
          }
          TYPE current_type;
          bool wasInit = true;
          Adapter adapter;
          / *    float scroll;* /
          //   float offset;
          / *   float? oldHeight;
             float? oldWidth;* /
          internal FavoritesController_Window current_controller;
    
          internal void REPAINT()
          {
    
            Repaint();
            adapter.RepaintWindowInUpdate();
    
          }
    
          void OnDestroy()
          {
            REPAINT();
          }
    
    
    
          public void OnGUI()
          {
    
            if (adapter == null)
            {
              var c = titleContent;
              var adapterName = c.text.Remove(c.text.IndexOf(' '));
              if (!Initializator.Adapters.ContainsKey( adapterName )) return;
              adapter = Initializator.Adapters[adapterName];
            }
    
    
            if (current_controller == null)
            {
              adapter.bottomInterface.FavoritControllers.RemoveAll( w => !w.REFERENCE_WINDOW );
              current_controller = new FavoritesController_Window( adapter, REPAINT ) { REFERENCE_WINDOW = this };
              adapter.bottomInterface.FavoritControllers.Add( current_controller );
            }
    
            if (!current_controller.REFERENCE_WINDOW) current_controller.REFERENCE_WINDOW = this;
    
    
            // adapter.bottomInterface._mFavoritesOnGUI( current_controller );
    
    
    
            Rect trect = new Rect();
    
    
            if (!wasInit)
            {
              if (Event.current.type == EventType.Repaint)
              {
                if (position.x < 15 && position.y < 50)
                {
                  var p = position;
                  p.x = (Screen.currentResolution.width - p.width) / 2;
                  p.y = (Screen.currentResolution.height - p.height) / 2;
                  position = p;
                }
                wasInit = true;
              }
    
            }
    
    
            if (!adapter.par.ENABLE_ALL || !adapter.par.ENABLE_BOTTOMDOCK || adapter.DISABLE_DES())
            {
              GUI.Label( new Rect( 0, 0, position.width, position.height ), "Cache Disabled!" );
              return;
            }
    
    
            adapter.ChangeGUI();
            try
            {
              trect.width = position.width;
              trect.height = position.height;
    
              current_controller.REFERENCE_WINDOW = this;
              current_controller.ModuleRect = trect;
              current_controller.CustomLineRect = trect;
    
    
              adapter.bottomInterface.FAVORIT_GUI( trect, current_controller, Adapter.LastActiveScene, false );
    
            }
            catch (Exception ex)
            {
              adapter.logProxy.LogError( "FavoritWindow - " + ex.Message + " " + ex.StackTrace );
            }
            adapter.RestoreGUI();
    
    
            if ((Event.current.type != EventType.Repaint && Event.current.type != EventType.Layout))
            {
              adapter.bottomInterface.EVENT_UPDATE( current_controller );
            }
    
            if (Event.current.rawType == EventType.MouseUp)
            {
              adapter.bottomInterface.EVENT_MOUSE_UP( current_controller );
            }
          }
    
        }
    
    */
    
    
}
}
