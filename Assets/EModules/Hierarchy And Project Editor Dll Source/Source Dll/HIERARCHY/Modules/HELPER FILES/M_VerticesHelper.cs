using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEditor.SceneManagement;
using UnityEditor.Sprites;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;
//namespace EModules

namespace EModules.EModulesInternal

{
internal partial class Hierarchy {
    internal class M_VerticesHelper {
    
        internal struct MeshGetter
        {   internal ParticleSystemRenderer pr;
            internal MeshFilter f;
            internal SkinnedMeshRenderer sr;
            
            internal GameObject SetMesh
            {   set
                {   f = value.GetComponent<MeshFilter>();
                    if ( !f ) sr = value.GetComponent<SkinnedMeshRenderer>();
                    if ( !f && !sr ) pr = value.GetComponent<ParticleSystemRenderer>();
                }
            }
            internal Mesh mesh
            {   get
                {   if ( f ) return f.sharedMesh;
                    if ( pr ) return pr.mesh;
                    if ( sr ) return sr.sharedMesh;
                    return null;
                }
            }
        }
        
        [NonSerialized]
        public bool Eroor = false;
        [NonSerialized]
        public bool WasFirst = false;
        [NonSerialized]
        public bool BroadcastingInitializeAllObjects = false;
        [NonSerialized]
        public Dictionary<Shader, string[]> shaderTextures = new Dictionary<Shader, string[]>();
        [NonSerialized]
        public bool Broadcasting = false;
        [NonSerialized]
        public Dictionary<int, M_VerticesHelper.MeshGetter> md = new Dictionary<int, M_VerticesHelper.MeshGetter>();
        [NonSerialized]
        public Dictionary<Texture, Dictionary<int, bool>> TEXTUREobjects = new Dictionary<Texture, Dictionary<int, bool>>();
        [NonSerialized]
        public Dictionary<int, Dictionary<int, TextureSplitter>> OBJECTtexture = new Dictionary<int, Dictionary<int, TextureSplitter>>();
        [NonSerialized]
        public Dictionary<int, GUIContent> cacheValue = new Dictionary<int, GUIContent>();
        [NonSerialized]
        public Dictionary<int, MemoryData> broadCastValue = new Dictionary<int, MemoryData>();
        [NonSerialized]
        public Dictionary<int, double> updateTimer = new Dictionary<int, double>();
        
        public struct TextureSplitter : IEquatable<TextureSplitter>
        {   public TextureSplitter(Texture2D texture)
            {   this.textures = new[] { texture };
            
            }
            public TextureSplitter(Texture texture)
            {   this.textures = new[] { texture };
            }
            
            public TextureSplitter(Texture[] textures)
            {   this.textures = textures;
            }
            
            
            public Texture[] textures;
            
            public bool Equals(TextureSplitter other)
            {   if (other.textures.Length != textures.Length) return false;
                for (var i = 0; i < textures.Length; i++)
                    if (textures[i] != other.textures[i])
                        return false;
                return true;
            }
        }
        
        public struct MemoryData : IEqualityComparer<MemoryData>, IComparable<MemoryData>, IComparer<MemoryData>
        {
        
            public long memory;
            public char postfix;
            public string addparams;
            public bool instance;
            
            public void AddMemory(long mem)
            {   memory += mem;
            }
            
            public void Clear()
            {   memory = 0;
                postfix = ' ';
                addparams = null;
                instance = false;
            }
            
            public int Compare(MemoryData x, MemoryData y)
            {   return x.memory.CompareTo(y.memory);
            }
            
            public int CompareTo(MemoryData other)
            {   return memory.CompareTo(other);
            }
            
            public bool Equals(MemoryData x, MemoryData y)
            {   return x.memory == y.memory;
            }
            
            public int GetHashCode(MemoryData obj)
            {   var right = (int)(obj.memory >> 32);
                return ((int)obj.memory) ^ right;
            }
        }
    }
}
}