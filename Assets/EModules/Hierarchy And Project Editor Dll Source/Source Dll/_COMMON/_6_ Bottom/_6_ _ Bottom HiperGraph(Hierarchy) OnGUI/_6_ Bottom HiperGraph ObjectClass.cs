﻿
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
#if PROJECT
	using EModules.Project;
#endif
//namespace EModules



namespace EModules.EModulesInternal

{
internal partial class Adapter {

	internal enum ScrollType { Default = 0, HyperGraphScroll = 1, HyperGraphScroll_Window = 2, FavorGraphScroll = 3, FavorGraphScroll_Window = 4 }
	
	internal Action<ScrollType, float> OnScroll;
	internal partial class BottomInterface {
		/*void OnInspectorUpdate()
		{
		    EditorApplication.on
		}*/
		
	}
	internal partial class HyperGraph {
		internal void Label( Rect r, string s, TextAnchor an )
		{	var a  = adapter. label.alignment;
			adapter.label.alignment = an;
			GUI.Label( r, s, adapter.label );
			adapter.label.alignment = a;
		}
		internal void Label( Rect r, string s )
		{	GUI.Label( r, s, adapter.label );
		}
		internal void Label( Rect r, GUIContent s )
		{	GUI.Label( r, s, adapter.label );
		}
		
		internal bool Button( Rect r, string s )
		{	return GUI.Button( r, s, adapter.button );
		}
		internal bool Button( Rect r, string s, TextAnchor an )
		{	var a  = adapter.button.alignment;
			adapter.button.alignment = an;
			var res = GUI.Button( r, s, adapter.button );
			adapter.button.alignment = a;
			return res;
		}
		internal bool Button( Rect r, GUIContent s )
		{	return GUI.Button( r, s, adapter.button );
		}
		internal bool Button( Rect r, GUIContent s, TextAnchor an )
		{	var a  = adapter.button.alignment;
			adapter.button.alignment = an;
			var res = GUI.Button( r, s, adapter. button );
			adapter.button.alignment = a;
			return res;
			
		}
		
		
		
		
		
		
		
		private bool SKANNING = false;
#pragma warning disable
		private bool ERROR = false;
#pragma warning restore
		
		private Object CurrentSelection = null;
		//  internal  bool wasInit = false;
		//   SortedDictionary<UnityEngine.Object, activeComps> TARGET_COMPS = new SortedDictionary<int, activeComps>();
		private Dictionary<int, ObjectDisplay> TARGET_COMPS = new Dictionary<int, ObjectDisplay>();
		//   private  Dictionary<int, ObjectDisplay> SELF_TARGETS = new Dictionary<int, ObjectDisplay>();
		
		private float __TARGET_HEIGHT = 0;
		private float TARGET_HEIGHT
		{	get { return Mathf.RoundToInt( __TARGET_HEIGHT ); }
		
			set { __TARGET_HEIGHT = value; }
		}
		private float __TARGET_CURRENT_Y = 0;
		private float TARGET_CURRENT_Y
		{	get { return Mathf.RoundToInt( __TARGET_CURRENT_Y ); }
		
			set { __TARGET_CURRENT_Y = value; }
		}
		private float __INPUT_CURRENT_Y = 0;
		private float INPUT_CURRENT_Y
		{	get { return Mathf.RoundToInt( __INPUT_CURRENT_Y ); }
		
			set { __INPUT_CURRENT_Y = value; }
		}
		
		/*     findedComopnentsAttached
		         findedComopnentsAttached.Add(currentList[currentIndex].GetInstanceID(), new activeComps() {
		                 comps = activeComps,
		                 DRAW_A_POSES = new Vector2[activeComps.Count],
		                 DRAW_B_POSES = new Vector2[fieldsCount],
		                 height = height
		             });*/
		private List<int> PTR = new List<int>();
		
		private GUIContent tootipContent = new GUIContent();
		
		private Rect moduleRect = new Rect();
		private Rect GAMEOBJECTRECT = new Rect();
		private GUIContent content = new GUIContent();
		private UnityEngine.Object[] comps = null;
		private bool[] compsinitialized = new bool[0];
		readonly private SortedList<int, int> compsSorted = new SortedList<int, int>();
		private Vector2[] comps_inPos = new Vector2[0];
		/*  private float __selectObject_height_WINDOW;
		  private float __selectObject_height_MAIN;*/
		private float __selectObject_height;
		private float SelectObject_height     //   get { return Mathf.RoundToInt( CURRENT_CONTROLLER.MAIN ? __selectObject_height_MAIN : __selectObject_height_WINDOW ); }
		{	get { return __selectObject_height; }
		
			set { __selectObject_height = value; }
			
			/*set {
			  if (CURRENT_CONTROLLER.MAIN) __selectObject_height_MAIN = value;
			  else __selectObject_height_WINDOW = value;
			}*/
		}
		/*void add_selectObject_height(float additional_value)
		{
		  __selectObject_height_MAIN += adapter.par.HiperGraphParams.SCALE * additional_value;
		  __selectObject_height_WINDOW += adapter.par.HiperGraphParams.WINDIOW_SCALE * additional_value;
		}
		void set_selectObject_height(float value)
		{
		  __selectObject_height_MAIN = value / CURRENT_SCALE * adapter.par.HiperGraphParams.SCALE;
		  __selectObject_height_WINDOW = value / CURRENT_SCALE * adapter.par.HiperGraphParams.WINDIOW_SCALE;
		}
		*/
		
		
		private List<Object> UndoList = new List<Object>();
		private int UndoPos = 0;
		private bool skipUndo = false;
		private GUIContent scanningContent = new GUIContent("?   ", "Searching for references...");
		private int DRAWING_INDEX = 0;
		
		private Vector2 p1,
		        p2;
		        
		private Color alpha = new Color(1, 1, 1, 0.3f);
		
		//BUTTON 19
		//COMP 12
		//VAR 7
		//
		//////////////////////////////////////////////////
		//////////////////////////////////////////////////  DRAW TARGET
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private Rect singleRect = new Rect();
		private Rect ARROW_RECT = new Rect();
		private int DRAWSINGLECOMP_TARGET_EVENT_START = 10000;
		private int DRAWSINGLECOMP_TARGET_EVENT_M2 = 20000;
		private int DRAWSINGLECOMP_TARGET_EVENT_INPUT = 30000;
		private Vector2 DRAWSINGLECOMP_RESULT = new Vector2();
		private Color prefab_color = new Color(0.05f, 0.6f, 0.9f);
		private Color event_color = new Color(0.35f, 0.9f, 1f);
		private Color asset_color = new Color(0.6f, 0.9f, 0.05f);
#pragma warning disable
		private Color arrowC1 = new Color32(236, 198, 60, 155);
		private Color arrowC1personal = new Color32(56 / 2, 89 / 2, 102 / 2, 255);
		//   private  Color arrowC1personal = Color.black;
		//  private  Color arrowC1personal = new Color32(236 / 5, 198 / 5, 60 / 5, 185);
		//private  Color arrowC1personal = new Color32(236 / 2, 198 / 2, 60 / 2, 185);
		// private  Color arrowC2 = new Color32(41, 41, 41, 255);
		//  private  Color arrowC3 = new Color32(255, 204, 9, 255);
#pragma warning restore
		
		//  Rect arrowRect = new Rect();
		private Rect point = new Rect(0, 0, 1, 1);
		private Rect? ScreenRect = null;
		
		/// <summary>
		/// ARRAY SUPPORT
		/// </summary>
		private static Type compType = typeof(UnityEngine.Object);
		
		//	private Type gameobjectType = typeof( GameObject );
		// private  Dictionary<string, Type> PTR_COMP = new Dictionary<string, Type>();
		
		private Dictionary<Type, FieldsAccessor> FiledsInfo = null;
		
		
		
		
		// private Rect INPUT_RECT = new Rect();
		
		private bool WASSCAN = false;
		
		/*  readonly private DoubleList<int, FIELD_PARAMS> findedList =
		      new DoubleList<int, FIELD_PARAMS>();*/
		readonly private Dictionary<int, FIELD_PARAMS> findedList =
		    new Dictionary<int, FIELD_PARAMS>();
		    
		readonly private Dictionary<int, ObjectDisplay> INPUT_COMPS =
		    new Dictionary<int, ObjectDisplay>();
		    
		readonly internal List<ObjectDisplay> SCANNING_COMPS =
		    new List<ObjectDisplay>();
		    
		//  readonly List<int> SCANNING_COMPS_REMOVER = new List<int>();
		
		private IEnumerator<HierarchyObject> currentList = null;
		int CountProgress = 0;
		private int currentIndex = 0;
		
		
		private float SKANNING_PROGRESS
		{	get
			{	if ( currentList == null || CountProgress == 0 ) return 0;
			
				return CountProgress < 2 ? 0 : (currentIndex / (CountProgress - 1f));
			}
		}
		
		private float DEFAULTWIDTH( BottomInterface.UniversalGraphController
		                            controller )     //return Math.Max( Mathf.RoundToInt( controller.WIDTH / 5 * 4 / 3.5f ), 100 ) * controller.DEFAULT_WIDTH( controller );
		{	var result = Math.Max( Mathf.RoundToInt( controller.WIDTH / 5 * 4 / 3.5f ), 100 );
		
			if ( !controller.MAIN ) result = Mathf.RoundToInt( result / 1.5f );
			
			return result;
		}
		
		private void CHANGEPLAYMODE()
		{	StoptBroadcasting();
			StartBroadcasting();
			SCENE_CHANGE();
		}
		
		
		void ON_SCROLL( ScrollType type, float sc )
		{	if ( type == ScrollType.HyperGraphScroll )
			{	if ( sc < 0 ) SET_SCALE( ScrollType.HyperGraphScroll, Mathf.CeilToInt( adapter.par.HiperGraphParams.SCALE * 10 + 0.001f ) / 10f );
				else SET_SCALE( ScrollType.HyperGraphScroll, Mathf.FloorToInt( adapter.par.HiperGraphParams.SCALE * 10 - 0.001f ) / 10f );
				
				EventUse();
			}
			
			else
				if ( type == ScrollType.HyperGraphScroll_Window )
				{	if ( sc < 0 ) SET_SCALE( ScrollType.HyperGraphScroll_Window, Mathf.CeilToInt( adapter.par.HiperGraphParams.WINDIOW_SCALE * 10 + 0.001f ) / 10f );
					else SET_SCALE( ScrollType.HyperGraphScroll_Window, Mathf.FloorToInt( adapter.par.HiperGraphParams.WINDIOW_SCALE * 10 - 0.001f ) / 10f );
					
					EventUse();
				}
		}
		
		//    HyperController currentController;
		private void INITIALIZE( BottomInterface.UniversalGraphController controller )
		{	if ( controller.wasInit ) return;
		
			// currentController = controller;
			
			adapter.bottomInterface.onSceneChange -= SCENE_CHANGE;
			adapter.bottomInterface.onSceneChange += SCENE_CHANGE;
			
			adapter.bottomInterface.onSelectionChange -= CHANGE_SELECTION;
			adapter.bottomInterface.onSelectionChange += CHANGE_SELECTION;
			
			//  EditorApplication.playmodeStateChanged -= CHANGEPLAYMODE;
			// EditorApplication.playmodeStateChanged += CHANGEPLAYMODE;
			adapter.PLAYMODECHANGE2 = CHANGEPLAYMODE;
			adapter.SubcripePlayModeStateChange();
			
			
			adapter.OnScroll -= ON_SCROLL;
			adapter.OnScroll += ON_SCROLL;
			
			
			var v1 = HierHyperController.wasInit;
			var v2 = adapter.bottomInterface.hyperGraph.WindowHyperController.wasInit;
			
			if ( CurrentSelection == null ) CHANGE_SELECTION_OVVERIDE( false );
			
			HierHyperController.wasInit = v1;
			adapter.bottomInterface.hyperGraph.WindowHyperController.wasInit = v2;
			controller.wasInit = true;
			
			controller.scrollPos.x = RECT.width / 2;
			var h = DRAWOBJECT(controller, true);
			controller.scrollPos.y = Math.Max( (controller.HEIGHT) / 2 - h / 2, 10 );
		}
		
		private void SCENE_CHANGE()
		{	CHANGE_SELECTION_OVVERIDE( true );
			CurrentSelection = null;
			StoptBroadcasting();
		}
		
		private void CHANGE_SELECTION()
		{	if ( !adapter.par.HiperGraphParams.AUTOCHANGE ) return;
		
			CHANGE_SELECTION_OVVERIDE();
		}
		
		
		bool OBJECT_ISVALID( Object o )
		{	if ( adapter.IS_HIERARCHY() ) return o is GameObject && ((GameObject)o).scene.IsValid() && ((GameObject)o).scene.isLoaded;
			else return o && !string.IsNullOrEmpty( adapter.bottomInterface.INSTANCEID_TOGUID( o.GetInstanceID() ) );
		}
		
		
		private void CHANGE_SELECTION_OVVERIDE( bool skipAutoHide = false, Object selection = null )
		{
#if UNITY_EDITOR
			// MonoBehaviour.print("CHANGE_SELECTION_OVVERIDE");
#endif
			WASDRAW = false;
			HierHyperController.wasInit = false;
			adapter.bottomInterface.hyperGraph.WindowHyperController.wasInit = false;
			var active = selection ?? Selection.activeObject;
			Object newSelection = null;
			
			
			
			if ( active && OBJECT_ISVALID( active ) )
			{	newSelection = active;
			}
			
			if ( !skipAutoHide && adapter.par.HiperGraphParams.AUTOHIDE && CurrentSelection != newSelection )
			{	SWITCH_ACTIVE( false );
				return;
			}
			
			if ( CurrentSelection != newSelection )
			{	if ( newSelection )
				{	comps = null;
					CurrentSelection = newSelection;
					
					if ( !skipUndo ) SETUNDO();
					
					skipUndo = false;
					StoptBroadcasting();
					StartBroadcasting();
				}
				
				else         //StoptBroadcasting();
				{
				}
			}
		}
		
		
		private void REFRESH()
		{	CurrentSelection = null;
			CHANGE_SELECTION_OVVERIDE( true );
		}
		
		internal void RefreshWithCurrentSelection()
		{	comps = null;
			CHANGE_SELECTION_OVVERIDE( true, CurrentSelection );
		}
		
		
		/*
		static Object GET_OBJECT(object target, FieldInfo fieldInfo)
		{
		   var result = fieldInfo.GetValue(target);
		   if (result != null && !(result is Object) )
		   {
		       if (!deep_analizer_Serializer_property_dropper.ContainsKey(fieldInfo)) return null;
		       else
		       {
		
		       }
		   }
		   return result as Object;
		}
		
		
		static readonly List<FieldInfo> cache_serializable_fieldsList = new List<FieldInfo>();
		static Dictionary<FieldInfo, bool> cache_serializable = new Dictionary<FieldInfo, bool>();
		static Dictionary<FieldInfo, FieldInfo> deep_analizer_Serializer_property_dropper = new Dictionary<FieldInfo, FieldInfo>();
		static bool FindSerializableInSerializable(FieldInfo type)
		{
		   if (cache_serializable.ContainsKey(type)) return cache_serializable[type];
		
		
		   GET_FIELDS(type);
		   var result = cache_serializable_fieldsList.FirstOrDefault(f=> IsFieldReturnObject(f));
		
		   if (result != null)
		   {
		       cache_serializable.Add(type, true);
		       return result;
		   }
		
		
		       cache_serializable
		}
		
		static void GET_FIELDS(FieldInfo type)
		{
		   var t = type.FieldType;
		   cache_serializable_fieldsList.Clear();
		   while (t != null)
		   {
		       cache_serializable_fieldsList.AddRange(t.GetFields(flags));
		       t = t.BaseType;
		   }
		}*/
		/* Type serType = typeof(SerializeField);
		 static bool IsFieldReturnObject(FieldInfo f)
		 {
		     bool first = false;
		     //.. bool secont = false;
		     if (f.IsPublic) first = true;
		     if (!first)
		     {
		         // if (f.GetCustomAttribute<SerializeField>() != null) first = true;
		         if ((f.Attributes & FieldAttributes.NotSerialized) == 0) first = true;
		       //  if (f.GetCustomAttributes( ser ) != null) first = true;
		         //  adapter.logProxy.Log(f.GetCustomAttributes(true).Length == 0 ? "" : f.GetCustomAttributes(true).Select(s => s.ToString()).Aggregate((a, b) => a + " " + b));
		     }
		     // (f.IsPublic ||  != null)
		     if (first && compType.IsAssignableFrom( f.FieldType )) return true;
		     return false;
		 }*/
		
		
		static  Type serType = typeof(SerializeField);
		static bool IsFieldReturnObject( FieldInfo f )
		{	bool first = false;
		
			if ( (f.Attributes & FieldAttributes.NotSerialized) != 0 ) return false;
			
			if ( f.IsPublic ) first = true;
			
			if ( !first )
			{	if ( f.GetCustomAttributes( serType, false ).Length != 0 )
				{	first = true;
				}
			}
			
			if ( first && compType.IsAssignableFrom( f.FieldType ) ) return true;
			
			return false;
		}
		
		
		// static Type ser_type = typeof(SerializeField);
		
		
		//static readonly Dictionary<Type, FieldInfo[]> cache_fields = new Dictionary<Type, FieldInfo[]>();
		// readonly List<FieldInfo> fieldsList = new List<FieldInfo>();
		// const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly;
		internal System.Object LOCKER = new System.Object();
		
		Vector3 zero = new Vector3(0, 0, 0);
		internal class FieldsAccessor {
			HyperGraph hyperGraph;
			internal FieldsAccessor( HyperGraph hyperGraph, Type c )
			{	this.hyperGraph = hyperGraph;
				this.type = c;
			}
			/*	public FieldsAccessor(  )
			{
			// reference_comp = c;
			}*/
			
			// internal Component reference_comp;
			internal List<ObjectDisplay> snannig_callback = new List<ObjectDisplay>();
			
			internal Type type;
			bool m_completed;
			internal bool completed_thead
			{	get
				{	if ( !m_completed && hyperGraph.current_job.stopped == false ) hyperGraph.current_job.StarTAllJobs();
				
					return m_completed;
				}
				
				set { m_completed = value; }
			}
			//internal FieldInfo[] f;
			internal FieldAdapter[] faList;
			
			internal bool InitializeTarget_InMainThead( UnityEngine.Object reference_comp )
			{	if ( faList.Length == 0 || !reference_comp ) return true;
			
				for ( int fIndex = 0 ; fIndex < faList.Length ; fIndex++ )     // hyperGraph.selectObject_height += hyperGraph.SIZES.SPACE() + hyperGraph.SIZES.VAR();
				{	hyperGraph.SelectObject_height += hyperGraph.SIZE.SPACE() + hyperGraph.SIZE.VAR();
				
					Dictionary<string, object> values = null;
					
					try
					{	values = faList[fIndex].GetAllValues( reference_comp, 0, hyperGraph.adapter._S_HG_EventsMode | hyperGraph.adapter._S_HG_SkipArrays );
					
						if (faList[fIndex].GetAllValuesCache != null  )
						{	faList[fIndex].CheckID(selection_id, SEL_INC);
						
							if (!faList[fIndex].GetAllValuesCache.ContainsKey(reference_comp.GetInstanceID() )) faList[fIndex].GetAllValuesCache.Add(reference_comp.GetInstanceID(), values);
							else faList[fIndex].GetAllValuesCache[reference_comp.GetInstanceID() ] = values;
						}
					}
					
					catch
					{	hyperGraph.CHANGEPLAYMODE();
						return false;
					}
					
					
					if ( values == null ) continue;
					
					// fieldsCount++;
					
					foreach ( var _value in values )
					{	var value = _value.Value as UnityEngine.Object;
					
						if ( !value ) continue;
						
						Component comp = value as Component;
						GameObject go = comp != null ? comp.gameObject : value as GameObject;
						
						if ( go && !hyperGraph.TARGET_COMPS.ContainsKey( go.GetInstanceID() ) ) //GO
						{	var r = new ObjectDisplay(go.GetInstanceID(), hyperGraph);
							hyperGraph.TARGET_COMPS.Add( go.GetInstanceID(), r );
							
							if ( go != hyperGraph.CurrentSelection )
							{	hyperGraph.TARGET_HEIGHT += hyperGraph.SIZE.OBJECT() + hyperGraph.SIZE.padding_y();
								r.height += hyperGraph.SIZE.OBJECT() + hyperGraph.SIZE.padding_y();
							}
							
							r.DRAW_A_POSES.Add( hyperGraph.zero );
						}
						
						if ( comp && !hyperGraph.TARGET_COMPS[go.GetInstanceID()].objecComps.ContainsKey( comp.GetInstanceID() ) ) //COMP
						{	var r = hyperGraph.TARGET_COMPS[go.GetInstanceID()];
							// TARGET_COMPS.Add(comp.GetInstanceID(), r);
							r.objecComps.Add( comp.GetInstanceID(), r.objecComps.Count );
							
							if ( go != hyperGraph.CurrentSelection )
							{	hyperGraph.TARGET_HEIGHT += hyperGraph.SIZE.COMP();
								r.height += hyperGraph.SIZE.COMP();
							}
							
							r.DRAW_A_POSES.Add( hyperGraph.zero );
						}
						
						if ( !go && !comp && !hyperGraph.TARGET_COMPS.ContainsKey( value.GetInstanceID() ) ) //ASSET
						{	var r = new ObjectDisplay(value.GetInstanceID(), hyperGraph);
							hyperGraph.TARGET_COMPS.Add( value.GetInstanceID(), r );
							
							if ( value != hyperGraph.CurrentSelection )
							{	hyperGraph.TARGET_HEIGHT += hyperGraph.SIZE.OBJECT() + hyperGraph.SIZE.padding_y();
								r.height += hyperGraph.SIZE.OBJECT() + hyperGraph.SIZE.padding_y();
							}
							
							r.DRAW_A_POSES.Add( hyperGraph.zero );
						}
						
					}
					
					/*// OLD SINLE VALUE
					UnityEngine.Object value = null;
					try
					{   value = f[fIndex].GetValue( reference_comp ) as UnityEngine.Object;
					}
					catch
					{   hyperGraph.CHANGEPLAYMODE();
					    return false;
					}
					if (value == null) continue;
					// fieldsCount++;
					Component comp = value as Component;
					GameObject go = comp != null ? comp.gameObject : value as GameObject;
					
					if (go && !hyperGraph.TARGET_COMPS.ContainsKey( go.GetInstanceID() )) //GO
					{   var r = new ObjectDisplay(go.GetInstanceID(), hyperGraph);
					    hyperGraph.TARGET_COMPS.Add( go.GetInstanceID(), r );
					
					    if (go != hyperGraph.CurrentSelection)
					    {   hyperGraph.TARGET_HEIGHT += hyperGraph.SIZE.OBJECT() + hyperGraph.SIZE.padding_y();
					        r.height += hyperGraph.SIZE.OBJECT() + hyperGraph.SIZE.padding_y();
					    }
					
					    r.DRAW_A_POSES.Add( hyperGraph.zero );
					}
					if (comp && !hyperGraph.TARGET_COMPS[go.GetInstanceID()].objecComps.ContainsKey( comp.GetInstanceID() )) //COMP
					{   var r = hyperGraph.TARGET_COMPS[go.GetInstanceID()];
					    // TARGET_COMPS.Add(comp.GetInstanceID(), r);
					    r.objecComps.Add( comp.GetInstanceID(), r.objecComps.Count );
					    if (go != hyperGraph.CurrentSelection)
					    {   hyperGraph.TARGET_HEIGHT += hyperGraph.SIZE.COMP();
					        r.height += hyperGraph.SIZE.COMP();
					    }
					    r.DRAW_A_POSES.Add( hyperGraph.zero );
					}
					if (!go && !comp && !hyperGraph.TARGET_COMPS.ContainsKey( value.GetInstanceID() )) //ASSET
					{   var r = new ObjectDisplay(value.GetInstanceID(), hyperGraph);
					    hyperGraph.TARGET_COMPS.Add( value.GetInstanceID(), r );
					
					    if (value != hyperGraph.CurrentSelection)
					    {   hyperGraph.TARGET_HEIGHT += hyperGraph.SIZE.OBJECT() + hyperGraph.SIZE.padding_y();
					        r.height += hyperGraph.SIZE.OBJECT() + hyperGraph.SIZE.padding_y();
					    }
					
					    r.DRAW_A_POSES.Add( hyperGraph.zero );
					}
					*/// OLD SINLE VALUE
				}
				
				return true;
				// objectDisplay.Value.DRAW_A_POSES = new Vector2[objectDisplay.Value.comps.Count + 1];
			}
			
			
			//internal bool Was_InitializeBroadcasting_InMainThead;
			/* internal void InitializeBroadcasting_InMainThead(Component reference_comp)
			 {
			     if (!reference_comp) return;
			     if (f.Length == 0) return;
			
			     // resilt.ComponentToBPosIndex.Add(currentComps[i], fieldsCount);
			     var needAdd = false;
			     var fds = new Dictionary<string, int>();
			     for (int fIndex = 0; fIndex < f.Length; fIndex++)
			     {
			         bool haveChange = false;
			         if (f[fIndex].FieldType == GameObjectType)
			         {
			             if ((GameObject)f[fIndex].GetValue(reference_comp) == CurrentSelection)
			             {
			                 findedList.Add(reference_comp,
			                     new FIELD_PARAMS(f[fIndex], CurrentSelection, null, fdsHotControl++));
			                 // activeComps.Add(currentComps[i], i);
			                 haveChange = true;
			             }
			         } else
			         {
			             var getComp = (Component)f[fIndex].GetValue(reference_comp);
			             if (getComp && compsSorted.ContainsKey(getComp.GetInstanceID()))
			             {
			                 findedList.Add(reference_comp,
			                     new FIELD_PARAMS(f[fIndex], null, getComp, fdsHotControl++));
			                 // activeComps.Add(currentComps[i], i);
			                 haveChange = true;
			             }
			         }
			         if (haveChange)
			         {
			             // fieldsCount++;
			             height += SIZES.VAR;
			             fds.Add(f[fIndex].Name, fdsIndex++);
			             // result.fields.Add(f[fIndex].Name, result.fields.Count);
			             needAdd = true;
			         }
			     }
			
			     if (needAdd)
			     {
			         result.AllFields.Add(currentComps[i], fds);
			         height += SIZES.COMP;
			     }
			 }
			*/
			
			
			
		}
		// [HostProtectionAttribute(SecurityAction.LinkDemand, Synchronization = true)]
		
		private FieldsAccessor GetReflectionFields( UnityEngine.Object comp, ObjectDisplay callbackobject = null )
		{
		
			if ( FiledsInfo == null ) FiledsInfo = new Dictionary<Type, FieldsAccessor>()
			{
				{ typeof(Transform), new FieldsAccessor(this, typeof(Transform)) {completed_thead = true, faList = new FieldAdapter[0]} },
				{ typeof(CanvasRenderer), new FieldsAccessor(this, typeof(CanvasRenderer)) {completed_thead = true, faList = new FieldAdapter[0]} }
			};
			
			// var n = comp.GetType().Name;
			// var n = GetTypeFullName(comp);
			var n = comp.GetType();
			
			if ( !FiledsInfo.ContainsKey( n ) )     // var result = new FieldsAccessor(comp.GetType());
			{	var result = new FieldsAccessor(this, Adapter.GetType_(comp));
			
				if ( callbackobject != null ) result.snannig_callback.Add( callbackobject );
				
				/*  lock ( scanQueue )
				  {   scanQueue.Enqueue( result );
				
				  }*/
				current_job.Push( result );
				FiledsInfo.Add( n, result );
				
				/*  if ( currentJob == null )
				      StarTJob();*/
				//if ( current_job.stopped) current_job.StarTJob();
			}
			
			else
			{	if ( callbackobject != null )
				{	lock ( FiledsInfo[n] )
					{	if ( !FiledsInfo[n].completed_thead ) FiledsInfo[n].snannig_callback.Add( callbackobject );
					}
				}
				
			}
			
			
			return FiledsInfo[n];
		}
		
		void mh_Finish()
		{	Repaint( WindowHyperController );
		}
		void mh_MainTheadInvoker()
		{
		
		}
		
		JobFactory _current_job;
		JobFactory current_job
		{	get
			{	if ( _current_job  == null )
				{	_current_job = new JobFactory();
					_current_job.Target = this;
				}
				
				return _current_job;
			}
		}
		class JobFactory {
		
			const int JOBCOUNT = 4;
			
			JobInstance[] _currentJob = Enumerable.Repeat( new JobInstance(), JOBCOUNT).ToArray();
			JobInstance[] currentJob { get { return _currentJob; } }
			
			int currentJobIndex = 0;
			internal HyperGraph Target;
			
			//internal bool stopped = true;
			/* internal StarTJob()
			 {
			
			 }*/
			
			internal void Push ( FieldsAccessor result )
			{	lock ( currentJob[currentJobIndex].scanQueue )
				{	currentJob[currentJobIndex].scanQueue.Enqueue( result );
					/* if ("MoodBoxManager" == result.type.Name)
					     MonoBehaviour.print(result.type);*/
				}
				
				StarTJob();
			}
			internal void StarTAllJobs()
			{	for ( int i = 0 ; i < JOBCOUNT ; i++ )
				{	StarTJob();
				}
			}
			internal bool stopped
			{	get
				{	for ( int i = 0 ; i < JOBCOUNT ; i++ )
					{	if ( _currentJob[0].wasInit ) return false;
					}
					
					return true;
				}
			}
			
			// static double? time__;
			// static System.Diagnostics.Stopwatch DIAG_WATCH_CLONE = null;
			
			
			void StarTJob()
			{
			
				/*  if ( DIAG_WATCH_CLONE == null )
				  {   DIAG_WATCH_CLONE = System.Diagnostics.Stopwatch.StartNew();
				      DIAG_WATCH_CLONE.Stop();
				      DIAG_WATCH_CLONE.Reset();
				      DIAG_WATCH_CLONE.Start();
				  }*/
				
				
				currentJobIndex++;
				
				if ( currentJobIndex >= currentJob.Length ) currentJobIndex = 0;
				
				if ( !currentJob[currentJobIndex].wasInit )
				{	Job tempJob = null;
				
					currentJob[currentJobIndex].wasInit = true;
					
					var inst = currentJob[currentJobIndex];
					tempJob = MultyThead.CreateJob( () =>
					{	if ( tempJob.RequestStop ) return;
					
						inst.mh_Action();
					}, Target.mh_Finish, Target.mh_MainTheadInvoker );
					
					inst.job = tempJob;
					inst.index = currentJobIndex;
					inst.Target = Target;
					
					
					
					tempJob.Start();
				}
			}
			
			class JobInstance {
				internal bool wasInit;
				internal   Job job;
				internal   int index;
				internal    HyperGraph. FieldsAccessor mh_tempAccessor;
				internal HyperGraph Target;
				internal   Queue<FieldsAccessor> scanQueue = new Queue<FieldsAccessor>();
				Type mh_tempType;
				
				
				int scanQueueCount
				{	get
					{	var res = 0;
					
						lock ( scanQueue ) { res = scanQueue.Count; }
						
						return res;
					}
				}
				
				internal   void mh_Action(  )
				{	try
					{	while ( scanQueueCount != 0 )
						{	if ( job.RequestStop )     // MonoBehaviour.print("ASD");
							{	return;
							}
							
							//  private static BindingFlags flags = ~BindingFlags.Static & ~BindingFlags.FlattenHierarchy;
							
							lock ( scanQueue ) mh_tempAccessor = scanQueue.Dequeue();
							
							/*  if ("MoodBoxManager" == mh_tempAccessor.type.Name)
							      MonoBehaviour.print(mh_tempAccessor.type); */
							//   BindingFlags.
							lock ( mh_tempAccessor )
							{	mh_tempType = mh_tempAccessor.type;
							}
							
							var t = mh_tempType;
							
							
							/*// OLD FIELDS
							fieldsList.Clear();
							while (t != null)
							{
							
							    if (!cache_fields.ContainsKey( t ))
							    {   cache_fields.Add( t, t.GetFields( flags ).Where( f =>
							        {   if (IsFieldReturnObject( f )) return true;
							
							            // if (!f.FieldType.IsSerializable) return false;
							
							            //  var ser = FindSerializableInSerializable(f);
							            return false;
							        } ).ToArray() );
							    }
							
							
							
							    fieldsList.AddRange( cache_fields[t] );
							
							    t = t.BaseType;
							}
							var result = fieldsList.ToArray();
							*/// OLD FIELDS
							
							/// NEW FIELDS
							// fieldsList.Clear();
							var result = GET_FIELDS( t, Target.adapter._S_HG_SkipArrays == 0   ).Values.ToArray();
							/*foreach ( var f in typeFields ) {
							    var allValues = f.Value.GetAllValues();
							
							}*/
							/// NEW FIELDS
							
							
							
							
							//  MonoBehaviour.print(mh_tempType.Name + " " + fields.Length);
							
							if ( job.RequestStop )     // currentJob = null;
							{	//  MonoBehaviour.print("ASD");
								return;
							}
							
							lock ( mh_tempAccessor )
							{	lock ( Target.SCANNING_COMPS )
								{	foreach ( var objectDisplay in mh_tempAccessor.snannig_callback )
									{	Target.SCANNING_COMPS.Add( objectDisplay );
									}
									
									mh_tempAccessor.faList = result;
									mh_tempAccessor.completed_thead = true;
								}
								
							}
						}
						
						/*lock ( currentJob )
						    currentJob[index] = null;*/
					}
					
					catch ( Exception ex )
					{	Target.adapter.logProxy.LogError( "HyperThead " + ex.Message + " " + ex.StackTrace );
					}
					
					lock ( this ) wasInit = false;
					
					/* if ( Target.current_job.stopped )
					 {   DIAG_WATCH_CLONE.Stop();
					     var res = DIAG_WATCH_CLONE.ElapsedTicks / (double)System.Diagnostics.Stopwatch.Frequency;
					     DIAG_WATCH_CLONE = null;
					     Debug.Log( res );
					 }*/
				}
			}
			
			
		}
		
		private void INIT_COMPS()
		{
		
		
			if ( adapter.IS_HIERARCHY() ) comps = HierarchyExtensions.Utilities.GetComponentFast<Component>.GetAll( CurrentSelection as GameObject ).Where( c => c ).ToArray();
			else comps = new Component[0];
			
			Array.Resize( ref compsinitialized, comps.Length );
			Array.Resize( ref sizememory, comps.Length + 1 );
			SEL_INC++;
			selection_id = CurrentSelection ? CurrentSelection.GetInstanceID() : -1;
			//GetAllValuesCache.Clear();
			compsSorted.Clear();
			
			for ( int i = 0 ; i < comps.Length ; i++ )
			{	compsSorted.Add( comps[i].GetInstanceID(), i );
				compsinitialized[i] = false;
			}
			
			if ( comps.Length + 1 != comps_inPos.Length ) Array.Resize( ref comps_inPos, comps.Length + 1 );
			
			//  repaintComps = true;
			//selectObject_height = SIZES.OBJECT();
			SelectObject_height = SIZE.OBJECT();
			// add_selectObject_height( SIZES.OBJECT() / CURRENT_SCALE );
			TARGET_HEIGHT = 0;
			
			
			
			
			TARGET_COMPS.Clear();
			// SELF_TARGETS.Clear();
			PTR.Clear();
			
			// int interator = 0;
			for ( int i = 0 ; i < comps.Length ; i++ )
			{	if ( !comps[i] ) continue;
			
				GetReflectionFields( comps[i] );
				// selectObject_height += SIZES.SPACE() + SIZES.COMP();
				SelectObject_height += SIZE.SPACE() + SIZE.COMP();
				
				
			}
			
			//ConcurrentDictionary
			/*   foreach (var objectDisplay in TARGET_COMPS)
			   {
			       objectDisplay.Value.DRAW_A_POSES = new Vector2[objectDisplay.Value.comps.Count + 1];
			   }*/
			
			
			
			
			
			
		}
		
		private void TOOLTIP( Rect r, GUIContent content )
		{	tootipContent.tooltip = content.tooltip;
			LABEL( r, tootipContent );
		}
		
		internal SIZES_CLASS SIZE;
		
		internal class SIZES_CLASS {
#pragma warning disable
			Adapter adapter;
#pragma warning restore
			internal SIZES_CLASS( Adapter adapter )
			{	this.adapter = adapter;
			}
			/* internal  int OBJECT = 19;
			 internal  int COMP = 12;
			 internal  int VAR = 7;
			 internal  int padding = 11;*/
			
			public float OBJECT()
			{	return 19;
				// return 19 * adapter.bottomInterface.hyperGraph.CURRENT_SCALE;
			}
			
			internal float COMP()
			{	return 12;
				//  return 12 * adapter.bottomInterface.hyperGraph.CURRENT_SCALE;
			}
			internal float ARROW_WIDTH()
			{	return 10;
				// return 10 * adapter.bottomInterface.hyperGraph.CURRENT_SCALE;
			}
			
			internal float VAR()
			{	return 7;
				// return 7 * adapter.bottomInterface.hyperGraph.CURRENT_SCALE;
			}
			
			internal float padding_y()
			{	return 11;
				// return 11 * adapter.bottomInterface.hyperGraph.CURRENT_SCALE;
			}
			
			internal float SPACE()
			{	return 2;
				// return 2 * adapter.bottomInterface.hyperGraph.CURRENT_SCALE;
			}
			
			internal float DEFAULT_PAD()
			{	return 20;
				// return 20 * adapter.bottomInterface.hyperGraph.CURRENT_SCALE;
			}
			/* private const int DEFAULT_PAD = 20;
			private const int SPACE = 2;*/
		}
		
		
		float HALF_SCALE()
		{	return (CURRENT_SCALE - 1) / 2 + 1;
		}
		void Reset()
		{	adapter.OneFrameActionOnUpdateAC = () => { comps = null; };
			adapter.OneFrameActionOnUpdate = true;
		}
		
		
		
		
		
		Vector2 _mConvertRect( Vector2 r )
		{	/* r.x = r.x / CURRENT_SCALE - CURRENT_CONTROLLER.scrollPos.x;
			     r.y = r.y / CURRENT_SCALE - CURRENT_CONTROLLER.scrollPos.y;*/
			/* r.x = r.x * CURRENT_SCALE + CURRENT_CONTROLLER.scrollPos.x;
			 r.y = r.y * CURRENT_SCALE + CURRENT_CONTROLLER.scrollPos.y;*/
			/*r.width *= CURRENT_SCALE;
			r.height *= CURRENT_SCALE;*/
			return r;
		}
		
		void _mConvertRect( ref Rect r )
		{	r.x = r.x * CURRENT_SCALE + CURRENT_CONTROLLER.scrollPos.x;
			r.y = r.y * CURRENT_SCALE + CURRENT_CONTROLLER.scrollPos.y;
			r.width *= CURRENT_SCALE;
			r.height *= CURRENT_SCALE;
		}
		void _mConvertRect_Unscalable( ref Rect r )
		{	r.x = r.x * CURRENT_SCALE + CURRENT_CONTROLLER.scrollPos.x;
			r.y = r.y * CURRENT_SCALE + CURRENT_CONTROLLER.scrollPos.y;
		}
		GUIContent LABEL_content = new GUIContent();
		void LABEL( Rect rect, string content )
		{	LABEL_content.text = content;
			LABEL( rect, LABEL_content );
		}
		void LABEL( Rect rect, GUIContent content )
		{	_mConvertRect( ref rect );
			Label( rect, content );
		}
		void Draw( Rect rect, GUIStyle style, bool b1, bool b2, bool b3, bool b4 )
		{	LABEL_content.text = null;
			Draw( rect, LABEL_content, style, b1, b2, b3, b4 );
		}
		void Draw( Rect rect, GUIContent content, GUIStyle style, bool b1, bool b2, bool b3, bool b4 )
		{	_mConvertRect( ref rect );
			style.Draw( rect, content, b1, b2, b3, b4 );
		}
		void DrawTexture_Unscalable( Rect point, Rect ScreenRect, Color color )
		{	_mConvertRect_Unscalable( ref point );
			Graphics.DrawTexture( point, Texture2D.whiteTexture, ScreenRect, 0, 0, 0, 0, color );
		}
		
		
		void GL_VERTEX3(Vector3 r)
		{	r.x = r.x * CURRENT_SCALE + CURRENT_CONTROLLER.scrollPos.x;
			r.y = r.y * CURRENT_SCALE + CURRENT_CONTROLLER.scrollPos.y;
			GL.Vertex3(r.x, r.y, 0);
		}
		
		
		
		
		
		
		
		
		
		
		//  int fieldsCount = 0;
		private float DRAWOBJECT( BottomInterface.UniversalGraphController controller, bool calculate = false )
		{	/* controller.scrollPos_.x
			      controller.scrollPos.y;*/
			moduleRect.x = -DEFAULTWIDTH( controller ) / 2;
			moduleRect.y = 0;
			moduleRect.width = DEFAULTWIDTH( controller );
			moduleRect.height = EditorGUIUtility.singleLineHeight;
			
			/*  MonoBehaviour.print(Event.current.type);
			  DragAndDrop.*/
			// MonoBehaviour.print(HIPER_HEIGHT());
			if ( CurrentSelection == null )
			{	if ( !calculate ) LABEL( moduleRect, "No GameObjects" );
			
				//GUI .Label( moduleRect, "No GameObjects" );
				//selectObject_height = moduleRect.height;
				SelectObject_height = moduleRect.height;
				TARGET_COMPS.Clear();
				//SELF_TARGETS.Clear();
				return SelectObject_height;
				// fieldsCount = 0;
			}
			
			if ( comps == null )
			{	INIT_COMPS();
				//  repaintComps = true;
			}
			
			/*  if (currentJob != null)
			  {
			      moduleRect.height = EditorGUIUtility.singleLineHeight;
			      if (!calculate) GUI .Label(moduleRect, "Updating...");
			      height = moduleRect.height;
			      return height;
			      // fieldsCount = 0;
			  }*/
			
			{	//  bool repaintComps = false;
			
				/*  if (repaintComps)
				  {
				      height = 19;
				      fieldsCount = 0;
				      for (int i = 0; i < comps.Length; i++)
				      {
				          var f = GetReflectionFields(comps[i]);
				          if (f.Count == 0) continue;
				          height += SPACE + 12;
				          for (int fIndex = 0; fIndex < f.Count; fIndex++)
				          {
				              height += SPACE + 7;
				              fieldsCount++;
				          }
				      }
				  }*/
				if ( calculate ) return SelectObject_height;
				
				
				
				var R = moduleRect;
				R.x -= 1;
				R.width = 5;
				R.height = SelectObject_height;
				
				// DEBUG_HORISONTAL(scrollPos.y,Color.red);
				if ( Event.current.type == EventType.Repaint )
					Draw( R, HIPERUI_LINE_BOX, false, false, false, false );
					
				foreach ( var objectDisplay in INPUT_COMPS )
				{	objectDisplay.Value.DRAW = false;
				}
				
				foreach ( var objectDisplay in TARGET_COMPS )
				{	objectDisplay.Value.DRAW = false;
				}
				
				moduleRect.height = SIZE.OBJECT();
				moduleRect.width = DEFAULTWIDTH( controller );
				///////////////GAMEOBJECT
				content.text = CurrentSelection.name;
				content.tooltip = content.text + " - GAMEOBJECT";
				HIPERUI_GAMEOBJECT.fontSize = Mathf.RoundToInt( adapter.FONT_8() * HALF_SCALE() );
				DO_BUTTON_FORSCROLL( controller, moduleRect, content, HIPERUI_GAMEOBJECT, 15, CurrentSelection );
				GAMEOBJECTRECT = moduleRect;
				GAMEOBJECTRECT.height = SelectObject_height - moduleRect.height;
				GAMEOBJECTRECT.y += moduleRect.height;
				
				/*  DEBUG_HORISONTAL(GAMEOBJECTRECT.y, Color.red);
				  DEBUG_HORISONTAL(GAMEOBJECTRECT.y + GAMEOBJECTRECT.height * 0.5f, Color.blue);*/
				
				WRITE_ARROW_A_POSES( moduleRect, comps.Length );
				sizememory[comps.Length] = moduleRect.width;
				DO_ARROW_A( moduleRect, comps.Length );
				///////////////GAMEOBJECT//////////////////////////////////////////////////
				moduleRect.y += moduleRect.height + SIZE.SPACE();
				DRAWING_INDEX = 0;
				TARGET_CURRENT_Y = 0;
				var mre = moduleRect.y;
				
				for ( int i = 0 ; i < comps.Length ; i++ )
				{	if ( !comps[i] )
					{	Reset();
						continue;
					}
					
					var accessor = GetReflectionFields(comps[i]);
					
					if ( !accessor.completed_thead ) continue;
					
					if ( !compsinitialized[i] )
					{	if ( accessor.InitializeTarget_InMainThead( (comps[i]) ) )
							compsinitialized[i] = true;
					}
					
					moduleRect.height = SIZE.COMP();
					WRITE_ARROW_A_POSES( moduleRect, i );
					moduleRect.y += moduleRect.height + SIZE.SPACE();
					
					for ( int fIndex = 0 ; fIndex < accessor.faList.Length ; fIndex++ )
					{	moduleRect.height = SIZE.VAR();
						moduleRect.y += moduleRect.height + SIZE.SPACE();
					}
				}
				
				
				moduleRect.y = mre;
				drawIndex = 0;
				
				for ( int i = 0 ; i < comps.Length ; i++ )
				{	if ( !comps[i] )
					{	Reset();
						continue;
					}
					
					var accessor = GetReflectionFields(comps[i]);
					
					if ( !accessor.completed_thead ) continue;
					
					if ( !compsinitialized[i] )
					{	if ( accessor.InitializeTarget_InMainThead( (comps[i]) ) )
							compsinitialized[i] = true;
					}
					
					//if (f.Count == 0) continue;
					moduleRect.height = SIZE.COMP();
					
					////////////////COMPONENT
					content.text = GetTypeName( comps[i] );
					content.tooltip = content.text + " : Component";
					HIPERUI_LINE_RDTRIANGLE.fontSize = Mathf.RoundToInt( (adapter.FONT_8() - 1) * HALF_SCALE() );
					sizememory[i] = HIPERUI_LINE_RDTRIANGLE.CalcSize( content ).x / CURRENT_SCALE;
					sizememory[i] = moduleRect.width = Math.Min( sizememory[i] + 10, DEFAULTWIDTH( controller ) );
					
					if ( Event.current.type == EventType.Repaint )
					{	/* var ccc = GUI.color;
						     if (!EditorGUIUtility.isProSkin)GUI.color *= alpha;*/
						Draw( moduleRect, content, HIPERUI_LINE_RDTRIANGLE, false, false, false, false );
						// GUI.color = ccc;
					}
					
					TOOLTIP( moduleRect, content );
					
					DO_ARROW_A( moduleRect, i );
					////////////////COMPONENT/////////////////////////////////////////////////
					moduleRect.y += moduleRect.height + SIZE.SPACE();
					
					for ( int fIndex = 0 ; fIndex < accessor.faList.Length ; fIndex++ )
					{	var value = accessor.faList[fIndex].GetValue(comps[i]) /*as UnityEngine.Object*/;
						// var asd = GUI.contentColor;
						bool isnull = Adapter.IsObjectNull(value);
						/* if (isnull)
						 {   GUI.contentColor = adapter.par.HiperGraphParams.RED_HIGKLIGHTING ? Color.red : alpha;
						 }*/
						
						// MonoBehaviour.print(value);
						moduleRect.height = SIZE.VAR();
						var lineRect = moduleRect;
						lineRect.x += 6;
						lineRect.width = lineRect.height;
						
						
						if ( Event.current.type == EventType.Repaint )
						{	var asdas = GUI.color;
						
							if ( isnull ) GUI.color *= adapter.par.HiperGraphParams.RED_HIGKLIGHTING ? Color.red : alpha;
							
							Draw( lineRect, HIPERUI_MARKER_BOX, false, false, false, false );
							GUI.color = asdas;
						}
						
						lineRect.x += lineRect.width;
						lineRect.width = DEFAULTWIDTH( controller ) - 6 - lineRect.width;
						
						// content.text = value == null ? "null" : value
						///////////////FIELD
						content.text = accessor.faList[fIndex].Name;
						content.tooltip = "var " + content.text;
						HIPERUI_LINE_BLUEGB_PERSONAL.fontSize = HIPERUI_LINE_BLUEGB.fontSize = Mathf.RoundToInt( (adapter.FONT_8() - 1) * HALF_SCALE() );
						
						//  HIPERUI_LINE_BLUEGB_PERSONAL.fontSize = FONT_8() - 1;
						if ( Event.current.type == EventType.Repaint )
						{	if ( adapter.par.HiperGraphParams.RED_HIGKLIGHTING || EditorGUIUtility.isProSkin ) Draw( lineRect, content, HIPERUI_LINE_BLUEGB, false, false, false, false );
							else Draw( lineRect, content, HIPERUI_LINE_BLUEGB_PERSONAL, false, false, false, false );
						}
						
						TOOLTIP( lineRect, content );
						
						
						// if (SELF_TARGETS.ContainsKey()
						accessor.faList[fIndex].CheckID(selection_id, SEL_INC);
						
						if (accessor.faList[fIndex].GetAllValuesCache == null || !accessor.faList[fIndex].GetAllValuesCache.ContainsKey(comps[i].GetInstanceID() ))
						{	if (accessor.faList[fIndex].GetAllValuesCache == null) accessor.faList[fIndex].GetAllValuesCache = new Dictionary<int, Dictionary<string, object>>();
						
						
							accessor.faList[fIndex].GetAllValuesCache.Add(comps[i].GetInstanceID(), accessor.faList[fIndex].GetAllValues( comps[i], 0, adapter._S_HG_EventsMode | adapter._S_HG_SkipArrays ));
						}
						
						foreach ( var item in accessor.faList[fIndex].GetAllValuesCache[comps[i].GetInstanceID() ] )
						{	var v = item.Value as UnityEngine.Object;
						
							if ( !v ) continue;
							
							DO_ARROW_B( controller, lineRect, v, fIndex );
						}
						
						/*// OLD SINLE VALUE
						    DO_ARROW_B( controller , lineRect , value , fIndex );
						*/// OLD SINLE VALUE
						///////////////FIELD/////////////////////////////////////////////////
						
						
						
						moduleRect.y += moduleRect.height + SIZE.SPACE();
						
						//GUI.contentColor = asd;
					}
				}
				
				DRAWARROWED_BEZIER_COMPOLETE();
				
				DRAWINPUTS( controller );
				
				
				//107 19 head
				//5 12 line box bg
				
				//10 12 arrow
				//18 7 line blue bg
				//39 12 line
				//7 7 marker
			}
			
			
			return SelectObject_height;
		}
		
		
		int CURRENT_COMP_INDEX;
		
		Rect? stateForDrag_B0;
		Object stateForDrag_B1;
		
		void rawOnUp()
		{	stateForDrag_B0 = null;
			stateForDrag_B1 = null;
		}
		private void DO_BUTTON_FORSCROLL( BottomInterface.UniversalGraphController controller, Rect rect, GUIContent content, GUIStyle style, int EVENT_ID,
		                                  Object newGo, Action action = null )
		{	_mConvertRect( ref rect );
		
			EditorGUIUtility.AddCursorRect( rect, MouseCursor.Link );
			
			if ( Event.current.type == EventType.Repaint )
			{	style.Draw( rect, content, false, false, false, false );
				TOOLTIP( rect, content );
				
				if ( HOVER( EVENT_ID, rect, controller ) )
				{	var h = rect;
					var GLOW = 8;
					h.x -= GLOW;
					h.y -= GLOW;
					h.width += GLOW * 2;
					h.height += GLOW * 2;
					
					
					HIPERUI_BUTTONGLOW.Draw( h, false, false, false, false );
				}
			}
			
			
			
			/* ------ */
			if ( newGo )
			{	if ( Event.current.rawType == EventType.MouseUp )
				{	rawOnUp();
				}
				
				//  if (Event.current.type == EventType.MouseDrag) Debug.Log( stateForDrag_B1 == newGo );
				//  if (stateForDrag_B0.HasValue ) adapter.RepaintWindowInUpdate();
				if ( stateForDrag_B0.HasValue )
				{	var m =  Event.current.mousePosition + Event.current.delta;
					var drag =  !stateForDrag_B0.Value.Contains( m );
					drag |= m.x < 3;
					drag |= m.x > controller.WIDTH - 9;
					
					// Debug.Log( m.x + "  " + controller.WIDTH );
					if ( stateForDrag_B1 == newGo && Event.current.type == EventType.MouseDrag && drag )     // DragAndDrop.PrepareStartDrag();// reset data
					{	adapter.InternalClearDrag();
						DragAndDrop.objectReferences = new[] { (UnityEngine.Object)newGo };
						// drawComps = emptyArr;
						DragAndDrop.StartDrag( "Dragging HyperGraph Object" );
						DragAndDrop.SetGenericData( "Dragging HyperGraph", true );
						// DragAndDrop.
						//  EventUse();
						stateForDrag_B0 = null;
						adapter.RepaintWindowInUpdate();
						EventUse();
					}
				}
				
				if ( Event.current.type == EventType.MouseDown && rect.Contains( Event.current.mousePosition ) )
				{
				
					if ( Event.current.button == 0 )
					{
					
						if (!stateForDrag_B0.HasValue )
						{	if ( controller.tempWin == adapter.window() )
							{	adapter.PUSH_ONMOUSEUP( rawOnUp );
							}
							
							else
							{	var w = controller.tempWin as _6__BottomWindow_HyperGraphWindow;
							
								if ( w ) w.PUSH_ONMOUSEUP( rawOnUp );
							}
						}
						
						stateForDrag_B0 = rect;
						stateForDrag_B1 = newGo;
					}
					
				}
			}
			
			/* ------ */
			
			
			
			
			
			
			
			if ( Event.current.type == EventType.MouseDown && Event.current.button == 0 &&
			        rect.Contains( Event.current.mousePosition ) )
			{	EventUse();
				ADD_ACTION( EVENT_ID, rect, contains =>
				{
				
					return false;
				}, contains =>
				{	if ( contains )
					{	if ( newGo != null )
						{	Selection.objects = new Object[] { newGo };
						}
						
						if ( action != null )
						{	action();
						}
					}
				}, controller );
			}
		}
		
		private void SETUNDO()
		{	if ( UndoList.Count != 0 && UndoList[UndoList.Count - 1] == CurrentSelection ) return;
		
			if ( UndoPos < UndoList.Count - 1 ) UndoList.RemoveRange( UndoPos + 1, UndoList.Count - UndoPos - 1 );
			
			/* if (UndoList.Count == 0) UndoList.Add(CurrentSelection);
			 else UndoList.Insert(0, CurrentSelection);*/
			
			UndoList.Add( CurrentSelection );
			
			if ( UndoList.Count > adapter.MAX20 )
			{	while ( UndoList.Count > adapter.MAX20 ) UndoList.RemoveAt( 0 );
			}
			
			UndoPos = UndoList.Count - 1;
		}
		
		private void DO_REDO()
		{	if ( UndoPos >= UndoList.Count - 1 ) return;
		
			UndoPos++;
			// Selection.objects = new[] { UndoList[UndoPos] };
			skipUndo = true;
			// CHANGE_SELECTION_OVVERIDE(true);
			var n = UndoList[UndoPos];
			CHANGE_SELECTION_OVVERIDE( true, n );
		}
		
		private void DO_UNDO()
		{	if ( UndoPos <= 0 ) return;
		
			UndoPos--;
			// Selection.objects = new[] { UndoList[UndoPos] };
			skipUndo = true;
			
			var n = UndoList[UndoPos];
			CHANGE_SELECTION_OVVERIDE( true, n );
			// CHANGE_SELECTION_OVVERIDE(true);
		}
		
		
		void WRITE_ARROW_A_POSES( Rect lineRect, int index )
		{	lineRect.x -= 2 + SIZE.ARROW_WIDTH();
			lineRect.y = lineRect.y + lineRect.height / 2 - 6;
			lineRect.width = SIZE.ARROW_WIDTH();
			lineRect.height = SIZE.COMP();
			
			p1.x = lineRect.x + 3;
			p1.y = lineRect.y + lineRect.height / 2;
			comps_inPos[index] = p1;
		}
		
		private void DO_ARROW_A( Rect lineRect /*, UnityEngine.Object source*/, int index = -1 )
		{	lineRect.x -= 2 + SIZE.ARROW_WIDTH();
			lineRect.y = lineRect.y + lineRect.height / 2 - 6;
			lineRect.width = SIZE.ARROW_WIDTH();
			lineRect.height = SIZE.COMP();
			
			if ( Event.current.type == EventType.Repaint )
				Draw( lineRect, HIPERUI_INOUT_A, false, false, false, false );
				
			if ( index != -1 )
			{	/*  p1.x = lineRect.x + 3;
				      p1.y = lineRect.y + lineRect.height / 2;
				      comps_inPos[index] = p1;*/
				
				if ( SKANNING )
				{	lineRect.x -= SIZE.DEFAULT_PAD();
					lineRect.width = SIZE.DEFAULT_PAD();
					/*var asd = GUI.color;
					GUI.color = Color.yellow;*/
					LABEL( lineRect, scanningContent );
					//GUI.color = asd;
				}
				
				// if (source != null)
			}
		}
		
		
		
		
		private void DO_ARROW_B( BottomInterface.UniversalGraphController controller, Rect lineRect, UnityEngine.Object target, int index)
		{
		
			//if ( adapter._S_HG_EventsMode && !(target is UnityEngine.Events.UnityEvent) ) return;
			
			lineRect.x += lineRect.width;
			lineRect.y -= 2;
			lineRect.width = SIZE.ARROW_WIDTH();
			lineRect.height = SIZE.COMP();
			
			var asd = GUI.color;
			
			if ( target == null && index != -1 ) GUI.color *= alpha;
			
			if ( Event.current.type == EventType.Repaint )
				Draw( lineRect, HIPERUI_INOUT_B, false, false, false, false );
				
			GUI.color = asd;
			
			
			/*   MonoBehaviour.print(target);
			   MonoBehaviour.print(target as Component);
			   MonoBehaviour.print(target as GameObject);
			   MonoBehaviour.print(Equals( target , null));*/
			if ( target != null )     // var FH = 35 + 4;
			{	var fieldsHeight = TARGET_HEIGHT;
				var mid = GAMEOBJECTRECT.y + GAMEOBJECTRECT.height * 0.5f;
				
				p1.x = lineRect.x + lineRect.width + 5;
				p1.y = lineRect.y + lineRect.height / 2;
				
				p2.x = p1.x + SIZE.DEFAULT_PAD();
				p2.y = mid - fieldsHeight / 2 + TARGET_CURRENT_Y;
				
				/* DEBUG_HORISONTAL(mid, Color.blue);
				 DEBUG_HORISONTAL(p2.y, Color.red);*/
				//   MonoBehaviour.print(index + " " + fieldsCount);
				GRAPH_ARGS _a;
				var targetPoint = DRAWSINGLECOMP(controller, p2, target, DRAWING_INDEX, true, out _a);
				
				if ( _a.isAsset && !adapter._S_HG_ShowAssets ) return;
				
				DRAWING_INDEX++;
				
				
				targetPoint.x += 2;
				
				if ( !_a.isBezier )
				{	DRAWARROWED_LINE( p1, targetPoint );
				
					p2 = p1;
					p2.x -= 5;
					DRAWLINE( p1, p2 );
				}
				
				else
				{	p1.x -= 9;
					// targetPoint.x += sizememory.x;
					// p1.x = targetPoint.x + 15;
					DRAWARROWED_BEZIER( CURRENT_COMP_INDEX, p1, targetPoint );
				}
				
				/* p1.y -= 2;
				 targetPoint.y -= 2;
				 DRAWARROWED_LINE(p1, targetPoint);*/
				
				
			}
			
			else       // lineRect.x += lineRect.width;
			{	/*  lineRect.y -= 1;
				  lineRect.height += 2;
				  lineRect.width = 50;
				  content.text = "X";
				  var fs = Adapter.GET_SKIN().label.fontSize;
				  var a = Adapter.GET_SKIN().label.alignment;
				  var fb = Adapter.GET_SKIN().label.fontStyle;
				  Adapter.GET_SKIN().label.fontSize = HIPERUI_LINE_BLUEGB.fontSize;
				  Adapter.GET_SKIN().label.alignment = TextAnchor.MiddleLeft;
				  Adapter.GET_SKIN().label.fontStyle = FontStyle.Bold;
				  if (Event.current.type == EventType.repaint)
				      Adapter.GET_SKIN().label. Draw(lineRect, content, false, false, false, false);
				  Adapter.GET_SKIN().label.fontSize = fs;
				  Adapter.GET_SKIN().label.alignment = a;
				  Adapter.GET_SKIN().label.fontStyle = fb;*/
			}
		}
		float[] sizememory;
		//	bool bezier = false;
		//	bool isAsset;
		private Vector2 DRAWSINGLECOMP( BottomInterface.UniversalGraphController controller, Vector2 startPos, Object target, int index, bool DRAW_A, out GRAPH_ARGS _a,
		                                object fieldParams = null )
		{	singleRect.x = startPos.x + 12;
			singleRect.y = startPos.y - 7;
			singleRect.width = DEFAULTWIDTH( controller );
			
			
			Component component = target as Component;
			GameObject gameObject = component != null ? component.gameObject : target as GameObject;
			// if (!go) continue;
			
			
			var isPrefab = gameObject && !gameObject.scene.IsValid();
			_a.isAsset = !gameObject;
			_a.isBezier = false;
			
			singleRect.height = SIZE.OBJECT();
			content.text = target.name;
			
			
			if ( _a.isAsset )
			{	if ( !adapter._S_HG_ShowAssets ) return startPos;
			
				var type = target.GetType().Name;
				content.tooltip = type.ToUpper() + ": " + content.text;
				content.text = "(" + type.ToLower() + ")" + content.text;
				
				component = null;
			}
			
			else
				if ( isPrefab )
				{	content.tooltip = "PREFAB: " + content.text;
					content.text = "(prefab)" + content.text;
				}
				
				else
				{	content.tooltip = content.text + " - GAMEOBJECT";
				}
				
			if ( gameObject ) target = gameObject;
			
			var ID = target.GetInstanceID();
			
			// if (isAsset) Debug.Log(DRAW_A);
			
			HIPERUI_GAMEOBJECT.fontSize = Mathf.RoundToInt( (adapter.FONT_8() - 1) * HALF_SCALE() );
			
			_a. isBezier = false;
			
			if ( DRAW_A )
			{	if ( !TARGET_COMPS.ContainsKey( ID ) )
				{	//if ( isAsset ) Debug.Log( DRAW_A );
					return startPos;
				}
				
				var dis = TARGET_COMPS[ID];
				
				if ( target == CurrentSelection )
				{	CURRENT_COMP_INDEX = component != null ? compsSorted[component.GetInstanceID()] : compsSorted.Count;
					_a.isBezier = true;
					return comps_inPos[CURRENT_COMP_INDEX];
					/* DRAW_B_POS
					 return comp != null ? dis.DRAW_A_POSES[dis.objecComps[comp.GetInstanceID()]] : dis.DRAW_A_POSES[dis.objecComps.Count];*/
					
				}
				
				else
					if ( !dis.DRAW )
					{	dis.DRAW = true;
					
						var asd = GUI.color;
						
						if ( adapter._S_HG_EventsMode != 0 ) GUI.color *= event_color;
						else
							if ( isPrefab ) GUI.color *= prefab_color;
							else
								if ( _a.isAsset ) GUI.color *= asset_color;
								
						DO_BUTTON_FORSCROLL( controller, singleRect, content, HIPERUI_GAMEOBJECT, DRAWSINGLECOMP_TARGET_EVENT_START + index,
						                     target );
						GUI.color = asd;
						
						DO_ARROW_A( singleRect );
						ARROW_RECT = singleRect;
						ARROW_RECT.y += ARROW_RECT.height / 2;
						ARROW_RECT.x -= 2 + SIZE.ARROW_WIDTH();
						DRAWSINGLECOMP_RESULT.Set( ARROW_RECT.x /*+ 5*/, ARROW_RECT.y );
						dis.DRAW_A_POSES[dis.objecComps.Count] = DRAWSINGLECOMP_RESULT;
						
						
						
						foreach ( var i in dis.objecComps )     //  }
						{	// if (comp != null && !prefab)
							// {
							singleRect.y += singleRect.height;
							singleRect.height = SIZE.COMP();
							var ob = EditorUtility.InstanceIDToObject(i.Key);
							
							if ( !ob ) continue;
							
							content.text = GetTypeName( ob );
							content.tooltip = content.text + " : Component";
							HIPERUI_LINE_RDTRIANGLE.fontSize = Mathf.RoundToInt( (adapter.FONT_8() - 1) * HALF_SCALE() );
							var size = HIPERUI_LINE_RDTRIANGLE.CalcSize(content);
							size.x = singleRect.width = Math.Min( size.x + 10, DEFAULTWIDTH( controller ) );
							DO_BUTTON_FORSCROLL( controller, singleRect, content, HIPERUI_LINE_RDTRIANGLE,
							                     DRAWSINGLECOMP_TARGET_EVENT_M2 + index, target );
							DO_ARROW_A( singleRect );
							
							
							ARROW_RECT = singleRect;
							ARROW_RECT.y += ARROW_RECT.height / 2;
							ARROW_RECT.x -= 2 + SIZE.ARROW_WIDTH();
							DRAWSINGLECOMP_RESULT.Set( ARROW_RECT.x /*+ 5*/, ARROW_RECT.y );
							dis.DRAW_A_POSES[i.Value] = DRAWSINGLECOMP_RESULT;
						}
						
						
						TARGET_CURRENT_Y += dis.height;
					}
					
					
				return component != null ? dis.DRAW_A_POSES[dis.objecComps[component.GetInstanceID()]] : dis.DRAW_A_POSES[dis.objecComps.Count];
			}
			
			
			// DRAW_B /////////////
			{	var dis = INPUT_COMPS[ID];
			
				if ( !dis.DRAW )
				{	dis.DRAW = true;
				
					DO_BUTTON_FORSCROLL( controller, singleRect, content, HIPERUI_GAMEOBJECT, DRAWSINGLECOMP_TARGET_EVENT_START + index,
					                     target );
					                     
					                     
					foreach ( var CompFields in dis.AllFields )
					{	singleRect.y += singleRect.height;
						singleRect.height = SIZE.COMP();
						var ob = EditorUtility.InstanceIDToObject(CompFields.Key);
						
						if ( !ob ) continue;
						
						content.text = GetTypeName( ob );
						content.tooltip = content.text + " : Component";
						HIPERUI_LINE_RDTRIANGLE.fontSize = Mathf.RoundToInt( (adapter.FONT_8() - 1) * HALF_SCALE() );
						var size = HIPERUI_LINE_RDTRIANGLE.CalcSize(content);
						size.x = singleRect.width = Math.Min( size.x + 10, DEFAULTWIDTH( controller ) );
						DO_BUTTON_FORSCROLL( controller, singleRect, content, HIPERUI_LINE_RDTRIANGLE,
						                     DRAWSINGLECOMP_TARGET_EVENT_M2 + index, target );
						// DO_ARROW_B(singleRect, null, -1);
						
						
						foreach ( var field1 in CompFields.Value )
						{	singleRect.y += singleRect.height;
							singleRect.width = DEFAULTWIDTH( controller );
							singleRect.height = SIZE.VAR();
							
							content.text = field1.Key;
							content.tooltip = "var " + content.text;
							HIPERUI_LINE_BLUEGB_PERSONAL.fontSize = HIPERUI_LINE_BLUEGB.fontSize = Mathf.RoundToInt( (adapter.FONT_8() - 1) * HALF_SCALE() );
							
							// HIPERUI_LINE_BLUEGB.fontSize = FONT_8() - 1;
							if ( Event.current.type == EventType.Repaint )
							{	if ( adapter.par.HiperGraphParams.RED_HIGKLIGHTING || EditorGUIUtility.isProSkin ) Draw( singleRect, content, HIPERUI_LINE_BLUEGB, false, false, false, false );
								else Draw( singleRect, content, HIPERUI_LINE_BLUEGB_PERSONAL, false, false, false, false );
								
								// HIPERUI_LINE_BLUEGB. Draw(singleRect, content, false, false, false, false);
							}
							
							TOOLTIP( singleRect, content );
							// GUI .Label(singleRect, content);
							DO_ARROW_B( controller, singleRect, null, -1);
							
							
							ARROW_RECT = singleRect;
							ARROW_RECT.y += ARROW_RECT.height / 2;
							ARROW_RECT.x += ARROW_RECT.width + 12 + 3;
							DRAWSINGLECOMP_RESULT.Set( ARROW_RECT.x /*+ 5*/, ARROW_RECT.y );
							dis.DRAW_B_POSES[field1.Value] = DRAWSINGLECOMP_RESULT;
						}
					}
					
					INPUT_CURRENT_Y += dis.height;
				}
				
				var FP = (FIELD_PARAMS)fieldParams;
				
				
				if ( _a.isAsset ) return startPos;
				
				return dis.DRAW_B_POSES[dis.AllFields[component.GetInstanceID()][FP.field.Name]];
				// return dis;
			}
		}
		
		private void DRAWARROWED_LINE( Vector2 p1, Vector2 p2, Color? col = null )
		{	if ( Event.current.type != EventType.Repaint || p1 == p2 ) return;
		
			/*  p1 = EditorGUIUtility.GUIToScreenPoint(p1);
			  p2 = EditorGUIUtility.GUIToScreenPoint(p2);*/
			/* p1.x += RECT.x;
			 p1.y += RECT.y;*/
			GL_BEGIN();
			
			GL.Color(col ??  (EditorGUIUtility.isProSkin ? arrowC1 : arrowC1personal));
			//	mat.SetColor(adapter._Color, col ?? Color.white);
			GL_VERTEX3(p1);
			GL_VERTEX3(p2);
			//GL.GL_VERTEX3Vertex3(p2.x, p2.y - 0.1f, 1);
			//GL.GL_VERTEX3Vertex3(p1.x-0.1f, p1.y, 0);
			//DRAWLINE( p1, p2, col );
			
			m_DrawArrow( p1, p2, col );
			GL_END();
			
			/*var ARROW_SIZE = 10;
			var arr = (p1 - p2).normalized * ARROW_SIZE;
			var a1 = arr;
			a1.y = -a1.x;
			a1.x = arr.y;
			var a2 = -a1;
			
			
			var s1 = Vector2.Lerp(arr, a1, 0.2f) + p2;
			var s2 = Vector2.Lerp(arr, a2, 0.2f) + p2;
			var len = ARROW_SIZE /#1# 2#1#;
			//  MonoBehaviour.print( p2);
			var c = col ?? (EditorGUIUtility.isProSkin ? arrowC1 : arrowC1personal);
			for (float i = 0; i < len - 1; i += 0.5f)
			{
			    var t1 = Vector2.Lerp(s1, p2, i / (len - 1f));
			    var t2 = Vector2.Lerp(s2, p2, i / (len - 1f));
			    DRAWLINE(t1, t2, c);
			}*/
			
			//GL.PopMatrix();
			
			/*  var matrixBackup = GUI.matrix;
			  var n = (p1 - p2).normalized;
			  var dot = Vector2.Dot(Vector2.down, n) < 0 ? -1 : 1;
			
			
			  Graphics.DrawTexture(
			  GUIUtility.RotateAroundPivot(dot * Vector2.Angle(Vector2.left, n), p2);
			  var dist = (p1 - p2).magnitude;
			  arrowRect.x = p2.x - dist;
			  arrowRect.width = dist;
			  arrowRect.height = 10;
			  arrowRect.y = p2.y - 5;
			  ARROW. Draw(arrowRect, false, false, false, false);
			  GUI.matrix = matrixBackup;*/
		}
		
		
		int[] COMPIND = new int[100];
		Vector2[] BA0 = new Vector2[100];
		Vector2[] BA1 = new Vector2[100];
		Vector2[] BA2 = new Vector2[100];
		Vector2[] BA3 = new Vector2[100];
		Color?[] BAC = new Color?[100];
		bool[] BAassign = new bool[100];
		/* PolyLineSegment GetBezierApproximation( int outputSegmentCount)
		 {
		     //Point[] points = new Point[outputSegmentCount + 1];
		     for (int i = 0; i <= outputSegmentCount; i++)
		     {
		         double t = (double)i / outputSegmentCount;
		         bezierarray[i] = GetBezierPoint(t, controlPoints, 0, controlPoints.Length);
		     }
		     return new PolyLineSegment(points, true);
		 }*/
		Vector2 m__tb;
		Vector2 GetBezierPoint( int getindex, float t, int index, int count )
		{	if ( count == 1 )
			{	switch ( index )
				{	case 0: return BA0[getindex];
				
					case 1: return BA1[getindex];
					
					case 2: return BA2[getindex];
					
					case 3: return BA3[getindex];
				}
			}
			
			var P0 = GetBezierPoint(getindex, t, index, count - 1);
			var P1 = GetBezierPoint(getindex, t, index + 1, count - 1);
			m__tb.Set( (1 - t) * P0.x + t * P1.x, (1 - t) * P0.y + t * P1.y );
			return m__tb;
		}
		/*    float t_1;
		   int index = 0;
		    Vector2 GetBezierPoint4(float t)
		   {
		       t_1 = 1 - t;
		       Math.Pow(t_1, 3) * BA[3].x + 3 * t * t_1 * t_1!
		         (1−t)3p1x + 3t(1−t)2p2x + 3t2(1−t)p3x + t3p4x
		
		       if (count == 1) return BA[index];
		       var P0 = GetBezierPoint(t, index, count - 1);
		       var P1 = GetBezierPoint(t, index + 1, count - 1);
		       m__tb.Set((1 - t) * P0.x + t * P1.x, (1 - t) * P0.y + t * P1.y);
		       return m__tb;
		   }*/
		Vector2 tp1, tp2;
		int drawIndex = 0;
		private void DRAWARROWED_BEZIER( int compIndex, Vector2 p1, Vector2 p2, Color? col = null )
		{	if ( Event.current.type != EventType.Repaint || p1 == p2 ) return;
		
			/*  p1 = EditorGUIUtility.GUIToScreenPoint(p1);
			  p2 = EditorGUIUtility.GUIToScreenPoint(p2);*/
			/* p1.x += RECT.x;
			 p1.y += RECT.y;*/
			// p2.x += size.x;
			// if ( Mathf.Abs( p1.x - p2.x ) < 20 ) p2.x = p1.x - 20;
			
			if ( !adapter._S_HG_ShowSelf ) return;
			
			BA0[drawIndex] = p1;
			BA1[drawIndex] = p1;
			BA2[drawIndex] = p2;
			BA3[drawIndex] = p2;
			/*    BA1[drawIndex].y = p2.y;
			    BA2[drawIndex].y = p1.y;*/
			
			/*  BA2[drawIndex].x = BA1[drawIndex].x;
			  //  BA[1].x += 15;
			  BAC[drawIndex] = col;*/
			BAC[drawIndex] = col;
			BAassign[drawIndex] = true;
			
			COMPIND[drawIndex] = compIndex;
			
			drawIndex++;
			
			if ( drawIndex >= BAassign.Length )
			{	Array.Resize( ref COMPIND, drawIndex + 1 );
				Array.Resize( ref BA0, drawIndex + 1 );
				Array.Resize( ref BA1, drawIndex + 1 );
				Array.Resize( ref BA2, drawIndex + 1 );
				Array.Resize( ref BA3, drawIndex + 1 );
				Array.Resize( ref BAC, drawIndex + 1 );
				Array.Resize( ref BAassign, drawIndex + 1 );
			}
			
			BAassign[drawIndex] = false;
		}
		
		private void DRAWARROWED_BEZIER_COMPOLETE()
		{	if (Event.current.type != EventType.Repaint) return;
		
			GL_BEGIN();
			//GL.Color(BAC[jjj] ??  (EditorGUIUtility.isProSkin ? arrowC1 : arrowC1personal));
			GL.Color(  (EditorGUIUtility.isProSkin ? arrowC1 : arrowC1personal));
			
			
			for ( int jjj = 0 ; jjj < drawIndex ; jjj++ )
			{	if ( !BAassign[jjj] ) break;
			
				BA3[jjj] = comps_inPos[COMPIND[jjj]];
				// BA3[jjj].x += sizememory[COMPIND[jjj]] + (9 + 3) * CURRENT_SCALE;
				BA3[jjj].x += sizememory[COMPIND[jjj]] + (9 + 3);
				// BA3[jjj].y += 4;
				BA2[jjj] = BA3[jjj];
				
				BA1[jjj].x = (BA3[jjj].x + BA1[jjj].x) / 2;
				BA2[jjj].x = BA0[jjj].x;
				
				
				tp2 = BA0[jjj];
				var L = 10;
				
				//mat.SetColor(adapter._Color, BAC[jjj] ?? Color.white);
				
				for ( int i = 0 ; i < L ; i++ )
				{	tp1 = tp2;
					tp2 = GetBezierPoint( jjj, i / (L - 1f), i / 5, 3 );
					GL_VERTEX3(tp1);
					GL_VERTEX3(tp2);
					//GL. Vertex3(tp2.x, tp2.y-0.1f, 1);
					//GL. Vertex3(tp1.x-0.1f, tp1.y, 0);
				}
				
				m_DrawArrow( tp1, tp2, BAC[jjj] );
			}
			
			GL_END();
			
		}
		
		
		
		void m_DrawArrow_( Vector2 p1, Vector2 p2, Color? col = null )
		{	var ARROW_SIZE = 10;
			var arr = (p1 - p2).normalized * ARROW_SIZE;
			var a1 = arr;
			a1.y = -a1.x;
			a1.x = arr.y;
			var a2 = -a1;
			
			
			var s1 = Vector2.Lerp(arr, a1, 0.2f) + p2;
			var s2 = Vector2.Lerp(arr, a2, 0.2f) + p2;
			float len = ARROW_SIZE /*/ 2*/;
			//  MonoBehaviour.print( p2);
			var c = col ?? (EditorGUIUtility.isProSkin ? arrowC1 : arrowC1personal);
			
			for ( float i = 0 ; i < len - 1 ; i += 0.5f / CURRENT_SCALE )
			{	var t1 = Vector2.Lerp(s1, p2, i / (len - 1f));
				var t2 = Vector2.Lerp(s2, p2, i / (len - 1f));
				DRAWLINE( t1, t2, c );
			}
		}
		void m_DrawArrow( Vector2 p1, Vector2 p2, Color? col = null )
		{	var ARROW_SIZE = 10;
			var arr = (p1 - p2).normalized * ARROW_SIZE;
			var a1 = arr;
			a1.y = -a1.x;
			a1.x = arr.y;
			var a2 = -a1;
			
			
			var s1 = Vector2.Lerp(arr, a1, 0.2f) + p2;
			var s2 = Vector2.Lerp(arr, a2, 0.2f) + p2;
			float len = ARROW_SIZE /*/ 2*/;
			//  MonoBehaviour.print( p2);
			//var c = col ?? (EditorGUIUtility.isProSkin ? arrowC1 : arrowC1personal);
			
			for ( float i = 0 ; i < len - 1 ; i += 0.5f / CURRENT_SCALE )
			{	var t1 = Vector2.Lerp(s1, p2, i / (len - 1f));
				var t2 = Vector2.Lerp(s2, p2, i / (len - 1f));
				GL_VERTEX3(t1);
				GL_VERTEX3(t2);
				//GL.Ver tex3(t2.x, t2.y-0.1f, 0);
				//GL.V ertex3(t2.x-0.1f, t2.y, 0);
			}
		}
		
		
		private void DRAWLINE_( Vector2 p1, Vector2 p2, Color? color = null )       // var oc = GUI.color;
		{	//  MonoBehaviour.print(p1 + " " +  p2);
			if ( ScreenRect == null )
				ScreenRect = new Rect( 0, 0, Screen.currentResolution.width, Screen.currentResolution.height );
				
			bool issX = Math.Abs(p2.x - p1.x) > Math.Abs(p2.y - p1.y);
			// var leng = issX ? p2.x - p1.x : p2.y - p1.y;
			float left = issX ? (int)p1.x : (int)p1.y;
			float right = issX ? (int)p2.x : (int)p2.y;
			float d = Math.Abs(right - (float)left);
			float step = right - left < 0 ? -1f : 1f;
			step /= CURRENT_SCALE;
			
			/*  GL.PushMatrix();
			  GL.LoadPixelMatrix();*/
			//GL.LoadPixelMatrix(0, 0 + RECT.width , 0 + RECT.height, 0);
			for ( float i = left ; step > 0 ? i <= right : i >= right ; i += step )
			{	if ( issX )
				{	point.x = i;
					point.y = Mathf.Lerp( p1.y, p2.y, Math.Abs( i - left ) / d );
				}
				
				else
				{	point.x = Mathf.Lerp( p1.x, p2.x, Math.Abs( i - left ) / d );
					point.y = i;
				}
				
				// GL.
				// GUI.DrawTexture(point, Texture2D.whiteTexture);
				if ( point.x < LOCAL_RECT.x || point.y < LOCAL_RECT.y || point.x > LOCAL_RECT.x + LOCAL_RECT.width || point.y > LOCAL_RECT.y + LOCAL_RECT.height )
				{	continue;
				}
				
				/*point.width = 1;
				point.height = 1;*/
				if ( color.HasValue )
				{	DrawTexture_Unscalable( point, ScreenRect.Value, color.Value );
					/* Graphics.DrawTexture( point, Texture2D.whiteTexture, ScreenRect.Value, 0, 0, 0, 0,
					     color.Value );*/
				}
				
				else
				{	/* point.x -= 1;
					     Graphics.DrawTexture(point, Texture2D.whiteTexture, ScreenRect.Value, 0, 0, 0, 0, arrowC2,null,-1);
					     Graphics.DrawTexture(point, Texture2D.whiteTexture, ScreenRect.Value, 0, 0, 0, 0, arrowC2,null,-1);*/
					DrawTexture_Unscalable( point, ScreenRect.Value, EditorGUIUtility.isProSkin ? arrowC1 : arrowC1personal );
					// Graphics.DrawTexture( point, Texture2D.whiteTexture, ScreenRect.Value, 0, 0, 0, 0, EditorGUIUtility.isProSkin ? arrowC1 : arrowC1personal );
					/*  point.x += 1;
					point.y -= 1;
					  Graphics.DrawTexture(point, Texture2D.whiteTexture, ScreenRect.Value, 0, 0, 0, 0, arrowC2);*/
				}
				
				
				// Graphics.DrawTexture(point, Texture2D.whiteTexture);
			}
			
			//GUI.color = oc;
		}
		
		
		
		private void DRAWLINE( Vector2 p1, Vector2 p2, Color? color = null )       // var oc = GUI.color;
		{	if (Event.current.type != EventType.Repaint) return;
		
			GL_BEGIN();
			GL.Color(color ?? (EditorGUIUtility.isProSkin ? arrowC1 : arrowC1personal));
			//	mat.SetColor(adapter._Color, color ?? Color.white);
			GL_VERTEX3(p1);
			GL_VERTEX3(p2);
			//GL. Vertex3(p2.x, p2.y-0.1f, 1);
			//GL. Vertex3(p1.x-0.1f, p1.y, 0);
			
			GL_END();
		}
		Material mat;
		internal void GL_BEGIN()
		{	GL.PushMatrix();
			// Set black as background color
			//GL.LoadPixelMatrix();
			GL.Clear(true, false, Color.black);
			var mat = adapter.DEFAULT_SHADER_SHADER.HIghlighterExternalMaterial;
			//mat = adapter.HIghlighterExternalMaterialNormal;
			
			mat.SetPass(0);
			//	GL.Begin(GL.LINES);
			GL.Begin(GL.LINES);
			mat.SetTexture(adapter._MainTex, Texture2D.whiteTexture);
		}
		internal void GL_END()
		{	GL.End();
		
			GL.PopMatrix();
		}
		
		/*	internal void GL_DRAW()
			{	if (Event.current.type != EventType.Repaint) return;
		
		
		
		
				foreach (var item in glStack)
				{	mat.SetTexture(_MainTex, item.texture);
					GL.Color(item.color);
					GL.TexCoord(new Vector3(0, 0, 0));
					GL.TexCoord(new Vector3(0, 1, 0));
					GL.TexCoord(new Vector3(1, 1, 0));
					GL.TexCoord(new Vector3(1, 0, 0));
					GL. Vertex3(item.rect.x, item.rect.y, 0);
					GL. Vertex3(item.rect.x, item.rect.y + item.rect.height, 0);
					GL. Vertex3(item.rect.x + item.rect.width, item.rect.y + item.rect.height, 0);
					GL. Vertex3(item.rect.x + item.rect.width, item.rect.y, 0);
				}
		
		
				//}
			}*/
		
		
		float? _FAST;
		float FAST
		{	get
			{	if ( _FAST.HasValue ) return _FAST.Value;
			
				if ( SystemInfo.processorFrequency >= 4000 ) return (_FAST = 0.5f).Value;
				
				if ( SystemInfo.processorFrequency < 3000 ) return (_FAST = 1.5f).Value;
				
				return  (_FAST = 1.0f).Value;
			}
		}
		
		
		private void DEBUG_HORISONTAL( float y, Color color )
		{	var asd = GUI.color;
			GUI.color *= color;
			GUI.DrawTexture( new Rect( 0, y, adapter.window().position.width, 2 ), Texture2D.whiteTexture );
			GUI.color = asd;
		}
		//GRAPH_ARGS temp_args;
		internal struct GRAPH_ARGS
		{	internal bool isAsset;
			internal bool isBezier;
		}
		
		private void DRAWINPUTS( BottomInterface.UniversalGraphController controller)
		{	var x = GAMEOBJECTRECT.x - SIZE.DEFAULT_PAD() - DEFAULTWIDTH( controller ) - 12 - SIZE.DEFAULT_PAD() - 10;
			// INPUT_RECT.width = DEFAULT_WIDTH;
			
			// var FH = 35 + 7 + 4;
			// var fieldsHeight = findedList.Count * FH;
			var fieldsHeight = INPUT_COMPS.Values.Sum(v => v.height);
			var mid = GAMEOBJECTRECT.y + GAMEOBJECTRECT.height * 0.5f;
			/*   DEBUG_HORISONTAL(GAMEOBJECTRECT.y,Color.red);
			   DEBUG_HORISONTAL(GAMEOBJECTRECT.y + GAMEOBJECTRECT.height * 0.5f,Color.blue);*/
			//  INPUT_RECT.height = FH;
			
			
			/*     p1.x = lineRect.x + lineRect.width + 5;
			     p1.y = lineRect.y + lineRect.height / 2;
			
			     p2.x = p1.x + DEFAULT_PAD;
			     p2.y = mid - fieldsHeight / 2 + index * FH;*/
			
			var i = 0;
			INPUT_CURRENT_Y = 0;
			
			foreach ( var fieldParams in findedList )
			{	var ob = EditorUtility.InstanceIDToObject(fieldParams.Key);
			
				if ( !ob )
				{	StoptBroadcasting();
					StartBroadcasting();
					return;
				}
				
				
				p2.x = x;
				p2.y = mid - fieldsHeight / 2 + INPUT_CURRENT_Y;
				
				// DEBUG_HORISONTAL(mid, Color.blue);
				// DEBUG_HORISONTAL(p2.y, Color.red);
				GRAPH_ARGS _a;
				p1 = DRAWSINGLECOMP( controller, p2, ob, DRAWSINGLECOMP_TARGET_EVENT_INPUT + i, false, out _a,
				                     fieldParams.Value );
				                     
				if ( _a.isAsset && !adapter._S_HG_ShowAssets ) continue;
				
				// DEBUG_HORISONTAL(p1.y, Color.yellow);
				
				for ( int tI = 0 ; tI < fieldParams.Value.targetGameObject.Count ; tI++ )
				{	if ( !fieldParams.Value.targetGameObject[tI] ) continue;
				
					p2 = comps_inPos[comps_inPos.Length - 1];
					DRAWARROWED_LINE( (p1), (p2) );
					p2 = p1;
					p2.x -= 5;
					DRAWLINE( (p1), (p2) );
				}
				
				for ( int tI = 0 ; tI < fieldParams.Value.targetComponent.Count ; tI++ )
				{	if ( !fieldParams.Value.targetComponent[tI] ) continue;
				
					var findIndex = -1;
					var cc = fieldParams.Value.targetComponent[tI].GetInstanceID();
					
					if ( compsSorted.ContainsKey( cc ) ) findIndex = compsSorted[cc];
					
					/* for ( int j = 0 ; j < comps.Length ; j++ ) {
					     if ( cc == comps[j] ) {
					         findIndex = j;
					         break;
					     }
					 }*/
					if ( findIndex == -1 ) continue;
					
					p2 = comps_inPos[findIndex];
					DRAWARROWED_LINE( (p1), (p2) );
					p2 = p1;
					p2.x -= 5;
					DRAWLINE( (p1), (p2) );
				}
				
				
				/*// OLD SINLE VALUE
				if ( fieldParams.Value.targetGameObject != null ) {
				p2 = comps_inPos[comps_inPos.Length - 1];
				} else {
				var findIndex = -1;
				var cc = fieldParams.Value.targetComponent;
				for ( int j = 0 ; j < comps.Length ; j++ ) {
				if ( cc == comps[j] ) {
				    findIndex = j;
				    break;
				}
				}
				if ( findIndex == -1 ) continue;
				p2 = comps_inPos[findIndex];
				}
				
				
				
				// p1.x += 12;
				DRAWARROWED_LINE( (p1) , (p2) );
				// DRAWARROWED_LINE( _mConvertRect( p1 ), _mConvertRect( p2 ) );
				
				p2 = p1;
				p2.x -= 5;
				DRAWLINE( (p1) , (p2) );
				// DRAWLINE( _mConvertRect( p1 ), _mConvertRect( p2 ) );
				*/// OLD SINLE VALUE
				
				i++;
			}
		}
		
		
		
		
		
		private void StoptBroadcasting()
		{	currentList = null;
			findedList.Clear();
			INPUT_COMPS.Clear();
			SKANNING = false;
			ERROR = false;
			WASSCAN = false;
		}
		
		private void StartBroadcasting()
		{
		
			if ( SKANNING ) return;
			
			// MonoBehaviour.print("ASD");
			
			WASSCAN = true;
			SKANNING = true;
			currentList = Utilities.AllSceneObjectsInterator( adapter, true ).GetEnumerator();
			CountProgress = Utilities.AllSceneObjectsInteratorCount( adapter, true );
			findedList.Clear();
			INPUT_COMPS.Clear();
			currentIndex = 0;
			
			if ( adapter.par.HiperGraphParams.SCANPERFOMANCE == 10 ) CalcBroadCast();
		}
		
		// int fdsHotControl = 0;
		double time;
		static int selection_id = -1;
		static int SEL_INC = 1;
		System.Diagnostics.Stopwatch WATCH_CLONE = System.Diagnostics.Stopwatch.StartNew();
		internal void CalcBroadCast()
		{	if ( !SKANNING || !adapter.ENABLE_BOTTOMDOCK_PROPERTY || !adapter.par.ENABLE_ALL || currentList == null ||
			        !CurrentSelection ) return;
			        
			if ( comps == null ) INIT_COMPS();
			
			// fdsHotControl = 0;
			var stopped = false;
			double counter = 0;
			bool start = false;
			
			while ( currentList.MoveNext() )
				// foreach (var current in currentList)
				//  while (currentIndex < currentList.Count)
			{
			
				if ( !start )
				{	WATCH_CLONE.Start();
					start = true;
				}
				
				
				
				var current = currentList.Current;
				
				if ( !/*currentList[currentIndex]*/current.Active() )     //var last2 = currentIndex;
				{	currentIndex = Utilities.AllSceneObjectsInteratorProgress( adapter );
					// if (last2 != currentIndex) Repaint();
					//currentIndex++;
					continue;
				}
				
				if ( adapter.IS_HIERARCHY() )
				{	if ( (current.go.hideFlags & Utilities.SearchFlags) == Utilities.SearchFlags )     //  var last2 = currentIndex;
					{	currentIndex = Utilities.AllSceneObjectsInteratorProgress( adapter );
						//  if (last2 != currentIndex) Repaint();
						// currentIndex++;
						continue;
					}
				}
				
				
				
				if ( current.go && current.go != CurrentSelection )
				{
				
					var currentComps =  HierarchyExtensions.Utilities.GetComponentFast<Component>.GetAll( current.go ) ;
					// var activeComps = new Dictionary<Component, int>();
					// var fieldsCount = 0;
					
					var result = new ObjectDisplay(current.go.GetInstanceID(), this)
					{	fAccessor =
						    new Dictionary<int, FieldsAccessor>(),
						GetComponents = currentComps
					};
					
					for ( int i = 0 ; i < currentComps.Length ; i++ )
					{	if ( !currentComps[i] ) continue;
					
					
						/*  if ("MoodBoxes" == current.name)
						      MonoBehaviour.print(current.name); */
						
						var f = GetReflectionFields(currentComps[i], result);
						result.fAccessor.Add( currentComps[i].GetInstanceID(), f );
						
						
						
					}
					
					/*     resilt.comps = activeComps;
					     resilt.DRAW_A_POSES = new Vector2[activeComps.Count];
					     resilt.DRAW_B_POSES = new Vector2[fieldsCount];
					     findedComopnentsAttached.Add(currentList[currentIndex].GetInstanceID(), resilt);*/
					
					
					if ( !result.WasAccessorInitialize_InMainThread )
					{	if ( result.AllowInitialize_InMainThread() )
						{	result.InitializeAccessor_InMainThread();
						}
						
						else       // SCANNING_COMPS.Add(current.GetInstanceID(), result);
						{
						}
					}
					
					
					
				}
				
				
				/* try
				 {
				     brc(dsc, currentList[currentIndex]);
				 }
				 catch
				 {
				     Clear();
				     currentIndex = 0;
				     dsc.Broadcasting = false;
				     ERROR = true;
				     dsc.WasFirst = false;
				     return;
				 }*/
				// var last = currentIndex;
				currentIndex = Utilities.AllSceneObjectsInteratorProgress( adapter );
				// if (last != currentIndex) Repaint();
				//     if ( Math.Abs( time - EditorApplication.timeSinceStartup ) > 0.5f )
				
				if ( Math.Abs( time - EditorApplication.timeSinceStartup ) > FAST )
				{	time = EditorApplication.timeSinceStartup;
					Repaint( WindowHyperController );
				}
				
				//currentIndex++;
				
				start = false;
				WATCH_CLONE.Stop();
				counter += WATCH_CLONE.ElapsedTicks / (double)System.Diagnostics.Stopwatch.Frequency;
				WATCH_CLONE.Reset();
				
				
				// if (
				
				if (//par.HiperGraphParams.SCANPERFOMANCE != 1 &&
				    /* interator > (par.HiperGraphParams.SCANPERFOMANCE - 0.2f) * 1600 + 0.2f * 150*/
				    CHECK_PERFOMANCE( counter ) /*> par.HiperGraphParams.SCANPERFOMANCE / 80*/)
				{	stopped = true;
					break;
				}
			}
			
			//   MonoBehaviour.print(dsc.TEXTUREobjects.Count + " " + dsc.OBJECTtexture.Count);
			if ( !stopped )     // MonoBehaviour.print("ASD");
			{	SKANNING = false;
				ERROR = false;
				Repaint( WindowHyperController );
			}
			
			else { }
		}
		
		bool CHECK_PERFOMANCE( double counter )
		{	switch ( adapter.par.HiperGraphParams.SCANPERFOMANCE )
			{	case 2: return counter > 0.1f / 80;
			
				case 4: return counter > 0.2f / 80;
				
				case 6: return counter > 1f / 80;
				
				case 8: return counter > 8f / 80;
				
				case 10: return false;
				
				default: throw new Exception( "unknowing performance" );
			}
		}
		
		
		internal class ObjectDisplay {
			internal int gameObjectId;
			internal HyperGraph hyperGraph;
			internal ObjectDisplay( int id, HyperGraph hyperGraph )
			{	gameObjectId = id;
				this.hyperGraph = hyperGraph;
			}
			
			internal Component[] GetComponents;
			
			bool AllowInitialize_InMainThread_cache;
			
			/*  internal bool WAS_INIT_CONTAINS(int id)
			  {
			      lock (wasInit)
			      {
			          return (wasInit.ContainsKey(id));
			      }
			  }*/
			//  Dictionary<int, int> wasInit = new Dictionary<int, int>();
			
			internal bool AllowInitialize_InMainThread()
			{	lock ( this )
				{	if ( AllowInitialize_InMainThread_cache ) return true;
				
					lock ( hyperGraph.LOCKER )
					{	foreach ( var fieldsAccessor in fAccessor )
						{	if ( !fieldsAccessor.Value.completed_thead ) return false;
						}
						
						AllowInitialize_InMainThread_cache = true;
						
						return true;
					}
				}
			}
			internal bool WasAccessorInitialize_InMainThread;
			internal void InitializeAccessor_InMainThread()
			{	if ( !AllowInitialize_InMainThread_cache ) hyperGraph.adapter.logProxy.LogError( "InitializeAccessor_InMainThread" );
			
				SEL_INC++;
				
				lock ( this )
				{	if ( WasAccessorInitialize_InMainThread ) return;
				
					WasAccessorInitialize_InMainThread = true;
				}
				
				/* lock (wasInit)
				 {
				     if (wasInit.ContainsKey(current.GetInstanceID())) return;
				     wasInit.Add(current.GetInstanceID(), 0);
				 }*/
				/* WasAccessorInitialize_InMainThread = true;
				wasInit
				 var height = SIZES.OBJECT + SIZES.padding;
				
				 var fdsIndex = 0;
				
				 foreach (var fieldsAccessor in fAccessor)
				 {
				     if (!fieldsAccessor.Key) continue;
				     var f = fieldsAccessor.Value;
				     if (!f.completed_thead) continue;
				     if (!f.Was_InitializeBroadcasting_InMainThead) f.InitializeBroadcasting_InMainThead(fieldsAccessor.Key);
				
				
				 }*/
				
				
				/* if (AllFields.Count != 0)
				 {
				     result.DRAW_B_POSES = new Vector2[fdsIndex];
				     result.height = height;
				     INPUT_COMPS.Add(current.GetInstanceID(), result);
				 }*/
				
				
				/*
				                        if ("MoodBoxes" == EditorUtility.InstanceIDToObject(gameObjectId).name)
				                            MonoBehaviour.print(EditorUtility.InstanceIDToObject(gameObjectId).name);*/
				
				// var currentComps = current.GetComponents<Component>();
				// var activeComps = new Dictionary<Component, int>();
				// var fieldsCount = 0;
				var height = hyperGraph.SIZE.OBJECT() + hyperGraph.SIZE.padding_y();
				/* var result = new ObjectDisplay() {
				     AllFields =
				                                          new Dictionary<Component, Dictionary<string, int>>()
				 };*/
				var fdsIndex = 0;
				
				foreach ( var c in GetComponents )
				{
				
					// for (int i = 0; i < currentComps.Length; i++)
					// {
					// var c = EditorUtility.InstanceIDToObject(fieldsAccessor.Key);
					if ( !c ) continue;
					
					// var f = fAccessor[c.GetInstanceID()].f;
					var id = c.GetInstanceID();
					var f = fAccessor[id].faList;
					
					if ( f.Length == 0 ) continue;
					
					// resilt.ComponentToBPosIndex.Add(currentComps[i], fieldsCount);
					var needAdd = false;
					var fds = new Dictionary<string, int>();
					/*  if (f.Length != 0 && c.gameObject.name == "Main Camera")
					      MonoBehaviour.print(f.Length);*/
					
					for ( int fIndex = 0 ; fIndex < f.Length ; fIndex++ )
					{	bool haveChange = false;
					
						f[fIndex].CheckID(selection_id, SEL_INC);
						
						if (f[fIndex].GetAllValuesCache == null || !f[fIndex].GetAllValuesCache.ContainsKey(c.GetInstanceID() ))
						{	if (f[fIndex].GetAllValuesCache == null) f[fIndex].GetAllValuesCache = new Dictionary<int, Dictionary<string, object>>();
						
							f[fIndex].GetAllValuesCache.Add(c.GetInstanceID(), f[fIndex].GetAllValues( c, 0, hyperGraph.adapter._S_HG_EventsMode | hyperGraph.adapter._S_HG_SkipArrays ));
						}
						
						foreach ( var item in  f[fIndex].GetAllValuesCache[c.GetInstanceID() ])
						{	if ( item.Value == null ) continue;
						
							if ( item.Value is GameObject )
							{	if ( (GameObject)item.Value == hyperGraph.CurrentSelection )
								{	if ( !hyperGraph.findedList.ContainsKey( id ) )
										hyperGraph.findedList.Add( id,
										                           new FIELD_PARAMS( f[fIndex] ) );
										                           
									hyperGraph.findedList[id].targetGameObject.Add( hyperGraph.CurrentSelection );
									// activeComps.Add(currentComps[i], i);
									haveChange = true;
								}
							}
							
							else
							{	var getComp = item.Value as UnityEngine.Object;
							
								if ( getComp && hyperGraph.compsSorted.ContainsKey( getComp.GetInstanceID() ) )
								{	if ( !hyperGraph.findedList.ContainsKey( id ) )
										hyperGraph.findedList.Add( id,
										                           new FIELD_PARAMS( f[fIndex] ) );
										                           
									hyperGraph.findedList[id].targetComponent.Add( getComp );
									haveChange = true;
								}
							}
						}
						
						/*// OLD SINLE VALUE
						if ( f[fIndex].ObjectType == hyperGraph.GameObjectType ) {
						if ( (GameObject)f[fIndex].GetValue( c ) == hyperGraph.CurrentSelection ) {
						hyperGraph.findedList.Add( id ,
						           new FIELD_PARAMS( f[fIndex] , hyperGraph.CurrentSelection , null , hyperGraph.fdsHotControl++ ) );
						// activeComps.Add(currentComps[i], i);
						haveChange = true;
						}
						} else {
						var getComp = (UnityEngine.Object)f[fIndex].GetValue(c);
						if ( getComp && hyperGraph.compsSorted.ContainsKey( getComp.GetInstanceID() ) ) {
						
						
						hyperGraph.findedList.Add( id ,
						           new FIELD_PARAMS( f[fIndex] , null , getComp , hyperGraph.fdsHotControl++ ) );
						// activeComps.Add(currentComps[i], i);
						haveChange = true;
						}
						}
						*/// OLD SINLE VALUE
						
						if ( haveChange && !fds.ContainsKey( f[fIndex].Name ) )     // fieldsCount++;
						{	height += hyperGraph.SIZE.VAR();
							fds.Add( f[fIndex].Name, fdsIndex++ );
							// result.fields.Add(f[fIndex].Name, result.fields.Count);
							needAdd = true;
						}
					}
					
					if ( needAdd )
					{	this.AllFields.Add( c.GetInstanceID(), fds );
						height += hyperGraph.SIZE.COMP();
					}
				}
				
				/*     resilt.comps = activeComps;
				 resilt.DRAW_A_POSES = new Vector2[activeComps.Count];
				 resilt.DRAW_B_POSES = new Vector2[fieldsCount];
				 findedComopnentsAttached.Add(currentList[currentIndex].GetInstanceID(), resilt);*/
				if ( this.AllFields.Count != 0 )
				{	this.DRAW_B_POSES = new Vector2[fdsIndex];
					this.height = height;
					hyperGraph.INPUT_COMPS.Add( gameObjectId, this );
					
				}
				
				
				
				
				
				
				
			}
			
			
			// internal FieldsAccessor fAccessor;
			internal Dictionary<int, FieldsAccessor> fAccessor = null;
			internal Dictionary<int, Dictionary<string, int>> AllFields = new Dictionary<int, Dictionary<string, int>>();
			internal Dictionary<int, int> objecComps = new Dictionary<int, int>();
			internal bool DRAW;
			
			internal List<Vector2> DRAW_A_POSES = new List<Vector2>();
			internal Vector2[] DRAW_B_POSES;
			
			internal float __height;
			internal float height     //get { return Mathf.RoundToInt( __height/* * hyperGraph.CURRENT_SCALE*/ ); }
			{	get { return __height/* * hyperGraph.CURRENT_SCALE*/; }
			
				set { __height = value; }
			}
			// internal Dictionary<Component, int> ComponentToBPosIndex;
		}
		
		private struct FIELD_PARAMS
		{	internal FIELD_PARAMS( FieldAdapter field )
			{	this.field = field;
				this.targetGameObject = new List<Object>();
				this.targetComponent = new List<Object>();
				//this.POS_INDEX = POS_INDEX;
			}
			
			internal FieldAdapter field;
			internal List<Object> targetGameObject;
			internal List<Object> targetComponent;
			//internal UnityEngine.Object targetComponent;
			// internal int POS_INDEX;
		}
		
		//////////////////////////////////////////////////
		//////////////////////////////////////////////////  DRAW TARGET
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	}
}
}