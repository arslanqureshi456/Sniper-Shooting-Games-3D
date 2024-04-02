#if !UNITY_EDITOR
//#define NAMESPACE
#endif
//#define GETFAVORITEOLD

//#define USEDIRTY

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using EModules;
using EModules.EModulesInternal;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

#if !PROJECT
	using A = EModules.EModulesInternal;
#else
	using A = EModules.EProjectInternal;
#endif

#if PROJECT
	using EModules.Project;
#endif



namespace EModules.EModulesInternal {
internal partial class Adapter {
	internal static string ReadLibraryFile( string fileName )
	{	if ( !Directory.Exists( UNITY_SYSTEM_PATH + "Library" ) ) Directory.CreateDirectory( UNITY_SYSTEM_PATH + "Library" );
	
		if ( !Directory.Exists( UNITY_SYSTEM_PATH + "Library/EModules" ) ) Directory.CreateDirectory( UNITY_SYSTEM_PATH + "Library/EModules" );
		
		if ( !File.Exists( UNITY_SYSTEM_PATH + "Library/EModules/" + fileName ) ) return "";
		
		return File.ReadAllText( UNITY_SYSTEM_PATH + "Library/EModules/" + fileName );
	}
	
	internal static void WriteLibraryFile( string fileName, ref System.Text.StringBuilder content )
	{	if ( !Directory.Exists( UNITY_SYSTEM_PATH + "Library" ) ) Directory.CreateDirectory( UNITY_SYSTEM_PATH + "Library" );
	
		if ( !Directory.Exists( UNITY_SYSTEM_PATH + "Library/EModules" ) ) Directory.CreateDirectory( UNITY_SYSTEM_PATH + "Library/EModules" );
		
		File.WriteAllText( UNITY_SYSTEM_PATH + "Library/EModules/" + fileName, content.ToString() );
	}
	
	internal static void RemoveLibraryFile( string fileName )
	{	if ( !Directory.Exists( UNITY_SYSTEM_PATH + "Library" ) ) return;
	
		if ( !Directory.Exists( UNITY_SYSTEM_PATH + "Library/EModules" ) ) return;
		
		if ( !File.Exists( UNITY_SYSTEM_PATH + "Library/EModules/" + fileName ) ) return;
		
		File.Delete( UNITY_SYSTEM_PATH + "Library/EModules/" + fileName );
	}
}
}



namespace EModules.EModulesInternal {

// #if NAMESPACE
//
//
// #if PROJECT
// namespace EModules.EProjectInternal {
// #else
// namespace EModules.EModulesInternal {
// #endif
//
//
// #endif


public class Hierarchy_GUI : ScriptableObject/*, ISerializable, IDeserializationCallback*/, PluginIDField {



	int PluginIDField.pluginID
	{	get { return pluginID; }
	
		set { pluginID = value; }
	}
	[HideInInspector, SerializeField]    internal int pluginID;
	
	
#pragma warning disable
	[HideInInspector, SerializeField]    List<string> listValue = new List<string>();
	[HideInInspector, SerializeField]    bool WasCustomNewApply = false;
#pragma warning restore
	
	[NonSerialized]  DoubleList<int, Vector2> __ScrollMemory;
	internal DoubleList<int, Vector2> ScrollMemory
	{	get
		{	if ( __ScrollMemory == null )
			{	__ScrollMemory = new DoubleList<int, Vector2>();
				__ScrollMemory.listKeys = ScrollMemoryKeys;
				__ScrollMemory.listValues = ScrollMemoryValues;
			}
			
			return __ScrollMemory;
		}
	}
	[HideInInspector, SerializeField]      List<int> ScrollMemoryKeys = new List<int>();
	[HideInInspector, SerializeField]      List<Vector2> ScrollMemoryValues = new List<Vector2>();
	
	
	
	
	
	
	
	
	
	/// ////////////////////////////////////////////////////////////////////
	/// ////////////////////////////////////////////////////////////////////
	/// ////////////////////////////////////////////////////////////////////
	/// ////////////////////OLD DATA////////////////////////////////////////
	/// ////////////////////////////////////////////////////////////////////
	/// ////////////////////////////////////////////////////////////////////
	/// ////////////////////////////////////////////////////////////////////
	//Temp Scriptable Always
#pragma warning disable
	[ SerializeField]     int[] _prefab_key = new int[500];
	[ SerializeField]     UnityEngine.Object[] _prefab_value = new UnityEngine.Object[500];
#pragma warning restore
	/* int[] GET_prefab_key
	 {   get { return EModulesHierarchy_ProjectSettings_Temp.Instance( adapter ).GET_prefab_key; }
	     set { EModulesHierarchy_ProjectSettings_Temp.Instance( adapter ).GET_prefab_key = value; }
	 }
	 UnityEngine.Object[] GET_prefab_value
	 {   get { return EModulesHierarchy_ProjectSettings_Temp.Instance( adapter ).GET_prefab_value; }
	     set { EModulesHierarchy_ProjectSettings_Temp.Instance( adapter ).GET_prefab_value = value; }
	 }*/
	internal EModulesHierarchy_ProjectSettings_Temp.CachedArray GET_prefab { get { return EModulesHierarchy_ProjectSettings_Temp.Instance( adapter ).GET_PREFABS; } }
	
	Adapter adapter { get { return Initializator.AdaptersByID[pluginID]; } }
	
	
	//Library Always
#pragma warning disable
	[HideInInspector, SerializeField]    internal string PRESETS_DATA;
	[HideInInspector, SerializeField]    internal float PRESETS_SCROLL_CAT;
	[HideInInspector, SerializeField]    internal float PRESETS_SCROLL_PRES;
	[HideInInspector, SerializeField]    internal int PRESETS_SELECT;
	[HideInInspector, SerializeField]    List<DataKeeperValue> prop_m_DataKeeper_Values = new List<DataKeeperValue>();
	[HideInInspector, SerializeField]  PrefabIDModeEnum m_PrefabIDMode = PrefabIDModeEnum.NotInitialized ;
	[HideInInspector, SerializeField]    internal bool WAS_NEW_INIT;
	[HideInInspector, SerializeField]    internal string __SaveToScriptableObject;
	[HideInInspector, SerializeField]    internal bool USE_REGISTRATOR;
	[HideInInspector, SerializeField]    List<SceneId> last_scenesV2 = new List<SceneId>();
	[HideInInspector, SerializeField]    List<string> m_hiddenComponents = new List<string>();
	[HideInInspector, SerializeField]    List<string> modulesParamsType = new List<string>();
	[HideInInspector, SerializeField]    List<Adapter.MyStruct> modulesParamsClass = new List<Adapter.MyStruct>();
	[HideInInspector, SerializeField]    int[] _mFavoritWindowParams_keys = null;
#pragma warning restore
#if GETFAVORITEOLD
	[HideInInspector, SerializeField]    FavoritWindowParams[] _mFavoritWindowParams_values = null;
#endif
	
	
	
	
	internal string GETPRESETS_DATA { get { LibraryAlwaysStringRead(); return LA_PRESETS_DATA; } set { LibraryAlwaysStringRead(); LA_PRESETS_DATA = value; } }
	internal float GETPRESETS_SCROLL_CAT { get { LibraryAlwaysStringRead(); return LA_PRESETS_SCROLL_CAT; } set { LibraryAlwaysStringRead(); LA_PRESETS_SCROLL_CAT = value; } }
	internal float GETPRESETS_SCROLL_PRES { get { LibraryAlwaysStringRead(); return LA_PRESETS_SCROLL_PRES; } set { LibraryAlwaysStringRead(); LA_PRESETS_SCROLL_PRES = value; } }
	internal int GETPRESETS_SELECT { get { LibraryAlwaysStringRead(); return LA_PRESETS_SELECT; } set { LibraryAlwaysStringRead(); LA_PRESETS_SELECT = value; } }
	List<DataKeeperValue> GETprop_m_DataKeeper_Values { get { LibraryAlwaysStringRead(); return LA_prop_m_DataKeeper_Values; } set { LibraryAlwaysStringRead(); LA_prop_m_DataKeeper_Values = value; } }
	PrefabIDModeEnum GETm_PrefabIDMode { get { LibraryAlwaysStringRead(); return LA_m_PrefabIDMode; } set { LibraryAlwaysStringRead(); LA_m_PrefabIDMode = value; } }
	internal bool GETWAS_NEW_INIT { get { LibraryAlwaysStringRead(); return LA_WAS_NEW_INIT; } set { LibraryAlwaysStringRead(); LA_WAS_NEW_INIT = value; } }
	internal string GET__SaveToScriptableObject { get { LibraryAlwaysStringRead(); return LA___SaveToScriptableObject; } set { LibraryAlwaysStringRead(); LA___SaveToScriptableObject = value; } }
	internal bool GETUSE_REGISTRATOR { get { LibraryAlwaysStringRead(); return LA_USE_REGISTRATOR; } set { LibraryAlwaysStringRead(); LA_USE_REGISTRATOR = value; } }
	List<SceneId> GETlast_scenesV2 { get { LibraryAlwaysStringRead(); return LA_last_scenesV2; } set { LibraryAlwaysStringRead(); LA_last_scenesV2 = value; } }
	List<string> GETm_hiddenComponents { get { LibraryAlwaysStringRead(); return LA_m_hiddenComponents; } set { LibraryAlwaysStringRead(); LA_m_hiddenComponents = value; } }
	List<string> GETmodulesParamsType { get { LibraryAlwaysStringRead(); return LA_modulesParamsType; } set { LibraryAlwaysStringRead(); LA_modulesParamsType = value; } }
	List<Adapter.MyStruct> GETmodulesParamsClass { get { LibraryAlwaysStringRead(); return LA_modulesParamsClass; } set { LibraryAlwaysStringRead(); LA_modulesParamsClass = value; } }
	
#if GETFAVORITEOLD
	int[] GET_mFavoritWindowParams_keys {get {LibraryAlwaysStringRead(); return LA__mFavoritWindowParams_keys;} set {LibraryAlwaysStringRead(); LA__mFavoritWindowParams_keys = value; } }
	FavoritWindowParams[] GET_mFavoritWindowParams_values  {get {LibraryAlwaysStringRead(); return LA__mFavoritWindowParams_values;} set {LibraryAlwaysStringRead(); LA__mFavoritWindowParams_values = value; } }
#endif
	
	
	[NonSerialized] bool  LA_AssignedDefault;
	[NonSerialized] bool  LA_readed;
	[NonSerialized]    internal string LA_PRESETS_DATA;
	[NonSerialized]    internal float LA_PRESETS_SCROLL_CAT;
	[NonSerialized]    internal float LA_PRESETS_SCROLL_PRES;
	[NonSerialized]    internal int LA_PRESETS_SELECT;
	[NonSerialized]    List<DataKeeperValue> LA_prop_m_DataKeeper_Values = new List<DataKeeperValue>();
	[NonSerialized]  PrefabIDModeEnum ___LA_m_PrefabIDMode = PrefabIDModeEnum.NotInitialized ;
	PrefabIDModeEnum LA_m_PrefabIDMode
	{	get { return ___LA_m_PrefabIDMode; }
	
		set
		{	___LA_m_PrefabIDMode = value;
			// if (pluginID == 0) Debug.Log(value);
		}
	}
	[NonSerialized]    internal bool LA_WAS_NEW_INIT;
	[NonSerialized]    internal string LA___SaveToScriptableObject;
	[NonSerialized]    internal bool LA_USE_REGISTRATOR;
	[NonSerialized]    List<SceneId> LA_last_scenesV2 = new List<SceneId>();
	[NonSerialized]    List<string> LA_m_hiddenComponents = new List<string>();
	[NonSerialized]    List<string> LA_modulesParamsType = new List<string>();
	[NonSerialized]    List<Adapter.MyStruct> LA_modulesParamsClass = new List<Adapter.MyStruct>();
	[NonSerialized]    int[] LA__mFavoritWindowParams_keys = new int[0];
#if GETFAVORITEOLD
	[NonSerialized]    FavoritWindowParams[] LA__mFavoritWindowParams_values = new FavoritWindowParams[0];
#endif
	void LA_CheckLists()
	{	if ( LA_prop_m_DataKeeper_Values == null ) LA_prop_m_DataKeeper_Values = new List<DataKeeperValue>();
	
		if ( LA_last_scenesV2 == null ) LA_last_scenesV2 = new List<SceneId>();
		
		if ( LA_m_hiddenComponents == null ) LA_m_hiddenComponents = new List<string>();
		
		if ( LA_modulesParamsType == null ) LA_modulesParamsType = new List<string>();
		
		if ( LA_modulesParamsClass == null ) LA_modulesParamsClass = new List<Adapter.MyStruct>();
		
		if ( LA__mFavoritWindowParams_keys == null ) LA__mFavoritWindowParams_keys = new int[0];
		
#if GETFAVORITEOLD
		
		if (LA__mFavoritWindowParams_values == null) LA__mFavoritWindowParams_values = new FavoritWindowParams[0];
		
#endif
	}
	
	void LibraryAlwaysStringSetDirty( Adapter adapter )
	{	if ( !LA_readed ) LibraryAlwaysStringRead();
	
		var result = new System.Text.StringBuilder();
		LA_CheckLists();
		result.AppendLine( LA_AssignedDefault.ToString() );
		result.AppendLine( LA_PRESETS_DATA );
		result.AppendLine( LA_PRESETS_SCROLL_CAT.ToString() );
		result.AppendLine( LA_PRESETS_SCROLL_PRES.ToString() );
		result.AppendLine( LA_PRESETS_SELECT.ToString() );
		
		foreach ( var item in LA_prop_m_DataKeeper_Values ) if ( item != null ) item.SaveToString( ref result ); //
		
		result.AppendLine( "---" );
		
		result.AppendLine( ((int)LA_m_PrefabIDMode).ToString() );
		result.AppendLine( LA_WAS_NEW_INIT.ToString() );
		result.AppendLine( LA___SaveToScriptableObject );
		result.AppendLine( LA_USE_REGISTRATOR.ToString() );
		
		foreach ( var item in LA_last_scenesV2 ) if ( item != null ) result.AppendLine( Adapter.SERIALIZE_SINGLE( item ) ); //
		
		result.AppendLine( "---" );
		
		foreach ( var item in LA_m_hiddenComponents ) if ( item != null ) result.AppendLine( item ); //
		
		result.AppendLine( "---" );
		
		foreach ( var item in LA_modulesParamsType ) if ( item != null ) result.AppendLine( item ); //
		
		result.AppendLine( "---" );
		
		foreach ( var item in LA_modulesParamsClass ) if ( item != null ) item.SaveToString( ref result ); //
		
		result.AppendLine( "---" );
		
		foreach ( var item in LA__mFavoritWindowParams_keys ) result.AppendLine( item.ToString() ); //
		
		result.AppendLine( "---" );
#if GETFAVORITEOLD
		result.AppendLine(Adapter.SERIALIZE_SINGLE(LA__mFavoritWindowParams_values));
#else
		result.AppendLine( " - " );
#endif
		
		Adapter.WriteLibraryFile( adapter.pluginID == Initializator.HIERARCHY_ID ? "HierarchySettings_UserInterface" : "ProjectSettings_UserInterface", ref result );
	}
	void LibraryAlwaysStringRead()
	{	if ( LA_readed ) return;
	
		LA_readed = true;
		var f = Adapter.ReadLibraryFile(pluginID == Initializator.HIERARCHY_ID ? "HierarchySettings_UserInterface" : "ProjectSettings_UserInterface");
		LA_CheckLists();
		
		if ( !string.IsNullOrEmpty( f ) )
		{	var reader = new System.IO.StringReader(f);
			string line;
			
			try
			{	LA_AssignedDefault = bool.Parse( reader.ReadLine() );
				LA_PRESETS_DATA = reader.ReadLine();
				LA_PRESETS_SCROLL_CAT = float.Parse( reader.ReadLine() );
				LA_PRESETS_SCROLL_PRES = float.Parse( reader.ReadLine() );
				LA_PRESETS_SELECT = int.Parse( reader.ReadLine() );
				
				while ( (line = reader.ReadLine()) != "---" ) LA_prop_m_DataKeeper_Values.Add( DataKeeperValue.ReadFromString( ref reader ) );
				
				LA_m_PrefabIDMode = (PrefabIDModeEnum)int.Parse( reader.ReadLine() );
				LA_WAS_NEW_INIT = bool.Parse( reader.ReadLine() );
				LA___SaveToScriptableObject = (reader.ReadLine());
				LA_USE_REGISTRATOR = bool.Parse( reader.ReadLine() );
				
				while ( (line = reader.ReadLine()) != "---" ) LA_last_scenesV2.Add( Adapter.DESERIALIZE_SINGLE<SceneId>( line ) );
				
				while ( (line = reader.ReadLine()) != "---" ) LA_m_hiddenComponents.Add( (line) );
				
				while ( (line = reader.ReadLine()) != "---" ) LA_modulesParamsType.Add( (line) );
				
				while ( (line = reader.ReadLine()) != "---" ) LA_modulesParamsClass.Add( Adapter.MyStruct.ReadFromString( ref reader ) );
				
				while ( (line = reader.ReadLine()) != "---" )
				{	Array.Resize( ref LA__mFavoritWindowParams_keys, LA__mFavoritWindowParams_keys.Length + 1 );
					LA__mFavoritWindowParams_keys[LA__mFavoritWindowParams_keys.Length - 1] = int.Parse( line );
				}
				
#if GETFAVORITEOLD
				LA__mFavoritWindowParams_values = Adapter.DESERIALIZE_SINGLE<FavoritWindowParams[]>(reader.ReadLine());
#else
				reader.ReadLine();
#endif
			}
			
			catch ( Exception ex )
			{	Debug.LogError( ex.Message + "\n\n" + ex.StackTrace );
			}
			
			reader.Dispose();
		}
		
		
		if ( !LA_AssignedDefault )
		{	LA_AssignedDefault = true;
			LA_PRESETS_DATA = PRESETS_DATA;
			LA_PRESETS_SCROLL_CAT = PRESETS_SCROLL_CAT;
			LA_PRESETS_SCROLL_PRES = PRESETS_SCROLL_PRES;
			LA_PRESETS_SELECT = PRESETS_SELECT;
			LA_prop_m_DataKeeper_Values = prop_m_DataKeeper_Values;
			LA_m_PrefabIDMode = m_PrefabIDMode;
			LA_WAS_NEW_INIT = WAS_NEW_INIT;
			LA___SaveToScriptableObject = __SaveToScriptableObject;
			LA_USE_REGISTRATOR = USE_REGISTRATOR;
			LA_last_scenesV2 = last_scenesV2;
			LA_m_hiddenComponents = m_hiddenComponents;
			LA_modulesParamsType = modulesParamsType;
			LA_modulesParamsClass = modulesParamsClass;
			LA__mFavoritWindowParams_keys = _mFavoritWindowParams_keys;
#if GETFAVORITEOLD
			LA__mFavoritWindowParams_values = _mFavoritWindowParams_values;
#endif
		}
		
	}
	
	
	
	
	
	//EdtiorPrefsStrings
#pragma warning disable
	[HideInInspector, SerializeField]    List<string> lastIconSelect = new List<string>();
	[HideInInspector, SerializeField]    List<Color32> _lasHiglightBackGroundColor = new List<Color32>();
	[HideInInspector, SerializeField]    List<Color32> _lasHiglightTextColor = new List<Color32>();
#pragma warning restore
	
	[NonSerialized] bool  EP__readed;
	[NonSerialized] List<string> EP__lastIconSelect;
	[NonSerialized] List<Color32>  EP___lasHiglightBackGroundColor;
	[NonSerialized] List<Color32>  EP___lasHiglightTextColor;
	void EP__CheckLists()
	{	if ( EP__lastIconSelect == null ) EP__lastIconSelect = new List<string>();
	
		if ( EP___lasHiglightBackGroundColor == null ) EP___lasHiglightBackGroundColor = new List<Color32>();
		
		if ( EP___lasHiglightTextColor == null ) EP___lasHiglightTextColor = new List<Color32>();
	}
	void EdtiorPrefsStringSetDirty( Adapter adapter )
	{	if ( !EP__readed ) EdtiorPrefsStringRead();
	
		EP__CheckLists();
		var result = new System.Text.StringBuilder();
		
		foreach ( var item in EP__lastIconSelect ) result.AppendLine( item ); //
		
		result.AppendLine( "---" );
		
		foreach ( var item in EP___lasHiglightBackGroundColor ) result.AppendLine( Adapter.ColorToString( item ) ); //
		
		result.AppendLine( "---" );
		
		foreach ( var item in EP___lasHiglightTextColor ) result.AppendLine( Adapter.ColorToString( item ) ); //
		
		Adapter.WriteLibraryFile( adapter.pluginID == Initializator.HIERARCHY_ID ? "HierarchySettings_LastColors" : "ProjectSettings_LastColors", ref result );
	}
	
	List<string> GETlastIconSelect { get { EdtiorPrefsStringRead(); return EP__lastIconSelect; } set { EdtiorPrefsStringRead(); EP__lastIconSelect = value; } }
	List<Color32> GET_lasHiglightBackGroundColor { get { EdtiorPrefsStringRead(); return EP___lasHiglightBackGroundColor; } set { EdtiorPrefsStringRead(); EP___lasHiglightBackGroundColor = value; } }
	List<Color32> GET_lasHiglightTextColor { get { EdtiorPrefsStringRead(); return EP___lasHiglightTextColor; } set { EdtiorPrefsStringRead(); EP___lasHiglightTextColor = value; } }
	void EdtiorPrefsStringRead()
	{	if ( EP__readed ) return;
	
		EP__readed = true;
		var f = Adapter.ReadLibraryFile(pluginID == Initializator.HIERARCHY_ID ? "HierarchySettings_LastColors" : "ProjectSettings_LastColors");
		EP__CheckLists();
		
		var reader = new System.IO.StringReader(f);
		{	string line;
		
			while ( (line = reader.ReadLine()) != "---" && line != null ) EP__lastIconSelect.Add( line );
			
			while ( (line = reader.ReadLine()) != "---" && line != null ) EP___lasHiglightBackGroundColor.Add( Adapter.ColorFromString( line ) );
			
			while ( (line = reader.ReadLine()) != null ) EP___lasHiglightTextColor.Add( Adapter.ColorFromString( line ) );
		}
		reader.Dispose();
	}
	
	
	//EModulesHierarchy_ProjectSettings_ComponentsIcons
	[HideInInspector, SerializeField]    List<string> listKey = new List<string>();
	[HideInInspector, SerializeField]    List<CustomIconParams> listValueNew = new List<CustomIconParams>();
	List<string> GETlistKey { get { EdtiorPrefsStringRead(); return EModulesHierarchy_ProjectSettings_ComponentsIcons.Instance( adapter ).listKey; } set { EdtiorPrefsStringRead(); EModulesHierarchy_ProjectSettings_ComponentsIcons.Instance( adapter ).listKey = value; } }
	List<CustomIconParams> GETlistValueNew { get { EdtiorPrefsStringRead(); return EModulesHierarchy_ProjectSettings_ComponentsIcons.Instance( adapter ).listValueNew; } set { EdtiorPrefsStringRead(); EModulesHierarchy_ProjectSettings_ComponentsIcons.Instance( adapter ).listValueNew = value; } }
	void ComponentsIconsSetDirty( Adapter adapter )
	{	if ( !EModulesHierarchy_ProjectSettings_ComponentsIcons.Instance( adapter ).Initialized ) ComponentsIconsStringRead( adapter );
	
		EModulesHierarchy_ProjectSettings_ComponentsIcons.SetDirty( adapter );
	}
	void ComponentsIconsStringRead( Adapter adapter )
	{	if ( EModulesHierarchy_ProjectSettings_ComponentsIcons.Instance( adapter ).Initialized ) return;
	
		EModulesHierarchy_ProjectSettings_ComponentsIcons.Instance( adapter ).Initialized = true;
		EModulesHierarchy_ProjectSettings_ComponentsIcons.Instance( adapter ).listKey = listKey;
		EModulesHierarchy_ProjectSettings_ComponentsIcons.Instance( adapter ).listValueNew = listValueNew;
		EModulesHierarchy_ProjectSettings_ComponentsIcons.SetDirty( adapter );
	}
	
	
	//EModulesHierarchy_ProjectSettings_HighLighter
	[SerializeField, HideInInspector] List<Adapter.ColorFilter> _colorFilters = new List<Adapter.ColorFilter>();
	
	List<Adapter.ColorFilter> GET_colorFilters { get { HighLighterStringRead( adapter ); return EModulesHierarchy_ProjectSettings_HighLighter.Instance( adapter )._colorFilters; } set { HighLighterStringRead( adapter ); EModulesHierarchy_ProjectSettings_HighLighter.Instance( adapter )._colorFilters = value; } }
	
	void HighLighterSetDirty( Adapter adapter )
	{	if ( !EModulesHierarchy_ProjectSettings_HighLighter.Instance( adapter ).Initialized ) HighLighterStringRead( adapter );
	
		EModulesHierarchy_ProjectSettings_HighLighter.SetDirty( adapter );
	}
	void HighLighterStringRead( Adapter adapter )
	{	if ( EModulesHierarchy_ProjectSettings_HighLighter.Instance( adapter ).Initialized ) return;
	
		EModulesHierarchy_ProjectSettings_HighLighter.Instance( adapter ).Initialized = true;
		EModulesHierarchy_ProjectSettings_HighLighter.Instance( adapter )._colorFilters = _colorFilters;
		EModulesHierarchy_ProjectSettings_HighLighter.SetDirty( adapter );
	}
	
	/// ////////////////////////////////////////////////////////////////////
	/// ////////////////////////////////////////////////////////////////////
	/// ////////////////////////////////////////////////////////////////////
	/// ////////////////////OLD DATA////////////////////////////////////////
	/// ////////////////////////////////////////////////////////////////////
	/// ////////////////////////////////////////////////////////////////////
	/// ////////////////////////////////////////////////////////////////////
	///
	
	
	
	
	
	
	internal static void Undo( Adapter adapter, string str )
	{	/* var g = Initialize();
		 if ( g == null ) return;*/
		// LogProxy.Log("REC");
		if ( adapter == null ) return;
		
		EModulesHierarchy_ProjectSettings_HighLighter.SetUndo( adapter, str );
		EModulesHierarchy_ProjectSettings_ComponentsIcons.SetUndo( adapter, str );
		// UnityEditor.Undo.RecordObject( fa.m_cache[adapter.pluginID], str );
	}
	internal static void SetDirtyObject( Adapter adapter )
	{	/*  var g = Initialize();
		  if ( g == null ) return;*/
		// LogProxy.Log("d");
		
		if ( adapter == null ) return;
		
#if USEDIRTY
		Adapter.SetDirty( fa.m_cache[adapter.pluginID] );
#endif
		Instance( adapter ).HighLighterSetDirty( adapter );
		Instance( adapter ).ComponentsIconsSetDirty( adapter );
		Instance( adapter ).EdtiorPrefsStringSetDirty( adapter );
		Instance( adapter ).LibraryAlwaysStringSetDirty( adapter );
	}
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	[Serializable]
	internal class CustomIconParams {
		internal CustomIconParams()
		{	color = Color.white;
		}
		[SerializeField]
		internal string value;
		[SerializeField]
		internal Color color;
		
		
		internal void SaveToString( ref System.Text.StringBuilder result )
		{	result.AppendLine( value.ToString() );
			result.AppendLine( Adapter.ColorToString( ref color ) );
		}
		internal static CustomIconParams ReadFromString( ref System.IO.StringReader reader )
		{	var result = new CustomIconParams();
			result.value = (reader.ReadLine());
			result.color = Adapter.ColorFromString( reader.ReadLine() );
			return result;
		}
	}
	
	
	[Serializable]
	internal class FavoritWindowParams {
		[SerializeField]
		internal int[] expandedIds = new int[0];
		
		internal void Save( Adapter adapter )
		{	SetDirtyObject( adapter );
		}
		internal void Undo( Adapter adapter, string undoName )
		{	Undo( adapter, undoName );
		}
	}
	[Serializable]
	internal enum PrefabIDModeEnum { NotInitialized = 0, SeparateInstances = 1, MergedInstances = 2 }
	
	// /* DATA KEEPER */
	internal List<DataKeeperValue> m_DataKeeper_Values
	{	get { return GETprop_m_DataKeeper_Values ?? (GETprop_m_DataKeeper_Values = new List<DataKeeperValue>()); }
	}
	[Serializable]
	internal class DataKeeperValue {
		[SerializeField]
		internal MonoScript value;
		
		
		internal void SaveToString( ref System.Text.StringBuilder result )
		{	result.AppendLine( "-" );
			result.AppendLine( !value ? "" : string.IsNullOrEmpty( AssetDatabase.GetAssetPath( value ) ) ? "" : AssetDatabase.AssetPathToGUID( AssetDatabase.GetAssetPath( value ) ) );
		}
		internal static DataKeeperValue ReadFromString( ref System.IO.StringReader reader )
		{	var result = new DataKeeperValue();
			var icon = reader.ReadLine();
			var path = string.IsNullOrEmpty(icon) ? "" : AssetDatabase.GUIDToAssetPath(icon);
			result.value = string.IsNullOrEmpty( path ) ? null : AssetDatabase.LoadAssetAtPath<MonoScript>( path );
			return result;
		}
	}
	
	
	
	
	
	
	
	
	
	
	
	
	
#if GETFAVORITEOLD
	internal FavoritWindowParams Get_FavoritWindowParams(int key)
	{	if (GET_mFavoritWindowParams_keys == null) GET_mFavoritWindowParams_keys = new int[0];
	
		if (GET_mFavoritWindowParams_values == null) GET_mFavoritWindowParams_values = new FavoritWindowParams[0];
		
		if (GET_mFavoritWindowParams_keys.Length != GET_mFavoritWindowParams_values.Length)
		{	var min = Math.Min(GET_mFavoritWindowParams_keys.Length, GET_mFavoritWindowParams_values.Length);
			var k = GET_mFavoritWindowParams_keys;
			Array.Resize( ref k, min );
			GET_mFavoritWindowParams_keys = k;
			var v = GET_mFavoritWindowParams_values;
			Array.Resize( ref v, min );
			GET_mFavoritWindowParams_values = v;
		}
		
		var indexOf = -1;
		
		for (int i = 0 ; i < GET_mFavoritWindowParams_values.Length ; i++)
		{	if (GET_mFavoritWindowParams_values[i] == null) GET_mFavoritWindowParams_values[i] = new FavoritWindowParams();
		
			if (GET_mFavoritWindowParams_keys[i] == key) indexOf = i;
		}
		
		if (indexOf == -1)
		{	indexOf = GET_mFavoritWindowParams_keys.Length;
			var k = GET_mFavoritWindowParams_keys;
			Array.Resize( ref k, indexOf + 1 );
			GET_mFavoritWindowParams_keys = k;
			var v = GET_mFavoritWindowParams_values;
			Array.Resize( ref v, indexOf + 1 );
			GET_mFavoritWindowParams_values = v;
			GET_mFavoritWindowParams_keys[indexOf] = key;
			GET_mFavoritWindowParams_values[indexOf] = new FavoritWindowParams();
		}
		
		return GET_mFavoritWindowParams_values[indexOf];
	}
#endif
	
	[NonSerialized] bool? cacheSaveToScriptableObject;
	internal PrefabIDModeEnum PrefabIDMode
	{	get
		{	if ( pluginID != 0 ) return PrefabIDModeEnum.SeparateInstances;
		
			if ( cacheSaveToScriptableObject ?? (cacheSaveToScriptableObject = SaveToScriptableObject == "FOLDER").Value ) return PrefabIDModeEnum.MergedInstances;
			
			if ( GETm_PrefabIDMode == PrefabIDModeEnum.NotInitialized || Adapter.HierAdapter.par.ENABLE_REGISTRATORT ) return PrefabIDModeEnum.SeparateInstances;
			
			return GETm_PrefabIDMode;
		}
		
		set
		{	GETm_PrefabIDMode = value;
		}
	}
	
	[NonSerialized]     internal bool TargetChecked = false;
	internal string SaveToScriptableObject
	{	get
		{
#pragma warning disable
		
			if ( !Adapter.ALLOW_FOLDER_SAVER ) return "SCENE";
			
#pragma warning restore
			return GET__SaveToScriptableObject;
		}
		
		set
		{	if ( !Adapter.ALLOW_FOLDER_SAVER || Application.isPlaying ) return;
		
			GET__SaveToScriptableObject = value;
		}
	}
	
	
	
	[NonSerialized] Dictionary<Type, bool> m_dataKeeperBake = null;
	void DataKeeper_Init()
	{	prop_m_dataKeeperObjects = null;
		m_dataKeeperBake = new Dictionary<Type, bool>();
		
		for ( int i = 0 ; i < m_DataKeeper_Values.Count ; i++ )
		{	if ( m_DataKeeper_Values[i].value && m_DataKeeper_Values[i].value.GetClass() != null )
			{	m_dataKeeperBake.Add( m_DataKeeper_Values[i].value.GetClass(), true );
			}
		}
	}
	
	internal void DataKeeper_AddScript( MonoScript ms )
	{	if ( ms != null && ms.GetClass() == null ) return;
	
		m_DataKeeper_Values.Add( new DataKeeperValue() { value = ms } );
		
		DataKeeper_Init();
	}
	internal void DataKeeper_InsertScript( int index, MonoScript ms )
	{	if ( ms != null && ms.GetClass() == null ) return;
	
		m_DataKeeper_Values.Insert( index, new DataKeeperValue() { value = ms } );
		
		DataKeeper_Init();
	}
	internal bool DataKeeper_HasScript( UnityEngine.Object ms )
	{	if ( m_dataKeeperBake == null ) DataKeeper_Init();
	
		return m_dataKeeperBake.ContainsKey( (ms.GetType()) );
	}
	internal void DataKeeper_SetScript( int index, MonoScript ms )
	{	if ( ms != null && ms.GetClass() == null ) return;
	
		if ( index > m_DataKeeper_Values.Count ) return;
		
		m_DataKeeper_Values[index].value = ms;
		
		DataKeeper_Init();
	}
	internal void DataKeeper_RemoveAt( int index )
	{	if ( index > m_DataKeeper_Values.Count ) return;
	
		m_DataKeeper_Values.RemoveAt( index );
		
		DataKeeper_Init();
	}
	
	[NonSerialized] Dictionary<GameObject, bool> prop_m_dataKeeperObjects = new Dictionary<GameObject, bool>();
	internal Dictionary<GameObject, bool> m_dataKeeperObjects
	{	get { return prop_m_dataKeeperObjects ?? (prop_m_dataKeeperObjects = new Dictionary<GameObject, bool>()); }
	}
	internal bool DataKeeper_IsObjectIncluded( GameObject o )
	{	if ( !m_dataKeeperObjects.ContainsKey( o ) )
		{	var comps =  HierarchyExtensions.Utilities.GetComponentFast<Component>.GetAll( o );
			var contains = false;
			
			for ( int i = 0 ; i < comps.Length ; i++ )
			{	if ( !comps[i] ) continue;
			
				if ( DataKeeper_HasScript( (comps[i]) ) )
				{	contains = true;
					break;
				}
			}
			
			m_dataKeeperObjects.Add( o, contains );
		}
		
		return m_dataKeeperObjects[o];
	}
	internal void DataKeeper_Check( GameObject o, Component[] comps )
	{
	
		bool find = false;
		
		foreach ( var component in comps )
		{	if ( !component ) continue;
		
			if ( DataKeeper_HasScript( (component) ) )
			{	find = true;
				break;
			}
		}
		
		var has = DataKeeper_IsObjectIncluded( o );
		
		if ( has != find )
			m_dataKeeperObjects[o] = find;
			
	}
	// /* DATA KEEPER */
	
	
	
	internal List<Adapter.ColorFilter> ColorFilters { get { return GET_colorFilters ?? (GET_colorFilters = new List<Adapter.ColorFilter>()); } }
	
	
	
	[NonSerialized]
	Dictionary<string, int> bakeDic = new Dictionary<string, int>();
	/* [NonSerialized]
	  Hierarchy_GUI _get;*/
	
	internal static List<SceneId> GetLastScenes( Adapter adapter )     //MonoBehaviour.print(_get == null ? "null" : _get.ToString());
	{	cache = fa.m_cache[adapter.pluginID];
	
		if ( cache == null ) return new List<SceneId>();
		
		return cache.GETlast_scenesV2 ?? (cache.GETlast_scenesV2 = new List<SceneId>());
	}
	
	
	
	
	internal List<string> HiddenComponents
	{	get
		{	if ( GETm_hiddenComponents == null ) GETm_hiddenComponents = new List<string>();
		
			return GETm_hiddenComponents;
		}
	}
	internal static bool HideIcon( string toString, Adapter adapter )
	{	cache = fa.m_cache[adapter.pluginID];
	
		if ( cache == null ) return false;
		
		if ( cache.GETm_hiddenComponents == null ) return false;
		
		return cache.GETm_hiddenComponents.Contains( toString );
	}
	
	[NonSerialized]
	DoubleList<string, CustomIconParams> res = new DoubleList<string, CustomIconParams>();  /* { listKeys = listKey, listValues = listValue }*/
	DoubleList<string, CustomIconParams> customIcons( Adapter adapter )
	{	cache = fa.m_cache[adapter.pluginID];
		/*if (!cache.GETWasCustomNewApply)
		{   cache.GETWasCustomNewApply = true;
		    cache.GETlistValueNew = cache.GETlistValue.Select( v => new CustomIconParams() { value = v } ).ToList();
		}*/
		res.listKeys = cache.GETlistKey;
		res.listValues = cache.GETlistValueNew;
		return res;
	}
	
	
	internal void Bake( Adapter adapter )
	{	if ( GETmodulesParamsType.Count != GETmodulesParamsClass.Count )     // wasI = true;
		{	while ( GETmodulesParamsType.Count != GETmodulesParamsClass.Count )
			{	if ( GETmodulesParamsType.Count < GETmodulesParamsClass.Count ) GETmodulesParamsClass.RemoveAt( GETmodulesParamsClass.Count - 1 );
				else GETmodulesParamsClass.Add( new Adapter.MyStruct() );
			}
			
			/*GETmodulesParamsType.Clear();
			GETmodulesParamsClass.Clear();
			adapter.wasModulesInitialize = false;*/
			Debug.LogWarning( "Hierarchy Plugin - Modules out of sync" );
			SetDirtyObject( adapter );
		}
		
		bakeDic.Clear();
		
		for ( int i = 0 ; i < GETmodulesParamsType.Count ; i++ )
		{	if ( bakeDic.ContainsKey( GETmodulesParamsType[i] ) )     // MonoBehaviour.print(modulesParamsType.Count + " " + modulesParamsClass.Count);
			{	GETmodulesParamsType.RemoveAt( i );
				GETmodulesParamsClass.RemoveAt( i );
				i--;
				continue;
			}
			
			bakeDic.Add( GETmodulesParamsType[i], i );
		}
		
		
	}
	
	internal Adapter.MyStruct modulesParamsGetValList( ref string set )     //return modulesParamsClass[modulesParamsType.IndexOf(set)];
	{	if ( !bakeDic.ContainsKey( set ) )
		{	for ( int i = 0 ; i < GETmodulesParamsType.Count ; i++ )
			{	if ( bakeDic.ContainsKey( GETmodulesParamsType[i] ) )     // MonoBehaviour.print(modulesParamsType.Count + " " + modulesParamsClass.Count);
				{	GETmodulesParamsType.RemoveAt( i );
					GETmodulesParamsClass.RemoveAt( i );
					i--;
					continue;
				}
				
				bakeDic.Add( GETmodulesParamsType[i], i );
			}
		}
		
		return GETmodulesParamsClass[bakeDic[set]];
	}
	internal void modulesParamsGetValList( ref string set, Adapter.MyStruct value )      //modulesParamsClass[modulesParamsType.IndexOf(set)] = value;
	{	if ( !bakeDic.ContainsKey( set ) ) return;
	
		GETmodulesParamsClass[bakeDic[set]] = value;
	}
	internal bool modulesParamsContainsKey( ref string str )
	{	return GETmodulesParamsType.Contains( str );
		//  return bakeDic.ContainsKey(str);
	}
	internal void SortSibligPoses()     // Debug.Log( AssetDatabase.GetAssetPath(this) );
	{	int sibPos = 0;
	
		foreach ( var item in GETmodulesParamsClass.OrderBy( m => m.sib ) )
		{	if ( item.sib == -1 ) continue;
		
			item.sib = sibPos++;
			//Debug.Log( item.sib );
		}
		
		// EditorUtility.SetDirty(this);
		/* int sibPos = 0;
		 Debug.Log( modulesParamsClass[4].sib );
		 foreach (var item in modulesParamsClass.OrderBy( m => m.sib ))
		 {
		     if (item.sib == -1) continue;
		     item.sib = sibPos++;
		 }
		 Debug.Log( modulesParamsClass[4].sib );*/
		// EditorUtility.SetDirty(this);
	}
	
	
	
	internal DoubleList<string, Adapter.MyStruct> modulesParams
	{	get
		{	if ( GETmodulesParamsType == null )
			{	GETmodulesParamsType = new List<string>();
				GETmodulesParamsClass = new List<Adapter.MyStruct>();
			}
			
			var res = new DoubleList<string, Adapter.MyStruct>();
			res.listKeys = GETmodulesParamsType;
			res.listValues = GETmodulesParamsClass;
			
			return res;
		}
	}
	
	
	
	internal static DoubleList<string, CustomIconParams> Get( Adapter adaper )
	{	cache = fa.m_cache[adaper.pluginID];
	
		if ( cache == null ) return new DoubleList<string, CustomIconParams>();
		
		return cache.customIcons( adaper );
	}
	internal static List<string> GetLastList( Adapter adaper )
	{	cache = fa.m_cache[adaper.pluginID];
	
		if ( cache == null ) return new List<string>();
		
		return cache.GETlastIconSelect;
	}
	
	static List<Color32> GetDefaultList()
	{	var res = new List<Color32>()
		{	new Color32( 49, 58, 63, 255 ),
			  new Color32( 255, 77, 67, 255 ),
			  new Color32( 38, 42, 45, 255 ),
			  new Color32( 136, 202, 96, 255 ),
			  new Color32( 255, 111, 111, 255 ),
			  new Color32( 13, 66, 98, 255 ),
			  new Color32( 0, 204, 153, 255 ),
			  new Color32( 0, 101, 153, 255 ),
			  new Color32( 130, 181, 63, 255 ),
			  new Color32( 65, 139, 202, 255 ),
			  new Color32( 232, 119, 85, 255 ),
			  new Color32( 82, 62, 125, 255 ),
			  new Color32( 246, 126, 4, 255 ),
			  new Color32( 25, 168, 40, 255 ),
			  new Color32( 245, 186, 31, 255 ),
		} ;
		
		if ( EditorGUIUtility.isProSkin ) return res;
		
		return res.Select( c => (Color)c ).Select( c => (Color32)new Color( 1 - (1 - c.r) / 2, 1 - (1 - c.g) / 2, 1 - (1 - c.b) / 2, c.a ) ).ToList();
		//};
	}
	static List<Color32> GetDefaultList2()
	{	var res = new List<Color32>()
		{	new Color32( 58, 103, 100, 255 ),
			  new Color32( 245, 41, 78, 255 ),
			  new Color32( 98, 125, 196, 255 ),
			  new Color32( 207, 27, 39, 255 ),
			  new Color32( 225, 35, 69, 255 ),
			  new Color32( 54, 180, 70, 255 ),
			  new Color32( 241, 89, 16, 255 ),
			  new Color32( 45, 80, 112, 255 ),
			  new Color32( 226, 110, 33, 255 ),
			  new Color32( 1, 114, 132, 255 ),
			  new Color32( 137, 121, 96, 255 ),
			  new Color32( 61, 148, 139, 255 ),
			  new Color32( 149, 33, 44, 255 ),
			  new Color32( 224, 224, 224, 255 ),
			  new Color32( 1, 254, 211, 255 ),
		};
		
		if ( EditorGUIUtility.isProSkin ) return res;
		
		return res.Select( c => (Color)c ).Select( c => (Color32)new Color( 1 - (1 - c.r) / 2, 1 - (1 - c.g) / 2, 1 - (1 - c.b) / 2, c.a ) ).ToList();
		//};
	}
	
	internal static List<Color32> GetLastHiglightList( Adapter adaper )
	{	cache = fa.m_cache[adaper.pluginID];
	
		if ( cache == null ) return new List<Color32>();
		
		if ( cache.GET_lasHiglightBackGroundColor.Count < 1 ) cache.GET_lasHiglightBackGroundColor = GetDefaultList2();
		
		return cache.GET_lasHiglightBackGroundColor;
	}
	internal static List<Color32> GetLastHiglightTextList( Adapter adaper )
	{	cache = fa.m_cache[adaper.pluginID];
	
		if ( cache == null ) return new List<Color32>();
		
		if ( cache.GET_lasHiglightTextColor.Count < 1 ) cache.GET_lasHiglightTextColor = GetDefaultList();
		
		return cache.GET_lasHiglightTextColor;
	}
	
	
	
	
	
	[NonSerialized] static   Hierarchy_GUI cache = null;
	
	
	[NonSerialized] static  FileAssetInitializator<Hierarchy_GUI> fa = new FileAssetInitializator<Hierarchy_GUI>();
	[NonSerialized] internal static   Hierarchy_GUI HierarchySettings ;
	
	internal static Hierarchy_GUI Instance( Adapter adapter )
	{
	
	
		if ( fa.TryGetCachedAsset( adapter, ref cache ) && cache != null ) return cache;
		
		bool wasCreated;
		cache = fa.TryGetAsset( adapter, adapter.pluginID == Initializator.HIERARCHY_ID ? "HierarchySettings.asset" : "ProjectSettings.asset", out wasCreated, CreateFile : false, useOldLoader : true );
		/* if (wasCreated)
		 {   cache.PrefabIDMode = PrefabIDModeEnum.MergedInstances;
		     cache.GETWAS_NEW_INIT = true;
		     #if USEDIRTY
		     EditorUtility.SetDirty( cache );
		     #endif
		 }*/
		
		if ( adapter.pluginID == Initializator.HIERARCHY_ID )
		{	HierarchySettings = cache;
			CheckSaveTargetForHierarchy( cache );
		}
		
		
		/* OLD SAVER
		#if !PROJECT
		if (wasCreated)
		{   var clear = AssetDatabase.GetAllAssetPaths().Where( p => p.EndsWith( "s" ) ).Where( p => p.EndsWith( "HierarchyClearSavedDataHelper.cs" ) );
		
		   if (clear.Count() != 0)
		   {   clear = AssetDatabase.GetAllAssetPaths().Where( p => p.EndsWith( "s" ) ).Where( p => p.EndsWith( "HierarchyClearSavedDataHelper.cs" ) );
		       foreach (var VARIABLE in clear)
		       {   //  File.Delete(Application.dataPath.Remove(Application.dataPath.Length - "Assets".Length) + VARIABLE);
		           AssetDatabase.DeleteAsset( VARIABLE );
		           //FileUtil.DeleteFileOrDirectory(VARIABLE);
		       }
		
		   }
		}
		#endif*/
		
		return cache;
	}
	
	
	internal static void CheckSaveTargetForHierarchy( Hierarchy_GUI t )
	{	if ( Application.isPlaying || !t || t.TargetChecked ) return;
	
		if ( t.SaveToScriptableObject != "FOLDER"
		        && t.SaveToScriptableObject != "SCENE" )
		{	bool saveInFolder = false;
			bool registrator = false;
			
			
			//#tag TODO Experimental Folder
#pragma warning disable
			
			if ( !Adapter.ALLOW_FOLDER_SAVER )
			{	saveInFolder = Hierarchy.HierarchyAdapterInstance.par.BAKED_DEFAULT_STATE == 2;
				registrator = Hierarchy.HierarchyAdapterInstance.par.BAKED_DEFAULT_STATE == 0;
			}
			
			else
			{	saveInFolder = EditorUtility.DisplayDialog( "Save \"Hierarchy PRO\" data",
				               "Choose the option to save the data (you can change the option in the cache settings later).\nSelect the external folder in case you are working with developers who do not use this plugin",
				               "Folder (Experimental)",
				               "Scene (Default)" );
				               
				               
				var hs = EditorUtility.DisplayDialog( "Display \"Hierarchy PRO\" modules",
				                                      "Do you want to enable \"Hide\" option for modules? Right modules will displaying only when pressed the \"Alt\" key. You may change it in the settings.",
				                                      "Yes",
				                                      "No" );
				                                      
				if (hs)
				{	t.adapter.par.USE_BUTTON_TO_INTERACT_AHC = ((~3) & t.adapter.par.USE_BUTTON_TO_INTERACT_AHC) | 1;
					t.adapter.SavePrefs();
				}
				
				else
				{	t.adapter.par.USE_BUTTON_TO_INTERACT_AHC = (~3) & t.adapter.par.USE_BUTTON_TO_INTERACT_AHC;
					t.adapter.SavePrefs();
				}
				
				if (Initializator.AdaptersByID.ContainsKey(1))
				{	if (Initializator.AdaptersByID[1].par.USE_BUTTON_TO_INTERACT_AHC != 0)
					{	Initializator.AdaptersByID[1].par.USE_BUTTON_TO_INTERACT_AHC = 0;
						Initializator.AdaptersByID[1].SavePrefs();
					}
				}
				
				registrator = false;
			}
			
#pragma warning restore
			
			
			t.SaveToScriptableObject = saveInFolder ? "FOLDER" : "SCENE";
			t.GETUSE_REGISTRATOR = registrator;
			t.GETWAS_NEW_INIT = true;
			Hierarchy_GUI.SetDirtyObject( Hierarchy.HierarchyAdapterInstance );
			
		}
	}
	
}


interface PluginIDField {
	int pluginID { get; set; }
}

internal class FileAssetInitializator<T> where T : ScriptableObject, PluginIDField {

	T cache = null;
	internal Dictionary<int, T> m_cache = new Dictionary<int, T>();
	
	string typeName;
	
	internal FileAssetInitializator()
	{	typeName = typeof( T ).Name;
	}
	
	internal bool TryGetCachedAsset( Adapter adapter, ref T result )
	{	m_cache.TryGetValue( adapter.pluginID, out cache );
	
		if ( cache )
		{	if ( !adapter.wasModulesInitialize ) adapter.MOI.InitModules();
		
			adapter.initializationCount = 2;
			result = cache;
			return true;
		}
		
		return false;
	}
	
	
	internal T TryGetAsset( Adapter adapter, string ASSET_NAME, out bool AssetWasCreated, bool CreateFile = false, bool useOldLoader = false )
	{	AssetWasCreated = false;
		m_cache.TryGetValue( adapter.pluginID, out cache );
		
		if ( cache )
		{	if ( useOldLoader && !CreateFile )
			{	if ( !adapter.wasModulesInitialize ) adapter.MOI.InitModules();
			
				adapter.initializationCount = 2;
			}
			
			return cache;
		}
		
		Adapter.CHECK_FOLDERS( adapter );
		
		
		
		
		
		var oldID = EditorPrefs.GetInt( adapter.pluginname + "/SObjGUID" + typeName, 0 );
		var old = Adapter.GET_OBJECT( oldID ) as T;
		
		if ( old != null && old.pluginID != adapter.pluginID ) old = null;
		
		if ( old != null )
		{
		
		
			// cache = (old);
			m_cache.Remove( adapter.pluginID );
			m_cache.Add( adapter.pluginID, old );
		}
		
		if ( !m_cache.ContainsKey( adapter.pluginID ) )
		{
		
			bool found = false;
			
			{	foreach ( var l in Resources.FindObjectsOfTypeAll<T>() )     // var l = AssetDatabase.LoadAssetAtPath<T>( AssetDatabase.GUIDToAssetPath( f ) );
				{	if ( !l || l.pluginID != adapter.pluginID ) continue;
				
					m_cache.Add( adapter.pluginID, l );
					found = true;
					break;
				}
			}
			
			if ( !found )
				if ( useOldLoader )
				{	foreach ( var f in AssetDatabase.FindAssets( "t:" + typeName ) )
					{	var l = AssetDatabase.LoadAssetAtPath<T>( AssetDatabase.GUIDToAssetPath( f ) );
					
						if ( !l || l.pluginID != adapter.pluginID ) continue;
						
						m_cache.Add( adapter.pluginID, l );
						
						/* Hierarchy_GUI.SetDirtyObject(adapter);
						 var path = AssetDatabase.GUIDToAssetPath( f );
						 if (!string.IsNullOrEmpty(path)) AssetDatabase.DeleteAsset(path);*/
						
						found = true;
						break;
					}
				}
				
				
			if ( !m_cache.ContainsKey( adapter.pluginID ) && adapter.initializationCount < 1 )
			{	adapter.initializationCount++;
				return null;
			}
			
			
		}
		
		adapter.initializationCount = 2;
		
		
		if ( m_cache.ContainsKey( adapter.pluginID ) )
		{	if ( !m_cache[adapter.pluginID] )
			{	m_cache.Remove( adapter.pluginID );
			}
			
			else
			{	EditorPrefs.SetInt( adapter.pluginname + "/SObjGUID" + typeName, m_cache[adapter.pluginID].GetInstanceID() );
				adapter.wasModulesInitialize = false;
				adapter.MOI.InitModules();
				return m_cache[adapter.pluginID];
			}
		}
		
		
		
		
		
		
		
		if ( CreateFile )
		{	if ( File.Exists( adapter.PluginExternalFolder + "/" + ASSET_NAME ) && AssetDatabase.LoadAssetAtPath<T>( adapter.PluginInternalFolder + "/" + ASSET_NAME ) )
			{	m_cache.Add( adapter.pluginID, AssetDatabase.LoadAssetAtPath<T>( adapter.PluginInternalFolder + "/" + ASSET_NAME ) );
				EditorPrefs.SetInt( adapter.pluginname + "/SObjGUID" + typeName, m_cache[adapter.pluginID].GetInstanceID() );
				adapter.wasModulesInitialize = false;
				adapter.MOI.InitModules();
				return m_cache[adapter.pluginID];
			}
		}
		
		
		AssetWasCreated = true;
		
		
		var preCache = ScriptableObject.CreateInstance<T>();
		preCache.hideFlags = HideFlags.DontSaveInBuild;
		preCache.pluginID = adapter.pluginID;
		
		m_cache.Add( adapter.pluginID, preCache );
		
		
		
		
		/*  OLD SAVER*/
		if ( CreateFile )
		{	var ASSET_PATH = adapter.PluginInternalFolder + "/" + ASSET_NAME;
		
			if ( File.Exists( adapter.PluginExternalFolder + "/" + ASSET_NAME ) ) File.Delete( adapter.PluginExternalFolder + "/" + ASSET_NAME );
			
			// AssetDatabase.DeleteAsset( ASSET_PATH );
			AssetDatabase.CreateAsset( m_cache[adapter.pluginID], ASSET_PATH );
			AssetDatabase.SaveAssets();
			AssetDatabase.ImportAsset( ASSET_PATH, ImportAssetOptions.ForceUpdate );
		}
		
		/* */
		if ( !CreateFile ) preCache.hideFlags |= HideFlags.DontSaveInEditor;
		
		// if (useOldLoader && !CreateFile) Debug.Log("ASD " + preCache.GetType().Name);
		
		if ( useOldLoader && !CreateFile )
			adapter.INIT_InitializeEditorSettings();
			
			
		if ( !adapter.par.ENABLE_ALL )
		{	adapter.par.ENABLE_ALL = true;
			adapter.SavePrefs();
		}
		
		
		
		
		/*   OLD SAVER
		Adapter. RequestScriptReload();
		*/
		if ( CreateFile )
			Adapter.RequestScriptReload();
			
			
			
		return m_cache[adapter.pluginID];
	}
}



#if NAMESPACE
}
#endif
}
