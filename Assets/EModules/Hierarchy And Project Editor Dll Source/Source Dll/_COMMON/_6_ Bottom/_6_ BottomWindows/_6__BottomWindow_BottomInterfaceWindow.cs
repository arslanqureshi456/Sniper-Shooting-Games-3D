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

public class _6__BottomWindow_BottomInterfaceWindow : EditorWindow
{
    // internal  Action OnGUIV;
    //internal  BottomInterfaceWindow bottomInterfaceWindow;

    internal enum TYPE
    {
        CUSTOM = 0,
        LAST = 1,
        HIER = 2
    }

    Adapter.MOUSE_RAW_UP_HELPER mouse_uo_helper = new Adapter.MOUSE_RAW_UP_HELPER();

    internal void PUSH_ONMOUSEUP(Action ac)
        {
        mouse_uo_helper.PUSH_ONMOUSEUP(ac, this);
        }


    internal class BottomControllerWindow : Adapter.BottomInterface.BottomController
    {
        static GUIContent c;

        internal BottomControllerWindow(Adapter adapter) : base(adapter) { }

        static List<Int32ListArray> list;


        internal override bool CheckCategoryIndex(int scene)
            {
            var r = GetCategoryIndex(scene, true);
            if (r < 0 || r >= list.Count) return false;
            return true;
            }

        internal override float WIDTH
            {
            get { return REFERENCE_WINDOW.position.width; }

            set { }
            }

        int? tempIndex;

        internal override int GetCategoryIndex(int scene)
            {
            return GetCategoryIndex(scene, false);
            }

        int GetCategoryIndex(int scene, bool raw)
            {
            c = REFERENCE_WINDOW.titleContent;
            var nn = GetCurerentCategoryName();
            adapter.bottomInterface.GET_BOOKMARKS(ref list, scene);

            for (int i = 0, L = list.Count; i < L; i++)
                {
                if (nn == list[i].name)
                    {
                    tempIndex = i;
                    return i;
                    }
                }

            if (raw) return tempIndex ?? 0;

            if (tempIndex.HasValue) return Mathf.Clamp(tempIndex.Value, 0, list.Count - 1);
            return 0;
            }

        internal override void SetCategoryIndex(int index, int scene)
            {
            var c = REFERENCE_WINDOW.titleContent;
            adapter.bottomInterface.GET_BOOKMARKS(ref list, scene);
            c.text = list[index].name + " " + adapter.pluginname;
            REFERENCE_WINDOW.titleContent = c;
            tempIndex = index;
            REFERENCE_WINDOW.Repaint();
            }

        internal override string GetCurerentCategoryName()
            {
            c = REFERENCE_WINDOW.titleContent;
            var ind = c.text.LastIndexOf(' ');
            if (ind == -1 || ind >= c.text.Length) return "";
            var nn = c.text.Remove(c.text.LastIndexOf(' '));
            return nn;
            }

        internal void SetCurerentCategoryName(string name)
            {
            var c = REFERENCE_WINDOW.titleContent;
            c.text = name + " " + adapter.pluginname;
            REFERENCE_WINDOW.titleContent = c;
            REFERENCE_WINDOW.Repaint();
            }
    }


    internal static void ShowW(Adapter adapter, TYPE type, string categoryName)
        {
        //foreach (var w in Resources.FindObjectsOfTypeAll<BottomInterfaceWindow>().Where(w => w.current_type == type)) w.Close();
        _6__BottomWindow_BottomInterfaceWindow bottomInterfaceWindow = null;
        if (type == TYPE.CUSTOM)
            {
            //bottomInterfaceWindow = (BottomInterfaceWindow)EditorWindow.GetWindow<BottomInterfaceWindow1>("Bookmarks");
            bottomInterfaceWindow =
                (_6__BottomWindow_BottomInterfaceWindow) EditorWindow
                    .CreateInstance<_6__BottomWindow_BottomInterfaceWindow1>();
            bottomInterfaceWindow.Show();
            }
        else
            {
            bottomInterfaceWindow = EditorWindow.GetWindow<_6__BottomWindow_BottomInterfaceWindow2>("Last");
            }

        //  bottomInterfaceWindow.mi
        //  bottomInterfaceWindow.current_type = type;
        bottomInterfaceWindow.adapter = adapter;
        bottomInterfaceWindow.wasInit = false;

        var c = bottomInterfaceWindow.titleContent;
        c.text = categoryName + " " + adapter.pluginname;
        bottomInterfaceWindow.titleContent = c;
        /* hyperwindow = EditorWindow.CreateInstance<HyperWindow>();
         hyperwindow.Show();*/
        //var window = GetWindow<SelectTargetGoWindow<T>>();
        // window.ShowUtility();
        //hyperwindow.


        adapter.bottomInterface.WindowController.RemoveAll(w => !w.REFERENCE_WINDOW);
        bottomInterfaceWindow.current_controller = new BottomControllerWindow(adapter)
                {REFERENCE_WINDOW = bottomInterfaceWindow};
        adapter.bottomInterface.WindowController.Add(bottomInterfaceWindow.current_controller);


        bottomInterfaceWindow.Show();
        }

    TYPE? current_type;

    // TYPE current_type;
    bool wasInit = true;

    Adapter adapter;

    /*    float scroll;*/
    //   float offset;
    /*   float? oldHeight;
       float? oldWidth;*/
    internal Adapter.BottomInterface.BottomController current_controller;

    internal void REPAIN()
        {
        Repaint();
        if (adapter != null) adapter.RepaintWindowInUpdate();
        }

    void OnDestroy()
        {
        REPAIN();
        }


    internal void EVENT_UPDATE()
        {
        if (current_controller.selection_action != null)
            {
            if (!current_controller.selection_action(false, adapter.deltaTime))
                REPAIN();
            }
        }

    internal void EVENT_MOUSE_UP()
        {
        if (current_controller.selection_action != null)
            {
            //MonoBehaviour.print("ASD");
            current_controller.selection_action(true, adapter.deltaTime);
            adapter.bottomInterface.ClearAction();

            GUIUtility.hotControl = 0;
            Adapter.EventUse();
            REPAIN();

            // RepaintWindow();
            }
        }

    internal void EVENT_MOUSE_UP_CLEAR()
        {
        if (current_controller.selection_action != null) //MonoBehaviour.print("ASD");
            {
            adapter.bottomInterface.ClearAction();

            GUIUtility.hotControl = 0;
            Adapter.EventUse();
            REPAIN();

            // RepaintWindow();
            }
        }

    //bool mayscroll;
    public void OnGUI()
        {
        if (adapter == null)
            {
            var c = titleContent;
            var adapterName = c.text.Substring(c.text.LastIndexOf(' ') + 1);
            if (!Initializator.AdaptersByName.ContainsKey(adapterName))
                {
                adapterName = c.text.Remove(c.text.IndexOf(' '));
                if (!Initializator.AdaptersByName.ContainsKey(adapterName))
                    return;
                }

            adapter = Initializator.AdaptersByName[adapterName];
            }

        /*  if (!bottomInterfaceWindow)
          {
              bottomInterfaceWindow = this;
              if (!bottomInterfaceWindow) return;



              // Hierarchy.BottomInterface.HyperGraph.CHECK_SCAN();
          }*/


        if (current_controller == null)
            {
            adapter.bottomInterface.WindowController.RemoveAll(w => !w.REFERENCE_WINDOW || w.REFERENCE_WINDOW == this);
            current_controller = new BottomControllerWindow(adapter) {REFERENCE_WINDOW = this};
            adapter.bottomInterface.WindowController.Add(current_controller);
            }

        if (!current_controller.REFERENCE_WINDOW) current_controller.REFERENCE_WINDOW = this;


        minSize = new Vector2(50, this is _6__BottomWindow_BottomInterfaceWindow1
            ? adapter.bottomInterface.DRAW_CUSTOM_MINHEIGHT(current_controller)
            : adapter.bottomInterface.DRAW_LAST_MINHEIGHT(
                current_controller));

        if (!current_type.HasValue)
            {
            current_type = (this is _6__BottomWindow_BottomInterfaceWindow1) ? TYPE.CUSTOM : TYPE.LAST;
            }
        /*  if (Event.current.type == EventType.scrollWheel && new Rect(0, 0, bottomInterfaceWindow.position.width, bottomInterfaceWindow.position.height).Contains(Event.current.mousePosition))
          {
              if (mayscroll)
              {
                  if (Hierarchy.OnScroll != null) Hierarchy.OnScroll(Event.current.delta.y);
                  mayscroll = false;
              }
          }
          if (Event.current.type == EventType.repaint)
          {
              mayscroll = true;
          }
        */


        /*   if (!oldHeight.HasValue) oldHeight = bottomInterfaceWindow.position.height;
           if (oldHeight.Value != bottomInterfaceWindow.position.height)
           {
               var oldH = oldHeight.Value;
               oldHeight = bottomInterfaceWindow.position.height;
               //  controller.HEIGHT = (startHeight + (startPos.y - p.y));
               //  CHECK_HEIGHT();
               WindowHyperController.scrollPos.y -= (oldH - WindowHyperController.HEIGHT) / 2;
               // Hierarchy.BottomInterface.HyperGraph.RESET_SMOOTH_HEIGHT();
           }


           if (!oldWidth.HasValue) oldWidth = bottomInterfaceWindow.position.width;
           if (oldWidth.Value != bottomInterfaceWindow.position.width)
           {
               var oldW = oldWidth.Value;
               oldWidth = bottomInterfaceWindow.position.width;
               //  controller.HEIGHT = (startHeight + (startPos.y - p.y));
               //  CHECK_HEIGHT();
               WindowHyperController.scrollPos.x -= (oldW - WindowHyperController.WIDTH) / 2;
               // Hierarchy.BottomInterface.HyperGraph.RESET_SMOOTH_HEIGHT();
           }






          */

        /*    if (current_controller.selection_action != null)
            {
                current_controller.selection_action(false, Hierarchy.deltaTime);
                Hierarchy.BottomInterface.HyperGraph.Repaint();
            }*/


        if (!wasInit)
            {
            if (Event.current.type == EventType.Repaint)
                {
                // MonoBehaviour.print(hyperwindow.position);
                if (position.x < 15 && position.y < 50)
                    {
                    var p = position;
                    p.x = Adapter.MAX_WINDOW_WIDTH.x + (Adapter.MAX_WINDOW_WIDTH.y - p.width) / 2;
                    p.y = Adapter.MAX_WINDOW_HEIGHT.x + (Adapter.MAX_WINDOW_HEIGHT.y - p.height) / 2;
                    position = p;
                    }

                wasInit = true;
                }

            // return;
            }


        if (!adapter.par.ENABLE_ALL || !adapter.par.ENABLE_BOTTOMDOCK || adapter.DISABLE_DES())
            {
            GUI.Label(new Rect(0, 0, position.width, position.height), "Cache Disabled!");
            return;
            } /* else
            {
                var wasGui = GUI.enabled;
                GUI.enabled = Hierarchy.par.USE_HIGLIGHT;

                DrawHiglighter(FULL_RECT);
                GUI.enabled = wasGui;
            }*/


        adapter.ChangeGUI();
        try
            {
            trect.width = position.width;
            trect.height = position.height;

            current_controller.REFERENCE_WINDOW = this;
            current_controller.ModuleRect = trect;
            current_controller.CustomLineRect = trect;


            if (current_type == TYPE.CUSTOM)
                {
                // line.height = LINE_HEIGHT() * par.BOTTOM_MAXLASTROWS;

                adapter.bottomInterface.DRAW_CUSTOM(trect,
                    (int) trect.height / adapter.bottomInterface.RowsParams[0].Rows, current_controller,
                    adapter.bottomInterface.MemCacheScene, false);
                }
            else if (current_type == TYPE.LAST)
                {
                //line.height = LINE_HEIGHT() * par.BOTTOM_MAXLASTROWS;
                adapter.bottomInterface.DRAW_LAST(trect,
                    (int) trect.height / adapter.bottomInterface.RowsParams[1].Rows, current_controller,
                    adapter.LastActiveScene.GetHashCode(), false);
                }

            /*  Hierarchy.BottomInterface.HyperGraph.EXTERNAL_HYPER_DRAWER(new Rect(0, 0, bottomInterfaceWindow.position.width, bottomInterfaceWindow.position.height),
             HyperGraphWindow.WindowHyperController);*/
            }
        catch (Exception ex)
            {
            adapter.logProxy.LogError("BottomInterfaceWindow - " + ex.Message + " " + ex.StackTrace);
            }

        adapter.RestoreGUI();


        if ((Event.current.type != EventType.Repaint && Event.current.type != EventType.Layout))
            {
            EVENT_UPDATE();
            }

        if (Event.current.rawType == EventType.MouseUp)
            {
            EVENT_MOUSE_UP();
            }


        mouse_uo_helper.Invoke();
        }

    Rect trect = new Rect();
}

/*      }
  }*/
}