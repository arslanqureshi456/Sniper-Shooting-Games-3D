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

public class _W__BottomWindow_ColorCategories : _W___IWindow {
    // const int WIDTH = 230;
    
    internal static _W___IWindow Init(MousePos rect, Adapter adapter, int scene)
    {   //  Debug.Log("ASD");
        if (adapter.DEFAUL_SKIN == null) adapter.DEFAUL_SKIN = Adapter.GET_SKIN();
        
        /*var oldL = adapter.DEFAUL_SKIN.label.fontSize;
        adapter.DEFAUL_SKIN.label.fontSize = adapter.WINDOW_FONT_10();*/
        if (rect.type != MousePos.Type.ColorChanger_230_0)
        {   Debug.LogWarning("Mismatch type");
            rect.SetType(MousePos.Type.ColorChanger_230_0);
        }
        
        adapter.bottomInterface.GET_BOOKMARKS( ref list, scene );
        
        // rect = InputData.WidnwoRect(new Vector2(rect.x, rect.y - list.Count * 23 + 32), WIDTH, list.Count * 23 + 32, adapter, disableClamp: true);
        
        rect.Height = list.Count * 23 + 32 + 10;
        rect.Y -= rect.Height + 40;
        
        var w = (_W__BottomWindow_ColorCategories)private_Init( rect, typeof( _W__BottomWindow_ColorCategories ), adapter, "Background Colors" );
        w.scene = scene;
        return w;
    }
    
    static List<Int32ListArray> list;
    int scene;
    static List<string> allow = new List<string>() { "ColorPicker", "_W__InputWindow" };
    /* internal override bool PIN
     {
         get { return false; }
         set { m_PIN = value; }
     }*/
    bool wasLoasFocus = false;
    internal override bool PIN
    {   get
        {   if (EditorWindow.focusedWindow == this)
            {   wasLoasFocus = false;
                return true;
            }
            if (EditorWindow.focusedWindow == null) return true;
            //MonoBehaviour.print(EditorWindow.focusedWindow.GetType().Name);
            if (allow.Any( l => EditorWindow.focusedWindow.GetType().Name.Contains( l ) ))
            {   wasLoasFocus = true;
                return true;
            }
            if (wasLoasFocus && this)
            {   wasLoasFocus = false;
                this.Focus();
                return true;
            }
            /*   if (pinOverride == null)
               {
                   pinOverride = Resources.FindObjectsOfTypeAll<EditorWindow>().FirstOrDefault(w => allow.Any(l => w.GetType().Name.Contains(l)));
               }
               if (pinOverride != null)
               {
                   return EditorWindow.focusedWindow == (EditorWindow)pinOverride;
               }*/
            //  return true;
            /* if (/*Resources.FindObjectsOfTypeAll(typeof(EditorWindow)).Any(w => allow.Any(l => w.GetType().Name.Contains(l)) && (EditorWindow.focusedWindow == (EditorWindow)w) ||#1#
                 EditorWindow.focusedWindow == this || pinOverride != null && allow.Any(l => pinOverride.GetType().Name.Contains(l)))
             {
                 // MonoBehaviour.print("ASD");
                 return true;
             }*/
            return false;
            
        }
        set { m_PIN = value; }
    }
    protected override void Update()
    {   if (EditorWindow.focusedWindow != this && !PIN) Close();
        base.Update();
    }
    
    static Rect rect;
    static GUIContent colorContent = new GUIContent() { tooltip = "Background Color" };
    
    protected override void OnGUI()
    {   if (_inputWindow == null) return;
    
        if (adapter == null)
        {   CloseThis();
            return;
        }
        
        
        base.OnGUI();
        
        
        // if (Event.current.type == EventType.keyDown) MonoBehaviour.print(Event.current.keyCode);
        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape)
        {   Adapter.EventUseFast();
            CloseThis();
            adapter.SKIP_PREFAB_ESCAPE = true;
            
            return;
        }
        if (Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.KeypadEnter))
        {   Adapter.EventUseFast();
            CloseThis();
            return;
        }
        
        
        adapter.ChangeGUI(false);
        
        
        
        var label = lastMousePos.Width - 55 - 5;
        rect.Set( 5, 5, label, 23 );
        Label( rect, "Category Name" );
        rect.x = lastMousePos.Width;
        rect.width = 55;
        Label( rect, "Color" );
        
        
        adapter.bottomInterface.GET_BOOKMARKS( ref list, scene );
        
        for (int i = 0 ; i < list.Count ; i++)
        {   rect.x = 0;
            rect.width = lastMousePos.Width - 55 - 5;
            rect.y += rect.height;
            Adapter. INTERNAL_BOX( rect, "" );
            var al = adapter.DEFAUL_SKIN.label.alignment;
            adapter.DEFAUL_SKIN.label.alignment = TextAnchor.MiddleLeft;
            Label( rect, "  " + (i + 1) + ") " + list[i].name );
            adapter.DEFAUL_SKIN.label.alignment = al;
            
            rect.x = +rect.width;
            rect.width = 55;
            var c = list[i].GET_COLOR() ?? adapter.bottomInterface.RowsParams[0].BgColorValue;
            if (i == 0) c = adapter.bottomInterface.RowsParams[0].BgColorValue;
            adapter.RestoreGUI();
            var newC2 = M_Colors_Window.PICKER(rect, colorContent, c);
            adapter.ChangeGUI();
            if (c != newC2)
            {   if (i == 0)
                {   adapter.bottomInterface.RowsParams[0].BgColorValue = newC2;
                    adapter.SavePrefs();
                }
                else
                {   // adapter.CreateUndoActiveDescription( "Change Color" );
                    list[i].SET_COLOR( newC2 );
                    adapter.MarkSceneDirty( scene );
                }
                GUI.changed = true;
                adapter.RepaintWindowInUpdate();
            }
        }
        
        adapter.RestoreGUI();
        
        
    }
}

}
/*  }
}*/
