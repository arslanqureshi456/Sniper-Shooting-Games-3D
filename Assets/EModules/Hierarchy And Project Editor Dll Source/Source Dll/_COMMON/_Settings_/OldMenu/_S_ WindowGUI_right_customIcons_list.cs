using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;
//namespace EModules
#if PROJECT
    using EModules.Project;
#endif


namespace EModules.EModulesInternal

{
internal partial class SETUPROOT {
    /*, ISerializable, IDeserializationCallback*/
    
    
    DrawCustomIconsClass __CI;
    internal DrawCustomIconsClass CI
    {   get
        {   var res = __CI ?? (__CI = new DrawCustomIconsClass( ));
            res.A = A;
            return res;
        }
    }
    public class DrawCustomIconsClass {
        public const float IC_H = 36;
        public Adapter A;
        public float CusomIconsHeight
        {   get { return customIcons.Count * IC_H + IC_H; }
        }
        public DoubleList<string, Hierarchy_GUI.CustomIconParams> customIcons { get { return Hierarchy_GUI.Get( A ); } }
        
        
        public void Updater( EditorWindow win )
        {   if ( Event.current.type == EventType.Repaint && currentY.Length != 0 )
            {   var tempDragIndex = dragIndex == -1 ? -1
                                    : Mathf.Clamp(
                                        Mathf.RoundToInt((EditorGUIUtility.GUIToScreenPoint(Event.current.mousePosition).y - MouseY -
                                                IC_H / 2) / (float)IC_H), 0, currentY.Length - 1);
                                                
                for ( int i = 0,
                        sib = 0 ; i < currentY.Length ; i++, sib++ )     // if (tempDragIndex == i && i > dragIndex) sib--;
                {   //if (tempDragIndex == i && i < dragIndex) sib++;
                    if ( dragIndex != -1 && i > dragIndex && i <= tempDragIndex ) sib = i - 1;
                    else if ( dragIndex != -1 && i < dragIndex && i >= tempDragIndex ) sib = i + 1;
                    else sib = i;
                    currentY[i] = Mathf.Lerp( currentY[i], sib * IC_H, 0.5f );
                }
                //print(tempDragIndex);
                if ( dragIndex != -1 )     // Repaint();
                {   win.Repaint();
                    //Hierarchy.RepaintAllView();
                }
            }
            
            if ( Event.current.rawType == EventType.MouseUp )
            {   var tempDragIndex = dragIndex == -1 ? -1
                                    : Mathf.Clamp(
                                        Mathf.RoundToInt((EditorGUIUtility.GUIToScreenPoint(Event.current.mousePosition).y - MouseY -
                                                IC_H / 2) / (float)IC_H), 0, currentY.Length - 1);
                if ( dragIndex != -1 && tempDragIndex != -1 && tempDragIndex != dragIndex )
                {   ReplaceFirstToSecond( dragIndex, tempDragIndex );
                    currentY = new float[0];
                    win.Repaint();
                    A.RepaintWindowInUpdate();
                    // Hierarchy.RepaintWindow();
                }
                dragIndex = -1;
            }
        }
        public   float MouseY = -1;
        float[] currentY = new float[0];
        EditorWindow win;
        public void DrawCustomIcons( EditorWindow win, Rect lr )
        {   this.win = win;
        
            A.ChangeGUI();
            int i;
            // content.tooltip = "User Icons";
            // GUILayout.Label("");
            //  var lr = GUILayoutUtility.GetLastRect();
            
            var XX = 7;
            var YY = 5;
            
            Adapter.INTERNAL_BOX( new Rect( XX, YY, lr.width - 15, CusomIconsHeight ), PlusContentEmpty );
            
            MouseY = EditorGUIUtility.GUIToScreenPoint( Vector2.zero ).y;
            
            
            if ( currentY.Length != customIcons.Count )
            {   currentY = new float[customIcons.Count];
                for ( i = 0 ; i < customIcons.Count ; i++ )
                    currentY[i] = i * IC_H;
            }
            
            // var lineRect = new Rect(0, 0, W, H);
            for ( i = 0 ; i < customIcons.Count ; i++ )
            {
            
                // var customIcon = Hierarchy.par.customIcons[i];
                // var r = new Rect(0, currentY[i], lr.width, IC_H);
                var r = new Rect(XX, YY + currentY[i], lr.width, IC_H);
                
                if ( dragIndex == i )
                {   r.x = Event.current.mousePosition.x - IC_H / 2;
                    r.y = Event.current.mousePosition.y - IC_H / 2;
                }
                
                // GUI.BeginClip(r);
                DrawLine( i, lr, r.x, r.y );
                // GUI.EndClip();
                // lineRect.y += H;
            }
            
            
            var lineRect = new Rect(XX, YY + customIcons.Count * IC_H, lr.width, IC_H);
            ExampleDragDropGUI( A, lineRect, null, DRAG_VALIDATOR_MONOANDTEXTURE, DRAG_PERFORM_USERICONS );
            /*  if (lineRect.Contains(Event.current.mousePosition))
              {
                  if (Event.current.type.Equals(EventType.Repaint)) GUI.DrawTexture(lineRect,Hierarchy.sec);
              }*/
            lineRect.width -= 15;
            var olds = A.button.fontSize;
            A.button.fontSize = 20;
            var butres = GUI.Button( lineRect, PlusContent,  A.button);
            A.button.fontSize = olds;
            if ( butres )
            {   if ( Event.current.button == 0 ) CreateLine( null, null, int.MaxValue );
            }
            A.RestoreGUI();
            
            
            
        }
        
        #region CUSTOM ICONS
        void ReplaceFirstToSecond( int i1, int i2 )
        {   Hierarchy_GUI.Undo( A, "Change Custom Icons" );
        
        
            // var min = Math.Min(i1, i2);
            // var max = Math.Max(i1, i2);
            var v1 = Hierarchy_GUI.Get(A)[i1];
            Hierarchy_GUI.Get( A ).RemoveAt( i1 );
            
            if ( i2 >= Hierarchy_GUI.Get( A ).Count ) Hierarchy_GUI.Get( A ).Add( v1.Key, v1.Value );
            else Hierarchy_GUI.Get( A ).Insert( i2, v1.Key, v1.Value );
            
            Hierarchy_GUI.SetDirtyObject( A );
            A.RepaintWindowInUpdate();
            // Hierarchy.RepaintAllView();
            
            
            
        }
        
        void RemoveLine( int index )
        {   if ( index < 0 || index >= Hierarchy_GUI.Get( A ).Count ) return;
            Hierarchy_GUI.Undo( A, "Change Custom Icons" );
            
            Hierarchy_GUI.Get( A ).RemoveAt( index );
            Hierarchy_GUI.SetDirtyObject( A );
            A.RepaintWindowInUpdate();
            //  Hierarchy.RepaintAllView();
        }
        
        void CreateLine( MonoScript component, Texture2D icon, int index )
        {   string key = null;
            Hierarchy_GUI.CustomIconParams value = new Hierarchy_GUI.CustomIconParams();
            if ( component != null ) key = AssetDatabase.AssetPathToGUID( AssetDatabase.GetAssetPath( component ) );
            if ( icon != null ) value.value = AssetDatabase.AssetPathToGUID( AssetDatabase.GetAssetPath( icon ) );
            
            //   var init = Hierarchy_GUI.Initialize();
            Hierarchy_GUI.Undo( A, "Change Custom Icons" );
            if ( index >= Hierarchy_GUI.Get( A ).Count ) Hierarchy_GUI.Get( A ).Add( key, value );
            else Hierarchy_GUI.Get( A ).Insert( index, key, value );
            Hierarchy_GUI.SetDirtyObject( A );
            A.RepaintWindowInUpdate();
            // Hierarchy.RepaintAllView();
        }
        
        
        //  bool dragContent = false;
        
        
        
        
        
        
        
        
        
        GUIContent PlusContent = new GUIContent()
        {   text = "+",
                tooltip = "Drag MonoBehaviour Script or Texture here"
        };
        GUIContent PlusContentEmpty = new GUIContent()
        {   text = "",
                tooltip = "Drag MonoBehaviour Script or Texture here"
        };
        
        int dragIndex = -1;
        
        void DrawLine( int i, Rect lr, float xOffset, float yOffset )        // ExampleDragDropGUI(lineRect, new CustomDragData() { index = i });
        {
        
            MonoScript script = null;
            Texture2D icon = null;
            if ( !string.IsNullOrEmpty( customIcons[i].Key ) )
            {   var scriptPath = AssetDatabase.GUIDToAssetPath(customIcons[i].Key);
                if ( !string.IsNullOrEmpty( scriptPath ) ) script = AssetDatabase.LoadAssetAtPath<MonoScript>( scriptPath );
            }
            if ( !string.IsNullOrEmpty( customIcons[i].Value.value ) )
            {   var scriptPath = AssetDatabase.GUIDToAssetPath(customIcons[i].Value.value);
                if ( !string.IsNullOrEmpty( scriptPath ) ) icon = AssetDatabase.LoadAssetAtPath<Texture2D>( scriptPath );
            }
            var r = new Rect(0, 0, IC_H, IC_H);
            
            r.x += xOffset;
            r.y += yOffset;
            
            var oldsl = A.label.fontSize;
            A.label.fontSize = (int)EditorGUIUtility.singleLineHeight;
            //GUI.Label(r, "■");
            //  if (GUI.Button(r, "▲"))
            GUI.Label( r, "=", A.label );
            A.label.fontSize = oldsl;
            // if (r.Contains( Event.current.mousePosition )) A.RepaintWindow();
            
            if ( r.Contains( Event.current.mousePosition ) && Event.current.type == EventType.MouseDown )
            {   if ( Event.current.button == 0 )
                {   dragIndex = i;
                    /*InternalEditorUtility.repa*/
                    win.Repaint();
                    A.RepaintWindowInUpdate();
                }
            }
            EditorGUIUtility.AddCursorRect( r, MouseCursor.Link );
            /* if (dragContent)*/
            
            // ExampleDragDropGUI(r, new CustomDragData() { index = i });
            
            A.RestoreGUI();
            
            // r.Set( r.width + 10 , (r.height - EditorGUIUtility.singleLineHeight) / 2 , lr.width - (IC_H + 10 + IC_H + 10 + IC_H) - IC_H / 3 , EditorGUIUtility.singleLineHeight );
            r.Set( r.width + 10, (r.height - EditorGUIUtility.singleLineHeight) / 2, lr.width - (IC_H + 10 + IC_H + 10 + IC_H) - IC_H / 3, EditorGUIUtility.singleLineHeight );
            
            r.x += xOffset;
            r.y += yOffset;
            
            
            
            
            
            Object newScript = script;
            try
            {   newScript = EditorGUI.ObjectField( r, script, typeof( MonoScript ), false );
            }
            catch
            {   newScript = script;
            }
            
            r.Set( lr.width - IC_H - IC_H, 0, IC_H, IC_H );
            
            r.x += xOffset;
            r.y += yOffset;
            
            var cRect = r;
            cRect.x -= cRect.width - 20;
            cRect.width -= 20;
            cRect.y += 4;
            cRect.height -= 8;
            var oldCol = customIcons[i].Value.color;
            var newCol = oldCol;
            try { newCol = Adapter.COLOR_FIELD( cRect, new GUIContent(), oldCol, false, true, false ); }
            catch { }
            EditorGUIUtility.AddCursorRect( cRect, MouseCursor.Link );
            
            
            Object newicon = icon;
            try
            {   var asdasd = GUI.color;
                GUI.color *= new Color( newCol.r, newCol.g, newCol.b, 1 );
                newicon = EditorGUI.ObjectField( r, icon, typeof( Texture2D ), false );
                GUI.color = asdasd;
            }
            catch
            {   newicon = icon;
            }
            
            
            A.ChangeGUI();
            
            if ( newScript != script )
            {   var v = customIcons[i];
                v.Key = AssetDatabase.AssetPathToGUID( AssetDatabase.GetAssetPath( newScript ) );
                customIcons[i] = v;
                A.RepaintWindowInUpdate();
                // Hierarchy.RepaintAllView();
            }
            if ( newicon != icon )
            {   var v = customIcons[i];
                v.Value.value = AssetDatabase.AssetPathToGUID( AssetDatabase.GetAssetPath( newicon ) );
                customIcons[i] = v;
                A.RepaintWindowInUpdate();
                //Hierarchy.RepaintAllView();
            }
            if ( oldCol != newCol )
            {   var v = customIcons[i];
                v.Value.color = newCol;
                customIcons[i] = v;
                A.RepaintWindowInUpdate();
            }
            
            r.Set( lr.width - IC_H - 3, 0, IC_H - 10, IC_H );
            r.x += xOffset;
            r.y += yOffset;
            if ( GUI.Button( r, "X", A.button ) )
            {   if ( Event.current.button == 0 )
                {   RemoveLine( i );
                    //CreateLine(null, null, int.MaxValue);
                }
                //dragContent = true;
            }
        }
        
        #endregion
        
        internal void DRAG_PERFORM_USERICONS()
        {   CreateLine( DragAndDrop.objectReferences[0] as MonoScript, DragAndDrop.objectReferences[0] as Texture2D, int.MaxValue );
        }
        
    }
    
    
    
    
    
    
    
    
    public class CustomDragData {
        public int index = -1;
        /* public int originalIndex;
         public List<object> originalList;*/
    }
    internal static bool DRAG_VALIDATOR_MONOANDTEXTURE()
    {   return DragAndDrop.objectReferences.Length == 1 && (DragAndDrop.objectReferences[0] is Texture2D || IsMonoScript( DragAndDrop.objectReferences[0] ));
    }
    
    
    
    internal static bool DRAG_VALIDATOR_ONLYMONO()
    {   return DragAndDrop.objectReferences.Length == 1 && IsMonoScript( DragAndDrop.objectReferences[0] );
    }
    
    internal static bool IsMonoScript( UnityEngine.Object ob )
    {   return (ob is MonoScript) && ((MonoScript)ob).GetClass() != null;
    }
    
    internal static void ExampleDragDropGUI( Adapter A, Rect dropArea, CustomDragData data, Func<bool> validate, Action perform, Color ? color = null )         // Cache References:
    {   //  Event currentEvent = Event.current;
        EventType currentEventType = Event.current.type;
        
        // The DragExited event does not have the same mouse position data as the other events,
        // so it must be checked now:
        if ( currentEventType == EventType.DragExited )
            A.InternalClearDrag();
            
        if ( !dropArea.Contains( Event.current.mousePosition ) ) return;
        
        switch ( currentEventType )
        {   case EventType.MouseDown:
                if ( data != null )     //dragContent = true;
                {   A.InternalClearDrag();
                    // DragAndDrop.PrepareStartDrag();// reset data
                    
                    /*                    CustomDragData dragData = new CustomDragData();
                                        dragData.originalIndex = somethingYouGotFromYourProperty;
                                        dragData.originalList = this.targetList;
                    
                                        DragAndDrop.SetGenericData(dragDropIdentifier, dragData);*/
                    
                    DragAndDrop.SetGenericData( "HY1", data );
                    //var objectReferences = new[] { property };// Careful, null values cause exceptions in existing editor code.
                    // DragAndDrop.objectReferences = objectReferences;// Note: this object won't be 'get'-able until the next GUI event.
                    
                    Adapter.EventUseFast();
                }
                
                
                break;
            case EventType.MouseDrag:
                // If drag was started here:
                CustomDragData existingDragData = DragAndDrop.GetGenericData("HY1") as CustomDragData;
                
                if ( existingDragData != null )
                {   DragAndDrop.StartDrag( "Dragging List ELement" );
                    Adapter.EventUseFast();
                }
                
                break;
            case EventType.DragUpdated:
                if ( validate() ) DragAndDrop.visualMode = DragAndDropVisualMode.Link;
                else DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
                
                Adapter.EventUseFast();
                break;
            case EventType.Repaint:
                if (
                    DragAndDrop.visualMode == DragAndDropVisualMode.None ||
                    DragAndDrop.visualMode == DragAndDropVisualMode.Rejected ) break;
                    
                if ( validate() ) Adapter.DrawRect( dropArea, color ?? Color.grey );
                break;
            case EventType.DragPerform:
                DragAndDrop.AcceptDrag();
                if ( data == null )
                {   if ( validate() )
                    {   perform();
                    }
                    
                }
                /*   CustomDragData receivedDragData = DragAndDrop.GetGenericData(dragDropIdentifier) as CustomDragData;
                
                   if (receivedDragData != null && receivedDragData.originalList == this.targetList) ReorderObject();
                   else AddDraggedObjectsToList();*/
                
                Adapter.EventUseFast();
                break;
            case EventType.MouseUp:
                // Clean up, in case MouseDrag never occurred:
                A.InternalClearDrag();
                //ADragAndDrop.PrepareStartDrag();
                break;
        }
        
    }
    
    
    
}
}
