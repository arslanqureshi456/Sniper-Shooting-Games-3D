
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

	bool wasUndo;
	
	internal sealed partial class BottomInterface {
	
		int LastActiveScene
		{	get { return adapter.LastActiveScene; }
		
			set { adapter.LastActiveScene = value; }
		}
		
		int frames = 0;
		public void Updater()     //if (Application.isPlaying) MonoBehaviour.print(Application.isPlaying);
		{
		
			foreach ( var scrollPo in adapter.GUIControlToWindowObject )
			{	adapter.ScrollPositions[scrollPo.Key] = null;
				adapter.WasPaint[scrollPo.Key] = null;
				adapter.WasEvent[scrollPo.Key] = null;
				adapter.WasInitDraw[scrollPo.Key] = null;
			}
			
			adapter.___ContentSize = null;
			
			if ( frames < 5 )
			{	frames++;
			
				if ( frames == 5 )
				{	NEW_RELOAD = true;
				}
			}
			
			
			if ( adapter.DISABLE_DES() ) return;
			
#if HIERARCHY
			
			if ( adapter.pluginID == Initializator.HIERARCHY_ID )
			{	bool wasShanged = false;
				var so  = adapter.SELECTED_GAMEOBJECTS();
				
				if ( adapter.modules.Length != 0 && adapter.modules[3].enable && adapter.par.LOCK_SELECTION )
				{	foreach ( var gameObject in so )
					{	if ( !gameObject.go ) continue;
					
						if ( (gameObject.go.hideFlags & HideFlags.NotEditable) != 0 )
						{	if ( adapter.wasUndo && !ignoreLock.Contains( gameObject.go ) ) ArrayUtility.Add( ref ignoreLock, gameObject.go );
						
							if ( !ignoreLock.Contains( gameObject.go ) )
							{	//  Selection.objects = lastSel.Where( l => l && ignoreLock.Any( i => i && i.GetInstanceID() == l.GetInstanceID() ) ).ToArray();
								//  InternalEditorUtility.RepaintAllViews();
								wasShanged = true;
								
								//Debug.Log( "ASD" );
								SelectChange = false;
							}
						}
					}
				}
				
				if ( wasShanged )
				{	/*so = so.Where( l => l.go && !ignoreLock.Any( i => i && i.GetInstanceID() == l.go.GetInstanceID() ) ).ToArray();
					Selection.objects = so.Select( g => g.go ).ToArray();
					adapter.InternalClearSelection( so.Select( g => g.go.GetInstanceID() ).ToArray() );*/
					var lastSel = so.Select(s1 => s1.go).Where(g => g && (g.hideFlags & HideFlags.NotEditable) == 0).ToArray();
					var so2 = lastSel.Where( l => l && !ignoreLock.Any( i => i && i.GetInstanceID() == l.GetInstanceID() ) ).ToArray();
					Selection.objects = so2;
					adapter.InternalClearSelection( so2.Select( l => l ? l.GetInstanceID() : -1 ).ToArray() );
					// Debug.Log( so2.Length + " " + ignoreLock.Length + " " + lastSel.Length );
					SelectChange = so2.Length != lastSel.Length;
				}
				
				adapter.wasUndo = false;
			}
			
#endif
			
			//  var activeScene = Adapter.GET_SCENE_BY_ID( adapter.GET_ACTIVE_SCENE);
			var activeScene = SceneManager.GetActiveScene();
			
			
			if ( activeScene.IsValid() && activeScene.isLoaded )     // MonoBehaviour.print(m_memCache[MemType.Last].Count(c=>c.InstanceID != null &&c.InstanceID.ActiveGameObject));
			{	//  MonoBehaviour.print( scene.path + " " + lastScenePath );
			
			
			
			
			
			
			
			
			
			
			
			
				bool rg_an_ca = false;
				
				if ( SelectChange )
				{	if ( ignoreLock.Length != 0 ) ignoreLock = new GameObject[0];
				
					SelectChange = false;
					
					if ( INT32_ACTIVE( newIds )/* && !SkipRemove*/ && !SkipRemoveFix )
					{	if ( adapter.IS_HIERARCHY() ) LastActiveScene = INT32_SCENE( newIds );
					
						//FindPreviousLastSelection();
						
						//  if ( findIndex == -1 )
						{	if ( adapter.MOI.des( LastActiveScene ).GetHash3().Count == 0 )
								adapter.MOI.des( LastActiveScene ).GetHash3().Add( newIds );
							else adapter.MOI.des( LastActiveScene ).GetHash3().Insert( 0, newIds );
						}
						LastIndex = 0;
					}
					
					
					if ( adapter.MOI.des( LastActiveScene ).GetHash3().Count > adapter.MAX20 )
					{	/*
						     //#tag TODO This thing can be cut in so that there is no loss of the last selections.
						    M_Descript.des(scene).GetHash3().RemoveAll(h => !h.ActiveGameObject);
						    */
						var scene = LastActiveScene;
						
						while ( adapter.MOI.des( scene ).GetHash3().Count > adapter.MAX20 )
							adapter.MOI.des( scene ).GetHash3().RemoveAt( adapter.MAX20 );
					}
					
					NeedRefreshBOttom = false;
					SkipRemoveFix = SkipRemove = false;
					
					if ( adapter.SAVE_LASTSELECTION_BEFORESCENEWILLSCLOSE || Hierarchy_GUI.Instance( adapter ).SaveToScriptableObject == "FOLDER" )
					{	adapter.SetDirtyDescription( adapter.MOI.des( LastActiveScene ), LastActiveScene );
					
						if ( adapter.SAVE_LASTSELECTION_BEFORESCENEWILLSCLOSE ) adapter.MarkSceneDirty( LastActiveScene );
					}
					
					rg_an_ca = true;
					/*	Scene_RefreshGUIAndClearActions( LastActiveScene );
						adapter.RepaintWindow(true);*/
				}
				
				
				
				
				
				
				
				
				
				
				
				
				
				
				
				
				
				if ( !string.IsNullOrEmpty( activeScene.path ) && (activeScene.GetHashCode() != lastSceneID || activeScene.path != lastScenePath) )
				{	/*cacheInit = false;
					    m_memCache.Clear();
					    m_memPosition.Clear();*/
					/* var sp = adapter. GetHierarchyWindowScrollPos();
					 adapter. ResetScroll( Vector2.zero );*/
					//
					// adapter.__GUI_ONESHOT = true;
					
					if ( adapter.IMGUI() )
						adapter.GUI_ONESHOTPUSH( () =>
						
						
												 // adapter.INIT_IF_NEDDED();
												 // adapter.INIT_TREE();
												 
					{	// adapter. ReloadEd = true;
					
						// adapter. ResetScroll(sp  );
						if ( !adapter.IMGUI20183() )
							adapter.HEIGHT_RIX_FUNCTIUON( adapter.window(), adapter.m_TreeView( adapter.window() ), true );
							
						/*adapter.GUI_ONESHOT = true;
						adapter.GUI_ONESHOTAC = () =>
						{   InternalEditorUtility.RepaintAllViews();
						    Debug.Log("ASD");
						};
						InternalEditorUtility.RepaintAllViews();*/
						//adapter.RepaintWindow();
					} );
					
					
					var sh = activeScene.GetHashCode();
					
					
					if ( adapter.IS_HIERARCHY() ) LastActiveScene = sh;
					
					LastIndex = -1;
					Scene_WriteLastScene( (sh) );
					Scene_SetDityMemory( (sh), new Scene[0] );
					rg_an_ca = true;
				}
				
				if (rg_an_ca)
					Scene_RefreshGUIAndClearActions( LastActiveScene );
					
					
			}
			
			
			if ( NeedRefreshBOttom )
			{	NeedRefreshBOttom = false;
				Scene_RefreshGUIAndClearActions( LastActiveScene );
			}
			
			if ( adapter.window() == null ) return;
			
			var old = HEIGHT;
			// HEIGHT = Mathf.RoundToInt( Mathf.MoveTowards( HEIGHT, adapter.ENABLE_BOTTOMDOCK_PROPERTY ? REFERENCE_HEIGHT : 0f, adapter.deltaTime * 940 * 2 ) );
			HEIGHT = Mathf.RoundToInt( adapter.ENABLE_BOTTOMDOCK_PROPERTY ? REFERENCE_HEIGHT : 0f);
			hyperGraph.Update();
			
			
			
			/*    if (hierarchy_windows.Count != 0)
			    {
			        foreach (var hierarchyWindow in hierarchy_windows)
			        {
			            Hierarchy.BottomInterface.BOTTOM_UPDATE_POSITION((EditorWindow)hierarchyWindow);
			            break;
			        }
			    } else
			    {
			        if (window() != null) Hierarchy.BottomInterface.BOTTOM_UPDATE_POSITION(window());
			    }
			*/
			
			if ( HEIGHT != old )
			{	adapter.RepaintWindowInUpdate();
				//window().Repaint();
			}
			
			// print(hierarchy_windows.Count);
			// print(GUIControlToWindowObject.Count);
			
			
			/*
			                if (WINH == null && window() != null) {
			                    print("A" + (Rect)m_Pos.GetValue(window()));
			                    var winrect = (Rect)m_Pos.GetValue(window());
			                    WINH = winrect.height - Math.Max(winrect.height - 140, 0);
			                    //winrect.height -= WINH.Value;
			                    winrect.height -= 150;
			                    m_Pos.SetValue(window(), winrect);
			                    print("B" + (Rect)m_Pos.GetValue(window()));
			
			                    //   print("A" + winrect);
			                }*/
			//  FITTER();
		}
		
		
		
		
		void FindPreviousLastSelection()
		{	var scene = LastActiveScene;
		
			// MonoBehaviour.print(newIds.ActiveGameObject.name);
			///REMOVE PREVIOUS
#pragma warning disable
			bool findPreview = false;
#pragma warning restore
			var HASH = adapter.MOI.des( scene ).GetHash3();
			
			if ( !SkipRemove )
			{	var  findIndexPrev = HASH.Count > 0 && (  adapter.IS_HIERARCHY() ? HASH[0].ActiveGameObject == newIds.ActiveGameObject :
									 HASH[0].GUIDsActiveGameObject_CheckAndGet == newIds.GUIDsActiveGameObject_CheckAndGet );
									 
				if ( findIndexPrev )
				{
				
					/* var anyCompare = adapter.IS_HIERARCHY() ? HASH[0].list != null &&newIds.list != null && Mathf.Abs (HASH[0].list.Count - newIds.list.Count ) == 1:
					      HASH[0].GUIDsList != null && newIds.GUIDsList != null &&Mathf.Abs( HASH[0].GUIDsList.Count - newIds.GUIDsList.Count) == 1;
					
					 if ( anyCompare ) {*/
					var findIndex = -1;
					
					if ( adapter.IS_HIERARCHY() )
					{	findIndex = newIds.list.Count( l => HASH[0].list.Contains( l ) ) == Mathf.Min( newIds.list.Count, HASH[0].list.Count ) ? 0 : -1;
					}
					
					else
					{	findIndex = newIds.GUIDsList.Count( l => HASH[0].GUIDsList.Contains( l ) ) == Mathf.Min( newIds.GUIDsList.Count, HASH[0].GUIDsList.Count ) ? 0 : -1;
					}
					
					// var newObject = newIds;
					if ( findIndex != -1 /* && !M_Descript.des.GetHash3()[findIndex].CompareTo(newObject)*/)
					{	adapter.MOI.des( scene ).GetHash3().RemoveAt( 0 );
						findPreview = true;
					}
					
					// }
					
				}
			}
			
			
			
			///REMOVE ALL
			//  if ( !findPreview )
			{	var anyCompare = HASH.Any(h => adapter.IS_HIERARCHY() ? h.list != null && newIds.list != null &&  h.list.Count == newIds.list.Count :
										  h.GUIDsList != null && newIds.GUIDsList != null && h.GUIDsList.Count == newIds.GUIDsList.Count);
										  
				if ( anyCompare )
				{	var findIndex = -1;
				
					if ( adapter.IS_HIERARCHY() )
					{	var dic = newIds.list.ToDictionary(v => v ? v.GetInstanceID() : -1, v => v);
						findIndex = HASH.FindIndex( h =>
													h.list != null &&
													h.list.Count == newIds.list.Count &&
													h.list.All( l => l && dic.ContainsKey( l.GetInstanceID() ) ) );
					}
					
					else
					{	var dic = newIds.GUIDsList.ToDictionary(v => v != null ? v : "", v => v);
						findIndex = HASH.FindIndex( h => h.GUIDsList != null &&
													h.GUIDsList.Count == newIds.GUIDsList.Count &&
													h.GUIDsList.All( l => l != null && dic.ContainsKey( l ) ) );
					}
					
					// var newObject = newIds;
					if ( findIndex != -1 /* && !M_Descript.des.GetHash3()[findIndex].CompareTo(newObject)*/)
					{	adapter.MOI.des( scene ).GetHash3().RemoveAt( findIndex );
					}
				}
			}
			
			
		}
		
	}
}
}
