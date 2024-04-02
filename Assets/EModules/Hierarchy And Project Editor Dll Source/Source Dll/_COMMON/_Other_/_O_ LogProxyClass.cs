using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
#if PROJECT
    using EModules.Project;
#endif


namespace EModules.EModulesInternal

{
internal partial class Adapter {
    internal class LogProxy {
        // Adapter adapter;
        internal LogProxy(Adapter adapter)
        {
        
        
            // this.adapter = adapter;
            // #if !UNITY_EDITOR
            EditorApplication.update += () =>
            {   // #endif
            
                if (messages.Count != 0)
                {   foreach (var kp in messages.ToDictionary(k => k.Key, v => v.Value))
                    {   var message = "[" + adapter.pluginname + " Plugin] " + kp.Key;
                        var type = kp.Value;
                        switch (type)
                        {   case LogType.Error:
                                Debug.LogError(message);
                                adapter.RESET_MODULE();
                                break;
                            case LogType.Assert:
                                Debug.LogAssertion(message);
                                break;
                            case LogType.Warning:
                                Debug.LogWarning(message);
                                break;
                            case LogType.Log:
                                Debug.Log(message);
                                break;
                            //case LogType.Exception:
                            default:
                                throw new ArgumentOutOfRangeException("type", type, null);
                        }
                    }
                    messages.Clear();
                }
                //  #if !UNITY_EDITOR
            };
            //   #endif
        }
        
        Dictionary<string, LogType> messages = new Dictionary<string, LogType>();
        
        internal void LogError(string message)
        {
            #if UNITY_EDITOR
            Debug.LogError( message );
            #endif
            
            if (messages.ContainsKey(message)) return;
            messages.Add(message, LogType.Error);
        }
        internal void LogAssertion(string message)
        {   if (messages.ContainsKey(message)) return;
            messages.Add(message, LogType.Assert);
        }
        internal void LogWarning(string message)
        {   if (messages.ContainsKey(message)) return;
            messages.Add(message, LogType.Warning);
        }
        internal void Log(string message)
        {   if (messages.ContainsKey(message)) return;
            messages.Add(message, LogType.Log);
        }
    }
}
}
