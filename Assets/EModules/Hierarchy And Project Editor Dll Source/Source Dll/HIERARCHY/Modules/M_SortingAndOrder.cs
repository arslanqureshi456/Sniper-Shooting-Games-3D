using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

//namespace EModules

namespace EModules.EModulesInternal

{
internal partial class Hierarchy {

    class GameObjectCacher<T> where T : Component {
        Dictionary<int, T> dic = new Dictionary<int, T>();
        Dictionary<int, double> update = new Dictionary<int, double>();
        const double UPDATETIME = 1;
        
        internal T GetValue( int instanceID )
        {   if ( !dic.ContainsKey( instanceID ) )
            {   var g = EditorUtility.InstanceIDToObject(instanceID);
                if ( !g ) return null;
                var g2 = ((GameObject)g).GetComponent<T>();
                dic.Add( instanceID, g2 );
                update.Add( instanceID, g2 == null ? EditorApplication.timeSinceStartup : -1 );
            }
            
            var result = dic[instanceID];
            
            if ( !result )
            {   if ( update[instanceID] == -1 )
                {   dic.Remove( instanceID );
                    update.Remove( instanceID );
                }
                else
                {   if ( Math.Abs( EditorApplication.timeSinceStartup - update[instanceID] ) > UPDATETIME )
                    {   var g = EditorUtility.InstanceIDToObject(instanceID);
                        if ( !g ) return null;
                        var g2 = ((GameObject)g).GetComponent<T>();
                        dic[instanceID] = g2;
                        update[instanceID] = g2 == null ? EditorApplication.timeSinceStartup : -1;
                    }
                }
            }
            
            return result;
        }
    }
    
    
    class M_SpritesOrder : Adapter.Module {
        public M_SpritesOrder( int restWidth, int sibbildPos, bool enable, Adapter adapter ) : base( restWidth, sibbildPos, enable, adapter )
        {
        
            /* var loadAss = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset");
             if (loadAss == null || loadAss.Length == 0)
             {
                 EditorUtility.DisplayDialog("Error", "Error load M_SpritesOrder", "Ok");
                 return;
             }
             var tagManager = new SerializedObject(loadAss[0]);
             foreach (var p in tagManager.GetIterator())
             {
                 MonoBehaviour.print(((SerializedProperty)p).name);
            
             }*/
            /* var findProperty = tagManager.FindProperty("layers");
             if (findProperty == null)
             {
                 EditorUtility.DisplayDialog("Error", "Error load M_SpritesOrder", "Ok");
                 return;
             }
             for (int j = 8; j < findProperty.arraySize; j++)
             {
                 SerializedProperty layerSP = findProperty.GetArrayElementAtIndex(j);
                 if (layerSP.stringValue == "")
                 {
                     layerSP.stringValue = str;
                     tagManager.ApplyModifiedProperties();
                     break;
                 }
                 if (j == findProperty.arraySize - 1)
                 {
                     EditorUtility.DisplayDialog("Error", "No free slots", "Ok");
                     return;
                 }
             }*/
        }
        
        static PropertyInfo _findProperty;
        private string[] sortingLayers
        {   get
            {
            
                if ( _findProperty == null )
                    _findProperty = typeof( InternalEditorUtility ).GetProperty( "sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic );
                return _findProperty.GetValue( null, null ) as string[];
            }
        }
        
        static GUIContent content = new GUIContent();
        static GameObjectCacher<Renderer> cache = new GameObjectCacher<Renderer>();
        static Rect RRR;
        
        
        
        internal override float Draw( Rect drawRect, Adapter.HierarchyObject _o )
        {
        
            if ( !START_DRAW( drawRect, _o ) ) return 0;
            
            
            var o = _o.go;
            
            // if (!r) return width;
            
            //MonoBehaviour.print(r.sortingLayerName);
            
            
            var r = cache.GetValue(o.GetInstanceID());
            
            drawRect.width /= 3;
            
            DrawSortingOrder( drawRect, r, _o );
            
            drawRect.x += drawRect.width;
            drawRect.width *= 2;
            
            
            DrawLayers( drawRect, r, _o );
            
            
            END_DRAW( _o );
            return width;
        }
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        void DrawSortingOrder( Rect drawRect, Renderer r, Adapter.HierarchyObject _o )
        {
        
            var   hasContent  = false;
            
            if ( r )
            {   content.text = r.sortingOrder.ToString();
                content.tooltip = "Order " + content.text;
                if ( content.text == "" ) content.text = "...Missing";
                if ( r.sortingOrder != 0 ) hasContent = true;
                /* var fs = Adapter.GET_SKIN().label.fontSize;
                 var al = Adapter.GET_SKIN().label.alignment;
                 Adapter.GET_SKIN().label.alignment = TextAnchor.MiddleRight;
                 if ( !callFromExternal() ) Adapter.GET_SKIN().label.fontSize = adapter.FONT_8();
                 else Adapter.GET_SKIN().label.fontSize = adapter.WINDOW_FONT_8();*/
                
                /* GUI.enabled = r.gameObject.activeInHierarchy;
                 GUI.Label( drawRect, content, !callFromExternal() ? adapter.STYLE_LABEL_8_right : adapter.STYLE_LABEL_8_WINDOWS_right );
                 GUI.enabled = true;*/
                Draw_Label( drawRect, content, !callFromExternal() ? adapter.STYLE_LABEL_8_right : adapter.STYLE_LABEL_8_WINDOWS_right, true );
                
                /*   Adapter.GET_SKIN().label.alignment = al;
                   Adapter.GET_SKIN().label.fontSize = fs;*/
            }
            
            
            
            /*  var so = EditorGUI.IntField(drawRect, r.sortingOrder);
              if (so != r.sortingOrder)
              {
                  SetOrder(r, so);
              }*/
            
            /* if ( adapter.ModuleButton( drawRect, null, hasContent ) )
             {
            
            
            
             }*/
            Draw_ModuleButton( drawRect, content, BUTTON_ACTION_ORDER_HASH, hasContent );
        }
        
        
        
        GUIContent emptyContent = new GUIContent();
        
        void DrawLayers( Rect drawRect, Renderer r, Adapter.HierarchyObject _o )
        {
        
            var hasContent = false;
            
            if ( r )
            {   content.tooltip = r.sortingLayerName /* base.ContextHelper*/;
                // content.text = r.sortingLayerName;
                content.text = GET_STRING( content.tooltip, callFromExternal() ? 0 : adapter.par.UPPER_SORT );
                // if ( Event.current.type == EventType.Repaint )
                {   RRR = drawRect;
                    RRR.y += 1;
                    RRR.height -= 2;
                    RRR.width -= 1;
                    
                    Draw_Style( RRR, adapter.box, emptyContent, USE_GO: true );
                    // adapter.box.Draw( RRR, "", false, false, false, false );
                }
                // MonoBehaviour.print(r.sortingLayerName);
                if ( r.sortingLayerName != "Default" )
                {   /*  var fs = Adapter.GET_SKIN().label.fontSize;
                      var al = Adapter.GET_SKIN().label.alignment;
                      Adapter.GET_SKIN().label.alignment = TextAnchor.MiddleLeft;
                      if ( !callFromExternal() ) Adapter.GET_SKIN().label.fontSize = adapter.FONT_8();
                      else Adapter.GET_SKIN().label.fontSize = adapter.WINDOW_FONT_8();*/
                    // Adapter.GET_SKIN().label.fontSize = Hierarchy.FONT_8();
                    
                    /* GUI.enabled = r.gameObject.activeInHierarchy;
                     GUI.Label( drawRect, content, !callFromExternal() ? adapter.STYLE_LABEL_8_right : adapter.STYLE_LABEL_8_WINDOWS_right );
                     GUI.enabled = true;*/
                    
                    Draw_Label( drawRect, content, !callFromExternal() ? adapter.STYLE_LABEL_8_right : adapter.STYLE_LABEL_8_WINDOWS_right, true );
                    
                    
                    hasContent = true;
                    /*Adapter.GET_SKIN().label.alignment = al;
                    Adapter.GET_SKIN().label.fontSize = fs;*/
                }
                else       //GUI.Label(drawRect, "-");
                {
                
                    /* var a = adapter.label.alignment;
                    adapter.label.alignment = __Align;
                    GUI.Label( drawRect, "-", adapter.label );
                    adapter.label.alignment = a;*/
                    Draw_Label( drawRect, "-", adapter.STYLE_LABEL_8_right, true );
                    
                    
                }
            }
            
            //var bg = Adapter.GET_SKIN().button.active.background;
            // Adapter.GET_SKIN().button.active.background = Hierarchy.GetIcon("BUT");
            
            /*if ( adapter.ModuleButton( drawRect, null, hasContent ) )
            {
            
            }*/
            Draw_ModuleButton( drawRect, content, BUTTON_ACTION_LAYER_HASH, hasContent );
            
            //   Adapter.GET_SKIN().button.active.background = bg;
            
        }
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        void SetLayer( Renderer r, string sortingLayer )
        {   var o = r.gameObject;
            if ( adapter.SELECTED_GAMEOBJECTS().All( selO => selO.go != o ) )
            {   Undo.RecordObject( r, "Change sortingLayerName" );
                r.sortingLayerName = sortingLayer;
                Adapter.SetDirty( r );
                adapter.MarkSceneDirty( o.scene );
                if ( Hierarchy.HierarchyAdapterInstance.par.ENABLE_PING_Fix ) adapter.TRY_PING_OBJECT( o );
                
            }
            else
            {   foreach ( var objectToUndo in adapter.SELECTED_GAMEOBJECTS() )
                {   var c = cache.GetValue(objectToUndo.go.GetInstanceID());
                    if ( !c ) continue;
                    Undo.RecordObject( c, "Change sortingLayerName" );
                    c.sortingLayerName = sortingLayer;
                    Adapter.SetDirty( c );
                    adapter.MarkSceneDirty( c.gameObject.scene );
                    //  if (Hierarchy.par.ENABLE_PING_Fix) adapter.TRY_PING_OBJECT(objectToUndo);
                    
                }
            }
        }
        void SetOrder( Renderer r, int order )
        {   var o = r.gameObject;
            if ( adapter.SELECTED_GAMEOBJECTS().All( selO => selO.go != o ) )
            {   Undo.RecordObject( r, "Change sortingOrder" );
                r.sortingOrder = order;
                Adapter.SetDirty( r );
                adapter.MarkSceneDirty( o.scene );
                if ( Hierarchy.HierarchyAdapterInstance.par.ENABLE_PING_Fix ) adapter.TRY_PING_OBJECT( o );
                
            }
            else
            {   foreach ( var objectToUndo in adapter.SELECTED_GAMEOBJECTS() )
                {   var c = cache.GetValue(objectToUndo.go.GetInstanceID());
                    if ( !c ) continue;
                    Undo.RecordObject( c, "Change sortingOrder" );
                    c.sortingOrder = order;
                    Adapter.SetDirty( c );
                    adapter.MarkSceneDirty( c.gameObject.scene );
                    //  if (Hierarchy.par.ENABLE_PING_Fix) adapter.TRY_PING_OBJECT(objectToUndo);
                    
                }
            }
        }
        
        
        
        bool Validate( GameObject o )     // return !string.IsNullOrEmpty(o.tag) && o.tag != "Untagged";
        {   return cache.GetValue( o.GetInstanceID() );
        }
        bool Validate( GameObject o, string filter )      // return !string.IsNullOrEmpty(o.tag) && o.tag != "Untagged";
        {   var g = cache.GetValue(o.GetInstanceID());
            return g != null && g.sortingLayerName == filter;
        }
        bool Validate( GameObject o, int sortingOrder )      // return !string.IsNullOrEmpty(o.tag) && o.tag != "Untagged";
        {   var g = cache.GetValue(o.GetInstanceID());
            return g != null && g.sortingOrder == sortingOrder;
        }
        
        
        
        
        
        /* FillterData.Init(Event.current.mousePosition, SearchHelper, LayerMask.LayerToName(o.layer),
                     Validate(o) ?
                     CallHeaderFiltered(LayerMask.LayerToName(o.layer)) :
                     CallHeader(),
                     this);*/
        /** CALL HEADER */
        internal override _W__SearchWindow.FillterData_Inputs CallHeader()
        {   var result = new _W__SearchWindow.FillterData_Inputs(callFromExternal_objects)
            {   Valudator = o => Validate(o.go) && (cache.GetValue(o.go.GetInstanceID()).sortingLayerName != "Default" || cache.GetValue(o.go.GetInstanceID()).sortingOrder != 0),
                    SelectCompareString = (d, i) =>
                {   return cache.GetValue(d.go.GetInstanceID()).sortingLayerName + " || " + cache.GetValue(d.go.GetInstanceID()).sortingOrder.ToString();
                },
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
            result.Valudator = s => Validate( s.go, filter );
            result.SelectCompareString = ( d, i ) => cache.GetValue( d.go.GetInstanceID() ).sortingLayerName;
            return result;
        }
        
        internal _W__SearchWindow.FillterData_Inputs CallHeaderFiltered( int sortingOrder )
        {   var result = CallHeader();
            result.Valudator = s => Validate( s.go, sortingOrder );
            result.SelectCompareString = ( d, i ) => cache.GetValue( d.go.GetInstanceID() ).sortingOrder.ToString();
            return result;
        }
        /** CALL HEADER */
        
        
        /*
        
                    internal override bool CallHeader(out GameObject[] obs, out int[] contentCost)
                    {
                        obs = Utilities.AllSceneObjects().Where(o => Validate(o)).Where(r => cache.GetValue(r.GetInstanceID()).sortingLayerName != "Default" || cache.GetValue(r.GetInstanceID()).sortingOrder != 0).ToArray();
                        contentCost = obs
                           .Select((d, i) => new { cache.GetValue(d.GetInstanceID()).sortingLayerName, startIndex = i, obj = d, cache.GetValue(d.GetInstanceID()).sortingOrder })
        
                           .OrderBy(d => d.sortingLayerName).ThenBy(d => d.sortingOrder)
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
                        obs = Utilities.AllSceneObjects().Where(s => Validate(s, filter)).ToArray();
                        contentCost = obs
                           .Select((d, i) => new { name = cache.GetValue(d.GetInstanceID()).sortingLayerName, startIndex = i, obj = d })
                           .OrderBy(d => d.name)
                           .Select((d, i) => {
                               var cost = i;
                               cost += d.obj.activeInHierarchy ? 0 : 100000000;
                               return new { d.startIndex, cost = cost };
                           })
                           .OrderBy(d => d.startIndex)
                           .Select(d => d.cost).ToArray();
                    }
        
                    internal void CallHeaderFiltered(out GameObject[] obs, out int[] contentCost, int sortingOrder)
                    {
                        obs = Utilities.AllSceneObjects().Where(s => Validate(s, sortingOrder)).ToArray();
                        contentCost = obs
                           .Select((d, i) => new { name = cache.GetValue(d.GetInstanceID()).sortingOrder, startIndex = i, obj = d })
                           .OrderBy(d => d.name)
                           .Select((d, i) => {
                               var cost = i;
                               cost += d.obj.activeInHierarchy ? 0 : 100000000;
                               return new { d.startIndex, cost = cost };
                           })
                           .OrderBy(d => d.startIndex)
                           .Select(d => d.cost).ToArray();
                    }*/
        
        
        
        
        
        
        
        
        Adapter. DrawStackMethodsWrapper __BUTTON_ACTION_LAYER_HASH = null;
        Adapter.DrawStackMethodsWrapper BUTTON_ACTION_LAYER_HASH { get { return __BUTTON_ACTION_LAYER_HASH ?? (__BUTTON_ACTION_LAYER_HASH = new Adapter.DrawStackMethodsWrapper( BUTTON_ACTION_LAYER )); } }
        void BUTTON_ACTION_LAYER( Rect worldOffset, Rect inputRect, Adapter.DrawStackMethodsWrapperData data, Adapter.HierarchyObject _o)
        {   var o = _o.go;
            var r = cache.GetValue(o.GetInstanceID());
#pragma warning disable
            var content = data.content;
#pragma warning restore
            if ( r && Event.current.button == 0 )
            {
            
            
            
                var l = sortingLayers;
                
                // var select = -1;
                // var ordered = l.OrderBy(f => f.Key).Select(f => f.Value).ToArray();
                var oldSelect = content.text;
                Action<int> Callback = (res) =>
                {   SetLayer(r, l[res]);
                    /*  Undo.RecordObject(o, "Change tag");
                      o.tag = l[res];
                      Hierarchy.SetDirty(o);
                      if (Hierarchy.par.ENABLE_PING_Fix) adapter.TRY_PING_OBJECT(o);*/
                };
                
                
                GenericMenu menu = new GenericMenu();
                
                for ( int i = 0 ; i < l.Length ; i++ )
                {   var ind = i;
                    content.text = l[i];
                    menu.AddItem( new GUIContent( content ), GET_STRING( content.text, adapter.par.UPPER_SORT ) == oldSelect, () => Callback( ind ) );
                }
                menu.AddSeparator( "" );
                
                /*    menu.AddItem(new GUIContent("Show 'Tags And Layers' Settings"), false, () => {
                      Selection.objects = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset");
                  });
                  menu.AddSeparator("");*/
                
                var pos = new MousePos( Event.current.mousePosition, MousePos.Type.Input_128_68, !callFromExternal(), adapter);
                //  var pos = InputData.WidnwoRect(!callFromExternal(), Event.current.mousePosition, 128, 68, adapter );
                menu.AddItem( new GUIContent( "+ Assign a New SortingLayer" ), false, () =>
                {
                
                
                    _W__InputWindow.Init( pos, "New SortingLayer name's", adapter, ( str ) =>
                    {   if ( string.IsNullOrEmpty( str ) ) return;
                        str = str.Trim();
                        var lowwer = l.Select(ord => ord.ToLower()).ToList();
                        var ind = lowwer.IndexOf(str.ToLower());
                        if ( ind != -1 )
                        {   SetLayer( r, l[ind] );
                            /* Undo.RecordObject(o, "Change tag");
                             o.tag = l[ind];
                             Hierarchy.SetDirty(o);
                             if (Hierarchy.par.ENABLE_PING_Fix) adapter.TRY_PING_OBJECT(o);*/
                        }
                        else
                        {
                        
                        
                            var UpdateSortingLayersOrder = typeof(InternalEditorUtility).GetMethod("UpdateSortingLayersOrder", (BindingFlags)(-1));
                            var AddSortingLayer = typeof(InternalEditorUtility).GetMethod("AddSortingLayer", (BindingFlags)(-1));
                            var SetSortingLayerName = typeof(InternalEditorUtility).GetMethod("SetSortingLayerName", (BindingFlags)(-1));
                            var GetSortingLayerCount = typeof(InternalEditorUtility).GetMethod("GetSortingLayerCount", (BindingFlags)(-1));
                            
                            var count = GetSortingLayerCount.Invoke(null, null);
                            AddSortingLayer.Invoke( null, null );
                            SetSortingLayerName.Invoke( null, new[] { count, str } );
                            UpdateSortingLayersOrder.Invoke( null, null );
                            /*
                                                                var loadAss = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset");
                                                                if (loadAss == null || loadAss.Length == 0)
                                                                {
                                                                    EditorUtility.DisplayDialog("Error", "Error M_SpritesOrder layers", "Ok");
                                                                    return;
                                                                }
                                                                var tagManager = new SerializedObject(loadAss[0]);
                                                                var findProperty = tagManager.FindProperty("m_SortingLayers");
                                                                if (findProperty == null)
                                                                {
                                                                    EditorUtility.DisplayDialog("Error", "Error M_SpritesOrder layers", "Ok");
                                                                    return;
                                                                }
                                                                findProperty.InsertArrayElementAtIndex(findProperty.arraySize);
                                                                SerializedProperty layerSP = findProperty.GetArrayElementAtIndex(findProperty.arraySize - 1);
                                                                layerSP.stringValue = str;
                                                                tagManager.ApplyModifiedProperties();*/
                            /**  UnityEditorInternal.InternalEditorUtility.AddTag(str);  */
                            
                            SetLayer( r, str );
                            /*  Undo.RecordObject(o, "Change tag");
                              o.tag = str;
                              Hierarchy.SetDirty(o);
                              if (Hierarchy.par.ENABLE_PING_Fix) adapter.TRY_PING_OBJECT(o);*/
                        }
                    } );
                } );
                
                menu.AddSeparator( "" );
                menu.AddItem( new GUIContent( "Show only uppercase letters" ), adapter.par.UPPER_SORT != 0, () =>
                {   adapter.par.UPPER_SORT = 1 - adapter.par.UPPER_SORT;
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
                /*     int[] contentCost = new int[0];
                     GameObject[] obs = new GameObject[0];
                
                     if (r && Validate(r.gameObject))
                     {
                         if (EditorSceneManager.GetActiveScene().rootCount != 0) CallHeaderFiltered(out obs, out contentCost, r.sortingLayerName);
                     } else
                     {
                         CallHeader(out obs, out contentCost);
                     }
                
                     FillterData.Init(Event.current.mousePosition, SearchHelper, "'sortingLayerName' = " + (r ? r.sortingLayerName.ToString() : "All assigned"), obs, contentCost, null, this);
                */
                
                var mp = new MousePos( Event.current.mousePosition, MousePos.Type.Search_356_0, !callFromExternal(), adapter);
                _W__SearchWindow.Init( mp, SearchHelper, "'sortingLayerName' = " + (r ? r.sortingLayerName.ToString() : "All assigned"),
                                       r && Validate( r.gameObject ) ?
                                       CallHeaderFiltered( r.sortingLayerName ) :
                                       CallHeader(),
                                       this, adapter, _o );
                // EditorGUIUtility.ic
            }
        }
        
        Adapter. DrawStackMethodsWrapper __BUTTON_ACTION_ORDER_HASH = null;
        Adapter.DrawStackMethodsWrapper BUTTON_ACTION_ORDER_HASH { get { return __BUTTON_ACTION_ORDER_HASH ?? (__BUTTON_ACTION_ORDER_HASH = new Adapter.DrawStackMethodsWrapper( BUTTON_ACTION_ORDER )); } }
        void BUTTON_ACTION_ORDER( Rect worldOffset, Rect inputRect, Adapter.DrawStackMethodsWrapperData data, Adapter.HierarchyObject _o )
        {   var o = _o.go;
            var r = cache.GetValue(o.GetInstanceID());
#pragma warning disable
            var content = data.content;
#pragma warning restore
            if ( r && Event.current.button == 0 )
            {
            
                var pos = new MousePos( Event.current.mousePosition, MousePos.Type.Input_190_68, !callFromExternal(), adapter);
                //  var pos = InputData.WidnwoRect(!callFromExternal(), Event.current.mousePosition, 190, 68, adapter );
                /*   var lowwer = ;
                   var ind = lowwer.IndexOf(str.ToLower());
                   string sendText = null;
                   if (ind != -1)
                   {
                       list.a
                   }*/
                
                Action<string> act = (str) =>
                {   int pars = 0;
                    int.TryParse(str, out pars);
                    SetOrder(r, pars);
                    
                    //Hierarchy.MarkSceneDirty(o.scene);
                    //var lowwer = list.listValues.Select(ord => ord.ToLower()).ToList();
                    // var ind = lowwer.IndexOf(str.ToLower());
                    /*  if (ind != -1)
                      {
                          Undo.RecordObject(o, "Change description");
                          list.a
                          //o.tag = l[ind];
                          Hierarchy.SetDirty(o);
                          adapter.TRY_PING_OBJECT(o);
                      }
                      else
                      {*/
                    
                    
                    //  }
                    
                };
                _W__InputWindow.InitTeger( pos, "Sorting Order", adapter, act, null, r.sortingOrder.ToString() );
            }
            
            if ( Event.current.button == 1 )
            {   Adapter.EventUse();
                /*      int[] contentCost = new int[0];
                      GameObject[] obs = new GameObject[0];
                
                      if (r && Validate(r.gameObject))
                      {
                          if (EditorSceneManager.GetActiveScene().rootCount != 0) CallHeaderFiltered(out obs, out contentCost, r.sortingOrder);
                      } else
                      {
                          CallHeader(out obs, out contentCost);
                      }
                
                      FillterData.Init(Event.current.mousePosition, SearchHelper, "'sortingOrder' = " + (r ? r.sortingOrder.ToString() : "All assigned"), obs, contentCost, null, this);
                */
                var mp = new MousePos( Event.current.mousePosition, MousePos.Type.Search_356_0, !callFromExternal(), adapter);
                _W__SearchWindow.Init( mp, SearchHelper, "'sortingOrder' = " + (r ? r.sortingOrder.ToString() : "All assigned"),
                                       r && Validate( r.gameObject ) ?
                                       CallHeaderFiltered( r.sortingOrder ) :
                                       CallHeader(),
                                       this, adapter, _o );
                // EditorGUIUtility.ic
            }
        }
        
        
        
        
        
        
        
        
        
        
        
    }
}
}



