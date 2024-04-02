
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

//namespace EModules



namespace EModules.EModulesInternal

{



internal partial class Adapter {


	internal bool ENABLE_ACTIVEGMAOBJECTMODULE
	{	get
		{	if (pluginID != 0) return false;
		
			return par.ENABLE_ACTIVEGMAOBJECTMODULE;
		}
	}
	
	internal bool? _DISP_NEW;
	internal bool DISP_NEW
	{	get { return _DISP_NEW ?? (_DISP_NEW = EditorPrefs.GetBool( "EModules/" + pluginname + "/DISP_NEW", true )).Value; }
	
		set
		{	if ( _DISP_NEW == value ) return;
		
			_DISP_NEW = value;
			EditorPrefs.SetBool( "EModules/" + pluginname + "/DISP_NEW", value );
		}
	}
	
	internal bool USE_NEW_MULTYLINE_SELECTION_BEGHAVIOUR { get { return par.USE_NEW_MULTYLINE_SELECTION_BEGHAVIOUR?? (par.USE_NEW_MULTYLINE_SELECTION_BEGHAVIOUR = true).Value; } set { par.USE_NEW_MULTYLINE_SELECTION_BEGHAVIOUR = value; } }
	internal bool USE_HOVER_EXPANDING { get { return par.USE_HOVER_EXPANDING?? (par.USE_HOVER_EXPANDING = false).Value; } set { par.USE_HOVER_EXPANDING = value; } }
	internal bool HIDE_HOVER_BG
	
	{	get { return par.HIDE_HOVER_BG?? (par.HIDE_HOVER_BG = false).Value; } set
		{	var oldV = HIDE_HOVER_BG;
			par.HIDE_HOVER_BG = value;
			
			if (oldV != HIDE_HOVER_BG) UpdateBGHOver();
		}
	}
	
	internal int INSTANTIATE_MODE
	{	get { return (par.INSTANTIATE_MODE ?? 1); }
	
		set { par.INSTANTIATE_MODE = value; }
	}
	internal bool SHIFT_TO_INSTANTIATE_BOTTOM
	{	get { return (par.SHIFT_TO_INSTANTIATE_BOTTOM ?? true); }
	
		set { par.SHIFT_TO_INSTANTIATE_BOTTOM = value; }
	}
	internal bool DRAW_SECOND_CHAR_FOR_MONO
	{	get { return (par.DRAW_SECOND_CHAR_FOR_MONO ?? true); }
	
		set { par.DRAW_SECOND_CHAR_FOR_MONO = value; }
	}
	internal bool EDITOR_FONT_AFFECTOTHERWINDOWS
	//	{	get { return (par.EDITOR_FONT_AFFECTOTHERWINDOWS ?? Adapter.UNITY_CURRENT_VERSION >= Adapter.UNITY_2019_3_0_VERSION); }
	{	get { return (par.EDITOR_FONT_AFFECTOTHERWINDOWS ?? false); }
	
		set { par.EDITOR_FONT_AFFECTOTHERWINDOWS = value; }
	}
	internal bool DRAW_ICONS_MONO_BG_INVERCE
	{	get
		{	return !DRAW_ICONS_MONO_BG;
		}
	}
	internal bool DRAW_ICONS_MONO_BG
	{	get { return (par.DRAW_ICONS_MONO_BG ?? true); }
	
		set { par.DRAW_ICONS_MONO_BG = value; }
	}
	internal int HIERARCHY_FONT_SIZE
	{	get { return (par.HIERARCHY_FONT_SIZE ?? (Adapter.UNITY_CURRENT_VERSION >= Adapter.UNITY_2019_3_0_VERSION ? -1 : 0 )) + lastFontSize; }
	
		//{	get { return (par.HIERARCHY_FONT_SIZE ?? ( 0 )) + lastFontSize; }
		
		set { par.HIERARCHY_FONT_SIZE = value - lastFontSize; }
	}
	internal float BOTTOM_CONTRAST
	{	get { return par.BOTTOM_CONTRAST ?? 0.8f; }
	
		set { par.BOTTOM_CONTRAST = value; }
	}
	
	internal bool SEARCH_SHOW_DISABLED_OBJECT
	{	get { return pluginID != 0 ||  (par.SEARCH_SHOW_DISABLED_OBJECT ?? true); }
	
		set { par.SEARCH_SHOW_DISABLED_OBJECT = value; }
	}
	internal bool SEARCH_USE_ROOT_ONLY
	{	get { return par.SEARCH_USE_ROOT_ONLY ?? false; }
	
		set { par.SEARCH_USE_ROOT_ONLY = value; }
	}
	internal bool DRAW_LABEL_FOR_EMPTY_CONTENT
	{	get { return par.DRAW_LABEL_FOR_EMPTY_CONTENT ?? false; }
	
		set { par.DRAW_LABEL_FOR_EMPTY_CONTENT = value; }
	}
	
	
	internal bool CACHING_TEXTURES_STACKS
	{	get { return par.newPerfomanceCaching ?? true; }
	
		set { par.newPerfomanceCaching = value; }
	}
	
	
	bool EXTERNAL_FOLDER_DISABLE
	{	get { return GET_STATE() != 2; }
	
		set { }
	}
	
	internal bool DRAW_ICONS_SHADOW
	{	get { return par.DRAW_ICONS_SHADOWS ?? (par.DRAW_ICONS_SHADOWS = false).Value; }
	
		set { par.DRAW_ICONS_SHADOWS = value; }
	}
	
	internal int GROUPING_CHILD_MODE
	{	get { return par.HIGHLIGHTER_CHILD_GROUPING ?? (par.HIGHLIGHTER_CHILD_GROUPING = 1).Value; }
	
		set { par.HIGHLIGHTER_CHILD_GROUPING = value; }
	}
	
	internal int HIGHLIGHTER_USE_SPECUAL_SHADER_TYPE
	{	get { return par.useSpecialHighlighterSHader_Type ?? (par.useSpecialHighlighterSHader_Type = EditorGUIUtility.isProSkin ? 0 : 0).Value; } //pro was 1
	
		set { par.useSpecialHighlighterSHader_Type = value; }
	}
	
	
	
	internal  SHADER_HELPER _DEFAULT_SHADER_SHADER;
	internal SHADER_HELPER DEFAULT_SHADER_SHADER
	{	get
		{	if ( _DEFAULT_SHADER_SHADER == null )
			{	_DEFAULT_SHADER_SHADER = new SHADER_HELPER( "DEFAULT_SHADER_SHADER",  this )
				{	SET_SHADER_GUID = ( guid ) =>
					{
					},
					GET_SHADER_GUID = () =>
					{	return "192bc8e8b4e0b5d40842dcca06a181e1";
					},
					GET_SHADER_LOCAL_PATH = () =>
					{	return "/Hierarchy/Source/Highlighter - Default Material.shader";
					}
				};
			}
			
			return _DEFAULT_SHADER_SHADER;
		}
	}
	
	
	
	internal bool IS_UNITY_2019_2_ABOVE
	{	get { return UNITY_CURRENT_VERSION < UNITY_2019_2_0_VERSION; }
	}
	internal bool HIGHLIGHTER_USE_SPECUAL_SHADER
	{	get { return par.useSpecialHighlighterSHader ?? (par.useSpecialHighlighterSHader = true).Value; }
	
		set { par.useSpecialHighlighterSHader = value; }
	}
	internal string HIGHLIGHTER_SHADER_GUID
	{	get { return par.highligterExternalShaderPath1 ?? (par.highligterExternalShaderPath1 = "f1e4fb55e490fe640a2524c82403070a"); }
	
		set { par.highligterExternalShaderPath1 = value; }
	}
	internal string HIGHLIGHTER_SHADER_GUID_ADD
	{	get { return par.highligterExternalShaderPath2 ?? (par.highligterExternalShaderPath2 = "8ff8db82c546a894689f103948df51a2"); }
	
		set { par.highligterExternalShaderPath2 = value; }
	}
	internal bool HIGHLIGHTER_TEXTURE_GUID_ALLOW
	{	get { return HIGHLIGHTER_TEXTURE_STYLE == 3; }
	
		set {  }
	}
	bool? __USE_LABEL_OFFSET;
	internal bool USE_LABEL_OFFSET
	{	get { return __USE_LABEL_OFFSET ?? (__USE_LABEL_OFFSET = par.highligterExternalTexturePath == "19b7d3f9eb031ad4a9d63d48600cb49b") .Value; }
	}
	internal string HIGHLIGHTER_TEXTURE_GUID
	{	get { return par.highligterExternalTexturePath ?? (par.highligterExternalTexturePath = "19b7d3f9eb031ad4a9d63d48600cb49b"); }
	
		set
		{	par.highligterExternalTexturePath = value;
			__USE_LABEL_OFFSET = null;
		}
	}
	internal int HIGHLIGHTER_TEXTURE_STYLE
	{	get {   return par.highligterTextureStyle ?? (par.highligterTextureStyle = 3).Value;  }
	
		set { par.highligterTextureStyle = value; }
	}
	internal int HIGHLIGHTER_TEXTURE_BORDER
	{	get { return par.highligterTextureBorder ?? (par.highligterTextureBorder = 6).Value; }
	
		set { par.highligterTextureBorder = value; }
	}
	internal bool HIGHLIGHTER_TEXTURE_BORDER_ALLOW
	{	get { return HIGHLIGHTER_TEXTURE_STYLE != 0; }
	
		set {  }
	}
	
	internal int HIGHLIGHTER_LEFT_OVERFLOW
	{	get
		{	if ( Adapter.UNITY_CURRENT_VERSION < Adapter.UNITY_2019_VERSION ) return 0;
		
			return par.HIGHLIGHTER_LEFT_OVERFLOW ?? (par.HIGHLIGHTER_LEFT_OVERFLOW = 1).Value;
		}
		
		set { par.HIGHLIGHTER_LEFT_OVERFLOW = value; }
	}
	
	internal float ADDITIONA_INPUT_WINDOWS_WIDTH
	{	get { return par.ADDITIONA_INPUT_WINDOWS_WIDTH ?? (par.ADDITIONA_INPUT_WINDOWS_WIDTH = 1.2f).Value;  }
	
		set { par.ADDITIONA_INPUT_WINDOWS_WIDTH = value; }
	}
	//  Color ___CHILDREN_LINE_COLOR_Hierarchy_pro = new Color32( 96, 96, 96, 90 );
	Color ___CHILDREN_LINE_COLOR_Hierarchy_pro = new Color( 0.8f, 0.8f, 0.8f, 0.35f );
	//  Color ___CHILDREN_LINE_COLOR_Hierarchy_pro = new Color32( 0, 0, 0, 12 );
	Color ___CHILDREN_LINE_COLOR_Hierarchy = new Color( .5f, .5f, .5f, 100 / 255F );
	// Color ___CHILDREN_LINE_COLOR_Project_pro = new Color( 34 / 255F, 34 / 255F, 34 / 255F, 100 / 255F );
	Color ___CHILDREN_LINE_COLOR_Project = new Color( 34 / 255F, 34 / 255F, 34 / 255F, 100 / 255F );
	internal Color CHILDREN_LINE_COLOR
	{	get
		{	if ( EditorGUIUtility.isProSkin )
			{	if ( par.CHILD_LINE_HR_COLOR_PRO != null )
				{	c.r = par.CHILD_LINE_HR_COLOR_PRO.Value.r;
					c.g = par.CHILD_LINE_HR_COLOR_PRO.Value.g;
					c.b = par.CHILD_LINE_HR_COLOR_PRO.Value.b;
					c.a = par.CHILD_LINE_HR_COLOR_PRO.Value.a;
					return c;
				}
				
				return ___CHILDREN_LINE_COLOR_Hierarchy_pro;
			}
			
			else
			{	if ( par.CHILD_LINE_HR_COLOR_PERSONAL != null )
				{	c.r = par.CHILD_LINE_HR_COLOR_PERSONAL.Value.r;
					c.g = par.CHILD_LINE_HR_COLOR_PERSONAL.Value.g;
					c.b = par.CHILD_LINE_HR_COLOR_PERSONAL.Value.b;
					c.a = par.CHILD_LINE_HR_COLOR_PERSONAL.Value.a;
					return c;
				}
				
				return IS_HIERARCHY() ? ___CHILDREN_LINE_COLOR_Hierarchy : ___CHILDREN_LINE_COLOR_Project;
				// return ___CHILDREN_LINE_COLOR;
			}
		}
		
		set
		{	if ( EditorGUIUtility.isProSkin )
			{	par.CHILD_LINE_HR_COLOR_PRO = new SerializeColor( value.r, value.g, value.b, value.a );
			}
			
			else
			{	par.CHILD_LINE_HR_COLOR_PERSONAL = new SerializeColor( value.r, value.g, value.b, value.a );
			}
		}
	}
	
	internal int PADDING_RIGHT_MODIFIED
	{	get
		{	return 0;
		}
	}
	
	internal bool HAS_HOVER
	{	get
		{	return hashoveredItem;
		}
	}
	
	
	//0  - deafault ; 1 - left ; 2 - right small
	internal int SETACTIVE_POSITION
	{	get
		{	if ( !par.SETACTIVE_POSITION.HasValue )
			{	if (UNITY_CURRENT_VERSION >= UNITY_2019_1_1_VERSION   )
				{	par.SETACTIVE_POSITION = 1; //0
				}
				
				else
				{	par.SETACTIVE_POSITION = 0;
				}
			}
			
			return par.SETACTIVE_POSITION.Value;
		}
		
		set { par.SETACTIVE_POSITION = value; }
	}
	
	internal int SETACTIVE_STYLE
	{	get
		{	if ( !par.SETACTIVE_STYLE.HasValue )
				//{
			{	//par.SETACTIVE_STYLE = EditorGUIUtility.isProSkin ? 1 : 0;
				par.SETACTIVE_STYLE = 0;
			}
			
			return par.SETACTIVE_STYLE.Value;
		}
		
		set { par.SETACTIVE_STYLE = value; }
	}
	internal bool SHOW_ONLY_HOVERED
	{	get { return (Event.current == null || !Event.current.alt) &&SHOW_ONLY_HOVERED_RAW; }
	
	}
	internal bool SHOW_ONLY_HOVERED_RAW
	{	get { return (par.SHOW_ONLY_HOVERED ?? 0) == 1; }
	
		set { par.SHOW_ONLY_HOVERED = value ? 1 : 0; }
	}
	
	internal bool FAVORITS_SHOWDESICON
	{	get { return false; }
	
		set { par.BottomParams.FAVORITS_SHOWDESICON = value; }
	}
	
	internal bool DataKeeperParamsUSE_SCRIPTS
	{	get { return par.DataKeeperParams.USE_SCRIPTS; }
	
		set { par.DataKeeperParams.USE_SCRIPTS = value; }
	}
	internal bool DataKeeperParamsSAVE_ENABLINGDISABLING_GAMEOBJEST
	{	get { return par.DataKeeperParams.SAVE_ENABLINGDISABLING_GAMEOBJEST; }
	
		set { par.DataKeeperParams.SAVE_ENABLINGDISABLING_GAMEOBJEST = value; }
	}
	internal bool DataKeeperParamsSAVE_GAMEOBJET_HIERARCHY
	{	get
		{	return par.DataKeeperParams.SAVE_GAMEOBJET_HIERARCHY;
		}
		
		set { par.DataKeeperParams.SAVE_GAMEOBJET_HIERARCHY = value; }
	}
	internal bool DataKeeperParamsSAVE_UNITYOBJECT
	{	get { return par.DataKeeperParams.SAVE_UNITYOBJECT; }
	
		set { par.DataKeeperParams.SAVE_UNITYOBJECT = value; }
	}
	internal bool DataKeeperParamsENABLE
	{	get { return par.DataKeeperParams.ENABLE; }
	
		set { par.DataKeeperParams.ENABLE = value; }
	}
	internal float BROADCASTING_PREFOMANCE01
	{	get { return (par.BROADCASTING_PREFOMANCE - 10f) / 2f; }
	
		set { par.RIGHTDOCK_TEMPHIDEMINWIDTH = value * 2 + 10; }
	}
	
	
	internal string USE_BUTTON_TO_INTERACT_AHC_KEY
	{	get
		{	var new_i = par.USE_BUTTON_TO_INTERACT_AHC & 3;
			var cts = new [] {"...", "'Alt'", "'Shift'", "'Ctrl'" };
			var key = cts[new_i];
			return key;
		}
	}
	internal bool USE_BUTTON_TO_INTERACT_AHC_BOOL
	{	get { return (par.USE_BUTTON_TO_INTERACT_AHC & 3) != 0; }
	}
	internal int RIGHTDOCK_TEMPHIDEMINWIDTH
	{	get { return Mathf.RoundToInt( par.RIGHTDOCK_TEMPHIDEMINWIDTH ); }
	
		set { par.RIGHTDOCK_TEMPHIDEMINWIDTH = value; }
	}
	internal int PADDING_RIGHT_SETTINGS
	{	get { return Mathf.RoundToInt( par.PADDING_RIGHT ); }
	
		set { par.PADDING_RIGHT = value; }
	}
	internal bool PLAYMODE_ShowComponents2
	{	get { return !par.PLAYMODE_HideComponents2; }
	
		set { par.PLAYMODE_HideComponents2 = !value; }
	}
	
	internal bool PLAYMODE_ShowRightPanel
	{	get { return !par.PLAYMODE_HideRightPanel; }
	
		set { par.PLAYMODE_HideRightPanel = !value; }
	}
	internal bool SaveSettingsCustomIconsToAsset
	{	get { return !par.SaveSettingsCustomIconsToLibrary; }
	
		set { par.SaveSettingsCustomIconsToLibrary = !value; }
	}
	internal bool SaveSettingsHighLighterToAsset
	{	get { return !par.SaveSettingsHighLighterToLibrary; }
	
		set { par.SaveSettingsHighLighterToLibrary = !value; }
	}
	internal bool SHOWUPDATINGINDICATOR
	{	get { return par.HiperGraphParams.SHOWUPDATINGINDICATOR; }
	
		set { par.HiperGraphParams.SHOWUPDATINGINDICATOR = value; }
	}
	internal bool RED_HIGKLIGHTING
	{	get { return par.HiperGraphParams.RED_HIGKLIGHTING; }
	
		set { par.HiperGraphParams.RED_HIGKLIGHTING = value; }
	}
	internal bool NOTAUTOHIDE
	{	get { return !par.HiperGraphParams.AUTOHIDE; }
	
		set { par.HiperGraphParams.AUTOHIDE = !value; }
	}
	internal bool AUTOHIDE
	{	get { return par.HiperGraphParams.AUTOHIDE; }
	
		set { par.HiperGraphParams.AUTOHIDE = value; }
	}
	internal bool AUTOCHANGE
	{	get { return par.HiperGraphParams.AUTOCHANGE; }
	
		set { par.HiperGraphParams.AUTOCHANGE = value; }
	}
	
	internal bool SHOW_PARENT_TREE
	{	get { return false; }
	
		set { par.SHOW_PARENT_TREE = value; }
	}
	internal bool DONTSHOW_PARENT_TREE_CURRENTOBJECT
	{	get { return !par.SHOW_PARENT_TREE_CURRENTOBJECT; }
	
		//  get { return !par.SHOW_PARENT_TREE_CURRENTOBJECT; }
		set { par.SHOW_PARENT_TREE_CURRENTOBJECT = !value; }
	}
	internal bool BOTTOM_TOOLTIPES
	{	get { return par.BottomParams.BOTTOM_TOOLTIPES; }
	
		set { par.BottomParams.BOTTOM_TOOLTIPES = value; }
	}
	
	
	internal bool DRAW_FOLDER_STARMARK
	{	get { return par.BottomParams.DRAW_FOLDER_STARMARK; }
	
		set { par.BottomParams.DRAW_FOLDER_STARMARK = value; }
	}
	
	internal bool _S_HG_ShowSelf
	{	get { return (bool? )par.HG_ShowSelf ?? true; }
	
		set
		{	if ( pluginID != 0 ) return;
		
			par.HG_ShowSelf = value;
			bottomInterface.hyperGraph.RefreshWithCurrentSelection();
		}
	}
	internal bool _S_HG_ShowAssets
	{	get { return (bool? )par.HG_ShowAssets ?? true; }
	
		set
		{	if ( pluginID != 0 ) return;
		
			par.HG_ShowAssets = value;
			bottomInterface.hyperGraph.RefreshWithCurrentSelection();
		}
	}
	internal int _S_HG_SkipArrays
	{	get { return (int? )par.HG_SkipArrays ?? 0; } //2
	
		set
		{	if ( pluginID != 0 ) return;
		
			par.HG_SkipArrays = value;
			bottomInterface.hyperGraph.RefreshWithCurrentSelection();
		}
	}
	internal int _S_HG_EventsMode
	{	get { return (int? )par.HG_EventsMode ?? 0; }
	
		set
		{	if ( pluginID != 0 ) return;
		
			par.HG_EventsMode = value;
			bottomInterface.hyperGraph.RefreshWithCurrentSelection();
		}
	}
	
	
	
	
	
	
	internal bool SAVE_LASTSELECTION_BEFORESCENEWILLSCLOSE
	{	get { return (bool? )par.SAVE_LASTSELECTION_BEFORESCENEWILLSCLOSE ?? false; }
	
		set { par.SAVE_LASTSELECTION_BEFORESCENEWILLSCLOSE = value; }
	}
	
	internal string[] AllWindows = { "SceneView", "Inspector", "GameView", "SceneHierarchy"/*, "ProjectBrowser"*/};
	
	
	Dictionary<string, bool> __CUSTOMMENU_HOTKEYS_WINDOWS;
	internal Dictionary<string, bool> CUSTOMMENU_HOTKEYS_WINDOWS
	{	get
		{	if ( __CUSTOMMENU_HOTKEYS_WINDOWS == null )
			{	__CUSTOMMENU_HOTKEYS_WINDOWS = new Dictionary<string, bool>();
			
				if ( par.CUSTOMMENU_HOTKEYS_WINDOWS == null )
				{	if ( IS_HIERARCHY() ) par.CUSTOMMENU_HOTKEYS_WINDOWS = AllWindows.Aggregate( ( a, b ) => a + " " + b );
					else par.CUSTOMMENU_HOTKEYS_WINDOWS = "";
					
					SavePrefs();
				}
				
				if ( !string.IsNullOrEmpty( par.CUSTOMMENU_HOTKEYS_WINDOWS ) )
					foreach ( var item in par.CUSTOMMENU_HOTKEYS_WINDOWS.Split( ' ' ) ) if ( !__CUSTOMMENU_HOTKEYS_WINDOWS.ContainsKey( item ) ) __CUSTOMMENU_HOTKEYS_WINDOWS.Add( item, true );
			}
			
			return __CUSTOMMENU_HOTKEYS_WINDOWS;
		}
		
		set
		{	if ( value.Keys.Count == 0 ) par.CUSTOMMENU_HOTKEYS_WINDOWS = "";
			else par.CUSTOMMENU_HOTKEYS_WINDOWS = value.Keys.Aggregate( ( a, b ) => a + " " + b );
			
			__CUSTOMMENU_HOTKEYS_WINDOWS = value;
		}
	}
	internal bool PREFAB_ESCAPE_ALL_WINDOWS { get { return (bool? )par.PREFAB_ESCAPE_ALL_WINDOWS ?? false; } set { par.PREFAB_ESCAPE_ALL_WINDOWS = value; } }
	
	internal int _S_CountNumber_OffsetX { get { return par.CHILDCOUNT_OFFSETX ?? 0; } set { par.CHILDCOUNT_OFFSETX = value; } }
	internal int _S_CountNumber_OffsetY { get { return (int)Mathf.Clamp( par.CHILDCOUNT_OFFSETY ?? (UNITY_CURRENT_VERSION >= UNITY_2019_1_1_VERSION ? 0 : 3), -par.LINE_HEIGHT / 2, par.LINE_HEIGHT / 2 ); } set { par.CHILDCOUNT_OFFSETY = value; } }
	//internal int _S_CountNumber_Align { get { return par.CHILDCOUNT_ALIGN ?? (UNITY_CURRENT_VERSION >= UNITY_2019_1_1_VERSION ? -1 : 0); } set { par.CHILDCOUNT_ALIGN = value; } }
	internal int _S_CountNumber_Align { get { return par.CHILDCOUNT_ALIGN ?? ( 0); } set { par.CHILDCOUNT_ALIGN = value; } }
	
	internal int _S_BottomPaddingForBgColor { get { return par.BottomPaddingForBgColor; } set { par.BottomPaddingForBgColor = value; } }
	internal bool _S_HideRightIfNoFunction { get { return par.HideRightIfNoFunction; } set { par.HideRightIfNoFunction = value; } }
	internal bool _S_HideBttomIfNoFunction { get { return par.HideBttomIfNoFunction; } set { par.HideBttomIfNoFunction = value; } }
	
	
	
	
	
	internal Color _S_TextColor
	{	get
		{	if ( EditorGUIUtility.isProSkin )
			{	if ( par.TEXT_COLOR_PRO.HasValue ) return par.TEXT_COLOR_PRO.Value;
			
				return new Color32( 180, 180, 180, 255 );
			}
			
			else
			{	if ( par.TEXT_COLOR.HasValue ) return par.TEXT_COLOR.Value;
			
				return Color.black;
			}
			
			
			// if (EditorGUIUtility.isProSkin) return GET_SKIN().label.normal.textColor;
			// return new Color32(46, 72, 134, 255); blue color
		}
		
		set
		{	if ( EditorGUIUtility.isProSkin )
				par.TEXT_COLOR_PRO = value;
			else
				par.TEXT_COLOR = value;
				
		}
	}
	
	internal bool _S_autorFiltersEnable { get { return par.autorFiltersEnable; } set { par.autorFiltersEnable = value; } }
	internal int _S_defaultIconSize { get { return Mathf.RoundToInt( par.defaultIconSize ); } set { par.defaultIconSize = value; } }
	internal bool _S_USEdefaultIconSize { get { return par.USEdefaultIconSize; } set { par.USEdefaultIconSize = value; } }
	
	
	internal bool _S_searchBintToLeft { get { return par.searchBintToLeft; } set { par.searchBintToLeft = value; } }
	internal float _S_searchWidthMulty { get { return par.SearchWidthMulty; } set { par.SearchWidthMulty = value; } }
	// internal int _S_bgColorDefaultAligment {get {return 0;} set { } }
#pragma warning disable
	internal int _S_bgButtonForIconsPlace // 0 - Left; 1 - Label ; 2 - Both
	{	get
		{	if ( IS_PROJECT() ) return 1;
		
			if ( !USE2018_3 && par.BgButtonIconsPlace > 0 ) return 0;
			
			return par.BgButtonIconsPlace_Nullable ?? (par.BgButtonIconsPlace_Nullable = UNITY_CURRENT_VERSION >= UNITY_2019_2_0_VERSION ? 0 : 0).Value;
		}
		
		set
		{	if ( IS_PROJECT() ) par.BgButtonIconsPlace = 1;
		
			if ( !USE2018_3 && par.BgButtonIconsPlace > 0 ) par.BgButtonIconsPlace = 0;
			else par.BgButtonIconsPlace = value;
		}
	}
	
	
	internal int _S_bgIconsPlace // 0 - Label ; 1 - Fold ; 2 - Align Left
	{	get
		{	if ( !USE2018_3 && par.BgIconsPlace == 0 ) return 2;
		
			return par.BgIconsPlace;
		}
		
		set
		{	if ( !USE2018_3 && par.BgIconsPlace == 1 ) par.BgIconsPlace = 0;
			else par.BgIconsPlace = value;
		}
	}
	
	// 0 - Label ; 1 - Fold ; 2 - Align Left
#pragma warning restore
	internal int _S_hoverState { get { return par.HoverState; } set { par.HoverState = value; } } // 0 - no ; 1 - only color window ; 2 - always
	
	
	internal bool _S_PresetsEnabled { get { return true; } set { } }
	internal bool _S_EnableWindowsAnimation { get { return true; } set { } }
	internal bool _S_USE_HIGLIGHT_IN_BOTTOM { get { return true; } set { } }
	internal static BgAligmentLeft[] BgAligmentLeftArray = {BgAligmentLeft.MinLeft, BgAligmentLeft.Fold, BgAligmentLeft.BeginLabel, BgAligmentLeft.EndLabel, BgAligmentLeft.Modules };
	internal enum BgAligmentLeft { MinLeft = 0, Fold = 1, BeginLabel = 2, EndLabel = 3, Modules = 4 }
	internal BgAligmentLeft BgAligmentToLeft( int source )
	{	return (BgAligmentLeft)(source & 7);
	}
	internal int BgLeftToAligment( int source, BgAligmentLeft leftAligment )
	{	return source & ~(7) | (int)leftAligment;
	}
	internal enum BgAligmentRight { MaxRight = 0, Modules = 1, EndLabel = 2, BeginLabel = 3, Icon = 4, WidthFixedGradient = 5 }
	internal static BgAligmentRight[] BgAligmentToRightArray = {BgAligmentRight.MaxRight, BgAligmentRight.Modules, BgAligmentRight.EndLabel, BgAligmentRight.BeginLabel, BgAligmentRight.Icon, BgAligmentRight.WidthFixedGradient};
	internal BgAligmentRight BgAligmentToRight( int source )
	{	return (BgAligmentRight)((source & (7 << 3) >> 3));
	}
	internal int BgRightToAligment( int source, BgAligmentRight leftAligment )
	{	return source & ~(7 << 3) | ((int)leftAligment << 3);
	}
	internal enum BgAligmentHeight { FullHeight = 0, Narrow = 1 }
	internal BgAligmentHeight BgAligmentToHeight( int source )
	{	return (BgAligmentHeight)((source & (1 << 6) >> 6));
	}
	internal int BgHeightToAligment( int source, BgAligmentHeight heightAligment )
	{	return source & ~(1 << 6) | ((int)heightAligment << 6);
	}
	/* internal enum BgAligmentRightFill {Solid = 0, Gradient = 1}
	 internal BgAligmentRightFill BgAligmentToFill(int source)
	 {   return (BgAligmentRightFill)((source & (1 << 7) >> 7));
	 }
	 internal int BgFilloAligment(int source, BgAligmentRightFill fillAligment)
	 {   return source & ~(1 << 7) | ((int)fillAligment << 7);
	 }*/
	int? _mLastBgAlignState = null;
	internal int LastBgAlignState
	{	get { return _mLastBgAlignState ?? (_mLastBgAlignState = EditorPrefs.GetInt( "EModules/" + pluginname + "/BgAlignLastState", 0 )).Value; }
	
		set
		{	if ( _mLastBgAlignState == value ) return;
		
			_mLastBgAlignState = value;
			EditorPrefs.SetInt( "EModules/" + pluginname + "/BgAlignLastState", value );
		}
	}
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	internal HierParams GET_DEFAULT_SETTINGS
	{	get
		{	var result = new HierParams()
			{	ENABLE_ALL = true,
				    //ENABLE_PING_Fix = true,
				    ENABLE_LEFTDOCK_FIX = true,
				    ENABLE_RIGHTDOCK_FIX = true,
				    //M_params = new Dictionary<Type, string>(),
				    FD_Icons_default = true,
				    FD_Icons_mono = true,
				    FD_Icons_user = true,
			};
			
#if UNITY_EDITOR
			
			if ( IS_PROJECT() )     //  Debug.Log( result.HierarhchyLines_Fix );
			{	/* var rr =  new EModules.EProjectInternal.HierParams()
				 {   ENABLE_ALL = true,
				         // ENABLE_PING_Fix = true,
				         ENABLE_LEFTDOCK_FIX = true,
				         ENABLE_RIGHTDOCK_FIX = true,
				         //M_params = new Dictionary<Type, string>(),
				         FD_Icons_default = true,
				         FD_Icons_mono = true,
				         FD_Icons_user = true,
				 };*/
				// Debug.Log( rr.HierarhchyLines_Fix );
				//  var ss = new UnionHierParams( rr );
				//  Debug.Log( ss.hierarchy.HierarhchyLines_Fix );
				//  result.DEEP_WIDTH = (int) EditorGUIUtility.singleLineHeight;// rr.DEEP_WIDTH;
				result.OVERRIDE_DEEP_WIDTH = true; // rr.OVERRIDE_DEEP_WIDTH;
				result.LINE_HEIGHT = EditorGUIUtility.singleLineHeight;// rr.LINE_HEIGHT;
				result.HIER_LINES_HR = 0;// rr.HIER_LINES_HR;
				result.DRAW_HIERARHCHY_LINES_V2 = 0;// rr.DRAW_HIERARHCHY_LINES_V2;
				result.SHOW_PARENT_TREE = false;// rr.SHOW_PARENT_TREE;
				result.HierarhchyLines_Fix = 2;// rr.HierarhchyLines_Fix;
				result.HIER_LINES_CHESSE = 0;// rr.HIER_LINES_CHESSE;
				result.COMPONENTS_NEXT_TO_NAME = false;// rr.COMPONENTS_NEXT_TO_NAME;
				result.DRAW_CHILDS_COUNT = false;// rr.DRAW_CHILDS_COUNT;
			}
			
#endif
			return result;
			
		}
	}
	
	
	
	internal string SettingsToString()
	{	SavePrefs();
		return EditorPrefs.GetString( GET_HIER_PARAM_KEY( this ) );
	}
	
	internal bool SettingsFromString( string settings )
	{
	
		HierParams? result = null;
		
		try
		{	result = Adapter.DESERIALIZE_SINGLE<HierParams>( settings );
		}
		
#pragma warning disable
		
		catch ( Exception ex ) { }
		
#pragma warning restore
		
		if ( result == null ) return false;
		
		par = result.Value;
		SavePrefs();
		RequestScriptReload();
		
		return true;
	}
	
	
	internal void ResetSettings()
	{	par = GET_DEFAULT_SETTINGS;
		SavePrefs();
		RequestScriptReload();
	}
	
	
	internal static string oldHierParamsGET_HIER_PARAM_KEY( Adapter adapter ) { return adapter.pluginID == Initializator.HIERARCHY_ID ? "HierParams" : "Project/EditorSettings"; }
	internal static string GET_HIER_PARAM_KEY_WITHOUTVERSIONS( Adapter adapter ) { return adapter.pluginID == Initializator.HIERARCHY_ID ? ("Hierarchy/EditorSettings") : ("Project/EditorSettings" ); }
	internal static string GET_HIER_PARAM_KEY( Adapter adapter ) { return adapter.pluginID == Initializator.HIERARCHY_ID ? ("Hierarchy/EditorSettings/" + Adapter.HIERARCHY_VERSION) : ("Project/EditorSettings/" + Adapter.HIERARCHY_VERSION); }
	internal static string GET_HIER_PARAM_KEY( Adapter adapter, int oldVersionIndex ) { return adapter.pluginID == Initializator.HIERARCHY_ID ? ("Hierarchy/EditorSettings/" + Adapter.HIERARCHY_OLDER_VERSIONS[oldVersionIndex]) : ("Project/EditorSettings/" + Adapter.HIERARCHY_OLDER_VERSIONS[oldVersionIndex]); }
	internal static int GET_HIER_PARAM_KEY_COUNT( Adapter adapter ) { return adapter.pluginID == Initializator.HIERARCHY_ID ? Adapter.HIERARCHY_OLDER_VERSIONS.Length : Adapter.HIERARCHY_OLDER_VERSIONS.Length; }
	
	
	
}









}
