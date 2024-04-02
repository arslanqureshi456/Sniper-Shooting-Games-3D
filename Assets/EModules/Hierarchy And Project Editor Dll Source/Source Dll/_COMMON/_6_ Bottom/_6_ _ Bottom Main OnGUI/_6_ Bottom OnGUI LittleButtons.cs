
#if UNITY_EDITOR
    #define HIERARCHY
    #define PROJECT
#endif


using System;
using System.Collections;
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
internal partial class Adapter {

    internal void Label( Rect r, string s, TextAnchor an )
    {   var a  = label.alignment;
        label.alignment = an;
        GUI.Label( r, s, label );
        label.alignment = a;
    }
    internal void Label( Rect r, string s )
    {   GUI.Label( r, s, label );
    }
    internal void Label( Rect r, GUIContent s )
    {   GUI.Label( r, s, label );
    }
    
    internal bool Button( Rect r, string s )
    {   return GUI.Button( r, s, button );
    }
    internal bool Button( Rect r, string s, TextAnchor an )
    {   var a  = button.alignment;
        button.alignment = an;
        var res = GUI.Button( r, s, button );
        button.alignment = a;
        return res;
    }
    internal bool Button( Rect r, GUIContent s )
    {   return GUI.Button( r, s, button );
    }
    internal bool Button( Rect r, GUIContent s, TextAnchor an )
    {   var a  = button.alignment;
        button.alignment = an;
        var res = GUI.Button( r, s, button );
        button.alignment = a;
        return res;
        
    }
    
    
    
    internal sealed partial class BottomInterface {
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
        
        
        
        
        
        private void DRAW_FOLD_ICONS( ref Rect foldRect, int scene )
        {
        
            DRAW_FOLD_ICONS_CONTROLID = 0;
            foldRect.y = Mathf.RoundToInt( foldRect.y );
            hiperRect = foldRect;
            hiperRect.y = Mathf.RoundToInt( hiperRect.y );
            hiperRect.width = 36;
            hiperRect.height = 16;
            
            hiperRect.x = foldRect.x + foldRect.width - hiperRect.width;
            
            
            
            if ( !Adapter.LITE )
            {
            
                if ( adapter.IS_PROJECT() )     //FAV WINDOW
                {   EditorGUIUtility.AddCursorRect( hiperRect, adapter.FAV_ENABLE() ? MouseCursor.ArrowMinus : MouseCursor.ArrowPlus );
                    GUI.DrawTexture( hiperRect, adapter.GetIcon( (hyperGraph.HYPER_FULL_ENABLE() ? "FAVORIT ACTIVE" : "FAVORIT") + (EditorGUIUtility.isProSkin ? "" : " PERSONAL") ) );
                    FoldActions( (Action < bool? >)favorGraph.SWITCH_ACTIVE );
                    iconsContent.tooltip = "Favorites Navigator";
                    iconsContent.text = null;
                    Label( hiperRect, iconsContent );
                    
                    foldRect.width -= hiperRect.width + 5;
                    hiperRect.x -= 20;
                    ++DRAW_FOLD_ICONS_CONTROLID;
                    
                }
                else       //HYPER WIN
                {   EditorGUIUtility.AddCursorRect( hiperRect, adapter.HYPER_ENABLE() ? MouseCursor.ArrowMinus : MouseCursor.ArrowPlus );
                    GUI.DrawTexture( hiperRect, adapter.GetIcon( (hyperGraph.HYPER_FULL_ENABLE() ? "HIPERGRAPH_ACTIVE" : "HIPERGRAPH") + (EditorGUIUtility.isProSkin ? "" : " PERSONAL") ) );
                    FoldActions( (Action < bool? >)hyperGraph.SWITCH_ACTIVE );
                    iconsContent.tooltip = "Open Object References - HyperGraph (CTRL+SHIFT+X)";
                    iconsContent.text = null;
                    Label( hiperRect, iconsContent );
                    
                    foldRect.width -= hiperRect.width + 5;
                    hiperRect.x -= 20;
                    ++DRAW_FOLD_ICONS_CONTROLID;
                }
            }
            
            hiperRect.height = 18;
            hiperRect.width = hiperRect.height;
            hiperRect.y -= (foldRect.height - hiperRect.height) / 2;
            hiperRect.y -= (18 - EditorGUIUtility.singleLineHeight);
            var OO = 4;
            hiperRect.y = Mathf.RoundToInt( hiperRect.y );
            hiperRect.width -= OO;
            hiperRect.height -= OO;
            //hiperRect.x += OO;
            
            FOLDER_BUTTON( ref foldRect, adapter.par.SHOW_SCENES_ROWS, SET_SCEN, "NEW_BOTTOM_BUTTON_SCENE", "Scene Buttons Rows" );
            FOLDER_BUTTON( ref foldRect, adapter.par.SHOW_HIERARCHYSLOTS_ROWS, SET_HIER, "NEW_BOTTOM_BUTTON_HIERARCHY", "Hierarchy States Rows" );
            hiperRect.x -= 4;
            FOLDER_BUTTON( ref foldRect, adapter.par.SHOW_LAST_ROWS, SET_LAST, "NEW_BOTTOM_BUTTON_LAST", "Last Selections Rows" );
            hiperRect.x -= 4;
            FOLDER_BUTTON( ref foldRect, adapter.par.SHOW_BOOKMARKS_ROWS, SET_BOOK, "NEW_BOTTOM_BUTTON_BOOKMARKS", "Bookmarks Rows" );
            
            
            
            var treeRect = foldRect;
            treeRect.x += treeRect.height;
            treeRect.width = hiperRect.x - treeRect.x;
            
            var doubleClickRect = treeRect;
            
            DRAW_CATEGORY_NAME( ref foldRect, treeRect, scene );
            
            
            
            
            if ( Event.current.type == EventType.MouseDown && anycatsenable )
            {   if ( Event.current.clickCount == 2 && Event.current.button == 0 && doubleClickRect.Contains( Event.current.mousePosition ) )
                {   adapter.par.BOTTOM_AUTOHIDE = !adapter.par.BOTTOM_AUTOHIDE;
                    adapter.SavePrefs();
                }
            }
            
            
            
            
            hiperRect = foldRect;
            hiperRect.width = hiperRect.height;
            foldRect = hiperRect;
            hiperRect.y -= 1;
            // var foldRectContains = foldRect.Contains(Event.current.mousePosition);
            // var foldAction = HierarchyController.selection_window == w && HierarchyController.selection_button == 0;
            if ( anycatsenable )
            {   GUI.DrawTexture( hiperRect, adapter.GetIcon( !adapter.par.BOTTOM_AUTOHIDE ? "NEW_BOTTOM_ARROW_DOWN" : "NEW_BOTTOM_ARROW_UP" ) );
                //  EditorStyles.foldout.Draw(foldRect, /*new GUIContent(""), */false, false, !adapter.par.BOTTOM_AUTOHIDE,foldRectContains && foldAction);
                Label( hiperRect, FoldContent );
                EditorGUIUtility.AddCursorRect( hiperRect, MouseCursor.Link );
            }
            
            
            // foldRect.width -= hiperRect.width + 5;
            
        }
        
        
        
        void DRAW_CATEGORY_NAME( ref Rect foldRect, Rect treeRect, int scene )
        {   if ( adapter.DISABLE_DESCRIPTION( (scene) ) || !adapter.par.SHOW_BOOKMARKS_ROWS || adapter.par.BOTTOM_AUTOHIDE ) return;
            EditorGUIUtility.AddCursorRect( foldRect, MouseCursor.Link );
            
            adapter.bottomInterface.GET_BOOKMARKS( ref list, scene );
            faveContent.text = list[HierarchyController.GetCategoryIndex( scene )].name;
            
            hiperRect = treeRect;
            FOLDER_BUTTON( ref foldRect, null, SET_BOOK, null, "Click - to change current category", faveContent.text );
            
        }
        
        
        
        void DRAW_CATEGORY_NAME( ref Rect foldRect, Rect treeRect )
        {   if ( adapter.SHOW_PARENT_TREE && (
                        (adapter.IS_HIERARCHY() && Selection.activeGameObject && Selection.activeGameObject.scene.IsValid()) ||
                        adapter.IS_PROJECT() && Selection.activeObject && (!Selection.activeGameObject || !Selection.activeGameObject.scene.IsValid())
                    ) )
            {   var O = adapter.GetHierarchyObjectByInstanceID(Selection.activeObject.GetInstanceID());
            
                v2offset.Set( treeRect.x, treeRect.y );
                GUI.BeginClip( treeRect );
                treeRect.x = treeRect.y = 0;
                // var BW = 80;
                // treeRect.x += treeRect.width - BW;
                // treeRect.width = BW;
                var p = adapter.par.SHOW_PARENT_TREE_CURRENTOBJECT ? O : O.parent(adapter);
                
                var  parentRect = treeRect;
                parentRect.x = parentRect.x + parentRect.width;
                var CHAR = "► ";
                //var CHAR = "| ";
                var FS = 8;
                while ( p/* && parentRect.x + BW > 0*/ != null )     //calcConetnt.text = "►" + p.name;
                {   calcConetnt.text = CHAR + p.name;
                    float w = CalcWidth( FS);
                    //if (w > BW) w = BW;
                    parentRect.width = w;
                    parentRect.x -= parentRect.width;
                    
                    
                    p = p.parent( adapter );
                    //treeRect.x -= treeRect.width;
                }
                
                
                var offset = 0f;
                // if (parentRect.x > 0) offset = parentRect.x;
                if ( parentRect.x > 0 ) offset = parentRect.x;
                p = adapter.par.SHOW_PARENT_TREE_CURRENTOBJECT ? O : O.parent( adapter );
                //parentRect.x -= offset;
                
                parentRect = treeRect;
                parentRect.x = parentRect.x + parentRect.width;
                while ( p/* && parentRect.x + BW > 0*/ != null )
                {   calcConetnt.text = CHAR + p.name;
                    float w = CalcWidth( FS);
                    // if (w > BW) w = BW;
                    parentRect.width = w;
                    parentRect.x -= parentRect.width;
                    
                    
                    hiperRect = parentRect;
                    hiperRect.x = hiperRect.x - offset;
                    
                    
                    if ( Event.current.control || Event.current.shift || Event.current.alt )
                        EditorGUIUtility.AddCursorRect( hiperRect, Event.current.alt ? MouseCursor.ArrowMinus : MouseCursor.ArrowPlus );
                    else
                        EditorGUIUtility.AddCursorRect( hiperRect, MouseCursor.Link );
                        
                    Object obj = null;
                    if ( hiperRect.Contains( Event.current.mousePosition ) || HierarchyController.selection_action != null ) obj = EditorUtility.InstanceIDToObject( p.id );
                    
                    FOLDER_BUTTON( ref foldRect, obj, SET_SELECT_OBECJT, null, null, calcConetnt.text, v2offset, SET_DRAG_OBECJT, FS );
                    
                    
                    
                    
                    p = p.parent( adapter );
                }
                
                
                
                GUI.EndClip();
                
                
            }
        }
        
        
        
        
        
        
        
        
        
        
        
        void FOLDER_BUTTON( ref Rect foldRect, object value, Action<object> setValue, string iconName, string toolTip, string text = null, Vector2? offset = null,
                            Action<object> dragAction = null, int? fontSIze = null )
        {
        
            ++DRAW_FOLD_ICONS_CONTROLID;
            
            
            //EditorGUIUtility.AddCursorRect(hiperRect,adapter.HYPER_ENABLE() ? MouseCursor.ArrowMinus : MouseCursor.ArrowPlus);
            var cccc = GUI.color;
            
            
            if ( iconName != null )
            {   var iconRect = hiperRect;
                if ( !EditorGUIUtility.isProSkin )     //
                {   iconRect.height -= 1;
                    iconRect.width -= 1;
                    iconName += " PERSONAL";
                }
                
                GUI.DrawTexture( iconRect, adapter.GetIcon( iconName ) );
                
                if ( !(bool)value )
                {   GUI.color *= coloAlpha;
                    GUI.DrawTexture( iconRect, adapter.GetIcon( iconName + " OFF" ) );
                }
            }
            GUI.color = cccc;
            
            // if (GUI.Button(hiperRect, ""))
            
            if ( Event.current.type == EventType.MouseDown && hiperRect.Contains( Event.current.mousePosition ) )
            {   HierarchyController.selection_button = DRAW_FOLD_ICONS_CONTROLID + HYPER_OFFSET;
            
            
            
                Adapter.EventUseFast();
                //var captureID = HierarchyController.selection_button;
                HierarchyController.selection_window = adapter.window();
                var captureRect = hiperRect;
                if ( offset.HasValue ) captureRect.Set( captureRect.x + offset.Value.x, Mathf.RoundToInt( captureRect.y + offset.Value.y ), captureRect.width, captureRect.height );
                HierarchyController.selection_action = ( mouseUp, deltaTIme ) =>      //  Debug.Log(mouseUp + " " + captureRect + " " + Event.current.mousePosition);
                {   if ( mouseUp && captureRect.Contains( Event.current.mousePosition ) )
                    {   setValue( value );
                    }
                    else if ( Event.current.type == EventType.MouseDrag && !captureRect.Contains( Event.current.mousePosition )
                              && !Event.current.control && !Event.current.shift && !Event.current.alt )
                    {   if ( dragAction != null )
                        {   dragAction( value );
                            HierarchyController.selection_action = null;
                        }
                    }
                    
                    return Event.current.delta.x == 0 && Event.current.delta.x == 0;
                    
                    
                    
                }; // ACTION
            }
            
            if ( Event.current.type == EventType.Repaint )
            {   if ( HierarchyController.selection_action != null && HierarchyController.selection_button != null )
                {   var hover = HierarchyController.selection_button == HYPER_OFFSET + DRAW_FOLD_ICONS_CONTROLID;
                    if ( hover )
                    {   GUI.DrawTexture( hiperRect, adapter.GetIcon( "BUTBLUE" ) );
                    }
                }
                
                
            }
            iconsContent.tooltip = toolTip;
            iconsContent.text = text;
            
            
            labelLeft8style.fontSize = fontSIze ?? adapter.FONT_8();
            GUI.Label( hiperRect, iconsContent, labelLeft8style );
            
            /* foldRect.x += hiperRect.width + 5;                */
            
            foldRect.width -= hiperRect.width;
            hiperRect.x -= hiperRect.width;
        }
        GUIStyle __iconStyle;
        GUIStyle labelLeft8style
        {   get
            {   if ( __iconStyle == null )
                {   __iconStyle = new GUIStyle( adapter.label );
                    __iconStyle.alignment = TextAnchor.MiddleLeft;
                }
                return __iconStyle;
            }
        }
        
        
        
        
        
        
        
        
        
        // GEGEGENENENERIRIRICICIC MEEEENNNUUU
        // GEGEGENENENERIRIRICICIC MEEEENNNUUU
        // GEGEGENENENERIRIRICICIC MEEEENNNUUU
        
        
        
        internal void CREATE_BUTTON_CUSTOM_MENU( BottomController controller, int scene )
        {   controller.adapter.bottomInterface.GET_BOOKMARKS( ref list, scene );
            m_CREATE_BUTTON_CUSTOM_MENU( controller, scene, true );
        }
        internal GenericMenu CREATE_BUTTON_CUSTOM_MENU( BottomController controller, int scene, bool showMenu, GenericMenu menu )
        {   controller.adapter.bottomInterface.GET_BOOKMARKS( ref list, scene );
            return m_CREATE_BUTTON_CUSTOM_MENU( controller, scene, showMenu, menu );
        }
        void DO_BUTTON_DESCRIPTION( BottomController controller, int scene )
        {   if ( controller.IS_MAIN ) adapter.par.BottomParams.SHOW_ALL_DESCRIPTIONS_INHIER = !adapter.par.BottomParams.SHOW_ALL_DESCRIPTIONS_INHIER;
            else adapter.par.BottomParams.SHOW_ALL_DESCRIPTIONS_INWIN = !adapter.par.BottomParams.SHOW_ALL_DESCRIPTIONS_INWIN;
            
            controller.adapter.SavePrefs();
            controller.adapter.RepaintWindowInUpdate();
        }
        internal void DO_BUTTON_COLOR( BottomController controller, int scene )      // var pos = InputData.WidnwoRect(controller.IS_MAIN, Event.current.mousePosition, 190, 68, controller.adapter );
        {   var pos = new MousePos( Event.current.mousePosition, MousePos.Type.ColorChanger_230_0, controller.IS_MAIN, controller.adapter);
            _W__BottomWindow_ColorCategories.Init( pos, controller.adapter, scene );
        }
        
        internal GenericMenu m_CREATE_BUTTON_CUSTOM_MENU( BottomController controller, int m_scene, bool showMenu, GenericMenu menu = null )
        {   return SHOW_CATEGORY_MENU( controller, m_scene, ( scene ) => { return controller.GetCategoryIndex( scene ); }, false, showMenu, _menu: menu );
        }
        
        internal void AddFavCategory( List<Int32ListArray> capture_list, int VAR_CAT_INDEX, int scene, BottomController controller )
        {   SHOW_STRING( "New Category Name", capture_list[VAR_CAT_INDEX].name, ( value ) =>
            {   if ( string.IsNullOrEmpty( value ) ) return;
                adapter.bottomInterface.GET_BOOKMARKS( ref capture_list, scene );
                if ( capture_list.Any( b => b.name == value ) ) return;
                
                adapter.CreateUndoActiveDescription( "New Category", scene );
                
                var result = new Int32ListArray()
                {   name = value,
                        array = new List<Int32List>(),
                        
                };
                result.FavParams = 0;
                result.SET_COLOR( capture_list[VAR_CAT_INDEX].GET_COLOR() ?? adapter.bottomInterface.RowsParams[0].BgColorValue );
                capture_list.Add( result );
                adapter.SetDirtyActiveDescription( scene );
                
                controller.SetCategoryIndex( capture_list.Count - 1, scene );
                
                adapter.bottomInterface.RefreshMemCache( scene );
                
                controller.REPAINT( adapter );
                
            }, controller );
        }
        internal GenericMenu SHOW_CATEGORY_MENU( BottomController controller, int scene, Func<int, int> CAT_INDEX, bool disableSwither, bool showMenu = true,
                GenericMenu _menu = null )           //  Debug.Log(curentIndex);
        {   var menu = _menu ?? new GenericMenu();
            var VAR_CAT_INDEX = CAT_INDEX(scene);
            
            
            adapter.bottomInterface.GET_BOOKMARKS( ref list, scene );
            var capture_list = list;
            
            
            menu.AddItem( new GUIContent( "Open in New Tab" ), false, () =>
            {   _6__BottomWindow_BottomInterfaceWindow.ShowW( adapter, _6__BottomWindow_BottomInterfaceWindow.TYPE.CUSTOM, list[controller.GetCategoryIndex( scene )].name );
            } );
            
            
            if ( !Application.isPlaying )
            {   menu.AddItem( new GUIContent( "Background Colors" ), false, () =>
                {
                
                    adapter.GUI_ONESHOTPUSH( () =>     // var pos = InputData.WidnwoRect(controller.IS_MAIN, Event.current.mousePosition, 190, 68, controller.adapter);
                    {   var pos = new MousePos( Event.current.mousePosition, MousePos.Type.ColorChanger_230_0, controller.IS_MAIN, controller.adapter);
                        _W__BottomWindow_ColorCategories.Init( pos, controller.adapter, scene );
                    } );
                    
                    /* ** */
                    /* ** */
                    /* ** */
                    /* ** */
                    /* ** */
                } );
            }
            
            
            /* if (disableSwither) menu.AddDisabledItem( new GUIContent( "Show Descriptions" ) );
             else*/
            //   if (disableSwither)
            menu.AddItem( new GUIContent( "Show Descriptions" ), controller.IS_MAIN && adapter.par.BottomParams.SHOW_ALL_DESCRIPTIONS_INHIER || !controller.IS_MAIN
                          && adapter.par.BottomParams.SHOW_ALL_DESCRIPTIONS_INWIN, () =>
            {   DO_BUTTON_DESCRIPTION( controller, scene );
            } );
            menu.AddSeparator( "" );
            
            // if ( adapter.par.SHOW_BOOKMARKS_ROWS && !adapter.par.BOTTOM_AUTOHIDE )
            {   if ( !disableSwither )
                {   menu.AddItem( new GUIContent( "1) 'Default'" ), VAR_CAT_INDEX == 0, () => { controller.SetCategoryIndex( 0, scene ); } );
                    for ( int INDEX = 1 ; INDEX < capture_list.Count ; INDEX++ )
                    {   var capptureI = INDEX;
                        menu.AddItem( new GUIContent( (INDEX + 1) + ") '" + capture_list[INDEX].name + "'" ), VAR_CAT_INDEX == INDEX, () => { controller.SetCategoryIndex( capptureI, scene ); } );
                    }
                    if ( !Application.isPlaying )
                    {   menu.AddItem( new GUIContent( "+ Add Category" ), false, () =>
                        {
                        
                            adapter.OneFrameActionOnUpdateAC += () =>
                            {   AddFavCategory( capture_list, VAR_CAT_INDEX, scene, controller );
                            };
                            
                            adapter.OneFrameActionOnUpdate = true;
                            
                        } );
                    }
                    if ( capture_list.Count > 1 ) menu.AddSeparator( "" );
                }
                
            }
            
            
            if ( VAR_CAT_INDEX == 0 || Application.isPlaying ) menu.AddDisabledItem( new GUIContent( "Rename '" + capture_list[VAR_CAT_INDEX].name + "'" ) );
            else menu.AddItem( new GUIContent( "Rename '" + capture_list[VAR_CAT_INDEX].name + "'" ), false, () =>
            {   if ( VAR_CAT_INDEX == 0 ) return;
            
                SHOW_STRING( "Rename", capture_list[VAR_CAT_INDEX].name, ( value ) =>
                {
                
                    if ( string.IsNullOrEmpty( value ) ) return;
                    adapter.bottomInterface.GET_BOOKMARKS( ref capture_list, scene );
                    if ( capture_list.Any( b => b.name == value ) ) return;
                    
                    
                    var oldValue = capture_list[CAT_INDEX(scene)].name;
                    var newValue = value;
                    
                    foreach ( var item in Resources.FindObjectsOfTypeAll<_6__BottomWindow_BottomInterfaceWindow1>() )
                    {   var cat = ((_6__BottomWindow_BottomInterfaceWindow.BottomControllerWindow)  item.current_controller).GetCurerentCategoryName();
                        if ( cat == oldValue )
                        {   Undo.RecordObject( item, "Rename" );
                            ((_6__BottomWindow_BottomInterfaceWindow.BottomControllerWindow)item.current_controller).SetCurerentCategoryName( newValue );
                            EditorUtility.SetDirty( item );
                        }
                    }
                    
                    adapter.CreateUndoActiveDescription( "Rename", scene );
                    
                    capture_list[VAR_CAT_INDEX].name = value;
                    adapter.MarkSceneDirty( scene );
                }, controller );
            } );
            
            if ( !Application.isPlaying )
            {   if ( VAR_CAT_INDEX == 0 || Application.isPlaying ) menu.AddDisabledItem( new GUIContent( "Remove '" + capture_list[VAR_CAT_INDEX].name + "'Category" ) );
                else menu.AddItem( new GUIContent( "Remove '" + capture_list[VAR_CAT_INDEX].name + "' Category" ), false, () =>
                {
                
                    if (
                        EditorUtility.DisplayDialog( "Remove Category?", "Are you sure?", "Yes", "Cancel" ) )
                    {   if ( VAR_CAT_INDEX == 0 ) return;
                        if ( VAR_CAT_INDEX >= capture_list.Count ) return;
                        
                        adapter.CreateUndoActiveDescription( "Remove Category", scene );
                        
                        
                        var oldValue = capture_list[CAT_INDEX(scene)].name;
                        foreach ( var item in Resources.FindObjectsOfTypeAll<_6__BottomWindow_BottomInterfaceWindow1>() )
                        {   var cat = ((_6__BottomWindow_BottomInterfaceWindow.BottomControllerWindow)  item.current_controller).GetCurerentCategoryName();
                            if ( cat == oldValue )
                            {   item.Close();
                            }
                        }
                        
                        capture_list.RemoveAt( VAR_CAT_INDEX );
                        adapter.SetDirtyActiveDescription( scene );
                        
                        adapter.bottomInterface.RefreshMemCache( scene );
                        
                        controller.REPAINT( adapter );
                        InternalEditorUtility.RepaintAllViews();
                    }
                    
                    
                    
                } );
            }
            
            if ( showMenu )
                menu.ShowAsContext();
            return menu;
        }
        
        
        
        
        
        
        
        
        // GEGEGENENENERIRIRICICIC MEEEENNNUUU
        // GEGEGENENENERIRIRICICIC MEEEENNNUUU
        // GEGEGENENENERIRIRICICIC MEEEENNNUUU
        
        
        
        
        
        
        internal void SET_BOOK_REF(ref GenericMenu menu)
        {
        
        
        
        
            //    CREATE_BUTTON_CUSTOM_MENU( FavoritControllers, -1, false, menu );
            
            
            
        }
        
        
        
        void SET_BOOK( object value )
        {
        
            GenericMenu menu = new GenericMenu();
            
            
            
            ADD_TO_MENU_LIST_OF_OBJECTS( MemType.Custom, ref menu );
            
            menu.AddSeparator( "" );
            
            menu.AddItem( GetContent( adapter.par.SHOW_BOOKMARKS_ROWS, "Bookmarks" ), false, () =>
                          //    menu.AddItem(new GUIContent("Enable Bookmarks Botom GUI"), adapter.par.SHOW_BOOKMARKS_ROWS, () =>
            {   adapter.par.SHOW_BOOKMARKS_ROWS = !(bool)value;
                adapter.SavePrefs();
            } );
            
            menu.AddSeparator( "" );
            
            CREATE_BUTTON_CUSTOM_MENU( HierarchyController, LastActiveScene.GetHashCode(), false, menu );
            
            
            
            
            
            
            menu.ShowAsContext();
        }
        internal void SET_BOOK_2( BottomController controller, int scene )
        {   GenericMenu menu = new GenericMenu();
            ADD_TO_MENU_LIST_OF_OBJECTS( MemType.Custom, ref menu );
            
            menu.AddSeparator( "" );
            
            menu.AddItem( GetContent( adapter.par.SHOW_BOOKMARKS_ROWS, "Bookmarks" ), false, () =>
            {   adapter.par.SHOW_BOOKMARKS_ROWS = !(bool)adapter.par.SHOW_BOOKMARKS_ROWS;
                adapter.SavePrefs();
            } );
            
            menu.AddSeparator( "" );
            
            CREATE_BUTTON_CUSTOM_MENU( controller, scene, false, menu );
            menu.ShowAsContext();
        }
        void SET_LAST( object value )
        {   GenericMenu menu = new GenericMenu();
        
        
            menu.AddItem( GetContent( adapter.par.SHOW_LAST_ROWS, "Last Selections" ), false, () =>
                          //   menu.AddItem(new GUIContent("Enable Last Botom GUI"), adapter.par.SHOW_LAST_ROWS, () =>
            {   adapter.par.SHOW_LAST_ROWS = !(bool)value;
                adapter.SavePrefs();
            } );
            
            menu.AddSeparator( "" );
            
            menu.AddItem( new GUIContent( "Open in New Tab" ), false, () =>
            {   _6__BottomWindow_BottomInterfaceWindow.ShowW( adapter, _6__BottomWindow_BottomInterfaceWindow.TYPE.LAST, "Last Selection" );
            } );
            menu.AddSeparator( "" );
            
            
            ADD_TO_MENU_LIST_OF_OBJECTS( MemType.Last, ref menu );
            
            
            menu.ShowAsContext();
        }
        void SET_HIER( object value )
        {   GenericMenu menu = new GenericMenu();
        
            menu.AddItem( GetContent( adapter.par.SHOW_HIERARCHYSLOTS_ROWS, "Expanded Items" ), false, () =>
                          //  menu.AddItem(new GUIContent("Enable HIerarchy Botom GUI"), adapter.par.SHOW_HIERARCHYSLOTS_ROWS, () =>
            {   adapter.par.SHOW_HIERARCHYSLOTS_ROWS = !(bool)value;
                adapter.SavePrefs();
            } );
            
            
            menu.AddSeparator( "" );
            
            
            
            ADD_TO_MENU_LIST_OF_OBJECTS( MemType.Hier, ref menu );
            
            menu.AddSeparator( "" );
            
            menu.AddItem( new GUIContent( "Collapse All" ), false, () =>
            {   SET_EXPAND_NULL();
            } );
            menu.AddSeparator( "" );
            
            menu.AddItem( new GUIContent( "+ Create New State" ), false, () =>
            {   DoHier_Plus( null, HierarchyController );
            } );
            
            menu.ShowAsContext();
        }
        void SET_SCEN( object value )
        {   GenericMenu menu = new GenericMenu();
        
        
            menu.AddItem( GetContent( adapter.par.SHOW_SCENES_ROWS, "Scenes" ), false, () =>
                          //  menu.AddItem(new GUIContent("Enable Scenes Botom GUI"), adapter.par.SHOW_SCENES_ROWS, () =>
            {   adapter.par.SHOW_SCENES_ROWS = !(bool)value;
                adapter.SavePrefs();
            } );
            
            menu.AddSeparator( "" );
            
            
            ADD_TO_MENU_LIST_OF_OBJECTS( MemType.Scenes, ref menu );
            
            
            menu.AddSeparator( "" );
            
            menu.AddItem( new GUIContent( "+ Add All Opened Scenes" ), false, () =>
            {   DoScenes_Plus( null );
            } );
            
            
            menu.ShowAsContext();
        }
        static List<Int32ListArray> __list;
        struct EstimItems
        {   public GUIContent content;
            public bool active;
            public GenericMenu.MenuFunction onClick;
        }
        void ADD_TO_MENU_LIST_OF_OBJECTS( MemType type, ref GenericMenu menu )
        {
        
            bool was = false;
            if ( type == MemType.Custom )
                adapter.bottomInterface.GET_BOOKMARKS( ref __list, LastActiveScene.GetHashCode() );
            var INV = type == MemType.Last;
            for ( int __c = 0 ; __c < (type == MemType.Custom ? m_memCache[type].Length : 1) ; __c++ )
            {   var memoryRoot = m_memCache[type][__c];
                var interator = 0;
                
                var itemscount = memoryRoot.Count(t => t.IsValid());
                
                if ( itemscount <= 0 )
                {   if ( type == MemType.Custom )
                    {   menu.AddDisabledItem( new GUIContent( "Category - " + __list[__c].name + "/" + "No Items" ) );
                    }
                    continue;
                }
                var rowClass = type == MemType.Custom ? GetRowClass(type).MaxItems : int.MaxValue;
                if ( itemscount > rowClass ) itemscount = rowClass;
                var _scene = LastActiveScene;
                
                List< EstimItems> result = new List<EstimItems>();
                for ( int __i = 0 ; __i < memoryRoot.Count && interator < itemscount ; __i++ )
                {   var i = __i;
                    if ( !memoryRoot[i].IsValid() )
                        continue;
                        
                    var h = type == MemType.Last || type == MemType.Custom ? INT32__ACTIVE_TOHIERARCHYOBJECT(memoryRoot[i].InstanceID) : null;
                    
                    if ( (type == MemType.Last || type == MemType.Custom) && (h == null || !h.Validate()) ) continue;
                    ++interator;
                    var content = memoryRoot[i].ToString().Replace('/', '\\');
                    var count = adapter.bottomInterface.INT32_TOOBJECTASLISTCT( memoryRoot[i].InstanceID ).Length;
                    if ( count > 1 ) content = "[" + count + "]   " + content;
                    if ( type == MemType.Custom ) content = "Category - " + __list[__c].name + "/" + content;
                    
                    //                         if ( i == 0 ) {
                    //
                    //                             Debug.Log( memoryRoot[i].IsSelectedHadrScan() );
                    //                             Debug.Log( memoryRoot[i].InstanceID.list.Count );
                    //                             Debug.Log( adapter.IsSelected( memoryRoot[i].InstanceID.list[0].GetInstanceID() ) );
                    //                             Debug.Log( adapter.selMax );
                    //                         }
                    
                    result.Add( new EstimItems()
                    {   content = new GUIContent( content ),
                        active = memoryRoot[i].IsSelectedHadrScan(),
                        onClick = () =>
                        {   memoryRoot[i].OnClick( false, _scene.GetHashCode() );
                        }
                    } );
                }
                if ( INV ) result.Reverse();
                foreach ( var item in result )
                {   menu.AddItem( item.content, item.active, item.onClick );
                    was = true;
                }
                
            }
            
            if ( !was ) menu.AddDisabledItem( new GUIContent( "No Items" ) );
            
            
        }
        
    }
}
}
