#define DISABLE_PING

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





	internal static void ClearM_CustomIconsCache()
	{	updateTimer.Clear();
		cache.Clear();
		cacheMono.Clear();
		null_cache.Clear();
		missing_cache.Clear();
	}
	
	internal static Dictionary<int, double> updateTimer = new Dictionary<int, double>();
	internal static Dictionary<int, Component[]> cache = new Dictionary<int, Component[]>();
	internal static Component[] get_from_cache( int id, GameObject go)
	{	if ( !Hierarchy.cache.ContainsKey( id) )
		{	//Hierarchy.cache.Add( id, HierarchyExtensions.Utilities.GetComponentFast<Component>.GetAll( go ) );
		
			bool? haveMissing = null;
			var comps = HierarchyExtensions.Utilities.GetComponentFast<Component>.GetAll( go ).Where( c =>
			{	var result = (bool)c && (c.hideFlags & HideFlags.HideInInspector) == 0;
			
				if ( !c ) haveMissing = true;
				
				return result;
			} ).ToArray();
			
			Hierarchy.cache.Add( id, comps );
			
			if ( Adapter.HierAdapter.par.SHOW_MISSINGCOMPONENTS && haveMissing.HasValue )
			{	if ( !missing_cache.ContainsKey( id ) ) missing_cache.Add( id, false );
			
				missing_cache[id] = haveMissing.Value;
			}
			
		}
		
		return  Hierarchy.cache[id];
	}
	internal static Dictionary<int, MonoBehaviour[]> cacheMono = new Dictionary<int, MonoBehaviour[]>();
	internal static Dictionary<int, bool> null_cache = new Dictionary<int, bool>();
	internal static Dictionary<int, bool> missing_cache = new Dictionary<int, bool>();
	
	internal partial class M_CustomIcons : Adapter.Module, IModuleOnnector_M_CustomIcons {
	
		EventType? IModuleOnnector_M_CustomIcons.useEvent
		{	get { return M_CustomIcons.useEvent; }
		
			set { M_CustomIcons.useEvent = value; }
		}
		Dictionary<int, double> IModuleOnnector_M_CustomIcons.updateTimer { get { return updateTimer; } }
		
		
		
		
		
		
		
		protected override void OnEnableChange( bool value )
		{	base.OnEnableChange( value );
		}
		
		public M_CustomIcons( int restWidth, int sib, bool enable, Adapter adapter ) : base( restWidth, sib, enable, adapter )
		{	adapter.M_CustomIconsModule = this;
			adapter.onSelectionChanged -= RawOnRemoveRaw;
			adapter.onSelectionChanged += RawOnRemoveRaw;
			adapter.onPlayModeChanged -= RawOnRemoveRaw;
			adapter.onPlayModeChanged += RawOnRemoveRaw;
			// MonoBehaviour.print(AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(MonoScript.FromMonoBehaviour(GameObject.FindObjectOfType<Image>() as MonoBehaviour))));
			// MonoBehaviour.print(Utilites.ObjectContent(null, typeof(MonoBehaviour)).image);
		}
		
		/*  static Dictionary<int, List<Component>> md = new Dictionary<int, List<Component>>();
		  void Init(GameObject o)
		  {
		      if (!md.ContainsKey(o.GetInstanceID()))
		      {
		          if (md.Any(meshFilter => meshFilter.Value == null))
		              foreach (var source in md.Keys.ToArray().Where(source => md[source] == null))
		                  md.Remove(source);
		
		          md.Add(o.GetInstanceID(), o.GetComponent<MeshFilter>());
		      }
		      if (md[o.GetInstanceID()] == null) return width;
		  }*/
		//float height;
		//GUIContent tempc = new GUIContent();
		bool drawIcon = false;
		
		Rect drawRect;
		Rect firstRect;
		//  GameObject o;
		
		
		
		Color t1 = new Color( .8f, .8f, .8f, .03f );
		internal override void InitializeModule()     // t1 = adapter.GET_TEXTURE( 0x006, new Color( .8f, .8f, .8f, .03f ) );
		{	/*    t1 = new Texture2D(1, 1, TextureFormat.ARGB32, false, true) { hideFlags = HideFlags.DontSave };
			    t1.SetPixel(0, 0, new Color(.8f, .8f, .8f, .03f));
			    t1.Apply();*/
			base.InitializeModule();
		}
		
		
		
		
		
		
		
		
		internal static EventType? useEvent = null;
		static Component[] comps;
		//static Component[] redrawComps = new Component[1];
		static bool DRAW_NEXTTONAME;
		static float Y, HEIGHT;
		static Color TC;
		// Adapter.HierarchyObject __o;
		internal override float Draw( Rect _drawRect, Adapter.HierarchyObject _o )
		{	var o = _o.go;
		
			//if (Adapter.OPT_EV_BR(Event.current)) return 0;
			
			// __o = _o;
			if ( o == null || (Application.isPlaying && adapter.par.PLAYMODE_HideComponents2) ) return width;
			
			if ( !START_DRAW( _drawRect, _o ) ) return 0;
			
			ClearIconPos( _o.id );
			
			DRAW_NEXTTONAME = adapter.par.COMPONENTS_NEXT_TO_NAME && !callFromExternal();
			
			
			if ( DRAW_NEXTTONAME && adapter.HAS_LABEL_ICON() )
			{
			
				if ( adapter._S_USEdefaultIconSize )
					_drawRect.x += adapter.DEFAULT_ICON_SIZE;
				else _drawRect.x += EditorGUIUtility.singleLineHeight;
			}
			
			
			drawRect = _drawRect;
			
			Y = drawRect.y;
			HEIGHT = drawRect.height;
			/* Hierarchy.colorText11.SetPixel(0, 0, new Color(.1f, .1f, .1f, .1f));
			Hierarchy.colorText11.Apply();*/
			// Hierarchy.colorText11half.SetPixel(0, 0, new Color(.1f, .1f, .1f, .15f));
			// Hierarchy.colorText11half.SetPixel(0, 0, new Color(.8f, .8f, .8f, .03f));
			// Hierarchy.colorText11half.Apply();
			firstRect = drawRect;
			// if (!md.ContainsKey(o.GetInstanceID())) Init(o);
			// GUI.DrawTexture(drawRect, Hierarchy.colorText11half);
			//  height = drawRect.height;
			
			if ( !DRAW_NEXTTONAME )
			{	drawRect.x += drawRect.width + Hierarchy.HierarchyAdapterInstance.par.ICONSPACE;
			}
			
			
			var oldH = drawRect.height;
			drawRect.height = 12;
			drawRect.width = 10;
			drawRect.y += (oldH - drawRect.height) / 2;
			drawRect.x -= 1;
			
			if ( DRAW_NEXTTONAME )
				drawRect.x += drawRect.width / 2;
				
				
			// var wasMono = false;
			// o.c
			/*  Component[] comps;
			  if (Event.current.type == EventType.repaint) {
			      if (cache.ContainsKey(o.GetInstanceID())) {
			          comps = cache[o.GetInstanceID()];
			          bool needClear = false;
			          for (int i = 0; i < comps.Length && !needClear; i++) if (!comps[i]) needClear = true;
			          if (needClear) {
			              comps = o.GetComponents<Component>().Where(c => c).ToArray();
			              cache[o.GetInstanceID()] = comps;
			          }
			      } else {
			          comps = o.GetComponents<Component>().Where(c => c).ToArray();
			          cache.Add(o.GetInstanceID(), comps);
			      }
			  } else {
			      if (cache.ContainsKey(o.GetInstanceID())) {
			          comps = cache[o.GetInstanceID()];
			          bool needClear = false;
			          for (int i = 0; i < comps.Length && !needClear; i++) if (!comps[i]) needClear = true;
			          if (needClear) {
			              comps = o.GetComponents<Component>().Where(c => c).ToArray();
			              cache[o.GetInstanceID()] = comps;
			          }
			      } else comps = new Component[0];
			  }*/
			// comps[0].GetInstanceID();
			// EditorUtility.
			//    o.
			/* if (drawModule is M_CustomIcons) */
			
			bool? haveNull = null;
			bool? haveMissing = null;
			
			var id = o.GetInstanceID();
			
			if ( !callFromExternal() && useEvent == null && adapter.pluginID == 0 ) useEvent = Event.current.type;
			
			var needAssign = false;
			
			if ( Application.isPlaying && adapter.par.PLAYMODE_UseBakedComponents ) needAssign = callFromExternal() || !cache.ContainsKey( id );
			else needAssign = callFromExternal() || !adapter.NEW_PERFOMANCE && Event.current.type == useEvent || !cache.ContainsKey( id );
			
			if ( needAssign/* || !cache.ContainsKey(id)*/)     //InternalEditorUtility.game
			{
			
			
			
				bool asd = false;
				
				if ( !adapter.NEW_PERFOMANCE )
				{	if ( !updateTimer.ContainsKey( o.GetInstanceID() ) ) updateTimer.Add( o.GetInstanceID(), 555 );
				
					asd = Math.Abs( EditorApplication.timeSinceStartup - updateTimer[id] ) > 0.5;
				}
				
				if ( !cache.ContainsKey( id ) || !adapter.NEW_PERFOMANCE && asd )
				{	if ( updateTimer.ContainsKey( id ) ) updateTimer[id] = EditorApplication.timeSinceStartup;
				
					haveMissing = false;
					
					comps = HierarchyExtensions.Utilities.GetComponentFast<Component>.GetAll( o ).Where( c =>
					{	var result = (bool)c && (c.hideFlags & HideFlags.HideInInspector) == 0;
					
						if ( !c ) haveMissing = true;
						
						return result;
					} ).ToArray();
					
					
					
					if ( !cache.ContainsKey( id ) ) cache.Add( id, comps );
					else cache[id] = comps;
					
					if ( !cacheMono.ContainsKey( id ) ) cacheMono.Add( id, comps.Where( _IsMonoBehaviour ).Select( c => (MonoBehaviour)c ).ToArray() );
					else cacheMono[id] = comps.Where( _IsMonoBehaviour ).Select( c => (MonoBehaviour)c ).ToArray();
					
					
					if ( adapter.par.DataKeeperParams.ENABLE && adapter.par.DataKeeperParams.USE_SCRIPTS )
					{	Hierarchy_GUI.Instance( adapter ).DataKeeper_Check( o, comps );
					}
					
				}
				
				
				
				
				
				
				
				
			} /*else
			
                {
                }*/

			comps = cache[id];
			
			
			
			if ( haveMissing.HasValue && !haveMissing.Value ) haveNull = comps.Length == 1;
			
			
			if ( adapter.par.SHOW_NULLS && haveNull.HasValue )
			{	if ( !null_cache.ContainsKey( id ) ) null_cache.Add( id, false );
			
				null_cache[id] = haveNull.Value;
			}
			
			if ( adapter.par.SHOW_MISSINGCOMPONENTS && haveMissing.HasValue )
			{	if ( !missing_cache.ContainsKey( id ) ) missing_cache.Add( id, false );
			
				missing_cache[id] = haveMissing.Value;
			}
			
			
			if ( !cacheMono.ContainsKey( id ) ) cacheMono.Add( id, comps.Where( _IsMonoBehaviour ).Select( c => (MonoBehaviour)c ).ToArray() );
			
			if ( typeFillter == null ) _REDRAW( _o, comps, cacheMono[id] );
			else
			{	/* for (int i = 0; i < comps.Length; i++) {
				     if ()
				 }*/
				var f = comps.Any(c => Adapter.GetType_(c) == typeFillter);
				
				if ( f )
				{	var dada = comps.Where( c => Adapter.GetType_( c ) == typeFillter ).ToArray();
					var mm =  dada.Where( _IsMonoBehaviour ).Select( c => (MonoBehaviour)c ).ToArray();
					_REDRAW( _o, dada, mm );
				}
				
				{	// _REDRAW( comps.Where( c => Adapter.GetType_( c ) != typeFillter ).ToArray() );
					var dada = comps.Where( c => Adapter.GetType_( c ) != typeFillter ).ToArray();
					var mm =  dada.Where( _IsMonoBehaviour ).Select( c => (MonoBehaviour)c ).ToArray();
					
					_REDRAW( _o, dada, mm );
					
				}
				
				typeFillter = null;
			}
			
			
			END_DRAW( _o );
			
			//  Adapter.GET_SKIN().button.normal.background = bg;
			return width;
		}
		int i;
		int asd;
		// int index;
		// Dictionary<string, int> customIconsIndexOf = new Dictionary<string, int>();
		Dictionary<string, Texture2D> customIconsTexture2D = new Dictionary<string, Texture2D>();
		Dictionary<int, string> menuText = new Dictionary<int, string>();
		string keyGuid;
		bool have222 = false;
		int[] indexes = new int[20];
		int[] sortindexes = new int[20];
		// private Hierarchy_GUI.CustomIconParams[] loaded = new Hierarchy_GUI.CustomIconParams[20];
		int max = -1;
		int findindex = -1;
		int lim = -1;
		int interator = -1;
		
		void _REDRAW(Adapter.HierarchyObject _o, Component[] comps, MonoBehaviour[] monocomps )
		{	drawIcon = false;
			var wasDDD = false;
			
			if ( Hierarchy.HierarchyAdapterInstance.par.FD_Icons_user )
			{	have222 = false;
			
				if ( indexes.Length < comps.Length ) Array.Resize( ref indexes, comps.Length );
				
				// if (loaded.Length < comps.Length) Array.Resize(ref loaded, comps.Length);
				if ( sortindexes.Length < comps.Length + 1 ) Array.Resize( ref sortindexes, comps.Length + 1 );
				
				for ( i = 0 ; i < comps.Length ; i++ )
				{	if ( HaveUserIcon( comps[i] ) )
					{	keyGuid = ComponentToGUID( comps[i] );
						/*  if (!customIconsIndexOf.ContainsKey(keyGuid))
						  {
						      index = Hierarchy.HierParamsHandler.customIcons.IndexOf(ComponentToGUID(comps[i]));
						      customIconsIndexOf.Add(keyGuid, index);
						  }*/
						indexes[i] = customIcons.IndexOf( ComponentToGUID( comps[i] ) );
						//loaded[i] = Hierarchy.HierParamsHandler.customIcons[keyGuid];
						
						have222 = true;
					}
					
					else
					{	indexes[i] = -1;
					}
				}
				
				if ( have222 )
				{
				
					lim = int.MaxValue;
					interator = 0;
					
					for ( asd = 0 ; asd < comps.Length ; asd++ )
					{	max = -1;
					
						for ( i = 0 ; i < comps.Length ; i++ )     // if (indexes[i] == -1) continue;
						{	if ( indexes[i] > max && indexes[i] < lim )
							{	findindex = i;
								max = indexes[i];
							}
						}
						
						if ( max == -1 ) break;
						
						lim = max;
						sortindexes[interator] = findindex;
						interator++;
					}
					
					sortindexes[interator] = -1;
					/*  readcomps = comps.Where(c => c is MonoBehaviour && HaveUserIcon(c));
					  var guid = ComponentToGUID(id);
					  have = readcomps.OrderByDescending(id => ));
					*/
					
					for ( asd = 0 ; asd < sortindexes.Length ; asd++ )
						//foreach (var comp in have)
					{	if ( sortindexes[asd] == -1 ) break;
					
						i = sortindexes[asd];
						// if (!(component is MonoBehaviour)) continue;
						keyGuid = ComponentToGUID( comps[i] );
						load = customIcons[keyGuid];
						
						if ( !string.IsNullOrEmpty( load.value ) && !customIconsTexture2D.ContainsKey( load.value ) )
						{	var valuePath = AssetDatabase.GUIDToAssetPath(load.value);
						
							if ( string.IsNullOrEmpty( valuePath ) ) continue;
							
							customIconsTexture2D.Add( load.value, AssetDatabase.LoadAssetAtPath<Texture2D>( valuePath ) );
							//var value = AssetDatabase.LoadAssetAtPath<Texture2D>(valuePath);
						}
						
						if ( string.IsNullOrEmpty( load.value ) || customIconsTexture2D[load.value] == null ) continue;
						
						
						if ( !DRAW_NEXTTONAME ) drawRect.x -= drawRect.width + Hierarchy.HierarchyAdapterInstance.par.ICONSPACE;
						
						//tempc.tooltip = comps[i].GetType().ToString();
						/*if ( drawRect.Contains( Event.current.mousePosition ) )
						{	var path = AssetDatabase.GUIDToAssetPath(keyGuid);
							path = path.Substring( path.LastIndexOf( '/' ) + 1 );
						
							if ( path.LastIndexOf( '.' ) != -1 ) path = path.Remove( path.LastIndexOf( '.' ) );
						
							tempc.tooltip = path;
						}*/
						
						var overImage = customIconsTexture2D[load.value];
						tempArr[0] = comps[i];
						//  GUI.DrawTexture(drawRect, Hierarchy._colorText11half);
						TC = load.color;
						TC.a = TC.a / 1.5f + 0.3334f;
						
						
						// MenuText = "Open The Settings To Hide User Icon...";
						// MenuText = null;
						if ( !menuText.ContainsKey( comps[i].GetInstanceID() ) ) menuText.Add( comps[i].GetInstanceID(), "Hide " + comps[i].GetType().Name + " icon" );
						
						var MenuText = menuText[comps[i].GetInstanceID()];
						Type callbackType = null;
						var allowHide = false;
						/////   set_drawComps( _o .id, tempArr ) ;
						//   allComps = comps;
						color = TC;
						drawName = false;
						DrawIcon( tempArr, _o, drawRect, overImage, callbackType, allowHide, ref MenuText );
						//  DrawIcon(o, drawRect, tempc, , null, false, tempArr, comps, TC);
						
						wasDDD = true;
						
						if ( DRAW_NEXTTONAME ) drawRect.x += drawRect.width + Hierarchy.HierarchyAdapterInstance.par.ICONSPACE;
					}
				}
			}
			
			if ( wasDDD )
			{	if ( !DRAW_NEXTTONAME ) drawRect.x -= 2f;
				else drawRect.x += 2f;
			}
			
			
			// drawRect.x -= 2;
			
			if ( Hierarchy.HierarchyAdapterInstance.par.FD_Icons_mono )
			{	//var cc = comps.Where(IsMonoBehaviour).ToArray();
				var cc = monocomps;
				
				
				if ( adapter.par.COMPS_SplitMode == 0 )
				{
					{	// var context = Utilites.ObjectContent(null, cc[0].GetType());
						var hasCallBack = cc.Count(c=>c) == 1;
						MonDrawer( _o, cc, hasCallBack ? Adapter.GetType_( cc.First(c=>c) ): null, false);
						
					}
				}
				
				else
					if ( adapter.par.COMPS_SplitMode == 1 )
					{	var hasCallBack = cc.Count(c=>c&& (c).enabled) == 1;
						MonDrawer( _o, cc.Where( c => (c).enabled ).ToArray(), hasCallBack ? Adapter.GetType_( cc.First(c=>c && (c).enabled) ): null, false);
						hasCallBack = cc.Count(c=>c&& !(c).enabled) == 1;
						MonDrawer( _o, cc.Where( c => !(c).enabled ).ToArray(), hasCallBack ? Adapter.GetType_( cc.First(c=>c && !(c).enabled) ): null, false);
					}
					
					else
					{	foreach ( var component in cc )
						{	tempArr[0] = component;
							var callbackType = Adapter.GetType_( component );
							MonDrawer( _o, tempArr, callbackType, false );
						}
					}
					
				if ( !DRAW_NEXTTONAME ) drawRect.x -= 2f;
				else drawRect.x += 2f;
				
			}
			
			
			foreach ( var c in comps )
			{	if ( !c ) continue;
			
				type = Adapter.GetType_( c );
				types = type;
				
				// if (comp is Transform /*|| comp is MeshRenderer*/ || comp is CanvasRenderer) return -1;
				//  if (types == TransformTypeName /*|| comp is MeshRenderer*/ || types == CanvasRendererTypeName) continue;
				if ( c is Transform/* || c is MeshRenderer*/ || c is CanvasRenderer /*|| IsMonoBehaviour(c)*/) continue;
				
				var context = Utilities.ObjectContent_NoCacher(adapter, (UnityEngine.Object)null, type);
				
				if ( !context.add_icon ) continue;
				
				/*  if (context.image == null)
				  {
				      if (wasMono || !Hierarchy.par.FD_Icons_mono) continue;
				      context.image = Hierarchy.GetIcon("MONO");
				      context.tooltip = comps.Where(comp => comp is MonoBehaviour).Select(comp => comp.GetType().ToString()).Aggregate((a, b) => a + '\n' + b);
				      wasMono = true;
				  }
				  else*/
				
				if ( !Hierarchy.HierarchyAdapterInstance.par.FD_Icons_default || Hierarchy_GUI.HideIcon( Adapter.GetTypeFullName( c ), adapter ) ) continue;
				
				
				if ( !DRAW_NEXTTONAME ) drawRect.x -= drawRect.width + Hierarchy.HierarchyAdapterInstance.par.ICONSPACE;
				
				//if ( drawRect.Contains( Event.current.mousePosition ) )
				{	//context.tooltip = context.tooltip.Replace( "None", "" ).Replace( "(", "" ).Replace( ")", "" );
				}
				
				tempArr[0] = c;
				
				
				//this.context.tooltip = context.tooltip;
				
				if ( !menuText.ContainsKey( c.GetInstanceID() ) ) menuText.Add( c.GetInstanceID(), "Hide " + c.GetType().Name + " icon" );
				
				var MenuText = menuText[c.GetInstanceID()];
				// MenuText = "Hide " + c.GetType().Name + " icon";
				var callbackType = Adapter.GetType_( c );
				var allowHide = true;
				// drawComps = tempArr;
				///// set_drawComps( _o.id, tempArr ) ;
				// allComps = comps;
				color = Color.white;
				drawName = false;
				
				DrawIcon(tempArr, _o, drawRect, (Texture2D)context.add_icon, callbackType, allowHide, ref MenuText);
				// DrawIcon(o, drawRect, context, "Hide " + c.GetType().Name + " icon", c.GetType(), true, tempArr, comps, Color.white);
				
				
				if ( DRAW_NEXTTONAME ) drawRect.x += drawRect.width + Hierarchy.HierarchyAdapterInstance.par.ICONSPACE;
				
				//GUI.DrawTexture(drawRect, Hierarchy.GetIcon("BUT"));
				
				//
				/*  context.image = null;
				  context.text = "asd";
				  GUI.Label(drawRect, context);*/
			}
			
			if ( drawIcon && !DRAW_NEXTTONAME )
			{	// Adapter.DrawTexture( firstRect , t1 );
				Draw_AdapterTexture( firstRect, t1 );
			}
			
			
		}
		
		
		
		//GameObjectCacher<string> cache = new GameObjectCacher<string>();
		Dictionary<int, string> GUID_cache = new Dictionary<int, string>();
		string ComponentToGUID( Component component )
		{	if ( !GUID_cache.ContainsKey( component.GetInstanceID() ) )
			{	GUID_cache.Add( component.GetInstanceID(), null );
				var t = component as MonoBehaviour;
				
				if ( t )
				{	var mono = MonoScript.FromMonoBehaviour(t);
				
					if ( mono != null )
					{	var keyPath = AssetDatabase.GetAssetPath(mono);
					
						if ( !string.IsNullOrEmpty( keyPath ) )
							GUID_cache[component.GetInstanceID()] = AssetDatabase.AssetPathToGUID( keyPath );
					}
				}
				
			}
			
			/*component.GetType().*/
			
			
			return GUID_cache[component.GetInstanceID()];
		}
		bool HaveUserIcon( Component component )
		{	if ( !component ) return false;
		
			var keyGUID = ComponentToGUID(component);
			
			if ( string.IsNullOrEmpty( keyGUID ) ) return false;
			
			if ( !customIcons.ContainsKey( keyGUID ) ) return false;
			
			return true;
		}
		
		Type type;
		Type types;
		
		Dictionary<Type, int> GetComponentType_helper = new Dictionary<Type, int>();
		
		int GetComponentType( Component comp )
		{	if ( !comp ) return -1;
		
			/* if ((comp.hideFlags & HideFlags.HideInInspector) != 0)
			 {
			   Debug.Log( comp );
			   GetComponentType_helper.Add( types, -1 );
			   return -1;
			 }*/
			
			type = Adapter.GetType_( comp );
			types = type;
			
			if ( GetComponentType_helper.ContainsKey( types ) ) return GetComponentType_helper[types];
			
			if ( comp is Transform /*|| comp is MeshRenderer*/ || comp is CanvasRenderer )
				// if (types == TransformTypeName /*|| comp is MeshRenderer*/ || types == CanvasRendererTypeName)
			{	GetComponentType_helper.Add( types, -1 );
				return -1;
			}
			
			var result = -1;
			
			if ( _IsMonoBehaviour( comp ) )
			{	if ( HaveUserIcon( comp ) && Hierarchy.HierarchyAdapterInstance.par.FD_Icons_user ) result = 0;
				else
					if ( Hierarchy.HierarchyAdapterInstance.par.FD_Icons_mono ) result = 1;
					else
					{	GetComponentType_helper.Add( types, -1 );
						return -1;
					}
			}
			
			if ( !Hierarchy.HierarchyAdapterInstance.par.FD_Icons_default || Hierarchy_GUI.Instance( adapter ).HiddenComponents.Contains( type.FullName ) )
			{	GetComponentType_helper.Add( types, -1 );
				return -1;
			}
			
			if ( result == -1 ) result = 2;
			
			if ( typeFillter != null )     //  MonoBehaviour.print(typeFillter);
			{	if ( typeFillter == type )
				{	GetComponentType_helper.Add( types, 0 );
					return 0;
				}
				
				GetComponentType_helper.Add( types, result + 1 );
				return result + 1;
			}
			
			GetComponentType_helper.Add( types, result );
			return result;
		}
		Dictionary<int, Component[]> Validate_cache = new Dictionary<int, Component[]>();
		
		Component[] Validate( GameObject o, Type fillter )
		{	if ( !o ) return new Component[0];
		
			if ( Validate_cache.ContainsKey( o.GetInstanceID() ) ) return Validate_cache[o.GetInstanceID()];
			
			var comps =  HierarchyExtensions.Utilities.GetComponentFast<Component>.GetAll( o );
			
			Validate_cache.Add( o.GetInstanceID(), comps );
			
			if ( comps.Length == 0 ) return comps;
			
			var tempBool = true;
			bool haveFilter = fillter != null;
			comps = comps.Where( c =>
			{	if ( !c ) return false;
			
				if ( haveFilter && Adapter.GetType_( c ) != fillter ) return false;
				
				var compInd = GetComponentType(c);
				var value = tempBool || compInd != 1;
				
				if ( compInd == 1 )
				{	tempBool = false;
				}
				
				return value && compInd != -1;
			} ).ToArray();
			
			Validate_cache[o.GetInstanceID()] = comps;
			
			if ( comps.Length == 0 ) return comps;
			
			comps = comps.OrderBy( GetComponentType ).ToArray();
			
			
			
			Validate_cache[o.GetInstanceID()] = comps;
			return comps;
			
			/*  if (comps.Length > maxCompCount) maxCompCount = comps.Length;
			  if (maxCompCount != CompNameLeng.Length) Array.Resize(ref CompNameLeng, maxCompCount);
			
			  //   var rr = "max = ";
			  for (int i = 0; i < comps.Length; i++)
			  {
			      var s = GetTypeName(comps[i]);
			      //  rr += comps[i].GetType().ToString() + " " + s.Length + "   - ";
			      if (s.Length > CompNameLeng[i]) CompNameLeng[i] = s.Length;
			  }
			  // MonoBehaviour.print(rr);
			  return comps;*/
		}
		// int maxCompCount = 0;
		//  int[] CompNameLeng = new int[0];
		//  private IOrderedEnumerable<Component> have;
		//  private IEnumerable<Component> readcomps;
		
		
		
		
		/* FillterData.Init(Event.current.mousePosition, SearchHelper, LayerMask.LayerToName(o.layer),
		         Validate(o) ?
		         CallHeaderFiltered(LayerMask.LayerToName(o.layer)) :
		         CallHeader(),
		         this);*/
		/** CALL HEADER */
		internal override _W__SearchWindow.FillterData_Inputs CallHeader()
		{	return null;
		}
		
		internal _W__SearchWindow.FillterData_Inputs CallHeaderFiltered( Type fillter )
		{
		
			Action<_W__SearchWindow.FillterData_Inputs> updateCache = (IN) =>
			{	//  IN.Registrate_FillterData_Inputs(IN);
				Validate_cache.Clear();
				//IN.analizeEnumerator = null;
			};
			
			//  Debug.Log( fillter );
			var result = new _W__SearchWindow.FillterData_Inputs(callFromExternal_objects)
			{	UpdateCache = updateCache,
				    Valudator = o => Validate(o.go, fillter).Length != 0,
				    SelectCompareString = (b, i) =>
				{	// if (!Validate_cache.ContainsKey(b.go.GetInstanceID())) Validate(b.go, fillter);
					if (!Validate_cache.ContainsKey(b.go.GetInstanceID())) return "";
					
					var Components = Validate_cache[b.go.GetInstanceID()];
					
					if (Components.Length == 0) return "";
					
					var res = (Math.Min(999, Components.Length) / 1000f).ToString();
					
					for (int j = 0; j < Components.Length; j++)
					{	var temp = Adapter.GetTypeName(Components[j]);
						// MonoBehaviour.print(CompNameLeng[j] + " " + temp.Length + " " + b.Components[j].GetType().ToString());
						// var add = new string(Enumerable.Repeat(' ', CompNameLeng[j] - temp.Length).ToArray());
						res += temp /*+ add*/;
					}
					
					return res;
				},
				SelectCompareCostInt = (s, i) =>
				{	//  if (!Validate_cache.ContainsKey(s.go.GetInstanceID())) Validate(s.go, fillter);
					if (!Validate_cache.ContainsKey(s.go.GetInstanceID()))  return 0;
					
					var Components = Validate_cache[s.go.GetInstanceID()];
					
					if (Components.Length == 0) return 0;
					
					var cost = i;
					var compType = GetComponentType(Components[0]);
					var A1 = GetEnable(Components[0]) ? 0 : 500000;
					var A2 = 1000000;
					var A3 = s.go.activeInHierarchy ? 0 : 100000000;
					cost += A1 + compType * A2 + A3;
					return cost;
				}
			};
			return result;
		}
		/** CALL HEADER */
		
		
		
		
		
		
		/*   internal override bool CallHeader(out GameObject[] obs, out int[] contentCost)
		   {
		       CallHeared(Utilities.AllSceneObjects(), out obs, out contentCost, null);
		       return true;
		   }*/
		/*
		            void CallHeared(GameObject[] sourceObjects, out GameObject[] obs, out int[] contentCost, Type fillter)
		            {
		                maxCompCount = 0;
		                CompNameLeng = new int[0];
		
		                var validate = sourceObjects.Select((o) => new { o, Components = Validate(o, fillter) }).Where(v => v.Components.Length != 0).ToArray();
		                if (validate.Length == 0)
		                {
		                    obs = new GameObject[0];
		                    contentCost = new int[0];
		                    return;
		                }
		
		                obs = validate.Select(v => v.o).ToArray();
		
		                /*    obs = new GameObject[0];
		                       contentCost = new int[0];
		                   return;#1#
		
		                var sort = from gameObject in validate.Select((b, i) => {
		
		                    var res = "";
		                    //  var res = new string[com.Length];
		                    for (int j = 0; j < b.Components.Length; j++)
		                    {
		                        var temp = GetTypeName(b.Components[j]);
		                        // MonoBehaviour.print(CompNameLeng[j] + " " + temp.Length + " " + b.Components[j].GetType().ToString());
		                        var add = new string(Enumerable.Repeat(' ', CompNameLeng[j] - temp.Length).ToArray());
		                        res += temp + add;
		                    }
		                    return new { startIndex = i, obj = b, name = res, first = b.Components.First() };
		                })
		                           orderby gameObject.name
		                           select gameObject;
		
		
		                contentCost = sort.Select(s => {
		                    var cost = s.startIndex;
		                    var compType = GetComponentType(s.first);
		                    var A1 = GetEnable(s.first) ? 0 : 500000;
		                    var A2 = 1000000;
		                    var A3 = s.obj.o.activeInHierarchy ? 0 : 100000000;
		                    cost += A1 + compType * A2 + A3;
		                    return new { s, cost };
		                }).OrderBy(s => s.s.startIndex).Select(s => s.cost).ToArray();
		            }*/
		
		
		Dictionary<Type, bool> have_enable_helper = new Dictionary<Type, bool>();
		Dictionary<Type, PropertyInfo> have_enable_field = new Dictionary<Type, PropertyInfo>();
		bool HaveEnable( Component comp )
		{	if ( !have_enable_helper.ContainsKey( comp.GetType() ) )
			{	have_enable_helper.Add( comp.GetType(), true );
				var pr = comp.GetType().GetProperty("enabled", BindingFlags.GetField | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
				
				if ( pr == null || !pr.CanWrite || pr.PropertyType != typeof( bool ) ) have_enable_helper[comp.GetType()] = false;
				
				have_enable_field.Add( comp.GetType(), pr );
				// else have_enable_helper[comp.GetInstanceID()] = true;
			}
			
			return have_enable_helper[comp.GetType()];
		}
		bool GetEnable( Component comp )
		{	/* var pr = comp.GetType().GetProperty("enabled", BindingFlags.GetField | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
			 if (pr == null || !pr.CanWrite || pr.PropertyType != typeof(bool)) return false;*/
			if ( !comp || !HaveEnable( comp ) ) return false;
			
			return (bool)have_enable_field[comp.GetType()].GetValue( comp, null );
		}
		void SetEnable( Component comp, bool value )
		{	if ( !HaveEnable( comp ) ) return;
		
			/*   var pr = comp.GetType().GetProperty("enabled", BindingFlags.GetField | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
			   if (pr == null || !pr.CanWrite || pr.PropertyType != typeof(bool)) return;*/
			have_enable_field[comp.GetType()].SetValue( comp, value, null );
		}
		
		bool DrawDisable( Component comp )
		{	if ( !HaveEnable( comp ) ) return false;
		
			return !GetEnable( comp );
		}
		
		
		//   Rect GetClipRect()
		void _S_(GameObject o, Component target, bool value )
		{
		
		
			ResetStack();
			
			if ( adapter.SELECTED_GAMEOBJECTS().All( selO => selO.go != o ) )
			{	Undo.RecordObject( target, "Enable/Disable Component" );
				SetEnable( target, value );
				Adapter.SetDirty( target );
				adapter.MarkSceneDirty( target.gameObject.scene );
#if !DISABLE_PING
				
				if (Hierarchy.par.ENABLE_PING_Fix) adapter.TRY_PING_OBJECT(o);
				
#endif
			}
			
			else
			{	var index = o.GetComponents(target.GetType()).ToList().IndexOf(target);
			
				if ( index == -1 ) return;
				
				foreach ( var objectToUndo in adapter.SELECTED_GAMEOBJECTS() )
				{	var c = objectToUndo.go.GetComponents(target.GetType());
				
					if ( index >= c.Length ) continue;
					
					var variable = c[index];
					// foreach (var variable in c)
					{	Undo.RecordObject( variable, "Enable/Disable Component" );
						SetEnable( variable, value );
						Adapter.SetDirty( variable );
						adapter.MarkSceneDirty( variable.gameObject.scene );
					}
					
					//  if (Hierarchy.par.ENABLE_PING_Fix) adapter.TRY_PING_OBJECT(objectToUndo);
				}
			}
		}
		
		//GUIContent context = new GUIContent();
		//string MenuText;
		//Type callbackType;
		//	bool allowHide;
		/* string GUID = null,*/
		/*  Dictionary<int, Component[] >_drawComps = new Dictionary<int, Component[]>();
		  Component[] get_drawComps(int id )
		  {   return _drawComps[id];
		  }
		  void set_drawComps( int id, Component[] value)
		  {   if ( !_drawComps.ContainsKey( id ) ) _drawComps.Add( id, new Component[1] );
		
		      if ( _drawComps[id].Length != value.Length )
		      {   var r = _drawComps[id];
		          Array.Resize( ref r, value.Length );
		          _drawComps[id] = r;
		      }
		      if ( value.Length == 1 ) _drawComps[id][0] = value[0];
		      else
		      {   var r = _drawComps[id];
		          for ( int i = 0 ; i < value.Length ; i++ )
		          {   r[i] = value[i];
		          }
		          _drawComps[id] = r;
		      }
		  }*/
		//  Component[] allComps;
		Color color;
		bool drawName;
		Color monoColor = new Color32(25, 128, 175, 255);
		
		GUIStyle __labelStyle;
		GUIStyle labelStyle
		{	get
			{	if ( __labelStyle  == null)
				{	__labelStyle  = new GUIStyle() { fontSize = 8, alignment = TextAnchor.LowerCenter/*, clipping = TextClipping.Clip*/ };
					__labelStyle.normal.textColor = monoColor;
					__labelStyle.fontStyle = FontStyle.Bold;
					__labelStyle.richText = true;
				}
				
				return __labelStyle;
			}
		}
		GUIStyle __labelStyleWhite;
		GUIStyle labelStyleWhite
		{	get
			{	if ( __labelStyleWhite == null )
				{	__labelStyleWhite = new GUIStyle() { fontSize = 8, alignment = TextAnchor.LowerCenter/*, clipping = TextClipping.Clip*/ };
					__labelStyleWhite.normal.textColor = new Color(1, 1, 1, 0.5f);
					__labelStyleWhite.fontStyle = FontStyle.Bold;
					__labelStyleWhite.richText = true;
				}
				
				return __labelStyleWhite;
			}
		}
		
		Rect? stateForDrag_B0;
		GameObject stateForDrag_B1;
		Component[] stateForDrag_B2;
		
		
		Component[] RawOnUpDragComponents_Array;
		HideFlags RawOnUpDragComponents_Flags = HideFlags.DontSaveInEditor | HideFlags.HideInHierarchy | HideFlags.HideInInspector;
		void RawOnUpDragComponents()
		{	if ( RawOnUpDragComponents_Array != null   )
		
			{	bool remove = true;
			
				if ( RawOnUpDragComponents_Array.Length == DragAndDrop.objectReferences.Length)
				{	var drawlist = DragAndDrop.objectReferences.Select(c => (c ? c.GetInstanceID() : -1) ).ToList();
					bool all = true;
					
					foreach ( var item in RawOnUpDragComponents_Array )
					{	if ( !item ) continue;
					
						if (drawlist.Contains(item.GetInstanceID())) continue;
						
						all = false;
						break;
					}
					
					if ( all ) remove = false;
				}
				
				if ( remove/* || DragAndDrop.GetGenericData("MoveComp") != RawOnUpDragComponents_Array*/ )
					{	RawOnRemoveRaw();
						RawOnUpDragComponents_Array = null;
					}
			}
		}
		void RawOnRemoveRaw()
		{	if ( RawOnUpDragComponents_Array == null ) return;
		
			foreach ( var item in RawOnUpDragComponents_Array )
			{	if ( item && item.gameObject )
				{	if ( (item.gameObject.hideFlags & RawOnUpDragComponents_Flags) != 0 )
					{	if ( Application.isPlaying ) GameObject.Destroy( item.gameObject );
						else GameObject.DestroyImmediate( item.gameObject );
					}
				}
			}
			
			
		}
		void RawOnUP()
		{	stateForDrag_B0 = null;
			stateForDrag_B1 = null;
		}
		
		
		static int SHORT = 3;
		static _W__SearchWindow lastFocusRoot;
		private Hierarchy_GUI.CustomIconParams load;
	}
}
}

