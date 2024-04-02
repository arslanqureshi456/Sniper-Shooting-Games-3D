#if UNITY_EDITOR
    #define HIERARCHY
    #define PROJECT
#endif

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


namespace EModules.EModulesInternal {

public class _S___Project_GUI2 : EditorWindow {
    public void OnGUI()
    {   if ( Adapter.ProjAdapter == null ) return;
        Adapter.ProjAdapter.Legacy_SetDraver( this );
    }
    
    
    internal const int P = 2000;
    internal static void showWindow()
    {   EditorWindow.GetWindow<_S___Project_GUI2>();
    }
}
}
