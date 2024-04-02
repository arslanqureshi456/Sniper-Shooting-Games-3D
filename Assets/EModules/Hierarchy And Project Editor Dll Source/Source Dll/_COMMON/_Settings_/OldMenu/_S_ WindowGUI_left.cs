using System;
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
//namespace EModules



namespace EModules.EModulesInternal

{
internal partial class SETUPROOT { /*, ISerializable, IDeserializationCallback*/

    void DRAW_PRE_ICON(ref Rect rect, string icon, bool enable)
    {   DRAW_PRE_ICON( ref rect, A.GetIcon( icon ), enable );
    }
    void DRAW_PRE_ICON(ref Rect rect, Texture icon, bool enable)
    {   var ICONSIZE = EditorGUIUtility.singleLineHeight;
        var R = rect;
        
        var OFF = (rect.height - ICONSIZE) / 2;
        rect.x += ICONSIZE;
        rect.width -= ICONSIZE;
        
        R.y += OFF;
        R.height = R.width = ICONSIZE;
        
        var oc = GUI.color;
        if (!enable || !GUI.enabled) GUI.color *= new Color( 1, 1, 1, 0.5f );
        GUI.DrawTexture( R, icon );
        GUI.color = oc;
    }
    
    Vector2 BEGIN_GREENLINE(Rect R, bool enable)
    {   Vector2 LAST_GREEN = Vector2.zero;
        if (Event.current.type == EventType.Repaint && enable
                && GUI.enabled) A.SETUP_GREENLINE.Draw( new Rect( LAST_GREEN.x = R.x + 8, LAST_GREEN.y = R.y + R.height / 2, 4, R.height ), false, false, false, false );
        LAST_GREEN.y += R.height;
        return LAST_GREEN;
    }
    float? DOCKLEFT_calcHeight = null;
#pragma warning disable
    float? DOCKLEFT_calcHeight2 = null;
#pragma warning restore
    
    Rect R;
    private void DOCK_LEFT(float start_X, float wOffset, ref float outY)
    {
    
    
    
    
    
    
    
    
    
        //**    *****************    **//
        
        BEGIN_CATEGORY( ref calcLEFT[0], false, /*new Color32(0, 0, 0, 40)*/null, 5 );
        DrawHeader( "LEFT PANEL" );
        DRAW_WIKI_BUTTON( "Left Panel" );
        
        //   BEGIN_PADDING(10, 10);
        
        R = GET_OFFSETRECT( EditorGUIUtility.singleLineHeight );
        A.par.ENABLE_LEFTDOCK_FIX = A.TOGGLE_LEFT( R, "☰ Enable Left Panel ☰", A.par.ENABLE_LEFTDOCK_FIX, defaultStyle: true);
        
        
        HR();
        
        
        
        
        
        
        var hlenable = GUI.enabled;
        R = GET_OFFSETRECT( EditorGUIUtility.singleLineHeight );
        //  BEGIN_GREENLINE(R, A.par.USE_HIGLIGHT);
        A.par.USE_HIGLIGHT = A.TOGGLE_LEFT( R, "Use Colors", A.par.USE_HIGLIGHT );
        GUI.enabled &= A.par.USE_HIGLIGHT;
        
        BEGIN_PADDING( 20 );
        DRAW_HELP_TEXTURE( "USE_HIGLIGHT", 40, GUI.enabled && A.par.USE_HIGLIGHT );
        END_PADDING();
        
        
        
        
        
        
        
        RawLeftNew( hlenable );
        
        
        /*   R = DRAW_GREENLINE_ANDGETRECT( ref LAST_GREEN, A.par.ENABLE_LEFTDOCK_FIX, true );
           var sss = A.TOGGLE_LEFT(R, "Colors are displayed on the bottom panel", A.par.USE_HIGLIGHT_IN_BOTTOM);
           var asdasdad = GUI.enabled;
           if (GUI.enabled) A.par.USE_HIGLIGHT_IN_BOTTOM = sss;
           GUI.enabled &= A.par.USE_HIGLIGHT_IN_BOTTOM;
        
           // BEGIN_PADDING(20);
           DRAW_HELP_TEXTURE( "USE_HIGLIGHT_FOR_BOTTOM", 36, GUI.enabled && A.par.USE_HIGLIGHT_IN_BOTTOM );
           Space( 10 );
        
        
           //  END_PADDING();
        
           GUI.enabled = asdasdad;*/
        
        // DrawNew(100);
        
        
        
        
        HR();
        Space(EditorGUIUtility.singleLineHeight);
        
        /* HelpBox( "To open the window with the settings of the icons and colors, click the left mouse button next to the object name", MessageType.None );*/
        HelpBox( "You can use right button to search by icons", MessageType.None );
        
        R = GetLastRect();
        R = new Rect( R.x, R.y + R.height + 2, R.width + 5 + 3, A.GetIcon( "HELP3" ).height );
        // DrawTiled(R, GetSetubBGTexture(), 64);
        
        // if (A.IS_HIERARCHY()) DRAW_HELP_TEXTURE( "HELP", 80 );
        // else
        
        if (A.IS_HIERARCHY()) DRAW_HELP_TEXTURE( "HELP3", 210 );
        else
        {   DRAW_HELP_TEXTURE( "HELP3" );
        }
        /*  R.y += R.height;
          R.height = 2;
          Adapter. INTERNAL_BOX(R, "");*/
        
        // DRAW_HELP_TEXTURE("HELP_COMPONENTMENU");
        
        
        
        //  END_PADDING();
        
        // END_PADDING();
        
        END_CATEGORY( ref calcLEFT[0] );
        
        //**    *****************    **//
        
        
        if (GetControlRect( 0 ).y > outY) outY = GetControlRect( 0 ).y;
        if (Event.current.type == EventType.Repaint) DOCKLEFT_calcHeight2 = GetLastRect().y + GetLastRect().height;
        
        Y = start_X;
        X += wOffset;
        
        
        
        
        
        
        
        
        
        GUI.enabled = true;
        
        BEGIN_PADDING(0, -120);
        BEGIN_CATEGORY( ref calcLEFT[2], false, /*new Color32(0, 0, 0, 40)*/null, 15 );
        DrawNew(40);
        
        
        
        DrawHeader( "HighLighter Filters", true );
        
        /* var oh = inputrect.height;
          inputrect.height = EditorGUIUtility.singleLineHeight;
          adapter._S_autorFiltersEnable = adapter.TOGGLE_LEFT(inputrect, "<i>Enable Auto Apply Mode:</i>", adapter._S_autorFiltersEnable);
          Adapter.TOOLTIP(inputrect, "Customized filters will be automatically applied to objects. You can disable auto mode to use this window as a set of pre-configured highlighter styles.");
          inputrect.y += inputrect.height;
          inputrect.height = oh;*/
        
        // BEGIN_PADDING(5, 5);
        
        var asdasdas = GUI.enabled;
        GUI.enabled &= A.par.USE_HIGLIGHT;
        
        var RR = GetControlRect( EditorGUIUtility.singleLineHeight );
        A._S_autorFiltersEnable = A.TOGGLE_LEFT( RR, "<i>Enable Auto Apply Mode:</i>", A._S_autorFiltersEnable);
        Adapter.TOOLTIP(RR, "Customized filters will be automatically applied to objects. You can disable auto mode to use this window as a set of pre-configured highlighter styles.");
        
        
        
        
        var f = M_Colors_Window.GetFH(win);
        var F_RECT = GetControlRect( 0 );
        F_RECT.height  = -1;
        //F_RECT.width += wOffset / 3;
        M_Colors_Window.CHANGE_GUI(A);
        f.source = null;
        var h =  f.DrawFilts(F_RECT, A);
        GetControlRect(h);
        M_Colors_Window.RESTORE_GUI();
        
        // END_PADDING();
        GUI.enabled = asdasdas;
        
        
        END_CATEGORY( ref calcLEFT[2] );
        END_PADDING();
        
        
        GUI.enabled = hlenable;
        
        
        
        
        
        
        /*
        
        Y = start_X;
        X += wOffset;
        
        if (A.IS_HIERARCHY())
        {
            // Space(30);
            BEGIN_CATEGORY( ref calcLEFT[2], false,null, 5 );
            // DrawNew(40);
            DrawHeader( "Presets Manager (Experimental)" );
            DRAW_WIKI_BUTTON( "Left Panel", "Presets Manager" );
            A.par.PresetManagerParams.ENABLE = TOGGLE_LEFT( "Use Presets Manager", A.par.PresetManagerParams.ENABLE );
            Space( 10 );
            // HelpBox( "You can use Component's context menu, or to Quickly Create a Preset in Last Category", MessageType.None );
            HelpBox( "Component's context menu has been disabled", MessageType.Warning );
            //   DrawNew(A.GetIcon("HELP_PRESETMANAGER").height);
            DRAW_HELP_TEXTURE( "HELP_PRESETMANAGER" );
        
            HelpBox( "You can create presets for components with same type.\nIn the near future, there will be an opportunity to save objects including their children", MessageType.Info );
            GUI.enabled &= A.par.PresetManagerParams.ENABLE;
        
            Space( 20 );
            R = GetControlRect( EditorGUIUtility.singleLineHeight );
            A.par.PresetManagerParams.SAVE_GAMEOBJEST = EditorGUI.ToggleLeft( R, "Save 'UnityEngine.Object' References", A.par.PresetManagerParams.SAVE_GAMEOBJEST );
            HelpBox( "Mark it, if you want to save references to 'UnityEngine.Object', such as 'GameObject' or 'MonoBehaviour script'.", MessageType.Info );
        
            END_CATEGORY( ref calcLEFT[2] );
        
            // R = GET_OFFSETRECT(10, 0, 0);
            // if (Event.current.type == EventType.repaint && GUI.enabled) SETUP_BOXCAT.Draw(new Rect(R.x, R.y, R.width, (DOCKLEFT_calcHeight2 ?? (R.y + 200)) - R.y), false, false, false, false);
        }
        */
        
        
        
        
        
        
        if (GetControlRect( 0 ).y > outY) outY = GetControlRect( 0 ).y;
    }
    
    
    
    
    
    
    
    
    
    
    
    
    void RawLeftNew(bool hlenable )
    {
    
        DrawNew();
        var lineRect = GetControlRect(EditorGUIUtility.singleLineHeight);
        
        A.par.highligterOpacity = Mathf.Clamp( EditorGUI.FloatField(lineRect, "HighLighter Opacity:", A.par.highligterOpacity ), 0, 1 );
        DrawNew();
        lineRect = GetControlRect(EditorGUIUtility.singleLineHeight);
        A._S_BottomPaddingForBgColor = Mathf.Clamp( EditorGUI.IntField(lineRect, "Vertical Padding '1':", A._S_BottomPaddingForBgColor ), 0, 16 );
        HR();
        
        
        GUI.enabled = hlenable;
        
        
        
        var on2 = GUI.enabled;
        GUI.enabled = !A.IS_PROJECT();
        DrawNew();
        lineRect = GetControlRect(EditorGUIUtility.singleLineHeight);
        var  nr =  M_Colors_Window.DrawHoverPlaceSettingLine(lineRect, A);
        GetControlRect(  nr.y + nr.height - (  lineRect.y + lineRect.height));
        GUI.enabled = on2;
        //Space(EditorGUIUtility.singleLineHeight);
        
        HR();
        
        A.ENABLE_RICH();
        A.par.COLOR_ICON_SIZE = Mathf.Clamp( EditorGUI.IntField(GET_OFFSETRECT( EditorGUIUtility.singleLineHeight ), "<b>Custom Icons</b> size '" + EditorGUIUtility.singleLineHeight + "'",
                                             A.par.COLOR_ICON_SIZE ), 10, 30 );
        A.DISABLE_RICH();
        
        A._S_USEdefaultIconSize = A.TOGGLE_LEFT(GET_OFFSETRECT( EditorGUIUtility.singleLineHeight ), "<i>Use Default Icons size:</i>", A._S_USEdefaultIconSize);
        
        var  on = GUI.enabled;
        GUI.enabled &= A._S_USEdefaultIconSize;
        A.ENABLE_RICH();
        A._S_defaultIconSize = Mathf.Clamp( EditorGUI.IntField(GET_OFFSETRECT( EditorGUIUtility.singleLineHeight ), "<b>Defaul Iconst</b> size '"
                                            + EditorGUIUtility.singleLineHeight + "'", A._S_defaultIconSize ), 10, 30 );
        A.DISABLE_RICH();
        GUI.enabled = on;
        
        
        
        
        DrawNew();
        lineRect = GetControlRect(EditorGUIUtility.singleLineHeight);
        nr = M_Colors_Window. DrawIconAligmentSettingsLine(lineRect, A);
        GetControlRect(  nr.y + nr.height - (  lineRect.y + lineRect.height));
        
        
        
        
        on = GUI.enabled;
        GUI.enabled = A.HAS_LABEL_ICON();
        lineRect = GetControlRect(EditorGUIUtility.singleLineHeight);
        var nv =  A. TOOGLE_POP( ref lineRect, "Draw yellow marks next to the assigned icons",
                                 A.par.BottomParams.DRAW_FOLDER_STARMARK ? 1 : 0, "Disable", "Enable") == 1;
        GetControlRect(  nr.y + nr.height - (  GetControlRect(0).y));
        if (nv != A.par.BottomParams.DRAW_FOLDER_STARMARK)
        {   A.par.BottomParams.DRAW_FOLDER_STARMARK = nv;
            A.RepaintAllViews();
        }
        GUI.enabled = on;
        
        Space(EditorGUIUtility.singleLineHeight * 2);
        Space(EditorGUIUtility.singleLineHeight );
        
        
        if (A.IS_HIERARCHY())
        {
        
            lineRect.y += lineRect.height;
            var R = GetControlRect(EditorGUIUtility.singleLineHeight);
            A.par.SHOW_NULLS = A.TOGGLE_LEFT(new Rect(R.x += R.height, R.y, R.width - R.height, R.height), "Show Locator for Object without Component", A.par.SHOW_NULLS );
            R.x -= R.height;
            DRAW_PRE_ICON( ref R, "NULL", A.par.SHOW_NULLS && GUI.enabled );
            
            lineRect.y += lineRect.height;
            R = GetControlRect(EditorGUIUtility.singleLineHeight);
            A.par.SHOW_PREFAB_ICON = A.TOGGLE_LEFT( new Rect(R.x += R.height, R.y, R.width - R.height, R.height), "Show Prefab icon", A.par.SHOW_PREFAB_ICON );
            R.x -= R.height;
            DRAW_PRE_ICON( ref R, "PREF", A.par.SHOW_PREFAB_ICON && GUI.enabled );
            
            lineRect.y += lineRect.height;
            R = GetControlRect(EditorGUIUtility.singleLineHeight);
            A.par.SHOW_MISSINGCOMPONENTS = A.TOGGLE_LEFT( new Rect(R.x += R.height, R.y, R.width - R.height, R.height), "Show Warning if Object has missing Component", A.par.SHOW_MISSINGCOMPONENTS );
            R.x -= R.height;
            DRAW_PRE_ICON( ref R, "WARNING", A.par.SHOW_MISSINGCOMPONENTS && GUI.enabled );
        }
        
        /*  if (A.IS_HIERARCHY())
          {   R = GET_OFFSETRECT( 10, 0, 0 );
              if (Event.current.type == EventType.Repaint && GUI.enabled) A.SETUP_BOXCAT.Draw( new Rect( R.x, R.y, R.width, (DOCKLEFT_calcHeight ?? (R.y + 200)) - R.y ), false, false, false, false );
        
              BEGIN_PADDING( 5 );
        
              R = GET_OFFSETRECT( EditorGUIUtility.singleLineHeight, 0, 0 );
              DRAW_PRE_ICON( ref R, "NULL", A.par.SHOW_NULLS && GUI.enabled );
              var NEWGR = BEGIN_GREENLINE(R, A.par.ENABLE_LEFTDOCK_FIX);
              A.par.SHOW_NULLS = A.TOGGLE_LEFT( GET_OFFSETRECT( EditorGUIUtility.singleLineHeight ), "Show Locator for Object without Component", A.par.SHOW_NULLS );
              //  BEGIN_PADDING(40);
              //HelpBox("This is only displayed with 'Components' module", MessageType.None);
              // END_PADDING();
        
        
              R = DRAW_GREENLINE_ANDGETRECT( ref NEWGR, A.par.ENABLE_LEFTDOCK_FIX, false );
              DRAW_PRE_ICON( ref R, "PREF", A.par.SHOW_PREFAB_ICON && GUI.enabled );
              A.par.SHOW_PREFAB_ICON = A.TOGGLE_LEFT( GET_OFFSETRECT( EditorGUIUtility.singleLineHeight ), "Show Prefab icon", A.par.SHOW_PREFAB_ICON );
              //BEGIN_PADDING(40);
              //HelpBox("This is always displayed", MessageType.None);
              //END_PADDING();
        
        
        
              R = DRAW_GREENLINE_ANDGETRECT( ref NEWGR, A.par.ENABLE_LEFTDOCK_FIX, true );
              DRAW_PRE_ICON( ref R, "WARNING", A.par.SHOW_MISSINGCOMPONENTS && GUI.enabled );
              A.par.SHOW_MISSINGCOMPONENTS = A.TOGGLE_LEFT( GET_OFFSETRECT( EditorGUIUtility.singleLineHeight ), "Show Warning if Object has missing Component", A.par.SHOW_MISSINGCOMPONENTS );
              //BEGIN_PADDING(40);
              //HelpBox("This is only displayed with 'Components' module", MessageType.None);
              //END_PADDING();
        
        
              if (Event.current.type == EventType.Repaint) DOCKLEFT_calcHeight = GetLastRect().y + GetLastRect().height;
              END_PADDING();
          }*/
        
        
    }
    
    
    
    
    
    
    void DrawLeftOld()
    {
    
    
    
        DrawNew();
        var lineRect = GetControlRect(EditorGUIUtility.singleLineHeight);
        var nr = M_Colors_Window. DrawIconAligmentSettingsLine(lineRect, A);
        GetControlRect(  nr.y + nr.height - (  lineRect.y + lineRect.height));
        
        
        DrawNew();
        lineRect = GetControlRect(EditorGUIUtility.singleLineHeight);
        nr =  M_Colors_Window.DrawHoverPlaceSettingLine(lineRect, A);
        GetControlRect(  nr.y + nr.height - (  lineRect.y + lineRect.height));
        
        
        
        
        GUI.enabled &= A.par.ENABLE_LEFTDOCK_FIX;
        
        
        
        R = GET_OFFSETRECT( EditorGUIUtility.singleLineHeight, 0, 0 );
        //  DrawNew(R);
        A.par.COLOR_ICON_SIZE = Mathf.Clamp( INT_FIELD( R, "Icons size \"(default: 16)\"", A.par.COLOR_ICON_SIZE ), 10, 30 );
        
        if (A.IS_HIERARCHY())
        {
        
            R = GET_OFFSETRECT( EditorGUIUtility.singleLineHeight );
            A.par.COLOR_ICON_LEFT = !A.TOGGLE_LEFT( R, "Draw icon next to object name", !A.par.COLOR_ICON_LEFT );
            
            //  HelpBox("(default: 12)", MessageType.Info);
        }
        
        //  if (A.IS_PROJECT())
        {   //  Space( 10 );
        
            var on = GUI.enabled;
            GUI.enabled = A.HAS_LABEL_ICON();
            DRAW_HELP_TEXTURE( "FOLDER_STARMARK" );
            R = GET_OFFSETRECT( EditorGUIUtility.singleLineHeight );
            A.par.BottomParams.DRAW_FOLDER_STARMARK = A.TOGGLE_LEFT( R, "Draw yellow marks next to the assigned icons", A.par.BottomParams.DRAW_FOLDER_STARMARK );
            GUI.enabled = on;
            //  Space( 30 );
        }
        
        
        if (A.IS_HIERARCHY())
        {   R = GET_OFFSETRECT( 10, 0, 0 );
            if (Event.current.type == EventType.Repaint && GUI.enabled) A.SETUP_BOXCAT.Draw( new Rect( R.x, R.y, R.width, (DOCKLEFT_calcHeight ?? (R.y + 200)) - R.y ), false, false, false, false );
            
            BEGIN_PADDING( 5 );
            
            R = GET_OFFSETRECT( EditorGUIUtility.singleLineHeight, 0, 0 );
            DRAW_PRE_ICON( ref R, "NULL", A.par.SHOW_NULLS && GUI.enabled );
            var NEWGR = BEGIN_GREENLINE(R, A.par.ENABLE_LEFTDOCK_FIX);
            A.par.SHOW_NULLS = A.TOGGLE_LEFT( GET_OFFSETRECT( EditorGUIUtility.singleLineHeight ), "Show Locator for Object without Component", A.par.SHOW_NULLS );
            //  BEGIN_PADDING(40);
            //HelpBox("This is only displayed with 'Components' module", MessageType.None);
            // END_PADDING();
            
            
            R = DRAW_GREENLINE_ANDGETRECT( ref NEWGR, A.par.ENABLE_LEFTDOCK_FIX, false );
            DRAW_PRE_ICON( ref R, "PREF", A.par.SHOW_PREFAB_ICON && GUI.enabled );
            A.par.SHOW_PREFAB_ICON = A.TOGGLE_LEFT( GET_OFFSETRECT( EditorGUIUtility.singleLineHeight ), "Show Prefab icon", A.par.SHOW_PREFAB_ICON );
            //BEGIN_PADDING(40);
            //HelpBox("This is always displayed", MessageType.None);
            //END_PADDING();
            
            
            
            R = DRAW_GREENLINE_ANDGETRECT( ref NEWGR, A.par.ENABLE_LEFTDOCK_FIX, true );
            DRAW_PRE_ICON( ref R, "WARNING", A.par.SHOW_MISSINGCOMPONENTS && GUI.enabled );
            A.par.SHOW_MISSINGCOMPONENTS = A.TOGGLE_LEFT( GET_OFFSETRECT( EditorGUIUtility.singleLineHeight ), "Show Warning if Object has missing Component", A.par.SHOW_MISSINGCOMPONENTS );
            //BEGIN_PADDING(40);
            //HelpBox("This is only displayed with 'Components' module", MessageType.None);
            //END_PADDING();
            
            
            if (Event.current.type == EventType.Repaint) DOCKLEFT_calcHeight = GetLastRect().y + GetLastRect().height;
            END_PADDING();
        }
        
        
        
    }
    
    
    
    
    
    
    
    
}
}
