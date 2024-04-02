
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
    
        bool dragReady = false;
        bool IsValidDrag()
        {   var type = (MemType? )DragAndDrop.GetGenericData( adapter.pluginname );
            if (type.HasValue && type.Value == MemType.Custom) return false;
            //   if ( GetDragData().Length != 0 ) Debug.Log( GetDragData()[0] );
            
            return  GetDragData().Length != 0;
        }
        
        UnityEngine.Object[] GetDragData()
        {   if (adapter.pluginID == Initializator.HIERARCHY_ID) return DragAndDrop.objectReferences.Select( o => o as GameObject ).Where( o => o && o.transform
                        && string.IsNullOrEmpty( AssetDatabase.GetAssetPath( o ) ) ).ToArray();
                        
            return DragAndDrop.objectReferences.Where( o => !string.IsNullOrEmpty( Adapter.isProjectObject( o ) ) ).ToArray();
        }
        
        void SetDragData(UnityEngine.Object[] data, MemType? type)
        {   if (data != null)
            {   adapter. InternalClearDrag();
            
                DragAndDrop.objectReferences = data;
                // DragAndDrop.objectReferences = data.Cast<Object>().ToArray();
                // MonoBehaviour.print(DragAndDrop.objectReferences.Length);
                //DragAndDrop.SetGenericData( "BI" , data );
                DragAndDrop.SetGenericData( adapter.pluginname, type );
            }
        }
        
        void UpdateDragArea(Rect dropArea, BottomController controller)
        {   // Cache References:
            Event currentEvent = Event.current;
            EventType currentEventType = currentEvent.rawType;
            
            // The DragExited event does not have the same mouse position data as the other events,
            // so it must be checked now:
            // if (currentEventType == EventType.dra) DragAndDrop.PrepareStartDrag();// Clear generic data when user pressed escape. (Unfortunately, DragExited is also called when the mouse leaves the drag area)
            if (currentEventType == EventType.DragExited)
            {   adapter. InternalClearDrag();
                // EventUse();
            }
            
            // if (!dropArea.Contains( currentEvent.mousePosition )) return;
            
            switch (currentEventType)
            {
            
                /*  case EventType.MouseDrag:
                // If drag was started here:
                var existingDragData = DragAndDrop.GetGenericData("BI") as GameObject[];
                
                if (existingDragData != null) {
                DragAndDrop.StartDrag("Dragging GameObject");
                currentEvent.Use();
                }
                
                break;*/
                
                
                case EventType.DragUpdated:
                    if (IsValidDrag())
                    {   dragReady = true;
                        DragAndDrop.visualMode = DragAndDropVisualMode.Link;
                    }
                    else DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
                    
                    EventUse();
                    break;
                case EventType.Repaint:
                    if (
                        DragAndDrop.visualMode == DragAndDropVisualMode.None ||
                        DragAndDrop.visualMode == DragAndDropVisualMode.Rejected || !dragReady) break;
                    // MonoBehaviour.print(DragAndDrop.visualMode);
                    var c = Color.grey;
                    c.a = 0.2f;
                    Adapter.DrawRect( dropArea, c );
                    break;
                case EventType.DragPerform:
                    DragAndDrop.AcceptDrag();
                    //if (data == null) {
                    if (IsValidDrag())
                    {   if (adapter.IS_HIERARCHY())
                        {   var res = GetDragData();
                            var ss = res.Select(r => r as GameObject).ToArray();
                            var sc = ss.FirstOrDefault();
                            if (sc != null)
                            {   var scene = sc.scene.GetHashCode();
                                var result = ss.Where( s => s.scene.GetHashCode() == scene ).ToArray();
                                AddAndRefreshCustom( result, result[0], controller.GetCategoryIndex( scene ), scene );
                            }
                        }
                        else
                        {   var result = GetDragData().Where(o => o).ToArray();
                            if (result.Length != 0)
                                AddAndRefreshCustom( result, result[0], controller.GetCategoryIndex( adapter.GET_ACTIVE_SCENE ), adapter.GET_ACTIVE_SCENE );
                        }
                        
                        // CreateLine(DragAndDrop.objectReferences[0] as MonoScript, DragAndDrop.objectReferences[0] as Texture2D, int.MaxValue);
                    }
                    
                    adapter.InternalClearDrag();
                    // DragAndDrop.PrepareStartDrag();
                    // DragAndDrop.objectReferences = null;
                    // }
                    /*   CustomDragData receivedDragData = DragAndDrop.GetGenericData(dragDropIdentifier) as CustomDragData;
                    
                    if (receivedDragData != null && receivedDragData.originalList == this.targetList) ReorderObject();
                    else AddDraggedObjectsToList();*/
                    
                    EventUse();
                    break;
                case EventType.MouseUp:
                    // Clean up, in case MouseDrag never occurred:
                    adapter.InternalClearDrag();
                    // DragAndDrop.AcceptDrag();
                    // EventUse();
                    
                    break;
                    
                    
                    
                    
                    
                    
                    
                    
                case EventType.MouseDown:
                    /* if (data != null) {
                    //dragContent = true;
                    DragAndDrop.PrepareStartDrag();// reset data
                    
                    /*                    CustomDragData dragData = new CustomDragData();
                     dragData.originalIndex = somethingYouGotFromYourProperty;
                     dragData.originalList = this.targetList;
                    
                     DragAndDrop.SetGenericData(dragDropIdentifier, dragData);#1#
                    
                    DragAndDrop.SetGenericData("BI", data);
                    //var objectReferences = new[] { property };// Careful, null values cause exceptions in existing editor code.
                    // DragAndDrop.objectReferences = objectReferences;// Note: this object won't be 'get'-able until the next GUI event.
                    
                    currentEvent.Use();
                    }*/
                    
                    break;
                    
            }
            
            
            
            
        }
        
        
        
    }
}
}