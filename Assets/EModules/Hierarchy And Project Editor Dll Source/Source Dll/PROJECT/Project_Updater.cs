using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using EModules.EModulesInternal;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;
using EModules.Project;
//namespace EModules

namespace EModules.Project {

}
namespace EModules.EProjectInternal {
internal partial class Project {

    internal static void SET_UNDO_FOR_HASHPROPERY(string name)
    {   /////UBNDI
    
        var p = ProjectDescriptionHelper.Initialize(adapter);
        if (!p) return;
        
        Undo.RecordObject( p, name );
        //SaveDescHashes();
        //* UNDO */
    }
    
    
    
    
    /*
    
     var path = AssetDatabase.GUIDToAssetPath(guid);
    var ext = path.LastIndexOf('.');
    if ( ext != -1 &&         ext > path.LastIndexOf('/') )
    {
        selectionRect.x += selectionRect.width;
        selectionRect.x -= 50;
        selectionRect.width = 50;
        GUI.Label(selectionRect, path.Substring(ext));
    }
    
    */
    
    
    //internal static InternalDescription DescHashes;
    internal static void SET_DIRTY()
    {   var p = ProjectDescriptionHelper.Initialize(adapter);
        if (!p) return;
        EditorUtility.SetDirty( p );
        // EditorPrefs.SetString(adapter.pluginname+ "/DescHashes" , Adapter.SERIALIZE_SINGLE( DescHashes ) );
    }
    
    
    internal static EModules.EModulesInternal.Adapter adapter;
    #if UNITY_EDITOR
    internal static EModules.EModulesInternal.HierParams par { get { return adapter.par; } }
    #else
    internal static HierParams par { get { return adapter.par; } }
    #endif
    
    internal static void AttachAdapter(EModules.EModulesInternal.Adapter externalAdapter)
    {   adapter = externalAdapter;
        externalAdapter.MOI = new MOI_Adapter();
        
        /* try { DescHashes = Adapter.DESERIALIZE_SINGLE< InternalDescription>( EditorPrefs.GetString( adapter.pluginname + "/DescHashes" ) )  ; }
         catch { }
         if ( DescHashes == null ) DescHashes = new InternalDescription();*/
        
        adapter.DefaulTypes = new string[]
        {   //  typeof(M_CustomIcons), typeof(M_Vertices), typeof(M_Audio),
            typeof(M_Descript).FullName, typeof(M_CustomIcons).FullName
        };
    }
    
    
    class MOI_Adapter : IMethodsInterface {
        public void InitModules() { Project.InitModules(); }
        
        public IHashProperty des(int scene) { return ProjectDescriptionHelper.Initialize( Adapter.ProjAdapter ); }
        public IHashProperty des(Scene scene) { return ProjectDescriptionHelper.Initialize( Adapter.ProjAdapter ); }
        
        
        public void RegistrateDescription(IDescriptionRegistrator o) { }
        
        public void CONTEXTMENU_STATICMODULES(GenericMenu menu) { Project.CONTEXTMENU_STATICMODULES( menu ); }
        
        
        IModuleOnnector_M_Vertices m_M_Vertices;
        public IModuleOnnector_M_Vertices M_Vertices
        {   get { return (m_M_Vertices ?? (m_M_Vertices = Adapter.ProjAdapter.modules.First( m => m is IModuleOnnector_M_Vertices ) as IModuleOnnector_M_Vertices)); }
        }
        //         Adapter.M_Colors m_M_Colors;
        //         public Adapter.M_Colors M_Colors
        //         {   get { return (m_M_Colors ?? (m_M_Colors = adapter.modules.First( m => m is Adapter.M_Colors ) as Adapter.M_Colors)); }
        //         }
        //  IModuleOnnector_M_CustomIcons m_M_CustomIcons;
        //         public IModuleOnnector_M_CustomIcons M_CustomIcons
        //         {   get { return (m_M_CustomIcons ?? (m_M_CustomIcons = adapter.modules.First( m => m is IModuleOnnector_M_CustomIcons ) as IModuleOnnector_M_CustomIcons)); }
        //         }
        /* IModuleOnnector_M_Decription m_M_Descript;
         public IModuleOnnector_M_Decription M_Descript
         {   get { return (m_M_Descript ?? (m_M_Descript = adapter.modules.First( m => m is IModuleOnnector_M_Decription ) as IModuleOnnector_M_Decription)); }
         }
         */
        
        
        
        public IModuleOnnector_M_PlayModeKeeper M_PlayModeKeeper
        {   get { return null; }
        }
        
        public IModuleOnnector_M_Freeze M_Freeze
        {   get { return null; }
        }
    }
    
    
    
    internal static void InitModules()
    {   if (adapter.wasModulesInitialize) return;
        adapter.wasModulesInitialize = true;
        //     MonoBehaviour.print("ASD");
        
        
        /*  var ComponentsModule = ;*/
        var START_W = 16;
        
        adapter.modules = new Adapter.Module[]
        {   /* new M_SetActive(START_W, -1, true, adapter)
             {
                 // SearchHelper = "Show 'GameObjects' whose 'Components' will persist in play mode",
                 HeaderText = "Enable-Disable GameObject",
                 ContextHelper = "Enable-Disable GameObject",
                 // HeaderTexture2D = "STORAGE_PASSIVE",
                 // disableSib = true
             },*/
            new M_Vertices(START_W * 2, 2, false, adapter)
            {   SearchHelper = "Show Optimizer",
                ContextHelper = "Memory Info",
                HeaderText = "Memory Info",
                HeaderTexture2D = "TRI"
            },
            new Adapter.M_Colors(START_W, -1, true, adapter)
            {   SearchHelper = "Show 'Objects' with",
                ContextHelper = "Change Object Icon"
            },
            /* new M_Warning(START_W, -1, true, adapter),
             new M_Freeze(START_W, 0, false, adapter)
             {
                 SearchHelper = "Show Locked 'GameObjects'",
                 ContextHelper = "Use for Lock/Unlock world object",
                 HeaderText = "Lock Toggle",
                 HeaderTexture2D = "LOCK"
             },
             new M_PrefabApply(START_W, 1, false, adapter)
             {
                 SearchHelper = "Show Prefabs",
                 ContextHelper = "Fast apply prefab changes",
                 HeaderText = "Prefab Button",
                 HeaderTexture2D = "PREF"
             },*/
            
            /*  new M_Audio(START_W, 3, true, adapter)
              {
                  SearchHelper = "Show 'GameObjects' with AudioSource",
                  ContextHelper = "Play AudioClip",
                  HeaderText = "Audio Player",
                  HeaderTexture2D = "AUDIO"
              },
              new M_Tag(48, 4, false, adapter)
              {
                  SearchHelper = "Show 'GameObjects' with Tag",
                  HeaderText = "Tags",
                  ContextHelper = "Witch tag was assigned to object",
              },
              new M_Layers(48, 5, false, adapter) //DISABLE
              {
                  SearchHelper = "Show 'GameObjects' with Layer",
                  HeaderText = "Layers",
                  ContextHelper = "Witch layer was assigned to object"
              },*/
            new M_CustomIcons(32, 6, true, adapter)
            {   //  SearchHelper = "Show GameObjects Which Component With",
                SearchHelper = "Show 'Objects' extension is",
                HeaderText = "*.*",
                ContextHelper = "Custom component icons",
                DRAW_AS_COLUMN = () => !adapter.par.COMPONENTS_NEXT_TO_NAME
            },
            new M_Descript(68, 8, true, adapter)
            {   SearchHelper = "Show 'Objects' with Description",
                HeaderText = "Descriptions",
                ContextHelper = "Short object description",
            },
            /* new M_SpritesOrder(68, 7, false, adapter)
             {
                 SearchHelper = "Show 'GameObjects' with SortingLayer",
                 HeaderText = "Sprites Order",
                 ContextHelper = "SortingLayer and order for sprites",
             },
             new M_PlayModeKeeper(START_W, -1, true, adapter)
             {
                 SearchHelper = "Show 'GameObjects' whose 'Components' will persist in play mode",
                 HeaderText = "PlayMode Data Keeper",
                 ContextHelper = "PlayMode data keeper",
                 HeaderTexture2D = "STORAGE_PASSIVE",
                 disableSib = true
             },*/
            
            new M_UserModulesRoot_Slot1(68, 9, false, adapter)
            {   SearchHelper = "Show 'Objects' whose have Custom Modules parameters",
                HeaderText_pref = "Custom 1",
                ContextHelper_pref = "Custom Module 1",
            } .SetCustomModule(adapter.m1 == null ? null : Activator.CreateInstance(adapter.m1) as CustomModule) as Adapter.Module,
            
            new M_UserModulesRoot_Slot2(68, 10, false, adapter)
            {   SearchHelper = "Show 'Objects' whose have Custom Modules parameters",
                HeaderText_pref = "Custom 2",
                ContextHelper_pref = "Custom Module 2",
            } .SetCustomModule(adapter.m2 == null ? null : Activator.CreateInstance(adapter.m2) as CustomModule)as Adapter.Module,
            new M_UserModulesRoot_Slot3(68, 11, false, adapter)
            {   SearchHelper = "Show 'Objects' whose have Custom Modules parameters",
                HeaderText_pref = "Custom 3",
                ContextHelper_pref = "Custom Module 3",
            } .SetCustomModule(adapter.m3 == null ? null : Activator.CreateInstance(adapter.m3) as CustomModule)as Adapter.Module,
            
            new M_UserModulesRoot_Slot4(68, 12, false, adapter)
            {   SearchHelper = "Show 'Objects' whose have Custom Modules parameters",
                HeaderText_pref = "Custom 4",
                ContextHelper_pref = "Custom Module 4",
            } .SetCustomModule(adapter.m4 == null ? null : Activator.CreateInstance(adapter.m4) as CustomModule)as Adapter.Module,
            new M_UserModulesRoot_Slot5(68, 13, false, adapter)
            {   SearchHelper = "Show 'Objects' whose have Custom Modules parameters",
                HeaderText_pref = "Custom 5",
                ContextHelper_pref = "Custom Module 5",
            } .SetCustomModule(adapter.m5 == null ? null : Activator.CreateInstance(adapter.m5) as CustomModule)as Adapter.Module,
            new M_UserModulesRoot_Slot6(68, 14, false, adapter)
            {   SearchHelper = "Show 'Objects' whose have Custom Modules parameters",
                HeaderText_pref = "Custom 6",
                ContextHelper_pref = "Custom Module 6",
            } .SetCustomModule(adapter.m6 == null ? null : Activator.CreateInstance(adapter.m6) as CustomModule)as Adapter.Module,
            
            new M_UserModulesRoot_Slot7(68, 15, false, adapter)
            {   SearchHelper = "Show 'Objects' whose have Custom Modules parameters",
                HeaderText_pref = "Custom 7",
                ContextHelper_pref = "Custom Module 7",
            } .SetCustomModule(adapter.m7 == null ? null : Activator.CreateInstance(adapter.m7) as CustomModule)as Adapter.Module,
            new M_UserModulesRoot_Slot8(68, 16, false, adapter)
            {   SearchHelper = "Show 'Objects' whose have Custom Modules parameters",
                HeaderText_pref = "Custom 8",
                ContextHelper_pref = "Custom Module 8",
            } .SetCustomModule(adapter.m8 == null ? null : Activator.CreateInstance(adapter.m8) as CustomModule)as Adapter.Module,
            new M_UserModulesRoot_Slot9(68, 17, false, adapter)
            {   SearchHelper = "Show 'Objects' whose have Custom Modules parameters",
                HeaderText_pref = "Custom 9",
                ContextHelper_pref = "Custom Module 9",
            } .SetCustomModule(adapter.m9 == null ? null : Activator.CreateInstance(adapter.m9) as CustomModule)as Adapter.Module,
            
            
        };
        
        var h = Hierarchy_GUI.Instance( adapter );
        h.SortSibligPoses();
        
        
        foreach (var module in adapter.modules)
        {   module.InitializeModule();
        }
        
        adapter.__modulesOrdered = null;
        
        
        // adapter.VerticesModule = adapter.modules.First( m => m is M_Vertices );
    }
    
    
    static void CONTEXTMENU_STATICMODULES(GenericMenu menu)
    {
    }
    
}
}
