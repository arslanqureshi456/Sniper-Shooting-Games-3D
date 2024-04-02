using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

//namespace EModules

namespace EModules.EModulesInternal {
internal partial class Hierarchy {
    // internal static HierParams par;
    // static Texture2D lightIcon;
    //  static Dictionary<int, GameObject> markedObjects = new Dictionary<int, GameObject>();
    
    /*  static Texture2D _colorText11half;
      internal static Texture2D colorText11half
      {
          get
          {
              if (_colorText11half == null) COnst();
              return _colorText11half;
          }
      }*/
    
    
    //  public static bool DrawLongChesse = false;
    
    //      [MenuItem("INIT/INIT")]
    
    // static bool wasFirstUpdate = false;
    
    
    //   static Module ComponentsModule;
    
    
    /*    private static UndoPropertyModification[] PostprocessModifications(UndoPropertyModification[] modifications)
        {
            MonoBehaviour.print("ASD");
            foreach (var m in modifications)
            {
                MonoBehaviour.print(m.currentValue.value + " " + m.currentValue.objectReference + " " + m.currentValue.propertyPath + " " + m.currentValue.target);
            }
            return modifications;
        }*/
    
    
    /*     internal static int counter = 0;
         static double lastFrame = 0;
         static float interval = 0.5f;
         static void Counter()
         {
             if (lastFrame == 0) lastFrame = EditorApplication.timeSinceStartup;
    
             if (Math.Abs(EditorApplication.timeSinceStartup - lastFrame) > interval)
             {
                 var dif = EditorApplication.timeSinceStartup - lastFrame;
                 var clamp = dif - interval;
                 var disp = Mathf.RoundToInt(counter * (float)((dif - clamp) / dif));
                 counter -= disp;
                 lastFrame += interval;
                 MonoBehaviour.print(disp);
             }
         }*/
    
    
    // static Module[] __modules = null;
    
    // static int DrawHeadind;
    
    //  static double lastEditTime = 0;
    
    // static float shrinkWidth = 150;
    /* static void CurrentRectClear()
        {
            //___CurrentRect.Remove(window());
            ___CurrentRect.Clear();
        }
        static bool CurrentRectContainsKey(EditorWindow w, Module m)
        {
           // if (!___CurrentRect.ContainsKey(w)) return false;
            if (!___CurrentRect.ContainsKey(m)) return false;
            return true;
        }
    
        static void CurrentRectInit(EditorWindow w, Module m, Rect r)
        {
          //  if (!___CurrentRect.ContainsKey(w)) ___CurrentRect.Add(w, new Dictionary<Module, Rect>());
            if (!___CurrentRect.ContainsKey(m)) ___CurrentRect.Add(m, r);
            if (hierarchy_windows.Count > 1) ___CurrentRect[m] = r;
        }
        static void CurrentRectSet(EditorWindow w, Module m, Rect r)
        {
            //MonoBehaviour.print(hierarchy_windows.Count);
          //  if (!___CurrentRect.ContainsKey(w)) ___CurrentRect.Add(w, new Dictionary<Module, Rect>());
            if (!___CurrentRect.ContainsKey(m)) ___CurrentRect.Add(m, r);
            ___CurrentRect[m] = r;
        }
        static Rect CurrentRect(EditorWindow w, Module m)
        {
            //if (!___CurrentRect.ContainsKey(w))___CurrentRect.Add(w, new Dictionary<Module, Rect>());
            return ___CurrentRect[m];
        }*/
    // static  Dictionary<Module, Rect> ___CurrentRect = new Dictionary<EditorWindow, Dictionary<Module, Rect>>();
    
    
    /*  void OnSelectionChange()
      {
          Hierarchy.RepaintAllViews();
      }*/
    
    
    /*  internal static void FadeRectWhite(Rect drawRect, float alpha = 0.6f)
      {
          var defColor = GUI.color;
          var c = Hierarchy.BGColor;
          float ADD = 0.1f;
          if (EditorGUIUtility.isProSkin) ADD = 0.03f;
          c.r += ADD;
          c.g += ADD;
          c.b += ADD;
          c.a = alpha;
          GUI.color = c;
    
          GUI.DrawTexture(drawRect, EditorGUIUtility.whiteTexture);
          GUI.color = defColor;
      }*/
    
    
    /* sealed class Version1ToVersion2DeserializationBinder : SerializationBinder
     {
         public override Type BindToType(string assemblyName, string typeName)
         {
             Type typeToDeserialize = null;
    
             // For each assemblyName/typeName that you want to deserialize to
             // a different type, set typeToDeserialize to the desired type.
             // String currentAssembly = Assembly.GetAssembly(typeof(System.Type)).FullName;
             String currentAssembly = Assembly.GetExecutingAssembly().FullName;
    
             //  String assemVer1 = Assembly.GetExecutingAssembly().FullName;
             // String typeVer1 = "Version1Type";
    
           /*  if (assemblyName == assemVer1 && typeName == typeVer1)
             {
                 // To use a type from a different assembly version,
                 // change the version number.
                 // To do this, uncomment the following line of code.
                 // assemblyName = assemblyName.Replace("1.0.0.0", "2.0.0.0");
    
                 // To use a different type from the same assembly,
                 // change the type name.
                 typeName = "Version2Type";
             }#1#
             currentAssembly = currentAssembly.Replace("2.0.0.0", "1.0.0.0");
    
             // The following line of code returns the type.
             typeToDeserialize = Type.GetType(String.Format("{0}, {1}",
                 typeName, currentAssembly));
    
             return typeToDeserialize;
    
             // return Type.GetType(typeName);
         }
     }*/
    
    
    /* public static Scene LastScene {
         get { return _lastScene; }
         set { _lastScene = value; }
     }*/
    
    /*   [MenuItem("UTILITES/EnableFreezzee")]
       public static void EnableFreezz()
       {
           enableFrezze = !enableFrezze;
           SceneView.RepaintAll();
       }*/
    // static bool enableFrezze = true;
    
    /*  internal static void ToolTip( string message )
    {
    
    if ( m_ToolTip != message ) m_ToolTipTime = 0;
    m_ToolTip = message;
    }
    */
    internal static Adapter HierarchyAdapterInstance
    {   get { return Adapter.HierAdapter; }
    }
    static DoubleList<string, Hierarchy_GUI.CustomIconParams> customIcons { get { return Hierarchy_GUI.Get( HierarchyAdapterInstance ); } }
    //  internal static EModules.HierParams adapter.par { get { return adapter.par; } }
    
    internal static void AttachAdapter( Adapter externalAdapter )    // __HierarchyAdapterInstance = externalAdapter;
    {   externalAdapter.MOI = new MOI_Adapter();
    
        HierarchyAdapterInstance.DefaulTypes = new[]
        {   typeof(M_CustomIcons).FullName, typeof(M_Tag).FullName, typeof(M_Vertices).FullName, //typeof(M_Audio).FullName,
            typeof(M_Descript).FullName
        };
        
    }
    
    
    class MOI_Adapter : IMethodsInterface {
    
        public void InitModules() { Hierarchy.InitModules(); }
        
        public IHashProperty des( int scene ) { return Hierarchy.M_Descript.des( scene ); }
        public IHashProperty des( Scene scene ) { return Hierarchy.M_Descript.des( scene.GetHashCode() ); }
        
        
        public void RegistrateDescription( IDescriptionRegistrator o ) { HierarchyAdapterInstance.DescriptionModule.RegistrateDescription( o, Hierarchy.HierarchyAdapterInstance ); }
        
        public void CONTEXTMENU_STATICMODULES( GenericMenu menu ) { Hierarchy.CONTEXTMENU_STATICMODULES( menu ); }
        
        
        IModuleOnnector_M_Vertices m_M_Vertices;
        public IModuleOnnector_M_Vertices M_Vertices
        {   get { return (m_M_Vertices ?? (m_M_Vertices = Hierarchy.HierarchyAdapterInstance.modules.First( m => m is IModuleOnnector_M_Vertices ) as IModuleOnnector_M_Vertices)); }
        }
        
        //         Adapter.M_Colors m_M_Colors;
        //         public Adapter.M_Colors M_Colors
        //         {   get { return (m_M_Colors ?? (m_M_Colors = Hierarchy.HierarchyAdapterInstance.modules.First( m => m is Adapter.M_Colors ) as Adapter.M_Colors)); }
        //         }
        
        //  IModuleOnnector_M_CustomIcons m_M_CustomIcons;
        /*  public IModuleOnnector_M_CustomIcons M_CustomIcons
          {   get { return (m_M_CustomIcons ?? (m_M_CustomIcons = Hierarchy.HierarchyAdapterInstance.modules.First( m => m is IModuleOnnector_M_CustomIcons ) as IModuleOnnector_M_CustomIcons)); }
          }*/
        
        IModuleOnnector_M_PlayModeKeeper m_M_PlayModeKeeper;
        public IModuleOnnector_M_PlayModeKeeper M_PlayModeKeeper
        {   get { return (m_M_PlayModeKeeper ?? (m_M_PlayModeKeeper = Hierarchy.HierarchyAdapterInstance.modules.First( m => m is IModuleOnnector_M_PlayModeKeeper ) as IModuleOnnector_M_PlayModeKeeper)); }
        }
        
        /*  IModuleOnnector_M_Decription m_M_Descript;
          public IModuleOnnector_M_Decription M_Descript
          {   get { return (m_M_Descript ?? (m_M_Descript = Hierarchy.adapter.modules.First( m => m is IModuleOnnector_M_Decription ) as IModuleOnnector_M_Decription)); }
          }*/
        
        IModuleOnnector_M_Freeze m_M_Freeze;
        public IModuleOnnector_M_Freeze M_Freeze
        {   get { return (m_M_Freeze ?? (m_M_Freeze = Hierarchy.HierarchyAdapterInstance.modules.First( m => m is IModuleOnnector_M_Freeze ) as IModuleOnnector_M_Freeze)); }
        }
    }
    
    
    /*  class MOI_IModuleOnnector_M_Vertices : IModuleOnnector_M_Vertices
      {
          public void Clear( ) { M_Vertices.Clear(); }
          public Dictionary<int , double> updateTimer { get { return M_Vertices.GetDescript().updateTimer; } }
          public void CalcBroadCast( ) { M_Vertices.CalcBroadCast(); }
      }
    
      class MOI_IModuleOnnector_M_Color : IModuleOnnector_M_Color
      {
          public TempColorClass needdrawGetColor( GameObject activeGameObject ) { return M_Colors.needdrawGetColor( activeGameObject ); }
          public Color GetColorForObject( GameObject activeGameObject ) { return M_Colors.GetColorForObject( activeGameObject ); }
          public ObjectCacheHelper<GameObject , SingleList> IconColorCacher { get { M_Colors.IconColorCacher; } }
          public void SetValue( TempColorClass c , bool b , Scene scene , GameObject needRestoreGameObjectName ) { M_Colors.SetValue( c , b , scene , needRestoreGameObjectName ); }
          public string GetValueToString( Scene scene , GameObject needRestoreGameObjectName ) { M_Colors.GetValueToString( scene , needRestoreGameObjectName ); }
          public void DrawBackground( Rect selectionRect , GameObject o ) { M_Colors.DrawBackground( selectionRect , o ); }
      }
    
      class MOI_IModuleOnnector_M_CustomIcons : IModuleOnnector_M_CustomIcons
      {
          public EventType? useEvent
          {
              get { return M_CustomIcons.useEvent; }
              set { M_CustomIcons.useEvent = value; }
          }
          public Dictionary<int , double> updateTimer { get { return M_CustomIcons.updateTimer; } }
      }
    
      class MOI_IModuleOnnector_M_PlayModeKeeper : IModuleOnnector_M_PlayModeKeeper { }
      class MOI_IModuleOnnector_M_Freeze : IModuleOnnector_M_Freeze { }*/
    
    
    
    
    static int START_W = 16;
    
    
    static M_PlayModeKeeper __get_keeper;
    static M_PlayModeKeeper GET_KEEPER
    {   get
        {   return __get_keeper ?? (__get_keeper = new M_PlayModeKeeper( START_W, -1, true, HierarchyAdapterInstance )
            {   SearchHelper = "Show 'GameObjects' whose 'Components' will persist in play mode",
                    HeaderText = "PlayMode Data Keeper",
                    ContextHelper = "PlayMode data keeper",
                    HeaderTexture2D = "STORAGE_PASSIVE",
                    disableSib = true
            });
        }
    }
    
    
    
    
    internal static void InitModules()
    {   if ( HierarchyAdapterInstance.wasModulesInitialize ) return;
        HierarchyAdapterInstance.wasModulesInitialize = true;
        //     MonoBehaviour.print("ASD");
        
        
        /*  var ComponentsModule = ;*/
        
        HierarchyAdapterInstance.modules = new Adapter.Module[]
        {   new M_SetActive(START_W, -1, true, HierarchyAdapterInstance)
            {   // SearchHelper = "Show 'GameObjects' whose 'Components' will persist in play mode",
                HeaderText = "SetActive GameObject",
                ContextHelper = "SetActive GameObject",
                // HeaderTexture2D = "STORAGE_PASSIVE",
                // disableSib = true
            },
            HierarchyAdapterInstance.ColorModule ?? new Adapter.M_Colors(START_W, -1, true, HierarchyAdapterInstance)
            {   SearchHelper = "Show 'GameObjects' with",
                ContextHelper = "Change GameObject Icon"
            },
            new M_Warning(START_W, -1, false, HierarchyAdapterInstance),
            new M_Freeze(START_W, 0, false, HierarchyAdapterInstance)
            {   SearchHelper = "Show Locked 'GameObjects'",
                ContextHelper = "Use for Lock/Unlock world object",
                HeaderText = "Lock Toggle",
                HeaderTexture2D = "LOCK"
            },
            new M_PrefabApply(START_W, 1, false, HierarchyAdapterInstance)
            {   SearchHelper = "Show Prefabs",
                ContextHelper = "Fast apply prefab changes",
                HeaderText = "Prefab Button",
                HeaderTexture2D = "PREF"
            },
            new M_Vertices(START_W * 2, 2, SystemInfo.processorFrequency < 3000 ? false : true, HierarchyAdapterInstance)
            {   SearchHelper = "Show 'GameObjects' Optimizer",
                ContextHelper = "Memory Info",
                HeaderText = "Memory Info",
                HeaderTexture2D = "TRI"
            },
            new M_Audio(START_W, 3, false, HierarchyAdapterInstance)
            {   SearchHelper = "Show 'GameObjects' with AudioSource",
                ContextHelper = "Play AudioClip",
                HeaderText = "Audio Player",
                HeaderTexture2D = "AUDIO"
            },
            new M_Tag(START_W * 2, 4, true, HierarchyAdapterInstance)
            {   SearchHelper = "Show 'GameObjects' with Tag",
                HeaderText = "Tags",
                ContextHelper = "Which tag was assigned to object",
            },
            new M_Layers(48, 5, false, HierarchyAdapterInstance) //DISABLE
            {   SearchHelper = "Show 'GameObjects' with Layer",
                HeaderText = "Layers",
                ContextHelper = "Which layer was assigned to object"
            },
            HierarchyAdapterInstance.M_CustomIconsModule ??  new M_CustomIcons(48, 6, true, HierarchyAdapterInstance)
            {   //  SearchHelper = "Show GameObjects Which Component With",
                SearchHelper = "Show 'GameObjects' whose 'Components' include",
                HeaderText = "Components",
                ContextHelper = "Custom component icons",
                DRAW_AS_COLUMN = () => !HierarchyAdapterInstance.par.COMPONENTS_NEXT_TO_NAME
            },
            new M_Descript(68, 8, true, HierarchyAdapterInstance)
            {   SearchHelper = "Show 'GameObjects' with Description",
                HeaderText = "Descriptions",
                ContextHelper = "Short object description",
            },
            new M_SpritesOrder(68, 7, false, HierarchyAdapterInstance)
            {   SearchHelper = "Show 'GameObjects' with SortingLayer",
                HeaderText = "Sprites Order",
                ContextHelper = "SortingLayer and order for sprites",
            },
            GET_KEEPER,
            
            new M_UserModulesRoot_Slot1(68, 9, false, HierarchyAdapterInstance)
            {   SearchHelper = "Show 'GameObjects' whose have Custom Modules parameters",
                HeaderText_pref = "Custom 1",
                ContextHelper_pref = "Custom Module 1",
            } .SetCustomModule(HierarchyAdapterInstance.m1 == null ? null : Activator.CreateInstance(HierarchyAdapterInstance.m1) as CustomModule) as Adapter.Module,
            
            new M_UserModulesRoot_Slot2(68, 10, false, HierarchyAdapterInstance)
            {   SearchHelper = "Show 'GameObjects' whose have Custom Modules parameters",
                HeaderText_pref = "Custom 2",
                ContextHelper_pref = "Custom Module 2",
            } .SetCustomModule(HierarchyAdapterInstance.m2 == null ? null : Activator.CreateInstance(HierarchyAdapterInstance.m2) as CustomModule)as Adapter.Module,
            new M_UserModulesRoot_Slot3(68, 11, false, HierarchyAdapterInstance)
            {   SearchHelper = "Show 'GameObjects' whose have Custom Modules parameters",
                HeaderText_pref = "Custom 3",
                ContextHelper_pref = "Custom Module 3",
            } .SetCustomModule(HierarchyAdapterInstance.m3 == null ? null : Activator.CreateInstance(HierarchyAdapterInstance.m3) as CustomModule)as Adapter.Module,
            
            new M_UserModulesRoot_Slot4(68, 12, false, HierarchyAdapterInstance)
            {   SearchHelper = "Show 'GameObjects' whose have Custom Modules parameters",
                HeaderText_pref = "Custom 4",
                ContextHelper_pref = "Custom Module 4",
            } .SetCustomModule(HierarchyAdapterInstance.m4 == null ? null : Activator.CreateInstance(HierarchyAdapterInstance.m4) as CustomModule)as Adapter.Module,
            new M_UserModulesRoot_Slot5(68, 13, false, HierarchyAdapterInstance)
            {   SearchHelper = "Show 'GameObjects' whose have Custom Modules parameters",
                HeaderText_pref = "Custom 5",
                ContextHelper_pref = "Custom Module 5",
            } .SetCustomModule(HierarchyAdapterInstance.m5 == null ? null : Activator.CreateInstance(HierarchyAdapterInstance.m5) as CustomModule)as Adapter.Module,
            new M_UserModulesRoot_Slot6(68, 14, false, HierarchyAdapterInstance)
            {   SearchHelper = "Show 'GameObjects' whose have Custom Modules parameters",
                HeaderText_pref = "Custom 6",
                ContextHelper_pref = "Custom Module 6",
            } .SetCustomModule(HierarchyAdapterInstance.m6 == null ? null : Activator.CreateInstance(HierarchyAdapterInstance.m6) as CustomModule)as Adapter.Module,
            
            new M_UserModulesRoot_Slot7(68, 15, false, HierarchyAdapterInstance)
            {   SearchHelper = "Show 'GameObjects' whose have Custom Modules parameters",
                HeaderText_pref = "Custom 7",
                ContextHelper_pref = "Custom Module 7",
            } .SetCustomModule(HierarchyAdapterInstance.m7 == null ? null : Activator.CreateInstance(HierarchyAdapterInstance.m7) as CustomModule)as Adapter.Module,
            new M_UserModulesRoot_Slot8(68, 16, false, HierarchyAdapterInstance)
            {   SearchHelper = "Show 'GameObjects' whose have Custom Modules parameters",
                HeaderText_pref = "Custom 8",
                ContextHelper_pref = "Custom Module 8",
            } .SetCustomModule(HierarchyAdapterInstance.m8 == null ? null : Activator.CreateInstance(HierarchyAdapterInstance.m8) as CustomModule)as Adapter.Module,
            new M_UserModulesRoot_Slot9(68, 17, false, HierarchyAdapterInstance)
            {   SearchHelper = "Show 'GameObjects' whose have Custom Modules parameters",
                HeaderText_pref = "Custom 9",
                ContextHelper_pref = "Custom Module 9",
            } .SetCustomModule(HierarchyAdapterInstance.m9 == null ? null : Activator.CreateInstance(HierarchyAdapterInstance.m9) as CustomModule)as Adapter.Module,
        };
        
        
        var h = Hierarchy_GUI.Instance( HierarchyAdapterInstance );
        h.SortSibligPoses();
        
        
        foreach ( var module in HierarchyAdapterInstance.modules )
        {   module.InitializeModule();
        }
        
        HierarchyAdapterInstance.VerticesModule = HierarchyAdapterInstance.modules.First( m => m is M_Vertices );
        
        
        HierarchyAdapterInstance.__modulesOrdered = null;
    }
    
    
    
    
    static void CONTEXTMENU_STATICMODULES( GenericMenu menu )
    {   menu.AddSeparator( "" );
    
    
        GUIContent cont = null;
        /*  if ( HierarchyAdapterInstance.par.DataKeeperParams.ENABLE )
              cont = new GUIContent( HierarchyAdapterInstance.modules.First( m => m is M_PlayModeKeeper ).HeaderText.ToString() );
          else*/
        cont = new GUIContent( "- " + HierarchyAdapterInstance.modules.First( m => m is M_PlayModeKeeper ).HeaderText.ToString() +  " -" );
        menu.AddItem( cont, HierarchyAdapterInstance.par.DataKeeperParams.ENABLE, () =>
        {   HierarchyAdapterInstance.par.DataKeeperParams.ENABLE = !HierarchyAdapterInstance.par.DataKeeperParams.ENABLE;
            HierarchyAdapterInstance.SavePrefs();
        } );
        
        /* if ( HierarchyAdapterInstance.par.DataKeeperParams.ENABLE )
             cont = new GUIContent( HierarchyAdapterInstance.modules.First( m => m is M_SetActive ).HeaderText.ToString() );
         else*/
        cont = new GUIContent( "- " + HierarchyAdapterInstance.modules.First( m => m is M_SetActive ).HeaderText.ToString() + " -" );
        menu.AddItem( cont, HierarchyAdapterInstance.par.ENABLE_ACTIVEGMAOBJECTMODULE, () =>
        {   HierarchyAdapterInstance.par.ENABLE_ACTIVEGMAOBJECTMODULE = !HierarchyAdapterInstance.par.ENABLE_ACTIVEGMAOBJECTMODULE;
            HierarchyAdapterInstance.SavePrefs();
        } );
        if (!HierarchyAdapterInstance.par.ENABLE_ACTIVEGMAOBJECTMODULE )
        {   cont = new GUIContent( "- SetActive Module Style" );
            menu.AddDisabledItem( cont );
        }
        else
        {   cont = new GUIContent( "- SetActive Module Style/Left Small" );
            menu.AddItem( cont, HierarchyAdapterInstance.SETACTIVE_POSITION == 1, () =>
            {   HierarchyAdapterInstance.SETACTIVE_POSITION = 1;
                HierarchyAdapterInstance.SavePrefs();
            } );
            
            cont = new GUIContent( "- SetActive Module Style/Right Default" );
            menu.AddItem( cont, HierarchyAdapterInstance.SETACTIVE_POSITION == 0, () =>
            {   HierarchyAdapterInstance.SETACTIVE_POSITION = 0;
                HierarchyAdapterInstance.SavePrefs();
            } );
            
            cont = new GUIContent( "- SetActive Module Style/Right Small" );
            menu.AddItem( cont, HierarchyAdapterInstance.SETACTIVE_POSITION == 2, () =>
            {   HierarchyAdapterInstance.SETACTIVE_POSITION = 2;
                HierarchyAdapterInstance.SavePrefs();
            } );
            
            menu.AddSeparator( "- SetActive Module Style/" );
            
            cont = new GUIContent( "- SetActive Module Style/Contrast Style" );
            menu.AddItem( cont, HierarchyAdapterInstance.SETACTIVE_STYLE == 1, () =>
            {   HierarchyAdapterInstance.SETACTIVE_STYLE = 1 - HierarchyAdapterInstance.SETACTIVE_STYLE ;
                HierarchyAdapterInstance.SavePrefs();
            } );
        }
        
    }
    
    
    /*   private static void GUIOver( )
       {
           /* if (m_ToolTipTime > 1)
            {
                r.Set(Event.current.mousePosition.x, Event.current.mousePosition.y, 50, EditorGUIUtility.singleLineHeight);
                Adapter. INTERNAL_BOX(r, m_ToolTip);
            }
            //MonoBehaviour.print(m_ToolTip);
            if (m_ToolTip == "") m_ToolTipTime = 0;
            m_ToolTip = "";#1#
       }*/
    
    
    /*    private static float? _GET_PADING;
        private static float GET_PADING {
            get {
                if (_GET_PADING == null) INIT_PADING();
                return _GET_PADING.Value;
            }
        }*/
    
    
    /*float[] currentX;
    int dragIndex;
    void UpdatePositions()
    {
        if (Event.current.type == EventType.repaint && currentX.Length != 0)
        {
            var tempDragIndex = dragIndex == -1 ? -1 : Mathf.Clamp(Mathf.RoundToInt((Event.current.mousePosition.y - MouseY - H / 2) / (float)H), 0, currentX.Length - 1);
    
            // MonoBehaviour.print(tempDragIndex);
            for (int i = 0, sib = 0; i < currentX.Length; i++, sib++)
            {
                // if (tempDragIndex == i && i > dragIndex) sib--;
                //if (tempDragIndex == i && i < dragIndex) sib++;
                if (dragIndex != -1 && i > dragIndex && i <= tempDragIndex) sib = i - 1;
                else if (dragIndex != -1 && i < dragIndex && i >= tempDragIndex) sib = i + 1;
                else sib = i;
                currentX[i] = Mathf.Lerp(currentX[i], sib * H, 0.5f);
            }
            Hierarchy.RepaintAllView();
            //if (dragIndex != -1)
        }
    }*/
    
    
    /* internal static DoubleList<string , Hierarchy_GUI.CustomIconParams> customIcons
     {
         get { return Hierarchy_GUI.Get( adapter ); }
     }*/
    
    
    /* class BindChanger : System.Runtime.Serialization.SerializationBinder
     {
         public override Type BindToType(string assemblyName, string typeName)
         {
             Type returntype = null;
             assemblyName = Assembly.GetExecutingAssembly().FullName;
             returntype =
                 Type.GetType(String.Format("{0}, {1}",
                 typeName, assemblyName));
             return returntype;
         }
     }*/
    
    private sealed class AllowAllAssemblyVersionsDeserializationBinder :
        System.Runtime.Serialization.SerializationBinder {
        public override Type BindToType( string assemblyName, string typeName )
        {   Type typeToDeserialize = null;
        
            String currentAssembly = Assembly.GetExecutingAssembly().FullName;
            
            // In this case we are always using the current assembly
            assemblyName = currentAssembly;
            
            // Get the type using the typeName and assemblyName
            typeToDeserialize = Type.GetType( String.Format( "{0}, {1}", typeName, assemblyName ) );
            
            return typeToDeserialize;
        }
    }
    
    
    //  static float chessWidth = 0;
    /*  {
    
              return __modules;
          }
      }*/
    
    
    /*   public static void DrawComponnentsIcon(Component component, Texture2D icon)
       {
           if (customIcons.ContainsKey(component)) customIcons[component] = icon;
           else customIcons.Add(component, icon);
       }*/
}
}