﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

//namespace EModules

namespace EModules.EModulesInternal {
internal partial class Hierarchy {
    static Dictionary<string, string> uppercast = new Dictionary<string, string>();
    static string GET_STRING(string str, int upper)
    {   /*   if (str == "MySortingLayer")
               MonoBehaviour.print("ASD");*/
        if (upper == 0 || string.IsNullOrEmpty( str )) return str;
        if (!uppercast.ContainsKey( str ))
        {   var temp = str.ToUpper().ToCharArray()
                       .Select((s, i) => s == str[i] ? s.ToString() : null)
                       .Where(s => !string.IsNullOrEmpty(s) && s != " ");
            var res = temp.Count() == 0 ? str[0].ToString() : temp.Aggregate((a, b) => a + b);
            uppercast.Add( str, res );
        }
        return uppercast[str];
    }
    
    class M_Layers : Adapter.Module {
        public M_Layers(int restWidth, int sib, bool enable, Adapter adapter) : base( restWidth, sib, enable, adapter )
        {
        }
        
        private Dictionary<int, string> layers
        {   get
            {   Dictionary<int, string> result = new Dictionary<int, string>();
                for (int i = 0 ; i < 32 ; i++)
                {   if (string.IsNullOrEmpty( LayerMask.LayerToName( i ) )) continue;
                    result.Add( i, LayerMask.LayerToName( i ) );
                }
                return result;
            }
        }
        
        
        void SetLayer(GameObject o, string layer)
        {
        
            if (adapter.SELECTED_GAMEOBJECTS().All( selO => selO.go != o ))
            {   Undo.RecordObject( o, "Change Layer" );
                o.layer = LayerMask.NameToLayer( layer );
                Adapter.SetDirty( o );
                adapter.MarkSceneDirty( o.scene );
                if (Hierarchy.HierarchyAdapterInstance.par.ENABLE_PING_Fix) adapter.TRY_PING_OBJECT( o );
            }
            else
            {   foreach (var objectToUndo in adapter.SELECTED_GAMEOBJECTS())
                {   Undo.RecordObject( objectToUndo.go, "Change Layer" );
                    objectToUndo.go.layer = LayerMask.NameToLayer( layer );
                    Adapter.SetDirty( objectToUndo.go );
                    adapter.MarkSceneDirty( objectToUndo.scene );
                    //  if (Hierarchy.par.ENABLE_PING_Fix) adapter.TRY_PING_OBJECT(objectToUndo);
                }
            }
            ResetStack();
        }
        //static Color alpha = new Color(1, 1, 1, 0.3f);
        static GUIContent content = new GUIContent();
        internal override float Draw(Rect drawRect, Adapter.HierarchyObject _o)
        {
        
        
            if ( !START_DRAW( drawRect, _o ) ) return 0;
            
            var o = _o.go;
            
            // if (drawRect.Contains(Event.current.mousePosition))
            content.tooltip = LayerMask.LayerToName( o.layer );
            // content.tooltip = base.ContextHelper;
            content.text = GET_STRING( content.tooltip, callFromExternal() ? 0 : adapter.par.UPPER_LAYERS );
            if (content.text == "") content.text = "...Missing";
            bool hasContent = false;
            if (LayerMask.LayerToName( o.layer ) != "Default")
            {   /*var fs = Adapter.GET_SKIN().label.fontSize;
                var al = Adapter.GET_SKIN().label.alignment;
                Adapter.GET_SKIN().label.alignment = TextAnchor.MiddleLeft;
                if () Adapter.GET_SKIN().label.fontSize = adapter.FONT_8();
                else Adapter.GET_SKIN().label.fontSize = adapter.WINDOW_FONT_8();*/
                
                // Adapter.GET_SKIN().label.fontSize = Hierarchy.FONT_8();
                
                /*   GUI.enabled = o.activeInHierarchy;
                   GUI.Label( drawRect, content, !callFromExternal()  ? adapter.STYLE_LABEL_8 : adapter.STYLE_LABEL_8_WINDOWS );
                   GUI.enabled = true;*/
                hasContent = true;
                Draw_Label( drawRect, content, !callFromExternal() ? adapter.STYLE_LABEL_8 : adapter.STYLE_LABEL_8_WINDOWS, true );
            }
            else
            {   /* var c = GUI.color;
                 GUI.color *= alpha;
                 GUI.Label( drawRect, "-", adapter.STYLE_LABEL_8_right );
                     GUI.color = c;*/
                Draw_Label( drawRect, "-", adapter.STYLE_LABEL_8_right, true );
                
                
                
                /* var a = Adapter.GET_SKIN().label.alignment;
                 Adapter.GET_SKIN().label.alignment = __;*/
                /*  Adapter.GET_SKIN().label.alignment = a;*/
                //  GUI.Label(drawRect, "-");
                
            }
            
            // var bg = Adapter.GET_SKIN().button.active.background;
            // Adapter.GET_SKIN().button.active.background = Hierarchy.GetIcon("BUT");
            
            
            Draw_ModuleButton( drawRect, content, BUTTON_ACTION_HASH, hasContent );
            /*   if (adapter.ModuleButton( drawRect, null, hasContent ))
               {
            
            
               }*/
            /* if (drawRect.Contains(Event.current.mousePosition))
             {*/
            
            //Adapter.GET_SKIN().button.active.background = bg;
            
            
            
            
            /* var res = EditorGUI.Popup(drawRect, select, ordered);
             if (GUI.changed && res != -1)
             {
            
             }
             //}*/
            
            
            END_DRAW( _o );
            return width;
        }
        
        
        bool Validate(GameObject o)
        {   return LayerMask.LayerToName( o.layer ) != "Default";
        }
        bool Validate(Adapter.HierarchyObject o)
        {   return LayerMask.LayerToName( o.go.layer ) != "Default";
        }
        
        internal override _W__SearchWindow.FillterData_Inputs CallHeader()
        {   var result = new _W__SearchWindow.FillterData_Inputs(callFromExternal_objects)
            {   Valudator = Validate,
                    SelectCompareString = (d, i) => LayerMask.LayerToName(d.go.layer),
                    SelectCompareCostInt = (d, i) =>
                {   var cost = i;
                    cost += d.go.activeInHierarchy ? 0 : 100000000;
                    return cost;
                }
            };
            return result;
            /* obs = .Where(Validate).ToArray();
             contentCost = obs
                .Select((d, i) => new { name = LayerMask.LayerToName(d.layer), startIndex = i, obj = d })
                .OrderBy(d => d.name)
                 .Select((d, i) => {
                     var cost = i;
                     cost += d.obj.activeInHierarchy ? 0 : 100000000;
                     return new { d.startIndex, cost = cost };
                 })
                .OrderBy(d => d.startIndex)
                .Select(d => d.cost).ToArray();
             return true;*/
        }
        
        internal _W__SearchWindow.FillterData_Inputs CallHeaderFiltered(string filter)
        {   var result = CallHeader();
            result.Valudator = s => Validate( s ) && LayerMask.LayerToName( s.go.layer ) == filter;
            return result;
            /* obs = Utilities.AllSceneObjects().Where(s => Validate(s) && LayerMask.LayerToName(s.layer) == filter).ToArray();
             contentCost = obs
                .Select((d, i) => new { name = LayerMask.LayerToName(d.layer), startIndex = i, obj = d })
                .OrderBy(d => d.name)
                 .Select((d, i) => {
                     var cost = i;
                     cost += d.obj.activeInHierarchy ? 0 : 100000000;
                     return new { d.startIndex, cost = cost };
                 })
                .OrderBy(d => d.startIndex)
                .Select(d => d.cost).ToArray();*/
        }
        
        
        
        
        Adapter. DrawStackMethodsWrapper __BUTTON_ACTION_HASH = null;
        Adapter.DrawStackMethodsWrapper BUTTON_ACTION_HASH { get { return __BUTTON_ACTION_HASH ?? (__BUTTON_ACTION_HASH = new Adapter.DrawStackMethodsWrapper( BUTTON_ACTION )); } }
        void BUTTON_ACTION( Rect worldOffset, Rect inputRect, Adapter.DrawStackMethodsWrapperData data, Adapter.HierarchyObject _o )
        {
        
            var o = _o.go;
            var content = data.content;
            if ( Event.current.button == 0 )
            {
            
            
                var l = layers;
                // var select = -1;
                var ordered = l.OrderBy(f => f.Key).Select(f => f.Value).ToArray();
                var oldSelect = content.text;
                Action<int> Callback = (res) =>
                {   SetLayer(o, ordered[res]);
                };
                
                
                GenericMenu menu = new GenericMenu();
                
                for ( int i = 0 ; i < ordered.Length ; i++ )
                {   var ind = i;
                    content.text = ordered[i];
                    menu.AddItem( new GUIContent( content ), GET_STRING( ordered[i], adapter.par.UPPER_LAYERS ) == oldSelect, () => Callback( ind ) );
                }
                menu.AddSeparator( "" );
                
                /*    menu.AddItem(new GUIContent("Show 'Tags And Layers' Settings"), false, () => {
                        Selection.objects = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset");
                    });
                    menu.AddSeparator("");*/
                var pos = new MousePos( Event.current.mousePosition, MousePos.Type.Input_128_68, !callFromExternal(), adapter);
                // var pos = InputData.WidnwoRect(!callFromExternal(), Event.current.mousePosition, 128, 68, adapter );
                menu.AddItem( new GUIContent( "+ Assign a New Layer" ), false, () =>
                {   Action<string> act = (str) =>
                    {   if (string.IsNullOrEmpty(str)) return;
                        str = str.Trim();
                        var lowwer = ordered.Select(ord => ord.ToLower()).ToList();
                        var ind = lowwer.IndexOf(str.ToLower());
                        if (ind != -1)
                        {   SetLayer(o, ordered[ind]);
                            /*Undo.RecordObject(o, "Change Layer");
                            o.layer = LayerMask.NameToLayer(ordered[ind]);
                            Hierarchy.SetDirty(o);
                            if (Hierarchy.par.ENABLE_PING_Fix) adapter.TRY_PING_OBJECT(o);*/
                            
                            
                        }
                        else
                        {   var loadAss = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset");
                            if (loadAss == null || loadAss.Length == 0)
                            {   EditorUtility.DisplayDialog("Error", "Error load layers", "Ok");
                                return;
                            }
                            var tagManager = new SerializedObject(loadAss[0]);
                            var findProperty = tagManager.FindProperty("layers");
                            if (findProperty == null)
                            {   EditorUtility.DisplayDialog("Error", "Error load layers", "Ok");
                                return;
                            }
                            for (int j = 8; j < findProperty.arraySize; j++)
                            {   SerializedProperty layerSP = findProperty.GetArrayElementAtIndex(j);
                                if (layerSP.stringValue == "")
                                {   layerSP.stringValue = str;
                                    tagManager.ApplyModifiedProperties();
                                    break;
                                }
                                if (j == findProperty.arraySize - 1)
                                {   EditorUtility.DisplayDialog("Error", "No free slots", "Ok");
                                    return;
                                }
                            }
                            
                            // if (Selection.gameObjects == null || Selection.gameObjects.All(selO => selO != o))
                            //{
                            SetLayer(o, str);
                            /* Undo.RecordObject(o, "Change Layer");
                             o.layer = LayerMask.NameToLayer(str);
                             Hierarchy.SetDirty(o);
                             if (Hierarchy.par.ENABLE_PING_Fix) adapter.TRY_PING_OBJECT(o);*/
                            /*   }
                               else
                               {
                            
                               }*/
                            
                        }
                    };
                    
                    _W__InputWindow.Init( pos, "New layer name's", adapter, act );
                    //EditorUtility.DisplayDialogComplex(
                } );
                /*   menu.AddSeparator("");
                   menu.AddItem(new GUIContent("SubMenu/MenuItem3"), false, Callback, "item 3");*/
                
                menu.AddSeparator( "" );
                menu.AddItem( new GUIContent( "Show only uppercase letters" ), adapter.par.UPPER_LAYERS != 0, () =>
                {   adapter.par.UPPER_LAYERS = 1 - adapter.par.UPPER_LAYERS;
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
                /*int[] contentCost = new int[0];
                GameObject[] obs = new GameObject[0];*/
                var mp = new MousePos( Event.current.mousePosition, MousePos.Type.Search_356_0, !callFromExternal(), adapter);
                _W__SearchWindow.Init( mp, SearchHelper, LayerMask.LayerToName( o.layer ),
                                       Validate( o ) ?
                                       CallHeaderFiltered( LayerMask.LayerToName( o.layer ) ) :
                                       CallHeader(),
                                       this, adapter, _o );
                // EditorGUIUtility.ic
            }
        }
    }
    
    
    
    
}


}


