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

	public const int INTERFACE_SIZE = 27;
	public static readonly GUIContent HyperGraphClose_Content = new GUIContent() { tooltip = "Close" };
	public static readonly GUIContent HyperGraphWindow_Content = new GUIContent() { tooltip = "Open Dockable Window" };
	
	
	
	static GUIStyle HIPERUI_BUTTONGLOW;
	static Texture2D ZOOM_MINUS;
	static Texture2D ZOOM_PLUS;
	static Texture2D ZOOM_THUMB;
	static Texture2D HIPERUI_CLOSE;
	static Texture2D HIPERGRAPH_DOCK;
	static GUIStyle m_HIPERUI_GAMEOBJECT;
	static GUIStyle m_HIPERUI_INOUT_A;
	static GUIStyle m_HIPERUI_INOUT_B;
	static GUIStyle m_HIPERUI_LINE_BLUEGB;
	static GUIStyle m_HIPERUI_LINE_BLUEGB_PERSONAL;
	static GUIStyle m_HIPERUI_LINE_BOX;
	static GUIStyle m_HIPERUI_LINE_RDTRIANGLE;
	static GUIStyle m_HIPERUI_MARKER_BOX;
#pragma warning disable
	static GUIStyle ARROW;
#pragma warning restore
	bool STYLES_WAS_INIT = false;
	
	static void INIT_STYLES( Adapter adapter )
	{	if ( adapter.STYLES_WAS_INIT ) return;
	
	
		ARROW = adapter.InitializeStyle( "ARROW", 0.4f, 0.4f, 0, 0 );
		
		
		m_HIPERUI_GAMEOBJECT = adapter.InitializeStyle( Adapter.ICONID.HIPERUI_GAMEOBJECT, 0f, 0f, 0, 0 );
		m_HIPERUI_GAMEOBJECT.fontStyle = FontStyle.Bold;
		//HIPERUI_GAMEOBJECT.padding.left = 16;
		
		// HIPERUI_GAMEOBJECT.border.left = 30;
		m_HIPERUI_GAMEOBJECT.focused.textColor = m_HIPERUI_GAMEOBJECT.active.textColor =
		            m_HIPERUI_GAMEOBJECT.hover.textColor =
		                m_HIPERUI_GAMEOBJECT.normal.textColor = new Color32( 42, 42, 42, 255 );
		                
		                
		m_HIPERUI_LINE_RDTRIANGLE =
		    adapter.InitializeStyle( ICONID.HIPERUI_LINE_RDTRIANGLE, 0.25f, 0.5f, 0.25f, 0.7f );
		    
		HIPERUI_BUTTONGLOW = adapter.InitializeStyle( ICONID.HIPERUI_BUTTONGLOW, 0.25f, 0.25f, 0.25f, 0.25f );
		ZOOM_MINUS = adapter.GetIcon( ICONID.ZOOM_MINUS );
		ZOOM_PLUS = adapter.GetIcon( ICONID.ZOOM_PLUS );
		ZOOM_THUMB = adapter.GetIcon( ICONID.ZOOM_THUMB );
		HIPERUI_CLOSE = adapter.GetIcon( ICONID.HIPERUI_CLOSE );
		HIPERGRAPH_DOCK = adapter.GetIcon( ICONID.HIPERGRAPH_DOCK );
		m_HIPERUI_INOUT_A = adapter.InitializeStyle( ICONID.HIPERUI_INOUT_A, 0 );
		m_HIPERUI_INOUT_B = adapter.InitializeStyle( ICONID.HIPERUI_INOUT_B, 0 );
		
		m_HIPERUI_LINE_BLUEGB = adapter.InitializeStyle( ICONID.HIPERUI_LINE_BLUEGB, 0.25f, 0.25f, 0, 0 );
		m_HIPERUI_LINE_BLUEGB.alignment = TextAnchor.MiddleRight;
		m_HIPERUI_LINE_BLUEGB.padding.right = 4;
		
		m_HIPERUI_LINE_BLUEGB_PERSONAL = adapter.InitializeStyle( ICONID.HIPERUI_LINE_BLUEGB + " PERSONAL", 0.25f, 0.25f, 0, 0 );
		m_HIPERUI_LINE_BLUEGB_PERSONAL.alignment = TextAnchor.MiddleRight;
		m_HIPERUI_LINE_BLUEGB_PERSONAL.padding.right = 4;
		m_HIPERUI_LINE_BLUEGB_PERSONAL.normal.textColor = new Color32( 40, 40, 40, 255 );
		
		m_HIPERUI_LINE_BOX = adapter.InitializeStyle( ICONID.HIPERUI_LINE_BOX, 0.4f, 0.4f, 0.4f, 0.4f );
		m_HIPERUI_MARKER_BOX = adapter.InitializeStyle( ICONID.HIPERUI_MARKER_BOX, 0.25f, 0.25f, 0.25f, 0.25f );
		
		if ( adapter.wasIconsInitialize ) adapter.STYLES_WAS_INIT = true;
	}
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	static Color BLUE = new Color( 0.05f, 0.6f, 0.9f );
	
	internal partial class HyperGraph : Adapter.BottomInterface.BOTTOM_GRAPH {
	
		internal HyperGraph( Adapter adapter, BottomInterface btInterface )
		{	this.adapter = adapter;
			this.bottomInterface = btInterface;
			
			SIZE = new SIZES_CLASS( adapter );
			
			WindowHyperController = new Adapter.HyperGraph.HyperControllerWindow( adapter );
			HierHyperController = new HyperControllerHierarchy( adapter );
		}
		
		
		internal override void SWITCH_ACTIVE_SCAN( bool? overrideActive )
		{	if ( bottomInterface._HIPER_HEIGHT == null )
				bottomInterface._HIPER_HEIGHT = adapter.HYPER_ENABLE() ? adapter.par.HiperGraphParams.HEIGHT : 0;
				
			adapter.CHECK_SMOOTH_HEIGHT();
			
			adapter.par.HiperGraphParams.ENABLE = overrideActive ?? !adapter.par.HiperGraphParams.ENABLE;
			
			if ( HYPER_FULL_ENABLE() && !WASSCAN )
			{	comps = null;
				// StoptBroadcasting();
				StartBroadcasting();
			}
		}
		
		internal override void DRAG_PERFORMER_SCAN()
		{	if ( adapter.IS_HIERARCHY() )
			{	var n = DragAndDrop.objectReferences.First( g => g is GameObject && ((GameObject)g).scene.IsValid() );
				CHANGE_SELECTION_OVVERIDE( true, n );
			}
			
			else
			{	var n = DragAndDrop.objectReferences.First( g => !string.IsNullOrEmpty( adapter.bottomInterface.INSTANCEID_TOGUID( g.GetInstanceID() ) ) );
				CHANGE_SELECTION_OVVERIDE( true, n );
			}
		}
		
		public override void Update()
		{	if ( SCANNING_COMPS.Count != 0 )
			{	lock ( SCANNING_COMPS )
				{	for ( int i = SCANNING_COMPS.Count - 1 ; i >= 0 ; i-- )
					{	var objectDisplay = SCANNING_COMPS[i];
					
						if ( objectDisplay.WasAccessorInitialize_InMainThread )     //   SCANNING_COMPS.RemoveAt(i);
						{	continue;
						}
						
						var ob = EditorUtility.InstanceIDToObject( objectDisplay.gameObjectId ) as GameObject;
						
						if ( !ob ) continue;
						
						/*  lock (objectDisplay)
						                {*/
						
						if ( objectDisplay.AllowInitialize_InMainThread() )
						{	objectDisplay.InitializeAccessor_InMainThread();
							//   SCANNING_COMPS.RemoveAt(i);
							// SCANNING_COMPS_REMOVER.Add(ob.GetInstanceID());
						}
						
						// }
					}
					
					SCANNING_COMPS.Clear();
				}
			}
			
			/* foreach (var objectDisplay in SCANNING_COMPS)
			         {
			
			         }*/
			/*  if (SCANNING_COMPS_REMOVER.Count != 0)
			          {
			              foreach (var i in SCANNING_COMPS_REMOVER)
			              {
			                  SCANNING_COMPS.Remove(i);
			              }
			              SCANNING_COMPS_REMOVER.Clear();
			          }*/
			/* if (BottomInterface.HiperGraph.currentAction != null)
			         {
			             Repaint();
			         }*/
			
			
			base.Update();
			
			if ( HYPER_FULL_ENABLE() || bottomInterface.m_HIPER_HEIGHT() != 0 ) CalcBroadCast();
			
		}
		internal void CHECK_SCAN()
		{	if ( !WASSCAN )
			{	comps = null;
				// StoptBroadcasting();
				StartBroadcasting();
			}
		}
		
		
		/*    int framecount = 0;
		          double lastTime;*/
		
		const float GR_SIZE = 120;
		
		/*  float GR_SIZE {
		            get { return 120 * HALF_SCALE(); }
		        }*/
		
		
		
		
		
		internal class HyperControllerHierarchy : BottomInterface.UniversalGraphController {
			public HyperControllerHierarchy( Adapter adapter ) : base( adapter )
			{
			}
			
			public override bool MAIN
			{	get { return true; }
			}
			
			internal override bool hide_hierarchy_ui_buttons
			{	get { return false; }
			}
			
			internal override float HEIGHT
			{	get { return adapter.par.HiperGraphParams.HEIGHT; }
			
				set { adapter.par.HiperGraphParams.HEIGHT = value; }
			}
			
			internal override float WIDTH
			{	get { return adapter.window() == null ? 0 : adapter.window().position.width; }
			
				set { }
			}
			
			/*internal override float DEFAULT_WIDTH(BottomInterface.UniversalGraphController controller)
			{
			  return controller.MAIN ? adapter.par.HiperGraphParams.SCALE : adapter.par.HiperGraphParams.WINDIOW_SCALE;
			
			  //return adapter.par.HiperGraphParams.SCALE;
			}*/
		}
		
		internal Adapter.HyperGraph.HyperControllerWindow WindowHyperController;
		
		internal HyperControllerHierarchy HierHyperController;
		
		Rect RECT = new Rect();
		Rect LOCAL_RECT = new Rect();
		
		// Rect CONTENT_RECT;
		Rect InterfaceRect = new Rect();
		
		
		
		
		
		Color HIPER_INTERFACE = new Color( 0.75f, 0.75f, 0.75f, 0.5f );
		
		
		
		
		internal Rect CAPTURE_CLIP_RECT;
		BottomInterface.UniversalGraphController CURRENT_CONTROLLER;
		float CURRENT_SCALE = 1;
		
		internal override void EXTERNAL_HYPER_DRAWER( Rect lineRect, BottomInterface.UniversalGraphController controller, EditorWindow win)
		{
#pragma warning disable
		
			if ( Adapter.LITE ) return;
			
#pragma warning restore
			
			controller.tempWin = win;
			
			
			CURRENT_CONTROLLER = controller;
			CURRENT_SCALE = controller.MAIN ? adapter.par.HiperGraphParams.SCALE : adapter.par.HiperGraphParams.WINDIOW_SCALE;
			
			CHECK_RIGHTCLICK( lineRect );
			
			
			////////////////// DRAG
			dragRect = lineRect;
			dragRect.height = 5;
			
			DRAG( controller );
			
			lineRect.y += dragRect.height;
			lineRect.height -= dragRect.height;
			////////////////// DRAG ////////////////////////////////
			
			
			InterfaceRect = lineRect;
			InterfaceRect.x = InterfaceRect.y = 0;
			InterfaceRect.height = INTERFACE_SIZE / 2;
			
			RECT.width = lineRect.width;
			RECT.height = lineRect.height;
			LOCAL_RECT = RECT;
			LOCAL_RECT.x -= CURRENT_CONTROLLER.scrollPos.x / CURRENT_SCALE;
			LOCAL_RECT.y -= CURRENT_CONTROLLER.scrollPos.y / CURRENT_SCALE;
			LOCAL_RECT.width /= CURRENT_SCALE;
			LOCAL_RECT.height /= CURRENT_SCALE;
			//_mConvertRect( ref LOCAL_RECT );
			
			if ( controller.MAIN ) CAPTURE_CLIP_RECT = lineRect;
			
			////////////////// MAIN
			// CLIP_RECT = lineRect;
			GUI.BeginClip( lineRect );
			
			if ( Event.current.type != EventType.Repaint ) INTERFACE( controller );
			
			
			// GUI.matrix *=  Matrix4x4.Scale(new Vector3(0.5f, 0.5f, 1));
			
			
			INITIALIZE( controller );
			
			
			//CONTENT_RECT.x = scrollPos.x;
			// CONTENT_RECT.y = scrollPos.y;
			// GUI.BeginGroup(CONTENT_RECT);
			
			MAIN( controller );
			
			
			// GUI.matrix = Matrix4x4.Scale(new Vector3(1, 1, 1));
			
			// GUI.EndGroup();
			////////////////// MAIN ////////////////////////////////
			
			if ( Event.current.type == EventType.Repaint ) INTERFACE( controller );
			
			GUI.EndClip();
			
			
			SETUPROOT.ExampleDragDropGUI( adapter, lineRect, null, DRAG_VALIDATOR, DRAG_PERFORMER, dragColor );
			
			////////////////// INTERFACE
			
			//InterfaceRect.y -= 4;
			
			
			/*  lineRect.y += InterfaceRect.height;
			          lineRect.height -= InterfaceRect.height;*/
			////////////////// INTERFACE ////////////////////////////////
		}
		
		//  Rect CLIP_RECT;
		void DRAG( BottomInterface.UniversalGraphController controller )
		{	if ( controller.hide_hierarchy_ui_buttons ) return;
		
			EditorGUIUtility.AddCursorRect( dragRect, MouseCursor.SplitResizeUpDown );
			
			if ( Event.current.type == EventType.Repaint )
			{	adapter.box.Draw( dragRect, false, false, false, false );
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
			
			
			var EVENT_ID = 100;
			
			if ( dragRect.Contains( Event.current.mousePosition ) && Event.current.button == 0 &&
			        Event.current.type == EventType.MouseDown )
			{	EventUse();
				var startPos = EditorGUIUtility.GUIToScreenPoint( Event.current.mousePosition );
				var startHeight = controller.HEIGHT;
				ADD_ACTION( EVENT_ID, null, contains =>
				{	var p = EditorGUIUtility.GUIToScreenPoint( Event.current.mousePosition );
				
					if ( startPos.y == p.y) return false;
					
					//  var newHeight = startHeight + (startPos.y - p.y);
					// Debug.Log( adapter.window().position.height );
					adapter.TEMP_LEFT_CACHE_FOR_BOTTOM = adapter.TOTAL_LEFT_PADDING_FORBOTTOM;
					var oldH = controller.HEIGHT;
					var nh = Mathf.RoundToInt((startHeight + (startPos.y - p.y)));
					nh = CHECK_HEIGHT( nh );
					controller.HEIGHT = nh;
					
					controller.scrollPos.y -= (oldH - controller.HEIGHT) / 2;
					//adapter.SavePrefs();
					// startPos = p;
					adapter.RESET_SMOOTH_HEIGHT();
					
					return true;
				}, contains => { adapter.SavePrefs(); }, controller );
			}
			
			if ( HOVER( EVENT_ID, null, controller ) )
			{	var asd = GUI.color;
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
		GUIStyle __toolbalStyle;
		GUIStyle toolbalStyle
		{	get
			{	if ( __toolbalStyle == null )
				{	__toolbalStyle = new GUIStyle( EditorStyles.toolbarButton );
					__toolbalStyle.fixedHeight = 0;
					__toolbalStyle.alignment = TextAnchor.MiddleLeft;
				}
				
				return __toolbalStyle;
			}
		}
		
		void DRAW_INTERFACE_BUTTON( int EVENT_ID, string label, bool ACTIVE, Rect r, BottomInterface.UniversalGraphController controller )
		{	if ( ACTIVE || HOVER( EVENT_ID, r, controller ) )
			{	var oldC = GUI.color;
				var newC = oldC;
				newC.a = 1;
				GUI.color = asd * newC;
				var active = HOVER( EVENT_ID, r, controller );
				toolbalStyle.fixedHeight = r.height;
				toolbalStyle.Draw( r, label, active, active, active, false );
				GUI.DrawTexture( r, adapter.GetIcon( "BUTBLUE" ) );
				GUI.color = oldC;
			}
			
			else
			{	toolbalStyle.fixedHeight = r.height;
				toolbalStyle.Draw( r, label, false, false, false, false );
			}
			
			EditorGUIUtility.AddCursorRect( r, MouseCursor.Link );
		}
		Color asd;
		void INTERFACE( BottomInterface.UniversalGraphController controller )
		{	var procrect = InterfaceRect;
			var hiderect = InterfaceRect;
			procrect.width /= 2;
			hiderect.width /= 2.5f;
			hiderect.x = InterfaceRect.x + InterfaceRect.width - hiderect.width;
			hiderect.width /= 2;
			var autorefreshrect = hiderect;
			hiderect.x += hiderect.width;
			
			EditorGUIUtility.AddCursorRect( procrect, MouseCursor.Link );
			
			Label( procrect, procContent );
			Label( autorefreshrect, autorefreshContent );
			
			if ( !controller.hide_hierarchy_ui_buttons ) Label( hiderect, autohideContent );
			
			procrect.width /= 5;
			
			
			
			InterfaceRect.y += InterfaceRect.height;
			/*  var hr = InterfaceRect;
			  hr.y += 2;
			 // hr.height -= 4;
			  hr.width /= 2;
			  hr.width /= 3;
			  var HR1 = hr;
			  hr.x += hr.width;
			  var HR2 = hr;
			  hr.x += hr.width;
			  var HR3 = hr;
			  hr.width = InterfaceRect.width / 2 / 5 * 4;
			  hr.x = InterfaceRect.width / 2 + InterfaceRect.width / 10;
			  var HR4 = hr;*/
			
			
			var hr = InterfaceRect;
			hr.y += 2;
			// hr.height -= 4;
			hr.width /= 2;
			hr.width /= 3;
			hr.x = InterfaceRect.width / 2;
			var HR1 = hr;
			hr.x += hr.width;
			var HR2 = hr;
			hr.x += hr.width;
			var HR3 = hr;
			hr.width = InterfaceRect.width / 2 / 5 * 4;
			hr.x = 0;
			var HR4 = hr;
			
			__TOOLTIP.text = "";
			__TOOLTIP.tooltip = "Arrays objects will be included";
			Label( HR1, __TOOLTIP );
			__TOOLTIP.tooltip = "Assets objects will be included";
			Label( HR2, __TOOLTIP );
			__TOOLTIP.tooltip = "References that point to themselves objects will be included";
			Label( HR3, __TOOLTIP );
			__TOOLTIP.tooltip = "Only refs to UnityEvents will be shown";
			Label( HR4, __TOOLTIP );
			
			
			EditorGUIUtility.AddCursorRect( autorefreshrect, MouseCursor.Link );
			
			if ( !controller.hide_hierarchy_ui_buttons )
				EditorGUIUtility.AddCursorRect( hiderect, MouseCursor.Link );
				
			var len = 5;
			
			var EVENT_ID = 110;
			
			if ( Event.current.type == EventType.Repaint )
			{	asd = GUI.color;
				//GUI.color = new Color(1,1,1,0.5f);
				GUI.color *= HIPER_INTERFACE;
				toolbalStyle.fixedHeight = INTERFACE_SIZE / 2;
				//EditorStyles.toolbar.Draw(InterfaceRect, false, false, false, false);
				// Adapter.GET_SKIN().box.Draw(InterfaceRect, false, false, false, false);
				
				
				for ( int i = 0 ; i < len ; i++ )
				{	var st = ((i + 1) * 20) + "%";
				
					if ( i == len - 1 ) st = "∞";
					
					if ( adapter.par.HiperGraphParams.SCANPERFOMANCE == perfomanceArray[i] ||
					        HOVER( EVENT_ID, procrect, controller ) )
					{	var oldC = GUI.color;
						var newC = oldC;
						newC.a = 1;
						GUI.color = asd * newC;
						var active = HOVER( EVENT_ID, procrect, controller );
						toolbalStyle.Draw( procrect, st, active, active, active, false );
						GUI.DrawTexture( procrect, adapter.GetIcon( "BUTBLUE" ) );
						GUI.color = oldC;
					}
					
					else
					{	toolbalStyle.Draw( procrect, st, false, false, false, false );
					}
					
					procrect.x += procrect.width;
					EVENT_ID++;
				}
				
				// if () GUI.DrawTexture(h, Texture2D.whiteTexture);
				if ( adapter.par.HiperGraphParams.AUTOCHANGE || HOVER( EVENT_ID, autorefreshrect, controller ) )
				{	var oldC = GUI.color;
					var newC = oldC;
					newC.a = 1;
					GUI.color = asd * newC;
					var active = HOVER( EVENT_ID, autorefreshrect, controller );
					toolbalStyle.Draw( autorefreshrect, "AutoReload", active, active, active, false );
					GUI.DrawTexture( autorefreshrect, adapter.GetIcon( "BUTBLUE" ) );
					GUI.color = oldC;
				}
				
				else
				{	toolbalStyle.Draw( autorefreshrect, "AutoReload", false, false, false, false );
				}
				
				EVENT_ID++;
				
				if ( !controller.hide_hierarchy_ui_buttons )
				{	if ( adapter.par.HiperGraphParams.AUTOHIDE || HOVER( EVENT_ID, hiderect, controller ) )
					{	var oldC = GUI.color;
						var newC = oldC;
						newC.a = 1;
						GUI.color = asd * newC;
						var active = HOVER( EVENT_ID, hiderect, controller );
						toolbalStyle.Draw( hiderect, "AutoHide", active, active, active, false );
						GUI.DrawTexture( hiderect, adapter.GetIcon( "BUTBLUE" ) );
						GUI.color = oldC;
					}
					
					else
					{	toolbalStyle.Draw( hiderect, "AutoHide", false, false, false, false );
					}
				}
				
				var en = GUI.enabled;
				GUI.enabled = adapter._S_HG_EventsMode == 0;
				DRAW_INTERFACE_BUTTON( ++EVENT_ID, "Arrays", adapter._S_HG_SkipArrays == 0, HR1, controller );
				DRAW_INTERFACE_BUTTON( ++EVENT_ID, "Assets", adapter._S_HG_ShowAssets, HR2, controller );
				DRAW_INTERFACE_BUTTON( ++EVENT_ID, "SelfRef", adapter._S_HG_ShowSelf, HR3, controller );
				GUI.enabled = en;
				DRAW_INTERFACE_BUTTON( ++EVENT_ID, "EVENTS MODE", adapter._S_HG_EventsMode != 0, HR4, controller );
				
				
				
				
				GUI.color = asd;
			}
			
			else
			{	for ( int i = 0 ; i < len ; i++ )
				{	if ( procrect.Contains( Event.current.mousePosition ) && Event.current.button == 0 &&
					        Event.current.type == EventType.MouseDown )
					{	var captureI = i;
						EventUse();
						ADD_ACTION( EVENT_ID, procrect, contains => { return false; }, contains =>
						{	if ( contains )
							{	adapter.par.HiperGraphParams.SCANPERFOMANCE = perfomanceArray[captureI];
								adapter.SavePrefs();
							}
							
						}, controller );
					}
					
					procrect.x += procrect.width;
					EVENT_ID++;
				}
				
				
				if ( autorefreshrect.Contains( Event.current.mousePosition ) && Event.current.button == 0 &&
				        Event.current.type == EventType.MouseDown )
				{	EventUse();
					ADD_ACTION( EVENT_ID, autorefreshrect, contains => { return false; }, contains =>
					{	if ( contains )
						{	adapter.par.HiperGraphParams.AUTOCHANGE = !adapter.par.HiperGraphParams.AUTOCHANGE;
							adapter.SavePrefs();
						}
					}, controller );
				}
				
				EVENT_ID++;
				
				if ( !controller.hide_hierarchy_ui_buttons )
				{	if ( hiderect.Contains( Event.current.mousePosition ) && Event.current.button == 0 &&
					        Event.current.type == EventType.MouseDown )
					{	EventUse();
						ADD_ACTION( EVENT_ID, hiderect, contains => { return false; }, contains =>
						{	if ( contains )
							{	adapter.par.HiperGraphParams.AUTOHIDE = !adapter.par.HiperGraphParams.AUTOHIDE;
								adapter.SavePrefs();
							}
						}, controller );
					}
				}
				
				
				
				var en = GUI.enabled;
				GUI.enabled = adapter._S_HG_EventsMode == 0;
				
				if ( GUI.enabled )
				{	++EVENT_ID;
				
					if ( HR1.Contains( Event.current.mousePosition ) && Event.current.button == 0 && Event.current.type == EventType.MouseDown )
					{	EventUse();
						ADD_ACTION( EVENT_ID, HR1, contains => { return false; }, contains =>
						{	if ( contains )
							{	adapter._S_HG_SkipArrays = 2 - adapter._S_HG_SkipArrays;
								adapter.SavePrefs();
							}
						}, controller );
					}
					
					++EVENT_ID;
					
					if ( HR2.Contains( Event.current.mousePosition ) && Event.current.button == 0 && Event.current.type == EventType.MouseDown )
					{	EventUse();
						ADD_ACTION( EVENT_ID, HR2, contains => { return false; }, contains =>
						{	if ( contains )
							{	adapter._S_HG_ShowAssets = !adapter._S_HG_ShowAssets;
								adapter.SavePrefs();
							}
						}, controller );
					}
					
					++EVENT_ID;
					
					if ( HR3.Contains( Event.current.mousePosition ) && Event.current.button == 0 && Event.current.type == EventType.MouseDown )
					{	EventUse();
						ADD_ACTION( EVENT_ID, HR3, contains => { return false; }, contains =>
						{	if ( contains )
							{	adapter._S_HG_ShowSelf = !adapter._S_HG_ShowSelf;
								adapter.SavePrefs();
							}
						}, controller );
					}
				}
				
				else
				{	++EVENT_ID;
					++EVENT_ID;
					++EVENT_ID;
					
				}
				
				GUI.enabled = en;
				++EVENT_ID;
				
				if ( HR4.Contains( Event.current.mousePosition ) && Event.current.button == 0 && Event.current.type == EventType.MouseDown )
				{	EventUse();
					ADD_ACTION( EVENT_ID, HR4, contains => { return false; }, contains =>
					{	if ( contains )
						{	adapter._S_HG_EventsMode = 1 - adapter._S_HG_EventsMode;
							adapter.SavePrefs();
						}
					}, controller );
				}
				
			}
			
			
			EVENT_ID++;
			
			var restorex = InterfaceRect.x;
			
			if ( !adapter.par.HiperGraphParams.AUTOHIDE )
			{	InterfaceRect.y += InterfaceRect.height + 15;
				InterfaceRect.width = adapter.bottomInterface.LINE_HEIGHT( null ) - 3;
				InterfaceRect.height = adapter.bottomInterface.LINE_HEIGHT( null ) - 3;
				
				//MonoBehaviour.print(Adapter.GET_SKIN().button.normal.textColor);
				
				if ( Event.current.type == EventType.Repaint && EditorGUIUtility.isProSkin )
					adapter.box.Draw( InterfaceRect, false, false, false, false );
					
				DO_BUTTON( controller, InterfaceRect, adapter.bottomInterface.ContentSelBack, CenteredButton, EVENT_ID, DO_UNDO );
				//  Adapter.GET_SKIN().button.Draw(plus, ContentSelBack, false, false, false, plus.Contains(Event.current.mousePosition) && selection_button == 20);
				
				GUI.Label( InterfaceRect, ContentHiperUndo, adapter.STYLE_LABEL_8_middle );
				
				InterfaceRect.x += InterfaceRect.width;
				
				if ( Event.current.type == EventType.Repaint && EditorGUIUtility.isProSkin )
					adapter.box.Draw( InterfaceRect, false, false, false, false );
					
				DO_BUTTON( controller, InterfaceRect, adapter.bottomInterface.ContentSelForw, CenteredButton, EVENT_ID + 1, DO_REDO );
				// Adapter.GET_SKIN().button.Draw(plus, ContentSelForw, false, false, false, plus.Contains(Event.current.mousePosition) && selection_button == 21);
				
				GUI.Label( InterfaceRect, ContenHiperRedo, adapter.STYLE_LABEL_8_middle );
				
				
				InterfaceRect.x += InterfaceRect.width;
				InterfaceRect.x += InterfaceRect.width;
				
				
				if ( Event.current.type == EventType.Repaint  && EditorGUIUtility.isProSkin)
					adapter.box.Draw( InterfaceRect, false, false, false, false );
					
				DO_BUTTON( controller, InterfaceRect, ContentHiperempty, adapter.button, EVENT_ID + 2,
				           REFRESH );
				GUI.DrawTexture( new Rect( InterfaceRect.x, InterfaceRect.y, InterfaceRect.width, InterfaceRect.height  ), adapter.GetIcon( "REFRESH" ) );
				// Adapter.GET_SKIN().button.Draw(plus, ContentSelForw, false, false, false, plus.Contains(Event.current.mousePosition) && selection_button == 21);
				/*   if (Event.current.type == EventType.repaint)*/
				GUI.Label( InterfaceRect, ContenHiperRefresh, adapter.STYLE_LABEL_8_middle );
			}
			
			
			EVENT_ID += 3;
			InterfaceRect.x = restorex;
			
			
			InterfaceRect.y += InterfaceRect.height + 3;
			
			if ( adapter.par.HiperGraphParams.SHOWUPDATINGINDICATOR && CurrentSelection != null )
			{	if ( SKANNING && Event.current.type == EventType.Repaint || WASDRAW )
				{	InterfaceRect.width = InterfaceRect.height = 44;
				
					GUI.DrawTexture( InterfaceRect, adapter.LOADING_TEXTURE() );
					InterfaceRect.y += InterfaceRect.height + 3;
					InterfaceRect.height = adapter.bottomInterface.LINE_HEIGHT( null );
					var prog = InterfaceRect;
					var val = SKANNING_PROGRESS;
					
					if ( !SKANNING && WASDRAW )
					{	WASDRAW = false;
						Repaint( WindowHyperController );
						val = 1;
					}
					
					else
					{	WASDRAW = true;
					}
					
					prog.width *= val;
					GUI.DrawTexture( prog, adapter.GetIcon( "BUTBLUE" ) );
					Label( InterfaceRect, Mathf.RoundToInt( val * 100 ) + "%" );
				}
			}
			
			
			var ACTIVE_RECT = RECT;
			ACTIVE_RECT.height -= INTERFACE_SIZE;
			ACTIVE_RECT.y += INTERFACE_SIZE;
			
			/** CLOASE **/
			if ( !controller.hide_hierarchy_ui_buttons )
			{	var closeRect = ACTIVE_RECT;
				closeRect.width = 14;
				closeRect.height = 14;
				closeRect.x = RECT.width - closeRect.width - 5;
				closeRect.y += 2;
				EditorGUIUtility.AddCursorRect( closeRect, MouseCursor.Link );
				
				Label( closeRect, HyperGraphClose_Content );
				
				EVENT_ID = 200;
				
				if ( Event.current.type == EventType.Repaint )
				{	GUI.DrawTexture( closeRect, HIPERUI_CLOSE );
				}
				
				if ( closeRect.Contains( Event.current.mousePosition ) && Event.current.button == 0 &&
				        Event.current.type == EventType.MouseDown )
				{	EventUse();
					ADD_ACTION( EVENT_ID, closeRect, contains => { return false; }, contains =>
					{	if ( contains ) SWITCH_ACTIVE( false );
					}, controller );
				}
				
				if ( HOVER( EVENT_ID, closeRect, controller ) )
				{	HIPERUI_BUTTONGLOW.Draw( closeRect, false, false, false, false );
				}
				
				/** CLOASE **/
				
				
				/** DOCK **/
				if ( !editorWindow )
				{	closeRect.x -= closeRect.width - 1;
					EditorGUIUtility.AddCursorRect( closeRect, MouseCursor.Link );
					
					
					Label( closeRect, HyperGraphWindow_Content );
					
					EVENT_ID = 201;
					
					if ( Event.current.type == EventType.Repaint )
					{	GUI.DrawTexture( closeRect, HIPERGRAPH_DOCK );
					}
					
					if ( closeRect.Contains( Event.current.mousePosition ) && Event.current.button == 0 &&
					        Event.current.type == EventType.MouseDown )
					{	EventUse();
						ADD_ACTION( EVENT_ID, closeRect, contains => { return false; }, contains =>
						{	if ( contains ) DOCK_HYPER();
						}, controller );
					}
					
					if ( HOVER( EVENT_ID, closeRect, controller ) )
					{	HIPERUI_BUTTONGLOW.Draw( closeRect, false, false, false, false );
					}
				}
				
				/** DOCK **/
			}
			
			/** DRAW_ZOOMER **/
			DRAW_ZOOMER( ACTIVE_RECT, 205, controller );
			/** DRAW_ZOOMER **/
		}
		
		
		
		
		private void DO_BUTTON( BottomInterface.UniversalGraphController controller, Rect rect, GUIContent content, GUIStyle style, int EVENT_ID,
		                        Action action = null )
		{	EditorGUIUtility.AddCursorRect( rect, MouseCursor.Link );
		
			if ( Event.current.type == EventType.Repaint )
			{	style.Draw( rect, content, false, false, false, false );
				TOOLTIP( rect, content );
				
				if ( HOVER( EVENT_ID, rect, controller ) )
				{	var h = rect;
					var GLOW = 8;
					h.x -= GLOW;
					h.y -= GLOW;
					h.width += GLOW * 2;
					h.height += GLOW * 2;
					
					
					HIPERUI_BUTTONGLOW.Draw( h, false, false, false, false );
				}
			}
			
			
			if ( Event.current.type == EventType.MouseDown && Event.current.button == 0 &&
			        rect.Contains( Event.current.mousePosition ) )
			{	EventUse();
				ADD_ACTION( EVENT_ID, rect, contains => { return false; }, contains =>
				{	if ( contains )
					{	if ( action != null )
						{	action();
						}
					}
				}, controller );
			}
		}
		
		
		
		
		bool WASDRAW = false;
		
		
		GUIContent ContentHiperempty = new GUIContent() { };
		GUIContent ContentHiperUndo = new GUIContent() { tooltip = "Previous HyperGraph Selection" };
		GUIContent ContenHiperRedo = new GUIContent() { tooltip = "Forward HyperGraph Selection" };
		GUIContent ContenHiperRefresh = new GUIContent() { tooltip = "Refresh HyperGraph Connections" };
		
		
		private GUIStyle HIPERUI_GAMEOBJECT
		{	get
			{	m_HIPERUI_GAMEOBJECT.padding.left = (int)(16 * CURRENT_SCALE);
				m_HIPERUI_GAMEOBJECT.padding.right = (int)(3 * CURRENT_SCALE);
				return m_HIPERUI_GAMEOBJECT;
			}
			
			set { m_HIPERUI_GAMEOBJECT = value; }
		}
		
		GUIStyle HIPERUI_INOUT_A
		{	get
			{	m_HIPERUI_INOUT_A.padding.left = (int)(3 * CURRENT_SCALE);
				m_HIPERUI_INOUT_A.padding.right = (int)(3 * CURRENT_SCALE);
				return m_HIPERUI_INOUT_A;
			}
			
			set { m_HIPERUI_INOUT_A = value; }
		}
		
		GUIStyle HIPERUI_INOUT_B
		{	get
			{	m_HIPERUI_INOUT_B.padding.left = (int)(3 * CURRENT_SCALE);
				m_HIPERUI_INOUT_B.padding.right = (int)(3 * CURRENT_SCALE);
				return m_HIPERUI_INOUT_B;
			}
			
			set { m_HIPERUI_INOUT_B = value; }
		}
		
		GUIStyle HIPERUI_LINE_BLUEGB
		{	get
			{	m_HIPERUI_LINE_BLUEGB.padding.left = (int)(3 * CURRENT_SCALE);
				m_HIPERUI_LINE_BLUEGB.padding.right = (int)(4 * CURRENT_SCALE);
				return m_HIPERUI_LINE_BLUEGB;
			}
			
			set { m_HIPERUI_LINE_BLUEGB = value; }
		}
		
		GUIStyle HIPERUI_LINE_BLUEGB_PERSONAL
		{	get
			{	m_HIPERUI_LINE_BLUEGB_PERSONAL.padding.left = (int)(3 * CURRENT_SCALE);
				m_HIPERUI_LINE_BLUEGB_PERSONAL.padding.right = (int)(4 * CURRENT_SCALE);
				return m_HIPERUI_LINE_BLUEGB_PERSONAL;
			}
			
			set { m_HIPERUI_LINE_BLUEGB_PERSONAL = value; }
		}
		
		GUIStyle HIPERUI_LINE_BOX
		{	get
			{	m_HIPERUI_LINE_BOX.padding.left = (int)(3 * CURRENT_SCALE);
				m_HIPERUI_LINE_BOX.padding.right = (int)(3 * CURRENT_SCALE);
				return m_HIPERUI_LINE_BOX;
			}
			
			set { m_HIPERUI_LINE_BOX = value; }
		}
		
		GUIStyle HIPERUI_LINE_RDTRIANGLE
		{	get
			{	m_HIPERUI_LINE_RDTRIANGLE.padding.left = (int)(3 * CURRENT_SCALE);
				m_HIPERUI_LINE_RDTRIANGLE.padding.right = (int)(3 * CURRENT_SCALE);
				return m_HIPERUI_LINE_RDTRIANGLE;
			}
			
			set { m_HIPERUI_LINE_RDTRIANGLE = value; }
		}
		
		GUIStyle HIPERUI_MARKER_BOX
		{	get
			{	m_HIPERUI_MARKER_BOX.padding.left = (int)(3 * CURRENT_SCALE);
				m_HIPERUI_MARKER_BOX.padding.right = (int)(3 * CURRENT_SCALE);
				return m_HIPERUI_MARKER_BOX;
			}
			
			set { m_HIPERUI_MARKER_BOX = value; }
		}
		
		
		
		
		int[] perfomanceArray = { 2, 4, 6, 8, 10 };
		
		readonly GUIContent procContent =
		new GUIContent() { tooltip = "Performance of updating references to the current object" };
		
		readonly GUIContent autohideContent =
		new GUIContent() { tooltip = "Automatic Hide HperGraph if selection has been changed" };
		
		readonly GUIContent autorefreshContent =
		new GUIContent() { tooltip = "Automatic reload object if selection has been changed" };
		
		// readonly GUIContent HyperGraph = new GUIContent() { tooltip = "HyperGraph" };
		
		
		
		//    Color colorWhite = new Color32(211, 211, 211, 255);
		void MAIN( BottomInterface.UniversalGraphController controller )
		{	DRAW_GRID( controller );
		
		
			/*   /*  if (ScrollWhell!= 0)#1# {
			           MonoBehaviour.print(ScrollWhell);
			          }*/
			////////////////////////////////////////////////////////
			////////////////// MAIN ////////////////////////////////
			////////////////////////////////////////////////////
			
			//GUI.Label(new Rect(0,0,20,20),autorefreshContent);
			
			INIT_STYLES( adapter );
			
			
			DRAWOBJECT( controller );
			
			
			var ACTIVE_RECT = RECT;
			ACTIVE_RECT.height -= INTERFACE_SIZE;
			ACTIVE_RECT.y += INTERFACE_SIZE;
			
			
			var titleRect = ACTIVE_RECT;
			titleRect.height = adapter.FONT_10() + 2;
			
			
			if ( CurrentSelection ) DRAW_BOLD_LABEL( titleRect, "HyperGraph: '" + CurrentSelection.ToString() + "'" );
			else DRAW_BOLD_LABEL( titleRect, "HyperGraph" );
			
			
			////////////////////////////////////////////////////////
			////////////////// MAIN ////////////////////////////////
			/////////////////////////////////////////////////////////
			
			CHECK_DRAG( controller );
			
			if ( Event.current.type == EventType.Repaint )
			{	if ( shadow == null ) shadow = adapter.InitializeStyle( ICONID.SHADOW, 0.25f, 0.25f, 0.25f, 0.25f );
			
				if ( shadow != null )
				{	var shRect = RECT;
					/*  shRect.height -= INTERFACE_SIZE;
					              shRect.y += INTERFACE_SIZE;*/
					shRect.height = Math.Max( shRect.height, shadow.border.bottom * 2 );
					shadow.Draw( shRect, false, false, false, false );
				}
			}
		}
		GUIStyle __CenteredButton;
		GUIStyle CenteredButton
		{	get
			{	if ( __CenteredButton  == null )
				{	__CenteredButton = new GUIStyle( adapter.STYLE_HYPERGRAPH_DEFBUTTON );
					__CenteredButton.alignment = TextAnchor.MiddleCenter;
				}
				
				return __CenteredButton;
			}
		}
		
		GUIStyle __boldLabel;
		GUIStyle boldLabel
		{	get
			{	if ( __boldLabel == null )
				{	__boldLabel = new GUIStyle( adapter.label );
					__boldLabel.fontStyle = FontStyle.Bold;
					__boldLabel.alignment = TextAnchor.MiddleLeft;
				}
				
				__boldLabel.fontSize = adapter.label.fontSize - 2;
				return __boldLabel;
			}
		}
		private void DRAW_BOLD_LABEL( Rect titleRect, string empty )
		{	GUI.Label( titleRect, empty, boldLabel );
		}
		
		private void DRAW_BOLD_LABEL( Rect titleRect, GUIContent empty )
		{	GUI.Label( titleRect, empty, boldLabel );
		}
		
		
		readonly GUIContent HyperGraphZoomPlus = new GUIContent() { tooltip = "Zoom" };
		
		readonly GUIContent HyperGraphZoomReset = new GUIContent() { tooltip = "Reset" };
		//  readonly GUIContent HyperGraphZoomMinus = new GUIContent() { tooltip = "Close" };
		
		/** DRAW_ZOOMER **/
		void DRAW_ZOOMER( Rect ACTIVE_RECT, int EVENT_ID, BottomInterface.UniversalGraphController controller )
		{	var type = controller.MAIN ? ScrollType.HyperGraphScroll : ScrollType.HyperGraphScroll_Window;
		
			var position = ACTIVE_RECT;
			position.width = 40;
			position.height = 14;
			position.x = RECT.width - position.width;
			position.y = RECT.height - 2 - position.height;
			EditorGUIUtility.AddCursorRect( position, MouseCursor.Link );
			
			EVENT_ID++;
			HyperGraphZoomReset.text = Mathf.RoundToInt( CURRENT_SCALE * 100 ).ToString() + "%";
			DRAW_BOLD_LABEL( position, HyperGraphZoomReset );  // TOOLTIP
			
			if ( position.Contains( Event.current.mousePosition ) && Event.current.button == 0 &&
			        Event.current.type == EventType.MouseDown )
			{	EventUse();
				ADD_ACTION( EVENT_ID, position, contains => { return false; }, contains =>
				{	if ( contains )
					{	SET_SCALE( type, 1 );
					}
				}, controller );
			}
			
			if ( HOVER( EVENT_ID, position, controller ) )
				HIPERUI_BUTTONGLOW.Draw( position, false, false, false, false );
				
			position.width = position.height;
			position.x -= position.width + 2;
			EditorGUIUtility.AddCursorRect( position, MouseCursor.Link );
			
			
			/** BUTTON **/
			EVENT_ID++;
			Label( position, HyperGraphZoomPlus );  // TOOLTIP
			
			if ( position.Contains( Event.current.mousePosition ) && Event.current.button == 0 &&
			        Event.current.type == EventType.MouseDown )
			{	EventUse();
				ADD_ACTION( EVENT_ID, position, contains => { return false; }, contains =>
				{	if ( contains )
					{	SET_SCALE( type, Mathf.CeilToInt( CURRENT_SCALE * 4 + 0.001f ) / 4f );
					}
				}, controller );
			}
			
			GUI.DrawTexture( position, ZOOM_PLUS );  // TESTURE
			
			if ( HOVER( EVENT_ID, position, controller ) )
				HIPERUI_BUTTONGLOW.Draw( position, false, false, false, false );
				
			/** **/
			
			
			position.width = 70;
			position.x -= position.width + 2;
			
			EVENT_ID++;
			// var news = GUI.HorizontalSlider(position, par.HiperGraphParams.SCALE, 0.5f, 3);
			Label( position, HyperGraphZoomPlus );  // TOOLTIP
			var news = HorizontalSlider( ref EVENT_ID, position, CURRENT_SCALE, 0.5f, 2, controller );
			
			if ( news != CURRENT_SCALE )
			{	SET_SCALE( type, news );
			}
			
			
			position.width = position.height;
			position.x -= position.width + 2;
			EditorGUIUtility.AddCursorRect( position, MouseCursor.Link );
			
			
			/** BUTTON **/
			EVENT_ID++;
			Label( position, HyperGraphZoomPlus );  // TOOLTIP
			
			if ( position.Contains( Event.current.mousePosition ) && Event.current.button == 0 &&
			        Event.current.type == EventType.MouseDown )
			{	EventUse();
				ADD_ACTION( EVENT_ID, position, contains => { return false; }, contains =>
				{	if ( contains )
						if ( contains )
						{	SET_SCALE( type, Mathf.FloorToInt( CURRENT_SCALE * 4 - 0.001f ) / 4f );
							// par.HiperGraphParams.SCALE -= 0.25f;
						}
				}, controller );
			}
			
			GUI.DrawTexture( position, ZOOM_MINUS );  // TESTURE
			
			if ( HOVER( EVENT_ID, position, controller ) )
				HIPERUI_BUTTONGLOW.Draw( position, false, false, false, false );
				
			/** **/
		}
		/** DRAW_ZOOMER **/
		
		void SET_SCALE( ScrollType type, float _newScale )      //  var oldH = par.HiperGraphParams.SCALE;
		{	var olds = 0f;
			var dif = 0f;
			
			if ( type == ScrollType.HyperGraphScroll )
			{	olds = adapter.par.HiperGraphParams.SCALE;
				adapter.par.HiperGraphParams.SCALE = _newScale;
				dif = adapter.par.HiperGraphParams.SCALE / olds;
				
				HierHyperController.scrollPos.x =
				    (HierHyperController.scrollPos.x - HierHyperController.WIDTH / 2) * dif +
				    HierHyperController.WIDTH / 2;
				HierHyperController.scrollPos.y =
				    (HierHyperController.scrollPos.y - HierHyperController.HEIGHT / 2) * dif +
				    HierHyperController.HEIGHT / 2;
			}
			
			else
			{	olds = adapter.par.HiperGraphParams.WINDIOW_SCALE;
				adapter.par.HiperGraphParams.WINDIOW_SCALE = _newScale;
				dif = adapter.par.HiperGraphParams.WINDIOW_SCALE / olds;
				
				WindowHyperController.scrollPos.x =
				    (WindowHyperController.scrollPos.x -
				     WindowHyperController.WIDTH / 2) * dif +
				    WindowHyperController.WIDTH / 2;
				WindowHyperController.scrollPos.y =
				    (WindowHyperController.scrollPos.y -
				     WindowHyperController.HEIGHT / 2) * dif +
				    WindowHyperController.HEIGHT / 2;
				    
			}
			
			
			
			
			
			
			
			
			
			//SelectObject_height *= dif;
			//TARGET_HEIGHT *= dif;
			//GAMEOBJECTRECT.height *= dif;
			/*if (INPUT_COMPS != null)
			  foreach (var objectDisplay in INPUT_COMPS)
			  {
			    objectDisplay.Value.height *= dif;
			  }
			if (TARGET_COMPS != null)
			  foreach (var objectDisplay in TARGET_COMPS)
			  {
			    objectDisplay.Value.height *= dif;
			  }*/
			
			// par.HiperGraphParams.SCALE = newScale;
			
			adapter.SavePrefs();
			Repaint( WindowHyperController );
		}
		
		Dictionary < int, float? > HorizontalSlider_helper = new Dictionary < int, float? >();
		
		float HorizontalSlider( ref int EVENT_ID, Rect position, float value, float left, float right,
		                        BottomInterface.UniversalGraphController controller )
		{	var helperID = EVENT_ID;
		
			if ( !HorizontalSlider_helper.ContainsKey( helperID ) ) HorizontalSlider_helper.Add( helperID, null );
			
			var thumbRect = position;
			thumbRect.width = 10;
			
			
			var workL = position.width - thumbRect.width;
			thumbRect.x = position.x + workL * (value - left) / (right - left);
			//thumbRect.y += 1;
			thumbRect.height -= 1;
			
			position.y += position.height / 4;
			position.height = position.height / 2 - 1;
			
			EVENT_ID++;
			
			if ( position.Contains( Event.current.mousePosition ) &&
			        !thumbRect.Contains( Event.current.mousePosition ) && Event.current.button == 0 &&
			        Event.current.type == EventType.MouseDown )
			{	EventUse();
				var screenLeft = EditorGUIUtility.GUIToScreenPoint( new Vector2( position.x, position.y ) );
				var screenRight = EditorGUIUtility.GUIToScreenPoint( new Vector2( position.x + position.width,
				                  position.y + position.height ) );
				var oldMouse = -1f;
				ADD_ACTION( EVENT_ID, position, contains =>
				{	if ( oldMouse != CALC_MOUSE( screenLeft, screenRight ) )
					{	oldMouse = CALC_MOUSE( screenLeft, screenRight );
						HorizontalSlider_helper[helperID] = (right - left) * oldMouse + left;
						Repaint( WindowHyperController );
					}
					
					return true;
				}, contains => { }, controller );
				Repaint( WindowHyperController );
			}
			
			GUI.DrawTexture( position, adapter.BoxTexture() );  // TESTURE
			
			if ( HOVER( EVENT_ID, position, controller ) )
				HIPERUI_BUTTONGLOW.Draw( position, false, false, false, false );
				
			EditorGUIUtility.AddCursorRect( position, MouseCursor.Link );
			
			
			EVENT_ID++;
			
			if ( thumbRect.Contains( Event.current.mousePosition ) && Event.current.button == 0 &&
			        Event.current.type == EventType.MouseDown )
			{	EventUse();
				var screenLeft = EditorGUIUtility.GUIToScreenPoint( new Vector2( position.x, position.y ) );
				var screenRight = EditorGUIUtility.GUIToScreenPoint(
				                      new Vector2( position.x + position.width - thumbRect.width, position.y + position.height ) );
				// var offset = Event.current.mousePosition - thumbRect.position;
				var oldMouse = CALC_MOUSE( screenLeft, screenRight );
				var off = CALC_MOUSE( screenLeft, screenRight, thumbRect.position ) - oldMouse;
				ADD_ACTION( EVENT_ID, thumbRect, contains =>
				{	if ( oldMouse != CALC_MOUSE( screenLeft, screenRight ) )      // var old = oldMouse;
					{	oldMouse = CALC_MOUSE( screenLeft, screenRight );
						// sum -= old - oldMouse;
						HorizontalSlider_helper[helperID] = (right - left) * (oldMouse + off) + left;
						Repaint( WindowHyperController );
					}
					
					return true;
				}, contains => { }, controller );
			}
			
			GUI.DrawTexture( thumbRect, ZOOM_THUMB );  // TESTURE
			
			if ( HOVER( EVENT_ID, thumbRect, controller ) )
				HIPERUI_BUTTONGLOW.Draw( thumbRect, false, false, false, false );
				
			EditorGUIUtility.AddCursorRect( position, MouseCursor.Link );
			
			
			/* var result = Mathf.RoundToInt(thumbRect.x - position.x) * (right - left) / workL + left;
			         return Mathf.Clamp(result, left, right);*/
			var result = HorizontalSlider_helper[helperID] ?? value;
			HorizontalSlider_helper[helperID] = null;
			return Mathf.Clamp( result, left, right );
		}
		
		
		float CALC_MOUSE( Vector2 screenLeft, Vector2 screenRight, Vector2? offset = null )
		{	var full = screenRight.x - screenLeft.x;
			var m = EditorGUIUtility.GUIToScreenPoint( (offset ?? Event.current.mousePosition) ).x - screenLeft.x;
			return m / full;
		}
		
		
		GUIStyle shadow = null;
		
		private void CHECK_RIGHTCLICK( Rect rrrr )     // GUI.Label(rrrr, "");
		{	if ( rrrr.Contains( Event.current.mousePosition ) && Event.current.button == 1 &&
			        (Event.current.type == EventType.MouseDown || Event.current.type == EventType.Used) )     // MonoBehaviour.print("asd");
			{	EventUse();
				/*  RepaintWindow();
				
				
				            var menu = new GenericMenu();
				
				            menu.ShowAsContext();
				            EventUse();*/
			}
		}
		
		private void CHECK_DRAG( BottomInterface.UniversalGraphController controller )
		{	var EVENT_ID = 99;
		
			if ( RECT.Contains( Event.current.mousePosition ) && Event.current.button == 2 &&
			        Event.current.type == EventType.MouseDown )
			{	EventUse();
				var startPos = EditorGUIUtility.GUIToScreenPoint( Event.current.mousePosition );
				var startScroll = controller.scrollPos;
				ADD_ACTION( EVENT_ID, null, contains =>
				{	var p = EditorGUIUtility.GUIToScreenPoint( Event.current.mousePosition );
					var result = (startPos.x == p.x) && startPos.y == p.y;
					
					if (!result && controller.MAIN) adapter.RepaintWindowInUpdate();
					
					controller.scrollPos.x = startScroll.x - (startPos.x - p.x);
					controller.scrollPos.y = startScroll.y - (startPos.y - p.y);
					return result;
				}, contains => { }, controller );
			}
			
			if ( HOVER( EVENT_ID, null, controller ) )
			{	var m = Event.current.mousePosition;
				EditorGUIUtility.AddCursorRect( new Rect( m.x - 100, m.y - 100, 200, 200 ), MouseCursor.Pan );
			}
		}
		
		//   Color BUTBLUECOLOR = new Color(1, 1, 1, 0.15f);
		Color BUTBLUECOLOR = new Color( 0, 0, 0, 0.4f );
		
		Rect R = new Rect();
		void DRAW_GRID( BottomInterface.UniversalGraphController controller )
		{	if ( Event.current.type != EventType.Repaint ) return;
		
			var startX = Mathf.FloorToInt( -controller.scrollPos.x / GR_SIZE ) * GR_SIZE;
			var startY = Mathf.FloorToInt( -controller.scrollPos.y / GR_SIZE ) * GR_SIZE;
			R.width = GR_SIZE;
			R.height = GR_SIZE;
			var T = adapter.GetIcon( EditorGUIUtility.isProSkin ? ICONID.GRID : "GRID_PERSONAL" );
			
			for ( float y = startY, ly = startY + GR_SIZE + RECT.height ; y < ly ; y += GR_SIZE )
			{	for ( float x = startX, lx = startX + GR_SIZE + RECT.width ; x < lx ; x += GR_SIZE )
				{	R.x = x + controller.scrollPos.x;
					R.y = y + controller.scrollPos.y;
					GUI.DrawTexture( R, T );
				}
			}
			
			R.width = DEFAULTWIDTH( controller ) * CURRENT_SCALE;
			R.x = -DEFAULTWIDTH( controller ) / 2 * CURRENT_SCALE + controller.scrollPos.x;
			R.y = 0;
			R.height = controller.HEIGHT;
			var asd = GUI.color;
			GUI.color *= BUTBLUECOLOR;
			
			if ( EditorGUIUtility.isProSkin ) GUI.DrawTexture( R, adapter.GetIcon( "BUTBLUE" ) );
			
			GUI.color = asd;
		}
		
		
		/* adapter.par.HiperGraphParams.HEIGHT = Mathf.Clamp(adapter.par.HiperGraphParams.HEIGHT, 20,
		                                           Math.Max( 20, adapter.window().position.height* 0.5f ) );*/
		/*     internal void CHECK_HEIGHT()
		 {
		
		     adapter.par.HiperGraphParams.HEIGHT = Mathf.Clamp( adapter.par.HiperGraphParams.HEIGHT, 20,
		                                           Math.Max( 20, (adapter.window().position.height + bottomInterface.HEIGHT) / 2 ) );
		 }*/
		
		internal int CHECK_HEIGHT(int h)
		{	/* adapter.par.HiperGraphParams.HEIGHT = Mathf.Clamp( adapter.par.HiperGraphParams.HEIGHT, 20,
			                                       Math.Max( 20, adapter.window().position.height * 0.5f ) );*/
			
			return Mathf.Clamp( h, 20,
			                    Math.Max( 20, (int)(adapter.window().position.height + bottomInterface.HEIGHT) / 2 ) );
		}
	}
}
}