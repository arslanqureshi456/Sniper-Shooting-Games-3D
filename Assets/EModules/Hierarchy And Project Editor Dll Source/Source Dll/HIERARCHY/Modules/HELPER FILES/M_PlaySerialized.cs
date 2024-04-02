using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine.SceneManagement;

//namespace EModules

namespace EModules.EModulesInternal {
internal partial class Hierarchy {

    // check enum, arrays, objects
    
    internal partial  class M_PlayModeKeeper : Adapter.Module {
        static Action applyOnUpdate;
        
        static Dictionary<string, bool> defaultTypes = new Dictionary<string, bool>();
        
        //if(typeof(Component).IsAssignableFrom(value.GetType())) throw new System.InvalidOperationException("Cannot serialize classes derived from Component!");
        static BinaryFormatter formatter;
        static void Initialize()
        {   //  JsonUtility.
            /* var so = new SerializedObject(null);
             so.FindProperty("").ob*/
            formatter = new BinaryFormatter();
            
            SurrogateSelector ss = new SurrogateSelector();
            
            // Tell the SurrogateSelector that Employee objects are serialized and deserialized
            // using the EmployeeSerializationSurrogate object.
            /*     ss.AddSurrogate(typeof(UnityEngine.Object), new StreamingContext(StreamingContextStates.All), new SSUnityObject());
                 ss.AddSurrogate(typeof(UnityEngine.Component), new StreamingContext(StreamingContextStates.All), new SSUnityObject());
                 ss.AddSurrogate(typeof(UnityEngine.MonoBehaviour), new StreamingContext(StreamingContextStates.All), new SSUnityObject());
                 ss.AddSurrogate(typeof(UnityEngine.TextAsset), new StreamingContext(StreamingContextStates.All), new SSUnityObject());
                 ss.AddSurrogate(typeof(MonoScript), new StreamingContext(StreamingContextStates.All), new SSUnityObject());
                 ss.AddSurrogate(typeof(GameObject), new StreamingContext(StreamingContextStates.All), new SSUnityObject());
                 ss.AddSurrogate(typeof(Transform), new StreamingContext(StreamingContextStates.All), new SSUnityObject());*/
            
            //MonoBehaviour.print(typeof(UnityEngine.Object).Assembly.GetTypes().Where(t => typeof(UnityEngine.Object).IsAssignableFrom(t)).Count());
            /* foreach (var t in typeof(UnityEngine.Object).Assembly.GetTypes().Where(t => typeof(UnityEngine.Object).IsAssignableFrom(t)))
                 ss.AddSurrogate(t, new StreamingContext(StreamingContextStates.All), new SSUnityObject());
             foreach (var t in typeof(MonoScript).Assembly.GetTypes().Where(t => typeof(UnityEngine.Object).IsAssignableFrom(t)))
                 ss.AddSurrogate(t, new StreamingContext(StreamingContextStates.All), new SSUnityObject());*/
            defaultTypes.Clear();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {   foreach (var t in assembly.GetTypes().Where(t => typeof(UnityEngine.Object).IsAssignableFrom(t)))
                    ss.AddSurrogate(t, new StreamingContext(StreamingContextStates.All), new SSUnityObject());
            }
            
            /*  ss.AddSurrogate(typeof(AnimationCurve), new StreamingContext(StreamingContextStates.All), new SSAnimationCurve());*/
            ss.AddSurrogate(typeof(Color), new StreamingContext(StreamingContextStates.All), new SSColor());
            ss.AddSurrogate(typeof(Color32), new StreamingContext(StreamingContextStates.All), new SSColor32());
            ss.AddSurrogate(typeof(Enum), new StreamingContext(StreamingContextStates.All), new SSEnum());
            ss.AddSurrogate(typeof(Quaternion), new StreamingContext(StreamingContextStates.All), new SSQuaternion());
            ss.AddSurrogate(typeof(Rect), new StreamingContext(StreamingContextStates.All), new SSRect());
            ss.AddSurrogate(typeof(RectOffset), new StreamingContext(StreamingContextStates.All), new SSRectOffset());
            ss.AddSurrogate(typeof(Matrix4x4), new StreamingContext(StreamingContextStates.All), new SSMatrix4x4());
            ss.AddSurrogate(typeof(Vector2), new StreamingContext(StreamingContextStates.All), new SSVector2());
            ss.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), new SSVector3());
            ss.AddSurrogate(typeof(Vector4), new StreamingContext(StreamingContextStates.All), new SSVector4());
            
            
            // Associate the SurrogateSelector with the BinaryFormatter.
            formatter.SurrogateSelector = ss;
            //formatter.TypeFormat = FormatterTypeStyle.TypesWhenNeeded;
            
            
            // Create a SurrogateSelector.
            
            
            
            
            
        }
        
        /* enum asdasd
         {
         }*/
        /*  void SERIALIZE_BY_FORMATTER(object ob)
          {
              using (Stream stream = new MemoryStream())
              {
                  try
                  {
                      // Serialize an Employee object into the memory stream.
                      formatter.Serialize(stream, ob);
                      formatter.
                       }
                  catch (SerializationException e)
                  {
                      Console.WriteLine("Serialization failed: {0}", e.Message);
                      throw;
                  }
              }
        
              // Rewind the MemoryStream.
              //stream.Position = 0;
              using (Stream stream = new MemoryStream())
              {
                  try
                  {
                      // Deserialize the Employee object from the memory stream.
                      var emp = (object)formatter.Deserialize(stream);
        
                  }
                  catch (SerializationException e)
                  {
                      Console.WriteLine("Deserialization failed: {0}", e.Message);
                      throw;
                  }
              }
          }*/
        
        
        sealed class SSUnityObject : ISerializationSurrogate {
            internal SSUnityObject()
            {   var type = this.GetType().FullName;
                if (!defaultTypes.ContainsKey(type)) defaultTypes.Add(type, true);
            }
            public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
            {   var emp = (UnityEngine.Object)obj;
                info.AddValue("id", emp.GetInstanceID());
            }
            
            public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
            {   var emp = EditorUtility.InstanceIDToObject(info.GetInt32("id"));
                return emp;
            }
        }
        /*   sealed class SSAnimationCurve : ISerializationSurrogate
           {
               public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
               {
                   var curve = (AnimationCurve)obj;
                   var keys = curve.keys;
                   var result = new StringBuilder();
                   for (int i = 0; i < keys.Length; i++)
                   {
                       result.AppendLine(keys[i].value.ToString());
                       result.AppendLine(keys[i].inTangent.ToString());
                       result.AppendLine(keys[i].outTangent.ToString());
                       result.AppendLine(keys[i].tangentMode.ToString());
                       result.AppendLine(keys[i].time.ToString());
                   }
                   info.AddValue("value", result.ToString());
                   info.AddValue("postWrapMode", curve.postWrapMode.ToString());
                   info.AddValue("preWrapMode", curve.preWrapMode.ToString());
        
               }
        
               public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
               {
                  // EditorJsonUtility. !!!!!!!!!!!!
        
                   var emp = obj as AnimationCurve;
                   if (emp == null) emp = new AnimationCurve();
                 //  var preWrapMode = (WrapMode)Enum.Parse(typeof(WrapMode), info.GetString("preWrapMode"));
                 //  var postWrapMode = (WrapMode)Enum.Parse(typeof(WrapMode), info.GetString("postWrapMode"));
        
                   using (TextReader reader = new StringReader(info.GetString("value")))
                   {
                       var keys = new List<Keyframe>();
                       string line;
                       while ((line = reader.ReadLine()) != null)
                       {
                           keys.Add(new Keyframe() {
                               value = float.Parse(line),
                               inTangent = float.Parse(reader.ReadLine()),
                               outTangent = float.Parse(reader.ReadLine()),
                               tangentMode = int.Parse(reader.ReadLine()),
                               time = float.Parse(reader.ReadLine())
                           });
                       }
                       applyOnUpdate += () => {
                         / *  emp.preWrapMode = preWrapMode;
                           emp.postWrapMode = postWrapMode;* /
                           emp.keys = keys.ToArray();
        
                       };
                   }
                   return emp;
               }
           }*/
        sealed class SSColor : ISerializationSurrogate {
            internal SSColor()
            {   var type = this.GetType().FullName;
                if (!defaultTypes.ContainsKey(type)) defaultTypes.Add(type, true);
            }
            public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
            {   var emp = (Color)obj;
                info.AddValue("r", emp.r);
                info.AddValue("g", emp.g);
                info.AddValue("b", emp.b);
                info.AddValue("a", emp.a);
            }
            
            public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
            {   var emp = (Color)obj;
                emp.r = info.GetSingle("r");
                emp.g = info.GetSingle("g");
                emp.b = info.GetSingle("b");
                emp.a = info.GetSingle("a");
                return emp;
            }
        }
        
        sealed class SSColor32 : ISerializationSurrogate {
            internal SSColor32()
            {   var type = this.GetType().FullName;
                if (!defaultTypes.ContainsKey(type)) defaultTypes.Add(type, true);
            }
            public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
            {   var emp = (Color32)obj;
                info.AddValue("r", emp.r);
                info.AddValue("g", emp.g);
                info.AddValue("b", emp.b);
                info.AddValue("a", emp.a);
            }
            
            public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
            {   var emp = (Color32)obj;
                emp.r = info.GetByte("r");
                emp.g = info.GetByte("g");
                emp.b = info.GetByte("b");
                emp.a = info.GetByte("a");
                return emp;
            }
        }
        
        sealed class SSEnum : ISerializationSurrogate {
            internal SSEnum()
            {   var type = this.GetType().FullName;
                if (!defaultTypes.ContainsKey(type)) defaultTypes.Add(type, true);
            }
            public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
            {   var type = obj.GetType().FullName;
                var str = obj.ToString();
                info.AddValue("type", type);
                info.AddValue("value", str);
            }
            
            public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
            {   try
                {   var gettedType = Type.GetType(info.GetString("type"));
                    if (gettedType != null)
                    {   var value = info.GetString("value");
                        var parse = Enum.Parse(gettedType, value);
                        return parse;
                    }
                }
                catch { }
                return obj;
            }
        }
        
        sealed class SSQuaternion : ISerializationSurrogate {
            internal SSQuaternion()
            {   var type = this.GetType().FullName;
                if (!defaultTypes.ContainsKey(type)) defaultTypes.Add(type, true);
            }
            public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
            {   var emp = (Quaternion)obj;
                info.AddValue("x", emp.x);
                info.AddValue("y", emp.y);
                info.AddValue("z", emp.z);
                info.AddValue("w", emp.w);
            }
            
            public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
            {   var emp = (Quaternion)obj;
                emp.x = info.GetSingle("x");
                emp.y = info.GetSingle("y");
                emp.z = info.GetSingle("z");
                emp.w = info.GetSingle("w");
                return emp;
            }
        }
        
        sealed class SSRect : ISerializationSurrogate {
            internal SSRect()
            {   var type = this.GetType().FullName;
                if (!defaultTypes.ContainsKey(type)) defaultTypes.Add(type, true);
            }
            public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
            {   var emp = (Rect)obj;
                info.AddValue("x", emp.x);
                info.AddValue("y", emp.y);
                info.AddValue("z", emp.width);
                info.AddValue("w", emp.height);
            }
            
            public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
            {   var emp = (Rect)obj;
                emp.x = info.GetSingle("x");
                emp.y = info.GetSingle("y");
                emp.width = info.GetSingle("z");
                emp.height = info.GetSingle("w");
                return emp;
            }
        }
        
        sealed class SSRectOffset : ISerializationSurrogate {
            internal SSRectOffset()
            {   var type = this.GetType().FullName;
                if (!defaultTypes.ContainsKey(type)) defaultTypes.Add(type, true);
            }
            public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
            {   var emp = (RectOffset)obj;
                info.AddValue("x", emp.bottom);
                info.AddValue("y", emp.left);
                info.AddValue("z", emp.right);
                info.AddValue("w", emp.top);
            }
            
            public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
            {
            
                var emp = new RectOffset();
                emp.bottom = (int)info.GetValue("x", typeof(int));
                emp.left = info.GetInt32("y");
                emp.right = info.GetInt32("z");
                emp.top = info.GetInt32("w");
                return emp;
            }
        }
        
        sealed class SSMatrix4x4 : ISerializationSurrogate {
            internal SSMatrix4x4()
            {   var type = this.GetType().FullName;
                if (!defaultTypes.ContainsKey(type)) defaultTypes.Add(type, true);
            }
            public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
            {   var emp = (Matrix4x4)obj;
                info.AddValue("m00", emp.m00);
                info.AddValue("m01", emp.m01);
                info.AddValue("m02", emp.m02);
                info.AddValue("m03", emp.m03);
                
                info.AddValue("m10", emp.m10);
                info.AddValue("m11", emp.m11);
                info.AddValue("m12", emp.m12);
                info.AddValue("m13", emp.m13);
                
                info.AddValue("m20", emp.m20);
                info.AddValue("m21", emp.m21);
                info.AddValue("m22", emp.m22);
                info.AddValue("m23", emp.m23);
                
                info.AddValue("m30", emp.m30);
                info.AddValue("m31", emp.m31);
                info.AddValue("m32", emp.m32);
                info.AddValue("m33", emp.m33);
            }
            
            public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
            {   var emp = (Matrix4x4)obj;
                emp.m00 = info.GetSingle("m00");
                emp.m01 = info.GetSingle("m01");
                emp.m02 = info.GetSingle("m02");
                emp.m03 = info.GetSingle("m03");
                
                emp.m10 = info.GetSingle("m10");
                emp.m11 = info.GetSingle("m11");
                emp.m12 = info.GetSingle("m12");
                emp.m13 = info.GetSingle("m13");
                
                emp.m20 = info.GetSingle("m20");
                emp.m21 = info.GetSingle("m21");
                emp.m22 = info.GetSingle("m22");
                emp.m23 = info.GetSingle("m23");
                
                emp.m30 = info.GetSingle("m30");
                emp.m31 = info.GetSingle("m31");
                emp.m32 = info.GetSingle("m32");
                emp.m33 = info.GetSingle("m33");
                return emp;
            }
        }
        
        sealed class SSVector2 : ISerializationSurrogate {
            internal SSVector2()
            {   var type = this.GetType().FullName;
                if (!defaultTypes.ContainsKey(type)) defaultTypes.Add(type, true);
            }
            public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
            {   var emp = (Vector2)obj;
                info.AddValue("x", emp.x);
                info.AddValue("y", emp.y);
            }
            
            public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
            {   var emp = (Vector2)obj;
                emp.x = info.GetSingle("x");
                emp.y = info.GetSingle("y");
                return emp;
            }
        }
        sealed class SSVector3 : ISerializationSurrogate {
            internal SSVector3()
            {   var type = this.GetType().FullName;
                if (!defaultTypes.ContainsKey(type)) defaultTypes.Add(type, true);
            }
            public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
            {   var emp = (Vector3)obj;
                info.AddValue("x", emp.x);
                info.AddValue("y", emp.y);
                info.AddValue("z", emp.z);
            }
            
            public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
            {   var emp = (Vector3)obj;
                emp.x = info.GetSingle("x");
                emp.y = info.GetSingle("y");
                emp.z = info.GetSingle("z");
                return emp;
            }
        }
        sealed class SSVector4 : ISerializationSurrogate {
            internal SSVector4()
            {   var type = this.GetType().FullName;
                if (!defaultTypes.ContainsKey(type)) defaultTypes.Add(type, true);
            }
            public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
            {   var emp = (Vector4)obj;
                info.AddValue("x", emp.x);
                info.AddValue("y", emp.y);
                info.AddValue("z", emp.z);
            }
            
            public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
            {   var emp = (Vector4)obj;
                emp.x = info.GetSingle("x");
                emp.y = info.GetSingle("y");
                emp.z = info.GetSingle("z");
                return emp;
            }
        }
        
        
        
    }
}
}
