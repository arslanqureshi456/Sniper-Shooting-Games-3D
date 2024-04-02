//namespace EModules
#if UNITY_EDITOR
    #define HIERARCHY
    #define PROJECT
#endif

using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

#if PROJECT
    using EModules.Project;
#endif

namespace EModules.EModulesInternal


{
internal partial class SETUPROOT { /*, ISerializable, IDeserializationCallback*/




    internal static void LABEL(Rect R, string text, bool bold = false, TextAnchor? l = null)
    {   Adapter.LABEL( R,  text,  bold, l);
    }
    
    // float? tileCalcAttribute;
    // float? tileCalcCustomIcons;
    
    
    
    void DRAW_CATEGORYES_BUTS()
    {   var names_enable = new[] { true, !Adapter.LITE, !Adapter.LITE, true };
        var names = new[] { "Data Keeper", "Func and Vars", "Memory", "Custom Icons" };
        var LL = names.Length;
        var r = GetControlRect( 50, LL );
        var L = names.Length;
        
        var selected = EditorPrefs.GetInt(A.pluginname + "/" + "Plugin Category Item", 0 );
        if (selected < 0 || selected >= LL || !names_enable[selected]) EditorPrefs.SetInt( A.pluginname + "/" + "Plugin Category Item", 0 );
        // Rect[] buttons = new Rect[names.Length];
        var en = GUI.enabled;
        for (int i = 0 ; i < L ; i++)
        {   var R = r;
            r.x += r.width;
            
            // buttons[i] = R;
            GUI.enabled &= names_enable[i];
            
            var style = i < L / 2 ? EditorStyles.miniButtonLeft : i > L / 2 ? EditorStyles.miniButtonRight : EditorStyles.miniButton;
            var tt = style.border.top;
            var bb = style.border.bottom;
            if (!EditorGUIUtility.isProSkin)
            {   style.border.top = 5;
                style.border.bottom = 5;
            }
            EditorGUIUtility.AddCursorRect( R, MouseCursor.Link );
            var cont = new GUIContent( names[i], names[i] );
            if (!names_enable[i]) cont.tooltip += " (Pro Only)";
            if (GUI.Button( R, cont, style ))
            {   EditorPrefs.SetInt( A.pluginname + "/" + "Plugin Category Item", i );
            }
            if (Event.current.type == EventType.Repaint && EditorPrefs.GetInt( A.pluginname + "/" + "Plugin Category Item", 0 ) == i)
            {   var c = GUI.color;
                GUI.color *= new Color( 1f, 0.7f, 0.6f );
                style.Draw( R, names[i]/*.ToUpper()*/, true, true, false, true );
                GUI.color = c;
            }
            style.border.top = tt;
            style.border.bottom = bb;
            /* if (i != 0 & i != L - 1)
             {
                 R.height -= i == L / 2 ? 10 : 5;
                 R.y +=i== L / 2 ? 10 : 5;
             }*/
            
        }
        GUI.enabled = en;
    }
    
    
    float[] calcRIGHT = new float[10];
    //float[] calcBOTTOM = new float[10];
    
    
    void DOCK_RIGHT(float start_X, float wOffset, ref float outY)
    {
    
        // int I = 0;
        BEGIN_CATEGORY( ref calcRIGHT[0], false, /*new Color32(0, 0, 0, 40)*/null, 5 );
        DrawHeader( "RIGHT PANEL" );
        DRAW_WIKI_BUTTON( "Right Panel", "Components" );
        
        
        var R = GET_OFFSETRECT( EditorGUIUtility.singleLineHeight );
        //  if ( Event.current.type == EventType.Repaint && GUI.enabled ) A.SETUP_GREENLINE_HORISONTAL.Draw( new Rect( 0 , R.y + 7 , 20 + PAD , 6 ) , false , false , false , false );
        //  var LAST_GREEN = BEGIN_GREENLINE( R , A.par.ENABLE_RIGHTDOCK_FIX );
        var newR = A.TOGGLE_LEFT( R, "☰ Enable Right Panel ☰", A.par.ENABLE_RIGHTDOCK_FIX, defaultStyle: true);
        if (A.par.ENABLE_RIGHTDOCK_FIX != newR && newR) A.par.RIGHTDOCK_TEMPHIDE = false;
        A.par.ENABLE_RIGHTDOCK_FIX = newR;
        
        
        // if (!DO_FOLD("Settings", FOLD_Right_KEY)) return;
        
        // Space( 10 );
        HR();
        
        
        var wasEn = GUI.enabled;
        GUI.enabled &= A.par.ENABLE_RIGHTDOCK_FIX;
        
        
        var R2 =  R = GET_OFFSETRECT( EditorGUIUtility.singleLineHeight);
        DrawNew(R);
        R.width -= 80;
        LABEL(R, "CLICK only when <b>pressed</b>:", false);
        R.x += R.width;
        R.width = 80;
        // A.par.USE_BUTTON_TO_INTERACT_AHC = EditorGUI.Popup(R, "Using plugin's modules only if pressed:", A.par.USE_BUTTON_TO_INTERACT_AHC, new[] { "Disabled", "Alt", "Ctrl", "Shift" }, A.SETUP_DROPDOWN);
        var new_i = EditorGUI.Popup(R,  A.par.USE_BUTTON_TO_INTERACT_AHC & 3, new[] { "Disabled", "Alt", "Shift", "Ctrl" });
        TOOLTIP(R2, "You can block interaction with modules and use a special key.");
        var on = GUI.enabled;
        GUI.enabled &= new_i != 0;
        
        var cts = new [] {"...", "'Alt'", "'Shift'", "'Ctrl'" };
        var key = cts[new_i];
        var new_8 = A.TOGGLE_LEFT( GET_OFFSETRECT( EditorGUIUtility.singleLineHeight), "Use the " + key + " for empty lines only", ( A.par.USE_BUTTON_TO_INTERACT_AHC & 8) == 0,
                                   defaultStyle:  true);
        TOOLTIP(R2, "If a module already has a content, you shouldn't use a key to change them.");
        A.par.USE_BUTTON_TO_INTERACT_AHC = new_i | (new_8 ? 0 : 8)                  ;
        
        
        var   lineRect = GetControlRect(EditorGUIUtility.singleLineHeight);
        A._S_HideRightIfNoFunction =  A. TOOGLE_POP(ref lineRect, "Hide <b>Right Bar</b> if " + key + " not pressed", A._S_HideRightIfNoFunction ? 1 : 0, "Show Always", "Hide") == 1;
        Space(EditorGUIUtility.singleLineHeight);
        lineRect = GetControlRect(EditorGUIUtility.singleLineHeight);
        A._S_HideBttomIfNoFunction =  A. TOOGLE_POP(ref lineRect, "Hide <b>Bottom Bar</b> if " + key + " not pressed", A._S_HideBttomIfNoFunction ? 1 : 0, "Show Always", "Hide") == 1;
        Space(EditorGUIUtility.singleLineHeight);
        
        
        
        GUI.enabled = on;
        // if (A.par.USE_BUTTON_TO_INTERACT_AHC != newV) A.par.USE_BUTTON_TO_INTERACT_AHC = newV;
        
        HR();
        
        
        R = GET_OFFSETRECT( EditorGUIUtility.singleLineHeight );
        //  Hierarchy.par.HEADER_OPACITY = Mathf.Clamp01(FLOAT_FIELD(R, "Background Opacity", Hierarchy.par.HEADER_OPACITY ?? Hierarchy.DefaultBGOpacity));
        A.par.HEADER_OPACITY = Mathf.Clamp01( SLIDER( R, "Background Opacity", A.par.HEADER_OPACITY ?? A.DefaultBGOpacity, 0, 1 ) );
        
        
        R = GET_OFFSETRECT( EditorGUIUtility.singleLineHeight );
        //  R = DRAW_GREENLINE_ANDGETRECT( ref LAST_GREEN , true , false );
        var www = R.width + R.x;
        R.width = 80;
        A.par.RIGHTDOCK_TEMPHIDE = A.TOGGLE_LEFT( R, "Auto-Hide", A.par.RIGHTDOCK_TEMPHIDE );
        var oldEnable = GUI.enabled; GUI.enabled &= A.par.RIGHTDOCK_TEMPHIDE;
        R.x += R.width;
        R.width = 130;
        LABEL( R, "If " + A.pluginname + " Width < " );
        R.x += R.width;
        R.width = www - R.x;
        
        A.par.RIGHTDOCK_TEMPHIDEMINWIDTH = Mathf.RoundToInt( SLIDER( R, null, (int)A.par.RIGHTDOCK_TEMPHIDEMINWIDTH, 100, 500/*, ExpandWidth(false) */) );
        GUI.enabled = oldEnable;
        
        R = GET_OFFSETRECT( EditorGUIUtility.singleLineHeight );
        A.par.HEADER_BIND_TO_SCENE_LINE = A.TOGGLE_LEFT( R, "Bind Header To The Top", A.par.HEADER_BIND_TO_SCENE_LINE );
        
        
        //BEGIN_PADDING(20);
        DRAW_HELP_TEXTURE( "BINDHEADERHELP", 41, A.par.HEADER_BIND_TO_SCENE_LINE );
        //  END_PADDING();
        if (A.IS_PROJECT())
        {   R = GET_OFFSETRECT( EditorGUIUtility.singleLineHeight );
            A.par.COMPONENTS_NEXT_TO_NAME = A.TOGGLE_LEFT( R, "'*.*' Extension Next To The Object Name", A.par.COMPONENTS_NEXT_TO_NAME );
            
            // Space( 10 );
        }
        
        
        // PLAYMODE
        on = GUI.enabled;
        R = GET_OFFSETRECT( EditorGUIUtility.singleLineHeight );
        var lastr = BEGIN_GREENLINE(R, !A.par.PLAYMODE_HideRightPanel);
        A.par.PLAYMODE_HideRightPanel = !A.TOGGLE_LEFT( R, "Show 'Right Panel' in PlayMode", !A.par.PLAYMODE_HideRightPanel );
        GUI.enabled &= !A.par.PLAYMODE_HideRightPanel;
        R = DRAW_GREENLINE_ANDGETRECT( ref lastr, true, !A.par.PLAYMODE_HideRightPanel && A.par.PLAYMODE_HideComponents2 );
        A.par.PLAYMODE_HideComponents2 = !A.TOGGLE_LEFT( R, "Show 'Components Module' in PlayMode", !A.par.PLAYMODE_HideComponents2 );
        TOOLTIP(R, "Baked components work fine but you can hide components module to improve performance");
        GUI.enabled &= !A.par.PLAYMODE_HideComponents2;
        R = DRAW_GREENLINE_ANDGETRECT( ref lastr, !A.par.PLAYMODE_HideComponents2, true );
        A.par.PLAYMODE_UseBakedComponents = A.TOGGLE_LEFT( R, "Use 'Baked Components' in PlayMode", A.par.PLAYMODE_UseBakedComponents );
        TOOLTIP(R, "Using baked components improves performance in PlayMode");
        GUI.enabled = on;
        // PLAYMODE
        
        
        HR();
        
        BEGIN_PADDING( 20 );
        float sliderVal = 200;
        R = GET_OFFSETRECT( EditorGUIUtility.singleLineHeight );
        A.par.PADDING_RIGHT = SLIDER( R, "Padding-right:", (int)A.par.PADDING_RIGHT, sliderVal, 0 );
        END_PADDING();
        
        
        R = GET_OFFSETRECT( EditorGUIUtility.singleLineHeight );
        //  R = DRAW_GREENLINE_ANDGETRECT( ref LAST_GREEN , true , true );
        A.par.PADDING_RIGHT_MoveSetActiveToo = A.TOGGLE_LEFT( R, "Padding-right affect non-draggable Categories", A.par.PADDING_RIGHT_MoveSetActiveToo, defaultStyle: false );
        
        
        // R = GET_OFFSETRECT(16, -3);
        
        
        if (A.IS_HIERARCHY())
        {   // Space( 10 );
        
            HR();
            ///  BEGIN_CATEGORY( ref calcRIGHT[2], true, /*new Color32(0, 0, 0, 40)*/null, 5 );
            
            /// DrawHeader( "Enable/Disable GameObject" );
            ///  DRAW_WIKI_BUTTON( "Right Panel", "Enable-Disable GameObject" );
            R = GET_OFFSETRECT( EditorGUIUtility.singleLineHeight );
            var last_rect = BEGIN_GREENLINE(R, A.par.ENABLE_ACTIVEGMAOBJECTMODULE);
            A.par.ENABLE_ACTIVEGMAOBJECTMODULE = A.TOGGLE_LEFT( R, "Enable 'Enable/Disable GameObject' category", A.par.ENABLE_ACTIVEGMAOBJECTMODULE );
            BEGIN_PADDING( 20 );
            /// DRAW_HELP_TEXTURE( "HELP_FIXED_ACTIVE" );
            
            // DrawNew(32);
            DRAW_GREENLINE_ANDGETRECT( ref last_rect, A.par.ENABLE_ACTIVEGMAOBJECTMODULE, true, 0 );
            END_PADDING();
            HelpBox( "To focus on an object in 'SceneView', use Right-CLICK", MessageType.None );
            
            // DrawNew();
            A.par.SMOOTH_FRAME = TOGGLE_LEFT( "Smooth Focusing", A.par.SMOOTH_FRAME );
            
            // END_CATEGORY(ref calcRIGHT[I++]);
            
            
            //Space(30);
            
            
            
            ///  END_CATEGORY( ref calcRIGHT[2] );
            
            
            //  dd++;
            // Space( 20 );
            
        }
        
        
        
        
        
        // Space( 20 );
        
        
        
        END_CATEGORY( ref calcRIGHT[0] );
        if (GetControlRect( 0 ).y > outY) outY = GetControlRect( 0 ).y;
        
        
        
        
        
        
        
        
        
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////CATEGORIOES////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        
        
        
        
        
        
        
        
        Y = start_X;
        X += wOffset;
        BEGIN_CATEGORY( ref calcRIGHT[2], false, /*new Color32(0, 0, 0, 40)*/null, 5 );
        DRAW_CAT_CUSTOMICS( Y, W, ref outY );
        END_CATEGORY( ref calcRIGHT[2] );
        if (GetControlRect( 0 ).y > outY) outY = GetControlRect( 0 ).y;
        
        
        
        
        
        
        // BEGIN_PADDING(20);
        //  END_PADDING();
        
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////CATEGORIOES////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        
        Y = start_X;
        X += wOffset;
        if (A.IS_HIERARCHY())
        {   var categories = new[] { "Show Display Settings", "Show PlayMode Data Keeper Settings", "Show Memory Optimizer Settings", "Show Custom Modules Settings" };
            // var sx = GetControlRect(0).y;
            var colors = new[] {new Color32(244, 67, 54, 127), new Color32(25, 118, 210, 127), new Color32(76, 175, 80, 127), new Color32(255, 158, 34, 127), new Color32(161, 136, 127, 127)};
            
            
            BEGIN_CATEGORY(ref calcRIGHT[4], true, /*new Color32(0, 0, 0, 40)*/null, 5, 25);
            bool wasEndCAtgory = false;
            for (int i = 0; i < 4; i++)
            {
            
            
                Rect RR = Rect.zero;
                
                
                switch (i)
                {   case 0:
                        DrawHeader( "Display of Functions and Vars:", true);
                        // RR = GetPreLastRect();
                        RR = GetLastRect();
                        DRAW_WIKI_BUTTON( "Right Panel", "Components" );
                        break;
                    case 1:
                        DrawHeader("PlayMode Data Keeper (Module)", true);
                        RR = GetLastRect();
                        DRAW_WIKI_BUTTON("Right Panel", "PlayMode Data Keeper");
                        break;
                    case 2:
                        DrawHeader( "Memory Optimizer (Module)", true);
                        RR = GetLastRect();
                        DRAW_WIKI_BUTTON( "Right Panel", "Memory Optimizer" );
                        break;
                    case 3:
                        DrawHeader( "Custom Modules", true);
                        RR = GetLastRect();
                        DRAW_WIKI_BUTTON( "Right Panel", "Memory Optimizer" );
                        break;
                }
                if (i == EditorPrefs.GetInt( A.pluginname + "/" + "Plugin RightMenu Item", 0))
                {
                
                    // BEGIN_CATEGORY(ref calcRIGHT[8], false, /*new Color32(0, 0, 0, 40)*/null, 5, 10);
                    BEGIN_PADDING(10, 10);
                    //var r = GetControlRect(0);
                    //r.height = calcRIGHT[4] + sx - r.x;
                    //BOX(RR, true);
                    switch (i)
                    {   case 0:  DRAW_CAT_VARSFUNKS( GetLastRect().x, W, ref outY );
                            break;
                        case 1: DRAW_PLAYMODEKEEPER( 4, GetLastRect().x, W, ref outY );
                            break;
                        case 2:  DRAW_CAT_MEMORY( GetLastRect().x, W, ref outY );
                        
                            break;
                        case 3:
                            DRAW_CUSTOM_MODULES(GetLastRect().x, W, ref outY );
                            break;
                    }
                    Space( EditorGUIUtility.singleLineHeight );
                    END_PADDING();
                    
                    
                    // END_CATEGORY(ref calcRIGHT[8]);
                    // wasEndCAtgory =  true;
                }
                
                
                
                // var RR = GetControlRect(EditorGUIUtility.singleLineHeight * 2);
                
                EditorGUIUtility.AddCursorRect( RR, MouseCursor.Link );
                BOX(RR, true);
                A.ChangeGUI();
                var b = GUI.Button( RR, categories[i]);
                /* var b = false;
                 if (i == EditorPrefs.GetInt( A.pluginname + "/" + "Plugin RightMenu Item", 0))  b = GUI.Button( RR, categories[i]);
                 else b = GUI.Button( RR, categories[i], A.SETUP_BUTTON );*/
                if (b)
                {   EditorPrefs.SetInt( A.pluginname + "/" + "Plugin RightMenu Item", i );
                }
                A.RestoreGUI();
                
                var  RECT_R = RR;
                Adapter.DrawRect( new Rect( RR.x + RECT_R.height / 2, RECT_R.y + RECT_R.height / 2 - 2, 4, 4 ), new Color(0, 0, 0) );
                Adapter.DrawRect( new Rect( RR.x + RR.width - RECT_R.height / 2, RECT_R.y + RECT_R.height / 2 - 2, 4, 4 ), new Color(0, 0, 0) );
                
                /*  if ( EditorPrefs.GetInt( A.pluginname + "/" + "Plugin RightMenu Item", 0 ) == i )
                  {   var c = GUI.color;
                      GUI.color *= new Color( 0.2f, 0.7f, 1f );
                      BOX( RR, false, true );
                      GUI.color = c;
                
                      Adapter.DrawRect( new Rect( RR.x + RECT_R.height / 2, RECT_R.y + RECT_R.height / 2 - 2, 4, 4 ), new Color( 0.2f, 0.7f, 1f ) );
                      Adapter.DrawRect( new Rect( RR.x + RR.width - RECT_R.height / 2, RECT_R.y + RECT_R.height / 2 - 2, 4, 4 ), new Color( 0.2f, 0.7f, 1f ) );
                
                  }*/
                
                
                
                if (EditorPrefs.GetInt( A.pluginname + "/" + "Plugin RightMenu Item", 0 ) == i) colors[i].a /= 2;
                Adapter.DrawRect(SHRINK(RR, 5), colors[i]);
                
                
                
                
            }
            if (!wasEndCAtgory)
                END_CATEGORY(ref calcRIGHT[4]);
                
        }
        
        
        
        
        
        /*   BEGIN_CATEGORY( ref calcRIGHT[1], false, null, 5 );
        
           DrawHeader( "Customizable Modules:" );
        
        
           END_CATEGORY( ref calcRIGHT[1] );*/
        if (GetControlRect( 0 ).y > outY) outY = GetControlRect( 0 ).y;
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        /*BEGIN_CATEGORY( ref calcRIGHT[3] , false ,null , 5 );
        DrawHeader( "Extra options" );
        
        BEGIN_PADDING( 3 );
        
        // BEGIN_CATEGORY(ref calcRIGHT[I], false, null, 5);
        
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // BEGIN_PADDING(10);
        DRAW_CATEGORYES_BUTS();
        // END_PADDING();
        
        
        GUI.enabled = wasEn;
        GUI.enabled &= A.par.ENABLE_RIGHTDOCK_FIX;
        
        
        
        switch ( EditorPrefs.GetInt( A.pluginname + "/"+ "Settings Plugin Category Item" , 0 ) )
        {
            case 0:
                DRAW__CATEGS( 4 );
                break;
            case 1:
                DRAW_CAT_VARSFUNKS();
                break;
            case 2:
                DRAW_CAT_MEMORY();
                break;
            case 3:
                DRAW_CAT_CUSTOMICS();
                break;
        }
        
        END_PADDING();
        
        Space( 40 );
        
        END_CATEGORY( ref calcRIGHT[3] );
        
        ////////////////////////////ATTRIBUTES
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        
        
        GUI.enabled = wasEn;*/
        
        
        
        if (GetControlRect( 0 ).y > outY) outY = GetControlRect( 0 ).y;
        
    }
    
    
    
    
    
    void DRAW_CAT_MEMORY(float start_X, float wOffset, ref float outY)
    {   var R = new Rect();
        /// BEGIN_PADDING( 3 );
        // BEGIN_PADDING(20);
        if (A.IS_HIERARCHY())
        {
        
        
            Space(EditorGUIUtility.singleLineHeight / 2);
            
            DRAW_HELP_TEXTURE( "HELP_OPTIMIZER" );
            
            Space(EditorGUIUtility.singleLineHeight / 2);
            // HR();
            BeginHorizontal();
            //Space( EditorGUIUtility.singleLineHeight );
            R = GetControlRect( EditorGUIUtility.singleLineHeight);
            R.width -= 50 + 20;
            GUI.Label( R, "Broadcasting Performance:" );
            R.x += R.width;
            R.width = 50;
            A.par.BROADCASTING_PREFOMANCE = Mathf.Clamp( EditorGUI.FloatField( R, (A.par.BROADCASTING_PREFOMANCE - 10f) / 2f ), 5, 95 ) * 2 + 10;
            R.x += R.width;
            R.width = 20;
            GUI.Label( R, "%" );
            EndHorizontal();
            
            DRAW_HELP_TEXTURE( "BROADCASTHELP", 48, true );
            
            HelpBox( "(High values may reduce performance)", MessageType.Warning );
        }
        else
        {   HelpBox( "This module is not available right now", MessageType.Warning );
        }
        //  END_PADDING();
        
        
        ///  END_PADDING();
        
        
        if (GetControlRect( 0 ).y > outY) outY = GetControlRect( 0 ).y;
        
        
        
    }
    
    
    void DRAW_CAT_VARSFUNKS(float start_X, float wOffset, ref float outY)
    {   var R = GET_OFFSETRECT( 0, 0, 0 );
        /* if (Event.current.type == EventType.Repaint
                 && EditorGUIUtility.isProSkin) DrawTiled( new Rect( R.x, R.y, R.width + 7, (tileCalcAttribute ?? (R.y + 200)) - R.y ), A.GetIcon( Adapter.ICONID.SETUP_NOISE ), 171 );*/
        /// BEGIN_PADDING( 5 );
        {
        
        
        
        
            // DrawHeader("'SHOW_IN_HIER' attribute:");
            // DrawNew(32);
            // HelpBox( "Display of variables and functions in the hierarchy window", MessageType.None );
            // DrawNew();
            
            
            
            
            //END_PADDING();
            
            //  DrawNew(20);
            R = GET_OFFSETRECT( EditorGUIUtility.singleLineHeight );
            var LAST_GREEN = BEGIN_GREENLINE( R, true );
            A.par.COMP_ATTRIBUTES_BUTTONS = A.TOGGLE_LEFT( R, "☰ Enable display of <b>Functions</b> ☰", A.par.COMP_ATTRIBUTES_BUTTONS, defaultStyle: true);
            //  DrawNew(20);
            R = DRAW_GREENLINE_ANDGETRECT( ref LAST_GREEN, true, true );
            A.par.COMP_ATTRIBUTES_FIELDS = A.TOGGLE_LEFT( R, "☰ Enable display of <B>Variables</b> ☰", A.par.COMP_ATTRIBUTES_FIELDS, defaultStyle: true );
            
            R = GET_OFFSETRECT( EditorGUIUtility.singleLineHeight );
            A.par.DISPLAYING_NULLSISRED = A.TOGGLE_LEFT( R, "Red color for unassigned variables", A.par.DISPLAYING_NULLSISRED );
            
            // Space( 20 );
            HR();
            Space( EditorGUIUtility.singleLineHeight );
            
            DRAW_HELP_TEXTURE( "HELP_ATTRIBUTES", 47, true );
            
            HelpBox( "Use '[SHOW_IN_HIER]' attribute in your code", MessageType.None );
            DRAW_HELP_TEXTURE( "HELP_ATTRIBUTES_VISUAL", 47, true );
            HelpBox( "There's Code Example inside 'FunctionsDisplaying_Example.cs", MessageType.None );
            
            
            //  BEGIN_PADDING(20);
            // HelpBox( "Here's a simple example of using the attribute declared", MessageType.None );
            // DrawNew(47);
            
            //Space( 10 );
            
            // if (Event.current.type == EventType.Repaint) tileCalcAttribute = GetLastRect().y + GetLastRect().height;
            
        }
        ///    END_PADDING();
        
        
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        
        if (GetControlRect( 0 ).y > outY) outY = GetControlRect( 0 ).y;
        
        
        
        ////////////////////////////OPTIMIZER
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        
        
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        
        
        
        
        
        
        
    }
    
    
    
    
    
    
    void DRAW_CUSTOM_MODULES(float start_X, float wOffset, ref float outY)
    {
    
        R = GetControlRect(EditorGUIUtility.singleLineHeight);
        A.par.USE_CUSTOMMODULES = A.TOGGLE_LEFT( R, "Use Custom Modules:", A.par.USE_CUSTOMMODULES );
        HR();
        
#pragma warning disable
        var  HH = 95;
        R = GetControlRect((HH));
        Space(EditorGUIUtility.singleLineHeight);
        var H3 = 325;
        var R2 = GetControlRect((H3));
#pragma warning restore
        
        
        _W__ModulesWindow.  DrawCustomModules1( A,  R, R2);
        HelpBox( "Source code of default modules are placed into: '" + A.PluginInternalFolder + "/CustomModule_Example.cs'", MessageType.None );
        _W__ModulesWindow. DrawCustomModules2( A,  R, R2);
        
        
        
    }
    
    
    
}
}
