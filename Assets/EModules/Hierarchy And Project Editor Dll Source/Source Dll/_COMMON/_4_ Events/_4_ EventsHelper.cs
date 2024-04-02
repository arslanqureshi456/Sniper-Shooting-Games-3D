#define FIX_DRAG


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
	internal static bool OPT_EV_BR(Event ev)
	{	//if (ev == null ) return false;
	
		//if (ev.type == EventType.Layout || ev.type == EventType.MouseDrag || ev.type == EventType.MouseMove) return true;
		var t = ev.type;
		
		if (t == EventType.Layout || t == EventType.MouseDrag || t == EventType.MouseMove) return true;
		
		return false;
	}
	
	
	
	
	bool NeedUndoCheck = true;
	bool NeedMouseCheck = true;
	
	internal Color redTTexure = new Color( 0.6f, 0.3f, 0.1f, 1 );
	
	
	void CheckMouseEventUpdater()
	{	NeedUndoCheck = true;
		NeedMouseCheck = true;
	}
	
	
	void getChilds_Hierarchy( Transform t, ref List<Transform> result )
	{	result.Add( t );
	
		for ( int i = 0, l = t.transform.childCount ; i < l ; i++ )
			getChilds_Hierarchy( t.transform.GetChild( i ), ref result );
	}
	
	static SortedList<int, HierarchyObject> tl;
	void getChilds_Project( HierarchyObject t, ref List<HierarchyObject> result )
	{	result.Add( t );
	
		GetPathToChildrens( ref t.project.assetPath, out tl );
		var childs = tl.Values.ToArray();
		
		for ( int i = 0 ; i < childs.Length ; i++ )
			getChilds_Project( childs[i], ref result );
	}
	
	List<GameObject> oldObs = null;
	const string START = " ⅝⅝";
	const string END = "⅞⅞ ";
	bool needDuplicateCache = false;
	List<GameObject> needRestoreGameObjectNames = null;
	
	void Duplicate()
	{	if ( pluginID == Initializator.PROJECT_ID ) return;
	
		if ( needDuplicateCache && Event.current.commandName != "Duplicate" && Event.current.commandName != "Paste" )
		{	needDuplicateCache = false;
			SaveDataFromName( GetBroadCastSelection().Select( g => g.go ).ToArray() );
			
			if ( oldObs != null )
			{	RemoveDataFromName( oldObs );
				oldObs = null;
				/*   var newObjects = new List<Transform>();
				
				   var sel = Selection.gameObjects;
				   //  var sel = Selection.GetTransforms(SelectionMode.TopLevel).Select(t => t.gameObject).ToArray();
				
				   foreach (var gameObject in top)
				   {
				       var tempList = new List<Transform>();
				       getChilds(gameObject.transform, ref tempList);
				       newObjects.AddRange(tempList);
				   }
				
				   if (newObjects.Count == oldObs.Length)
				   {
				       for (int i = 0; i < oldObs.Length; i++)
				       {
				           if (!oldObs[i]) continue;
				           newObjects[i].name += " + " + oldObs[i].name;
				       }
				   }
				
				   needRestoreGameObjectNames = oldObs.ToList();*/
			}
			
			
			//MonoBehaviour.print("needBack" + " " + Selection.activeGameObject.name + " " + Event.current.type);
		}
		
		if ( needRestoreGameObjectNames != null && Event.current.commandName != "Duplicate" && Event.current.commandName != "Paste" && Event.current.commandName != "Copy" )
		{	RemoveDataFromName( needRestoreGameObjectNames ); //REMOVE
			needRestoreGameObjectNames = null;
		}
		
		if ( Event.current.type == EventType.ExecuteCommand && Event.current.commandName == "Paste" )
		{	needDuplicateCache = true;
			// MonoBehaviour.print(Selection.activeGameObject.name);
		}
		
		if ( Event.current.type == EventType.ExecuteCommand && Event.current.commandName == "Copy" )
		{	needRestoreGameObjectNames = AddDataToName( GetBroadCastSelection().Select( g => g.go ).ToArray(), false );  //ADD
		}
		
		//MonoBehaviour.print(GUIUtility.systemCopyBuffer);
		
		if ( Event.current.type == EventType.ExecuteCommand && Event.current.commandName == "Duplicate" )
		{
		
			oldObs = AddDataToName( GetBroadCastSelection().Select( g => g.go ).ToArray(), true );  //ADD
			//  MonoBehaviour.print(Event.current.commandName + " " + Selection.activeGameObject.name + " " + Event.current.type);
			needDuplicateCache = true;
		}
	}
	
	internal void SaveDataFromName( GameObject[] transs )
	{	foreach ( var needRestoreGameObjectName in transs )
		{	if ( !needRestoreGameObjectName ) continue;
		
			{
			
			
				//  var s = needRestoreGameObjectName.name.IndexOf(START, StringComparison.Ordinal);
				// var e = needRestoreGameObjectName.name.IndexOf(END, StringComparison.Ordinal);
				var mn = needRestoreGameObjectName.GetComponent(DF);
				
				//   if (s != -1 && e != -1 && e > s)
				if ( mn )
				{
				
					var dh = mn as IDescriptionFlush;
					
					if ( !string.IsNullOrEmpty( dh.cachedData ) )
					{
					
						var o = GetHierarchyObjectByInstanceID(needRestoreGameObjectName.GetInstanceID());
						
						// var data = needRestoreGameObjectName.name.Substring(s + START.Length, e - s - START.Length);
						var data = dh.cachedData;
						
						// //////////// FIND_ALL_CACHE /////////////
						foreach ( var s1 in data.Split( '⅜' ) )
						{	if ( s1.Length < 4 ) continue;
						
							if ( s1.StartsWith( "[D]" ) )
							{	var res = s1.Substring(3);
								DescriptionModule.SetValue( res, needRestoreGameObjectName.scene.GetHashCode(), o );
								
							}
							
							else
								if ( s1.StartsWith( "[C]" ) )
								{	var res = s1.Substring(3).Split(' ');
									var c = Adapter.String4ToColor(res);
									
									if ( c != null && (c.HAS_BG_COLOR || c.HAS_LABEL_COLOR) )     //var b = Adapter.StringToBool(4, res);
									{	//Color32 c = new Color32(result[0], result[1], result[2], result[3]);
										ColorModule.SetHighlighterValue( c, needRestoreGameObjectName.scene.GetHashCode(), o );
									}
								}
								
								else
									if ( s1.StartsWith( "[Q]" ) )
									{
									
										var res = s1.Substring(3).Split(' ');
										var c = Adapter.String4ToList(res);
										
										if ( c != null )     //Color32 c = new Color32(result[0], result[1], result[2], result[3]);
										{	ColorModule.IconColorCacher.SetValue( new SingleList() { list = c }, needRestoreGameObjectName.scene.GetHashCode(), SetPair( o ), true );
										}
									}
									
									else
										if ( s1.StartsWith( "[K]" ) )
										{
										
#if HIERARCHY || UNITY_EDITOR
										
											if ( IS_HIERARCHY() )
											{	var res = s1.Substring(3);
												uint resultKeeper;
												
												if ( uint.TryParse( res, out resultKeeper ) )
												{	if ( resultKeeper == uint.MaxValue )
													{	Hierarchy.M_PlayModeKeeper.DataKeeperCache.SetValue( new SingleList() { list = new[] { 1 } .ToList() }, o.go.scene.GetHashCode(), o.go, true );
													}
													
													else
													{	var comps =  HierarchyExtensions.Utilities.GetComponentFast<Component>.GetAll( o.go ).Select(c => c.GetInstanceID()).ToList();
														List<int> tempList = new[] { 0 } .ToList();
														
														for ( int i = 0 ; i < 64 ; i++ )
														{	if ( i >= comps.Count ) break;
														
															if ( (resultKeeper & ((uint)1 << i)) != 0 )
															{	tempList.Add( comps[i] );
															}
														}
														
														Hierarchy.M_PlayModeKeeper.DataKeeperCache.SetValue( new SingleList() { list = tempList }, o.go.scene.GetHashCode(), o.go, true );
													}
												}
											}
											
											//
#endif
											
										}
										
										
						}
						
						{	if ( Application.isPlaying ) GameObject.Destroy( mn );
							else GameObject.DestroyImmediate( mn, false );
						}
						
						/* var newName = needRestoreGameObjectName.name.Remove(s, e - s + END.Length - 1);
						 newName = GameObjectUtility.GetUniqueNameForSibling( needRestoreGameObjectName.transform.parent, newName );
						 needRestoreGameObjectName.name = newName;*/
					}
				}
			}
		}
	}
	
	internal List<GameObject> AddDataToName( GameObject[] transs, bool writeUndo )
	{	var result = new List<GameObject>();
	
		if ( DISABLE_DES() ) return result;
		
		foreach ( var needRestoreGameObjectName in transs )
		{	if ( !needRestoreGameObjectName ) continue;
		
			if ( !needRestoreGameObjectName.scene.IsValid() || !needRestoreGameObjectName.scene.isLoaded ) continue;
			
			var buildString = "";
			
			var o = GetHierarchyObjectByInstanceID(needRestoreGameObjectName.GetInstanceID());
			
			
			var des = DescriptionModule.GetValue(needRestoreGameObjectName.scene.GetHashCode(), o);
			
			if ( !string.IsNullOrEmpty( des ) ) buildString += "[D]" + des;
			
			var col = ColorModule.GetValueToString(needRestoreGameObjectName.scene.GetHashCode(), o);
			
			if ( !string.IsNullOrEmpty( col ) && !string.IsNullOrEmpty( buildString ) ) buildString += "⅜";
			
			if ( !string.IsNullOrEmpty( col ) ) buildString += "[C]" + col;
			
			col = ColorModule.IconColorCacher.GetValueToString( needRestoreGameObjectName.scene.GetHashCode(), o );
			
			if ( !string.IsNullOrEmpty( col ) && !string.IsNullOrEmpty( buildString ) ) buildString += "⅜";
			
			if ( !string.IsNullOrEmpty( col ) ) buildString += "[Q]" + col;
			
#if HIERARCHY || UNITY_EDITOR
			
			if ( IS_HIERARCHY() )
			{	if ( Hierarchy.M_PlayModeKeeper.DataKeeperCache.HasKey( o.scene, o ) )
				{
				
					var v = Hierarchy.M_PlayModeKeeper.DataKeeperCache.GetValue(o.go.scene, o);
					
					if ( v.list != null && v.list.Count > 0 )
					{	uint resultKeeper = 0;
					
						if ( v.list[0] == 1 )
						{	resultKeeper = uint.MaxValue;
						}
						
						else
						{
						
						
							var comps = HierarchyExtensions.Utilities.GetComponentFast<Component>.GetAll( o.go ).Select(c => c.GetInstanceID()).ToList();
							
							for ( int i = 1 ; i < v.list.Count ; i++ )
							{	var ind = comps.IndexOf(v.list[i]);
							
								if ( ind < 0 ) continue;
								
								if ( ind >= 64 ) continue;
								
								resultKeeper |= ((uint)1) << ind;
							}
						}
						
						if ( resultKeeper != 0 )
						{	if ( !string.IsNullOrEmpty( buildString ) ) buildString += "⅜";
						
							buildString += "[K]" + resultKeeper;
						}
					}
				}
			}
			
			//
#endif
			
			
			if ( !string.IsNullOrEmpty( buildString ) )
			{
			
				//if (writeUndo && !Application.isPlaying) Undo.RecordObject( needRestoreGameObjectName, "Duplicate" );
				
				//needRestoreGameObjectName.name += START + buildString + END;
				var dh =   needRestoreGameObjectName.AddComponent(DF) as IDescriptionFlush;
				
				if ( writeUndo && !Application.isPlaying ) Undo.RegisterCreatedObjectUndo( dh as MonoBehaviour, "Duplicate" );
				
				dh.cachedData = buildString;
				EditorUtility.SetDirty( dh as MonoBehaviour );
				// EditorUtility.SetDirty(needRestoreGameObjectName);
				result.Add( needRestoreGameObjectName );
			}
		}
		
		return result;
	}
	
	internal void RemoveDataFromName( List<GameObject> transs )
	{
	
		foreach ( var needRestoreGameObjectName in transs )
		{	if ( needRestoreGameObjectName )
			{	/*var s = needRestoreGameObjectName.name.IndexOf(START, StringComparison.Ordinal);
				    var e = needRestoreGameObjectName.name.IndexOf(END, StringComparison.Ordinal);
				    if (s != -1 && e != -1 && e > s)
				    {   needRestoreGameObjectName.name = needRestoreGameObjectName.name.Remove( s, e - s + END.Length );
				    }*/
				
				var dh = needRestoreGameObjectName.GetComponent( DF);
				
				if ( dh )
				{	if ( Application.isPlaying ) GameObject.Destroy( dh );
					else GameObject.DestroyImmediate( dh, false );
				}
			}
		}
	}
	
	
	List<HierarchyObject> GetBroadCastSelection()     //Selection.objects = Selection.gameObjects.OrderBy(g => g.transform.GetSiblingIndex()).ToArray();
	{	var result = new List<HierarchyObject>();
		var sel = SELECTED_GAMEOBJECTS();
		// var sel = Selection.GetTransforms(SelectionMode.TopLevel).Select(t => t.gameObject).ToArray();
		// var top = sel.Where(g => g.GetComponentsInParent<Transform>(true).Count(p => sel.Contains(p.gameObject)) == 1).Select(g => g.gameObject).ToArray();
		var top = Utilities.GetOnlyTopObjects(sel, this);
		
		
		if ( pluginID == Initializator.HIERARCHY_ID )
		{	foreach ( var gameObject in top )
			{	var tempList = new List<Transform>();
				getChilds_Hierarchy( gameObject.go.transform, ref tempList );
				result.AddRange( tempList.Select( t => GetHierarchyObjectByInstanceID( t.gameObject ) ) );
			}
		}
		
		if ( pluginID == Initializator.PROJECT_ID )
		{	foreach ( var gameObject in top )
			{	var tempList = new List<HierarchyObject>();
				getChilds_Project( gameObject, ref tempList );
				result.AddRange( tempList );
			}
		}
		
		return result;
	}
	
	internal object currentTree, currentState;
	internal MethodInfo m_HasFocus;
	PropertyInfo dragging;//, m_SelectedIDs;
	FieldInfo m_DragSelection;//, m_SelectedIDs;
	FieldInfo m_SelectedIDs;
	internal IList<int> current_DragSelection_List = new List<int>();
	internal IList<int> current_selectedIDs = new List<int>();
	internal bool currentFocus = false;
	Dictionary<int, Action> controlIDsAndOnMouseUp = new Dictionary<int, Action>();
	double? lastEditorTime;
#pragma warning disable
	float?[] aasdasd = new float?[100];
	bool HeightFixIfDrag_Flag = false;
	bool blockHF = false;
	internal void HeightFixIfDrag()
	{	//if ( firstFrame < 4 ) return;
		if ( !HeightFixIfDrag_Flag || blockHF ) return;
		
		HeightFixIfDrag_Flag = false;
		//  Debug. Log( "SET" );
		ClearTree(true);
		lastmPos = lastm_TotalRect = 0;
		/* for ( int i = 0 ; i < modules.Length ; i++ )
		 {   modules[i].ClearDrawStack();
		 }*/
		ColorModule.ResetStack();
		
		return;
		
		if ( !window() ) return;
		
		
		//  if ( Event.current.type == EventType.Repaint ) return;
		//  Debug.Log( Event.current.type );
		// return;
		lastmPos = lastm_TotalRect = 0;
		var w = window();
		// lastm_VisibleRect = 0;
		//lastInst = null;
		//NEW_RELOAD = true;
		HEIGHT_RIX_FUNCTIUON( window(), m_TreeView( w ), true );
		//  Debug.Log("ASD");
		RepaintWindowInUpdate();
		ClearTree();
	}
#pragma warning restore
	
	internal void InternalClearSelection( int[] ids )
	{	InternalClearDrag();
	
		if ( !window() ) return;
		
		var currentTree  = m_TreeView(window());
		
		if ( currentTree == null ) return;
		
		currentState = m_state.GetValue( currentTree, null );
		
		if ( currentState == null ) return;
		
		if ( m_SelectedIDs == null )
		{	m_SelectedIDs = currentState.GetType().GetField( "m_SelectedIDs", (BindingFlags)(-1) );
		
			if ( m_SelectedIDs == null )
				m_SelectedIDs = currentState.GetType().BaseType.GetField( "m_SelectedIDs", (BindingFlags)(-1) );
		}
		
		if ( m_SelectedIDs != null )
		{
		
			var asd = m_SelectedIDs.GetValue( currentState ) as IList<int>;
			
			if ( asd != null ) asd.Clear();
			else asd = new List<int>();
			
			foreach ( var id in ids )
			{	asd.Add( id );
			}
			
			m_SelectedIDs.SetValue( currentState, asd );
			
			
			if ( dragging == null )
			{	dragging = currentTree.GetType().GetProperty( "dragging", (BindingFlags)(-1) );
			}
			
			var dr = dragging.GetValue(currentTree, null);
			
			if ( dr != null )
			{	var m  = dragging.PropertyType.GetMethod( "DragCleanup", (BindingFlags)(-1) );
			
				if ( m != null )
				{	m.Invoke( dr, new object[] { true } );
				}
			}
		}
	}
	internal void InternalClearDrag()
	{	ClearTree();
	
		if ( !window() ) return;
		
		var currentTree  = m_TreeView(window());
		
		if ( m_DragSelection == null )
		{	if ( currentTree == null ) return;
		
			m_DragSelection = currentTree.GetType().GetField( "m_DragSelection", (BindingFlags)(-1) );
		}
		
		
		// PushAction(() =>
		if ( m_DragSelection != null )
		{	if ( currentTree == null ) return;
		
			var current_DragSelection_List = m_DragSelection.GetValue( currentTree ) as IList<int>;
			
			if ( current_DragSelection_List != null ) current_DragSelection_List.Clear();
			
			m_DragSelection.SetValue( currentTree, current_DragSelection_List );
			
			
			if ( dragging == null )
			{	dragging = currentTree.GetType().GetProperty( "dragging", (BindingFlags)(-1) );
			}
			
			var dr = dragging.GetValue(currentTree, null);
			
			if ( dr != null )
			{	/*var m  = dragging.PropertyType.GetMethod( "DragElement", (BindingFlags)(-1) );
				    if (m != null)
				    {   m.Invoke( dr, new object[] {(UnityEditor.IMGUI.Controls.TreeViewItem) null, new Rect(), -1 });
				        Debug.Log("ASD");
				    }*/
				
				var m  = dragging.PropertyType.GetMethod( "DragCleanup", (BindingFlags)(-1) );
				
				if ( m != null )
				{	m.Invoke( dr, new object[] { true } );
				}
			}
			
			
			
		}
		
		//);
		
		// DragAndDrop.PrepareStartDrag();// reset data
		
		/* if (m_SelectedIDs == null)
		 {   m_SelectedIDs = currentState.GetType().GetField( "m_SelectedIDs", (BindingFlags)(-1) );
		     if (m_SelectedIDs == null)
		         m_SelectedIDs = currentState.GetType().BaseType.GetField( "m_SelectedIDs", (BindingFlags)(-1) );
		 }
		 var current_selectedIDs = m_SelectedIDs.GetValue( currentState ) as IList<int>;
		 if (current_selectedIDs != null) current_selectedIDs.Clear();*/
		
		
		
	}
	
	
	
	int oldDragValue = 0;
	
	void ChechEvents( Rect selectionRect )
	{
	
	
	
		if ( DEFAUL_SKIN == null ) DEFAUL_SKIN = Adapter.GET_SKIN();
		
		
		Duplicate();
		
		
		
		DescriptionModule.UpdateSwitchRegistratorEnable();
		
		
		
		
		// MonoBehaviour.print("ASD");
		if ( NeedUndoCheck )
		{	NeedUndoCheck = false;
		
			if ( Event.current.type == EventType.ValidateCommand )
			{	switch ( Event.current.commandName )  /**/
				{	case "UndoRedoPerformed":
						CurrentRectClear();
						RepaintWindow(true);
						
						break;
				}
				
			}
		}
		
		//  if (Event.current.keyCode == KeyCode.LeftShift || Event.current.keyCode == KeyCode.RightShift) RepaintWindow();
		// if (Event.current.keyCode == KeyCode.LeftControl || Event.current.keyCode == KeyCode.RightControl) RepaintWindow();
		
		if ( Event.current.isKey && Event.current.type == EventType.KeyDown )
		{	customMenuModules.CheckKeyDown( Event.current.control, Event.current.shift, Event.current.alt, Event.current.keyCode );
		}
		
		// if (window() == null) return;
		
		
		/*  if (Event.current.keyCode == KeyCode.Escape)
		  {
		      if (mouseEvent != null || mouseEventDrag != null)
		      {
		          mouseEventDrag = null;
		          mouseEvent = null;
		         // Undo.PerformUndo();
		      }
		
		  }*/
		
		if ( m_HasFocus == null )
			currentTree = m_TreeView( window() );
			
		if ( m_HasFocus == null ) m_HasFocus = currentTree.GetType().GetMethod( "HasFocus", (BindingFlags)(-1) );
		
		currentFocus = (bool)m_HasFocus.Invoke( currentTree, null );
		
		if ( m_DragSelection == null )
		{	m_DragSelection = currentTree.GetType().GetField( "m_DragSelection", (BindingFlags)(-1) );
		}
		
		currentState = m_state.GetValue( currentTree, null );
		
		// Debug.Log( currentState.GetType().BaseType );
		if ( m_SelectedIDs == null )
		{	m_SelectedIDs = currentState.GetType().GetField( "m_SelectedIDs", (BindingFlags)(-1) );
		
			if ( m_SelectedIDs == null )
				m_SelectedIDs = currentState.GetType().BaseType.GetField( "m_SelectedIDs", (BindingFlags)(-1) );
		}
		
		//  Debug.Log( m_SelectedIDs == null );
		current_DragSelection_List = m_DragSelection.GetValue( currentTree ) as IList<int>;
		current_selectedIDs = m_SelectedIDs.GetValue( currentState ) as IList<int>;
		
		
#if FIX_DRAG
		
		if ( Event.current.type == EventType.DragPerform )
		{	/* lastmPos = lastm_TotalRect =   0;
			     HEIGHT_FUNCTIUON( window(), m_TreeView(window()), true);
			     //  Debug.Log("ASD");
			     RepaintWindowInUpdate();*/
			HeightFixIfDrag();
		}
		
		if ( !CACHING_TEXTURES_STACKS && oldDragValue != current_DragSelection_List.Count )
		{	oldDragValue = current_DragSelection_List.Count;
		
			//  Debug.Log( oldDragValue );
			if ( oldDragValue == 0 )     // RESET_SMOOTH_HEIGHT();
			{	HeightFixIfDrag();
				//   Reload
			}
		}
		
#endif
		
		
		
		// if ( Event.current.type == EventType.Repaint ) return;
		
		CalcHeaderRects( selectionRect );
		
		
		if ( lastEditorTime == null ) lastEditorTime = EditorApplication.timeSinceStartup;
		
		var deltaTime = (float)(lastEditorTime - EditorApplication.timeSinceStartup);
		bool repaint = false;
		
		
		if ( bottomInterface.hyperGraph.HierHyperController.currentAction != null )
		{	if ( !bottomInterface.hyperGraph.HierHyperController.currentAction( false, deltaTime ) )
			{	//repaint = true;
				bottomInterface.hyperGraph.Repaint();
			}
		}
		
		if ( bottomInterface.favorGraph.HierFavorController.currentAction != null )
		{	if ( !bottomInterface.favorGraph.HierFavorController.currentAction( false, deltaTime ) )
			{	//repaint = true;
				bottomInterface.favorGraph.Repaint();
			}
		}
		
		
		// MonoBehaviour.print(Event.current.type);
		
		
		
		if ( NeedMouseCheck && (Event.current.type != EventType.Repaint && Event.current.type != EventType.Layout) )
		{
		
			NeedMouseCheck = false;
			
			if ( mouseEvent != null )
			{	mouseEvent( false, deltaTime );
				repaint = true;
			}
			
			
			if ( mouseEventDrag != null )
			{	mouseEventDrag( false, deltaTime );
				repaint = true;
			}
			
			
			//BottomInterface.EVENTS_UPDATE();
			
			if ( bottomInterface.HierarchyController.selection_action != null )
			{	if ( !bottomInterface.HierarchyController.selection_action( false, deltaTime ) )
					repaint = true;
			}
			
			
			
			if ( controlIDsAndOnMouseUp.Count != 0 )
			{	repaint = true;
			}
			
			
			/* if (mouseEvent != null) {
			     mouseEvent(Event.current.rawType == EventType.MouseUp);
			     RepaintWindow();
			     if (Event.current.rawType == EventType.MouseUp) {
			         mouseEvent = null;
			         //  RepaintWindow();
			     }
			 }
			
			
			 if (mouseEventDrag != null) {
			     mouseEventDrag(Event.current.rawType == EventType.MouseUp);
			     RepaintWindow();
			     if (Event.current.rawType == EventType.MouseUp) {
			         mouseEventDrag = null;
			         //  RepaintWindow();
			     }
			 }
			
			
			
			
			
			 if (Bottom_Interface.selection_action != null) {
			     Bottom_Interface.selection_action(Event.current.rawType == EventType.MouseUp);
			     RepaintWindow();
			
			     if (Event.current.rawType == EventType.MouseUp) {
			         Bottom_Interface.selection_action = null;
			         Bottom_Interface.selection_button = null;
			         Bottom_Interface.selection_window = null;
			         // RepaintWindow();
			     }
			 }*/
		}
		
		/* if (Event.current.keyCode == KeyCode.Escape)
		 {
		   bottomInterface.ClearAction();
		   EventUse();
		   repaint = true;
		   DragAndDrop.activeControlID = 0;
		 }*/
		
		
		if ( Event.current.rawType == EventType.MouseUp /*|| Event.current.keyCode == KeyCode.Escape*/)
		{	EVENT_HELPER_ONUP();
		
		}
		
		
		if ( repaint )
		{
		
			RepaintWindowInUpdate();
		}
		
		
		
	}
	
	
	void PUSH_EVENT_HELPER_RAW()
	{	PUSH_ONMOUSEUP( EVENT_HELPER_ONUP, window() );
	}
	private Action<bool, float> __mouseEvent;
	private Action<bool, float> mouseEvent { get { return __mouseEvent; } set { __mouseEvent = value; if (value != null) PUSH_ONMOUSEUP( EVENT_HELPER_ONUP ); } }
	private Action<bool, float> __mouseEventDrag;
	private Action<bool, float> mouseEventDrag { get { return __mouseEventDrag; } set { __mouseEventDrag = value; if ( value != null ) PUSH_ONMOUSEUP( EVENT_HELPER_ONUP ); } }
	void EVENT_HELPER_ONUP()
	{	_6__BottomWindow_HyperGraphWindow.CHECK_MOUSE_UP( this );
	
	
		var repaint  = false;
		//Debug.Log( "ASD" );
		
		if ( mouseEvent != null )
		{	mouseEvent( true, deltaTime );
			mouseEvent = null;
			
			GUIUtility.hotControl = 0;
			EventUse();
			repaint = true;
		}
		
		if ( mouseEventDrag != null )
		{	mouseEventDrag( true, deltaTime );
			mouseEventDrag = null;
			
			GUIUtility.hotControl = 0;
			EventUse();
			repaint = true;
		}
		
		
		if ( bottomInterface.HierarchyController.selection_action != null )     //
		{	bottomInterface.HierarchyController.selection_action( true, deltaTime );
			bottomInterface.ClearAction();
			//MonoBehaviour.print( "ASD" );
			GUIUtility.hotControl = 0;
			EventUse();
			repaint = true;
			
			// RepaintWindow();
		}
		
		if ( bottomInterface.hyperGraph.HierHyperController.currentAction != null )     //
		{	bottomInterface.hyperGraph.HierHyperController.currentAction( true, deltaTime );
			bottomInterface.ClearAction();
			//MonoBehaviour.print( "ASD" );
			GUIUtility.hotControl = 0;
			EventUse();
			repaint = true;
			bottomInterface.hyperGraph.Repaint();
			
			// RepaintWindow();
		}
		
		if ( bottomInterface.favorGraph.HierFavorController.currentAction != null )     //
		{	bottomInterface.favorGraph.HierFavorController.currentAction( true, deltaTime );
			bottomInterface.ClearAction();
			// MonoBehaviour.print( "ASD" );
			GUIUtility.hotControl = 0;
			EventUse();
			repaint = true;
			
			bottomInterface.favorGraph.Repaint();
		}
		
		
		if ( controlIDsAndOnMouseUp.Count != 0 )
		{	foreach ( var controlID in controlIDsAndOnMouseUp.Values.ToArray() )
			{	controlID();
			}
			
			controlIDsAndOnMouseUp.Clear();
			repaint = true;
		}
		
		
		if ( repaint )
		{
		
			RepaintWindowInUpdate();
		}
		
		
	}
}
}
