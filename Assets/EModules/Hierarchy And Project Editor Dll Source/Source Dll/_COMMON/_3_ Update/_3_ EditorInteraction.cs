#if UNITY_EDITOR
	#define HIERARCHY
	#define PROJECT
#endif


using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using EModules.EModulesInternal;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization;
using UnityEditorInternal;
using System.Reflection;


namespace EModules.EModulesInternal



{

partial class Adapter {




	static int? _mLastActiveScene;
	internal  int LastActiveScene
	{	get
		{	if (IS_PROJECT()) return -1;
		
			if (_mLastActiveScene == null || !GET_SCENE_BY_ID(_mLastActiveScene.Value).IsValid() || !GET_SCENE_BY_ID(_mLastActiveScene.Value).isLoaded)
			{	if (!Selection.activeGameObject || !Selection.activeGameObject.scene.IsValid() || !Selection.activeGameObject.scene.isLoaded) return EditorSceneManager.GetActiveScene().GetHashCode();
				else return Selection.activeGameObject.scene.GetHashCode();
			}
			
			return _mLastActiveScene.Value;
		}
		
		set
		{	if (_mLastActiveScene != value && IS_HIERARCHY() && HierAdapter.bottomInterface.onSceneChange != null) HierAdapter.bottomInterface.onSceneChange();
		
			_mLastActiveScene = value;
		}
	}
	
	internal int GET_ACTIVE_SCENE
	{	get
		{	if (IS_PROJECT()) return -1;
		
			if ( Selection.activeGameObject ) return Selection.activeGameObject.scene.GetHashCode();
			
			return EditorSceneManager.GetActiveScene().GetHashCode();// EditorSceneManager.GetActiveScene().GetHashCode();
		}
	}
#pragma warning disable
	Scene raw;
#pragma warning restore
	internal Scene GET_ACTIVE_SCENE_RAW
	{	get
		{	if ( IS_PROJECT() ) return raw;
		
			if ( Selection.activeGameObject ) return Selection.activeGameObject.scene;
			
			return EditorSceneManager.GetActiveScene();// EditorSceneManager.GetActiveScene().GetHashCode();
		}
	}
	
	
	
	internal Action onPlayModeChanged;
	internal Action PLAYMODECHANGE1, PLAYMODECHANGE2;
	void PlayModeStateChanged()
	{	if (PLAYMODECHANGE1 != null) PLAYMODECHANGE1();
	
		if (PLAYMODECHANGE2 != null) PLAYMODECHANGE2();
		
		CloseWindows();
		
		if ( onPlayModeChanged != null) onPlayModeChanged();
		
		PlayModeChanged = true;
	}
	
	
	
	
	
	internal Action SubcripeSceneLoader_method;
	
	internal void EditorSceneManagerOnSceneUnloaded(Scene arg0, LoadSceneMode loadSceneMode)
	{	if (SubcripeSceneLoader_method != null) SubcripeSceneLoader_method();
	
		ltg.Clear();
		CloseWindows();
	}
	
	internal void EditorSceneManagerOnSceneOpening(string path, OpenSceneMode mode)
	{	if (SubcripeSceneLoader_method != null) SubcripeSceneLoader_method();
	
		ltg.Clear();
		CloseWindows();
	}
	
	void CloseWindows()
	{	foreach (var w in _W___IWindow.__inputWindow)
		{	if (w.Value is _W__SearchWindow || w.Value is _W__InputWindow || w.Value is M_Colors_Window) ((_W___IWindow)w.Value).CloseThis();
		}
	}
	
	// internal  Dictionary<int, int> _mSelectedO = new Dictionary<int, int>();
	void S_CH()
	{	/* _mSelectedO.Clear();
		 foreach (var o in Selection.objects)
		 {   if (!o) continue;
		     if (!_mSelectedO.ContainsKey( o.GetInstanceID() )) _mSelectedO.Add( o.GetInstanceID(), 0 );
		 }*/
		_IsSelectedCache.Clear();
		_IsDraggedCache.Clear();
	}
	
	private void modifierKeysChanged()
	{	//if ( Application.isPlaying ) return;
		// Debug.Log( Event.current );
		/*  if ( USE_BUTTON_TO_INTERACT_AHC_BOOL  && (_S_HideRightIfNoFunction || _S_HideBttomIfNoFunction)
		     ) RepaintWindowInUpdate();
		  if (EditorWindow.focusedWindow && ( EditorWindow.focusedWindow .GetType().Name.Contains("SceneHierarchy") || EditorWindow.focusedWindow.GetType().Name.Contains( "Project" )) )
		      RepaintWindowInUpdate();*/
	}
	
	bool oldPrev = false;
	bool OLD_SHIFT, OLD_ALT, OLD_CTRL;
	private void modifierKeysChangedFix()     //if ( Application.isPlaying ) return;
	{
	
	
	
	
	
		var k = Event.current.keyCode;
		var mask = KeyCode.LeftAlt | KeyCode.RightAlt | KeyCode.LeftShift | KeyCode.RightShift | KeyCode.LeftControl | KeyCode.RightControl;
		
		if ((k & mask) == 0) return;
		
		
		
		
		var alt = OLD_ALT != Event.current.alt;
		OLD_ALT = Event.current.alt;
		var shift = OLD_SHIFT != Event.current.shift;
		OLD_SHIFT = Event.current.shift;
		var control = OLD_CTRL != Event.current.control;
		OLD_CTRL = Event.current.control;
		
		
		if ((control || shift || alt) && (!EditorWindow.focusedWindow || !EditorWindow.focusedWindow.GetType().Name.Contains( "SceneView" ) && !EditorWindow.focusedWindow.GetType().Name.Contains( "GameView" )
		                                  ||  !hashoveredItem || hoverID != -1)
		   )
			if ( USE_BUTTON_TO_INTERACT_AHC_BOOL && (_S_HideRightIfNoFunction  || _S_HideBttomIfNoFunction))
			{	var re = par.USE_BUTTON_TO_INTERACT_AHC & 3;
				var   prev  = false;
				
				if ( re == 1 && alt ) { RepaintWindowInUpdate(); prev = true; }
				
				if ( re == 2 && shift ) { RepaintWindowInUpdate(); prev = true; }
				
				if ( re == 3 && control ) { RepaintWindowInUpdate(); prev = true; }
				
				if (oldPrev != prev) RepaintWindowInUpdate();
				
				oldPrev = prev;
				
				
				//Debug.Log(EditorWindow.focusedWindow.GetType() + " " +hashoveredItem  + " " + hoverID );
			}
			
		if (SHOW_ONLY_HOVERED_RAW && alt &&
		        ( (!EditorWindow.focusedWindow || !EditorWindow.focusedWindow.GetType().Name.Contains( "SceneView" ) && !EditorWindow.focusedWindow.GetType().Name.Contains( "GameView" ))
		          || hashoveredItem && hoverID != -1
		        )
		   )
		{	RepaintWindowInUpdate();
		}
		
		//Debug.Log( SHIFT_TO_INSTANTIATE_BOTTOM + " " + shift  + " " +  EditorWindow.focusedWindow   + " " +  EditorWindow.focusedWindow.GetType().Name.Contains( "SceneView") );
		
		if (SHIFT_TO_INSTANTIATE_BOTTOM && shift && EditorWindow.focusedWindow  && EditorWindow.focusedWindow.GetType().Name.Contains( "SceneView"))
		{	RepaintWindowInUpdate();
		}
		
		/*	if (EditorWindow.focusedWindow && ((control || shift || alt) &&
			                                   (EditorWindow.focusedWindow.GetType().Name.Contains( "SceneHierarchy" )
			                                    ||  EditorWindow.focusedWindow.GetType().Name.Contains( "Project" )
			                                    ||  EditorWindow.focusedWindow.GetType().Name.Contains( "Inspector" )
			                                    || hashoveredItem && hoverID != -1
			                                   ) ||
			                                   control  &&EditorWindow.focusedWindow.GetType().Name.Contains( "SceneView")
			                                  )
			   )
				RepaintWindowInUpdate();*/
	}
	
	
	
	private  bool _didSelectionChange = true;
	private void OnSelectionChanged()
	{	_didSelectionChange = true;
	
		if (onSelectionChanged != null) onSelectionChanged();
	}
	
	private void OnEditorUpdate()
	{	if (_didSelectionChange)
		{	_didSelectionChange = false;
		}
	}
	internal Action onSelectionChanged;
	
	
	
	
	
	
	
	
	
	
	
	
	
}
}
