#define USE_CACHE__

#if UNITY_EDITOR
	#define PROJECT
	#define HIERARCHY
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine.Profiling;
#if PROJECT
	using EModules.Project;
#endif
using UnityEditor.SceneManagement;
namespace EModules.EModulesInternal


{


internal partial class Adapter

{






	HierarchyObject gettedObject;
	//object[] args = new object[1];
	//Dictionary<string , int> guid_to_instanceid = new Dictionary<string , int>();
	Dictionary<int, object> wasInitWindows = new Dictionary<int, object>();
	Dictionary<int, FieldInfo> wasInitWindowsField = new Dictionary<int, FieldInfo>();
	
	EventType? oldEvent;
	
	private void proj_Main( string guid, Rect selectionrect )
	{
	
		if ( oldEvent.HasValue && oldEvent != Event.current.type ) return;
		
		oldEvent = Event.current.type;
		
		//#initialize #tag
		if ( !INIT_TREE() ) oldEvent = null;
		
		// m_Main( GetHierarchyObjectByGUID( guid ), selectionrect );
		/*  w.GetInstanceID
		  var path = AssetDatabase.GUIDToAssetPath( guid );
		  if (path.Contains( "tree_mesh" )) Debug.Log( path + " " + guid );*/
		
		//args[0] = guid;
		//var _id = (int)GetInstanceIDFromGUID.Invoke( null , args );
		
		
		/*AssetDatabase.GUIDToAssetPath( guid );
		Hash*/
		/*  AssetDatabase
		  m_Main()*/
		
		//m_Main( GetHierarchyObjectByGUID( guid ), selectionrect );
		
	}
	private void proj_Additional( int id, Rect selectionrect )      //var o = EditorUtility.InstanceIDToObject(id);
	{	// if (o.name.Contains( "tree_mesh" )) Debug.Log( o.name );
		m_Main( GetHierarchyObjectByInstanceID( id ), selectionrect );
	}
	
	private void hier_Main( int instanceid, Rect selectionrect )
	{	m_Main( GetHierarchyObjectByInstanceID( instanceid ), selectionrect );
	}
	
	void SWITCHER_RELOAD_DATA( Adapter adapter )     //#initialize #tag
	{	/*#if !USE2018
		      adapter.RepaintAllViews();
		#else*/
		/// Debug.Log( "ASD" );
		var treeView = adapter.m_TreeView(adapter.window());
		// treeView.GetType().GetField( "m_StopIteratingItems" , (BindingFlags)(-1) ).SetValue( treeView , true );
		treeView.GetType().GetMethod( "ReloadData", (BindingFlags)(-1) ).Invoke( treeView, null );
		adapter.ClearTree();
		
		//SendEventAll( new Event() { type = EventType.Layout } );
		//HEIGHT_RIX_FUNCTIUON( adapter.window(), treeView );
		/*#endif*/
	}
	
	bool INIT_TREE()
	{
	
		if ( onGUIRowCallback == null ) return false;
		
		var w = window();
		
		if ( w == null )
		{	return false;
		}
		
		
		
		if ( m_TreeViewFieldInfo == null )
		{	return false;
		}
		
		if ( onGUIRowCallback == null )
		{	return false;
		}
		
		
		if ( pluginID == Initializator.PROJECT_ID )
		{	if ( !wasInitWindowsField.ContainsKey( w.GetInstanceID() ) || wasInitWindowsField[w.GetInstanceID()] != m_TreeViewFieldInfo )
			{	wasInitWindowsField.Remove( w.GetInstanceID() );
				wasInitWindowsField.Add( w.GetInstanceID(), m_TreeViewFieldInfo );
				tree_cache.Clear();
				bottomInterface.NEED_READ_LIST.Clear();
				/*  GUI_ONESHOT = true;
				  GUI_ONESHOTAC = () =>
				  {   INIT_TREE( w.GetInstanceID() );
				  };*/
			}
		}
		
		var tree = m_TreeView(w);
		
		if ( !wasInitWindows.ContainsKey( w.GetInstanceID() ) )
		{	wasInitWindows.Add( w.GetInstanceID(), tree );
			INIT_TREE( w.GetInstanceID() );
		}
		
		
		
		if ( wasInitWindows[w.GetInstanceID()] != tree )
		{	wasInitWindows[w.GetInstanceID()] = tree;
			INIT_TREE( w.GetInstanceID() );
		}
		
		
		
		return true;
	}
	
	Dictionary<object, bool> wasassigned = new Dictionary<object, bool>();
	void INIT_TREE( int wID )
	{	var tree = wasInitWindows[wID];
	
		if ( tree == null ) return;
		
		if ( wasassigned.ContainsKey( tree ) ) return;
		
		wasassigned.Add( tree, true );
		var eventInfo = (Action<int, Rect>)onGUIRowCallback.GetValue(tree, null);
		// eventInfo -= proj_Additional;
		eventInfo += proj_Additional;
		onGUIRowCallback.SetValue( tree, eventInfo, null );
	}
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	bool needRepaint = false;
	// int repC = 0;
	
	internal void RepaintAllViews()     //
	{	InternalEditorUtility.RepaintAllViews();
		//MonoBehaviour.print( "RepaintAllViews" );
	}
	
	internal void SendEventAll( Event ev )
	{	if ( Application.isPlaying ) return;
	
		var oldControl = GUIUtility.keyboardControl;
		
		if ( hierarchy_windows.Count != 0 )
		{	foreach ( var hierarchyWindow in hierarchy_windows )
			{	if ( hierarchyWindow == null ) continue;
			
				var tree = m_TreeView(hierarchyWindow as EditorWindow);
				
				if ( tree == null ) continue;
				
				GUIUtility.keyboardControl = (int)m_KeyboardControlIDField.GetValue( tree );
				
				((EditorWindow)hierarchyWindow).SendEvent( ev );
				// bottomInterface. BOTTOM_UPDATE_POSITION( ((EditorWindow)hierarchyWindow), true );
				
			}
		}
		
		else
			if ( window() != null )
			{	var tree = m_TreeView(window());
			
				if ( tree != null )
				{	GUIUtility.keyboardControl = (int)m_KeyboardControlIDField.GetValue( tree );
				
					((EditorWindow)window()).SendEvent( ev );
					//  bottomInterface. BOTTOM_UPDATE_POSITION( ((EditorWindow)window()), true );
				}
				
			}
			
		GUIUtility.keyboardControl = oldControl;
	}
	internal void RepaintWindow( bool force = false )
{	if ( !force ) if ( Event.current != null || m_KeyboardControlIDField == null )
			{	RepaintWindowInUpdate();
				return;
			}
			
		needRepaint = false;
		
		//  MonoBehaviour.print("RepaintWindow");
		var oldControl = GUIUtility.keyboardControl;
		
		if ( hierarchy_windows.Count != 0 )
		{	foreach ( var hierarchyWindow in hierarchy_windows )
			{	if ( hierarchyWindow == null ) continue;
			
				var tree = m_TreeView(hierarchyWindow as EditorWindow);
				
				if ( tree == null ) continue;
				
				GUIUtility.keyboardControl = (int)m_KeyboardControlIDField.GetValue( tree );
				
				((EditorWindow)hierarchyWindow).Repaint();
			}
		}
		
		else
			if ( window() != null )
			{	var tree = m_TreeView(window());
			
				if ( tree != null )
				{	GUIUtility.keyboardControl = (int)m_KeyboardControlIDField.GetValue( tree );
				
					((EditorWindow)window()).Repaint();
				}
				
			}
			
			
			
		GUIUtility.keyboardControl = oldControl;
		
		foreach ( var w in bottomInterface.WindowController )
		{	if ( w.REFERENCE_WINDOW ) w.REFERENCE_WINDOW.Repaint();
		}
		
		foreach ( var w in bottomInterface.FavoritControllers )
		{	if ( w.REFERENCE_WINDOW ) w.REFERENCE_WINDOW.Repaint();
		}
		
		// RepaintAllViewInUpdate();
		//  window().Repaint();
		//MonoBehaviour.print("RepaintWindow");
		// RestoreGUI();
	}
	
	
	internal void RepaintWindowInUpdate()     // MonoBehaviour.print("RepaintWindowInUpdate");
	{	// CLearAdditionalCache();
		//Debug.Log( "ASD" );
		needRepaint = true;
	}
	
	
	
	
	private IList hierarchy_windows = new ArrayList();
	
	
	
	int GET_CONTROL() { return GUIUtility.keyboardControl; }
	string ID_STRING = "m_KeyboardControlID";
	string ID_STRING2 = "m_TreeViewKeyboardControlID";
	
	
	
	
	
	Dictionary<int, EditorWindow> GUIControlToWindowObject = new Dictionary<int, EditorWindow>();
	Dictionary < int, Vector2? > ScrollPositions = new Dictionary < int, Vector2? >();
	Dictionary < int, float? > WasInitDraw = new Dictionary < int, float? >();
	Dictionary < int, float? > WasEvent = new Dictionary < int, float? >();
	Dictionary < int, float? > WasPaint = new Dictionary < int, float? >();
	Type SearchableWindowType;
	MethodInfo GetTotalSizeMethodInfo;
	
	
	
	//SearchFilter
	bool? seach_baked;
	internal void SEARCH_HAS_BAKE()
	{	if ( m_SearchFilterString == null )
		{	seach_baked = false;
			return;
		}
		
		if ( pluginID == Initializator.HIERARCHY_ID )
		{	seach_baked = !string.IsNullOrEmpty( (string)m_SearchFilterString.GetValue( window() ) );
			return;
		}
		
		seach_baked = (bool)m_SearchFilterClass_Has.Invoke( m_SearchFilterString.GetValue( window() ), null );
	}
	
	
	internal bool IS_SEARCH_MOD_OPENED()
	{
	
		if ( ChechButton( _S_HideBttomIfNoFunction ) || PlayModeFix && IMGUI() ) return true;
		
		if ( !seach_baked.HasValue ) SEARCH_HAS_BAKE();
		
		return seach_baked.Value;
	}
	internal bool IS_PREFAB_MOD_OPENED()
	{	if ( hasShowingPrefabHeader && showingPrefabHeader.GetValue( window(), null ).Equals( true ) ) return true;
	
		return false;
	}
	
	
	bool? oldIS_SEARCH_MODE_OR_PREFAB_OPENED;
	internal bool IS_SEARCH_MODE_OR_PREFAB_OPENED()
	{	var res = __IS_SEARCH_MODE_OR_PREFAB_OPENED();
	
		if ( !oldIS_SEARCH_MODE_OR_PREFAB_OPENED.HasValue ) oldIS_SEARCH_MODE_OR_PREFAB_OPENED = res;
		
		if ( oldIS_SEARCH_MODE_OR_PREFAB_OPENED.Value != res ) RedrawInit = true;
		
		return res;
	}
	bool __IS_SEARCH_MODE_OR_PREFAB_OPENED()
	{	// return false;
		if ( ChechButton( _S_HideBttomIfNoFunction ) || PlayModeFix && IMGUI() ) return true;
		
		if ( !seach_baked.HasValue ) SEARCH_HAS_BAKE();
		
		if ( hasShowingPrefabHeader && showingPrefabHeader.GetValue( window(), null ).Equals( true ) ) return true;
		
		return seach_baked.Value;
	}
	object inst, em;
	MethodInfo meth;
	List<Type> AllTypesOfIRepository;
	internal void CLOSE_PREFAB_MODE()
	{	if ( !hasShowingPrefabHeader ) return;
	
		if ( !showingPrefabHeader.GetValue( window(), null ).Equals( true ) ) return;
		
		if ( AllTypesOfIRepository == null)    AllTypesOfIRepository = (from x in Assembly.GetAssembly(typeof(EditorWindow)).GetTypes()
			        let y = x.BaseType
			        where !x.IsAbstract && !x.IsInterface &&
			        y != null && y.IsGenericType &&
			        y.GetGenericTypeDefinition() == typeof(ScriptableSingleton<>)
			        select x
			                                                               ).ToList();
			                                                               
		/*GUI_ONESHOTPUSH( () =>
		{   UnityEditor.SceneManagement.StageUtility.GoBackToPreviousStage();
		} );*/
		GUI_ONESHOTPUSH( () =>
		{	foreach ( var asd in AllTypesOfIRepository )
			{	if ( asd.Name == "StageNavigationManager" )
				{	if ( meth == null )
					{	inst = asd.BaseType.GetProperty( "instance", (BindingFlags)(-1) ).GetValue( null, null );
						meth = inst.GetType().GetMethod( "NavigateBack", (BindingFlags)(-1) );
						em = Enum.Parse( meth.GetParameters()[0].ParameterType, "NavigateBackViaHierarchyHeaderLeftArrow" );  //NavigateBackViaUnknown  NavigateBackViaHierarchyHeaderLeftArrow NavigateViaBreadcrumb
					}
					
					meth.Invoke( inst, new[] { em } );
				}
			}
		} );
		Adapter.EventUseFast();
		
		SendEventAll( new Event() { type = EventType.Layout, mousePosition = Vector2.zero, button = 0  } );
		
		
		
		
	}
	
	/*    internal bool SEARCH_GETSTRING( )
	{
	if ( pluginID == Initializator.HIERARCHY_ID )
	{
	  return
	}
	}*/
	
	
	/*     MethodInfo m_SetExpanded;
	     PropertyInfo m_treeData;*/
	
#if USE_CACHE__
	void BACK_WIN()
	{	__window = bakewin();
		/* if ( __window )
		 {   var b = bakem_TreeView( __window );
		     if ( !tree_cache.ContainsKey( __window ) ) tree_cache.Add( __window, b );
		     else tree_cache[__window] = b;
		 }*/
	}
	EditorWindow __window;
	internal EditorWindow window( bool brakable = false )     // EditorWindow.FocusWindowIfItsOpen<>(
	{
	
		if ( __window ) return __window;
		
		return __window = bakewin( brakable );
	}
	EditorWindow bakewin( bool brakable = false )
#else
	void BACK_WIN() { }
	internal EditorWindow window( bool brakable = false )     // EditorWindow.FocusWindowIfItsOpen<>(
#endif
	{
	
		// if (has_s_LastInteractedHierarchy) return s_LastInteractedHierarchy.GetValue(null) as EditorWindow;
		
		var id = GET_CONTROL();
		
		/* if (hierarchy_windows.Count == 0) {
		 }*/
		//   return GUIUtility.GetStateObject(hierarchy_windows[0].GetType(), GUIUtility.GetControlID(FocusType.Passive)) as EditorWindow;
		//  EditorGUIUtility .
		
		if ( !GUIControlToWindowObject.ContainsKey( id ) || !GUIControlToWindowObject[id] )     //  Debug.Log(id);
		{	if ( brakable ) return null;
		
			if ( strange_field == null ) return null;
			
			// hierarchy_windows = (strange_field.GetValue( null )) as IList;
			if ( hierarchy_windows == null || hierarchy_windows.Count == 0 ) return null;
			
			if ( id != 0 )
				for ( int i = 1 ; i < hierarchy_windows.Count ; i++ )
				{	var tree = m_TreeView(hierarchy_windows[i] as EditorWindow);
				
					if ( tree == null ) continue;
					
					//   var m_KeyboardControlIDField = tree.GetType().GetField(ID_STRING, (BindingFlags)(-1));
					var m_KeyboardControlID = (int)m_KeyboardControlIDField.GetValue(tree);
					
					if ( m_KeyboardControlID == id ) return null;
				}
				
				
			if ( GUIControlToWindowObject.ContainsKey( id ) )
			{	GUIControlToWindowObject.Remove( id );
				ScrollPositions.Remove( id );
				WasInitDraw.Remove( id );
				WasEvent.Remove( id );
				WasPaint.Remove( id );
			}
			
			
			object target = null;
			
			if ( hierarchy_windows.Count != 0 )
			{	target = (EditorWindow)hierarchy_windows[0];
			
			
				/*     foreach (var hierarchyWindow in hierarchy_windows)
				     {
				         var tree = m_TreeView.GetValue(hierarchyWindow, null);
				         //   var m_KeyboardControlIDField = tree.GetType().GetField(ID_STRING, (BindingFlags)(-1));
				         var m_KeyboardControlID = (int)m_KeyboardControlIDField.GetValue(tree);
				         if (m_KeyboardControlID != id) continue;
				         target = (EditorWindow)hierarchyWindow;
				     }*/
				
				/*  if (target == null)*/
				/*  var tree = m_TreeView.GetValue(hierarchy_windows[0], null);
				  //   var m_KeyboardControlIDField = tree.GetType().GetField(ID_STRING, (BindingFlags)(-1));
				  var m_KeyboardControlID = (int)m_KeyboardControlIDField.GetValue(tree);
				 // if (m_KeyboardControlID != id) return null;
				  target = (EditorWindow)hierarchy_windows[0];*/
			}
			
			/*    #if PROJECT
			else if (pluginname == Initializator.PROJECT_NAME)
			{
			target = Resources.FindObjectsOfTypeAll<EditorWindow>().FirstOrDefault( w => w.GetType().FullName == "UnityEditor.ProjectBrowser" );
			}
			#endif
			
			
			#if HIERARCHY
			else if ( pluginID == Initializator.HIERARCHY_ID )
			{
			target = Resources.FindObjectsOfTypeAll<EditorWindow>().FirstOrDefault( w => w.GetType().FullName == "UnityEditor.SceneHierarchyWindow" );
			}
			#endif*/
			
			//MonoBehaviour.print(pluginname + " " + (target != null ? target.GetType().FullName : "null") );
			
			
			/*  foreach (var hierarchyWindow in hierarchy_windows) {
			      var tree = m_TreeView.GetValue(hierarchyWindow, null);
			      //   var m_KeyboardControlIDField = tree.GetType().GetField(ID_STRING, (BindingFlags)(-1));
			      var m_KeyboardControlID = (int)m_KeyboardControlIDField.GetValue(tree);
			      if (m_KeyboardControlID == id) {
			          target = hierarchyWindow;
			          break;
			      }
			  }*/
			
			
			GUIControlToWindowObject.Add( id, (EditorWindow)target );
			ScrollPositions.Add( id, null );
			WasInitDraw.Add( id, null );
			WasEvent.Add( id, null );
			WasPaint.Add( id, null );
			
			//   var m_KeyboardControlIDField = tree.GetType().GetField(ID_STRING, (BindingFlags)(-1));
			// MonoBehaviour.print(hierarchy_windows.Count + " " + m_KeyboardControlIDField.GetValue(m_TreeView.GetValue(target, null)));
			
			//#TODO 2019 DISABLE BOTTOM UPDATE
			//bottomInterface.BOTTOM_UPDATE_POSITION( (EditorWindow)target );
			
			
			
		}
		
		//  MonoBehaviour.print( GUIControlToWindowObject.Count + " " + GUIControlToWindowObject[id] );
		
		return  GUIControlToWindowObject[GET_CONTROL()];
		
		/*
		 if (hierarchy_windows == null)
		 {
		
		     hierarchy_windows = ((IList)field.GetValue(null));
		
		 }*/
		
		
		
		/*     var w = GUIControlToWindowObject[GETID()];
		     var m_Parent = typeof(EditorWindow).GetField("m_Parent", (BindingFlags)(-1)).GetValue(w);
		
		     var hostVIew = Assembly.GetAssembly(typeof(EditorWindow)).GetType("UnityEditor.HostView");
		     w = (EditorWindow)hostVIew.GetField("m_ActualView", (BindingFlags)(-1)).GetValue(m_Parent);
		
		     return w;*/
		
		
		//  return _window;
	}
	
	
	Vector2 GetHierarchyWindowScrollPos()
	{
	
		if ( window() == null ) return Vector2.zero;
		
		// return Vector2.zero;
		if ( !ScrollPositions.ContainsKey( GET_CONTROL() ) ) ScrollPositions.Add( GET_CONTROL(), null );
		
		if ( ScrollPositions[GET_CONTROL()] == null )
		{
		
			var tree = m_TreeView(window());
			var state = m_state.GetValue(tree, null);
			
			if ( !PLAYMODECHANGE ) ScrollPositions[GET_CONTROL()] = (Vector2)scrollPosField.GetValue( state );
			else return Vector2.zero;
			
			/* ScrollPositions[GETID()] = (Vector2)stateVal.GetType()
			
			     .GetValue(stateVal);*/
		}
		
		return ScrollPositions[GET_CONTROL()].Value;
		
	}
	
	internal void ResetScroll()
	{	var tree = m_TreeView(window());
	
		if ( tree == null ) return;
		
		var state = m_state.GetValue(tree, null);
		scrollPosField.SetValue( state, Vector2.zero );
		RepaintWindow();
	}
	internal void ResetScroll( Vector2 v )
	{	var tree = m_TreeView(window());
	
		if ( tree == null ) return;
		
		var state = m_state.GetValue(tree, null);
		scrollPosField.SetValue( state, v );
		RepaintWindow();
	}
	
	
	
	
	//         internal void RESET_HEIGHT_RIX_FUNCTIUON() {
	//             lastInst = null;
	//         }
	
	
	object lastInst ;
	float lastmPos;
	float lastm_VisibleRect;
#pragma warning disable
	float lastm_TotalRect;
#pragma warning restore
	internal void HEIGHT_RIX_FUNCTIUON( EditorWindow w, object treeView, bool SCENECHANGED = false )
	{
	
	
		if ( !ENABLE_BOTTOMDOCK_PROPERTY && bottomInterface.HEIGHT == 0 ) return;
		
		if ( treeView == null ) treeView = m_TreeView( w );
		
		// if ( IS_LAYOUT && RedrawInit ) return;
		// if ( firstFrame < 4 || IS_LAYOUT) return;
		// if ( firstFrame < 2 ) return;
		// if ( firstFrame < 4 && IS_LAYOUT ) return;
		
		// if (  IS_LAYOUT) return;
		
		
		bool resetScroll = false;
		
		if ( lastInst != treeView/* && IMGUI()*/ )
		{	lastInst = treeView;
		
			// var tree = m_TreeView( window() );
			
			//  Debug.Log( "ASD" );
			//   lastm_TotalRect = lastmPos = lastm_VisibleRect = 0;
#pragma warning disable
			
			if ( AGAIM_HEIGHT_FIXER )
			{	resetScroll = true;
				bottomInterface.BOTTOM_UPDATE_POSITION( window() );
			}
			
#pragma warning restore
		}
		
		// if ( IS_LAYOUT ) return;
		
		if ( treeView == null ) return;
		
		
		if ( (UNITY_5_5 || SCENECHANGED || resetScroll) )
		{	var mTotalRectGet = (Rect)m_TotalRect.GetValue(treeView);
		
			if ( lastm_TotalRect != mTotalRectGet.height )
			{	mTotalRectGet.height -= bottomInterface.HEIGHT;
				m_TotalRect.SetValue( treeView, mTotalRectGet );
				lastm_TotalRect = mTotalRectGet.height;
				
			}
		}
		
		if ( Event.current != null/* && Event.current.type == EventType.Repaint*/ ) //
		{	var mVisibleRect = (Rect)m_VisibleRect.GetValue(treeView);
		
			if ( lastm_VisibleRect != mVisibleRect.height )
			{
			
				//  mVisibleRect.height -= bottomInterface.HEIGHT - ((w.position.height / parLINE_HEIGHT) - (w.position.height / EditorGUIUtility.singleLineHeight)) * parLINE_HEIGHT;
				mVisibleRect.height -= bottomInterface.HEIGHT/* - ((w.position.height / parLINE_HEIGHT) - (w.position.height / EditorGUIUtility.singleLineHeight)) * parLINE_HEIGHT*/;
				m_VisibleRect.SetValue( treeView, mVisibleRect );
				lastm_VisibleRect = mVisibleRect.height;
			}
		}
		
		
		var mPos = (Rect)m_Pos.GetValue(w);
		
		if ( lastmPos != mPos.height )
		{	mPos.height -= bottomInterface.HEIGHT;
			// Debug.Log( lastmPos );
			
			m_Pos.SetValue( w, mPos );
			lastmPos = mPos.height;
		}
		
		
		
		
		if ( resetScroll )
		{
#pragma warning disable
			bottomInterface.BOTTOM_UPDATE_POSITION( window() );
#pragma warning restore
			RepaintWindow();
		}
	}
	
	
	
	
	
	internal void HEIGHT_RIX_FUNCTIUON_BACKUP( EditorWindow w, object treeView, bool SCENECHANGED = false )
	{
	
	
		if ( !ENABLE_BOTTOMDOCK_PROPERTY && bottomInterface.HEIGHT == 0 ) return;
		
		if ( treeView == null ) treeView = m_TreeView( w );
		
		// if ( IS_LAYOUT && RedrawInit ) return;
		
		if ( firstFrame < 4 && IS_LAYOUT ) return;
		
		
		
		
		bool resetScroll = false;
		
		// if ( !IS_LAYOUT  )
		if ( lastInst != treeView/* && IMGUI()*/ )
		{	lastInst = treeView;
			//  INIT_IF_NEDDED();
			// SWITCHER_RELOAD_DATA(this);
			// Debug.Log( "ASD" );
			lastm_TotalRect = lastmPos = lastm_VisibleRect = 0;
#pragma warning disable
			
			if ( AGAIM_HEIGHT_FIXER )
			{	resetScroll = true;
				bottomInterface.BOTTOM_UPDATE_POSITION( window() );
			}
			
#pragma warning restore
		}
		
		if ( treeView == null ) return;
		
		//  if ( !IS_LAYOUT  )
		
		if ( (UNITY_5_5 /*|| PLAYMODECHANGE || USE2018 */ || SCENECHANGED || resetScroll) )
		{	var mTotalRectGet = (Rect)m_TotalRect.GetValue(treeView);
		
			//  if (Event.current.type == EventType.layout) MonoBehaviour.print(mTotalRectGet);
			if ( lastm_TotalRect != mTotalRectGet.height )
			{	mTotalRectGet.height -= bottomInterface.HEIGHT;
				m_TotalRect.SetValue( treeView, mTotalRectGet );
				lastm_TotalRect = mTotalRectGet.height;
				
			}
			
			// Debug.Log("ASD");
			//if (SCENECHANGED) return;
		}
		
		/* if (NEW_INITIALIZER())
		 {
		
		     //  if (Event.current.type != EventType.Repaint)
		     {   var mVisibleRect = (Rect)m_VisibleRect.GetValue(treeView);
		         // if (Event.current.type == EventType.layout) MonoBehaviour.print(mVisibleRect);
		         if (lastm_VisibleRect != mVisibleRect.height)
		         {   mVisibleRect.height -= bottomInterface. HEIGHT;
		             m_VisibleRect.SetValue( treeView, mVisibleRect );
		             lastm_VisibleRect = mVisibleRect.height;
		         }
		     }
		
		     {   var mPos = (Rect)m_Pos.GetValue(w);
		         // if (Event.current.type == EventType.layout) MonoBehaviour.print(mPos);
		         if (lastmPos != mPos.height)
		         {   mPos.height -= bottomInterface. HEIGHT;
		
		
		             m_Pos.SetValue( w, mPos );
		             lastmPos = mPos.height;
		         }
		     }
		     return;
		 }*/
		
		// if (!PLAYMODECHANGE)
		// if (treeView != null)
		{
		
			if ( Event.current != null && Event.current.type == EventType.Repaint || NEW_RELOAD )
				//  if (Event.current.type != EventType.Repaint)
			{	var mVisibleRect = (Rect)m_VisibleRect.GetValue(treeView);
			
				// if (Event.current.type == EventType.layout) MonoBehaviour.print(mVisibleRect);
				if ( lastm_VisibleRect != mVisibleRect.height )
				{	mVisibleRect.height -= bottomInterface.HEIGHT;
					m_VisibleRect.SetValue( treeView, mVisibleRect );
					lastm_VisibleRect = mVisibleRect.height;
					//   Debug.Log( "VIS" );
				}
			}
			
			//  }
			
			/*if (Event.current.type == EventType.Repaint) */
			// if (Event.current.type == EventType.layout)
			//if ( !adapter.UNITY_2017_1 )
			{	var mPos = (Rect)m_Pos.GetValue(w);
			
				// if (Event.current.type == EventType.layout) MonoBehaviour.print(mPos);
				if ( lastmPos != mPos.height )
				{	mPos.height -= bottomInterface.HEIGHT;
				
					m_Pos.SetValue( w, mPos );
					lastmPos = mPos.height;
					// Debug.Log( "POS" );
				}
			}
		}
		
		
		
		
		if ( resetScroll )
		{	/* var state = m_state.GetValue(tree, null);
			     scrollPosField.SetValue( state, v );
			     ResetScroll();*/
#pragma warning disable
			bottomInterface.BOTTOM_UPDATE_POSITION( window() );
#pragma warning restore
			RepaintWindow();
		}
	}
	
	
	
	
	
	
	
	
	// static bool? haveInternalBox = null;
	static Color InternalBoxColor = new Color( 1, 1, 1, 0.5f );
	public static void INTERNAL_BOX( Rect r, GUIContent tc )
	{
	
	
		if ( !string.IsNullOrEmpty( tc.tooltip ) )
		{	GUI.Label( r, tc );
		}
		
		if ( Event.current.type == EventType.Repaint )
		{	var oc = GUI.color;
			GUI.color *= InternalBoxColor;
			Adapter.STYLE_InternalBoxStyle.Draw( r, tc.text, false, false, false, false );
			GUI.color = oc;
		}
		
		/*if ( !haveInternalBox.HasValue ) haveInternalBox = Adapter.GET_SKIN().box.normal.background;
		if ( haveInternalBox.Value ) GUI.Box( r , tc );
		else {
		    if ( Event.current.type == EventType.Repaint ) {
		        var oc = GUI.color;
		        GUI.color *= InternalBoxColor;
		        Adapter.GET_SKIN().textArea.Draw( r , tc.text , false , false , false , false );
		        GUI.color = oc;
		    }
		    if ( !string.IsNullOrEmpty( tc.tooltip ) ) {
		        oc = GUI.color;
		        GUI.color = new Color( 0 , 0 , 0 , 0 );
		        GUI.Label( r , tc );
		        GUI.color = oc;
		    }
		
		}*/
	}
	static GUIContent tc = new GUIContent();
	public static void INTERNAL_BOX( Rect r, string text )
	{	tc.text = text;
		INTERNAL_BOX( r, tc );
	}
	public static void INTERNAL_BOX( Rect r )
	{	INTERNAL_BOX( r, "" );
	}
	
	bool __GUI_POSTSHOT = false;
	Action __GUI_POSTSHOTAC = null;
	internal void GUI_POSTSHOTPUSH( Action a )
	{	__GUI_POSTSHOT = true;
		__GUI_POSTSHOTAC = a;
		RepaintWindowInUpdate();
	}
	
	bool __GUI_ONESHOT = false;
	Action __GUI_ONESHOTAC = null;
	internal void GUI_ONESHOTPUSH( Action a )
	{	__GUI_ONESHOT = true;
		__GUI_ONESHOTAC = a;
		RepaintWindowInUpdate();
	}
	
	bool ReloadEd = false;
	
	internal static void RequestScriptReload()
	{	Adapter.EditorUtilityRequestScriptReload();
		static_Reload();
	}
	
	[UnityEditor.Callbacks.DidReloadScripts]
	internal static void static_Reload()
	{	EditorSceneManager.sceneLoaded -= SCN1;
		EditorSceneManager.sceneLoaded += SCN1;
		staic_reload_a();
		
		foreach ( var a in Initializator.AdaptersByName )
		{	a.Value.Reload();
		}
	}
	static void SCN1( UnityEngine.SceneManagement.Scene s, UnityEngine.SceneManagement.LoadSceneMode mode )
	{	Hierarchy.HierarchyAdapterInstance.Reload();
		Hierarchy.HierarchyAdapterInstance.Again_Reloder_UsingWhenCopyPastOrAssets();
		//Hierarchy_GUI.Instance( Hierarchy.adapter).   ClearPrefabs();
	}
	static void SCN2( UnityEngine.SceneManagement.Scene s, UnityEngine.SceneManagement.Scene s2 )
	{	// Debug.Log( Hierarchy.HierarchyAdapterInstance.PLAYMODECHANGE );
		// Hierarchy.HierarchyAdapterInstance.Reload();
		//  Hierarchy.HierarchyAdapterInstance.Again_Reloder_UsingWhenCopyPastOrAssets();
		// Hierarchy_GUI.Instance( Hierarchy.adapter).   ClearPrefabs();
	}
	
	
	internal void Again_Reloder_UsingWhenCopyPastOrAssets()
	{	Hierarchy.HierarchyAdapterInstance.EditorSceneManagerOnSceneOpening( null, OpenSceneMode.Single );
	
		if ( Hierarchy.HierarchyAdapterInstance.onDidReloadScript != null ) Hierarchy.HierarchyAdapterInstance.onDidReloadScript();
		
		Hierarchy.HierarchyAdapterInstance.need_ClearHierarchyObjects1 = true;
		
		if ( onUndoAction != null ) onUndoAction();
	}
	
	internal Action onDidReloadScript;
	void Reload()     // if (par.ENABLE_ALL && IMGUI())
	{
	
	
		ReloadEd = true;
		ClearTree();
		
		if ( onDidReloadScript != null ) onDidReloadScript();
		
		//if (bottomInterface != null && bottomInterface.favorGraph != null) bottomInterface.favorGraph.ReloadScript();
	}
	
#pragma warning disable
	internal bool SCROOL_MEM()
	{	return false;
	}
	
	
	internal bool IMGUI()
	{
	
		return false;
		return !Adapter.NEW_RELOAD;
		return true;
		//return !Adapter.USE2018_3;
		//return ENABLE_BOTTOMDOCK_PROPERTY && par.FIX_IMGUI_Controls;
	}
	
	
	internal bool IMGUI20183()
	{	return false;
		//return Adapter.USE2018_3;
		//return ENABLE_BOTTOMDOCK_PROPERTY && par.FIX_IMGUI_Controls;
	}
#pragma warning restore
	
	
	Type dsType;
	FieldInfo m_Rows;
	MethodInfo InitIfNeeded;
	internal void INIT_IF_NEDDED()
	{	var treeView = m_TreeView(window());
		//    var state = adapter.m_state.GetValue(treeView, null);
		
		//  Debug.Log( "ASD" );
		//**//
		//  MonoBehaviour.print("DISABLE");
		if ( treeView == null ) return;
		
		var d = m_data.GetValue(treeView, null);
		
		if ( dsType == null ) dsType = Assembly.GetAssembly( typeof( EditorWindow ) ).GetType( "UnityEditor.IMGUI.Controls.TreeViewDataSource" );
		
		if ( m_Rows == null ) m_Rows = dsType.GetField( "m_Rows", (BindingFlags)(-1) );
		
		if ( InitIfNeeded == null ) InitIfNeeded = dsType.GetMethod( "InitIfNeeded", (BindingFlags)(-1) );
		
		m_Rows.SetValue( d, null );
		InitIfNeeded.Invoke( d, null );
		
		ClearTree();
		
	}
	internal void INIT_IF_NEDDED_SIMPLE()
	{	var treeView = m_TreeView(window());
	
		if ( treeView == null ) return;
		
		var d = m_data.GetValue(treeView, null);
		
		if ( dsType == null ) dsType = Assembly.GetAssembly( typeof( EditorWindow ) ).GetType( "UnityEditor.IMGUI.Controls.TreeViewDataSource" );
		
		if ( m_Rows == null ) m_Rows = dsType.GetField( "m_Rows", (BindingFlags)(-1) );
		
		if ( InitIfNeeded == null ) InitIfNeeded = dsType.GetMethod( "InitIfNeeded", (BindingFlags)(-1) );
		
		m_Rows.SetValue( d, null );
		InitIfNeeded.Invoke( d, null );
		// ClearTree();
		
	}
	
	Type wl;
	FieldInfo s_max;
	PropertyInfo activated;
	static bool? was_activate;
	
	internal void CHECK_MAXIMIZATION()
	{	if ( wl == null ) wl = typeof( EditorWindow ).Assembly.GetType( "UnityEditor.WindowLayout" );
	
		if ( s_max == null ) s_max = wl.GetField( "s_MaximizeKey", (BindingFlags)(-1) );
		
		if ( activated == null ) activated = s_max.FieldType.GetProperty( "activated", (BindingFlags)(-1) );
		
		var maxValue = s_max.GetValue(null);
		
		if ( maxValue == null ) return;
		
		Debug.Log( activated.GetValue( maxValue, null ) );
		
		if ( was_activate != (bool)activated.GetValue( maxValue, null ) )
		{	was_activate = (bool)activated.GetValue( s_max.GetValue( null ), null );
			//  INIT_IF_NEDDED();
			// Debug.Log( was_activate + " " + GUIUtility.hotControl );
			// Debug.Log("ASD");
		}
	}
	
	
	bool chk_app;
	internal void CHECK_APPLICATION()
	{	if ( chk_app ) return;
	
		chk_app = true;
		
		
		
		var WSA = typeof(MonoBehaviour).Assembly.GetType("UnityEngine.WSA.Application");
		var windowActivated = WSA.GetEvent("windowActivated", (BindingFlags)(-1));
		
		
		var method = this.GetType().GetMethod("METHOD", (BindingFlags)(-1));
		Debug.Log( method + " - " + windowActivated.EventHandlerType );
		Delegate handler = Delegate.CreateDelegate(windowActivated.EventHandlerType, this, method);
		// Assign the eventhandler. This corresponds with `control.Load += ...`.
		windowActivated.AddEventHandler( null, handler );
		
		
	}
	/*
	void METHOD(WindowActivationState state)
	{   Debug.Log( "ASD" );
	}*/
	
	
	internal void CHECK_HOSTVIEW()
	{	if ( chk_app ) return;
	
		chk_app = true;
		
		
		var HV = typeof(EditorWindow).Assembly.GetType("UnityEditor.HostView");
		var actualViewChanged = HV.GetEvent("actualViewChanged", (BindingFlags)(-1));
		
		var method = this.GetType().GetMethod("METHOD2", (BindingFlags)(-1));
		
		//  actualViewChanged.EventHandlerType
		//  DynamicMethod.CreateDelegate(,,);
		
		//   Debug.Log(method + " - " + windowActivated.EventHandlerType);
		Delegate handler = Delegate.CreateDelegate(actualViewChanged.EventHandlerType, this, method);
		// Assign the event handler. This corresponds with `control.Load += ...`.
		actualViewChanged.AddEventHandler( null, handler );
		
		
	}
	
	void METHOD2( object state )
	{	Debug.Log( "ASD" );
	}
	
	/*
	void Expample()
	{   var p = new Program();
	 var eventInfo = p.GetType().GetEvent("TestEvent");
	 //  var methodInfo = p.GetType().GetMethod("TestMethod");
	
	
	
	 / *DynamicMethod squareIt = new DynamicMethod(
	     "SquareIt",
	     null, //return
	     new[] { typeof(string) });
	
	
	
	 Delegate handler =
	     Delegate.CreateDelegate(eventInfo.EventHandlerType,
	                             p,
	                             squareIt);
	
	 eventInfo.AddEventHandler( p, handler );* /
	
	 p.Test();
	}*/
	class Program {
		public event Func<string> TestEvent;
		
		public object TestMethod()
		{	return "ASD";
		}
		
		public void Test()
		{	if ( TestEvent != null )
			{	Debug.Log( TestEvent() );
			}
		}
	}
}
}







