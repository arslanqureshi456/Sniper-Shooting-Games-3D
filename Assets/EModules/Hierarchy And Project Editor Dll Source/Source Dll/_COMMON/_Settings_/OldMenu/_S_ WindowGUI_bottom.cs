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
//namespace EModules



    namespace EModules.EModulesInternal

{
    internal partial class SETUPROOT { /*, ISerializable, IDeserializationCallback*/
    
        int IntField(string text, int value)
        {   var R = GET_OFFSETRECT(EditorGUIUtility.singleLineHeight);
            A.PREPARE_TEXTFIELD();
            var r = EditorGUI.IntField(R, text, value);
            A.RESTORE_TEXTFIELD();
            return r;
        }
        
        Texture2D[] SORT_TEXTURES, SORT_TEXTURES_PERSONAL;
        
        Rect APPLY_RECT(Rect R, Rect r, float? width = null)
        {   R.x = r.x;
            R.width = width ?? r.width;
            return R;
        }
        
        //  bool foldBottom = false;
        private void DOCK_BOTTOM(float start_X, float wOffset, ref float outY)
        {
        
            // BEGIN_CATEGORY(ref calcBOTTOM, false, new Color32(150, 170, 210, 40));
            BEGIN_CATEGORY( ref calcBOTTOM, false, new Color32( 0, 0, 0, 40 ), 5 );
            DrawHeader( "BOTTOM PANEL" );
            DRAW_WIKI_BUTTON( "Bottom Panel", "Navigation Module" );
            
            
            
            var R = GET_OFFSETRECT(EditorGUIUtility.singleLineHeight);
            //  if ( Event.current.type == EventType.Repaint && GUI.enabled ) A.SETUP_GREENLINE_HORISONTAL.Draw( new Rect( 0 , R.y + 7 , 20 + PAD , 6 ) , false , false , false , false );
            // var LAST_GREEN = Vector2.zero;
            //  if ( Event.current.type == EventType.Repaint && A.par.ENABLE_BOTTOMDOCK && GUI.enabled ) A.SETUP_GREENLINE.Draw( new Rect( R.x + 8 , LAST_GREEN.y = R.y + R.height / 2 , 4 , R.height ) , false , false , false , false );
            A.par.ENABLE_BOTTOMDOCK = A.TOGGLE_LEFT( R, "☰ Enable Bottom Panel ☰", A.par.ENABLE_BOTTOMDOCK, defaultStyle: true );
            
            HR();
            
            
            R = GET_OFFSETRECT( EditorGUIUtility.singleLineHeight );
            A.par.BottomParams.BOTTOM_TOOLTIPES = A.TOGGLE_LEFT( R, "Enable <b>Tooltips</b> for Bookmarks", A.par.BottomParams.BOTTOM_TOOLTIPES );
            
            
            // if (!DO_FOLD("Settings", FOLD_BOTTOM_KEY)) return;
            
            var wasEn = GUI.enabled;
            GUI.enabled &= A.par.ENABLE_BOTTOMDOCK;
            
            
            
            
            // Space(10);
            
            
            /* EditorGUI.BeginChangeCheck();
            R = GET_OFFSETRECT( EditorGUIUtility.singleLineHeight );
            var LINE_R = new Rect( LAST_GREEN.x = R.x + 8 , LAST_GREEN.y + 10 , 4 , R.y - LAST_GREEN.y );
            if ( Event.current.type == EventType.Repaint && A.par.ENABLE_BOTTOMDOCK && GUI.enabled ) A.SETUP_GREENLINE.Draw( LINE_R , false , false , false , false );
            LAST_GREEN.y = LINE_R.y + LINE_R.height;
            A.par.BOTTOM_AUTOHIDE = A.TOGGLE_LEFT( R , "Minimize Bottom Dock" , A.par.BOTTOM_AUTOHIDE );
             if ( EditorGUI.EndChangeCheck() ) A.RESET_SMOOTH_HEIGHT();*/
            
            R = GET_OFFSETRECT( EditorGUIUtility.singleLineHeight );
            A.SHOW_PARENT_TREE = A.TOGGLE_LEFT( R, "Draw <b>Parents List</b> for Current Selection", A.SHOW_PARENT_TREE );
            DRAW_HELP_TEXTURE( "NEW_BOTTOM_SETUP_PARENTS", null, true, 0, DDD: 1 );
            R = GET_OFFSETRECT( EditorGUIUtility.singleLineHeight );
            A.par.SHOW_PARENT_TREE_CURRENTOBJECT = ! A.TOGGLE_LEFT( R, "Don't Draw <b>Current Selection</b>", !A.par.SHOW_PARENT_TREE_CURRENTOBJECT, defaultStyle: true);
            
            
            
            R = GET_OFFSETRECT( EditorGUIUtility.singleLineHeight );
            LABEL( R, "<i>Sorting:</i>" );
            if (SORT_TEXTURES == null || SORT_TEXTURES[0] == null || SORT_TEXTURES[0] == Texture2D.blackTexture)
            {   SORT_TEXTURES = new Texture2D[] { A.GetIcon( "SETUP_SORT0" ), A.GetIcon( "SETUP_SORT1" ), A.GetIcon( "SETUP_SORT2" ), A.GetIcon( "SETUP_SORT3" ) };
                SORT_TEXTURES_PERSONAL = new Texture2D[] { A.GetIcon( "SETUP_SORT0 PERSONAL" ), A.GetIcon( "SETUP_SORT1 PERSONAL" ), A.GetIcon( "SETUP_SORT2 PERSONAL" ), A.GetIcon( "SETUP_SORT3 PERSONAL" ) };
            }
            BEGIN_PADDING( 20, 20 );
            A.par.BottomParams.SORT_LINES = DRAW_BUTTONS( null, new[] { "", "", "", "" }, A.par.BottomParams.SORT_LINES, 35, true, EditorGUIUtility.isProSkin ? SORT_TEXTURES : SORT_TEXTURES_PERSONAL );
            END_PADDING();
            
            
            Space( 10 );
            
            BEGIN_PADDING( -10 );
            
            // HelpBox( "You can create bookmarks for selections in first line" , MessageType.None );
            // HelpBox( "You can switch between recently selected GameObjects" , MessageType.None );
            //  HelpBox( "You can switch between recently selected Scenes" , MessageType.None );
            /*You can store these GameObjects in this line
            You can switch between recently selected Scenes", MessageType.None);*/
            /*  var lastRect = GetLastRect();
              lastRect.x += 15;
              lastRect.y += lastRect.height;
              // lastRect.width = 327;
              lastRect.width = 327;
              //lastRect.height = 261;
              lastRect.height = 280;
              if (Event.current.type.Equals(EventType.Repaint)) GUI.DrawTexture(lastRect, Hierarchy.GetIcon("BOTTOMHELP"));
              if (!GUI.enabled) Hierarchy.FadeRect(lastRect, 0.7f);
            
            */
            
            
            DRAW_ROWS();
            
            END_PADDING();
            
            Space(EditorGUIUtility.singleLineHeight);
            R = GET_OFFSETRECT( EditorGUIUtility.singleLineHeight );
            A.SAVE_LASTSELECTION_BEFORESCENEWILLSCLOSE = A.TOGGLE_LEFT( R, "Save LastSelections before the scene closes", A.SAVE_LASTSELECTION_BEFORESCENEWILLSCLOSE );

            HelpBox( @"if the option is turned off then the last selection will be saved only when you change something in the scene

if this option is enabled, the last selection will always be saved, but the scene will be marked dirty when the selection changes" , MessageType.None );
            
            // DRAW_HELP_TEXTURE( "BOTTOMHELP" , 261 , GUI.enabled );
            
            
            END_CATEGORY( ref calcBOTTOM );
            
            if (GetControlRect( 0 ).y > outY) outY = GetControlRect( 0 ).y;
            Y = start_X;
            X += wOffset;
            
            
            
            
            BEGIN_CATEGORY( ref calcBOTTOM2, false );
            
            
            
            DrawHeader( "Quick Help", true );
            
            Y -= 20;
            DRAW_HELP_TEXTURE( "BOTTOMHELP", null, GUI.enabled, DDD: 1 );
            
            
            
            // DRAW_HELP_TEXTURE( "ITEMS_PER_ROW", 137, true, 0 );
            //   Space( 10 );
            //  HelpBox( "Use Control+CLICK or Alt+CLICK to Merge or to Except selections", MessageType.Info );
            
            
            
            
            
            
            
            EditorGUI.BeginChangeCheck();
            DRAWCONTROLS();
            if (EditorGUI.EndChangeCheck())
            {   //MonoBehaviour.print("ASd");
                A.RESET_SMOOTH_HEIGHT();
                UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
            }
            
            HR();
            Space(EditorGUIUtility.singleLineHeight);
            
            HelpBox( " - Use the '+' button to add more than one scene\n - Tap on the scene icon to pin button\n - Use 'shift+CLICK' to load scene as additional\n - Use 'alt+CLICK' to revival source in project view",
                     MessageType.None );
            DRAW_HELP_TEXTURE( "HELP_SCENEPIN", 40, GUI.enabled );
            
            
            
            
            
            
            if (A.IS_HIERARCHY())
            {   HelpBox( " - Use Ctrl+Shift+Z / Ctrl+Shift+Y to quickly switch between recent selections", MessageType.None );
                DRAW_HELP_TEXTURE( "ITEMS_PER_ROW", 40, true, 0, -137 );
            }
            
            //  DrawHeader("Hierarchy SnapShots");
            
            HelpBox( " - Use the '+' button to create and save a snapshot of expanded objects",
                     MessageType.None );
            DRAW_HELP_TEXTURE( "HIER_HELP", null, true );
            
            // DRAW_HELP_TEXTURE("HELP_TOPMENU", 180, true, 0);
            
            //  Space( 20 );
            
            //  BEGIN_PADDING(20);
            
            //  Space(20);
            
            
            // Space( 20 );
            GUI.enabled = wasEn;
            
            
            
            END_CATEGORY( ref calcBOTTOM2 );
            
            if (GetControlRect( 0 ).y > outY) outY = GetControlRect( 0 ).y;
            
        }
        
        
        
        
        
        
        void DRAWCONTROLS()
        {   var HHH = 8;
            // A.par.BOTTOM_MAXSCENECOUNT = SLIDER("Max SCENES per row", A.par.BOTTOM_MAXSCENECOUNT, 1, 20);
            var r = DRAW_HELP_TEXTURE( "HOT_CONTROL", align: TextAlignment.Left);
            r.height += HHH;
            Space( HHH );
            HelpBox( r, "CLICK to add/remove object from current selection", MessageType.None );
            // GUI.DrawTexture( r, A.GetIcon( "HOT_CONTROL" ) );
            r = DRAW_HELP_TEXTURE( "HOT_SHIFT", align: TextAlignment.Left);
            r.height += HHH;
            Space( HHH );
            HelpBox( r, "CLICK to add object to current selection", MessageType.None );
            
            r = DRAW_HELP_TEXTURE( "HOT_ALT", align: TextAlignment.Left);
            r.height += HHH;
            Space( HHH );
            HelpBox( r, "CLICK to select object and keep scroll position for hierarchy window", MessageType.None );
            // Space( 10 );
            
        }
        
        
        
        
        
        
        
        
        void DRAW_HIPER(float start_X, float wOffset, ref float outY)
        //void DRAW_HIPER(float start_X, float wOffset, ref float outY)
        {
        
            if (!Adapter.LITE)
            {
            
            
                // BEGIN_CATEGORY( ref calcBOTTOM1, false, new Color32( 0, 0, 0, 40 ), 5 );
                DrawHeader( "HyperGraph" + (Adapter.LITE ? " (Pro Only)" : "") );
                DRAW_WIKI_BUTTON( "Bottom Panel", "HyperGraph" );
                
                //   HelpBox( "You can track the references assigned to the variables of the selected object and the external references to the current object 'CTRL+SHIFT+X'", MessageType.None );
                
                
                
                //  R = DRAW_GREENLINE_ANDGETRECT(ref LAST_GREEN, Hierarchy.par.ENABLE_BOTTOMDOCK);
                BEGIN_PADDING( 10, 10);
                R = GET_OFFSETRECT( EditorGUIUtility.singleLineHeight );
                var LAST_GREEN = BEGIN_GREENLINE(R, A.par.ENABLE_BOTTOMDOCK);
                A.par.HiperGraphParams.AUTOHIDE = A.TOGGLE_LEFT( R, "<b>Auto-Hide</b> when changing selection", A.par.HiperGraphParams.AUTOHIDE );
                var asd = GUI.enabled;
                GUI.enabled &= !A.par.HiperGraphParams.AUTOHIDE;
                R = DRAW_GREENLINE_ANDGETRECT( ref LAST_GREEN, A.par.ENABLE_BOTTOMDOCK );
                A.par.HiperGraphParams.AUTOCHANGE = A.TOGGLE_LEFT( R, "<b>Auto-Reload</b> when changing selection", A.par.HiperGraphParams.AUTOCHANGE );
                GUI.enabled = asd;
                
                BEGIN_PADDING( 20 );
                R = GetControlRect( EditorGUIUtility.singleLineHeight  );
                GUI.Label( R, "<i>Reloading Performance:</i>" );
                BeginHorizontal();
                R = GetControlRect( 24, 5 );
                if (GUI.Button( R, "20%", A.SETUP_BUTTON )) A.par.HiperGraphParams.SCANPERFOMANCE = 2;
                var SELR = R;
                R.x += R.width;
                if (GUI.Button( R, "40%", A.SETUP_BUTTON )) A.par.HiperGraphParams.SCANPERFOMANCE = 4;
                if (A.par.HiperGraphParams.SCANPERFOMANCE == 4) SELR = R;
                R.x += R.width;
                if (GUI.Button( R, "60%", A.SETUP_BUTTON )) A.par.HiperGraphParams.SCANPERFOMANCE = 6;
                if (A.par.HiperGraphParams.SCANPERFOMANCE == 6) SELR = R;
                R.x += R.width;
                if (GUI.Button( R, "80%", A.SETUP_BUTTON )) A.par.HiperGraphParams.SCANPERFOMANCE = 8;
                if (A.par.HiperGraphParams.SCANPERFOMANCE == 8) SELR = R;
                R.x += R.width;
                if (GUI.Button( R, "∞", A.SETUP_BUTTON )) A.par.HiperGraphParams.SCANPERFOMANCE = 10;
                if (A.par.HiperGraphParams.SCANPERFOMANCE == 10) SELR = R;
                GUI.DrawTexture( SELR, A.GetIcon( "BUTBLUE" ) );
                EndHorizontal();
                END_PADDING();
                
                // Hierarchy.par.HiperGraphParams.SCANPERFOMANCE = FloatField("HiperGraphParams.SCANPERFOMANCE", Hierarchy.par.HiperGraphParams.SCANPERFOMANCE);
                R = DRAW_GREENLINE_ANDGETRECT( ref LAST_GREEN, A.par.ENABLE_BOTTOMDOCK );
                A.par.HiperGraphParams.SHOWUPDATINGINDICATOR = A.TOGGLE_LEFT( R, "Show 'Spinner' while Loading", A.par.HiperGraphParams.SHOWUPDATINGINDICATOR );
                R = DRAW_GREENLINE_ANDGETRECT( ref LAST_GREEN, A.par.ENABLE_BOTTOMDOCK, true );
                A.par.HiperGraphParams.RED_HIGKLIGHTING = A.TOGGLE_LEFT( R, "Red color for unassigned variables", A.par.HiperGraphParams.RED_HIGKLIGHTING );
                // END_PADDING();
                // END_PADDING();
                
                
                HR();
                Space(EditorGUIUtility.singleLineHeight);
                
                HelpBox( "You can drag and drop objects in and out of the HyperGraph", MessageType.None );
                DRAW_HELP_TEXTURE( "HELP_HIPERGRAPH", 221, true, 0, DDD: 1);
                HelpBox( "There're Examples in 'HyperGraph Example Scene'", MessageType.None );
                
                END_PADDING();
                
                //  END_CATEGORY( ref calcBOTTOM1 );
                
                
                if (GetControlRect( 0 ).y > outY) outY = GetControlRect( 0 ).y;
                
            }
            
        }
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        void DRAW_BOOKMARKSMANAGER(float start_X, float wOffset, ref float outY)
        //void DRAW_HIPER(float start_X, float wOffset, ref float outY)
        {
        
            if (!Adapter.LITE)
            {
            
                var P = 10;
                
                
                DrawHeader( "BookMarks Manager" );
                DRAW_WIKI_BUTTON( "Bottom Panel", "BookMarks Manager" );
                BEGIN_PADDING( 10, 10);
                /*  HelpBox( "You can track the references assigned to the variables of the selected object and the external references to the current object 'CTRL+SHIFT+X'", MessageType.None );
                  DRAW_HELP_TEXTURE( "HELP_HIPERGRAPH", 221, true, 0 );
                  HelpBox( "You can drag and drop objects in and out of the HyperGraph ", MessageType.None );
                  HelpBox( "There's Example in 'HyperGraph Example Scene'", MessageType.Info );*/
                
                // BEGIN_PADDING( 20 );
                
                
                DRAW_HELP_TEXTURE( "FAVSET_123");
                var i = 1;
                HelpBox( i++ + " - Button Header. Such as last selection or last scenes", MessageType.None );
                HelpBox( i++ + " - This button opens the dockable window for bookmarks manager", MessageType.None );
                HelpBox( i++ + " - This is Category", MessageType.None );
                HelpBox( i++ + " - Filter for objects inside a Category", MessageType.None );
                HelpBox( i++ + " - Show only objects without folders", MessageType.None );
                HelpBox( i++ + " - Description bar", MessageType.None );
                HelpBox( i++ + " - Enable/Disable the feature that allows showing only objects without folders.", MessageType.None );
                Space( 20 );
                DRAW_HELP_TEXTURE( "FAVSET_DRAG" );
                HelpBox( "You can drag and drop objects to another window", MessageType.Info );
                Space( P );
                DRAW_HELP_TEXTURE( "FAVSET_R1" );
                HelpBox( "Right-CLICK to call the context menu for categories", MessageType.Info );
                Space( P );
                DRAW_HELP_TEXTURE( "FAVSET_R2" );
                HelpBox( "Right-CLICK to Remove object", MessageType.Info );
                
                
                
                // END_PADDING();
                // END_PADDING();
                
                /*  if (GetControlRect( 0 ).y > outY) outY = GetControlRect( 0 ).y;
                  Y = start_X;
                  X += wOffset;
                
                
                
                  //  BEGIN_PADDING( 20 );
                
                
                  Space( 10 );
                  R = GET_OFFSETRECT( EditorGUIUtility.singleLineHeight );
                  A.par.FavoritesNavigatorParams.SHOW_SELECTIONS = A.TOGGLE_LEFT( R, "Show blue line for selected objects", A.par.FavoritesNavigatorParams.SHOW_SELECTIONS );
                  Space( 20 );
                  DRAW_HELP_TEXTURE( "FAVORIT_FOLDERS_ICON" );
                  R = GET_OFFSETRECT( EditorGUIUtility.singleLineHeight );
                  A.par.FavoritesNavigatorParams.DEFAULTAUTO_EXPANDFOLDERS = A.TOGGLE_LEFT( R, "Default value for 'objects inside the folder' toggle", A.par.FavoritesNavigatorParams.DEFAULTAUTO_EXPANDFOLDERS );
                  HelpBox( "Enable toggle by default for toggles which enables/disables drawing objects inside the folder (will be used for a new bookmark or category)", MessageType.None );
                
                  DRAW_HELP_TEXTURE( "FAVORIT_LIST_ICON ON" );
                  R = GET_OFFSETRECT( EditorGUIUtility.singleLineHeight );
                  A.par.FavoritesNavigatorParams.DEFAULTAUTO_FLATHIERARCHY = A.TOGGLE_LEFT( R, "Default value for 'subfolders names' toggle", A.par.FavoritesNavigatorParams.DEFAULTAUTO_FLATHIERARCHY );
                  HelpBox( "Enable toggle by default for toggles which enables/disables drawing subfolders names (will be used for a new bookmark or category)", MessageType.None );
                  Space( 20 );
                
                
                  DrawHeader( "Additional Tips" );
                
                
                  DRAW_HELP_TEXTURE( "FAVSET_BUTOFF" );
                  HelpBox( "If you enable this toggle, all subobjects inside the each folders will be displayed in the manager'", MessageType.Info );
                  Space( P );
                  DRAW_HELP_TEXTURE( "FAVSET_BUTTYPE" );
                  HelpBox( "This is if you want to display only objects of a certain type", MessageType.Info );
                  Space( P );
                  DRAW_HELP_TEXTURE( "FAVSET_BUTLIST" );
                  HelpBox( "If you do not want to display folder names and display only subobjects then you have to enable this toggle", MessageType.Info );
                  Space( P );
                
                
                
                  DRAWCONTROLS();*/
                
                END_PADDING();
                
                
                //   END_PADDING();
                
                if (GetControlRect( 0 ).y > outY) outY = GetControlRect( 0 ).y;
                
                
                
            }
            
        }
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        Rect DRAW_GREENLINE_ANDGETRECT(ref Vector2 LAST_GREEN, bool enable, bool LAST = false, float? height = null)
        {
        
            var R = GET_OFFSETRECT(height ?? EditorGUIUtility.singleLineHeight);
            enable &= GUI.enabled;
            if (!enable) LAST = true;
            var LINE_R = new Rect(R.x + 8, LAST_GREEN.y, 4, R.y - LAST_GREEN.y + (LAST ? 0 : R.height));
            LINE_R.x = LAST_GREEN.x;
            if (Event.current.type == EventType.Repaint && enable) A.SETUP_GREENLINE.Draw( LINE_R, false, false, false, false );
            LAST_GREEN.y = LINE_R.y + LINE_R.height;
            return R;
        }
        
        
        static int[] intMaxItemsPopUp;
        static int[] rowsPopUp;
        static int[] OBJECT_X = { 0, 21, 39, 62, 182, 222, 267, 292 };
        static int[] OBJECT_WIDTH = { 16, 19, 19, 120, 36, 36, 16, 55 };
        
        GUIContent CatName = new GUIContent() { tooltip = "Category Name" };
        GUIContent EnableDisable = new GUIContent() { tooltip = "Enable/Disable" };
        GUIContent Sorting = new GUIContent() { tooltip = "Sort Order" };
        GUIContent SortingUP = new GUIContent() { tooltip = "Sort Order", text = "▲" };
        GUIContent SortingDOWN = new GUIContent() { tooltip = "Sort Order", text = "▼" };
        //    GUIContent CellsRowsCount = new GUIContent() { tooltip = "Cells and Rows Amount" };
        GUIContent ButtonsCount = new GUIContent() { tooltip = "Buttons Cells Amount" };
        GUIContent RowsCount = new GUIContent() { tooltip = "Rows Amount" };
        GUIContent higlighterColor = new GUIContent() { tooltip = "Apply Highlighter Colors" };
        GUIContent backgroundColor = new GUIContent() { tooltip = "Background Color" };
        
        void DRAW_ROWS()
        {
        
            A.bottomInterface.SORT_DRAW_ROWS();
            
            var RowsClasses = A.bottomInterface.RowsParams;
            if (intMaxItemsPopUp == null) intMaxItemsPopUp = Enumerable.Repeat( 0, HierParams.MAX_SELECTION_ITEMS - 3 ).Select( (e, i) => i + 3 ).ToArray();
            if (rowsPopUp == null) rowsPopUp = Enumerable.Repeat( 0, 3 ).Select( (e, i) => i + 1 ).ToArray();
            
            
            var r = R = GetControlRect(23);
            r.y += (R.height - EditorGUIUtility.singleLineHeight) / 2;
            
            r.x = R.x + OBJECT_X[1];
            r.width = OBJECT_WIDTH[1] * 2;
            GUI.Label( r, "Sort" );
            GUI.Label( r, Sorting );
            r.x = R.x + OBJECT_X[3];
            r.width = OBJECT_WIDTH[3];
            GUI.Label( r, "Name" );
            GUI.Label( r, CatName );
            r.x = R.x + OBJECT_X[4];
            r.width = OBJECT_WIDTH[4];
            GUI.Label( r, "Cells" );
            GUI.Label( r, ButtonsCount );
            r.x = R.x + OBJECT_X[5];
            r.width = OBJECT_WIDTH[5];
            GUI.Label( r, "Rows" );
            GUI.Label( r, RowsCount );
            r.x = R.x + OBJECT_X[6];
            r.width = OBJECT_WIDTH[6];
            GUI.Label( r, "HL" );
            GUI.Label( r, higlighterColor );
            r.x = R.x + OBJECT_X[7];
            r.width = OBJECT_WIDTH[7];
            GUI.Label( r, "BG" );
            GUI.Label( r, backgroundColor );
            
            
            for (int __index = 0, L = A.bottomInterface.DRAW_INDEX.Length ; __index < L ; __index++)
            {   var i = A.bottomInterface.DRAW_INDEX[__index];
            
                R = GetControlRect( 23 );
                
                
                r = R;
                var _bY = (R.height - EditorGUIUtility.singleLineHeight) / 2;
                r.y += _bY;
                
                r.height = EditorGUIUtility.singleLineHeight;
                
                r.x = R.x + OBJECT_X[0];
                r.width = OBJECT_WIDTH[0];
                RowsClasses[i].Enable = EditorGUI.Toggle( r, EnableDisable, RowsClasses[i].Enable );
                GUI.Label( r, EnableDisable );
                
                
                r.x = R.x + OBJECT_X[1];
                r.width = OBJECT_WIDTH[1];
                var e = GUI.enabled;
                GUI.enabled &= __index != 0;
                if (GUI.Button( r, SortingUP ))
                {   var CI = A.bottomInterface.DRAW_INDEX[__index];
                    var NI = A.bottomInterface.DRAW_INDEX[__index - 1];
                    var OLD = RowsClasses[CI].IndexPos;
                    RowsClasses[CI].IndexPos = RowsClasses[NI].IndexPos;
                    RowsClasses[NI].IndexPos = OLD;
                }
                
                r.x = R.x + OBJECT_X[2];
                r.width = OBJECT_WIDTH[2];
                GUI.enabled = e & __index < L - 1;
                if (GUI.Button( r, SortingDOWN ))
                {   var CI = A.bottomInterface.DRAW_INDEX[__index];
                    var NI = A.bottomInterface.DRAW_INDEX[__index + 1];
                    var OLD = RowsClasses[CI].IndexPos;
                    RowsClasses[CI].IndexPos = RowsClasses[NI].IndexPos;
                    RowsClasses[NI].IndexPos = OLD;
                }
                GUI.enabled = e;
                
                r.x = R.x + OBJECT_X[3];
                r.width = OBJECT_WIDTH[3];
                GUI.Label( r, RowsClasses[i].Name );
                
                
                
                r.x = R.x + OBJECT_X[4];
                r.width = OBJECT_WIDTH[4];
                //  RowsClasses[i].MaxItems = EditorGUI.IntField(r, RowsClasses[i].MaxItems, null, intMaxItemsPopUp);
                RowsClasses[i].MaxItems = EditorGUI.IntField( r, ButtonsCount, RowsClasses[i].MaxItems );
                GUI.Label( r, ButtonsCount );
                
                r.x = R.x + OBJECT_X[5];
                r.width = OBJECT_WIDTH[5];
                RowsClasses[i].Rows = EditorGUI.IntField( r, RowsCount, RowsClasses[i].Rows );
                GUI.Label( r, RowsCount );
                
                if (RowsClasses[i].AllowHiglighter)
                {   r.x = R.x + OBJECT_X[6];
                    r.width = OBJECT_WIDTH[6];
                    RowsClasses[i].HiglighterValue = EditorGUI.Toggle( r, higlighterColor, RowsClasses[i].HiglighterValue );
                    GUI.Label( r, higlighterColor );
                    r.x += 5;
                }
                
                if (RowsClasses[i].AllowBgColor)
                {   r.x = R.x + OBJECT_X[7];
                    r.width = OBJECT_WIDTH[7];
                    r.y = R.y;
                    r.height = R.height;
                    
                    SAVE_ANDCLEAR_PADDING();
                    var newC2 = M_Colors_Window.PICKER(new Rect(r.x + r.width - 55, r.y, 55, 23), backgroundColor, RowsClasses[i].BgColorValue);
                    RESTORE_PADDING();
                    
                    if (RowsClasses[i].BgColorValue != newC2)
                    {   RowsClasses[i].BgColorValue = newC2;
                        GUI.changed = true;
                    }
                }
                
            }
            
            /*A.par.BOTTOM_MAXITEMS = SLIDER("Max OBJECTS per row", A.par.BOTTOM_MAXITEMS, 1, 20);
            
            
            
            
            
            var C1 = GET_OFFSETRECT(EditorGUIUtility.singleLineHeight);
            C1.width /= 3;
            var C2 = C1;
            C2.x += C2.width;
            LABEL(C2, "Bookmarks");
            var C3 = C2;
            C3.x += C3.width;
            LABEL(C3, "Last Selections");
            
            R = GET_OFFSETRECT(25);
            LABEL(R, "ROWS");
            A.par.BOTTOM_MAXCUSTOMROWS = DRAW_BUTTONS(APPLY_RECT(R, C2, C2.width - 20), null, new[] { "1", "2", "3" }, A.par.BOTTOM_MAXCUSTOMROWS - 1, true) + 1;
            A.par.BOTTOM_MAXLASTROWS = DRAW_BUTTONS(APPLY_RECT(R, C3, C2.width - 20), null, new[] { "1", "2", "3" }, A.par.BOTTOM_MAXLASTROWS - 1, true) + 1;
            
            R = GET_OFFSETRECT(23);
            LABEL(R, "BG Tints Color");
            SAVE_ANDCLEAR_PADDING();
            var newC1 = IconData.PICKER(APPLY_RECT(R, C2, 55), new GUIContent(), A.par.BottomParams.FAVORITS_COLOR);
            var newC2 = IconData.PICKER(APPLY_RECT(R, C3, 55), new GUIContent(), A.par.BottomParams.LAST_COLOR);
            RESTORE_PADDING();
            if (A.par.BottomParams.FAVORITS_COLOR != newC1 || A.par.BottomParams.LAST_COLOR != newC2)
            {
                A.par.BottomParams.FAVORITS_COLOR = newC1;
                A.par.BottomParams.LAST_COLOR = newC2;
                GUI.changed = true;
            }
            
            R = GET_OFFSETRECT(EditorGUIUtility.singleLineHeight);
            LABEL(R, "Tints Affect Buttons");
            A.par.BottomParams.FAVORITS_TINT_BUTTONS = A.TOGGLE_LEFT(APPLY_RECT(R, C2), "", A.par.BottomParams.FAVORITS_TINT_BUTTONS);
            A.par.BottomParams.LAST_TINT_BUTTONS = A.TOGGLE_LEFT(APPLY_RECT(R, C3), "", A.par.BottomParams.LAST_TINT_BUTTONS);*/
        }
        
        
        void DRAW_SINGLE_ROW()
        {
        
        }
        
        
    }
}
