using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
#if PROJECT
    using EModules.Project;
#endif

namespace EModules.EModulesInternal


{
internal partial class Adapter {
    internal float FACTOR_8()
    {   return FONT_8() / 8f;
    }
    internal float HALFFACTOR_8()
    {   return ((FONT_8() - 8) / 3 + 8) / 8f;
    }
    
    internal float FACTOR_10()
    {   return FONT_10() / 10f;
    }
    
    internal int FONT_8()
    {   return 8 + Mathf.RoundToInt( par.FONTSIZENEW );
    }
    
    internal int FONT_10()
    {   return 10 + Mathf.RoundToInt( par.FONTSIZENEW );
    }
    
    public int WINDOW_FONT_12() //FILTER DATA HUGE BUTTONS
    {   return Mathf.RoundToInt( 12 * HALFFACTOR_8() );
    }
    
    public int WINDOW_FONT_10() //FILTER DATA HUGE BUTTONS
    {   return Mathf.RoundToInt( 10 * HALFFACTOR_8() );
    }
    
    public int WINDOW_FONT_8() //FILTER DATA HUGE BUTTONS
    {   return Mathf.RoundToInt( 8 * HALFFACTOR_8() );
    }
    
    // float DefaultLabelFontSize;
    /*  Texture2D[][] button_t2x = new Texture2D[4][];
      Texture2D[] blackTexture = { Texture2D.blackTexture };*/
    internal void ChangeGUI( bool changeColorText = true )
    {   if ( guichange )
        {   RestoreGUI();
            //throw new Exception( "Error gui was changed" );
        }
        //  LogProxy.Log("SET");
        guichange = true;
        
        
        
        __button = STYLE_DEFBUTTON;
        
        if ( changeColorText ) __label = STYLE_LABEL_10_COLORED;
        else __label = STYLE_LABEL_10;
        
        if ( !EditorGUIUtility.isProSkin )
            __box = Adapter.STYLE_DEFBOX;
            
            
    }
    
    internal Texture BoxTexture()
    {   if ( box.normal.background ) return box.normal.background;
        return box.normal.scaledBackgrounds[0];
    }
    
    /*
    private static Texture2D __NewBoxT;
    private Texture2D NewBoxT {
        get {
    
            if ( __NewBoxT == null ) {
                Texture2D T =  Adapter.GET_SKIN().box.normal.background;
                if ( !T ) T = Adapter.GET_SKIN().textArea.normal.background;
                if ( !T ) T = Adapter.GET_SKIN().box.normal.scaledBackgrounds[0];
    
                var __NewBoxTtemp =     new Texture2D(  T.width,
                                                        T.height, T.format, false, !Adapter.USE2018 )
                {
                    name = pluginname + "_KEY_#1",
                    hideFlags = HideFlags.DontSave
                };
    
                __NewBoxT = new Texture2D( T.width ,
                                                T.height , TextureFormat.ARGB32 , false , !Adapter.USE2018 ) {
                    name = pluginname + "_KEY_#1" ,
                    hideFlags = HideFlags.DontSave
                };
    
                if ( T && T.width > 0 && T.height > 0 ) {
                    var raw = T.GetRawTextureData();
                    __NewBoxTtemp.LoadRawTextureData( raw );
                    // __NewBoxT.LoadImage( __NewBoxTtemp.EncodeToPNG(), false);
    
                    __NewBoxTtemp.Apply();
                    var pxls = __NewBoxTtemp.GetPixels();
    
                    for ( int y = 0 ; y < pxls.Length ; y++ ) {
                        var c = pxls[y];
                        c.a = 0.2f;
                        pxls[y] = c;
                    }
                    __NewBoxT.SetPixels( pxls );
                    __NewBoxT.Apply();
                }
    
                if ( Application.isPlaying ) GameObject.Destroy( __NewBoxTtemp );
                else GameObject.DestroyImmediate( __NewBoxTtemp , true );
    
            }
            return __NewBoxT;
        }
    }*/
    
    GUIContent content = new GUIContent();
    /*internal Color oldTc;
    internal int oldFOnt, oldFOntl, bt, br, bb, bl;
    internal TextAnchor oldFOnal, al;
    internal Texture2D oldact, oldf, oldBh, oldT, BOXT;
    internal Color oldColl;*/
    
    
    
    
    internal GUIStyle __button, __box, __label;
    internal GUIStyle button { get { return __button ?? (__button = new GUIStyle( Adapter.GET_SKIN().button )); } }
    internal GUIStyle box { get { return __box ?? (__box = new GUIStyle( Adapter.GET_SKIN().box )); } }
    internal GUIStyle label { get { return __label ?? (__label = new GUIStyle( Adapter.GET_SKIN().label )); } }
    
    internal void RestoreGUI()
    {   if ( !guichange ) return;
        guichange = false;
        
        __button = Adapter.GET_SKIN().button;
        __box = Adapter.GET_SKIN().box;
        __label = Adapter.GET_SKIN().label;
        //Adapter.GET_SKIN().box.normal.background = BOXT;
        /* Adapter.GET_SKIN().button.normal.background = oldT;
         Adapter.GET_SKIN().button.hover.background = oldBh;
         Adapter.GET_SKIN().button.focused.background = oldf;
         Adapter.GET_SKIN().button.active.background = oldact;
         Adapter.GET_SKIN().button.alignment = al;
        
         Adapter.GET_SKIN().button.normal.scaledBackgrounds = button_t2x[0];
         Adapter.GET_SKIN().button.hover.scaledBackgrounds = button_t2x[1];
         Adapter.GET_SKIN().button.focused.scaledBackgrounds = button_t2x[2];
         Adapter.GET_SKIN().button.active.scaledBackgrounds = button_t2x[3];
        
        
         Adapter.GET_SKIN().button.fontSize = oldFOnt;
         Adapter.GET_SKIN().button.normal.textColor = oldTc;
         Adapter.GET_SKIN().button.border.top = bt;
         Adapter.GET_SKIN().button.border.right = br;
         Adapter.GET_SKIN().button.border.bottom = bb;
         Adapter.GET_SKIN().button.border.left = bl;
        
        
         Adapter.GET_SKIN().label.fontSize = oldFOntl;
         Adapter.GET_SKIN().label.alignment = oldFOnal;
         Adapter.GET_SKIN().label.normal.textColor = oldColl;*/
    }
    
    
    
}
}
