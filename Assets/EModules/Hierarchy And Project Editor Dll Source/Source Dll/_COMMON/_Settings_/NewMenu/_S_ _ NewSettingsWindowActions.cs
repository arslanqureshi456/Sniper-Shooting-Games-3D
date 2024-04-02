using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Reflection;
#if PROJECT
	using EModules.Project;
#endif
//namespace EModules



namespace EModules.EModulesInternal

{



internal partial class Adapter {



	SETUPROOT. DRAW_KEEPER_SCRIPTS_Class __PKC;
	internal SETUPROOT.DRAW_KEEPER_SCRIPTS_Class PKC
	{	get
		{	var res = __PKC ?? (__PKC = new SETUPROOT. DRAW_KEEPER_SCRIPTS_Class( ));
			res.A = this;
			return res;
		}
	}
	
	
	bool HIghlighterExternalTexture_init;
	Texture2D _HIghlighterExternalTexture;
	Texture2D HIghlighterExternalTexture
	{	get
		{	if ( !HIghlighterExternalTexture_init )
			{	HIghlighterExternalTexture_init = true;
			
				if (!string.IsNullOrEmpty( HIGHLIGHTER_TEXTURE_GUID ) )
				{	var path =                         AssetDatabase.GUIDToAssetPath( HIGHLIGHTER_TEXTURE_GUID );
				
					if ( !string.IsNullOrEmpty( path ) )
					{	_HIghlighterExternalTexture = AssetDatabase.LoadAssetAtPath<Texture>( path ) as Texture2D;
					}
				}
			}
			
			return _HIghlighterExternalTexture;
		}
		
		set
		{	if (value != HIghlighterExternalTexture )
			{	_HIghlighterExternalTexture = value;
			
				if ( !value )
				{	HIGHLIGHTER_TEXTURE_GUID = "";
				}
				
				else
				{	var path = AssetDatabase.GetAssetPath( value );
				
					if ( !string.IsNullOrEmpty( path ) )
					{	var guid = AssetDatabase.AssetPathToGUID( path );
					
						if ( !string.IsNullOrEmpty( guid ) )
						{	HIGHLIGHTER_TEXTURE_GUID = guid;
						}
						
						else HIGHLIGHTER_TEXTURE_GUID = "";
					}
					
					else HIGHLIGHTER_TEXTURE_GUID = "";
				}
				
				if ( HIGHLIGHTER_TEXTURE_GUID == "" ) _HIghlighterExternalTexture = null;
			}
		}
	}
	
	void DrawExternalTextureObject()
	{	Texture2D newScript = HIghlighterExternalTexture;
	
		try
		{	var c = GUI.color;
		
			if ( !newScript ) GUI.color *= Color.red;
			
			newScript = (Texture2D)EditorGUILayout.ObjectField( HIghlighterExternalTexture, typeof( Texture2D ), false );
			GUI.color = c;
		}
		
		catch
		{	newScript = HIghlighterExternalTexture;
		}
		
		if ( newScript != HIghlighterExternalTexture )
		{	HIghlighterExternalTexture = newScript;
			DRAW_STACK.ValueChanged();
		}
	}
	
	
	internal class cachedInt {
		int defaultValue;
		string key;
		internal cachedInt(string key, Adapter ad, int defaultValue)
		{	this.key = "EMX/" + ad.pluginname + "/" + key;
			this.defaultValue = defaultValue;
		}
		int? _value;
		internal int VALUE
		{	get
			{	return _value ?? (_value = EditorPrefs.GetInt(key, defaultValue )).Value;
			}
			
			set
			{	if ( value == VALUE ) return;
			
				EditorPrefs.SetInt( key, value);
				_value = value;
			}
		}
	}
	
	internal   class SHADER_HELPER {
		string keyMat/*, keyShader*/;
		Adapter adapter;
		internal SHADER_HELPER(string key, Adapter adapter )
		{	this.adapter = adapter;
			this.keyMat = key + "-Material";
			// this.keyShader = key + "-Shader";
		}
		internal   Func<string> GET_SHADER_GUID;
		internal   Func<string> GET_SHADER_LOCAL_PATH;
		internal     Action<string> SET_SHADER_GUID;
		
		Shader oldSHader;
		Material _HIghlighterExternalMaterial;
		
		
		cachedInt  _matID;
		cachedInt matID {   get  { return _matID ?? (_matID = new cachedInt( keyMat, adapter, -1 )); } }
		
		
		internal Material HIghlighterExternalMaterial
		{	get
			{	if ( oldSHader != HIghlighterExternalShader )
				{	oldSHader = HIghlighterExternalShader;
				
					if ( oldSHader == null ) _HIghlighterExternalMaterial = null;
					else _HIghlighterExternalMaterial = new Material( _HIghlighterExternalShader );
					
					matID.VALUE = _HIghlighterExternalMaterial == null ? -1 : _HIghlighterExternalMaterial.GetInstanceID();
				}
				
				if (!_HIghlighterExternalMaterial  && matID.VALUE  != -1)
				{	_HIghlighterExternalMaterial = EditorUtility.InstanceIDToObject( matID.VALUE ) as Material;
				
					if ( !_HIghlighterExternalMaterial && HIghlighterExternalShader )
					{	oldSHader = null;
						return HIghlighterExternalMaterial;
					}
				}
				
				return _HIghlighterExternalMaterial;
			}
		}
		
		
		
		
		bool HIghlighterExternalShader_init;
		Shader _HIghlighterExternalShader;
		internal   Shader HIghlighterExternalShader
		{	get
			{	if ( !HIghlighterExternalShader_init || _HIghlighterExternalShader == null )
				{	HIghlighterExternalShader_init = true;
				
					if ( !string.IsNullOrEmpty( GET_SHADER_GUID() ) )
					{	var path =                         AssetDatabase.GUIDToAssetPath( GET_SHADER_GUID() );
					
						if (string.IsNullOrEmpty( path ) && GET_SHADER_LOCAL_PATH != null) path = EMInternalFolder + GET_SHADER_LOCAL_PATH();
						
						if ( !string.IsNullOrEmpty( path ) )
						{	_HIghlighterExternalShader = AssetDatabase.LoadAssetAtPath<Shader>( path ) as Shader;
						}
						
					}
				}
				
				return _HIghlighterExternalShader;
			}
			
			set
			{	if ( value != HIghlighterExternalShader )
				{	_HIghlighterExternalShader = value;
				
					if ( !value )
					{	SET_SHADER_GUID( "");
					}
					
					else
					{	var path = AssetDatabase.GetAssetPath( value );
					
						if ( !string.IsNullOrEmpty( path ) )
						{	var guid = AssetDatabase.AssetPathToGUID( path );
						
							if ( !string.IsNullOrEmpty( guid ) )
							{	SET_SHADER_GUID( guid);
							}
							
							else SET_SHADER_GUID( "");
						}
						
						else SET_SHADER_GUID( "");
					}
					
					if ( GET_SHADER_GUID() == "" ) _HIghlighterExternalShader = null;
				}
			}
		}
	}
	internal Material HIghlighterExternalMaterialNormal
	{	get { return  SHADER_A.HIghlighterExternalMaterial; }
	}
	internal Material HIghlighterExternalMaterial
	{	get { return HIGHLIGHTER_USE_SPECUAL_SHADER_TYPE == 0 ? SHADER_A.HIghlighterExternalMaterial : SHADER_B.HIghlighterExternalMaterial; }
	}
	
	
	SHADER_HELPER _SHADER_A, _SHADER_B;
	SHADER_HELPER SHADER_A
	{	get
		{	if (_SHADER_A == null )
			{	_SHADER_A = new SHADER_HELPER( "SHADER_A", this )
				{	SET_SHADER_GUID = ( guid ) =>
					{	HIGHLIGHTER_SHADER_GUID = guid;
					},
					GET_SHADER_GUID = () =>
					{	return HIGHLIGHTER_SHADER_GUID;
					}
				};
			}
			
			return _SHADER_A;
		}
	}
	SHADER_HELPER SHADER_B
	{	get
		{	if ( _SHADER_B == null )
			{	_SHADER_B = new SHADER_HELPER( "SHADER_B", this )
				{	SET_SHADER_GUID = ( guid ) =>
					{	HIGHLIGHTER_SHADER_GUID_ADD = guid;
					},
					GET_SHADER_GUID = () =>
					{	return HIGHLIGHTER_SHADER_GUID_ADD;
					}
				};
			}
			
			return _SHADER_B;
		}
	}
	
	void DrawExternalShaderObject()
	{	Shader newScript = SHADER_A.HIghlighterExternalShader;
		var rect = EditorGUILayout.GetControlRect();
		rect.width /= 2;
		var og = GUI.enabled;
		GUI.enabled = og & HIGHLIGHTER_USE_SPECUAL_SHADER_TYPE == 0;
		
		try
		{	newScript = (Shader)EditorGUI.ObjectField( rect, SHADER_A.HIghlighterExternalShader, typeof( Shader ), false );
		}
		
		catch
		{	newScript = SHADER_A.HIghlighterExternalShader;
		}
		
		if ( newScript != SHADER_A.HIghlighterExternalShader )
		{	SHADER_A.HIghlighterExternalShader = newScript;
			DRAW_STACK.ValueChanged();
		}
		
		GUI.enabled = og & HIGHLIGHTER_USE_SPECUAL_SHADER_TYPE == 1;
		newScript = SHADER_B.HIghlighterExternalShader;
		rect.x += rect.width;
		
		try
		{	newScript = (Shader)EditorGUI.ObjectField( rect, SHADER_B.HIghlighterExternalShader, typeof( Shader ), false );
		}
		
		catch
		{	newScript = SHADER_B.HIghlighterExternalShader;
		}
		
		GUI.enabled = og;
		
		if ( newScript != SHADER_B.HIghlighterExternalShader )
		{	SHADER_B.HIghlighterExternalShader = newScript;
			DRAW_STACK.ValueChanged();
		}
	}
	
	
	
	
	
	
	Vector2 DrawHidenIconsscrollPos;
	void DrawHidenIcons()
	{	var list = Hierarchy_GUI.Instance(this).HiddenComponents.ToList();
		var RECT = EditorGUILayout.GetControlRect(GUILayout.Height(50));
		//  RECT.y -= 10;
		RECT.x += 20;
		//  RECT.width = Math.Min( W + 10 , RECT.width ) - 20;
		RECT.width = RECT.width - 20;
		DrawHidenIconsscrollPos = GUI.BeginScrollView( RECT, DrawHidenIconsscrollPos, new Rect( 0, 0, list.Count * 32, 32 ), true, false );
		Assembly[] asms = AppDomain.CurrentDomain.GetAssemblies();
		//Assembly asm = typeof(Image).Assembly;
		Adapter.INTERNAL_BOX( new Rect( 0, 0, RECT.width, 32 ), "" );
		
		if ( list.Count == 0 ) Adapter.INTERNAL_BOX( new Rect( 0, 0, RECT.width, 32 ), "Use the left click in the hierarchy on the icon to hide" );
		
		for ( int i = 0 ; i < list.Count ; i++ )
		{	RECT = new Rect( i * 32, 0, 32, 32 );
			//print(list[i] + " " + asm.GetType(list[i]));
			Type target = null;
			
			foreach ( var assembly in asms )
			{	target = assembly.GetType( list[i] );
			
				if ( target != null ) break;
			}
			
			//  if (Event.current.type.Equals(EventType.Repaint)) GUI.DrawTexture(lastRect, Utilites.ObjectContent(null, asm.GetType(list[i])).image);
			var find = EditorGUIUtility.ObjectContent(null, target);
			
			if ( Event.current.type.Equals( EventType.Repaint ) && find.image != null ) GUI.DrawTexture( RECT, find.image );
			
			if ( !GUI.enabled ) Adapter.FadeRect( RECT, 0.7f );
			
			// if (!GUI.enabled) if (Event.current.type.Equals(EventType.Repaint)) GUI.DrawTexture(lastRect,Hierarchy.sec);
			RECT.x += RECT.width / 2;
			RECT.height = RECT.width = RECT.width / 2;
			
			if ( GUI.Button( RECT, "X" ) )
			{	Hierarchy_GUI.Undo( this, "Restore Icon" );
				Hierarchy_GUI.Instance( this ).HiddenComponents.RemoveAt( i );
				Hierarchy_GUI.SetDirtyObject( this );
				DRAW_STACK.ValueChanged();
			}
			
		}
		
		GUI.EndScrollView();
	}
	void DrawGroupMonoIcons()
	{
	
	
		var  R = EditorGUILayout.GetControlRect( GUILayout.Height(46) );
		R.x += 20;
		R.width -= 20;
		Adapter.INTERNAL_BOX( R, "" );
		R.width /= 3;
		
		// GUILayout.BeginHorizontal(GUILayout.Height(46));
		// Space(40);
		/* var B_normal = Adapter.GET_SKIN().button.normal.background;
		 var B_active = Adapter.GET_SKIN().button.active.background;
		 var B_focused = Adapter.GET_SKIN().button.focused.background;
		 var B_hover = Adapter.GET_SKIN().button.hover.background;
		 var B_normalSCL = Adapter.GET_SKIN().button.normal.scaledBackgrounds;
		 var B_activeSCL = Adapter.GET_SKIN().button.active.scaledBackgrounds;
		 var B_focusedSCL = Adapter.GET_SKIN().button.focused.scaledBackgrounds;
		 var B_hoverSCL = Adapter.GET_SKIN().button.hover.scaledBackgrounds;
		 Adapter.GET_SKIN().button.normal.background = null;
		 Adapter.GET_SKIN().button.hover.background = Adapter.GET_SKIN().button.focused.background = Adapter.GET_SKIN().button.active.background = GetIcon( "BUTBLUE" );
		 Adapter.GET_SKIN().button.normal.scaledBackgrounds = blackTexture;
		 Adapter.GET_SKIN().button.hover.scaledBackgrounds = Adapter.GET_SKIN().button.focused.scaledBackgrounds = Adapter.GET_SKIN().button.active.scaledBackgrounds = blackTexture;
		*/
		
		GUI.Label( R, "" );
		
		if ( par.COMPS_SplitMode == 0 ) GUI.DrawTexture( R, GetIcon( "BUTBLUE" ) );
		
		var lr = R;
		lr.height = 25;
		lr.y += 4 + EditorGUIUtility.singleLineHeight;
		GUI.DrawTexture( lr, GetIcon( "COMP_SPLIT1" ), ScaleMode.ScaleToFit, true, 1 / 0.3f );
		EditorGUIUtility.AddCursorRect( lr, MouseCursor.Link );
		lr = R;
		lr.height = EditorGUIUtility.singleLineHeight;
		LABEL( lr, "All in <b>One</b>" );
		
		if ( GUI.Button( R, "", STYLE_DEFBUTTON) )
		{	par.COMPS_SplitMode = 0;
			DRAW_STACK.ValueChanged();
		}
		
		R.x += R.width;
		GUI.Label( R, "" );
		
		if ( par.COMPS_SplitMode == 1 ) GUI.DrawTexture( R, GetIcon( "BUTBLUE" ) );
		
		if ( GUI.Button( R, "", STYLE_DEFBUTTON ) )
		{	par.COMPS_SplitMode = 1;
			DRAW_STACK.ValueChanged();
		}
		
		lr = R;
		lr.height = 25;
		lr.y += 4 + EditorGUIUtility.singleLineHeight;
		GUI.DrawTexture( lr, GetIcon( "COMP_SPLIT2" ), ScaleMode.ScaleToFit, true, 1 / 0.3f );
		EditorGUIUtility.AddCursorRect( lr, MouseCursor.Link );
		lr = R;
		lr.height = EditorGUIUtility.singleLineHeight;
		//if (Event.current.type == EventType.repaint) EditorStyles.largeLabel.Draw(lr, new GUIContent("Merge\nEnable/Disable"), false, false, false, false);
		LABEL( lr, "<b>Two</b> <i>(On & Off)</i>" );
		
		
		R.x += R.width;
		GUI.Label( R, "" );
		
		if ( par.COMPS_SplitMode == 2 ) GUI.DrawTexture( R, GetIcon( "BUTBLUE" ) );
		
		if ( GUI.Button( R, "", STYLE_DEFBUTTON ) )
		{	par.COMPS_SplitMode = 2;
			DRAW_STACK.ValueChanged();
			
		}
		
		lr = R;
		lr.height = 25;
		lr.y += 4 + EditorGUIUtility.singleLineHeight;
		GUI.DrawTexture( lr, GetIcon( "COMP_SPLIT3" ), ScaleMode.ScaleToFit, true, 1 / 0.3f );
		EditorGUIUtility.AddCursorRect( lr, MouseCursor.Link );
		lr = R;
		lr.height = EditorGUIUtility.singleLineHeight;
		LABEL( lr, "Show <b>All</b>" );
		/*   Adapter.GET_SKIN().button.normal.background = B_normal;
		   Adapter.GET_SKIN().button.active.background = B_active;
		   Adapter.GET_SKIN().button.focused.background = B_focused;
		   Adapter.GET_SKIN().button.hover.background = B_hover;
		   Adapter.GET_SKIN().button.normal.scaledBackgrounds = B_normalSCL;
		   Adapter.GET_SKIN().button.active.scaledBackgrounds = B_activeSCL;
		   Adapter.GET_SKIN().button.focused.scaledBackgrounds = B_focusedSCL;
		   Adapter.GET_SKIN().button.hover.scaledBackgrounds = B_hoverSCL;*/
	}
#pragma warning disable
	GUIContent PlusContent = new GUIContent()
	{	text = "+",
		    tooltip = "Drag MonoBehaviour Script or Texture here"
	};
	GUIContent PlusContentEmpty = new GUIContent()
	{	text = "",
		    tooltip = "Drag MonoBehaviour Script or Texture here"
	};
#pragma warning restore
	
	SETUPROOT. DrawCustomIconsClass __CI;
	internal SETUPROOT.DrawCustomIconsClass CI
	{	get
		{	var res = __CI ?? (__CI = new SETUPROOT.DrawCustomIconsClass( ));
			res.A = this;
			return res;
		}
	}
	void DrawUserIcons()
	{	var boxRect = EditorGUILayout.GetControlRect( GUILayout.Height(0) );
		boxRect.height = CI.CusomIconsHeight + 12;
		Adapter.INTERNAL_BOX( boxRect, "" );
		
		var  R = EditorGUILayout.GetControlRect( GUILayout.Height(CI.CusomIconsHeight) );
		//R.x += 7;
		// R.y += 6;
		GUI.BeginScrollView( R, Vector2.zero, new Rect( 0, 0, R.width, SETUPROOT.DrawCustomIconsClass.IC_H * (CI.customIcons.Count + 1) ), false, false/*, GUILayout.Width(W), GUILayout.ExpandWidth(true)*/);
		
		var lr = EditorGUILayout. GetControlRect(GUILayout.Height( EditorGUIUtility.singleLineHeight));
		CI.DrawCustomIcons( prefWindow, lr );
		EditorGUILayout.GetControlRect( GUILayout.Height( 25 ) );
		
		GUI.EndScrollView();
		
		CI.Updater( prefWindow );
	}
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	string HIERARCHY_MENU_HELP = @"
class MyMenu_SelectAllChildren : HierarchyExtensions.IGenericMenu
{
    public string Name { get { return ""Select All Children""; } }
    public int PositionInMenu { get { return 200; } }

    public bool IsEnable(GameObject clickedObject) { return clicedkObject.transform.childCount != 0; }
    public bool NeedExcludeFromMenu(GameObject clickedObject) { return false; }

    public void OnClick(GameObject[] affectedObjectsArray)
    {
        Selection.objects = selectedObjects.SelectMany(s => s.GetComponentsInChildren<Transform>(true)).Select(s => s.gameObject).ToArray();
    }
}";



	string PROJECT_MENU_HELP = @"
    class MyMenu : ProjectExtensions.IGenericMenu
    {
        public string Name { get { return ""MySubItem/MyMenuItem %k""; } }
        public int PositionInMenu { get { return 0; } }

        public bool IsEnable(string clickedObjectPath, string clickedObjectGUID, int instanceId, bool isFolder, bool isMainAsset) { return true; }
        public bool NeedExcludeFromMenu(string clickedObjectPath, string clickedObjectGUID, int instanceId, bool isFolder, bool isMainAsset) { return false; }
    
        public void OnClick(string[] affectedObjectsPathArray, string[] affectedObjectsGUIDArray, int[] affectedObjectsInstanceId, bool[] affectedObjectIsFolder, bool[] isMainAsset)
        {
          throw new System.NotImplementedException();
        }
    }";











	bool LibraryFiltersDialog( object value )
	{	var c = (bool)value;
		var compex = UnityEditor.EditorUtility.DisplayDialogComplex("Auto-HighLighter Settings",
		             "Now it will create a copy of your current settings.\nDo you want to remove old settings file?", "Yes", "Cancel", "No");
		             
		if ( compex == 0 || compex == 2 ) EModulesHierarchy_ProjectSettings_HighLighter.SwitchLibraryMode( this, c, compex == 2 );
		
		return compex == 0 || compex == 2;
	}
	bool LibraryIconsDialog( object value )
	{	var c = (bool)value;
		var compex = UnityEditor.EditorUtility.DisplayDialogComplex("Components Icons Settings",
		             "Now it will create a copy of your current settings.\nDo you want to remove old settings file?", "Yes", "Cancel", "No");
		             
		if ( compex == 0 || compex == 2 ) EModulesHierarchy_ProjectSettings_ComponentsIcons.SwitchLibraryMode( this, c, compex == 2 );
		
		return compex == 0 || compex == 2;
	}
	void CacheEditorSetBut0()
	{	var lastPath = EditorPrefs.GetString("Hierarchy/LastPath", Adapter. UNITY_SYSTEM_PATH);
		var path = EditorUtility.OpenFilePanel("Import Settings", lastPath, "settings");
		
		if ( path.Length != 0 )
		{	EditorPrefs.SetString( "Hierarchy/LastPath", path );
			var load = File.ReadAllText(path, System.Text.Encoding.Unicode);
			
			if ( !SettingsFromString( load ) )
			{	EditorUtility.DisplayDialog( "Import Settings", "Wrong format", "Ok" );
			}
		}
	}
	void CacheEditorSetBut1()
	{	var path = EditorUtility.SaveFilePanel("Export Settings", Adapter.UNITY_SYSTEM_PATH, pluginname + ".settings", "settings");
	
		if ( path.Length != 0 )
		{	EditorPrefs.SetString( "Hierarchy/LastPath", path );
			File.WriteAllText( path, SettingsToString(), System.Text.Encoding.Unicode );
		}
	}
	void CacheEditorSetBut2()
	{	if ( EditorUtility.DisplayDialog( "Default Editor Settings", "Do you want to reset editor's editor settings", "Ok", "Cancel" ) )
		{	ResetSettings();
		}
	}
	void CacheEditorPrefab0()
	{	if ( Hierarchy_GUI.Instance( this ).PrefabIDMode == Hierarchy_GUI.PrefabIDModeEnum.SeparateInstances ) return;
	
		if ( EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo() )
		{	if ( EditorUtility.DisplayDialog( "Enable Separate Prefabs Instances", "This mode will store data for each instance separately?", "Yes", "Cancel" ) )
			{	Hierarchy_GUI.Instance( this ).PrefabIDMode = Hierarchy_GUI.PrefabIDModeEnum.SeparateInstances;
				Hierarchy_GUI.SetDirtyObject( this );
				Adapter.RequestScriptReload();
			}
		}
	}
	void CacheEditorPrefab1()
	{	if ( Hierarchy_GUI.Instance( this ).PrefabIDMode == Hierarchy_GUI.PrefabIDModeEnum.MergedInstances ) return;
	
		if ( EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo() )
		{	if ( EditorUtility.DisplayDialog( "Enable Merged Prefabs Instances", "This mode will store data for all instance in sync?", "Yes", "Cancel" ) )
			{	Hierarchy_GUI.Instance( this ).PrefabIDMode = Hierarchy_GUI.PrefabIDModeEnum.MergedInstances;
				Hierarchy_GUI.SetDirtyObject( this );
				Adapter.RequestScriptReload();
			}
		}
	}
	int GET_STATE()     //if ( A.DISABLE_DES() ) return 2;
	{	if ( IS_PROJECT() ) return 0;
	
		if ( Hierarchy_GUI.Instance( this ).SaveToScriptableObject == "FOLDER" ) return 2;
		
		if ( Hierarchy_GUI.Instance( this ).GETUSE_REGISTRATOR ) return 0;
		
		return 1;
	}
	void CacheGUI()
	{
	
	
		var R1 = new Rect();
		var R2 = new Rect();
		var R3 = new Rect();
		
		
		
		GUILayout.BeginHorizontal();
		
		R1 = EditorGUILayout.GetControlRect( GUILayout.Height( 60 ) );
		R1.width /= 2;
		
		R2 = R1;
		
		if ( GUI.Button( R2, "Cache in Scenes\n(Default)" ) && GET_STATE() != 1 )
		{	if ( EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo() )
			{	if ( EditorUtility.DisplayDialog( "Save in scene", "Do you want to enable scene caching mode? ", "Yes", "Cancel" ) )        //  par.DISABLE_DES = false;
				{	Adapter.SWITCH_TO_SCENE_MODE( this );
				
				}
			}
		}
		
		if ( GET_STATE() == 1 ) GUI.DrawTexture( R2, GetIcon( "BUTBLUE" ) );
		
		R3 = R2;
		R3.x += R3.width;
		
		if ( GUI.Button( R3, "Cache in Folder\n(Experimental)" )
		        && GET_STATE() !=
		        2 )      //EditorUtility.DisplayDialog( "Save in folder", "this function is not yet available it is in the test, it will be added to version 18.2 in the near future", "Cancel", "Cancel" );
		{
		
			//if ( Adapter.ALLOW_FOLDER_SAVER ) {
			if ( EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo() )
			{	if ( EditorUtility.DisplayDialog( "Save in folder",
				                                  "Every scene will save params in separate scriptableobject files, if you will need to copy data to another scene you will need to use copy/paste interface in inspector for scriptableobject file",
				                                  "Yes", "Cancel" ) )      // par.DISABLE_DES = true;
				{	Hierarchy_GUI.Instance( this ).SaveToScriptableObject = "FOLDER";
					Hierarchy_GUI.Instance( this ).GETUSE_REGISTRATOR = false;
					Hierarchy_GUI.SetDirtyObject( this );
					Adapter.RequestScriptReload();
				}
			}
			
			//                 } else {
			//                     EditorUtility.DisplayDialog( "Warning!" ,
			//                                                  "To use this feature you need to install \"Hierarchy PRO OpenSource\" version" ,
			//                                                  "Ok" );
			//                 }
			
		}
		
		if ( GET_STATE() == 2 ) GUI.DrawTexture( R3, GetIcon( "BUTBLUE" ) );
		
		GUILayout.EndHorizontal();
		
		
		R1.y += R1.height;
		R2.y += R2.height;
		R3.y += R3.height;
		var startY = R1.y;
		
		
		// R1.height = EditorStyles.helpBox.CalcHeight( C1 , R1.width );
		// R2.height = EditorStyles.helpBox.CalcHeight( C2 , R2.width );
		// R3.height = EditorStyles.helpBox.CalcHeight( C3 , R3.width );
		GUILayout.BeginHorizontal();
		var  TD = GUI.enabled;
		GUI.enabled = GET_STATE() == 1;
		EditorGUILayout.HelpBox( C2.text, MessageType.None );
		R2 = GUILayoutUtility.GetLastRect();
		//EditorGUI.HelpBox( R2 , C2.text , MessageType.None );
		R2.y += R2.height + 5;
		R2.height = EditorGUIUtility.singleLineHeight;
		GUI.Label( R2, "DescriptionHelper" );
		R2.y += R2.height;
		R2.height = 20;
		var  d = TOGGLE_LEFT( R2, "Hide Helper", par.HIDE_DES );
		
		if ( d != par.HIDE_DES )
		{	par.HIDE_DES = d;
#if !PROJECT
			Hierarchy.M_Descript.UpdateFlags();
#endif
			Adapter.RequestScriptReload();
		}
		
		R2.y += R2.height;
		GUI.enabled = TD;
		
		
		
		
		
		
		
		
		
		
		
		TD = GUI.enabled;
		GUI.enabled = GET_STATE() == 2;
		// EditorGUI.HelpBox( R3 , C3.text , MessageType.None );
		EditorGUILayout.HelpBox( C3.text, MessageType.None );
		R3 = GUILayoutUtility.GetLastRect();
		GUILayout.EndHorizontal();
		R3.y += R3.height;
		
		
		GUI.enabled = TD;
		
		
		
		//  GUILayout.Space( Math.Max( Math.Max( R1.y , R2.y ) , R3.y ) - startY );
		// GUILayout.Space(  Math.Max( R3.height , R2.height )  );
		
		
		GUILayout.Space( 10 );
		GUILayout.Space( EditorGUIUtility.singleLineHeight );
		GUILayout.Space( EditorGUIUtility.singleLineHeight );
		Adapter.DrawRect( EditorGUILayout.GetControlRect( GUILayout.Height( 15 ) ), new Color( 0.4f, 0.1f, 0.1f, 0.3f ) );
		
		R3 = EditorGUILayout.GetControlRect( GUILayout.Height( 40 ) );
		R3.width = R1.width * 2 / 3;
		
		if ( GUI.Button( R3, "Cache Manager", SETUP_BUTTON ) )
		{	_S___ScenesScannerWindow.Init( this );
		}
		
		R3.x += R3.width;
		R3.width = R3.width * 2;
		EditorGUI.HelpBox( R3, @"If you have already used the hierarchy plugin and saved some data in scenes, you can clear the created cache.", MessageType.None );
		
		GUILayout.Space( 30 );
		
		
		EditorGUILayout.HelpBox( "Any data will not be included in the build", MessageType.Info );
		
		
		
		
		
		
	}
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	void HYPER_RELOADPERFOMANCE()
	{
	
		GUILayout.BeginHorizontal();
		GUILayout.Space( 15 );
		
		var pwm = par.HiperGraphParams.SCANPERFOMANCE;
		var R = EditorGUILayout.GetControlRect( GUILayout .Height( 24 ) );
		R.width /= 5;
		
		if ( GUI.Button( R, "20%", SETUP_BUTTON ) ) pwm = 2;
		
		var SELR = R;
		R.x += R.width;
		
		if ( GUI.Button( R, "40%", SETUP_BUTTON ) ) pwm = 4;
		
		if ( par.HiperGraphParams.SCANPERFOMANCE == 4 ) SELR = R;
		
		R.x += R.width;
		
		if ( GUI.Button( R, "60%", SETUP_BUTTON ) ) pwm = 6;
		
		if ( par.HiperGraphParams.SCANPERFOMANCE == 6 ) SELR = R;
		
		R.x += R.width;
		
		if ( GUI.Button( R, "80%", SETUP_BUTTON ) ) pwm = 8;
		
		if ( par.HiperGraphParams.SCANPERFOMANCE == 8 ) SELR = R;
		
		R.x += R.width;
		
		if ( GUI.Button( R, "∞", SETUP_BUTTON ) ) pwm = 10;
		
		if ( par.HiperGraphParams.SCANPERFOMANCE == 10 ) SELR = R;
		
		GUI.DrawTexture( SELR, GetIcon( "BUTBLUE" ) );
		
		if ( pwm != par.HiperGraphParams.SCANPERFOMANCE )
		{	par.HiperGraphParams.SCANPERFOMANCE = pwm;
			DRAW_STACK.ValueChanged();
		}
		
		GUILayout.Space( 15 );
		GUILayout.EndHorizontal();
	}
	
	
	
	
	
	
	Texture2D[] SORT_TEXTURES, SORT_TEXTURES_PERSONAL;
	
	void DRAW_BOTTOM_BUTTONS()
	{	if ( SORT_TEXTURES == null || SORT_TEXTURES[0] == null || SORT_TEXTURES[0] == Texture2D.blackTexture )
		{	SORT_TEXTURES = new Texture2D[] { GetIcon( "SETUP_SORT0" ), GetIcon( "SETUP_SORT1" ), GetIcon( "SETUP_SORT2" ), GetIcon( "SETUP_SORT3" ) };
			SORT_TEXTURES_PERSONAL = new Texture2D[] { GetIcon( "SETUP_SORT0 PERSONAL" ), GetIcon( "SETUP_SORT1 PERSONAL" ), GetIcon( "SETUP_SORT2 PERSONAL" ), GetIcon( "SETUP_SORT3 PERSONAL" ) };
		}
		
		GUILayout.BeginHorizontal();
		GUILayout.Space( 15 );
		var newS = __DRAW_BUTTONS( null, new[] { "", "", "", "" }, par.BottomParams.SORT_LINES, 35, true, EditorGUIUtility.isProSkin ? SORT_TEXTURES : SORT_TEXTURES_PERSONAL );
		
		if ( newS != par.BottomParams.SORT_LINES )
		{	par.BottomParams.SORT_LINES = newS;
			DRAW_STACK.ValueChanged();
		}
		
		GUILayout.Space( 15 );
		GUILayout.EndHorizontal();
	}
	int __DRAW_BUTTONS( string tit, string[] names, int selected, int height, bool usered = false, Texture2D[] textures = null )
	{	// var r = GET_OFFSETRECT(height);
		var r = EditorGUILayout.GetControlRect( GUILayout.Height(height));
		var L = names.Length;
		
		if ( tit != null )
		{
		
		
			r.width /= 2;
			GUI.Label( r, tit );
			r.x += r.width;
		}
		
		r.width /= L;
		
		for ( int i = 0 ; i < L ; i++ )
		{	var R = r;
			r.x += r.width;
			
			var style = i < L / 2 ? GUI.skin.button : i > L / 2 ? GUI.skin.button : GUI.skin.button;
			var tt = style.border.top;
			var bb = style.border.bottom;
			
			if ( !EditorGUIUtility.isProSkin )
			{	style.border.top = 5;
				style.border.bottom = 5;
			}
			
			if ( GUI.Button( R, names[i], style ) )
			{	selected = i;
			}
			
			if ( Event.current.type == EventType.Repaint && selected == i )
			{	var c = GUI.color;
			
				if ( usered ) GUI.color *= new Color( 1f, 0.7f, 0.6f );
				
				style.Draw( R, names[i]/*.ToUpper()*/, true, true, false, true );
				GUI.color = c;
			}
			
			style.border.top = tt;
			style.border.bottom = bb;
			
			if ( textures != null )
			{	GUI.DrawTexture( new Rect( R.x + (R.width - textures[i].width) / 2, R.y + (R.height - textures[i].height) / 2, textures[i].width, textures[i].height ), textures[i] );
			}
		}
		
		return selected;
	}
	
	static int[] intMaxItemsPopUp;
	static int[] rowsPopUp;
	static int[] OBJECT_X = { 0, 21, 39, 62, 182, 222, 267, 292 };
	static int[] OBJECT_WIDTH = { 16, 19, 19, 120, 36, 36, 16, 55 };
	
	
	void DRAW_ROWS()
	{
	
		bottomInterface.SORT_DRAW_ROWS();
		
		var RowsClasses = bottomInterface.RowsParams;
		
		if ( intMaxItemsPopUp == null ) intMaxItemsPopUp = Enumerable.Repeat( 0, HierParams.MAX_SELECTION_ITEMS - 3 ).Select( ( e, i ) => i + 3 ).ToArray();
		
		if ( rowsPopUp == null ) rowsPopUp = Enumerable.Repeat( 0, 3 ).Select( ( e, i ) => i + 1 ).ToArray();
		
		Rect R;
		var r = R = EditorGUILayout.GetControlRect(GUILayout.Height( 23));
		r.y += (R.height - EditorGUIUtility.singleLineHeight) / 2;
		
		r.x = R.x + OBJECT_X[1];
		r.width = OBJECT_WIDTH[1] * 2;
		GUI.Label( r, "Sort" );
		GUI.Label( r, Sorting );
		r.x = R.x + OBJECT_X[3];
		r.width = OBJECT_WIDTH[3];
		GUI.Label( r, "Name" );
		GUI.Label( r, CatName );
		r.x = R.x + OBJECT_X[4];
		r.width = OBJECT_WIDTH[4];
		GUI.Label( r, "Cells" );
		GUI.Label( r, ButtonsCount );
		r.x = R.x + OBJECT_X[5];
		r.width = OBJECT_WIDTH[5];
		GUI.Label( r, "Rows" );
		GUI.Label( r, RowsCount );
		r.x = R.x + OBJECT_X[6];
		r.width = OBJECT_WIDTH[6];
		GUI.Label( r, "HL" );
		GUI.Label( r, higlighterColor );
		r.x = R.x + OBJECT_X[7];
		r.width = OBJECT_WIDTH[7];
		GUI.Label( r, "BG" );
		GUI.Label( r, backgroundColor );
		
		
		for ( int __index = 0, L = bottomInterface.DRAW_INDEX.Length ; __index < L ; __index++ )
		{	var i = bottomInterface.DRAW_INDEX[__index];
		
			R = EditorGUILayout.GetControlRect( GUILayout.Height( 23 ) );
			
			
			r = R;
			var _bY = (R.height - EditorGUIUtility.singleLineHeight) / 2;
			r.y += _bY;
			
			r.height = EditorGUIUtility.singleLineHeight;
			
			r.x = R.x + OBJECT_X[0];
			r.width = OBJECT_WIDTH[0];
			RowsClasses[i].Enable = EditorGUI.Toggle( r, EnableDisable, RowsClasses[i].Enable );
			GUI.Label( r, EnableDisable );
			
			
			r.x = R.x + OBJECT_X[1];
			r.width = OBJECT_WIDTH[1];
			var e = GUI.enabled;
			GUI.enabled &= __index != 0;
			
			if ( GUI.Button( r, SortingUP ) )
			{	var CI = bottomInterface.DRAW_INDEX[__index];
				var NI = bottomInterface.DRAW_INDEX[__index - 1];
				var OLD = RowsClasses[CI].IndexPos;
				RowsClasses[CI].IndexPos = RowsClasses[NI].IndexPos;
				RowsClasses[NI].IndexPos = OLD;
				DRAW_STACK.ValueChanged();
			}
			
			r.x = R.x + OBJECT_X[2];
			r.width = OBJECT_WIDTH[2];
			GUI.enabled = e & __index < L - 1;
			
			if ( GUI.Button( r, SortingDOWN ) )
			{	var CI = bottomInterface.DRAW_INDEX[__index];
				var NI = bottomInterface.DRAW_INDEX[__index + 1];
				var OLD = RowsClasses[CI].IndexPos;
				RowsClasses[CI].IndexPos = RowsClasses[NI].IndexPos;
				RowsClasses[NI].IndexPos = OLD;
				DRAW_STACK.ValueChanged();
			}
			
			GUI.enabled = e;
			
			r.x = R.x + OBJECT_X[3];
			r.width = OBJECT_WIDTH[3];
			GUI.Label( r, RowsClasses[i].Name );
			
			
			
			r.x = R.x + OBJECT_X[4];
			r.width = OBJECT_WIDTH[4];
			//  RowsClasses[i].MaxItems = EditorGUI.IntField(r, RowsClasses[i].MaxItems, null, intMaxItemsPopUp);
			var a1 = EditorGUI.IntField( r, ButtonsCount, RowsClasses[i].MaxItems );
			
			if ( a1 != RowsClasses[i].MaxItems )
			{	RowsClasses[i].MaxItems = a1;
				DRAW_STACK.ValueChanged();
			}
			
			GUI.Label( r, ButtonsCount );
			
			r.x = R.x + OBJECT_X[5];
			r.width = OBJECT_WIDTH[5];
			var a2 = EditorGUI.IntField( r, RowsCount, RowsClasses[i].Rows );
			
			if ( a2 != RowsClasses[i].Rows )
			{	RowsClasses[i].Rows = a2;
				DRAW_STACK.ValueChanged();
			}
			
			GUI.Label( r, RowsCount );
			
			if ( RowsClasses[i].AllowHiglighter )
			{	r.x = R.x + OBJECT_X[6];
				r.width = OBJECT_WIDTH[6];
				var a3 = EditorGUI.Toggle( r, higlighterColor, RowsClasses[i].HiglighterValue );
				
				if (a3 != RowsClasses[i].HiglighterValue )
				{	RowsClasses[i].HiglighterValue = a3;
					DRAW_STACK.ValueChanged();
				}
				
				GUI.Label( r, higlighterColor );
				r.x += 5;
			}
			
			if ( RowsClasses[i].AllowBgColor )
			{	r.x = R.x + OBJECT_X[7];
				r.width = OBJECT_WIDTH[7];
				r.y = R.y;
				r.height = R.height;
				
				var newC2 = M_Colors_Window.PICKER(new Rect(r.x + r.width - 55, r.y, 55, 23), backgroundColor, RowsClasses[i].BgColorValue);
				
				if ( RowsClasses[i].BgColorValue != newC2 )
				{	RowsClasses[i].BgColorValue = newC2;
					DRAW_STACK.ValueChanged();
				}
			}
			
		}
	}
}
}
