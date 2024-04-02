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
    
        internal partial class FavorGraph : Adapter.BottomInterface.BOTTOM_GRAPH {
        
        
        
        
        
            string[] popDisplay = new string[1];
            GUIContent cc2 = new GUIContent() { tooltip = "Filter files within a folder by a certain type"};
            GUIContent cc3 = new GUIContent() { tooltip = "Hide folders and show objects as one list"};
            GUIContent TypeButtonGUIContent = new GUIContent();
            // TreeItem  item = null;
            
            //              void DRAW_OBJECT_ITEM(Adapter.HierarchyObject m_h, int m_scene, TempColorClass OTHER,
            //                                    bool m_IS_ROOT, bool m_drawCount, bool m_higlightDrag,  /*IStringSetter*/ IStringSetter m_favString, int m_favString_index,
            //                                    int m_DEEP, bool mayFolded /*= true*/, bool foldOverride/* = false*/)
            //              {   var FOLDER = 0;
            //                  // m_drawCount |= m_h.project.IsFolder;
            //                  m_drawCount = false;
            //                  //if (m_drawCount || m_h.project.IsFolder) FOLDER = 1;
            //                  // if (mayFolded) FOLDER = 1;
            //
            //                  if ( m_h.project.IsFolder && ((refBookmarks[CURRENT_CAT].FavParams ) != SHOW_ALL_CONTENT)) FOLDER = 2; //mayFolded &&
            //                  // mouseEventStruct.memoryRoot.InstanceID.g
            //                  if (!string.IsNullOrEmpty( m_favString.GetString( m_favString_index, adapter.pluginname ) )) FOLDER = 3;
            //
            //
            //                  item = GET_TREE_ITEM( m_h.project.guid, refBookmarks[CURRENT_CAT], CURRENT_CONTROLLER, m_IS_ROOT );
            //
            //                  Debug.Log(m_h.project.assetPath + " " + FOLDER + " " + mayFolded + " " + foldOverride + " " + m_drawCount + " " + refBookmarks[CURRENT_CAT].FavParams + " ");
            //
            //                  if (CURRENT_INDEX < 0 )CURRENT_INDEX = 0;
            //                  if (CURRENT_INDEX >= refBookmarks[CURRENT_CAT].array.Count )CURRENT_INDEX = refBookmarks[CURRENT_CAT].array.Count - 1;
            //
            //                  var folderForSkip = (refBookmarks[CURRENT_CAT].array[CURRENT_INDEX].FavParams ) == SHOW_ALL_CONTENT && FOLDER == 0 && m_h.project.IsFolder;
            //                  //if (m_h.project.assetPath.Contains("EModules") && m_h.project.assetPath.Contains("SAVED DATA")) Debug.Log(m_h.name);
            //                  if (!SKIP && !folderForSkip)
            //                  {
            //
            //                      // content.text = mouseEventStruct.memoryRoot != null ? mouseEventStruct.memoryRoot.ToString() : m_h.name;
            //                      content.text = m_h.name;
            //
            //
            //                      //
            //                      // Debug.Log(m_h.project.assetPath);
            //                      /*  var cellRectContains = cell.Contains(Event.current.mousePosition);
            //                           var cellAction = controller.selection_window == controller.REFERENCE_WINDOW && controller.selection_button == CONTROL_ID;
            //                           var active = cellAction && !controller.selection_info && cellRectContains;
            //                           var selected = false; */
            //
            //                      if (CURRENT_STATE != 0)
            //                      {   EditorGUIUtility.AddCursorRect( cell, CURRENT_STATE == 1 ? MouseCursor.ArrowPlus : CURRENT_STATE == 2 ? MouseCursor.ArrowMinus : MouseCursor.ScaleArrow );
            //                      }
            //
            //                      var context = adapter.bottomInterface.GetContent( m_h );
            //                      // if (context != null)
            //                      {   icon = context.add_icon;
            //                          if (icon && context.add_hasiconcolor)
            //                          {   iconColor = context.add_iconcolor;
            //                          }
            //
            //                      }
            //
            //                      var fl = 0f;
            //
            //                      var HAS_TYPEBUT = m_IS_ROOT && (mayFolded && m_h.project.IsFolder && ((refBookmarks[CURRENT_CAT].FavParams ) != SHOW_ALL_CONTENT));
            //
            //                      var HAS_DESC = CURRENT_CONTROLLER.IS_MAIN ? adapter.par.BottomParams.SHOW_ALL_DESCRIPTIONS_INHIER : adapter.par.BottomParams.SHOW_ALL_DESCRIPTIONS_INWIN;
            //                      if (HAS_DESC)
            //                          cell.width -= fullCell.width / 3 - fl;
            //
            //
            //                      var index = -1;
            //                      float type_width = 0;
            //                      if (HAS_TYPEBUT)
            //                      {   var ext = GET_PATHS(m_h.project.assetPath).extensions;
            //                          if (!string.IsNullOrEmpty( m_favString.GetString( m_favString_index, adapter.pluginname ) ) && ext.ContainsKey( m_favString.GetString( m_favString_index, adapter.pluginname ) ))
            //                          {   popDisplay[0] = m_favString.GetString( m_favString_index, adapter.pluginname );
            //                              index = ext[m_favString.GetString( m_favString_index, adapter.pluginname )];
            //                          }
            //                          else
            //                          {   popDisplay[0] = "type";
            //                          }
            //
            //
            //                          TypeButtonGUIContent.text = popDisplay[0];
            //                          // Adapter.GET_SKIN().button.CalcMinMaxWidth( cc, out type_width, out type_width );
            //                          EditorStyles.popup.CalcMinMaxWidth( TypeButtonGUIContent, out type_width, out type_width );
            //
            //                          if (CURRENT_INDEX < refBookmarks[CURRENT_CAT].array.Count)
            //                          {   var flat = (refBookmarks[CURRENT_CAT].array[CURRENT_INDEX].FavParams ) == SHOW_ALL_CONTENT;
            //                              var lit_icon = adapter.GetIconWithPersonal( flat ? "FAVORIT_LIST_ICON ON" : "FAVORIT_LIST_ICON");
            //                              type_width += lit_icon.width;
            //
            //                              cell.width -= type_width;
            //                          }
            //                          else
            //                          {   HAS_TYPEBUT = false;
            //                          }
            //
            //                      }
            //                      //#COLUP
            //                      // icon = adapter.bottomInterface.GetContent( m_h );
            //                      //if (icon != null) iconColor = adapter.MOI.M_Colors.GetColorForObject( m_h );
            //
            //                      if (m_IS_ROOT && !icon)
            //                      {   /* Debug.Log( m_h.name  );
            //                           Debug.Log( adapter.bottomInterface.GetContent( m_h ) == null );*/
            //                          //icon = AssetDatabase.GetCachedIcon( m_h.project.assetPath );
            //                          icon = adapter.GetIcon( "STAR" );
            //                      }
            //
            //                      //** SLECTION BG **////////////////////////
            //                      //if (adapter._mSelectedO.ContainsKey( h.id )) Adapter.SelectRect( fullCell, .1f );
            //                      if (!drawg1.HasValue
            //                              && adapter.par.FavoritesNavigatorParams.SHOW_SELECTIONS
            //                              && adapter.IsSelected( m_h.id ))
            //                          adapter.SelectRect( fullCell, 1f );
            //
            //                      //** HIGLIGHTER BG **////////////////////////
            //                      if (OTHER != null && OTHER.HAS_BG_COLOR)
            //                      {   var oc = GUI.color;
            //                          var c = OTHER.BGCOLOR;
            //                          //  c.a /= 2;
            //                          c.a = (byte)(c.a / 1.2f);
            //                          GUI.color *= c;
            //                          GUI.DrawTexture( fullCell, Texture2D.whiteTexture );
            //                          GUI.color = oc;
            //                      }
            //
            //
            //                      /*if (textColor.HasValue)
            //                      {
            //                        var c = textColor.Value;
            //                        if (c.r != 0 || c.g != 0 || c.b != 0 || c.a != 0)
            //                          adapter.bottomInterface.SetStyleColor( style, c ););****************
            //                      }*/
            //
            //                      // set event
            //
            //                      /*var style = Adapter.GET_SKIN().label;
            //                      style.padding.left = (int)(LH * 0.6f);
            //                      var drawOffset = cell;
            //                      if (drawCount)
            //                      {
            //                        drawOffset.x += LH / 2;
            //                        drawOffset.width -= LH / 2;
            //                      }
            //                      else
            //                      {
            //                        style.padding.left = (int)LH;
            //                      }*/
            //
            //
            //                      //if ()
            //                      //if (foldOverride) Debug.Log(m_h.name + " " + FOLDER);
            //                      //    #region DRAWLINE
            //                      if (FOLDER > 1  || (FOLDER == 1  ) || foldOverride)
            //                      {   item = DO_FOLD( ref cell, CURRENT_CONTROLLER, refBookmarks[CURRENT_CAT], m_h.project.guid, false, m_IS_ROOT );
            //                          // Debug.Log(FOLDER + " " + foldOverride);
            //                          if (m_IS_ROOT)
            //                          {   if (string.IsNullOrEmpty(m_h.project.assetPath)) content.text = "Error load";
            //                              else content.text = "<i>" + m_h.project.assetFolder + "</i>/<b>" + content.text + "</b>";
            //                          }
            //
            //
            //                      }
            //                      else
            //                      {   cell.x += FOLD_WIDTH;
            //                          cell.width -= FOLD_WIDTH;
            //                      }
            //
            //
            //
            //                      //** ICON **////////////////////////
            //                      var drawOffset = cell;
            //                      var oldH = drawOffset.height;
            //                      drawOffset.height = LH;
            //                      drawOffset.y += (oldH - drawOffset.height) / 2;
            //                      drawOffset.width = drawOffset.height;
            //
            //
            //                      if (string.IsNullOrEmpty(content.text) )
            //                      {
            //
            //                          var ol = GUI.skin.label.alignment;
            //                          var oc = GUI.skin.label.normal.textColor;
            //                          GUI.skin.label.alignment = TextAnchor.MiddleLeft;
            //                          GUI.skin.label.normal.textColor = Color.red;
            //                          GUI.Label(cell, "Error load");
            //                          GUI.skin.label.alignment = ol;
            //                          GUI.skin.label.normal.textColor = oc;
            //
            //                      }
            //                      else
            //                      {   if (icon != null)
            //                          {   var c = GUI.color;
            //                              if (iconColor.HasValue) GUI.color *= iconColor.Value;
            //                              Adapter.DrawTexture( drawOffset, icon );
            //                              if (iconColor.HasValue) GUI.color = c;
            //                          }
            //                          //** COUNT **////////////////////////
            //                          if (m_drawCount)
            //                          {   drawOffset = cell;
            //                              /* drawOffset.x += LH / 2;
            //                               drawOffset.width -= LH / 2;*/
            //                              adapter.bottomInterface.DRAW_COUNT( drawOffset, (int)LH, Adapter.BottomInterface.MemType.Custom, mouseEventStruct.memoryRoot, false );
            //                          }
            //                      }
            //
            //                      float labelWidth = CALC_LABEL( cell, content, m_IS_ROOT ? FONT_SIZE : (FONT_SIZE /*- 1*/), OTHER );
            //
            //
            //                      if (m_higlightDrag && FAVOR_DRAG_VALIDATOR()) GUI.DrawTexture( fullCell, adapter.colorStatic );
            //
            //
            //
            //
            //
            //                      cell.x += drawOffset.height;
            //                      cell.width -= drawOffset.height;
            //                      if (!string.IsNullOrEmpty(content.text) )
            //                      {   if (m_IS_ROOT && labelWidth > cell.width)
            //                          {   var c = cell;
            //                              c.width  = c.height * 0.85f;
            //                              GUI.Label(c, "...");
            //                              c.x += c.width;
            //                              c.width = cell.width - c.width;
            //                              DRAW_LABEL( c, content, m_IS_ROOT ? FONT_SIZE : (FONT_SIZE /*- 1*/), OTHER, TextAnchor.MiddleRight);
            //                          }
            //                          else
            //                          {   DRAW_LABEL( cell, content, m_IS_ROOT ? FONT_SIZE : (FONT_SIZE /*- 1*/), OTHER);
            //                          }
            //                      }
            //                      //!///* EVENT *//////
            //
            //                      if (m_IS_ROOT)
            //                      {   DRAG_DRAG( cell, false, CURRENT_CONTROLLER, m_scene, ref refBookmarks, drawg1, drawg3, m_h.scene, ref refBookmarks, CURRENT_CAT, mouseEventStruct.memoryRoot.ArrayIndex );
            //
            //
            //
            //                      }
            //                      cell.x += cell.width;
            //
            //
            //                      //if (mayFolded && m_h.project.IsFolder && ((refBookmarks[CURRENT_CAT].FavParams ) != SHOW_ALL_CONTENT))
            //                      if (HAS_TYPEBUT)
            //                          // {   if (m_h.project.IsFolder)
            //                      {   var ic = cell;
            //                          //ic.x -= cell.width;
            //
            //                          // ic.x += type_width;
            //
            //
            //                          var flat = (refBookmarks[CURRENT_CAT].array[CURRENT_INDEX].FavParams ) == SHOW_ALL_CONTENT;
            //                          var lit_icon = adapter.GetIconWithPersonal( flat ? "FAVORIT_LIST_ICON ON" : "FAVORIT_LIST_ICON");
            //
            //                          ic.width = type_width - lit_icon.width;
            //
            //                          ic.y += (cell.height - EditorStyles.popup.normal.background.height) / 2;
            //                          ic.height = EditorStyles.popup.normal.background.height;
            //                          //ic.height -= 3;
            //                          // Debug.Log( cell );
            //                          /*
            //                          var searchIcon = adapter.GetIcon("FAVORIT_FOLDERS_ICON");
            //                          cell.width = searchIcon.width;
            //                          /* var ic = cell;
            //                          ic.y += (cell.height - searchIcon.height) / 2;
            //                          ic.height = searchIcon.height;
            //                          GUI.DrawTexture( ic, searchIcon );*/
            //                          //GUI.DrawTexture( ic, Texture2D.whiteTexture );
            //                          //  Debug.Log( Event.current.type );
            //                          // if (Event.current.type == EventType.MouseDown || Event.current.type == EventType.Repaint && FLAG)
            //
            //                          if (Event.current.type == EventType.Repaint)
            //                          {   EditorStyles.popup.Draw( ic, TypeButtonGUIContent, false, false, false, false );
            //                          }
            //
            //                          /*FILTER_ON_DOWN__scanneddata = GET_PATHS( m_h.project.assetPath );
            //                          FILTER_ON_DOWN__index = index;
            //                          FILTER_ON_DOWN__m_favString = m_favString;
            //                          FILTER_ON_DOWN__m_favString_index = m_favString_index;
            //                          FILTER_ON_DOWN__m_scene = m_scene;*/
            //                          //FILTER_ON_DOWN__ic = ic;
            //                          var nms = mouseEventStruct.Clone();
            //                          nms.hierarchy_obect = m_h;
            //                          nms.itemIndex = index;
            //                          nms.categoryIndex = m_favString_index;
            //                          nms.otherstring = m_favString;
            //                          //  mouseEventStruct.otherobject = GET_PATHS( m_h.project.assetPath );
            //                          nms.scene = m_scene;
            //
            //                          EVENT_EVENT_EVENT( ic, null, null, "FILTER_ON_DOWN", nms, CURRENT_CONTROLLER, CONTROL_ID++, true );
            //
            //                          /* else {
            //                             EditorGUI.Popup( ic, 0, popDisplay );
            //                           }*/
            //                          if (Event.current.type == EventType.Repaint) GUI.Label( ic, cc2 );
            //
            //
            //                          ic.x += ic.width;
            //                          ic.width = lit_icon.width;
            //                          Adapter.DrawTexture( ic, lit_icon );
            //                          EVENT_EVENT_EVENT( ic, null, null, "LIST_ON_DOWN", mouseEventStruct, CURRENT_CONTROLLER, CONTROL_ID++, true );
            //                          if (Event.current.type == EventType.Repaint) GUI.Label( ic, cc3 );
            //                          //EVENT_EVENT_EVENT( ic, null, null, "FILTER_ON_DOWN", mouseEventStruct, CURRENT_CONTROLLER, CONTROL_ID++, true );
            //                      }
            //
            //                      //style.Draw( drawOffset, content, active, active, false, active );
            //
            //
            //                      //** CLOSE **////////////////////////
            //                      /* if (selected)
            //                       {
            //                         fl = cell.width = 16;
            //                         cell.x -= cell.width;
            //                         close_rect.Set( cell.x + cell.width - 2 - 10, cell.y + (fullCell.height - 12) / 2, 12, 12 );
            //                         //GUI.DrawTexture( close_rect, adapter.GetIcon( "HIPERUI_CLOSE" ) );
            //                         GUI.Label( close_rect, "X" );
            //                         //!/// * EVENT * //////
            //                         EVENT_EVENT_EVENT( close_rect, null, null, "REMOVE_ON_UP", mouseEventStruct, controller, CONTROL_ID++, true );
            //                       }*/
            //
            //
            //
            //                      //** DESCRIPTION **////////////////////////
            //                      //  if (IS_ROOT)
            //                      if (HAS_DESC)
            //                      {   cell.width = fullCell.width / 3 - fl;
            //                          cell.x = fullCell.x + fullCell.width / 3 * 2 + fl;
            //
            //                          if (mouseEventStruct.memoryRoot != null) content_des = GET_DESCRIPTION( mouseEventStruct.memoryRoot );
            //                          else content_des = GET_DESCRIPTION( m_h );
            //
            //                          if (Event.current.type == EventType.Repaint)
            //                          {   //var c = GUI.color;
            //                              // GUI.color *= adapter.bottomInterface.coloAlpha* adapter.bottomInterface.coloAlpha * adapter.bottomInterface.coloAlpha * adapter.bottomInterface.coloAlpha;
            //                              //Adapter. INTERNAL_BOX( new Rect( cell.xnew Rect( cell.x, cell.y, 1, cell.height ) cell.y, 1, cell.height ), "" );
            //                              GUI.DrawTexture( new Rect( cell.x, cell.y, 2, cell.height ), adapter.GetIcon( "SEPARATOR" ) );
            //                              /* GUI.DrawTexture( new Rect( cell.x, cell.y, 1, cell.height ), adapter.GetIcon())
            //                               EditorGUI.DrawRect( new Rect( cell.x, cell.y, 1, cell.height ),  adapter.HR_COLOR );*/
            //                              //GUI.color = c;
            //                          }
            //                          cell.x += 2;
            //                          cell.width -= 2;
            //                          DRAW_LABEL( cell, content_des, adapter.FONT_8() );
            //                          //!///* EVENT *//////
            //                          //  mouseEventStruct.MousePosStruct =   InputData.WidnwoRect( false, Event.current.mousePosition, 190, 68, adapter  );
            //                          mouseEventStruct.MousePosStruct =  new MousePos(Event.current.mousePosition, MousePos.Type.Input_190_68, false, adapter);
            //
            //                          EVENT_EVENT_EVENT( cell, null, null, "DESCRIPTION_ON_UP", mouseEventStruct, CURRENT_CONTROLLER, CONTROL_ID++, true );
            //                          // if (!string.IsNullOrEmpty( content_des.text ))
            //                          EVENT_EVENT_EVENT( cell, null, null, "DESCRIPTION_ON_UP_RIGHT", mouseEventStruct, CURRENT_CONTROLLER, CONTROL_ID++, true, 1 );
            //                      }
            //
            //
            //                      /*if (controller.selection_action != null && controller.selection_button == CONTROL_ID + 5)
            //                        Adapter.GET_SKIN().button.Draw( controller.lastRect, REALEMPTY_CONTENT, false, false, false, true );*/
            //
            //                      //DRAWSTYLE( style, drawOffset, active, controller, memoryRoot[i], CONTROL_ID );
            //                      //////////////////////////////////////////////////////
            //
            //                      //adapter.bottomInterface.RestoreStyleColor( style );****************
            //
            //                      mouseEventStruct.LocalModuleRect = fullCell;
            //                      EVENT_EVENT_EVENT( fullCell, null, "ITEM_ON_DRAG", "ITEM_ON_DOWN", mouseEventStruct, CURRENT_CONTROLLER, CONTROL_ID++, true );
            //                      EVENT_EVENT_EVENT( fullCell, null, null, "MENUCALL", mouseEventStruct, CURRENT_CONTROLLER, CONTROL_ID++, true, 1 );
            //
            //
            //
            //
            //
            //
            //                  }
            //
            //
            //
            //                  if (!folderForSkip)
            //                  {   fullCell.y += fullCell.height;
            //                  }
            //
            //
            //
            //                  //#endregion
            //
            //
            //
            //
            //
            //                  // 3 level
            //                  if (FOLDER != 0 && item != null && item.Expand)
            //                  {   if (FOLDER == 1 )
            //                      {   // var favStringList = mouseEventStruct.memoryRoot.InstanceID.GET_FavString();
            //                          //var favString = mouseEventStruct.memoryRoot.InstanceID.GET_FavString();
            //                          var guidList = mouseEventStruct.memoryRoot.InstanceID.GUIDsList;
            //                          var pathList = mouseEventStruct.memoryRoot.InstanceID.GET_PATHsList(adapter.pluginname);
            //                          var captureMeem = mouseEventStruct.memoryRoot.InstanceID;
            //
            //                          for (int i = 0 ; i < guidList.Count ; i++)
            //                          {   if (guidList[i] == mouseEventStruct.memoryRoot.InstanceID.GUIDsActiveGameObject) continue;
            //
            //                              var nextDEEP = m_DEEP + 1;
            //                              cell = fullCell;
            //                              cell.x = indents * nextDEEP;
            //                              cell.width -= indents * nextDEEP;
            //
            //                              var guid = guidList[i];
            //                              var h = adapter.GetHierarchyObjectByGUID(ref guid, pathList[i]);
            //
            //
            //                              var higlightDrag =  HL_DR == h.id ;
            //                              // long ID = (int)m_id << 32 | h.id;
            //                              // var favString = favStringList[i];
            //                              // var me = mouseEventStruct.Clone();
            //                              mouseEventStruct.Set_Get( null, h, h.scene, CURRENT_CAT, -1, CURRENT_CONTROLLER, CURRENT_STATE );
            //
            //                              DRAW_OBJECT_ITEM( h, m_scene, tempColor, false, false, higlightDrag,  captureMeem, i, nextDEEP, true, false );
            //                          }
            //                      }
            //                      if (FOLDER == 2)
            //                      {   var flat = (refBookmarks[CURRENT_CAT].array[CURRENT_INDEX].FavParams ) == SHOW_ALL_CONTENT;
            //                          DRAW_FOLDERS( m_h.project.assetPath, null, m_DEEP + 1,  flat );
            //                      }
            //                      if (FOLDER == 3)
            //                      {   var flat = (refBookmarks[CURRENT_CAT].array[CURRENT_INDEX].FavParams ) == SHOW_ALL_CONTENT;
            //                          DRAW_FOLDERS( m_h.project.assetPath, m_favString.GetString( m_favString_index, adapter.pluginname ), m_DEEP + 1,  flat);
            //                      }
            //                  }
            //
            //
            //
            //              }
            //
            //
            //
            //
            
            
            
            
            
            
        }
    }
}
}
