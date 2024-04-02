//#define USE_CACHE__


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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



	public class ReflType {
		public   bool isprop;
		public FieldInfo field;
		public PropertyInfo prop;
		public ReflType( FieldInfo f, PropertyInfo p )
		{	this.field = f;
			this.prop = p;
			this.isprop = p != null;
		}
		public ReflType( Type type, string key )
		{	this.field = type.GetField( key, (BindingFlags)(-1) );
		
			if ( this.field == null ) this.prop = type.GetProperty( key, (BindingFlags)(-1) );
			
			this.isprop = this.prop != null;
		}
		public object GetValue( object target )
		{	return isprop ? prop.GetValue( target, null ) : field.GetValue( target );
		}
		
		public void SetValue( object target, object args )
		{	if ( isprop ) prop.SetValue( target, args, null );
			else field.SetValue( target, args );
		}
	}
	
	internal PropertyInfo m_data;
	internal PropertyInfo m_gui;
	//internal MethodInfo EnsureFullyInitialized;
	
	internal static Type UnityEventArgsType;
	// private static FieldInfo extraInsertionMarkerIndent;
	internal  FieldInfo k_BottomRowMargin;
	//  static PropertyInfo extraSpaceBeforeIconAndLabel;
	ReflType k_LineHeight;
	//internal FieldInfo f_k_LineHeight;
	
	internal FieldInfo foldoutYOffset;
	internal FieldInfo s_GOStyles;
	internal FieldInfo sceneHeaderBg;
	internal List<FieldInfo> treestyles = new List<FieldInfo>();
	internal FieldInfo lineStyle;
	internal FieldInfo lineBoldStyle;
	internal PropertyInfo alignment;
	internal PropertyInfo paddingProp;
	internal FieldInfo m_Ping;
	internal FieldInfo m_PingStyle;
	internal PropertyInfo fixedHeight;
	internal PropertyInfo guiProp;
	// internal MethodInfo GetTotalSize;
	internal FieldInfo k_IconWidth;
	internal PropertyInfo   iconOverlayGUI ;
	internal PropertyInfo   labelOverlayGUI ;
	internal bool haslabelOverlayGUI = false;
	internal PropertyInfo   overlayIcon ;
	internal bool   HasoverlayIcon ;
	internal FieldInfo k_IndentWidth;
	internal FieldInfo k_BaseIndent;
	internal FieldInfo m_UseHorizontalScroll;
	internal  PropertyInfo showingPrefabHeader;
	internal bool hasShowingPrefabHeader;
	
	internal  PropertyInfo hoveredItem;
	internal bool hashoveredItem;
	
	
	internal FieldInfo strange_field;
	internal FieldInfo m_KeyboardControlIDField;
	internal FieldInfo scrollPosField;
	internal FieldInfo m_SearchFilterString;
	internal MethodInfo m_SearchFilterClass_Has;
	// internal FieldInfo m_SearchFilterClass_String;
	internal FieldInfo m_TotalRect;
	internal FieldInfo m_VisibleRect;
	internal FieldInfo m_Pos;
	internal FieldInfo s_Style;
	// internal FieldInfo m_FramingAnimFloat;
	internal FieldInfo m_UseExpansionAnimation;
	internal MethodInfo frame_method;
	
	internal MethodInfo m_IsExpanded;
	
	internal MethodInfo GetInstanceIDFromGUID;
	internal MethodInfo GetMainAssetInstanceIDFromPath;
	
	//  FieldInfo m_TreeView;
	FieldInfo __m_TreeView;
	FieldInfo __m_FoldView;
	FieldInfo __m_AssetsView;
	FieldInfo m_ViewMode;
	/* FieldInfo s_LastInteractedHierarchy;
	 bool has_s_LastInteractedHierarchy;*/
	//   internal   Dictionary<object, Dictionary<int, UnityEditor.IMGUI.Controls.TreeViewItem>> __ti = new Dictionary<object, Dictionary<int, UnityEditor.IMGUI.Controls.TreeViewItem>>();
	//  internal static  Dictionary<object, UnityEditor.IMGUI.Controls.TreeViewItem> __ti = new Dictionary<object, UnityEditor.IMGUI.Controls.TreeViewItem>();
	// internal int TreeReloaderID;
	internal void ClearTree(bool force = false)
	{	// Debug.Log( "ASD" );
		// if (!force && NEW_PERFOMANCE) return;
		tree_cache.Clear();
		//
		//TreeReloaderID++;
		//__ti.Clear();
	}
	
#if USE_CACHE__
	Dictionary<EditorWindow, object> tree_cache = new Dictionary<EditorWindow, object>();
	object resres; //mis one frame
	internal object m_TreeView( EditorWindow window )
	{	if ( tree_cache.ContainsKey(window)) return tree_cache[window];
	
		var b = bakem_TreeView( window );
		tree_cache.Add( window, b );
		return b;
	}
	
	//  object _asd_m_TreeView;
	internal object bakem_TreeView( EditorWindow window )
	{	if ( UseRootWindow ) return m_TreeViewFieldInfo.GetValue( window ) ;
	
		var sub = m_SceneHierarchy.GetValue(window);
		return m_TreeViewFieldInfo.GetValue( sub ) ;
#else
	Dictionary<EditorWindow, object> tree_cache = new Dictionary<EditorWindow, object>();
	object resres; //mis one frame
	internal object m_TreeView( EditorWindow window )
	{	if ( UseRootWindow ) return m_TreeViewFieldInfo.GetValue( window ) ;
		
		var sub = m_SceneHierarchy.GetValue(window);
		return m_TreeViewFieldInfo.GetValue( sub ) ;
#endif
		
		
		/*if ( !tree_cache.TryGetValue( window, out resres ) || resres == null )
		{   tree_cache.Remove( window );
		
		if ( UseRootWindow ) tree_cache.Add( window, resres = m_TreeViewFieldInfo.GetValue( window ) );
		else
		{   var sub = m_SceneHierarchy.GetValue(window);
		    tree_cache.Add( window, resres = m_TreeViewFieldInfo.GetValue( sub ) );
		}
		
		if ( tree_cache[window] != null )
		{   if ( m_UseExpansionAnimation != null )
		        m_UseExpansionAnimation.SetValue( tree_cache[window], false );
		}
		else
		{   tree_cache.Remove( window );
		}
		
		
		
		
		
		}
		//  if ( resres == null)     Debug.Log( resres );
		return resres;*/
	}
	internal FieldInfo m_TreeViewFieldInfo
	{	get
		{	if ( pluginID == Initializator.HIERARCHY_ID ) return __m_TreeView;
		
			if ( !window( true ) ) return __m_FoldView;
			
			if ( (int)m_ViewMode.GetValue( window( true ) ) == 1 ) return __m_FoldView;
			
			return __m_AssetsView;
			/*
			
			if ( __m_TreeView.GetValue( window() ) == null )
			{
			//MonoBehaviour.print( "init" );
			window().GetType().GetMethod( "Init" , (BindingFlags)(-1) ).Invoke( window() , null );
			}*/
		}
	}
	internal PropertyInfo m_state;
	
	Type ___SceneHierarchyWindowRoot;
	internal Type SceneHierarchyWindowRoot
	{	get
		{	if ( ___SceneHierarchyWindowRoot == null )
			{	___SceneHierarchyWindowRoot = Assembly.GetAssembly( typeof( EditorWindow ) ).GetType(
				                                  pluginID == Initializator.HIERARCHY_ID ?
				                                  "UnityEditor.SceneHierarchyWindow" :
				                                  "UnityEditor.ProjectBrowser"
				                              );
				                              
			}
			
			return ___SceneHierarchyWindowRoot;
		}
	}
	public bool UseRootWindow;
	Type ___SceneHierarchyWindow;
	public FieldInfo m_SceneHierarchy;
	internal Type SceneHierarchyWindow
	{	get
		{	if ( ___SceneHierarchyWindow == null )
			{	var ass = Assembly.GetAssembly( typeof( EditorWindow ) );
			
				if ( pluginID == Initializator.HIERARCHY_ID )
				{	___SceneHierarchyWindow = ass.GetType( "UnityEditor.SceneHierarchy" );
				
					if ( UseRootWindow = ___SceneHierarchyWindow == null ) ___SceneHierarchyWindow = ass.GetType( "UnityEditor.SceneHierarchyWindow" );
					else
					{	m_SceneHierarchy = SceneHierarchyWindowRoot.GetField( "m_SceneHierarchy", (BindingFlags)(-1) );
					}
				}
				
				else
				{	___SceneHierarchyWindow = ass.GetType( "UnityEditor.ProjectBrowser" );
					UseRootWindow = true;
				}
				
			}
			
			return ___SceneHierarchyWindow;
		}
	}
	
	bool fixget;
	object[] args = new object[1];
	string getid_path;
	internal int GET_ID_BY_GUID( ref string guid )
	{	getid_path = AssetDatabase.GUIDToAssetPath( guid );
	
		if ( string.IsNullOrEmpty( getid_path ) ) return 0;
		
		// AssetDatabase.guid()
		
		//  try
		{	if ( !fixget )
			{	args[0] = guid;
				return (int)GetInstanceIDFromGUID.Invoke( null, args );
			}
			
			else
			{	args[0] = getid_path;
				return (int)GetMainAssetInstanceIDFromPath.Invoke( null, args );
			}
		}
		/*catch Ex
		{
		
		    return 0;
		}*/
		
		
	}
	internal Color? hoveredBackgroundColor;
	internal Type gs;
	internal FieldInfo __m_AssetTreeState, __m_FolderTreeState;
	object GetValue_Field(Type type, string field )
	{	var res =    type.GetField(field, (BindingFlags)(-1) );
	
		if ( res == null ) return null;
		
		return res.GetValue( null );
	}
	void SetValue_Field(Type type, string field, object value)
	{	var res =    type.GetField(field, (BindingFlags)(-1) );
	
		if ( res == null ) return ;
		
		res.SetValue( null, value);
	}
	void FixStyle(GUIStyle style )
	{	style.fixedHeight = 0;
		style.stretchHeight = true;
		style.alignment = TextAnchor.MiddleLeft;
		style.padding.top = 0;
		style.padding.bottom = 0;
		style.margin.top = 0;
		style.overflow.top = 0;
	}
	bool TryToInitializeDefaultStylesWasInit;
	internal List<GUIStyle> LABLES = new  List<GUIStyle>();
	Color? backBG;
	void UpdateBGHOver()
	{	if (!hashoveredItem) return;
	
		var v=  GetValue_Field(gs, "hoveredBackgroundColor") ;
		
		if (v == null) return;
		
		if (!backBG.HasValue) backBG = (Color)v;
		
		
		
		if (HIDE_HOVER_BG)
		{	SetValue_Field(gs, "hoveredBackgroundColor", Color.clear);
			hoveredBackgroundColor = Color.clear;
		}
		
		else
		{	SetValue_Field(gs, "hoveredBackgroundColor", backBG.Value);
			hoveredBackgroundColor = backBG.Value;
		}
		
	}
	// GUIStyle sceneHeaderBg;
	internal void TryToInitializeDefaultStyles()
	{
	
	
	
		if ( TryToInitializeDefaultStylesWasInit ) return;
		
		TryToInitializeDefaultStylesWasInit = true;
		
		gs =  typeof( EditorWindow ).Assembly.GetType( "UnityEditor.GameObjectTreeViewGUI+GameObjectStyles" );
		
		if ( gs != null )
		{
		
		
			/*foreach ( var m in typeof( EditorWindow ).Assembly.GetTypes() )
			 {   if (m.Name.Contains( "GameObjectStyles" ) )
			         Debug.Log( m.FullName );
			
			 }*/
			/*  gs.get
			  if ( gs != null ) {*/
			//UnityEditor.GameObjectTreeViewGUI.GameObjectStyles
			LABLES.Clear();
			
			var ls =  typeof( EditorWindow ).Assembly.GetType( "UnityEditor.IMGUI.Controls.TreeViewGUI+Styles" );
			
			if (ls != null)
			{	var l =  GetValue_Field(ls, "lineStyle") as GUIStyle;
			
				if ( l != null )
				{	FixStyle( l );
					LABLES.Add(l);
				}
			}
			
			var disabledLabel = GetValue_Field( gs, "disabledLabel" ) as GUIStyle;
			
			if ( disabledLabel != null )
			{	FixStyle( disabledLabel );
				LABLES.Add(disabledLabel);
				
				if (Adapter.UNITY_CURRENT_VERSION >= Adapter.UNITY_2019_VERSION && Adapter.UNITY_CURRENT_VERSION  < Adapter.UNITY_2019_3_0_VERSION) disabledLabel.padding.bottom = 2;
			}
			
			
			var prefabLabel =  GetValue_Field(gs, "prefabLabel") as GUIStyle;
			var disabledPrefabLabel = GetValue_Field( gs, "disabledPrefabLabel") as GUIStyle;
			var brokenPrefabLabel = GetValue_Field(gs, "brokenPrefabLabel") as GUIStyle;
			var disabledBrokenPrefabLabel = GetValue_Field(gs, "disabledBrokenPrefabLabel") as GUIStyle;
			
			// sceneHeaderBg = GetValue_Field(gs, "sceneHeaderBg" ) as GUIStyle;
			if ( prefabLabel != null ) {FixStyle(prefabLabel); LABLES.Add(prefabLabel);}
			
			if ( disabledPrefabLabel != null ) { FixStyle( disabledPrefabLabel ); LABLES.Add(disabledPrefabLabel); }
			
			if ( brokenPrefabLabel != null ) { FixStyle( brokenPrefabLabel ); 	LABLES.Add(brokenPrefabLabel);}
			
			if ( disabledBrokenPrefabLabel != null ) {FixStyle( disabledBrokenPrefabLabel );	LABLES.Add(disabledBrokenPrefabLabel); }
			
			UpdateBGHOver();
			
			
			
			
			
			
			//  }
		}
		
		var ns =  typeof( EditorWindow ).Assembly.GetType( "UnityEditor.SceneVisibilityHierarchyGUI+Styles" );
		
		if ( ns != null )
		{
		
		
			var vicContent =  GetValue_Field(ns, "sceneVisibilityStyle") as GUIStyle;
			
			if ( vicContent != null ) FixStyle( vicContent );
		}
		
		
		
		
		/*  gs =  typeof( EditorWindow ).Assembly.GetType( "UnityEditor.PrefabUtility+GameObjectStyles" );
		  if ( gs != null )
		  {
		
		      var prefabLabel =  GetValue_Field(gs, "prefabLabel") as GUIStyle;
		      var disabledPrefabLabel = GetValue_Field( gs, "disabledPrefabLabel") as GUIStyle;
		      var brokenPrefabLabel = GetValue_Field(gs, "brokenPrefabLabel") as GUIStyle;
		      var disabledBrokenPrefabLabel = GetValue_Field(gs, "disabledBrokenPrefabLabel") as GUIStyle;
		      if ( prefabLabel != null ) FixStyle( prefabLabel );
		      if ( disabledPrefabLabel != null ) FixStyle( disabledPrefabLabel );
		      if ( brokenPrefabLabel != null ) FixStyle( brokenPrefabLabel );
		      if ( disabledBrokenPrefabLabel != null ) FixStyle( disabledBrokenPrefabLabel );
		      //  }
		  }*/
		
		
	}
	
	
	internal void InitializeReflection()
	{	frame_method = (typeof( SceneView ).GetMethod( "Frame", (BindingFlags)(-1) ));
	
	
	
	
	
	
	
	
	
	
	
	
		GetInstanceIDFromGUID = typeof( AssetDatabase ).GetMethod( "GetInstanceIDFromGUID", (BindingFlags)(-1) );
		
		if ( GetInstanceIDFromGUID == null )
		{	fixget = true;
			GetMainAssetInstanceIDFromPath = typeof( AssetDatabase ).GetMethod( "GetMainAssetInstanceID", (BindingFlags)(-1) );
			//GetMainAssetInstanceIDFromPath = typeof( AssetDatabase ).GetMethod( "GetMainAssetOrInProgressProxyInstanceID", (BindingFlags)(-1) );
		}
		
		/* if (GetMainAssetInstanceIDFromPath == null)
		 {
		   Debug.Log( "ASD" );
		   fixget = false;
		   GetInstanceIDFromGUID = typeof( AssetDatabase ).GetMethod( "GetInstanceIDFromGUID", (BindingFlags)(-1) );
		 }*/
		if ( pluginID == Initializator.HIERARCHY_ID )
		{	strange_field = (SceneHierarchyWindowRoot.GetField( "s_SceneHierarchyWindow", (BindingFlags)(-1) ));
		
			if ( strange_field == null ) strange_field = (SceneHierarchyWindowRoot.GetField( "s_SceneHierarchyWindows", (BindingFlags)(-1) ));
			
			if ( strange_field == null ) logProxy.LogError( "Scenes List not found" );
			
			// Debug.Log(SceneHierarchyWindowRoot.GetFields((BindingFlags)(-1)).Select(f => f.Name).Aggregate((a, b) => a + " " + b));
			
			/*  m_EditorWindow m_FrameRequestID m_FrameRequestPing m_CustomScenes m_CustomParentForNewGameObjects m_CustomDragHandler <position>k__BackingField kInvalidSceneHandle m_TreeView m_TreeViewState
			  m_ExpandedScenes m_TreeViewKeyboardControlID m_CurrenRootInstanceID m_LockTracker m_ParentNamesForSelectedSearchResult m_SearchFilter m_SearchMode m_LastFramedID m_TreeViewReloadNeeded
			  m_SelectionSyncNeeded m_FrameOnSelectionSync m_DidSelectSearchResult m_LastUserInteractionTime m_CurrentSortingName m_SortingObjects m_AllowAlphaNumericalSort <>f__mg$cache0 <>f__mg$cache1
			  <>f__am$cache0 <>f__am$cache1 <>f__am$cache2 <>f__am$cache3 <>f__am$cache4 <>f__am$cache5 <>f__am$cache6 <>f__am$cache7 <>f__am$cache8
			  UnityEngine.Debug: Log(Object)
			  EModules.EModulesInternal.Adapter: InitializeReflection() (at Assets / EModules / _DLL_Hier / _COMMON / _ADAPTER_REFLECTION.cs: 181)
			  EModules.EModulesInternal.Adapter: Loopinit() (at Assets / EModules / _DLL_Hier / _COMMON / Hierarchy / Hierarhy_INIT.cs: 115)
			  EModules.EModulesInternal.Adapter: InitializateAdapter(String, Int32) (at Assets / EModules / _DLL_Hier / _COMMON / __SWITCHER.cs: 466)
			  EModules.EModulesInternal.Adapter: RESET_MODULE() (at Assets / EModules / _DLL_Hier / _COMMON / _MAIN.cs: 423)
			  EModules.EModulesInternal.<LogProxy>c__AnonStorey0: <>m__0() (at Assets / EModules / _DLL_Hier / _COMMON / ADDONS / LogProxy.cs: 34)
			  UnityEditor.EditorApplication: Internal_CallUpdateFunctions()*/
		}
		
		else
		{	strange_field = (SceneHierarchyWindowRoot.GetField( "s_ProjectBrowsers", (BindingFlags)(-1) ));
		}
		
		/*   s_LastInteractedHierarchy = SceneHierarchyWindowRoot.GetField("s_LastInteractedHierarchy",  (BindingFlags)(-1) );
		   has_s_LastInteractedHierarchy = s_LastInteractedHierarchy != null;*/
		//  Debug.Log(has_s_LastInteractedHierarchy);
		// Debug.Log( strange_field );
		// m_TreeView = (SceneHierarchyWindow.GetField("m_TreeView", (BindingFlags)(-1)));m_AssetTree
		__m_TreeView = SceneHierarchyWindow.GetField( "m_treeView", (BindingFlags)(-1) );
		
		if ( __m_TreeView == null ) __m_TreeView = SceneHierarchyWindow.GetField( "m_TreeView", (BindingFlags)(-1) );
		
		__m_AssetsView = SceneHierarchyWindow.GetField( "m_AssetTree", (BindingFlags)(-1) );
		__m_AssetTreeState = SceneHierarchyWindow.GetField( "m_AssetTreeState", (BindingFlags)(-1) );
		__m_FoldView = SceneHierarchyWindow.GetField( "m_FolderTree", (BindingFlags)(-1) );
		__m_FolderTreeState = SceneHierarchyWindow.GetField( "m_FolderTreeState", (BindingFlags)(-1) );
		m_ViewMode = SceneHierarchyWindow.GetField( "m_ViewMode", (BindingFlags)(-1) );
		
		
		//  Debug.Log( m_TreeView );
		// m_TreeView = SceneHierarchyWindow.GetField( "treeView" , (BindingFlags)(-1) );
		hoveredItem = m_TreeViewFieldInfo.FieldType.GetProperty( "hoveredItem", (BindingFlags)(-1) );
		hashoveredItem = pluginID == 0 && hoveredItem != null;
		
		
		m_KeyboardControlIDField = m_TreeViewFieldInfo.FieldType.GetField( ID_STRING, (BindingFlags)(-1) );
		
		if ( m_KeyboardControlIDField == null ) m_KeyboardControlIDField = m_TreeViewFieldInfo.FieldType.GetField( ID_STRING2, (BindingFlags)(-1) );
		
		m_UseExpansionAnimation = m_TreeViewFieldInfo.FieldType.GetField( "m_UseExpansionAnimation", (BindingFlags)(-1) );
		m_data = m_TreeViewFieldInfo.FieldType.GetProperty( "data", (BindingFlags)(-1) );
		m_gui = m_TreeViewFieldInfo.FieldType.GetProperty( "gui", (BindingFlags)(-1) );
		m_IsExpanded = m_data.PropertyType.GetMethods( (BindingFlags)(-1) ).First( m => m.Name == "IsExpanded" && m.GetParameters()[0].ParameterType == typeof( int ) );
		// m_IsExpanded = m_data.PropertyType.GetMethod("IsExpanded", (BindingFlags)(-1));
		// m_Animating = m_TreeView.PropertyType.GetProperty("animatingExpansion", (BindingFlags)(-1));
		// m_Animating = m_FramingAnimFloat.FieldType.GetProperty("isAnimating", (BindingFlags)(-1));
		// m_CurrentClipRect = m_FramingAnimFloat.FieldType.GetField("m_CurrentClipRect", (BindingFlags)(-1));
		//   isExpanding = m_FramingAnimFloat.FieldType.GetProperty("isExpanding", (BindingFlags)(-1));
		// m_LerpPosition = m_FramingAnimFloat.FieldType.BaseType.GetField("m_LerpPosition", (BindingFlags)(-1));
		// MonoBehaviour.print(m_Animating);
		
		m_Pos = typeof( EditorWindow ).GetField( "m_Pos", (BindingFlags)(-1) );
		m_TotalRect = m_TreeViewFieldInfo.FieldType.GetField( "m_TotalRect", (BindingFlags)(-1) );
		m_VisibleRect = m_TreeViewFieldInfo.FieldType.GetField( "m_VisibleRect", (BindingFlags)(-1) );
		
		if ( pluginname == Initializator.PROJECT_NAME )
		{	m_SearchFilterString = SceneHierarchyWindow.GetField( "m_SearchFilter", (BindingFlags)(-1) );
			m_SearchFilterClass_Has = m_SearchFilterString.FieldType.GetMethod( "IsSearching", (BindingFlags)(-1) );
		}
		
		if ( pluginID == Initializator.HIERARCHY_ID )
		{	SearchableWindowType = Assembly.GetAssembly( typeof( EditorWindow ) ).GetType( "UnityEditor.SearchableEditorWindow" );
			m_SearchFilterString = SearchableWindowType.GetField( "m_SearchFilter", (BindingFlags)(-1) );
			showingPrefabHeader = ___SceneHierarchyWindowRoot.GetProperty( "showingPrefabHeader", (BindingFlags)(-1) );
			hasShowingPrefabHeader = showingPrefabHeader != null;
			
		}
		
		
		m_state = m_TreeViewFieldInfo.FieldType.GetProperty( "state" );
		scrollPosField = m_state.PropertyType.GetField( "scrollPos" );
		m_expandedIDs = m_state.PropertyType.GetProperty( "expandedIDs", (BindingFlags)(-1) );
		//  MonoBehaviour.print(scrollPosField);
		/*
		                      var treeViewVal = m_TreeView.GetValue(window(), null);
		
		                var stateVal = treeViewVal.GetType()
		                    .GetProperty("state")
		                    .GetValue(treeViewVal, null);
		                 .GetField("scrollPos")*/
		var GameObjectTreeViewGUI = Assembly.GetAssembly(typeof(EditorWindow)).GetType("UnityEditor.GameObjectTreeViewGUI");
		// s_GOStyles = GameObjectTreeViewGUI.GetField("s_GOStyles", (BindingFlags)(-1));
		s_GOStyles = GameObjectTreeViewGUI.GetField( "s_GOStyles", (BindingFlags)(-1) );
		
		var GOStype = s_GOStyles != null ? s_GOStyles.FieldType : GameObjectTreeViewGUI.GetNestedTypes((BindingFlags)(-1)).First(t => t.Name == "GameObjectStyles");
		// var GOStype = s_GOStyles != null ? s_GOStyles.FieldType : GameObjectTreeViewGUI.GetNestedType("GameObjectStyles", (BindingFlags)(-1));
		
		sceneHeaderBg = GOStype.GetField( "sceneHeaderBg", (BindingFlags)(-1) );
		treestyles.Add( GOStype.GetField( "disabledLabel", (BindingFlags)(-1) ) );
		treestyles.Add( GOStype.GetField( "disabledPrefabLabel", (BindingFlags)(-1) ) );
		treestyles.Add( GOStype.GetField( "prefabLabel", (BindingFlags)(-1) ) );
		treestyles.Add( GOStype.GetField( "disabledBrokenPrefabLabel", (BindingFlags)(-1) ) );
		treestyles.Add( GOStype.GetField( "brokenPrefabLabel", (BindingFlags)(-1) ) );
		
		var treeViewBaseType = GameObjectTreeViewGUI.BaseType;
		
		
		s_Style = GameObjectTreeViewGUI.BaseType.GetField( "s_Styles", (BindingFlags)(-1) );
		// MonoBehaviour.print(s_Style);
		var Styles = s_Style != null ? s_Style.FieldType : treeViewBaseType.GetNestedTypes((BindingFlags)(-1)).First(t => t.Name == "Styles");
		
		// var Styles = s_Style != null ? s_Style.FieldType : treeViewBaseType.GetNestedType("Styles", (BindingFlags)(-1));
		if ( Styles != null )
		{	lineStyle = Styles.GetField( "lineStyle", (BindingFlags)(-1) );
			lineBoldStyle = Styles.GetField( "lineBoldStyle", (BindingFlags)(-1) );
			alignment = lineStyle.FieldType.GetProperty( "alignment", (BindingFlags)(-1) );
			paddingProp = lineStyle.FieldType.GetProperty( "padding", (BindingFlags)(-1) );
		}
		
		
		guiProp = m_TreeViewFieldInfo.FieldType.GetProperty( "gui", (BindingFlags)(-1) );
		//GetTotalSize = m_TreeViewFieldInfo.FieldType.GetProperty( "GetTotalSize", (BindingFlags)(-1) );
		
		
		//   extraSpaceBeforeIconAndLabel = treeViewBaseType.GetProperty("extraSpaceBeforeIconAndLabel", (BindingFlags)(-1));
		// extraInsertionMarkerIndent = treeViewBaseType.GetField("k_BaseIndent", (BindingFlags)(-1));
		k_BottomRowMargin = treeViewBaseType.GetField( "k_BottomRowMargin", (BindingFlags)(-1) );
		/* var f_k_LineHeight = treeViewBaseType.GetField( "k_LineHeight" , (BindingFlags)(-1) );
		 if ( f_k_LineHeight == null ) {
		     var p_k_LineHeight = treeViewBaseType.GetProperty( "k_LineHeight" , (BindingFlags)(-1) );
		
		 }*/
		k_LineHeight = new ReflType( treeViewBaseType, "k_LineHeight" );
		k_IndentWidth = treeViewBaseType.GetField( "k_IndentWidth", (BindingFlags)(-1) );
		k_BaseIndent = treeViewBaseType.GetField( "k_BaseIndent", (BindingFlags)(-1) );
		m_UseHorizontalScroll = treeViewBaseType.GetField( "m_UseHorizontalScroll", (BindingFlags)(-1) );
		foldoutYOffset = treeViewBaseType.GetField( "foldoutYOffset", (BindingFlags)(-1) );
		
		if ( foldoutYOffset == null ) foldoutYOffset = treeViewBaseType.GetField( "customFoldoutYOffset", (BindingFlags)(-1) );
		
		k_IconWidth = treeViewBaseType.GetField( "k_IconWidth", (BindingFlags)(-1) );
		iconOverlayGUI = treeViewBaseType.GetProperty( "iconOverlayGUI", (BindingFlags)(-1) );
		labelOverlayGUI = treeViewBaseType.GetProperty( "labelOverlayGUI", (BindingFlags)(-1) );
		
		if ( labelOverlayGUI != null ) haslabelOverlayGUI = true;
		
		if ( IS_HIERARCHY() )
		{	var goti = Assembly.GetAssembly(typeof(EditorWindow)).GetType("UnityEditor.GameObjectTreeViewItem", false);
		
			if ( goti != null )
			{	overlayIcon = goti.GetProperty( "overlayIcon", (BindingFlags)(-1) );
			
				if ( overlayIcon != null ) HasoverlayIcon = true;
			}
			
			UnityEventArgsType = Assembly.GetAssembly( typeof( UnityEngine.Events.UnityEvent ) ).GetType( "UnityEngine.Events.ArgumentCache", true );
		}
		
		
		
		m_Ping = treeViewBaseType.GetField( "m_Ping", (BindingFlags)(-1) );
		m_PingStyle = m_Ping.FieldType.GetField( "m_PingStyle", (BindingFlags)(-1) );
		fixedHeight = m_PingStyle.FieldType.GetProperty( "fixedHeight", (BindingFlags)(-1) );
		
		
		GetTotalSizeMethodInfo = m_TreeViewFieldInfo.FieldType.GetMethod( "GetContentSize", (BindingFlags)(-1) );
		
		
		m_SetExpandedIDs = m_data.PropertyType.GetMethods().First( m => m.Name == "SetExpandedIDs" );
		m_dataSetExpanded = m_data.PropertyType.GetMethods().First( m => m.Name == "SetExpanded" && m.GetParameters().Any( p => p.ParameterType == typeof( int ) ) );
		m_dataIsExpanded = m_data.PropertyType.GetMethods().First( m => m.Name == "IsExpanded" && m.GetParameters().Any( p => p.ParameterType == typeof( int ) ) );
		
		m_dataSetExpandWithChildrens = m_data.PropertyType.GetMethods().First( m => m.Name == "SetExpandedWithChildren" && m.GetParameters().Any( p => p.ParameterType == typeof( int ) ) );
		
		
		m_dataFindItem = m_data.PropertyType.GetMethods().First( m => m.Name == "FindItem" && m.GetParameters().Any( p => p.ParameterType == typeof( int ) ) );
		onGUIRowCallback = m_TreeViewFieldInfo.FieldType.GetProperty( "onGUIRowCallback", (BindingFlags)(-1) );
		
		/*foreach (var VARIABLE in m_TreeView.FieldType.GetEvents( (BindingFlags)(-1) ))
		{
		  Debug.Log( VARIABLE.Name );
		}*/
		// m_dataGetItem = m_data.PropertyType.GetMethods().First( m => m.Name == "GetItem" && m.GetParameters().Any( p => p.ParameterType == typeof( int ) ) );
		m_data_treeitem_children = m_dataFindItem.ReturnType.GetProperty( "children", (BindingFlags)(-1) );
		m_data_treeitem_m_ID = m_dataFindItem.ReturnType.GetField( "m_ID", (BindingFlags)(-1) );
		m_data_treeitem_m_Parent = m_dataFindItem.ReturnType.GetField( "m_Parent", (BindingFlags)(-1) );
		m_data_treeitem_m_DisplayName = m_dataFindItem.ReturnType.GetField( "m_DisplayName", (BindingFlags)(-1) );
		
		
		
		INITIALIZE_UTILITIES();
		
		
		
		/* EditorApplication.ExecuteMenuItem("Window/Hierarchy");
		 var window = EditorWindow.focusedWindow;
		 if (window == null || m_treeData == null || m_SetExpanded == null) return;
		
		 var t = m_TreeView.GetValue(Convert.ChangeType(window, SceneHierarchyWindow), null);
		 MonoBehaviour.print(t);
		 if (t == null) return;
		 var d = m_treeData.GetValue(t, null);
		 MonoBehaviour.print(d);*/
		// m_SetExpanded.Invoke(d, new[] { (object)instanceId, isExpand });
		//  m_SetExpanded.Invoke(m_treeData.GetValue(window, null), new[] { (object)instanceId, isExpand });
		// foreach (var @interface in m_treeData.PropertyType.GetMethod("SetExpanded")) {
		
		/*    MonoBehaviour.print(.First(i=>i.FullName);
		 //  m_SetExpanded = m_treeData.PropertyType.GetInterface("ITreeViewDataSource").GetMethod("SetExpanded");
		   MonoBehaviour.print(m_SetExpanded);*/
	}
	
	internal PropertyInfo m_expandedIDs, m_data_treeitem_children;
	
	internal MethodInfo m_dataSetExpanded, m_dataIsExpanded, m_SetExpandedIDs, m_dataSetExpandWithChildrens;
	//internal MethodInfo m_dataGetRow, m_dataGetItem;
	internal MethodInfo m_dataFindItem;
	internal FieldInfo m_data_treeitem_m_ID, m_data_treeitem_m_Parent, m_data_treeitem_m_DisplayName;
	internal PropertyInfo onGUIRowCallback;
	
}
}
