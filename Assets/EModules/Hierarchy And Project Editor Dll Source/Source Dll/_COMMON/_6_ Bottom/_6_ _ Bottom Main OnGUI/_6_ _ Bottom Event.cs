
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
    bool BrakeBottom
    {   get
        {   return false;
            //return Mathf.RoundToInt( par.LINE_HEIGHT ) != EditorGUIUtility.singleLineHeight && firstFrame < 4;
        }
    }
    
    internal sealed partial class BottomInterface {
    
        Vector2 LastTotalSize;
        
        
        internal void BottomEventGUI( Rect selectRect, Adapter.HierarchyObject _o, EditorWindow w )
        {
        
        
        
            //    var treeView = adapter.m_TreeView(w);
            //  if (Selection.activeGameObject) MonoBehaviour.print(GetMemorySize(Selection.activeGameObject.GetComponent<SpriteRenderer>().sprite.texture));
            
            /*  if (!string.IsNullOrEmpty(Event.current.commandName)) {
                  MonoBehaviour.print(Event.current.commandName + " " + (EditorGUIUtility.systemCopyBuffer as GameObject[]));
                //  MonoBehaviour.print(Event.current.commandName + " " + Selection.activeGameObject.name + " " + Event.current.type);
              }*/
            
            
            
            
            var commandName = Event.current.commandName;
            if ( commandName == "ObjectSelectorUpdated" &&
                    EditorGUIUtility.GetObjectPickerControlID() == pickerId ) { }
            if ( commandName == "ObjectSelectorClosed" && EditorGUIUtility.GetObjectPickerControlID() == pickerId )
            {   pickerId = -1;
                pickerAction( EditorGUIUtility.GetObjectPickerObject() );
            }
            /*     List<int> intList = new List<int>();
                 foreach (var hierarchyWindow in hierarchy_windows) {
                     var tree = m_TreeView.GetValue(hierarchyWindow, null);
                     // var m_KeyboardControlIDField = m_KeyboardControlIDField.GetField(ID_STRING, (BindingFlags)(-1));
                     //  intList.Add((int)m_KeyboardControlIDField.GetValue(tree));
                     intList.Add((int)((EditorWindow)hierarchyWindow).position.height);
                 }*/
            
            
            // MonoBehaviour.print( GUIUtility.keyboardControl + " " + intList.Select(l => l.ToString()).Aggregate((a, b) => a + " " + b));
            
            // MonoBehaviour.print(GUIUtility.keyboardControl + " " + window().position.height);
            
            // MonoBehaviour.print(Event.current.type + " "  + Event.current.mousePosition);
            
            
            
            // if (!(bool)m_Animating.GetValue(treeView, null))
            
            //#TODO 2019 OLD HEIGHT_FIX PLACE
            /*  if ( IMGUI() )       // if (ii) {
              {
            
                  var ts = ContentSize;
                  if ( ts.y != LastTotalSize.y || adapter.PLAYMODECHANGE )
                  {   LastTotalSize = ts;
                      lastInst = null;
            
            
                     // if (adapter.PLAYMODECHANGE)adapter.HEIGHT_FUNCTIUON( w, treeView );
                     // else
                     // {
                     // }
                     //  adapter.INIT_IF_NEDDED();
                     //  BOTTOM_UPDATE_POSITION( (EditorWindow)w );
                  }
                  else
                  {   HEIGHT_RIX_FUNCTIUON( w, treeView );
                  }
            
            
            
                  //   if (PLAYMODECHANGE && Event.current.type == EventType.repaint)
                  //   {
                  //       PLAYMODECHANGE = false;
                  //   }
              }
              else
              {   HEIGHT_RIX_FUNCTIUON( w, treeView );
              }*/
            
            adapter.HEIGHT_RIX_FUNCTIUON( adapter.window(), null );
            if ( !cacheInit ) RefreshMemCache( LastActiveScene.GetHashCode() );
            
            if ( !adapter.ENABLE_BOTTOMDOCK_PROPERTY && HEIGHT == 0  /*|| BrakeBottom*/) return;
            
            
            //  SetNavigationRect(treeView, selectRect.x + selectRect.width);
            //if (Event.current.type == EventType.scrollWheel) MonoBehaviour.print(w.position.height);
            // MonoBehaviour.print(Event.current.type);
            // if (Event.current.type == EventType.repaint) MonoBehaviour.print(rect.y);
            
            
            /* if ( Event.current.type != EventType.Repaint )
                 NEW_BOTTOM( selectRect, _o, w );*/
            if ( Event.current.type != EventType.Repaint )
                OLD_EVENTS( selectRect, w );
        }
        
        
        internal  Rect? lastBottomRectUI, lastBottomRectSelectLine;
        
        
        
        
        void NEW_BOTTOM( Rect selectRect, Adapter.HierarchyObject _o, EditorWindow w )
        {
        
            var moduleRect = GetNavigatorRect( /*treeView,*/ selectRect.x + selectRect.width);
            // var navRect =  navRect;// GetNavigatorRect( /*treeView,*/ selectRect.x + selectRect.width);
            HierarchyController.ModuleRect = moduleRect;
            
            var lineRect = GetLineRect(moduleRect);
            var foldRect = GetFoldOutRect(ref lineRect);
            
            if ( !moduleRect.Contains( Event.current.mousePosition) && (Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseUp) ) return;
            
            
            // FIX LEFT COLUMN
            if ( Event.current.type == EventType.Repaint )
            {   if ( UNITY_CURRENT_VERSION >= UNITY_2019_VERSION && Event.current.type == EventType.Repaint )
                {   if ( EditorGUIUtility.isProSkin )
                    {   Adapter.DrawRect( new Rect( 0, selectRect.y + selectRect.height, adapter.TOTAL_LEFT_PADDING_FORBOTTOM, mTotalRectGet2.height ), leftFixColorPro );
                    }
                    else
                    {   Adapter.DrawRect( new Rect( 0, selectRect.y + selectRect.height, adapter.TOTAL_LEFT_PADDING_FORBOTTOM, mTotalRectGet2.height ), leftFixColorPersonal );
                    }
                }
            }
            
            
            /*  var line = GetLineRect(rect);
              var foldRect = GetFoldOutRect(ref line);*/
            if ( moduleRect.Contains( Event.current.mousePosition ))
                if ( Event.current.type == EventType.DragUpdated || Event.current.type == EventType.DragPerform ||
                        Event.current.type == EventType.DragExited ) EventUse();
                        
                        
                        
                        
            //BG
            if ( Event.current.type == EventType.Repaint )
            {   //FadeRect
                Adapter.DrawRect( moduleRect, Adapter.EditorBGColor );
                //FadeRect
                if ( Adapter.UNITY_CURRENT_VERSION < Adapter.UNITY_2019_3_0_VERSION )
                    Adapter.GET_SKIN().window.Draw( moduleRect, /*new GUIContent("Navigator"),*/ false, false, false, false );
                else
                    Adapter.GET_SKIN().textArea.Draw( moduleRect, /*new GUIContent("Navigator"),*/ false, false, false, false );
            }
            
            //FOLD
            DRAW_FOLD_ICONS( ref foldRect, LastActiveScene.GetHashCode() );
            if ( moduleRect.Contains( Event.current.mousePosition ))
                if ( Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseUp )
                {   if ( anycatsenable )
                    {   if ( foldRect.Contains( Event.current.mousePosition ) )
                        {   HierarchyController.selection_window = w;
                            HierarchyController.selection_button = 0;
                            HierarchyController.selection_action = ( mouseUp, deltaTIme ) =>
                            {   if ( mouseUp && foldRect.Contains( Event.current.mousePosition ) )
                                {   adapter.par.BOTTOM_AUTOHIDE = !adapter.par.BOTTOM_AUTOHIDE;
                                    adapter.SavePrefs();
                                }
                                return Event.current.delta.x == 0 && Event.current.delta.x == 0;
                                
                            };
                        }
                    }
                }
                
            //LINES
            /* if ( adapter.BOT_DRAW_STACK.START_DRAW_PARTLY_TRYDRAW( _o ) )
             {   if ( Event.current.type == EventType.Repaint ) adapter.BOT_DRAW_STACK.START_DRAW_PARTLY_CREATEINSTANCE( _o );
                 DoLines( lineRect, w );
                 adapter.BOT_DRAW_STACK.END_DRAW( _o );
             }*/
            DoLines( lineRect, w );
            
            //EVENTS
            if ( moduleRect.Contains( Event.current.mousePosition ))
                if ( Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseUp )
                {   GUIUtility.hotControl = 0;
                    EventUse();
                    adapter.RepaintWindow( true );
                    
                }
                
                
                
                
                
                
                
                
        }
        
        Rect frr;
        private void DoLines( Rect foldOutRect, EditorWindow win )
        {   if ( adapter.DISABLE_DESCRIPTION( (LastActiveScene) ) ) return;
        
            var line = foldOutRect;
            
            line.height = GRAPH_HEIGHT();
            if ( ENABLE_HYPERGUI() ) hyperGraph.DRAW( line, hyperGraph.HierHyperController, win );
            if ( ENABLE_FAVORGUI() ) favorGraph.DRAW( line, favorGraph.HierFavorController, win );
            line.y += line.height;
            
            HierarchyController.REFERENCE_WINDOW = adapter.window();
            
            if ( HEIGHT <= ___REFERENCE_HEIGHT_AUTOHIDE() ) return;
            
            SORT_DRAW_ROWS();
            
            for ( int __index = 0 ; __index < DRAW_INDEX.Length ; __index++ )
            {   var i = DRAW_INDEX[__index];
            
                if ( !RowsParams[i].Enable ) continue;
                
                line.height = RowsParams[i].FULL_HEIGHT;
                DRAW_BY_INDEX( RowsParams[i], line, HierarchyController, LastActiveScene.GetHashCode() );
                
                if ( RowsParams[i].PluginID == PLUGIN_ID.BOOKMARKS ) HierarchyController.CustomLineRect = line;
                
                line.y += line.height;
            }
        }
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        void OLD_EVENTS(Rect selectRect, EditorWindow w)
        {
        
        
            var rect = GetNavigatorRect( /*treeView,*/ selectRect.x + selectRect.width);
            HierarchyController.ModuleRect = rect;
            
            
            if ( rect.Contains( Event.current.mousePosition ) )
            {   if ( Event.current.type == EventType.MouseDrag || Event.current.type == EventType.DragUpdated ||
                        Event.current.type == EventType.DragPerform || Event.current.type == EventType.DragExited || Event.current.type == EventType.Layout )
                {   var line = GetLineRect(rect);
                    GetFoldOutRect( ref line );
                    DoLines( line, w );
                }
                
                
                
                if ( Event.current.type == EventType.DragUpdated || Event.current.type == EventType.DragPerform ||
                        Event.current.type == EventType.DragExited ) EventUse();
                        
                        
                        
                if ( Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseUp )
                {   var line = GetLineRect(rect);
                    var foldRect = GetFoldOutRect(ref line);
                    
                    
                    
                    DRAW_FOLD_ICONS( ref foldRect, LastActiveScene.GetHashCode() );
                    
                    if ( anycatsenable )
                    {   if ( foldRect.Contains( Event.current.mousePosition ) )
                        {   HierarchyController.selection_window = w;
                            HierarchyController.selection_button = 0;
                            HierarchyController.selection_action = ( mouseUp, deltaTIme ) =>
                            {   if ( mouseUp && foldRect.Contains( Event.current.mousePosition ) )
                                {   adapter.par.BOTTOM_AUTOHIDE = !adapter.par.BOTTOM_AUTOHIDE;
                                    adapter.SavePrefs();
                                }
                                return Event.current.delta.x == 0 && Event.current.delta.x == 0;
                                
                            };
                        }
                        
                    }
                    
                    
                    DoLines( line, w );
                    
                    
                    GUIUtility.hotControl = 0;
                    EventUse();
                    adapter.RepaintWindow( true );
                }
            }
            
            
            
        }
        
        void OLD_PAINT(Rect selectRect, EditorWindow w)
        {   if ( UNITY_CURRENT_VERSION >= UNITY_2019_VERSION && Event.current.type == EventType.Repaint )     // FIX LEFT COLUMN
            {   if ( EditorGUIUtility.isProSkin )
                {   Adapter.DrawRect( new Rect( 0, selectRect.y + selectRect.height, adapter.TOTAL_LEFT_PADDING_FORBOTTOM, mTotalRectGet2.height ), leftFixColorPro );
                }
                else
                {   Adapter.DrawRect( new Rect( 0, selectRect.y + selectRect.height, adapter.TOTAL_LEFT_PADDING_FORBOTTOM, mTotalRectGet2.height ), leftFixColorPersonal );
                }
            }
            
            
            
            
            var navRect = GetNavigatorRect( /*treeView,*/ selectRect.x + selectRect.width);
            var lineRect = GetLineRect(navRect);
            var foldRect = GetFoldOutRect(ref lineRect);
            
            lastBottomRectUI = navRect;
            lastBottomRectSelectLine = selectRect;
            
            //FadeRect
            var defColor = GUI.color;
            var c = Adapter.EditorBGColor;
            c.a = 1;
            GUI.color *= c;
            GUI.DrawTexture( navRect, EditorGUIUtility.whiteTexture );
            GUI.color = defColor;
            //FadeRect
            
            
            if ( Adapter.UNITY_CURRENT_VERSION < Adapter.UNITY_2019_3_0_VERSION )
                Adapter.GET_SKIN().window.Draw( navRect, /*new GUIContent("Navigator"),*/ false, false, false, false );
            else
                Adapter.GET_SKIN().textArea.Draw( navRect, /*new GUIContent("Navigator"),*/ false, false, false, false );
                
                
            DRAW_FOLD_ICONS( ref foldRect, LastActiveScene.GetHashCode() );
            
            
            // if ( !adapter.BOT_DRAW_STACK.START_DRAW_PARTLY_TRYDRAW( o ) ) return;
            // if ( Event.current.type == EventType.Repaint ) adapter.BOT_DRAW_STACK.START_DRAW_PARTLY_CREATEINSTANCE( o );
            DoLines( lineRect, w );
            //adapter.BOT_DRAW_STACK.END_DRAW( o );
        }
    }
}
}
