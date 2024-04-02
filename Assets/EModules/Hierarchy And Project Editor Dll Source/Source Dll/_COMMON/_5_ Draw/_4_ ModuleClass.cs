
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




#if UNITY_EDITOR
namespace EModules.EProjectInternal

{


internal partial class Initializator : EModules.EModulesInternal.Initializator {

}

internal partial class _W__InputWindow : EModules.EModulesInternal._W__InputWindow {

}

internal partial class _W__SearchWindow : EModules.EModulesInternal._W__SearchWindow {

}

internal partial class _W___IWindow : EModules.EModulesInternal._W___IWindow {

}

/*
internal partial class Adapter : EModules.EModulesInternal.Adapter {

internal interface IMethodsInterface : EModules.EModulesInternal.IMethodsInterface {

}
internal Adapter() : base()
{
}
}*/
}
#endif


namespace EModules.EModulesInternal

{
internal partial class Adapter {




	[Serializable]
	internal class MyStruct {
		[SerializeField]
		internal bool enable;
		
		[SerializeField]
		internal int sib;
		
		[SerializeField]
		internal int width;
		
		
		internal void SaveToString( ref System.Text.StringBuilder result )
		{	result.AppendLine( "-" );
			result.AppendLine( enable.ToString() );
			result.AppendLine( sib.ToString() );
			result.AppendLine( width.ToString() );
		}
		internal static MyStruct ReadFromString( ref System.IO.StringReader reader )
		{	var result = new MyStruct();
			result.enable = bool.Parse( reader.ReadLine() );
			result.sib = int.Parse( reader.ReadLine() );
			result.width = int.Parse( reader.ReadLine() );
			return result;
		}
	}
	
	
	
	
	internal class DrawStackItem {
#pragma warning disable
		internal Rect localRect;
		internal Rect rect( Rect worldOffset )
		{	worldOffset.x += localRect.x;
			worldOffset.y += localRect.y;
			worldOffset.width = localRect.width;
			worldOffset.height = localRect.height;
			return worldOffset;
		}
		internal void rect(ref Rect worldRect, ref Rect worldOffset )
		{	localRect.x = worldRect.x - worldOffset.x;
			localRect.y = worldRect.y - worldOffset.y;
			localRect.width = worldRect.width;
			localRect.height = worldRect.height;
		}
		internal ModulesDrawType type;
		internal GUIStyle style;
		// internal Material material;
		internal Texture2D texture;
		internal Color color;
		internal DynamicColor dynamicColor;
		internal Color? nullable_Color;
		internal ScaleMode scaleMode;
		internal  GUIContent content;
		internal DrawStackMethodsWrapper action;
		internal  GUIContent self_ContentInstance;
		internal bool SetStyleColor;
		internal bool OverrideEnable;
		internal bool UseGameObjectEnable;
		internal bool hasContent;
		internal bool self_SkipContentInstance;
		internal int borders;
		internal int SWITCHER_MARKER;
		internal SwithcerMethodsWrapper SWITCHER_SELECTOR;
		public DrawStackItem ADD_SWITCHER( int v )
		{	SWITCHER_MARKER = v;
			return this;
		}
#if !UNITY_EDITOR
		object _args;
		internal object args
		{	get
			{	return _args;
			}
			
			set
			{	if ( value != null &&
			
				        value.GetType() != typeof( AudioSource ) &&
				        value.GetType() != typeof( Hierarchy.M_VerticesHelper ) &&
				        value.GetType() != typeof( Hierarchy.M_CustomIcons.AttributeButton ) &&
				        value.GetType() != typeof( Hierarchy.M_CustomIcons.AttributeField ) &&
				        
				        ( value.GetType().IsClass || value.GetType().IsArray) ) throw new Exception( "class exept" );
				        
				_args = value;
			}
		}
#else
		internal object args;
#endif
		/*  internal string content ;
		  internal string tooltip ;*/
#pragma warning restore
		
		internal void Clear( ModulesDrawType type )
		{	this.type = type;
			// if ( this.tooltip != null ) this.tooltip = null;
		}
	}
	
	internal enum ModulesDrawType
	{	GUIDrawTexture, GUIDrawColor, AdapterDrawTexture, AdapterDrawColor, Label, Button, Style, GUIDrawTextureWithBorders, GUIDrawTextureWithBordersAndMaterial, Action, ModuleButton, SimpleButton
		, BeginClip, EndClip, BEGIN_DRAW_SWITCHER, END_DRAW_SWITCHER
	}
	
	
	internal class DrawStack {
		//  internal HierarchyObject o;
		//      static GUIContent contentDraw = new GUIContent();
		internal bool GO_ENABLE_STATE ;
		int Count;
		internal  int currentStackPos = -1;
		List<DrawStackItem> stack = new List<DrawStackItem>(3) { new DrawStackItem(), new DrawStackItem(), new DrawStackItem()};
		static DrawStackItem emptyStackItem = new DrawStackItem();
		internal  Rect worldOffset;
		public DrawStackItem PUSH( ModulesDrawType type )
		{	if ( DRAW_AS_EMPTY )
			{	emptyStackItem.Clear( type );
				return emptyStackItem;
			}
			
			Count++;
			
			if ( currentStackPos == -2 ) currentStackPos++;
			
			currentStackPos++;
			
			if ( currentStackPos >= stack.Count ) stack.Add( new DrawStackItem() );
			
			stack[currentStackPos].Clear( type );
			return stack[currentStackPos];
		}
		/* public DrawStackItem POP()
		 {   var res = stack[currentStackPos];
		     currentStackPos--;
		     return res;
		 }*/
		
		public void ResetStack(  )
		{	Count = 0;
			currentStackPos = -1;
			NOW_SWITCHER = false;
		}
		/*   public DrawStackItem ACTION( Action action, bool skipSearch )
		 {   if ( skipSearch && HAS_SEARCH_STRING() ) return empty;
		     if ( !DODRAW_SEARCH() ) return empty;
		     var s = PUSH(ModulesDrawType.Action);
		     s.action = action;
		     return s;
		 }*/
		
		public DrawStackItem BEGIN_DRAW_SWITCHER( SwithcerMethodsWrapper method )
		{	var s = PUSH(ModulesDrawType.BEGIN_DRAW_SWITCHER);
			s.SWITCHER_SELECTOR = method;
			return s;
		}
		public DrawStackItem END_DRAW_SWITCHER(  )
		{	var s = PUSH(ModulesDrawType.END_DRAW_SWITCHER);
			return s;
		}
		public DrawStackItem Draw_BeginClip( Rect rect )
		{	var s = PUSH(ModulesDrawType.BeginClip);
			s.rect (ref rect, ref  worldOffset);
			return s;
		}
		public DrawStackItem Draw_EndClip()
		{	var s = PUSH(ModulesDrawType.EndClip);
			return s;
		}
		public DrawStackItem Draw_Style( Rect rect,  GUIStyle style, GUIContent content, Color? color, bool USE_GO  )         // return BUTTON( new[] { textArray }, new[] { buttonActionArray }, height );
		{	var s = PUSH(ModulesDrawType.Style);
			s.rect( ref rect, ref worldOffset );
			s.style = style;
			s.content = content;
			s.UseGameObjectEnable = USE_GO;
			s.nullable_Color = color;
			s.OverrideEnable = true;
			/*  s.tooltip = content.tooltip;
			  s.content = content.text;*/
			return s;
		}
		public DrawStackItem Draw_Style( Rect rect, GUIStyle style, string content, Color? color, bool USE_GO )           // return BUTTON( new[] { textArray }, new[] { buttonActionArray }, height );
		{	var s = PUSH(ModulesDrawType.Style);
			s.rect( ref rect, ref worldOffset );
			s.style = style;
			
			if ( s.self_ContentInstance == null ) s.self_ContentInstance = new GUIContent();
			
			s.self_ContentInstance.text = content;
			s.self_ContentInstance.tooltip = null;
			s.content = null;
			s.UseGameObjectEnable = USE_GO;
			s.nullable_Color = color;
			s.OverrideEnable = true;
			/*  s.tooltip = content.tooltip;
			  s.content = content.text;*/
			return s;
		}
		public DrawStackItem Draw_Button( Rect rect, string textArray, Action buttonActionArray, int? height )
		{	// return BUTTON( new[] { textArray }, new[] { buttonActionArray }, height );
			var s = PUSH(ModulesDrawType.Button);
			s.rect( ref rect, ref worldOffset );
			return s;
		}
		public DrawStackItem Draw_GUITexture( Rect rect, Texture2D icon, Color guiColor, bool USE_GO)
		{	var s = PUSH(ModulesDrawType.GUIDrawTexture);
			s.rect( ref rect, ref worldOffset );
			s.color = guiColor ;
			
			if ( !icon ) throw new NullReferenceException();
			
			s.texture = icon;
			s.UseGameObjectEnable = USE_GO;
			return s;
		}
		public DrawStackItem Draw_GUITexture( Rect rect, Color color, bool USE_GO )
		{	var s = PUSH(ModulesDrawType.GUIDrawColor);
			s.rect( ref rect, ref worldOffset );
			s.color = color;
			s.UseGameObjectEnable = USE_GO;
			return s;
		}
		public DrawStackItem Draw_AdapterTexture( Rect rect, Texture2D icon, Color guiColor, bool USE_GO )
		{	var s = PUSH(ModulesDrawType.AdapterDrawTexture);
			s.rect( ref rect, ref worldOffset );
			
			if ( !icon ) throw new NullReferenceException();
			
			s.scaleMode = ScaleMode.ScaleToFit;
			s.color = guiColor ;
			s.texture = icon;
			s.UseGameObjectEnable = USE_GO;
			return s;
		}
		public DrawStackItem Draw_AdapterTexture( Rect rect, Color color, bool USE_GO )
		{	var s = PUSH(ModulesDrawType.AdapterDrawColor);
			s.rect( ref rect, ref worldOffset );
			s.color = color;
			s.dynamicColor = null;
			s.scaleMode = ScaleMode.ScaleToFit;
			s.UseGameObjectEnable = USE_GO;
			return s;
		}
		public DrawStackItem Draw_AdapterTexture( Rect rect, DynamicColor dynamicColor, bool USE_GO )
		{	var s = PUSH(ModulesDrawType.AdapterDrawColor);
			s.rect( ref rect, ref worldOffset );
			s.dynamicColor = dynamicColor;
			s.scaleMode = ScaleMode.ScaleToFit;
			s.UseGameObjectEnable = USE_GO;
			return s;
		}
		internal DrawStackItem Draw_GUITextureWithBorders( Rect rect,  Texture2D texture, int borders, DynamicColor color, bool USE_GO, object args )
		{	var s = PUSH(ModulesDrawType.GUIDrawTextureWithBorders);
			s.rect( ref rect, ref worldOffset );
			s.dynamicColor = color;
			s.scaleMode = ScaleMode.ScaleToFit;
			s.UseGameObjectEnable = USE_GO;
			s.texture = texture;
			s.borders = borders;
			s.args = args;
			return s;
		}
		
		internal DrawStackItem Draw_GUITextureWithBordersAndMaterial( Rect rect, Texture2D texture, int borders, Material mat, DynamicColor color, bool USE_GO, object args )
		{	var s = PUSH(ModulesDrawType.GUIDrawTextureWithBordersAndMaterial);
			s.rect( ref rect, ref worldOffset );
			s.dynamicColor = color;
			s.scaleMode = ScaleMode.ScaleToFit;
			s.UseGameObjectEnable = USE_GO;
			s.texture = texture;
			// s.material = mat;
			s.borders = borders;
			s.args = args;
			return s;
		}
		public DrawStackItem Draw_Action( Rect rect, DrawStackMethodsWrapper action, object args, GUIContent content)
		{	var s = PUSH(ModulesDrawType.Action);
			s.rect( ref rect, ref worldOffset );
			s.action = action;
			s.args = args;
			
			if ( content  == null)
				s.content = null;
			else
			{	if ( s.self_ContentInstance == null ) s.self_ContentInstance = new GUIContent();
			
				s.self_ContentInstance.text = content.text;
				s.self_ContentInstance.tooltip = content.tooltip;
				s.content = s.self_ContentInstance;
			}
			
			return s;
		}
		public DrawStackItem Draw_LabelWithTextColor( Rect rect, string content, Color textColor, GUIStyle style, bool ENABLE_ACTIVE_IN_HIERARCHY, Color? c, bool? OVERRIDE_ENABLE )
		{	var s = PUSH(ModulesDrawType.Label);
			s.rect( ref rect, ref worldOffset );
			
			if ( s.self_ContentInstance == null ) s.self_ContentInstance = new GUIContent();
			
			s.self_ContentInstance.text = content;
			s.self_ContentInstance.tooltip = null;
			s.style = style;
			s.UseGameObjectEnable = ENABLE_ACTIVE_IN_HIERARCHY;
			s.nullable_Color = c;
			s.color = textColor;
			s.dynamicColor = null;
			s.SetStyleColor = true;
			s.OverrideEnable = OVERRIDE_ENABLE ?? true;
			return s;
		}
		public DrawStackItem Draw_LabelWithTextColor( Rect rect, string content, DynamicColor textColor, GUIStyle style, bool ENABLE_ACTIVE_IN_HIERARCHY, Color? c, bool? OVERRIDE_ENABLE )
		{	var s = PUSH(ModulesDrawType.Label);
			s.rect( ref rect, ref worldOffset );
			
			if ( s.self_ContentInstance == null ) s.self_ContentInstance = new GUIContent();
			
			s.self_ContentInstance.text = content;
			s.self_ContentInstance.tooltip = null;
			s.style = style;
			s.UseGameObjectEnable = ENABLE_ACTIVE_IN_HIERARCHY;
			s.nullable_Color = c;
			s.dynamicColor = textColor;
			s.SetStyleColor = true;
			s.OverrideEnable = OVERRIDE_ENABLE ?? true;
			return s;
		}
		
		
		public DrawStackItem Draw_Label( Rect rect, string content, GUIStyle style,  bool ENABLE_ACTIVE_IN_HIERARCHY, Color? c, bool? OVERRIDE_ENABLE)
		{	var s = PUSH(ModulesDrawType.Label);
			s.rect( ref rect, ref worldOffset );
			
			if ( s.self_ContentInstance == null ) s.self_ContentInstance = new GUIContent();
			
			s.self_ContentInstance.text = content;
			s.self_ContentInstance.tooltip = null;
			s.style = style;
			s.UseGameObjectEnable = ENABLE_ACTIVE_IN_HIERARCHY;
			s.dynamicColor = null;
			s.nullable_Color = c;
			s.SetStyleColor = false;
			s.OverrideEnable = OVERRIDE_ENABLE ?? true;
			return s;
		}
		public DrawStackItem Draw_Label( Rect rect, GUIContent content, GUIStyle style,  bool ENABLE_ACTIVE_IN_HIERARCHY, Color? c, bool? OVERRIDE_ENABLE )
		{	var s = PUSH(ModulesDrawType.Label);
			s.rect( ref rect, ref worldOffset );
			
			if ( s.self_ContentInstance == null ) s.self_ContentInstance = new GUIContent();
			
			s.self_ContentInstance.text = content.text;
			s.self_ContentInstance.tooltip = content.tooltip;
			s.style = style;
			s.UseGameObjectEnable = ENABLE_ACTIVE_IN_HIERARCHY;
			s.dynamicColor = null;
			s.nullable_Color = c;
			s.SetStyleColor = false;
			s.OverrideEnable = OVERRIDE_ENABLE ?? true;
			return s;
		}
		public DrawStackItem Draw_ModuleButton( Rect rect, GUIContent content, DrawStackMethodsWrapper action, bool hasContent, object args, bool useContentForButton, GUIStyle style,
		                                        bool USE_GO = false)
		{	var s = PUSH(ModulesDrawType.ModuleButton);
			s.rect( ref rect, ref worldOffset );
			s.action = action;
			s.style = style;
			s.content = null;
			
			if ( content != null )
			{	if ( useContentForButton )
				{	if ( s.self_ContentInstance == null ) s.self_ContentInstance = new GUIContent();
				
					s.self_ContentInstance.text = content.text;
					s.self_ContentInstance.tooltip = content.tooltip;
					s.content = s.self_ContentInstance;
				}
				
				else
				{	if ( s.self_ContentInstance == null ) s.self_ContentInstance = new GUIContent();
				
					s.self_ContentInstance.text = content.text;
					s.self_ContentInstance.tooltip = content.tooltip;
				}
				
				s.self_SkipContentInstance = false;
			}
			
			else s.self_SkipContentInstance = true;
			
			s.hasContent = hasContent;
			s.args = args;
			s.UseGameObjectEnable = USE_GO;
			return s;
		}
		public DrawStackItem Draw_SimpleButton( Rect rect, GUIContent content, DrawStackMethodsWrapper action, object args, bool useContentForButton, GUIStyle style, bool USE_GO = false )
		{	var s = PUSH(ModulesDrawType.SimpleButton);
			s.rect( ref rect, ref worldOffset );
			s.action = action;
			s.style = style;
			s.content = null;
			
			if ( content != null )
			{	if ( useContentForButton )
				{	if ( s.self_ContentInstance == null ) s.self_ContentInstance = new GUIContent();
				
					s.self_ContentInstance.text = content.text;
					s.self_ContentInstance.tooltip = content.tooltip;
					s.content = s.self_ContentInstance;
				}
				
				else
				{	if ( s.self_ContentInstance == null ) s.self_ContentInstance = new GUIContent();
				
					s.self_ContentInstance.text = content.text;
					s.self_ContentInstance.tooltip = content.tooltip;
				}
				
				s.self_SkipContentInstance = false;
			}
			
			else s.self_SkipContentInstance = true;
			
			s.args = args;
			s.UseGameObjectEnable = USE_GO;
			return s;
		}
		
		
		
		
		
		public DrawStackItem LABEL( string text )
		{	var s = PUSH(ModulesDrawType.Button);
			return s;
		}
		
		internal void Draw(Rect worldOffset,  Adapter.HierarchyObject o, Adapter adapter )
		{	// Debug.Log( Count );
			this.worldOffset = worldOffset;
			
			for ( int i = 0 ; i < Count ; i++ )
			{	DrawSIngleItem( stack[i], o, adapter );
			}
		}
		
		internal  Adapter.HierarchyObject current_object = null;
		
		internal void DrawSIngleItem( DrawStackItem stack, Adapter adapter )
		{	if ( current_object == null ) throw new NullReferenceException( "current_object" );
		
			DrawSIngleItem(  stack, current_object, adapter );
		}
		static int colorProperty = Shader.PropertyToID("_Color");
		Color cache_c;
		SwithcerMethodsWrapper NOW_SWITCHER_SELECTOR;
		bool NOW_SWITCHER;
		
		internal void DrawSIngleItem(   DrawStackItem stack, Adapter.HierarchyObject _o, Adapter adapter)
		{
		
			if ( NOW_SWITCHER && stack.type != ModulesDrawType.END_DRAW_SWITCHER && NOW_SWITCHER_SELECTOR.action( _o ) != stack.SWITCHER_MARKER ) return;
			
			
			switch ( stack.type )
			{
			
				case ModulesDrawType.BEGIN_DRAW_SWITCHER:
				{	NOW_SWITCHER = true;
					NOW_SWITCHER_SELECTOR = stack.SWITCHER_SELECTOR;
					break;
				}
				
				case ModulesDrawType.END_DRAW_SWITCHER:
				{	NOW_SWITCHER = false;
					break;
				}
				
				
				case ModulesDrawType.BeginClip:
				{	GUI.BeginClip( stack.rect(worldOffset) );
					break;
				}
				
				case ModulesDrawType.EndClip:
				{	GUI.EndClip(  );
					break;
				}
				
				case ModulesDrawType.GUIDrawTextureWithBordersAndMaterial:
				{	if ( Event.current.type != EventType.Repaint ) return;
				
					var c = stack.dynamicColor.GET(_o, stack.args);
					
					if ( stack.UseGameObjectEnable && !_o.Active() ) c.a *= 0.5f;
					
					// bordersWidth.x = bordersWidth.y = bordersWidth.z = bordersWidth.w = stack.borders;
					// if ( !stack.material ) stack.material = adapter.HIghlighterExternalMaterial;
					// c = Color.black;
					adapter.HIghlighterExternalMaterial.SetColor( colorProperty, c * GUI.color );
					Graphics.DrawTexture( stack.rect( worldOffset ), stack.texture, stack.borders, stack.borders, stack.borders, stack.borders,
					                      adapter.HIghlighterExternalMaterial, 0 );
					break;
				}
				
				case ModulesDrawType.GUIDrawTextureWithBorders:
				{	if ( Event.current.type != EventType.Repaint ) return;
				
					var c = stack.dynamicColor.GET(_o, stack.args);
					
					if ( stack.UseGameObjectEnable && !_o.Active() ) c.a *= 0.5f;
					
					// bordersWidth.x = bordersWidth.y = bordersWidth.z = bordersWidth.w = stack.borders;
					//   c = Color.black;
					//  adapter.GL_DrawTexture( stack.rect, stack.texture, ScaleMode.StretchToFill, true, 1, c, bordersWidth, Vector4.zero );
					Adapter.DrawTexture( stack.rect( worldOffset ), stack.texture, ScaleMode.StretchToFill, true, 1, c, stack.borders, 0 );
					break;
				}
				
				case ModulesDrawType.GUIDrawTexture:
				{	if ( Event.current.type != EventType.Repaint ) return;
				
					var c = stack.color;
					
					if ( stack.UseGameObjectEnable && !_o.Active() ) c.a *= 0.5f;
					
					// adapter.GL_DrawTexture( stack.rect, stack.texture, ScaleMode.StretchToFill, true, 1, c, Vector4.zero, Vector4.zero );
					Adapter.DrawTexture( stack.rect( worldOffset ), stack.texture, ScaleMode.StretchToFill, true, 1, c, 0, 0 );
					break;
				}
				
				case ModulesDrawType.GUIDrawColor:
				{	if ( Event.current.type != EventType.Repaint ) return;
				
					var c = stack.color;
					
					if ( stack.UseGameObjectEnable && !_o.Active() ) c.a *= 0.5f;
					
					//  adapter.GL_DrawTexture( stack.rect, Texture2D.whiteTexture, ScaleMode.StretchToFill, true, 1, c, Vector4.zero, Vector4.zero );
					Adapter.DrawTexture( stack.rect( worldOffset ), Texture2D.whiteTexture, ScaleMode.StretchToFill, true, 1, c, 0, 0 );
					break;
				}
				
				case ModulesDrawType.AdapterDrawTexture:
				{	if ( Event.current.type != EventType.Repaint ) return;
				
					var c = stack.color;
					
					if ( stack.UseGameObjectEnable && !_o.Active() ) c.a *= 0.5f;
					
					//   adapter.GL_DrawTexture( stack.rect, stack.texture, stack.scaleMode, true, 1, c, Vector4.zero, Vector4.zero );
					Adapter.DrawTexture( stack.rect( worldOffset ), stack.texture, stack.scaleMode, true, 1, c, 0, 0 );
					break;
				}
				
				case ModulesDrawType.AdapterDrawColor:
				{	if ( Event.current.type != EventType.Repaint ) return;
				
					var c = stack.dynamicColor != null ? stack.dynamicColor.GET(_o, null) : stack.color;
					
					if ( stack.UseGameObjectEnable && !_o.Active() ) c.a *= 0.5f;
					
					//  adapter.GL_DrawTexture( stack.rect, Texture2D.whiteTexture, ScaleMode.StretchToFill, true, 1, c, Vector4.zero, Vector4.zero );
					Adapter.DrawTexture( stack.rect( worldOffset ), Texture2D.whiteTexture, ScaleMode.StretchToFill, true, 1, c, 0, 0 );
					break;
				}
				
				case ModulesDrawType.Style:
				{	if ( Event.current.type == EventType.Repaint )
					{	/*  contentDraw.text = stack.content;
						  contentDraw.tooltip = stack.tooltip;*/
						if ( stack.nullable_Color.HasValue )
						{	cache_c = GUI.color;
							GUI.color *= stack.nullable_Color.Value;
						}
						
						if ( stack.UseGameObjectEnable || !stack.OverrideEnable )
						{	var en = GUI.enabled;
							var a = _o.Active();
							GUI.enabled &= stack.OverrideEnable && a;
							
							if ( !a )
							{	alphacache = GUI.color;
								GUI.color *= alpha;
							}
							
							stack.style.Draw( stack.rect( worldOffset ), stack.content ?? stack.self_ContentInstance, false, false, false, false );
							
							if ( !a ) GUI.color = alphacache;
							
							GUI.enabled = en;
						}
						
						else
						{	stack.style.Draw( stack.rect( worldOffset ), stack.content ?? stack.self_ContentInstance, false, false, false, false );
						}
						
						if ( stack.nullable_Color.HasValue ) GUI.color = cache_c;
					}
					
					
					break;
				}
				
				case ModulesDrawType.Action:
				{	data.args = stack.args;
					data.content = stack.content;
					stack.action.action( worldOffset, stack.rect( worldOffset ), data, _o);
					break;
				}
				
				case ModulesDrawType.Label:
				{
				
					bool notol = ( stack.self_ContentInstance.tooltip == null || stack.self_ContentInstance.tooltip == "");
					
					// bool notol = false;
					if ( notol && Event.current.type != EventType.Repaint ) return;
					
					if ( stack.nullable_Color.HasValue )
					{	cache_c = GUI.color;
						GUI.color *= stack.nullable_Color.Value;
					}
					
					if (stack.UseGameObjectEnable || !stack.OverrideEnable)
					{	var en = GUI.enabled;
						var a = _o.Active();
						GUI.enabled &= stack.OverrideEnable && a;
						
						if ( !a )
						{	alphacache = GUI.color;
							GUI.color *= alpha;
						}
						
						if ( stack.SetStyleColor )
						{	styleCacheColor = stack.style.normal.textColor;
						
							if ( stack.dynamicColor != null )
							{	stack.style.normal.textColor = stack.dynamicColor.GET( _o, null);
							}
							
							else stack.style.normal.textColor = stack.color;
							
							// if ( Event.current.type == EventType.Repaint ) stack.style.Draw( stack.rect, "", false, false, false, false );
							if ( notol ) stack.style.Draw( stack.rect( worldOffset ), stack.self_ContentInstance, false, false, false, false);
							else GUI.Label( stack.rect( worldOffset ), stack.self_ContentInstance, stack.style );
							
							stack.style.normal.textColor = styleCacheColor;
						}
						
						else
						{	if ( notol ) stack.style.Draw( stack.rect( worldOffset ), stack.self_ContentInstance, false, false, false, false );
							else GUI.Label( stack.rect( worldOffset ), stack.self_ContentInstance, stack.style );
						}
						
						if ( !a ) GUI.color = alphacache;
						
						GUI.enabled = en;
					}
					
					else
					{	if ( stack.SetStyleColor )
						{	styleCacheColor = stack.style.normal.textColor;
						
							if ( stack.dynamicColor != null )
							{	stack.style.normal.textColor = stack.dynamicColor.GET( _o, null );
							
							}
							
							else stack.style.normal.textColor = stack.color;
							
							//if ( Event.current.type == EventType.Repaint ) stack.style.Draw( stack.rect, "", false, false, false, false );
							if ( notol ) stack.style.Draw( stack.rect( worldOffset ), stack.self_ContentInstance, false, false, false, false );
							else GUI.Label( stack.rect( worldOffset ), stack.self_ContentInstance, stack.style );
							
							stack.style.normal.textColor = styleCacheColor;
						}
						
						else
						{	if ( notol ) stack.style.Draw( stack.rect( worldOffset ), stack.self_ContentInstance, false, false, false, false );
							else GUI.Label( stack.rect( worldOffset ), stack.self_ContentInstance, stack.style );
						}
					}
					
					if ( stack.nullable_Color.HasValue ) GUI.color = cache_c;
					
					break;
				}
				
				case ModulesDrawType.ModuleButton://stack.self_SkipContentInstance ? null : stack.self_ContentInstance
				{	if ( stack.UseGameObjectEnable )
					{	var a = _o.Active();
					
						if ( !a )
						{	alphacache = GUI.color;
							GUI.color *= alpha;
						}
						
						if ( ( stack.content != null && !string.IsNullOrEmpty(stack.content.text)|| !adapter.hashoveredItem ||adapter.hoverID ==_o.id )  &&
						        adapter.ModuleButton( stack.rect( worldOffset ), stack.content, stack.hasContent, stack.style ) )
						{	data.args = stack.args;
							data.content = !stack.self_SkipContentInstance ? stack.self_ContentInstance : null;
							
							stack.action.action( worldOffset, stack.rect( worldOffset ), data, _o );
						}
						
						if ( !a ) GUI.color = alphacache;
					}
					
					else
					{	if ( ( stack.content != null&& !string.IsNullOrEmpty(stack.content.text) || !adapter.hashoveredItem ||adapter.hoverID ==_o.id )  &&
						        adapter.ModuleButton( stack.rect( worldOffset ), stack.content, stack.hasContent, stack.style ) )
						{	data.args = stack.args;
							data.content = !stack.self_SkipContentInstance ? stack.self_ContentInstance : null;
							
							stack.action.action( worldOffset, stack.rect( worldOffset ), data, _o);
						}
					}
					
					break;
				}
				
				case ModulesDrawType.SimpleButton://stack.self_SkipContentInstance ? null : stack.self_ContentInstance
				{	if ( stack.UseGameObjectEnable )
					{	var a = _o.Active();
					
						if ( !a )
						{	alphacache = GUI.color;
							GUI.color *= alpha;
						}
						
						if (( stack.content != null && !string.IsNullOrEmpty(stack.content.text)|| !adapter.hashoveredItem ||adapter.hoverID ==_o.id )  &&
						        adapter.SimpleButton( stack.rect( worldOffset ), stack.content, stack.style ) )
						{	data.args = stack.args;
							data.content = !stack.self_SkipContentInstance ? stack.self_ContentInstance : null;
							stack.action.action( worldOffset, stack.rect( worldOffset ), data, _o  );
						}
						
						if ( !a ) GUI.color = alphacache;
					}
					
					else
					{	if ( ( stack.content != null && !string.IsNullOrEmpty(stack.content.text) || !adapter.hashoveredItem ||adapter.hoverID ==_o.id )  &&
						        adapter.SimpleButton( stack.rect( worldOffset ), stack.content, stack.style ) )
						{	data.args = stack.args;
							data.content = !stack.self_SkipContentInstance ? stack.self_ContentInstance : null;
							stack.action.action( worldOffset, stack.rect( worldOffset ), data, _o  );
						}
					}
					
					break;
				}
			}
		}
		static DrawStackMethodsWrapperData data;
		static Color styleCacheColor;
		static Color alphacache =  new Color(1, 1, 1, 0.5f);
		static Color alpha =  new Color(1, 1, 1, 0.5f);
		internal bool DRAW_AS_EMPTY = false;
	}
	
	
	
	internal class DrawStackAdapter {
	
		internal virtual Adapter adapter { get; set; }
		bool perfadditionalcondition = true;
		
		internal Rect ConvertToLocal(Rect rect )
		{	rect.x -= GetCurrentStack().worldOffset.x;
			rect.y -= GetCurrentStack().worldOffset.y;
			return rect;
		}
		
		internal bool START_DRAW(Rect worldOffset, Adapter.HierarchyObject _o)
		{	if ( PERFOMANCE_BARS )
			{	if ( TryToDraw( worldOffset, _o ) ) return false;
			
				CURRENT_STACK = StackInstance( _o );
				CURRENT_STACK.current_object = _o;
				CURRENT_STACK.worldOffset = worldOffset;
			}
			
			else
			{	emptyStack.current_object = _o;
				emptyStack.worldOffset = worldOffset;
			}
			
			return true;
		}
		internal bool START_DRAW_PARTLY_TRYDRAW( Rect worldOffset, Adapter.HierarchyObject _o )
		{	if ( PERFOMANCE_BARS )
			{	if ( TryToDraw( worldOffset, _o ) ) return false;
			}
			
			else
			{
			}
			
			return true;
		}
		internal void START_DRAW_PARTLY_CREATEINSTANCE( Rect worldOffset, Adapter.HierarchyObject _o, bool additionalcondition )
		{	perfadditionalcondition = additionalcondition;
		
			if ( PERFOMANCE_BARS && additionalcondition )
			{	CURRENT_STACK = StackInstance( _o );
				CURRENT_STACK.current_object = _o;
				CURRENT_STACK.worldOffset = worldOffset;
			}
			
			else
			{	emptyStack.current_object = _o;
				emptyStack.worldOffset = worldOffset;
			}
		}
		internal void END_DRAW( Adapter.HierarchyObject _o )
		{	if ( CURRENT_STACK != null )
			{	CURRENT_STACK.Draw( CURRENT_STACK.worldOffset, _o, adapter);
			
				if ( CURRENT_STACK.currentStackPos == -1 ) CURRENT_STACK.currentStackPos = -2;
				
				CURRENT_STACK = null;
			}
		}
		
		internal virtual bool PERFOMANCE_BARS
		{	get
			{	return adapter.CACHING_TEXTURES_STACKS ;
			}
		}
		
		Adapter.DrawStack GetCurrentStack()
		{	if ( PERFOMANCE_BARS && perfadditionalcondition ) return CURRENT_STACK;
		
			return emptyStack;
		}
		
		internal void BEGIN_DRAW_SWITCHER( SwithcerMethodsWrapper method  )
		{	if ( PERFOMANCE_BARS && perfadditionalcondition ) CURRENT_STACK.BEGIN_DRAW_SWITCHER(  method );
			else emptyStack.DrawSIngleItem( emptyStack.BEGIN_DRAW_SWITCHER(method), adapter );
		}
		internal void END_DRAW_SWITCHER()
		{	if ( PERFOMANCE_BARS && perfadditionalcondition ) CURRENT_STACK.END_DRAW_SWITCHER(  );
			else emptyStack.DrawSIngleItem( emptyStack.END_DRAW_SWITCHER(  ), adapter );
		}
		
		internal  Adapter.DrawStack emptyStack = new DrawStack() { DRAW_AS_EMPTY = true };
		internal   Adapter.DrawStack CURRENT_STACK;
		internal void Draw_BeginClip( Rect rect )
		{	if ( PERFOMANCE_BARS && perfadditionalcondition ) CURRENT_STACK.Draw_BeginClip( rect);
			else emptyStack.DrawSIngleItem( emptyStack.Draw_BeginClip( rect ), adapter );
		}
		internal void Draw_EndClip(  )
		{	if ( PERFOMANCE_BARS && perfadditionalcondition ) CURRENT_STACK.Draw_EndClip(  );
			else emptyStack.DrawSIngleItem( emptyStack.Draw_EndClip(  ), adapter );
		}
		internal void Draw_GUITexture( Rect rect, Texture2D texture, Color? guiColor = null, bool USE_GO = false )
		{	if ( PERFOMANCE_BARS && perfadditionalcondition ) CURRENT_STACK.Draw_GUITexture( rect, texture, guiColor ?? Color.white, USE_GO );
			else emptyStack.DrawSIngleItem( emptyStack.Draw_GUITexture( rect, texture, guiColor ?? Color.white, USE_GO ), adapter );
		}
		internal void Draw_AdapterTexture( Rect rect, Color color, Color? guiColor = null, bool USE_GO = false )
		{	if ( guiColor.HasValue ) color *= guiColor.Value;
		
			if ( PERFOMANCE_BARS && perfadditionalcondition ) CURRENT_STACK.Draw_AdapterTexture( rect, color, USE_GO );
			else emptyStack.DrawSIngleItem( emptyStack.Draw_AdapterTexture( rect, color, USE_GO ), adapter );
		}
		internal void Draw_AdapterTextureWithDynamicColor( Rect rect, DynamicColor color, bool USE_GO = false, int SWITCHER = 0)
		{	if ( PERFOMANCE_BARS && perfadditionalcondition ) CURRENT_STACK.Draw_AdapterTexture( rect, color, USE_GO ).ADD_SWITCHER( SWITCHER);
			else emptyStack.DrawSIngleItem( emptyStack.Draw_AdapterTexture( rect, color, USE_GO ).ADD_SWITCHER( SWITCHER ), adapter );
		}
		internal void Draw_AdapterTexture( Rect rect, Texture2D texture, Color? guiColor = null, bool USE_GO = false )
		{	if ( PERFOMANCE_BARS && perfadditionalcondition ) CURRENT_STACK.Draw_AdapterTexture( rect, texture, guiColor ?? Color.white, USE_GO );
			else emptyStack.DrawSIngleItem( emptyStack.Draw_AdapterTexture( rect, texture, guiColor ?? Color.white, USE_GO ), adapter );
		}
		internal void Draw_GUITextureWithBorders( Rect rect, Texture2D texture, int borders, DynamicColor guiColor, object args  )
		{	if ( PERFOMANCE_BARS && perfadditionalcondition ) CURRENT_STACK.Draw_GUITextureWithBorders( rect,  texture,  borders, guiColor, false, args );
			else emptyStack.DrawSIngleItem( emptyStack.Draw_GUITextureWithBorders( rect, texture, borders, guiColor, false, args ), adapter );
		}
		internal void Draw_GUITextureWithBordersAndMaterial( Rect rect, Texture2D texture, int borders, Material mat, DynamicColor guiColor, object args )
		{	if ( PERFOMANCE_BARS && perfadditionalcondition ) CURRENT_STACK.Draw_GUITextureWithBordersAndMaterial( rect,  texture,  borders,  mat, guiColor, false, args );
			else emptyStack.DrawSIngleItem( emptyStack.Draw_GUITextureWithBordersAndMaterial( rect, texture, borders, mat, guiColor, false, args ), adapter );
		}
		internal void Draw_Style( Rect rect, GUIStyle style, GUIContent content, Color? color = null, bool USE_GO = false )
		{	if ( PERFOMANCE_BARS && perfadditionalcondition ) CURRENT_STACK.Draw_Style( rect, style, content, color, USE_GO );
			else emptyStack.DrawSIngleItem( emptyStack.Draw_Style( rect, style, content, color, USE_GO ), adapter );
		}
		internal void Draw_Style( Rect rect, GUIStyle style, string content, Color? color = null, bool USE_GO = false)
		{	if ( PERFOMANCE_BARS && perfadditionalcondition ) CURRENT_STACK.Draw_Style( rect, style, content, color, USE_GO );
			else emptyStack.DrawSIngleItem( emptyStack.Draw_Style( rect, style, content, color, USE_GO ), adapter );
		}
		internal void Draw_Action(Rect rect, DrawStackMethodsWrapper method, object args, GUIContent content = null )
		{	if ( PERFOMANCE_BARS && perfadditionalcondition ) CURRENT_STACK.Draw_Action( rect, method, args, content );
			else emptyStack.DrawSIngleItem( emptyStack.Draw_Action( rect, method, args, content ), adapter );
		}
		internal void Draw_Label( Rect rect, GUIContent content, GUIStyle style, bool USE_GO, Color? color = null, bool? ADDITIONAL_ENABLE = null)
		{	if ( !adapter.DRAW_LABEL_FOR_EMPTY_CONTENT && content != null && content.text == "-" ) return;
		
			if ( PERFOMANCE_BARS && perfadditionalcondition ) CURRENT_STACK.Draw_Label(  rect,  content,  style,  USE_GO,  color, ADDITIONAL_ENABLE );
			else emptyStack.DrawSIngleItem( emptyStack.Draw_Label(  rect,  content,  style, USE_GO,  color, ADDITIONAL_ENABLE ), adapter );
		}
		internal void Draw_Label( Rect rect, string content, GUIStyle style, bool USE_GO, Color? color = null, bool? ADDITIONAL_ENABLE = null )
		{	if ( !adapter.DRAW_LABEL_FOR_EMPTY_CONTENT && content == "-" ) return;
		
			if ( PERFOMANCE_BARS && perfadditionalcondition ) CURRENT_STACK.Draw_Label( rect, content, style, USE_GO, color, ADDITIONAL_ENABLE );
			else emptyStack.DrawSIngleItem( emptyStack.Draw_Label( rect, content, style, USE_GO, color, ADDITIONAL_ENABLE ), adapter );
		}
		internal void Draw_LabelWithTextColor( Rect rect, string content, Color textColor, GUIStyle style, bool USE_GO, Color? color = null, bool? ADDITIONAL_ENABLE = null )
		{	//if ( !adapter.DRAW_LABEL_FOR_EMPTY_CONTENT && content == "-" ) return;
			if ( PERFOMANCE_BARS && perfadditionalcondition ) CURRENT_STACK.Draw_LabelWithTextColor( rect, content, textColor, style, USE_GO, color, ADDITIONAL_ENABLE );
			else emptyStack.DrawSIngleItem( emptyStack.Draw_LabelWithTextColor( rect, content, textColor, style, USE_GO, color, ADDITIONAL_ENABLE ), adapter );
		}
		internal void Draw_LabelWithTextDynamicColor( Rect rect, string content, DynamicColor textColor, GUIStyle style, bool USE_GO, Color? color = null,
		        bool? ADDITIONAL_ENABLE = null, int SWITCHER = 0 )          //if ( !adapter.DRAW_LABEL_FOR_EMPTY_CONTENT && content == "-" ) return;
		{	if ( PERFOMANCE_BARS && perfadditionalcondition ) CURRENT_STACK.Draw_LabelWithTextColor( rect, content, textColor, style, USE_GO, color, ADDITIONAL_ENABLE ).ADD_SWITCHER( SWITCHER);
			else emptyStack.DrawSIngleItem( emptyStack.Draw_LabelWithTextColor( rect, content, textColor, style, USE_GO, color, ADDITIONAL_ENABLE ).ADD_SWITCHER( SWITCHER ), adapter );
		}
		
		internal void Draw_ModuleButton( Rect rect, GUIContent content, DrawStackMethodsWrapper method, bool hasContent, object args = null, bool useContentForButton = false,
		                                 GUIStyle style = null,
		                                 bool USE_GO = false)
		{	if ( PERFOMANCE_BARS && perfadditionalcondition ) CURRENT_STACK.Draw_ModuleButton( rect,  content,  method,  hasContent, args, useContentForButton, style, USE_GO );
			else emptyStack.DrawSIngleItem( emptyStack.Draw_ModuleButton( rect, content, method, hasContent, args, useContentForButton, style, USE_GO ), adapter);
		}
		internal void Draw_SimpleButton( Rect rect, GUIContent content, DrawStackMethodsWrapper method, object args = null, bool useContentForButton = false, GUIStyle style = null,
		                                 bool USE_GO = false )
		{	if ( PERFOMANCE_BARS && perfadditionalcondition ) CURRENT_STACK.Draw_SimpleButton( rect, content, method,  args, useContentForButton, style, USE_GO );
			else emptyStack.DrawSIngleItem( emptyStack.Draw_SimpleButton( rect, content, method,  args, useContentForButton, style, USE_GO ), adapter );
		}
		
		internal virtual void ResetStack()
		{	foreach ( var item in DRAW_STACK )
			{	item.Value.ResetStack();
			}
			
#if EMX_HIERARCHY_DEBUG_STACKS
			Debug.Log( "RESET_MODULE_STACK " + GetType().Name );
#endif
			//  adapter.RepaintWindowInUpdate();
		}
		internal void ResetStack( int id, bool disableLog = false)
		{	if ( !DRAW_STACK.ContainsKey( id ) ) return;
		
			DRAW_STACK[id].ResetStack();
#if EMX_HIERARCHY_DEBUG_STACKS
			
			if (!disableLog)  Debug.Log( "RESET_OBJECT_STACK " + GetType().Name + " - " + EditorUtility.InstanceIDToObject(id)?.name);
			
#endif
			//  Debug.Log( "ASD" );
			// adapter.RepaintWindowInUpdate();
		}
		
		internal Dictionary<int, DrawStack > DRAW_STACK = new Dictionary<int, DrawStack >();
		internal bool TryToDraw(Rect worldOffset, Adapter.HierarchyObject o  )
		{	if ( !DRAW_STACK.ContainsKey( o.id ) || DRAW_STACK[o.id].currentStackPos == -1 ) return false;
		
			if ( DRAW_STACK[o.id] .GO_ENABLE_STATE != o.Active() )
			{	DRAW_STACK[o.id].GO_ENABLE_STATE = o.Active();
				DRAW_STACK[o.id].ResetStack();
				return false;
			}
			
			DRAW_STACK[o.id].Draw( worldOffset, o, adapter );
			
			return true;
		}
		internal DrawStack StackInstance( Adapter.HierarchyObject id )
		{	if ( DRAW_STACK.ContainsKey( id.id ) ) return DRAW_STACK[id.id];
		
			var res = new DrawStack() { GO_ENABLE_STATE = id.Active()};
			DRAW_STACK.Add( id.id, res  );
			return res;
		}
		internal void ClearDrawStack()
		{	if ( DRAW_STACK.Count == 0 ) return;
		
			DRAW_STACK.Clear();
		}
	}
	
	internal struct DrawStackMethodsWrapperData
	{	internal GUIContent content;
		internal object args;
	}
	internal class DrawStackMethodsWrapper {
		internal DrawStackMethodsWrapper( Action<Rect, Rect, DrawStackMethodsWrapperData, Adapter.HierarchyObject> action )
		{	this.action = action;
		}
		internal Action<Rect, Rect, DrawStackMethodsWrapperData, Adapter.HierarchyObject> action = null;
	}
	internal class SwithcerMethodsWrapper {
		internal SwithcerMethodsWrapper( Func<Adapter.HierarchyObject, int> action )
		{	this.action = action;
		}
		internal Func< Adapter.HierarchyObject, int> action = null;
	}
	
	internal abstract class Module : DrawStackAdapter, IEquatable<Module>, IComparable<Module>, IEqualityComparer<Module> {
		/* : IEquatable<Module>*/
		internal bool? SKIP_BAKED = null;
		int hash = -1;
		public override int GetHashCode()
		{	if ( hash == -1 ) hash = GetType().FullName.GetHashCode();
		
			return hash;
		}
		public bool Equals( Module other )
		{	return other.GetHashCode() == GetHashCode();
		}
		
		public bool Equals( Module x, Module y )
		{	return x.GetHashCode() == y.GetHashCode();
		}
		
		public int GetHashCode( Module obj )
		{	return obj.GetHashCode();
		}
		
		public int CompareTo( Module other )
		{	return other.GetHashCode() - GetHashCode();
		}
		
		
		internal override bool PERFOMANCE_BARS
		{	get
			{	return adapter.CACHING_TEXTURES_STACKS && !callFromExternal();
			}
		}
		
		/* internal void Label( Rect r, string s, TextAnchor an )
		  {   var a  = adapter. label.alignment;
		      adapter.label.alignment = an;
		      GUI.Label( r, s, adapter.label );
		      adapter.label.alignment = a;
		  }
		  internal void Label( Rect r, string s )
		  {   GUI.Label( r, s, adapter.label );
		  }
		  internal void Label( Rect r, GUIContent s )
		  {   GUI.Label( r, s, adapter.label );
		  }
		
		  internal bool Button( Rect r, string s )
		  {   return GUI.Button( r, s, adapter.button );
		  }
		  internal bool Button( Rect r, string s, TextAnchor an )
		  {   var a  = adapter.button.alignment;
		      adapter.button.alignment = an;
		      var res = GUI.Button( r, s, adapter.button );
		      adapter.button.alignment = a;
		      return res;
		  }
		  internal bool Button( Rect r, GUIContent s, TextAnchor an )
		  {   var a  = adapter.button.alignment;
		      adapter.button.alignment = an;
		      var res = GUI.Button( r, s, adapter. button );
		      adapter.button.alignment = a;
		      return res;
		
		  }
		  */
		/* internal bool Button( Rect r, GUIContent s )
		 {   return GUI.Button( r, s, adapter.button );
		 }*/
		
		
		
		internal virtual object SetCustomModule( object customModule ) { throw new NotImplementedException(); }
		internal  bool ENABLE = false;
		internal  bool wasAnyStart = false;
		
		internal virtual bool SKIP()
		//	{	return Adapter.OPT_EV_BR(Event.current);
		{	return false;
		}
		internal virtual int STATIC_WIDTH()
		{	return 0;
		}
		internal virtual void STATIC_MENU()
		{
		}
		
		// protected static Rect tR = new Rect();
		
		internal string batchType = null;
		internal _W__SearchWindow.FillterData_Inputs callFromExternal_objects = null;
		internal bool callFromExternal() { return callFromExternal_objects != null; }
		internal string ContextHelper = "-";
		internal bool disableSib = false;
		internal Func<bool> DRAW_AS_COLUMN = ( ) => true;
		internal string HeaderText = "-";
		internal string HeaderTexture2D = null;
		internal Color? headOverrideTexture = null;
		//internal int width = 0;
		
		virtual internal float GetInputWidth()
		{	return -1;
		}
		
		
		// int restWidth;
		internal string SearchHelper = "-";
		internal Type typeFillter = null;
		//    internal Adapter adapter = null;
		internal  int identifire;
		static int incrimenter;
		internal Module( int restWidth, int sibbildPos, bool _enable, Adapter adapter )
		{	this.adapter = adapter;
			this.identifire = incrimenter++;
			
			if ( batchType == null ) batchType = GetType().FullName;
			
			var h = Hierarchy_GUI.Instance( adapter );
			
			if ( h == null ) Debug.LogError( "Hierarchy plugin critical initialization exception A" );
			
			h.Bake( adapter );
			
			if ( !h.modulesParamsContainsKey( ref batchType ) || h.modulesParamsGetValList( ref batchType ) == null )
			{	h.modulesParams.RemoveAll( batchType );
				h.modulesParams.Add( batchType, new MyStruct() { enable = _enable, sib = sibbildPos, width = restWidth } );
				h.Bake( adapter );
			}
			
			else
			{
			
			}
			
			if ( sibbildPos == -1 )     //      Debug.Log( GetType() );
			{	disableSib = true;
			}
			
			//if (batchType.Contains("M_Vertices")) Debug.Log(width);
			
			/* var value = h.modulesParamsGetValList(batchType);
			 if (!disableSib && value.sib == -1)
			 {
			     var wasSibs = modules.Select(m => m.sibbildPos);
			     var targetSib = 0;
			     while (wasSibs.Contains(targetSib)) targetSib++;
			     value.sib = targetSib;
			 }*/
			/*  v.sib = -1;
			  h.modulesParamsGetValList(batchType, v);*/
			
			InitializeModule();
			
			if ( p == null ) Debug.LogError( "Hierarchy plugin critical initialization exception B" );
			
			OnEnableChange( p.enable );
		}
		
		private MyStruct p
		{	get { return Hierarchy_GUI.Instance( adapter ).modulesParamsGetValList( ref batchType ); }
		
			/*  set {
			
			      if (!disableSib && value.sib == -1)
			      {
			          var wasSibs = modules.Select(m => m.sibbildPos);
			          var targetSib = 0;
			          while (wasSibs.Contains(targetSib)) targetSib++;
			          value.sib = targetSib;
			      }
			
			      Hierarchy_GUI.Initialize().modulesParamsGetValList(batchType, value);
			      OnEnableChange(enable);
			  }*/
		}
		
		
		// SIBLING
		internal int? cache_sibbildPos;
		internal void write_sibbildPos()
		{	if ( disableSib ) cache_sibbildPos = -1;
		
			if ( !disableSib && p.sib == -1 )
			{	var wasSibs = adapter.modules.Select( m => m.p.sib );
				var targetSib = 0;
				
				while ( wasSibs.Contains( targetSib ) ) targetSib++;
				
				p.sib = targetSib;
			}
			
			cache_sibbildPos = p.sib;
		}
		internal int sibbildPos
		{	get
			{	if ( !cache_sibbildPos.HasValue ) write_sibbildPos();
			
				return cache_sibbildPos.Value;
			}
			
			set
			{	if (value != p.sib) adapter.RESET_DRAW_STACKS();
			
				cache_sibbildPos = p.sib = value;
			}
		}
		// SIBLING
		
		
		internal virtual bool enableOverride() { return true; }
		internal virtual string enableOverrideMessage() { return null; }
		
		
		
		// ENABLE
		internal bool? cache_enable;
		internal void write_enable()
		{	cache_enable = p.enable && enableOverride();
		}
		internal virtual bool enable
		{	get
			{	if ( !cache_enable.HasValue ) write_enable();
			
				return cache_enable.Value && ENABLE;
			}
			
			set
			{	if ( !enableOverride() ) return;
			
				cache_enable = p.enable = value;
				OnEnableChange( value );
			}
		}
		// ENABLE
		
		// WIDTH
		internal int? cache_width;
		internal void write_width()
		{	cache_width = disableSib ? STATIC_WIDTH() : p.width;
		}
		internal int width
		{	get
			{	if ( !cache_width.HasValue ) write_width();
			
				return cache_width.Value;
			}
			
			set
			{	if (value != p.width) adapter.RESET_DRAW_STACKS();
			
				cache_width = p.width = value;
			}
		}
		// WIDTH
		
		protected Color B_PASSIVE
		{	get { return Adapter.B_PASSIVE; }
		}
		
		protected Color B_ACTIVE
		{	get { return Adapter.B_ACTIVE; }
		}
		
		internal void CreateUndo()
		{	Hierarchy_GUI.Instance( adapter );
			Hierarchy_GUI.Undo( adapter, "Change Hierarchy" );
		}
		
		internal void SetDirty()
		{	Hierarchy_GUI.SetDirtyObject( adapter );
			adapter.RESET_DRAW_STACKS();
			/* Hierarchy_GUI.Initialize();
			 if (!Hierarchy_GUI.Initialize().modulesParams.ContainsKey(GetType())) return;
			 try
			 {
			     var o = (MyStruct)DESERIALIZE_SINGLE(Hierarchy_GUI.Initialize().modulesParams[GetType()]);
			     p = o;
			 }
			 catch
			 {
			 }*/
		}
		
		protected virtual void OnEnableChange( bool value )
		{	ENABLE = value;
		}
		
		// bool wasInit = false;
		internal virtual void InitializeModule()
		{	/*  if (wasInit) return;
			      wasInit = true;
			      var h = Hierarchy_GUI.Initialize();*/
		}
		
		/*  internal void Reset()
		  {
		      width = restWidth;
		  }*/
		internal abstract float Draw( Rect drawRect, HierarchyObject o );
		internal abstract _W__SearchWindow.FillterData_Inputs CallHeader();
		
		
		/*  public bool Equals(Module other)
		{
		return identifire == other.identifire;
		}
		
		bool IEquatable<Module>.Equals(Module other)
		{
		return Equals( (Module)other );
		}
		
		public override bool Equals(object obj)
		{
		return Equals( (Module)obj );
		}
		
		public override int GetHashCode()
		{
		return identifire;
		}*/
		//  abstract internal void Title(Rect drawRect);
	}
}
}
