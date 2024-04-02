#if UNITY_EDITOR
    #define HIERARCHY
    #define PROJECT
#endif


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
internal partial class Adapter {

    public struct pathcomparerclass : IComparer<pathcomparerclass>, IComparable<pathcomparerclass>
    {   public pathcomparerclass( string p )
        {   this.p = p;
        
        
            var ls =  p.LastIndexOf('/');
            
            
            var ld =  p.LastIndexOf('.');
            if ( ls != -1 && ld != -1 && ld > ls && !AssetDatabase.IsValidFolder( p ) )     //name = p.Substring(ls + 1);
            {   // isFolder = false;
            
                comparePath = p.Remove( ls ).Replace( "/", "//" ) + p.Substring( ls );
            }
            else
            {   comparePath = p.Replace( "/", "//" );
            }
            
            /* comparePath = p.Replace("/", "//");
            }
            else
            {   if (ls != -1) comparePath = p.Remove(ls) + '0' + p.Substring(ls);
                else comparePath = "/" + p;
            }*/
            
            // isFolder = false;
            // compareName = p;
        }
        //  string name;
        string comparePath;
        //  int depth ;
        //  string path;
        // bool isFolder;
        // string compareName;
        public string p;
        public int Compare( pathcomparerclass x, pathcomparerclass y )      //return x.p.CompareTo(y.p);
        {   return x.comparePath.CompareTo( y.comparePath );
        
            /* if (x.depth == y.depth && x.path == y.path)
             {   if (x.isFolder || y.isFolder)
                 {   if (x.isFolder == y.isFolder) return x.p.CompareTo(y.p);
            
                     if (x.isFolder) return -1;
                     if (y.isFolder) return 1;
                 }
             }
             return x.p.CompareTo(y.p);*/
            
            /*var v1 = string.IsNullOrEmpty(name);
            var v2 = string.IsNullOrEmpty(y.name);
            if (v1 != v2 ) return v1 ? -1 : 1;
            return x.p.CompareTo(y.p);*/
            
            /* var v1 = x.p.StartsWith(y.p);
             var v2 =  !v1 ? y.p.StartsWith(x.p) : false;
             if (v1) return 1;
             if (v2) return -1;
             return x.p.CompareTo(y.p);*/
        }
        public int CompareTo( pathcomparerclass other )
        {   return Compare( this, other );
        }
    }
    static string[] _getAls;
    internal static string[] ALL_ASSETS_PATHS
    {   get
        {   var r = _getAls ?? (_getAls = AssetDatabase.GetAllAssetPaths().Select(p => new pathcomparerclass(p)).OrderBy(p => p).Select(p => p.p).ToArray());
            /* Debug.Log(" --- ");
             for (int i = 0; i < 15; i++)
             {   Debug.Log(r[i]);
             }*/
            return r;
        }
        set
        {   _getAls = null;
        }
    }
    
    internal partial class BottomInterface {
    
        internal partial class FavorGraph : Adapter.BottomInterface.BOTTOM_GRAPH {
            internal FavorControllerWindow WindowFavorController;
            internal FavorControllerHierarchy HierFavorController;
            // TempColorClass tempColor = new TempColorClass();
            internal FavorGraph( Adapter adapter, BottomInterface btInterface )
            {   this.adapter = adapter;
                this.bottomInterface = btInterface;
                
                WindowFavorController = new FavorControllerWindow( adapter );
                HierFavorController = new FavorControllerHierarchy( adapter ) { IS_MAIN = true };
                INIT_FAVORITE();
            }
            
            internal void DOCK_FAVOR()
            {   _6__BottomWindow_FavoritWindow.ShowW( adapter );
            }
            
            internal override void DRAG_PERFORMER_SCAN()
            {
            
            }
            internal Rect CAPTURE_CLIP_RECT;
            
            internal override void EXTERNAL_HYPER_DRAWER( Rect lineRect, UniversalGraphController controller, EditorWindow win)
            {   INIT_STYLES( adapter );
            
                controller.tempWin = win;
                
                dragRect = lineRect;
                dragRect.height = 5;
                
                DRAG( controller );
                
                lineRect.y += dragRect.height;
                lineRect.height -= dragRect.height;
                // if (Event.current.type != EventType.Repaint) INTERFACE( HierHyperController );
                
                if ( controller.MAIN ) CAPTURE_CLIP_RECT = lineRect;
                
                localRect = lineRect;
                localRect.x = 0;
                localRect.y = 0;
                GUI.BeginClip( lineRect );
                
                
                
                if ( Event.current.type != EventType.Repaint ) INTERFACE( localRect, adapter.parLINE_HEIGHT, controller );
                FAVORIT_GUI( localRect, (FavorUniversalController)controller );
                if ( Event.current.type == EventType.Repaint ) INTERFACE( localRect, adapter.parLINE_HEIGHT, controller );
                
                
                GUI.EndClip();
                
                
                
                
                //          SETUPROOT.ExampleDragDropGUI( lineRect, null, FAVOR_DRAG_VALIDATOR, FAVOR_DRAG_PERFORMER );
                
                // if (Event.current.type == EventType.Repaint) INTERFACE( HierHyperController );
                
                
                //base.EXTERNAL_HYPER_DRAWER( lineRect, HierHyperController );
            }
            
            
            
            
            
            
            
            
            Rect localRect;
            
            
            
            void INTERFACE( Rect RECT, float LH, BottomInterface.UniversalGraphController controller )
            {
            
            
                var ACTIVE_RECT = RECT;
                /* ACTIVE_RECT.height -= INTERFACE_SIZE;
                 ACTIVE_RECT.y += INTERFACE_SIZE;*/
                
                /** CLOASE **/
                var closeRect = ACTIVE_RECT;
                closeRect.width = 14;
                closeRect.height = 14;
                closeRect.x = RECT.width - closeRect.width;
                closeRect.y += (LH - 14) / 2;
                EditorGUIUtility.AddCursorRect( closeRect, MouseCursor.Link );
                
                GUI.Label( closeRect, HyperGraphClose_Content, style );
                
                var EVENT_ID = idOffset - 10;
                if ( Event.current.type == EventType.Repaint )
                {   Adapter.DrawTexture( closeRect, HIPERUI_CLOSE );
                }
                if ( closeRect.Contains( Event.current.mousePosition ) && Event.current.button == 0 &&
                        Event.current.type == EventType.MouseDown )
                {   EventUse();
                    ADD_ACTION( EVENT_ID, closeRect, contains => { return false; }, contains =>
                    {   if ( contains )
                        {   if ( !editorWindow ) SWITCH_ACTIVE( false );
                            else if ( WindowFavorController.REFERENCE_WINDOW ) WindowFavorController.REFERENCE_WINDOW.Close();
                        }
                    }, controller );
                }
                if ( HOVER( EVENT_ID, closeRect, controller ) )
                {   HIPERUI_BUTTONGLOW.Draw( closeRect, false, false, false, false );
                }
                /** CLOASE **/
                
                if ( !controller.hide_hierarchy_ui_buttons )
                {
                
                    /** DOCK **/
                    if ( !editorWindow )
                    {   closeRect.x -= closeRect.width - 1;
                        EditorGUIUtility.AddCursorRect( closeRect, MouseCursor.Link );
                        
                        
                        GUI.Label( closeRect, HyperGraphWindow_Content, style );
                        
                        EVENT_ID = idOffset - 20;
                        if ( Event.current.type == EventType.Repaint )
                        {   Adapter.DrawTexture( closeRect, HIPERGRAPH_DOCK );
                        }
                        if ( closeRect.Contains( Event.current.mousePosition ) && Event.current.button == 0 &&
                                Event.current.type == EventType.MouseDown )
                        {   EventUse();
                            ADD_ACTION( EVENT_ID, closeRect, contains => { return false; }, contains =>
                            {   if ( contains ) DOCK_FAVOR();
                            }, controller );
                        }
                        if ( HOVER( EVENT_ID, closeRect, controller ) )
                        {   HIPERUI_BUTTONGLOW.Draw( closeRect, false, false, false, false );
                        }
                    }
                    /** DOCK **/
                }
                
                
                
                /** COLOR **/
                /*  closeRect.x -= closeRect.width - 1;
                  EditorGUIUtility.AddCursorRect( closeRect, MouseCursor.Link );
                
                
                  GUI.Label( closeRect, HyperGraphWindow_Content );
                
                  EVENT_ID = idOffset - 30;
                  if (Event.current.type == EventType.Repaint) {
                    GUI.DrawTexture( closeRect, adapter.GetIcon( "HIPERUI_COLOR" ) );
                  }
                  if (closeRect.Contains( Event.current.mousePosition ) && Event.current.button == 0 &&
                    Event.current.type == EventType.MouseDown) {
                    EventUse();
                    var pos = InputData.WidnwoRect(Event.current.mousePosition, 190, 68, adapter);
                    ADD_ACTION( EVENT_ID, closeRect, contains => { return false; }, contains => {
                      COLOR_CHANGER.Init( pos, adapter, adapter.bottomInterface.MemCacheScene );
                    }, controller );
                  }
                  if (HOVER( EVENT_ID, closeRect, controller )) {
                    HIPERUI_BUTTONGLOW.Draw( closeRect, false, false, false, false );
                  }*/
                /** COLOR **/
                
            }
            
            
            
            
            internal override void SWITCH_ACTIVE_SCAN( bool? overrideActive )
            {   if ( bottomInterface._FAV_HEIGHT == null )
                    bottomInterface._FAV_HEIGHT = adapter.FAV_ENABLE() ? adapter.par.FavoritesNavigatorParams.HEIGHT : 0;
                adapter.CHECK_SMOOTH_HEIGHT();
                
                adapter.par.FavoritesNavigatorParams.ENABLE = overrideActive ?? !adapter.par.FavoritesNavigatorParams.ENABLE;
            }
            
            
            
            bool INIT_FAVORITE_b = false;
            
            const int idOffset = 50000;
            // Texture icon;
            // Color? iconColor;
            Rect  close_rect;
            
            void INIT_FAVORITE()
            {   if ( INIT_FAVORITE_b ) return;
                INIT_FAVORITE_b = true;
                
                adapter.OnScroll -= ON_SCROLL;
                adapter.OnScroll += ON_SCROLL;
                
                /* Selection.selectionChanged -= asd;
                 Selection.selectionChanged += asd;*/
            }
            
            /*  private void asd()
              {
                Debug.Log( "ASD" + Selection.objects.Length );
              }*/
            
            
            
            void SWAP( FavorUniversalController controller, int scene, int arrayIndex, int next )
            {   List<Int32List> l1 = controller.GetCategoryIndex(scene) == 0 ? adapter.MOI.des(scene).GetHash4() : adapter.MOI.des(scene).GetBookMarks()[controller.GetCategoryIndex(scene)].array;
            
                // var l1 = adapter.MOI.des(s).GetHash4();
                var b = l1[arrayIndex];
                l1.RemoveAt( arrayIndex );
                if ( next >= l1.Count ) l1.Add( b );
                else l1.Insert( next, b );
                
                adapter.SetDirtyActiveDescription( scene );
            }
            
            struct MouseEventStruct
            {
            
                internal Rect LocalModuleRect;
                internal Rect WorldModuleRect;
                internal MousePos MousePosStruct;
                internal FavorUniversalController controller;
                internal int itemIndex;
                internal int categoryIndex;
                internal Adapter.BottomInterface.MemoryRoot memoryRoot;
                internal Adapter.HierarchyObject hierarchy_obect;
                internal int scene;
                internal int selectionState;
                internal Int32List otherstring;
                internal object otherobject;
                
                public MouseEventStruct Clone()
                {   var c = (MouseEventStruct)MemberwiseClone();
                    c.otherstring = otherstring;
                    c.otherobject = otherobject;
                    c.hierarchy_obect = hierarchy_obect;
                    c.memoryRoot = memoryRoot;
                    c.controller = controller;
                    return c;
                }
                
                internal MouseEventStruct Set_Get( Adapter.BottomInterface.MemoryRoot memoryRoot,
                                                   Adapter.HierarchyObject hierarchy_obect,
                                                   int scene, int categoryIndex, int itemIndex, FavorUniversalController controller, int selectionState )
                {   this.selectionState = selectionState;
                    this.memoryRoot = memoryRoot;
                    this.hierarchy_obect = hierarchy_obect;
                    this.scene = scene;
                    this.categoryIndex = categoryIndex;
                    this.itemIndex = itemIndex;
                    this.controller = controller;
                    this.controller.breaking = false;
                    
                    return this;
                }
            }
            
            MouseEventStruct mouseEventStruct;
            void ITEM_ON_DOWN( MouseEventStruct currentStruct )
            {   if ( currentStruct.controller.currentAction == null ) return;
                if ( currentStruct.controller.breaking ) return;
                if ( currentStruct.memoryRoot != null ) currentStruct.memoryRoot.OnClick( false, (currentStruct.scene) );
                else
                {   Selection.objects = new[] { currentStruct.hierarchy_obect.GetHardLoadObject() };
                    if ( currentStruct.selectionState == 3 ) bottomInterface.adapter.SAVE_SCROLL();
                    
                }
            }
            void ITEM_ON_DRAG( MouseEventStruct currentStruct )
            {   if ( currentStruct.controller.breaking ) return;
                var controller = currentStruct.controller;
                
                var m = GUIUtility.GUIToScreenPoint(Event.current.mousePosition + Event.current.delta);
                
                
                var drag = !currentStruct.WorldModuleRect.Contains( m );
                /*  drag |= m.x < 3;
                  drag |= m.x > controller.WIDTH - 9;*/
                
                
                if ( drag && Event.current.type == EventType.MouseDrag && !Event.current.control && !Event.current.shift && !Event.current.alt )
                {   /*  List<Int32List> targetList = null;
                
                        targetList = controller.GetCategoryIndex( scene ) == 0 ? adapter.MOI.des( scene ).GetHash4() : adapter.MOI.des( scene ).GetBookMarks()[controller.GetCategoryIndex( scene )].array;
                         Utilities.MoveFromTo( ref targetList, arrayIndex, backupArraIndex );
                         RefreshMemCache( scene );*/
                    
                    if ( currentStruct.memoryRoot != null )
                    {   var result = adapter.bottomInterface.INT32_TOOBJECTASLISTCT( currentStruct.memoryRoot.InstanceID ).Where( h => h ).ToArray();
                    
                        adapter.bottomInterface.SetDragData( result, MemType.Custom );
                        DragAndDrop.StartDrag( "Dragging Assets" );
                        // DragAndDrop.objectReferences =
                        //Debug.Log( result[0] );
                        DragAndDrop.SetGenericData( "FavI1", currentStruct.categoryIndex );
                        DragAndDrop.SetGenericData( "FavI2", currentStruct.itemIndex );
                        DragAndDrop.SetGenericData( "FavI3", currentStruct.memoryRoot.ArrayIndex );
                    }
                    else       // DragAndDrop.PrepareStartDrag();// reset data
                    {   adapter.InternalClearDrag();
                        DragAndDrop.objectReferences = new[] { currentStruct.hierarchy_obect.GetHardLoadObject() };
                        DragAndDrop.StartDrag( "Dragging Assets" );
                    }
                    DragAndDrop.SetGenericData( "FavI4", currentStruct.hierarchy_obect.id );
                    DragAndDrop.SetGenericData( adapter.pluginname, null );
                    
                    
                    EventUse();
                    controller.__ModuleRect = currentStruct.LocalModuleRect;
                    
                    currentStruct.controller.breaking = true;
                    adapter.bottomInterface.ClearAction();
                    controller.REPAINT( adapter );
                }
            }
            
            
            void MENUCALL( MouseEventStruct currentStruct )
            {   var menu = new GenericMenu();
            
                adapter.bottomInterface.SHOW_CATEGORY_MENU( currentStruct.controller, (currentStruct.scene), ( s ) => currentStruct.categoryIndex, true, _menu: menu );
                
                // adapter.bottomInterface.SET_BOOK_REF( ref menu );
                menu.AddSeparator( "" );
                if ( currentStruct.memoryRoot == null ) menu.AddDisabledItem( new GUIContent( "Remove" ) );
                else
                {   menu.AddItem( new GUIContent( "Remove"), false, () =>
                    {   REMOVE_ON_UP( currentStruct );
                    } );
                }
                menu.ShowAsContext();
            }
            void DESCRIPTION_ON_UP( MouseEventStruct currentStruct )
            {   if ( currentStruct.hierarchy_obect == null || !currentStruct.hierarchy_obect.Validate() ) return;
                /* if (!currentStruct.memoryRoot.IsValid() || currentStruct.memoryRoot.InstanceID == null
                                           || !adapter.bottomInterface.INT32_ISVALID( currentStruct.memoryRoot.InstanceID )
                                           ) return;*/
                // currentStruct.scene
                //var d = adapter.MOI.des(adapter.bottomInterface.INT32_SCENE(currentStruct.memoryRoot.InstanceID));
                var d = adapter.MOI.des(currentStruct.hierarchy_obect.scene);
                if ( d == null ) return;
                //  adapter.MOI.CREATE_NEW_ESCRIPTION( adapter.bottomInterface.INT32__ACTIVE_TOHIERARCHYOBJECT( currentStruct.memoryRoot.InstanceID ), d, true );
                adapter.DescriptionModule.CREATE_NEW_ESCRIPTION( adapter, mouseEventStruct.MousePosStruct, currentStruct.hierarchy_obect, currentStruct.hierarchy_obect.scene, false );
                
            }
            void DESCRIPTION_ON_UP_RIGHT( MouseEventStruct currentStruct )
            {
            
                var fillter = currentStruct.memoryRoot != null ? GET_DESCRIPTION( currentStruct.memoryRoot ).text : GET_DESCRIPTION( currentStruct.hierarchy_obect ).text ;
                if ( string.IsNullOrEmpty( fillter ) ) return;
                
                
                // var pos = new MousePos( Event.current.mousePosition, MousePos.Type.ColorChanger_230_0, controller.IS_MAIN, adapter);
                currentStruct.MousePosStruct.type = MousePos.Type.Search_356_0;
                
                _W__SearchWindow.Init( currentStruct.MousePosStruct, adapter.DescriptionModule.SearchHelper, fillter,
                                       adapter.DescriptionModule.CallHeaderFiltered( fillter ),
                                       adapter.DescriptionModule, adapter, currentStruct.hierarchy_obect );
            }
            void CATEGORY_TITLE1( MouseEventStruct
                                  currentStruct )     // adapter.bottomInterface.SHOW_CATEGORY_MENU( currentStruct.controller, Adapter.GET_SCENE_BY_ID( currentStruct.scene ), (s) => currentStruct.categoryIndex, true );
            {
            }
            void CATEGORY_TITLE2( MouseEventStruct currentStruct )
            {   adapter.bottomInterface.SHOW_CATEGORY_MENU( currentStruct.controller, (currentStruct.scene), ( s ) => currentStruct.categoryIndex, true );
            }
            void FAVORIT_FOLDERS_ICON( MouseEventStruct currentStruct )
            {   adapter.bottomInterface.GET_BOOKMARKS( ref refBookmarks, (currentStruct.scene) );
                adapter.CreateUndoActiveDescription( "Change favorites params", (currentStruct.scene) );
                refBookmarks[currentStruct.categoryIndex].FavParams = !(1 == refBookmarks[currentStruct.categoryIndex].FavParams) ? 1 : 0;
                SET_DIRTY( (currentStruct.scene) );
            }
            
            
            /* scanneddata FILTER_ON_DOWN__scanneddata;
             int FILTER_ON_DOWN__index;
             IStringSetter FILTER_ON_DOWN__m_favString;
             int FILTER_ON_DOWN__m_favString_index;
             int FILTER_ON_DOWN__m_scene;*/
            // Rect FILTER_ON_DOWN__ic;
            
            void FILTER_ON_DOWN( MouseEventStruct obj )
            {   var   FILTER_ON_DOWN__scanneddata = GET_PATHS( obj.hierarchy_obect.project.assetPath );
                var         FILTER_ON_DOWN__index = obj.itemIndex;
                var        FILTER_ON_DOWN__m_favString = obj.otherstring ;
                var         FILTER_ON_DOWN__m_favString_index = obj.categoryIndex;
                var          FILTER_ON_DOWN__m_scene = obj.scene;
                //FILTER_ON_DOWN__ic = ic;
                
                
                var menu = new GenericMenu();
                var ext = FILTER_ON_DOWN__scanneddata.extensions;
                
                
                
                
                
                var list  = ext.Select( d => new { k = d.Key, v = d.Value } ).OrderBy( d => d.v ).Select( d => d.k ).ToArray();
                if ( list.Length == 0 ) ArrayUtility.Add( ref list, "*.*" );
                else ArrayUtility.Insert( ref list, 0, "*.*" );
                var index = FILTER_ON_DOWN__index + 1;
                
                //   Debug.Log(FILTER_ON_DOWN__m_favString);
                for ( int i = 0 ; i < list.Length ; i++ )
                {   var newI = i;
                    menu.AddItem( new GUIContent( list[i] ), index == i, () =>
                    {   adapter.CreateUndoActiveDescription( "Change filter", (FILTER_ON_DOWN__m_scene) );
                        if ( newI == 0 ) FILTER_ON_DOWN__m_favString.SetString( FILTER_ON_DOWN__m_favString_index, null, adapter.pluginname );
                        else FILTER_ON_DOWN__m_favString.SetString( FILTER_ON_DOWN__m_favString_index, list[newI], adapter.pluginname );
                        // Debug.Log(FILTER_ON_DOWN__m_favString);
                        //Debug.Log( (FILTER_ON_DOWN__m_favString is Int32List) + " " + FILTER_ON_DOWN__m_favString.GetString( FILTER_ON_DOWN__m_favString_index, adapter.pluginname ) );
                        SET_DIRTY( (FILTER_ON_DOWN__m_scene) );
                    } );
                }
                
                menu.ShowAsContext();
                /* var newI =  EditorGUI.Popup( FILTER_ON_DOWN__ic, FILTER_ON_DOWN__index, list );
                 Debug.Log( newI );
                 if (newI != FILTER_ON_DOWN__index) {
                
                 }*/
            }
            
            
            void LIST_ON_DOWN( MouseEventStruct currentStruct )
            {
            
            
                adapter.bottomInterface.GET_BOOKMARKS( ref refBookmarks, (currentStruct.scene) );
                adapter.CreateUndoActiveDescription( "Change favorites params", (currentStruct.scene) );
                refBookmarks[currentStruct.categoryIndex].array[currentStruct.itemIndex].FavParams = 1 - refBookmarks[currentStruct.categoryIndex].array[currentStruct.itemIndex].FavParams;
                SET_DIRTY( (currentStruct.scene) );
            }
            
            
            
            
            void REMOVE_ON_UP( MouseEventStruct currentStruct )
            {   adapter.bottomInterface.RemoveAndRefresh( Adapter.BottomInterface.MemType.Custom, currentStruct.itemIndex, currentStruct.categoryIndex, (currentStruct.scene) );
            }
            
            
            GUIStyle _mGET_DRAG_BOX;
            GUIStyle GET_DRAG_BOX()
            {   if ( _mGET_DRAG_BOX == null )
                {   _mGET_DRAG_BOX = new GUIStyle();
                    _mGET_DRAG_BOX.normal.background = adapter.GetIcon( "DRAG_BOX" );
                    _mGET_DRAG_BOX.border = new RectOffset( 7, 7, 7, 7 );
                }
                return _mGET_DRAG_BOX;
            }
            
            GUIStyle _mGET_DRAG_LINE;
            GUIStyle GET_DRAG_LINE()
            {   if ( _mGET_DRAG_LINE == null )
                {   _mGET_DRAG_LINE = new GUIStyle();
                    _mGET_DRAG_LINE.normal.background = adapter.GetIcon( "DRAG_LINE" );
                    _mGET_DRAG_LINE.border = new RectOffset( 6, 0, 6, 0 );
                }
                return _mGET_DRAG_LINE;
            }
            
            
            void SET_DIRTY( int scene )
            {   var sc = scene;
                var d = adapter.MOI.des(sc);
                if ( d == null ) return;
                adapter.SetDirtyDescription( d, sc );
            }
            FavorUniversalController currentContoller;
            int CONTROL_ID;
            Vector2 scrollPos;
            
            
            class EmtyString : IStringSetter {
                public string GetString( int index, string adapter )
                {   return null;
                }
                
                public void SetString( int index, string value, string adapter )
                {
                }
            }
            // EmtyString emptyString = new EmtyString();
            
            
            
            List<Int32ListArray> refBookmarks2;
            
            void CREATE_NEW_CAT( MouseEventStruct currentStruct )
            {   adapter.bottomInterface.GET_BOOKMARKS( ref refBookmarks2, currentStruct.scene );
                var VAR_CAT_INDEX =  currentStruct.controller.GetCategoryIndex(currentStruct.scene);
                adapter.bottomInterface.AddFavCategory( refBookmarks2, VAR_CAT_INDEX, currentStruct.scene, currentStruct.controller );
            }
            
            Rect Shrink( Rect rect, int i )
            {   rect.x += i;
                rect.y += i;
                rect.width -= i * 2;
                rect.height -= i * 2;
                return rect;
            }
            
            
            
            internal void ClearHeight()
            {   HEIGHT[CONTROLLER] = null;
                adapter.RepaintWindow(true);
            }
            
            
            scanneddata GET_PATHS( string folder )
            {   if ( !adapter.scanned_folder.ContainsKey( folder ) )     //Debug.Log( folder );
                {   //var L = (UNITY_SYSTEM_PATH + folder).Length;
                    // var paths = Directory.GetFiles( UNITY_SYSTEM_PATH + folder, "*.*", SearchOption.AllDirectories ).Where( f => !f.EndsWith( ".meta" ) ).Select(f => f.Substring(L).Replace('\\', '/')).ToList();
                    if ( folder.EndsWith( "/" ) ) throw new Exception( "/" );
                    
                    var stw = string.IsNullOrEmpty(folder) ? "Assets" : folder;
                    stw += '/';
                    var L = stw.Length - 1;
                    var paths = ALL_ASSETS_PATHS.Where(p => p.StartsWith(stw)).Select(p => p.Substring(L))/*.Select(p => p.Replace('\\', '/'))*/.ToArray();
                    stw = stw.Trim( '/' );
                    
                    
                    var list = paths.Select(p => new {p, fld = p.Trim('/').Split( '/' ).ToArray() }).Where(p => p.fld.Length != 0).Select( p => new scanneditem(
                        adapter: adapter,
                        name: p.p.Substring( p.p.LastIndexOf( '/' ) + 1),
                        rootpath: stw,
                        fullPath: stw + p.p,
                        folders: p.fld ) ).ToList();
                        
                        
                    /*foreach (var item in list)
                     {   // var guid = AssetDatabase.AssetPathToGUID( stw + item.rootPath );
                         // Debug.Log(item.rootPath + "\n" + item.fullPath + "\n" + item.extension + "\n" + item.folders[0]);
                         Debug.Log( "-" + item.fullPath + "\n" + item.name);
                     }*/
                    // var extensions = paths.Select(p => p.LastIndexOf('.') == -1 ? "" : p.Substring(p.LastIndexOf('.') + 1).ToLower()).GroupBy(p => p).Select((g, i) => new {k = g.Key, i } ).ToDictionary(k => k.k,
                    var extensions = list
                                     //.Select(p => p.name.LastIndexOf('.') == -1 ? "" : p.name.Substring(p.name.LastIndexOf('.') + 1).ToLower())
                                     .Select(p => p.extension)
                                     .GroupBy(p => p).Select((g,
                    i) => new {k = g.Key, i } ).ToDictionary(
                        k => k.k,
                        v => v.i);
                    adapter.scanned_folder.Add( folder, new scanneddata() { scanneditems = list, extensions = extensions } );
                }
                /*   foreach (var f in adapter.scanned_folder[folder].extensions)
                   {   Debug.Log( folder + "\n" + f.Key);
                   }*/
                return adapter.scanned_folder[folder];
            }
            
            
            
            //  string emptystr = null;
            //  Color? temp_bgColor, temp_textColor;
            
            
            
            
            
            
            
            GUIContent content = new GUIContent();
            GUIContent content_des = new GUIContent();
            // Color coloAlpha = new Color(1, 1, 1, 0.6f);
            GUIContent REALEMPTY_CONTENT = new GUIContent();
            
            
            
            
            void DRAW_LABEL( Rect rect, GUIContent content, int fontSize, TempColorClass styleColor, TextAnchor? align = null )
            {   DRAW_LABEL( rect, content, fontSize, styleColor != null && styleColor.HAS_LABEL_COLOR ? styleColor.LABELCOLOR : (Color? )null, align );
            }
            
            void DRAW_LABEL( Rect rect, GUIContent content, int fontSize, Color? styleColor = null, TextAnchor? align = null )         // if (Event.current.type == EventType.Repaint)
            {
                {   var a = style.alignment;
                    var f = style.fontSize;
                    var r = style.richText;
                    style.alignment = align ?? TextAnchor.MiddleLeft;
                    style.fontSize = fontSize;
                    style.richText = true;
                    
                    var cc = GUI.color;
                    if ( styleColor != null ) GUI.color *= styleColor.Value;
                    //GUI.Label( rect, content );
                    // ROUND_RECT( ref rect );
                    //Adapter.GET_SKIN().label.Draw( rect, content, false, false, false, false );
                    GUI.Label( rect, content, style );
                    if ( styleColor != null ) GUI.color = cc;
                    
                    style.alignment = a;
                    style.fontSize = f;
                    style.richText = r;
                }
                
            }
            
            /*   float CALC_LABEL(Rect rect, GUIContent content, int fontSize, TempColorClass styleColor )
               {   return CALC_LABEL(rect,  content,  fontSize, styleColor != null && styleColor.HAS_LABEL_COLOR ? styleColor.LABELCOLOR : (Color? )null);
               }*/
            
            float CALC_LABEL( Rect rect, GUIContent content, int fontSize )       //if (Event.current.type == EventType.Repaint)
            {   var a = style.alignment;
                var f = style.fontSize;
                var r = style.richText;
                style.alignment = TextAnchor.MiddleLeft;
                style.fontSize = fontSize;
                style.richText = true;
                
                //GUI.Label( rect, content );
                // ROUND_RECT( ref rect );
                float w, x;
                style.CalcMinMaxWidth( content, out w, out x );
                //  Adapter.GET_SKIN().label.Draw( rect, content, false, false, false, false );
                
                style.alignment = a;
                style.fontSize = f;
                style.richText = r;
                return Mathf.Min( rect.width, w );
            }
            
            GUIContent GET_DESCRIPTION( Adapter.BottomInterface.MemoryRoot mem )
            {   var o = mem.InstanceID != null && adapter.bottomInterface.INT32_ISVALID(mem.InstanceID) ? adapter.bottomInterface.INT32__ACTIVE_TOHIERARCHYOBJECT(mem.InstanceID) : null;
                if ( o != null /*&& o.scene.IsValid() && o.scene.isLoaded*/ )
                {   var scene = adapter.bottomInterface.INT32_SCENE(mem.InstanceID);
                
                    if ( adapter.DescriptionModule.HasKey( scene.GetHashCode(), o ) )
                    {   var d = adapter.DescriptionModule.GetValue(scene.GetHashCode(), o);
                        content_des.text = content_des.tooltip = d;
                    }
                    else
                    {   content_des.text = "";
                        content_des.tooltip = "No Description\nLeft CLICK to add Description";
                    }
                }
                else
                {   content_des.text = content_des.tooltip = "- ...";
                }
                return content_des;
            }
            GUIContent GET_DESCRIPTION( Adapter.HierarchyObject o )
            {   if ( o != null /*&& o.scene.IsValid() && o.scene.isLoaded*/ )
                {
                
                    if ( adapter.DescriptionModule.HasKey( o.scene, o ) )
                    {   var d = adapter.DescriptionModule.GetValue(o.scene, o);
                        content_des.text = content_des.tooltip = d;
                    }
                    else
                    {   content_des.text = "";
                        content_des.tooltip = "No Description\nLeft CLICK to add Description";
                    }
                }
                else
                {   content_des.text = content_des.tooltip = "- ...";
                }
                return content_des;
            }
            
            
            
            TreeItem GET_TREE_ITEM( string id, int category, UniversalGraphController contoller, bool IS_ROOT )
            {   return GET_TREE_ITEM( id, refBookmarks[category], contoller, IS_ROOT );
            }
            TreeItem GET_TREE_ITEM( string id, Int32ListArray category, UniversalGraphController contoller, bool IS_ROOT )
            {   var C_I = contoller.MAIN ? 0 : 1;
                var tree = contoller.MAIN ? tree_item_hierarchy : tree_item_windows;
                if ( IS_ROOT ) id += "1";
                //  if (IS_ROOT)
                if ( !tree.ContainsKey( id ) )
                {   tree.Add( id, new TreeItem() );
                    /*if (!SKIPEXPAND)*/
                    tree[id].Expand = COLABSE_CACHE.Get( C_I, category, id, IS_ROOT );
                    
                    /*if (colabsed_cache[C_I].ContainsKey( category.InstanceID ))
                    {   tree[id].Expand = !colabsed_cache[C_I][category.InstanceID].ContainsKey( id );
                        if (IS_ROOT && tree[id].Expand) tree[id].Expand = false;
                        // if (IS_ROOT ) tree[id].Expand = !tree[id].Expand;
                    }*/
                }
                
                return tree[id];
            }
            
            Dictionary<string, TreeItem> tree_item_hierarchy = new Dictionary<string, TreeItem>();
            Dictionary<string, TreeItem> tree_item_windows = new Dictionary<string, TreeItem>();
            
            internal class TreeItem {
                internal bool Expand = false;
                /* internal bool Select
                 {
                   get { return BROAD_SELECTED( this ); }
                   set {
                     BROAD_SELECTED( this, value );
                   }
                 }
                 internal void BROAD(Action<TreeItem> ac)
                 {
                   foreach (var sceneData in treeItems)
                   {
                     ac( sceneData.Value );
                     sceneData.Value.BROAD( ac );
                   }
                 }*/
                /* internal void BROAD(Action<SceneData> ac)
                 {
                   foreach (var sceneData in treeItems)
                     sceneData.Value.BROAD( ac );
                   foreach (var sceneData in items)
                     ac( sceneData.Value );
                
                 }*/
                /* void BROAD_SELECTED(TreeItem item, bool value)
                 {
                   foreach (var sceneData in item.items) sceneData.Value.Select = value;
                   foreach (var sceneData in item.treeItems) BROAD_SELECTED( sceneData.Value, value );
                 }
                 bool BROAD_SELECTED(TreeItem item)
                 {
                   foreach (var sceneData in item.items) if (!sceneData.Value.Select) return false;
                   foreach (var sceneData in item.treeItems) return BROAD_SELECTED( sceneData.Value );
                   return true;
                 }
                 internal Dictionary<string , TreeItem> treeItems = new Dictionary<string , TreeItem>();
                 internal Dictionary<string , SceneData> items = new Dictionary<string , SceneData>();*/
            }
            
            
            
            
            /*
            
            
            
              internal TreeData(string[] input)
              {
                foreach (var path in input)
                {
                  var currentCatalog = root;
                  foreach (var seg in path.Split( '/' ))
                  {
                    if (seg.Contains( '.' ))
                    {
                      currentCatalog.items.Add( seg, new SceneData() { name = seg, path = path, Select = !path.StartsWith( "Assets/Plugins" ) && !path.StartsWith( "Assets/Editor" ) } );
                    }
                    else
                    {
                      TreeItem newTree = null;
                      if (currentCatalog.treeItems.ContainsKey( seg )) newTree = currentCatalog.treeItems[seg];
                      else
                      {
                        newTree = new TreeItem();
                        currentCatalog.treeItems.Add( seg, newTree );
                      }
                      currentCatalog = newTree;
                    }
                  }
                }
              }
            }
            
            
            
            void DrawTreeView(TreeData.TreeItem treeData, int deep)
            {
              foreach (var treeItem in treeData.treeItems)
              {
                //GUILayout.BeginHorizontal();
                GUILayout.BeginHorizontal();
            
                var R = EditorGUILayout.GetControlRect( false , GUILayout.Height( H ) , GUILayout.Width( WIDTH * columns[0] ) );
                var drawR = R;
                drawR.width = WIDTH * 2;
                DrawColor( drawR, lineC[line++ % 2] );
                R.x += deep * H;
                R.width -= deep * H;
                drawR = R;
                drawR.width = H;
                drawR.x += drawR.width;
                var newS = EditorGUI.Toggle( drawR , treeItem.Value.Select );
                if (newS != treeItem.Value.Select)
                {
                  treeItem.Value.Select = newS;
                }
                drawR.x += drawR.width;
                drawR.width = R.width - drawR.x;
                GUI.Label( drawR, new GUIContent( treeItem.Key, adapter.GetIcon( "FOLDER" ) ) );
            
                GUILayout.EndHorizontal();
            
            
                //GUILayout.EndHorizontal();
                if (treeItem.Value.Expand) DrawTreeView( treeItem.Value, deep + 1 );
              }
            
            
              foreach (var treeItem in treeData.items)
              {
                //GUILayout.BeginHorizontal();
                GUILayout.BeginHorizontal();
                var R = EditorGUILayout.GetControlRect( false , GUILayout.Height( H ) , GUILayout.Width( WIDTH * columns[0] ) );
                var drawR = R;
                drawR.width = WIDTH * 2;
                DrawColor( drawR, lineC[line++ % 2] );
                DrawColor( drawR, treeItem.Value.Select ? sceneC : sceneC5 );
                R.x += deep * H + H;
                R.width -= deep * H + H;
                drawR = R;
                drawR.width = H;
                treeItem.Value.Select = EditorGUI.Toggle( drawR, treeItem.Value.Select );
                drawR.x += drawR.width;
                drawR.width = R.width - drawR.x;
            
                GUI.Label( drawR, new GUIContent( treeItem.Key, adapter.GetIcon( "SCENE" ) ) );
            
                DRAWSCENE_GUI( treeItem );
                GUILayout.EndHorizontal();
                //GUILayout.EndHorizontal();
              }
            }*/
            
            
            
            
            internal void CHECK_HEIGHT()
            {   adapter.par.FavoritesNavigatorParams.HEIGHT = Mathf.Clamp( adapter.par.FavoritesNavigatorParams.HEIGHT, 20,
                        Math.Max( 20, adapter.window().position.height * 0.5f ) );
            }
            
            
            
            //  const int EVENT_ADD = 1000000;
        }
        
        
        
    }
}
}
