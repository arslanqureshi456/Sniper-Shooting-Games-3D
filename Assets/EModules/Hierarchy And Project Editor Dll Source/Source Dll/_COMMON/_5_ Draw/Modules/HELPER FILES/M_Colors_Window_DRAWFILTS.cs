//#define CLOSE_AFTERICON
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
//namespace EModules
#if PROJECT
	using EModules.Project;
#endif

namespace EModules.EModulesInternal {



internal partial class Adapter {

	internal static string ColorToString( ref Color __c )
	{	Color32 c = __c;
		return c.r + " " + c.g + " " + c.b + " " + c.a;
	}
	internal static string ColorToString( Color __c )
	{	Color32 c = __c;
		return c.r + " " + c.g + " " + c.b + " " + c.a;
	}
	internal static Color ColorFromString( string s )
	{	var a = s.Split(' ');
		return new Color32( byte.Parse( a[0] ), byte.Parse( a[1] ), byte.Parse( a[2] ), byte.Parse( a[3] ) );
	}
	internal static string ListToString<T>( ref List<T> __c )
	{	var result = new System.Text.StringBuilder();
	
		for ( int i = 0 ; i < __c.Count ; i++ )
		{	if ( i != 0 ) result.Append( ' ' );
		
			result.Append( __c[i].ToString() );
		}
		
		return result.ToString();
	}
	internal static List<int> Int32ListFromString( string s )
	{	var result = new List<int>();
		var a = s.Split(' ');
		
		for ( int i = 0 ; i < a.Length ; i++ )
		{	if ( string.IsNullOrEmpty( a[i] ) ) continue;
		
			result.Add( int.Parse( a[i] ) );
		}
		
		return result;
	}
	
	
	[Serializable]
	public class ColorFilter : IEquatable<ColorFilter> {
	
		[SerializeField] int __ENABLE;
		[SerializeField] internal string NAME = "New Filter";
		[SerializeField] internal string NameFilter = "", ComponentFilter = "", TagFilter = "", LayerFilter ="";
		[SerializeField] internal Texture _icon;
		[SerializeField] internal bool hasColorText, hasColorBg, hasColorIcon/*, child*/;
		[SerializeField] internal Color colorBg = Color.white, colorText = Color.white, colorIcon = Color.white;
		[SerializeField] internal List<int> __Aligment;
		internal List<int> _Aligment { get { return __Aligment ?? (__Aligment = Enumerable.Repeat( 0, 5 ).ToList()); } set { __Aligment = value; } }
		internal bool child { get { return _Aligment[4] == 1; } set { _Aligment[4] = value ? 1 : 0; } } //set { el.SetByte(4, 0, 1,  value ? 1 : 0);} }
		internal int GetFilterByNameToCompLength {get {return 4;}}
		internal string GetFilterByNameToComp(int NameToComp)
		{	switch (NameToComp)
			{	case 0: return NameFilter;
			
				case 1: return ComponentFilter;
				
				case 2: return TagFilter;
				
				case 3: return LayerFilter;
			}
			
			return "";
		}
		internal bool IsNullOrEmptyGetFilterByNameToComp(int NameToComp)
		{	switch (NameToComp)
			{	case 0: return string.IsNullOrEmpty( NameFilter.Trim(trimChars));
			
				case 1: return string.IsNullOrEmpty(ComponentFilter.Trim(trimChars));
				
				case 2: return string.IsNullOrEmpty( TagFilter.Trim(trimChars));
				
				case 3: return string.IsNullOrEmpty(LayerFilter.Trim(trimChars));
			}
			
			return false;
		}
		
		
		internal void SaveToString( ref System.Text.StringBuilder result )
		{	result.AppendLine( __ENABLE.ToString() );
			result.AppendLine( NAME.ToString() );
			result.AppendLine( NameFilter.ToString() );
			result.AppendLine( ComponentFilter.ToString() );
			result.AppendLine( TagFilter.ToString() );
			result.AppendLine( !_icon ? "" : string.IsNullOrEmpty( AssetDatabase.GetAssetPath( _icon ) ) ? "" : AssetDatabase.AssetPathToGUID( AssetDatabase.GetAssetPath( _icon ) ) );
			result.AppendLine( hasColorText.ToString() ); result.AppendLine( hasColorBg.ToString() ); result.AppendLine( hasColorIcon.ToString() ); result.AppendLine( "" );
			result.AppendLine( ColorToString( ref colorBg ) ); result.AppendLine( ColorToString( ref colorText ) ); result.AppendLine( ColorToString( ref colorIcon ) );
			result.AppendLine( __Aligment != null ? ListToString( ref __Aligment ) : "" );
			result.AppendLine( LayerFilter.ToString() );
		}
		internal static ColorFilter ReadFromString( ref System.IO.StringReader reader )
		{	var result = new ColorFilter();
			//  using (var reader = new System.IO.StringReader(str))
			{	result.__ENABLE = int.Parse( reader.ReadLine() );
				result.NAME = (reader.ReadLine());
				result.NameFilter = (reader.ReadLine());
				result.ComponentFilter = (reader.ReadLine());
				result.TagFilter = (reader.ReadLine());
				var icon = reader.ReadLine();
				var path = string.IsNullOrEmpty(icon) ? "" : AssetDatabase.GUIDToAssetPath(icon);
				result._icon = string.IsNullOrEmpty( path ) ? null : AssetDatabase.LoadAssetAtPath<Texture>( path );
				result.hasColorText = bool.Parse( reader.ReadLine() ); result.hasColorBg = bool.Parse( reader.ReadLine() );
				result.hasColorIcon = bool.Parse( reader.ReadLine() ); /*result.child =*/ bool.Parse( reader.ReadLine() );
				result.colorBg = ColorFromString( reader.ReadLine() ); result.colorText = ColorFromString( reader.ReadLine() ); result.colorIcon = ColorFromString( reader.ReadLine() );
				result._Aligment = Int32ListFromString( reader.ReadLine() );
				
				try
				{	result.LayerFilter = (reader.ReadLine());
				
				}
				
				catch
				{
				
				}
			}
			return result;
		}
		
		
		[NonSerialized] static Dictionary<int, int> __id = new Dictionary<int, int>();
		[NonSerialized] int? _id;
		[NonSerialized] internal static SingleList __SingleList = new SingleList();
		[NonSerialized] internal  static  TempColorClass __TempColorClass = new TempColorClass();
		[NonSerialized] static Dictionary<string,  States[]>  __GetAllStates = new Dictionary<string, States[]>();
		
		
		public bool Equals( ColorFilter other )
		{	return this == other;
		}
		
		internal bool ENABLE
		{	get { return __ENABLE == 0; }
		
			set { if ( value ) __ENABLE = 0; else __ENABLE = 1; }
		}
		int id
		{	get
			{	if ( _id.HasValue ) return _id.Value;
			
				var genV = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
				
				while ( __id.ContainsKey( genV ) ) genV = UnityEngine.Random.Range( int.MinValue, int.MaxValue );
				
				_id = genV;
				__id.Add( genV, genV );
				return _id.Value;
			}
		}
		public static bool operator ==( ColorFilter a, ColorFilter b )
		{	if ( ReferenceEquals( a, null ) && ReferenceEquals( b, null ) ) return true;
		
			if ( ReferenceEquals( a, null ) || ReferenceEquals( b, null ) ) return false;
			
			return a.id == b.id;
		}
		public static bool operator !=( ColorFilter a, ColorFilter b )
		{	return !(a == b);
		}
		public override bool Equals( object obj )
		{	return this == (obj as ColorFilter);
		}
		public override int GetHashCode()
		{	return id;
		}
		
		[NonSerialized] public const string STARTWITH = "<";
		[NonSerialized] public const string ENDWITH = ">";
		[NonSerialized] public const string AND = "+";
		[NonSerialized] public const string OR = "`";
		[NonSerialized] public const string NOT = "!";
		[NonSerialized] public const string IGNORECASE = "*";
		[NonSerialized] public const string ACTIVE = "╬@╥";
		[NonSerialized] public const char SEPARATOR = '╥';
		static char[] trimChars =
		    STARTWITH.ToCharArray().Concat(
		        ENDWITH.ToCharArray().Concat(
		            AND.ToCharArray().Concat(
		                OR.ToCharArray().Concat(
		                    NOT.ToCharArray().Concat(
		                        IGNORECASE.ToCharArray().Concat(
		                            ACTIVE.ToCharArray().Concat( new[] {SEPARATOR, ' ', '\n'}
		                                                       ))))))).ToArray();
		                                                       
		                                                       
		internal List<int> Aligment
		{	get
			{	if ( _Aligment == null ) _Aligment = new List<int>();
			
				if ( _Aligment.Count < 5 ) _Aligment.AddRange( Enumerable.Repeat( 0, 5 ) );
				
				return _Aligment;
			}
		}
		
		
		internal bool LABEL_SHADOW
		{	get
			{	__SingleList.list = Aligment;
				return __SingleList.GetByte( 10, 0, 1 ) == 1;
			}
			
			set
			{	__SingleList.list = Aligment;
				__SingleList.SetByte( 10, 0, 1, value ? 1 : 0 );
			}
		}
		
		internal int BG_HEIGHT
		{	get
			{	__SingleList.list = Aligment;
				return __SingleList.GetByte( 9, 7, 2 );
			}
			
			set
			{	__SingleList.list = Aligment;
				__SingleList.SetByte( 9, 7, 2, value );
			}
		}
		
		
		internal TempColorClass AS_TEMPCOLOR_ALIGN_ONLY
		{	get
			{	__SingleList.list = Aligment.ToList();
				__TempColorClass.AssignFromList( __SingleList );
				__TempColorClass.BGCOLOR = hasColorBg ? colorBg : Color.clear;
				__TempColorClass.LABELCOLOR = hasColorText ? colorText : Color.clear;
				__TempColorClass.child = child;
				// __TempColorClass.child = child;
				
				__TempColorClass.add_icon = _icon;
				__TempColorClass.add_hasiconcolor = hasColorIcon;
				
				if ( __TempColorClass.add_hasiconcolor )
					__TempColorClass.add_iconcolor = colorIcon;
				else
					__TempColorClass.add_iconcolor = Color.clear;
					
				return __TempColorClass;
			}
			
			set
			{	_Aligment = value.el.list.ToList();
				child = value.child;
				colorBg = value.BGCOLOR;
				hasColorBg = value.HAS_BG_COLOR;
				colorText = value.LABELCOLOR;
				hasColorText = value.HAS_LABEL_COLOR;
				// child = value.child;
				
				hasColorIcon = value.add_hasiconcolor;
				_icon = value.add_icon;
				colorIcon = value.add_iconcolor;
			}
		}
		
		public class States : ICloneable {
			public States() { }
			public States( bool AND,
			               bool STARTWITH,
			               bool ENDWITH,
			               bool OR,
			               bool NOT,
			               bool IGNORECASE,
			               string filter )
			{	this.AND = AND;
				this.STARTWITH = STARTWITH;
				this.ENDWITH = ENDWITH;
				this.OR = OR;
				this.NOT = NOT;
				this.IGNORECASE = IGNORECASE;
				this.filter = filter;
			}
			
			public bool STARTWITH { get; private set; }
			public bool ENDWITH { get; private set; }
			bool __AND = true;
			public bool AND { get { return __AND; } private set { __AND = value; } }
			public bool OR { get; private set; }
			public bool NOT { get; private set; }
			public bool IGNORECASE { get; private set; }
			public string filter { get; private set; }
			
			public enum Compar { Contains, StartWith, EndWith, Equals }
			public Compar GetCompar
			{	get
				{	if ( !STARTWITH && !ENDWITH ) return Compar.Contains;
				
					if ( STARTWITH && !ENDWITH ) return Compar.StartWith;
					
					if ( !STARTWITH && ENDWITH ) return Compar.EndWith;
					
					return Compar.Equals;
				}
			}
			
			object ICloneable.Clone()
			{	return MemberwiseClone();
			}
			public States Clone()     //var res// = new State();
			{	return MemberwiseClone() as ColorFilter.States;
			}
			public States SWAP_STARTWITH( bool v ) { var s = Clone(); s.STARTWITH = v; return s; }
			public States SWAP_ENDWITH( bool v ) { var s = Clone(); s.ENDWITH = v; return s; }
			public States SWAP_AND( bool v ) { var s = Clone(); s.AND = v; return s; }
			public States SWAP_OR( bool v ) { var s = Clone(); s.OR = v; return s; }
			public States SWAP_NOT( bool v ) { var s = Clone(); s.NOT = v; return s; }
			public States SWAP_IGNORECASE( bool v ) { var s = Clone(); s.IGNORECASE = v; return s; }
			
			
			public string ConvertToString()
			{	var result = filter;
			
				if ( STARTWITH ) result = ColorFilter.STARTWITH + SEPARATOR + result;
				
				if ( ENDWITH ) result = ColorFilter.ENDWITH + SEPARATOR + result;
				
				if ( AND ) result = ColorFilter.AND + SEPARATOR + result;
				
				if ( OR ) result = ColorFilter.OR + SEPARATOR + result;
				
				if ( NOT ) result = ColorFilter.NOT + SEPARATOR + result;
				
				if ( IGNORECASE ) result = ColorFilter.IGNORECASE + SEPARATOR + result;
				
				return ColorFilter.ACTIVE + result;
			}
			
			internal string GetComparationString()
			//  {   return !STARTWITH && !ENDWITH ? "=..A.." : STARTWITH && !ENDWITH ? "=A..." : ENDWITH && !STARTWITH ? "=...A" : " == " ;
			{	return !STARTWITH && !ENDWITH ? "\"..A..\"" : STARTWITH && !ENDWITH ? "\"A...\"" : ENDWITH && !STARTWITH ? "\"...A\"" : "\"==\"";
			}
			
			internal string GetConditionString()
			//  {   return OR ? "||" : "&&";
			{	return !AND ? "[OR]" : "[AND]";
			}
			
			
		}
		
		public States[] AllStatesForName { get { return (GetAllStates( NameFilter )); } }
		public States[] AllStatesForComps { get { return (GetAllStates( ComponentFilter )); } }
		public States[] AllStatesForLayerss { get { return (GetAllStates( LayerFilter )); } }
		public States[] AllStatesForTagss { get { return (GetAllStates( TagFilter )); } }
		public States[] GetAllStates( string text )
		{	if ( __GetAllStates.ContainsKey( text ) ) return __GetAllStates[text];
		
			var conditions = text.Split('╩').Select(c => c == null ? "" : c).ToArray();
			
			if ( conditions.Length == 0 ) return new States[1] { new States() };
			
			var result = new States[conditions.Length];
			
			for ( int i = 0 ; i < conditions.Length ; i++ )
			{	var _case = conditions[i].Split(SEPARATOR);
			
				if ( _case.Length == 0 ) _case = new string[1] { "" };
				
				var or = _case.Any(s => s == OR);
				
				var state = new States(
				    AND : ( _case.Any(s => s == AND) || ! or ),
				    STARTWITH : _case.Any(s => s == STARTWITH),
				    ENDWITH : _case.Any(s => s == ENDWITH),
				    OR : or,
				    NOT : _case.Any(s => s == NOT),
				    IGNORECASE : _case.Any(s => s == IGNORECASE),
				    filter : _case.Last()
				);
				
				result[i] = state;
			}
			
			return result;
		}
		
		
	}
}

public partial class M_Colors_Window : _W___IWindow {







	/*  new void Repaint()
	  {   Debug.Log("R");
	      base.Repaint();
	  }*/
	
	
	
	static  Dictionary<EditorWindow, FilterHelper> __FH = new Dictionary<EditorWindow, FilterHelper>();
	static internal FilterHelper GetFH( EditorWindow w )
	{	if ( __FH.ContainsKey( w ) ) return __FH[w];
	
		__FH.Add( w, new FilterHelper( w, adapter ) );
		return
		    __FH[w];
		    
	}
	
	void DrawFilts( Rect rect )
	{	var f = GetFH(this);
		f.source = source;
		f.DrawFilts( rect, adapter );
	}
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	internal class FilterHelper {
	
		float FilterLineHeight = 16;
		
		
		internal Adapter.HierarchyObject source ;
		internal FilterHelper( EditorWindow w, Adapter a )
		{	ew = w;
			adapter = a;
		}
		void Repaint()
		{	ew.Repaint();
			adapter.RepaintWindowInUpdate();
		}
		EditorWindow ew;
		GUIContent emptyContent = new GUIContent();
		internal Adapter adapter;
		
		
		void CHANGE_GUI() { M_Colors_Window.CHANGE_GUI( adapter ); }
		
		static Dictionary < int, float > _scrollPos = new Dictionary < int, float >();
		float scrollPos
		{	get
			{	if ( !_scrollPos.ContainsKey( adapter.pluginID ) ) _scrollPos.Add( adapter.pluginID, EditorPrefs.GetFloat( "EModules/Hierarchy/FilterScroll" + adapter.pluginID, 0 ) );
			
				return _scrollPos[adapter.pluginID];
			}
			
			set
			{	if ( value != _scrollPos[adapter.pluginID] ) EditorPrefs.SetFloat( "EModules/Hierarchy/FilterScroll" + adapter.pluginID, value );
			
				_scrollPos[adapter.pluginID] = value;
			}
		}
		Dictionary < int, int >[] _editIndex = new Dictionary < int, int >[2];
		Dictionary<int, int> editIndex
		{	get
			{	if ( _editIndex[adapter.pluginID] == null )
				{	_editIndex[adapter.pluginID] = new Dictionary<int, int>();
					int i = 0;
					int key;
					
					while ( (key = EditorPrefs.GetInt( "EModules/Hierarchy/EditIndex" + adapter.pluginID + " " + i, -1 )) != -1 )
					{	_editIndex[adapter.pluginID].Add( key, -1 );
						i++;
					}
				}
				
				//if (!_editIndex.ContainsKey(adapter.pluginID)) _editIndex.Add(adapter.pluginID, EditorPrefs.GetInt("EModules/Hierarchy/EditIndex" + adapter.pluginID, -1) );
				return _editIndex[adapter.pluginID];
			}
			
			set
			{
			
			
				_editIndex[adapter.pluginID] = value;
				currentY = new float[0];
				int i = 0;
				
				while ( EditorPrefs.GetInt( "EModules/Hierarchy/EditIndex" + adapter.pluginID + " " + i, -1 ) != -1 )
				{	EditorPrefs.DeleteKey( "EModules/Hierarchy/EditIndex" + adapter.pluginID + " " + i );
					i++;
				}
				
				i = 0;
				
				foreach ( var item in value )
				{	EditorPrefs.SetInt( "EModules/Hierarchy/EditIndex" + adapter.pluginID + " " + i, item.Key );
					i++;
				}
				
				//  if (value != _editIndex[adapter.pluginID])
				//     _editIndex[adapter.pluginID] = value;
			}
		}
		
		
		
		
		
		
		
		/// <summary>
		/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// ///////////////////////////MAIN DRAWER////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// </summary>
		
		Vector2 dragPos;
		// float MouseY;
		float[] currentH = new float[0];
		float[] currentY = new float[0];
		float[] targetY = new float[0];
		
		internal float DrawFilts( Rect inputrect, Adapter a )
		{	// if ( Event.current.type == EventType.Layout && currentY.Length != Hierarchy_GUI.Instance( a ).ColorFilters.Count ) return 0;
			adapter = a;
			M_Colors_Window.adapter = a;
			
			if ( inputrect.height != -1 ) inputrect.height -= 10;
			
			inputrect.width -= 4;
			
			
			
			
			
			
			var W = inputrect.width;
			var PAD = 2;
			var H = FilterLineHeight + 2 + PAD;
			var EDIT_H = FilterLineHeight * 4 + 23  * 3 + EditorStyles.toolbarButton.fixedHeight * 2;
			
			var customFilters = Hierarchy_GUI.Instance(adapter).ColorFilters;
			
			var lr = inputrect;
			lr.height = FilterLineHeight;
			//INTERNAL_BOX( new Rect( 0, 0, Math.Min( lr.width, W ) - 15, CusomIconsHeight ), PlusContentEmpty );
			//MouseY = EditorGUIUtility.GUIToScreenPoint( Vector2.zero ).y;
			
			// if (editIndex != -1) editIndex = Mathf.Clamp(editIndex, 0, customIcons.Count - 1);
			var startY = inputrect.height == -1 ? inputrect.y : 0f;
			
			if ( currentY.Length != customFilters.Count )
			{	currentY = new float[customFilters.Count];
				targetY = new float[customFilters.Count];
				currentH = new float[customFilters.Count];
				float _y_interator = startY;
				
				for ( int i = 0 ; i < customFilters.Count ; i++ )
				{	currentY[i] = _y_interator;
					currentH[i] = H;
					
					if ( editIndex.ContainsKey( i ) )     // currentY[i] += EDIT_H;
					{	currentH[i] += EDIT_H;
					}
					
					targetY[i] = currentY[i];
					
					_y_interator += currentH[i];
				}
			}
			
			var contentH = currentY.Length != 0 ? targetY[currentY.Length - 1] + currentH[currentH.Length - 1] + EDIT_H : (startY + EDIT_H) ;
			
			//if (editIndex != -1)contentH += EDIT_H;
			if ( inputrect.height != -1 )
			{	var scrollHeightOver =  inputrect.height != -1 &&    contentH > inputrect.height;
				scrollPos = GUI.BeginScrollView( inputrect, new Vector2( 0, scrollPos ), new Rect( 0, 0, inputrect.width - (scrollHeightOver ? GUI.skin.verticalScrollbar.fixedWidth : 0), contentH ) ).y;
				W -= 10;
				
				if ( scrollHeightOver ) W -= GUI.skin.verticalScrollbar.fixedWidth;
			}
			
			for ( int i = 0 ; i < customFilters.Count && i < currentY.Length ; i++ )
			{	var r = new Rect(lr.x,  currentY[i], W, currentH[i] - PAD);
			
				if ( dragIndex == i )     // r.x = Event.current.mousePosition.x - r.height / 2;
				{	r.y = Event.current.mousePosition.y - dragPos.y - 1;
				}
				
				DrawLine( i, r );
			}
			
			
			// if (currentY.Length != 0) startY = targetY.Last() + H;
			/*  var olds = Adapter.GET_SKIN().button.alignment;
			  Adapter.GET_SKIN().button.alignment = TextAnchor.MiddleCenter;*/
			var lineRect = new Rect(lr.x,  contentH + PAD, W,  EDIT_H - PAD * 2);
			lineRect.y -= EDIT_H + PAD;
			// if (editIndex == currentY.Length - 1 && currentY.Length != 0)lineRect.y += EDIT_H;
			GUI.Box( lineRect, "" );
			//  ExampleDragDropGUI( lineRect, null, DRAG_VALIDATOR_MONOANDTEXTURE, DRAG_PERFORM_USERICONS );
			
			/*  if (lineRect.Contains(Event.current.mousePosition))
			  {
			      if (Event.current.type.Equals(EventType.Repaint)) GUI.DrawTexture(lineRect,Hierarchy.sec);
			  }*/
			
			// lineRect.width -= 15;
			if ( Button( lineRect, PlusContent, TextAnchor.MiddleCenter ) )
			{	if ( Event.current.button == 0 )
				{	FromObjectButton( source, int.MaxValue );
				}
			}
			
			// Adapter.GET_SKIN().button.alignment = olds;
			
			
			if ( inputrect.height != -1 )
				GUI.EndScrollView();
				
				
				
				
			if ( (Event.current.type == EventType.Repaint || Event.current.rawType == EventType.MouseUp || Event.current.rawType == EventType.Used) && currentY.Length != 0 && (dragIndex != -1
			        || Event.current.rawType == EventType.MouseUp || Event.current.rawType == EventType.Used
			|| currentY.Select( ( c, i ) => new { c, t = targetY[i] }
			                          ).Any( ( c ) => Mathf.Abs( c.t - c.c ) > 0.001f )) )
			{
			
				//   var estimPos = Mathf.RoundToInt((EditorGUIUtility.GUIToScreenPoint(Event.current.mousePosition).y  - inputrect.y - MouseY - (H - PAD) / 2) / (float)H);
				var mp = Event.current.mousePosition.y ;
				
				if ( inputrect.height != -1 ) mp -= inputrect.y;
				
				int I = 0;
				
				for ( I = 0 ; I < currentY.Length ; I++ ) if ( mp < targetY[I] + currentH[I] ) break;
				
				var estimPos = I;
				
				tempDragIndex = dragIndex == -1 ? -1 : Mathf.Clamp(estimPos, 0, targetY.Length - 1);
				
				if ( Event.current.type == EventType.Repaint && currentY.Length != 0 )
				{	bool notCompleted = false;
				
					for ( int i = 0, sib = 0 ; i < currentY.Length ; i++, sib++ )
					{	if ( dragIndex != -1 && i > dragIndex && i <= tempDragIndex ) sib = i - 1;
						else
							if ( dragIndex != -1 && i < dragIndex && i >= tempDragIndex ) sib = i + 1;
							else sib = i;
							
						sib = Mathf.Clamp( sib, 0, currentY.Length - 1 );
						currentY[i] = Mathf.Lerp( currentY[i], targetY[sib] /*+ currentH[sib] - (dragIndex == sib ? H + EDIT_H : H)*/, 0.25f );
						var fin = Mathf.Abs(currentY[i] - targetY[sib]) <= 1;
						notCompleted |= fin;
						
						if ( fin ) currentY[i] = targetY[sib];
					}
					
					//print(tempDragIndex);
					if ( dragIndex != -1 || notCompleted )
					{	Repaint();
					}
				}
				
				if ( Event.current.rawType == EventType.MouseUp || Event.current.rawType == EventType.Used )
				{	OnRawUp();
				}
			}
			
			
			
			if ( Event.current.keyCode == KeyCode.Escape ) dragIndex = -1;
			
			
			
			
			
			
			
			return contentH;
			
		}
		GUIContent PlusContent = new GUIContent()
		{	text = "Create New",
			    tooltip = "Create new filter from current selection"
		};
		
		
		int __dragIndex = -1;
		int dragIndex
		{	get
			{	return __dragIndex;
			}
			
			set
			{	__dragIndex = value;
			
				if (value != -1 )
				{	var win = ew as _W___IWindow;
				
					if (win)
						win. PUSH_ONMOUSEUP( OnRawUp, win);
				}
			}
		}
		
		
		int    tempDragIndex;
		
		void OnRawUp()
		{	if ( dragIndex != -1 && tempDragIndex != -1 && tempDragIndex != dragIndex )
			{	Swap( dragIndex, tempDragIndex );
				//  if (editIndex == tempDragIndex) editIndex = dragIndex;
				currentY = new float[0];
				SaveFilters( null );
				
			}
			
			dragIndex = -1;
		}
		
		
		
		
		
		
		void ToObjectButton( Adapter.HierarchyObject reference, int index )
		{	if ( source == null ) return;
		
			if ( index > Hierarchy_GUI.Instance( adapter ).ColorFilters.Count ) return;
			
			var targetFilter =  Hierarchy_GUI.Instance( adapter ).ColorFilters[index];
			
			
			SetIconImage( targetFilter._icon, "Apply Filter To Object" );
			
			if ( targetFilter.hasColorIcon ) SetIconColor( targetFilter.colorIcon, "Apply Filter To Object" );
			else SetIconColor( Color.clear, "Apply Filter To Object" );
			
			//  var tempColor = new Adapter.TempColorClass().AssignFromList(new SingleList() { list = Enumerable.Repeat(0, 9).ToList()});
			var tempColor = new Adapter.TempColorClass().AssignFromList(0, false);
			targetFilter.AS_TEMPCOLOR_ALIGN_ONLY.OverrideTo( ref tempColor );
			SetHiglightData( tempColor, "Apply Filter To Object" );
		}
		
		
		void FromObjectButton( Adapter.HierarchyObject reference, int index )
		{
		
			bool hasColorText = false, hasColorBg = false, hasColorIcon = false;
			Color colorText = Color.black, colotBg = Color.black, colorIcon = Color.black;
			bool child = false;
			Texture2D _icon = null;
			string _name = "New name";
			
			//Load
			Adapter.TempColorClass data;
			
			if ( reference != null )
			{	data = GetHiglightData();
				var cont =  adapter.ColorModule.GET_CONTENT(reference);
				_icon = cont.add_icon as Texture2D;
				hasColorIcon = cont.add_hasiconcolor;
				colorIcon = cont.add_iconcolor;
				_name = reference.name;
				ListToBgColor( GetIconColor(), out colorIcon, out hasColorIcon );
			}
			
			else
			{	data = new Adapter.TempColorClass();
				data = data.GetLastTempColor( adapter );
			}
			
			ListToTextColor( data, out colorText, out hasColorText );
			ListToBgColor( data, out colotBg, out hasColorBg );
			child = data.child;
			
			//Load
			
			var value = new Adapter.ColorFilter()
			{	hasColorText = hasColorText,
				    hasColorBg = hasColorBg,
				    hasColorIcon = hasColorIcon,
				    colorText = colorText,
				    colorBg = colotBg,
				    colorIcon = colorIcon,
				    _icon = _icon,
				    // child = child,
				    NAME = _name,
				    NameFilter = Adapter.ColorFilter.ENDWITH + Adapter.ColorFilter.SEPARATOR + Adapter.ColorFilter.STARTWITH + Adapter.ColorFilter.SEPARATOR +
				                 _name,
				                 ComponentFilter = "",
				                 TagFilter = "", LayerFilter = ""
			};
			
			
			
			
			
			
			var tempColor = value.AS_TEMPCOLOR_ALIGN_ONLY;
			tempColor.child = data.child;
			tempColor.BG_WIDTH = data.BG_WIDTH;
			tempColor.BG_HEIGHT = data.BG_HEIGHT;
			tempColor.LABEL_SHADOW = data.LABEL_SHADOW;
			tempColor.BG_ALIGMENT_LEFT = data.BG_ALIGMENT_LEFT;
			tempColor.BG_ALIGMENT_RIGHT = data.BG_ALIGMENT_RIGHT;
			value.AS_TEMPCOLOR_ALIGN_ONLY = tempColor;
			
			
			
			
			
			
			
			
			
			
			Hierarchy_GUI.Undo( adapter, "Change Color Filters" );
			
			
			Adapter.ColorFilter oldFilter = null;
			
			if ( index >= Hierarchy_GUI.Instance( adapter ).ColorFilters.Count )
			{	index = Hierarchy_GUI.Instance( adapter ).ColorFilters.Count;
				Hierarchy_GUI.Instance( adapter ).ColorFilters.Add( value );
			}
			
			else
			{	oldFilter = Hierarchy_GUI.Instance( adapter ).ColorFilters[index];
				Hierarchy_GUI.Instance( adapter ).ColorFilters[index] = value;
			}
			
			if ( oldFilter != null )
			{	Hierarchy_GUI.Instance( adapter ).ColorFilters[index].ENABLE = oldFilter.ENABLE;
				Hierarchy_GUI.Instance( adapter ).ColorFilters[index].NAME = oldFilter.NAME;
				Hierarchy_GUI.Instance( adapter ).ColorFilters[index].NameFilter = oldFilter.NameFilter;
				Hierarchy_GUI.Instance( adapter ).ColorFilters[index].ComponentFilter = oldFilter.ComponentFilter;
				Hierarchy_GUI.Instance( adapter ).ColorFilters[index].TagFilter = oldFilter.TagFilter;
				Hierarchy_GUI.Instance( adapter ).ColorFilters[index].LayerFilter = oldFilter.LayerFilter;
			}
			
			
			
			SaveFilters( null );
			
		}
		void RemoveLine( int index )
		{	if ( index < 0 || index >= Hierarchy_GUI.Instance( adapter ).ColorFilters.Count ) return;
		
			Hierarchy_GUI.Undo( adapter, "Change Color Filters" );
			
			Hierarchy_GUI.Instance( adapter ).ColorFilters.RemoveAt( index );
			
			SaveFilters( null );
			
		}
		void Swap( int i1, int i2 )
		{	Hierarchy_GUI.Undo( adapter, "Change Color Filters" );
		
			var v1 = Hierarchy_GUI.Instance( adapter ).ColorFilters[i1];
			Hierarchy_GUI.Instance( adapter ).ColorFilters.RemoveAt( i1 );
			
			if ( i2 >= Hierarchy_GUI.Instance( adapter ).ColorFilters.Count ) Hierarchy_GUI.Instance( adapter ).ColorFilters.Add( v1 );
			else Hierarchy_GUI.Instance( adapter ).ColorFilters.Insert( i2, v1 );
			
			SaveFilters( null );
		}
		
		
		
		
		
		
		
		
		/// <summary>
		/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// ////////////////////////////////HEADER FUNCS//////////////////////////////////////////////////////////////////////////////////////////////////////
		/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// </summary>
		
		
		
		
		private void DrawColoredLabel( Rect labelRect, Adapter.ColorFilter filter )
		{	EditorGUIUtility.AddCursorRect( labelRect, MouseCursor.Text );
		
			// Adapter.INTERNAL_BOX( labelRect );
			
			//labelRect = Shrink( labelRect, 1 );
			var tempColor = filter.AS_TEMPCOLOR_ALIGN_ONLY;
			
			if ( filter.hasColorBg )
			{	var bgcolorold = filter.colorBg;
				var bgrect = labelRect;
				
				if ( tempColor.BG_HEIGHT == 1 )
				{	var newH = bgrect.height - label.CalcHeight(Adapter.GET_CONTENT(filter.NAME), 10000) ;
					bgrect.y += newH / 2;
					bgrect.height += newH;
				}
				
				else
					if ( tempColor.BG_HEIGHT == 2 )
					{	bgrect.y += bgrect.height / 2;
						bgrect.height = 1;
					}
					
				bgcolorold.a *= adapter.par.highligterOpacity;
				
				adapter.ColorModule.DRAW_BGTEXTURE_OLD( bgrect, bgcolorold );
				// EditorGUI.DrawRect( bgrect, bgcolorold );
			}
			
			else
			{	EditorGUI.DrawRect( labelRect, Adapter.EditorBGColor );
			}
			
			var c = label.normal.textColor;
			
			if ( filter.hasColorText )
			{	label.normal.textColor = filter.colorText;
			}
			
			if ( filter.hasColorText && tempColor.LABEL_SHADOW == true )
			{
			
				var _oc2 =  label.normal.textColor;
				var c2 = Color.black;
				c2.a = _oc2.a;
				label.normal.textColor = c2;
				labelRect.y -= 0.5f;
				labelRect.x -= 1f;
				Label( labelRect, filter.NAME );
				label.normal.textColor = _oc2;
				labelRect.y += 0.5f;
				labelRect.x += 1f;
			}
			
			
			Label( labelRect, filter.NAME );
			label.normal.textColor = c;
			
			
		}
		
		
		
		
		void DrawFilterText( ref string text, Adapter.ColorFilter filter, int case_, Rect pos )
		{	var conditions = text.Split('╩').Select(c => c == null ? "" : c).ToArray();
		
			if ( conditions.Length == 0 ) conditions = new string[1] { "" };
			
			//  var activeIndex = Array.FindIndex(conditions,  s => s.StartsWith( Adapter.ColorFilter.ACTIVE));
			var activeIndex = conditions.ToList().FindIndex( s => s.StartsWith( Adapter.ColorFilter.ACTIVE));
			
			//   if (case_ == 0) Debug.Log(conditions[0] + " " + conditions[1] + " " + conditions[1][0] + " " + activeIndex);
			if ( activeIndex == -1 ) activeIndex = 0;
			
			
			var _case =  conditions[activeIndex].Split(Adapter.ColorFilter.SEPARATOR);
			
			if ( _case.Length == 0 ) _case = new string[1] { "" };
			
			pos.width -= pos.height * 1.5f;
			
			var newName = GUI.TextField(pos, _case.Last(), textFieldStyle).Trim(' ');
			
			if ( newName != _case.Last() )
			{	_case[_case.Length - 1] = newName;
				conditions[activeIndex] = _case.Aggregate( ( a, b ) => a + Adapter.ColorFilter.SEPARATOR + b );
				text = SetDirtyConditions( ref conditions );
				/* currentFilter = filter;
				 if ( case_ == 0) SetDirtyConditions(ref conditions, SetTextAction0);
				 else SetDirtyConditions(ref conditions, SetTextAction1);*/
				// Debug.Log(conditions[activeIndex]);
			}
			
			
			pos.x += pos.width;
			pos.width = pos.height * 1.2f;
			
			RESTORE_GUI();
			
			F1();
			var but =  Button(pos, "▼");
			F2();
			
			if ( but )
			{	var menu = new GenericMenu();
				currentFilter = filter;
				Action<string> setTextAction = GetActionByIndex(case_);
				
				for ( int i = 0 ; i < conditions.Length ; i++ )
				{	var captureI = i;
					var states = currentFilter.GetAllStates(text)[captureI];
					var name =  states.GetComparationString() + " " +  ConditionToName(ref conditions[i]) + "";
					
					if ( states.NOT ) name = "NOT " + name;
					
					if ( i != 0 ) name = states.GetConditionString() + " " + name;
					
					menu.AddItem( new GUIContent( name ), activeIndex == i, () =>
					{	SetActiveCondition( ref conditions, setTextAction, captureI );
					} );
				}
				
				menu.AddSeparator( "" );
				menu.AddItem( new GUIContent( "Create New Condition" ), false, () =>
				{	Array.Resize( ref conditions, conditions.Length + 1 );
					conditions[conditions.Length - 1] = "";
					SetActiveCondition( ref conditions, setTextAction, conditions.Length - 1 );
				} );
				
				if ( conditions.Length > 1 )
					menu.AddItem( new GUIContent( "Remove" ), false, () =>
				{	UnityEditor.ArrayUtility.RemoveAt( ref conditions, activeIndex );
					SetActiveCondition( ref conditions, setTextAction, Mathf.Clamp( activeIndex, 0, conditions.Length - 1 ) );
				} );
				else menu.AddDisabledItem( new GUIContent( "Remove" ) );
				
				menu.ShowAsContext();
			}
			
			CHANGE_GUI();
		}
		int ofs;
		FontStyle ob ;
		void F1()
		{	ofs = GUI.skin.button.fontSize;
			ob = GUI.skin.button.fontStyle;
			GUI.skin.button.fontSize = Mathf.RoundToInt( FilterLineHeight / 2 );
			GUI.skin.button.fontStyle = FontStyle.Bold;
		}
		void F2()
		{	GUI.skin.button.fontSize = ofs;
			GUI.skin.button.fontStyle = ob;
		}
		void DrawFilterConditions( ref string text, Adapter.ColorFilter filter, int case_, Rect pos, bool divWidth = true )
		{	var conditions = text.Split('╩').Select(c => c == null ? "" : c).ToArray();
		
			if ( conditions.Length == 0 ) conditions = new string[1] { "" };
			
			var activeIndex = conditions.ToList().FindIndex( s => s.StartsWith( Adapter.ColorFilter.ACTIVE));
			
			if ( activeIndex == -1 ) activeIndex = 0;
			
			var states = filter.GetAllStates(text)[activeIndex];
			var r = pos;
			
			if ( divWidth ) r.width /= 3f;
			else r.height /= 3;
			
			RESTORE_GUI();
			
			string o1;
			bool B1;
			
			if ( activeIndex != 0 )
			{	GUI.enabled = !string.IsNullOrEmpty( states.filter );
				o1 = states.GetConditionString();
				B1 = false;
				
				F1();
				
				if ( divWidth ) B1 = Button( r, new GUIContent( o1, o1 + "\nConditions for several conditions (Option available for additional conditions)" ) );
				else B1 = Button( r, new GUIContent( "Operator: '" + o1 + "' ▼", "Conditions for several conditions (Option available for additional conditions)" ) );
				
				F2();
				
				if ( B1 )
				{	var menu = new GenericMenu();
					currentFilter = filter;
					menu.AddItem( new GUIContent( "&&" ), !states.OR, () =>
					{	var s = states.SWAP_OR(false);
						s = s.SWAP_AND( true );
						//states.OR = false;
						// states.AND = true;
						conditions[activeIndex] = s.ConvertToString();
						SetDirtyConditions( ref conditions, GetActionByIndex( case_ ) );
					} );
					menu.AddItem( new GUIContent( "||" ), states.OR, () =>
					{	var s = states.SWAP_OR(true);
						s = s.SWAP_AND( false );
						conditions[activeIndex] = s.ConvertToString();
						SetDirtyConditions( ref conditions, GetActionByIndex( case_ ) );
					} );
					menu.ShowAsContext();
				}
			}
			
			
			GUI.enabled = !string.IsNullOrEmpty( states.filter );
			
			if ( divWidth )
			{	r.x += r.width / 2;
				r.width *= 1.1f;
			}
			
			else r.y += r.height;
			
			var o2 = !states.STARTWITH && !states.ENDWITH ? "Contains" : states.STARTWITH && !states.ENDWITH ? "StartWith" : states.ENDWITH && !states.STARTWITH ? "EndWith" : "Equals" ;
			var o2simb = states.GetComparationString();
			F1();
			
			if ( divWidth ) B1 = Button( r, new GUIContent( o2simb, o2 + " (" + o2simb + ")" + "\nString comparison conditions" ) );
			else B1 = Button( r, new GUIContent( "Comparation: '" + o2 + " (" + o2simb + ")" + "' ▼", "String comparison conditions" ) );
			
			F2();
			
			
			if ( B1 )
			{	var menu = new GenericMenu();
				currentFilter = filter;
				menu.AddItem( new GUIContent( "Contains (=..A..)" ), !states.STARTWITH && !states.ENDWITH, () =>
				{	var s = states.SWAP_STARTWITH(false);
					s = s.SWAP_ENDWITH( false );
					conditions[activeIndex] = s.ConvertToString();
					SetDirtyConditions( ref conditions, GetActionByIndex( case_ ) );
				} );
				menu.AddItem( new GUIContent( "StartWith (=A...)" ), states.STARTWITH && !states.ENDWITH, () =>
				{	var s = states.SWAP_STARTWITH(true);
					s = s.SWAP_ENDWITH( false );
					conditions[activeIndex] = s.ConvertToString();
					SetDirtyConditions( ref conditions, GetActionByIndex( case_ ) );
				} );
				menu.AddItem( new GUIContent( "EndWith (=...A)" ), !states.STARTWITH && states.ENDWITH, () =>
				{	var s = states.SWAP_STARTWITH(false);
					s = s.SWAP_ENDWITH( true );
					conditions[activeIndex] = s.ConvertToString();
					SetDirtyConditions( ref conditions, GetActionByIndex( case_ ) );
				} );
				menu.AddItem( new GUIContent( "Equals ( == )" ), states.STARTWITH && states.ENDWITH, () =>
				{	var s = states.SWAP_STARTWITH(true);
					s = s.SWAP_ENDWITH( true );
					conditions[activeIndex] = s.ConvertToString();
					SetDirtyConditions( ref conditions, GetActionByIndex( case_ ) );
				} );
				menu.ShowAsContext();
			}
			
			
			if ( divWidth ) r.x += r.width;
			else r.y += r.height;
			
			F1();
			
			if ( divWidth ) B1 = Button( r, new GUIContent( "Other", o2 + "\nOther Options" ) );
			else B1 = Button( r, new GUIContent( "Other ▼", "Other Options" ) );
			
			F2();
			
			if ( B1 )
			{	var menu = new GenericMenu();
				currentFilter = filter;
				//  if (activeIndex != 0)
				menu.AddItem( new GUIContent( "Not (!..) - Not equal" ), states.NOT && !states.OR, () =>
				{	var s = states.SWAP_NOT(!states.NOT);
					conditions[activeIndex] = s.ConvertToString();
					SetDirtyConditions( ref conditions, GetActionByIndex( case_ ) );
				} );
				//  else  menu.AddDisabledItem(new GUIContent("Not (!..) - Not equal"));
				menu.AddItem( new GUIContent( "Ignore Case (AbCd)" ), states.IGNORECASE, () =>
				{	var s = states.SWAP_IGNORECASE(!states.IGNORECASE);
					conditions[activeIndex] = s.ConvertToString();
					SetDirtyConditions( ref conditions, GetActionByIndex( case_ ) );
				} );
				menu.ShowAsContext();
			}
			
			/*var newNo = EditorGUI.ToggleLeft(r, new GUIContent("Not", "The result of the comparison will be excluded (Option available for additional conditions)"), states.NOT);
			if (newNo != states.NOT)
			{   states.NOT = newNo;
			    currentFilter = filter;
			    conditions[activeIndex] = states.ConvertToString();
			    if ( case_ == 0) SetDirtyConditions(ref conditions, SetTextAction0);
			    else SetDirtyConditions(ref conditions, SetTextAction1);
			}*/
			
			CHANGE_GUI();
			GUI.enabled = true;
		}
		
		
		
		string ConditionToName( ref string condition )
		{	var arr = condition.Split(Adapter.ColorFilter.SEPARATOR);
		
			if ( arr.Length == 0 ) return "- no filter string -";
			
			return string.IsNullOrEmpty( arr.Last() ) ? "- no filter string -" : arr.Last();
		}
		string SetDirtyConditions( ref string[] conditionslots )
		{	return conditionslots.Aggregate( ( s1, s2 ) => s1 + '╩' + s2 );
		}
		void SetDirtyConditions( ref string[] conditionslots, Action<string> setText )
		{	Hierarchy_GUI.Undo( adapter, "Change Color Filters" );
			setText( conditionslots.Aggregate( ( s1, s2 ) => s1 + '╩' + s2 ) );
			SaveFilters( null );
			
		}
		void SetActiveCondition( ref string[] conditions, Action<string> setText, int captureI )
		{	if ( conditions == null ) throw new Exception( "ref string[] conditions == null" );
		
			for ( int n = 0 ; n < conditions.Length ; n++ ) if ( conditions[n].StartsWith( Adapter.ColorFilter.ACTIVE ) ) conditions[n] = conditions[n].Substring( Adapter.ColorFilter.ACTIVE.Length );
			
			conditions[captureI] = Adapter.ColorFilter.ACTIVE + conditions[captureI];
			SetDirtyConditions( ref conditions, setText );
		}
		Adapter.ColorFilter currentFilter;
		void SetTextAction0( string t )     //var ac = currentFilter.NameFilter.StartsWith(Adapter.ColorFilter.ACTIVE);
		{	currentFilter.NameFilter = t;
			// if (ac && !currentFilter.NameFilter.StartsWith(Adapter.ColorFilter.ACTIVE)) currentFilter.NameFilter = Adapter.ColorFilter.ACTIVE + currentFilter.NameFilter;
		}
		void SetTextAction1( string t )     //var ac = currentFilter.ComponentFilter.StartsWith(Adapter.ColorFilter.ACTIVE);
		{	currentFilter.ComponentFilter = t;
			//if (ac && !currentFilter.ComponentFilter.StartsWith(Adapter.ColorFilter.ACTIVE)) currentFilter.ComponentFilter = Adapter.ColorFilter.ACTIVE + currentFilter.ComponentFilter;
		}
		
		void SetTextAction2( string t )     //var ac = currentFilter.ComponentFilter.StartsWith(Adapter.ColorFilter.ACTIVE);
		{	currentFilter.TagFilter = t;
			//if (ac && !currentFilter.ComponentFilter.StartsWith(Adapter.ColorFilter.ACTIVE)) currentFilter.ComponentFilter = Adapter.ColorFilter.ACTIVE + currentFilter.ComponentFilter;
		}
		void SetTextAction3( string t )     //var ac = currentFilter.ComponentFilter.StartsWith(Adapter.ColorFilter.ACTIVE);
		{	currentFilter.LayerFilter = t;
			//if (ac && !currentFilter.ComponentFilter.StartsWith(Adapter.ColorFilter.ACTIVE)) currentFilter.ComponentFilter = Adapter.ColorFilter.ACTIVE + currentFilter.ComponentFilter;
		}
		
		Action<string> GetActionByIndex( int index )
		{	switch ( index )
			{	case 0: return SetTextAction0;
			
				case 1: return SetTextAction1;
				
				case 2: return SetTextAction2;
				
				case 3: return SetTextAction3;
			}
			
			return null;
		}
		
		
		
		
		
		
		
		/// <summary>
		/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// ///////////////////////////////COMMON OPTIONS SUCH ALIGN//////////////////////////////////////////////////////////////////////////////////////////
		/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// </summary>
		void SaveFilters( Adapter.ColorFilter filter )
		{	adapter.ColorModule.__FilterCacheClear();
		
			if ( filter != null ) filter.AS_TEMPCOLOR_ALIGN_ONLY.SetLastTempColor( adapter );
			
			Hierarchy_GUI.SetDirtyObject( adapter );
			Repaint();
			REPAINT_ALL_HIERW();
		}
		void LABEL_DrawOtherButton( Rect lineRect, Adapter.ColorFilter filter, bool enalbe )
		{	//  var tempColor = filter.AS_TEMPCOLOR_ALIGN_ONLY;
		
			F1();
			var ge = GUI.enabled;
			GUI.enabled &= enalbe;
			var B1 =   Button(lineRect, new GUIContent("Other ▼", "Other Options"));
			GUI.enabled = ge;
			F2();
			
			if ( B1 )
			{	var menu = new GenericMenu();
				currentFilter = filter;
				
				//else  menu.AddDisabledItem(new GUIContent("Not (!..) - Not equal"));
				menu.AddItem( new GUIContent( "Has Label Shadow" ), filter.LABEL_SHADOW, () =>
				{	Hierarchy_GUI.Undo( adapter, "Change Color Filters" );
					filter.LABEL_SHADOW = !filter.LABEL_SHADOW;
					// filter.AS_TEMPCOLOR_ALIGN_ONLY = tempColor;
					SaveFilters( filter );
				} );
				menu.ShowAsContext();
			}
		}
		void BACKGROUND_DrawOtherButton( Rect lineRect, Adapter.ColorFilter filter, bool enalbe )
		{	// var tempColor = filter.AS_TEMPCOLOR_ALIGN_ONLY;
		
			F1();
			var ge = GUI.enabled;
			GUI.enabled &= enalbe;
			var B1 =   Button(lineRect, new GUIContent("Other ▼", "Other Options"));
			GUI.enabled = ge;
			F2();
			
			if ( B1 )
			{	var menu = new GenericMenu();
				currentFilter = filter;
				
				//else  menu.AddDisabledItem(new GUIContent("Not (!..) - Not equal"));
				menu.AddItem( new GUIContent( "Narrow Height" ), filter.BG_HEIGHT == 1, () =>
				{	Hierarchy_GUI.Undo( adapter, "Change Color Filters" );
				
					if ( filter.BG_HEIGHT != 1 ) filter.BG_HEIGHT = 1;
					else filter.BG_HEIGHT = 0;
					
					//  filter.AS_TEMPCOLOR_ALIGN_ONLY = tempColor;
					SaveFilters( filter );
				} );
				menu.AddItem( new GUIContent( "1 pixel Height" ), filter.BG_HEIGHT == 2, () =>
				{	Hierarchy_GUI.Undo( adapter, "Change Color Filters" );
				
					if ( filter.BG_HEIGHT != 2 ) filter.BG_HEIGHT = 2;
					else filter.BG_HEIGHT = 0;
					
					// filter.AS_TEMPCOLOR_ALIGN_ONLY = tempColor;
					SaveFilters( filter );
				} );
				menu.ShowAsContext();
			}
		}
		void BACKGROUND_DrawAlignLine( Rect lineRect, Adapter.ColorFilter filter )
		{	lineRect.height = FilterLineHeight;
			var tempColor = filter.AS_TEMPCOLOR_ALIGN_ONLY;
			
			var oldl = tempColor.BG_ALIGMENT_LEFT;
			var oldr = tempColor.BG_ALIGMENT_RIGHT;
			var oldw = tempColor.BG_WIDTH;
			
			var RECT = lineRect;
			RECT.width /= 3;
			
			/*RECT.width /= 2;
			Adapter.LABEL (RECT, "<i>Left:</i>");
			RECT.x += RECT.width;
			RECT.width *= 2;*/
			
			tempColor.BG_ALIGMENT_LEFT = EditorGUI.Popup( RECT, tempColor.BG_ALIGMENT_LEFT, tempColor.ALIGMENT_LEFT_CATEGORIES, EditorStyles.toolbarButton );
			Adapter.TOOLTIP( RECT, "Left Align Position for Background Color." );
			RECT.x += RECT.width;
			
			/* RECT.width /= 2;
			 Adapter.LABEL (RECT, "<i>Right:</i>");
			 RECT.x += RECT.width;
			 RECT.width *= 2;*/
			
			var cats = tempColor.ALIGMENT_RIGHT_CATEGORIES.ToList();
			cats.Reverse();
			cats.Add( "Fixed Width" );
			cats.Reverse();
			tempColor.BG_ALIGMENT_RIGHT = 5 - EditorGUI.Popup( RECT, 5 - tempColor.BG_ALIGMENT_RIGHT, cats.ToArray(), EditorStyles.toolbarButton );
			Adapter.TOOLTIP( RECT, "Right Align Position for Background Color." );
			RECT.x += RECT.width;
			
			if ( tempColor.BG_ALIGMENT_RIGHT == 5 )
			{	tempColor.BG_WIDTH = Mathf.Clamp( EditorGUI.IntField( RECT, tempColor.BG_WIDTH, textFieldStyle ), 10, 255 );
			}
			
			
			if (
			    oldl != tempColor.BG_ALIGMENT_LEFT ||
			    oldr != tempColor.BG_ALIGMENT_RIGHT ||
			    oldw != tempColor.BG_WIDTH )
			{	Hierarchy_GUI.Undo( adapter, "Change Color Filters" );
				filter.AS_TEMPCOLOR_ALIGN_ONLY = tempColor;
				SaveFilters( filter );
			}
			
		}
		
		
		/// <summary>
		/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// //////////////////////////////////////DRAW LINE///////////////////////////////////////////////////////////////////////////////////////////////////
		/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// </summary>
		
		
		GUIStyle _textFieldStyleTransparent;
		GUIStyle textFieldStyleTransparent
		{	get
			{	if (_textFieldStyleTransparent == null )
				{	_textFieldStyleTransparent = new GUIStyle( Adapter.GET_SKIN().textField );
					_textFieldStyleTransparent.alignment = TextAnchor.MiddleLeft;
					_textFieldStyleTransparent.normal.textColor = Color.clear;
				}
				
				return _textFieldStyleTransparent;
			}
		}
		GUIStyle _textFieldStyle;
		GUIStyle textFieldStyle
		{	get
			{	if ( _textFieldStyle == null )
				{	_textFieldStyle = new GUIStyle( Adapter.GET_SKIN().textField );
					_textFieldStyle.alignment = TextAnchor.MiddleLeft;
				}
				
				return _textFieldStyle;
			}
		}
		
		
		
		void DrawLine( int index, Rect lineRect )
		{
		
			var filter = Hierarchy_GUI.Instance( adapter ).ColorFilters[index];
			
			GUI.Box( lineRect, "" );
			lineRect.y += 1;
			lineRect.x += 1;
			lineRect.width -= 2;
			lineRect.height -= 2;
			
			var H = FilterLineHeight;
			var rect = new Rect(lineRect.x, lineRect.y, H * 1.5f, H);
			
			
			
			var new_enable = EditorGUI.ToggleLeft(rect, (string)null, filter.ENABLE);
			
			if ( new_enable != filter.ENABLE )
			{	Hierarchy_GUI.Undo( adapter, "Change Color Filters" );
				filter.ENABLE = new_enable;
				SaveFilters( null );
			}
			
			
			
			rect.x += rect.width;
			
			
			
			var oldsl = label.fontSize;
			label.fontSize = (int)FilterLineHeight;
			Label( rect, new GUIContent( "=", "Change Order Position" ) );
			label.fontSize = oldsl;
			
			if ( rect.Contains( Event.current.mousePosition ) && Event.current.type == EventType.MouseDown )
			{	if ( Event.current.button == 0 )
				{	dragIndex = index;
					editIndex = new Dictionary<int, int>();
					currentY = new float[0];
					dragPos = Event.current.mousePosition - new Vector2( lineRect.x, lineRect.y );
					Event.current.Use();
					Repaint();
				}
			}
			
			EditorGUIUtility.AddCursorRect( rect, MouseCursor.Link );
			
			
			// lineRect.x += rect.width;
			// lineRect.width -= rect.width;
			var sh = lineRect.height - FilterLineHeight * 2;
			// lineRect.height = FilterLineHeight;
			
			
			
			
			rect.x += rect.width;
			// rect.width = lineRect.width - rect.width;
			rect.width = rect.height;
			
			
			var newExpand = GUI.Toggle(rect, editIndex.ContainsKey(index), GUIContent.none, adapter.foldoutStyle);
			
			if ( newExpand != editIndex.ContainsKey( index ) )
			{	var d = editIndex;
			
				if ( newExpand ) d.Add( index, -1 );
				else d.Remove( index );
				
				editIndex = d;
			}
			
			rect.x += rect.width * 1.1f;
			
			
			rect.width = 1;
			//if (filter._icon) Adapter.INTERNAL_BOX(Shrink(rect, 1));
			rect.x += rect.width;
			rect.width = FilterLineHeight;
			
			if ( filter._icon )     // Adapter.INTERNAL_BOX(Shrink(rect, 1));
			{	Adapter.DrawTexture( Shrink( rect, 2 ), filter._icon, filter.hasColorIcon ? filter.colorIcon : Color.white );
			}
			
			rect.x += rect.width;
			
			
			
			
			rect.width = lineRect.width - (rect.x - lineRect.x);
			rect.width -= rect.height * 12f + 5f;
			
			
			//LABEL NAME
			/*  if (filter.hasColorBg) EditorGUI.DrawRect(rect, filter.colorBg);
			  var obc = GUI.backgroundColor;
			  var otc = GUI.skin.textField.normal.textColor;
			  if (filter.hasColorBg && filter.colorBg.a > 0.01 )
			  {   GUI.backgroundColor *= filter.colorBg;
			      GUI.backgroundColor *= new Color(1, 1, 1, 0.5f);
			      EditorGUI.DrawRect(rect, filter.colorBg );
			  }
			  if (filter.hasColorText && filter.colorText.a > 0.01 ) GUI.skin.textField.normal.textColor = filter.colorText;
			  var new_SlotName = GUI.TextFieDld(rect, filter.SlotName);
			  GUI.backgroundColor = obc;
			  GUI.skin.textField.normal.textColor = otc;*/
			
			/* var oc = GUI.skin.label.normal.textColor;
			 if (filter.hasColorText) GUI.skin.label.normal.textColor = filter.colorText;
			 Label(rect, filter.SlotName);
			 GUI.skin.label.normal.textColor = oc;*/
			//  rect.x += rect.width;
			
			GUI.SetNextControlName( "MyTextField" );
			
			var new_SlotName = GUI.TextField(rect, filter.NAME, textFieldStyleTransparent);
			var occ = GUI.color;
			
			if ( GUI.GetNameOfFocusedControl() == "MyTextField" )
			{	GUI.color *= new Color( 1, 1, 1, 0 );
			
				if ( Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.KeypadEnter || Event.current.keyCode == KeyCode.Escape )
				{	GUI.FocusControl( "" );
					adapter.SKIP_PREFAB_ESCAPE = true;
					// EditorWindow.focusedWindow.Focus();
					Adapter.EventUse();
				}
			}
			
			if ( new_SlotName != filter.NAME )
			{	Hierarchy_GUI.Undo( adapter, "Change Color Filters" );
				filter.NAME = new_SlotName;
				SaveFilters( null );
			}
			
			DrawColoredLabel( rect, filter );
			GUI.color = occ;
			
			
			
			
			
			
			
			rect.x += rect.width;
			rect.x += 5;
			
			//PASTE
			// RESTORE_GUI();
			/* if (Button( rect, new GUIContent( "| Edit", "Edit current filter" )))
			 {   if (Event.current.button == 0)
			     {   if (editIndex != index) editIndex = index;
			         else editIndex = new Dictionary<int, int>();
			         currentY = new float[0];
			         Repaint();
			     }
			 }
			 EditorGUIUtility.AddCursorRect( rect, MouseCursor.Link );
			 if (Event.current.type == EventType.Repaint && editIndex == index) GUI.skin.button.Draw(rect, new GUIContent( "| Edit", "Edit current filter" ), true, true, false, true);
			 rect.x += rect.width;*/
			// rect.x += 5;
			
			rect.width = lineRect.width - (rect.x - lineRect.x);
			var cross = rect;
			cross.width = cross.height * 1.5f;
			cross.x = rect.x + rect.width - cross.width;
			rect.width -= cross.width;
			rect.width /= 2;
			
			if ( source != null )
			{	var c = new GUIContent( "To Object", "Apply current filter to selected object" );
				var or = GUI.skin.button.richText;
				GUI.skin.button.richText = true;
				rect.width = GUI.skin.button.CalcSize( c ).x;
				var b = Button( rect, c);
				GUI.skin.button.richText = or;
				
				if ( b )
				{	if ( Event.current.button == 0 )
					{	ToObjectButton( source, index );
					}
				}
				
				EditorGUIUtility.AddCursorRect( rect, MouseCursor.Link );
				
				
				rect.x += rect.width;
				c = new GUIContent( "From Object", "Overwrite current filter from selected object" );
				rect.width = GUI.skin.button.CalcSize( c ).x;
				
				if ( Button( rect, c ) )  //✂
				{	if ( Event.current.button == 0 )
					{	FromObjectButton( source, index );
					}
				}
				
				EditorGUIUtility.AddCursorRect( rect, MouseCursor.Link );
			}
			
			
			
			
			if ( editIndex.ContainsKey( index ) )
			{
			
			
				// rect.x += 5;
				
				//REMOVE
				
			}
			else
			{	/*rect.width = rect.height;
				    int SHR = 4;
				    if (filter.hasColorText && filter.colorText.a > 0.01 ) EditorGUI.DrawRect(Shrink(rect, SHR), filter.colorText);
				    else GUI.Box(Shrink(rect, SHR), "");
				    Label(rect, new GUIContent("", "Label Color"));
				
				    rect.x += rect.width;
				    if (filter.hasColorBg && filter.colorBg.a > 0.01 ) EditorGUI.DrawRect(Shrink(rect, SHR), filter.colorBg);
				    else GUI.Box(Shrink(rect, SHR), "");
				    Label(rect, new GUIContent("", "BG Color"));
				
				    rect.x += rect.width;
				    if (filter.child) GUI.Box(Shrink(rect, 0), "C");
				    else GUI.Box(Shrink(rect, SHR), "");
				    Label(rect, new GUIContent("", "Apply to child"));
				
				
				    rect.x += rect.width;
				    rect.width *= 2;
				    if ((filter.Aligment & 1 ) != 0 && (filter.Aligment & 2 ) != 0) GUI.Box(Shrink(rect, SHR), "FULL");
				    else GUI.Box(Shrink(rect, SHR), "");
				    Label(rect, new GUIContent("", "Background Align"));
				
				    rect.x += rect.width;
				    rect.width /= 2;
				    if (filter._icon  )
				    {   var asdasd = GUI.color;
				        if (filter.hasColorIcon)  GUI.color *= new Color( filter.colorIcon.r, filter.colorIcon.g, filter.colorIcon.b, 1 );
				        GUI.DrawTexture(Shrink(rect, SHR), filter._icon);
				        GUI.color = asdasd;
				    }
				    else GUI.Box(Shrink(rect, SHR), "");
				    Label(rect,  new GUIContent("", "Icon Texture"));
				
				    rect.x += rect.width;
				    if (filter.hasColorIcon && filter._icon  ) EditorGUI.DrawRect(Shrink(rect, SHR), filter.colorIcon);
				    else GUI.Box(Shrink(rect, SHR), "");
				    Label(rect, new GUIContent("", "Icon Color"));*/
			}
			
			if ( Button( cross, new GUIContent( "✖", "Remove Filter" ) ) )
			{	if ( Event.current.button == 0 )
				{	RemoveLine( index );
				}
			}
			
			EditorGUIUtility.AddCursorRect( cross, MouseCursor.Link );
			//  CHANGE_GUI();
			
			
			if ( editIndex.ContainsKey( index ) )
			{	rect = lineRect;
				rect.height = FilterLineHeight;
				rect.y += rect.height + 5;
				// rect.width *= 0.95f;
				//  rect.width /= 2;
				//  rect.width -= 5;
				
				var new_NameFilter = filter.NameFilter;
				var new_ComponentFilter = filter.ComponentFilter;
				var new_TagFilter = filter.TagFilter;
				var new_LayerFilter = filter.LayerFilter;
				
				//FILTER NAMES
				Label( new Rect( rect.x, rect.y, rect.width * 0.2f, rect.height ), new GUIContent( "Name: ", "A string to compare with the name of GameObject" ), adapter.STYLE_LABEL_8 );
				GUI.SetNextControlName( "NAME" );
				DrawFilterText( ref new_NameFilter, filter, 0, new Rect( rect.x + rect.width * 0.2f, rect.y, rect.width * 0.5f, rect.height ) );
				DrawFilterConditions( ref new_NameFilter, filter, 0, new Rect( rect.x + rect.width * 0.7f, rect.y, rect.width * 0.3f, rect.height ) );
				//rect.x += rect.width;
				//  rect.x += 5;
				rect.y += rect.height;
				
				if ( adapter.IS_HIERARCHY() )     //FILTER COMPS
				{	Label( new Rect( rect.x, rect.y, rect.width * 0.2f, rect.height ), new GUIContent( "Component: ", "A string to compare with the name of all the GameObject's Components" ), adapter.STYLE_LABEL_8 );
					GUI.SetNextControlName( "COMP" );
					DrawFilterText( ref new_ComponentFilter, filter, 1, new Rect( rect.x + rect.width * 0.2f, rect.y, rect.width * 0.5f, rect.height ) );
					DrawFilterConditions( ref new_ComponentFilter, filter, 1, new Rect( rect.x + rect.width * 0.7f, rect.y, rect.width * 0.3f, rect.height ) );
					
					rect.y += rect.height;
					Label( new Rect( rect.x, rect.y, rect.width * 0.2f, rect.height ), new GUIContent( "Tag: ", "A string to compare with the tag of GameObject" ), adapter.STYLE_LABEL_8);
					GUI.SetNextControlName( "TAG" );
					DrawFilterText( ref new_TagFilter, filter, 2, new Rect( rect.x + rect.width * 0.2f, rect.y, rect.width * 0.5f, rect.height ) );
					DrawFilterConditions( ref new_TagFilter, filter, 2, new Rect( rect.x + rect.width * 0.7f, rect.y, rect.width * 0.3f, rect.height ) );
					
					rect.y += rect.height;
					Label( new Rect( rect.x, rect.y, rect.width * 0.2f, rect.height ), new GUIContent( "Layer: ", "A string to compare with the layer of GameObject" ), adapter.STYLE_LABEL_8);
					GUI.SetNextControlName( "LAYER" );
					DrawFilterText( ref new_LayerFilter, filter, 3, new Rect( rect.x + rect.width * 0.2f, rect.y, rect.width * 0.5f, rect.height ) );
					DrawFilterConditions( ref new_LayerFilter, filter, 3, new Rect( rect.x + rect.width * 0.7f, rect.y, rect.width * 0.3f, rect.height ) );
					
				}
				
				
				
				lineRect.y = rect.y + rect.height + 5;
				lineRect.height = sh;
				rect = lineRect;
				
				/*if ( GUI.GetNameOfFocusedControl() == "NAME")
				{   DrawFilterConditions( ref new_NameFilter, filter, 0, rect);
				}
				else if ( GUI.GetNameOfFocusedControl() == "COMP")
				{   DrawFilterConditions( ref new_ComponentFilter, filter, 1, rect);
				}
				else*/
				{	var PPP  = 4;
				
					var thri = false;
					
					var seg = rect;
					seg.x += PPP;
					seg.width -= PPP * 2;
					var FULL_W = seg.width;
					seg.width = seg.width * 0.65f;
					// var PARTS = new[] {0.3f, 0.3f, 0.2f, ( seg.height * 2) / FULL_W, 0.4f};
					// var DIF = (PARTS[0] + PARTS[2] + PARTS[4] ) / (1 - PARTS[3]);
					// for (int i = 0; i < PARTS.Length; i++) if (i != 3) PARTS[i] /= DIF;
					seg.height = 23 * 2 + EditorStyles.toolbarButton.fixedHeight * 2 + PPP * 2;
					Adapter.INTERNAL_BOX( seg );
					var _R_ = Shrink( seg, PPP);
					
					//var ox = t.x;
					////////////////////////////////////////////////////////
					_R_.width = (seg.width - PPP * 2) / 3;
					_R_.height = 20;
					var new_hasColorText = adapter.TOGGLE_LEFT(_R_, "<i>Label</i>:", filter.hasColorText);
					
					if ( new_hasColorText != filter.hasColorText )
					{
					
						if ( new_hasColorText && !filter.AS_TEMPCOLOR_ALIGN_ONLY.HAS_LABEL_COLOR )
						{	var getcolor = filter.AS_TEMPCOLOR_ALIGN_ONLY;
							getcolor.CopyFromTo( Adapter.TempColorClass.CopyType.LABEL, from: getcolor.GetLastTempColor( adapter ), to: ref getcolor );
							filter.AS_TEMPCOLOR_ALIGN_ONLY = getcolor;
							thri = true;
						}
						
						// SaveFilters( filter );
					}
					
					var new_colorText = filter.colorText;
					_R_.x += _R_.width;
					_R_.height = 23;
					var hasC = false;
					
					if ( new_hasColorText )
					{	emptyContent.tooltip = "Label Color";
						RESTORE_GUI();
						new_colorText = PICKER( _R_, emptyContent, new_colorText );
						CHANGE_GUI();
						hasC = true;
					}
					
					else
					{	Label( new Rect( _R_.x, _R_.y, 55, _R_.height ), "-" );
					}
					
					_R_.x += _R_.width;
					LABEL_DrawOtherButton( _R_, filter, new_hasColorText );
					////////////////////////////////////////////////////////
					//  t.x += t.width;
					
					
					////////////////////////////////////////////////////////
					_R_.y += _R_.height;
					_R_.x = seg.x + PPP;
					_R_.height = 20;
					var new_hasColorBg = adapter.TOGGLE_LEFT(_R_, "<i>BG</i>   :", filter.hasColorBg);
					
					if ( new_hasColorBg != filter.hasColorBg )
					{	if ( new_hasColorBg && !filter.AS_TEMPCOLOR_ALIGN_ONLY.HAS_BG_COLOR )
						{	var getcolor = filter.AS_TEMPCOLOR_ALIGN_ONLY;
							getcolor.CopyFromTo( Adapter.TempColorClass.CopyType.BG, from: getcolor.GetLastTempColor( adapter ), to: ref getcolor );
							// Hierarchy_GUI.Undo( adapter , "Change Color Filters" );
							filter.AS_TEMPCOLOR_ALIGN_ONLY = getcolor;
							thri = true;
							// SaveFilters( filter );
						}
					}
					
					var new_colorBg = filter.colorBg;
					_R_.x += _R_.width;
					_R_.height = 23;
					
					if ( new_hasColorBg )
					{	emptyContent.tooltip = "BG Color";
						RESTORE_GUI();
						new_colorBg = PICKER( _R_, emptyContent, new_colorBg );
						CHANGE_GUI();
						hasC = true;
					}
					
					else
					{	Label( new Rect( _R_.x, _R_.y, 55, _R_.height ), "-" );
					}
					
					_R_.x += _R_.width;
					_R_.width = (seg.width - PPP * 2) - _R_.x;
					BACKGROUND_DrawOtherButton( _R_, filter, new_hasColorBg );
					////////////////////////////////////////////////////////
					
					
					
					////////////////////////////////////////////////////////
					_R_.y += _R_.height;
					_R_.height = EditorStyles.toolbarButton.fixedHeight;
					_R_.x = seg.x + PPP;
					_R_.width = seg.width - PPP * 2;
					BACKGROUND_DrawAlignLine( _R_, filter );
					
					_R_.y += _R_.height;
					_R_.x = seg.x + PPP;
					// t.width /= 2;
					// t.width = (FULL_W - PPP * 2) * PARTS[1];
					
					GUI.enabled = hasC;
					string[]  cats = new [] { "This", "Child"};
					var nv = GUI.Toolbar(_R_,   filter.child ? 1 : 0, cats, EditorStyles.toolbarButton) == 1;
					
					if ( filter.child != nv )
					{	Hierarchy_GUI.Undo( adapter, "Change Color Filters" );
						filter.child = nv;
						SaveFilters( filter );
						thri = true;
					}
					
					// var new_child = EditorGUI.ToggleLeft(_R_, "Apply to Child", filter.child);
					GUI.enabled = true;
					////////////////////////////////////////////////////////
					
					
					
					var ox = seg.x;
					seg.x += seg.width + PPP;
					seg.width = FULL_W - (seg.x - ox);
					
					Adapter.INTERNAL_BOX( seg );
					
					_R_ = Shrink( seg, PPP );
					_R_.height = FilterLineHeight;
					
					Adapter.LABEL( _R_, "<i>Icon:</i>" );
					var IR = _R_;
					IR.x += seg.width * 0.33f;
					IR.height = IR.width = seg.width * 0.33f;
					_R_.y += seg.width * 0.33f;
					
					// "BG Aligment:"
					/* EditorGUI.Popup(t, 0,  new[] { "L:FULL - R:FULL", "L:Icon - R:FULL", "L:Label - R:FULL", "L:FULL - R:Half", "L:Icon - R:Half", "L:Label - R:Half"});
					 if (Event.current.type == EventType.Repaint)
					 {   Label(t, new GUIContent() {tooltip = "Background Align\nL-Left Align\nR-Right Align"});
					 }*/
					// _R_.y -= _R_.height;
					//  _R_.width = FULL_W * PARTS[3];
					RESTORE_GUI();
					Texture newicon = filter._icon;
					
					try
					{	var asdasd = GUI.color;
					
						if ( filter.hasColorIcon && newicon ) GUI.color *= new Color( filter.colorIcon.r, filter.colorIcon.g, filter.colorIcon.b, 1 );
						
						// newicon = EditorGUI.ObjectField( new Rect(_R_.x, _R_.y, _R_.height * 2, _R_.height * 2), newicon, typeof( Texture2D ), false ) as Texture2D;
						newicon = EditorGUI.ObjectField( IR, newicon, typeof( Texture2D ), false ) as Texture2D;
						GUI.color = asdasd;
					}
					
					catch
					{	newicon = filter._icon;
					}
					
					CHANGE_GUI();
					
					
					// _R_.y += _R_.height * 2 ;
					_R_.height = 20;
					//   _R_.width = FULL_W * PARTS[4] * 1.15f;
					var fon = GUI.enabled;
					GUI.enabled = newicon;
					var new_hasColorIcon = adapter.TOGGLE_LEFT(_R_, "<i>Icon Color</i>:", filter.hasColorIcon);
					var new_colorIcon = filter.colorIcon;
					_R_.y += _R_.height;
					_R_.height = 23;
					
					if ( new_hasColorIcon )
					{	emptyContent.tooltip = "Icon Color";
						RESTORE_GUI();
						new_colorIcon = PICKER( _R_, emptyContent, new_colorIcon );
						CHANGE_GUI();
					}
					
					else
					{	Label( _R_, "-" );
					}
					
					_R_.x += _R_.width;
					GUI.enabled = fon;
					
					
					var first = newicon != filter._icon
					            || filter.colorIcon != new_colorIcon
					            || filter.hasColorIcon != new_hasColorIcon
					            || filter.colorBg != new_colorBg || new_LayerFilter != filter.LayerFilter
					            || filter.hasColorBg != new_hasColorBg
					            || filter.colorText != new_colorText
					            || filter.hasColorText != new_hasColorText
					            ||  filter.ComponentFilter != new_ComponentFilter
					            ||  filter.TagFilter != new_TagFilter
					            || filter.NameFilter != new_NameFilter;
					var second =   filter.NAME != new_SlotName;
					
					if ( first || second || thri )
					{	Hierarchy_GUI.Undo( adapter, "Change Color Filters" );
						filter._icon = newicon;
						
						filter.colorIcon = new_colorIcon;
						filter.hasColorIcon = new_hasColorIcon;
						filter.colorBg = new_colorBg;
						filter.hasColorBg = new_hasColorBg;
						filter.colorText = new_colorText;
						filter.hasColorText = new_hasColorText;
						
						filter.ComponentFilter = new_ComponentFilter;
						filter.TagFilter = new_TagFilter;
						filter.NameFilter = new_NameFilter;
						filter.LayerFilter = new_LayerFilter;
						filter.NAME = new_SlotName;
						
						SaveFilters( first || thri ? filter : null );
						//SaveFilters(filter);
						
					}
					
					
				}
			}
		}
		
		
	}
}
}