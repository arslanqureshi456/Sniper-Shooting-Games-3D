using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
#if PROJECT
    using EModules.Project;
#endif

namespace EModules.EModulesInternal

{
// description
//false, Event.current.mousePosition, 190, 68, adapter

internal struct WindowParams
{   internal float Width, Height;
}
internal struct MousePos
{


    Vector2 _creenMp;
    internal Type type;
    internal bool Clamp;
    internal Adapter adapter;
    
    internal static Dictionary<Type, WindowParams > w_params = new Dictionary<Type, WindowParams>()
    {    {Type.Input_128_68, new WindowParams() {Width = 128, Height = 68 }  }, //80
        {Type.Input_190_68, new WindowParams() {Width = 190, Height = 68 }  }, //80
        {Type.Search_356_0, new WindowParams() {Width = 356, Height = 0 }  },
        {Type.Highlighter_410_0, new WindowParams() {Width = 410, Height = 0 }  },
        {Type.ModulesListWindow_380_700, new WindowParams() {Width = 380, Height = 700 }  },
        {Type.ColorChanger_230_0, new WindowParams() {Width = 330, Height = 0 }  },
        {Type.SceneScanner_X_X, new WindowParams() {Width = 0, Height = 0 }  },
    };
    internal enum Type { Input_190_68, Input_128_68, Search_356_0, Highlighter_410_0, ModulesListWindow_380_700, ColorChanger_230_0, SceneScanner_X_X }
    internal MousePos( Vector2? mouse, Type type, bool Clamp, Adapter adapter )
    {
    
    
    
        if ( !mouse.HasValue )
        {   Rect rect;
            /* if (adapter.window())
             {   var p = adapter.window().position;
                 rect = new Rect( p.x + p.width / 2 - w_params[type].Width / 2, p.y + p.height / 2 - w_params[type].Height / 2, w_params[type].Width, w_params[type].Height );
             }
             else*/
            {   rect = new Rect( Adapter.MAX_WINDOW_WIDTH.x + Adapter.MAX_WINDOW_WIDTH.y / 2 - w_params[type].Width / 2,
                                 Adapter.MAX_WINDOW_HEIGHT.x + Adapter.MAX_WINDOW_HEIGHT.y / 2 - w_params[type].Height / 2, w_params[type].Width, w_params[type].Height );
            }
            this._creenMp = new Vector2( rect.x, rect.y );
        }
        else
        {   this._creenMp = EditorGUIUtility.GUIToScreenPoint( mouse.Value );
        }
        
        
        this.type = type;
        this.Clamp = Clamp;
        this.adapter = adapter;
        this.__Height = null;
        this.__Width = null;
    }
    internal MousePos( Rect mouse, Type type, bool Clamp, Adapter adapter )
    {   this._creenMp = EditorGUIUtility.GUIToScreenPoint( new Vector2( mouse.x, mouse.y ) );
        this.type = type;
        this.Clamp = Clamp;
        this.adapter = adapter;
        this.__Height = null;
        this.__Width = null;
    }
    
    /*  internal void Set(Vector2 mouse)
     {   _creenMp = EditorGUIUtility.GUIToScreenPoint(mouse);
     }
    internal void Set(float x, float y)
     {   _creenMp = EditorGUIUtility.GUIToScreenPoint(new Vector2(x, y));
     }*/
    internal Vector2 ScreenMousePosition { get { return _creenMp; } set { _creenMp = value; } }
    internal Vector2 GUIMousePosition { get { return EditorGUIUtility.ScreenToGUIPoint( _creenMp ); } }
    
    internal Rect GetRect()
    {   return WidnwoRect( this, Width, Height, adapter );
    }
    
    float? __Height;
    internal float Height
    {   get { return __Height ?? w_params[type].Height; }
        set
        {   if ( value == 0 ) throw new Exception( "value cannot be null" );
            __Height = value;
        }
    }
    float? __Width;
    internal float Width { get { return (__Width ?? 1) * w_params[type].Width; } set { __Width = value; } }
    internal float X { get { return _creenMp.x; } set { _creenMp.Set( value, _creenMp.y ); } }
    internal float Y { get { return _creenMp.y; } set { _creenMp.Set( _creenMp.x, value ); } }
    
    
    
    
    /* internal enum WidnwoRectType {Clamp, Full}
    internal static Rect WidnwoRect(WidnwoRectType type, MousePos? mouse, float width, float height, Adapter adapter, MousePos? savePosition = null, bool lockPos = false  )
    {   return WidnwoRect( type == WidnwoRectType.Clamp,  mouse,  width,  height,  adapter,  savePosition = null,  lockPos = false  );
    }*/
    internal static Rect WidnwoRect( MousePos? _mouse, float width, float height, Adapter adapter, MousePos? savePosition = null
                                     /*, bool lockPos = false*/
                                   )     //if (!lockPos) width *= adapter.HALFFACTOR_8();
    {
    
        if ( savePosition.HasValue ) _mouse = savePosition.Value;
        // else mouse = GUIUtility.GUIToScreenPoint( mouse );
        
        var mouse =  _mouse.Value.ScreenMousePosition;
        
        var hierWin = adapter.window();
        // MonoBehaviour.print(hierWin);
        
        if ( hierWin != null && _mouse.Value.Clamp )     //   if (width > hierWin.position.width) width = hierWin.position.width;
        {   // if (height > hierWin.position.height - 45) height = hierWin.position.height - 65;
            /*  if (height > Screen.currentResolution.height - 100) height = Screen.currentResolution.height - 100;
            
            
              if (mouse.x < hierWin.position.x) mouse.x = hierWin.position.x;
              if (mouse.y < hierWin.position.y) mouse.y = hierWin.position.y;
              if (mouse.x - hierWin.position.x + width > hierWin.position.width) mouse.x = hierWin.position.x + hierWin.position.width - width;
              if (mouse.y - hierWin.position.y + height > hierWin.position.height) mouse.y = hierWin.position.y + hierWin.position.height - height - 65;*/
            
            //  var max = Screen.currentResolution.height * (HEIGH_CLAMPER ?? 0.9f);
            var max = _W__SearchWindow.MAX_HEIGHT(adapter);
            if ( height > max ) height = max;
            
            
            if ( mouse.x < hierWin.position.x ) mouse.x = hierWin.position.x;
            if ( mouse.y < hierWin.position.y ) mouse.y = hierWin.position.y;
            if ( mouse.x - hierWin.position.x + width > hierWin.position.width ) mouse.x = hierWin.position.x + hierWin.position.width - width;
            if ( mouse.y - hierWin.position.y + height > hierWin.position.height )
            {   if ( mouse.y - hierWin.position.y > height )
                {   mouse.y = mouse.y - height - EditorGUIUtility.singleLineHeight * 2;
                }
                else
                {   mouse.y = hierWin.position.y + hierWin.position.height - height;
                }
            }
        }
        else
        {   if ( mouse.y - Adapter.MAX_WINDOW_HEIGHT.x + height > Adapter.MAX_WINDOW_HEIGHT.y )
            {   if ( mouse.y - Adapter.MAX_WINDOW_HEIGHT.x > height )
                {   mouse.y = mouse.y - height - EditorGUIUtility.singleLineHeight * 2;
                }
                else
                {   mouse.y = Adapter.MAX_WINDOW_HEIGHT.x + Adapter.MAX_WINDOW_HEIGHT.y - height;
                }
            }
            
            
            /*Debug.Log(Adapter.MAX_WINDOW_WIDTH);
            Debug.Log(Adapter.MAX_WINDOW_HEIGHT);*/
            /* EditorGUIUtility.mo*/
            /*   if (mouse.y  + height > Adapter.MAX_WINDOW_HEIGHT)
               {   if (mouse.y  > height)
                   {   mouse.y = mouse.y - height - EditorGUIUtility.singleLineHeight * 2;
                   }
                   else
                   {   mouse.y = Adapter.MAX_WINDOW_HEIGHT - height;
                   }
            
               }*/
        }
        //else
        {   //var W = Screen.currentResolution.width;
            // var H = Screen.currentResolution.height;
            
            /* foreach (var item in Resources.FindObjectsOfTypeAll<EditorWindow>())
             {   Debug.Log(item.GetType().Name);
             }*/
            /* foreach (var item in Resources.FindObjectsOfTypeAll(typeof(EditorWindow).Assembly.GetType("UnityEngine.UIElements.Panel") ))
             {
            
                 var n =  typeof(EditorWindow).Assembly.GetType("UnityEngine.UIElements.Panel").GetField("m_PanelName", (System.Reflection.BindingFlags)(-1)).GetValue(item);
                 Debug.Log(n);
             }*/
            
            // float TOP = float.MaxValue, RIGHT = float.MinValue, LEFT = float.MaxValue, BOTTOM = float.MinValue;
            
            Rect windRect = Adapter.WINBORDER;
            float RIGHT = windRect.x + windRect.width;
            float BOTTOM = windRect.y + windRect.height;
            
            //Debug.Log(windRect);
            /*var all_t = typeof(EditorWindow).Assembly.GetType("UnityEditor.ContainerWindow");
            var all_list = all_t.GetField("s_AllWindows", (System.Reflection.BindingFlags)(-1)).GetValue(null) as System.Collections.IList;
            Debug.Log(all_list.Count);
            var all_rect = all_t.GetField("m_PixelRect", (System.Reflection.BindingFlags)(-1));
            foreach (var item in all_list)
            {   Debug.Log(all_rect.GetValue(item));
            }*/
            
            var P  = 5 ;
            if ( mouse.x < P + windRect.x ) mouse.x = P + windRect.x;
            if ( mouse.y < P + 15 + windRect.y ) mouse.x = P + 15 + windRect.y;
            if ( mouse.x + width + P > RIGHT ) mouse.x = RIGHT - width - P;
            if ( mouse.y + height + P > BOTTOM ) mouse.y = BOTTOM - height - P;
            //  if (mouse.y - hierWin.position.y + height > hierWin.position.height) mouse.y = hierWin.position.y + hierWin.position.height - height;
        }
        // if (mouse.y < 20) mouse.y = 20;
        // if (mouse.x < 0) mouse.x = 0;
        var res = new Rect( mouse.x, mouse.y, width, height );
        
        
        
        return res;
    }
    
    
    
    internal void SetType( Type type )
    {   this.type = type;
    }
}


public class _W___IWindow : EditorWindow {


    internal void Label( Rect r, string s, TextAnchor an )
    {   var a  = adapter. label.alignment;
        adapter.label.alignment = an;
        GUI.Label( r, s, adapter.label );
        adapter.label.alignment = a;
    }
    internal void Label( Rect r, string s )
    {   GUI.Label( r, s, adapter.label );
    }
    internal void Label( Rect r, GUIContent s )
    {   GUI.Label( r, s, adapter.label );
    }
    
    internal bool Button( Rect r, string s )
    {   return GUI.Button( r, s, adapter.button );
    }
    internal bool Button( Rect r, string s, TextAnchor an )
    {   var a  = adapter.button.alignment;
        adapter.button.alignment = an;
        var res = GUI.Button( r, s, adapter.button );
        adapter.button.alignment = a;
        return res;
    }
    internal bool Button( Rect r, GUIContent s )
    {   return GUI.Button( r, s, adapter.button );
    }
    internal bool Button( Rect r, GUIContent s, TextAnchor an )
    {   var a  = adapter.button.alignment;
        adapter.button.alignment = an;
        var res = GUI.Button( r, s, adapter. button );
        adapter.button.alignment = a;
        return res;
        
    }
    
    Adapter.MOUSE_RAW_UP_HELPER  mouse_uo_helper = new Adapter.MOUSE_RAW_UP_HELPER();
    internal void PUSH_ONMOUSEUP( Action ac, EditorWindow win)
    {   mouse_uo_helper.PUSH_ONMOUSEUP( ac, win );
    }
    
    
    
    internal Action<Texture2D> pickerAc = null;
    internal int pickerId = -1;
    protected bool m_PIN = false;
    int PinInterator = 0;
    internal virtual bool PIN
    {   get { return m_PIN || PinInterator > 0; }
        set { m_PIN = value; }
    }
    void PickerCommandNameUpdate()
    {   var commandName = Event.current.commandName;
        // if (!string.IsNullOrEmpty(commandName)) MonoBehaviour.print("command = " + commandName);
        if ( commandName == "ObjectSelectorUpdated" && EditorGUIUtility.GetObjectPickerControlID() == pickerId )     //ObjectSelectorClosed
        {   // MonoBehaviour.print(EditorGUIUtility.GetObjectPickerObject());
            //comformAction((Texture2D)EditorGUIUtility.GetObjectPickerObject());
        }
        if ( commandName == "ObjectSelectorClosed" && EditorGUIUtility.GetObjectPickerControlID() == pickerId )
        {   pickerId = -1;
        
            pickerAc( EditorGUIUtility.GetObjectPickerObject() as Texture2D );
            //CloseThis();
            
            //ObjectSelectorClosed
            //comformAction((Texture2D)EditorGUIUtility.GetObjectPickerObject());
        }
        
        
    }
    internal virtual bool OnLostFocusPIN
    {   get { return PIN; }
    }
    internal virtual void OnLostFocus()    // Debug.Log( EditorGUIUtility.GetObjectPickerControlID() );
    {
    
    
        if ( pickerId != -1 || OnLostFocusPIN ) return;
        CloseThis();
    }
    
    internal virtual void OnSelectionChange()
    {   if ( PIN ) return;
        CloseThis();
    }
    protected internal virtual void CloseThis()
    {   if ( !init ) return;
        init = false;
        if ( _inputWindow != null ) _inputWindow.Close();
    }
    
    
    // internal virtual float HEIGHT_CLAMPER {get {return 0.88f;} }
    bool init = false;
    //  static Color matColor = Color.white;
    internal MousePos lastMousePos;
    
    
    // [MenuItem("Example/Color Change")]
    internal static Dictionary<Type, _W___IWindow> __inputWindow = new Dictionary<Type, _W___IWindow>();
    protected _W___IWindow _inputWindow;
    internal Type winType;
    internal Adapter adapter;
    internal static _W___IWindow private_Init( MousePos inrect, Type type, Adapter adapter, string title = "", Vector2? minSize = null, Vector2? maxSize = null, bool utils = false,
            bool useAnim = true/*, bool skipPositionAssign = false*/)
    {   if ( !__inputWindow.ContainsKey( type ) ) __inputWindow.Add( type, null );
    
        if ( __inputWindow[type] != null ) __inputWindow[type].CloseThis();
        // Get existing open window() or if none, make a new one:
        
        //  var w = __inputWindow[type] = (FocusRoot)EditorWindow.GetWindow(type, true, title, true);
        
        //  var w = __inputWindow[type] = (FocusRoot)EditorWindow.GetWindow(type, false, title, true);
        /*  var w = __inputWindow[type] = (FocusRoot)EditorWindow.GetWindow(type, true, title, true);*/
        var w = __inputWindow[type] = (_W___IWindow)EditorWindow.CreateInstance(type);
        w.titleContent = new GUIContent( title );
        if ( utils ) __inputWindow[type].Show();
        else __inputWindow[type].ShowPopup();
        __inputWindow[type].Focus();
        
        __inputWindow[type]._inputWindow = __inputWindow[type];
        w.init = true;
        //var p = window().position;
        // rect.Set(pos.x, pos.y, 100, 50);
        w.pickerId = -1;
        w.firstLaunch = true;
        
        
        // if (inrect == null) inrect = __inputWindow[type].position;
        w.lastMousePos = inrect;
        var rect = inrect.GetRect();
        
        /* else
         lastMousePos = inrect ?? lastMousePos ;
        
         var rect = inrect == null ? __inputWindow[type].position : inrect.Value.GetRect() ;*/
        // if (_inputWindow.comformAction)
        // rect.y += 40;
        w.targetRect = rect;
        w.Screenrect = rect;
        
        if ( adapter._S_EnableWindowsAnimation && useAnim ) w.Screenrect.height = 25;
        //MonoBehaviour.print(EditorGUIUtility.wi);
        // _inputWindow.ShowTab();
        // _inputWindow.ShowUtility();
        //_inputWindow.st
        w.adapter = adapter;
        
        // if (!skipPositionAssign)
        {   w.position = w.Screenrect;
            // Debug.Log( "ASD" );
            w.maxSize = maxSize ?? new Vector2( rect.width, w.Screenrect.height );
            w.minSize = minSize ?? new Vector2( rect.width, w.Screenrect.height );
            w.SIZIBLE_X = minSize.HasValue || maxSize.HasValue;
            w.SIZIBLE_X = minSize.HasValue || maxSize.HasValue;
            w.position = w.Screenrect;
        }
        
        w.winType = type;
        w.PIN = false;
        w.PinInterator = 2;
        // MonoBehaviour.print(_inputWindow.position);
        
        w.lastTime = EditorApplication.timeSinceStartup;
        w.wasAnim = false;
        
        Adapter.EventUse();
        //  w.adapter.RepaintAllViews();
        w.Repaint();
        
        return __inputWindow[type];
        // deltaTime = (float)(EditorApplication.timeSinceStartup - lastTime);
        // lastTime = EditorApplication.timeSinceStartup;
        // wasFocus = false;
    }
    // static bool wasFocus = false;
    protected bool wasAnim = false;
    Rect targetRect;
    Rect Screenrect;
#pragma warning disable
    internal bool firstLaunch = false;
#pragma warning restore
    
    
    protected virtual void Update()
    {   /* if (!firstLaunch)
             {   firstLaunch = true;
                 foreach (var VARIABLE in Resources.FindObjectsOfTypeAll<EditorWindow>().Where( v => v is FocusRoot ))
                     if (VARIABLE) VARIABLE.Close();
                 var input = __inputWindow.Values.FirstOrDefault(i => i);
                 if (!input) return;
                 input.Focus();
             }*/
        if ( !(PinInterator < 1) ) PinInterator--;
        
    }
    internal bool animcomplete;
    
    
    internal void SET_NEW_HEIGHT( Adapter adapter, float height )
    {
    
        if ( Event.current != null && Event.current.type != EventType.Repaint ) return;
        
        var h = MousePos.WidnwoRect( null, lastMousePos.Width, height, adapter, lastMousePos);
        targetRect.height = h.height;
        // targetRect.y = _inputWindow.position.y;
        
        if ( lastMousePos.Clamp )
        {   if ( targetRect.y + targetRect.height > adapter.window().position.y + _W__SearchWindow.MAX_HEIGHT( adapter ) ) targetRect.y -= (targetRect.y + targetRect.height) -
                        (adapter.window().position.y + _W__SearchWindow.MAX_HEIGHT( adapter ));
        }
        
        //if (targetRect.height > FillterData.MAX_HEIGHT(adapter)) targetRect.height = FillterData.MAX_HEIGHT(adapter);
        //targetRect.y = h.y;
        if ( wasAnim )
        {   AniimationUpdateR( 1 );
            animcomplete = true;
            //_inputWindow.position = targetRect;
        }
        
        Repaint();
    }
    
    
    internal double lastTime = 0;
    protected virtual void OnGUI()
    {   if ( _inputWindow == null )
        {   return;
        }
        
        
        /*  if (Event.current.type == EventType.repaint)
           {
               // MonoBehaviour.print(Screenrect.height);
               //  Screenrect.height = Mathf.Lerp(Screenrect.height, targetRect.height, 0.1f);
               Screenrect.height = Mathf.MoveTowards(Screenrect.height, targetRect.height, Hierarchy.deltaTime * 3.1f * 60);
               var s = _inputWindow.minSize;
               s.y = Screenrect.height;
               _inputWindow.minSize = _inputWindow.maxSize = s;
               _inputWindow.position = Screenrect;
        
               //window().Show();
           }*/
        
        
        if ( Event.current.type == EventType.Repaint )
        {
        
            var O = 0;
            var br = new Rect(O, O, position.width - 2 * O, position.height - 2 * O);
            // Adapter.GET_SKIN().textArea.Draw( br , "" , false , false , false , false );
            Adapter.STYLE_DEFBOX.Draw( br, "", false, false, false, false );
            /* O = 2;
             br = new Rect( O , O , position.width - 2 * O , position.height - 2 * O );
             GUI.Box( br , "" );
             O = 2;
             br = new Rect( O , O , position.width - 2 * O , position.height - 2 * O );
             GUI.Box( br , "" );*/
            
            if ( !wasAnim )     // MonoBehaviour.print(Screenrect.height);
            {   //  Screenrect.height = Mathf.Lerp(Screenrect.height, targetRect.height, 0.1f);
            
            
                var targetTime = Mathf.Clamp01((float)(EditorApplication.timeSinceStartup - lastTime) * 4.5f);
                if ( targetTime == 1 ) wasAnim = true;
                //Screenrect.height = Mathf.MoveTowards(Screenrect.height, targetRect.height, deltaTime * 3.1f * 220);
                AniimationUpdateR( targetTime );
                //window().Show();
            }
        }
        
        
        
        if ( _inputWindow == null )
        {   return;
        }
        PickerCommandNameUpdate();
        
        mouse_uo_helper.Invoke();
        
        //if (Event.current.type == EventType.layout)
        
    }
    
    internal bool SIZIBLE_X = false;
    internal bool SIZIBLE_Y = false;
    
    protected void AniimationUpdateR( float targetTime )
    {   Screenrect = _inputWindow.position;
        if ( Screenrect.x == 0 && Screenrect.y == 0 )
        {   Screenrect.x = targetRect.x;
            Screenrect.y = targetRect.y;
        }
        // if (Screenrect.x <= 0 || Screenrect.y <= 0) return;
        Screenrect.height = Mathf.Lerp( Screenrect.height, targetRect.height, targetTime );
        Screenrect.y = Mathf.Lerp( Screenrect.y, targetRect.y, targetTime );
        animcomplete = targetTime >= 1;
        
        var s = _inputWindow.minSize;
        s.y = Screenrect.height;
        if ( !SIZIBLE_X ) _inputWindow.minSize = s;
        if ( !SIZIBLE_Y )
        {   _inputWindow.maxSize = s;
            // Debug.Log( "ASD" );
        }
        
        if ( _inputWindow.position != Screenrect )
        {   _inputWindow.position = Screenrect;
        }
        /*if (Screenrect.height != targetRect.height)*/
        _inputWindow.Repaint();
    }
    
    void OnInspectorUpdate()
    {   Repaint();
    }
}
}
