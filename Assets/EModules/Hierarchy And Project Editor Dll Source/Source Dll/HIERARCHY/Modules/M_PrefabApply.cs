using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


using UnityEditor;
using UnityEngine.SceneManagement;
//namespace EModules

namespace EModules.EModulesInternal {
internal partial class Hierarchy {
    class M_PrefabApply : Adapter.Module {
        public M_PrefabApply(int restWidth, int sib, bool enable, Adapter adapter ) : base(restWidth, sib, enable, adapter )
        {
        }
        
        GUIContent content = new GUIContent();
        
        internal override float Draw(Rect drawRect, Adapter.HierarchyObject _o)
        {
        
        
        
            if ( !START_DRAW( drawRect, _o ) ) return 0;
            
            var o = _o.go;
            var prefab_root = adapter.FindPrefabRoot(o);
            if ( !prefab_root )
            {
            
                END_DRAW( _o );
                return 0;
            }
            var prefab_src = adapter.GetCorrespondingObjectFromSource(prefab_root);
            
            var oldRect = drawRect;
            
            if (prefab_src != null)
            {   var oldW = drawRect.width;
                var oldH = drawRect.height;
                if (o != prefab_root)
                    drawRect.width = drawRect.height = 6;
                else
                    drawRect.width = drawRect.height = 12;
                drawRect.x += (oldW - drawRect.width) / 2;
                drawRect.y += (oldH - drawRect.width) / 2;
                
                var c = Color.white;
                //  if ( o != prefab_root ) c.a *= 0.5f;
                // Adapter.DrawTexture(drawRect, adapter.GetIcon("PREF"));
                Draw_AdapterTexture( drawRect, adapter.GetIcon( "PREF" ), c, USE_GO: true );
                
                //**// if (o != prefab_root)
                //**//  {
                /* var col = Hierarchy.LINE;
                               col.r = col.g = col.b = 0.2f;
                               col.a = .6f;
                               Hierarchy.colorText11.SetPixel(0, 0, col);
                               Hierarchy.colorText11.Apply();
                               GUI.DrawTexture(drawRect, Hierarchy.colorText11, ScaleMode.ScaleAndCrop, true, 1); */
                
                //**// Adapter.FadeRect(drawRect, 0.5f);
                //**// }
                
                //**// if ( !o.activeInHierarchy) Adapter.FadeRect(drawRect, 0.5f);
                
                if (oldRect.Contains(Event.current.mousePosition))
                    content.tooltip = "'" + prefab_src.name + "' prefab\n(CLICK - Select source in project)\n(Ctrl+CLICK - Apply changes to Prefab)";
            }
            else
            {   if (oldRect.Contains(Event.current.mousePosition))
                    content.tooltip = "";
            }
            
            
            // drawRect.y -= 2;
            /*if (adapter.ModuleButton(oldRect, content, true))
            {
            
            }*/
            str.prefab_root = prefab_root;
            str.prefab_src = prefab_src;
            Draw_ModuleButton( oldRect, content, BUTTON_ACTION_HASH, true,  str, true );
            
            
            END_DRAW( _o );
            
            return width;
        }
        prefab_str str;
        
        bool Validate(GameObject o)
        {   if ( !o ) return false;
            var prefab_root = adapter.FindPrefabRoot(o);
            var prefab_src = adapter.GetCorrespondingObjectFromSource(prefab_root);
            return prefab_src != null;
        }
        bool ValidateObyTop(GameObject o)
        {   if ( !o ) return false;
            var prefab_root = adapter.FindPrefabRoot(o);
            var prefab_src = adapter.GetCorrespondingObjectFromSource(prefab_root);
            return prefab_src != null && o == prefab_root;
        }
        bool Validate(GameObject o, UnityEngine.Object prefabsrc)
        {   if ( !o ) return false;
            var prefab_root = adapter.FindPrefabRoot(o);
            var prefab_src = adapter.GetCorrespondingObjectFromSource(prefab_root);
            return prefab_src != null && prefab_src == prefabsrc && o == prefab_root;
        }
        
        
        
        /* FillterData.Init(Event.current.mousePosition, SearchHelper, LayerMask.LayerToName(o.layer),
                     Validate(o) ?
                     CallHeaderFiltered(LayerMask.LayerToName(o.layer)) :
                     CallHeader(),
                     this);*/
        /** CALL HEADER */
        internal override _W__SearchWindow.FillterData_Inputs CallHeader()
        {   var result = new _W__SearchWindow.FillterData_Inputs(callFromExternal_objects)
            {   Valudator = (oo) => ValidateObyTop(oo.go),
                    SelectCompareString = (d, i) => "",
                    SelectCompareCostInt = (d, i) =>
                {   var cost = i;
                    cost += d.go.activeInHierarchy ? 0 : 100000000;
                    return cost;
                }
            };
            return result;
        }
        
        internal _W__SearchWindow.FillterData_Inputs CallHeaderFiltered(UnityEngine.Object prefabsrc)
        {   var result = CallHeader();
            result.Valudator = s => Validate(s.go, prefabsrc);
            result.SelectCompareString = (d, i) => i.ToString();
            return result;
        }
        /** CALL HEADER */
        
        
        /*
                    internal override bool CallHeader(out GameObject[] obs, out int[] contentCost)
                    {
                        obs = Utilities.AllSceneObjects().Where(ValidateObyTop).ToArray();
                        contentCost = obs.Select(o => 0).ToArray();
                        return true;
                    }
        
                    void CallHeaderFiltered(out GameObject[] obs, out int[] contentCost, UnityEngine.Object prefabsrc)
                    {
                        obs = Utilities.AllSceneObjects().Where(s => Validate(s, prefabsrc)).ToArray();
                        contentCost = obs.Select(o => o.activeInHierarchy ? 0 : 1).ToArray();
                    }*/
        
        
        
        internal struct prefab_str
        {   internal UnityEngine.Object prefab_src;
            internal GameObject prefab_root;
        }
        Adapter. DrawStackMethodsWrapper __BUTTON_ACTION_HASH = null;
        Adapter.DrawStackMethodsWrapper BUTTON_ACTION_HASH { get { return __BUTTON_ACTION_HASH ?? (__BUTTON_ACTION_HASH = new Adapter.DrawStackMethodsWrapper( BUTTON_ACTION )); } }
        void BUTTON_ACTION( Rect worldOffset, Rect inputRect, Adapter.DrawStackMethodsWrapperData data, Adapter.HierarchyObject _o )
        {
        
#pragma warning disable
            var content = data.content;
#pragma warning restore
            var o = _o.go;
            var args = (prefab_str)data.args;
            var prefab_src = args.prefab_src;
            var prefab_root = args.prefab_root;
            if ( Event.current.button == 0 && !Application.isPlaying )
            {   if ( !Event.current.control )
                {   Adapter.EventUse();
                    Selection.objects = new[] { prefab_src };
                }
                else
                {   if ( adapter.SELECTED_GAMEOBJECTS().All( selO => selO.go != o ) )
                    {   adapter.ReplacePrefab( prefab_root, prefab_src );
                        // adapter.logProxy.Log( "Updated prefab : " + AssetDatabase.GetAssetPath( prefab_src ) );
                        if ( Hierarchy.HierarchyAdapterInstance.par.ENABLE_PING_Fix ) adapter.TRY_PING_OBJECT( o );
                    }
                    else
                    {   Dictionary<GameObject, UnityEngine.Object> result = new Dictionary<GameObject, UnityEngine.Object>();
                        bool error = false;
                        foreach ( var selob in adapter.SELECTED_GAMEOBJECTS() )
                        {   if ( !Validate( selob.go ) ) continue;
                            var p = adapter.FindPrefabRoot(selob.go);
                            var src = adapter.GetCorrespondingObjectFromSource(p);
                            if ( result.ContainsKey( p ) ) continue;
                            if ( result.Values.Any( v => v == src ) )
                            {   var first = result.First(v => v.Value == src);
                                adapter.logProxy.LogWarning( "Two or more selected objects refer to the same prefab. \n" +
                                                             "- '" + (first.Key.transform.parent == null ? "" : first.Key.transform.parent.name + "/") + first.Key.name + "'"
                                                             + " \n" +
                                                             "- '" + (p.transform.parent == null ? "" : p.transform.parent.name + "/") + p.name + "'"
                                                           );
                                error = true;
                            }
                            result.Add( p, src );
                        }
                        
                        if ( !error )
                        {   foreach ( var kp in result )
                            {   adapter.ReplacePrefab( kp.Key, kp.Value );
                                //adapter.logProxy.Log( "Updated prefab : " + AssetDatabase.GetAssetPath( kp.Value ) );
                            }
                        }
                        
                        
                    }
                    
                }
                
                
                
                
                
            }
            
            if ( Event.current.button == 1 )
            {   Adapter.EventUse();
                /*  int[] contentCost = new int[0];
                  GameObject[] obs = new GameObject[0];*/
                var mp = new MousePos( Event.current.mousePosition, MousePos.Type.Search_356_0, !callFromExternal(), adapter);
                
                if ( Validate( o ) )     // if (EditorSceneManager.GetActiveScene().rootCount != 0) CallHeaderFiltered(out obs, out contentCost, prefab_src);
                {
                
                    // FillterData.Init(Event.current.mousePosition, SearchHelper + ": '" + prefab_src.name + "'", "'" + prefab_src.name + "'", obs, contentCost, null, this);
                    
                    _W__SearchWindow.Init( mp, SearchHelper + ": '" + prefab_src.name + "'", "'" + prefab_src.name + "'",
                                           CallHeaderFiltered( prefab_src ),
                                           this, adapter, _o );
                }
                else         //  if (EditorSceneManager.GetActiveScene().rootCount != 0) CallHeader(out obs, out contentCost);
                {
                
                    //   FillterData.Init(Event.current.mousePosition, SearchHelper + ": 'All'", "'All'", obs, contentCost, null, this);
                    
                    _W__SearchWindow.Init( mp, SearchHelper + ": 'All'", "'All'",
                                           CallHeader(),
                                           this, adapter, _o );
                }
                
                
                
                // EditorGUIUtility.ic
            }
        }
    }
}
}


