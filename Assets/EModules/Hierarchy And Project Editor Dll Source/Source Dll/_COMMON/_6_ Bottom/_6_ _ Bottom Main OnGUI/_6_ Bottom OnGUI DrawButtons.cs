
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


	static bool BottomIconSwap = true;
	
	
	internal partial class BottomInterface {
	
	
		/*     public  void EVENTS_UPDATE()
		     {
		         if (BottomInterface.HierarchyController.selection_action != null)
		         {
		             BottomInterface.HierarchyController.selection_action(false, deltaTime);
		             BottomInterface.HierarchyController.REPAINT();
		         }
		
		         foreach (var VARIABLE in Docka) {
		
		         }
		
		     }*/
		
		
		
		
		static List<Int32ListArray> list;
		TempColorClass tempColor = new TempColorClass();
		
		
		
		
		
		bool needRestore = false;
		Color c1, c2, c3, c4, c5, c6, c7, c8;
		internal void SetStyleColor( GUIStyle style, Color color )
		{	if ( needRestore ) throw new Exception( "SetStyleColor" );
		
			needRestore = true;
			c1 = style.normal.textColor;
			c2 = style.focused.textColor;
			c3 = style.active.textColor;
			c4 = style.hover.textColor;
			c5 = style.onNormal.textColor;
			c6 = style.onFocused.textColor;
			c7 = style.onActive.textColor;
			c8 = style.onHover.textColor;
			style.normal.textColor =
			    style.focused.textColor =
			        style.active.textColor =
			            style.hover.textColor =
			                style.onNormal.textColor =
			                    style.onFocused.textColor =
			                        style.onActive.textColor =
			                            style.onHover.textColor = color;
		}
		internal void RestoreStyleColor( GUIStyle style )
		{	if ( !needRestore ) return;
		
			needRestore = false;
			
			style.normal.textColor = c1;
			style.focused.textColor = c2;
			style.active.textColor = c3;
			style.hover.textColor = c4;
			style.onNormal.textColor = c5;
			style.onFocused.textColor = c6;
			style.onActive.textColor = c7;
			style.onHover.textColor = c8;
		}
		
		Rect lastDESrect;
		internal Rect GET_CELL_RECT( Rect cell, Rect line, MemType type, int interator, int itemscount, int countPerRow )
		{
		
			var lineIndex = interator / countPerRow;
			var currenItemsInLine = interator / countPerRow == itemscount / countPerRow ? itemscount % countPerRow : countPerRow;
			return __GET_CELL_RECT( cell, line, interator % countPerRow, lineIndex, currenItemsInLine, (itemscount - 1) / countPerRow + 1 );
		}
		Rect __GET_CELL_RECT( Rect cell, Rect line, int x, int y, int rightX, int rightY )
		{	var width = (line.width - SPACE * (rightX - 1)) / rightX; ;
			var height = (line.height - 2 * rightY) / rightY;
			var zerox = line.x + line.width - cell.width + 5;
			
			//* INVERSION *//
			switch ( adapter.par.BottomParams.SORT_LINES )
			{	case 1: x = rightX - 1 - x; break;
			
				case 0: break;
				
				case 3: x = rightX - 1 - x; y = rightY - 1 - y; break;
				
				case 2: y = rightY - 1 - y; break;
			}
			
			//* INVERSION *//
			
			
			cell.x = zerox + (width + SPACE) * x;
			cell.y = cell.y + (height + 2) * y;
			cell.width = width;
			
			
			
			return cell;
			
			/*  if (interator % countPerRow == 0)
			  {
			      lineIndex++;
			
			      cell.width = (line.width - SPACE * (currenItemsInLine - 1)) / currenItemsInLine;
			      cell.x = line.x + line.width - cell.width + 3;
			      zeroX = cell.x;
			
			      cell.x = zeroX;
			      if (interator != 0)
			      {
			          cell.y += cell.height + 2;
			      }
			  } else
			  {
			      cell.x -= cell.width + SPACE;
			      /* if (interator == countPerRow)
			       {
			       }#1#
			  }*/
		}
		
		
		ISET_ROW GetRowClass( MemType type )
		{	switch ( type )
			{	case MemType.Custom: return RowsParams[PLUGIN_ID.BOOKMARKS];
			
				case MemType.Last: return RowsParams[PLUGIN_ID.LAST];
				
				case MemType.Hier: return RowsParams[PLUGIN_ID.HIER];
				
				case MemType.Scenes: return RowsParams[PLUGIN_ID.SCENES];
			}
			
			return null;
		}
		
		
		
		void SHOW_STRING( string title, string s, Action<string> action, BottomController controller )
		{	if ( Event.current == null )     // adapter.__GUI_ONESHOT = true;
			{	adapter.GUI_ONESHOTPUSH( () =>
				{	_mSHOW_STRING( title, s, action, controller );
				} );
				return;
				//throw new Exception("Error imnput in hui");
			}
			
			_mSHOW_STRING( title, s, action, controller );
			
		}/** SHOW_STRING */
		void _mSHOW_STRING( string title, string s, Action<string> action,
		                    BottomController controller )        // var pos = InputData.WidnwoRect(false, Event.current.mousePosition, 190, 68, controller.adapter);
		{	var pos = new MousePos( Event.current.mousePosition, MousePos.Type.Input_190_68, false, controller.adapter);
			_W__InputWindow.Init( pos, title, controller.adapter, action, null, s );
		}
		
		
		
		
		GUIContent faveContent = new GUIContent();
		private int pickerId;
#pragma warning disable
		private Action<Object> pickerAction;
#pragma warning restore
		Rect DragRect;
		
		internal GUIContent categoryColorContent = new GUIContent() { text = "☰", tooltip = "Click - to change current category" };
		// internal GUIContent categoryColorContent = new GUIContent() { text = "☰", tooltip = "Click to change background color for category" };
		internal GUIContent categoryDescriptionContent = new GUIContent() { text = "i", tooltip = "Show all Descriptions in Hierarchy Window" };
		
		//   ;
		// private  void DrawButtons(Rect line, MemoryRoot[] memoryRoot, int idOffset, MemType type)
		private bool DrawButtons( Rect line, int __LH, MemType type, Color styleColor, BottomController controller, int scene )
		{	var SHOW_DES = (type == MemType.Custom
			                && (controller.IS_MAIN && adapter.par.BottomParams.SHOW_ALL_DESCRIPTIONS_INHIER || !controller.IS_MAIN && adapter.par.BottomParams.SHOW_ALL_DESCRIPTIONS_INWIN));
			                
			var idOffset = IDOFFSET(type);
			
			adapter.bottomInterface.GET_BOOKMARKS( ref list, scene );
			
			if ( !m_memCache.ContainsKey( type ) || m_memCache[type].Length == 0 ) return false;
			
			var CUSTOM_DRAWER = type == MemType.other;
			
			var memoryRoot = type == MemType.Custom ? m_memCache[type][Mathf.Clamp(controller.GetCategoryIndex(scene), 0, m_memCache[type].Length - 1)] : m_memCache[type][0];
			
			var rowClass = GetRowClass(type);
			
			var itemscount = memoryRoot.Count(t => t.IsValid());
			
			if ( itemscount <= 0 ) return false;
			
			if ( CUSTOM_DRAWER ) itemscount++;
			
			var ROWS_COUNT = rowClass.Rows;
			
			if ( itemscount > rowClass.MaxItems ) itemscount = rowClass.MaxItems;
			
			var COUNT_PER_ROW = 0;
			
			if ( type == MemType.Custom )
				COUNT_PER_ROW = Mathf.CeilToInt( rowClass.MaxItems / (float)ROWS_COUNT );
			else
				COUNT_PER_ROW = Mathf.CeilToInt( itemscount / (float)ROWS_COUNT );
				
				
			/* var ROWS_COUNT = rowClass.Rows;
			 var COUNT_PER_ROW = Mathf.CeilToInt( rowClass.MaxItems / (float)ROWS_COUNT);
			 if (itemscount > COUNT_PER_ROW * ROWS_COUNT) itemscount = COUNT_PER_ROW * ROWS_COUNT;
			 if (itemscount <= 0) return false;*/
			
			/* var _MAX_MULTY = MAX_MULTY(type);
			 if (itemscount > MAXCOUNT(type) * _MAX_MULTY) itemscount = MAXCOUNT(type) * _MAX_MULTY;
			 if (itemscount <= 0) return false;
			
			 var countPerRow = MAXCOUNT(type);*/
			
			var WIDTH = controller.WIDTH;
			var __cell = line;
			// var shrink = 0;
			//  if (type == MemType.Scenes) shrink = 1;
			line.width -= 3;
			__cell.y += 2;
			/*  cell.width = (line.width - SPACE * (countPerRow - 1)) / countPerRow;
			  cell.x = line.x + line.width - cell.width + 3;
			
			  var zeroX = cell.x;*/
			//  var zeroX = -1f;
			var maxLines = Mathf.CeilToInt(itemscount / (float)COUNT_PER_ROW);
			
			/*  if (type == MemType.Custom && (controller.IS_MAIN && par.BottomParams.SHOW_ALL_DESCRIPTIONS_INHIER || !controller.IS_MAIN && par.BottomParams.SHOW_ALL_DESCRIPTIONS_INWIN))
			      __LH = (int)(__LH - DESCRIPTION_MULTY(controller.IS_MAIN) / ((_MAX_MULTY + 1f) / maxLines));*/
			
			__cell.height = __LH * ROWS_COUNT / maxLines;
			
			if ( type == MemType.Custom && (controller.IS_MAIN && adapter.par.BottomParams.SHOW_ALL_DESCRIPTIONS_INHIER || !controller.IS_MAIN
			                                && adapter.par.BottomParams.SHOW_ALL_DESCRIPTIONS_INWIN) )     // __LH = (int)(__LH - DESCRIPTION_MULTY(controller.IS_MAIN) / ((_MAX_MULTY + 1f) / maxLines));
			{	__LH = (int)(__LH - DESCRIPTION_MULTY( controller.IS_MAIN ));
				__cell.height -= DESCRIPTION_MULTY( controller.IS_MAIN );
			}
			
			__cell.height -= 2;
			
			bool ? wasSelect = null;
			
			var defaultCell = __cell;
			//  if (type == MemType.Scenes) cell.height -= 1;
			var wasDraw = false;
			var interator = 0;
			
			//var lineIndex = -1;
			// var FirstFav = true;
			
			
			
			// if (type == MemType.Custom) Debug.Log( interator + " " + itemscount + " " + COUNT_PER_ROW );
			
			for ( int __i = 0 ; __i < memoryRoot.Count && interator < itemscount ; __i++ )
			{	var i = __i;
				bool DisableCursor = false;
				
				//  if (type == MemType.Custom) Debug.Log( "ASD" );
				
				/* if (par.BottomParams.SORT_LINES == 0)
				 {
				     var t = __i % itemscount;
				     var c = (memoryRoot.Count - 1) % itemscount;
				     if ((__i / itemscount) == ((memoryRoot.Count - 1) / itemscount)) i = c - t;
				     else i = itemscount - t - 1;
				     i += (__i / itemscount) * itemscount;
				 }*/
				// Adapter.GET_SKIN().button.Draw(cell, new GUIContent("Object" + (i + 1)), false, false, false, false);
				
				
				if (/*!CUSTOM_DRAWER && */!memoryRoot[i].IsValid() )
				{	/* if (type == MemType.Custom) {
					         MonoBehaviour.print(i + " " + count);
					     }*/
					continue;
				}
				
				
				var h = type == MemType.Last || type == MemType.Custom ? INT32__ACTIVE_TOHIERARCHYOBJECT(memoryRoot[i].InstanceID) : null;
				
				if ( (type == MemType.Last || type == MemType.Custom) && (h == null || !h.Validate()) ) continue;
				
				
				/* var cell = __cell;
				 var currenItemsInLine = interator / countPerRow == itemscount / countPerRow ? itemscount % countPerRow : countPerRow;
				 if (interator % countPerRow == 0)
				 {
				     lineIndex++;
				
				      currenItemsInLine = interator / countPerRow == itemscount / countPerRow ? itemscount % countPerRow : countPerRow;
				     cell.width = (line.width - SPACE * (currenItemsInLine - 1)) / currenItemsInLine;
				     cell.x = line.x + line.width - cell.width + 3;
				     zeroX = cell.x;
				
				     cell.x = zeroX;
				     if (interator != 0)
				     {
				         cell.y += cell.height + 2;
				     }
				 } else
				 {
				     cell.x -= cell.width + SPACE;
				     /* if (interator == countPerRow)
				      {
				      }#1#
				 }*/
				
				var lineIndex = interator / COUNT_PER_ROW;
				
				/* var currenItemsInLine = interator / countPerRow == itemscount / countPerRow ? itemscount % countPerRow : countPerRow;
				 var cell = GET_CELL_RECT(__cell, line, interator % countPerRow, lineIndex, currenItemsInLine, (itemscount - 1) / countPerRow + 1);*/
				var cell = GET_CELL_RECT(__cell, line, type, interator, itemscount, COUNT_PER_ROW);
				
				++interator;
				
				
				var contains = cell.Contains(Event.current.mousePosition);
				
				if ( memoryRoot[i].IsSelectablePlus() || memoryRoot[i].IsSelectableMinus() )
				{	var STATE = memoryRoot[i].GET_SELECTION_STATE();
				
					if ((type == MemType.Last || type == MemType.Custom ) && adapter.SHIFT_TO_INSTANTIATE_BOTTOM && Event.current.shift) STATE = 10;
					
					if ( STATE != 0 && type != MemType.Scenes )
					{	/*if (type == MemType.Scenes) {
						      if (STATE == 1) STATE = 0;
						      if ( STATE == 2 ) STATE = 1;
						    }*/
						if (STATE == 10)EditorGUIUtility.AddCursorRect(cell,  MouseCursor.FPS );
						else EditorGUIUtility.AddCursorRect( cell, STATE == 1 ? MouseCursor.ArrowPlus : STATE == 2 ? MouseCursor.ArrowMinus : MouseCursor.ScaleArrow );
						
						DisableCursor = true;
					}
					
					if ( type == MemType.Scenes )
					{	if ( Event.current.shift /*|| Event.current.alt*/) EditorGUIUtility.AddCursorRect( cell, MouseCursor.ArrowPlus );
					
						if ( Event.current.control ) EditorGUIUtility.AddCursorRect( cell, MouseCursor.Zoom );
						
						DisableCursor = true;
					}
					
					/* bool selected = false;
					 if (Event.current.control)
					 {
					   selected = memoryRoot[i].IsSelectedHadrScan();
					 }
					
					 bool mayMinus = false;
					
					 if (memoryRoot[i].IsSelectablePlus())
					 {
					   if ((Event.current.control && !selected || Event.current.shift))
					   {
					     EditorGUIUtility.AddCursorRect( cell, MouseCursor.ArrowPlus );
					     DisableCursor = true;
					
					   }
					   else
					   {
					     mayMinus = true;
					   }
					 }
					
					
					 if (memoryRoot[i].IsSelectableMinus() && mayMinus)
					 {
					   if (Event.current.alt || Event.current.control && selected)
					   {
					     EditorGUIUtility.AddCursorRect( cell, MouseCursor.ArrowMinus );
					     DisableCursor = true;
					   }
					 }*/
					
					
				}
				
				
				
				
				if ( CUSTOM_DRAWER )
				{	var buttonRect = cell;
					//  if (SHOW_DES) buttonRect.height += DESCRIPTION_MULTY(controller.IS_MAIN) - 3;
					DRAW_CATEGORY( buttonRect, controller, scene );
					
					
					
					CUSTOM_DRAWER = false;
					__i--;
					continue;
				}
				
				
				
				
				wasDraw = true;
				
				if ( SHOW_DES )
				{	var LH = Math.Min(__LH, (int)cell.width / 3);
					lastDESrect.x = cell.x + LH / 2;
					lastDESrect.width = cell.width - LH / 2;
					lastDESrect.y = cell.y + cell.height;
					lastDESrect.height = DESCRIPTION_MULTY( controller.IS_MAIN ) - 3;
					
					/*if (lastDESrect.Contains(Event.current.mousePosition)) */
					EditorGUIUtility.AddCursorRect( lastDESrect, MouseCursor.ArrowPlus );
				}
				
				// Debug.Log(lastDESrect.Contains(Event.current.mousePosition));
				if ( Event.current.type == EventType.MouseDown && SHOW_DES && lastDESrect.Contains( Event.current.mousePosition ) )
				{
				
				
					if ( Event.current.button == 0 )     // Debug.Log("ASD");
					{	controller.selection_button = idOffset + i + 100;
						controller.selection_window = controller.REFERENCE_WINDOW;
						controller.lastRect = lastDESrect;
						var captureCell = lastDESrect;
						var captureI = i;
						//  var arrayIndex = memoryRoot[i].ArrayIndex;
						var pos = new MousePos(Event.current.mousePosition, MousePos.Type.Input_190_68, false, adapter);
						
						// var  pos = InputData.WidnwoRect( false, Event.current.mousePosition, 190, 68, adapter  );
						controller.selection_action = ( mouseUp, deltaTIme ) =>
						{	if ( mouseUp && captureCell.Contains( Event.current.mousePosition ) )
							{	if ( !memoryRoot[captureI].IsValid() || memoryRoot[captureI].InstanceID == null
								        || !INT32_ISVALID( memoryRoot[captureI].InstanceID )
								   ) return false;
								   
								   
								var d = adapter.MOI.des(INT32_SCENE(memoryRoot[captureI].InstanceID));
								
								if ( d == null ) return false;
								
								adapter.DescriptionModule.CREATE_NEW_ESCRIPTION( adapter, pos, INT32__ACTIVE_TOHIERARCHYOBJECT( memoryRoot[captureI].InstanceID ), scene, true );
								
								//  RemoveAndRefresh(type, arrayIndex);
							}
							
							return Event.current.delta.x == 0 && Event.current.delta.x == 0;
							
						}; // ACTION
					} //if button
				}
				
				else
				
					if ( contains && Event.current.type == EventType.MouseDown )
					{	var LH = Math.Min(__LH, (int)cell.width / 3);
					
						if ( Event.current.button == 0 && type == MemType.Scenes && Event.current.mousePosition.x > cell.x + cell.width - LH )
						{	controller.selection_button = 1000000 + i;
							var r = controller.selection_button;
							controller.selection_window = controller.REFERENCE_WINDOW;
							var captureCell = cell;
							//  var captureI = i;
							var arrayIndex = memoryRoot[i].ArrayIndex;
							controller.selection_action = ( mouseUp, deltaTIme ) =>
							{	var cc = captureCell.Contains(Event.current.mousePosition) && Event.current.mousePosition.x > captureCell.x + cell.width - captureCell.height;
							
								if ( cc ) controller.selection_button = r;
								else controller.selection_button = -1;
								
								if ( mouseUp && cc )
								{	if ( arrayIndex < Hierarchy_GUI.GetLastScenes( adapter ).Count && Hierarchy_GUI.GetLastScenes( adapter )[arrayIndex] != null )
									{	Undo.RecordObject( Hierarchy_GUI.Instance( adapter ), Hierarchy_GUI.GetLastScenes( adapter )[arrayIndex].pin ? "UnPin Scene" : "Pin Scene" );
										Hierarchy_GUI.GetLastScenes( adapter )[arrayIndex].pin = !Hierarchy_GUI.GetLastScenes( adapter )[arrayIndex].pin;
										Hierarchy_GUI.SetDirtyObject( adapter );
										RefreshMemCache( scene );
										ClearAction();
									}
								}
								
								return Event.current.delta.x == 0 && Event.current.delta.x == 0;
								
							}; // ACTION
						}
						
						else
						
							if ( Event.current.button == 0 )
							{	controller.selection_button = idOffset + i;
								controller.selection_window = controller.REFERENCE_WINDOW;
								var captureCell = cell;
								var captureI = i;
								// var pos = Event.current.mousePosition;
								
								
								var startRect = cell;
								var startMouse = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
								var startX = startRect.x;
								var startY = startRect.y;
								controller.wasDrag = false;
								
								var arrayIndex = memoryRoot[i].ArrayIndex;
								var backupArraIndex = arrayIndex;
								var captureType = type;
								
								var captureLine = lineIndex;
								var LineMin = GUIUtility.GUIToScreenPoint(new Vector2(cell.x, cell.y)).y;
								var LineMax = GUIUtility.GUIToScreenPoint(new Vector2(cell.x + cell.width, cell.y + cell.height)).y;
								
								if ( SHOW_DES ) LineMax += DESCRIPTION_MULTY( controller.IS_MAIN );
								
								var LineDif = LineMax - LineMin + 2;
								var swap = adapter.par.BottomParams.SORT_LINES > 1;
								
								if ( swap ) LineDif = -LineDif;
								
								var startLineMin = LineMin;
								
								var r_info = GET_INFO_RECT(type, cell);
								var infoContains = type == MemType.Custom && !SHOW_DES && adapter.FAVORITS_SHOWDESICON && r_info.Contains(Event.current.mousePosition);
								controller.selection_info = infoContains;
								
								controller.selection_action = ( mouseUp, deltaTIme ) =>      /////////////////////////////
								{	//  var y = -(int)startMouse.y + (int)GUIUtility.GUIToScreenPoint(Event.current.mousePosition).y + startY;
								
								
								
								
								
								
								
								
								
								
									/*  var max = captureType == MemType.Last ?
									  M_Descript.des(s).GetHash3().Count : captureType == MemType.Custom ? M_Descript.des(s).GetHash4().Count : Hierarchy_GUI.GetLastScenes().Count;
									  if (max > m_memCache[captureType].Count) max = m_memCache[captureType].Count;
									
									  var min = captureLine * countPerRow;
									  max = Math.Min(countPerRow, max) + min;
									
									  var upnext = arrayIndex;
									  var cached_upnext = upnext;
									  while (cached_upnext < max - 1)
									  {
									      cached_upnext++;
									      if (!m_memCache[captureType][cached_upnext].IsValid()) continue;
									      upnext = cached_upnext;
									      break;
									  }
									
									  var downnext = arrayIndex;
									  var cached_downnext = downnext;
									  while (cached_downnext > min)
									  {
									      cached_downnext--;
									      if (!m_memCache[captureType][cached_downnext].IsValid()) continue;
									      downnext = cached_downnext;
									      break;
									  }
									
									
									  var next = arrayIndex;
									  if (upnext != arrayIndex && DragRect.x < startRect.x - startRect.width * 0.8f) next = upnext;
									  if (downnext != arrayIndex && DragRect.x > startRect.x + startRect.width * 0.8f) next = downnext;
									
									  if (wasDrag)
									  {
									      var y = (int)GUIUtility.GUIToScreenPoint(Event.current.mousePosition).y;
									      // MonoBehaviour.print(y + " " + LineMax);
									      if (y > LineMax)
									      {
									          if (captureLine < maxLines - 1)
									          {
									              var wasMove = 0;
									              var ind = next;
									              while (++ind < m_memCache[captureType].Count)
									              {
									                  if (!m_memCache[captureType][ind].IsValid()) continue;
									                  wasMove++;
									                  if (wasMove == countPerRow)
									                  {
									                      captureLine++;
									                      LineMin += LineDif;
									                      LineMax += LineDif;
									                      next = ind;
									                  }
									              }
									          }
									      }
									      if (y < LineMin)
									      {
									          if (captureLine > 0)
									          {
									              var wasMove = 0;
									              var ind = next;
									              while (--ind >= 0)
									              {
									                  if (!m_memCache[captureType][ind].IsValid()) continue;
									                  wasMove++;
									                  if (wasMove == countPerRow)
									                  {
									                      captureLine--;
									                      LineMin -= LineDif;
									                      LineMax -= LineDif;
									                      next = ind;
									                  }
									              }
									          }
									      }
									  }*/
									
									
									// var scene = EditorSceneManager.GetActiveScene();
									
									var next = arrayIndex;
									
									if ( controller.wasDrag )
									{
									
										var interator_2 = CUSTOM_DRAWER ? 1 : 0;
										// var LINE = -1;
										var __rect = defaultCell;
										
										for ( int index = 0 ; index < memoryRoot.Count && interator_2 < itemscount ; index++ )     // Adapter.GET_SKIN().button.Draw(cell, new GUIContent("Object" + (i + 1)), false, false, false, false);
										{
										
										
											if ( !memoryRoot[index].IsValid() )
											{	/* if (type == MemType.Custom) {
												                 MonoBehaviour.print(i + " " + count);
												             }*/
												continue;
											}
											
											/* if (interator_2 % countPerRow == 0)
											 {
											     LINE++;
											
											     currenItemsInLine = interator_2 / countPerRow == itemscount / countPerRow ? itemscount % countPerRow : countPerRow;
											     rect.width = (line.width - SPACE * (currenItemsInLine - 1)) / currenItemsInLine;
											     rect.x = line.x + line.width - rect.width + 3;
											     zeroX = rect.x;
											
											     rect.x = zeroX;
											     if (interator_2 != 0)
											     {
											         rect.y += rect.height + 2;
											     }
											 } else
											 {
											     rect.x -= rect.width + SPACE;
											     /* if (interator == countPerRow)
											      {
											      }#1#
											 }*/
											var LINE = interator_2 / COUNT_PER_ROW;
											var rect = GET_CELL_RECT(__rect, line, type, interator_2, itemscount, COUNT_PER_ROW);
											
											++interator_2;
											
											var p1 = GUIUtility.GUIToScreenPoint(new Vector2(rect.x, rect.y));
											var p2 = GUIUtility.GUIToScreenPoint(new Vector2(rect.x + rect.width, rect.y + rect.height));
											var worldRect = new Rect(p1.x, p1.y, p2.x - p1.x, p2.y - p1.y);
											
											//  MonoBehaviour.print(Event.current.mousePosition + " " +worldRect.Contains(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)) + " " + worldRect + " " + LINE + "  " + captureLine + "  " + interator_2);
											if ( worldRect.Contains( GUIUtility.GUIToScreenPoint( Event.current.mousePosition ) ) && memoryRoot[index].ArrayIndex != next )     //Debug.Log(memoryRoot[index].ArrayIndex + " " + next);
											{	next = memoryRoot[index].ArrayIndex;
											
												if ( LINE > captureLine )
												{	LineMin += LineDif;
													LineMax += LineDif;
												}
												
												if ( LINE < captureLine )
												{	LineMin -= LineDif;
													LineMax -= LineDif;
												}
												
												captureLine = LINE;
												startRect.width = rect.width;
												break;
											}
										}
									}
									
									DragRect = startRect;
									DragRect.x = -(int)startMouse.x + (int)GUIUtility.GUIToScreenPoint( Event.current.mousePosition ).x + startX;
									DragRect.y = startRect.y + LineMin - startLineMin;
									
									if ( controller.wasDrag && next != arrayIndex )     // var target = memoryRoot[next];
									{
									
										/*;*/
										
										switch ( captureType )
										{	case MemType.Last:
											{	var l1 = adapter.MOI.des(scene).GetHash3();
												var b = l1[arrayIndex];
												l1.RemoveAt( arrayIndex );
												
												if ( next >= l1.Count ) l1.Add( b );
												else l1.Insert( next, b );
											}
											
												//  Utilities.Swap(ref l1, arrayIndex, backupArraIndex);
												//  Utilities.Swap(ref l1, backupArraIndex, next);
											break;
											
											case MemType.Custom:
											{	List<Int32List> l1 = controller.GetCategoryIndex(scene) == 0 ? adapter.MOI.des(scene).GetHash4() : adapter.MOI.des(scene).GetBookMarks()[controller.GetCategoryIndex(scene)].array;
											
												// var l1 = adapter.MOI.des(s).GetHash4();
												
												adapter.CreateUndoActiveDescription( "Move Favorite", scene );
												
												var b = l1[arrayIndex];
												l1.RemoveAt( arrayIndex );
												
												if ( next >= l1.Count ) l1.Add( b );
												else l1.Insert( next, b );
												
												
												
												
												
												adapter.SetDirtyActiveDescription( scene );
											}
												//  Utilities.Swap(ref l2, arrayIndex, backupArraIndex);
												//  Utilities.Swap(ref l2, backupArraIndex, next);
											break;
											
											case MemType.Hier:
											{	adapter.CreateUndoActiveDescription( "Move Hierarchy SLot", scene );
											
												var l1 = adapter.MOI.des(scene).HierarchyCache();
												
												var b = l1[arrayIndex];
												l1.RemoveAt( arrayIndex );
												
												if ( next >= l1.Count ) l1.Add( b );
												else l1.Insert( next, b );
												
												adapter.SetDirtyActiveDescription( scene );
											}
												//  Utilities.Swap(ref getList, arrayIndex, backupArraIndex);
												//  Utilities.Swap(ref getList, backupArraIndex, next);
											break;
											
											case MemType.Scenes:
											{	adapter.CreateUndoActiveDescription( "Move Scene", scene );
											
												var l1 = Hierarchy_GUI.GetLastScenes(adapter);
												var b = l1[arrayIndex];
												l1.RemoveAt( arrayIndex );
												
												if ( next >= l1.Count ) l1.Add( b );
												else l1.Insert( next, b );
												
												adapter.SetDirtyActiveDescription( scene );
											}
												//  Utilities.Swap(ref getList, arrayIndex, backupArraIndex);
												//  Utilities.Swap(ref getList, backupArraIndex, next);
											break;
											
											default:
												throw new ArgumentOutOfRangeException( "type", captureType, null );
										}
										
										if ( next > arrayIndex ) startRect.x -= startRect.width + SPACE;
										else startRect.x += startRect.width + SPACE;
										
										arrayIndex = next;
										
										
										RefreshMemCache( scene );
										controller.REPAINT( adapter );
										
										//Debug.Log( adapter.bottomInterface.FavoritControllers.Count );
										
										foreach ( var w in adapter.bottomInterface.WindowController )
											if ( w.REFERENCE_WINDOW ) w.REFERENCE_WINDOW.Repaint();
											
										foreach ( var w in adapter.bottomInterface.FavoritControllers )
											if ( w.REFERENCE_WINDOW ) w.REFERENCE_WINDOW.Repaint();
											
										/*  foreach (var w in adapter.bottomInterface.WindowController)
										  {
										    if (w.REFERENCE_WINDOW) w.REFERENCE_WINDOW.Repaint();
										  }
										  foreach (var w in adapter.bottomInterface.FavoritControllers)
										  {
										    if (w.REFERENCE_WINDOW) w.REFERENCE_WINDOW.Repaint();
										  }*/
										// RepaintWindowInUpdate();
										
									}
									
									
									
									if ( Math.Abs( startMouse.x - (int)GUIUtility.GUIToScreenPoint( Event.current.mousePosition ).x ) > 5
									        || Math.Abs( startMouse.y - (int)GUIUtility.GUIToScreenPoint( Event.current.mousePosition ).y ) > 5 )
									{	if ( !infoContains ) controller.wasDrag = true;
									}
									
									/// /////////////////////////
									
									
									
									if ( !controller.wasDrag && mouseUp && captureCell.Contains( Event.current.mousePosition ) )
									{	if ( infoContains )
										{
										
											if ( !memoryRoot[captureI].IsValid() || memoryRoot[captureI].InstanceID == null || !INT32_ISVALID( memoryRoot[captureI].InstanceID ) ) return false;
											
											
											//	var o = memoryRoot[captureI].InstanceID.ActiveGameObject;
											//var d = adapter.MOI.des( o.scene );
											// var scene = INT32_SCENE(memoryRoot[captureI].InstanceID);
											var d = adapter.MOI.des(INT32_SCENE(memoryRoot[captureI].InstanceID));
											
											if ( d == null ) return false;
											
											//adapter.MOI.CREATE_NEW_ESCRIPTION( memoryRoot[captureI].InstanceID.ActiveGameObject , d , true );
											// var  pos = InputData.WidnwoRect( false, Event.current.mousePosition, 190, 68, adapter  );
											var pos = new MousePos( Event.current.mousePosition, MousePos.Type.Input_190_68, false, adapter);
											adapter.DescriptionModule.CREATE_NEW_ESCRIPTION( adapter, pos, INT32__ACTIVE_TOHIERARCHYOBJECT( memoryRoot[captureI].InstanceID ), scene, true );
											
										}
										
										else
										{	if ( !memoryRoot[captureI].IsValid() ) return false;
										
											if ( !memoryRoot[captureI].OnClick( false, scene ) )
											{	RemoveAndRefresh( type, arrayIndex, controller.GetCategoryIndex( scene ), scene );
											}
											
											else
											{	if ( type == MemType.Custom ) LastIndex = -1;
											}
										}
										
									}
									
									else
										if ( Event.current.keyCode == KeyCode.Escape )
										{	adapter.SKIP_PREFAB_ESCAPE = true;
										
											// List<Int32List> targetList = null;
											switch ( captureType )
											{	case MemType.Last:
												{	var targetList = adapter.MOI.des( scene ).GetHash3();
													Utilities.MoveFromTo( ref targetList, arrayIndex, backupArraIndex );
													RefreshMemCache( scene );
												}
												
												break;
												
												case MemType.Custom:
												{	var targetList = controller.GetCategoryIndex( scene ) == 0 ? adapter.MOI.des( scene ).GetHash4() : adapter.MOI.des( scene ).GetBookMarks()[controller.GetCategoryIndex( scene )].array;
													Utilities.MoveFromTo( ref targetList, arrayIndex, backupArraIndex );
													RefreshMemCache( scene );
												}
												
													//  targetList = adapter.MOI.des(s).GetHash4();
												break;
												
												case MemType.Hier:
												{	var targetList = adapter.MOI.des(scene).HierarchyCache();
													Utilities.MoveFromTo( ref targetList, arrayIndex, backupArraIndex );
													RefreshMemCache( scene );
												}
													//  Utilities.Swap(ref getList, arrayIndex, backupArraIndex);
													//  Utilities.Swap(ref getList, backupArraIndex, next);
												break;
												
												case MemType.Scenes:
												{	var targetList = Hierarchy_GUI.GetLastScenes(adapter);
													Utilities.MoveFromTo( ref targetList, arrayIndex, backupArraIndex );
													RefreshMemCache( scene );
													
												}
													//  Utilities.Swap(ref getList, arrayIndex, backupArraIndex);
													//  Utilities.Swap(ref getList, backupArraIndex, next);
												break;
												
												default:
													throw new ArgumentOutOfRangeException( "type", captureType, null );
											}
											
											
											EventUse();
											ClearAction();
											controller.REPAINT( adapter );
										}
										
										else
											if ( (type == MemType.Last || type == MemType.Custom || type == MemType.Scenes) )
											{	var m = Event.current.mousePosition + Event.current.delta;
												var drag =  (!controller.ModuleRect.Contains( m ) || type == MemType.Last && controller.CustomLineRect.Contains( m ));
												drag |= m.x < 3;
												drag |= m.x > WIDTH - 9;
												// Debug.Log( m.x + " " + WIDTH );
												
												if ( (!infoContains) && Event.current.type == EventType.MouseDrag && !Event.current.control && !Event.current.shift && !Event.current.alt &&
												        drag )
												{	switch ( captureType )
													{	case MemType.Last:
														{	var  targetList = adapter.MOI.des( scene ).GetHash3();
															Utilities.MoveFromTo( ref targetList, arrayIndex, backupArraIndex );
															RefreshMemCache( scene );
														}
														break;
														
														case MemType.Custom:
														{	var targetList = controller.GetCategoryIndex( scene ) == 0 ? adapter.MOI.des( scene ).GetHash4() : adapter.MOI.des( scene ).GetBookMarks()[controller.GetCategoryIndex( scene )].array;
															Utilities.MoveFromTo( ref targetList, arrayIndex, backupArraIndex );
															RefreshMemCache( scene );
														}
														
															//  targetList = adapter.MOI.des(s).GetHash4();
														break;
														
														case MemType.Hier:
														{	var targetList = adapter.MOI.des(scene).HierarchyCache();
															Utilities.MoveFromTo( ref targetList, arrayIndex, backupArraIndex );
															RefreshMemCache( scene );
														}
															//  Utilities.Swap(ref getList, arrayIndex, backupArraIndex);
															//  Utilities.Swap(ref getList, backupArraIndex, next);
														break;
														
														case MemType.Scenes:
														{	var targetList = Hierarchy_GUI.GetLastScenes(adapter);
															Utilities.MoveFromTo( ref targetList, arrayIndex, backupArraIndex );
															RefreshMemCache( scene );
														}
														break;
														
														default:
															throw new ArgumentOutOfRangeException( "type", captureType, null );
													}
													
													
													
													if ( type == MemType.Scenes )
													{	var targetList = Hierarchy_GUI.GetLastScenes(adapter);
														List<string> guidlist = new List<string>();
														guidlist.Add( targetList[backupArraIndex].guid );
														guidlist.AddRange( targetList[backupArraIndex].additional_guid );
														var path = guidlist.Select(g => AssetDatabase.GUIDToAssetPath(g)).Where(g => !string.IsNullOrEmpty(g));
														SetDragData( path .Select(p => AssetDatabase.LoadAssetAtPath<SceneAsset>(p)).Where(s => s).ToArray(), type );
														DragAndDrop.StartDrag( "Dragging Scenes" );
													}
													
													else
														if ( type == MemType.Last )
														{	var  targetList = adapter.MOI.des( scene ).GetHash3();
															SetDragData( INT32_TOOBJECTASLISTCT( targetList[backupArraIndex] ), type );
															DragAndDrop.StartDrag( "Dragging GameObject" );
														}
														
														else
															if ( type == MemType.Custom )
															{	var targetList = controller.GetCategoryIndex( scene ) == 0 ? adapter.MOI.des( scene ).GetHash4() : adapter.MOI.des( scene ).GetBookMarks()[controller.GetCategoryIndex( scene )].array;
																SetDragData( INT32_TOOBJECTASLISTCT( targetList[backupArraIndex] ), type );
																DragAndDrop.StartDrag( "Dragging GameObject" );
															}
															
															
													EventUse();
													
													ClearAction();
													controller.REPAINT( adapter );
												}
											}
											
											
											
									return Event.current.delta.x == 0 && Event.current.delta.x == 0;
									
									
									// pos += Event.current.delta;
								}; //ACTION
							}
							
							else
								if ( Event.current.button == 1 )
								{	controller.selection_button = idOffset + i;
									controller.selection_window = controller.REFERENCE_WINDOW;
									var captureCell = cell;
									//  var captureI = i;
									var arrayIndex = memoryRoot[i].ArrayIndex;
									controller.selection_action = ( mouseUp, deltaTIme ) =>
									{	if ( mouseUp && captureCell.Contains( Event.current.mousePosition ) )
										{	RemoveAndRefresh( type, arrayIndex, controller.GetCategoryIndex( scene ), scene );
										}
										
										return Event.current.delta.x == 0 && Event.current.delta.x == 0;
										
									}; // ACTION
								} //if button
					} //contains
					
				if ( Event.current.type == EventType.Repaint )
				{	var style = refStyle;
				
					var oldColor = GUI.color;
					GUI.color *= refColor;
					var oldA = style.alignment;
					style.alignment = /*type == MemType.Scenes ? TextAnchor.MiddleCenter :*/ TextAnchor.MiddleLeft;
					/*  var oldM = style.margin;
					  style.margin = new RectOffset(0,0,0,0);*/
					var oldB = style.border;
					/* var BR = 5;
					 style.border = new RectOffset(BR, BR, BR, BR);*/
					/* if (style.border.top > 8) style.border.top = 8;
					 if (style.border.bottom > 8) style.border.bottom = 8;*/
					
					var oldF = style.fontSize;
					style.fontSize = adapter.FONT_8();
					var padl = style.padding.left;
					var padt = style.padding.top;
					var over = style.wordWrap;
					style.wordWrap = false;
					style.padding.top = type == MemType.Last ? 3 : 0;
					content.text = memoryRoot[i].ToString();
					var ymon = style.fixedHeight;
					style.fixedHeight = 0;
					
					var r_info = GET_INFO_RECT(type, cell);
					var infoContains = type == MemType.Custom && !SHOW_DES && adapter.FAVORITS_SHOWDESICON && r_info.Contains(Event.current.mousePosition);
					
					// style.stretchHeight = true;
					
					var cellRectContains = cell.Contains(Event.current.mousePosition);
					var cellAction = controller.selection_window == controller.REFERENCE_WINDOW && controller.selection_button == idOffset + i;
					var active = cellAction && !controller.selection_info && cellRectContains && !infoContains;
					
					
					
					Color iconColor = Color.white;
					Texture icon = null;
					
					if ( type == MemType.Scenes )
					{
					
					
						var checkPin = controller.selection_window == controller.REFERENCE_WINDOW && controller.selection_button == 1000000 + i;
						
						if ( checkPin && !memoryRoot[i].pin )
						{	icon = adapter.GetIcon( "BOTTOM_SCENE_DOWN" );
						}
						
						else
							if ( memoryRoot[i].pin )
							{	icon = adapter.GetIcon( "BOTTOM_SCENE_ACTIVE" );
							}
							
							else
							{	icon = adapter.GetIcon( "SCENE" );
							}
							
						var guid_equals = lastSceneGUID == memoryRoot[i].GUID;
						
						if ( guid_equals )
						{	if ( memoryRoot[i].additional_GUID != null && memoryRoot[i].additional_GUID.Length != 0 )     //Debug.Log( lastSceneGUID_ALL[1] + " " + memoryRoot[i].additional_GUID[0] );
							{
							
								if ( memoryRoot[i].additional_GUID.Length != lastSceneGUID_ALL.Length - 1 ) guid_equals = false;
								
								if ( guid_equals ) for ( int b = 0, length = memoryRoot[i].additional_GUID.Length ; b < length ; b++ )
									{	if ( !lastSceneGUID_ALL.Contains( memoryRoot[i].additional_GUID[b] ) )
										{	guid_equals = false;
											break;
										}
									}
							}
							
							//else if (lastSceneGUID_ALL.Length != 0) guid_equals = false;
						}
						
						active |= guid_equals;
					}
					
					else
						if ( type == MemType.Hier )
						{	icon = adapter.GetIcon( "NEW_BOTTOM_HIERARCHY ICON" );
						}
						
						else
						{
						
						
							//var h = INT32__ACTIVE_TOHIERARCHYOBJECT(memoryRoot[i].InstanceID);
							var context = GetContent( h );
							//if (context != null)
							{	icon = context.add_icon;
							
								if ( icon && context.add_hasiconcolor )     // iconColor = adapter.MOI.M_Colors.GetColorForObject( h ); //#COLUP
								{	iconColor = context.add_iconcolor;
								}
								
								if ( adapter.pluginID == 0 && adapter.HAS_LABEL_ICON() )
								{	if ( !icon ) icon = EditorGUIUtility.ObjectContent( h.go, adapter.t_GameObject ).image;
								}
							}
							
							
							/* else
							              {
							                  if (type == MemType.Custom)
							                  {
							                      // wasSceneDraw = true;
							                      icon = global::EModules.EModulesInternal.Hierarchy.GetIcon("STAR");
							                  }
							              }*/
						}
						
						
						
					// FirstFav = false;
					
					Rect drawCell = cell;
					/*
					 var rectIndex = memoryRoot[i].RectBindIndex;
					 if (cell.x != m_memPosition[type][rectIndex].x) {
					     m_memPosition[type][rectIndex].x = Mathf.MoveTowards(m_memPosition[type][rectIndex].x, cell.x, deltaTime * 300);
					     m_memPosition[type][rectIndex].y = cell.y;
					     m_memPosition[type][rectIndex].width = cell.width;
					     m_memPosition[type][rectIndex].height = cell.height;
					     drawCell = m_memPosition[type][rectIndex];
					     RepaintAllViewInUpdate();
					 } else
					{
					  drawCell = cell;
					} */
					
					
					
					
					if ( type == MemType.Custom && !icon )     // wasSceneDraw = true;
					{	//icon = FirstFav ? adapter.GetIcon("FAV"): adapter.GetIcon("STAR");
					
						if ( BottomIconSwap )
							icon = adapter.GetIcon( "FAV" );
						else
							icon = adapter.GetIcon( "STAR" );
							
					}
					
					tempColor = null;
					///////////////////////////////////////
					///////////////////////////////////////
					///////////////////////////////////////
					////// SYNCHRONIZATION FAVORITE ///////
					var drawCount = (type == MemType.Last || type == MemType.Custom) && (INT32_COUNT(memoryRoot[i].InstanceID) > 1) || type == MemType.Scenes && memoryRoot[i].additional_GUID != null
					                && memoryRoot[i].additional_GUID.Length != 0;
					                
					//////////////////////////////////////////////////////
					if ( icon != null || drawCount )     //style.padding.left = 6;
					{
					
						var LH = Math.Min(__LH, (int)cell.width / 3);
						
						var drawOffset = drawCell;
						
						if (type != MemType.Scenes )
						{	style.padding.left = (int)(LH * 0.6f);
							/* if (type != MemType.Last || drawCount)
							 {   drawOffset.x += LH / 2;
							     drawOffset.width -= LH / 2;
							 }
							 else*/
							{	// style.padding.left += Mathf.RoundToInt(drawOffset.height / 2);
								//  style.padding.left +=(int) (LH * 0.7f);
								style.padding.left = LH;
							}
						}
						
						if ( type == MemType.Last && LastIndex == i ) active = true;
						
						//////////////////////////////////////////////////////
						
						//  Color32? bgColor = null;
						//   Color32? textColor = null;
						
						if ( (type == MemType.Last || type == MemType.Custom) && adapter._S_USE_HIGLIGHT_IN_BOTTOM )
						{	var drawHL = type == MemType.Last && RowsParams[PLUGIN_ID.LAST].HiglighterValue || type == MemType.Custom && RowsParams[PLUGIN_ID.BOOKMARKS].HiglighterValue;
						
							if ( drawHL && INT32_ACTIVE( memoryRoot[i].InstanceID ) )
							{	tempColor = adapter.ColorModule.needdrawGetColor( h );
								/* if (col != null)
								 {   bgColor = col[0];
								     textColor = col[1];
								 }*/
							}
						}
						
						
						
						
						var bC = styleColor;
						var cC = Color.white;
						
						
						var isSelect = wasSelect ?? INT32_ACTIVE( memoryRoot[i].InstanceID ) &&  memoryRoot[i].IsSelectedHadrScan();
						
						// adapter.IsSelected(h.id);
						
						if ( isSelect )
						{	EditorGUI.DrawRect( drawCell, type == MemType.Custom ? adapter.SelectColor : adapter.SelectColorOverrided( false )/* adapter.SelectColor*/ );
							wasSelect = false;
						}
						
						
						
						
						
						DRAWSTYLE_A( style, bC, cC, drawOffset, active, type, controller, memoryRoot[i], idOffset + i, tempColor );
						
						
						
						
						if ( tempColor != null && tempColor.HAS_BG_COLOR )
						{	var c = tempColor.BGCOLOR;
							c.a = (byte)(c.a * adapter.par.highligterOpacity * adapter.BOTTOM_CONTRAST);
							
							//  c.a /= 2;
							// c.a = (byte)(c.a / 1.2f);
							/* if (type == MemType.Custom)
							 {   bC = c;
							 }
							 else*/
							{	var oc = GUI.color;
							
								if ( isSelect ) c.a = (byte)(c.a / 255f * 200f);
								
								// GUI.color *= c;
								var bgrect = drawCell;
								
								if ( tempColor.BG_HEIGHT == 1 )
								{	var newH = bgrect.height - adapter.labelStyle.CalcHeight(content, 10000) + 4;
									bgrect.y += newH / 2;
									bgrect.height -= newH;
								}
								
								else
									if ( tempColor.BG_HEIGHT == 2 )
									{	bgrect.y += bgrect.height / 2;
										bgrect.height = 1;
									}
									
								float LEFT = 0;
								float RIGHT = bgrect.width;
								
								if ( tempColor.BG_ALIGMENT_LEFT >= 3 ) LEFT = 0.75f * bgrect.width;
								
								if ( tempColor.BG_ALIGMENT_RIGHT == 3 || tempColor.BG_ALIGMENT_RIGHT == 4 ) RIGHT = 0.15f * bgrect.width;
								
								bgrect.x += LEFT;
								bgrect.width -= (bgrect.width - RIGHT);
								bgrect.width -= LEFT;
								
								if ( tempColor.BG_HEIGHT != 2 )
								{	bgrect.y += 1;
									bgrect.height -= 2;
								}
								
								
								if ( tempColor.BG_HEIGHT == 2 )
								{	GUI.BeginClip( bgrect );
									bgrect.x = bgrect.y = 0;
								}
								
								adapter.ColorModule.DRAW_BGTEXTURE_OLD( bgrect, c );
								
								if ( tempColor.BG_HEIGHT == 2 ) GUI.EndClip();
								
								//  GUI.DrawTexture( bgrect, Texture2D.whiteTexture );
								GUI.color = oc;
							}
							
						}
						
						else
						{
						
							/*  if ( type == MemType.Custom )
							      if ( isSelect ) bC.a = (byte)(bC.a / 255f * 200f);*/
							
						}
						
						
						if ( tempColor != null && tempColor.HAS_LABEL_COLOR )
						{	var c = tempColor.LABELCOLOR;
						
							if ( c.r != 0 || c.g != 0 || c.b != 0 || c.a != 0 )
							{	SetStyleColor( style, c );
								cC = Color.white;
							}
						}
						
						//////////////////////////////////////////////////////
						DRAWSTYLE_B( style, bC, cC, drawOffset, active, type, controller, memoryRoot[i], idOffset + i, tempColor );
						
						//////////////////////////////////////////////////////
						
						RestoreStyleColor( style );
						
						////// SYNCHRONIZATION FAVORITE ///////
						///////////////////////////////////////
						///////////////////////////////////////
						///////////////////////////////////////
						
						//////////////////////////////////////////////////////
						if ( active && style == adapter.box ) Adapter.DrawTexture( drawOffset, adapter.colorStatic );
						
						//////////////////////////////////////////////////////
						
						drawOffset = drawCell;
						
						var oldH = drawOffset.height;
						drawOffset.height = LH;
						drawOffset.y += (oldH - drawOffset.height) / 2;
						drawOffset.width = drawOffset.height;
						
						if ( type == MemType.Last )
						{	drawOffset.y++;
							drawOffset.width -= 2;
							drawOffset.height -= 2;
						}
						
						if ( icon != null )
						{	if ( type == MemType.Scenes )
							{	/* drawOffset.y -= 2;
								 drawOffset.height += 2;
								 drawOffset.x -= 1;
								 drawOffset.width += 1;*/
								drawOffset = Shrink( drawOffset, 2 );
								//drawOffset.x -= drawOffset.width;
								drawOffset.x = drawCell.width + drawOffset.x - drawOffset.width;
							}
							
							if ( type == MemType.Custom || type == MemType.Last )
							{	drawOffset = Shrink( drawOffset, 2 );
							}
							
							var c = GUI.color;
							GUI.color *= iconColor;
							GUI.DrawTexture( drawOffset, icon );
							GUI.color = c;
						}
					}
					
					else
					{	style.padding.left = 2;
					
						// var c = GUI.color;
						// GUI.color = new Color(1, 1, 1, .5f);
						if ( type == MemType.Last && LastIndex == i ) active = true;
						
						//////////////////////////////////////////////////////
						//Color32? bgColor = null;/////////
						// Color32? textColor = null; /////////
						if ( (type == MemType.Last || type == MemType.Custom) && adapter._S_USE_HIGLIGHT_IN_BOTTOM ) /////////
						{	/////////
							var drawHL = type == MemType.Last && RowsParams[PLUGIN_ID.LAST].HiglighterValue || type == MemType.Custom && RowsParams[PLUGIN_ID.BOOKMARKS].HiglighterValue;
							
							if ( drawHL && INT32_ACTIVE( memoryRoot[i].InstanceID ) )
							{
							
								tempColor = adapter.ColorModule.needdrawGetColor( h ); /////////
								/*if (col != null) /////////
								{   /////////
								    bgColor = col[0];/////////
								    textColor = col[1];/////////
								}*/
							}/////////
						}/////////
						
						
						var bC = styleColor;
						var cC = styleColor;
						
						var isSelect = wasSelect?? INT32_ACTIVE( memoryRoot[i].InstanceID ) &&  memoryRoot[i].IsSelectedHadrScan();
						//adapter.IsSelected(h.id);
						
						if ( isSelect )
						{	EditorGUI.DrawRect( drawCell, type == MemType.Custom ? adapter.SelectColor : adapter.SelectColorOverrided( false )/* adapter.SelectColor*/ );
							wasSelect = false;
						}
						
						
						DRAWSTYLE_A( style, bC, cC, drawCell, active, type, controller, memoryRoot[i], idOffset + i, tempColor );
						
						
						if ( tempColor != null && tempColor.HAS_BG_COLOR )
						{	var c = tempColor.BGCOLOR;
							c.a = (byte)(c.a * adapter.par.highligterOpacity * adapter.BOTTOM_CONTRAST);
							//  c.a /= 2;
							// c.a = (byte)(c.a / 1.2f);
							/*if (type == MemType.Custom) bC = c;
							else*/
							{	var oc = GUI.color;
							
								if ( isSelect ) c.a = (byte)(c.a / 255f * 200f);
								
								//GUI.color *= c;
								var bgrect = drawCell;
								
								if ( tempColor.BG_HEIGHT == 1 )
								{	var newH = bgrect.height - adapter.labelStyle.CalcHeight(content, 10000) + 4;
									bgrect.y += newH / 2;
									bgrect.height -= newH;
								}
								
								else
									if ( tempColor.BG_HEIGHT == 2 )
									{	bgrect.y += bgrect.height / 2;
										bgrect.height = 1;
									}
									
								float LEFT = 0;
								float RIGHT = bgrect.width;
								
								if ( tempColor.BG_ALIGMENT_LEFT >= 3 ) LEFT = 0.75f * bgrect.width;
								
								if ( tempColor.BG_ALIGMENT_RIGHT == 3 || tempColor.BG_ALIGMENT_RIGHT == 4 ) RIGHT = 0.15f * bgrect.width;
								
								// if (tempColor.BG_ALIGMENT_RIGHT != 4) RIGHT = 0.75f * bgrect.width;
								bgrect.x += LEFT;
								bgrect.width -= (bgrect.width - RIGHT);
								bgrect.width -= LEFT;
								
								if ( tempColor.BG_HEIGHT != 2 )
								{	bgrect.y += 1;
									bgrect.height -= 2;
								}
								
								
								if ( tempColor.BG_HEIGHT == 2 )
								{	GUI.BeginClip( bgrect );
									bgrect.x = bgrect.y = 0;
								}
								
								adapter.ColorModule.DRAW_BGTEXTURE_OLD( bgrect, c );
								
								if ( tempColor.BG_HEIGHT == 2 ) GUI.EndClip();
								
								// GUI.DrawTexture( bgrect , Texture2D.whiteTexture );
								GUI.color = oc;
							}
							
						}
						
						else
						{
						
							/* if ( type == MemType.Custom )
							     if ( isSelect ) bC.a = (byte)(bC.a / 255f * 200f);*/
							
						}
						
						var oldC = style.normal.textColor; /////////
						
						if ( tempColor != null && tempColor.HAS_LABEL_COLOR ) /////////
						{	/////////
							var c = tempColor.LABELCOLOR; /////////
							
							if ( c.r != 0 || c.g != 0 || c.b != 0 || c.a != 0 ) /////////
							{	/////////
								// c.a = Math.Max((byte)10, c.a); /////////
								// style.normal.textColor = c; /////////
								SetStyleColor( style, c );
								cC = Color.white;
							}/////////
						}//////////////////////////////////////////////////////
						
						/* var cc = GUI.color;
						 GUI.color *= styleColor;
						 style.Draw(drawCell, content, active, active, false, active);
						 GUI.color = cc;*/
						DRAWSTYLE_B( style, bC, cC, drawCell, active, type, controller, memoryRoot[i], idOffset + i, tempColor );
						
						
						//  style.normal.textColor = oldC; ;///////////////////////////
						RestoreStyleColor( style );
						
						
						
						
						
						if ( active && style == adapter.box ) Adapter.DrawTexture( drawCell, adapter.colorStatic );
						
						//GUI.color = c;
					}
					
					
					var o = memoryRoot[i].InstanceID != null && INT32_ISVALID(memoryRoot[i].InstanceID) ? h : null;
					
					if ( type == MemType.Custom && !SHOW_DES && adapter.FAVORITS_SHOWDESICON && o != null )
					{
					
						//var r_info = GET_INFO_RECT(drawCell);
						//var iconSizey = Mathf.Min(t.width, r_info.height - 6);
						// MonoBehaviour.print(r_info);
						// var scene = INT32_SCENE(memoryRoot[i].InstanceID);
						var containsKey = adapter.DescriptionModule.HasKey(scene.GetHashCode(), o);
						
						var c = GUI.color;
						
						if ( !containsKey )
						{	GUI.color *= coloAlpha;
							Adapter.DrawTexture( r_info, adapter.GetIcon( EditorGUIUtility.isProSkin ? "BOTTOM_INFO DISABLE" : "BOTTOM_INFO DISABLE PERSONAL" ) );
						}
						
						else
						{
						
							Adapter.DrawTexture( r_info, adapter.GetIcon( EditorGUIUtility.isProSkin ? "BOTTOM_INFO" : "BOTTOM_INFO PERSONAL" ) );
						}
						
						GUI.color = c;
						
						
						EditorGUIUtility.AddCursorRect( r_info, MouseCursor.ArrowPlus );
						
						
						if ( infoContains )
						{	if ( cellAction && cellRectContains )
							{
							
								GUI.DrawTexture( Shrink( r_info, -5 ), adapter.GetIcon( "HIPERUI_BUTTONGLOW" ) );
							}
						}
						
						if ( r_info.Contains( Event.current.mousePosition ) )
						{
						
							if ( containsKey )
							{	var d = adapter.DescriptionModule.GetValue(scene.GetHashCode(), o);
								Label( r_info, new GUIContent() { tooltip = "- " + d } );
							}
							
							else
							{	Label( r_info, new GUIContent() { tooltip = "No Description\nLeft CLICK to add Description" } );
							}
						}
						
						
						/* if (M_Descript.HasKey(o.scene, o))
						 {
						     //var dl = M_Descript.getDoubleList(o.scene);
						     var d = M_Descript.GetValue(o.scene, o);
						     GUI.Label(r_info, new GUIContent() { tooltip = d });
						     /*
						                                             dRect.x += LINE_REFERENCE_HEIGHT;
						                                             dRect.width -= LINE_REFERENCE_HEIGHT + hiperRect.width;
						
						                                             var tf = Adapter.GET_SKIN().label.fontSize;
						                                             Adapter.GET_SKIN().label.fontSize = oldFOntl;
						                                             var oc = GUI.color;
						                                             GUI.color *= ca5;
						                                             GUI.Label(dRect, );
						                                             GUI.color = oc;
						                                             Adapter.GET_SKIN().label.fontSize = tf;#1#
						 } else
						 {
						     GUI.Label(r_info, new GUIContent() { tooltip = "No Description\nLeft CLICK to add Description" });
						 }*/
						
					}
					
					
					if ( drawCount )
					{	var drawOffset = drawCell;
						var LH = Math.Min(__LH, (int)cell.width / 3);
						
						DRAW_COUNT( drawOffset, LH, type, memoryRoot[i], type == MemType.Last && !icon );
					}
					
					
					
					/* style.margin = oldM;*/
					style.alignment = oldA;
					style.border = oldB;
					style.fixedHeight = ymon;
					style.wordWrap = over;
					style.fontSize = oldF;
					style.padding.left = padl;
					style.padding.top = padt;
					GUI.color = oldColor;
					// EditorStyles.helpBox.Draw(cell, new GUIContent("Object" + (i + 1)), false, false, false, false);
				}
				
				
				if ( !DisableCursor )
				{	//if ( type == MemType.Scenes )
					EditorGUIUtility.AddCursorRect( cell, MouseCursor.Link );
				}
				
				var LH2 = Math.Min(__LH, (int)cell.width / 3);
				
				if ( type == MemType.Scenes )
				{	var linkRect = cell;
					linkRect.x = cell.x + cell.width - LH2;
					linkRect.width = cell.x + cell.width - linkRect.x;
					EditorGUIUtility.AddCursorRect( linkRect, MouseCursor.Link );
				}
				
				var ctt = GETTOOLTIPPEDCONTENT(type, type == MemType.Custom ? memoryRoot[i].ToString() : memoryRoot[i].FullString(), controller);
				
				//if ( type == MemType.Custom ) Debug.Log( memoryRoot[i].ToString() );
				if ( (adapter.par.BottomParams.BOTTOM_TOOLTIPES || type == MemType.Scenes) && cell.Contains( Event.current.mousePosition ) )
				{	if ( type == MemType.Scenes && Event.current.mousePosition.x > cell.x  + cell.width - LH2 )
					{	ctt.tooltip += '\n';
						ctt.tooltip += pinScene.tooltip;
					}
					
					else
						if ( type == MemType.Custom )
						{	var o = memoryRoot[i].InstanceID != null && INT32_ISVALID(memoryRoot[i].InstanceID) ? h : null;
							//var o = memoryRoot[i].InstanceID != null ? memoryRoot[i].InstanceID.ActiveGameObject : null;
							
							if ( type == MemType.Custom && o != null/*&& o.scene.IsValid() && o.scene.isLoaded*/ )     //  var scene = INT32_SCENE(memoryRoot[i].InstanceID);
							{
							
								if ( adapter.DescriptionModule.HasKey( scene.GetHashCode(), o ) )      //var dl = M_Descript.getDoubleList(o.scene);
								{	var d = adapter.DescriptionModule.GetValue(scene.GetHashCode(), o);
									ctt.tooltip += "- " + d;
									//  ctt.tooltip += '\n';
									
									//  GUI.Label(r_info, new GUIContent() { tooltip = d });
									/*
									                                        dRect.x += LINE_REFERENCE_HEIGHT;
									                                        dRect.width -= LINE_REFERENCE_HEIGHT + hiperRect.width;
									
									                                        var tf = Adapter.GET_SKIN().label.fontSize;
									                                        Adapter.GET_SKIN().label.fontSize = oldFOntl;
									                                        var oc = GUI.color;
									                                        GUI.color *= ca5;
									                                        GUI.Label(dRect, );
									                                        GUI.color = oc;
									                                        Adapter.GET_SKIN().label.fontSize = tf;*/
								}
								
								else
								{	ctt.tooltip += "- No Description"; //\nLeft CLICK on 'i' icon to add Description
									//  ctt.tooltip += '\n';
								}
							}
							
							else
							{	ctt.tooltip += "- ...";
								// ctt.tooltip += '\n';
							}
						}
						
						else
						{	// ctt.tooltip += '\n';
						}
				}
				else
				{	ctt.tooltip = "";
				}
				
				/* if ( type == MemType.Custom )
				     ctt.tooltip += "\nLeft CLICK - Selecting\nLeft DRAG - Draging\nCtrl/Shift - Additive Selecting\nAlt - Keep Scrool Selecting";*/
				
				if (!string.IsNullOrEmpty( ctt.tooltip ) && (DragAndDrop.visualMode == DragAndDropVisualMode.None && !controller.wasDrag ))
				{
				
					ctt.tooltip = ctt.tooltip.Trim( tr );
					Label( cell, ctt );
				}
				
				
			} // for
			
			
			if ( controller.selection_action != null && controller.wasDrag )
			{	var c = GUI.color;
				var c2 = c;
				c2.a = 0.5f;
				GUI.color = c2;
				GUI.DrawTexture( DragRect, adapter.button.active.background );
				GUI.color = c;
			}
			
			
			
			return wasDraw;
		}
		char[] tr = new[] {'\n', '\r', ' ', ';', '\0'};
		GUIStyle __countStyle;
		GUIStyle countStyle
		{	get
			{	if ( __countStyle == null )
				{	__countStyle = new GUIStyle( adapter.label );
					__countStyle.alignment = TextAnchor.MiddleCenter;
					__countStyle.fontStyle = FontStyle.Bold;
				}
				
				return __countStyle;
			}
		}
		internal void DRAW_COUNT( Rect drawOffset, float LH, MemType type, MemoryRoot memoryRoot, bool DRAW_CTN )
		{
		
			var HH = drawOffset.y + drawOffset.height;
			var WW = drawOffset.x + drawOffset.width;
			
			
			var oldH = drawOffset.height;
			drawOffset.height = LH + 2;
			drawOffset.y += (oldH - drawOffset.height) / 2;
			drawOffset.width = drawOffset.height;
			
			countStyle.fontStyle = type == MemType.Last ? FontStyle.Normal :  FontStyle.Bold;
			countStyle.fontSize = type != MemType.Last ? adapter.STYLE_LABEL_10.fontSize : (adapter.STYLE_LABEL_8.fontSize - 1);
			drawOffset.y = HH - drawOffset.height;
			drawOffset.x = WW - drawOffset.width;
			
			
			drawOffset.height -= 1;
			//drawOffset.y++;
			/*drawOffset.width = drawOffset.height;
			drawOffset.height++;*/
			
			var cc = countStyle.normal.textColor;
			//                 var align = Adapter.GET_SKIN().label.alignment;
			//
			//                 var st = Adapter.GET_SKIN().label.fontStyle;
			//                 var font = Adapter.GET_SKIN().label.fontSize;
			//
			//                 Adapter.GET_SKIN().label.alignment = TextAnchor.MiddleCenter;
			//                 Adapter.GET_SKIN().label.fontSize--;
			//                 Adapter.GET_SKIN().label.fontStyle = FontStyle.Bold;
			var print = type == MemType.Scenes ? (memoryRoot.additional_GUID.Length + 1).ToString() :  INT32_COUNT(memoryRoot.InstanceID).ToString();
			countStyle.normal.textColor = Color.black;
			
			
			if ( DRAW_CTN ) GUI.DrawTexture( drawOffset, adapter.GetIcon( "OBJECTCONTENTCOUNT" ) );
			
			drawOffset.width += 2;
			drawOffset.y -= 1;
			GUI.Label( drawOffset, print, countStyle );
			drawOffset.y += 2;
			GUI.Label( drawOffset, print, countStyle );
			drawOffset.x -= 2;
			GUI.Label( drawOffset, print, countStyle );
			drawOffset.y -= 2;
			GUI.Label( drawOffset, print, countStyle );
			
			countStyle.normal.textColor = Color.white;
			drawOffset.y += 1;
			drawOffset.x += 1;
			
			
			
			GUI.Label( drawOffset, print, countStyle );
			
			//                 Adapter.GET_SKIN().label.alignment = align;
			//                 Adapter.GET_SKIN().label.fontSize = font;
			countStyle.normal.textColor = cc;
			//  Adapter.GET_SKIN().label.fontStyle = st;
		}
		
		GUIContent content_des = new GUIContent();
		Color coloAlpha = new Color(1, 1, 1, 0.6f);
		Rect r_des;
		private void DRAWSTYLE_A( GUIStyle style, Color bColor, Color cColor, Rect drawOffset, bool active,
		                          MemType type, BottomController controller, MemoryRoot mem, int cintrolID, TempColorClass otherStyles )
		{
		
			var resof = drawOffset;
			
			if ( type == MemType.Scenes && Adapter.UNITY_CURRENT_VERSION >= Adapter.UNITY_2019_3_0_VERSION)
			{	//drawOffset.y += drawOffset.height / 3;
				drawOffset.height /= 3 / 2.1f;
			}
			
			
			
			var bc = GUI.backgroundColor;
			var cc = GUI.contentColor;
			var oldOver = style.clipping;
			GUI.contentColor = Color.clear;
			GUI.backgroundColor = bc * bColor; ;
			style.clipping = TextClipping.Clip;
			style.Draw( drawOffset, content, active, active, false, active );
			style.clipping = oldOver;
			drawOffset = resof;
			GUI.backgroundColor = bc;
			GUI.contentColor = cc;
			
			
			
			
		}
		
		private void DRAWSTYLE_B( GUIStyle style, Color bColor, Color cColor, Rect drawOffset, bool active,
		                          MemType type, BottomController controller, MemoryRoot mem, int cintrolID, TempColorClass otherStyles )
		{
		
			// var resof = drawOffset;
			if ( type == MemType.Scenes && Adapter.UNITY_CURRENT_VERSION >= Adapter.UNITY_2019_3_0_VERSION )     //drawOffset.y += drawOffset.height / 3;
			{	drawOffset.height /= 3 / 2.1f;
			}
			
			var bc = GUI.backgroundColor;
			var cc = GUI.contentColor;
			var oldOver = style.clipping;
			GUI.backgroundColor = Color.clear;
			
			if ( otherStyles != null && otherStyles.LABEL_SHADOW && otherStyles.HAS_LABEL_COLOR )     // var _oc2 =  Adapter.GET_SKIN().label.normal.textColor;
			{
			
			
				//var c2 = Adapter.GET_SKIN().label.normal.textColor;
				var c2 = Color.black;
				c2.a = cColor.a;
				GUI.contentColor = c2;
				drawOffset.y -= 0.5f;
				drawOffset.x -= 1f;
				style.clipping = TextClipping.Clip;
				style.Draw( drawOffset, content, active, active, false, active );
				style.clipping = oldOver;
				drawOffset.y += 0.5f;
				drawOffset.x += 1f;
			}
			
			
			
			//  GUI.contentColor = (Adapter.GET_SKIN().button.normal.textColor ) * cColor;
			
			GUI.contentColor = cc * cColor;
			var casd = style.normal.textColor;
			
			if ( EditorGUIUtility.isProSkin && !needRestore ) style.normal.textColor = adapter.labelStyle.normal.textColor;
			
			style.clipping = TextClipping.Clip;
			style.Draw( drawOffset, content, active, active, false, active );
			style.clipping = oldOver;
			style.normal.textColor = casd;
			
			
			GUI.backgroundColor = bc;
			GUI.contentColor = cc;
			
			
			var DES = (type == MemType.Custom && (controller.IS_MAIN && adapter.par.BottomParams.SHOW_ALL_DESCRIPTIONS_INHIER || !controller.IS_MAIN && adapter.par.BottomParams.SHOW_ALL_DESCRIPTIONS_INWIN));
			
			if ( DES )
			{	r_des = drawOffset;
				r_des.height += DESCRIPTION_MULTY( controller.IS_MAIN );
				
				var c = GUI.color;
				GUI.color *= coloAlpha;
				Adapter.INTERNAL_BOX( r_des, "" );
				GUI.color = c;
				
				//adapter.InitializeStyle
			}
			
			if ( DES )
			{	r_des.y = drawOffset.y + drawOffset.height;
				r_des.height -= drawOffset.height;
				
				
				var o = mem.InstanceID != null && INT32_ISVALID(mem.InstanceID) ? INT32__ACTIVE_TOHIERARCHYOBJECT(mem.InstanceID) : null;
				
				//var o = mem.InstanceID != null ? mem.InstanceID.ActiveGameObject : null;
				
				if ( o != null /*&& o.scene.IsValid() && o.scene.isLoaded*/ )
				{	var scene = INT32_SCENE(mem.InstanceID);
				
					if ( adapter.DescriptionModule.HasKey( scene.GetHashCode(), o ) )
					{	var d = adapter.DescriptionModule.GetValue(scene.GetHashCode(), o);
						content_des.text = content_des.tooltip = d;
					}
					
					else
					{	content_des.text = "-";
						content_des.tooltip = "No Description\nLeft CLICK on 'i' icon to add Description";
					}
				}
				
				else
				{	content_des.text = content_des.tooltip = "- ...";
				}
				
				//                     var a = Adapter.GET_SKIN().label.alignment;
				//                     var f = Adapter.GET_SKIN().label.fontSize;
				
				descrStyle.fontSize = adapter.FONT_8();
				
				if ( controller.IS_MAIN ) descrStyle.fontSize -= 1;
				
				
				content_des.tooltip = content_des.tooltip.Trim( tr );
				GUI.Label( r_des, content_des, descrStyle );
				//                     Adapter.GET_SKIN().label.alignment = a;
				//                     Adapter.GET_SKIN().label.fontSize = f;
				
				
				if ( controller.selection_action != null && controller.selection_button == cintrolID + 100 )
				{	adapter.button.Draw( controller.lastRect, REALEMPTY_CONTENT, false, false, false, true );
				}
			}
		}
		GUIStyle __descrStyle;
		GUIStyle descrStyle
		{	get
			{	if ( __descrStyle == null )
				{	__descrStyle = new GUIStyle( adapter.label );
					__descrStyle.alignment = TextAnchor.MiddleLeft;
					__descrStyle.clipping = TextClipping.Clip;
				}
				
				return __descrStyle;
			}
		}
		GUIContent REALEMPTY_CONTENT = new GUIContent();
		// DrawButton()
		
		void Shrink( ref Rect rect, int amount )
		{	rect.x += amount;
			rect.y += amount;
			rect.width -= 2 * amount;
			rect.height -= 2 * amount;
		}
		Rect Shrink( Rect rect, int amount )
		{	rect.x += amount;
			rect.y += amount;
			rect.width -= 2 * amount;
			rect.height -= 2 * amount;
			return rect;
		}
		Rect zero = new Rect(0, 0, 0, 0);
		Rect GET_INFO_RECT( MemType type, Rect drawCell )
		{	if ( type != MemType.Custom ) return zero;
		
			// var t = adapter.GetIcon("BOTTOM_INFO");
			var SIZE = 12;
			var r_info = drawCell;
			r_info.x -= 4;
			r_info = Shrink( r_info, 3 );
			var iconSizey = Mathf.RoundToInt(Mathf.Min(SIZE, r_info.height - 6));
			var iconSizex = iconSizey;
			r_info.Set( r_info.x + r_info.width - iconSizex, r_info.y + (r_info.height - iconSizey) / 2, iconSizex, iconSizey );
			return r_info;
		}
		
		
		GUIContent pinScene = new GUIContent() { tooltip = "Press to 'Pin' this scene" };
		
		internal TempColorClass __GetContentEmpty = new TempColorClass().AddIcon(null);
		internal TempColorClass GetContent( HierarchyObject o )     //  if (adapter.IS_PROJECT()) return AssetDatabase.GetCachedIcon( o.project.assetPath );
		{	if ( o == null || !o.Validate() ) return __GetContentEmpty;
		
		
			/* if (adapter.IS_HIERARCHY() && adapter.HAS_LABEL_ICON() )
			 {   var context2 = Utilities.ObjectContent_NoCacher(adapter, o.go, o.GET_TYPE());
			     if (!context2.add_icon) return __GetContentEmpty;
			     return context2;
			 }*/
			
			
			
			// adapter.MOI.M_Colors.GetColorForObject
			var context = Utilities.ObjectContent_IncludeCacher(adapter, o, o.GET_TYPE(adapter), true);
			
			if ( !context.add_icon ) return __GetContentEmpty;
			
			if ( adapter.IS_HIERARCHY() && !adapter.HAS_LABEL_ICON() ) //#COLUP
			{	if ( context.add_icon == Utilities.ObjectContent_NoCacher( adapter, (UnityEngine.Object)null, o.GET_TYPE( adapter ) ).add_icon
				        || Utilities.IsPrefabIcon( context.add_icon ) ) context = __GetContentEmpty;
			}
			
			return context;
			// return GetContent( EditorUtility.InstanceIDToObject( o.id ) );
		}
		/*internal Texture GetContent(UnityEngine.Object o)
		{
		  // var o = EditorUtility.InstanceIDToObject(instanceID);
		  if (!o) return null;
		  var context = Utilities.ObjectContent_NoCacher(adapter, o, Adapter.GetType_(o)).image;
		  if (context == null || context == Utilities.ObjectContent_NoCacher( adapter, (UnityEngine.Object)null, Adapter.GetType_( o ) ).image || context.name == "PrefabNormal Icon" || context.name == "PrefabModel Icon") return null;
		  return context;
		}*/
		
		
		internal abstract class MemoryRoot {
			internal BottomInterface bottomInterface;
			internal MemoryRoot( BottomInterface bottomInterface )
			{	this.bottomInterface = bottomInterface;
			}
			
			
			public Int32List InstanceID;
			public string GUID;
			public string PATH;
			public string[] additional_GUID;
			public string[] additional_PATH;
			public bool pin;
			public int ArrayIndex = -1;
			public int RectBindIndex;
			
			public virtual void SetStringValues( SceneId scene, int arrayIndex ) { }
			public virtual void SetIntValues( Int32List instanceId, int arrayIndex ) { }
			public virtual void SetObjectValues( object instanceId, int arrayIndex ) { }
			public abstract bool OnClick( bool par1, int scene );
			public new abstract string ToString();
			public virtual string FullString() { return ""; }
			public abstract bool IsValid();
			public abstract bool IsSelectedHadrScan();
			public abstract bool IsSelectablePlus();
			public abstract bool IsSelectableMinus();
			public int GET_SELECTION_STATE()
			{	var STATE  = 0;
				var mayMinus = false;
				
				var selected = false;
				
				if ( Event.current.control )
				{	selected = this is GameObjectMemory ? InstanceID.list.Any( o => o && bottomInterface.adapter.IsSelected( o.GetInstanceID() ) ) // IsSelectedHadrScan();
					           : false;
				}
				
				// var selected = IsSelectedHadrScan();
				
				
				if ( IsSelectablePlus() )
					if ( (Event.current.control && !selected || Event.current.shift ) )
						STATE = 1;
					else
						mayMinus = true;
						
				if ( IsSelectableMinus() && mayMinus )
					if ( Event.current.control && selected )
						STATE = 2;
						
				if ( Event.current.alt ) STATE = 3;
				
				return STATE;
			}
		}
		
		private class SceneMemory : MemoryRoot {
		
		
			private string sceneName;
			private string sceneName_Full;
			
			public SceneMemory( BottomInterface bottomInterface ) : base( bottomInterface )
			{
			}
			
			public override void SetStringValues( SceneId scene, int arrayIndex )
			{	if ( scene != null )
				{	this.GUID = scene.guid;
					this.additional_GUID = scene.additional_guid ?? new string[0];
					this.PATH = scene.path;
					this.additional_PATH = scene.additional_path ?? new string[0];
					this.pin = scene.pin;
				}
				
				this.ArrayIndex = arrayIndex;
				this.RectBindIndex = arrayIndex;
				this.validCache = null;
				UpdateSceneName();
			}
			
			private void UpdateSceneName()
			{	var path = AssetDatabase.GUIDToAssetPath(GUID);
			
				if ( !path.Contains( '/' ) )
				{	sceneName_Full = sceneName = "...";
					return;
				}
				
				sceneName = sceneName_Full = Path_ToSceneNamr( path );
				
				if ( additional_PATH == null ) additional_PATH = new string[0];
				
				for ( int i = 0 ; i < additional_PATH.Length ; i++ )
					sceneName_Full += '\n' + Path_ToSceneNamr( path );
			}
			
			string Path_ToSceneNamr( string path )
			{	var tempName = path.LastIndexOf( '/' ) != -1 ? path.Substring( path.LastIndexOf( '/' ) + 1 ) : path;
				return tempName.LastIndexOf( '.' ) != -1 ? tempName.Remove( tempName.LastIndexOf( '.' ) ) : tempName;
			}
			
			
			public override bool OnClick( bool par1, int scene )
			{	if ( Application.isPlaying ) return true;
			
				if ( Event.current != null && (Event.current.control || Event.current.alt) )
				{
				
					//  var ass = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(GUID);
					// if (!ass) return false;
					var path = AssetDatabase.GUIDToAssetPath(GUID);
					
					List<Object> result = new List<Object>();
					// if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
					{	try
						{	AssetDatabase.LoadMainAssetAtPath( path ).GetInstanceID();
							result.Add( AssetDatabase.LoadMainAssetAtPath( path ) );
						}
						
						catch
						{	try { result.Add( AssetDatabase.LoadMainAssetAtPath( PATH ) ); }
						
							catch { return false; }
						}
					}
					
					if ( additional_GUID != null && additional_GUID.Length != 0 )
					{	for ( int Index = 0 ; Index < additional_GUID.Length ; Index++ )
						{	path = AssetDatabase.GUIDToAssetPath( additional_GUID[Index] );
							var  PATH = additional_PATH[Index];
							
							try
							{	AssetDatabase.LoadMainAssetAtPath( path ).GetInstanceID();
								result.Add( AssetDatabase.LoadMainAssetAtPath( path ) );
							}
							
							catch
							{	try
								{	result.Add( AssetDatabase.LoadMainAssetAtPath( PATH ) );
								}
								
								catch { }
							}
						}
					}
					
					Selection.objects = result.ToArray();
					
				}
				
				else
				{	var path = AssetDatabase.GUIDToAssetPath(GUID);
					UpdateSceneName();
					bottomInterface.LastIndex = -1;
					
					var type = Event.current != null && Event.current.shift ? OpenSceneMode.Additive : OpenSceneMode.Single;
					
					if ( EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo() )
					{	try { EditorSceneManager.OpenScene( path, type ); }
					
						catch
						{	try { EditorSceneManager.OpenScene( PATH, type ); }
						
							catch { return false; }
						}
					}
					
					if ( additional_GUID != null && additional_GUID.Length != 0 )
					{	for ( int Index = 0 ; Index < additional_GUID.Length ; Index++ )
						{	path = AssetDatabase.GUIDToAssetPath( additional_GUID[Index] );
							var  PATH = additional_PATH[Index];
							
							try
							{	EditorSceneManager.OpenScene( path, OpenSceneMode.Additive );
							}
							
							catch
							{	try
								{	EditorSceneManager.OpenScene( PATH, OpenSceneMode.Additive );
								}
								
								catch { }
							}
						}
					}
					
					
					bottomInterface.adapter.LastActiveScene = bottomInterface.adapter.GET_ACTIVE_SCENE;
					bottomInterface.Scene_WriteLastScene( (bottomInterface.adapter.GET_ACTIVE_SCENE) );
					
					if ( bottomInterface.onSceneChange != null ) bottomInterface.onSceneChange();
					
					bottomInterface.Scene_RefreshGUIAndClearActions( bottomInterface.adapter.GET_ACTIVE_SCENE );
				}
				
				
				//
				
				return true;
			}
			
			public override bool IsSelectedHadrScan()
			{	return false;
			}
			public override bool IsSelectablePlus()
			{	return true;
			}
			public override bool IsSelectableMinus()
			{	return false;
			}
			
			public override string ToString()
			{	return sceneName;
			}
			
			bool? validCache = null;
			public override bool IsValid()
			{	return !string.IsNullOrEmpty( GUID )
				       && (validCache ?? (validCache = !string.IsNullOrEmpty( AssetDatabase.GUIDToAssetPath(
				               GUID ) )).Value); /*(validCache ?? (validCache =File.Exists(adapter.PluginExternalFolder +  AssetDatabase.GUIDToAssetPath( GUID ))*/;
			}
			
			public override string FullString()
			{	return sceneName_Full;
			}
			
		}
		
		private class GameObjectMemory : MemoryRoot {
		
			public GameObjectMemory( BottomInterface bottomInterface ) : base( bottomInterface )
			{
			}
			
			string m_FullString;
			public override void SetIntValues( Int32List instanceId, int arrayIndex )
			{	this.ArrayIndex = arrayIndex;
				this.InstanceID = instanceId;
				this.RectBindIndex = arrayIndex;
				
				
				/*  if (InstanceID != null)
				  {   if (!string.IsNullOrEmpty(InstanceID.GUIDsActiveGameObject) &&
				              string.IsNullOrEmpty(AssetDatabase.GUIDToAssetPath(InstanceID.GUIDsActiveGameObject)))
				      {   if (AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(InstanceID.PATHsActiveGameObject))
				              InstanceID.GUIDsActiveGameObject = AssetDatabase.AssetPathToGUID(InstanceID.PATHsActiveGameObject);
				          for (int i = 0; i < InstanceID.PATHsList.Count; i++)
				          {   if (AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(InstanceID.PATHsList[i]))
				                  InstanceID.GUIDsList[i] = AssetDatabase.AssetPathToGUID(InstanceID.PATHsList[i]);
				
				          }
				      }
				  }*/
				
				
				var result = "";
				var incr = 0;
				
				if ( InstanceID != null && !bottomInterface.INT32_ISNULL( InstanceID ) )
					foreach ( var name in bottomInterface.INT32_TOSTRINGARRAY( InstanceID ) )     //  var o = EditorUtility.InstanceIDToObject(t);
					{	//if ( o )
						{	if ( incr == 8 )
							{	result += "...";
								break;
							}
							
							// result += '\'' + o.name + "\' ";
							// result += o.name + '\n';
							result += name + "; ";
							++incr;
						}
					}
					
				m_FullString = incr == 0 ? "-" : result;
				m_FullString = m_FullString.Trim( '\n' );
				m_FullString = m_FullString.Trim( ' ' );
				// m_FullString = m_FullString.Trim(';');
			}
			
			public override bool OnClick( bool selectLocking, int scne )
			{
			
			
				if ( InstanceID == null || bottomInterface.INT32_ISNULL( InstanceID ) || bottomInterface.INT32_COUNT( InstanceID ) == 0 ) return false;
				
				//var result = InstanceID.list.Select(EditorUtility.InstanceIDToObject).Where(o => o).ToArray();
				//  if (result.Length == 0) return false;
				var result = bottomInterface.INT32_TOOBJECTASLISTCT(InstanceID).Where(o => o && (!selectLocking || bottomInterface.adapter.IS_HIERARCHY() || (o.hideFlags & HideFlags.NotEditable) == 0)).ToArray();
				
				if ( bottomInterface.adapter.IS_HIERARCHY() )
				{
				
					if ( !selectLocking )
					{	if ( InstanceID.list.Any( o => o && (o.hideFlags & HideFlags.NotEditable) != 0 ) )
							bottomInterface.ignoreLock = InstanceID.list.Where( o => (o.hideFlags & HideFlags.NotEditable) != 0 ).ToArray();
					}
					
				}
				
				if ( result.Length == 0 ) return false;
				
				
				bottomInterface.SkipRemove = true;
				bottomInterface.SkipRemoveFix = false;
				bottomInterface.LastIndex = -1;
				
				if ( Event.current != null && Event.current.isMouse )
				{	var STATE  = GET_SELECTION_STATE();
				
					if (Event.current != null && Event.current.shift && Adapter.HierAdapter.SHIFT_TO_INSTANTIATE_BOTTOM)
					{	var selectedObject = EditorUtility.InstanceIDToObject( Adapter.HierAdapter._IsSelectedCache_lastID) as GameObject;
						var root = selectedObject ? selectedObject.transform.parent : null;
						var sib = selectedObject ? (selectedObject.transform.GetSiblingIndex() + 1) : -1;
						var isCanvas = root ? (root.GetComponent<Canvas>() ?? root.GetComponentInParent<Canvas>()):null;
						
						List<GameObject> targetToSelect  = new List<GameObject>();
						
						
						if (!isCanvas)
						{	if (Adapter.HierAdapter.INSTANTIATE_MODE == 0 ||  SceneView.sceneViews.Count == 0)
							{	foreach (var _item in result)
								{	var item = _item as GameObject;
									var inst  = GameObject.Instantiate(item, root) as GameObject;
									inst.name = inst.name.Replace("(Clone)", "");
									
									if (sib != -1)inst.transform.SetSiblingIndex(sib);
									
									inst.transform.position = item.transform.position;
									inst.transform.rotation = item.transform.rotation;
									Undo.RegisterCreatedObjectUndo(inst, "Instantiate");
									targetToSelect.Add(inst);
								}
								
								Selection.objects = targetToSelect.ToArray();
							}
							
							else
							{	foreach (var _item in result)
								{	var item = _item as GameObject;
									var inst  = GameObject.Instantiate(item, root) as GameObject;
									inst.name = inst.name.Replace("(Clone)", "");
									
									if (sib != -1)inst.transform.SetSiblingIndex(sib);
									
									targetToSelect.Add(inst);
								}
								
#pragma warning disable
								var dif = targetToSelect[0].transform.position;
								var r = targetToSelect[0].transform.rotation;
								var s = SceneView.sceneViews[0] as SceneView;
								s.MoveToView(targetToSelect[0].transform);
								dif = targetToSelect[0].transform.position - dif;
								targetToSelect[0].transform.rotation = r;
								
								for (int i = 0; i < targetToSelect.Count; i++)
								{	var item = result[i] as GameObject;
									var inst = targetToSelect[i];
									
									//if (i != 0)inst.transform.position = item.transform.position + dif;
									if (i != 0)inst.transform.position = targetToSelect[0].transform.position;
									
									inst.transform.rotation = item.transform.rotation;
									Undo.RegisterCreatedObjectUndo(inst, "Instantiate");
								}
								
#pragma warning restore
								
								Selection.objects = targetToSelect.ToArray();
							}
						}
						
						else
						{	if (Adapter.HierAdapter.INSTANTIATE_MODE == 0 ||  !selectedObject)
							{	foreach (var _item in result)
								{	var item = _item as GameObject;
									var inst  = GameObject.Instantiate(item, root) as GameObject;
									inst.name = inst.name.Replace("(Clone)", "");
									
									if (sib != -1)inst.transform.SetSiblingIndex(sib);
									
									inst.transform.position = isCanvas.transform.position;
									inst.transform.rotation = isCanvas.transform.rotation;
									Undo.RegisterCreatedObjectUndo(inst, "Instantiate");
									targetToSelect.Add(inst);
								}
								
								Selection.objects = targetToSelect.ToArray();
							}
							
							else
							{	foreach (var _item in result)
								{	var item = _item as GameObject;
									var inst  = GameObject.Instantiate(item, root) as GameObject;
									inst.name = inst.name.Replace("(Clone)", "");
									
									if (sib != -1)inst.transform.SetSiblingIndex(sib);
									
									inst.transform.position = selectedObject.transform.position;
									inst.transform.rotation = selectedObject.transform.rotation;
									Undo.RegisterCreatedObjectUndo(inst, "Instantiate");
									targetToSelect.Add(inst);
								}
								
								Selection.objects = targetToSelect.ToArray();
							}
						}
						
					}
					
					else
					{	switch ( STATE )
						{	case 0:
							case 3:
							
								//  Selection.Set
								if ( STATE == 3 ) bottomInterface.adapter.SAVE_SCROLL();
								
								Selection.objects = result;
								bottomInterface.LastIndex = ArrayIndex;
								break;
								
							case 1:
								Selection.objects = Selection.objects.Concat( result ).ToArray();
								break;
								
							case 2:
								Selection.objects = Selection.objects.Except( result ).ToArray();
								break;
						}
					}
					
					
					
				}
				
				else
				{	Selection.objects = result;
					bottomInterface.LastIndex = ArrayIndex;
				}
				
				bottomInterface.adapter.InternalClearDrag();
				/*  bottomInterface.adapter.OnSelectionChanged();*/
				//  MonoBehaviour.print("ASD");
				// SelectChange = false;
				return true;
			}
			
			static List<string> asda;
			static string ts;
			public override bool IsSelectedHadrScan()
			{
			
			
			
			
				if ( InstanceID.list.Count > 500 ) return false;
				
				if ( bottomInterface.adapter.IS_HIERARCHY() )
				{	return InstanceID.list.All( o => o && bottomInterface.adapter.IsSelected( o.GetInstanceID() ) ) && InstanceID.list.Count == bottomInterface.adapter.selMax;
				
				}
				
				/*
				if (bottomInterface.adapter.IS_HIERARCHY())
				{   if (InstanceID.list.Count > 50) return false;
				    return InstanceID.list.All( o => !o || bottomInterface.adapter.IsSelected( o.GetInstanceID() ) );
				}*/
#pragma warning disable
				var ts = InstanceID.GUIDsActiveGameObject;
				var getted = bottomInterface.adapter.GetHierarchyObjectByGUID( ref ts, InstanceID.PATHsActiveGameObject );
				
				if ( ts != InstanceID.GUIDsActiveGameObject )
				{	InstanceID.GUIDsActiveGameObject = ts;
					bottomInterface.adapter.ON_GUID_BACKCHANGED();
				}
				
				if (getted == null || !getted.Validate()) return false;
				
				return bottomInterface.adapter.IsSelected( getted.id );
#pragma warning restore
				/*  asda = InstanceID.GET_PATHsList( bottomInterface.adapter.pluginname );
				for (int i = 0 ; i < InstanceID.GUIDsList.Count ; i++)
				{   ts = InstanceID.GUIDsList[i];
				  var getted = bottomInterface.adapter.GetHierarchyObjectByGUID(ref ts,  asda[i]);
				  if (ts != InstanceID.GUIDsList[i])
				  {   InstanceID.GUIDsList[i] = ts;
				      bottomInterface.adapter.ON_GUID_BACKCHANGED();
				  }
				  if (getted == null || !getted.Validate()) return true;
				  if (!bottomInterface.adapter.IsSelected( getted.id )) return false;
				}
				return true;*/
			}
			public override bool IsSelectablePlus()
			{	return true;
			}
			public override bool IsSelectableMinus()
			{	return true;
			}
			
			//while only the first object is checked for optimization
			public override string ToString()
			{	if ( InstanceID == null || !bottomInterface.INT32_ACTIVE( InstanceID ) ) return "-";
			
				return bottomInterface.INT32_TOSTRING( InstanceID );
				
			}
			
			public override string FullString()
			{	return m_FullString;
			}
			
			public override bool IsValid()
			{	if ( InstanceID == null || bottomInterface.INT32_ISNULL( InstanceID ) || bottomInterface.INT32_COUNT( InstanceID ) == 0 ) return false;
			
				return bottomInterface.INT32_ACTIVE( InstanceID );
			}
		}
		
		
		
		private class HierarchyMemory : MemoryRoot {
		
			string[] GUIDids;
			string[] PATHids;
			GameObject[] ids;
			private string name;
			
			public HierarchyMemory( BottomInterface bottomInterface ) : base( bottomInterface )
			{
			}
			
			public override void SetObjectValues( object ob, int arrayIndex )
			{	if ( ob == null )
				{	this.ids = null;
					return;
				}
				
				var ids = (HierarchySnapShotArray)ob;
				this.ArrayIndex = arrayIndex;
				this.RectBindIndex = arrayIndex;
				this.name = ids.name;
				this.ids = ids.array;
#pragma warning disable
				this.GUIDids = ids.GUIDarray;
#pragma warning restore
				this.PATHids = ids.PATHarray;
				
			}
			
			
			
			public override bool OnClick( bool par1, int scene )
			{	if ( Application.isPlaying ) return true;
			
				if ( GUIDids == null ) GUIDids = new string[0];
				
				if ( GUIDids.Length != PATHids.Length ) PATHids = new string[GUIDids.Length];
				
				bottomInterface.SET_EXPAND_GO_SNAPSHOT( ids, GUIDids, PATHids, scene );
				
				return true;
			}
			
			public override bool IsSelectedHadrScan()
			{	return false;
			}
			public override bool IsSelectablePlus()
			{	return false;
			}
			public override bool IsSelectableMinus()
			{	return false;
			}
			
			public override string ToString()
			{	return name;
			}
			
			//  bool? validCache = null;
			public override bool IsValid()
			{	return ids != null;
			}
			
			public override string FullString()
			{	return name;
			}
			
		}
	}
}
}
