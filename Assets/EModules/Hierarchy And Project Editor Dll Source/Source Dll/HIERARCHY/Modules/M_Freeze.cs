﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

//namespace EModules

namespace EModules.EModulesInternal

{
internal partial class Hierarchy {
	internal class M_Freeze : Adapter.Module, IModuleOnnector_M_Freeze {
		/*  MyStruct dasd;
		  struct MyStruct
		  {
		      internal bool? asd;
		  }*/
		internal override bool SKIP()
		{	return false;
		}
		
		bool? lastUpdateState  = null;
		void UpdateSceneFlags( bool value )
		{	if ( lastUpdateState == value ) return;
		
			lastUpdateState = value;
			adapter.OneFrameActionOnUpdate = true;
			adapter.OneFrameActionOnUpdateAC += () =>
			{
			
				for ( int asd = 0 ; asd < SceneManager.sceneCount ; asd++ )
				{	var s = SceneManager.GetSceneAt(asd);
				
					if ( !s.isLoaded || !s.IsValid() ) continue;
					
					
					var list = getDoubleList(s.GetHashCode());
					
					for ( int i = 0 ; i < list.Count ; i++ )
					{	if ( !list[i].Value ) continue;
					
						var t = adapter.GetHierarchyObjectByPair( ref list.listKeys, i);
						
						if ( !t.Validate() )
						{	list.RemoveAt( i );
							i--;
							continue;
						}
						
						if ( value )
							t.go.hideFlags |= HideFlags.NotEditable;
						else
							t.go.hideFlags &= ~HideFlags.NotEditable;
					}
				}
			};
			
			
			
		}
		protected override void OnEnableChange( bool value )
		{	UpdateSceneFlags( value );
			base.OnEnableChange( value );
		}
		
		DoubleList<GoGuidPair, bool> dl = new DoubleList<GoGuidPair, bool>();
		public DoubleList<GoGuidPair, bool> getDoubleList( int s )
		{	var d = adapter.MOI.des(s);
			dl.listKeys = d.GetFreezeHashKeys();
			dl.listValues = d.GetFreezeHashValues();
			return dl;
		}
		void setDoubleList( int s )
		{	var dl = getDoubleList(s);
			var d = adapter.MOI.des(s);
			d.SetFreezeHashKeys( dl.listKeys );
			d.SetFreezeHashValues( dl.listValues );
		}
		// Dictionary<int, bool> wasFreezeInit = new Dictionary<int, bool>();
		
		
		public M_Freeze( int restWidth, int sib, bool enable, Adapter adapter ) : base( restWidth, sib, enable, adapter )
		{
		
			UpdateSceneFlags( enable );
			/*  List<int> compare = new List<int>();
			
			UpdateSceneFlags(
			foreach (var gameObject in Utilities.AllSceneObjectsInterator())
			{
			  compare.Add(gameObject.GetInstanceID());
			}
			List<int> compare2 = Utilities.AllSceneObjects(EditorSceneManager.GetActiveScene()).Select(g => g.GetInstanceID()).ToList();
			compare2.Reverse();
			MonoBehaviour.print(compare2.Count + " " +  compare.Count);
			if (compare2.Count != compare.Count) Debug.LogError("ASD");
			for (int i = 0; i < compare2.Count; i++)
			{
			  if (compare[i] != compare2[i]) Debug.LogError("ASD11");
			}*/
			/* foreach (var gameObject in Utilities.AllSceneObjectsInterator())
			                         MonoBehaviour.print(gameObject.name);*/
		}
		// internal static Color colCache;
		internal static Color target = new Color( 0.2f, 0.2f, 0.2f, 1 );
		GUIContent content = new GUIContent();
		
		void OnRawUp()
		{	if ( stateForDrag != null ) adapter.RepaintAllViews();
		
			stateForDrag = null;
		}
		
		internal override float Draw( Rect drawRect, Adapter.HierarchyObject _o )
		{
		
		
		
			if ( !START_DRAW( drawRect, _o ) ) return 0;
			
			var o = _o.go;
			
			
			/* if ( !wasFreezeInit.ContainsKey( _o.scene ) ) {
			     wasFreezeInit.Add( _o.scene , true );
			
			 }*/
			UpdateSceneFlags( true );
			
			if ( o.activeInHierarchy || (o.hideFlags & HideFlags.NotEditable) != 0 )
			{
			
				var oldRect = drawRect;
				
				//  Adapter.GET_SKIN().button.fontSize = 4;
				var oldW = drawRect.width;
				var oldH = drawRect.height;
				drawRect.width = drawRect.height = 12;
				drawRect.x += (oldW - drawRect.width) / 2;
				drawRect.y += (oldH - drawRect.height) / 2;
				//  MonoBehaviour.print(drawRect);
				/* colorText11.SetPixel(0, 0, (o.hideFlags & HideFlags.NotEditable) != 0 ? Color.blue : Color.gray);
				 colorText11.Apply();*/
				
				var c = Color.white;
				
				if ( !EditorGUIUtility.isProSkin )
				{	//colCache = GUI.color;
					c *= target;
				}
				
				//Adapter.DrawTexture( drawRect, (o.hideFlags & HideFlags.NotEditable) != 0 ? adapter.GetIcon( "LOCK" ) : adapter.GetIcon( "UNLOCK" ) );
				Draw_AdapterTexture( drawRect, (o.hideFlags & HideFlags.NotEditable) != 0 ? adapter.GetIcon( "LOCK" ) : adapter.GetIcon( "UNLOCK" ), c, true );
				
				if ( !EditorGUIUtility.isProSkin )
				{	//GUI.color = colCache;
				}
				
				//  content.tooltip = objectIsHiddenAndLock ? "Object hided" : "Left CLICK/Left DRAG Show/Hide GameObject \n( Right CLICK/Right DRAG - Focus on the object in the SceneView )";
				if ( oldRect.Contains( Event.current.mousePosition ) )
					content.tooltip = (o.hideFlags & HideFlags.NotEditable) != 0 ? "This Object Locked" : "This Object Unlocked";
					
				/*  if ( !o.activeInHierarchy )
				  {   // var defColor = GUI.color;
				      c = Adapter.EditorBGColor;
				      c.a = 0.7f;
				      // GUI.color *= c;
				
				      Draw_AdapterTexture( drawRect, c, USE_GO: true );
				   //   GUI.DrawTexture( drawRect, EditorGUIUtility.whiteTexture );
				      // GUI.color = defColor;
				  }*/
				
				
				
				drawRect.y -= 2;
				
				
				
				Draw_Action( oldRect, SET_ACTIVE_ACTION_HASH, null );
				
				
				Draw_ModuleButton( oldRect, content, BUTTON_ACTION_HASH, true, useContentForButton: true);
				//   oldRect.y += 2;
				
				
				//  if ( adapter.ModuleButton( oldRect, content, true ) )
				{	/*if (Event.current.button == 0)
					{
					
					
					    // Undo.RecordObject(o, "GameObject Lock");
					    var old = o.hideFlags & HideFlags.NotEditable;
					    if (old != 0) o.hideFlags &= ~old;
					    else o.hideFlags |= HideFlags.NotEditable;
					    // Hierarchy.SetDirty(o);
					    foreach (var VARIABLE in o.GetComponentsInChildren<Transform>(true))
					    {
					        //Und
					        //  Undo.RecordObject(VARIABLE.gameObject, "GameObject Lock");
					        VARIABLE.gameObject.hideFlags = o.hideFlags & HideFlags.NotEditable | VARIABLE.gameObject.hideFlags & ~HideFlags.NotEditable;
					        //Hierarchy.SetDirty(VARIABLE.gameObject);
					    }
					    Hierarchy.RepaintAllView();
					}*/
					
					
					// EditorGUIUtility.ExitGUI();
				}
			}
			
			
			END_DRAW( _o );
			return width;
		}
		static HideFlags? stateForDrag = null;
		
		bool Validate( Adapter.HierarchyObject o )
		{	return o.go && ((o.go.hideFlags & HideFlags.NotEditable) != 0);
		}
		
		internal void SetLockToggle( int scene )
		{	setDoubleList( scene );
		
			if ( !Application.isPlaying )
			{	adapter.SetDirtyDescription( adapter.MOI.des( scene ), scene );
				adapter.MarkSceneDirty( scene );
			}
		}
		
		
		/* FillterData.Init(Event.current.mousePosition, SearchHelper, LayerMask.LayerToName(o.layer),
		             Validate(o) ?
		             CallHeaderFiltered(LayerMask.LayerToName(o.layer)) :
		             CallHeader(),
		             this);*/
		/** CALL HEADER */
		internal override _W__SearchWindow.FillterData_Inputs CallHeader()
		{	var result = new _W__SearchWindow.FillterData_Inputs( callFromExternal_objects )
			{	Valudator = Validate,
				    SelectCompareString = ( d, i ) => i.ToString(),
				    SelectCompareCostInt = ( d, i ) =>
				{	var cost = i;
					cost += d.go.activeInHierarchy ? 0 : 100000000;
					return cost;
				}
			};
			return result;
		}
		
		/* internal FillterData.FillterData_Inputs CallHeaderFiltered(string filter)
		 {
		     var result = CallHeader();
		     result.Valudator = s => Validate(s) && LayerMask.LayerToName(s.layer) == filter;
		     return result;
		 }*/
		/** CALL HEADER */
		
		
		
		
		/* internal override bool CallHeader(out GameObject[] obs, out int[] contentCost)
		 {
		     obs = Utilities.AllSceneObjects().Where(Validate).ToArray();
		     contentCost = obs.Select(o => o.activeInHierarchy ? 0 : 1).ToArray();
		     return true;
		 }*/
		
		
		
		
		
		
		
		
		
		
		
		Adapter. DrawStackMethodsWrapper __BUTTON_ACTION_HASH = null;
		Adapter.DrawStackMethodsWrapper BUTTON_ACTION_HASH { get { return __BUTTON_ACTION_HASH ?? (__BUTTON_ACTION_HASH = new Adapter.DrawStackMethodsWrapper( BUTTON_ACTION )); } }
		void BUTTON_ACTION( Rect worldOffset, Rect inputRect, Adapter.DrawStackMethodsWrapperData data, Adapter.HierarchyObject _o )
		{	if ( Event.current.button == 1 )
			{	Adapter.EventUse();
				/*   int[] contentCost = new int[0];
				   GameObject[] obs = new GameObject[0];
				
				   if (Validate(o))
				   {
				       if (EditorSceneManager.GetActiveScene().rootCount != 0) CallHeader(out obs, out contentCost);
				
				       FillterData.Init(Event.current.mousePosition, SearchHelper, "All", obs, contentCost, null, this);
				   } else
				   {
				       if (EditorSceneManager.GetActiveScene().rootCount != 0) CallHeader(out obs, out contentCost);
				
				       FillterData.Init(Event.current.mousePosition, SearchHelper, "All", obs, contentCost, null, this);
				   }*/
				
				var mp = new MousePos( Event.current.mousePosition, MousePos.Type.Search_356_0, !callFromExternal(), adapter);
				_W__SearchWindow.Init( mp, SearchHelper, "All", CallHeader(), this, adapter, _o );
				// EditorGUIUtility.ic
			}
		}
		
		
		
		
		
		
		Adapter. DrawStackMethodsWrapper __SET_ACTIVE_ACTION_HASH = null;
		Adapter.DrawStackMethodsWrapper SET_ACTIVE_ACTION_HASH
		{	get
			{	if ( __SET_ACTIVE_ACTION_HASH == null )
				{	__SET_ACTIVE_ACTION_HASH = new Adapter.DrawStackMethodsWrapper( SET_ACTIVE_ACTION );
				}
				
				return __SET_ACTIVE_ACTION_HASH;
			}
		}
		// int SET_ACTIVE_ACTION_HASH = "SET_ACTIVE_ACTION".GetHashCode();
		void SET_ACTIVE_ACTION( Rect worldOffset, Rect inputRect, Adapter.DrawStackMethodsWrapperData data, Adapter.HierarchyObject _o )
		{
		
			var  tR = inputRect;
			var o = _o.go;
			
			if ( Event.current.rawType == EventType.MouseUp )
			{	OnRawUp();
				// Hierarchy.RepaintAllView();
			}
			
			var contains =  (tR.Contains( Event.current.mousePosition ) || (adapter.hashoveredItem && adapter.hoverID == _o.id));
			
			if ( stateForDrag.HasValue && contains && Event.current.button == 0 )
			{	GUI.DrawTexture( tR, adapter.STYLE_DEFBUTTON.active.background );
				var any = false;
				
				foreach ( var VARIABLE in o.GetComponentsInChildren<Transform>( true ) )
				{	/* var res = stateForDrag.Value & HideFlags.NotEditable | VARIABLE.gameObject.hideFlags & ~HideFlags.NotEditable;
					 if ((res & VARIABLE.gameObject.hideFlags) != res )
					 {   VARIABLE.gameObject.hideFlags |= res;
					     any = true;
					 }*/
					var check  = VARIABLE.gameObject.hideFlags;
					
					if ( stateForDrag.Value != 0 ) VARIABLE.gameObject.hideFlags &= ~stateForDrag.Value;
					else VARIABLE.gameObject.hideFlags |= HideFlags.NotEditable;
					
					any |= check != VARIABLE.gameObject.hideFlags;
					
					if ( check != VARIABLE.gameObject.hideFlags )
					{	if ( (VARIABLE.gameObject.hideFlags & HideFlags.NotEditable) != 0 )
							getDoubleList( VARIABLE.gameObject.scene.GetHashCode() ).SetByKey( new GoGuidPair() { go = VARIABLE.gameObject.gameObject }, true );
						else
							getDoubleList( VARIABLE.gameObject.scene.GetHashCode() ).RemoveAll( new GoGuidPair() { go = VARIABLE.gameObject.gameObject } );
						SetLockToggle( VARIABLE.gameObject.scene.GetHashCode() );
					}
					
					/* if (Selection.gameObjects.Any(g => g == VARIABLE.gameObject))
					 {
					     Selection.objects = Selection.objects;
					 Hierarchy.RepaintAllViews();
					 }*/
				}
				
				if ( any ) ResetStack();
				
				if ( Event.current.isMouse ) Adapter.EventUse();
			}
			
			if ( tR.Contains( Event.current.mousePosition ) )
			{
			
				if ( tR.Contains( Event.current.mousePosition ) && Event.current.type == EventType.MouseDown && Event.current.button == 0 )
				{
				
				
					var targetOarr = new[] { o };
					var sel = adapter.SELECTED_GAMEOBJECTS();
					
					if ( sel.Any( c => c.id == _o.id ) /*&& Event.current.control*/) targetOarr = Utilities.GetOnlyTopObjects( sel, adapter ).Select( g => g.go ).ToArray(); ;
					
					//  targetOarr = Utilities.GetOnlyTopObjects( targetOarr , adapter ).Select( g => g.go ).ToArray(); ;
					
					
					var old = o.hideFlags & HideFlags.NotEditable;
					
					// bool needSelect = false;
					foreach ( var targetO in targetOarr )
					{
					
						var checkValue = targetO.hideFlags;
						
						if ( old != 0 ) targetO.hideFlags &= ~old;
						else targetO.hideFlags |= HideFlags.NotEditable;
						
						if ( checkValue != targetO.hideFlags )
						{	if ( (targetO.hideFlags & HideFlags.NotEditable) != 0 )
								getDoubleList( targetO.scene.GetHashCode() ).SetByKey( new GoGuidPair() { go = targetO.gameObject }, true );
							else
								getDoubleList( targetO.scene.GetHashCode() ).RemoveAll( new GoGuidPair() { go = targetO.gameObject } );
							ResetStack();
							SetLockToggle( targetO.scene.GetHashCode() );
						}
						
						
						if ( stateForDrag == null ) adapter.PUSH_ONMOUSEUP( OnRawUp );
						
						stateForDrag = old;
						
						var any = false;
						
						//  bool needRepaint = false;
						foreach ( var VARIABLE in targetO.GetComponentsInChildren<Transform>( true ) )     // Undo.RecordObject(VARIABLE, "Change Lock/Unlock state");
						{	// VARIABLE.gameObject.hideFlags = stateForDrag.Value & HideFlags.NotEditable | VARIABLE.gameObject.hideFlags & ~HideFlags.NotEditable;
						
							var check  = targetO.hideFlags;
							
							if ( stateForDrag.Value != 0 ) targetO.hideFlags &= ~stateForDrag.Value;
							else targetO.hideFlags |= HideFlags.NotEditable;
							
							any |= check != targetO.hideFlags;
							
							if (check != targetO.hideFlags )
							{	if ( (targetO.hideFlags & HideFlags.NotEditable) != 0 )
									getDoubleList( targetO.scene.GetHashCode() ).SetByKey( new GoGuidPair() { go = targetO.gameObject }, true );
								else
									getDoubleList( targetO.scene.GetHashCode() ).RemoveAll( new GoGuidPair() { go = targetO.gameObject } );
								SetLockToggle( targetO.scene.GetHashCode() );
							}
							
							//EditorUtility.SetDirty(VARIABLE);
							/*  if ( Selection.gameObjects.Any( g => g == VARIABLE.gameObject ) )
							  {   needSelect = true;
							      // Hierarchy.RepaintAllViews();
							  }*/
						}
						
						if ( any ) ResetStack();
					}
					
					// if ( needSelect ) Selection.objects = Selection.objects;
					adapter.RepaintAllViews();
					//  Hierarchy.RepaintAllView();
				}
				
				if ( Event.current.isMouse && Event.current.button == 0 ) Adapter.EventUse();
			}
			
		}
	}
}
}


