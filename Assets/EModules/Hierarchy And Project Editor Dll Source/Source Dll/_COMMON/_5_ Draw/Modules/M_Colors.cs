#if UNITY_EDITOR
	#define HIERARCHY
#endif

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

//namespace EModules
#if PROJECT
	using EModules.Project;
#endif
namespace EModules.EModulesInternal

{




internal partial class Adapter {



	internal partial class M_Colors : Adapter.Module {
		//TempColorClass needdrawGetColor(Adapter.HierarchyObject activeGameObject) { return needdrawGetColor( activeGameObject ); }
		// Color GetColorForObject(Adapter.HierarchyObject activeGameObject) { return GetColorForObject( activeGameObject ); }
		//  Texture GetImageForObject_OnlyCacher(Adapter.HierarchyObject activeGameObject) { return GetImageForObject_OnlyCacher( activeGameObject ); }
		
		
		/*  public void IconColorCacherSetValue(SingleList list, int scene, Adapter.HierarchyObject o, bool b)
		  {   IconColorCacher.SetValue( list, scene, SetPair( o ), b );
		  }
		
		  public string IconColorCacherGetValueToString(int scene, Adapter.HierarchyObject o)
		  {   return IconColorCacher.GetValueToString( scene, o );
		  }
		  */
		
		// void SetValue(TempColorClass c, int scene, Adapter.HierarchyObject needRestoreGameObjectName) { SetValue( c, scene, needRestoreGameObjectName ); }
		//  string GetValueToString(int scene, Adapter.HierarchyObject needRestoreGameObjectName) { return GetValueToString( scene, needRestoreGameObjectName ); }
		//  void DrawBackground(EditorWindow w, Rect selectionRect, Adapter.HierarchyObject o) { DrawBackground( w, selectionRect, o ); }
		//  void DrawBackground(EditorWindow w, Rect selectionRect, Adapter.HierarchyObject o, int labelOverride) {  DrawBackground(w, selectionRect, o, labelOverride ); }
		// Color32? GetLabelColor( Adapter.HierarchyObject o) { return GetLabelColor( o ); }
		
		
		/*internal override bool SKIP()
		{	return false;
		}*/
		
		internal ObjectCacheHelper<GoGuidPair, SingleList> IconColorCacher;
		internal ObjectCacheHelper<GoGuidPair, Int32List> IconImageCacher;
		
		
		DoubleList<GoGuidPair, SingleList> dl = new DoubleList<GoGuidPair, SingleList>();
		
		public DoubleList<GoGuidPair, SingleList> getDoubleList(int scene)
		{	dl.listKeys = adapter.MOI.des(scene).GetHash5_Fix2_0();
			dl.listValues = adapter.MOI.des(scene).GetHash6();
			
			// var res = new DoubleList<GameObject, string> { listKeys = des.listKeys, listValues = des.listValues };
			return dl;
		}
		
		HighlighterResult HighlighterResultOut = new HighlighterResult() {PTR = -1};
		HighlighterResult HighlighterResultFalse = new HighlighterResult() {PTR = -1};
		
		internal struct HighlighterResult
		{	internal int BroadCast;
			internal int PTR;
			
			internal bool IsTrue(bool CheckBroadCast)
			{	if (CheckBroadCast) return BroadCast != -1 && PTR != -1;
				else return PTR != -1;
			}
		}
		
		Dictionary<int, Dictionary<Adapter.HierarchyObject, HighlighterResult>> cacheDichighlighter =
		    new Dictionary<int, Dictionary<Adapter.HierarchyObject, HighlighterResult>>();
		    
		//  internal  bool anyNeedBroadcast = false;
		internal HighlighterResult HighlighterHasKey(int scene, Adapter.HierarchyObject _o)
		{	if (!_o.Validate() || !adapter.par.USE_HIGLIGHT) return HighlighterResultFalse;
		
			if (!cacheDichighlighter.ContainsKey(scene)) //var anyNeedBroadcast = false;
			{	var h = adapter.MOI.des(scene).GetHash5_Fix2_0();
				var h2 = adapter.MOI.des(scene).GetHash6();
				// Debug.Log( h2.Count );
				
				
				cacheDichighlighter.Add(scene, new Dictionary<Adapter.HierarchyObject, HighlighterResult>());
				
				for (int i = 0; i < Math.Min(h.Count, h2.Count); i++)
				{	var r = adapter.GetHierarchyObjectByPair(ref h, i);
				
					// Debug.Log( r.fileID + "  " + r.name );
					if (r == null) continue;
					
					if (cacheDichighlighter[scene].ContainsKey(r))
					{	adapter.MOI.des(scene).GetHash5_Fix2_0().RemoveAt(i);
						adapter.MOI.des(scene).GetHash6().RemoveAt(i);
						i--;
						/*if (i < 0)*/
						continue;
					}
					
					else
					{	cacheDichighlighter[scene].Add(r,
						                               new HighlighterResult()
						{PTR = i, BroadCast = h2[i].list.Count > 4 && h2[i].list[4] == 1 ? _o.id : -1});
						//  if (h2[i].list.Count > 4 && h2[i].list[4] == 1) anyNeedBroadcast = true;
					}
				}
			}
			
			
			if (cacheDichighlighter[scene].TryGetValue(_o, out HighlighterResultOut)) return HighlighterResultOut;
			
			/* if ( cacheDichighlighter[scene].Count > 0 ) {
			     if ( _o.name == "Styles" ) Debug.Log( cacheDichighlighter[scene].Count );
			     if ( _o.name == "Styles" ) Debug.Log( _o.fileID );
			     if ( _o.name == "Styles" ) Debug.Log( cacheDichighlighter[scene].First().Key.fileID );
			
			 }*/
			return HighlighterResultFalse;
		}
		
		internal HighlighterResult HighlighterHasKey_IncludeFilters(int scene, Adapter.HierarchyObject _o)
		{	if (!_o.Validate() || !adapter.par.USE_HIGLIGHT) return HighlighterResultFalse;
		
			if (!cacheDichighlighter.ContainsKey(scene)) // anyNeedBroadcast = false;
			{	var h = adapter.MOI.des(scene).GetHash5_Fix2_0();
				var h2 = adapter.MOI.des(scene).GetHash6();
				cacheDichighlighter.Add(scene, new Dictionary<Adapter.HierarchyObject, HighlighterResult>());
				
				for (int i = 0; i < Math.Min(h.Count, h2.Count); i++)
				{	var r = adapter.GetHierarchyObjectByPair(ref h, i);
				
					//if ( adapter.pluginID == 0 ) Debug.Log( r.fileID + " " + r.name );
					if (r == null) continue;
					
					if (cacheDichighlighter[scene].ContainsKey(r))
					{	adapter.MOI.des(scene).GetHash5_Fix2_0().RemoveAt(i);
						adapter.MOI.des(scene).GetHash6().RemoveAt(i);
						i--;
						
						/*if (i < 0)*/
						continue;
					}
					
					else
					{	cacheDichighlighter[scene].Add(r,
						                               new HighlighterResult()
						{PTR = i, BroadCast = h2[i].list.Count > 4 && h2[i].list[4] == 1 ? _o.id : -1});
					}
					
					//if (h2[i].list.Count > 4 && h2[i].list[4] == 1) anyNeedBroadcast = true;
				}
			}
			
			
			if (cacheDichighlighter[scene].TryGetValue(_o, out HighlighterResultOut)) return HighlighterResultOut;
			
			
			var filtered = this.GetFilter(adapter, _o);
			
			if (filtered != null && (filtered.HAS_BG_COLOR || filtered.HAS_LABEL_COLOR))
			{	HighlighterResultOut.PTR = -2;
				HighlighterResultOut.BroadCast = filtered.child ? _o.id : -1;
				return HighlighterResultOut;
			}
			
			
			return HighlighterResultFalse;
		}
		
		
		
		bool loop = false;
		
		internal SingleList GetHighlighterValue(int scene, Adapter.HierarchyObject o)
		{	if (!HighlighterHasKey(scene, o).IsTrue(false) || !adapter.par.USE_HIGLIGHT) return null;
		
			var d = adapter.MOI.des(scene);
			
			if (d == null) return null;
			
			var ptr = cacheDichighlighter[scene][o].PTR;
			
			if (ptr == -1) return null;
			
			if (ptr >= d.GetHash6().Count)
			{	if (loop)
				{	adapter.logProxy.LogWarning("GetHighlighterValue Out Of Range please contact support");
				}
				
				loop = true;
				cacheDichighlighter.Clear();
				return GetHighlighterValue(scene, o);
			}
			
			loop = false;
			return d.GetHash6()[ptr];
		}
		
		internal string GetValueToString(int scene, Adapter.HierarchyObject o)
		{	var r = GetHighlighterValue(scene, o);
		
			if (r == null) return null;
			
			return r.ToString();
		}
		
		
		
		
		
		
		
		
		internal Color ConverterSingle(SingleList source)
		{	Color color;
			color.r = source.list[0];
			color.g = source.list[1];
			color.b = source.list[2];
			color.a = source.list[3];
			return color;
		}
		
		
		
		internal GoGuidPair tempPair = GoGuidPairConstructor_WithoutCache();
		
		internal GoGuidPair SetPair(Adapter.HierarchyObject _o)
		{	tempPair.go = _o.go;
#pragma warning disable
			tempPair.guid = adapter.IS_HIERARCHY() ? "" : _o.project.guid;
#pragma warning restore
			tempPair.path = adapter.IS_HIERARCHY() ? "" : _o.project.assetPath;
			return tempPair;
		}
		
		internal void ClearCache()
		{	// UndoClear();
			/* addIconChecherCache.Clear();
			 cached_colors.Clear();
			 if ( cacheDichighlighter != null ) cacheDichighlighter.Clear();
			 if ( IconColorCacher != null ) IconColorCacher.cacheDic.Clear();
			
			 Utilities.ObjectContent_cache.Clear();
			 Utilities.ObjectContent_Objectcache.Clear();
			 Utilities.cache_ObjectContent_byType.Clear();
			 Utilities.cache_ObjectContent_byId.Clear();*/
			//if ( adapter.onUndoAction != null ) adapter.onUndoAction();
			//Hierarchy_GUI.Instance(adapter).ClearPrefabs();
			// Debug.Log("ASD");.
			
			adapter.ClearHierarchyObjects(false);
		}
		
		internal void ClearCacheFull()
		{	adapter.ClearHierarchyObjects(true);
		}
		
		void ClearGroupingCache()
		{	foreach (var nc in adapter.new_child_cache_dic)
			{	/* if ( nc.Value.lastRect != nc.Value.__rect )
				             {   nc.Value.lastRect = nc.Value.__rect;
				              needRepaint = true;
				             }*/
				nc.Value.wasInit = false;
				nc.Value.wasLastAssign = false;
			}
			
			//k  Debug.Log( "ASD" );
		}
		
		internal static Action additionalClear = null;
		
		internal void ClearCacheAdditional()
		{	addIconChecherCache.Clear();
			cached_colors.Clear();
			ClearGroupingCache();
			adapter.RESET_COLOR_STACKS();
			
			new_perfomance_onlycaher_icons.Clear();
			Utilities.new_perfomance_includecaher_icons.Clear();
			
			if (cacheDichighlighter != null) cacheDichighlighter.Clear();
			
			if (IconColorCacher != null) IconColorCacher.cacheDic.Clear();
			
			if (additionalClear != null) additionalClear();
			
			Hierarchy.ClearM_CustomIconsCache();
		}
		
		struct TestStr
		{	int g;
		
			public int GET
			{	get { return g; }
			
				set { g = value; }
			}
		}
		
		internal void SetHighlighterValue(TempColorClass c, int s, Adapter.HierarchyObject _o,
		                                  bool SaveRegistrator = true) // var o = _o.go;
		{	if (!_o.Validate()) return;
		
			var d = adapter.MOI.des(s);
			
			if (d == null) return;
			
			
			// Debug.Log(_o.GetHashCode() + " " + (c == null) + " " + c.HAS_BG_COLOR + " " + c.HAS_LABEL_COLOR);
			//  Debug.Log(adapter.MOI.des(s).GetHash6().Count + " " + getDoubleList(s).listValues.Count);
			var list = getDoubleList(s);
			
			/*  Debug.Log( "--------" );
			    Debug.Log( _o.fileID );
			    var pp = SetPair( _o ) ;
			    Debug.Log( pp.fileID );
			    Debug.Log( list.ContainsKey( pp ) );
			    Debug.Log( adapter.MOI.des( s ).GetHash6().Count );*/
			
			if (c == null || (!c.HAS_BG_COLOR && !c.HAS_LABEL_COLOR))
			{	list.RemoveAll(SetPair(_o));
			}
			
			else // var color1 = c[0];
			{	//var color2 = c.Length > 1 ? c[1] : (Color32)Adapter.TRANSP_COLOR;
				// var value = new SingleList() { list = new List<int>() { color1.r, color1.g, color1.b, color1.a, applyToChildrens ? 1 : 0, color2.r, color2.g, color2.b, color2.a } };
				var value = new SingleList() {list = c.ToList()};
				
				
				var p = SetPair(_o);
				
				if (list.ContainsKey(p))
				{	list[p] = value;
				}
				
				else list.Add(p, value);
			}
			
			/*   Debug.Log( adapter.MOI.des( s ).GetHash6().Count );
			     Debug.Log( list.ContainsKey( pp ) );
			     Debug.Log( "..." );
			     Debug.Log( _o.BACKGROUNDED );*/
			
			
			if (SaveRegistrator) adapter.DescriptionModule.TrySaveHiglighterRegistrator(_o, c);
			
			if (!Application.isPlaying)
			{	/*  Hierarchy.SetDirty(d.component);
				                   Hierarchy.SetDirty(d.gameObject);*/
				adapter.SetDirtyDescription(d,
				                            d.gameObject ? d.gameObject.scene.GetHashCode() : adapter.GET_ACTIVE_SCENE);
				adapter.MarkSceneDirty(d.gameObject ? d.gameObject.scene.GetHashCode() : adapter.GET_ACTIVE_SCENE);
			}
			
			//  Debug.Log(adapter.MOI.des(s).GetHash6().Count + " " + getDoubleList(s).listValues.Count);
			ClearCache();
			
			// cacheDichighlighter.Clear();
			/* Debug.Log( "--------" );
			 Debug.Log( HighlighterHasKey( s , _o ).IsTrue( false ) );
			
			
			 Debug.Log( "--------" );*/
			
			
			adapter.RepaintWindowInUpdate();
		}
		
		
		
		
		
		
		
		/*
		internal TempColorClass MCOLOR_NEEDGETCOLOR(Adapter.HierarchyObject o)
		{
		
		    if (!adapter.ENABLE_LEFTDOCK_PROPERTY || adapter.DISABLE_DES()) return null;
		
		    if (HasKey( o.scene, o ))
		    {   return DrawColoredBG( null, o, o, false );
		    }
		    else if (anyNeedBroadcast)
		    {   var parent = o.parent(adapter);
		        while (parent != null)
		        {   if (HasKey( o.scene, parent ))
		            {   return
		                    DrawColoredBG( null, parent, o, false );
		            }
		            parent = parent.parent( adapter );
		        }
		    }
		    return null;
		}
		
		*/
		
		
		
		
		
		public M_Colors(int restWidth, int sib, bool enable, Adapter adapter) : base(restWidth, sib, enable, adapter)
		{	// adapter.OnClearObjects += ClearCache;
		
			adapter.ColorModule = this;
			
			IconColorCacher = new ObjectCacheHelper<GoGuidPair, SingleList>(
			    property => property.GetHash7_Fix2_0(), property => property.GetHash8(), Adapter.CacherType.IconColor,
			    adapter, "M_ColorsIcons");
			    
			IconImageCacher = new ObjectCacheHelper<GoGuidPair, Int32List>(
			    property => property.GetHash_IconImageKey(), property => property.GetHash_IconImageValue(),
			    Adapter.CacherType.ImageIcon, adapter, "M_ImageIcons");
			/*   var c = Assembly.GetAssembly(typeof(EditorWindow)).GetType("UnityEditor.AssetStoreContext");
			   var url = c.GetField("initialOpenURL", (BindingFlags)(-1));
			   var inst = c.GetField("s_Instance", (BindingFlags)(-1));
			   var OpenPackage = c.GetMethod("OpenPackageInternal", (BindingFlags)(-1));
			   bool opened = false;
			   EditorApplication.update += () => {
			       if ((inst.GetValue(null)) != null)
			       {
			           if (opened) return;
			           //AssetStore.Open("/assets/list");
			        //
			          // AssetStore.Open("https://www.assetstore.unity3d.com/#!/search/page=1/sortby=relevance/query=hierarchy&pro");
			           //AssetStore.Open("query=hierarchy&pro");
			           opened = true;
			           /*
			             var reult = OpenPackage.Invoke(inst.GetValue(null), new[] { "89542" });
			             MonoBehaviour.print(reult);#1#
			
			           /*var val = url.);
			           MonoBehaviour.print(val);#1#
			       }
			   };*/
			// AssetStore.Open("/content/89542");
			/*     var www = Resources.FindObjectsOfTypeAll<EditorWindow>().FirstOrDefault(w => w.GetType().Name.EndsWith("AssetStoreWindow"));
			  www.GetType()
			     if (www == null) EditorApplication.ExecuteMenuItem("Window/Asset Store");
			     AssetStore.Open("/content/89542");*/
			// UnityEditorInternal.AssetStore.
			/*
			   www.Focus();
			
			   www.GetType().GetMethod("OpenURL", (BindingFlags)(-1))
			       .Invoke(www, new[] { "hier" });*/
			
			/*
			www.GetType().GetMethod("InvokeJSMethod", (BindingFlags)(-1))
			    .Invoke(www, new[] { (object)"document.AssetStore", (object)"openURL", new[] { "hier" } });*/
			// .Invoke(www, new[] { "document.AssetStore.pkgs", "OnDownloadProgress", (object)new object[] { } });
			//.Invoke(EditorWindow.focusedWindow, new[] { "document.AssetStore.pkgs", "OnDownloadProgress" });
		}
		
		
		/*private  List<GameObject> findBrokenObjectsInScene()
		    {
		
		        // Find all of the GameObjects in the scene and sort them
		        // by the "path" in the scene hierarchy
		        var brokenObjects = Resources
		            .FindObjectsOfTypeAll( typeof( GameObject ) )
		            .Cast<GameObject>()
		            .Where( x => x.activeInHierarchy && x.GetComponents<Component>().Any( c => c == null ) )
		            .OrderBy( x => getObjectPath( x ) )
		            .ToList();
		
		        return brokenObjects;
		
		    }*/
#pragma warning disable
		GUIContent content = new GUIContent() {tooltip = "HighLighter"};
		GUIContent contentNull = new GUIContent() {tooltip = "Empty Transform"};
		GUIContent contentMis = new GUIContent() {tooltip = "This Object Has Missing MonoScript"};
#pragma warning restore
		
		Color prefabColorPro = new Color32(76, 128, 217, 255);
		Color prefabColorPersonal = new Color32(0, 39, 131, 255);
		
		Color prefabMissinColorPro = new Color32(164, 94, 94, 255);
		Color prefabMissinColorPersonal = new Color32(63, 13, 13, 255);
		
		GUIContent switchedConten = new GUIContent();
		//  Color contentColor = new Color32(255, 255, 255, 90);
		
		//TempColorClass _tempColor = new TempColorClass();
		// Color32[] tempColor = new Color32[2];
		//  PrefabType prefabMask = PrefabType.DisconnectedModelPrefabInstance ||;
		//  [MethodImpl(MethodImplOptions.InternalCall)]
		
		/*Color32 glc;
		internal Color32? GetLabelColor(Adapter.HierarchyObject o)
		{   var parent = o;
		    while (parent != null)
		    {   if (HighlighterHasKey_IncludeFilters( o.scene, parent ))
		        {
		
		            var ptr = cacheDichighlighter[o.scene][parent];
		            var d = adapter.MOI.des(o.scene);
		            var list = d.GetHash6()[ptr].list;
		            if (list.Count >= 9 && (list[5] != 0 || list[6] != 0 || list[7] != 0 || list[8] != 0))
		            {   glc.r = (byte)list[5];
		                glc.g = (byte)list[6];
		                glc.b = (byte)list[7];
		                //glc.a = (byte)(Math.Max( 10, list[8] ));
		                glc.a = (byte)list[8];
		
		                return glc;
		            }
		        }
		        parent = parent.parent( adapter );
		    }
		
		
		    return null;
		
		}*/
		
		
		
		
		bool TryToFindInternalIcon(Adapter.HierarchyObject o)
		{	var iac = EditorGUIUtility.ObjectContent(o.go, adapter.t_GameObject).image;
		
			if (iac && (iac.name.StartsWith("sv_icon_dot") || iac.name.StartsWith("sv_label_")))
			{	if (AssetDatabase.GetAssetPath(iac).StartsWith("Library/"))
				{	// var guid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath( iac ));
					SetImageForObject_OnlyCacher(o, "Library/" + iac.name);
					return true;
				}
			}
			
			return false;
		}
		
		Dictionary<GameObject, int> addIconChecherCache = new Dictionary<GameObject, int>();
		internal Dictionary<string, Texture> cached_colors = new Dictionary<string, Texture>();
		
		TempColorClass __GetImageForObject_OnlyCacherTempColor_Empty = new TempColorClass().AddIcon(null);
		
		// TempColorClass __GetImageForObject_OnlyCacherTempColor = new TempColorClass();
		internal TempColorClass GetImageForObject_OnlyCacher(Adapter.HierarchyObject o)
		{	return GetImageForObject_OnlyCacher(o, false);
		}
		
		internal TempColorClass GetImageForObject_OnlyCacher(Adapter.HierarchyObject o, bool skipFilter)
		{	if (adapter.NEW_PERFOMANCE && new_perfomance_onlycaher_icons.ContainsKey(o.id))
				return new_perfomance_onlycaher_icons[o.id];
				
				
			if (IconImageCacher == null || !IconImageCacher.HasKey(o.scene, o))
			{	if (IconImageCacher == null)
					IconImageCacher = new ObjectCacheHelper<GoGuidPair, Int32List>(
					    property => property.GetHash_IconImageKey(), property => property.GetHash_IconImageValue(),
					    Adapter.CacherType.ImageIcon, adapter, "M_ImageIcons");
					    
				bool tryToApplyilter = true;
				
				if (IconImageCacher != null && adapter.pluginID == 0 && adapter.HAS_LABEL_ICON())
				{	if (o.go && !addIconChecherCache.ContainsKey(o.go))
					{	addIconChecherCache.Add(o.go, 0);
						tryToApplyilter = !TryToFindInternalIcon(o);
					}
				}
				
				
				if (tryToApplyilter)
				{	if (!skipFilter)
					{	if (IconImageCacher != null) //#COLUP
						{	var filtered = this.GetFilter(adapter, o);
						
							if (filtered != null)
							{	if (adapter.NEW_PERFOMANCE)
								{	var result = new TempColorClass();
									filtered.OverrideTo(ref result);
									new_perfomance_onlycaher_icons.Add(o.id, result);
								}
								
								return filtered;
							}
						}
					}
					
					if (adapter.NEW_PERFOMANCE)
					{	new_perfomance_onlycaher_icons.Add(o.id, __GetImageForObject_OnlyCacherTempColor_Empty);
					}
					
					return __GetImageForObject_OnlyCacherTempColor_Empty;
				}
			}
			
			
			var value = IconImageCacher != null && IconImageCacher.HasKey(o.scene, o)
			            ? IconImageCacher.GetValue(o.scene, o)
			            : null;
			            
			            
			if (value != null && !cached_colors.ContainsKey(value.GUIDsActiveGameObject_CheckAndGet))
			{
#pragma warning disable
				bool _else = true;
				
				if (value.GUIDsActiveGameObject.StartsWith("Library/"))
				{	var resource = value.GUIDsActiveGameObject.Substring(value.GUIDsActiveGameObject.IndexOf('/') + 1);
				
					if ((resource.StartsWith("sv_icon_dot") || resource.StartsWith("sv_label_")))
					{	var t = EditorGUIUtility.IconContent(resource).image;
						cached_colors.Add(value.GUIDsActiveGameObject_CheckAndGet, t);
						_else = false;
					}
					
#pragma warning restore
				}
				
				if (_else)
				{	var path = AssetDatabase.GUIDToAssetPath(value.GUIDsActiveGameObject_CheckAndGet);
				
					if (string.IsNullOrEmpty(path))
					{	/* if ( texture.name.StartsWith( "sv_icon_dot" ) || texture.name.StartsWith( "sv_label_" ) ) {
						     if ( AssetDatabase.GetAssetPath( texture ).StartsWith( "Library/" ) ) {
						         SetImageForObject_OnlyCacher( o , "Library/" + texture.name );
						     }
						 }*/
						//  if ( TryToFindInternalIcon (o) )
						
						var newGuid = AssetDatabase.AssetPathToGUID(value.PATHsActiveGameObject);
						
						if (string.IsNullOrEmpty(newGuid))
						{	cached_colors.Add(value.GUIDsActiveGameObject_CheckAndGet, null);
							// return __GetImageForObject_OnlyCacherTempColor_Empty;
							goto skip;
						}
						
						value.GUIDsActiveGameObject_CheckAndGet = newGuid;
						adapter.SetDirtyActiveDescription((o.scene));
						path = value.PATHsActiveGameObject;
					}
					
					else
						if (path != value.PATHsActiveGameObject)
						{	value.PATHsActiveGameObject = path;
							adapter.SetDirtyActiveDescription((o.scene));
						}
						
					var t = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
					cached_colors.Add(value.GUIDsActiveGameObject_CheckAndGet, t);
				}
				
				
skip: ;
			}
			
			Color32? ic = null;
			var icon = value != null ? cached_colors[value.GUIDsActiveGameObject_CheckAndGet] : null;
			
			if (!adapter.DISABLE_DES() && IconColorCacher != null && IconColorCacher.HasKey(o.scene, o))
			{	var color = IconColorCacher.GetValue(o.scene, o);
				var hasIconColor = color.list.Count > 3 &&
				                   (color.list[0] > 0 || color.list[1] > 0 || color.list[2] > 0 || color.list[3] > 0);
				                   
				if (hasIconColor)
					ic = new Color32((byte) color.list[0], (byte) color.list[1], (byte) color.list[2],
					                 (byte) color.list[3]);
					                 
				// return __GetImageForObject_OnlyCacherTempColor.AddIcon(icon, hasIconColor, color);
			}
			
			else // return __GetImageForObject_OnlyCacherTempColor.AddIcon(icon);
			{ }
			
			//IconColorCacher
			var __GetImageForObject_OnlyCacherTempColor = new TempColorClass();
			__GetImageForObject_OnlyCacherTempColor.add_icon = icon;
			__GetImageForObject_OnlyCacherTempColor.add_hasiconcolor = ic.HasValue;
			
			if (ic.HasValue) __GetImageForObject_OnlyCacherTempColor.add_iconcolor = ic.Value;
			
			if (adapter.NEW_PERFOMANCE)
				new_perfomance_onlycaher_icons.Add(o.id, __GetImageForObject_OnlyCacherTempColor);
				
				
			if (adapter.NEW_PERFOMANCE && adapter.pluginID == 0)
			{	if (EditorGUIUtility.ObjectContent(o.go, adapter.t_GameObject).image !=
				        __GetImageForObject_OnlyCacherTempColor.add_icon)
				{	if (TryToFindInternalIcon(o))
					{	if (cached_colors.ContainsKey(value.GUIDsActiveGameObject_CheckAndGet))
							cached_colors[value.GUIDsActiveGameObject_CheckAndGet] = EditorGUIUtility.ObjectContent(
							            o.go,
							            adapter.t_GameObject).image;
							            
						__GetImageForObject_OnlyCacherTempColor.add_icon =
						    EditorGUIUtility.ObjectContent(o.go, adapter.t_GameObject).image;
					}
				}
			}
			
			
			return __GetImageForObject_OnlyCacherTempColor;
		} //GetImageForObject_OnlyCacher
		
		Dictionary<int, TempColorClass> new_perfomance_onlycaher_icons = new Dictionary<int, TempColorClass>();
		
		internal void SetImageForObject_OnlyCacher(Adapter.HierarchyObject o, string guid)
		{	adapter.CreateUndoActiveDescription("Change Icon", (o.scene));
		
			if (string.IsNullOrEmpty(guid)) IconImageCacher.SetValue(null, o.scene, SetPair(o), true);
			else
				IconImageCacher.SetValue(
				    new Int32List()
			{	GUIDsActiveGameObject_CheckAndGet = guid,
				    PATHsActiveGameObject = AssetDatabase.GUIDToAssetPath(guid)
			}, o.scene, SetPair(o), true);
			adapter.SetDirtyActiveDescription((o.scene));
		} //GetImageForObject_OnlyCacher
		
		internal void SetImageForObject_OnlyCacher_2(Adapter.HierarchyObject o, Texture2D t)
		{	adapter.CreateUndoActiveDescription("Change Icon", (o.scene));
		
			if (!t) IconImageCacher.SetValue(null, o.scene, SetPair(o), true);
			else
			{	var p = AssetDatabase.GetAssetPath(t);
				var guid = AssetDatabase.AssetPathToGUID(p);
				IconImageCacher.SetValue(
				new Int32List() {GUIDsActiveGameObject_CheckAndGet = guid, PATHsActiveGameObject = p}, o.scene,
				SetPair(o), true);
			}
			
			adapter.SetDirtyActiveDescription((o.scene));
		} //GetImageForObject_OnlyCacher
		
		Dictionary<int, bool> _CHeckIcon_cache = new Dictionary<int, bool>();
		
		internal void CHeckIcon(HierarchyObject o, Texture texture)
		{	if (adapter.IS_PROJECT()) return;
		
			if (_CHeckIcon_cache.ContainsKey(o.id)) return;
			
			_CHeckIcon_cache.Add(o.id, true);
			
			if (!AssetDatabase.GetAssetPath(texture).StartsWith("Assets")) return;
			
			var p = AssetDatabase.GetAssetPath(texture);
			var guid = AssetDatabase.AssetPathToGUID(p);
			
			if (IconImageCacher.HasKey(o) &&
			        IconImageCacher.GetValue(o.scene, o).GUIDsActiveGameObject_CheckAndGet == guid
			   ) return;
			   
			IconImageCacher.SetValue(
			new Int32List() {GUIDsActiveGameObject_CheckAndGet = guid, PATHsActiveGameObject = p}, o.scene,
			SetPair(o), true);
			// var guid =         "Library/" + texture.name ;
			/*IconImageCacher.SetValue( new Int32List() { GUIDsActiveGameObject_CheckAndGet = guid , PATHsActiveGameObject = AssetDatabase.GUIDToAssetPath( guid ) } ,
			    o.scene , SetPair( o ) , true );*/
			ClearCache();
			
			/* if ( texture.name.StartsWith( "sv_icon_dot" ) || texture.name.StartsWith( "sv_label_" ) ) {
			     if ( _CHeckIcon_cache.ContainsKey( o.id ) ) return;
			     _CHeckIcon_cache.Add( o.id , true );
			     if ( AssetDatabase.GetAssetPath( texture ).StartsWith( "Library/" ) ) {
			         var guid =         "Library/" + texture.name ;
			         IconImageCacher.SetValue( new Int32List() { GUIDsActiveGameObject_CheckAndGet = guid , PATHsActiveGameObject = AssetDatabase.GUIDToAssetPath( guid ) } ,
			             o.scene , SetPair( o ) , true );
			     }
			 }*/
		}
		
		
		
		internal static int ICON_WIDTH = 20;
		
		
		Color? bgCol;
		Rect tt;
		
		Rect tRr;
		// Rect standrardRect = new Rect();
		//Color alpha = new Color(1, 1, 1, 0.8f);
		//    Color backCol = new Color();
		// Color tc = new Color();
		
		
		enum GetIconRectIfNextToLabelType
		{	CustomIcon,
			DefaultIcon
		}
		
		Rect GetIconRectIfNextToLabel(Rect selectionRect, GetIconRectIfNextToLabelType type)
		{	TryToFaeBG_Rect.ref_selectionRect = selectionRect;
		
			TryToFaeBG_Rect.ref_selectionRect.x += adapter.TOTAL_LEFT_PADDING;
			// TryToFaeBG_Rect.HasIcon = adapter._S_bgIconsPlace == 0;
			TryToFaeBG_Rect.adapter = adapter;
			TryToFaeBG_Rect.HasIcon = true;
			TryToFaeBG_Rect.MinLeft = adapter.TOTAL_LEFT_PADDING;
			
			// var size = type == GetIconRectIfNextToLabelType.DefaultIcon ? adapter. DEFAULT_ICON_SIZE : EditorGUIUtility.singleLineHeight;
			var size = adapter.DEFAULT_ICON_SIZE;
			// size = (adapter. DEFAULT_ICON_SIZE - EditorGUIUtility.singleLineHeight) / 2 + EditorGUIUtility.singleLineHeight;
			// if (type == GetIconRectIfNextToLabelType.DefaultIcon)
			{ }
			/* else
			 {
			
			 }*/
			
			tt.Set(
			    TryToFaeBG_Rect.GET_LEFT(BgAligmentLeft.BeginLabel) - size,
			    selectionRect.y,
			    size, selectionRect.height);
			    
			//  tt.x += adapter.raw_old_leftpadding;
			size = Mathf.Min(selectionRect.height, size);
			
			//  if (type == GetIconRectIfNextToLabelType.DefaultIcon) size = EditorGUIUtility.singleLineHeight;
			
			tt.y += (tt.height - size) / 2;
			tt.height = Mathf.RoundToInt(size);
			
			return tt;
		}
		internal int lastIconPlace;
		internal Rect GetIconRect(Rect selectionRect, int? overrideValue = null, int? overrideSBGIconPlace = null)
		{	var icon_place = overrideValue ??
			                 (callFromExternal() ? 2 : (overrideSBGIconPlace ?? adapter._S_bgIconsPlace));
			lastIconPlace = icon_place;
			var icon_rect = Rect.zero;
			
			switch (icon_place)
			{	case 0:
					icon_rect = GetIconRectIfNextToLabel(selectionRect, GetIconRectIfNextToLabelType.CustomIcon);
					
					break;
					
				case 1:
				case 2:
					if (icon_place == 2)
					{	icon_rect.x = adapter.TOTAL_LEFT_PADDING;
					}
					
					else icon_rect.x = selectionRect.x - EditorGUIUtility.singleLineHeight * 2;
					
					icon_rect.y = selectionRect.y;
					icon_rect.height = selectionRect.height;
					//  icon_rect.x += (icon_rect.width - icon_rect.width) / 2 + (adapter.par.COLOR_ICON_SIZE - 12) / 2f;
					icon_rect.width = EditorGUIUtility.singleLineHeight;
					
					icon_rect.y += (icon_rect.height - EditorGUIUtility.singleLineHeight) / 2;
					icon_rect.height = EditorGUIUtility.singleLineHeight;
					
					break;
			}
			
			return icon_rect;
		}
		
		// bool internalIcon = false;
		internal void TryToFadeBG(Rect selectionRect, Adapter.HierarchyObject _o)
		{	_o.internalIcon = false;
		
			if (!_o.Validate() || adapter.MOI.des(_o.scene) == null) return;
			
			if (!adapter.HAS_LABEL_ICON()) return;
			
			_o.drawIcon = GET_CONTENT(_o);
			
			if (adapter._S_bgIconsPlace != 0 && _o.switchType != 1) return;
			
			//  if ( _o.switchType == 1 ) Debug.Log( "ASD" );
			_o.internalIcon = true;
			
			if ((!_o.drawIcon.add_icon
			        //|| !HighlighterHasKey_IncludeFilters(__o.scene, __o).IsTrue(false)
			    ) && (_o.switchType == 0)) return;
			    
			    
			tt = GetIconRectIfNextToLabel(selectionRect, GetIconRectIfNextToLabelType.CustomIcon);
			//tt.Set( defaultX - 3, defaultY, EditorGUIUtility.singleLineHeight + 6, defaultHeight );
			
			/*  var style = new GUIStyle("WhiteBackground");
			if ( Event.current.type == EventType.Repaint ) style.Draw( tt, "", false, false, false, false );*/
			// tt.width -= adapter.PREFAB_BUTTON_SIZE;
			/* var oldC = GUI.color;
			 GUI.color *= SourceBGColor( _o.id );
			 GUI.DrawTexture( tt, Texture2D.whiteTexture );
			 GUI.color = oldC;*/
			// Draw_AdapterTexture( tt, SourceBGColor( _o.id ) );
			Draw_AdapterTextureWithDynamicColor(tt, SourceBGColor);
			
			/* tt.x += tt.width - 1;
			 tt.width = 1;
			 adapter.  tempDynamicRect.Set(tt, tt, true, __o, adapter.IS_HIERARCHY() && Adapter.USE2018_3 || adapter.IS_PROJECT());
			
			 if (adapter.ENABLE_LEFTDOCK_PROPERTY && !adapter.DISABLE_DESCRIPTION( __o ))
			 {   adapter.MOI.M_Colors.DrawBackground(callFromExternal() ? null : adapter.window(), adapter.  tempDynamicRect, __o, 1 );
			 }*/
		}
		
		internal DynamicRect TryToFaeBG_Rect = new DynamicRect();
		Color inactiveColor = new Color(1, 1, 1, 0.2f);
		
		
		protected virtual bool
		DoFoldout(Rect rect, UnityEditor.IMGUI.Controls.TreeViewItem item, int id) // if (!active) return;
		{	adapter.obj_array[0] = id;
			var expandedState =
			    (bool) adapter.m_IsExpanded.Invoke(adapter.m_data.GetValue(adapter.m_TreeView(adapter.window()), null),
			                                       adapter.obj_array);
			return expandedState;
			////  Rect foldoutRect = new Rect(rect.x + foldoutIndent, this.GetFoldoutYPosition(rect.y), foldoutStyleWidth, EditorGUIUtility.singleLineHeight);
			// var on = GUI.color;
			// if (!active)GUI.color *= inactiveColor;
			//  adapter.foldoutStyle.Draw(rect,  GUIContent.none, adapter.foldoutStyle, );
			// GUI.color = on;
		}
		
		
		
		Dictionary<object, Action<UnityEditor.IMGUI.Controls.TreeViewItem, Rect>> __ti = new
		Dictionary<object, Action<UnityEditor.IMGUI.Controls.TreeViewItem, Rect>>();
		
		void IconOverlayGUI(Rect rect,
		                    UnityEditor.IMGUI.Controls.TreeViewItem item) // rect1.width = this.k_IconWidth + this.iconTotalPadding;
		{	var tree = adapter.m_TreeView(adapter.window());
		
			if (!__ti.ContainsKey(tree))
			{	__ti.Add(tree, null);
				var gui = adapter.guiProp.GetValue(tree, null);
				__ti[tree] =
				    adapter.iconOverlayGUI.GetValue(gui, null) as Action<UnityEditor.IMGUI.Controls.TreeViewItem, Rect>;
			}
			
			if (__ti[tree] == null) return;
			
			__ti[tree].Invoke(item, rect);
		}
		
		Dictionary<object, Action<UnityEditor.IMGUI.Controls.TreeViewItem, Rect>> __lo = new
		Dictionary<object, Action<UnityEditor.IMGUI.Controls.TreeViewItem, Rect>>();
		
		void LabelOverlayGUI(Rect rect,
		                     UnityEditor.IMGUI.Controls.TreeViewItem item) // rect1.width = this.k_IconWidth + this.iconTotalPadding;
		{	if (!adapter.haslabelOverlayGUI) return;
		
			var tree = adapter.m_TreeView(adapter.window());
			
			if (!__lo.ContainsKey(tree))
			{	__lo.Add(tree, null);
				var gui = adapter.guiProp.GetValue(tree, null);
				__lo[tree] =
				    adapter.labelOverlayGUI.GetValue(gui, null) as
				    Action<UnityEditor.IMGUI.Controls.TreeViewItem, Rect>;
			}
			
			if (__lo[tree] == null) return;
			
			__lo[tree].Invoke(item, rect);
		}
		
		void OverlayIconGUI(Rect rect, UnityEditor.IMGUI.Controls.TreeViewItem item, bool active) /* */
		{	if (!adapter.HasoverlayIcon) return;
		
			var icon = adapter.overlayIcon.GetValue(item, null) as Texture2D;
			
			if (!icon) return;
			
			Adapter.DrawTexture(rect, icon, active ? Color.white : inactiveColor);
		}
		
		internal override void ResetStack()
		{	foreach (var nc in adapter.new_child_cache_dic)
			{	nc.Value.wasLastAssign = false;
			}
			
			base.ResetStack();
		}
		
		// bool cache_prefab = false;
		
		// TempColorClass __INTERNAL_GetContentTempColor_Empty = new TempColorClass().AddIcon(null);
		TempColorClass __INTERNAL_GetContentTempColor = new TempColorClass();
		
		TempColorClass INTERNAL_GetContent(Adapter.HierarchyObject o, bool includePrefab = true)
		{	///
		
			// cache_prefab = false;
			
			
			/*    //  MonoBehaviour.print(M_CustomIcons.ENABLE);
			    var temoO  = EditorUtility.InstanceIDToObject(o.id);
			    if (!temoO) return null;
			    // var id = o.GetInstanceID();
			    prefab = false;*/
			// Debug.Log( Hierarchy.missing_cache.ContainsKey( o.id ));
			
#if HIERARCHY
			if (adapter.IS_HIERARCHY())
			{	if (adapter.M_CustomIcontsEnable && adapter.par.ENABLE_RIGHTDOCK_FIX &&
				        adapter.par.SHOW_MISSINGCOMPONENTS && Hierarchy.missing_cache.ContainsKey(o.id)
				        && Hierarchy.missing_cache[o.id])
				{	o.switchType = 2;
					return __INTERNAL_GetContentTempColor.AddIcon(adapter.GetIcon("WARNING"));
				}
			}
			
#endif
			
			/*var context = Utilities.ObjectContent(adapter,temoO, t).image;*/
			
			
			var context = Utilities.ObjectContent_IncludeCacher(adapter, o, adapter.t_GameObject, includePrefab);
			
			
			/* if (context.add_icon != null)
			 {   // Debug.Log(context.name);
			     if (context.add_icon == NullContext)
			     {   context = __INTERNAL_GetContentTempColor_Empty;
			     }
			     else if (adapter.IS_HIERARCHY() && Utilities.IsPrefabIcon(context.add_icon))
			     {   if (adapter.par.SHOW_PREFAB_ICON && includePrefab)
			         {
			
			             var prefab_root = adapter.FindPrefabRoot(o.go);
			             // var prefab_src = PrefabUtility.GetPrefabParent(prefab_root);
			             if (prefab_root != o.go) context = __INTERNAL_GetContentTempColor_Empty;
			             else prefab = true;
			         }
			         else
			         {   context = __INTERNAL_GetContentTempColor_Empty;
			         }
			
			     }
			 }*/ //#COLUP
#if HIERARCHY
			
			if (adapter.IS_HIERARCHY() /*&& !adapter.IS_SEARCH_MOD_OPENED()*/) //
			{	if (adapter.M_CustomIcontsEnable && adapter.par.ENABLE_RIGHTDOCK_FIX && adapter.par.SHOW_NULLS &&
				        (!context.add_icon)
				        && ((Hierarchy.null_cache.ContainsKey(o.go.GetInstanceID()) &&
				             Hierarchy.null_cache[o.go.GetInstanceID()]) ||
				            (HierarchyExtensions.Utilities.GetComponentFast<Component>.GetAll(o.go).Length < 2)))
				{	o.switchType = 1;
					return __INTERNAL_GetContentTempColor.AddIcon(adapter.GetIcon("NULL"));
				}
			}
			
#endif
			
			
			o.switchType = 0;
			return context;
		}
		
		public TempColorClass GET_CONTENT(Adapter.HierarchyObject __o)
		{	if (!adapter.IS_PROJECT()) return INTERNAL_GetContent(__o);
			else return GetImageForObject_OnlyCacher(__o);
		}
		
		public Texture GetUnityContent(Adapter.HierarchyObject o, bool includePrefab = true)
		{	var t = Utilities.ObjectContent_IncludeCacher(adapter, o, o.GET_TYPE(adapter));
		
			if (!t.add_icon) return null;
			
			var context = t.add_icon;
			//  if (context != null)
			{	if (context == adapter.NullContext)
				{	context = null;
				}
				
				else
					if (adapter.IS_HIERARCHY() && Utilities.IsPrefabIcon(context))
					{	if (adapter.par.SHOW_PREFAB_ICON && includePrefab)
						{	var prefab_root = adapter.FindPrefabRoot(o.go);
						
							// var prefab_src = PrefabUtility.GetPrefabParent(prefab_root);
							if (prefab_root != o.go) context = null;
						}
						
						else
						{	context = null;
						}
					}
			}
			
			return context;
		}
		
		
		
		
		
		bool ValidateWithoutNulls(Adapter.HierarchyObject o)
		{	if (adapter.IS_PROJECT()) return IconImageCacher.HasKey(o);
		
			//  if ( adapter.IS_PROJECT() ) return IconImageCacher.HasKey( o );
			
			
			//return INTERNAL_ GetContent(o, false) != null;
			return GetUnityContent(o, false) != null;
			//var context = INTERNAL_ GetContent(o);
			// if (context != null && context.name.Contains("Prefab")) MonoBehaviour.print(context.name);
			// return !(context == null || context == NullContext || context.name == "PrefabNormal Icon" || context.name == "PrefabModel Icon");
		}
		
		bool ValidateIncludeNulls(Adapter.HierarchyObject o)
		{	//  if ( adapter.IS_PROJECT() ) return IconImageCacher.HasKey( o );
			if (adapter.IS_PROJECT()) return true;
			
			
			//return INTERNAL_ GetContent(o, false) != null;
			return INTERNAL_GetContent(o, true).add_icon;
			//var context = INTERNAL_ GetContent(o);
			// if (context != null && context.name.Contains("Prefab")) MonoBehaviour.print(context.name);
			// return !(context == null || context == NullContext || context.name == "PrefabNormal Icon" || context.name == "PrefabModel Icon");
		}
		
		
		
		void RefreshNullsAndMissings()
		{
#if HIERARCHY
		
			if (adapter.IS_HIERARCHY())
			{	foreach (var allSceneObject in Utilities.AllSceneObjects())
				{	var id = allSceneObject.GetInstanceID();
				
					// var comps = allSceneObject.GetComponents<Component>();
					var comps = HierarchyExtensions.Utilities.GetComponentFast<Component>.GetAll(allSceneObject);
					
					
					if (adapter.par.SHOW_NULLS && comps.Length == 1)
					{	if (!Hierarchy.null_cache.ContainsKey(id)) Hierarchy.null_cache.Add(id, false);
					
						Hierarchy.null_cache[id] = true;
					}
					
					if (adapter.par.SHOW_MISSINGCOMPONENTS && comps.Any(c => !c))
					{	if (!Hierarchy.missing_cache.ContainsKey(id)) Hierarchy.missing_cache.Add(id, false);
					
						Hierarchy.missing_cache[id] = true;
					}
				}
			}
			
#endif
		}
		
		
		
		
		/* FillterData.Init(Event.current.mousePosition, SearchHelper, LayerMask.LayerToName(o.layer),
		             Validate(o) ?
		             CallHeaderFiltered(LayerMask.LayerToName(o.layer)) :
		             CallHeader(),
		             this);*/
		/** CALL HEADER */
		_W__SearchWindow.FillterData_Inputs m_CallHeader()
		{	Func<Adapter.HierarchyObject, TempColorClass> gettexture = null;
			//GetImageForObject_OnlyCacher;
			/* if ( adapter.IS_PROJECT() ) gettexture = INTERNAL_GetContent( b , false );
			 else*/
			gettexture = (b) => INTERNAL_GetContent(b, false);
			
			var result = new _W__SearchWindow.FillterData_Inputs(callFromExternal_objects)
			{	Valudator = ValidateWithoutNulls,
				    SelectCompareString = (b, i) =>
				{	var r = gettexture(b).add_icon;
				
					if (r == null) return "";
					
					return r.name;
				},
				SelectCompareCostInt = (b, i) =>
				{	var cost = i;
					cost += b.Active() ? 0 : 100000000;
					var c = gettexture(b).add_icon;
					
					if (c != null && c.name.StartsWith("GUID=")) cost += 1000000; ////////////////////////////////////
					
					return cost;
				}
			};
			return result;
		}
		
		internal override _W__SearchWindow.FillterData_Inputs CallHeader()
		{	if (adapter.par.SHOW_NULLS || adapter.par.SHOW_MISSINGCOMPONENTS) RefreshNullsAndMissings();
		
			return m_CallHeader();
		}
		
		internal _W__SearchWindow.FillterData_Inputs CallHeaderFiltered(Texture contentTexture)
		{	Func<Adapter.HierarchyObject, bool> gettexture = null;
			/* if ( adapter.IS_PROJECT() ) gettexture = s => GetImageForObject_OnlyCacher( s ).add_icon == contentTexture;
			 else gettexture = s => ValidateIncludeNulls( s ) && INTERNAL_GetContent( s , true ).add_icon == contentTexture;*/
			
			if (adapter.IS_PROJECT()) gettexture = s => INTERNAL_GetContent(s, true).add_icon == contentTexture;
			else gettexture = s => ValidateIncludeNulls(s) && INTERNAL_GetContent(s, true).add_icon == contentTexture;
			
			if (adapter.IS_HIERARCHY())
			{	if (adapter.par.SHOW_NULLS && contentTexture == adapter.GetIcon("NULL") ||
				        adapter.par.SHOW_MISSINGCOMPONENTS && contentTexture == adapter.GetIcon("WARNING"))
					RefreshNullsAndMissings();
			}
			
			var result = m_CallHeader();
			result.Valudator = gettexture;
			return result;
		}
		/** CALL HEADER */
		
		
		
		
		/*   internal override bool CallHeader(out GameObject[] obs, out int[] contentCost)
		   {
		       if (par.SHOW_NULLS || par.SHOW_MISSINGCOMPONENTS) RefreshNullsAndMissings();
		       obs = Utilities.AllSceneObjects().Where(ValidateWithoutNulls).ToArray();
		
		       var cont = obs.Select((b, i) => new { obj = b, name = INTERNAL_GetContent(b, false).name, startIndex = i }).OrderBy(b => b.name).Select((b, i) => {
		           var cost = i;
		           cost += b.obj.activeInHierarchy ? 0 : 100000000;
		           if (!b.name.StartsWith("GUID=")) cost += 1000000;
		           return new { b.obj, cost, b.startIndex };
		       }
		       ).OrderBy(b => b.startIndex);
		
		       contentCost = cont.Select(c => c.cost).ToArray();
		       return true;
		   }
		
		   internal bool CallHeader(out GameObject[] obs, out int[] contentCost, Texture contentTexture)
		   {
		       if (par.SHOW_NULLS && contentTexture == Hierarchy.GetIcon("NULL") || par.SHOW_MISSINGCOMPONENTS && contentTexture == Hierarchy.GetIcon("WARNING")) RefreshNullsAndMissings();
		       obs = Utilities.AllSceneObjects().Where(ValidateIncludeNulls).ToArray();
		
		       var firstList = obs.Select((b, i) => new { obj = b, texture = INTERNAL_GetContent(b, true), startIndex = i }).Where(s => s.texture == contentTexture).ToArray();
		       if (firstList.Length == 0)
		       {
		           obs = new GameObject[0];
		           contentCost = new int[0];
		           return true;
		       }
		
		       obs = firstList.Select(l => l.obj).ToArray();
		
		       var cont = firstList.OrderBy(b => b.texture.name).Select((b, i) => {
		           var cost = i;
		           cost += b.obj.activeInHierarchy ? 0 : 100000000;
		           if (!b.texture.name.StartsWith("GUID=")) cost += 1000000;
		           return new { b.obj, cost, b.startIndex };
		       }
		       ).OrderBy(b => b.startIndex);
		
		       contentCost = cont.Select(c => c.cost).ToArray();
		       return true;
		   }
		*/
		/*     contentCost = cont.ToArray();
		     }
		 int ToContentCost(GameObject o)
		 {
		     o
		     var aud = o.GetComponent<AudioSource>();
		     if (aud.clip == null) return 1;
		     return 0;
		 }
		*/
		
		
		
		
		//         "sv_icon_name" with a "" postFix (label icons: max 8),
		// "sv_icon_dot" with a "_sml" postFix (small icon, max 16),
		//"sv_icon_dot" with a "_pix16_gizmo" postFix (large icon, max 16)
		public static GUIContent[] labelIcons;
		public static GUIContent[] largeIcons;
		
		/* public  void SetIcon(GameObject gObj, LabelIcon icon)
		 {
		     if (labelIcons == null)
		     {
		         labelIcons = GetTextures("sv_label_", string.Empty, 0, 8);
		     }
		
		     SetIcon(gObj, labelIcons[(int)icon].image as Texture2D);
		 }
		
		 public  void SetIcon(GameObject gObj, Icon icon)
		 {
		     if (largeIcons == null)
		     {
		         largeIcons = GetTextures("sv_icon_dot", "_pix16_gizmo", 0, 16);
		     }
		
		     SetIcon(gObj, largeIcons[(int)icon].image as Texture2D);
		 }*/
		
		public static void InitIcons()
		{	if (labelIcons == null)
			{	labelIcons = GetTextures("sv_label_", string.Empty, 0, 8);
			}
			
			if (largeIcons == null)
			{	largeIcons = GetTextures("sv_icon_dot", "_pix16_gizmo", 0, 16);
			}
		}
		
		internal Texture2D GetIcon(string str)
		{	GUIContent result = null;
			var arr = labelIcons.Select(l => l.image.name).ToList();
			
			if (arr.IndexOf(str) != -1) result = labelIcons[Mathf.Clamp(arr.IndexOf(str), 0, labelIcons.Length - 1)];
			
			arr = largeIcons.Select(l => l.image.name).ToList();
			
			if (arr.IndexOf(str) != -1) result = largeIcons[Mathf.Clamp(arr.IndexOf(str), 0, largeIcons.Length - 1)];
			
			return result == null ? null : (Texture2D) result.image;
		}
		
		/*  public  void SetIcon(GameObject gObj, string icon)
		  {
		      InitIcons();
		
		      GUIContent result = null;
		      if (LabelIcon.IndexOf(icon) != -1) result = labelIcons[Mathf.Clamp(LabelIcon.IndexOf(icon), 0, labelIcons.Length - 1)];
		      if (Icon.IndexOf(icon) != -1) result = largeIcons[Mathf.Clamp(Icon.IndexOf(icon), 0, largeIcons.Length - 1)];
		
		      if (result == null) return;
		      SetIcon(gObj, (Texture2D)result.image);
		  }*/
		
		private void SetIcon(Adapter.HierarchyObject o, Texture2D texture)
		{	// if (!adapter.IS_PROJECT() || o.project.assetPath.EndsWith( ".prefab" ) || o.project.assetPath.EndsWith( ".cs" )) {
		
		
			if (adapter.SELECTED_GAMEOBJECTS().All(selO => selO != o))
			{	____SetIcon(o, texture);
				// if ( adapter.par.ENABLE_PING_Fix ) adapter.TRY_PING_OBJECT( o );
				/*Undo.RecordObject(o, "Change tag");
				o.tag = tag;
				Hierarchy.SetDirty(o);
				if (Hierarchy.par.ENABLE_PING_Fix) adapter.TRY_PING_OBJECT(o);*/
			}
			
			else
			{	foreach (var objectToUndo in adapter.SELECTED_GAMEOBJECTS())
				{	____SetIcon(objectToUndo, texture);
				}
			}
			
			ClearCache();
			adapter.RepaintAllViews();
		}
		
		MethodInfo mi;
		
		private void ____SetIcon(Adapter.HierarchyObject o, Texture2D texture)
		{	var SAVE_GUID = adapter.IS_PROJECT();
		
			if (mi == null)
			{	var ty = typeof(EditorGUIUtility);
				mi = ty.GetMethod("SetIconForObject", (BindingFlags) (-1));
			}
			
			if (SAVE_GUID)
			{	if (!texture)
				{	adapter.CreateUndoActiveDescription("Set Icon", (o.scene));
					SetImageForObject_OnlyCacher(o, null);
					adapter.SetDirtyActiveDescription((o.scene));
				}
				
				else
				{	var path = AssetDatabase.GetAssetPath(texture);
				
					if (!string.IsNullOrEmpty(path))
					{	var guid = AssetDatabase.AssetPathToGUID(path);
						adapter.CreateUndoActiveDescription("Set Icon", (o.scene));
						SetImageForObject_OnlyCacher(o, guid);
						adapter.SetDirtyActiveDescription((o.scene));
					}
				}
			}
			
			else
			{	var t = adapter.HO_TO_OBJECT(o);
			
				if (t)
				{	adapter.HO_RECORD_UNDO(o, "Set Icon");
					mi.Invoke(null, new object[] {t, texture});
					//var guid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(texture));
					bool setted = false;
					
					if (texture && (texture.name.StartsWith("sv_icon_dot") || texture.name.StartsWith("sv_label_")))
					{	if (AssetDatabase.GetAssetPath(texture).StartsWith("Library/"))
						{	SetImageForObject_OnlyCacher(o, "Library/" + texture.name);
							setted = true;
						}
					}
					
					//if ( !texture )
					//SetImageForObject_OnlyCacher( o , null );
					if (!setted)
						SetImageForObject_OnlyCacher_2(o, texture);
						
					// SetImageForObject_OnlyCacher( o , guid );
					adapter.HO_SETDIRTY(o);
					adapter.MarkSceneDirty(o.scene);
				}
			}
		}
		
		private void ____SetIconOnlyInternal(Adapter.HierarchyObject o, Texture2D texture)
		{	if (adapter.IS_PROJECT()) return;
		
			if (mi == null)
			{	var ty = typeof(EditorGUIUtility);
				mi = ty.GetMethod("SetIconForObject", (BindingFlags) (-1));
			}
			
			var t = adapter.HO_TO_OBJECT(o);
			
			if (t)
				mi.Invoke(null, new object[] {t, texture});
		}
		
		
		private void
		SetAction(Adapter.HierarchyObject o,
		          Action<Adapter.HierarchyObject> ac) //  var ty = typeof(EditorGUIUtility);
		{	//  var mi = ty.GetMethod("SetIconForObject", /*BindingFlags.InvokeMethod |*/ BindingFlags. | BindingFlags.NonPublic);
		
			if (adapter.SELECTED_GAMEOBJECTS().All(selO => selO != o))
			{	/*  Undo.RecordObject(o, "Set Icon");
				                   mi.Invoke(null, new object[] { o, texture });
				                   Hierarchy.SetDirty(o);
				                   Hierarchy.MarkSceneDirty(o.scene);*/
				ac(o);
				//   if (Hierarchy.par.ENABLE_PING_Fix) adapter.TRY_PING_OBJECT(o);
				/*Undo.RecordObject(o, "Change tag");
				o.tag = tag;
				Hierarchy.SetDirty(o);
				if (Hierarchy.par.ENABLE_PING_Fix) adapter.TRY_PING_OBJECT(o);*/
			}
			
			else
			{	foreach (var objectToUndo in adapter.SELECTED_GAMEOBJECTS())
				{	ac(objectToUndo);
					/*   Undo.RecordObject(objectToUndo, "Set Icon");
					   mi.Invoke(null, new object[] { objectToUndo, texture });
					   Hierarchy.SetDirty(objectToUndo);
					   Hierarchy.MarkSceneDirty(objectToUndo.scene);*/
				}
			}
		}
		
		/* private  GUIContent[] GetTextures(string baseName, string postFix, int startIndex, int count)
		 {
		     GUIContent[] guiContentArray = new GUIContent[count];
		
		     var t = typeof(EditorGUIUtility);
		     var mi = t.GetMethod("IconContent", BindingFlags.NonPublic | BindingFlags., null, new Type[] { typeof(string) }, null);
		
		     for (int index = 0; index < count; ++index)
		     {
		         guiContentArray[index] = mi.Invoke(null, new object[] { baseName + (object)(startIndex + index) + postFix }) as GUIContent;
		     }
		
		     return guiContentArray;
		 }*/
		internal static GUIContent[] GetTextures(string baseName, string postFix, int startIndex, int count)
		{	GUIContent[] array = new GUIContent[count];
		
			for (int i = 0; i < count; i++)
			{	array[i] = EditorGUIUtility.IconContent(baseName + (startIndex + i) + postFix);
			}
			
			return array;
		}
		
		
		
		/*#pragma warning disable
		         List<string> LabelIcon = new List<string>
		       {
		            "Gray",
		            "Blue",
		            "Teal",
		            "Green",
		            "Yellow",
		            "Orange",
		            "Red",
		            "Purple"
		        };
		
		         List<string> Icon = new List<string>{
		        "CircleGray",
		        "CircleBlue",
		        "CircleTeal",
		        "CircleGreen",
		        "CircleYellow",
		        "CircleOrange",
		        "CircleRed",
		        "CirclePurple",
		        "DiamondGray",
		        "DiamondBlue",
		        "DiamondTeal",
		        "DiamondGreen",
		        "DiamondYellow",
		        "DiamondOrange",
		        "DiamondRed",
		        "DiamondPurple"
		        };*/
		
		
		
		
		
	}
	
	
	
	
	
	
	
	
}


class HierarchyImpl { }
}