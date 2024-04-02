
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security;
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



	bool __NEW_SCROLL_BEHAVIOUR = true;
#pragma warning disable
	bool NEW_SCROLL_BEHAVIOUR { get { return __NEW_SCROLL_BEHAVIOUR; } }
	//   bool RedrawInit = false;
#pragma warning restore
	Vector2 ScrollMemory
	{	get
		{	return Hierarchy_GUI.Instance( this ).ScrollMemory.GetByKey( window().GetInstanceID() );
		}
		
		set     //if (value.y == 0 )return;
		{	if ( PlayModeFix || EditorApplication.isCompiling ) return;
		
			Hierarchy_GUI.Instance( this ).ScrollMemory.SetByKey( window().GetInstanceID(), value );
		}
	}
	
	
	
	
#pragma warning disable
	bool __RedrawInit ;
	internal bool RedrawInit
	
	{	get { return __RedrawInit; } set
		{	__RedrawInit = value;
		
			if ( value ) firstFrame = 0;
		}
	}
	Dictionary<int, bool> wasRedrawGoups = new Dictionary<int, bool>();
	
	internal class MOUSE_RAW_UP_HELPER {
		Vector2? lastRepaintPos = null;
		internal void Invoke()
		{	if ( __OnRawMouseUp != null )
			{
			
			
				/*   if ( win )
				   {   var pw = (new Vector2(win.position.x, win.position.y));
				       var wh = (new Vector2(win.position.x + win.position.width, win.position.y + win.position.height));
				
				       var mp = EditorGUIUtility.GUIToScreenPoint(Event.current.mousePosition);
				       Debug.Log( mp + " " + wh + " " + pw );
				       if ( mp.x < pw.x || mp.x > wh .x || mp.y < pw.y || mp.y > wh .y)
				       {   return;
				       }
				   }*/
				
				
				
				if ( hierScrilEv22 != Event.current.type )
				{	hierScrilEv22 = Event.current.type;
				
				
					if ( hierScrilEvMousePoss22 != EditorGUIUtility.GUIToScreenPoint( Event.current.mousePosition )
					        //hierScrilEv22 != Event.current.type && window()
					   )
					{	wasMouse = 1;
						//  Debug.Log( "MOVING : " + hierScrilEvMousePoss22 + " - " + EditorGUIUtility.GUIToScreenPoint( Event.current.mousePosition ) );
						StartLeaveMouse();
						
						if ( win ) win.wantsMouseMove = true;
					}
					
					bool b_wasDrag = false;
					
					if ( Event.current.type == EventType.MouseDrag )
					{	wasMouse = 0;
						wasDrag = 0;
						
						if ( win ) win.wantsMouseMove = true;
						
						//   Debug.Log( "drag" );
						b_wasDrag = true;
					}
					
					if ( wasMouse != 0 )
					{
					
						// window().wantsMouseMove = true;
						
						
						if (!b_wasDrag )
						{	// Debug.Log( Event.current.type + " " + wasMouse );
							if ( Event.current.type == EventType.Repaint && lastRepaintPos != EditorGUIUtility.GUIToScreenPoint( Event.current.mousePosition ) )
							{	wasMouse--;
								wasDrag++;
								// Debug.Log( "REPAINT : " + lastRepaintPos + " - " + EditorGUIUtility.GUIToScreenPoint( Event.current.mousePosition ) );
								lastRepaintPos = EditorGUIUtility.GUIToScreenPoint( Event.current.mousePosition );
								
								if ( win ) win.wantsMouseMove = true;
							}
							
							if (/*wasMouse == 0*/ wasDrag == 3 )
							{	WantMouseLeave();
							
								// window().wantsMouseMove = false;
								if ( win ) win.wantsMouseMove = false;
							}
						}
						
						if ( Event.current.type == EventType.Repaint )
						{	hierScrilEvMousePoss22 = EditorGUIUtility.GUIToScreenPoint( Event.current.mousePosition );
						}
						
						/*
						  hierScrilEvMousePoss22 = Event.current.mousePosition;
						  Debug.Log( Event.current.type );
						  //if (OnRawMouseUp != null )
						  window().wantsMouseMove = true;
						
						  if ( Event.current.isMouse )
						  {   wasMouse = 2;
						      //Debug.Log( "SET : " + 2 );
						  }
						  if ( Event.current.type == EventType.Used )
						  {   wasMouse = 2;
						      //  Debug.Log( "SET2 : " + 2 );
						  }
						  if ( Event.current.type == EventType.Repaint )
						  {   if ( wasMouse == 0 )
						      {   WantMouseLeave();
						          window().wantsMouseMove = false;
						          // Debug.Log( "!=0 : " + wasMouse );
						          // window().wantsMouseMove = false;
						      }
						      else
						      {   wasMouse--;
						          //  Debug.Log( "___ : " + wasMouse );
						      }
						  }*/
					}
				}
				
				
				
				if ( Event.current.type == EventType.MouseUp ||/* Event.current.type == EventType.MouseDown || */Event.current.keyCode == KeyCode.Escape )
				{	WantMouseLeave();
					wasMouse = 0;
					
					if ( win ) win.wantsMouseMove = false;
					
					//  Debug.Log( "ASD" );
					//  window().wantsMouseMove = false;
				}
			}
		}
		
		int wasMouse = 0;
		int wasDrag = 0;
		Action __OnRawMouseUp;
		Vector2? hierScrilEvMousePoss22 = null;
		EventType? hierScrilEv22 = null;
		EditorWindow win;
		internal void PUSH_ONMOUSEUP( Action ac, EditorWindow win =  null)
		{	__OnRawMouseUp += ac;
			StartLeaveMouse();
			this.win = win;
			
			if ( win ) win.wantsMouseMove = true;
			
			wasDrag = 0;
			lastRepaintPos = EditorGUIUtility.GUIToScreenPoint( Event.current.mousePosition );
			// Debug.Log( "PUSH" );
		}
		
		void StartLeaveMouse()
		{	hierScrilEvMousePoss22 = EditorGUIUtility.GUIToScreenPoint( Event.current.mousePosition );
			// window().wantsMouseMove = true;
			// hierScrilEv22 = null;
		}
		
		void WantMouseLeave()
		{	if ( __OnRawMouseUp != null )
			{	__OnRawMouseUp();
				__OnRawMouseUp = null;
			}
		}
	}
#pragma warning restore
	internal Rect BACKUP_RECT;
	MOUSE_RAW_UP_HELPER  mouse_uo_helper = new MOUSE_RAW_UP_HELPER();
	internal void PUSH_ONMOUSEUP(Action ac, EditorWindow win = null )
	{	mouse_uo_helper.PUSH_ONMOUSEUP( ac, win );
	}
	
	
	
	internal class new_child_struct {
		// internal Rect lastRect;
		Rect __rect;
		internal Rect rect { get { return __rect; } }
		internal void SetRect( Rect rect, bool isPrefab )
		{	__rect = rect;
			isMaxRight = false;
			this.isPrefab = isPrefab;
			
			if ( !wasInit ) wasInit = true;
		}
#pragma warning disable
		internal bool wasInit;
		internal bool wasLastAssign;
		bool isPrefab = false;
		bool isMaxRight = false;
#pragma warning restore
		internal void SetMax( Rect value, Rect SelectionRect, Adapter adapter, TempColorClass tc )
		{	if ( tc.BG_ALIGMENT_RIGHT_CONVERTED == BgAligmentRight.BeginLabel || tc.BG_ALIGMENT_RIGHT_CONVERTED == BgAligmentRight.Icon )
			{	if ( value.x < __rect.x )
				{	__rect.width += __rect.x - value.x;
					__rect.x = value.x;
				}
				
				if ( value.x + value.width < __rect.x + __rect.width ) __rect.width = value.x + value.width - __rect.x;
			}
			
			else
				if ( tc.BG_ALIGMENT_LEFT_CONVERTED == BgAligmentLeft.EndLabel )
				{	if ( value.x > __rect.x )
					{	__rect.width += __rect.x - value.x;
						__rect.x = value.x;
					}
					
					if ( value.x + value.width > __rect.x + __rect.width ) __rect.width = value.x + value.width - __rect.x;
				}
				
				else
				{	if ( value.x < __rect.x )
					{	__rect.width += __rect.x - value.x;
						__rect.x = value.x;
					}
					
					if ( value.x + value.width > __rect.x + __rect.width ) __rect.width = value.x + value.width - __rect.x;
				}
				
			if ( value.y < __rect.y )
			{	__rect.height += __rect.y - value.y;
				__rect.y = value.y;
			}
			
			if ( value.y + value.height > __rect.y + __rect.height ) __rect.height = value.y + value.height - __rect.y;
			
			isMaxRight = tc.BG_ALIGMENT_RIGHT_CONVERTED == BgAligmentRight.MaxRight;
			SelectionRect.width += adapter.PREFAB_BUTTON_SIZE;
			
			if ( __rect.x + __rect.width > SelectionRect.x + SelectionRect.width ) __rect.width = SelectionRect.x + SelectionRect.width - __rect.x;
			
			if ( !wasLastAssign )
			{	wasLastAssign = true;
			
				if ( __rect.y + __rect.height > SelectionRect.y + SelectionRect.height ) __rect.height = SelectionRect.y + SelectionRect.height - __rect.y;
			}
		}
		
		internal Rect ModifiedSelectionRect( Adapter adapter )
		{	var res =  SelectionRect;
			res.width += adapter.PREFAB_BUTTON_SIZE;
			return res;
		}
		
		internal Rect SelectionRect;
		internal Rect LocalRect;
		internal new_child_struct ConvertToLocal( Rect SelectionRect, Adapter adapter )
		{	LocalRect = __rect;
		
			if ( LocalRect.x + LocalRect.width > SelectionRect.x + SelectionRect.width + adapter.PREFAB_BUTTON_SIZE ) LocalRect.width = SelectionRect.x + SelectionRect.width - LocalRect.x +
				        adapter.PREFAB_BUTTON_SIZE;
				        
				        
				        
			LocalRect.x -= SelectionRect.x;
			LocalRect.y -= SelectionRect.y;
			
			if ( adapter.PREFAB_BUTTON_SIZE != 0 && !isPrefab )     //LocalRect.width += adapter.PREFAB_BUTTON_SIZE;
			{
			}
			
			
			this.SelectionRect = SelectionRect;
			return this;
		}
		// internal int lastObjject;
	}
	internal bool CHILD_GROUP_FIX_FLAG = false;
	internal     Dictionary<int, new_child_struct> new_child_cache_dic =  new Dictionary<int, new_child_struct>();
	bool oldPrefab = false;
	
	
#pragma warning disable
	bool oldPrefabA = false;
	bool oldPrefabB = false;
	Rect redrawRect;
	bool wasPrint = false;
	int layoutount = 0;
	bool layoutCheck = false;
	EventType? newHeightFixLayout;
#pragma warning restore
	MethodInfo dataInitMinimal = null;
	float? _lastOffset;
	float lastOffset
	{	get
		{
		
			if (!_lastOffset.HasValue )
			{	var visualRect = (Rect)m_VisibleRect.GetValue( m_TreeView( window() ) );
				var totalRect = (Rect)m_TotalRect.GetValue( m_TreeView( window() ) );
				_lastOffset = Math.Abs( totalRect.width - visualRect.width );
			}
			
			return _lastOffset.
			       Value;
		}
	}
	DrawStackAdapter _BOT_DRAW_STACK;
	internal DrawStackAdapter BOT_DRAW_STACK
	{	get
		{	return _BOT_DRAW_STACK ?? (_BOT_DRAW_STACK = new DrawStackAdapter() { adapter = this });
		}
	}
	bool resetStacks = false;
#if EMX_HIERARCHY_DEBUG_STACKS
	const double STACK_RESET_TIME = 8;
#else
	const double STACK_RESET_TIME = 4;
#endif
	internal void RESET_BOTTOM_STACKS()
	{
#if EMX_HIERARCHY_DEBUG_STACKS
		Debug.Log( "RESET_BOTTOM_STACKS" );
#endif
	}
	internal void RESET_COLOR_STACKS()
	{
	
		ColorModule.ResetStack();
		ColorModule.ICON_STACK.ResetStack();
		
		if ( M_CustomIconsModule != null ) M_CustomIconsModule.ResetStack();
		
#if EMX_HIERARCHY_DEBUG_STACKS
		Debug.Log( "RESET_COLOR_STACKS" );
#endif
	}
	internal void RESET_COLOR_STACKS(int id, bool disableLog = false)
	{
	
		ColorModule.ResetStack( id, disableLog );
		ColorModule.ICON_STACK.ResetStack( id, disableLog );
		
		if ( M_CustomIconsModule != null ) M_CustomIconsModule.ResetStack( id, disableLog );
		
#if EMX_HIERARCHY_DEBUG_STACKS
		
		if (!disableLog) Debug.Log( "RESET_COLOR_STACKS " + GetType().Name + " - " + EditorUtility.InstanceIDToObject(id)?.name);
		
#endif
	}
	internal void RESET_DRAW_STACKS()
	{	resetStacks = true;
#if EMX_HIERARCHY_DEBUG_STACKS
		Debug.Log( "RESET_DRAW_STACKS" );
#endif
		//   Debug.Log( "qqwe" );
	}
	int resetHoverStack1, resetHoverStack2;
	bool resetTimerStack = false;
	void RESET_TIMER_STACKS()
	{	//  __reset_stacks();
		resetTimerStack = true;
	}
	
	
	double? lastCacheClean;
	void __reset_stacks()
	{	for ( int i = 0 ; i < modules.Length ; i++ )
		{	foreach ( var stack in modules[i].DRAW_STACK )
			{	stack.Value.ResetStack();
			}
		}
	}
	EventType? ev = null;
#pragma warning disable
	bool prefabWas = false;
	bool prefab = false;
	bool PlayModeChanged = false;
	bool oldAPpPlay = false;
	Scene oldSc;
	Scene lastScene;
	Rect oldWPos;
	Vector2 oldScrollPos;
	Dictionary<int, float> previousObjects = new Dictionary<int, float>();
	Dictionary<int, float> checkObjects = new Dictionary<int, float>();
	Material oldMaterial;
#pragma warning restore
	static bool labelsInit = false;
	static internal int lastFontSize = 11;
	bool? oldProSkin;
	static int cacheFontSize = -1;
	void START_LABELS()
	{	if (Event.current.type != EventType.Repaint || pluginID != 0) return;
	
		////if (EditorApplication.isCompiling || firstFrame <= 4) return;
		//bool wasInit = false;
		
		if (!labelsInit)
		{	labelsInit = true;
			/*LABLES.Add(EditorStyles.label);
			FixStyle( EditorStyles.label );
			LABLES.Add(GUI.skin.label);
			FixStyle( GUI.skin.label );*/
			lastFontSize = (int)EditorStyles.label.fontSize;
			
			if (lastFontSize == 0 ) lastFontSize = 11;
			
			//wasInit = true;
		}
		
		foreach (var item in LABLES)
		{	item.fontSize = HIERARCHY_FONT_SIZE;
		}
		
		if (cacheFontSize == -1) cacheFontSize = lastFontSize;
		
		if (cacheFontSize != HIERARCHY_FONT_SIZE  && EDITOR_FONT_AFFECTOTHERWINDOWS)
		{	//InternalEditorUtility.RepaintAllViews();
			cacheFontSize = HIERARCHY_FONT_SIZE;
			RepaintAllViews();
		}
		
		//if (wasInit)
	}
	void END_LABLES()
	{	if (EDITOR_FONT_AFFECTOTHERWINDOWS) return;
	
		if (Event.current.type != EventType.Repaint || pluginID != 0) return;
		
		foreach (var item in LABLES)
		{	item.fontSize = lastFontSize;
		}
	}
	private void m_Main( HierarchyObject o, Rect selectionrect )
	{	/* var gui = guiProp.GetValue(m_TreeView(window()), null);
		  k_LineHeight.SetValue( gui, parLINE_HEIGHT );
		Debug.Log( parLINE_HEIGHT );*/
		//  if ( o.name == "Blob" ) Debug.Log( o.name + " " + Event.current.type );
		//   if ( o.name == "BloodPos" ) Debug.Log( o.name + " " + Event.current.type );
		
		// if ( RedrawInit  ) Debug.Log( o.name);
		
#pragma warning disable
		// if ( Adapter.USE2018_3 && Event.current.type == EventType.Layout ) return;
#pragma warning restore
		// if (selectionrect.height > parLINE_HEIGHT)
		
		if ( pluginID == Initializator.HIERARCHY_ID && selectionrect.height > selectionrect.width )     // Debug.Log(selectionrect);
		{
		
			// Debug.Log( "2" );
			return;
		}
		
		
		//if ( window().wantsMouseEnterLeaveWindow ) Debug.Log( "ASD" );
		
		if ( __GUI_ONESHOT && o.Validate()  /* && Event.current.type == EventType.Repaint*/ )
		{	var ta = __GUI_ONESHOTAC;
			__GUI_ONESHOTAC = null;
			__GUI_ONESHOT = false;
			ta();
		}
		
		
		
		mouse_uo_helper.Invoke();
		
		
		/*  if ( hierScrilEv23 != Event.current.type && window() )
		  {   hierScrilEv23 = Event.current.type;
		      if ( pluginID == 0 ) Debug.Log( Event.current.type + " " + Event.current.isMouse + " " + window().wantsMouseMove );
		  }*/
		
		o.filterAssigned = false;
		
		
		if ( tempAdapterBlock || !wasAdapterInitalize || !par.ENABLE_ALL )
		{	// Debug.Log( "!" );
			return;
		}
		
		
		TryToInitializeDefaultStyles();
		
		TintColor = GUI.color;
		BACKUP_RECT = selectionrect;
		
		//   CHECK_HOSTVIEW();
		//  Expample();
		//  if(pluginID == 0)
		//     CHECK_MAXIMIZATION();
		if ( Event.current.type == EventType.Repaint ) prefab = false;
		
		
		if ( Event.current.type  != ev )
		{	ev = Event.current.type;
		
			if (!oldProSkin.HasValue) oldProSkin = EditorGUIUtility.isProSkin;
			
			if (oldProSkin != EditorGUIUtility.isProSkin) CLearAdditionalCache();
			
			START_LABELS();
			
			/* if ( PlayModeChanged )
			 {   ColorModule.ClearCache();
			     PlayModeChanged = false;
			 }*/
			if ( oldAPpPlay  != Application.isPlaying )
			{	oldAPpPlay = Application.isPlaying;
				ColorModule.ClearCache();
			}
			
			//  if (CACHING_TEXTURES_STACKS)
			BACK_WIN();
			
			
			SEARCH_HAS_BAKE();
			
			/*   if ( oldMaterial != HIghlighterExternalMaterial )
			   {   oldMaterial = HIghlighterExternalMaterial;
			       RESET_COLOR_STACKS();
			   }*/
			if (/* (  Event.current.type == EventType.Layout) &&*/ IS_SEARCH_MODE_OR_PREFAB_OPENED() != oldPrefab/* || GET_ACTIVE_SCENE != oldSc*/ )
			{
			
				/* if ( oldSc != GET_ACTIVE_SCENE )
				 {   HeightFixIfDrag_Flag = true;
				     Debug.Log( "ASD" );
				 }
				 else*/
				{	oldPrefab = IS_SEARCH_MODE_OR_PREFAB_OPENED();
					HeightFixIfDrag();
					//oldSc = GET_ACTIVE_SCENE;
					///  prefab = oldPrefabA;
					//   oldPrefabA = IS_PREFAB_MOD_OPENED();
					firstFrame = 0;
					prefab = true;
					prefabWas = true;
				}
				
				
				// WasInitDraw.Clear();
				
			}
		}
		
		//PREFAB WAS HERE + LAYOUT
		//  bool drawdraw = false;
		
		
		float? drawInitY ;
		
		if (!WasInitDraw.TryGetValue( GUIUtility.keyboardControl, out drawInitY )) drawInitY = selectionrect.y;
		
		if ( drawInitY == null ) drawInitY = WasInitDraw[GUIUtility.keyboardControl] = selectionrect.y;
		
		if (   drawInitY.Value == selectionrect.y && Event.current.type != EventType.Repaint )
		{	if (!lastCacheClean .HasValue) lastCacheClean = EditorApplication.timeSinceStartup;
		
			if (  Math.Abs( lastCacheClean.Value - EditorApplication.timeSinceStartup ) > STACK_RESET_TIME )
			{	RESET_TIMER_STACKS();
				// Debug.Log( Math.Abs( lastCacheClean.Value - EditorApplication.timeSinceStartup ) );
				lastCacheClean = EditorApplication.timeSinceStartup;
			}
		}
		
		
		if ( /*(Event.current.type == EventType.Layout || Event.current.type == EventType.Repaint) &&*/ (
		            drawInitY.Value == selectionrect.y
		            || newHeightFixLayout != Event.current.type
		            // || prefabWas
		            // || !HeightFixIfDrag_Flag
		        )
		        && window() )
		{	newHeightFixLayout = Event.current.type;
			drawInitY = selectionrect.y;
			
			
			var p = window().position;
			
			if ( p.width != oldWPos.width || p.height != oldWPos.height )
			{	oldWPos = window().position;
				__reset_stacks();
			}
			
			if ( oldScrollPos.y  != GetHierarchyWindowScrollPos().y )
			{	oldScrollPos = GetHierarchyWindowScrollPos();
				// ColorModule.ResetStack(  );
				
				if ( previousObjects.Count > 0 )
				{	checkObjects = previousObjects.ToDictionary( k => k.Key, v => v.Value );
					checkObjects.Remove( o.id );
					var m = checkObjects.Values.Max();
					var findedId = -1;
					
					foreach ( var item in checkObjects )
					{	if ( item.Value == m )
						{	findedId = item.Key;
							break;
						}
						
					}
					
					if ( findedId != -1 ) checkObjects.Remove( findedId );
				}
				
				else
				{	checkObjects.Clear();
				}
				
				previousObjects.Clear();
			}
			
			if ( prefabWas )
			{	/*  var posRect = (Rect)m_TotalRect.GetValue( m_TreeView( window() ) );
				       var visualRect = (Rect)m_VisibleRect.GetValue( m_TreeView( window() ) );
				       visualRect.height = posRect.height / EditorGUIUtility.singleLineHeight * parLINE_HEIGHT;
				       m_VisibleRect.SetValue( m_TreeView( window() ), visualRect );*/
				/*   var posRect = (Rect)m_TotalRect.GetValue( m_TreeView( window() ) );
				   posRect.height += bottomInterface.HEIGHT;
				   m_TotalRect.SetValue( m_TreeView( window() ), posRect );*/
				
			}
			
			
			
			if ( lastScene != SceneManager.GetActiveScene() )
			{	lastScene = SceneManager.GetActiveScene();
			
				if ( firstFrame >= 4 ) HeightFixIfDrag();
				
				//  firstFrame = 0;
			}
			
			//  Debug.Log( "CHECK" );
			/*  var gui = guiProp.GetValue(m_TreeView(window()), null);
			  if ( parLINE_HEIGHT != (float)k_LineHeight.GetValue( gui ) )
			  {   //Debug.Log( "ASD" );
			      HeightFixIfDrag();
			  }*/
			// var oldH = (float)k_LineHeight.GetValue( gui );
			
			
			
			bottomInterface.BOTTOM_UPDATE_POSITION( window() );
			
			//   Debug.Log( prefabWas );
			if ( firstFrame < 4 && !prefabWas )  HEIGHT_RIX_FUNCTIUON( window(), null );
			
			
			
			
			
			if ( IS_LAYOUT   )
			{
			
				// var exec = lastOffset;
			}
			
			//  Debug.Log( selectionrect + " " + IS_LAYOUT );
			
			
			
			var control = GET_CONTROL();
			
			if ( !wasRedrawGoups.ContainsKey( control ) ) wasRedrawGoups.Add( control, false );
			
			//   if (  window() )
			{	/*bool needRepaint = false;
				foreach ( var nc in ColorModule.new_child_cache )
				{   if ( nc.Value.lastRect != nc.Value.__rect )
				    {   nc.Value.lastRect = nc.Value.__rect;
				        needRepaint = true;
				    }
				}
				if (needRepaint) RepaintWindow(true);*/
				
				if (!HeightFixIfDrag_Flag )
				{	HeightFixIfDrag_Flag = true;
				
				
					//  if ( !IS_SEARCH_MODE_OR_PREFAB_OPENED() )
					if ( !IS_SEARCH_MOD_OPENED() )
					{
					
						// var posRect = (Rect)m_TotalRect.GetValue( m_TreeView( window() ) );
						// if (!prefab )
						{	var visualRect = (Rect)m_VisibleRect.GetValue( m_TreeView( window() ) );
							visualRect.height += bottomInterface.HEIGHT + 20;
							visualRect.y -= 20;
							m_VisibleRect.SetValue( m_TreeView( window() ), visualRect );
						}
						
						//  INIT_IF_NEDDED_SIMPLE();
						
						var tree = m_TreeView( window() );
						var d = m_data.GetValue( tree, null );
						
						
						if ( dataInitMinimal == null ) dataInitMinimal = d.GetType().GetMethod( "InitializeMinimal", (System.Reflection.BindingFlags)(-1) );
						
						try
						{	// dataInitMinimal.Invoke( d, null );
							if ( GET_ACTIVE_SCENE_RAW != oldSc )
							{	oldSc = GET_ACTIVE_SCENE_RAW;
							}
							
							if ( !string.IsNullOrEmpty( oldSc.path) ) dataInitMinimal.Invoke( d, null );
						}
						
						catch
						{
						
						}
						
						// Debug.Log( "ASD" );
						//  GET_ACTIVE_SCENE != oldSc
						if ( m_UseExpansionAnimation != null )
						{	m_UseExpansionAnimation.SetValue( tree, false );
						}
					}
					
					blockHF = true;
					
					if ( firstFrame  < 4 && hashoveredItem /*&& !prefabWas*/) SendEventAll( new Event() { type = EventType.Layout } );
					//#TODO
					// SendEventAll( new Event() { type = EventType.Layout } );
					/*   if (prefab)SendEventAll( new Event() { type = EventType.Layout } );
					*/
					//  if ( prefab ) SendEventAll( new Event() { type = EventType.Layout } );
					// Debug.Log( prefab );
					blockHF = false;
					//     RepaintWindowInUpdate();
					// Debug.Log( "ASD" );
				}
				
				
				
				//
				
				
				/* var visualRect = (Rect)m_VisibleRect.GetValue( m_TreeView( window() ) );
				 var currentLines = visualRect.height / EditorGUIUtility.singleLineHeight;
				   var H = (selectionrect.y - GetHierarchyWindowScrollPos().y ) + parLINE_HEIGHT;
				   if (H > ().height )
				   {
				       EditorGUIUtility.ExitGUI();q
				   }*/
				if ( !wasRedrawGoups[control] )
				{	wasRedrawGoups[control] = true;
					RedrawInit = true;
					redrawRect = selectionrect;
				}
				
				/* window().SendEvent( new Event() { type = EventType.Layout } );
				 window().SendEvent( new Event() { type = EventType.Repaint } );*/
				//   window().SendEvent( new Event() { type = EventType.Layout } );
				// -= WantMouseLeave;
				//bottomInterface. BOTTOM_UPDATE_POSITION( window() );
				
				if ( Event.current.type == EventType.Repaint )
				{	// SendEventAll( new Event() { type = EventType.Layout } );
					//  Debug.Log( "SEND ALL" );
				}
				
				//  bottomInterface.BOTTOM_UPDATE_POSITION( window() );
				// RedrawInit = false;
				try     // (typeof( EditorWindow )).GetMethod( "RepaintImmediately", (BindingFlags)(-1) ).Invoke( window(), null );
				{
				}
				catch
				{
				
				
				}
				
				//repaintImmidiately.Add( window() );
			}
			
			if (firstFrame >= 4) prefabWas = false;
		}
		
		/*  if (Event.current.type == EventType.Repaint )
		  {   if ( GET_ACTIVE_SCENE != oldSc )
		      {   oldSc = GET_ACTIVE_SCENE;
		          if ( GET_ACTIVE_SCENE < 0 )     //  d.GetType().GetMethod( "InitializeFull", (System.Reflection.BindingFlags)(-1) ).Invoke( d, null );
		          {   //m_VisibleRect.SetValue( m_TreeView( window() ), (Rect)m_TotalRect.GetValue( m_TreeView( window() ) ) );
		              Debug.Log( "ASD" );
		              // INIT_IF_NEDDED();
		              //d.GetType().GetMethod( "InitializeFull", (System.Reflection.BindingFlags)(-1) ).Invoke( d, null );
		          }
		      }
		  }*/
		
		
		if (  Event.current.type == EventType.Layout  )
		{	//selectionrect.width -= lastOffset;
			if ( !previousObjects.ContainsKey( o.id ) ) previousObjects.Add( o.id, selectionrect.y );
			
			if ( !checkObjects.ContainsKey( o.id ) )
			{	checkObjects.Add( o.id, selectionrect.y );
				ColorModule.ResetStack( o.id );
				ColorModule.ICON_STACK.ResetStack( o.id );
				
				if ( M_CustomIconsModule != null ) M_CustomIconsModule.ResetStack( o.id );
			}
			
			
			if ( resetStacks )     //RESET_DRAW_STACKS();
			{	__reset_stacks();
				resetStacks = false;
			}
			
			if ( resetTimerStack )
			{	__reset_stacks();
				resetTimerStack = false;
			}
			
			if (resetHoverStack1 != 0 )
			{	//  RESET_COLOR_STACKS( resetHoverStack1, true );
				resetHoverStack1 = 0;
			}
			
			if ( resetHoverStack2 != 0 )
			{	//RESET_COLOR_STACKS( resetHoverStack2, true );
				resetHoverStack2 = 0;
			}
		}
		
		// if ( drawdraw ) Debug.Log( selectionrect + " " + Event.current.type);
		/*   if ( RedrawInit )
		   {   if ( Event.current.type == EventType.Repaint )
		       {   RedrawInit = false;
		           layoutCheck = true;
		       }
		
		
		       layoutount++;
		       //if ( redrawRect.y == selectionrect.y) RedrawInit = false;
		   }*/
		/*      var visualRect = (Rect)m_VisibleRect.GetValue( m_TreeView( window() ) );
		        visualRect
		    var H = (selectionrect.y - GetHierarchyWindowScrollPos().y ) + parLINE_HEIGHT;
		    if (H > ().height )
		    {
		        EditorGUIUtility.ExitGUI();q
		    }*/
		/* var vr = (Rect)m_VisibleRect.GetValue( m_TreeView(window()));
		     vr.height += parLINE_HEIGHT;
		     m_VisibleRect.SetValue( m_TreeView( window() ), vr );*/
		/*  Debug.Log( "INIT: " + o.name + " "  + H  + " "
		             + Event.current.type + " "
		             + m_VisibleRect.GetValue( m_TreeView( window() ) )
		           );*/
		
		/*     if ( layoutCheck )
		     {   if ( Event.current.type != EventType.Repaint )
		         {   layoutCheck = false;
		         }
		         layoutount--;
		         if ( layoutount < 0 )
		         {   if (pluginID == 0) Debug.Log( "ERROR: " + o.name );
		             layoutCheck = false;
		         }
		     }*/
		
		//if ( !wasPrint && pluginID == 0 && o.name == "test" ) { wasPrint = true; Debug.Log( Event.current.type + " " + selectionrect.height + " " + selectionrect.width ); }
		
		
		if ( PlayModeFix )
		{	/* HeightFixIfDrag();
			     Debug.Log("123");*/
			// PlayModeFix = false;
			//HEIGHT_FUNCTIUON( window(), m_TreeView(window()), true);
			// Reload();
		}
		
		/*  if ( !applicationIsPlaying.HasValue ) applicationIsPlaying = Application.isPlaying;
		  if ( applicationIsPlaying != Application.isPlaying )
		  {   applicationIsPlaying = Application.isPlaying;
		      ReloadEd = true;
		  }*/
		
		//if ( firstFrame < 4 && ReloadEd ) ReloadEd = false;
		
		
		if (ReloadEd )
		{	if ( bottomInterface.onSceneChange != null ) bottomInterface.onSceneChange();
		
		
			if ( IMGUI() ) HEIGHT_RIX_FUNCTIUON( window(), m_TreeView( window() ), true );
			
			ReloadEd = false;
			
			if ( firstFrame > 4)   PLAYMODECHANGE = true;
			
			if ( IMGUI() )
			{	if ( ReloadEd ) PLAYMODE_RELOADED = true;
			
				if ( window() )     //var treeView = m_TreeView.GetValue( window() );
				{	//var state = m_state.GetValue( treeView , null );
				
					//  Debug.Log("ASD");
					
					//#initialize #tag
					
					if ( !IMGUI20183() ) INIT_IF_NEDDED();
					
					if ( oldScroll == null )
					{	if ( NEW_SCROLL_BEHAVIOUR )
						{	oldScroll = ScrollMemory;
						}
						
						else
						{	var treeView = m_TreeView( window() );
							var state = m_state.GetValue( treeView, null );
							oldScroll = (Vector2)scrollPosField.GetValue( state );
						}
						
					}   //
					
					//**////
					// MonoBehaviour.print("SET");
					//**// if ( oldScroll == null ) oldScroll = (Vector2)scrollPosField.GetValue( state );
					//**//
					
					//	scrollPosField.SetValue( state , Vector2.zero );
					//  HierWinScrollPos = Vector2.zero;
					
					//EditorWindow
					/*	MonoBehaviour.print( ((Rect)m_VisibleRect.GetValue( treeView )).y );
					    treeView.GetType().GetField( "m_StopIteratingItems" , (BindingFlags)(-1) ).SetValue( treeView , true );
					
					    var r1 = (Rect)SceneHierarchyWindow.GetProperty( "treeViewRect" , (BindingFlags)(-1) ).GetValue( window() , null );
					    var r2 = (Rect)m_VisibleRect.GetValue( treeView );
					    r2.height = r1.height;
					    m_VisibleRect.SetValue( treeView , r2 );
					    m_TotalRect.SetValue( treeView , r2 );*/
					
					/*	var r = SceneHierarchyWindow.GetProperty( "treeViewRect" , (BindingFlags)(-1) ).GetValue( SceneHierarchyWindow , null );
					m_VisibleRect.SetValue( treeView , r );
					m_TotalRect.SetValue( treeView , r );
					
					
					m_data.GetType().GetMethod( "OnInitialize" , (BindingFlags)(-1) ).Invoke( m_data.GetValue( treeView , null ) , null );
					m_gui.GetType().GetMethod( "OnInitialize" , (BindingFlags)(-1) ).Invoke( m_gui.GetValue( treeView , null ) , null );
					
					window().Repaint();*/
				}
			}
			
			// MonoBehaviour.print("ENABLE");
			
			
			
			
			//PLAY_MODE_CHANGE_ACTION();
			/* EditorGUIUtility.ExitGUI();
			 return;*/
		}
		
		// if (PLAYMODECHANGE) return;
		
		
		
		
		
		//  MonoBehaviour.print(GUIUtility.keyboardControl);
		/*   var s = typeof(Profiler).GetMethod("GetSamplerNames", (BindingFlags)(-1));
		var strs = (string[]) s.Invoke(null,null);
		 MonoBehaviour.print(strs.Length);*/
		/*  foreach (var VARIABLE in strs) {
		      MonoBehaviour.print(VARIABLE);
		  }
		*/
		if ( par.ENABLE_ALL ) MOI.InitModules();
		
		// if (Event.current.type == EventType.ScrollWheel) return;
		
		//  ProfilerDriver.deepProfiling = false;
		
		//Event.current.type != EventType.Layout &&
		if ( par.ENABLE_ALL  /*&& ENABLE_BOTTOMDOCK_PROPERTY*/)    // try  && !IS_LAYOUT
		{
			{	bottomInterface.EventGUI( o, selectionrect );
				//  BottomInterface.EventGUI(selectionrect);
			}
			/*  catch (Exception ex)
			  {   logProxy.LogError( "EventGUI " + ex.Message + " " + ex.StackTrace );
			      return;
			  }*/
		}
		
		else
		{	PLAYMODECHANGE = false;
		}
		
		//  ProfilerDriver.deepProfiling = true;
		
		
		
		// EditorApplication.hierarchyWindowItemOnGUI(
		// Profiler.BeginSample("My Sample");
		//  var old = Profiler.enabled;
		/*  try {
		     // Profiler.EndSample();
		  }
		  catch {
		  }*/
		// Profiler.GetTempAllocatorSize(
		//Profiler.EndSample();
		// Profiler.enabled = false;
		//  var s = typeof(Profiler).GetMethod("SetSamplersEnabled", (BindingFlags)(-1));
		// s.Invoke(null, new object[] { false });
		
		
		
		Drawing( o, selectionrect );
		// Profiler.enabled = true;
		// Profiler.BeginSample("Skip");
		//Profiler.enabled = old;
		//Profiler.EndSample();
		/*  window().Profiling
		  Bottom_Interface.EventGUI();
		  Bottom_Interface.PaintGUI();*/
		
		
		if ( Event.current.type == EventType.MouseDown && par.DOUBLECLICK_IS_EXPAND )
		{	if ( Event.current.clickCount == 2 && Event.current.button == 0
			        && selectionrect.Contains( Event.current.mousePosition ) )     // float labelWidth = Adapter.GET_SKIN().label.CalcSize(new GUIContent(o.ToString())).y;
			{	// if (Event.current.mousePosition.x > adapter.BACKUP_RECT.x && Event.current.mousePosition.x < adapter.BACKUP_RECT.x + labelWidth)
				if ( o.ChildCount( this ) > 0 )
				{
					{	bottomInterface.EXPAND_SWITCHER( o );
					}
					EventUse();
					RepaintWindowInUpdate();
				}
				
			}
		}
		
		if ( Event.current.type == EventType.KeyDown && USE_HOVER_EXPANDING && (Event.current.keyCode == KeyCode.RightArrow || Event.current.keyCode == KeyCode.LeftArrow))
		{	if (hashoveredItem  && hoverID != -1 )
			{	/*bottomInterface.EXPAND_SWITCHER( GetHierarchyObjectByInstanceID(hoverID),  Event.current.keyCode == KeyCode.RightArrow ? true : false);
				EventUse();*/
				
				
				
				if (!_IsSelectedCache.ContainsKey(hoverID))
				{	bottomInterface.EXPAND_SWITCHER( GetHierarchyObjectByInstanceID(hoverID),  Event.current.keyCode == KeyCode.RightArrow ? true : false);
				}
				
				else
				{	foreach (var item in _IsSelectedCache)
					{	bottomInterface.EXPAND_SWITCHER( GetHierarchyObjectByInstanceID(item.Key),  Event.current.keyCode == KeyCode.RightArrow ? true : false);
					
					}
				}
				
				EventUse();
			}
		}
		
		if ( Event.current.type == EventType.KeyDown && USE_NEW_MULTYLINE_SELECTION_BEGHAVIOUR &&(Event.current.keyCode == KeyCode.DownArrow || Event.current.keyCode == KeyCode.UpArrow))
		{	if (Selection.objects.Length > 1)
			{	if (MultyLineOffset())
					EventUse();
					
				/*else
					Debug.Log("ADS");*/
			}
		}
		
		//Type.GetType("UnityEditor.SceneManagement.StageNavigationManager")
		// ScriptableSingleton<StageNavigationManager>.instance.NavigateBack(StageNavigationManager.Analytics.ChangeType.NavigateBackViaHierarchyHeaderLeftArrow);
		
	}
	
	System.Reflection.PropertyInfo rowToID;
	System.Reflection.PropertyInfo lastClickedID;
	System.Reflection.MethodInfo EnsureRowIsVisible;
	System.Reflection.MethodInfo GetIndexOfID;
	System.Reflection.MethodInfo NewSelectionFromUserInteraction;
	Type GetIndexOfIDAss;
	bool MultyLineOffset()
	{	try
		{	var w = window();
			var treeView = m_TreeView(w);
			
			
			if (dsType == null) dsType = Assembly.GetAssembly(typeof(EditorWindow)).GetType("UnityEditor.IMGUI.Controls.TreeViewDataSource");
			
			if (m_Rows == null) m_Rows = dsType.GetField("m_Rows", (BindingFlags)(-1));
			
			if (treeView == null) return false;
			
			var d = m_data.GetValue(treeView, null);
			var state = m_state.GetValue(treeView, null);
			
			if (d == null || state == null) return false;
			
			var rows = m_Rows.GetValue(d) as System.Collections.IList;
			
			if (rows == null) return false;
			
			if (rows.Count == 0) return false;
			
			
			if (GetIndexOfIDAss == null) GetIndexOfIDAss = Assembly.GetAssembly(typeof(EditorWindow)).GetType("UnityEditor.IMGUI.Controls.TreeViewController");
			
			if (GetIndexOfID == null) GetIndexOfID = GetIndexOfIDAss.GetMethod("GetIndexOfID", (BindingFlags)(-1));
			
			if (EnsureRowIsVisible == null) EnsureRowIsVisible = GetIndexOfIDAss.GetMethod("EnsureRowIsVisible", (BindingFlags)(-1));
			
			
			
			if (lastClickedID == null) lastClickedID = state.GetType().GetProperty("lastClickedID", (BindingFlags)(-1));
			
			var last = (int)lastClickedID.GetValue(state, null);
			int? newlast = null;
			var res = new Dictionary<int, int>();
			
			//	var sel_dic = _IsSelectedCache.ToDictionary(item => (int)GetIndexOfID.Invoke(null, new object[] { rows, item.Key }), item=>item.Key);
			var sel_dic = Selection.objects.ToDictionary(item => (int)GetIndexOfID.Invoke(null, new object[] { rows, item.GetInstanceID() }), item=>item.GetInstanceID());
			//orderedSelection.Sort();
			
			
			if (Event.current.shift)
			{
			
			
			
				int expand = 0;
				
				foreach (var item in sel_dic)
				{	if (!res.ContainsKey(item.Key)) res.Add(item.Key, item.Value);
				
					//if (res.ContainsKey(item.Key+ 1) || res.ContainsKey(item.Key - 1)) hasNear = true;
				}
				
				if (res.Values.Any(v => v == last))
				{	var f = res.First(v=>v.Value == last);
					var up = res.ContainsKey(f.Key- 1);
					var down = res.ContainsKey(f.Key + 1);
					
					if (up && down || !up && !down) expand = 0;
					else expand = up ? -1 : 1;
				}
				
				if (expand != 0 && (expand == 1 && Event.current.keyCode == KeyCode.DownArrow || expand == -1 && Event.current.keyCode == KeyCode.UpArrow))
					//remove
				{	foreach (var item in res.ToDictionary(k=>k.Key, v=>v.Value))
					{	if (Event.current.keyCode == KeyCode.DownArrow && res.ContainsKey(item.Key + 1) && !res.ContainsKey(item.Key - 1))
						{	res.Remove(item.Key);
						}
						
						if (Event.current.keyCode == KeyCode.UpArrow && !res.ContainsKey(item.Key + 1) && res.ContainsKey(item.Key - 1))
						{	res.Remove(item.Key);
						}
					}
					
					newlast = Event.current.keyCode  != KeyCode.UpArrow ? res[res.Keys.Min()] : res[res.Keys.Max()];
				}
				
				else
					//add
				{	var change = Event.current.keyCode == KeyCode.UpArrow ? -1 : 1;
				
					foreach (var item in sel_dic)
					{
					
						int rowIndex = Mathf.Clamp(item.Key+change, 0, rows.Count - 1);
						
						EnsureRowIsVisible.Invoke(treeView, new object[] { rowIndex, false });
						var r = rows[rowIndex] as UnityEditor.IMGUI.Controls.TreeViewItem;
						//if ( rowToID == null ) rowToID = rows[rowIndex].GetType().GetProperty( "id", (BindingFlags)(-1) );
						var rowID = r.id;
						//var rowID = (int)rowToID.GetValue(rows[rowIndex], null);
						
						if (!res.ContainsKey(rowIndex)) res.Add(rowIndex, rowID);
						
						//if (item.Value == last) newlast = rowID;
					}
					
					newlast = Event.current.keyCode == KeyCode.UpArrow ? res[res.Keys.Min()] : res[res.Keys.Max()];
				}
				
			}
			
			else
			{	var change = Event.current.keyCode == KeyCode.UpArrow ? -1 : 1;
			
				foreach (var item in sel_dic)
				{
				
					int rowIndex = Mathf.Clamp(item.Key+change, 0, rows.Count - 1);
					
					EnsureRowIsVisible.Invoke(treeView, new object[] { rowIndex, false });
					var r = rows[rowIndex] as UnityEditor.IMGUI.Controls.TreeViewItem;
					//if ( rowToID == null ) rowToID = rows[rowIndex].GetType().GetProperty( "id", (BindingFlags)(-1) );
					var rowID = r.id;
					//var rowID = (int)rowToID.GetValue(rows[rowIndex], null);
					
					res.Add(rowIndex, rowID);
					
					//if (item.Value == last) newlast = rowID;
				}
				
				newlast = Event.current.keyCode == KeyCode.UpArrow ? res[res.Keys.Min()] : res[res.Keys.Max()];
				
			}
			
			
			if (NewSelectionFromUserInteraction == null) NewSelectionFromUserInteraction = GetIndexOfIDAss.GetMethod("NewSelectionFromUserInteraction", (BindingFlags)(-1));
			
			NewSelectionFromUserInteraction.Invoke(treeView, new object[] { res.Values.ToList(), newlast ?? res.First().Value });
			//this.EnsureRowIsVisible(row, false);
			//this.SelectionByKey(rows[row]);
		}
		
		catch (Exception ex)
		{
		
		
			Debug.LogError("[Hierarchy Plugin] Multi-selection caused an error, disable this option in the settings\n\n" + ex.Message + "\n\n" + ex.StackTrace);
			return false;
		}
		
		return true;
		
	}
	
	
	
	
	internal bool SKIP_CHILDCOUNT = false;
	
	
	
	bool IS_LAYOUT
	{	get { return Event.current != null && Event.current.type == EventType.Layout/*|| Event.current.type == EventType.ExecuteCommand && Event.current.commandName == "NewKeyboardFocus"*/; }
	}
	
	bool LockChess = false;
	object[] obj_array = new object[1];
	// [ThreadAndSerializationSafe]
	internal  Color TintColor;
	
	
	//  internal  float ScrollWhell;
	internal  bool mayscroll = false;
	internal  bool? applicationIsPlaying;
	bool groupintLayout;
	EventType? hierScrilEv = null;
#pragma warning disable
	EventType? hierScrilEv23 = null;
#pragma warning restore
	
	List<EditorWindow> repaintImmidiately = new List<EditorWindow>();
	[SecuritySafeCritical]
	// [SecurityCritical]
	void Drawing( HierarchyObject o, Rect selectionRect )
	{	if ( oldScroll.HasValue )
		{	//   Debug.Log( "ASD" );
			return;
		}
		
		/*  if ( IS_LAYOUT && GROUPING_CHILD_MODE == 0 )
		  {   // Debug.Log( "ASD" );
		      return;
		  }*/
		// MonoBehaviour.print(Event.current.type);
		
		/*if (Event.current.type == EventType.ValidateCommand) {
		    MonoBehaviour.print(Event.current.command);
		    MonoBehaviour.print(Event.current.commandName);
		}
		if (Event.current.type == EventType.ExecuteCommand) {
		    MonoBehaviour.print(Event.current.command);
		    MonoBehaviour.print(Event.current.commandName);
		}*/
		
		if ( Event.current.type == EventType.DragPerform )     //M_CustomIconsModule.Clear();
		{	ColorModule.ClearCacheFull();
			// RepaintWindowInUpdate();
		}
		
		/* if (Event.current.type == EventType.dragUpdated) {
		    MonoBehaviour.print("dragUpdated");
		}*/
		/* if (Event.current.type == EventType.DragExited)
		 {
		     M_CustomIcons.updateTimer.Clear();
		    // RepaintWindowInUpdate();
		     // MonoBehaviour.print("DragExited");
		 }*/
		
		if ( Event.current.type == EventType.ScrollWheel )
		{	var HYPER  = HYPER_ENABLE() && bottomInterface.hyperGraph.CAPTURE_CLIP_RECT.Contains(Event.current.mousePosition);
		
			if ( mayscroll && HYPER )     // if (!) return;
			{	// MonoBehaviour.print("ASD");
				if ( OnScroll != null ) OnScroll( ScrollType.HyperGraphScroll, Event.current.delta.y );
				
				mayscroll = false;
			}
			
			if ( HYPER ) EventUse();
			
			var FAVOR  = FAV_ENABLE() && bottomInterface.favorGraph.CAPTURE_CLIP_RECT.Contains(Event.current.mousePosition);
			
			if ( mayscroll && FAVOR )     // if (!) return;
			{	// MonoBehaviour.print("ASD");
				if ( OnScroll != null ) OnScroll( ScrollType.FavorGraphScroll, Event.current.delta.y );
				
				mayscroll = false;
			}
			
			if ( FAVOR ) EventUse();
			
			/*  var rect = bottomInterface.GetNavigatorRect(  selectRect.x + selectRect.width);
			
			  if ()*/
		}
		
		if ( Event.current.type == EventType.Repaint )
		{	mayscroll = true;
		}
		
		
		
		if ( !Event.current.isMouse && Event.current.type != EventType.Repaint && Event.current.type != EventType.Ignore && Event.current.type != EventType.Used ||
		        Event.current.type == EventType.MouseMove/* || Event.current.type == EventType.mouseDrag*/ )
		{
		
			groupintLayout = GROUPING_CHILD_MODE == 1 && IS_LAYOUT;
			
			if ( !groupintLayout )
			{	// Debug.Log( Event.current.command + " " + Event.current.commandName + " " + selectionRect );
				// return;
			}
		}
		
		// if (Event.current.type== EventType.mouseDrag || Event.current.type == EventType.mouseMove) return;
		
		/*   MonoBehaviour.print(window().);
		if (Application.isPlaying && window().position.Contains(Input.mousePosition)) {
		   MonoBehaviour.print("ADAS");
		}*/
		var w = window();
		
		if ( w == null )
		{	//Debug.Log( "1ASD" );
		
			return;
		}
		
		
		
		if ( hierScrilEv  != Event.current.type || Event.current.type == EventType.Repaint )
		{	hierScrilEv = Event.current.type;
			HierWinScrollPos = GetHierarchyWindowScrollPos();
			
			
		}
		
		
		/*   if ( RedrawInit ) Debug.Log(
		           o.Validate() + "\n"
		           + HierWinScrollPos + "\n"
		           + selectionRect + "\n"
		           + w.position + "\n"
		           + bottomInterface._HEIGHT  + "\n"
		           + (selectionRect.y - HierWinScrollPos.y > w.position.height + (bottomInterface._HEIGHT ?? 0) + 50)); ;    */       //   if (window() != null)
		{	// MonoBehaviour.print(scrollPos);
			if ( selectionRect.y - HierWinScrollPos.y < -50 )
			{	//Debug.Log( o.name );
				return;
			}
			
			if ( UNITY_5_5 )
			{	if ( selectionRect.y - HierWinScrollPos.y > w.position.height + 50 ) return;
			}
			
			else
			{	if ( selectionRect.y - HierWinScrollPos.y > w.position.height + (bottomInterface._HEIGHT ?? 0) + 50 ) return;
			
			}
			
			
		}
		bool drawPlugin = selectionRect.y - HierWinScrollPos.y > w.position.height;
		drawPlugin = !drawPlugin;
		//  drawPlugin = true;
		//if (drawPlugin) selectionRect.width -= PREFAB_BUTTON_SIZE;
		var Layout = groupintLayout && IS_LAYOUT;
		//if ( Layout ) return;
		
		if ( o.Validate() && !Layout )
		{	customMenuModules.DrawExtensionMenu( this, o, selectionRect );
		
		
		}
		
		
		
		if ( Event.current.type == EventType.Used )
		{	//  Debug.Log( Event.current.type );
			return;
		}
		
		//  if (!wasFirstUpdate)
		{	// wasFirstUpdate = true;
			if ( !wasIconsInitialize ) InitializeIcons();
		}
		
		
		
		
		/*if (!markedObjects.ContainsKey(instanceID))
		{
		
		    HierarchyWindowChanged();
		    //if (selectionRect.x != 0) HierarchyWindowChanged();
		    return;
		}*/
		
		
		
		
		if ( !par.ENABLE_ALL || !wasIconsInitialize )
		{	// Debug.Log( Event.current.type );
			return;
		}
		
		// if (colorText11 == null) COnst();
		
		if ( !Hierarchy_GUI.Instance( this ) )
		{	if ( !wasWarning ) logProxy.LogWarning( "Please Reinstall Hierarchy Plugin" );
		
			wasWarning = true;
			//   Debug.Log( Event.current.type );
			return;
		}
		
		/*  var oldFOnt = Adapter.GET_SKIN().button.fontSize;
		  var oldFOntl = Adapter.GET_SKIN().label.fontSize;
		  var oldFOnal = Adapter.GET_SKIN().label.alignment;
		  var oldT = Adapter.GET_SKIN().button.normal.background;
		  var oldBh = Adapter.GET_SKIN().button.hover.background;
		  var oldf = Adapter.GET_SKIN().button.focused.background;
		  var oldact = Adapter.GET_SKIN().button.active.background;
		  var oldTc = Adapter.GET_SKIN().button.normal.textColor;
		
		  Adapter.GET_SKIN().button.normal.background = null;
		  Adapter.GET_SKIN().button.hover.background = null;
		  Adapter.GET_SKIN().button.focused.background = GetIcon("BUTBLUE");
		  Adapter.GET_SKIN().button.active.background = GetIcon("BUTBLUE");
		  var bt = Adapter.GET_SKIN().button.border.top;
		  var br = Adapter.GET_SKIN().button.border.right;
		  var bb = Adapter.GET_SKIN().button.border.bottom;
		  var bl = Adapter.GET_SKIN().button.border.left;
		  Adapter.GET_SKIN().button.border.top = Adapter.GET_SKIN().button.border.right = Adapter.GET_SKIN().button.border.bottom = Adapter.GET_SKIN().button.border.left = 0;
		
		  var al = Adapter.GET_SKIN().button.alignment;
		  Adapter.GET_SKIN().button.alignment = TextAnchor.MiddleCenter;
		  Adapter.GET_SKIN().button.fontSize = 8;
		  Adapter.GET_SKIN().label.fontSize = 10;
		  Adapter.GET_SKIN().label.alignment = TextAnchor.MiddleCenter;
		
		  */
		//  ChechEvents();
		
		ChangeGUI();
		
		
		//    if ( Event.current.type == EventType.Repaint )
		
		
		
		
		
		if ( drawPlugin )
		{	var headRect = selectionRect;
		
		
			if ( !Layout )
			{
			
				//EditorGUI.DrawRect(headRect, Color.white);
				// headRect.width -=  PREFAB_BUTTON_SIZE;
				if (/*Event.current.type != EventType.repaint &&*/ (!DrawHeaderOther.HasValue || DrawHeaderOther == o.id) )
				{	/*  if (oldRect.HasValue)
					      {
					          var currentRect = oldRect.Value;
					          currentRect.x = selectionRect.x;
					          foreach (var drawModule in modulesOrdered)
					          {
					              if (!drawModule.enable || (par.RIGHTDOCK_TEMPHIDE && window().position.width <= Hierarchy.par.RIGHTDOCK_TEMPHIDEMINWIDTH/* && !(drawModule is M_Freeze)#1#)) continue;
					              var width = Math.Max(drawModule.width, defWDTH);
					              currentRect.x -= width;
					          }
					          oldRect = currentRect;
					      }*/
					
					
					DrawHeaderOther = o.id;
					//  if (scrollPos.y != 0 && DrawHeadind == 0) DrawHeadind = 1;
					//   MonoBehaviour.print(selectionRect );
					// chessWidth = ;
					//  if (EditorSceneManager.GetActiveScene().isLoaded)
					_DG( headRect );
				}
				
			}
			
			//else
			if ( o.Validate() )
			{
			
				SKIP_CHILDCOUNT = false;
				
				// var drawRect = selectionRect;
				
				
				var oc = button.normal.textColor;
				
				try
				{	var fadeRect = selectionRect;
					fadeRect.x = GET_PADING( selectionRect.x + selectionRect.width );
					
					
					DrawBG( o, selectionRect, fadeRect );
					
					if ( !Layout )
					{
					
						if ( Event.current.type == EventType.Repaint )
						{
						
							if ( par.DRAW_HIERARHCHY_LINES_V2 == 1 && (pluginID == Initializator.HIERARCHY_ID || o.parentCount() != 0) ) DrawLines( o, selectionRect );
							
							// selectionRect.width -= drawRect.x;
							// MonoBehaviour.print(chessWidth);
							//  if (par.HierarhchyLines_Fix == 2) DrawChess(selectionRect, window().position.width - 20);
							if ( !LockChess )     //    Debug.Log( par.HierarhchyLines_Fix );
							{	if ( par.HierarhchyLines_Fix == 2 || ChechButton( _S_HideRightIfNoFunction ) ) DrawChess( selectionRect, selectionRect.x + selectionRect.width );
								else
									if ( par.HierarhchyLines_Fix == 1 ) DrawChess( selectionRect, fadeRect.x );
							}
							
						}
						
						
						
						//  if (window() != null) fadeRect.width = window().position.width - fadeRect.x;
						fadeRect.width = selectionRect.width + selectionRect.x - fadeRect.x;
						/* if (Selection.gameObjects.Contains(o)) SelectRect(fadeRect, par.HEADER_OPACITY ?? DefaultBGOpacity);
						 else */
						/*oldRect = */
						
						button.normal.textColor *= o.Active() ? Color.white : new Color(1, 1, 1, 0.5f);
						DrawModules( o, selectionRect, fadeRect );
					}
				}
				
				catch ( Exception ex )         //MonoBehaviour.print(ex.Source + " " +ex.HelpLink + " " +ex.GetObjectData();
				{	logProxy.LogError( "DrawModuleError " + ex.Message + " " + ex.StackTrace );
				
					RestoreGUI();
					return;
					/*par.ENABLE_ALL = false;
					SavePrefs();*/
				}
				
				button.normal.textColor = oc;
				
				if ( !Layout )
				{
				
				
					if ( Event.current.type == EventType.Repaint && par.DRAW_CHILDS_COUNT && !SKIP_CHILDCOUNT )
					{	var c = o.ChildCount(this);
					
						if ( c != 0 && window() )
						{	bool draw = !(par.HIDE_CHILDCOUNT_IFROOT && o.ParentIsNull());
						
							if ( draw && par.HIDE_CHILDCOUNT_IFEXPANDED )
							{	obj_array[0] = o.id;
								draw = !(bool)m_IsExpanded.Invoke(
								           m_data.GetValue(
								               m_TreeView( window() ), null ), obj_array );
							}
							
							if ( draw )
							{	r = selectionRect;
							
								r.y += (r.height - EditorGUIUtility.singleLineHeight) / 2;
								
								switch ( _S_CountNumber_Align )
								{	case -1: r.x = 0; break;
								
									case 0: r.x -= 3 + EditorGUIUtility.singleLineHeight; break;
									
									case 1:
										labelStyle.CalcMinMaxWidth( GET_CONTENT( o.name ), out w1, out w1 );
										r.x += w1 + EditorGUIUtility.singleLineHeight / 1.75f;
										break;
								}
								
								r.x += TOTAL_LEFT_PADDING;
								r.width -= TOTAL_LEFT_PADDING;
								r.height = r.width = EditorGUIUtility.singleLineHeight;
								//. r.y += r.height / 6;
								r.x += _S_CountNumber_OffsetX;
								r.y += _S_CountNumber_OffsetY;
								DRAW_SMALL_NUMBER( r, o.ChildCount( this ) );
							}
						}
						
					}
				}
				
			}
			
			else
			{	/* var rect = selectionRect;
				     rect.x = GET_PADING(selectionRect.x + selectionRect.width) - defWDTH;*/
				//FadeSceneRect(rect, 1);
			}
			
			//if (!o) GUI.DrawTexture(selectionRect, Texture2D.whiteTexture);
			if ( !Layout )
			{	if ( (headRect.y >= HierWinScrollPos.y + headRect.height || HierWinScrollPos.y == 0) && (!DrawHeadRepaint.HasValue || DrawHeadRepaint == o.id) )
				{	DrawHeadRepaint = o.id;
				
					//  if (scrollPos.y != 0 && DrawHeadind == 0) DrawHeadind = 1;
					// chessWidth = ;
					//  if (EditorSceneManager.GetActiveScene().isLoaded)
					//  if (Event.current.type == EventType.repaint)
					{	//   MonoBehaviour.print(instanceId);
						_DG( headRect );
						
						//  global::EModules.EModulesInternal.Hierarchy.Bottom_Interface.PaintGUI(selectionRect);
					}
				}
			}
		}
		
		
		//bottomInterface.BottomEventGUI( selectionRect, w );
		
		
		// if (!Layout )
		{	//#if !UNITY_EDITOR
			bottomInterface.BottomPaintGUI( o, selectionRect );
			//#if !UNITY_EDITOR
			
			
			//#endif
		}
		
		
		
		
		
		RestoreGUI();
		
		
		
		
	}
	float w1;
#pragma warning disable
	private bool PlayModeFix;
#pragma warning restore
	GUIStyle __smallNumbStyle;
	GUIStyle smallNumbStyle
	{	get
		{	if ( __smallNumbStyle == null )
			{	__smallNumbStyle = new GUIStyle( label );
				__smallNumbStyle.alignment = TextAnchor.MiddleCenter;
				smallNumbStyle.normal.textColor = EditorGUIUtility.isProSkin ? Color.black : Color.white;
			}
			
			__smallNumbStyle.fontSize = FONT_8() - 1;
			return __smallNumbStyle;
		}
	}
	GUIStyle __smallNumbStyleNormal;
	GUIStyle smallNumbStyleNormal
	{	get
		{	if ( __smallNumbStyleNormal == null )
			{	__smallNumbStyleNormal = new GUIStyle( label );
				__smallNumbStyleNormal.alignment = TextAnchor.MiddleCenter;
			}
			
			__smallNumbStyleNormal.fontSize = FONT_8() - 1;
			return __smallNumbStyleNormal;
		}
	}
	internal void DRAW_SMALL_NUMBER( Rect rect, int number )
	{	if (Event.current.type != EventType.Repaint) return;
	
		//         var s = Adapter.GET_SKIN().label.fontSize;
		//             var a = Adapter.GET_SKIN().label.alignment;
		//             Adapter.GET_SKIN().label.fontSize = FONT_8() - 1;
		//             Adapter.GET_SKIN().label.alignment = TextAnchor.MiddleCenter;
		rect.x -= 1;
		//   GUI.Label( rect, number.ToString(), smallNumbStyle );
		smallNumbStyle.Draw( rect, number.ToString(), false, false, false, false );
		rect.x += 2;
		rect.y += 1;
		//  GUI.Label( rect, number.ToString(), smallNumbStyle );
		smallNumbStyle.Draw( rect, number.ToString(), false, false, false, false );
		rect.x -= 1;
		rect.y -= 1;
		// GUI.Label( rect, number.ToString(), smallNumbStyle );
		smallNumbStyleNormal.Draw( rect, number.ToString(), false, false, false, false );
	}
	internal void DRAW_SMALL_NUMBER( Rect rect, string number )
	{	if (Event.current.type != EventType.Repaint) return;
	
		//  var casd = smallNumbStyle.normal.textColor;
		//   smallNumbStyle.normal.textColor = EditorGUIUtility.isProSkin ? Color.black : Color.white;
		rect.x -= 1;
		rect.y += 1;
		//  GUI.Label( rect, number, smallNumbStyle );
		smallNumbStyle.Draw( rect, number,  false, false, false, false );
		rect.x += 2;
		// GUI.Label( rect, number, smallNumbStyle );
		smallNumbStyle.Draw( rect, number,  false, false, false, false );
		rect.y -= 2;
		// GUI.Label( rect, number, smallNumbStyle );
		smallNumbStyle.Draw( rect, number,  false, false, false, false );
		rect.x -= 2;
		// GUI.Label( rect, number, smallNumbStyle );
		smallNumbStyle.Draw( rect, number,  false, false, false, false );
		rect.x += 1;
		rect.y += 1;
		/*   rect.x -= 1;
		   GUI.Label(rect, number);
		   rect.x += 1;
		   rect.y += 1;
		   GUI.Label(rect, number);
		   rect.x += 1;
		   rect.y -= 1;
		   GUI.Label(rect, number);
		   rect.x -= 1;
		   rect.y -= 1;
		   GUI.Label(rect, number);
		rect.y += 1;
		   */
		// smallNumbStyle.normal.textColor = casd;
		// rect.x -= 1;
		//   GUI.Label( rect, number, smallNumbStyle );
		smallNumbStyleNormal.Draw( rect, number,  false, false, false, false );
		//             Adapter.GET_SKIN().label.fontSize = s;
		//             Adapter.GET_SKIN().label.alignment = a;
	}
}
}
