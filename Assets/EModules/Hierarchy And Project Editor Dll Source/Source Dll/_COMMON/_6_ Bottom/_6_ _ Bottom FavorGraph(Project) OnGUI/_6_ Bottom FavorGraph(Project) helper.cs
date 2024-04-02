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


    internal partial class BottomInterface {
    
        /*   static class FAV_PARAMS {
               internal const int SHOW_CONTENT = 1 << 0;
               internal const int TYPIZATED_CONTENT = 1 << 1;
           }*/
        /*internal class FavoritesController_Bottom : Adapter.BottomInterface.HyperController
        {
          static GUIContent c;
        
          internal FavoritesController_Bottom(Adapter adapter) : base( adapter )
          {
          }
        
          internal override bool CheckCategoryIndex(Scene scene)
          {
            throw new NotImplementedException();
          }
        
          internal override int GetCategoryIndex(Scene scene)
          {
            throw new NotImplementedException();
          }
        
          internal override string GetCurerentCategoryName()
          {
            throw new NotImplementedException();
          }
        
          internal override void SetCategoryIndex(int index, Scene scene)
          {
            throw new NotImplementedException();
          }
        }
        
        
        internal void EVENT_UPDATE(FavoritesController_Bottom current_controller)
        {
        
          if (current_controller.selection_action != null)
          {
            current_controller.selection_action( false, adapter.deltaTime );
            current_controller.REPAINT( adapter );
          }
        }
        
        internal void EVENT_MOUSE_UP(FavoritesController_Bottom current_controller)
        {
        
          if (current_controller.selection_action != null)
          {
            //MonoBehaviour.print("ASD");
            current_controller.selection_action( true, adapter.deltaTime );
            adapter.bottomInterface.ClearAction();
        
            GUIUtility.hotControl = 0;
            Adapter.EventUse();
        
            current_controller.REPAINT( adapter );
        
          }
        }
        
        FavoritesController_Bottom current_controller;
        
        public void FavoritesBottomGUI()
        {
          if (current_controller == null)
          {
            adapter.bottomInterface.FavoritControllers.RemoveAll( w => !w.REFERENCE_WINDOW );
            current_controller = new FavoritesController_Bottom( adapter ) { REFERENCE_WINDOW = adapter.window() };
            adapter.bottomInterface.FavoritControllers.Add( current_controller );
          }
        
          if (!current_controller.REFERENCE_WINDOW) current_controller.REFERENCE_WINDOW = adapter.window();
        
          _mFavoritesOnGUI( current_controller );
        }
        
        
        public void _mFavoritesOnGUI(FavoritesController_Bottom current_controller)
        {
        
        
        }
        */
        internal class FavorUniversalController : BottomControllerDefault {
            public FavorUniversalController(Adapter adapter) : base( adapter )
            {
            }
            
            /*internal override bool CheckCategoryIndex(Scene scene) { throw new NotImplementedException(); }
            internal override int GetCategoryIndex(Scene scene) { throw new NotImplementedException(); }
            internal override string GetCurerentCategoryName() { throw new NotImplementedException(); }
            internal override void SetCategoryIndex(int index, Scene scene) { throw new NotImplementedException(); }*/
            internal bool breaking;
            internal Rect __ModuleRect;
            
        }
        
        
        
        internal class FavorControllerHierarchy : FavorUniversalController {
            public FavorControllerHierarchy(Adapter adapter) : base( adapter )
            {
            }
            
            public override bool MAIN
            {   get { return true; }
            }
            
            internal override bool hide_hierarchy_ui_buttons
            {   get { return false; }
            }
            
            internal override float HEIGHT
            {   get { return adapter.par.FavoritesNavigatorParams.HEIGHT; }
                set { adapter.par.FavoritesNavigatorParams.HEIGHT = value; }
            }
            
            internal override float WIDTH
            {   get { return adapter.window() == null ? 0 : adapter.window().position.width; }
                set { }
            }
            
            /*  internal override float DEFAULT_WIDTH(UniversalGraphController controller)
              {
                return controller.MAIN ? adapter.par.HiperGraphParams.SCALE : adapter.par.HiperGraphParams.WINDIOW_SCALE;
                / *get { return adapter.par.FavoritesNavigatorParams.SCALE; }
                set { }* /
              }*/
        } // class FavorControllerHierarchy
        
        
        internal class FavorControllerWindow : FavorUniversalController {
            public FavorControllerWindow(Adapter adapter) : base( adapter )
            {
            }
            
            internal override bool hide_hierarchy_ui_buttons { get { return true; } }
            internal override float HEIGHT
            {   get { return adapter.bottomInterface.favorGraph.editorWindow ? adapter.bottomInterface.favorGraph.editorWindow.position.height : 0; }
                set { }
            }
            internal override float WIDTH
            {   get { return adapter.bottomInterface.favorGraph.editorWindow ? adapter.bottomInterface.favorGraph.editorWindow.position.width : 0; }
                set { }
            }
            /* internal override float DEFAULT_WIDTH(UniversalGraphController controller)
             {
               return / *(par.HiperGraphParams.SCALE - 1) / 5 +* / 1;
             }*/
            
            
        } // class FavorControllerWindow
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        internal partial class FavorGraph : Adapter.BottomInterface.BOTTOM_GRAPH {
        
        
        
        
            void DRAG_DRAG(Rect rect, bool fullAdd, FavorUniversalController controller, int sourcescene, ref List<Int32ListArray> sourcecategoryref, int? sourcecategory_I, int? sourcearrayIndex,
                           int targetscene, ref List<Int32ListArray> targetcategoryref, int targetcategoryref_I, int targetarrayIndex)
            {   if (DragAndDrop.visualMode != DragAndDropVisualMode.Rejected && rect.Contains( Event.current.mousePosition ))
                {   switch (Event.current.type)
                    {   case EventType.DragUpdated:
                            if (FAVOR_DRAG_VALIDATOR()) DragAndDrop.visualMode = DragAndDropVisualMode.Move;
                            else DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
                            
                            Adapter.EventUseFast();
                            break;
                        case EventType.Repaint:
                            if (
                                DragAndDrop.visualMode == DragAndDropVisualMode.None ||
                                DragAndDrop.visualMode == DragAndDropVisualMode.Rejected) break;
                                
                            if (FAVOR_DRAG_VALIDATOR())
                            {   if (fullAdd) GET_DRAG_BOX().Draw( rect, false, false, false, false );
                                else
                                {   var drawR = rect;
                                    drawR.height = 8;
                                    drawR.y = rect.y - 4;
                                    if (Event.current.mousePosition.y > rect.y + rect.height / 2) drawR.y += rect.height;
                                    GET_DRAG_LINE().Draw( drawR, false, false, false, false );
                                }
                                
                                //if (controller.__ModuleRect)
                                //EditorGUI.DrawRect( rect, Color.grey );
                            }
                            // if (validate()) EditorGUI.DrawRect( dropArea, color ?? Color.grey );
                            break;
                        case EventType.DragPerform:
                            DragAndDrop.AcceptDrag();
                            if (Event.current.mousePosition.y > rect.y + rect.height / 2) targetarrayIndex++;
                            FAVOR_DRAG_PERFORMER( controller, ( sourcescene ), ref sourcecategoryref, sourcecategory_I, sourcearrayIndex, ( targetscene ), ref targetcategoryref,
                                                  targetcategoryref_I, targetarrayIndex );
                            Adapter.EventUseFast();
                            break;
                        case EventType.MouseUp:
                            adapter.InternalClearDrag();
                            // DragAndDrop.PrepareStartDrag();
                            break;
                    }
                }
            }
            
            Int32List t = null;
            //  List<Int32ListArray> FAVOR_DRAG_PERFORMER_LIST = new  List<Int32ListArray>();
            internal void FAVOR_DRAG_PERFORMER(FavorUniversalController controller, int sourcescene, ref List<Int32ListArray> sourcecategoryref, int? sourcecategory_I, int? sourcearrayIndex,
                                               int targetscene, ref List<Int32ListArray> targetcategoryref, int targetcategoryref_I, int targetarrayIndex)
            {   if (FAVOR_DRAG_VALIDATOR())
                {   // List<Int32List> l1 = controller.GetCategoryIndex(scene) == 0 ? adapter.MOI.des(scene).GetHash4() : adapter.MOI.des(scene).GetBookMarks()[controller.GetCategoryIndex(scene)].array;
                    // var l1 = adapter.MOI.des(s).GetHash4();
                    
                    adapter.CreateUndoActiveDescription( "Drag Favorite", sourcescene );
                    adapter.CreateUndoActiveDescription( "Drag Favorite", targetscene );
                    
                    t = null;
                    if (sourcearrayIndex.HasValue)
                    {   //adapter.bottomInterface.GET_BOOKMARKS( ref FAVOR_DRAG_PERFORMER_LIST, sourcescene );
                    
                        // Debug.Log( adapter.MOI.des( sourcescene ).GetHash4().Count );
                        /* Debug.Log( sourcecategoryref[0].array.Count );
                         Debug.Log( sourcecategoryref.Count );*/
                        
                        
                        
                        if (sourcearrayIndex.Value < 0 || sourcearrayIndex.Value >= sourcecategoryref[sourcecategory_I.Value].array.Count) return;
                        t = sourcecategoryref[sourcecategory_I.Value].array[sourcearrayIndex.Value];
                        sourcecategoryref[sourcecategory_I.Value].array.RemoveAt( sourcearrayIndex.Value );
                        adapter.SetDirtyActiveDescription( sourcescene );
                        
                        if (targetarrayIndex > sourcearrayIndex.Value && sourcecategory_I == targetcategoryref_I) targetarrayIndex--;
                        /* Debug.Log( sourcecategoryref[sourcecategory_I.Value].array.Count );
                         Debug.Log( adapter.MOI.des( sourcescene ).GetHash4().Count );*/
                        
                    }
                    else
                    {   var result = GetDragData().Where(o => o).ToArray();
                        /* if (result.Length != 0)
                           adapter.bottomInterface.AddAndRefreshCustom( result, result[0], targetcategory, targetscene );*/
                        if (result.Length != 0) t = adapter.bottomInterface.OBJECTS_TO_LIST32( result, result[0] );
                        /* sourcearrayIndex = targetcategory.Count - 1;
                         sourcecategory = targetcategory;*/
                        //  t = DragAndDrop.objectReferences.Select(o=>adapter.GetHierarchyObjectByInstanceID(o.GetInstanceID())).
                    }
                    
                    if (t != null && t.list != null && t.list.Count == 0)
                    {   // Debug.Log(t .InstanceID);
                        targetcategoryref[targetcategoryref_I].array.RemoveAll(a => a.GUIDsActiveGameObject_CheckAndGet == t .GUIDsActiveGameObject_CheckAndGet);
                        
                        if (targetarrayIndex >= targetcategoryref[targetcategoryref_I].array.Count) targetcategoryref[targetcategoryref_I].array.Add( t );
                        else targetcategoryref[targetcategoryref_I].array.Insert( targetarrayIndex, t );
                        
                        /*  for (int i = 0; i < 2; i++)
                          {   if (!colabsed_cache[i].ContainsKey(t.InstanceID))
                                  colabsed_cache[i].Add(t.InstanceID, new Dictionary<long, bool>());
                              if (!colabsed_cache[i][t.InstanceID].ContainsKey(t.InstanceID))
                                  colabsed_cache[i][t.InstanceID].Add(t.InstanceID, true);
                          }*/
                    }
                    /*var b = l1[arrayIndex];
                    l1.RemoveAt( arrayIndex );*/
                    
                    // Debug.Log( t );
                    
                    adapter.InternalClearDrag();
                    //  DragAndDrop.PrepareStartDrag();
                    
                    adapter.SetDirtyActiveDescription( targetscene );
                    
                    
                    adapter.bottomInterface.RefreshMemCache( sourcescene );
                    adapter.bottomInterface.RefreshMemCache( targetscene );
                    
                    // adapter.RepaintWindowInUpdate();
                    controller.REPAINT( adapter );
                    foreach (var w in adapter.bottomInterface.WindowController)
                        if (w.REFERENCE_WINDOW) w.REFERENCE_WINDOW.Repaint();
                    foreach (var w in adapter.bottomInterface.FavoritControllers)
                        if (w.REFERENCE_WINDOW) w.REFERENCE_WINDOW.Repaint();
                }
            }
            bool FAVOR_DRAG_VALIDATOR()
            {   /* var type = (bool?)DragAndDrop.GetGenericData( "Dragging Assets" );
                 if (type.HasValue && type.Value) return false;*/
                if (DragAndDrop.objectReferences.Length == 0) return false;
                if (adapter.IS_HIERARCHY()) return DragAndDrop.objectReferences.Any( g => g is GameObject && ((GameObject)g).scene.IsValid() );
                else return DragAndDrop.objectReferences.Any( g => !string.IsNullOrEmpty( adapter.bottomInterface.INSTANCEID_TOGUID( g.GetInstanceID() ) ) );
            }
            UnityEngine.Object[] GetDragData()
            {   if (adapter.pluginID == Initializator.HIERARCHY_ID) return DragAndDrop.objectReferences.Select( o => o as GameObject ).Where( o => o && o.transform
                            && string.IsNullOrEmpty( AssetDatabase.GetAssetPath( o ) ) ).ToArray();
                            
                return DragAndDrop.objectReferences.Where( o => !string.IsNullOrEmpty( Adapter.isProjectObject( o ) ) ).ToArray();
            }
            
            
            
            float[]pixelCost = new float[2];
            GUIStyle shadow = null;
            float SCROLL_DOWN_START;
            void SCROLL_DOWN(FavorUniversalController controller)
            {   SCROLL_DOWN_START = EditorGUIUtility.GUIToScreenPoint( Event.current.mousePosition ).y;
            }
            void SCROLL_DRAG(FavorUniversalController controller)
            {   var M = EditorGUIUtility.GUIToScreenPoint( Event.current.mousePosition).y;
                var dif = M - SCROLL_DOWN_START;
                if (dif != 0)
                {   SCROLL_DOWN_START = M;
                    var C_I = controller.MAIN ? 0 : 1;
                    int scene = adapter.bottomInterface.MemCacheScene;
                    adapter.bottomInterface.GET_BOOKMARKS( ref refBookmarks, scene );
                    if (controller.MAIN) refBookmarks[0].Hierarchy_ScrollY += dif * pixelCost[C_I];
                    else refBookmarks[0].Window_ScrollY += dif * pixelCost[C_I];
                    
                    
                    
                    adapter.RepaintWindowInUpdate();
                }
            }
            
            void ON_SCROLL(ScrollType type, float sc)
            {   if (type == ScrollType.FavorGraphScroll || type == ScrollType.FavorGraphScroll_Window)
                {   int scene = adapter.bottomInterface.MemCacheScene;
                    adapter.bottomInterface.GET_BOOKMARKS( ref refBookmarks, scene );
                    var dif = 40;
                    if (sc < 0) dif *= -1;
                    if (type == ScrollType.FavorGraphScroll) refBookmarks[0].Hierarchy_ScrollY += dif;
                    else refBookmarks[0].Window_ScrollY += dif;
                    SET_DIRTY( scene );
                    
                    EventUse();
                }
            }
            
            
            void EVENT_EVENT_EVENT(Rect lastRect, string mouseDown, string mouseMove, string mouseUp, object arg, FavorUniversalController controller, int controlID, bool useHighlight, int button = 0,
                                   string tooltip = null)
            {   if (Event.current.type == EventType.MouseDown && lastRect.Contains( Event.current.mousePosition ))
                {
                
                    if (Event.current.button == button)
                    {   Adapter.EventUseFast();
                    
                        var target = this;
                        if (arg != null && arg is MouseEventStruct)
                        {   var clone = (MouseEventStruct)((MouseEventStruct)arg).Clone();
                            var p = GUIUtility.GUIToScreenPoint( new Vector2( clone.LocalModuleRect.x, clone.LocalModuleRect.y) );
                            clone.WorldModuleRect = new Rect( p.x + 3, p.y, clone.LocalModuleRect.width - 9, clone.LocalModuleRect.height );
                            arg = clone;
                        }
                        
                        
                        // Debug.Log("ASD");
                        adapter.bottomInterface.ClearAction();
                        
                        INVOKE( mouseDown, arg, target );
                        
                        ADD_ACTION( controlID, lastRect, contains =>
                        {   if (controller.currentAction == null) return true;
                            INVOKE( mouseMove, arg, target );
                            return false;
                        }, contains =>
                        {   INVOKE( mouseMove, arg, target );
                            if (contains)
                            {   INVOKE( mouseUp, arg, target );
                                //  RemoveAndRefresh(type, arrayIndex);
                            }
                        }, currentContoller );
                        
                        /* controller.selection_button = controlID;
                         controller.selection_window = controller.REFERENCE_WINDOW;
                         controller.lastRect = lastRect;
                         var captureCell = lastRect;*/
                        
                        
                        /* controller.selection_action = (mouseUp_b, deltaTIme) =>
                         {
                           adapter.bottomInterface.ClearAction();
                        
                           INVOKE( mouseMove, arg );
                        
                           if (mouseUp_b && captureCell.Contains( Event.current.mousePosition ))
                           {
                             INVOKE( mouseUp, arg );
                             //  RemoveAndRefresh(type, arrayIndex);
                           }
                         }; // ACTION*/
                    } //if button
                }
                
                if (!string.IsNullOrEmpty( tooltip ))
                {   tooltipcontent.tooltip = tooltip;
                    Label( lastRect, tooltipcontent );
                }
                
                if (Event.current.type == EventType.Repaint && useHighlight && HOVER( controlID, lastRect, controller ))
                    // HIPERUI_BUTTONGLOW.Draw( lastRect, false, false, false, false );
                    // Adapter.GET_SKIN().button.Draw( lastRect, true, true, false, false );
                    Adapter.DrawTexture( lastRect, adapter.colorStatic );
                /* if (controller.selection_action != null && useHighlight && controller.selection_button == controlID)
                   GUI.DrawTexture( lastRect, adapter.colorStatic );*/
                
            }
            
            static GUIContent tooltipcontent = new GUIContent();
            
            
            Dictionary<string, MethodInfo> mm = new Dictionary<string, MethodInfo>();
            void INVOKE(string method, object args, object target)
            {   //Debug.Log( method + " " + args );
                if (string.IsNullOrEmpty( method )) return;
                if (!mm.ContainsKey( method )) mm.Add( method, target.GetType().GetMethod( method, (BindingFlags)(-1) ) );
                // Debug.Log( mm[method].GetParameters()[0].ParameterType == args.GetType() );
                
                // try
                {   mm[method].Invoke( target, args == null ? null : new[] { args } );
                }
                /*catch (Exception e)
                { // adapter.logProxy.LogError( e.Message + "\n" + e.StackTrace );
                    throw new Exception(e);
                }*/
            }
            
            
            
            
            
            
            
            
            
            
            
            void DRAG(BottomInterface.UniversalGraphController controller)
            {
            
                EditorGUIUtility.AddCursorRect( dragRect, MouseCursor.SplitResizeUpDown );
                if (Event.current.type == EventType.Repaint)
                {   adapter.box.Draw( dragRect, false, false, false, false );
                    var r = dragRect;
                    r.width = 2;
                    var asd = GUI.color;
                    GUI.color *= BLUE;
                    GUI.DrawTexture( r, Texture2D.whiteTexture );
                    r.x += r.width * 2;
                    GUI.DrawTexture( r, Texture2D.whiteTexture );
                    r.x += dragRect.width - r.width * 2 - r.width;
                    GUI.DrawTexture( r, Texture2D.whiteTexture );
                    r.x -= r.width * 2;
                    GUI.DrawTexture( r, Texture2D.whiteTexture );
                    GUI.color = asd;
                }
                
                
                var EVENT_ID = idOffset - 1;
                if (dragRect.Contains( Event.current.mousePosition ) && Event.current.button == 0 &&
                        Event.current.type == EventType.MouseDown)
                {   EventUse();
                    var startPos = EditorGUIUtility.GUIToScreenPoint( Event.current.mousePosition );
                    var startHeight = controller.HEIGHT;
                    ADD_ACTION( EVENT_ID, null, contains =>
                    {   var p = EditorGUIUtility.GUIToScreenPoint( Event.current.mousePosition );
                        //  var newHeight = startHeight + (startPos.y - p.y);
                        var oldH = controller.HEIGHT;
                        var result = (startPos.y == p.y);
                        controller.HEIGHT = (startHeight + (startPos.y - p.y));
                        CHECK_HEIGHT();
                        controller.scrollPos.y -= (oldH - controller.HEIGHT) / 2;
                        //adapter.SavePrefs();
                        // startPos = p;
                        adapter.RESET_SMOOTH_HEIGHT();
                        
                        return result;
                    }, contains => { adapter.SavePrefs(); }, controller );
                }
                if (HOVER( EVENT_ID, null, controller ))
                {   var asd = GUI.color;
                    var b = BLUE;
                    b.a = 0.4f;
                    GUI.color *= b;
                    var h = dragRect;
                    h.height -= 3;
                    h.y += 1;
                    GUI.DrawTexture( h, Texture2D.whiteTexture );
                    GUI.color = asd;
                    
                    var m = Event.current.mousePosition;
                    EditorGUIUtility.AddCursorRect( new Rect( m.x - 100, m.y - 100, 200, 200 ),
                                                    MouseCursor.SplitResizeUpDown );
                }
            }
            
            
            
            
            
            
            
        }
    }
}
}
