#if UNITY_EDITOR
	#define HIERARCHY
	#define PROJECT
#endif

//#define USE_LASTRECT
#if PROJECT
	using EModules.Project;
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

//namespace EModules

//% (ctrl on Windows, cmd on macOS), # (shift), &


namespace EModules.EModulesInternal


{



internal partial class Adapter {

	internal Type m1, m2, m3, m4, m5, m6, m7, m8, m9;
	
	internal class CustomMenuAndModule/*s<PROInterface, ONEInterface, M_CustomModuleParser>
            where PROInterface : class
            where ONEInterface : class*/
	{	/*    internal void Initialize( )
		    {
		
		    }*/
		Type PROInterface, ONEInterface, M_CustomModuleParser;
		internal CustomMenuAndModule( Type PROInterface, Type ONEInterface, Type M_CustomModuleParser, Adapter adapter )
		{	this.PROInterface = PROInterface;
			this.ONEInterface = ONEInterface;
			this.M_CustomModuleParser = M_CustomModuleParser;
			
			this.InitExtensionMenu( adapter );
		}
		
#if HIERARCHY
		List<HierarchyExtensions.IGenericMenu> Hierarchy_ExensionMenus = new List<HierarchyExtensions.IGenericMenu>();
#endif
#if PROJECT
		List<ProjectExtensions.IGenericMenu> Project_ExensionMenus = new List<ProjectExtensions.IGenericMenu>();
#endif
		
		KeyKodeData resultKeyCode;
		internal void InitExtensionMenu( Adapter adapter )
		{	RegistratedKeys.Clear();
			{	var sour = ONEInterface;
				var type = PROInterface;
				var types = ass_raw.SelectMany( a =>
				{	try
					{
					
						return a.GetTypes();
					}
					
					catch
					{
					
					}
					
					return new Type[0];
				}
											  );
				/* .SelectMany(s =>
				{   if (s == null) return new Type[0];
				Type[] _t = null;
				try
				{   _t = s.GetTypes();
				}
				catch
				{   return new Type[0];
				}
				return _t;
				}).ToArray();*/
				
				
#if HIERARCHY
				
				if ( adapter.pluginID == Initializator.HIERARCHY_ID )
					Hierarchy_ExensionMenus.Clear();
					
#endif
#if PROJECT
					
				if ( adapter.pluginID == Initializator.PROJECT_ID )
					Project_ExensionMenus.Clear();
					
#endif
					
					
#pragma warning disable
					
				foreach ( var type1 in types.Where( p => p != null && (!Adapter.LITE || sour.IsAssignableFrom( p )) && type.IsAssignableFrom( p ) && type.FullName != p.FullName ) )     //string name;
				{
#if HIERARCHY
				
					if ( adapter.pluginID == Initializator.HIERARCHY_ID )
					{	var item = Activator.CreateInstance(type1) as HierarchyExtensions.IGenericMenu;
						Hierarchy_ExensionMenus.Add( item );
						
						if ( TryAddToHotKeys( adapter, item.Name, ref resultKeyCode ) )
						{	Action resultAction = null;
						
							Func<HierarchyObject, bool> enableCap = (inputObject) => item.IsEnable(inputObject.go) && !item.NeedExcludeFromMenu(inputObject.go);
							resultAction = () =>
							{	var sel = adapter.SELECTED_GAMEOBJECTS().Where(enableCap).ToArray();
							
								if ( sel.Length == 0 ) return;
								
								CommonAction_Hierarchy( adapter, item, sel.Select( s => s.go ).ToArray() );
							};
							
							RegistratedKeys.Add( resultKeyCode, resultAction );
						}
					}
					
#endif
#if PROJECT
					
					if ( adapter.pluginID == Initializator.PROJECT_ID )
					{	var item = Activator.CreateInstance(type1) as ProjectExtensions.IGenericMenu;
						Project_ExensionMenus.Add( item );
						
						if ( TryAddToHotKeys( adapter, item.Name, ref resultKeyCode ) )
						{	Action resultAction = null;
						
							Func<HierarchyObject, bool> enableCap = (inputObject) =>
																	item.IsEnable(inputObject.project.assetPath, inputObject.project.guid, inputObject.id, inputObject.project.IsFolder, inputObject.project.IsMainAsset)
																	&& !item.NeedExcludeFromMenu(inputObject.project.assetPath, inputObject.project.guid, inputObject.id, inputObject.project.IsFolder, inputObject.project.IsMainAsset);
							resultAction = () =>
							{	var sel = adapter.SELECTED_GAMEOBJECTS().Where(enableCap).ToArray();
							
								if ( sel.Length == 0 ) return;
								
								CommonAction_Project( adapter, item, sel );
							};
							
							RegistratedKeys.Add( resultKeyCode, resultAction );
						}
					}
					
#endif
					
					
					
					
				}
				
#pragma warning restore
				
				adapter.m1 = null;
				adapter.m2 = null;
				adapter.m3 = null;
				
				if ( !Adapter.LITE )     //  type = PROInterface;
				{
#if HIERARCHY
				
					if ( adapter.IS_HIERARCHY() ) type = typeof( CustomModule );
					
#endif
					
#if PROJECT
					
					if ( adapter.IS_PROJECT() ) type = typeof( EProjectInternal.CustomModule );
					
#endif
					// var exclude = new[] { type.FullName, typeof(CustomModule_Slot1).FullName, typeof(CustomModule_Slot2).FullName, typeof(CustomModule_Slot3).FullName };
					
					// Debug.Log(type + " " + M_CustomModuleParser);
					
					var candidates = types.Where(p => p != null && type.IsAssignableFrom(p)).ToArray();
					//  Debug.Log(adapter.modulesInit);
					
					if ( adapter.IS_HIERARCHY() )
					{
#if HIERARCHY
						adapter.m1 = candidates.FirstOrDefault( c => typeof( HierarchyExtensions.CustomModule_Slot1 ).IsAssignableFrom( c ) && typeof( HierarchyExtensions.CustomModule_Slot1 ).FullName != c.FullName );
						adapter.m2 = candidates.FirstOrDefault( c => typeof( HierarchyExtensions.CustomModule_Slot2 ).IsAssignableFrom( c ) && typeof( HierarchyExtensions.CustomModule_Slot2 ).FullName != c.FullName );
						adapter.m3 = candidates.FirstOrDefault( c => typeof( HierarchyExtensions.CustomModule_Slot3 ).IsAssignableFrom( c ) && typeof( HierarchyExtensions.CustomModule_Slot3 ).FullName != c.FullName );
						adapter.m4 = candidates.FirstOrDefault( c => typeof( HierarchyExtensions.CustomModule_Slot4 ).IsAssignableFrom( c ) && typeof( HierarchyExtensions.CustomModule_Slot4 ).FullName != c.FullName );
						adapter.m5 = candidates.FirstOrDefault( c => typeof( HierarchyExtensions.CustomModule_Slot5 ).IsAssignableFrom( c ) && typeof( HierarchyExtensions.CustomModule_Slot5 ).FullName != c.FullName );
						adapter.m6 = candidates.FirstOrDefault( c => typeof( HierarchyExtensions.CustomModule_Slot6 ).IsAssignableFrom( c ) && typeof( HierarchyExtensions.CustomModule_Slot6 ).FullName != c.FullName );
						adapter.m7 = candidates.FirstOrDefault( c => typeof( HierarchyExtensions.CustomModule_Slot7 ).IsAssignableFrom( c ) && typeof( HierarchyExtensions.CustomModule_Slot7 ).FullName != c.FullName );
						adapter.m8 = candidates.FirstOrDefault( c => typeof( HierarchyExtensions.CustomModule_Slot8 ).IsAssignableFrom( c ) && typeof( HierarchyExtensions.CustomModule_Slot8 ).FullName != c.FullName );
						adapter.m9 = candidates.FirstOrDefault( c => typeof( HierarchyExtensions.CustomModule_Slot9 ).IsAssignableFrom( c ) && typeof( HierarchyExtensions.CustomModule_Slot9 ).FullName != c.FullName );
#endif
					}
					
					else
					{
#if PROJECT
						adapter.m1 = candidates.FirstOrDefault( c => typeof( ProjectExtensions.CustomModule_Slot1 ).IsAssignableFrom( c ) && typeof( ProjectExtensions.CustomModule_Slot1 ).FullName != c.FullName );
						adapter.m2 = candidates.FirstOrDefault( c => typeof( ProjectExtensions.CustomModule_Slot2 ).IsAssignableFrom( c ) && typeof( ProjectExtensions.CustomModule_Slot2 ).FullName != c.FullName );
						adapter.m3 = candidates.FirstOrDefault( c => typeof( ProjectExtensions.CustomModule_Slot3 ).IsAssignableFrom( c ) && typeof( ProjectExtensions.CustomModule_Slot3 ).FullName != c.FullName );
						adapter.m4 = candidates.FirstOrDefault( c => typeof( ProjectExtensions.CustomModule_Slot4 ).IsAssignableFrom( c ) && typeof( ProjectExtensions.CustomModule_Slot4 ).FullName != c.FullName );
						adapter.m5 = candidates.FirstOrDefault( c => typeof( ProjectExtensions.CustomModule_Slot5 ).IsAssignableFrom( c ) && typeof( ProjectExtensions.CustomModule_Slot5 ).FullName != c.FullName );
						adapter.m6 = candidates.FirstOrDefault( c => typeof( ProjectExtensions.CustomModule_Slot6 ).IsAssignableFrom( c ) && typeof( ProjectExtensions.CustomModule_Slot6 ).FullName != c.FullName );
						adapter.m7 = candidates.FirstOrDefault( c => typeof( ProjectExtensions.CustomModule_Slot7 ).IsAssignableFrom( c ) && typeof( ProjectExtensions.CustomModule_Slot7 ).FullName != c.FullName );
						adapter.m8 = candidates.FirstOrDefault( c => typeof( ProjectExtensions.CustomModule_Slot8 ).IsAssignableFrom( c ) && typeof( ProjectExtensions.CustomModule_Slot8 ).FullName != c.FullName );
						adapter.m9 = candidates.FirstOrDefault( c => typeof( ProjectExtensions.CustomModule_Slot9 ).IsAssignableFrom( c ) && typeof( ProjectExtensions.CustomModule_Slot9 ).FullName != c.FullName );
#endif
					}
					
					//Debug.Log(candidates[0].FullName + " " + typeof(CustomModule_Slot1).FullName + " " + typeof(CustomModule_Slot1).IsAssignableFrom(candidates[0]));
					
					if ( adapter.wasModulesInitialize )
					{	var mds = adapter.modules.Where(m => M_CustomModuleParser.IsAssignableFrom(m.GetType())).ToArray();
					
						if ( mds.Length >= 9 )
						{	if ( adapter.IS_HIERARCHY() )
							{
#if HIERARCHY
							
								if ( adapter.m1 != null ) ((Module)mds[0]).SetCustomModule( Activator.CreateInstance( adapter.m1 ) as CustomModule );
								
								if ( adapter.m2 != null ) ((Module)mds[1]).SetCustomModule( Activator.CreateInstance( adapter.m2 ) as CustomModule );
								
								if ( adapter.m3 != null ) ((Module)mds[2]).SetCustomModule( Activator.CreateInstance( adapter.m3 ) as CustomModule );
								
								if ( adapter.m4 != null ) ((Module)mds[3]).SetCustomModule( Activator.CreateInstance( adapter.m4 ) as CustomModule );
								
								if ( adapter.m5 != null ) ((Module)mds[4]).SetCustomModule( Activator.CreateInstance( adapter.m5 ) as CustomModule );
								
								if ( adapter.m6 != null ) ((Module)mds[5]).SetCustomModule( Activator.CreateInstance( adapter.m6 ) as CustomModule );
								
								if ( adapter.m7 != null ) ((Module)mds[6]).SetCustomModule( Activator.CreateInstance( adapter.m7 ) as CustomModule );
								
								if ( adapter.m8 != null ) ((Module)mds[7]).SetCustomModule( Activator.CreateInstance( adapter.m8 ) as CustomModule );
								
								if ( adapter.m9 != null ) ((Module)mds[8]).SetCustomModule( Activator.CreateInstance( adapter.m9 ) as CustomModule );
								
#endif
							}
							
							else
							{
#if PROJECT
							
								if ( adapter.m1 != null ) ((Module)mds[0]).SetCustomModule( Activator.CreateInstance( adapter.m1 ) as EProjectInternal.CustomModule );
								
								if ( adapter.m2 != null ) ((Module)mds[1]).SetCustomModule( Activator.CreateInstance( adapter.m2 ) as EProjectInternal.CustomModule );
								
								if ( adapter.m3 != null ) ((Module)mds[2]).SetCustomModule( Activator.CreateInstance( adapter.m3 ) as EProjectInternal.CustomModule );
								
								if ( adapter.m4 != null ) ((Module)mds[3]).SetCustomModule( Activator.CreateInstance( adapter.m4 ) as EProjectInternal.CustomModule );
								
								if ( adapter.m5 != null ) ((Module)mds[4]).SetCustomModule( Activator.CreateInstance( adapter.m5 ) as EProjectInternal.CustomModule );
								
								if ( adapter.m6 != null ) ((Module)mds[5]).SetCustomModule( Activator.CreateInstance( adapter.m6 ) as EProjectInternal.CustomModule );
								
								if ( adapter.m7 != null ) ((Module)mds[6]).SetCustomModule( Activator.CreateInstance( adapter.m7 ) as EProjectInternal.CustomModule );
								
								if ( adapter.m8 != null ) ((Module)mds[7]).SetCustomModule( Activator.CreateInstance( adapter.m8 ) as EProjectInternal.CustomModule );
								
								if ( adapter.m9 != null ) ((Module)mds[8]).SetCustomModule( Activator.CreateInstance( adapter.m9 ) as EProjectInternal.CustomModule );
								
#endif
							}
							
							
							
							
						}
						
						
						
					}
				}
			}
			
			
			
			//WINDOW_expandMethod = adapter.SceneHierarchyWindow.GetMethod( "ExpandTreeViewItem" , (BindingFlags)(-1) );
			/* WINDOW_treeController = adapter.SceneHierarchyWindow.GetField( adapter.pluginID == Initializator.HIERARCHY_ID ? "m_treeView" : "m_AssetTree", (BindingFlags)(-1) );
			
			 //CONTROLLER_Data = WINDOW_treeController.FieldType.GetMethod( "get_data" , (BindingFlags)(-1) );
			
			 wasMethod = WINDOW_treeController != null;*/
			
			
			
			
			
		}
		internal Rect SHRINK( Rect R, float v )
		{	R.x += v - 2;
			R.y += v;
			R.width -= v * 2;
			R.height -= v * 2;
			return R;
		}
		
		bool wasMethod = false;
		
		//	internal MethodInfo WINDOW_expandMethod = null;
		// FieldInfo WINDOW_treeController = null;
		
		//MethodInfo CONTROLLER_Data = null;
		//  GUIContent menuToolTip = new GUIContent() {tooltip = ""};//Custom Menu
		Rect r;
		Rect? lastExtMenRect;
		static string textureName = "FAVORIT_LIST_ICON ON"; //"NULL"
		internal void DrawExtensionMenu( Adapter adapter, Adapter.HierarchyObject o, Rect drawRect )
		{
		
		
#if HIERARCHY
		
			if ( adapter.pluginID == Initializator.HIERARCHY_ID )
				if ( Hierarchy_ExensionMenus.Count == 0 ) return;
				
#endif
#if PROJECT
				
			if ( adapter.pluginID == Initializator.PROJECT_ID )
				if ( Project_ExensionMenus.Count == 0 ) return;
				
#endif
			//  var o = _o;
			//if (o.transform.childCount == 0) return;
			drawRect.x -= 16;
			drawRect.width = 16;
			drawRect.x += adapter.TOTAL_LEFT_PADDING;
			
			
			if ( o.Active() /*&& o.transform.childCount == 0*/ && o.ParentIsNull() )
			{	r = drawRect;
				r.y += (drawRect.height - EditorGUIUtility.singleLineHeight) / 2;
				// var oldS = r.height;
				var oldS = EditorGUIUtility.singleLineHeight;
				r.width = r.height = 9;
				r.x += (oldS - r.width) / 2;
				r.y += (oldS - r.width) / 2;
				//GUI.DrawTexture( r , adapter.GetIcon( "UNLOCK" ) );
				
				
			}
			
#if USE_LASTRECT
			
			if (Event.current.button == 1 && (Event.current.type == EventType.MouseUp || Event.current.type == EventType.ContextClick ) && ! lastExtMenRect.HasValue
					&& drawRect.Contains( Event.current.mousePosition ) )
			{	lastExtMenRect = new Rect(drawRect.x, drawRect.y, drawRect.height, drawRect.height);
				adapter.window().Focus();
			}
			
			var CONTAINS = lastExtMenRect.HasValue && lastExtMenRect.Value.Contains(Event.current.mousePosition);
			
			//if (Event.current.rawType == EventType.MouseUp && lastExtMenRect.HasValue )lastExtMenRect = null;
			
			if (!drawRect.Contains( Event.current.mousePosition ) || Event.current.button != 1) return;
			
			
			
			if (Event.current.button == 1 && lastExtMenRect.HasValue)
			{	var oc = GUI.color;
				GUI.color *= Color.red;
				drawRect.width = drawRect.height;
				GUI.DrawTexture(lastExtMenRect.Value,  adapter.GetIcon("NULL"));
				GUI.color = oc;
				//  GUI.DrawTexture(drawRect,  adapter.GetIcon("HIPERUI_BUTTONGLOW"));
			}
			
			// Debug.Log(Event.current.type + "  " +  CONTAINS + " " + drawRect.ToString() + " " + (lastExtMenRect.HasValue ? lastExtMenRect.Value.ToString() : "null") + " " + Event.current.mousePosition);
			
			if (Event.current.button == 1 && !lastExtMenRect.HasValue && (Event.current.type == EventType.Repaint ) && drawRect.Contains( Event.current.mousePosition ))
			{	var oc = GUI.color;
				GUI.color *= Color.red;
				drawRect.width = drawRect.height;
				GUI.DrawTexture(drawRect,  adapter.GetIcon("NULL"));
				GUI.color = oc;
				//  GUI.DrawTexture(drawRect,  adapter.GetIcon("HIPERUI_BUTTONGLOW"));
			}
			
#else
			
			
			var CONTAINS = drawRect.Contains( Event.current.mousePosition );
			var SIZE = EditorGUIUtility.singleLineHeight ;
			drawRect.x = drawRect.x + drawRect.width / 2 - SIZE / 2;
			drawRect.y = drawRect.y + drawRect.height / 2 - SIZE / 2;
			drawRect.height = drawRect.width = SIZE;
			drawRect.x += 2;
			drawRect.y += 1;
			
			//GUI.Label( drawRect, menuToolTip );
			
			if ( Event.current.button == 1 && (Event.current.type == EventType.MouseUp || Event.current.type == EventType.ContextClick) && !lastExtMenRect.HasValue && CONTAINS )
			{	lastExtMenRect = new Rect( drawRect.x, drawRect.y, drawRect.height, drawRect.height );
			
				if (Adapter.UNITY_CURRENT_VERSION < Adapter.UNITY_2019_1_1_VERSION) adapter.window().Focus();
			}
			
			if ( lastExtMenRect.HasValue && Event.current.button != 1 ) lastExtMenRect = null;
			
			if ( (!CONTAINS &&
					(!lastExtMenRect.HasValue ||
					 !lastExtMenRect.Value.Contains( Event.current.mousePosition )
					)
				 )
					||
					Event.current.button != 1 ) return;
			
			
			/*  drawRect.x -= 30;
			  drawRect.width += 60;*/
			if ( CONTAINS )
			{	//  var oc = GUI.color;
				//      GUI.color *= Color.red;
			
			
				GUI.DrawTexture( SHRINK( drawRect, 0 ), adapter.GetIcon( textureName ) );
				// GUI.color = oc;
				//  GUI.DrawTexture(drawRect,  adapter.GetIcon("HIPERUI_BUTTONGLOW"));
			}
			
			else
				if ( lastExtMenRect.HasValue )
				{	// var oc = GUI.color;
					//     GUI.color *= Color.red;
					//     drawRect.width = drawRect.height;
					GUI.DrawTexture( SHRINK( lastExtMenRect.Value, 0 ), adapter.GetIcon( textureName ) );
					//GUI.color = oc;
					//  GUI.DrawTexture(drawRect,  adapter.GetIcon("HIPERUI_BUTTONGLOW"));
				}
			
#endif
			
			/*if (Event.current.type != EventType.Repaint && Event.current.type != EventType.Layout)
			{   Debug.Log(Event.current.type + "  " + isExpanded + " "  + CONTAINS + " " + (lastExtMenRect.HasValue ? lastExtMenRect.Value.ToString() : "null") + " " + Event.current.mousePosition);
			}*/
			/*if ( Event.current.type != EventType.Repaint && Event.current.type != EventType.Layout )
			{   Debug.Log( Event.current.type );
			}*/
			if ( (Event.current.type == EventType.MouseUp || Event.current.type == EventType.Used || Event.current.type == EventType.ContextClick) && CONTAINS && lastExtMenRect.HasValue )
			
				// if (((Event.current.type == EventType.ContextClick || Event.current.type == EventType.Used) && isExpanded || Event.current.type == EventType.MouseDown && !isExpanded))
			{	// if (treeItems == null) treeItems = new List<TreeViewItem>();
				//  MonoBehaviour.print(wasMethod);
				if ( !wasMethod ) InitExtensionMenu( adapter );
				
				// wasMouseDown = true;
				
				
				adapter.RepaintWindow(true);
				/* if (wasMethod && isExpanded && Event.current.type == EventType.Used)
				 {
				
				     // var tree = WINDOW_treeController.GetValue(adapter.window());
				     var tree = adapter.m_TreeView(adapter.window());
				     var data = adapter.m_data.GetValue(tree, null);
				
				
				     var face = data.GetType().GetInterfaces().FirstOrDefault(i => i.Name.Contains("ITreeViewDataSource"));
				     if (face != null)
				     {   var f = data.GetType().GetInterfaceMap(face).InterfaceMethods;
				         var DATA_GetExpandedIDs = f.First(m => m.Name == "GetExpandedIDs");
				         var expandedIds = DATA_GetExpandedIDs.Invoke(data, null) as int[];
				         var expand = expandedIds.Contains(o.id);
				         adapter.m_dataSetExpanded.Invoke( data, new object[] { o.id, expand } );
				     }
				 }*/
				
				
#if HIERARCHY
				
				if ( adapter.pluginID == Initializator.HIERARCHY_ID )
				{	var sorted = Hierarchy_ExensionMenus.OrderBy(e => e.PositionInMenu).ToArray();
					var curerntPos = int.MinValue;
					
					var menu = new GenericMenu();
					
					for ( int i = 0 ; i < sorted.Length ; i++ )
					{
					
						if ( sorted[i].NeedExcludeFromMenu( o.go ) ) continue;
						
						if ( i > 0 && Math.Abs( sorted[i].PositionInMenu - curerntPos ) > 1 )     //MonoBehaviour.print(sorted[i].Name);
						{	menu.AddSeparator( "" );
						}
						
						var item = sorted[i];
						var name = GetName_Hierarchy(item);
						
						if ( !sorted[i].IsEnable( o.go ) )
						{	menu.AddDisabledItem( new GUIContent( name ) );
						}
						
						else
						{	var undoName = name;
							Func<HierarchyObject, bool> enableCap = (inputObject) => item.IsEnable(inputObject.go) && !item.NeedExcludeFromMenu(inputObject.go);
							
							menu.AddItem( new GUIContent( name ), false, () =>
							{	var sel = adapter.SELECTED_GAMEOBJECTS().Where(enableCap).ToList();
							
								if ( !sel.Contains( o ) )
								{	Undo.RecordObject( o.go, undoName );
									item.OnClick( new[] { o.go } );
									
									if ( o.go )
									{	Adapter.SetDirty( o.go );
										adapter.MarkSceneDirty( o.scene );
									}
									
									//adapter.RepaintWindow();
								}
								
								else
								{	sel.Remove( o );
								
									if ( sel.Count == 0 ) sel.Add( o );
									else sel.Insert( 0, o );
									
									CommonAction_Hierarchy( adapter, item, sel.Select( s => s.go ).ToArray() );
								}
								
								adapter.window().Focus();
								adapter.RepaintWindow(true);
							} );
						}
						
						curerntPos = sorted[i].PositionInMenu;
					}
					
					menu.ShowAsContext();
					
				}
				
#endif
				
#if PROJECT
				
				if ( adapter.pluginID == Initializator.PROJECT_ID )
				{	var sorted = Project_ExensionMenus.OrderBy(e => e.PositionInMenu).ToArray();
					var curerntPos = int.MinValue;
					
					var menu = new GenericMenu();
					
					
					
					if ( !o.project.IsFolder ) menu.AddDisabledItem( new GUIContent( "Select children by type" ) );
					else
					{	var L = UNITY_SYSTEM_PATH.Length;
						var types = Directory.GetFiles( UNITY_SYSTEM_PATH + o.project.assetPath, "*.*", SearchOption.AllDirectories )
									.Where(p => !p.EndsWith(".meta"))
									.GroupBy( f => f.LastIndexOf( '.' ) == -1 ? "" : f.Substring( f.LastIndexOf( '.' ) ) )
									.OrderBy(g => g.Key);
									
						foreach ( var t in types )
						{	var captureTypes = t.ToArray();
							menu.AddItem( new GUIContent( "Select children by type/" + t.Key ), false, () =>
							{	Selection.objects = captureTypes.Select( p => AssetDatabase.LoadMainAssetAtPath( p.Substring( L ) ) ).ToArray();
							} );
						}
					}
					
					menu.AddSeparator( "" );
					
					
					
					
					
					
					
					
					
					for ( int i = 0 ; i < sorted.Length ; i++ )
					{
					
						if ( sorted[i].NeedExcludeFromMenu( o.project.assetPath, o.project.guid, o.id, o.project.IsFolder, o.project.IsMainAsset ) ) continue;
						
						if ( i > 0 && Math.Abs( sorted[i].PositionInMenu - curerntPos ) > 1 )     //MonoBehaviour.print(sorted[i].Name);
						{	menu.AddSeparator( "" );
						}
						
						var item = sorted[i];
						var name = GetName_Project(item);
						
						if ( !sorted[i].IsEnable( o.project.assetPath, o.project.guid, o.id, o.project.IsFolder, o.project.IsMainAsset ) )
						{	menu.AddDisabledItem( new GUIContent( name ) );
						}
						
						else
						{	var undoName = name;
							Func<HierarchyObject, bool> enableCap = (inputObject) =>
																	item.IsEnable(inputObject.project.assetPath, inputObject.project.guid, inputObject.id, inputObject.project.IsFolder, inputObject.project.IsMainAsset)
																	&& !item.NeedExcludeFromMenu(inputObject.project.assetPath, inputObject.project.guid, inputObject.id, inputObject.project.IsFolder, inputObject.project.IsMainAsset);
																	
							menu.AddItem( new GUIContent( name ), false, () =>
							{	var sel = adapter.SELECTED_GAMEOBJECTS().Where(enableCap).ToList();
							
								if ( !sel.Contains( o ) )
								{	if ( o.go ) Undo.RecordObject( o.go, undoName );
								
									item.OnClick( new[] { o.project.assetPath }, new[] { o.project.guid }, new[] { o.id }, new[] { o.project.IsFolder }, new[] { o.project.IsMainAsset } );
									
									if ( o.go )
									{	Adapter.SetDirty( o.go );
									}
									
									// adapter.RepaintWindow();
								}
								
								else
								{	sel.Remove( o );
								
									if ( sel.Count == 0 ) sel.Add( o );
									else sel.Insert( 0, o );
									
									CommonAction_Project( adapter, item, sel.ToArray() );
								}
								
								adapter.window().Focus();
								adapter.RepaintWindow(true);
								
							} );
						}
						
						curerntPos = sorted[i].PositionInMenu;
					}
					
					menu.ShowAsContext();
					
				}
				
#endif
				
				
				Adapter.EventUse();
				lastExtMenRect = null;
				EditorGUIUtility.hotControl = 0;
				
				//window().Repaint();
			}
			
			else
			
				if ( Event.current.type == EventType.MouseDown || Event.current.type == EventType.Used || Event.current.type == EventType.ContextClick )
				{	lastExtMenRect = new Rect( drawRect.x, drawRect.y, drawRect.height, drawRect.height );
					EventUse();
					adapter.window().Focus();
					adapter.RepaintWindow(true);
					EditorGUIUtility.hotControl = 0;
				}
				
			// Event.current.
			//  MonoBehaviour.print("ASD");
			//  if (Event.current.isMouse) EventUse();
			
		}
		
		
#if HIERARCHY
		
		string GetName_Hierarchy( HierarchyExtensions.IGenericMenu item )
		{
		
			return string.IsNullOrEmpty( item.Name ) ? "- Unidentified -" : item.Name.Replace( "$", "" );
		}
		
		void CommonAction_Hierarchy( Adapter adapter, HierarchyExtensions.IGenericMenu item, GameObject[] sel )
		{	foreach ( var objectToUndo in sel )
				Undo.RecordObject( objectToUndo, GetName_Hierarchy( item ) );
				
			item.OnClick( sel );
			
			foreach ( var objectToUndo in sel )
				if ( objectToUndo )
				{	Adapter.SetDirty( objectToUndo );
					adapter.MarkSceneDirty( objectToUndo.scene );
				}
				
			adapter.RepaintWindow(true);
		}
#endif
		
#if PROJECT
		string GetName_Project( ProjectExtensions.IGenericMenu item )
		{	return string.IsNullOrEmpty( item.Name ) ? "- Unidentified -" : item.Name.Replace( "$", "" );
		}
		
		void CommonAction_Project( Adapter adapter, ProjectExtensions.IGenericMenu item, Adapter.HierarchyObject[] sel )
		{	foreach ( var objectToUndo in sel )
				if ( objectToUndo.go ) Undo.RecordObject( objectToUndo.go, GetName_Project( item ) );
				
			item.OnClick( sel.Select( s => s.project.assetPath ).ToArray(), sel.Select( s => s.project.guid ).ToArray(), sel.Select( s => s.id ).ToArray(), sel.Select( s => s.project.IsFolder ).ToArray()
						  , sel.Select( s => s.project.IsMainAsset ).ToArray() );
						  
			foreach ( var objectToUndo in sel )
				if ( objectToUndo.go )
					Adapter.SetDirty( objectToUndo.go );
					
			adapter.RepaintWindow(true);
		}
#endif
		
		
		/*   internal GameObject[] SELECTED_GAMEOBJECTS( )
		{
		return /* Selection.gameObjects == null ? new GameObject[0] :#1# Selection.gameObjects.Where( isSceneObject ).ToArray();
		}
		
		internal GameObject activeGameObject( )
		{
		return isSceneObject( Selection.activeGameObject ) ? Selection.activeGameObject : null;
		}
		
		bool isSceneObject( GameObject o )
		{
		return o && o.scene.IsValid();
		}*/
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		KeyKodeData tempKey;
		
		internal Dictionary<KeyKodeData, Action> RegistratedKeys = new Dictionary<KeyKodeData, Action>();
		
		
		bool TryAddToHotKeys( Adapter adapter, string name, ref KeyKodeData resultKeyCode )
		{	name = name.Trim();
		
			if ( string.IsNullOrEmpty( name ) ) return false;
			
			var split = name.Split(' ');
			
			if ( split.Length < 2 ) return false;
			
			var last = split[split.Length - 1].ToUpper();
			
			if ( string.IsNullOrEmpty( last ) ) return false;
			
			// char key = last[last.Length - 1];
			string key = last.Trim(new [] {'#', '%', '&', '_'});
			
			if ( !avaliableKeys.ContainsKey( key ) ) return false;
			
			bool ctrl = last.Contains('%');
			bool shift = last.Contains('#');
			bool alt = last.Contains('&');
			var keyCode = avaliableKeys[key];
			
			// if (!ctrl && !shift && !alt && !last.Contains( '_' )) return false;
			
			resultKeyCode = new KeyKodeData( ctrl, shift, alt, keyCode );
			
			if ( RegistratedKeys.ContainsKey( resultKeyCode ) ) return false;
			
			return true;
		}
		
		
		internal void CheckKeyDown( bool ctrl, bool shift, bool alt, KeyCode keyCode )
		{	tempKey.ctrl = ctrl;
			tempKey.shift = shift;
			tempKey.alt = alt;
			tempKey.keyCode = (int)keyCode;
			
			if ( RegistratedKeys.ContainsKey( tempKey ) && RegistratedKeys[tempKey] != null )
			{	RegistratedKeys[tempKey]();
				Adapter.EventUseFast();
				EventUse();
			}
		}
		Dictionary<string, KeyCode> avaliableKeys = new Dictionary<string, KeyCode>()
		{
			{ "1", KeyCode.Alpha1 },
			{ "2", KeyCode.Alpha2 },
			{ "3", KeyCode.Alpha3 },
			{ "4", KeyCode.Alpha4 },
			{ "5", KeyCode.Alpha5 },
			{ "6", KeyCode.Alpha6 },
			{ "7", KeyCode.Alpha7 },
			{ "8", KeyCode.Alpha8 },
			{ "9", KeyCode.Alpha9 },
			{ "0", KeyCode.Alpha0 },
			{ "Q", KeyCode.Q },
			{ "W", KeyCode.W },
			{ "E", KeyCode.E },
			{ "R", KeyCode.R },
			{ "T", KeyCode.T },
			{ "Y", KeyCode.Y },
			{ "U", KeyCode.U },
			{ "I", KeyCode.I },
			{ "O", KeyCode.O },
			{ "P", KeyCode.P },
			{ "A", KeyCode.A },
			{ "S", KeyCode.S },
			{ "D", KeyCode.D },
			{ "F", KeyCode.F },
			{ "G", KeyCode.G },
			{ "H", KeyCode.H },
			{ "J", KeyCode.J },
			{ "K", KeyCode.K },
			{ "L", KeyCode.L },
			{ "Z", KeyCode.Z },
			{ "X", KeyCode.X },
			{ "C", KeyCode.C },
			{ "V", KeyCode.V },
			{ "B", KeyCode.B },
			{ "N", KeyCode.N },
			{ "M", KeyCode.M },
			{ ",", KeyCode.Comma },
			{ ".", KeyCode.Period },
			{ "/", KeyCode.Slash },
			{ ";", KeyCode.Semicolon },
			{ "'", KeyCode.Quote },
			{ "[", KeyCode.LeftBracket },
			{ "]", KeyCode.RightBracket },
			{ "-", KeyCode.Minus },
			{ "=", KeyCode.Equals },
			{ "\\", KeyCode.Backslash},
			{ "HOME", KeyCode.Home },
			{ "END", KeyCode.End },
			{ "PAGEUP", KeyCode.PageUp },
			{ "PAGEDOWN", KeyCode.PageDown },
			{ "UP", KeyCode.UpArrow },
			{ "LEFT", KeyCode.LeftArrow },
			{ "DOWN", KeyCode.DownArrow },
			{ "RIGHT", KeyCode.RightArrow },
			{ "RETURN", KeyCode.Return },
		};
		
		
		internal struct KeyKodeData : IEqualityComparer<KeyKodeData>, IEquatable<KeyKodeData>, IComparable<KeyKodeData>
		{	internal KeyKodeData( bool ctrl, bool shift, bool alt, KeyCode keyCode )
			{	this.ctrl = ctrl;
				this.shift = shift;
				this.alt = alt;
				this.keyCode = (int)keyCode;
			}
			
			internal bool ctrl;
			internal bool shift;
			internal bool alt;
			internal int keyCode;
			
			
			public int CompareTo( KeyKodeData other )
			{	return GetHashCode() - other.GetHashCode();
			}
			
			public int Compare( KeyKodeData x, KeyKodeData y )
			{	return x.GetHashCode() - y.GetHashCode();
			}
			
			public int GetHashCode( KeyKodeData obj )
			{	return obj.GetHashCode();
			}
			
			public override bool Equals( object obj )
			{	return Equals( (KeyKodeData)obj );
			}
			
			public bool Equals( KeyKodeData x, KeyKodeData y )
			{	return x.GetHashCode() == y.GetHashCode();
			}
			
			public bool Equals( KeyKodeData other )
			{	return GetHashCode() == other.GetHashCode();
			}
			public override int GetHashCode()
			{	var result = keyCode;
			
				if ( ctrl ) result += 10000000;
				
				if ( shift ) result += 20000000;
				
				if ( alt ) result += 40000000;
				
				return result;
			}
			
			
			
		}
		
		
		
		
	}
}
}
