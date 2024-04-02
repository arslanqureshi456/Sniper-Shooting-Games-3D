
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

    internal sealed partial class BottomInterface {
    
        bool? isProSkin;
        int fixScrolCounterSHouldMoreThan2 = 0;
        public void EventGUI( HierarchyObject o, Rect selectRect )
        {   /* if (applicationIsPlaying != Application.isPlaying)
                 {
                            // MonoBehaviour.print("ASDF");
                     applicationIsPlaying = Application.isPlaying;
                     PLAY_MODE_CHANGE_ACTION();
                    /* EditorGUIUtility.ExitGUI();
                     return;#1#
                 }*/
            // if (Application.isPlaying) MonoBehaviour.print(Application.isPlaying);
            // bool ii = false;
            
            /*  selectRect.x += adapter.TOTAL_LEFT_PADDING;
              selectRect.width += adapter. PREFAB_BUTTON_SIZE - adapter.TOTAL_LEFT_PADDING;*/
            selectRect.width += adapter.PREFAB_BUTTON_SIZE;
            bool needSet = false;
            
            
            if ( !NEED_READ_LIST.ContainsKey( GUIUtility.keyboardControl ) )
            {   NEED_READ_LIST.Add( GUIUtility.keyboardControl, true );
                Keys.Add( GUIUtility.keyboardControl );
                needSet = true;
            }
            
            
            
            
            if ( NEED_READ_LIST[GUIUtility.keyboardControl] )     // ii = true;
            {   if ( adapter.hierarchy_windows.Count == 0 )
                {   adapter.InitWindList();
                
                    needSet = true;
                }
                if ( adapter.hierarchy_windows.Count != 0 )
                {   NEED_READ_LIST[GUIUtility.keyboardControl] = false;
                }
            }
            
            
            var w = adapter.window();
            if ( w == null || adapter.m_TreeView( w ) == null )     // ChechEvents(selectRect);
            {
            
                adapter.PLAYMODECHANGE = false;
                return;
            }
            
            if ( !adapter.WasEvent.ContainsKey( GUIUtility.keyboardControl ) )     //  ChechEvents(selectRect);
            {   adapter.PLAYMODECHANGE = false;
                return;
            }
            
            
            
            if ( !isProSkin.HasValue ) isProSkin = EditorGUIUtility.isProSkin;
            if ( isProSkin  != EditorGUIUtility.isProSkin )
            {   isProSkin = EditorGUIUtility.isProSkin;
                adapter.ResetStyles();
            }
            
            
            if ( adapter.WasEvent[GUIUtility.keyboardControl] == null ) adapter.WasEvent[GUIUtility.keyboardControl] = selectRect.y;
            if ( adapter.WasEvent[GUIUtility.keyboardControl].Value != selectRect.y ) return;
            
            
            ONE_FRAME_EVENT( w );
            
            // BOTTOM_UPDATE_POSITION( adapter.window() );
            
            if ( !adapter.wasModulesInitialize ) adapter.MOI.InitModules();
            
            //  if ( Event.current.type == EventType.Repaint ) adapter.HierWinScrollPos = adapter.GetHierarchyWindowScrollPos();
            
            //  hierarchy_windows = ((IList)field.GetValue(null));
            
            adapter.ChechEvents( selectRect );
            
            
            //MonoBehaviour.print( Event.current.type );
            //var treeView2 = adapter.m_TreeView.GetValue( adapter.window() );
            //MonoBehaviour.print( (Rect)adapter.m_VisibleRect.GetValue( treeView2 ) );
            
            
            if ( (adapter.PLAYMODECHANGE || adapter.oldScroll.HasValue)
                    //    && adapter.IMGUI()
                    //#TODO Removed IMGUI
               )     // if (!Application.isPlaying)
            {
                {   //adapter.RepaintWindowInUpdate();
                    // MonoBehaviour.print( "STOP" );
                    //  if ( !adapter.IMGUI20183() )
                    adapter.  SWITCHER_RELOAD_DATA( adapter );
                    
                    
                }
            }
            
            if ( (adapter.PLAYMODECHANGE || adapter.oldScroll.HasValue) )
            {
            
            
            
                //  if (!adapter.IMGUI20183())
                {   if ( adapter.IMGUI() )
                    {   if ( adapter.oldScroll.HasValue )
                        {
                        
                            var treeView = adapter.m_TreeView( adapter.window() );
                            var state = adapter.m_state.GetValue( treeView, null );
                            
                            adapter.HEIGHT_RIX_FUNCTIUON( adapter.window(), treeView, true );
                            BOTTOM_UPDATE_POSITION( adapter.window()  );
                            
                            if ( adapter.NEW_SCROLL_BEHAVIOUR )
                            {   adapter.scrollPosField.SetValue( state, adapter.ScrollMemory );
                                fixScrolCounterSHouldMoreThan2 = 0;
                                //   Debug.Log("SET " + adapter.ScrollMemory);
                            }
                            else
                            {   adapter.scrollPosField.SetValue( state, adapter.oldScroll.Value );
                            }
                            adapter.RepaintWindow();
                        }
                    }
                    
                    
                    
                }
                if ( Event.current.type == EventType.Repaint )
                    if ( adapter.oldScroll != null )
                    {   adapter.oldScroll = null;
                    }
                /* if (Event.current.type == EventType.Repaint)
                {   if (adapter.oldScroll != null) adapter.oldScroll = null;
                    else  adapter.PLAYMODECHANGE = false;
                }*/
                
                
            }
            else if ( adapter.IMGUI() )
            {   if ( fixScrolCounterSHouldMoreThan2 < 10 ) fixScrolCounterSHouldMoreThan2++;
                if ( fixScrolCounterSHouldMoreThan2 < 3 )
                {   var treeView = adapter.m_TreeView( adapter.window() );
                    var state = adapter.m_state.GetValue( treeView, null );
                    adapter.scrollPosField.SetValue( state, adapter.ScrollMemory );
                }
                if ( adapter.NEW_SCROLL_BEHAVIOUR && adapter.oldScroll == null && fixScrolCounterSHouldMoreThan2 > 2 )
                {   var tree = adapter.m_TreeView( adapter.window() );
                    if ( tree != null )
                    {   var state = adapter.m_state.GetValue( tree, null );
                        adapter.ScrollMemory = (Vector2)adapter.scrollPosField.GetValue( state );
                    }
                    
                    // Debug.Log("ScrollMemory = " + adapter.ScrollMemory);
                }
            }
            
            
            
            
            
            
            
            
            if ( adapter.PLAYMODECHANGE )
            {   adapter.PLAYMODECHANGE = false;
                // adapter.SendEventAll( new Event() { type = EventType.Layout } );
                
            }
            //  InternalEditorUtility.RepaintAllViews();
            
            //  MonoBehaviour.print(M_Descript.des(EditorSceneManager.GetActiveScene()).GetHash1().Count);
            
            try
            {   if ( adapter.hierarchy_windows.Count == 0 || adapter.DISABLE_DES()
                        /*DISABLE_DESCRIPTION(o)*/ /*|| !object.ReferenceEquals(w, hierarchy_windows[0])*/) return;
                        
                var treeView = adapter.m_TreeView(w);
                if ( treeView != null )
                {   if ( needSet && adapter.m_UseExpansionAnimation != null )
                    {   adapter.m_UseExpansionAnimation.SetValue( treeView, false );
                    }//else
                    
                    
                    if ( !adapter.PLAYMODECHANGE && !adapter.oldScroll.HasValue && adapter.IMGUI() )     /* OLD RESETER */
                    {   if ( UseExpAnim( treeView ) )
                        {   if ( !adapter.IMGUI20183() )
                            {   adapter.PLAYMODECHANGE = true;
                                adapter.oldScroll = Vector2.zero;
                                adapter.INIT_IF_NEDDED();
                            }
                            if ( adapter.m_UseExpansionAnimation != null )
                                adapter.m_UseExpansionAnimation.SetValue( treeView, false );
                        }
                    }
                }
                
                
                BottomEventGUI( selectRect, o, w );
                
                
                /*
                if (!adapter.ENABLE_HOVER_ITEMS && adapter.pluginID == Initializator.HIERARCHY_ID && adapter.hoveredItem != null)
                    // var treeView = m_TreeView(ENABLE_HOVER_ITEMS_WIN);
                {   // var treeView = m_TreeView(window());
                    adapter. hoveredItem.SetValue(treeView, null, null);
                }*/
                
            }
            catch ( Exception ex )
            {   adapter.logProxy.LogError( "EventGUI " + ex.Message + " " + ex.StackTrace );
                return;
            }
            
            
            
            
            
        }
        
        
        bool UseExpAnim( object treeView )
        {   return   //false;
                adapter.m_UseExpansionAnimation != null && (bool)adapter.m_UseExpansionAnimation.GetValue( treeView );
        }
        
        float?[] ocs = new float?[100] ;
        void ONE_FRAME_EVENT( EditorWindow w )
        {   if ( Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape )
            {   /* adapter.  CLOSE_PREFAB_MODE();
                     Adapter.EventUseFast();*/
            }
            
            if ( adapter.firstFrame != 5 ) adapter
                .firstFrame++;
                
                
            if (
                // Event.current.type != EventType.Repaint )
                !Event.current.isMouse && Event.current.type != EventType.DragUpdated &&
                Event.current.type != EventType.DragPerform &&
                Event.current.type != EventType.DragExited &&
                Event.current.type != EventType.MouseDrag )
            {   //  var ind = (int)Event.current.type;
                var ind = 0;
                
                if ( ind < ocs.Length && ind >= 0 )
                {   var treeView = adapter.m_TreeView(adapter.window());
                    var  ___ContentSize = (Vector2)adapter.GetTotalSizeMethodInfo.Invoke( treeView, null );
                    if ( ___ContentSize.y != (ocs[ind] ?? (ocs[ind] = ___ContentSize.y).Value) )
                    {   // adapter.lastmPos = ((Rect)adapter.m_Pos.GetValue( w )).height;
                        adapter.HeightFixIfDrag();
                        //  adapter.INIT_HEIGHT_FIX
                        // adapter.InternalClearDrag();
                        ocs[ind] = ___ContentSize.y;
                        //   ocs[(int)Event.current.type] = ___ContentSize.y;
                    }
                }
                
                
                
            }
            /* if (!adapter.IMGUI() &&
                     ( Event.current.type != EventType.Layout &&
                       Event.current.type != EventType.Repaint &&
                       Event.current.type != EventType.DragUpdated &&
                       Event.current.type != EventType.DragPerform &&
                       Event.current.type != EventType.DragExited &&
                       Event.current.type != EventType.MouseDrag &&
                       Event.current.button != 1 //|| FixIMGUISelectionChangeMarker
                     )
                )
             {   adapter. HeightFixIfDrag();
             }*/
            
            //FixIMGUISelectionChangeMarker = false;
            
            if ( _W___IWindow.__inputWindow.Count( k => k.Value ) > 0 )
            {   DISABLE_HOVER();
            }
            
            if ( GetNavigatorRect( /*treeView,*/ 10000 ).y - HEIGHT < Event.current.mousePosition.y )
                DISABLE_HOVER();
                
                
            if ( adapter.hashoveredItem && w )
            {   var tree = adapter.m_TreeView(w);
                var h = tree == null ? null : adapter.hoveredItem.GetValue(tree, null) as UnityEditor.IMGUI.Controls.TreeViewItem;
                //hoveredItem.SetValue(tree, null, null);
                if ( h != null ) adapter.hoverID = lastID = h.id;
                else adapter.hoverID = -1;
                if ( tree == null ) adapter.hoverID = lastID;
                
            }
            else
            {   adapter.hoverID = -1;
            }
            if ( lastLastID  != adapter.hoverID )
            {   adapter.resetHoverStack1 = lastLastID;
                adapter.resetHoverStack2 = adapter.hoverID;
                lastLastID = adapter.hoverID;
            }
        }
        int lastID = -1;
        int lastLastID = -1;
        
        void DISABLE_HOVER()
        {   if ( adapter.hashoveredItem && adapter.window() )
            {   var tree = adapter.m_TreeView(adapter.window());
                if ( tree != null )
                    adapter.hoveredItem.SetValue( tree, null, null );
                //hoveredItem.SetValue(tree, null, null);
            }
        }
        
    }
    
    internal static void FocusToInspector()
    {   if ( !EditorWindow.focusedWindow.titleContent.text.ToLower().Contains( "inspector" ) )
        {   var fi = Adapter.ALL_WINDOWS.FirstOrDefault(w => w.titleContent.text.ToLower().Contains("inspector"));
            if ( !fi )
                fi = Resources.FindObjectsOfTypeAll<EditorWindow>().FirstOrDefault( w => w.titleContent.text.ToLower().Contains( "inspector" ) );
            if ( fi ) fi.Focus();
        }
    }
}
}





// EditorApplication.
//EditorWindow
//adapter.window().
// adapter.window().Repaint();
//adapter.RepaintWindowInUpdate();

//	MonoBehaviour.print( "DISABLE" );



//bool keep = false;

//MonoBehaviour.print( "ASD" );
//if ( PLAYMODECHANGE )
/*{

var treeView = adapter.m_TreeView.GetValue( adapter.window() );
keep = true;
var r1 = (Rect)adapter.SceneHierarchyWindow.GetProperty( "treeViewRect" , (BindingFlags)(-1) ).GetValue( adapter.window() , null );
var r2 = (Rect)adapter.m_VisibleRect.GetValue( treeView );
r2.height = r1.height;
adapter.m_VisibleRect.SetValue( treeView , r2 );


//adapter.m_data.GetType().GetMethod( "OnInitialize" , (BindingFlags)(-1) ).Invoke( adapter.m_data.GetValue( treeView , null ) , null );
//adapter.m_gui.GetType().GetMethod( "OnInitialize" , (BindingFlags)(-1) ).Invoke( adapter.m_gui.GetValue( treeView , null ) , null );


adapter.window().Repaint();
}*/



/*  EditorGUIUtility.ExitGUI();
  BOTTOM_UPDATE_POSITION(window());
  var mm = typeof(EditorWindow).GetField("m_Parent", (BindingFlags)(-1));
  mm.FieldType.GetMethod("RepaintImmediately", (BindingFlags)(-1)).Invoke(mm.GetValue(window()), null);
  //typeof(EditorWindow).GetMethod("RepaintImmediately", (BindingFlags)(-1)).Invoke(window(), null);
  window().Repaint();
  return;*/
//
//InternalEditorUtility.RepaintAllViews();
/*GUI_ONESHOTAC += () => {
    InternalEditorUtility.RepaintAllViews();
};
GUI_ONESHOT = true;*/
