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

[InitializeOnLoad]
public class _W__InputWindow : _W___IWindow {
    internal override bool PIN
    {   get { return m_PIN; }
        set { m_PIN = value; }
    }
    
    static float destroyV = -1;
    static bool Integer = false;
    internal static void InitTeger( MousePos rect, string title, Adapter adapter, Action<string> conform, Action<string> close = null, string textInputSet = "" )
    {   Integer = true;
        titleWin = title;
        textInput = textInputSet;
        comformAction = conform;
        closeAction = close;
        
        _W___IWindow.private_Init( rect, typeof( _W__InputWindow ), adapter );
    }
    
    internal static _W___IWindow Init( MousePos rect, string title, Adapter adapter, Action<string> conform, Action<string> close = null, string textInputSet = "",
                                       float destroy = -1 )     //  Debug.Log("ASD");
    {
    
        if ( rect.type != MousePos.Type.Input_128_68 && rect.type != MousePos.Type.Input_190_68 )
        {   Debug.LogWarning( "Mismatch type" );
            rect.SetType( MousePos.Type.Input_190_68 );
        }
        
        
        Integer = false;
        titleWin = title;
        textInput = textInputSet;
        comformAction = conform;
        destroyV = destroy;
        closeAction = close;
        _W___IWindow result = null;
        float H = rect.Height;
        rect.Width = adapter.ADDITIONA_INPUT_WINDOWS_WIDTH;
        
        if ( rect.Width > adapter.window().position.width )
        {   rect.Width = 1;
            var d = adapter.window().position.width / rect.Width;
            rect.Width = d;
        }
        /*  if (useClamp)
          {   rect.y -= EditorGUIUtility.singleLineHeight * 1.5f;
              rect = WidnwoRect( WidnwoRectType.Full, new Vector2( rect.x, rect.y ), rect.width, rect.height, adapter, savePosition: new Vector2( rect.x, rect.y ));
        
        
          }*/
        
        if ( conform == null )
        {   if ( Event.current == null && adapter.DEFAUL_SKIN == null )
            {   EditorUtility.DisplayDialog( title, textInputSet, "Ok" );
                return null;
            }
            if ( adapter.DEFAUL_SKIN == null ) adapter.DEFAUL_SKIN = Adapter.GET_SKIN();
            
            var oldL = adapter.DEFAUL_SKIN.label.fontSize;
            adapter.DEFAUL_SKIN.label.fontSize = adapter.WINDOW_FONT_10();
            float w1, w2;
            adapter.DEFAUL_SKIN.label.CalcMinMaxWidth( new GUIContent( textInputSet + " " ), out w1, out w2 );
            var targetW = w1 + 10;
            if ( targetW > rect.Width )
            {   rect.Width = 1;
                var m = targetW / rect.Width;
                rect.Width = m;
                //rect.width = w1 + 10;
            }
            adapter.DEFAUL_SKIN.label.fontSize = oldL;
            var t = titleWin;
            titleWin = null;
            result = _W___IWindow.private_Init( rect, typeof( _W__InputWindow ), adapter, t );
        }
        else
        {   result = _W___IWindow.private_Init( rect, typeof( _W__InputWindow ), adapter );
        }
        
        result.SET_NEW_HEIGHT( adapter, H );
        
        return result;
    }
    
    
    static string titleWin;
    static Action<string> comformAction;
    static Action<string> closeAction;
    static string textInput = "";
    
    
    protected internal override void CloseThis()
    {   if ( closeAction != null ) closeAction( textInput );
    
    
        base.CloseThis();
        
        
        if ( __inputWindow.ContainsKey( typeof( _W__SearchWindow ) ) && __inputWindow[typeof( _W__SearchWindow )] )
        {   if ( !__inputWindow[typeof( _W__SearchWindow )].PIN ) __inputWindow[typeof( _W__SearchWindow )].Focus();
            //__inputWindow[typeof(FillterData)].CloseThis();
        }
        
    }
    
    protected override void Update()
    {   base.Update();
    }
    GUIStyle __inputStyle;
    GUIStyle inputStyle
    {   get
        {   if ( __inputStyle == null )
            {   __inputStyle = new GUIStyle( adapter.label );
            
            }
            return __inputStyle;
        }
    }
    GUIStyle __tfStyle;
    GUIStyle tfStyle
    {   get
        {   if ( __tfStyle == null )
            {   __tfStyle = new GUIStyle( Adapter.GET_SKIN().textField );
            }
            return __tfStyle;
        }
    }
    GUIStyle __tButton;
    GUIStyle tButton
    {   get
        {   if ( __tButton == null )
            {   __tButton = new GUIStyle( adapter.button );
                __tButton.alignment = TextAnchor.MiddleCenter;
            }
            return __tButton;
        }
    }
    protected override void OnGUI()
    {   if ( _inputWindow == null ) return;
    
        if ( adapter == null )
        {   CloseThis();
            return;
        }
        
        
        base.OnGUI();
        
        
        // if (Event.current.type == EventType.keyDown) MonoBehaviour.print(Event.current.keyCode);
        if ( Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape )
        {   Adapter.EventUseFast();
            adapter.SKIP_PREFAB_ESCAPE = true;
            
            closeAction = null;
            CloseThis();
            return;
        }
        if ( Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.KeypadEnter) )
        {   try
            {   if ( comformAction != null ) comformAction( textInput );
            }
            catch ( Exception ex )
            {   adapter.logProxy.LogError( "Changing Value: " + ex.Message + " " + ex.StackTrace );
            }
            
            Adapter.EventUseFast();
            CloseThis();
            return;
        }
        
        
        adapter.ChangeGUI( false );
        
        //  var oldL = Adapter.GET_SKIN().label.fontSize;
        inputStyle.fontSize = adapter.WINDOW_FONT_10();
        GUILayout.Label( titleWin + ":", inputStyle );
        
        if ( comformAction != null )
        {   GUI.SetNextControlName( "MyTextField" );
            //var oldT = Adapter.GET_SKIN().textField.fontSize;
            tfStyle.fontSize = adapter.WINDOW_FONT_10();
            if ( Integer )
            {   int pars = 0;
                int.TryParse( textInput, out pars );
                textInput = EditorGUILayout.IntField( pars, tfStyle ).ToString();
            }
            else textInput = EditorGUILayout.TextField( textInput, tfStyle );
        }
        else       // foreach (var t in textInput.Split('\n'))
        {   //  {
            // EditorGUILayout.LabelField( textInput );
            EditorGUILayout.LabelField( textInput );
            // GUILayout.Label( titleWin , inputStyle );
            
            // }
        }
        
        
        //  var oldS = Adapter.GET_SKIN().button.fontSize;
        tButton.fontSize = adapter.WINDOW_FONT_12();
        if ( GUILayout.Button( "Ok", tButton ) )
        {   try
            {   if ( comformAction != null ) comformAction( textInput );
            }
            catch ( Exception ex )
            {   adapter.logProxy.LogError( "Changing Value: " + ex.Message + " " + ex.StackTrace );
            }
            
            CloseThis();
        }
        // Adapter.GET_SKIN().button.fontSize = oldS;
        
        
        adapter.RestoreGUI();
        
        
        
        {   //  wasFocus = true;
            EditorGUI.FocusTextInControl( "MyTextField" );
            // GUI.FocusControl("MyTextField");
        }
        /*    matColor = EditorGUI.ColorField(new Rect(3, 3, position.width - 6, 15), "New Color:", matColor);
            if (GUI.Button(new Rect(3, 25, position.width - 6, 30), "Change"))
                ChangeColors();*/
        
        if ( destroyV != -1 && Event.current.type == EventType.Repaint )
        {   Repaint();
            destroyV -= adapter.deltaTime;
            if ( destroyV < 0 )
            {   adapter.OneFrameActionOnUpdate = true;
                adapter.OneFrameActionOnUpdateAC += () =>
                {   CloseThis();
                
                };
                
            }
        }
    }
}

}
