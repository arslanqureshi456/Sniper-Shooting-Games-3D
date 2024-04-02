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



    internal partial class SETUPROOT {
    
    
        void HR(float space = 10)
        {   Space(space);
            R =  GET_OFFSETRECT( 0 );
            R.height = 1;
            INTERNAL_BOX(R);
            Space(space);
            
            
        }
        bool DRAW_MAIN_TOGGLES(float start_y, float offset_x, float HEIGHT)
        {
        
            BEGIN_CATEGORY( ref calcMM, false, null, 5);
            DrawHeader( "Main Settings" );
            // Space( 1 );
            
            
            
            var PAD = 10;
            
            R = GET_OFFSETRECT( 0 );
            R.width =  R.width + PAD * 2;
            R.height = HEIGHT;
            R.y -= 14;
            //   BOX( R );
            //  BEGIN_PADDING( PAD, PAD );
            
            
            
            
            
            
            
            
            
            var updateHeight = false;
            
            /*   R =  GET_OFFSETRECT( 0 );
               R.height = (EditorGUIUtility.singleLineHeight + 2) * 3;
               R.width += 2;
               R = SHRINK(R, -2);
               INTERNAL_BOX(R);*/
            
            var SH = EditorGUIUtility.singleLineHeight ;
            
            
            
            
            
            R = GET_OFFSETRECT( SH);
            //  A.par.ENABLE_WINDOW_ANIMATION = A.TOGGLE_LEFT(R, "Enable 'Drop-down' animation for Windows", A.par.ENABLE_WINDOW_ANIMATION);
            A.par.DOUBLECLICK_IS_EXPAND = A.TOGGLE_LEFT( R, "<b>Double-click</b>: Expand hierarchy item", A.par.DOUBLECLICK_IS_EXPAND, false);
            TOOLTIP(R, "You can enable the ability to expand items by double-clicking. By default, double-click to move camera to an object in the scene window.");
            
            
            if (A.IS_HIERARCHY())
            {   R = GET_OFFSETRECT( SH);
                GUI.enabled = A.hasShowingPrefabHeader;
                DrawNew(R);
                A.par.ESCAPE_CLOSE_PREFAB = A.TOGGLE_LEFT( R, "Escape: Close <b>Prefab Mode</b>", A.par.ESCAPE_CLOSE_PREFAB, false);
                R = GetControlRect( EditorGUIUtility.singleLineHeight );
                LABEL(R, "For which windows does Escape work?");
                R = GetControlRect( EditorStyles.toolbarButton.fixedHeight );
                A.PREFAB_ESCAPE_ALL_WINDOWS =  GUI.Toolbar(R, A.PREFAB_ESCAPE_ALL_WINDOWS ? 1 : 0, new[] {   "Hierarchy/Scene/Inspector", "Foreach Windows"}, EditorStyles.toolbarButton) == 1;
                
                GUI.enabled = true;
            }
            
            
            R = GET_OFFSETRECT( SH);
            A.par.HEADER_OPACITY = Mathf.Clamp01( SLIDER( R, "Modules Background:", A.par.HEADER_OPACITY ?? A.DefaultBGOpacity, 0, 1 ) );
            
            
            
            
            
            
            
            HR();
            //X += offset_x;
            //Y = start_y - 2;
            
            
            
            R = GET_OFFSETRECT( SH);
            var newFontSize = SLIDER(R, "<b>Font</b> Size '11'", A.par.FONTSIZENEW + 10, 10, 17) - 10;
            
            
            var WWW = 85;
            
            DrawNew();
            R = GetControlRect( SH);
            SAVE_ANDCLEAR_PADDING();
            R.width -= WWW;
            // A.par.HIER_LINES_HR = A.TOGGLE_LEFT( R, "Labels Color", A.par.HIER_LINES_HR == 1 ) ? 1 : 0;
            LABEL( R, "Labels Color") ;
            var newC23 = M_Colors_Window.PICKER(new Rect(R.x + R.width, R.y - 3, WWW, 23), new GUIContent(), A._S_TextColor);
            if (A._S_TextColor != newC23)
            {   A._S_TextColor = newC23;
                GUI.changed = true;
            }
            RESTORE_PADDING();
            
            
            
            
            R = GET_OFFSETRECT( SH);
            // GUI.enabled = Adapter.USE2018;
            var newLineHeight = SLIDER(R, "↕ <b>Height</b> of Lines '" +  EditorGUIUtility.singleLineHeight + "'", A.parLINE_HEIGHT, 12, 32);
            /* GUI.enabled = true;
            if (!Adapter.USE2018)
            {   GUI.Label(R, new GUIContent("", "Awaliable for 2018 or newest"));
            }*/
            updateHeight |= newLineHeight != A.parLINE_HEIGHT;
            EditorGUI.BeginChangeCheck();
            R = GET_OFFSETRECT( SH);
            A.par.HEIGHT_APPLY_TOBOTTOM = A.TOGGLE_LEFT(R, "Height of Lines affect the bottom panel", A.par.HEIGHT_APPLY_TOBOTTOM );
            if (EditorGUI.EndChangeCheck()) A.RESET_SMOOTH_HEIGHT();
            //  DRAW_HELP_TEXTURE("LINE_HEIGHT");
            
            if (newFontSize != A.par.FONTSIZENEW || newLineHeight != A.parLINE_HEIGHT)
            {   A.par.FONTSIZENEW = newFontSize;
                A.parLINE_HEIGHT = newLineHeight;
                A.ResetScroll();
                A.RESET_SMOOTH_HEIGHT();
                Repaint();
            }
            
            
            
            // Adapter. INTERNAL_BOX(R, "");
            
            R = GET_OFFSETRECT( SH );
            if (A.IS_HIERARCHY()) A.par.DEEP_WIDTH = SLIDER( R, "Indentation in children '14'", A.par.DEEP_WIDTH ?? 14, 4, 30 );
            else A.par.DEEP_WIDTH = SLIDER( R, "Indentation in children '16'", A.par.DEEP_WIDTH ?? 16, 4, 30 );
            DRAW_HELP_TEXTURE( "HELP_INDENT", 30, yOffset: -100 );
            
            
            HR();
            
            
            R = GET_OFFSETRECT( SH ); //EditorGUIUtility.singleLineHeight
            DRAW_HELP_TEXTURE( "CHILD_COUNT", xOffset: 10 );
            if (Event.current.type == EventType.Repaint && A.par.DRAW_CHILDS_COUNT) A.SETUP_GREENLINE.Draw( new Rect( R.x + 8, R.y + R.height / 2, 4,
                        R.height + SH + SH / 2 + EditorGUIUtility.singleLineHeight + EditorStyles.toolbarButton.fixedHeight
                                                                                                                        ), false, false, false, false );
            A.par.DRAW_CHILDS_COUNT = A.TOGGLE_LEFT( R, "Draw childCount next to GameObject's name", A.par.DRAW_CHILDS_COUNT );
            var oldGUI = GUI.enabled;
            GUI.enabled &= A.par.DRAW_CHILDS_COUNT;
            
            
            DrawNew();
            BEGIN_PADDING(20);
            R = GetControlRect( EditorStyles.toolbarButton.fixedHeight );
            A._S_CountNumber_Align =  GUI.Toolbar(R, A._S_CountNumber_Align + 1, new[] {  "Align Left", "Align Midlle", "Align Right"}, EditorStyles.toolbarButton) - 1;
            R = GetControlRect( EditorGUIUtility.singleLineHeight );
            R.width /= 4;
            LABEL(R, "Offset X");
            R.x += R.width;
            A._S_CountNumber_OffsetX = INT_FIELD(R, A._S_CountNumber_OffsetX);
            R.x += R.width;
            LABEL(R, "Offset Y");
            R.x += R.width;
            A._S_CountNumber_OffsetY = INT_FIELD(R, A._S_CountNumber_OffsetY);
            END_PADDING();
            
            
            R = GET_OFFSETRECT( SH );
            if (Event.current.type == EventType.Repaint && A.par.DRAW_CHILDS_COUNT) A.SETUP_GREENLINE.Draw( new Rect( R.x + 8, R.y + R.height / 2, 4, R.height ), false, false, false, false );
            A.par.HIDE_CHILDCOUNT_IFEXPANDED = A.TOGGLE_LEFT( R, "Hide childCount if line expanded", A.par.HIDE_CHILDCOUNT_IFEXPANDED, defaultStyle: true );
            R = GET_OFFSETRECT( SH );
            A.par.HIDE_CHILDCOUNT_IFROOT = A.TOGGLE_LEFT( R, "Hide childCount if root", A.par.HIDE_CHILDCOUNT_IFROOT, defaultStyle: true);
            TOOLTIP(R, "Do not show the number for the topmost objects.");
            GUI.enabled = oldGUI;
            
            
            
            HR();
            
            
            
            
            
            
            //  R = GetControlRect((0));
            // R.height = calcMAIN == 0 ? 500 : calcMAIN;
            
            /* if (!EditorGUIUtility.isProSkin)
             {
                 if (Event.current.type == EventType.Repaint) A.SETUP_BOXCAT.Draw(R, "", false, false, false, false);
                 if (EditorGUIUtility.isProSkin) DrawTiled(new Rect(R.x + 3, R.y + 3, R.width - 6, R.height - 6), A.GetIcon( Adapter.ICONID.SETUP_NOISE), 171);
            
             }*/
            
            // BEGIN_CATEGORY(ref calcMAIN);
            
            
            // DrawHeader("MAIN");
            // DRAW_WIKI_BUTTON("Getting Started", "General Features");
            
            
            // HelpBox("(Using left and right mouse buttons in the hierarchy window\n do different actions)", MessageType.None);
            
            /* var oldG = GUI.enabled;
             GUI.enabled = !EditorPrefs.GetBool("TreeViewExpansionAnimation", false);*/
            
            
            // Hierarchy.par.HierarhchyLines_Fix = DRAW_BUTTONS("Background Lines Width", new[] { "none", "HalfLength", "FullLine" }, Hierarchy.par.HierarhchyLines_Fix, 25);
            
            
            
            
            
            //  R = GET_OFFSETRECT(0);
            // R.height = 105;
            // BEGIN_PADDING(15, 15);
            // Space(5);
            var r = GetControlRect(SH);
            SAVE_ANDCLEAR_PADDING();
            r.width -= WWW;
            A.par.HIER_LINES_CHESSE = A.TOGGLE_LEFT( r, "BG Chess Lines", A.par.HIER_LINES_CHESSE == 1 ) ? 1 : 0;
            var newC1 = M_Colors_Window.PICKER(new Rect(r.x + r.width, r.y - 3, WWW, 23), new GUIContent(), A.CHESS_COLOR);
            if (A.CHESS_COLOR != newC1)
            {   A.CHESS_COLOR = newC1;
                GUI.changed = true;
            }
            
            RESTORE_PADDING();
            r = GetControlRect( SH);
            SAVE_ANDCLEAR_PADDING();
            r.width -= WWW;
            A.par.HIER_LINES_HR = A.TOGGLE_LEFT( r, "BG Separating Lines", A.par.HIER_LINES_HR == 1 ) ? 1 : 0;
            var newC2 = M_Colors_Window.PICKER(new Rect(r.x + r.width, r.y - 3, WWW, 23), new GUIContent(), A.HR_COLOR);
            if (A.HR_COLOR != newC2)
            {   A.HR_COLOR = newC2;
                GUI.changed = true;
            }
            
            
            RESTORE_PADDING();
            r = GetControlRect( SH);
            var rn = GUI.enabled;
            GUI.enabled = A.par.HIER_LINES_HR == 1 || A.par.HIER_LINES_CHESSE == 1;
            A.par.HierarhchyLines_Fix = A.TOGGLE_LEFT(r,  "BG Lines Clamp", A.par.HierarhchyLines_Fix == 1 ) ? 1 : 2;
            
            GUI.enabled = rn;
            
            /* Label("Background Lines Width");
             A.par.HierarhchyLines_Fix = DRAW_BUTTONS(null, new[] { "none", "HalfLength", "FullLine" }, A.par.HierarhchyLines_Fix, 25, true);
             A.par.HIER_LINES_HR = DRAW_BUTTONS("Draw Horisontal Lines", new[] { "none", "Draw" }, A.par.HIER_LINES_HR, 20);
             A.par.HIER_LINES_CHESSE = DRAW_BUTTONS("Draw Chess Lines", new[] { "none", "Draw" }, A.par.HIER_LINES_CHESSE, 20);
             Space(15);*/
            /*R = GetControlRect((24));
            Hierarchy.par.HierarhchyLines_Fix = EditorGUI.Popup(R, "Background Lines Width", Hierarchy.par.HierarhchyLines_Fix, new[] { "none", "HalfLength", "FullLine" }, SETUP_DROPDOWN);
            EditorGUIUtility.AddCursorRect(R, MouseCursor.Link);*/
            
            /*   GUI.enabled = oldG;
               if (EditorPrefs.GetBool("TreeViewExpansionAnimation", false))
                   HelpBox("At this time, this lines can not be displayed simultaneously with the 'TreeView Animation'", MessageType.Warning);*/
            
            /*   R = GetControlRect((18));
               R.y -= 6;
               R.height = 24;
               Hierarchy.par.HIER_LINES_HR = EditorGUI.Popup(R, "Draw Horisontal Lines", Hierarchy.par.HIER_LINES_HR, new[] { "Draw", "Hide" }, SETUP_DROPDOWN);
               EditorGUIUtility.AddCursorRect(R, MouseCursor.Link);
            
               R = GetControlRect((18));
               R.y -= 6;
               R.height = 24;
               Hierarchy.par.HIER_LINES_CHESSE = EditorGUI.Popup(R, "Draw Chess Lines", Hierarchy.par.HIER_LINES_CHESSE, new[] { "Draw", "Hide" }, SETUP_DROPDOWN);
               EditorGUIUtility.AddCursorRect(R, MouseCursor.Link);*/
            
            
            
            
            r = GetControlRect( SH );
            SAVE_ANDCLEAR_PADDING();
            r.width -= WWW;
            A.par.DRAW_HIERARHCHY_LINES_V2 = A.TOGGLE_LEFT( r, "Child Lines", A.par.DRAW_HIERARHCHY_LINES_V2 == 1 ) ? 1 : 0;
            var newC3 = M_Colors_Window.PICKER(new Rect(r.x + r.width, r.y - 3, WWW, 23), new GUIContent(), A.CHILDREN_LINE_COLOR);
            if (A.CHILDREN_LINE_COLOR != newC3)
            {   A.CHILDREN_LINE_COLOR = newC3;
                GUI.changed = true;
            }
            RESTORE_PADDING();
            
            
            HR();
            
            
            r = GetControlRect( SH );
            A.par.ENABLE_PING_Fix = A.TOGGLE_LEFT(r, "Enable ping if changing Object's parameters", A.par.ENABLE_PING_Fix );
            TOOLTIP(GetLastRect(), "When changing the parameters of an object, its name will be highlighted with a special animated frame.");
            
            r = GetControlRect( SH );
            if (A.IS_HIERARCHY())
            {   A.par.LOCK_SELECTION = A.TOGGLE_LEFT( r, "Enable 'SceneView' blocking for locked objects", A.par.LOCK_SELECTION );
                TOOLTIP(GetLastRect(), "You cannot select locked objects in the scene window.");
            }
            
            //A.par.USE_HORISONTAL_SCROLL = TOGGLE_LEFT( "Use 'Horisontal Scroll' (Experimental)", A.par.USE_HORISONTAL_SCROLL );
            
            
            
            //A.par.DRAWLINES = 1 - DRAW_BUTTONS("Child Lines", new[] { "Hide", "Show" }, 1 - A.par.DRAWLINES, 25);
            
            
            
            
            /* R = GetControlRect((24));
             Hierarchy.par.DRAWLINES = EditorGUI.Popup(R, "Hierarchy Lines", Hierarchy.par.DRAWLINES, new[] { "Show", "Hide" }, SETUP_DROPDOWN);
             EditorGUIUtility.AddCursorRect(R, MouseCursor.Link);*/
            /* EditorStyles.toolbar.normal.background = SETUP_SLIDER.normal.background;
             var vs = EditorStyles.numberField;*/
            
            //Space(10);
            
            
            /*    var oldTF = Adapter.GET_SKIN().textField;
                Adapter.GET_SKIN().textField = SETUP_TEXTFIELD;*/
            
            // updateHeight |= A.par.FONTSIZE != newFontSize;
            //  Adapter.GET_SKIN().textField = oldTF;
            
            // END_PADDING();
            
            
            
            /*var slid = new GUIStyle(Adapter.GET_SKIN().horizontalSliderThumb);
            slid.normal.background = slid.focused.background = slid.hover.background = SETUP_SLIDER.normal.background;
            slid.active.background = SETUP_SLIDER.active.background;
            // EditorStyles.numberField = SETUP_SLIDER;
            var oldS = Adapter.GET_SKIN().horizontalSliderThumb;
            Adapter.GET_SKIN().horizontalSliderThumb = slid;
            var newFontSize = Slider("Font Size", Hierarchy.par.FONTSIZE + 10, 10, 17) - 10;
            Adapter.GET_SKIN().horizontalSliderThumb = oldS;*/
            // EditorStyles.numberField = vs;
            
            // DrawNew();
            
            
            //Adapter. INTERNAL_BOX(R, "");
            
            
            
            
            
            
            // Hierarchy.par.ENABLE_PING_Fix = ToggleLeft("Enable ping if changing GameObject's parameters", Hierarchy.par.ENABLE_PING_Fix);
            
            
            
            
            END_CATEGORY(ref calcMM);
            
            
            // END_PADDING();
            
            
            if (Event.current.type == EventType.Repaint && calcMAIN < 0) calcMAIN = GetLastRect().y + GetLastRect().height - (-calcMAIN) + 10;
            
            // END_CATEGORY//(ref calcMAIN);
            //
            
            
            
            
            
            
            
            
            
            
            return updateHeight;
            
            
            
        }
        
        
        ///DEPRIOCATED MAIN TOGGLES
        bool OLD_DRAW_MAIN_TOGGLES(float start_y, float offset_x, float HEIGHT)
        {
        
        
        
            Space( 1 );
            
            
            
            var PAD = 10;
            
            R = GET_OFFSETRECT( 0 );
            R.width = 2 * R.width + PAD * 2;
            R.height = HEIGHT;
            R.y -= 14;
            BOX( R );
            BEGIN_PADDING( PAD, PAD );
            
            
            
            
            
            
            
            
            
            var updateHeight = false;
            
            /*   R =  GET_OFFSETRECT( 0 );
               R.height = (EditorGUIUtility.singleLineHeight + 2) * 3;
               R.width += 2;
               R = SHRINK(R, -2);
               INTERNAL_BOX(R);*/
            
            
            var R2 =  R = GET_OFFSETRECT( EditorGUIUtility.singleLineHeight );
            DrawNew(R);
            R.width -= 80;
            LABEL(R, "Using plugin's modules only if pressed:");
            R.x += R.width;
            R.width = 80;
            // A.par.USE_BUTTON_TO_INTERACT_AHC = EditorGUI.Popup(R, "Using plugin's modules only if pressed:", A.par.USE_BUTTON_TO_INTERACT_AHC, new[] { "Disabled", "Alt", "Ctrl", "Shift" }, A.SETUP_DROPDOWN);
            A.par.USE_BUTTON_TO_INTERACT_AHC = EditorGUI.Popup(R,  A.par.USE_BUTTON_TO_INTERACT_AHC, new[] { "Disabled", "Alt", "Ctrl", "Shift" });
            TOOLTIP(R2, "You can block interaction with modules and use a special button.");
            // if (A.par.USE_BUTTON_TO_INTERACT_AHC != newV) A.par.USE_BUTTON_TO_INTERACT_AHC = newV;
            
            
            
            R = GET_OFFSETRECT( EditorGUIUtility.singleLineHeight );
            //  A.par.ENABLE_WINDOW_ANIMATION = A.TOGGLE_LEFT(R, "Enable 'Drop-down' animation for Windows", A.par.ENABLE_WINDOW_ANIMATION);
            A.par.DOUBLECLICK_IS_EXPAND = A.TOGGLE_LEFT( R, "Double-click - Expand hierarchy item", A.par.DOUBLECLICK_IS_EXPAND );
            TOOLTIP(R, "You can enable the ability to expand items by double-clicking. By default, double-click to move camera to an object in the scene window.");
            
            
            if (A.IS_HIERARCHY())
            {   R = GET_OFFSETRECT( EditorGUIUtility.singleLineHeight);
                GUI.enabled = A.hasShowingPrefabHeader;
                DrawNew(R);
                A.par.ESCAPE_CLOSE_PREFAB = A.TOGGLE_LEFT( R, "Escape - Close Prefab Mode", A.par.ESCAPE_CLOSE_PREFAB );
                GUI.enabled = true;
            }
            
            
            R = GET_OFFSETRECT( EditorGUIUtility.singleLineHeight);
            A.par.HEADER_OPACITY = Mathf.Clamp01( SLIDER( R, "Modules Background:", A.par.HEADER_OPACITY ?? A.DefaultBGOpacity, 0, 1 ) );
            
            
            HR();
            
            
            
            
            
            
            
            //  R = GetControlRect((0));
            // R.height = calcMAIN == 0 ? 500 : calcMAIN;
            
            /* if (!EditorGUIUtility.isProSkin)
             {
                 if (Event.current.type == EventType.Repaint) A.SETUP_BOXCAT.Draw(R, "", false, false, false, false);
                 if (EditorGUIUtility.isProSkin) DrawTiled(new Rect(R.x + 3, R.y + 3, R.width - 6, R.height - 6), A.GetIcon( Adapter.ICONID.SETUP_NOISE), 171);
            
             }*/
            
            // BEGIN_CATEGORY(ref calcMAIN);
            
            
            // DrawHeader("MAIN");
            // DRAW_WIKI_BUTTON("Getting Started", "General Features");
            
            
            // HelpBox("(Using left and right mouse buttons in the hierarchy window\n do different actions)", MessageType.None);
            
            /* var oldG = GUI.enabled;
             GUI.enabled = !EditorPrefs.GetBool("TreeViewExpansionAnimation", false);*/
            
            
            // Hierarchy.par.HierarhchyLines_Fix = DRAW_BUTTONS("Background Lines Width", new[] { "none", "HalfLength", "FullLine" }, Hierarchy.par.HierarhchyLines_Fix, 25);
            
            var SH = EditorGUIUtility.singleLineHeight + 4;
            
            
            
            
            //  R = GET_OFFSETRECT(0);
            // R.height = 105;
            // BEGIN_PADDING(15, 15);
            // Space(5);
            
            var r = GetControlRect(SH);
            SAVE_ANDCLEAR_PADDING();
            r.width -= 55;
            A.par.HIER_LINES_CHESSE = A.TOGGLE_LEFT( r, "BG Chess Lines", A.par.HIER_LINES_CHESSE == 1 ) ? 1 : 0;
            var newC1 = M_Colors_Window.PICKER(new Rect(r.x + r.width, r.y, 55, 23), new GUIContent(), A.CHESS_COLOR);
            if (A.CHESS_COLOR != newC1)
            {   A.CHESS_COLOR = newC1;
                GUI.changed = true;
            }
            
            RESTORE_PADDING();
            r = GetControlRect( SH);
            SAVE_ANDCLEAR_PADDING();
            r.width -= 55;
            A.par.HIER_LINES_HR = A.TOGGLE_LEFT( r, "BG Separating Lines", A.par.HIER_LINES_HR == 1 ) ? 1 : 0;
            var newC2 = M_Colors_Window.PICKER(new Rect(r.x + r.width, r.y, 55, 23), new GUIContent(), A.HR_COLOR);
            if (A.HR_COLOR != newC2)
            {   A.HR_COLOR = newC2;
                GUI.changed = true;
            }
            
            
            RESTORE_PADDING();
            var rn = GUI.enabled;
            GUI.enabled = A.par.HIER_LINES_HR == 1 || A.par.HIER_LINES_CHESSE == 1;
            A.par.HierarhchyLines_Fix = TOGGLE_LEFT( "BG Lines Clamp", A.par.HierarhchyLines_Fix == 1 ) ? 1 : 2;
            
            GUI.enabled = rn;
            
            /* Label("Background Lines Width");
             A.par.HierarhchyLines_Fix = DRAW_BUTTONS(null, new[] { "none", "HalfLength", "FullLine" }, A.par.HierarhchyLines_Fix, 25, true);
             A.par.HIER_LINES_HR = DRAW_BUTTONS("Draw Horisontal Lines", new[] { "none", "Draw" }, A.par.HIER_LINES_HR, 20);
             A.par.HIER_LINES_CHESSE = DRAW_BUTTONS("Draw Chess Lines", new[] { "none", "Draw" }, A.par.HIER_LINES_CHESSE, 20);
             Space(15);*/
            /*R = GetControlRect((24));
            Hierarchy.par.HierarhchyLines_Fix = EditorGUI.Popup(R, "Background Lines Width", Hierarchy.par.HierarhchyLines_Fix, new[] { "none", "HalfLength", "FullLine" }, SETUP_DROPDOWN);
            EditorGUIUtility.AddCursorRect(R, MouseCursor.Link);*/
            
            /*   GUI.enabled = oldG;
               if (EditorPrefs.GetBool("TreeViewExpansionAnimation", false))
                   HelpBox("At this time, this lines can not be displayed simultaneously with the 'TreeView Animation'", MessageType.Warning);*/
            
            /*   R = GetControlRect((18));
               R.y -= 6;
               R.height = 24;
               Hierarchy.par.HIER_LINES_HR = EditorGUI.Popup(R, "Draw Horisontal Lines", Hierarchy.par.HIER_LINES_HR, new[] { "Draw", "Hide" }, SETUP_DROPDOWN);
               EditorGUIUtility.AddCursorRect(R, MouseCursor.Link);
            
               R = GetControlRect((18));
               R.y -= 6;
               R.height = 24;
               Hierarchy.par.HIER_LINES_CHESSE = EditorGUI.Popup(R, "Draw Chess Lines", Hierarchy.par.HIER_LINES_CHESSE, new[] { "Draw", "Hide" }, SETUP_DROPDOWN);
               EditorGUIUtility.AddCursorRect(R, MouseCursor.Link);*/
            
            
            
            
            r = GetControlRect( SH );
            SAVE_ANDCLEAR_PADDING();
            r.width -= 55;
            A.par.DRAW_HIERARHCHY_LINES_V2 = A.TOGGLE_LEFT( r, "Child Lines", A.par.DRAW_HIERARHCHY_LINES_V2 == 1 ) ? 1 : 0;
            var newC3 = M_Colors_Window.PICKER(new Rect(r.x + r.width, r.y, 55, 23), new GUIContent(), A.CHILDREN_LINE_COLOR);
            if (A.CHILDREN_LINE_COLOR != newC3)
            {   A.CHILDREN_LINE_COLOR = newC3;
                GUI.changed = true;
            }
            RESTORE_PADDING();
            
            
            HR();
            A.par.ENABLE_PING_Fix = TOGGLE_LEFT( "Enable ping if changing Object's parameters", A.par.ENABLE_PING_Fix );
            TOOLTIP(GetLastRect(), "When changing the parameters of an object, its name will be highlighted with a special animated frame.");
            
            if (A.IS_HIERARCHY())
            {   A.par.LOCK_SELECTION = TOGGLE_LEFT( "Enable 'SceneView' blocking for locked objects", A.par.LOCK_SELECTION );
                TOOLTIP(GetLastRect(), "You cannot select locked objects in the scene window.");
            }
            
            //A.par.USE_HORISONTAL_SCROLL = TOGGLE_LEFT( "Use 'Horisontal Scroll' (Experimental)", A.par.USE_HORISONTAL_SCROLL );
            
            
            
            //A.par.DRAWLINES = 1 - DRAW_BUTTONS("Child Lines", new[] { "Hide", "Show" }, 1 - A.par.DRAWLINES, 25);
            
            
            
            
            /* R = GetControlRect((24));
             Hierarchy.par.DRAWLINES = EditorGUI.Popup(R, "Hierarchy Lines", Hierarchy.par.DRAWLINES, new[] { "Show", "Hide" }, SETUP_DROPDOWN);
             EditorGUIUtility.AddCursorRect(R, MouseCursor.Link);*/
            /* EditorStyles.toolbar.normal.background = SETUP_SLIDER.normal.background;
             var vs = EditorStyles.numberField;*/
            
            //Space(10);
            
            
            /*    var oldTF = Adapter.GET_SKIN().textField;
                Adapter.GET_SKIN().textField = SETUP_TEXTFIELD;*/
            
            // updateHeight |= A.par.FONTSIZE != newFontSize;
            //  Adapter.GET_SKIN().textField = oldTF;
            
            // END_PADDING();
            
            
            
            /*var slid = new GUIStyle(Adapter.GET_SKIN().horizontalSliderThumb);
            slid.normal.background = slid.focused.background = slid.hover.background = SETUP_SLIDER.normal.background;
            slid.active.background = SETUP_SLIDER.active.background;
            // EditorStyles.numberField = SETUP_SLIDER;
            var oldS = Adapter.GET_SKIN().horizontalSliderThumb;
            Adapter.GET_SKIN().horizontalSliderThumb = slid;
            var newFontSize = Slider("Font Size", Hierarchy.par.FONTSIZE + 10, 10, 17) - 10;
            Adapter.GET_SKIN().horizontalSliderThumb = oldS;*/
            // EditorStyles.numberField = vs;
            
            // DrawNew();
            
            
            //Adapter. INTERNAL_BOX(R, "");
            
            X += offset_x;
            Y = start_y - 2;
            
            
            
            R = GET_OFFSETRECT( EditorGUIUtility.singleLineHeight );
            var newFontSize = SLIDER(R, "Font Size '11'", A.par.FONTSIZENEW + 10, 10, 17) - 10;
            
            
            R = GET_OFFSETRECT( EditorGUIUtility.singleLineHeight );
            // GUI.enabled = Adapter.USE2018;
            var newLineHeight = SLIDER(R, "Height of Lines '" +  EditorGUIUtility.singleLineHeight + "'", A.parLINE_HEIGHT, 12, 32);
            /* GUI.enabled = true;
            if (!Adapter.USE2018)
            {   GUI.Label(R, new GUIContent("", "Awaliable for 2018 or newest"));
            }*/
            updateHeight |= newLineHeight != A.parLINE_HEIGHT;
            EditorGUI.BeginChangeCheck();
            A.par.HEIGHT_APPLY_TOBOTTOM = TOGGLE_LEFT( "Height of Lines affect the bottom panel", A.par.HEIGHT_APPLY_TOBOTTOM );
            if (EditorGUI.EndChangeCheck()) A.RESET_SMOOTH_HEIGHT();
            //  DRAW_HELP_TEXTURE("LINE_HEIGHT");
            
            if (newFontSize != A.par.FONTSIZENEW || newLineHeight != A.parLINE_HEIGHT)
            {   A.par.FONTSIZENEW = newFontSize;
                A.parLINE_HEIGHT = newLineHeight;
                A.ResetScroll();
                A.RESET_SMOOTH_HEIGHT();
                Repaint();
            }
            
            
            
            // Adapter. INTERNAL_BOX(R, "");
            
            R = GET_OFFSETRECT( EditorGUIUtility.singleLineHeight );
            if (A.IS_HIERARCHY()) A.par.DEEP_WIDTH = SLIDER( R, "Indentation in children '14'", A.par.DEEP_WIDTH ?? 14, 4, 30 );
            else A.par.DEEP_WIDTH = SLIDER( R, "Indentation in children '16'", A.par.DEEP_WIDTH ?? 16, 4, 30 );
            DRAW_HELP_TEXTURE( "HELP_INDENT", 30, yOffset: -100 );
            
            
            HR();
            
            
            R = GET_OFFSETRECT( EditorGUIUtility.singleLineHeight );
            DRAW_HELP_TEXTURE( "CHILD_COUNT", xOffset: 10 );
            if (Event.current.type == EventType.Repaint && A.par.DRAW_CHILDS_COUNT) A.SETUP_GREENLINE.Draw( new Rect( R.x + 8, R.y + R.height / 2, 4, R.height + 26 ), false, false, false, false );
            A.par.DRAW_CHILDS_COUNT = A.TOGGLE_LEFT( R, "Draw childCount next to GameObject's name", A.par.DRAW_CHILDS_COUNT );
            var oldGUI = GUI.enabled;
            GUI.enabled &= A.par.DRAW_CHILDS_COUNT;
            
            
            DrawNew();
            R = GetControlRect( EditorStyles.toolbarButton.fixedHeight );
            A._S_CountNumber_Align =  GUI.Toolbar(R, A._S_CountNumber_Align, new[] {  "Align Left", "Align Midlle", "Align Right"}, EditorStyles.toolbarButton);
            R = GetControlRect( EditorGUIUtility.singleLineHeight );
            R.width /= 2;
            A._S_CountNumber_OffsetX = INT_FIELD(R, "Offset X", A._S_CountNumber_OffsetX);
            R.x += R.width;
            A._S_CountNumber_OffsetY = INT_FIELD(R, "Offset Y", A._S_CountNumber_OffsetY);
            
            
            R = GET_OFFSETRECT( EditorGUIUtility.singleLineHeight );
            if (Event.current.type == EventType.Repaint && A.par.DRAW_CHILDS_COUNT) A.SETUP_GREENLINE.Draw( new Rect( R.x + 8, R.y + R.height / 2, 4, R.height ), false, false, false, false );
            A.par.HIDE_CHILDCOUNT_IFEXPANDED = A.TOGGLE_LEFT( R, "Hide childCount if line expanded", A.par.HIDE_CHILDCOUNT_IFEXPANDED );
            R = GET_OFFSETRECT( EditorGUIUtility.singleLineHeight );
            A.par.HIDE_CHILDCOUNT_IFROOT = A.TOGGLE_LEFT( R, "Hide childCount if root", A.par.HIDE_CHILDCOUNT_IFROOT );
            TOOLTIP(R, "Do not show the number for the topmost objects.");
            GUI.enabled = oldGUI;
            
            
            
            // Hierarchy.par.ENABLE_PING_Fix = ToggleLeft("Enable ping if changing GameObject's parameters", Hierarchy.par.ENABLE_PING_Fix);
            
            
            
            
            
            
            END_PADDING();
            
            
            if (Event.current.type == EventType.Repaint && calcMAIN < 0) calcMAIN = GetLastRect().y + GetLastRect().height - (-calcMAIN) + 10;
            
            // END_CATEGORY//(ref calcMAIN);
            //
            
            
            
            
            
            
            
            
            
            
            return updateHeight;
            
            
            
        }
    }
}
