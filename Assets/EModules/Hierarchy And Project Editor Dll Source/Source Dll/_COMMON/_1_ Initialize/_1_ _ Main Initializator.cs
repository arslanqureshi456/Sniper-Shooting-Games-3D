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
internal partial class Adapter : IEquatable<Adapter>, IEqualityComparer<Adapter> {
	public static bool operator ==( Adapter a, Adapter b ) { return ReferenceEquals( a, null ) && ReferenceEquals( b, null ) || !ReferenceEquals( a, null ) && !ReferenceEquals( b, null ) && a.pluginID == b.pluginID; }
	public static bool operator !=( Adapter a, Adapter b ) { return !(a == b); }
	public bool Equals( Adapter other ) { return pluginID == other.pluginID; }
	public override bool Equals( object obj ) { return pluginID == ((Adapter)obj).pluginID; }
	bool IEquatable<Adapter>.Equals( Adapter other ) { return Equals( other ); }
	public override int GetHashCode() { return pluginID; }
	public bool Equals( Adapter x, Adapter y ) { return x == y; }
	bool IEqualityComparer<Adapter>.Equals( Adapter x, Adapter y ) { return x == y; }
	public int GetHashCode( Adapter obj ) { return obj.GetHashCode(); }
	int IEqualityComparer<Adapter>.GetHashCode( Adapter obj ) { return obj.GetHashCode(); }
	
	
	
	internal bool IS_HIERARCHY()
	{	return pluginID == Initializator.HIERARCHY_ID;
	}
	internal bool IS_PROJECT()
	{	return pluginID == Initializator.PROJECT_ID;
	}
	internal bool HAS_LABEL_ICON()
	{	return IS_PROJECT() || IS_HIERARCHY() && USE2018_3;
	}
	internal int pluginID;
	internal string pluginname;
	
	internal static string EMInternalFolder = null;
	string _PluginInternalFolder = null;
	internal string PluginInternalFolder
	{	get
		{	if ( _PluginInternalFolder == null )
				_PluginInternalFolder = EditorPrefs.GetString( "EModules/PluginInternalFolder/" + pluginname, "Assets/Plugins/EModules" + pluginname );
				
			return _PluginInternalFolder;
		}
		
		set
		{	if ( _PluginInternalFolder == value ) return;
		
			EditorPrefs.SetString( "EModules/PluginInternalFolder/" + pluginname, value );
			_PluginInternalFolder = value;
		}
	}
	string _PluginExternalFolder = null;
	internal string PluginExternalFolder
	{	get
		{	if ( _PluginExternalFolder == null )
				_PluginExternalFolder = UNITY_SYSTEM_PATH + PluginInternalFolder;
				
			return _PluginExternalFolder;
		}
		
		set
		{	if ( _PluginExternalFolder == value ) return;
		
			_PluginExternalFolder = value;
		}
	}
	
	
	
	internal bool wasAdapterInitalize = false;
	internal bool wasModulesInitialize = false;
	/*  internal bool wasModulesInitialize
	  {   get
	      {   if (modules == null || modules.Length == 0) return false;
	          return ___wasModulesInitialize;
	      }
	      set
	      {   ___wasModulesInitialize = value;
	      }
	  }*/
	internal bool wasIconsInitialize = false;
	bool __tempAdapterBlock;
	internal bool tempAdapterBlock
	{	get { return __tempAdapterBlock; }
	
		set
		{	__tempAdapterBlock = value;
#if UNITY_EDITOR
			// if (value) Debug.LogError("Temp Block");
#endif
		}
	}
	internal bool tempAdapterDisableCache;
	internal int initializationCount;
	internal int lastScanIndex = 0;
	internal bool needReinstall { set { logProxy.LogWarning( "Installation error - Please reinstall plugin" ); } }
	
	
	
	internal LogProxy logProxy;
	internal Adapter.CustomMenuAndModule customMenuModules;
	internal IMethodsInterface MOI;
	internal HierParams par;
	
	internal float parLINE_HEIGHT
	{	get     // if (!USE2018) return EditorGUIUtility.singleLineHeight;
		{	return par.LINE_HEIGHT;
		}
		
		set     //if (!USE2018) return;
		{	par.LINE_HEIGHT = value;
		}
	}
	
	
	/*  */
#pragma warning disable
	int GetPluginID() { return pluginID; }
	static void CheckAndGet( Int32List InstanceID )
	{	if ( string.IsNullOrEmpty( AssetDatabase.GUIDToAssetPath( InstanceID.GUIDsActiveGameObject ) ) )
		{	if ( AssetDatabase.LoadAssetAtPath<UnityEngine.Object>( InstanceID.PATHsActiveGameObject ) )
				InstanceID.GUIDsActiveGameObject = AssetDatabase.AssetPathToGUID( InstanceID.PATHsActiveGameObject );
				
			for ( int i = 0 ; i < InstanceID.PATHsList.Count ; i++ )
			{	if ( AssetDatabase.LoadAssetAtPath<UnityEngine.Object>( InstanceID.PATHsList[i] ) )
					InstanceID.GUIDsList[i] = AssetDatabase.AssetPathToGUID( InstanceID.PATHsList[i] );
					
			}
		}
	}
#pragma warning restore
	/* MAIN INITIALIZATION FUNCTION */
	internal void InitializateAdapter( string pluginname, int pluginID )
	{	this.pluginname = pluginname;
		this.pluginID = pluginID;
		
		tempAdapterBlock = false;
		logProxy = new LogProxy( this );
		
		if ( pluginID == 0 )     //EModules.GoGuidPair.PluginID = GetPluginID;
		{	EModules.GoGuidPair.GetFileID = GetFileID;
		}
		
		EModules.Int32List.CheckAndGet = CheckAndGet;
		
		//  GoGuidPair.GetPrefab =  GetPrefabInstanceHandleGameObject;
		
		
		bottomInterface = new BottomInterface( this, pluginID == Initializator.HIERARCHY_ID );
		
		
		INIT_CustomMenuModules();
		
		__modulesOrdered = null;
		
		/* if (_colorText11 == null)
		 {   _colorText11 = GET_TEXTURE( 0x002,  );
		 }
		 if (colorStatic == null)
		 {   var col = Color.gray;
		     col.a /= 4;
		     colorStatic = GET_TEXTURE( 0x003, col );
		 }
		 if (secA == null)
		 {   secA = GET_TEXTURE( 0x004, Adapter.LINE );
		     secB = GET_TEXTURE( 0x005, new Color( Adapter.LINE.a, Adapter.LINE.r, Adapter.LINE.b, Adapter.LINE.a / 3 ) );
		 }*/
		
		
		bottomInterface.Init();
		
		if (pluginID == 0 )
		{	if ( EditorPrefs.GetInt( GET_HIER_PARAM_KEY_WITHOUTVERSIONS( this ) + "/showWelcome", 0 ) != 1 )
			{	EditorPrefs.SetInt( GET_HIER_PARAM_KEY_WITHOUTVERSIONS( this ) + "/showWelcome", 1);
			
				var p = new Rect(0, 0, Screen.currentResolution.width, Screen.currentResolution.height);
				WelcomeScreen.Init( p );
			}
		}
		
		
		wasAdapterInitalize = false;
		initializationCount = 0;
		wasModulesInitialize = false;
		//tempBlock = false;
		__modulesOrdered = null;
		
		
		
		bottomInterface.INIT_ON_LOAD();
		
		
		
		Undo.undoRedoPerformed -= UNDO_AC;
		Undo.undoRedoPerformed += UNDO_AC;
		
		INIT_HEIGHT_FIX();
		
		Selection.selectionChanged -= OnSelectionChanged;
		Selection.selectionChanged += OnSelectionChanged;
		
		EditorApplication.update -= OnEditorUpdate;
		EditorApplication.update += OnEditorUpdate;
		
		EditorApplication.update -= UpdateCb;
		EditorApplication.update += UpdateCb;
		
		if ( pluginID == Initializator.HIERARCHY_ID )
		{	EditorApplication.hierarchyWindowItemOnGUI -= hier_Main;
			EditorApplication.hierarchyWindowItemOnGUI += hier_Main;
		}
		
		if ( pluginID == Initializator.PROJECT_ID )
		{	EditorApplication.projectWindowItemOnGUI -= proj_Main;
			EditorApplication.projectWindowItemOnGUI += proj_Main;
		}
		
		EditorSceneManager.sceneUnloaded -= EditorSceneManagerOnSceneLoaded;
		EditorSceneManager.sceneUnloaded += EditorSceneManagerOnSceneLoaded;
		
		EditorApplication.modifierKeysChanged -= modifierKeysChanged;
		EditorApplication.modifierKeysChanged += modifierKeysChanged;
		
		onSelectionChanged -= S_CH;
		onSelectionChanged += S_CH;
		
		var info = typeof (EditorApplication).GetField ("globalEventHandler", (BindingFlags)(-1));
		var value = (EditorApplication.CallbackFunction)info.GetValue (null);
		value += EditorGlobalKeyPress;
		info.SetValue( null, value );
		
		INIT_InitializeEditorSettings();
		
		INIT_AdapterRepeatableInitializationFunction();
		
		
		if ( pluginID == 1 )
			Hierarchy_GUI.Instance( this );
			
		// AssetDatabase.
		
	}
	
	internal bool SKIP_PREFAB_ESCAPE = false;
	
	void EditorGlobalKeyPress()
	{
	
	
		/*  Debug.Log( Event.current );
		  if ( Event.current != null && Event.current.rawType == EventType.MouseUp )
		  {   if ( OnRawMouseUp != null )
		      {   OnRawMouseUp();
		          OnRawMouseUp = null;
		      }
		  }*/
		if ( Event.current != null && (Event.current.rawType == EventType.KeyDown || Event.current.rawType == EventType.KeyUp) ) modifierKeysChangedFix();
		
		if ( Event.current.type == EventType.KeyDown && USE_HOVER_EXPANDING && (Event.current.keyCode == KeyCode.RightArrow || Event.current.keyCode == KeyCode.LeftArrow))
		{	if (hashoveredItem  && hoverID != -1  )
			{
			
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
		
		
		if ( Application.isPlaying ) return;
		
		//  if ( Event.current != null && Event.current.type == EventType.KeyDown && Event.current.functionKey ) RepaintWindowInUpdate();
		
		
		
		
		
		
		if ( Event.current != null && Event.current.rawType == EventType.KeyDown )
		{	bool used = false;
		
			if ( Event.current.keyCode == KeyCode.Escape && !SKIP_PREFAB_ESCAPE )
			{	if ( !par.ESCAPE_CLOSE_PREFAB || !EditorWindow.focusedWindow ) goto skipPrefab;
			
				if ( !PREFAB_ESCAPE_ALL_WINDOWS )
				{	var n = EditorWindow.focusedWindow.GetType().Name;
					var res = false;
					res |= n.Contains( "SceneView" );
					res |= n.Contains( "Inspector" );
					res |= n.Contains( "GameView" );
					res |= n.Contains( "SceneHierarchy" );
					res |= n.Contains( "ProjectBrowser" );
					
					if ( !res ) goto skipPrefab;
				}
				
				CLOSE_PREFAB_MODE();
skipPrefab:;
				used = true;
			}
			
			if ( !used )
			{	if ( Event.current.keyCode != KeyCode.None && EditorWindow.focusedWindow )
				{	var n = EditorWindow.focusedWindow.GetType().Name;
				
					if ( n.Contains( "SceneHierarchy" ) ) goto skipHier;
					
					foreach ( var item in CUSTOMMENU_HOTKEYS_WINDOWS )
					{	if ( IS_HIERARCHY() && item.Key == "SceneHierarchy" ) continue;
					
						if ( IS_PROJECT() && item.Key == "ProjectBrowser" ) continue;
						
						if ( n.Contains( item.Key ) )
						{	customMenuModules.CheckKeyDown( Event.current.control, Event.current.shift, Event.current.alt, Event.current.keyCode );
						}
					}
				}
				
skipHier:;

			}
		}
	}
	
	
	internal void INIT_InitializeEditorSettings()
	{	if ( !EditorPrefs.HasKey( GET_HIER_PARAM_KEY( this ) ) ) //check old versiona
		{	for ( int i = GET_HIER_PARAM_KEY_COUNT( this ) - 1 ; i >= 0 ; i-- )
			{	if ( EditorPrefs.HasKey( GET_HIER_PARAM_KEY( this, i ) ) )
				{	EditorPrefs.SetString( GET_HIER_PARAM_KEY( this ), EditorPrefs.GetString( GET_HIER_PARAM_KEY( this, i ) ) );
					break;
				}
			}
		}
		
		
		/* if (!EditorPrefs.HasKey( GET_HIER_PARAM_KEY( this ) ) && EditorPrefs.HasKey( oldHierParamsGET_HIER_PARAM_KEY( this ) )) //check old data
		     EditorPrefs.SetString( GET_HIER_PARAM_KEY( this ), EditorPrefs.GetString( oldHierParamsGET_HIER_PARAM_KEY( this ) ) );*/
		
		bool exept = true;
		
		if ( EditorPrefs.HasKey( GET_HIER_PARAM_KEY( this ) ) )
		{	try     // EditorJsonUtility.ToJson(
			{	//  System.Xml.
				par = Adapter.DESERIALIZE_SINGLE<HierParams>( EditorPrefs.GetString( GET_HIER_PARAM_KEY( this ) ) );
				exept = false;
			}
			
			catch ( Exception ex )
			{
#if UNITY_EDITOR
				Debug.LogError( "SERIALIZE PAR\n" + ex.Message + "\n" + ex.StackTrace );
				//MonoBehaviour.print("LODA " + par.FD_Icons_default);
#endif
				//  LogProxy.LogError(ex.Message + " " + ex.StackTrace);
				//  LogProxy.Log("EModules - Missing data was set to default");
			}
		}
		
		if ( exept )
		{	par = GET_DEFAULT_SETTINGS;
		
			if ( EditorPrefs.HasKey( GET_HIER_PARAM_KEY( this ) ) ) SavePrefs();
		}
		
		
		
		/*if (!modsInits)
		{   modsInits = true;
		    if (!wasModulesInitialize) MOI.InitModules();
		    wasModulesInitialize = false;
		    MOI.InitModules();
		    //SET_TO_DEFAULT();
		}*/
		
	}
	// bool modsInits = false;
	
	
	
	
	
	
	
	bool INIT_AdapterRepeatableInitializationFunction()
	{	if ( wasAdapterInitalize || tempAdapterBlock ) return true;
	
		//  if (Event.current != null) return false;
		//  if (Event.current != null && Event.current.type != EventType.layout) return false;
		
		//   MonoBehaviour.print("1");
		
		var was = initializationCount >= 2;
		var result = Hierarchy_GUI.Instance( this );
		
		//  MonoBehaviour.print("23");
		// EditorApplication.update -= Loopinit;
		if ( result == null )
		{	if ( !was )     // EditorApplication.update += Loopinit;
			{	// InternalEditorUtility.RepaintAllViews();
				//   MonoBehaviour.print("2");
				return false;
			}
			
			tempAdapterBlock = true;
			needReinstall = true;
			
			logProxy.Log( "Can not start the Hierarchy plugin. Please reinstall plugin" );
			return false;
		}
		
		
		
		tempAdapterDisableCache = false;
		
		var checkFilesResult = CheckFiles( this );
		
		if ( checkFilesResult != null )
		{	if ( !checkFilesResult.EndsWith( ".cs" ) )
			{	if ( !was )     // EditorApplication.update += Loopinit;
				{	//  MonoBehaviour.print("3");
					//InternalEditorUtility.RepaintAllViews();
					return false;
				}
				
				tempAdapterBlock = true;
				needReinstall = true;
				
				//LogProxy.Log("Can not start the Hierarchy plugin. Please reinstall plugin");
				logProxy.Log( "'" + checkFilesResult + "' is Missing" );
				return false;
			}
			
			else
			{	tempAdapterDisableCache = true;
			}
		}
		
		wasAdapterInitalize = true;
		// InitModules();
		
		
		
		// EditorPrefs.DeleteKey("HierParams");
		
		InitializeReflection();
		
		// Init
		//  InitializeIcons();
		// COnst();
		
		
		
		if ( !UNITY_5_5 )
		{	/*  hierarchy_windows = ((IList)strange_field.GetValue( null ));
			      for (int i = 0 ; i < hierarchy_windows.Count ; i++)
			      {   if (hierarchy_windows[i] != null) continue;
			
			          var treeView = m_TreeView.GetValue( hierarchy_windows[i] );
			          if (m_data == null) m_data = treeView.GetType().GetProperty( "data", (BindingFlags)(-1) );
			          if (m_data != null) break;
			      }*/
			InitWindList();
		}
		
		CheckProjectDefines();
		
		
		RepaintWindowInUpdate();
		
		
		// if (IS_PROJECT())
		RunFileWatcher();
		
		
		OnSelectionChanged();
		
		return true;
	}
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	public void InitWindList()
	{	hierarchy_windows = ((IList)strange_field.GetValue( null ));
	
		/*  if (UseRootWindow)
		  {
		  }
		  else
		  {   var rootWindows = ((IList)strange_field.GetValue( null ));
		      List<object> result = new List<object>();
		      foreach (var item in rootWindows)
		      {   result.Add(m_SceneHierarchy.GetValue(item));
		      }
		      hierarchy_windows = (IList)result;
		  }*/
	}
	
	//FileSystemWatcher watcher = null;
	// object servise;
	public void RunFileWatcher()     // watcher.BeginInit();
	{	/* if (watcher != null)
		 {
		   watcher.EnableRaisingEvents = false;
		   //watcher = null;
		 }
		// if (watcher == null)
		 {
		   watcher = new FileSystemWatcher();
		
		   watcher.Path = PluginExternalFolder;
		   watcher.Filter = "*";
		   watcher.NotifyFilter = / *NotifyFilters.LastWrite |* / NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.CreationTime / *| NotifyFilters.LastAccess* /;
		
		 }
		
		// watcher.IncludeSubdirectories = true;
		
		 //watcher.Changed -= ( OnChanged );
		 watcher.Changed += new FileSystemEventHandler( OnChanged2 );
		//  watcher.Created -= ( OnChanged );
		 watcher.Created += new FileSystemEventHandler( OnChanged3 );
		//  watcher.Deleted -= ( OnChanged );
		 watcher.Deleted += new FileSystemEventHandler( OnChanged4 );
		//  watcher.Renamed -= ( OnRenamed );
		 watcher.Renamed += new RenamedEventHandler(OnRenamed );
		 watcher.Disposed += OnDisaposed;
		 watcher.Error += Error;
		
		 watcher.EnableRaisingEvents = true;
		//UnityEditor.Callbacks.
		*/
		/* watcher.Error += (e,b) =>
		 {
		   Debug.LogError( "File Watcher " + b + " " + e );
		 };*/
		
		// ..watcher.EndInit();
		
		//servise = watcher.InitializeLifetimeService();
		
		EditorApplication.update -= clean_updater;
		EditorApplication.update += clean_updater;
	}
	
	static  char[] asdasd = new char[] {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9'};
	class MyAllPostprocessor : AssetPostprocessor {
		static void OnPostprocessAllAssets( string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths )        // Debug.Log( "OnPostprocessAllAssets "  );
		{	Adapter.ON_ASSET_IMPORT();
		
			if ( importedAssets.Length != 0 )
			{	for ( int i = 0 ; i < importedAssets.Length ; i++ )
				{	if ( importedAssets[i].ToLower().EndsWith( ".unity" ) )
					{	var estim = importedAssets[i].Remove(importedAssets[i].Length - ".unity".Length );
					
						if ( asdasd.Contains( estim[estim.Length - 1] ) )
						{	var lastind =  estim.LastIndexOf(" ");
						
							if ( estim.Length - lastind <= 3 )
							{
							
							
								/*var numb = estim.Substring( estim.LastIndexOf( " " ) + 1);
								var numbInt  = -1;
								var tryCompleted = false;
								if ( int.TryParse( numb , out numbInt ) ) {
								
								    numbInt--;
								    if ( numbInt >= 0 ) {
								        var oldPath = estim.Remove( estim.LastIndexOf( " " ) )( numbInt == 0 ? "" : (" "+ numbInt)) + ".unity";
								        var sa = AssetDatabase.LoadAssetAtPath<SceneAsset>( oldPath );
								        if ( sa ) {
								            var newPathExternal = Hierarchy.HierarchyAdapterInstance.GetStoredDataPathExternal(importedAssets[i]);
								            if ( File.Exists( newPathExternal ) ) {
								                if ( Hierarchy.M_Descript.TryCreateBackupForCache( Hierarchy.HierarchyAdapterInstance.GetStoredDataPathInternal( importedAssets[i] ) ) ) {
								                    AssetDatabase.Refresh( ImportAssetOptions.ForceUpdate );
								                    Adapter.RequestScriptReload();
								                }
								            }
								            AssetDatabase.CopyAsset( Hierarchy.HierarchyAdapterInstance.GetStoredDataPathInternal( oldPath ) ,
								   Hierarchy.HierarchyAdapterInstance.GetStoredDataPathInternal( importedAssets[i] ) );
								            tryCompleted = true;
								        }
								    }
								
								
								}
								
								if ( !tryCompleted )*/
								{	var oldPath = estim.Remove( estim.LastIndexOf( " " ) ) + ".unity";
									var sa = AssetDatabase.LoadAssetAtPath<SceneAsset>( oldPath );
									
									if ( sa )
									{	var newPathExternal = Hierarchy.HierarchyAdapterInstance.GetStoredDataPathExternal(importedAssets[i]);
									
										if ( File.Exists( newPathExternal ) )
										{	if ( Hierarchy.M_Descript.TryCreateBackupForCache( Hierarchy.HierarchyAdapterInstance.GetStoredDataPathInternal( importedAssets[i] ) ) )
											{	AssetDatabase.Refresh( ImportAssetOptions.ForceUpdate );
												Adapter.RequestScriptReload();
											}
										}
										
										AssetDatabase.CopyAsset( Hierarchy.HierarchyAdapterInstance.GetStoredDataPathInternal( oldPath ),
										                         Hierarchy.HierarchyAdapterInstance.GetStoredDataPathInternal( importedAssets[i] ) );
									}
								}
								
							}
						}
					}
				}
			}
			
			if ( movedAssets.Length != 0 )
			{	bool wasBackUp = false;
			
				for ( int i = 0 ; i < movedAssets.Length ; i++ )
				{	if ( movedAssets[i].ToLower().EndsWith( ".unity" ) )
					{	var newPathExternal = Hierarchy.HierarchyAdapterInstance.GetStoredDataPathExternal(movedAssets[i]);
					
						if ( File.Exists( newPathExternal ) )
						{	if ( Hierarchy.M_Descript.TryCreateBackupForCache( Hierarchy.HierarchyAdapterInstance.GetStoredDataPathInternal( movedAssets[i] ) ) )
							{	AssetDatabase.Refresh( ImportAssetOptions.ForceUpdate );
								Adapter.RequestScriptReload();
								wasBackUp = true;
							}
						}
						
						
						AssetDatabase.MoveAsset( Hierarchy.HierarchyAdapterInstance.GetStoredDataPathInternal( movedFromAssetPaths[i] ),
						                         Hierarchy.HierarchyAdapterInstance.GetStoredDataPathInternal( movedAssets[i] ) );
					}
				}
				
				if ( wasBackUp )
				{
				
				}
			}
			
			if ( deletedAssets.Length != 0 )
				Hierarchy.HierarchyAdapterInstance.Again_Reloder_UsingWhenCopyPastOrAssets();
				
			//Initializator.Adapters[Initializator.PROJECT_NAME].ClearHierarchyObjects();
		}
	}
	
	private void Error( object sender, ErrorEventArgs e )
	{	Debug.Log( "Error " + sender );
	}
	
	private void OnDisaposed( object sender, EventArgs e )
	{	Debug.Log( "OnDisaposed " + sender );
	}
	
	private object m_Handle = new object();
	
	private void OnChanged( object source, FileSystemEventArgs e )      // OnRenamed( null, null );
	{
	}
	internal bool need_ClearHierarchyObjects1 = false;
	bool need_ClearHierarchyObjects2 = false;
	bool need_ClearHierarchyObjects3 = false;
	bool need_ClearHierarchyObjects4 = false;
	
	private void OnRenamed( object source, RenamedEventArgs e )
	{	lock ( m_Handle ) need_ClearHierarchyObjects1 = true;
	
		// RunFileWatcher();
		
		// watcher.EnableRaisingEvents = true;
	}
	
	private void OnChanged2( object source, FileSystemEventArgs e )
	{	lock ( m_Handle ) need_ClearHierarchyObjects2 = true;
	}
	private void OnChanged3( object source, FileSystemEventArgs e )
	{	lock ( m_Handle ) need_ClearHierarchyObjects3 = true;
	}
	private void OnChanged4( object source, FileSystemEventArgs e )
	{	lock ( m_Handle ) need_ClearHierarchyObjects4 = true;
	}
	
	void clean_updater()
	{	if ( NEW_PERFOMANCE ) return;
	
		if ( (need_ClearHierarchyObjects1 || need_ClearHierarchyObjects2 || need_ClearHierarchyObjects3 || need_ClearHierarchyObjects4) )     // lock (m_Handle)
		{
			{	need_ClearHierarchyObjects1 = false;
				need_ClearHierarchyObjects2 = false;
				need_ClearHierarchyObjects3 = false;
				need_ClearHierarchyObjects4 = false;
			}
			
			if ( !EditorApplication.isCompiling )
			{	ClearHierarchyObjects(true);
				UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
			}
			
			// Debug.Log( "clean_updater" );
			
			
		}
	}
	
	
	
	
	internal Action onUndoAction;
	
	void UNDO_AC()
	{	if ( !par.ENABLE_ALL ) return;
	
		wasUndo = true;
		bottomInterface.NeedRefreshBOttom = true;
		
		if ( onUndoAction != null ) onUndoAction();
		
		RepaintWindow(true);
		InternalClearDrag();
	}
	
	
	
#if UNITY_EDITOR
	/* [MenuItem( "EXPERT/disable" )]
	     public void Disable( )
	     {
	         RESET_MODULE();
	     }*/
#endif
	
	
	private void InitDics( string type )
	{	if ( par.sortType2108193717 == null ) par.sortType2108193717 = new Dictionary<string, int>();
	
		if ( par.SortDescending151046911 == null ) par.SortDescending151046911 = new Dictionary<string, bool>();
		
		if ( !par.sortType2108193717.ContainsKey( type.ToString() ) )
			par.sortType2108193717.Add( type.ToString(),
			                            type.GetType().Name.EndsWith( "M_CustomIcons" ) ? (int)SortType.Content : (int)SortType.Hierarchy );
			                            
		if ( !par.SortDescending151046911.ContainsKey( type.ToString() ) )
			par.SortDescending151046911.Add( type.ToString(), false );
	}
	
	internal KeyValuePair<SortType, bool> GetSortMode( Type type )
	{	return GetSortMode( type == null ? "" : type.ToString() );
	}
	internal KeyValuePair<SortType, bool> GetSortMode( string key )
	{	InitDics( key );
	
		return new KeyValuePair<SortType, bool>( (SortType)par.sortType2108193717[key],
		        par.SortDescending151046911[key] );
	}
	
	internal KeyValuePair<SortType, bool> SetSortMode( Type _type, SortType sort, bool descending )
	{	var key = _type == null ? "" : _type.ToString();
	
		InitDics( key );
		
		par.sortType2108193717[key] = (int)sort;
		par.SortDescending151046911[key] = descending;
		SavePrefs();
		return GetSortMode( key );
	}
	
	
	
	/*  void COnst( )
	      {
	
	
	
	
	      }*/
	
	private void EditorSceneManagerOnSceneLoaded( Scene arg0 )
	{
	
		//  MonoBehaviour.print(arg0.GetRootGameObjects().Where(t => t.name == "DescriptionHelperObject" && t.GetComponents<Component>().Any(c => !c)).Count());
		
		//  MonoBehaviour.print(Resources.FindObjectsOfTypeAll<Transform>().Where(t => t.parent == null).Where(t => t.name == "DescriptionHelperObject" && t.GetComponents<Component>().Any(c => !c)).Count());
	}
	
	/*  private  void PlaymodeStateChanged()
	    {
	        if (EditorApplication.isPlaying) {
	            MonoBehaviour.print("ASD");
	        }
	    }*/
	
	
	
	void EmptyF()
	{
	
	}
	
	
	
	void INITIALIZE_UTILITIES()     // HierarchyExtensions.Utilities.
	{	var memWindow = SceneHierarchyWindow;
		//var property = m_TreeView.FieldType.GetProperty( "data" );
		//var first = property.PropertyType.GetMethods().First( m => m.Name == "GetExpandedIDs" );
		//var ds = property.PropertyType.GetMethods().First( m => m.Name == "SetExpandedIDs" );
		var property = m_data;
		//  var isExpanded = m_dataIsExpanded;
		var SetExpanded = m_dataSetExpanded;
		var dataSetExpandWithChildrens = m_dataSetExpandWithChildrens;
		
		//WINDOW_expandMethod.Invoke(window(), new object[] { o.id, expand });
		
		
		
		var tempD =  SceneHierarchyWindow.GetMethod( "DuplicateGO", (BindingFlags)(-1) );
		Action dup = ( ) =>
		{	if (window() == null ) return;
		
			if ( SELECTED_GAMEOBJECTS().Length == 0 ) return;
			
			var olddata = AddDataToName( GetBroadCastSelection().Select(g => g.go).ToArray(), true );
			
			if (UseRootWindow)
				tempD.Invoke(window(), null );
			else
			{	tempD.Invoke(m_SceneHierarchy.GetValue(window()), null );
			}
			
			SaveDataFromName( GetBroadCastSelection().Select( g => g.go ).ToArray() );
			RemoveDataFromName( olddata );
		};
		
		if ( pluginID != Initializator.HIERARCHY_ID ) dup = EmptyF;
		
		
		
		/*	Func<object , object , object , object> func = ( o1 , o2 , o3 ) => {
		    var arr = o1 as int[];
		    var id = (int?)o2;
		    var expand = (bool?)o3;
		    if ( arr == null || id == null || expand == null ) return null;
		    var l = arr.ToList();
		    if ( expand.Value ) l.Add( id.Value );
		    else l.Remove( id.Value );
		    return l.ToArray();
		  };*/
		
		Action<EditorWindow, int, bool> ac = ( window, i, arg3 ) =>
		{	if ( memWindow == null ) return;
		
			EditorWindow convertedWindow = null;
			
			// try {convertedWindow = Convert.ChangeType(window, memWindow)  as EditorWindow;}
			try {convertedWindow = window  as EditorWindow;}
			
			catch (Exception ex)
			{	Debug.Log(ex.Message + "\n\n" + ex.StackTrace);
			}
			
			if (convertedWindow == null) return;
			
			//var t = IS_HIERARCHY() ? tas.GetValue( gettedobject ) : m_TreeView == null ? null : m_TreeView.GetValue( gettedobject );
			var t =  m_TreeView( convertedWindow);
			
			if ( t == null ) return;
			
			var d = property.GetValue( t, null );
			
			if ( d == null ) return;
			
			SetExpanded.Invoke(d, new[] { (object)i, arg3 });
		};
		
		
		Action<EditorWindow, int, bool> expandWIthChildren = ( w, i, arg3 ) =>
		{	if ( memWindow == null ) return;
		
			EditorWindow convertedWindow = null;
			
			try {convertedWindow = Convert.ChangeType(w, memWindow)  as EditorWindow;}
			
			catch {return;}
			
			if (convertedWindow == null) return;
			
			// var t = IS_HIERARCHY() ? tas.GetValue( gettedobject ) : m_TreeView == null ? null : m_TreeView.GetValue( gettedobject );
			var t =  m_TreeView( convertedWindow );
			
			if ( t == null ) return;
			
			var d = property.GetValue( t, null );
			
			if ( d == null ) return;
			
			dataSetExpandWithChildrens.Invoke(d, new[] { (object)i, arg3 });
		};
		
		
		
		Action<IDescriptionRegistrator> dcrReg = ( o ) =>
		{	MOI.RegistrateDescription( o );
		};
		
		var init = new[]
		{	(object)"TFx10",
			this,
			ac,
			dup,
			dcrReg,
			expandWIthChildren
		};
		
#if HIERARCHY
		
		if ( IS_HIERARCHY() ) HierarchyExtensions.Utilities.InitializeUtilities( init.ToList(), this );
		
#endif
		
		
#if PROJECT
		
		if ( IS_PROJECT() ) ProjectExtensions.Utility.InitializeUtilities( init.ToList(), this );
		
#endif
		
		
	}
	
	
	
}
}
