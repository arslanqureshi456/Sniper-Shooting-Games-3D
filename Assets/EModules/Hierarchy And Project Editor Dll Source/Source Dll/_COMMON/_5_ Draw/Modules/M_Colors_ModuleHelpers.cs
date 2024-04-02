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


	internal class DynamicColor {
		internal Func<HierarchyObject, object, Color> GET;
	}
	
	
	
	internal Type t_GameObject = typeof(GameObject);
	internal Texture __NullContext;
	
	internal Texture NullContext
	{	get
		{	return __NullContext ?? (__NullContext =
			                             Utilities.ObjectContent_NoCacher(this, (UnityEngine.Object) null, t_GameObject).add_icon);
		}
	}
	
	internal TempColorClass __INTERNAL_TempColor_Empty = new TempColorClass().AddIcon(null);
	
	// static class GLD {
	static internal void GL_DrawTexture(Rect rect, Texture tex, ScaleMode scale, bool alpha, float ascpect, Color color,
	                                    float roderWidth, float borderRadius)
	{	GL_DrawTexture(rect, (Texture2D) tex, scale, alpha, ascpect, color, roderWidth, borderRadius);
	}
	
	static internal void GL_DrawTexture(Rect rect, Texture2D tex, ScaleMode scale, bool alpha, float ascpect,
	                                    Color color, float roderWidth, float borderRadius)
	{	if (Event.current.type == EventType.Repaint)
		{	/* DEFAULT_SHADER_SHADER.HIghlighterExternalMaterial.SetPass( 0 );
			GL.PushMatrix();
			// GL.LoadPixelMatrix();
			GL.Begin( GL.QUADS );
			GL.Color( color );
			GL.Vertex3( rect.x, rect.y, 0 );
			GL.Vertex3( rect.x, rect.y + rect.height, 0 );
			GL.Vertex3( rect.x + rect.width, rect.y + rect.height, 0 );
			GL.Vertex3( rect.x + rect.width, rect.y, 0 );
			GL.End();
			GL.PopMatrix();*/
			
			
			//   GL.BeginClip( rect ); // only if drawing on the Unity inspector
			
			// GL.EndClip();
			if (glStackPos >= glStack.Count ) glStack.Add(new GL_STACK());
			
			glStack[glStackPos].color = color;
			glStack[glStackPos].rect = rect;
			glStack[glStackPos].texture = tex;
			glStackPos++;
		}
	}
	
	internal class GL_STACK {
		internal Color color;
		internal Rect rect;
		internal Texture2D texture;
	}
	static int glStackPos = 0;
	static List<GL_STACK> glStack = new List<GL_STACK>(500);
	
	void PREPARE_DRAW_GL()
	{	glStackPos = 0;
	}
	
	//int _Color = Shader.PropertyToID("_Color");
	int _MainTex = Shader.PropertyToID("_MainTex");
	
	internal void DRAW_GL()
	{
#pragma warning disable
		return;
		
		if (Event.current.type != EventType.Repaint) return;
		
		GL.PushMatrix();
		// Set black as background color
		GL.Clear(true, false, Color.black);
		var mat = DEFAULT_SHADER_SHADER.HIghlighterExternalMaterial;
		mat.SetPass(0);
		GL.Begin(GL.QUADS);
		
		for (int i = 0; i < glStackPos; i++)
		{	var item = glStack[i];
			mat.SetTexture(_MainTex, item.texture);
			GL.Color(item.color);
			
			GL.TexCoord(new Vector3(0, 0, 0));
			GL.TexCoord(new Vector3(0, 1, 0));
			GL.TexCoord(new Vector3(1, 1, 0));
			GL.TexCoord(new Vector3(1, 0, 0));
			GL.Vertex3(item.rect.x, item.rect.y, 0);
			GL.Vertex3(item.rect.x, item.rect.y + item.rect.height, 0);
			GL.Vertex3(item.rect.x + item.rect.width, item.rect.y + item.rect.height, 0);
			GL.Vertex3(item.rect.x + item.rect.width, item.rect.y, 0);
		}
		
		GL.End();
		
		GL.PopMatrix();
		//}
		PREPARE_DRAW_GL();
#pragma warning restore
	}
	
	
	
	internal float DEFAULT_ICON_SIZE
	{	get
		{	float res;
		
			if (par.USEdefaultIconSize) res = par.defaultIconSize;
			else res = EditorGUIUtility.singleLineHeight;
			
			// return Mathf.Min(par.LINE_HEIGHT, res);
			return res;
		}
	}
	
	GUIStyle __foldoutStyle;
	
	internal GUIStyle foldoutStyle
	{	get { return __foldoutStyle ?? (__foldoutStyle = (GUIStyle) "IN Foldout"); }
	}
	
	internal float foldoutStyleWidth
	{	get { return foldoutStyle.fixedWidth + 2; }
	}
	
	internal float foldoutStyleHeight
	{	get { return foldoutStyle.fixedHeight != 0 ? foldoutStyle.fixedHeight : EditorGUIUtility.singleLineHeight; }
	}
	
	
	
	static EditorWindow[] __ALL_WINDOWS;
	
	public static EditorWindow[] ALL_WINDOWS
	{	get
		{	if (__ALL_WINDOWS != null) return __ALL_WINDOWS;
		
			var tv = Resources.FindObjectsOfTypeAll<EditorWindow>();
			
			if (tv.Any(w => w.GetType().Name.Contains("SceneView")) &&
			        tv.Any(w => w.GetType().Name.Contains("SceneHierarchy"))) __ALL_WINDOWS = tv;
			        
			return tv;
		}
		
		set { }
	}
	
	//static Vector2? __MAX_HEIGHT;
	// static Vector2? __MAX_WIDTH;
	internal static Vector2 MAX_WINDOW_HEIGHT
	{	get
		{	/* var max = ALL_WINDOWS.Select( w => w.position.height ).Max();
			   var ww =   ALL_WINDOWS.First(w => w.position.height == max);
			   return new Vector2(ww.position.y, ww.position.height);*/
			/*var poses = ALL_WINDOWS.Select( w => w.position).ToArray();
			var x =   poses.Select( w => w.y).Min();
			var y =   poses.Select( w => w.y + w.height).Max();
			return new Vector2(x, y - x );*/
			var WB = WINBORDER;
			return new Vector2(WB.y, WB.height);
		}
	}
	
	internal static Vector2 MAX_WINDOW_WIDTH
	{	get
		{	/*var poses = ALL_WINDOWS.Select( w => w.position).ToArray();
			  var x =   poses.Select( w => w.x).Min();
			  var y =   poses.Select( w => w.x + w.width).Max();
			  return new Vector2(x, y - x );*/
			var WB = WINBORDER;
			return new Vector2(WB.x, WB.width);
		}
	}
	
	static System.Reflection.FieldInfo __hostView_type;
	
	static System.Reflection.FieldInfo hostView_type
	{	get
		{	return __hostView_type ?? (__hostView_type =
			                               typeof(EditorWindow).GetField("m_Parent", (System.Reflection.BindingFlags) (-1)));
		}
	}
	
	static System.Reflection.PropertyInfo __parent;
	
	static System.Reflection.PropertyInfo parent
	{	get
		{	return __parent ?? (__parent =
			                        hostView_type.FieldType.BaseType.BaseType.GetProperty("parent",
			                                (System.Reflection.BindingFlags) (-1)));
		}
	}
	
	static System.Reflection.FieldInfo __m_Window;
	
	static System.Reflection.FieldInfo m_Window
	{	get
		{	return __m_Window ?? (__m_Window =
			                          hostView_type.FieldType.BaseType.BaseType.GetField("m_Window",
			                                  (System.Reflection.BindingFlags) (-1)));
		}
	}
	
	static System.Reflection.PropertyInfo __windowPosition;
	
	static System.Reflection.PropertyInfo windowPosition
	{	get
		{	return __windowPosition ?? (__windowPosition =
			                                hostView_type.FieldType.BaseType.BaseType.GetProperty("windowPosition",
			                                        (System.Reflection.BindingFlags) (-1)));
		}
	}
	
	static System.Reflection.PropertyInfo __screenPosition;
	
	static System.Reflection.PropertyInfo screenPosition
	{	get
		{	return __screenPosition ?? (__screenPosition =
			                                hostView_type.FieldType.BaseType.BaseType.GetProperty("screenPosition",
			                                        (System.Reflection.BindingFlags) (-1)));
		}
	}
	
	internal static Rect WINBORDER
	{	get
		{	float TOP = float.MaxValue, RIGHT = float.MinValue, LEFT = float.MaxValue, BOTTOM = float.MinValue;
		
			foreach (var item in EModulesInternal.Adapter.ALL_WINDOWS)
			{	object hostView = null;
			
				try
				{	hostView = hostView_type.GetValue(item);
				
					if (hostView == null) continue;
				}
				
				catch
				{	continue;
				}
				
				// var cntxw = m_Window.GetValue(hostView);
				try
				{	while (parent.GetValue(hostView, null) != null)
					{	hostView = parent.GetValue(hostView, null);
					}
				}
				
				catch
				{	continue;
				}
				
				Rect tempR;
				
				try
				{	tempR = (Rect) screenPosition.GetValue(hostView, null);
				}
				
				catch
				{	continue;
				}
				
				if (tempR.x < LEFT) LEFT = tempR.x;
				
				if (tempR.x + tempR.width > RIGHT) RIGHT = tempR.x + tempR.width;
				
				if (tempR.y < TOP) TOP = tempR.y;
				
				if (tempR.y + tempR.height > BOTTOM) BOTTOM = tempR.y + tempR.height;
			}
			
			if (TOP == float.MaxValue)
			{	TOP = 0;
				RIGHT = Screen.currentResolution.width;
				BOTTOM = Screen.currentResolution.height;
				LEFT = 0;
			}
			
			return new Rect(LEFT, TOP, RIGHT - LEFT, BOTTOM - TOP);
		}
	}
	
	
	internal class DynamicRect {
		internal void Set(Rect r1, Rect r2, bool isMain, HierarchyObject o, bool HasIcon, float MinLeft)
		{	ref_selectionRect = r1;
			ref_fadeRect = r2;
			this.isMain = isMain;
			this.HasIcon = HasIcon;
			this.o = o;
			this.MinLeft = MinLeft;
			ref_selectionRect.x += MinLeft;
		}
		
		
		internal Adapter adapter;
		internal Rect ref_selectionRect;
		internal Rect ref_fadeRect;
		internal bool isMain;
		internal bool HasIcon;
		internal HierarchyObject o;
		internal float? labelSize;
		internal float MinLeft;
		
		
		internal float GET_LEFT(BgAligmentLeft align)
		{	switch (align)
			{	case BgAligmentLeft.MinLeft: return MinLeft;
			
				case BgAligmentLeft.Fold:
				{	var res = ref_selectionRect.x - EditorGUIUtility.singleLineHeight;
				
					if (isMain && Adapter.UNITY_CURRENT_VERSION >= Adapter.UNITY_2019_2_0_VERSION)
						res -= adapter.raw_old_leftpadding;
						
					return res;
				}
				
				/*  if (adapter._S_bgIconsPlace != 0) return ref_selectionRect.x - EditorGUIUtility.singleLineHeight ;
				  else return ref_selectionRect.x;*/
				case BgAligmentLeft.BeginLabel:
				{	var res = ref_selectionRect.x;
				
					if (HasIcon) res += adapter.DEFAULT_ICON_SIZE;
					
					if (isMain && Adapter.UNITY_CURRENT_VERSION >= Adapter.UNITY_2019_2_0_VERSION)
						res -= adapter.raw_old_leftpadding;
						
					return res;
				}
				
				case BgAligmentLeft.EndLabel:
				{	/*if ( adapter.M_CustomIcontsEnable && adapter.par.ENABLE_RIGHTDOCK_FIX && par.COMPONENTS_NEXT_TO_NAME )
					{   var pos = adapter.M_CustomIconsModule.GetIconPos( o.id );
					if (pos != -1 )
					{   return pos;
					}
					}*/
					if (!labelSize.HasValue)
						adapter.labelStyle.CalcMinMaxWidth(GET_CONTENT(o.name), out MinHeight, out MaxHeight);
					else MinHeight = labelSize.Value;
					
					var res = ref_selectionRect.x + MinHeight;
					
					if (isMain && Adapter.UNITY_CURRENT_VERSION >= Adapter.UNITY_2019_2_0_VERSION)
						res -= adapter.raw_old_leftpadding;
						
					if (HasIcon) return res + adapter.DEFAULT_ICON_SIZE + EditorGUIUtility.singleLineHeight / 3;
					
					return res;
				}
				
				case BgAligmentLeft.Modules: return ref_fadeRect.x;
			}
			
			throw new Exception();
		}
		
		
		internal float GET_RIGHT(BgAligmentRight align)
		{	switch (align)
			{	case BgAligmentRight.Icon: return GET_LEFT(BgAligmentLeft.Fold);
			
				case BgAligmentRight.BeginLabel: return GET_LEFT(BgAligmentLeft.BeginLabel);
				
				case BgAligmentRight.EndLabel: return GET_LEFT(BgAligmentLeft.EndLabel);
				
				case BgAligmentRight.Modules: return GET_LEFT(BgAligmentLeft.Modules);
				
				case BgAligmentRight.MaxRight:
					return ref_selectionRect.x + ref_selectionRect.width - adapter.raw_old_leftpadding +
					       adapter.PREFAB_BUTTON_SIZE;
					       
				case BgAligmentRight.WidthFixedGradient: return -1;
			}
			
			throw new Exception();
		}
		
		
		static Rect tr = new Rect();
		
		internal Rect ConvertToBGFromTempColor(TempColorClass tempColor)
		{	tr.x = GET_LEFT(tempColor.BG_ALIGMENT_LEFT_CONVERTED);
			tr.width = GET_RIGHT(tempColor.BG_ALIGMENT_RIGHT_CONVERTED) - tr.x;
			
			if (tempColor.BG_ALIGMENT_LEFT_CONVERTED == BgAligmentLeft.BeginLabel && adapter.USE_LABEL_OFFSET)
			{	/* tr.x -= 3;
				tr.width += 3;*/
			}
			
			if (tempColor.BG_ALIGMENT_RIGHT_CONVERTED == BgAligmentRight.WidthFixedGradient)
				tr.width = tempColor.BG_WIDTH * 2;
				
			tr.y = ref_selectionRect.y;
			tr.height = ref_selectionRect.height;
			
			if (tempColor.BG_HEIGHT == 1)
			{	var h = adapter.labelStyle.CalcHeight(GET_CONTENT(o.name), 10000) - 4;
				var d = (ref_selectionRect.height - h);
				tr.y += d / 2;
				tr.height -= d;
			}
			
			else
				if (tempColor.BG_HEIGHT == 2)
				{	tr.y += tr.height / 2;
					tr.height = 1;
				}
				
				else
				{	var odd = Mathf.Abs(adapter._S_BottomPaddingForBgColor) % 2;
					var div = adapter._S_BottomPaddingForBgColor / 2;
					tr.y += div;
					tr.height -= div * 2 + odd;
				}
				
			return tr;
		}
		
	}
	
	
	internal class ArrayPrefs {
		internal ArrayPrefs(string key)
		{	this.key = key + " ";
		}
		
		string key;
		
		internal List<int> Value
		{	get
			{	var result = new List<int>();
				int i = 0;
				int value;
				
				while ((value = EditorPrefs.GetInt(key + i, -1)) != -1)
				{	result.Add(value);
					i++;
				}
				
				return result;
			}
			
			set
			{	int i = 0;
			
				while (EditorPrefs.GetInt(key + i, -1) != -1)
				{	EditorPrefs.DeleteKey(key + i);
					i++;
				}
				
				i = 0;
				
				foreach (var item in value)
				{	EditorPrefs.SetInt(key + i, item);
					i++;
				}
			}
		}
	}
	
	
	internal class TempColorClass {
		internal Color32 BGCOLOR, LABELCOLOR;
		
		
		
		public override int GetHashCode()
		{	if (el == null) return -1;
		
			var hashCode = -1973547999;
			hashCode = hashCode * -1521134295 + EqualityComparer<Color32>.Default.GetHashCode(BGCOLOR);
			hashCode = hashCode * -1236236295 + EqualityComparer<Color32>.Default.GetHashCode(LABELCOLOR);
			hashCode = hashCode * -1525235251 + child.GetHashCode();
			int _th = 0;
			
			for (int i = 9; i < el.list.Count; i++)
			{	_th ^= _th * -0525623251 + el.list[i] * 1000;
			}
			
			if (el.list.Count >= 5) _th ^= _th * -0525623251 + el.list[4] * 1000;
			
			hashCode = hashCode * -1521134295 + _th;
			return hashCode;
		}
		
		public static bool operator ==(TempColorClass a, TempColorClass b)
		{	if (ReferenceEquals(a, null) && ReferenceEquals(b, null)) return true;
		
			if (!ReferenceEquals(a, null) && a.protect_add && ReferenceEquals(b, null))
			{	throw new Exception("Protected TempColor cannot be null");
				//return a.add_icon == null;
			}
			
			if (ReferenceEquals(a, null) || ReferenceEquals(b, null)) return false;
			
			var result = true;
			result &= a.BGCOLOR.Equals(b.BGCOLOR);
			result &= a.LABELCOLOR.Equals(b.LABELCOLOR);
			result &= a.child == b.child;
			
			if (result)
			{	if (a.el == null || b.el == null) return false;
			
				if (a.el.list.Count <= 9 && b.el.list.Count <= 9) return true;
				
				if (a.el.list.Count != b.el.list.Count) return false;
				
				for (int i = 9; i < b.el.list.Count; i++)
				{	if (a.el.list[i] != b.el.list[i]) return false;
				}
			}
			
			return result;
		}
		
		public static bool operator !=(TempColorClass a, TempColorClass b)
		{	return !(a == b);
		}
		
		public override bool Equals(object obj)
		{	return (obj as TempColorClass) == this;
		}
		/*  internal bool? _mchild {get {return null; } set { } }
		  internal bool? _mLABEL_SHADOW {get {return null; } set { } }
		  internal int? _mBG_ALIGMENT_LEFT {get {return null; } set { } }
		  internal int? _mBG_ALIGMENT_RIGHT {get {return null; } set { } }
		  internal int? _mBG_FILL {get {return null; } set { } }
		  internal int? _mBG_HEIGHT {get {return null; } set { } }*/
		
		static Color color32;
		
		internal TempColorClass AddIcon(Texture icon, bool hascolor, SingleList color)
		{	this._add_icon = icon;
			this._add_hasiconcolor = hascolor;
			color32.r = color.list[0] / 255f;
			color32.g = color.list[1] / 255f;
			color32.b = color.list[2] / 255f;
			color32.a = color.list[3] / 255f;
			this._add_iconcolor = color32;
			protect_add = true;
			return this;
		}
		
		internal TempColorClass AddIcon(Texture icon, bool hascolor, Color color)
		{	this._add_icon = icon;
			this._add_hasiconcolor = hascolor;
			this._add_iconcolor = color;
			protect_add = true;
			return this;
		}
		
		internal TempColorClass AddIcon(Texture icon)
		{	this._add_icon = icon;
			this._add_hasiconcolor = false;
			protect_add = true;
			return this;
		}
		
		internal TempColorClass AddIcon(Texture icon, string tooltip)
		{	AddIcon(icon);
			this.tooltip = tooltip;
			return this;
		}
		
		bool protect_add;
		internal string tooltip = null;
		Texture _add_icon;
		
		internal Texture add_icon
		{	get { return _add_icon; }
		
			set
			{	if (protect_add) throw new Exception("TempColor locked");
			
				_add_icon = value;
			}
		}
		
		bool _add_hasiconcolor;
		
		internal bool add_hasiconcolor
		{	get { return _add_hasiconcolor; }
		
			set
			{	if (protect_add) throw new Exception("TempColor locked");
			
				_add_hasiconcolor = value;
			}
		}
		
		Color _add_iconcolor;
		
		internal Color add_iconcolor
		{	get { return _add_iconcolor; }
		
			set
			{	if (protect_add) throw new Exception("TempColor locked");
			
				_add_iconcolor = value;
			}
		}
		
		internal SingleList el;
		internal bool LockOverwrite = false;
		static SingleList _el = new SingleList() {list = new int[9].ToList()};
		
		internal void Reset(SingleList __el)
		{	LABELCOLOR = BGCOLOR = Color.clear;
		
			add_hasiconcolor = false;
			add_icon = null;
			add_iconcolor = Color.clear;
			
			/*  _mchild = _mLABEL_SHADOW = null;
			_mBG_ALIGMENT_LEFT = null;
			_mBG_ALIGMENT_RIGHT = null;
			_mBG_FILL = null;
			_mBG_HEIGHT = null;*/
			
			// if (__el == null) return;
			if (LockOverwrite) throw new Exception("LockOverwrite");
			
			if (el == null) el = new SingleList() {list = _el.list.ToList()};
			
			//el = new SingleList();
			for (int i = 0; i < __el.list.Count; i++)
			{	if (el.list.Count <= i) el.list.Add(__el.list[i]);
				else el.list[i] = __el.list[i];
			}
			
			for (int i = __el.list.Count; i < el.list.Count; i++)
			{	el.list[i] = 0;
			}
			
			//el = __el;
		}
		
		internal void Clear() // Reset( el );
		{	LABELCOLOR = BGCOLOR = Color.clear;
		
			add_hasiconcolor = false;
			add_icon = null;
			add_iconcolor = Color.clear;
			
			if (el == null) el = new SingleList() {list = _el.list.ToList()};
			
			for (int i = 0; i < el.list.Count; i++)
			{	el.list[i] = 0;
			}
		}
		
		internal TempColorClass empty
		{	get
			{	/*Reset( _el );
				for ( int i = 0 ; i < el.list.Count ; i++ )
				{   el.list[i] = 0;
				}*/
				Clear();
				return this;
			}
		}
		
		
		internal enum CopyType
		{	BG,
			LABEL
		}
		
		internal void CopyFromTo(CopyType type, TempColorClass from, ref TempColorClass to)
		{	switch (type)
			{	case CopyType.LABEL:
					to.LABELCOLOR = from.LABELCOLOR;
					to.LABEL_SHADOW = from.LABEL_SHADOW;
					break;
					
				case CopyType.BG:
					to.BGCOLOR = from.BGCOLOR;
					to.BG_ALIGMENT_LEFT = from.BG_ALIGMENT_LEFT;
					to.BG_ALIGMENT_RIGHT = from.BG_ALIGMENT_RIGHT;
					to.BG_HEIGHT = from.BG_HEIGHT;
					to.BG_WIDTH = from.BG_WIDTH;
					to.BG_FILL = from.BG_FILL;
					break;
			}
			
			// to.el.list = from.ToList();
		}
		
		internal void OverrideTo(ref TempColorClass to)
		{	if (this.HAS_BG_COLOR || this.HAS_LABEL_COLOR)
			{	to.child = this.child;
			
				if (this.HAS_BG_COLOR) CopyFromTo(TempColorClass.CopyType.BG, this, ref to);
				
				if (this.HAS_LABEL_COLOR) CopyFromTo(TempColorClass.CopyType.LABEL, this, ref to);
			}
			
			if (this.add_icon)
			{	to.add_icon = this.add_icon;
			
				if (this.add_hasiconcolor)
				{	to.add_hasiconcolor = this.add_hasiconcolor;
					to.add_iconcolor = this.add_iconcolor;
				}
			}
		}
		
		
		
		static ArrayPrefs[] _GetLastTempColor = new ArrayPrefs[2];
		
		internal TempColorClass GetLastTempColor(Adapter adapter)
		{	if (_GetLastTempColor[adapter.pluginID] == null)
				_GetLastTempColor[adapter.pluginID] =
				    new ArrayPrefs("EModules/Hierarchy/LastTempColor" + adapter.pluginID);
				    
			var sl = new SingleList() {list = _GetLastTempColor[adapter.pluginID].Value};
			
			if (sl.list.Count < 5)
			{	sl.list.AddRange(Enumerable.Repeat(0, 5));
			}
			
			var _result = new TempColorClass();
			_result.AssignFromList(sl);
			
			if (!_result.HAS_BG_COLOR)
			{	var lastBG = Hierarchy_GUI.GetLastHiglightList(adapter);
			
				if (lastBG.Count == 0) _result.BGCOLOR = Color.white;
				else _result.BGCOLOR = lastBG[0];
				
				_result.BG_ALIGMENT_LEFT = (int) Adapter.BgAligmentLeft.Fold;
				_result.BG_ALIGMENT_RIGHT = (int) Adapter.BgAligmentLeft.EndLabel;
			}
			
			if (!_result.HAS_LABEL_COLOR)
			{	var lastText = Hierarchy_GUI.GetLastHiglightTextList(adapter);
			
				if (lastText.Count == 0) _result.LABELCOLOR = Color.white;
				else _result.LABELCOLOR = lastText[0];
			}
			
			return _result;
		}
		
		internal void SetLastTempColor(Adapter adapter)
		{	if (_GetLastTempColor[adapter.pluginID] == null)
				_GetLastTempColor[adapter.pluginID] =
				    new ArrayPrefs("EModules/Hierarchy/LastTempColor" + adapter.pluginID);
				    
			TempColorClass tempColor = new TempColorClass();
			tempColor.AssignFromList(new SingleList() {list = this.ToList()});
			var last = GetLastTempColor(adapter);
			
			if (!tempColor.HAS_BG_COLOR && last.HAS_BG_COLOR)
			{	CopyFromTo(CopyType.BG, from: last, to: ref tempColor);
			}
			
			if (!tempColor.HAS_LABEL_COLOR && last.HAS_LABEL_COLOR)
			{	CopyFromTo(CopyType.LABEL, from: last, to: ref tempColor);
			}
			
			_GetLastTempColor[adapter.pluginID].Value = tempColor.ToList();
		}
		
		internal TempColorClass AssignFromList(int v, bool andLock) // var _ConverterFull = this;
		{	BGCOLOR.r = 0;
			BGCOLOR.g = 0;
			BGCOLOR.b = 0;
			BGCOLOR.a = 0;
			
			if (el == null) el = new SingleList() {list = new int[9].ToList()};
			else
				for (int i = 0; i < el.list.Count; i++)
				{	el.list[i] = 0;
				}
				
			//  child = false;
			LABELCOLOR.r = 0;
			LABELCOLOR.g = 0;
			LABELCOLOR.b = 0;
			LABELCOLOR.a = 0;
			LockOverwrite = andLock;
			return this;
		}
		
		internal TempColorClass AssignFromList(SingleList source, bool andLock)
		{	var res = AssignFromList(source);
			LockOverwrite = andLock;
			return res;
		}
		
		internal TempColorClass AssignFromList(SingleList source) // var _ConverterFull = this;
		{	Reset(source);
			BGCOLOR.r = (byte) source.list[0];
			BGCOLOR.g = (byte) source.list[1];
			BGCOLOR.b = (byte) source.list[2];
			BGCOLOR.a = (byte) source.list[3];
			child = source.list[4] == 1;
			
			if (source.list.Count > 5)
			{	LABELCOLOR.r = (byte) source.list[5];
				LABELCOLOR.g = (byte) source.list[6];
				LABELCOLOR.b = (byte) source.list[7];
				LABELCOLOR.a = (byte) source.list[8];
			}
			
			return this;
		}
		
		internal TempColorClass CopyFromFilter(TempColorClass source) // var _ConverterFull = this;
		{	// Reset(null);
		
			var L = Mathf.Max(
			            el.list.Count,
			            source.el.list.Count);
			            
			for (int i = 9; i < L; i++)
			{	var _s = i < source.el.list.Count ? source.el.list[i] : 0;
			
				if (i < el.list.Count) el.list[i] = _s;
				else el.list.Add(_s);
			}
			
			BGCOLOR = source.BGCOLOR;
			child = source.child;
			LABELCOLOR = source.LABELCOLOR;
			
			return this;
		}
		
		internal TempColorClass CopyFromList(List<int> source) //var _ConverterFull = this;
		{	// Reset(null);
			BGCOLOR.r = (byte) source[0];
			BGCOLOR.g = (byte) source[1];
			BGCOLOR.b = (byte) source[2];
			BGCOLOR.a = (byte) source[3];
			
			if (source.Count > 5)
			{	LABELCOLOR.r = (byte) source[5];
				LABELCOLOR.g = (byte) source[6];
				LABELCOLOR.b = (byte) source[7];
				LABELCOLOR.a = (byte) source[8];
			}
			
			else
				LABELCOLOR = Color.clear;
				
			var L = Mathf.Max(
			            el.list.Count,
			            source.Count);
			            
			for (int i = 9; i < L; i++)
			{	var _s = i < source.Count ? source[i] : 0;
			
				if (i < el.list.Count) el.list[i] = _s;
				else el.list.Add(_s);
			}
			
			child = source[4] == 1;
			
			
			return this;
		}
		
		/* internal bool child {get {return _mchild ?? (_mchild = el. list[4] == 1).Value; } set { el.SetByte(4, 0, 1, (_mchild = value).Value ? 1 : 0);} }
		 internal bool LABEL_SHADOW {get {return  _mLABEL_SHADOW ?? (_mLABEL_SHADOW = el.GetByte(10, 0, 1) == 1).Value; } }
		 internal int BG_ALIGMENT_LEFT  {get {return  _mBG_ALIGMENT_LEFT ?? (_mBG_ALIGMENT_LEFT = el.GetByte(9, 0, 3) ).Value; } }
		 internal int BG_ALIGMENT_RIGHT  {get {return   _mBG_ALIGMENT_RIGHT ?? (_mBG_ALIGMENT_RIGHT = el.GetByte(9, 3, 3) ).Value;} }
		 internal int BG_FILL  {get {return  _mBG_FILL ?? (_mBG_FILL = el.GetByte(9, 6, 1) ).Value; } }
		 internal int BG_HEIGHT  {get {return  _mBG_HEIGHT ?? (_mBG_HEIGHT = el.GetByte(9, 7, 1)).Value ;} }*/
		internal bool child
		{	get { return el.list[4] == 1; }
		
			set { el.list[4] = value ? 1 : 0; }
		} //set { el.SetByte(4, 0, 1,  value ? 1 : 0);} }
		
		internal bool HAS_BG_COLOR
		{	get { return BGCOLOR.r > 0 || BGCOLOR.g > 0 || BGCOLOR.b > 0 || BGCOLOR.a > 0; }
		}
		
		internal bool HAS_LABEL_COLOR
		{	get { return LABELCOLOR.r > 0 || LABELCOLOR.g > 0 || LABELCOLOR.b > 0 || LABELCOLOR.a > 0; }
		}
		
		internal bool LABEL_SHADOW
		{	get { return el.GetByte(10, 0, 1) == 1; }
		
			set { el.SetByte(10, 0, 1, value ? 1 : 0); }
		}
		
		internal int BG_WIDTH
		{	get
			{	var t = el.GetByte(10, 1, 8);
			
				if (t == 0) return 80;
				
				return t;
			}
			
			set { el.SetByte(10, 1, 8, value); }
		}
		
		internal Adapter.BgAligmentLeft BG_ALIGMENT_LEFT_CONVERTED
		{	get { return Adapter.BgAligmentLeftArray[BG_ALIGMENT_LEFT]; }
		}
		
		internal int BG_ALIGMENT_LEFT
		{	get { return el.GetByte(9, 0, 3); }
		
			set
			{	if (value < 5 && BG_ALIGMENT_RIGHT < 5)
				{	var estv = 4 - value;
				
					if (estv < BG_ALIGMENT_RIGHT) BG_ALIGMENT_RIGHT = estv;
				}
				
				el.SetByte(9, 0, 3, value);
			}
		}
		
		//  internal string[] ALIGMENT_LEFT_CATEGORIES = new [] { "<<Min", "•Icon", "•Label", "Label•", "•Modules"};
		internal string[] ALIGMENT_LEFT_CATEGORIES = new[] {"<<Min", "•Fold", "•Label", "Label•", "•Modules"};
		
		internal Adapter.BgAligmentRight BG_ALIGMENT_RIGHT_CONVERTED
		{	get { return Adapter.BgAligmentToRightArray[BG_ALIGMENT_RIGHT]; }
		}
		
		internal int BG_ALIGMENT_RIGHT
		{	get { return el.GetByte(9, 3, 3); }
		
			set
			{	if (value < 5 && BG_ALIGMENT_LEFT < 5)
				{	var estv = 4 - value;
				
					if (estv < BG_ALIGMENT_LEFT) BG_ALIGMENT_LEFT = estv;
				}
				
				el.SetByte(9, 3, 3, value);
			}
			
			/* get {return    el.GetByte(9, 3, 3) ;} set
			{   if (value < 5 && BG_ALIGMENT_LEFT < 5)
			    {   var estv = 4 - BG_ALIGMENT_LEFT;
			        if (value > estv) BG_ALIGMENT_LEFT = 4 - value;
			    }
			    el.SetByte(9, 3, 3,  value );
			}*/
		}
		
		internal string[] ALIGMENT_RIGHT_CATEGORIES = new[] {"•Fold", "•Label", "Label•", "•Modules", "Max>>"};
		
		//internal string[] ALIGMENT_RIGHT_CATEGORIES = new [] { "•Icon", "•Label", "Label•", "•Modules", "Max>>"};
		internal int BG_FILL
		{	get { return el.GetByte(9, 6, 1); }
		
			set { el.SetByte(9, 6, 1, value); }
		}
		
		internal int BG_HEIGHT
		{	get { return el.GetByte(9, 7, 2); }
		
			set { el.SetByte(9, 7, 2, value); }
		}
		
		internal List<int> ToList()
		{	var result = new List<int>();
			result.Add(BGCOLOR.r);
			result.Add(BGCOLOR.g);
			result.Add(BGCOLOR.b);
			result.Add(BGCOLOR.a);
			result.Add(child == true ? 1 : 0);
			result.Add(LABELCOLOR.r);
			result.Add(LABELCOLOR.g);
			result.Add(LABELCOLOR.b);
			result.Add(LABELCOLOR.a);
			result.Add(BG_ALIGMENT_LEFT << 0 | BG_ALIGMENT_RIGHT << 3 | BG_FILL << 6 | BG_HEIGHT << 7);
			result.Add((LABEL_SHADOW ? 1 : 0) | (BG_WIDTH << 1));
			return result;
		}
		
		
	}
	
	/*static int[] tempI = new int[10];
	// static Color32 tempC = new Color32();
	
	static Color32[] result = new Color32[1];
	static public Color32[] String4ToColor(string[] res)
	{
	
	   //byte[] result = new byte[res.Length];
	   bool error = false;
	   for (int i = 0 ; i < res.Length ; i++)
	   {   int parse;
	       if (!int.TryParse( res[i], out parse ))
	       {   error = true;
	           break;
	       }
	       tempI[i] = parse;
	   }
	
	   if (!error && res.Length >= 4)
	   {   result[0].r = (byte)tempI[0];
	       result[0].g = (byte)tempI[1];
	       result[0].b = (byte)tempI[2];
	       result[0].a = (byte)tempI[3];
	
	       if (res.Length >= 9)
	       {   if (result.Length != 2) Array.Resize( ref result, 2 );
	           result[1].r = (byte)tempI[5];
	           result[1].g = (byte)tempI[6];
	           result[1].b = (byte)tempI[7];
	           result[1].a = (byte)tempI[8];
	       }
	
	       return result;
	       //if (list.Count < res.Length) list.AddRange(Enumerable.Repeat(0, res.Length - list.Count));
	       // for (int i = 0; i < tempI.Length; i++)
	       //    list[i] = tempI[i];
	   }
	
	   return null;
	}*/
	
	
	
	
	
	
	
	static TempColorClass temp = new TempColorClass().empty;
	
	static public TempColorClass String4ToColor(string[] res)
	{	var list = String4ToList(res);
		var el = new SingleList();
		el.list = list;
		temp.AssignFromList(el);
		return temp;
	}
	
	static public List<int> String4ToList(string[] res)
	{	//byte[] result = new byte[res.Length];
		bool error = false;
		List<int> result = new List<int>();
		
		for (int i = 0; i < res.Length; i++)
		{	int parse;
		
			if (!int.TryParse(res[i], out parse))
			{	error = true;
				break;
			}
			
			result.Add(parse);
		}
		
		if (!error)
		{	return result;
			//if (list.Count < res.Length) list.AddRange(Enumerable.Repeat(0, res.Length - list.Count));
			// for (int i = 0; i < tempI.Length; i++)
			//    list[i] = tempI[i];
		}
		
		return null;
	}
	
	static public bool StringToBool(int index, string[] res)
	{	if (res.Length <= index) return false;
	
		return res[index] == "1";
	}
	
	
	
	
	
	
	
	
	internal static GoGuidPair GoGuidPairConstructor(GameObject go)
	{	return new GoGuidPair() { /*GetFileID = GET_INSTANCE_ID,*/ go = go};
	}
	
	internal static GoGuidPair GoGuidPairConstructor_WithoutCache()
	{	return new GoGuidPair() { /*GetFileID = GET_INSTANCE_ID*/ DISABLE_CACHE = true};
	}
	
	internal GoGuidPair tempPair = GoGuidPairConstructor_WithoutCache();
	
	internal GoGuidPair SetPair(Adapter.HierarchyObject _o)
	{	tempPair.go = IS_HIERARCHY() ? _o.go : null;
#pragma warning disable
		tempPair.guid = IS_PROJECT() ? _o.project.guid : "";
#pragma warning restore
		tempPair.path = IS_PROJECT() ? _o.project.assetPath : "";
		return tempPair;
	}
	
	
	
	
}
}