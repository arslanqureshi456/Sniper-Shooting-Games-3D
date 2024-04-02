
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngineInternal;




namespace EModules.EModulesInternal {
internal partial class Hierarchy {

    internal partial class M_CustomIcons : Adapter.Module {
        // internal  const int BUT_WIDTH = 48;
        internal  const int BUT_WIDTH = 80;
        // internal const int FIELD_WIDTH = 32;
        internal const int FIELD_WIDTH = 80;
        
        internal class MyAttribute {
            internal MyAttribute( string name, float? width, Color? color )
            {   this.name = name;
                this.width = width;
                this.color = color;
            }
            
            internal bool IsStatic = false;
            internal string name;
            internal float? width;
            internal Color? color = null;
        }
        internal class AttributeButton : MyAttribute {
            internal AttributeButton( string name, float ? width, Color ? color ) : base( name, width, color ) { }
            internal object[] parameters;
            
            internal MethodInfo method;
        }
        
        internal class AttributeField : MyAttribute {
            internal AttributeField( string name, float ? width, Color ? color ) : base( name, width, color ) { }
            
            internal bool IsProperty = false;
            internal FieldInfo field;
            internal PropertyInfo property;
        }
        AttrArgs attrArgs;
        internal struct AttrArgs
        {   internal Component script;
            internal AttributeButton el;
            internal AttributeField field;
            internal bool isNull;
            //  internal string  content;
        }
        GUIContent emptyContent = new GUIContent();
        Dictionary<Type, List<AttributeButton>> buttons = new Dictionary<Type, List<AttributeButton>>();
        Dictionary<Type, List<AttributeField>> fields = new Dictionary<Type, List<AttributeField>>();
        GUIStyle buttonArrtStyle, labelAttrStyle, textAttrStyle, textAttrStyleRedColor, popStyle;
        bool DrawAttributes( Adapter.HierarchyObject _o, Component script )
        {   if ( !adapter.par.COMP_ATTRIBUTES_BUTTONS && !adapter.par.COMP_ATTRIBUTES_FIELDS || Adapter.LITE ) return false;
        
            var t = Adapter.GetType_(script);
            var key = t.Name;
            var wasDraw = false;
            ///// INITIALIZE
            
            if ( adapter.par.COMP_ATTRIBUTES_BUTTONS )  /** par.COMP_ATTRIBUTES_BUTTONS */
            {   if ( !buttons.ContainsKey( t ) )
                {   buttons.Add( t, new List<AttributeButton>() );
                
                    foreach ( var methodInfo in t.GetMethods( (BindingFlags)(-1) ) )
                    {   if ( methodInfo.IsGenericMethod ) continue;
                        var result = methodInfo.GetCustomAttributes(typeof(SHOW_IN_HIER), false);
                        if ( result != null )
                        {   foreach ( var o1 in result )
                            {   var o = (SHOW_IN_HIER)o1;
                                var value = new AttributeButton(methodInfo.Name, o.width, o.color) { IsStatic = methodInfo.IsStatic, method = methodInfo };
                                value.parameters = methodInfo.GetParameters().Select( p =>
                                {   if ( !p.ParameterType.IsClass )
                                    {   return Activator.CreateInstance( p.ParameterType );
                                    }
                                    return null;
                                }
                                                                                    ).ToArray();
                                /*  foreach (var p in value.parameters) {
                                      MonoBehaviour.print(p.GetType());
                                  }*/
                                buttons[t].Add( value );
                            }
                        }
                    }
                }
            }/** par.COMP_ATTRIBUTES_BUTTONS */
            
            
            
            
            if ( adapter.par.COMP_ATTRIBUTES_FIELDS ) /** par.COMP_ATTRIBUTES_FIELDS */
            {   if ( !fields.ContainsKey( t ) )
                {   fields.Add( t, new List<AttributeField>() );
                
                    foreach ( var propertyInfo in t.GetProperties( (BindingFlags)(-1) ) )
                    {   if ( !propertyInfo.CanRead ) continue;
                        var result = propertyInfo.GetCustomAttributes(typeof(SHOW_IN_HIER), false);
                        if ( result != null )
                        {   foreach ( var o1 in result )
                            {   var o = (SHOW_IN_HIER)o1;
                                var value = new AttributeField(propertyInfo.Name, o.width, o.color) { IsStatic = propertyInfo.GetAccessors(true)[0].IsStatic, IsProperty = true, property = propertyInfo };
                                fields[t].Add( value );
                            }
                        }
                    }
                    
                    foreach ( var fieldInfo in t.GetFields( (BindingFlags)(-1) ) )
                    {   var result = fieldInfo.GetCustomAttributes(typeof(SHOW_IN_HIER), false);
                        if ( result != null )
                        {   foreach ( var o1 in result )
                            {   var o = (SHOW_IN_HIER)o1;
                                var value = new AttributeField(fieldInfo.Name, o.width, o.color) { IsStatic = fieldInfo.IsStatic, IsProperty = false, field = fieldInfo };
                                fields[t].Add( value );
                            }
                        }
                    }
                }
            }/** par.COMP_ATTRIBUTES_FIELDS */
            
            ///// INITIALIZE
            var width = 0f;
            
            //att_Rect = drawRect;
            var newH = Math.Max(EditorGUIUtility.singleLineHeight, HEIGHT - 8);
            att_Rect.y = Y + (HEIGHT - newH) / 2;
            att_Rect.height = newH;
            
            /* att_Rect.y = Y;
             att_Rect.height = HEIGHT;*/
            //  att_Rect.y -= 2;
            
            if ( buttonArrtStyle == null )
            {   buttonArrtStyle = new GUIStyle(adapter.button);
                buttonArrtStyle.alignment = TextAnchor.MiddleCenter;
                buttonArrtStyle.normal.textColor = EditorGUIUtility.isProSkin ? TCCC : TCCC2;
            }
            buttonArrtStyle.fontSize = adapter.FONT_8(); //Mathf.RoundToInt( adapter.button.fontSize * 0.8f );
            if ( labelAttrStyle == null )
            {   labelAttrStyle = new GUIStyle( adapter.label );
                labelAttrStyle.alignment = TextAnchor.MiddleCenter;
            }
            labelAttrStyle.fontSize = buttonArrtStyle.fontSize;
            if ( textAttrStyle == null )
            {   textAttrStyle = new GUIStyle( Adapter.GET_SKIN().textField );
                textAttrStyle.alignment = TextAnchor.MiddleCenter;
            }
            textAttrStyle.fontSize = buttonArrtStyle.fontSize;
            if ( textAttrStyleRedColor == null )
            {   textAttrStyleRedColor = new GUIStyle( textAttrStyle );
                textAttrStyleRedColor.normal.textColor = Color.red;
            }
            textAttrStyleRedColor.fontSize = buttonArrtStyle.fontSize;
            if ( popStyle == null )
            {   popStyle = new GUIStyle( EditorStyles.popup );
                popStyle.alignment = TextAnchor.MiddleCenter;
            }
            popStyle.fontSize = buttonArrtStyle.fontSize;
            
            
            
            
            if ( adapter.par.COMP_ATTRIBUTES_BUTTONS )
            {   var list = buttons[t];
                var col = Color.white;
                for ( int i = 0 ; i < list.Count ; i++ )
                {   wasDraw = true;
                
                    //   att_Rect.width = list[i].width;
                    att_content.text = att_content.tooltip = list[i].name;
                    att_Rect.width = Math.Min( BUT_WIDTH, buttonArrtStyle.CalcSize( att_content ).x ) ;
                    
                    //**//  GUI.color = c * (list[i].color ?? Color.white);
                    if ( list[i].color.HasValue ) col *= list[i].color.Value;
                    
                    
                    if ( !DRAW_NEXTTONAME ) width -= att_Rect.width;
                    att_Rect.x = drawRect.x + width;
                    //  if (Event.current.type == EventType.repaint) Adapter.GET_SKIN().box.Draw(att_Rect, true, true, true, true);
                    
                    var style = EditorStyles.miniButton;
                    /*  var tt = style.border.top;
                      var bb = style.border.bottom;
                      if (!EditorGUIUtility.isProSkin)
                      {
                          style.border.top = 5;
                          style.border.bottom = 5;
                      }*/
                    
                    
                    
                    //**//  if ( Event.current.type == EventType.Repaint ) style.Draw( att_Rect, false, false, false, false );
                    
                    
                    Draw_Style( att_Rect, style, emptyContent, col, true );
                    
                    
                    /* style.border.top = tt;
                     style.border.bottom = bb;*/
                    
                    //  var tc = buttonArrtStyle.normal.textColor;
                    //  attrArgs.script = !list[i].IsStatic ? script : null;
                    attrArgs.script = script;
                    attrArgs.el = list[i];
                    //  attrArgs.content = att_content.text;
                    Draw_ModuleButton( att_Rect, att_content, ATTR_BUT_HASH, true, attrArgs,  true, buttonArrtStyle, USE_GO: true );
                    //**//
                    /* if ( adapter.ModuleButton( att_Rect, att_content, true, buttonArrtStyle ) )
                     {   // buttonArrtStyle.normal.textColor = tc;
                         var el = list[i];
                         adapter.PushActionToUpdate( () =>
                         {   if ( el.IsStatic ) el.method.Invoke( null, el.parameters );
                             else if ( script ) el.method.Invoke( script, el.parameters );
                             adapter.RepaintWindow();
                         } );
                     }*/
                    //**//
                    //   buttonArrtStyle.normal.textColor = tc;
                    if ( DRAW_NEXTTONAME ) width += att_Rect.width;
                }
                //**//GUI.color = c;
            }
            if ( adapter.par.COMP_ATTRIBUTES_FIELDS )
            {   var list = fields[t];
                //***// var c = GUI.color;
                var col = Color.white;
                for ( int i = 0 ; i < list.Count ; i++ )
                {   wasDraw = true;
                    var isnull = false;
                    var hasSetter = false;
                    if ( list[i].IsProperty )
                    {   object result = null;
                        try
                        {   result = list[i].property.GetValue( list[i].IsStatic ? null : script, null );
                        }
                        catch
                        {
                        }
                        att_content.text = (isnull = Adapter.IsObjectNull( /*list[i].property.PropertyType ,*/ result )) ? "null" : result.ToString();
                        hasSetter = list[i].property.GetSetMethod() != null;
                    }
                    else
                    {   object result = list[i].field.GetValue( list[i].IsStatic ? null : script );
                        try
                        {   //result =
                        }
                        catch
                        {
                        }
                        att_content.text = (isnull = Adapter.IsObjectNull( /*list[i].field.FieldType ,*/ result )) ? "null" : result.ToString();
                        /* Debug.Log( result == null );
                         Debug.Log( Adapter.IsObjectNull( list[i].field.FieldType , result ) );
                         Debug.Log( result as UnityEngine.Object );*/
                        hasSetter = true;
                    }
                    
                    
                    // att_Rect.width = list[i].width;
                    var captureList = list[i];
                    var type = captureList.IsProperty ? captureList.property.PropertyType : captureList.field.FieldType;
                    att_content.tooltip = list[i].name + ": " + att_content.text;
                    if ( type.IsEnum )
                        att_Rect.width = Math.Min( FIELD_WIDTH, popStyle.CalcSize( att_content ).x + popStyle.padding.left);
                    else
                        att_Rect.width = Math.Min( FIELD_WIDTH, textAttrStyle.CalcSize( att_content ).x  );
                        
                    //  GUI.color = /*c **/ (list[i].color ?? Color.white);
                    if ( list[i].color.HasValue ) col *= list[i].color.Value;
                    
                    
                    if ( !DRAW_NEXTTONAME ) width -= att_Rect.width;
                    att_Rect.x = drawRect.x + width;
                    
                    
                    // if (Event.current.type == EventType.Repaint) Adapter.GET_SKIN().box.Draw(att_Rect, true, true, true, true);
                    // if (!(list[i].IsProperty || list[i].field.FieldType != typeof(int) && list[i].field.FieldType != typeof(float) && list[i].field.FieldType != typeof(string)))
                    if ( hasSetter )     //var newText = EditorGUI.TextField(att_Rect, att_content.text );
                    {   //if (newText != att_content.text)
                    
                    
                        attrArgs.script = script;
                        attrArgs.field = captureList;
                        attrArgs.isNull = isnull;
                        //  attrArgs.content = att_content.text;
                        
                        
                        Draw_Action( att_Rect, SETTER_REPAINT_ACTION_HASH, attrArgs, content: att_content);
                        
                        Draw_ModuleButton( att_Rect, att_content, SETTER_BUT_HASH, true, attrArgs, false, buttonArrtStyle, USE_GO: true);
                        
                        
                        //if (type.IsEnum)
                        {   att_content.text = null;
                            // GUI.Label( att_Rect, att_content, labelAttrStyle);
                            Draw_Label( att_Rect, att_content, labelAttrStyle, true, col);
                        }
                        
                    }
                    else
                    {   /*var ccc = labelAttrStyle.normal.textColor;
                        if ( adapter.par.DISPLAYING_NULLSISRED && isnull ) labelAttrStyle.normal.textColor = Color.red;
                        GUI.Label( att_Rect, att_content, labelAttrStyle);
                        labelAttrStyle.normal.textColor = ccc;*/
                        
                        var ccc = textAttrStyle;
                        if ( adapter.par.DISPLAYING_NULLSISRED && isnull ) ccc = textAttrStyleRedColor;
                        Draw_Label( att_Rect, att_content, ccc, true, col );
                    }
                    if ( DRAW_NEXTTONAME ) width += att_Rect.width;
                }
                //**// GUI.color = c;
            }
            
            
            
            drawRect.x += width;
            return wasDraw;
            // return width;
        }
        
        Dictionary<Type, List<string>> enumToNames = new Dictionary<Type, List<string>>();
        
        static Rect att_Rect = new Rect();
        static GUIContent att_content = new GUIContent();
        static Color TCCC = new Color(0.85f, 0.94f, 0.94f, 1);
        static Color TCCC2 = new Color(1 - 0.85f, 1 - 0.94f, 1 - 0.94f, 1);
        
        
        Adapter. DrawStackMethodsWrapper __ATTR_BUT_HASH = null;
        Adapter.DrawStackMethodsWrapper ATTR_BUT_HASH { get { return __ATTR_BUT_HASH ?? (__ATTR_BUT_HASH = new Adapter.DrawStackMethodsWrapper( ATTR_BUT )); } }
        void ATTR_BUT( Rect worldOffset, Rect inputRect, Adapter.DrawStackMethodsWrapperData data, Adapter.HierarchyObject _o )
        {   var args = (AttrArgs)data.args;
            var el = args.el;
            adapter.PushActionToUpdate( () =>
            {   if ( el.IsStatic ) el.method.Invoke( null, el.parameters );
                else if ( args.script ) el.method.Invoke( args.script, el.parameters );
                adapter.RepaintWindow();
            } );
        }
        
        Adapter. DrawStackMethodsWrapper __SETTER_BUT_HASH = null;
        Adapter.DrawStackMethodsWrapper SETTER_BUT_HASH { get { return __SETTER_BUT_HASH ?? (__SETTER_BUT_HASH = new Adapter.DrawStackMethodsWrapper( SETTER_BUT )); } }
        void SETTER_BUT( Rect worldOffset, Rect inputRect, Adapter.DrawStackMethodsWrapperData data, Adapter.HierarchyObject _o )
        {   var args = (AttrArgs)data.args;
            // var att_content = args.content;
#pragma warning disable
            var att_content = data.content;
#pragma warning restore
            var captureList = args.field;
            var script = captureList.IsStatic ? null : args.script ;
            var type = captureList.IsProperty ? captureList.property.PropertyType : captureList.field.FieldType;
            var allowUndo = script && !captureList.IsStatic && captureList.IsStatic && !Application.isPlaying && (!captureList.IsProperty || type.IsEnum);
            //   if ( Button( att_Rect, "" ) )
            {
            
                /* var captureList = list[i];
                  var type = captureList.IsProperty ? captureList.property.PropertyType : captureList.field.FieldType;*/
                
                
                if ( type.IsEnum )
                {
                
                    if ( !enumToNames.ContainsKey( type ) )
                    {   enumToNames.Add( type, Enum.GetNames( type ).ToList() );
                    }
                    var ind = enumToNames[type].IndexOf( att_content.text);
                    ind = Mathf.Clamp( ind, 0, enumToNames[type].Count - 1 );
                    
                    CustomModule.SHOW_DropDownMenu( ind, Enum.GetNames( type ), ( newValue ) =>
                    {   if ( !captureList.IsStatic && !script ) return;
                        var nv = Enum.Parse(type, Enum.GetNames(type)[newValue]);
                        if ( newValue != ind )
                        {   if ( allowUndo )   Undo.RecordObject( script, "Change Field" );
                            if ( captureList.IsProperty )
                                captureList.property.SetValue( captureList.IsStatic ? null : script, nv, null );
                            else
                                captureList.field.SetValue( captureList.IsStatic ? null : script, nv );
                            if ( script && !Application.isPlaying ) EditorUtility.SetDirty( script );
                            if ( allowUndo ) adapter.MarkSceneDirty( script.gameObject.scene );
                            ResetStack();
                        }
                        
                    } );
                    
                }
                else if ( type == typeof( int ) )
                {
                
                    CustomModule.SHOW_StringInput( att_content.text, ( newValue ) =>
                    {   int result;
                        if ( !captureList.IsStatic && !script ) return;
                        if ( int.TryParse( newValue, out result ) )
                        {   if ( allowUndo )   Undo.RecordObject( script, "Change Field" );
                            if ( captureList.IsProperty )
                                captureList.property.SetValue( captureList.IsStatic ? null : script, result, null );
                            else
                                captureList.field.SetValue( captureList.IsStatic ? null : script, result );
                            if ( script && !Application.isPlaying ) EditorUtility.SetDirty( script );
                            if ( allowUndo ) adapter.MarkSceneDirty( script.gameObject.scene );
                            ResetStack();
                        }
                    } );
                    
                }
                else if ( type == typeof( float ) )           //float result;
                {   // if (float.TryParse(newText, out result)) captureList.field.SetValue(captureList.IsStatic ? null : script, result);
                    CustomModule.SHOW_StringInput( att_content.text, ( newValue ) =>
                    {   float result;
                        if ( !captureList.IsStatic && !script ) return;
                        if ( float.TryParse( newValue, out result ) )
                        {   if ( allowUndo )   Undo.RecordObject( script, "Change Field" );
                            if ( captureList.IsProperty )
                                captureList.property.SetValue( captureList.IsStatic ? null : script, result, null );
                            else
                                captureList.field.SetValue( captureList.IsStatic ? null : script, result );
                            if ( script && !Application.isPlaying ) EditorUtility.SetDirty( script );
                            if ( allowUndo ) adapter.MarkSceneDirty( script.gameObject.scene );
                            ResetStack();
                        }
                        
                    } );
                }
                else if ( type == typeof( string ) )
                {   CustomModule.SHOW_StringInput( att_content.text, ( newValue ) =>
                    {   if ( !captureList.IsStatic && !script ) return;
                    
                        if ( allowUndo )   Undo.RecordObject( script, "Change Field" );
                        if ( captureList.IsProperty )
                            captureList.property.SetValue( captureList.IsStatic ? null : script, newValue, null );
                        else
                            captureList.field.SetValue( captureList.IsStatic ? null : script, newValue );
                        if ( script && !Application.isPlaying ) EditorUtility.SetDirty( script );
                        if ( allowUndo ) adapter.MarkSceneDirty( script.gameObject.scene );
                        ResetStack();
                    } );
                }
                
                
                //Undo.RecordObject( _o.GetHardLoadObject() , "Change Field" );
                
            }
            
        }
        
        
        
        
        Adapter. DrawStackMethodsWrapper __SETTER_REPAINT_ACTION_HASH = null;
        Adapter.DrawStackMethodsWrapper SETTER_REPAINT_ACTION_HASH { get { return __SETTER_REPAINT_ACTION_HASH ?? (__SETTER_REPAINT_ACTION_HASH = new Adapter.DrawStackMethodsWrapper( SETTER_REPAINT_ACTION )); } }
        void SETTER_REPAINT_ACTION( Rect worldOffset, Rect att_Rect, Adapter.DrawStackMethodsWrapperData data, Adapter.HierarchyObject _o )
        {   if (Event.current.type == EventType.Repaint )
            {
            
            
                var args = (AttrArgs)data.args;
                var att_content = data.content;
                var captureList = args.field;
                var isnull = (bool)args.isNull;
                
                var c = GUI.color;
                if (!_o.Active()) GUI.color *= new Color( 1, 1, 1, 0.5f );
                var type = captureList.IsProperty ? captureList.property.PropertyType : captureList.field.FieldType;
                if (type.IsEnum )
                {   popStyle.Draw(att_Rect, att_content, false, false, false, false );
                }
                else
                {   var ccc = textAttrStyle;
                    if (adapter.par.DISPLAYING_NULLSISRED && isnull ) ccc = textAttrStyleRedColor;
                    ccc.Draw(att_Rect, att_content, false, false, false, false );
                }
                GUI.color = c;
            }
            EditorGUIUtility.AddCursorRect(att_Rect, MouseCursor.Link);
        }
        // static GUIContent empty_content = new GUIContent();
    }
}
}
