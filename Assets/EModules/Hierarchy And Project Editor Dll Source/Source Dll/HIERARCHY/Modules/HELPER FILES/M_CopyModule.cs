

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
    /*struct __CopyPasteS
    {
        internal __CopyPasteS(SerializedProperty[] arr)
        {
            props = arr;
        }
        internal SerializedProperty[] props;
    }*/
    
    //static Dictionary<Type, List<__CopyPasteS>> __copyPastDic = new Dictionary<Type, List<__CopyPasteS>>();
    static Type lastType;
    static void Copy(Component comp)
    {   ComponentUtility.CopyComponent(comp);
        lastType = comp.GetType();
        /*  var s = new SerializedObject(comp);
          var current = s.GetIterator();
          if (current == null) return;
        
          var result = new List<SerializedProperty>();
          while (current.Next(true))
          {
              result.Add(current.Copy());
          }
        
          if (!__copyPastDic.ContainsKey(comp.GetType())) __copyPastDic.Add(comp.GetType(), new List<__CopyPasteS>());
        
          if (__copyPastDic[comp.GetType()].Count > 0)
              __copyPastDic[comp.GetType()][0] = new __CopyPasteS(result.ToArray());
          else
              __copyPastDic[comp.GetType()].Add(new __CopyPasteS(result.ToArray()));*/
        //     PrefabUtility.SetPropertyModifications(newGO, PrefabUtility.GetPropertyModifications(gameObject));
        
    }
    
    static bool PastValidate(Component comp)
    {   return lastType == (comp.GetType());
        // return __copyPastDic.ContainsKey(comp.GetType());
    }
    
    static void Paste(Component comp)
    {   ComponentUtility.PasteComponentValues(comp);
        /* if (!PastValidate(comp)) return;
        
         var s = new SerializedObject(comp);
         //s.
         foreach (var p in __copyPastDic[comp.GetType()].First().props)
         {
            MonoBehaviour.print(p.propertyPath);
             s.CopyFromSerializedProperty(p);
         }
         s.ApplyModifiedProperties();
         s.SetIsDifferentCacheDirty();*/
        
        //     PrefabUtility.SetPropertyModifications(newGO, PrefabUtility.GetPropertyModifications(gameObject));
        
    }
    
    static void PasteComponentAsNew( Component comp )
    {   if ( !comp || !comp.gameObject ) return;
        ComponentUtility.PasteComponentAsNew( comp.gameObject );
        
        
    }
    
    
}
}

