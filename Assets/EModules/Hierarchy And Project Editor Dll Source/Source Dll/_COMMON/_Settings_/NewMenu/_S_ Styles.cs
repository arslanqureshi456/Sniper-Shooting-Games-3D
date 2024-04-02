//#define USE_RIGHT_CHECK
//#define USE_GREEN_CHECK

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



	internal static GUIContent wikibutcontent = new GUIContent();
	internal void __DRAW_WIKI_BUTTON( Rect rect, string category, string note = null )
	{	//string pluginname = pluginname;
		// var rect = GetPreLastRect();
		var P = 0;
		rect.height -= P * 2;
		rect.y += P;
		rect.x += rect.width - 2;
		rect.width = rect.height * 3;
		rect.x -= rect.width - P;
		
		var lr = rect;
		lr.width *= 0.66f;
		//LABEL( new Rect( lr.x - lr.width , lr.y , lr.width * 2 , lr.height ) , "<i>online doc</i>" );
		LABEL( new Rect( lr.x + 3, lr.y, lr.width, lr.height ), "<i>doc</i>" );
		lr.x += lr.width;
		lr.width /= 1.8f;
		// GUI.Label(lr, "?");
		wikibutcontent.text = "?";
		wikibutcontent.tooltip = "www.emem.store/wiki/hierarchy/" + category;
		
		if ( note != null ) wikibutcontent.tooltip += "/" + note;
		
		// Hierarchy.ChangeGUI();
		if ( GUI.Button( lr, wikibutcontent ) )
		{	OpenWikiUrl( pluginname, category, note );
		}
	}
	internal static void OpenWikiUrl( string plugin, string category, string note = null )
	{	var url = "http://www.emem.store/wiki" + "?=" + plugin + "&" + category;
	
		if ( note != null ) url += "&" + note;
		
		Application.OpenURL( url );
	}
	
	
	void DRAW_PRE_ICON( ref Rect rect, string icon, bool enable )
	{	DRAW_PRE_ICON( ref rect, GetIcon( icon ), enable );
	}
	void DRAW_PRE_ICON( ref Rect rect, Texture icon, bool enable )
	{	var ICONSIZE = EditorGUIUtility.singleLineHeight;
		var R = rect;
		
		var OFF = (rect.height - ICONSIZE) / 2;
		// rect.x += ICONSIZE;
		rect.width -= ICONSIZE;
		
		R.y += OFF;
		R.height = R.width = ICONSIZE;
		
		var oc = GUI.color;
		
		if ( !enable || !GUI.enabled ) GUI.color *= new Color( 1, 1, 1, 0.5f );
		
		GUI.DrawTexture( R, icon );
		GUI.color = oc;
	}
	
	
	GUIStyle glow;
	internal Rect S_DRAW_HELP_TEXTURE( string textureName, int? height = null, bool? enable = null, int xOffset = 0, float? yOffset = null, TextAlignment? align = null, float DDD = 1.2f,
	                                   GUIStyle bgstyle = null, bool bgStyleW = false )
	{	var texture = GetIcon(textureName + (EditorGUIUtility.isProSkin ? "" : " PERSONAL"));
	
		if ( texture == Texture2D.whiteTexture ) texture = GetIcon( textureName );
		
		return S_DRAW_HELP_TEXTURE( texture, height, enable, xOffset, yOffset, align, DDD, bgstyle, bgStyleW );
	}
	internal Rect S_DRAW_HELP_TEXTURE( Texture texture, int? height = null, bool? enable = null, int xOffset = 0, float? yOffset = null, TextAlignment? align = null, float DDD = 1.2f,
	                                   GUIStyle bgstyle = null, bool bgStyleW = false )
	{	float HHH = height ?? texture.height;
		HHH /= DDD;
		
		if ( yOffset.HasValue ) yOffset /= DDD;
		
		var lastRect = EditorGUILayout. GetControlRect( GUILayout.Height(HHH));
		//    var WW = 327;
		var WW = lastRect.width;
		
		if ( bgStyleW && bgstyle != null ) Adapter.LABEL( lastRect, "", bgstyle );
		
		var dx = lastRect.x;
		var dw = lastRect.width;
		
		if ( align == null ) lastRect.x += xOffset + (lastRect.width - WW) / 2;
		else lastRect.x += xOffset;
		
		lastRect.width = texture.width;
		
		if ( align == null ) lastRect.x += (WW - lastRect.width) / 2;
		
		
		var off = lastRect;
		off.x -= 2;
		off.y -= 2;
		off.width += 4;
		off.height += 4;
		
		if ( !bgStyleW && bgstyle != null ) Adapter.LABEL( off, "", bgstyle );
		
		
		
		var clampRect = lastRect;
		lastRect.height = texture.height / DDD;
		clampRect.height = HHH;
		// lastRect.height = height ?? texture.height;
		var c = GUI.color;
		
		/* if (!EditorGUIUtility.isProSkin)
		 {
		     var cc = GUI.color;
		     cc.a = 0.8f;
		     GUI.color = cc;
		 }*/
		if ( Event.current.type.Equals( EventType.Repaint ) )
		{	GUI.BeginClip( clampRect );
			lastRect.y = lastRect.x = 0;
			
			if ( yOffset.HasValue ) lastRect.y += yOffset.Value;
			
			GUI.DrawTexture( lastRect, texture, ScaleMode.ScaleToFit );
			
			//  if (shadow == null) shadow = A.InitializeStyle( "SHADOW", 0.25f, 0.25f, 0.25f, 0.25f );
			if ( glow == null ) glow = InitializeStyle( "HIPERUI_BUTTONGLOW", 0.25f, 0.25f, 0.25f, 0.25f );
			
			//  if (Event.current.type == EventType.Repaint) shadow.Draw(lastRect, "", false, false, false, false);
			if ( Event.current.type == EventType.Repaint )
			{	var c12 = GUI.color;
			
				if ( EditorGUIUtility.isProSkin ) GUI.color *= new Color32( 50, 50, 50, 255 );
				
				glow.Draw( lastRect, "", false, false, false, false );
				GUI.color = c12;
			}
			
			GUI.EndClip();
		}
		
		//Y += yOffset ?? 0;
		if ( !GUI.enabled || !(enable ?? true) ) Adapter.FadeRect( clampRect, 0.7f );
		
		GUI.color = c;
		
		clampRect.x += clampRect.width;
		clampRect.width = dw - clampRect.x + dx;
		
		
		return clampRect;
		// GUILayout.Space(lastRect.height);
		/*  var lastRect = GUILayoutUtility.GetLastRect();
		  lastRect.x += 15 + xOffset;
		  lastRect.y += lastRect.height;
		  lastRect.width = WW;
		  lastRect.height = width;
		  if (Event.current.type.Equals(EventType.Repaint)) GUI.DrawTexture(lastRect, GetIcon(textureName));
		  GUILayout.Space(lastRect.height);*/
	}
	
	GUIStyle slid;
	internal float S_SLIDER( Rect rect, string title, float value, float left, float right )
	{
	
		//
		//             if ( slid == null ) slid = new GUIStyle( Adapter.GET_SKIN().horizontalSliderThumb );
		//             slid.normal.background = slid.focused.background = slid.hover.background = SETUP_SLIDER.normal.background;
		//             slid.active.background = SETUP_SLIDER.active.background;
		//             var oldS = Adapter.GET_SKIN().horizontalSliderThumb;
		//             Adapter.GET_SKIN().horizontalSliderThumb = slid;
		PREPARE_TEXTFIELD();
		
		var oldT = EditorStyles.label.richText;
		EditorStyles.label.richText = true;
		
		var result = title == null ? EditorGUI.Slider(rect, value, left, right) : EditorGUI.Slider(rect, title, value, left, right);
		
		EditorStyles.label.richText = oldT;
		
		RESTORE_TEXTFIELD();
		//  Adapter.GET_SKIN().horizontalSliderThumb = oldS;
		
		return result;
	}
	internal int S_SLIDER( string title, int value, int left, int right )
	{	var R = EditorGUILayout.GetControlRect();
		return S_SLIDER( R, title, value, left, right );
	}
	internal int S_SLIDER( Rect rect, string title, int value, int left, int right )
	{
	
		//             if ( slid == null ) slid = new GUIStyle( Adapter.GET_SKIN().horizontalSliderThumb );
		//             slid.normal.background = slid.focused.background = slid.hover.background = SETUP_SLIDER.normal.background;
		//             slid.active.background = SETUP_SLIDER.active.background;
		//             // EditorStyles.numberField = SETUP_SLIDER;
		//             var oldS = Adapter.GET_SKIN().horizontalSliderThumb;
		//  Adapter.GET_SKIN().horizontalSliderThumb = slid;
		PREPARE_TEXTFIELD();
		
		var oldT = EditorStyles.label.richText;
		EditorStyles.label.richText = true;
		var result = title == null ? EditorGUI.IntSlider(rect, value, left, right) : EditorGUI.IntSlider(rect, title, value, left, right);
		EditorStyles.label.richText = oldT;
		RESTORE_TEXTFIELD();
		//  Adapter.GET_SKIN().horizontalSliderThumb = oldS;
		
		return result;
	}
	
	//         internal float S_FLOAT_FIELD( string title , float value ) {   // var R = GetControlRect(false, GUILayout.Height(20));
	//             var R = EditorGUILayout.GetControlRect();
	//             return S_FLOAT_FIELD( R , title , value );
	//         }
	internal float S_FLOAT_FIELD( Rect rect, float value, string postFix = null )
	{
	
		PREPARE_TEXTFIELD();
		var crop = rect;
		crop.width /= 2;
		var crop1 = crop;
		
		crop.x += crop.width;
		GUI.SetNextControlName( "MyTextField" );
		
		/* if ( GUI.GetNameOfFocusedControl() == "MyTextField" )
		 {   crop.x -= crop.width;
		     crop.width *= 2;
		
		 }*/
		if ( GUI.enabled ) value = EditorGUI.FloatField( crop, value );
		
		if ( GUI.GetNameOfFocusedControl() != "MyTextField" )
		{	if ( GUI.enabled )
			{	GUI.BeginClip( crop1 );
				value = EditorGUI.FloatField( new Rect( 0, 0, crop1.width, rect.height ), " ", value );
				GUI.EndClip();
			}
		}
		
		if ( Event.current.type == EventType.Repaint
		        && GUI.GetNameOfFocusedControl() != "MyTextField" )
		{	//if (!EditorGUIUtility.isProSkin)
			Adapter.GET_SKIN().textField.Draw( rect, new GUIContent( value.ToString() + (postFix ?? "") ), false, false, false, false );
			
			Adapter.GET_SKIN().textField.Draw( rect, new GUIContent( value.ToString() + (postFix ?? "") ), false, false, false, false );
		}
		
		RESTORE_TEXTFIELD();
		
		return value;
	}
	
	
	/*  internal int S_INT_FIELD( string title , int value ) {
	      var R = EditorGUILayout.GetControlRect();
	      // var R = GetControlRect(false, GUILayout.Height(20));
	      return S_INT_FIELD( R , title , value );
	  }
	  internal int S_INT_FIELD( Rect rect , string title , int value ) {
	
	      PREPARE_TEXTFIELD();
	      var result = EditorGUI.IntField(rect, title, value);
	      RESTORE_TEXTFIELD();
	
	      return result;
	  }*/
	internal int S_INT_FIELD( Rect rect, int value, string postFix = null )
	{
	
		PREPARE_TEXTFIELD();
		var crop = rect;
		crop.width /= 2;
		var crop1 = crop;
		
		crop.x += crop.width;
		GUI.SetNextControlName( "MyTextField" );
		
		if ( GUI.enabled ) value = EditorGUI.IntField( crop, value );
		
		if ( GUI.GetNameOfFocusedControl() != "MyTextField" )
		{	if ( GUI.enabled )
			{	GUI.BeginClip( crop1 );
				value = EditorGUI.IntField( new Rect( 0, 0, crop1.width, rect.height ), " ", value );
				GUI.EndClip();
			}
		}
		
		if ( Event.current.type == EventType.Repaint
		        && GUI.GetNameOfFocusedControl() != "MyTextField" )
		{	//if ( !EditorGUIUtility.isProSkin )
			Adapter.GET_SKIN().textField.Draw( rect, new GUIContent( value.ToString() + (postFix ?? "") ), false, false, false, false );
			
			Adapter.GET_SKIN().textField.Draw( rect, new GUIContent( value.ToString() + (postFix ?? "") ), false, false, false, false );
			RESTORE_TEXTFIELD();
		}
		
		return value;
		
		
	}
	
	
	
	
	
	
	
	
	
	
	
	
	static  GUIContent __TOOLTIP = new GUIContent();
	internal static void TOOLTIP( Rect R, string text, bool bold = false )
	{	__TOOLTIP.text = "";
		__TOOLTIP.tooltip = text;
		var label = Adapter.GET_SKIN().label;
		var st = label.fontStyle;
		
		//  var rt = label.richText;
		//  label.richText = true;
		if ( bold ) label.fontStyle = FontStyle.Bold;
		
		GUI.Label( R, __TOOLTIP );
		//   label.richText = rt;
		label.fontStyle = st;
	}
	
	
	internal static void LABEL( Rect R, string text )
	{	var label = Adapter.GET_SKIN().label;
		var al = label.alignment;
		var st = label.fontStyle;
		var rt = label.richText;
		label.richText = true;
		GUI.Label( R, text );
		label.richText = rt;
		label.alignment = al;
		label.fontStyle = st;
	}
	
	internal static void LABEL( Rect R, string text, GUIStyle label )
	{	var al = label.alignment;
		var st = label.fontStyle;
		var rt = label.richText;
		label.richText = true;
		// if ( bold ) label.fontStyle = FontStyle.Bold;
		//  label.alignment = ta ?? TextAnchor.MiddleLeft;
		GUI.Label( R, text, label );
		label.richText = rt;
		label.alignment = al;
		label.fontStyle = st;
	}
	internal static void LABEL( Rect R, string text, bool bold, TextAnchor? ta )
	{	var label = Adapter.GET_SKIN().label;
		var al = label.alignment;
		var st = label.fontStyle;
		var rt = label.richText;
		label.richText = true;
		
		if ( bold ) label.fontStyle = FontStyle.Bold;
		
		label.alignment = ta ?? TextAnchor.MiddleLeft;
		GUI.Label( R, text, label );
		label.richText = rt;
		label.alignment = al;
		label.fontStyle = st;
	}
	
	internal int TOOGLE_POP_INVERCE( ref Rect lineRect, string titile, int value, params string[] cats )
	{	lineRect.height = EditorGUIUtility.singleLineHeight;
		Adapter.LABEL( lineRect, "<i>" + titile + ":</i>" );
		lineRect.y += lineRect.height;
		lineRect.height = EditorGUIUtility.singleLineHeight + 2;
		return cats.Length - 1 - GUI.Toolbar( lineRect, cats.Length - 1 - value, cats.Reverse().ToArray(), EditorStyles.toolbarButton );
	}
	internal int TOOGLE_POP( ref Rect lineRect, string titile, int value, params string[] cats )
	{	lineRect.height = EditorGUIUtility.singleLineHeight;
		Adapter.LABEL( lineRect, "<i>" + titile + ":</i>" );
		lineRect.y += lineRect.height;
		lineRect.height = EditorStyles.toolbarButton.fixedHeight;
		return GUI.Toolbar( lineRect, value, cats, EditorStyles.toolbarButton );
	}
	
	bool _enRich1, _enRich2;
	internal void ENABLE_RICH()
	{	_enRich1 = GET_SKIN().label.richText;
		GET_SKIN().label.richText = true;
		
		_enRich2 = EditorStyles.label.richText;
		EditorStyles.label.richText = true;
		
	}
	internal void DISABLE_RICH()
	{	GET_SKIN().label.richText = _enRich1;
		EditorStyles.label.richText = _enRich2;
	}
	
	internal bool TOGGLE_RIGHT( Rect rect, string title, bool value, bool bold = false, bool? defaultStyle = false, bool skipMark = false )
	{
	
		var oL = EditorStyles.label.alignment;
		var st = EditorStyles.label.fontStyle;
		var oldRT = EditorStyles.label.richText;
		
		EditorStyles.label.richText = true;
		
		if ( bold ) EditorStyles.label.fontStyle = FontStyle.Bold;
		
		EditorStyles.label.alignment = TextAnchor.MiddleLeft;
		
		var result = EditorGUI/*Layout*/.Toggle( rect, /*s + " " +*/ title/* + " " + s*/, value );
		
		EditorStyles.label.richText = oldRT;
		EditorStyles.label.fontStyle = st;
		EditorStyles.label.alignment = oL;
		
		return result;
	}
	internal bool TOGGLE_LEFT( Rect rect, string title, bool value, bool bold = false, bool? defaultStyle = false, bool skipMark = false )
	{	InitializeStyles();
	
	
	
#pragma warning disable
	
		// defaultStyle = null;
		if ( defaultStyle == false )
		{
		
		}
		else
		{	rect.x += 10;
			rect.width -= 8;
		}
		
#pragma warning restore;
		
		var oL = EditorStyles.label.alignment;
		EditorStyles.label.alignment = TextAnchor.MiddleLeft;
		var st = EditorStyles.label.fontStyle;
		
		if ( bold ) EditorStyles.label.fontStyle = FontStyle.Bold;
		
		var oldRT = EditorStyles.label.richText;
		EditorStyles.label.richText = true;
		
		var result = EditorGUI/*Layout*/.ToggleLeft( rect, /*s + " " +*/ title/* + " " + s*/, value );
		
		EditorStyles.label.richText = oldRT;
		EditorStyles.label.fontStyle = st;
		EditorStyles.label.alignment = oL;
		
#if USE_RIGHT_CHECK
		var s =  (value ? "✔" : "✖");//☐☑
		
		if (defaultStyle != null && !skipMark)
		{	var oc = GUI.color;
		
			if (!value) GUI.color *= new Color(1, 1, 1, 0.2f);
			
			GUI.Label(new Rect(rect.x + rect.width - rect.height - 2, rect.y, rect.height, rect.height), s, EditorStyles.label);
			GUI.color = oc;
		}
		
#endif
		
		return result;
	}
	
	
	
	
	
	
	bool _InitializeStylesWas = false;
	internal class ICONID {
		internal static string SETUP_BG = "SETUP_BG";                                      //337x337
		internal static string SETUP_BLUELINE = "SETUP_BLUELINE";                          //22x16
		internal static string SETUP_BOXCAT = "SETUP_BOXCAT";                              //40x38
		internal static string SETUP_BUTTON = "SETUP_BUTTON";                                  //23x22
		internal static string SETUP_BUTTON_HOVER = "SETUP_BUTTON_HOVER";                     //23x22
		internal static string SETUP_CHECKBOX = "SETUP_CHECKBOX";                           //20x20
		internal static string SETUP_CHECKBOX_HOVER = "SETUP_CHECKBOX_HOVER";                   //20x20
		internal static string SETUP_CHECKBOXON = "SETUP_CHECKBOXON";                       //20x20
		internal static string SETUP_CHECKBOXON_HOVER = "SETUP_CHECKBOXON_HOVER";                 //20x20
		internal static string SETUP_DROPDOWN = "SETUP_DROPDOWN";                          //44x18
		internal static string SETUP_DROPDOWN_HOVER = "SETUP_DROPDOWN_HOVER";                   //44x18
		internal static string SETUP_GREENLINE = "SETUP_GREENLINE";                        //3x7
		internal static string SETUP_GREENLINE_HORISONTAL = "SETUP_GREENLINE_HORISONTAL";             //10x6
		internal static string SETUP_NOISE = "SETUP_NOISE";                                    //171x171
		internal static string SETUP_RATEBUTTON = "SETUP_RATEBUTTON";                       //218x29
		internal static string SETUP_RATEBUTTON_HOVER = "SETUP_RATEBUTTON_HOVER";                 //218x29
		internal static string SETUP_SLIDER = "SETUP_SLIDER";                                   //12x12
		internal static string SETUP_SLIDER_HOVER = "SETUP_SLIDER_HOVER";                     //12x12
		internal static string SETUP_TEXTFIELD = "SETUP_TEXTFIELD";                        //24x20
		internal static string SETUP_TEXTFIELD_HOVER = "SETUP_TEXTFIELD_HOVER";                  //24x20
		internal static string SETUP_TITLEHOGLIGHT = "SETUP_TITLEHOGLIGHT";                    //29x28
		
		
		
		internal const string GRID = "GRID";
		internal const string SHADOW = "SHADOW";
		internal const string HIPERUI_BUTTONGLOW = "HIPERUI_BUTTONGLOW";
		internal const string HIPERUI_CLOSE = "HIPERUI_CLOSE";
		internal const string HIPERGRAPH_DOCK = "HIPERGRAPH_DOCK";
		internal const string HIPERUI_GAMEOBJECT = "HIPERUI_GAMEOBJECT";
		internal const string HIPERUI_INOUT_A = "HIPERUI_INOUT_A";
		internal const string HIPERUI_INOUT_B = "HIPERUI_INOUT_B";
		internal const string HIPERUI_LINE_BLUEGB = "HIPERUI_LINE_BLUEGB";
		internal const string HIPERUI_LINE_BOX = "HIPERUI_LINE_BOX";
		internal const string HIPERUI_LINE_RDTRIANGLE = "HIPERUI_LINE_RDTRIANGLE";
		internal const string HIPERUI_MARKER_BOX = "HIPERUI_MARKER_BOX";
		
		internal const string ZOOM_MINUS = "ZOOM MINUS";
		internal const string ZOOM_PLUS = "ZOOM PLUS";
		internal const string ZOOM_THUMB = "ZOOM THUMB";
	}
#pragma warning disable
	internal GUIStyle SETUP_BLUELINE;
	internal GUIStyle SETUP_BOXCAT;
	internal GUIStyle SETUP_BUTTON;
	internal GUIStyle SETUP_CHECKBOX;
	
	internal GUIStyle SETUP_DROPDOWN;
	internal GUIStyle SETUP_GREENLINE;
	internal GUIStyle SETUP_GREENLINE_HORISONTAL;
	internal GUIStyle SETUP_NOISE;
	internal GUIStyle SETUP_RATEBUTTON;
	internal GUIStyle SETUP_SLIDER;
	internal GUIStyle SETUP_TEXTFIELD;
	
	string PSN { get { return (EditorGUIUtility.isProSkin ? "" : " PERSONAL"); } }
	
	internal  GUIStyle SETUP_TITLEHOGLIGHT;
#pragma warning restore
	internal void InitializeStyles()
	{
	
		if ( _InitializeStylesWas ) return;
		
		if ( !wasIconsInitialize ) InitializeIcons();
		
		
		SETUP_BLUELINE = this.InitializeStyle( ICONID.SETUP_BLUELINE, 0.45f, 0.45f, 0, 0 );
		SETUP_BOXCAT = this.InitializeStyle( EditorGUIUtility.isProSkin ? ICONID.SETUP_BOXCAT : "SETUP_BOXCAT_PERSONAL", 0.1f, 0.1f, 0.1f, 0.1f );
		SETUP_BUTTON = this.InitializeStyle( ICONID.SETUP_BUTTON, ICONID.SETUP_BUTTON_HOVER, 0.4f, 0.4f, 0.4f, 0.4f, TextClipping.Clip, Adapter.GET_SKIN().button );
		SETUP_BUTTON.alignment = TextAnchor.MiddleCenter;
		SETUP_CHECKBOX = this.InitializeStyle( ICONID.SETUP_CHECKBOX + PSN, ICONID.SETUP_CHECKBOX_HOVER + PSN, 1f, 0f, 0f, 0f, TextClipping.Clip, Adapter.GET_SKIN().toggle );
		SETUP_CHECKBOX.onFocused.background = SETUP_CHECKBOX.onHover.background = SETUP_CHECKBOX.onNormal.background = GetIcon( ICONID.SETUP_CHECKBOXON + PSN );
		SETUP_CHECKBOX.onActive.background = GetIcon( ICONID.SETUP_CHECKBOXON_HOVER + PSN );
		SETUP_CHECKBOX.border.left = 20;
		
		SETUP_DROPDOWN = this.InitializeStyle( ICONID.SETUP_DROPDOWN + PSN, ICONID.SETUP_DROPDOWN_HOVER + PSN, 0.6f, .2f, 0, 0, TextClipping.Clip, EditorStyles.foldout );
		SETUP_DROPDOWN.padding.left = SETUP_DROPDOWN.normal.background.height + 10;
		//SETUP_DROPDOWN.fontSize = Adapter.GET_SKIN().label.fontSize - 1;
		
		SETUP_GREENLINE = this.InitializeStyle( ICONID.SETUP_GREENLINE, 0f, 0f, 0f, 0f );
		SETUP_GREENLINE_HORISONTAL = this.InitializeStyle( ICONID.SETUP_GREENLINE_HORISONTAL, 0f, 0f, 0f, 0f );
		SETUP_NOISE = this.InitializeStyle( ICONID.SETUP_NOISE, 0f, 0f, 0f, 0f );
		SETUP_RATEBUTTON = this.InitializeStyle( ICONID.SETUP_RATEBUTTON, ICONID.SETUP_RATEBUTTON_HOVER, 0f, 0f, 0f, 0f );
		SETUP_SLIDER = this.InitializeStyle( ICONID.SETUP_SLIDER, ICONID.SETUP_SLIDER_HOVER, 0f, 0f, 0f, 0f, TextClipping.Clip, Adapter.GET_SKIN().horizontalSlider );
		SETUP_TEXTFIELD = this.InitializeStyle( ICONID.SETUP_TEXTFIELD + PSN, ICONID.SETUP_TEXTFIELD_HOVER + PSN, 0f, 0f, 0f, 0f, TextClipping.Clip, Adapter.GET_SKIN().textField );
		
		if ( EditorGUIUtility.isProSkin )
			SETUP_TITLEHOGLIGHT = this.InitializeStyle( ICONID.SETUP_TITLEHOGLIGHT, 0.4f, 0.4f, 0.4f, 0.4f );
		else
		{	SETUP_TITLEHOGLIGHT = this.InitializeStyle( ICONID.SETUP_TITLEHOGLIGHT + " PERSONAL", 0.4f, 0.4f, 0.4f, 0.4f );
		}
		
		if ( wasIconsInitialize ) _InitializeStylesWas = true;
	}
#pragma warning disable
	
	Color __CTEXTFIELDDN;
	Color __CTEXTFIELDDA;
	Color __CTEXTFIELDDF;
	Color __CTEXTFIELDDH;
	Texture2D __TEXTFIELDDN;
	Texture2D __TEXTFIELDDA;
	Texture2D __TEXTFIELDDF;
	Texture2D __TEXTFIELDDH;
	Texture2D __TEXTFIELDON;
	Texture2D __TEXTFIELDOA;
	Texture2D __TEXTFIELDOF;
	Texture2D __TEXTFIELDOH;
	Texture2D[] __TEXTFIELDDNRES;
	Texture2D[] __TEXTFIELDDARES;
	Texture2D[] __TEXTFIELDDFRES;
	Texture2D[] __TEXTFIELDDHRES;
	Texture2D[] __TEXTFIELDONRES;
	Texture2D[] __TEXTFIELDOARES;
	Texture2D[] __TEXTFIELDOFRES;
	Texture2D[] __TEXTFIELDOHRES;
	int __TEXTFIELDFT;
	int __TEXTFIELDFL;
	int __TEXTFIELDFB;
	int __TEXTFIELDFR;
	TextAnchor __TEXTFIAAA;
	int __TEXTFIOPADDR;
	internal void PREPARE_TEXTFIELD()
	{
	
		return;
		
		if ( UNITY_CURRENT_VERSION >= UNITY_2019_3_0_VERSION ) return;
		
		var style =   Adapter.GET_SKIN().textField;
		
		__CTEXTFIELDDN = style.normal.textColor;
		__CTEXTFIELDDA = style.active.textColor;
		__CTEXTFIELDDF = style.focused.textColor;
		__CTEXTFIELDDH = style.hover.textColor;
		
		__TEXTFIELDDN = style.normal.background;
		__TEXTFIELDDA = style.active.background;
		__TEXTFIELDDF = style.focused.background;
		__TEXTFIELDDH = style.hover.background;
		__TEXTFIELDON = style.onNormal.background;
		__TEXTFIELDOA = style.onActive.background;
		__TEXTFIELDOF = style.onFocused.background;
		__TEXTFIELDOH = style.onHover.background;
		__TEXTFIELDDNRES = style.normal.scaledBackgrounds;
		__TEXTFIELDDARES = style.active.scaledBackgrounds;
		__TEXTFIELDDFRES = style.focused.scaledBackgrounds;
		__TEXTFIELDDHRES = style.hover.scaledBackgrounds;
		__TEXTFIELDONRES = style.onNormal.scaledBackgrounds;
		__TEXTFIELDOARES = style.onActive.scaledBackgrounds;
		__TEXTFIELDOFRES = style.onFocused.scaledBackgrounds;
		__TEXTFIELDOHRES = style.onHover.scaledBackgrounds;
		
		__TEXTFIELDFT = style.border.top;
		__TEXTFIELDFL = style.border.left;
		__TEXTFIELDFB = style.border.bottom;
		__TEXTFIELDFR = style.border.right;
		__TEXTFIAAA = style.alignment;
		__TEXTFIOPADDR = style.padding.left;
		
		
		if ( EditorGUIUtility.isProSkin )
		{	if ( EditorGUIUtility.isProSkin )
			{	style.normal.textColor =
				    style.active.textColor =
				        style.focused.textColor =
				            style.hover.textColor = new Color32( 200, 200, 200, 200 );
			}
			
			
			style.normal.background = SETUP_TEXTFIELD.normal.background;
			style.active.background = SETUP_TEXTFIELD.active.background;
			style.focused.background = SETUP_TEXTFIELD.focused.background;
			style.hover.background = SETUP_TEXTFIELD.hover.background;
			style.onNormal.background = SETUP_TEXTFIELD.onNormal.background;
			style.onActive.background = SETUP_TEXTFIELD.onActive.background;
			style.onFocused.background = SETUP_TEXTFIELD.onFocused.background;
			style.onHover.background = SETUP_TEXTFIELD.onHover.background;
			style.normal.scaledBackgrounds = null;
			style.active.scaledBackgrounds = null;
			style.focused.scaledBackgrounds = null;
			style.hover.scaledBackgrounds = null;
			style.onNormal.scaledBackgrounds = null;
			style.onActive.scaledBackgrounds = null;
			style.onFocused.scaledBackgrounds = null;
			style.onHover.scaledBackgrounds = null;
			
			style.border.top = 8;
			style.border.left = 10;
			style.border.bottom = 8;
			style.border.right = 10;
			
		}
		
		style.alignment = TextAnchor.MiddleLeft;
		style.padding.left = 6;
	}
	
	internal void RESTORE_TEXTFIELD()
	{
	
	
		return;
		
		if ( UNITY_CURRENT_VERSION >= UNITY_2019_3_0_VERSION ) return;
		
		var style =   Adapter.GET_SKIN().textField;
		style.normal.textColor = __CTEXTFIELDDN;
		style.active.textColor = __CTEXTFIELDDA;
		style.focused.textColor = __CTEXTFIELDDF;
		style.hover.textColor = __CTEXTFIELDDH;
		style.normal.background = __TEXTFIELDDN;
		style.active.background = __TEXTFIELDDA;
		style.focused.background = __TEXTFIELDDF;
		style.hover.background = __TEXTFIELDDH;
		style.onNormal.background = __TEXTFIELDON;
		style.onActive.background = __TEXTFIELDOA;
		style.onFocused.background = __TEXTFIELDOF;
		style.onHover.background = __TEXTFIELDOH;
		style.normal.scaledBackgrounds = __TEXTFIELDDNRES;
		style.active.scaledBackgrounds = __TEXTFIELDDARES;
		style.focused.scaledBackgrounds = __TEXTFIELDDFRES;
		style.hover.scaledBackgrounds = __TEXTFIELDDHRES;
		style.onNormal.scaledBackgrounds = __TEXTFIELDONRES;
		style.onActive.scaledBackgrounds = __TEXTFIELDOARES;
		style.onFocused.scaledBackgrounds = __TEXTFIELDOFRES;
		style.onHover.scaledBackgrounds = __TEXTFIELDOHRES;
		style.border.top = __TEXTFIELDFT;
		style.border.left = __TEXTFIELDFL;
		style.border.bottom = __TEXTFIELDFB;
		style.border.right = __TEXTFIELDFR;
		
		
		style.alignment = __TEXTFIAAA;
		style.padding.left = __TEXTFIOPADDR;
		
		
	}
	
#pragma warning restore
	
	
}
}
