
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

	int MAX20 { get { return HierParams.MAX_SELECTION_ITEMS; } }
	
	internal BottomInterface bottomInterface;
	
	internal sealed partial class BottomInterface {
	
	
	
	
	
	
	
	
	
		internal float DRAW_CUSTOM_MINHEIGHT( BottomController controller )     //  return LINE_HEIGHT(controller.IS_MAIN, true) * adapter.par.BOTTOM_MAXCUSTOMROWS;
		{	return LINE_HEIGHT( controller.IS_MAIN, true ) * RowsParams[0].Rows;
		}
		
		
		
		internal void DRAW_BY_INDEX( ISET_ROW row, Rect line, BottomController controller, int scene, bool drawWindowPanel = true )
		{	switch ( row.PluginID )
			{	case 0: DRAW_CUSTOM( line, row.RowHeight, controller, scene, drawWindowPanel = true ); break;
			
				case 1: DRAW_LAST( line, row.RowHeight, controller, scene, drawWindowPanel = true ); break;
				
				case 2: DRAW_HIER( line, row.RowHeight, controller, scene, drawWindowPanel = true ); break;
				
				case 3: DRAW_SCENE( line, row.RowHeight, controller, scene, drawWindowPanel = true ); break;
			}
		}
		
		
		internal void DRAW_CUSTOM( Rect line, int LH, BottomController controller, int scene, bool drawWindowPanel = true )
		{
		
			if ( !controller.IS_MAIN ) line = DO_DOCKABLE_BUTTON( line, _6__BottomWindow_BottomInterfaceWindow.TYPE.CUSTOM, controller, scene );
			
			// DoCustom( drawWindowPanel ? DO_DOCKABLE_BUTTON( line , _6__BottomWindow_BottomInterfaceWindow.TYPE.CUSTOM , controller , scene ) : line , LH , controller , scene );
			DoCustom( line, LH, controller, scene );
			// DoCustom( DO_DOCKABLE_BUTTON( line, BottomInterfaceWindow.TYPE.CUSTOM, controller ), LH, controller );
		}
		
		internal float DRAW_LAST_MINHEIGHT( BottomController controller )     //  return LINE_HEIGHT(controller.IS_MAIN) * adapter.par.BOTTOM_MAXLASTROWS;
		{	return LINE_HEIGHT( controller.IS_MAIN ) * RowsParams[1].Rows;
		}
		internal void DRAW_LAST( Rect line, int LH, BottomController controller, int scene, bool drawWindowPanel = true )
		{	// DoLast( drawWindowPanel ? DO_DOCKABLE_BUTTON( line , _6__BottomWindow_BottomInterfaceWindow.TYPE.LAST , controller , scene ) : line , LH , controller , scene );
			DoLast( line, LH, controller, scene );
		}
		
		
		internal void DRAW_HIER( Rect line, int LH, BottomController controller, int scene, bool drawWindowPanel = true )         // DoScenes( line, LH, controller);
		{	DoHier( line, LH, controller, scene );
		}
		
		
		internal void DRAW_SCENE( Rect line, int LH, BottomController controller, int scene, bool drawWindowPanel = true )
		{	DoScenes( line, LH, controller, scene );
		}
		
		
		
		Rect DO_DOCKABLE_BUTTON( Rect line, _6__BottomWindow_BottomInterfaceWindow.TYPE type, BottomController controller, int scene )
		{
		
			frr = line;
			frr.width = EditorGUIUtility.singleLineHeight;
			DRAW_CATEGORY( frr, controller, scene );
			line.x += frr.width;
			line.width -= frr.width;
			return line;
		}
		
		
		
		Color WHITE = new Color(1, 1, 1, 1);
		
		Rect DOCK1, DOCK2;
		// GUIContent DO_DOCKABLE_BUTTON_content = new GUIContent() { tooltip = "Open Dockable Window" };
		//    GUIContent DO_DOCKABLE_BUTTON_HDES = new GUIContent() { tooltip = "Show all Descriptions in Hierarchy Window" };
		//  GUIContent DO_DOCKABLE_BUTTON_WDES = new GUIContent() { tooltip = "Show all Descriptions in Dockable Window" };
		/* Rect DO_DOCKABLE_BUTTON( Rect line , _6__BottomWindow_BottomInterfaceWindow.TYPE type , BottomController controller , int scene ) {
		     var max = type == _6__BottomWindow_BottomInterfaceWindow.TYPE.CUSTOM
		               || type == _6__BottomWindow_BottomInterfaceWindow.TYPE.HIER ? DRAW_CUSTOM_MINHEIGHT(controller) : DRAW_LAST_MINHEIGHT(controller);
		
		
		     var IS = (int)Math.Min(10 * adapter.FACTOR_8() * 2, max) / 2;
		
		     frr = line;
		     frr.x = frr.x + frr.width - IS;
		     frr.y += 2;
		     frr.height = frr.width = IS;
		     DOCK1 = frr;
		     DOCK2 = frr;
		     DOCK2.y += DOCK2.height;
		
		
		     Shrink( ref DOCK1 , -1 );
		     GUI.DrawTexture( DOCK1 , adapter.GetIcon( "HIPERGRAPH_DOCK" ) );
		     EditorGUIUtility.AddCursorRect( DOCK1 , MouseCursor.Link );
		     /1*
		                     if (GUI.Button(frr, ""))
		                     {
		
		                         Debug.Log("ASD");
		                         BottomInterfaceWindow.ShowW(type);
		                     }*1/
		     if ( Event.current.type == EventType.MouseDown && DOCK1.Contains( Event.current.mousePosition ) ) {
		         controller.selection_button = 50 + (int)type;
		         controller.selection_window = adapter.window();
		         var captureRect = DOCK1;
		         controller.selection_action = ( mouseUp , deltaTIme ) => {
		             if ( mouseUp && captureRect.Contains( Event.current.mousePosition ) ) {
		                 adapter.bottomInterface.GET_BOOKMARKS( ref list , scene );
		                 var cat = list[controller.GetCategoryIndex(scene)].name;
		                 _6__BottomWindow_BottomInterfaceWindow.ShowW( adapter , type , cat );
		             }
		             return false;
		         }; // ACTION
		     }
		     GUI.Label( DOCK1 , DO_DOCKABLE_BUTTON_content );
		     Shrink( ref DOCK1 , -3 );
		     if ( DOCK1.Contains( Event.current.mousePosition ) && controller.selection_button == 50 + (int)type ) {
		         GUI.DrawTexture( DOCK1 , adapter.GetIcon( "HIPERUI_BUTTONGLOW" ) );
		     }
		
		
		
		
		     if ( type == _6__BottomWindow_BottomInterfaceWindow.TYPE.CUSTOM || type == _6__BottomWindow_BottomInterfaceWindow.TYPE.HIER ) {
		         Shrink( ref DOCK2 , -1 );
		         GUI.DrawTexture( DOCK2 , adapter.GetIcon( "BOTTOM_DESBUT" ) );
		         EditorGUIUtility.AddCursorRect( DOCK2 , MouseCursor.Link );
		         /1*
		                         if (GUI.Button(frr, ""))
		                         {
		
		                             Debug.Log("ASD");
		                             BottomInterfaceWindow.ShowW(type);
		                         }*1/
		         if ( Event.current.type == EventType.MouseDown && DOCK2.Contains( Event.current.mousePosition ) ) {
		             controller.selection_button = 50 + (int)type;
		             controller.selection_window = adapter.window();
		             var captureRect = DOCK2;
		             controller.selection_action = ( mouseUp , deltaTIme ) => {
		                 if ( mouseUp && captureRect.Contains( Event.current.mousePosition ) ) {
		                     if ( type == _6__BottomWindow_BottomInterfaceWindow.TYPE.CUSTOM ) {
		                         if ( controller.IS_MAIN ) adapter.par.BottomParams.SHOW_ALL_DESCRIPTIONS_INHIER = !adapter.par.BottomParams.SHOW_ALL_DESCRIPTIONS_INHIER;
		                         else adapter.par.BottomParams.SHOW_ALL_DESCRIPTIONS_INWIN = !adapter.par.BottomParams.SHOW_ALL_DESCRIPTIONS_INWIN;
		
		                         adapter.SavePrefs();
		                         adapter.RepaintWindowInUpdate();
		                     }
		
		                     if ( type == _6__BottomWindow_BottomInterfaceWindow.TYPE.HIER ) {   // ** SAVE HIER ** //
		                         // ** SAVE HIER ** //
		                         // ** SAVE HIER ** //
		                         // ** SAVE HIER ** //
		                         // ** SAVE HIER ** //
		                     }
		
		                 }
		                 return false;
		             }; // ACTION
		         }
		         GUI.Label( DOCK2 , DO_DOCKABLE_BUTTON_HDES );
		         Shrink( ref DOCK2 , -3 );
		         if ( DOCK2.Contains( Event.current.mousePosition ) && controller.selection_button == 50 + (int)type ) {
		             GUI.DrawTexture( DOCK2 , adapter.GetIcon( "HIPERUI_BUTTONGLOW" ) );
		         }
		
		
		     }
		
		
		
		
		
		     // line.x += IS;
		     line.width -= IS;
		     return line;
		 }
		
		
		     */
		
		
		
		void FoldActions( Action < bool? > release )
		{	if ( !Adapter.LITE && Event.current.type == EventType.MouseDown && hiperRect.Contains( Event.current.mousePosition ) )
			{	HierarchyController.selection_button = DRAW_FOLD_ICONS_CONTROLID + HYPER_OFFSET;
			
				//var captureID = HierarchyController.selection_button;
				HierarchyController.selection_window = adapter.window();
				var captureRect = hiperRect;
				HierarchyController.selection_action = ( mouseUp, deltaTIme ) =>
				{	if ( mouseUp && captureRect.Contains( Event.current.mousePosition ) )
					{	release( null );
					}
					
					return Event.current.delta.x == 0 && Event.current.delta.x == 0;
					
				}; // ACTION
			}
			
			if ( Event.current.type == EventType.Repaint ) //HOVER
				if ( HierarchyController.selection_action != null && HierarchyController.selection_button != null
				        && HierarchyController.selection_button == DRAW_FOLD_ICONS_CONTROLID + HYPER_OFFSET )
					GUI.DrawTexture( hiperRect, adapter.GetIcon( "BUTBLUE" ) );
		}
		
		//  Color alpha22 = new Color(1, 1, 1, 0.3f);
		
		bool anycatsenable
		{	get
			{	return (adapter.par.SHOW_SCENES_ROWS ||
				        adapter.par.SHOW_HIERARCHYSLOTS_ROWS ||
				        adapter.par.SHOW_LAST_ROWS ||
				        adapter.par.SHOW_BOOKMARKS_ROWS);
			}
		}
		
		Vector2 v2offset;
		float CalcWidth( int? fontSIze = null )
		{	//                 var al = Adapter.GET_SKIN().label.alignment;
			//                 var fs = Adapter.GET_SKIN().label.fontSize;
			//                 Adapter.GET_SKIN().label.alignment = TextAnchor.MiddleLeft;
			labelLeft8style.fontSize = fontSIze ?? adapter.FONT_8();
			
			float w = labelLeft8style.CalcSize(calcConetnt).x;
			
			//                 Adapter.GET_SKIN().label.alignment = al;
			//                 Adapter.GET_SKIN().label.fontSize = fs;
			
			return w;
		}
		
		static GUIContent calcConetnt = new GUIContent();
		static string ShowContent = "Show % Interface";
		static string HideContent = "Hide % Interface";
		static GUIContent GetContent( bool enable, string replace )
		{	if ( enable ) return new GUIContent( HideContent.Replace( "%", replace ) );
		
			return new GUIContent( ShowContent.Replace( "%", replace ) );
		}
		
		
		
		void SET_SELECT_OBECJT( object _value )
		{	var value =  _value as Object;
		
			if ( !value ) return;
			
			var result = new[] { (Object)value };
			
			if ( Event.current.control || Event.current.shift )
			{	Selection.objects = Selection.objects.Concat( result ).ToArray();
			}
			
			else
				if ( Event.current.alt )
				{	Selection.objects = Selection.objects.Except( result ).ToArray();
				}
				
				else
				{	Selection.objects = result;
				}
		}
		void SET_DRAG_OBECJT( object _value )
		{	var value =  _value as Object;
		
			// Debug.Log( value );
			if ( !value ) return;
			
			var result = new[] { (Object)value };
			
			SetDragData( result, MemType.other );
			DragAndDrop.StartDrag( "Dragging GameObject" );
			EventUse();
			
			ClearAction();
			HierarchyController.REPAINT( adapter );
		}
		
		
		
		
		
		Rect mTotalRectGet2;
		
		Color leftFixColorPro = new Color32(45, 45, 45, 255);
		Color leftFixColorPersonal = new Color32(170, 170, 170, 255);
		
		
		// internal Rect? lastBottomRect;
		public void BottomPaintGUI( Adapter.HierarchyObject o, Rect
		                            selectRect )      // if (!WasPaint.ContainsKey(GUIUtility.keyboardControl) || WasPaint[GUIUtility.keyboardControl] || window() == null) return;
		{
		
		
			selectRect.x += adapter.PREFAB_BUTTON_SIZE;
			
			
			if ( Event.current.type != EventType.Repaint )     //var tempNavRect = GetNavigatorRect( /*treeView,*/ selectRect.x + selectRect.width);
			{
			
			
				//#TOGO commmented - then hypergraph expanding left offset reseted to zero
				//     lastBottomRectUI = tempNavRect;
				//    lastBottomRectSelectLine = selectRect;
				// adapter.TEMP_LEFT_CACHE_FOR_BOTTOM_AFTER_REPAINT = adapter.TOTAL_LEFT_PADDING_FORBOTTOM;
				
				return;
			}
			
			/*
			selectRect.width += adapter. PREFAB_BUTTON_SIZE - adapter.TOTAL_LEFT_PADDING;*/
			// selectRect.width += adapter. PREFAB_BUTTON_SIZE;
			
			
			
			
			
			var w = adapter.window();
			
			if ( adapter.oldScroll.HasValue )
			{	return;
			}
			
			if ( !w ) return;
			
			
			//print(selectRect.y + "    " + selectRect.height + "    " + ContentSize.y + "    " + scrollPos.y);
			
			
			var NeedDraw = selectRect.y + selectRect.height * 1.5f >= adapter.ContentSize.y - HEIGHT;
			//var NeedDraw = selectRect.y + selectRect.height * 1.5f >= adapter.HierWinScrollPos.y + ((Rect) adapter.m_VisibleRect.GetValue( treeView2 )).height - HEIGHT;
			//var NeedDraw = mTotalRectGet2.height - selectRect.height < selectRect.y - adapter.HierWinScrollPos.y;
			
			if ( !NeedDraw )     // var pos = w.position;
			{	// MonoBehaviour.print((pos.height - EditorGUIUtility.singleLineHeight) + " " + (selectRect.y - scrollPos.y) + " " + (selectRect.y - scrollPos.y + selectRect.height));
				var treeView2 = adapter.m_TreeView(w);
				
				if ( treeView2 != null )
				{	mTotalRectGet2 = (Rect)adapter.m_TotalRect.GetValue( treeView2 );
				
					if ( adapter.UNITY_5_5 )
						NeedDraw = mTotalRectGet2.height - selectRect.height < selectRect.y - adapter.HierWinScrollPos.y
						           /* && pos.height - 17 < selectRect.y - scrollPos.y + selectRect.height*/;
					else
					{	NeedDraw = mTotalRectGet2.height - selectRect.height < selectRect.y - adapter.HierWinScrollPos.y;
						/* && pos.height - 17 < selectRec*/
						// NeedDraw = pos.height - 17 - parLINE_HEIGHT + HEIGHT < selectRect.y - scrollPos.y/* && pos.height - 17 < selectRect.y - scrollPos.y + selectRect.height*/;
					}
				}
				
				else       //  NeedDraw = true;
				{
				}
				
				
				
			}
			
			
			
			//   MonoBehaviour.print(NeedDraw + " " + selectRect.y);
			
			/*  if (!NeedDraw) return;
			
			  if (!WasPaint.ContainsKey(GUIUtility.keyboardControl)) return;
			  if (WasPaint[GUIUtility.keyboardControl] == null) WasPaint[GUIUtility.keyboardControl] = selectRect.y;
			  //  if (WasPaint[GUIUtility.keyboardControl].Value == selectRect.y) print(window());
			  if (WasPaint[GUIUtility.keyboardControl].Value != selectRect.y) return;
			  // print(selectRect.y + " target=" + ContentSize.y);*/
			// WasPaint[GUIUtility.keyboardControl] = true;
			// wasPaint = true;
			
			//Rect rect = GUILayoutUtility.GetRect(new GUIContent("ghjg"), EditorStyles.toolbarDropDown, null);
			
			
			if ( !NeedDraw ) return;
			
			if ( !adapter.WasPaint.ContainsKey( GUIUtility.keyboardControl ) ) return;
			
			if ( lastRect == null ) lastRect = w.position;
			
			// lastBottomRect = selectRect;
			
			if ( adapter.WasPaint[GUIUtility.keyboardControl] == null ) adapter.WasPaint[GUIUtility.keyboardControl] = selectRect.y;
			
			//  if (WasPaint[GUIUtility.keyboardControl].Value == selectRect.y) print(window());
			if ( lastRect.Value.height == w.position.height &&
			        adapter.WasPaint[GUIUtility.keyboardControl].Value != selectRect.y ) return;
			        
			if ( lastRect.Value.height != w.position.height )
			{	lastRect = w.position;
			}
			
			
			// MonoBehaviour.print(HEIGHT);
			
			
			
			
			if ( !cacheInit ) RefreshMemCache( LastActiveScene.GetHashCode() );
			
			
			if ( Event.current.type == EventType.Repaint)
			{	if ( !adapter.ENABLE_BOTTOMDOCK_PROPERTY && HEIGHT == 0 || adapter.DISABLE_DES() /*DISABLE_DESCRIPTION(o)*/ || adapter.BrakeBottom )
				{
				
				}
				else
				{
				
					try     //#endif
					{	var bc = GUI.color;
						GUI.color *= new Color( 1, 1, 1, adapter.BOTTOM_CONTRAST );
						//  NEW_BOTTOM( selectRect, o, w );
						OLD_PAINT( selectRect, w );
						GUI.color = bc;
					}
					
					catch ( Exception ex )
					{
					
						adapter.logProxy.LogError( "BottomInterface " + ex.Message + " " + ex.StackTrace );
					}
				}
				
				
				
				
				adapter.END_LABLES();
				
				
				if ( adapter. __GUI_POSTSHOT && o.Validate()  /* && Event.current.type == EventType.Repaint*/ )
				{	var ta = adapter.__GUI_POSTSHOTAC;
					adapter.__GUI_POSTSHOTAC = null;
					adapter.__GUI_POSTSHOT = false;
					ta();
				}
				
				adapter.DRAW_GL();
				
				
				
			}
			
		}
		
		static void ROUND_RECT( ref Rect rect )
		{	rect.x = Mathf.FloorToInt( rect.x );
			rect.y = Mathf.FloorToInt( rect.y );
			rect.width = Mathf.FloorToInt( rect.width );
			rect.height = Mathf.FloorToInt( rect.height );
		}
		
		/*  Rect frr;
		  private void DoLines( Rect foldOutRect , EditorWindow win )      // Debug.Log( LastActiveScene.isLoaded );
		  {
		      if ( adapter.DISABLE_DESCRIPTION( (LastActiveScene) ) ) return;
		
		      var line = foldOutRect;
		
		
		
		      line.height = GRAPH_HEIGHT();
		      if ( ENABLE_HYPERGUI() ) hyperGraph.DRAW( line , hyperGraph.HierHyperController , win );
		      if ( ENABLE_FAVORGUI() ) favorGraph.DRAW( line , favorGraph.HierFavorController , win );
		      line.y += line.height;
		      // var LH = LINE_HEIGHT(true, true);
		
		      HierarchyController.REFERENCE_WINDOW = adapter.window();
		
		      if ( HEIGHT <= ___REFERENCE_HEIGHT_AUTOHIDE() ) return;
		
		      SORT_DRAW_ROWS();
		
		      for ( int __index = 0 ; __index < DRAW_INDEX.Length ; __index++ ) {
		          var i = DRAW_INDEX[__index];
		
		          if ( !RowsParams[i].Enable ) continue;
		
		          line.height = RowsParams[i].FULL_HEIGHT;
		          DRAW_BY_INDEX( RowsParams[i] , line , HierarchyController , LastActiveScene.GetHashCode() );
		
		          if ( RowsParams[i].PluginID == PLUGIN_ID.BOOKMARKS ) HierarchyController.CustomLineRect = line;
		
		          line.y += line.height;
		      }
		
		
		      / * line.height = LINE_HEIGHT(true, true) * adapter.par.BOTTOM_MAXCUSTOMROWS;
		       DRAW_CUSTOM(line, LH, HierarchyController);
		
		
		       HierarchyController.CustomLineRect = line;
		       line.y += line.height;
		
		
		
		
		       LH = LINE_HEIGHT(true);
		       line.height = LINE_HEIGHT(true) * adapter.par.BOTTOM_MAXLASTROWS;
		       DRAW_LAST(line, LH, HierarchyController);
		
		
		       line.y += line.height;
		       line.height = LINE_HEIGHT(true);
		       DoScenes(line, LH, HierarchyController);* /
		
		
		      //  refStyle = EditorStyles.miniButtonRight;
		      //  refStyle = EditorStyles.textArea;
		      //  refStyle = EditorStyles.toolbarTextField;
		      //  refStyle = EditorStyles.toolbar;
		  }*/
		
	}
	
	
	private class BottomInterfaceImpl { } /*: BottomInterface { }*/
}
}