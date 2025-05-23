﻿#define DISABLE_PING

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine.SceneManagement;
using UnityEngineInternal;
using Object = UnityEngine.Object;

//namespace EModues

namespace EModules.EModulesInternal {
internal partial class Hierarchy {



	internal partial class M_CustomIcons : Adapter.Module, IModuleOnnector_M_CustomIcons {
	
		internal struct ARGS
		{	internal Component[] drawComps;
			internal string MenuText;
			internal bool allowHide;
			internal Type callbackType;
		}
		
		
		
		
		
		Adapter. DrawStackMethodsWrapper __BUTTON_ACTION_HASH = null;
		Adapter.DrawStackMethodsWrapper BUTTON_ACTION_HASH { get { return __BUTTON_ACTION_HASH ?? (__BUTTON_ACTION_HASH = new Adapter.DrawStackMethodsWrapper( BUTTON_ACTION )); } }
		void BUTTON_ACTION( Rect worldOffset, Rect inputRect, Adapter.DrawStackMethodsWrapperData data, Adapter.HierarchyObject _o)
		{
		
			var o = _o.go;
			
			
			if ( Event.current.button == 0 )
			{
			
			
				var arr = (M_CustomIcons.ARGS) data.args;
				var drawComps = arr.drawComps.Where(c => c);
				//  var drawComps = get_drawComps(_o.id);
				var MenuText = arr.MenuText;
				var allowHide = arr.allowHide;
				var callbackType = arr.callbackType;
				// Debug.Log( drawComps[0] );
				
				
				
				
				
				
				
				
				var components = new List<Component>();
				
				/* if (GUID != null) components.Add(readcomps.First(asd => ComponentToGUID(asd) == GUID));
				 else*/
				{	foreach ( var component in drawComps )
					{	components.Add( component );
					}
				}
				
				if ( Event.current.control )
				{	bool? val = null;
				
					foreach ( var component in components )
					{	if ( !component ) continue;
					
						var target = component;
						
						if ( HaveEnable( target ) )
						{	if ( val == null ) val = !GetEnable( target );
						
							_S_(o, component, val.Value );
						}
						
						
						/* var target = component;
						 if (HaveEnable(target))
						 {
						     Undo.RecordObject(target, "Enable/Disable Component");
						     SetEnable(target, val.Value);
						     Hierarchy.SetDirty(target);
						 }*/
					}
				}
				
				else         // MonoBehaviour.print(components.Count);
				{	var menu = new GenericMenu();
					var types = components.Select(c => c.GetType());
					var dic = types.Distinct().ToDictionary(t => t, t => 0);
					
					foreach ( var component in components )
					{	var name = components.Count == 1 ? "" : component.GetType().Name ;
					
						if ( components.Count > SHORT ) name = "Enabled/" + name;
						else name = "Enabled " + name;
						
						if ( HaveEnable( component ) )
						{	var target = component;
							var type = component.GetType();
							string postfix = null;
							
							if ( types.Count( t => t == type ) > 1 ) postfix = " [" + (dic[type]++) + "]";
							
							menu.AddItem( new GUIContent(/*"Enabled '"*/ name + /*"'" + type.Name + "'"  +*/ (postfix ?? "") + " %click" ), GetEnable( component ), () =>
							{	_S_( o, target, !GetEnable( target ) );
							} );
						}
						
						else
						{	menu.AddDisabledItem( new GUIContent(/*"Enabled '"*/name + "'" + component.GetType().Name + "'" ) );
						}
					}
					
					menu.AddDisabledItem( new GUIContent(/*"Enabled '"*/ "Ctrl+DRAG to move or Ctrl+Shift+DRAG to copy" ) ); //Drag %drag"
					menu.AddSeparator( "" );
					
					AudioSource aus = null;
					
					foreach ( var component in components ) if ( component is AudioSource ) aus = component as AudioSource;
					
					if ( aus )
					{	if ( !aus.clip )
							menu.AddDisabledItem( new GUIContent( "Play AudioSource" ) );
						else
							menu.AddItem( new GUIContent( aus.isPlaying ? "Stop AudioSource" : "Play AudioSource" ),
							              false, () =>
						{	((M_Audio)adapter.modules.First(m => m is M_Audio)).PlayAudio( aus );
						} );
						
						menu.AddSeparator( "" );
					}
					
					
					
					
					foreach ( var component in components )
					{	var target = component;
						var name = "'" + component.GetType().Name + "'";
						
						if ( components.Count > SHORT ) name = "Filter Selection By Component/" + name;
						else name = "Filter Selection By " + name;
						
						if ( adapter.SELECTED_GAMEOBJECTS().Any( s => s.go == target.gameObject ) )
						{
						
							menu.AddItem( new GUIContent( name ), false, () =>
							{	Selection.objects = adapter.SELECTED_GAMEOBJECTS().Where( s => s.go.GetComponent( target.GetType() ) ).Select( g => g.go ).ToArray();
							} );
						}
						
						else
						{	menu.AddDisabledItem( new GUIContent( name ) );
						
						}
						
					}
					
					
					menu.AddSeparator( "" );
					
					bool sumAdd = false;
					bool wasAdd = false;
					
					foreach ( var component in components )
					{	if ( !(component is MonoBehaviour) ) continue;
					
						var category = "Methods";/* at '" + component.GetType().Name + "'*/
						var methods = component.GetType().GetMethods((BindingFlags)(-1));
						var comp = component;
						
						foreach ( var methodInfo in methods )
						{	if ( methodInfo.IsStatic ) continue;
						
							var capt = methodInfo;
							wasAdd = true;
							menu.AddItem( new GUIContent( category + "/" + methodInfo.Name ), false, () =>
							{	adapter.OneFrameActionOnUpdate = true;
								adapter.OneFrameActionOnUpdateAC += () =>
								{	if ( !comp ) return;
								
									var pars = capt.GetParameters().Select(p =>
									{	if (!p.ParameterType.IsClass)
										{	return Activator.CreateInstance(p.ParameterType);
										}
										
										return null;
									}).ToArray();
									var result = capt.Invoke(comp, pars);
									
									if ( capt.ReturnType != typeof( void ) ) Debug.Log( "'" + capt.Name + "' returned: " + (result == null ? "null" : result.ToString()) + " (" + capt.ReturnType.Name + ")" );
								};
							} );
						}
						
						if ( wasAdd ) menu.AddSeparator( category );
						
						foreach ( var methodInfo in methods )
						{	if ( !methodInfo.IsStatic ) continue;
						
							var capt = methodInfo;
							wasAdd = true;
							menu.AddItem( new GUIContent( category + "/" + methodInfo.Name ), false, () =>
							{	adapter.OneFrameActionOnUpdate = true;
								adapter.OneFrameActionOnUpdateAC += () =>
								{	if ( !comp ) return;
								
									var pars = capt.GetParameters().Select(p =>
									{	if (!p.ParameterType.IsClass)
										{	return Activator.CreateInstance(p.ParameterType);
										}
										
										return null;
									}).ToArray();
									var result = capt.Invoke(comp, pars);
									
									if ( capt.ReturnType != typeof( void ) ) Debug.Log( "'" + capt.Name + "' returned: " + (result == null ? "null" : result.ToString()) + " (" + capt.ReturnType.Name + ")" );
								};
							} );
						}
						
						// Selection.objects = Selection.gameObjects.Where(s => s.GetComponent(target.GetType())).ToArray();
						
						/*  NeedApplyMod = true;
						  Mody += () => {
						      GUI_ONESHOT = true;
						      GUI_ONESHOTAC += () => {
						
						          // EditorUtility.ResetMouseDown();
						          /* GUIUtility.keyboardControl = 0;
						          #1#
						          //  MonoBehaviour.print(Event.PopEvent(new Event() { type = EventType.MouseDown }));
						          var reflectorMenu = new GenericMenu();
						          reflectorMenu.AddItem(new GUIContent("ASD"), false, () => { });
						          reflectorMenu.ShowAsContext();
						          InternalEditorUtility.RepaintAllViews();
						          EditorGUIUtility.CommandEvent("Redraw");
						      };
						  };*/
						
						
						//   EditorUtility.DisplayCustomMenu(
						//  RepaintWindowInUpdate());
						//
						/* menu.AddItem(new GUIContent(tc.text + "/asd"), false, () => {
						
						 });*/
						/*   var target = component;
						   if (Selection.gameObjects.Contains(target.gameObject))
						   {
						
						
						   } else
						   {
						       menu.AddDisabledItem(new GUIContent("Invoke Method at '" + component.GetType().Name + "'"));
						
						   }*/
						
					}
					
					sumAdd |= wasAdd;
					wasAdd = false;
					
					foreach ( var component in components )
					{	if ( !(component is MonoBehaviour) ) continue;
					
						var category = "Fields"; //at '" + component.GetType().Name + "'"
						var methods = component.GetType().GetFields((BindingFlags)(-1));
						var comp = component;
						
						foreach ( var methodInfo in methods )
						{	if ( methodInfo.IsStatic ) continue;
						
							var capt = methodInfo;
							wasAdd = true;
							menu.AddItem( new GUIContent( category + "/" + methodInfo.Name ), false, () =>
							{	adapter.OneFrameActionOnUpdate = true;
								adapter.OneFrameActionOnUpdateAC += () =>
								{	if ( !comp ) return;
								
									var result = capt.GetValue(comp);
									Debug.Log( /*"'" + capt.Name + "' returned: " +*/ (result == null ? "null" : result.ToString()) );
								};
							} );
						}
						
						if ( wasAdd ) menu.AddSeparator( category );
						
						foreach ( var methodInfo in methods )
						{	if ( !methodInfo.IsStatic ) continue;
						
							var capt = methodInfo;
							wasAdd = true;
							menu.AddItem( new GUIContent( category + "/" + methodInfo.Name ), false, () =>
							{	adapter.OneFrameActionOnUpdate = true;
								adapter.OneFrameActionOnUpdateAC += () =>
								{	if ( !comp ) return;
								
									var result = capt.GetValue(comp);
									Debug.Log( /*"'" + capt.Name + "' returned: " +*/ (result == null ? "null" : result.ToString()) );
								};
							} );
						}
					}
					
					sumAdd |= wasAdd;
					wasAdd = false;
					
					foreach ( var component in components )
					{	if ( !(component is MonoBehaviour) ) continue;
					
						var category = "Properties"; //at '" + component.GetType().Name + "'"
						var methods = component.GetType().GetProperties((BindingFlags)(-1));
						var comp = component;
						
						foreach ( var methodInfo in methods )
						{	var m = methodInfo.GetGetMethod( true );
						
							if ( m == null || methodInfo.GetGetMethod( true ).IsStatic ) continue;
							
							var capt = methodInfo;
							wasAdd = true;
							menu.AddItem( new GUIContent( category + "/" + methodInfo.Name ), false, () =>
							{	adapter.OneFrameActionOnUpdate = true;
								adapter.OneFrameActionOnUpdateAC += () =>
								{	if ( !comp ) return;
								
									var result = capt.GetValue(comp, null);
									Debug.Log( /*"'" + capt.Name + "' returned: " +*/ (result == null ? "null" : result.ToString()) );
								};
							} );
						}
						
						if ( wasAdd ) menu.AddSeparator( category );
						
						foreach ( var methodInfo in methods )
						{	var m = methodInfo.GetGetMethod( true );
						
							if ( m == null || !methodInfo.GetGetMethod( true ).IsStatic ) continue;
							
							var capt = methodInfo;
							wasAdd = true;
							menu.AddItem( new GUIContent( category + "/" + methodInfo.Name ), false, () =>
							{	adapter.OneFrameActionOnUpdate = true;
								adapter.OneFrameActionOnUpdateAC += () =>
								{	if ( !comp ) return;
								
									var result = capt.GetValue(comp, null);
									Debug.Log( /*"'" + capt.Name + "' returned: " +*/ (result == null ? "null" : result.ToString()) );
								};
							} );
						}
					}
					
					sumAdd |= wasAdd;
					
					
					if ( sumAdd ) menu.AddSeparator( "" );
					
					foreach ( var component in components )
					{	var target = component;
						var name = "'" + component.GetType().Name + "'";
						
						if ( components.Count > SHORT ) name = "Copy Component/" + name;
						else name = "Copy Component";
						
						//else name = "Copy Component " + name;
						
						menu.AddItem( new GUIContent( name ), false, () =>         // EditorUtility.CopySerialized
						{	// EditorUtility.NaturalCompare(
							/*UnityEngine.Scripting.;*/
							//   ComponentUtility.CopyComponent(o,component1 => { });
							Hierarchy.Copy( target );
							// UnityEditorInternal.ComponentUtility.CopyComponent(target);
							
						} );
					}
					
					foreach ( var component in components )
					{	var target = component;
					
						//var able = UnityEditorInternal.ComponentUtility.PasteComponentValues(target);
						// Undo.PerformUndo();
						if ( Hierarchy.PastValidate( target ) )
						{	var name = "'" + component.GetType().Name + "'";
						
							if ( components.Count > SHORT ) name = "Paste Component Values/" + name;
							else name = "Paste Component Values";
							
							//else name = "Paste Component Values " + name;
							
							menu.AddItem( new GUIContent( name ), false, () =>
							{
							
								if ( adapter.SELECTED_GAMEOBJECTS().All( selO => selO.go != o ) )
								{	Hierarchy.Paste( target );
								
#if !DISABLE_PING
								
									if ( Hierarchy.par.ENABLE_PING_Fix ) adapter.TRY_PING_OBJECT( o );
									
#endif
								}
								
								else
								{
								
									foreach ( var objectToUndo in adapter.SELECTED_GAMEOBJECTS() ) /////#tag TODO index not working
									{	var c = objectToUndo.go.GetComponents(target.GetType());
									
										foreach ( var variable in c )
										{	Hierarchy.Paste( variable );
										}
										
										// if (Hierarchy.par.ENABLE_PING_Fix) adapter.TRY_PING_OBJECT(objectToUndo);
									}
								}
								
								// EditorUtility.CopySerialized
							} );
						}
						
						else menu.AddDisabledItem( new GUIContent( "Paste Component As New '" + component.GetType().Name + "'" ) );
						
						{	var name = "'" + component.GetType().Name + "'";
						
							if ( components.Count > SHORT ) name = "Paste Component Values As New/" + name;
							else name = "Paste Component Values As New";
							
							menu.AddItem( new GUIContent( name ), false, () =>
							{
							
								if ( adapter.SELECTED_GAMEOBJECTS().All( selO => selO.go != o ) )
								{	Hierarchy.PasteComponentAsNew( target );
								
#if !DISABLE_PING
								
									if ( Hierarchy.par.ENABLE_PING_Fix ) adapter.TRY_PING_OBJECT( o );
									
#endif
								}
								
								else
								{
								
									foreach ( var objectToUndo in adapter.SELECTED_GAMEOBJECTS() ) /////#tag TODO index not working
									{	var c = objectToUndo.go.GetComponents(target.GetType());
									
										foreach ( var variable in c )
										{	Hierarchy.PasteComponentAsNew( variable );
										}
										
										// if (Hierarchy.par.ENABLE_PING_Fix) adapter.TRY_PING_OBJECT(objectToUndo);
									}
								}
								
								ResetStack();
								// EditorUtility.CopySerialized
							} );
						}
						
					}
					
					menu.AddSeparator( "" );
					
					dic = types.Distinct().ToDictionary( t => t, t => 0 );
					
					
					foreach ( var component in components )
					{	var target = component;
						var type = component.GetType();
						string postfix = null;
						
						if ( types.Count( t => t == type ) > 1 ) postfix = " [" + (dic[type]++) + "]";
						
						var name = "'" + component.GetType().Name + "'" + (postfix ?? "");
						
						if ( components.Count > SHORT ) name = "Remove Component/" + name;
						else name = "Remove";
						
						// else name = "Remove " + name;
						
						menu.AddItem( new GUIContent( name ), false, () =>
						{	/*   var oaa = target.gameObject;
							       Undo.DestroyObjectImmediate(target);
							       Hierarchy.SetDirty(oaa);*/
							
							if ( adapter.SELECTED_GAMEOBJECTS().All( selO => selO.go != o ) )
							{	var oaa = target.gameObject;
								Undo.DestroyObjectImmediate( target );
								Adapter.SetDirty( oaa );
								adapter.MarkSceneDirty( oaa.scene );
#if !DISABLE_PING
								
								if ( Hierarchy.par.ENABLE_PING_Fix ) adapter.TRY_PING_OBJECT( o );
								
#endif
							}
							
							else
							{	var index = o.GetComponents(target.GetType()).ToList().IndexOf(target);
							
								if ( index == -1 ) return;
								
								foreach ( var objectToUndo in adapter.SELECTED_GAMEOBJECTS() )
								{	var c = objectToUndo.go.GetComponents(target.GetType());
								
									if ( index >= c.Length ) continue;
									
									var variable = c[index];
									/*  foreach (var objectToUndo in Hierarchy.gameObjects())
									  {
									      var c = objectToUndo.GetComponents(target.GetType());
									      foreach (var variable in c)*/
									{	var oaa = variable.gameObject;
										Undo.DestroyObjectImmediate( variable );
										Adapter.SetDirty( oaa );
										adapter.MarkSceneDirty( oaa.scene );
									}
									
									//  if (Hierarchy.par.ENABLE_PING_Fix) adapter.TRY_PING_OBJECT(objectToUndo);
								}
							}
							
							ResetStack();
						} );
						
					}
					
					menu.AddSeparator( "" );
					
					if ( components.Count == 1 && callbackType != null )
					{	if ( components.FirstOrDefault( c => c.GetType() == callbackType ) != null && components.First( c => c.GetType() == callbackType ) is MonoBehaviour )
						{	var target = MonoScript.FromMonoBehaviour(components.First(c => c.GetType() == callbackType) as MonoBehaviour);
							var path = AssetDatabase.GetAssetPath(target);
							
							if ( string.IsNullOrEmpty( path ) ) menu.AddDisabledItem( new GUIContent( "Edit Script" ) );
							else menu.AddItem( new GUIContent( "Edit Script" ), false, () =>         //EditorUtility.opebn
							{	AssetDatabase.OpenAsset( target.GetInstanceID() );
								//InternalEditorUtility.OpenFileAtLineExternal( path, 0 );
								//  Selection.objects = new[] { target };
							} );
							menu.AddItem( new GUIContent( "Revival '" + callbackType.Name + "' in Project" ), false, () =>
							{	Selection.objects = new[] { target };
							} );
						}
					}
					
					else
					{	var res = components.Where(c => c is MonoBehaviour).ToArray();
					
						if ( res.Length != 0 )
						{	foreach ( var component in res )
							{	var target = MonoScript.FromMonoBehaviour(component as MonoBehaviour);
								menu.AddItem( new GUIContent( "Revival Component in Project/'" + component.GetType().Name + "'" ), false, () =>
								{	Selection.objects = new[] { target };
								} );
							}
						}
					}
					
					if ( MenuText != null )
					{	if ( allowHide )
						{	var type = callbackType;
							menu.AddItem( new GUIContent( MenuText ), false, () =>
							{	Hierarchy_GUI.Undo( adapter, "Hide Icon" );
								Hierarchy_GUI.Instance( adapter ).HiddenComponents.Add( type.FullName );
								Hierarchy_GUI.SetDirtyObject( adapter );
							} );
							ResetStack();
							/* menu.AddSeparator("");
							 menu.AddItem(new GUIContent("Open Settings"), false, SETUPROOT.showWindow);*/
						}
						
						else
						{	menu.AddDisabledItem( new GUIContent( MenuText ) );
						
						}
					}
					
					
					//  menu.AddSeparator("");
					
					
					menu.AddItem( new GUIContent( "Open Settings" ), false, () =>
					{	adapter.SHOW_HIER_SETTINGS_GENERICMENU();
					} );
					
					menu.ShowAsContext();
				}
				
				
				Adapter.EventUse();
			}
			
			if ( Event.current.button == 1 )
			{	Adapter.EventUse();
				var arr = (M_CustomIcons.ARGS) data.args;
				var drawComps = arr.drawComps.Where(c => c);
				
				var mp = new MousePos( Event.current.mousePosition, MousePos.Type.Search_356_0, !callFromExternal(), adapter);
				var oldFilt = lastFocusRoot == null ? null : lastFocusRoot.SearchFilter;
				/*  int[] contentCost = new int[0];
				  GameObject[] obs = new GameObject[0];*/
				var captureCall = callFromExternal();
				Action<Type> result = (filt) =>
				{
				
					/*  if (captureCall && lastFocusRoot != null)
					  {
					      // contentCost = lastFocusRoot.contentCost.ToArray();
					      CallHeared(lastFocusRoot.objects.ToArray(), out obs, out contentCost, filt);
					  } else
					  {
					
					      // if (EditorSceneManager.GetActiveScene().rootCount != 0) CallHeared(Utilities.AllSceneObjectsInterator().GetEnumerator(), out obs, out contentCost, filt);
					      if (EditorSceneManager.GetActiveScene().rootCount != 0) CallHeared(Utilities.AllSceneObjects(), out obs, out contentCost, filt);
					  }
					
					  lastFocusRoot = (FillterData)FillterData.Init(mp, SearchHelper + " " + filt.Name, filt.Name, obs, contentCost, null, this, filt);*/
					
					var ttt = filt.Name.Replace(adapter.pluginname + "_KEY_#1", "default");
					lastFocusRoot = (_W__SearchWindow)_W__SearchWindow.Init(mp, SearchHelper + " " + ttt, ttt, CallHeaderFiltered(filt), this, adapter, _o);
					
					if (captureCall && oldFilt != null && lastFocusRoot)
						lastFocusRoot.FiltersOf = oldFilt;
				};
				
				
				if ( arr.callbackType == null )
				{	/* if (GUID != null) callbackType = readcomps.First(asd => ComponentToGUID(asd) == GUID).GetType();
					     else*/
					{	var menu = new GenericMenu();
					
						foreach ( var component in drawComps )
						{	var comp = component;
							menu.AddItem( new GUIContent( Adapter.GetTypeName( comp ) ), false, () =>
							{	result( Adapter.GetType_( comp ) );
							} );
						}
						
						menu.ShowAsContext();
						Adapter.EventUse();
						return;
					}
				}
				
				
				result( arr.callbackType );
				
				
				
			}
		}
	}
}
}
