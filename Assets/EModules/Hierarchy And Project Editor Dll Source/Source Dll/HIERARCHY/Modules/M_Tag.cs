using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
//namespace EModules

namespace EModules.EModulesInternal {
internal partial class Hierarchy {
    static TextAnchor __Align = TextAnchor.MiddleRight;
    static   GUIStyle __ ;
    
    class M_Tag : Adapter.Module {
        public M_Tag( int restWidth, int sibbildPos, bool enable, Adapter adapter ) : base( restWidth, sibbildPos, enable, adapter )
        {
        }
        
        // static SerializedProperty _findProperty;
        private string[] tags
        {   get
            {   return UnityEditorInternal.InternalEditorUtility.tags;
            }
        }
        static Color alpha = new Color(1, 1, 1, 0.3f);
        
        static GUIContent _content = new GUIContent();
        internal override float Draw( Rect drawRect, Adapter.HierarchyObject _o )
        {   if ( !START_DRAW( drawRect, _o ) ) return 0;
            var o = _o.go;
            
            _content.tooltip = o.tag;
            // content.tooltip = base.ContextHelper;
            _content.text = GET_STRING( _content.tooltip, callFromExternal() ? 0 : adapter.par.UPPER_TAGS );
            if ( _content.text == "" ) _content.text = "...Missing";
            // content.tooltip = content.text;
            var hasContent = false;
            if ( !string.IsNullOrEmpty( o.tag ) && o.tag != "Untagged" )
            {   /*  var fs = Adapter.GET_SKIN().label.fontSize;
                var al = Adapter.GET_SKIN().label.alignment;
                Adapter.GET_SKIN().label.alignment = TextAnchor.MiddleLeft;
                if (!callFromExternal()) Adapter.GET_SKIN().label.fontSize = adapter.FONT_8();
                else Adapter.GET_SKIN().label.fontSize = adapter.WINDOW_FONT_8();*/
                // Adapter.GET_SKIN().label.fontSize = Hierarchy.FONT_8();
                hasContent = true;
                /* GUI.enabled = o.activeInHierarchy;
                 GUI.Label( drawRect, content, !callFromExternal() ? adapter.STYLE_LABEL_8 : adapter.STYLE_LABEL_8_WINDOWS );
                 GUI.enabled = true;*/
                Draw_Label( drawRect, _content, !callFromExternal() ? adapter.STYLE_LABEL_8 : adapter.STYLE_LABEL_8_WINDOWS, true);
                
                /*  Adapter.GET_SKIN().label.alignment = al;
                  Adapter.GET_SKIN().label.fontSize = fs;*/
            }
            else
            {   if ( __ == null )
                {   __ = new GUIStyle( adapter.label );
                    __.alignment = __Align;
                }
                /*  var c = GUI.color;
                  GUI.color *= alpha;
                  var a = adapter.label.alignment;
                  adapter.label.alignment = __;
                  GUI.Label( drawRect, "-", adapter.label );
                  adapter.label.alignment = a;
                  GUI.color = c;*/
                Draw_Label( drawRect, "-", adapter.label, true, alpha );
                
            }
            
            //  var bg = Adapter.GET_SKIN().button.active.background;
            // Adapter.GET_SKIN().button.active.background = Hierarchy.GetIcon("BUT");
            
            // if (drawRect.Contains(Event.current.mousePosition) && Event.current.type != EventType.repaint) MonoBehaviour.print(Event.current.type);
            
            
            Draw_ModuleButton( drawRect, _content, BUTTON_ACTION_HASH, hasContent );
            
            /* if ( adapter.ModuleButton( drawRect, null, hasContent ) )
             {
            
            
             }*/
            
            // Adapter.GET_SKIN().button.active.background = bg;
            
            
            
            END_DRAW(_o);
            return width;
        }
        
        
        
        Adapter. DrawStackMethodsWrapper __BUTTON_ACTION_HASH = null;
        Adapter.DrawStackMethodsWrapper BUTTON_ACTION_HASH   {   get  {    return __BUTTON_ACTION_HASH ?? (__BUTTON_ACTION_HASH = new Adapter.DrawStackMethodsWrapper( BUTTON_ACTION )); }  }
        void BUTTON_ACTION( Rect worldOffset, Rect inputRect, Adapter.DrawStackMethodsWrapperData data, Adapter.HierarchyObject _o)
        {   var o = _o.go;
        
#pragma warning disable
            var content = data.content;
#pragma warning restore
            if ( Event.current.button == 0 )
            {
            
            
            
                var l = tags;
                
                // var select = -1;
                // var ordered = l.OrderBy(f => f.Key).Select(f => f.Value).ToArray();
                var oldSelect = data. content.text;
                Action<int> Callback = (res) =>
                {   SetTAg(o, l[res]);
                    /*  Undo.RecordObject(o, "Change tag");
                      o.tag = l[res];
                      Hierarchy.SetDirty(o);
                      if (Hierarchy.par.ENABLE_PING_Fix) adapter.TRY_PING_OBJECT(o);*/
                };
                
                
                GenericMenu menu = new GenericMenu();
                
                for ( int i = 0 ; i < l.Length ; i++ )
                {   var ind = i;
                    var c =  new GUIContent(data. content );
                    c.text = l[i];
                    menu.AddItem( c, GET_STRING( data.content.text, adapter.par.UPPER_TAGS ) == oldSelect, () => Callback( ind ) );
                }
                menu.AddSeparator( "" );
                
                
                
                /*  menu.AddItem(new GUIContent("Show 'Tags And Layers' Settings"), false, () => {
                      Selection.objects = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset");
                  });
                  menu.AddSeparator("");*/
                
                
                var pos = new MousePos( Event.current.mousePosition, MousePos.Type.Input_128_68, !callFromExternal(), adapter);
                //  var pos = InputData.WidnwoRect(!callFromExternal(), Event.current.mousePosition, 128, 68, adapter );
                menu.AddItem( new GUIContent( "+ Assign a New tag" ), false, () =>
                {
                
                
                    _W__InputWindow.Init( pos, "New tag name's", adapter, ( str ) =>
                    {   if ( string.IsNullOrEmpty( str ) ) return;
                        str = str.Trim();
                        var lowwer = l.Select(ord => ord.ToLower()).ToList();
                        var ind = lowwer.IndexOf(str.ToLower());
                        if ( ind != -1 )
                        {   SetTAg( o, l[ind] );
                            /* Undo.RecordObject(o, "Change tag");
                             o.tag = l[ind];
                             Hierarchy.SetDirty(o);
                             if (Hierarchy.par.ENABLE_PING_Fix) adapter.TRY_PING_OBJECT(o);*/
                        }
                        else
                        {   UnityEditorInternal.InternalEditorUtility.AddTag( str );
                        
                            SetTAg( o, str );
                            /*  Undo.RecordObject(o, "Change tag");
                              o.tag = str;
                              Hierarchy.SetDirty(o);
                              if (Hierarchy.par.ENABLE_PING_Fix) adapter.TRY_PING_OBJECT(o);*/
                        }
                    } );
                } );
                
                menu.AddSeparator( "" );
                menu.AddItem( new GUIContent( "Show only uppercase letters" ), adapter.par.UPPER_TAGS != 0, () =>
                {   adapter.par.UPPER_TAGS = 1 - adapter.par.UPPER_TAGS;
                    adapter.SavePrefs();
                } );
                menu.AddSeparator( "" );
                menu.AddItem( new GUIContent( "Show 'Tags And Layers' Settings" ), false, () =>
                {   Selection.objects = AssetDatabase.LoadAllAssetsAtPath( "ProjectSettings/TagManager.asset" );
                    Adapter.FocusToInspector();
                } );
                
                menu.ShowAsContext();
                Adapter.EventUse();
                
            }
            
            
            if ( Event.current.button == 1 )
            {   Adapter.EventUse();
                /*  int[] contentCost = new int[0];
                  GameObject[] obs = new GameObject[0];
                
                  if (Validate(o))
                  {
                      if (EditorSceneManager.GetActiveScene().rootCount != 0) CallHeaderFiltered(out obs, out contentCost, o.tag);
                  } else
                  {
                      CallHeader(out obs, out contentCost);
                  }
                
                  FillterData.Init(Event.current.mousePosition, SearchHelper, o.tag, obs, contentCost, null, this);
                */
                var mp = new MousePos( Event.current.mousePosition, MousePos.Type.Search_356_0, !callFromExternal(), adapter);
                
                _W__SearchWindow.Init( mp, SearchHelper, Validate( o ) ? o.tag : "All Tags",
                                       Validate( o ) ?
                                       CallHeaderFiltered( o.tag ) :
                                       CallHeader(),
                                       this, adapter, _o );
                // EditorGUIUtility.ic
            }
        }
        
        
        
        
        void SetTAg( GameObject o, string tag )
        {   if ( adapter.SELECTED_GAMEOBJECTS().All( selO => selO.go != o ) )
            {   Undo.RecordObject( o, "Change tag" );
                o.tag = tag;
                Adapter.SetDirty( o );
                ResetStack( o.GetInstanceID() );
                adapter.MarkSceneDirty( o.scene );
                if ( Hierarchy.HierarchyAdapterInstance.par.ENABLE_PING_Fix ) adapter.TRY_PING_OBJECT( o );
                
            }
            else
            {   foreach ( var objectToUndo in adapter.SELECTED_GAMEOBJECTS() )
                {   Undo.RecordObject( objectToUndo.go, "Change tag" );
                    objectToUndo.go.tag = tag;
                    Adapter.SetDirty( objectToUndo.go );
                    ResetStack( objectToUndo.go.GetInstanceID() );
                    adapter.MarkSceneDirty( objectToUndo.scene );
                    //  if (Hierarchy.par.ENABLE_PING_Fix) adapter.TRY_PING_OBJECT(objectToUndo);
                    
                }
            }
        }
        
        
        
        bool Validate( GameObject o )
        {   return !string.IsNullOrEmpty( o.tag ) && o.tag != "Untagged";
        }
        
        
        
        /* FillterData.Init(Event.current.mousePosition, SearchHelper, LayerMask.LayerToName(o.layer),
                     Validate(o) ?
                     CallHeaderFiltered(LayerMask.LayerToName(o.layer)) :
                     CallHeader(),
                     this);*/
        /** CALL HEADER */
        internal override _W__SearchWindow.FillterData_Inputs CallHeader()
        {   var result = new _W__SearchWindow.FillterData_Inputs(callFromExternal_objects)
            {   Valudator = (oo) => Validate(oo.go),
                    SelectCompareString = (d, i) => d.go.tag,
                    SelectCompareCostInt = (d, i) =>
                {   var cost = i;
                    cost += d.go.activeInHierarchy ? 0 : 100000000;
                    return cost;
                }
            };
            return result;
        }
        
        internal _W__SearchWindow.FillterData_Inputs CallHeaderFiltered( string filter )
        {   var result = CallHeader();
            result.Valudator = s => Validate( s.go ) && s.go.tag == filter;
            return result;
        }
        /** CALL HEADER */
        
        
        /*
                    internal override bool CallHeader(out GameObject[] obs, out int[] contentCost)
                    {
                        obs = Utilities.AllSceneObjects().Where(Validate).ToArray();
                        contentCost = obs
                           .Select((d, i) => new { name = d.tag, startIndex = i, obj = d })
                           .OrderBy(d => d.name)
                            .Select((d, i) => {
                                var cost = i;
                                cost += d.obj.activeInHierarchy ? 0 : 100000000;
                                return new { d.startIndex, cost = cost };
                            })
                           .OrderBy(d => d.startIndex)
                           .Select(d => d.cost).ToArray();
                        return true;
                    }
        
                    internal void CallHeaderFiltered(out GameObject[] obs, out int[] contentCost, string filter)
                    {
                        obs = Utilities.AllSceneObjects().Where(s => Validate(s) && s.tag == filter).ToArray();
                        contentCost = obs
                           .Select((d, i) => new { name = d.tag, startIndex = i, obj = d })
                           .OrderBy(d => d.name)
                           .Select((d, i) => {
                               var cost = i;
                               cost += d.obj.activeInHierarchy ? 0 : 100000000;
                               return new { d.startIndex, cost = cost };
                           })
                           .OrderBy(d => d.startIndex)
                           .Select(d => d.cost).ToArray();
                    }*/
    }
}
}

