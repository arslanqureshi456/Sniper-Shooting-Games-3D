//#define CLOSE_AFTERICON
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine.SceneManagement;
//namespace EModules
#if PROJECT
    using EModules.Project;
#endif

namespace EModules.EModulesInternal {

public partial class M_Colors_Window : _W___IWindow {






    /*
    
        internal void GenerateSetterFunctions(IModuleOnnector_M_Color colorModule, Adapter.HierarchyObject __o)
        {   Action<Texture> SET_TEXTURE = (currentSelection) =>
            {
    
    
                if (currentSelection == null)
                {  colorModule. SetIcon(__o, (Texture2D)null);
                }
                else
                {   var library = AssetDatabase.GetAssetPath(currentSelection).StartsWith("Library/");
                    string texName = library
                                     ? currentSelection.name
                                     : "GUID=" + AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(currentSelection));
    
                    Hierarchy_GUI.Undo(adapter, "Set Icon");
                    if (!library)
                    {   Hierarchy_GUI.GetLastList(adapter).RemoveAll(t => t == texName);
                        if (Hierarchy_GUI.GetLastList(adapter).Count == 0) Hierarchy_GUI.GetLastList(adapter).Add(texName);
                        else Hierarchy_GUI.GetLastList(adapter).Insert(0, texName);
                        while (Hierarchy_GUI.GetLastList(adapter).Count > 20) Hierarchy_GUI.GetLastList(adapter).RemoveAt(20);
                    }
                    Hierarchy_GUI.SetDirtyObject(adapter);
    
                    SetIcon(__o, (Texture2D)currentSelection);
                }
    
    
    
            };
            //
    
            var capture_o = __o;
            var GET_TEXTURE = Utilities.ObjectContent_IncludeCacher(adapter, __o, t).image;
    
    
            Action<SingleList> SET_HIGLIGHT_COLOR = (singleList) =>
            {   SetAction(capture_o, (in_o) =>
                {   if (singleList == null || !in_o.Validate(true) ) return;
                    var c = singleList.list;
                    // var crs = Converter(singleList);
                    var d = adapter.MOI.des( in_o.scene );
                    adapter.SET_UNDO(d, "Change Highlighter Color");
    
                    // Undo.RecordObject(d.gameObject, "Change Highlighter Color");
                    cacheDic.Clear();
    
                    Color32 color1 = c[0] == 0 & c[1] == 0 & c[2] == 0 & c[3] == 0 ? (Color32)Adapter.TRANSP_COLOR : new Color32((byte)c[0], (byte)c[1], (byte)c[2], (byte)c[3]);
                    Color32 color2 = c.Count < 9 || c[5] == 0 & c[6] == 0 & c[7] == 0 & c[8] == 0 ? (Color32)Adapter.TRANSP_COLOR : new Color32((byte)c[5], (byte)c[6], (byte)c[7], (byte)c[8]);
                    // MonoBehaviour.print(c[4] == 1);
                    SetValue(color1 == Adapter.TRANSP_COLOR && color2 == Adapter.TRANSP_COLOR ? null : new[] { color1, color2 }, c[4] == 1, in_o.scene, in_o);
                });
            };
    
            Action<SingleList> SET_TEXT_COLOR = (singleList) =>
            {   SetAction(capture_o, (_o) =>
                {   if (singleList == null || !_o.Validate(true)) return;
                    var c = singleList.list;
                    // var crs = Converter(singleList);
                    var d = adapter.MOI.des( _o.scene );
                    adapter.SET_UNDO(d, "Change Text Color");
                    cacheDic.Clear();
    
                    Color32 color1 = c[0] == 0 & c[1] == 0 & c[2] == 0 & c[3] == 0 ? (Color32)Adapter.TRANSP_COLOR : new Color32((byte)c[0], (byte)c[1], (byte)c[2], (byte)c[3]);
                    Color32 color2 = c.Count < 9 || c[5] == 0 & c[6] == 0 & c[7] == 0 & c[8] == 0 ? (Color32)Adapter.TRANSP_COLOR : new Color32((byte)c[5], (byte)c[6], (byte)c[7], (byte)c[8]);
    
                    SetValue(color1 == Adapter.TRANSP_COLOR && color2 == Adapter.TRANSP_COLOR ? null : new[] { color1, color2 }, c[4] == 1, _o.scene, _o);
                });
            };
    
    
            Func<SingleList> GET_HIGLIGHTER_COLOR = () =>
            {   var _o = capture_o;
                if (!_o.Validate(true)) return new SingleList() { list = new int[9].ToList() };
                var c = GetValue(_o.scene, _o);
                if (c != null) return new SingleList() { list = c.list.ToList() };
                // if (c != null) return c;
                else return new SingleList() { list = new int[9].ToList() };
            };
    
            Func<SingleList> GET_TEXT_COLOR = () =>
            {   var _o = capture_o;
                if (!_o.Validate(true)) return new SingleList() { list = new int[9].ToList() };
                var c = GetValue(_o.scene, _o);
                if (c != null) return new SingleList() { list = c.list.ToList() };
                // if (c != null) return c;
                else return new SingleList() { list = new int[9].ToList() };
            };
        }
        */
    
    
    
    
    
    
    
    
    
    
    
    
    
    static float WH303 = 450 + singleLineHeight / 2;
    internal static void Init( MousePos rect, string title, Action<Texture, string> _SetIconImage, Texture _GetTexture, Action<Adapter.TempColorClass, string> _SetHiglightColor
                               , Func<Adapter.TempColorClass> _GetHiglightColor
                               , Action<SingleList, string> _SetIconColor, Func<SingleList> _GetIconColor, Adapter.HierarchyObject _source, Adapter inadapter )
    {   /*  rect.width = 180;
        rect.height = 215;*/
        
        /*   rect.width = 285;
           rect.height = 230;*/
        
        localIdInFIle_to_instanceId.Clear();
        
        // rect.width = 410;
        adapter = inadapter;
        rect.Height = WH303;
        
        
        if ( rect.type != MousePos.Type.Highlighter_410_0 ) throw new Exception();
        // rect = WidnwoRect( WidnwoRectType.Clamp, new Vector2( rect.x, rect.y ), rect.width, rect.height, adapter, savePosition: new Vector2( rect.x, rect.y ),  lockPos: true );
        
        
        
        Adapter.M_Colors.InitIcons();
        
        if ( _GetTexture == null )
        {   current = "";
        }
        else if ( AssetDatabase.GetAssetPath( _GetTexture ).StartsWith( "Library/" ) )
        {   current = _GetTexture.name;
        }
        else
        {   current = "GUID=" + AssetDatabase.AssetPathToGUID( AssetDatabase.GetAssetPath( _GetTexture ) );
        }
        WriteLastColor();
        //titleWin = title;
        source = _source;
        SetIconImage = _SetIconImage;
        GetHiglightData = _GetHiglightColor;
        __SetHiglightData = _SetHiglightColor;
        GetHiglightData().SetLastTempColor( adapter );
        
        GetIconColor = _GetIconColor;
        __SetIconColor = _SetIconColor;
        
        repaintW = _W___IWindow.private_Init( rect, typeof( M_Colors_Window ), adapter, title, useAnim: true ) as M_Colors_Window;
        repaintW.SET_NEW_HEIGHT( adapter, rect.Height );
        adapter.onUndoAction -= repaint;
        adapter.onUndoAction += repaint;
        ClearCacheAndRepaint();
        
    }
    new  static Adapter adapter = null;
    // static Adapter static_adapter { get { return adapter; } }
    static M_Colors_Window repaintW;
    static void repaint()
    {   if ( repaintW )
        {   cache = null;
            repaintW.Repaint();
        }
    }
    //Change Icons Color
    // "Set Icon"
    internal static bool Enable { get { return repaintW; } }
    
    static string current;
    
    // static string titleWin;
    internal static Adapter.HierarchyObject source;
    static Action<Texture, string> SetIconImage;
    static List<string> allow = new List<string>() { "ObjectSelector", "ColorPicker", "_W__InputWindow" };
    //EditorWindow pinOverride;
    bool wasLoasFocus = false;
    
    
    internal override bool OnLostFocusPIN
    {   get
        {
        
            if ( Resources.FindObjectsOfTypeAll( typeof( EditorWindow ) ).Any( w => allow.Any( l => w.GetType().Name.Contains( l ) ) ) )
            {   wasLoasFocus = false;
                return true;
            }
            // Debug.Log( EditorWindow.focusedWindow );
            
            if ( EditorWindow.focusedWindow == null ) return true;
            //MonoBehaviour.print(EditorWindow.focusedWindow.GetType().Name);
            if ( allow.Any( l => EditorWindow.focusedWindow.GetType().Name.Contains( l ) ) )
            {   wasLoasFocus = true;
                fixFocus = false;
                return true;
            }
            if ( wasLoasFocus && this )
            {   wasLoasFocus = false;
                this.Focus();
                return true;
            }
            return false;
        }
    }
    internal override bool PIN
    {   get
        {
        
        
        
            if ( EditorWindow.focusedWindow == this )
            {   wasLoasFocus = false;
                return true;
            }
            if ( EditorWindow.focusedWindow == null ) return true;
            //MonoBehaviour.print(EditorWindow.focusedWindow.GetType().Name);
            if ( allow.Any( l => EditorWindow.focusedWindow.GetType().Name.Contains( l ) ) )
            {   wasLoasFocus = true;
                fixFocus = false;
                return true;
            }
            if ( wasLoasFocus && this )
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
            return m_PIN;
            
        }
        set { m_PIN = value; }
    }
    static void REPAINT_ALL_HIERW()
    {   adapter.RepaintWindowInUpdate();
        foreach ( var w in __inputWindow ) if ( w.Value ) w.Value.Repaint();
    }
    
    
    protected override void Update()     //  MonoBehaviour.print(GUIUtility.GetControlID(FocusType.Keyboard));
    {   //  MonoBehaviour.print(GUIUtility.GetControlID(FocusType.Passive));
        //  MonoBehaviour.print(Resources.FindObjectsOfTypeAll(typeof(EditorWindow)).Select(w => w.GetType().Name).Aggregate((a, b) => a + " " + b));
        if ( wasLoasFocus && !fixFocus && EditorWindow.focusedWindow == this )
        {   CloseThis();
            return;
        }
        if ( wasLoasFocus && !fixFocus )
        {   if ( !EditorWindow.focusedWindow && this )
            {   this.Focus();
                wasLoasFocus = false;
            }
            
            /*if ( allow.All( l => !EditorWindow.focusedWindow.GetType().Name.Contains( l ) ))
            {
                {   CloseThis();
                    return ;
                }
            }*/
        }
        
        
        if ( !PIN && EditorWindow.focusedWindow != this ) CloseThis();
        
        if ( LostFocus )
        {   if ( EditorWindow.focusedWindow && EditorWindow.focusedWindow.GetType().Name == "IconData" )
            {   CloseThis();
                return;
            }
            LostFocus = false;
        }
        
        
        base.Update();
    }
    GUIContent emptyContent = new GUIContent();
    GUIContent nullContent = new GUIContent();
    
    
    static GUIStyle HIGHLIGHTER_COLOR_FG, HIGHLIGHTER_COLOR_DECORATION, HIGHLIGHTER_COLOR_PICKER;
    static bool stylesWasInit = false;
    
    static void InitStyles()
    {   if ( adapter == null ) adapter = Initializator.AdaptersByName.First().Value;
    
        stylesWasInit = true;
        
        HIGHLIGHTER_COLOR_FG = adapter.InitializeStyle( "HIGHLIGHTER_COLOR_FG", 0.5f );
        HIGHLIGHTER_COLOR_DECORATION = adapter.InitializeStyle( "HIGHLIGHTER_COLOR_DECORATION", 0.5f );
        HIGHLIGHTER_COLOR_PICKER = adapter.InitializeStyle( "HIGHLIGHTER_COLOR_PICKER", 0.5f );
        
        
    }
    static void _labelRich( Rect r, GUIContent s, GUIStyle style = null )
    {   var ra =  label.richText;
        label.richText = true;
        if ( style == null)GUI.Label( r, s, label );
        else GUI.Label( r, s, style);
        label.richText = ra;
    }
    static void _labelRich( Rect r, string s )
    {   var ra =  label.richText;
        label.richText = true;
        GUI.Label( r, s, label );
        label.richText = ra;
    }
    new static void Label( Rect r, string s, TextAnchor an )
    {   var a  = label.alignment;
        label.alignment = an;
        _labelRich( r, s );
        label.alignment = a;
    }
    static void Label( Rect r, GUIContent s, GUIStyle style )
    {   var a  = label.alignment;
        _labelRich( r, s, style );
        label.alignment = a;
    }
    new static void Label( Rect r, string s )
    {   _labelRich( r, s );
    }
    new static void Label( Rect r, GUIContent s )
    {   _labelRich( r, s );
    }
    
    static bool _buttonRich( Rect r, string s )
    {   var ra =  adapter.STYLE_DEFBUTTON.richText;
        adapter.STYLE_DEFBUTTON.richText = true;
        var res = GUI.Button( r, s, adapter.STYLE_DEFBUTTON );
        adapter.STYLE_DEFBUTTON.richText = ra;
        return res;
    }
    static bool _buttonRich( Rect r, GUIContent s )
    {   var ra =  adapter.STYLE_DEFBUTTON.richText;
        adapter.STYLE_DEFBUTTON.richText = true;
        var res = GUI.Button( r, s, adapter.STYLE_DEFBUTTON );
        adapter.STYLE_DEFBUTTON.richText = ra;
        return res;
    }
    new static bool Button( Rect r, string s )
    {   return _buttonRich( r, s );
    }
    new static bool Button( Rect r, string s, TextAnchor an )
    {   var a  = button.alignment;
        button.alignment = an;
        var res = _buttonRich( r, s );
        button.alignment = a;
        return res;
    }
    new static bool Button( Rect r, GUIContent s )
    {   return _buttonRich( r, s );
    }
    new static bool Button( Rect r, GUIContent s, TextAnchor an )
    {   var a  = button.alignment;
        button.alignment = an;
        var res = _buttonRich( r, s );
        button.alignment = a;
        return res;
        
    }
    //         static   TextAnchor label_a;
    //         static   TextAnchor button_a;
    //         static  Texture2D[] button_t = new Texture2D[4];
    //         static  Texture2D[][] button_t2x = new Texture2D[4][];
    static bool changedgui = false;
    static  internal GUIStyle __label;
    static internal GUIStyle label
    {   get
        {   if ( __label == null ) __label = Adapter.GET_SKIN().label;
            return __label;
        }
    }
    static internal GUIStyle __button;
    static internal GUIStyle button
    {   get
        {   if ( __button == null ) __button = Adapter.GET_SKIN().button;
            return __button;
        }
    }
    static GUIStyle changed_button, changed_label;
    
    internal static void CHANGE_GUI() { CHANGE_GUI( adapter ); }
    internal static void CHANGE_GUI( Adapter adapter )
    {   if ( changedgui )
        {   RESTORE_GUI();
            // throw new Exception( "Cannot change highlighter gui" );
        }
        changedgui = true;
        // Hierarchy.ChangeGUI();
        
        if ( changed_button == null )
        {   changed_button = new GUIStyle( adapter.STYLE_DEFBUTTON );
            changed_button.alignment = TextAnchor.MiddleLeft;
        }
        if ( changed_label == null )
        {   changed_label = new GUIStyle( Adapter.GET_SKIN().label );
            changed_label.alignment = TextAnchor.MiddleLeft;
        }
        __button = changed_button;
        __label = changed_label;
    }
    internal static void RESTORE_GUI()
    {   if ( !changedgui )     // throw new Exception( "highlighter gui not changed" );
        {
        }
        changedgui = false;
        //Hierarchy.RestoreGUI();
        /*  Adapter.GET_SKIN().label.alignment = label_a;
          Adapter.GET_SKIN().button.alignment = button_a;
          Adapter.GET_SKIN().button.normal.background = button_t[0];
          Adapter.GET_SKIN().button.normal.scaledBackgrounds = button_t2x[0];
          Adapter.GET_SKIN().button.hover.background = button_t[1];
          Adapter.GET_SKIN().button.hover.scaledBackgrounds = button_t2x[1];
          Adapter.GET_SKIN().button.active.background = button_t[2];
          Adapter.GET_SKIN().button.active.scaledBackgrounds = button_t2x[2];
          Adapter.GET_SKIN().button.focused.background = button_t[3];
          Adapter.GET_SKIN().button.focused.scaledBackgrounds = button_t2x[3];*/
        __button = Adapter.GET_SKIN().button;
        __label = Adapter.GET_SKIN().label;
    }
    
    bool LostFocus = false;
    internal override void OnLostFocus()
    {   if ( EditorWindow.focusedWindow && EditorWindow.focusedWindow.GetType().Name == "IconData" )
        {   LostFocus = true;
        }
        base.OnLostFocus();
    }
    
    
    static void TryToClose()
    {   if ( Event.current != null && Event.current.control && repaintW ) repaintW.CloseThis();
    }
    
    /*  float width;
      float w10;*/
    //static Color32 newCol;
    bool fixFocus = false;
    float LINE_H { get { return singleLineHeight * 1.5f; } }
    bool skipDown = false;
    protected override void OnGUI()
    {   if ( wasLoasFocus && EditorWindow.focusedWindow == this )
        {   fixFocus = true;
            skipDown = true;
            /*  Debug.Log(Event.current.mousePosition);
              if (Event.current.mousePosition.x <= 0 || Event.current.mousePosition.y <= 0 || Event.current.mousePosition.x >= position.width || Event.current.mousePosition.y >= position.height)
              {   CloseThis();
                  return;
              }*/
        }
        if ( EditorWindow.focusedWindow == this )
        {   if ( skipDown && Event.current.type == EventType.MouseDown ) Adapter.EventUseFast();
            if ( Event.current.type == EventType.Repaint ) skipDown = false;
        }
        
        if ( _inputWindow == null ) return;
        
        if ( source == null || adapter == null )
        {   CloseThis();
            return;
        }
        
        if ( !source.Validate() || adapter.tempAdapterBlock )
        {   CloseThis();
            return;
        }
        
        base.OnGUI();
        
        if ( !stylesWasInit ) InitStyles();
        
        CHANGE_GUI();
        
        
        var PPP = 5;
        
        
        var LABEL_HEIGHT = singleLineHeight; /* Adapter.GET_SKIN().label.fontSize;*/
        Rect FULL_RECT = new Rect(PPP, PPP, _inputWindow.position.width - PPP * 2 + 5, LABEL_HEIGHT * 2);
        
        
        string[]  cats = null;
        /*  if (adapter.IS_HIERARCHY()) cats = new [] { "HighLighter",  "HL - Templates", "HL - Settings", "Comps Presets" };
          else cats = new [] { "HighLighter", "HL - Templates", "HL - Settings"};☰*/
        if ( adapter.IS_HIERARCHY() ) cats = new[] { "SINGLE MODE", "AUTO MODE", "", "☰CompsPresets" };
        else cats = new[] { "SINGLE MODE", "AUTO MODE", "" };
        
        var ov = EditorPrefs.GetInt("EModules/Hierarchy/HighlighterCat");
        ov = Mathf.Clamp( ov, 0, cats.Length - 1 );
        var toolBarRect = new Rect(FULL_RECT.x, FULL_RECT.y, FULL_RECT.width - 5, FULL_RECT.height);
        var TBM = 2;
        
        var news = Adapter.UNITY_CURRENT_VERSION >= Adapter.UNITY_2019_3_0_VERSION;
        var style = news ? GUI.skin.button : EditorStyles.toolbarButton;
        var nv = ov;
        if (!news )
        {   var oh = EditorStyles.toolbarButton.fixedHeight;
            EditorStyles.toolbarButton.fixedHeight *= TBM;
            nv  = GUI.Toolbar(toolBarRect,   ov, cats, style );
            if ( nv == 2 ) nv = 0;
            if ( adapter.IS_HIERARCHY() )
            {   var q = toolBarRect;
                q.width /= 4;
                q.x += q.width * 2;
                GUI.Box( q, "" );
            }
            EditorStyles.toolbarButton.fixedHeight = oh;
            if ( ov != nv )
            {   EditorPrefs.SetInt( "EModules/Hierarchy/HighlighterCat", nv );
            }
            toolBarRect.height = EditorStyles.toolbarButton.fixedHeight * TBM;
        }
        
        
        toolBarRect.width /= cats.Length;
        var colors = new[] {new Color32(244, 67, 54, 127), new Color32(25, 118, 210, 127), new Color32(76, 175, 80, 127), new Color32(255, 158, 34, 127)};
        for ( int i = 0 ; i < cats.Length ; i++ )     //if (i == nv) colors[i].a /= (byte)(EditorGUIUtility.isProSkin ? 3 : 12);
        {
        
            if ( /*adapter.IS_HIERARCHY() && */i == 2 )
            {
            
            
            
                //  float w;
                // GUI.skin.button.CalcMinMaxWidth( new GUIContent( "clear" ), out w, out w );
                //  if (GUI.Button( new Rect( toolBarRect.x + toolBarRect .width / 2 - w / 2, toolBarRect.y + toolBarRect .height / 2 - EditorGUIUtility.singleLineHeight / 2, w, EditorGUIUtility.singleLineHeight
                /* if (GUI.Button( new Rect( toolBarRect.x, toolBarRect.y + toolBarRect .height / 2 - EditorGUIUtility.singleLineHeight / 2, toolBarRect.width, EditorGUIUtility.singleLineHeight
                                         ),
                                 "clear", adapter.STYLE_DEFBUTTON ) )
                 {
                
                 }*/
                
                
                
                
                toolBarRect.x += toolBarRect.width;
                continue;
            }
            
            
            if ( news  )
            {   var fh = EditorStyles.toolbarButton.fixedHeight;
                var aa = EditorStyles.toolbarButton.alignment;
                EditorStyles.toolbarButton.fixedHeight = 0;
                EditorStyles.toolbarButton.alignment = TextAnchor.MiddleCenter;
                var res = GUI.Button( toolBarRect, cats[i], EditorStyles.toolbarButton );
                EditorStyles.toolbarButton.fixedHeight = fh;
                EditorStyles.toolbarButton.alignment = aa;
                if ( res )
                {   nv = i;
                    EditorPrefs.SetInt( "EModules/Hierarchy/HighlighterCat", nv );
                }
            }
            
            
            /* if ( i == nv )
             {   if ( EditorGUIUtility.isProSkin )
                 {
                 }
                 else
                 {   colors[i].a /= (byte)(EditorGUIUtility.isProSkin ? 3 : 12);
                 }
             }*/
            /* if ( !EditorGUIUtility.isProSkin )
             {   EditorGUI.DrawRect( Shrink( toolBarRect, 0 ), colors[i] );
             }
             else*/
            {   if ( i == nv )     //EditorGUI.DrawRect(Shrink(toolBarRect, 0), colors[i]);
                {   colors[i].r = (byte)(255 - (255 - colors[i].r) / 2);
                    colors[i].g = (byte)(255 - (255 - colors[i].g) / 2);
                    colors[i].b = (byte)(255 - (255 - colors[i].b) / 2);
                    colors[i].a /= 2;
                    EditorGUI.DrawRect( Shrink( toolBarRect, 5 ), colors[i] );
                    
                }
                else
                {   colors[i].a /= 1;
                    var R = Shrink(toolBarRect, 5);
                    R.height /= 8;
                    EditorGUI.DrawRect( R, colors[i] );
                    /*     R.y += R.height * 5;
                    EditorGUI.DrawRect(R, colors[i]);
                     R.height /= 2;
                     R.y += R.height / 2;
                     if (Event.current.type == EventType.Repaint) EditorStyles.toolbarButton.Draw(R, emptyContent, 0);*/
                }
            }
            /* var casd = GUI.skin.label.normal.textColor;
             var al = GUI.skin.label.alignment;
             var fs = GUI.skin.label.fontSize;
             // GUI.skin.label.normal.textColor = Color.black;
             GUI.skin.label.alignment = TextAnchor.MiddleCenter;
             GUI.skin.label.fontSize = EditorStyles.toolbarButton.fontSize ;
             Label(toolBarRect, cats[i]);
             GUI.skin.label.normal.textColor = casd;
             GUI.skin.label.alignment = al;
             GUI.skin.label.fontSize = fs;*/
            
            toolBarRect.x += toolBarRect.width;
        }
        
        var OFF = toolBarRect.height + singleLineHeight / 2;
        FULL_RECT.y += OFF;
        FULL_RECT.height = WH303 - OFF;
        
        
        if ( !adapter.IS_HIERARCHY() && nv == 3 ) nv = 0;
        
        if ( nv == 0 )     //   var PRESET_STRING = "Highlighter";
        {   if ( Application.isPlaying )
            {   var PRESET_STRING = " (Warning! Changes Will Lost after Stop Playing)";
                Label( FULL_RECT, PRESET_STRING );
                FULL_RECT.y += FULL_RECT.height;
            }
            
            
            
            if ( adapter.DISABLE_DESCRIPTION( source ) )
            {   Label( FULL_RECT, "Cache Disabled!" );
            }
            else
            {   var wasGui = GUI.enabled;
                GUI.enabled = adapter.par.USE_HIGLIGHT;
                
                
                
                FULL_RECT.y = DrawHiglighter( FULL_RECT );
                GUI.enabled = wasGui;
            }
            
            // FULL_RECT.y += FULL_RECT.height;
            //   FULL_RECT.height += singleLineHeight;
            FULL_RECT.height = singleLineHeight;
            Adapter.LABEL( FULL_RECT, "<i>Icon:</i>" );
            FULL_RECT.y += FULL_RECT.height - singleLineHeight;
            FULL_RECT.height = 147;
            
            adapter.HR( FULL_RECT, 15);
            //FULL_RECT.y = WH303 - FULL_RECT.height ;
            DRAWICON( FULL_RECT );
            
            
            /*///////////////////*/
            /**   ICON    **/
            // FULL_RECT.y += FULL_RECT.height;
            // FULL_RECT.height = LABEL_HEIGHT;
            
            //PRESET_STRING = adapter.IS_HIERARCHY() ? "Icon of GameObject" : "Icon";
            // if (Application.isPlaying) {PRESET_STRING += " (Warning! Changes Will Lost after Stop Playing)";
            // Label( FULL_RECT, PRESET_STRING );
            
            //
            //
            
            /**   ICON    **/
            /*///////////////////*/
        }
        
        
        if ( nv == 2 )    // DrawDettings(FULL_RECT);
        {
        }
        
        if ( nv == 1 )
        {   var oy = FULL_RECT.y;
            var oh = FULL_RECT.height;
            FULL_RECT.height = singleLineHeight;
            FULL_RECT.width -= 4;
            
            EditorGUI.BeginChangeCheck();
            
            adapter._S_autorFiltersEnable = adapter.TOGGLE_LEFT( FULL_RECT, "<i>Enable Auto Apply Mode:</i>", adapter._S_autorFiltersEnable );
            Adapter.TOOLTIP( FULL_RECT, "Customized filters will be automatically applied to objects. You can disable auto mode and to use this window as a set of pre-configured highlighter styles list." );
            FULL_RECT.y += FULL_RECT.height;
            
            if ( EditorGUI.EndChangeCheck() )
            {   adapter.SavePrefs();
                adapter.RepaintAllViews();
            }
            
            //HR////////////////////
            FULL_RECT.y += 3;
            FULL_RECT.height = 1;
            Adapter.INTERNAL_BOX( FULL_RECT );
            //HR////////////////////
            //  FULL_RECT.height = EditorGUIUtility.singleLineHeight;
            //  Adapter.LABEL(FULL_RECT, "Highlighter Templates:");
            FULL_RECT.y += FULL_RECT.height + 5;
            FULL_RECT.height = oh - (FULL_RECT.y - oy);
            
            FULL_RECT.width += 4;
            DrawFilts( FULL_RECT );
            FULL_RECT.width -= 4;
            
            //HR////////////////////
            FULL_RECT.y += FULL_RECT.height + 3;
            FULL_RECT.height = 1;
            //HR////////////////////
            Adapter.INTERNAL_BOX( FULL_RECT );
        }
        /*///////////////////*/
        /**   PRESETS MANAGER    **/
        if ( nv == 3 )
        {
        
            var wasGui2 = GUI.enabled;
            GUI.enabled = adapter._S_PresetsEnabled;
            /* var PRESET_STRING = "Manager of Presets for Design";
             if (!adapter._S_PresetsEnabled) PRESET_STRING += " (Disabled)";
             Label( FULL_RECT, PRESET_STRING );
             if (Application.isPlaying) Label( new Rect( FULL_RECT.x, FULL_RECT.y + FULL_RECT.height * 0.75f, FULL_RECT.width, FULL_RECT.height ), "(Changes Persist after Stop Playing)" );
            SETUPROOT.pretr = FULL_RECT;
            // RESTORE_GUI();
            SETUPROOT.DRAW_WIKI_BUTTON( adapter.pluginname, "Left Panel", "Presets Manager" );  */
            // CHANGE_GUI();
            
            GUI.enabled = wasGui2;
            
            
            // FULL_RECT.y += FULL_RECT.height;
            FULL_RECT.height = 23 + 50 + 2;
            
            
            if ( adapter._S_PresetsEnabled && adapter.IS_HIERARCHY() ) DRAWPRESETS( FULL_RECT );
            else Label( FULL_RECT, "Presets disabled" );
        }
        
        
        
        
        
        /*  var wasGui2 = GUI.enabled;
          GUI.enabled = Hierarchy.par.PresetManagerParams.ENABLE;*/
        // GUI.enabled = wasGui2;
        
        /**   PRESETS MANAGER    **/
        /*///////////////////*/
        
        
        
        
        
        
        
        if ( Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.Escape || Event.current.keyCode == KeyCode.KeypadEnter || Event.current.keyCode == KeyCode.Return) )
        {   Adapter.EventUseFast();
            CloseThis();
            adapter.SKIP_PREFAB_ESCAPE = true;
        }
        /*if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return)
        {   // comformAction(textInput);
            Adapter.EventUseFast();
            CloseThis();
        }*/
        
        
        
        RESTORE_GUI();
        
    }
    
    
    void OnDestroy()
    {   OnDestroySwitcher();
        WriteLastColor();
    }
    
    protected internal override void CloseThis()
    {   WriteLastColor();
        Hierarchy_GUI.SetDirtyObject( adapter );
        base.CloseThis();
        repaintW = null;
        if ( adapter != null ) adapter.RepaintWindow();
    }
    
    static Color32? lastColor;
    static Color32? lastTextColor;
    void UpdateLastHiglightColors( Color32 new32 )
    {   REPAINT_ALL_HIERW();
        // InternalEditorUtility.RepaintAllViews();
        lastColor = new32;
    }
    void UpdateLastHiglightTextColors( Color32 new32 )
    {   REPAINT_ALL_HIERW();
        // InternalEditorUtility.RepaintAllViews();
        lastTextColor = new32;
    }
    static void WriteLastColor()
    {   if ( lastColor.HasValue )
        {   if ( Hierarchy_GUI.GetLastHiglightList( adapter ).Count != 0 )
            {   Hierarchy_GUI.GetLastHiglightList( adapter ).RemoveAll( t => t.r == lastColor.Value.r && t.g == lastColor.Value.g && t.b == lastColor.Value.b );
                if ( Hierarchy_GUI.GetLastHiglightList( adapter ).Count == 0 ) Hierarchy_GUI.GetLastHiglightList( adapter ).Add( lastColor.Value );
                else Hierarchy_GUI.GetLastHiglightList( adapter ).Insert( 0, lastColor.Value );
                lastColor = null;
                while ( Hierarchy_GUI.GetLastHiglightList( adapter ).Count > 20 ) Hierarchy_GUI.GetLastHiglightList( adapter ).RemoveAt( 20 );
            }
        }
        //  if (Hierarchy_GUI.GetLastHiglightList == null) return;
        if ( lastTextColor.HasValue )
        {   if ( Hierarchy_GUI.GetLastHiglightTextList( adapter ).Count != 0 )
                //  {   Hierarchy_GUI.GetLastHiglightTextList( adapter ).RemoveAll( t => t.Equals( lastTextColor.Value ) );
            {   Hierarchy_GUI.GetLastHiglightTextList( adapter ).RemoveAll( t => t.r == lastTextColor.Value.r && t.g == lastTextColor.Value.g && t.b == lastTextColor.Value.b );
                if ( Hierarchy_GUI.GetLastHiglightTextList( adapter ).Count == 0 ) Hierarchy_GUI.GetLastHiglightTextList( adapter ).Add( lastTextColor.Value );
                else Hierarchy_GUI.GetLastHiglightTextList( adapter ).Insert( 0, lastTextColor.Value );
                lastTextColor = null;
                while ( Hierarchy_GUI.GetLastHiglightTextList( adapter ).Count > 20 ) Hierarchy_GUI.GetLastHiglightTextList( adapter ).RemoveAt( 20 );
            }
        }
        
    }
    
    static GUIContent ecc = new GUIContent();
    
    internal static Color PICKER( Rect inputrect, GUIContent content, Color color, bool DRAW_REPAINT = true )       /*55x23*/
    {   if ( GUI.enabled ) EditorGUIUtility.AddCursorRect( inputrect, MouseCursor.Link );
    
        if ( !stylesWasInit ) InitStyles();
        
        var result = GUI.enabled ? Adapter.COLOR_FIELD(new Rect(inputrect.x + 1, inputrect.y + 1, inputrect.width - 2, inputrect.height - 2), ecc, color, false, true, false) : color;
        
        if ( DRAW_REPAINT )
        {   if ( Event.current.type == EventType.Repaint )
            {   var a = GUI.color;
                if ( !GUI.enabled ) GUI.color = new Color( GUI.color.r, GUI.color.g, GUI.color.b, 0.4f );
                HIGHLIGHTER_COLOR_DECORATION.Draw( inputrect, ecc, 0 );
                GUI.color = a;
                GUI.color *= Adapter.SettingsBGColor;
                HIGHLIGHTER_COLOR_FG.Draw( inputrect, ecc, 0 );
                GUI.color = a;
                HIGHLIGHTER_COLOR_PICKER.Draw( new Rect( inputrect.x + inputrect.width - 3 - 17, inputrect.y + 3, 17, 17 ), ecc, 0 );
            }
            Label( inputrect, content );
        }
        
        return result;
    }
    
    
    
    
    
    Color black = new Color(.2f, .2f, .2f, 1f);
    // Color white = new Color(.5f, .5f, .5f, 1f);
    // Color white = new Color(.5f, .5f, .5f, 1f);
    
    /*   internal void DrawStroke(Rect R, string text, Color selCol)
       {   var c = Adapter.GET_SKIN().label.normal.textColor;
           //var cds = Adapter.GET_SKIN().label.fontStyle;
           Adapter.GET_SKIN().label.normal.textColor = EditorGUIUtility.isProSkin ? black : white;
           Adapter.GET_SKIN().label.normal.textColor = new Color( Adapter.GET_SKIN().label.normal.textColor.r, Adapter.GET_SKIN().label.normal.textColor.g, Adapter.GET_SKIN().label.normal.textColor.b, 0.2f );
           // Adapter.GET_SKIN().label.fontStyle = FontStyle.Bold;
           var outline = 1f;
           R.x -= outline;
           R.y -= outline;
           Label( R, text );
           R.x += outline;
           Label( R, text );
           R.x += outline;
           Label( R, text );
           R.y += outline;
           Label( R, text );
           R.y += outline;
           Label( R, text );
           R.x -= outline;
           Label( R, text );
           R.x -= outline;
           Label( R, text );
           R.y -= outline;
           Label( R, text );
           //Adapter.GET_SKIN().label.fontStyle = cds;
    
           selCol.r = 1 - selCol.r;
           selCol.g = 1 - selCol.g;
           selCol.b = 1 - selCol.b;
           var mid = (selCol.r + selCol.g + selCol.b) / 3;
           var sat = 0.7f;
           selCol.r = Mathf.Lerp( selCol.r, mid, sat );
           selCol.g = Mathf.Lerp( selCol.g, mid, sat );
           selCol.b = Mathf.Lerp( selCol.b, mid, sat );
           selCol.a = 1;
           Adapter.GET_SKIN().label.normal.textColor = selCol;
           Label( R, text );
    
           Adapter.GET_SKIN().label.normal.textColor = c;
       }*/
    
    
    int conrollerID;
    private static Action<Adapter.TempColorClass, string> __SetHiglightData;
    private static void SetHiglightData( Adapter.TempColorClass a, string b )
    {   a.SetLastTempColor( adapter );
        __SetHiglightData( a, b );
        TryToClose();
        
    }
    private static Func<Adapter.TempColorClass> GetHiglightData;
    private static Action<SingleList, string> __SetIconColor;
    private static Func<SingleList> GetIconColor;
    
    
    static void SetIconColor( Color32 newCol, string undoString )
    {   var new32 = (Color32)newCol;
        if ( new32.a == 0 && (new32.r != 0 || new32.g != 0 || new32.b != 0) ) new32.a = 255;
        __SetIconColor( new SingleList() { list = new[] { (int)new32.r, new32.g, new32.b, new32.a } .ToList() }, undoString );
        
    }
    
    
    /*
           // if (!wasFocus)
    
                    var wasGui = GUI.enabled;
                    GUI.enabled = Hierarchy.par.USE_HIGLIGHT;
    
                    Label("Highlighter Color (Experimental)");
                    GUI.BeginHorizontal();
                    //   MonoBehaviour.print(EditorGUIUtility.GetControlID(FocusType.Keyboard));
                    //  MonoBehaviour.print(EditorGUIUtility.hotControl);
                    //PIN = true;
    
                    var single = GetColor();
                    var c = single.list;
                    var color = new Color32((byte)c[0], (byte)c[1], (byte)c[2], (byte)c[3]);
                    var child = c[4] == 1;
                    //if (c.a == 0 || c.r == 0 && c.g == 0 && c.b == 0)
                    Hierarchy.RestoreGUI();
                    var newc = (Color32)EditorGUILayout.ColorField(color);
                    if (newc.a == 0 && (newc.r != 0 || newc.g != 0 || newc.b != 0)) newc.a = 255;
                    Hierarchy.ChangeGUI();
                    // MonoBehaviour.print(EditorGUIUtility.GetObjectPickerObject());
    
                    if (GUI.Button("X"))
                    {
                        SetColor(new SingleList() { list = new List<int>() { 0, 0, 0, 0, 0 } });
                        CloseThis();
                    }
                    GUI.EndHorizontal();
                    var newchild = EditorGUILayout.ToggleLeft("Apply to childes", child);
    
                    if (!color.Equals(newc) || child != newchild)
                    {
                        single.list[0] = newc.r;
                        single.list[1] = newc.g;
                        single.list[2] = newc.b;
                        single.list[3] = newc.a;
                        single.list[4] = newchild ? 1 : 0;
                        SetColor(single);
                    }
    
                    // if (!wasFocus)
                    if (Event.current.keyCode == KeyCode.Escape)
                    {
                        CloseThis();
                    }
                    if (Event.current.keyCode == KeyCode.Return)
                    {
                        // comformAction(textInput);
                        CloseThis();
                    }
    
                    GUI.enabled = wasGui;*/
}
}
