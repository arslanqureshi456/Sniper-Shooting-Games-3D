using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
#if PROJECT
    using EModules.Project;
#endif


namespace EModules.EModulesInternal

{

[Serializable]
internal class DoubleList<l1, l2> : IEnumerable {

    internal T[] Select<T>(Func<l1, l2, T> select)
    {   T[] result = new T[0];
        for (int i = 0 ; i < listKeys.Count ; i++)
        {   ArrayUtility.Add( ref result, select( listKeys[i], listValues[i] ) );
        }
        return result;
    }
    internal string ToString(Func<l1, l2, string> select, string space = " ")
    {   string result = "";
        for (int i = 0 ; i < listKeys.Count ; i++)
        {   result += select( listKeys[i], listValues[i] );
            if (i == listKeys.Count) break;
            result += space;
        }
        return result;
    }
    
    [SerializeField]
    internal List<l1> listKeys = new List<l1>();
    [SerializeField]
    internal List<l2> listValues = new List<l2>();
    internal void Add(l1 key, l2 value)
    {   if (listKeys.Count != listValues.Count)
        {   var max = Math.Min(listKeys.Count, listValues.Count);
            while (listKeys.Count > max) listKeys.RemoveAt( listKeys.Count - 1 );
            while (listValues.Count > max) listValues.RemoveAt( listValues.Count - 1 );
        }
        listKeys.Add( key );
        listValues.Add( value );
        //if (listKeys.Count != listValues.Count) throw new Exception( "Double List out of sinc" );
    }
    
    internal int IndexOf(l1 key)
    {   return listKeys.IndexOf( key );
    }
    internal bool RemoveFrom(l1 key)
    {   var indStart = listKeys.IndexOf(key);
        if (indStart == -1) return false;
        while (indStart < listKeys.Count)
        {   listKeys.RemoveAt( indStart );
            listValues.RemoveAt( indStart );
        }
        return true;
    }
    internal void Insert(int index, l1 key, l2 value)
    {   listKeys.Insert( index, key );
        listValues.Insert( index, value );
    }
    
    internal bool RemoveAll(l1 key)
    {   // MonoBehaviour.print("ASD");
        if (key == null)
        {   for (int i = 0 ; i < listKeys.Count ; i++)
            {   // MonoBehaviour.print(listKeys[i]);
                if (listKeys[i] == null)
                {   listKeys.RemoveAt( i );
                    listValues.RemoveAt( i );
                    i--;
                }
            }
        }
        else
        {   for (int i = 0 ; i < listKeys.Count ; i++)
            {   if (listKeys[i] == null) continue;
                // MonoBehaviour.print(listKeys[i]);
                if (listKeys[i].Equals( key ))
                {   listKeys.RemoveAt( i );
                    listValues.RemoveAt( i );
                    i--;
                }
            }
        }
        
        //MonoBehaviour.print("aaa");
        
        return true;
    }
    internal bool Remove(l1 key, l2 value)
    {   int i = 0;
        for (i = 0 ; i < listKeys.Count ; i++)
        {   if (listKeys[i].Equals( key ) && listValues[i].Equals( value )) break;
        }
        //int findInd = listKeys.IndexOf( key );
        if (i == -1) return false;
        listKeys.RemoveAt( i );
        listValues.RemoveAt( i );
        return true;
    }
    
    
    
    internal void RemoveAt(int index)
    {   listKeys.RemoveAt( index );
        listValues.RemoveAt( index );
    }
    internal MyStrutempStr<l1, l2> Last()
    {   int ind = listKeys.Count - 1;
        tempKP.Key = listKeys[ind];
        tempKP.Value = listValues[ind];
        return tempKP;
    }
    
    internal bool ContainsKey(l1 key)
    {   return listKeys.Contains( key );
    }
    internal bool Contains(l1 key, l2 value)
    {   int ind = listKeys.IndexOf(key);
        if (ind == -1) return false;
        /*tempKP.Key = listKeys[ ind ];
        tempKP.Value = listValues[ ind ];*/
        return listValues[ind].Equals( value );
    }
    internal void Clear()
    {   listKeys.Clear();
        listValues.Clear();
    }
    
    internal void CopyTo(DoubleList<l1, l2> to)
    {   for (int i = 0 ; i < Count ; i++)
        {   to.Add( listKeys[i], listValues[i] );
        }
    }
    
    
    
    internal struct MyStrutempStr<inl1, inl2>
    {   internal inl1 Key;
        internal inl2 Value;
    }
    [NonSerialized]
    MyStrutempStr<l1, l2> tempKP;
    public IEnumerator<MyStrutempStr<l1, l2>> GetEnumerator()
    {   for (int i = 0 ; i < listKeys.Count ; i++)
        {   tempKP.Key = listKeys[i];
            tempKP.Value = listValues[i];
            yield return tempKP;
        }
    }
    
    IEnumerator IEnumerable.GetEnumerator()
    {   return GetEnumerator();
    }
    
    internal int Count
    {   get { return Math.Min( listKeys.Count, listValues.Count ); }
    }
    
    internal MyStrutempStr<l1, l2> this[int index]
    {   get
        {   tempKP.Key = listKeys[index];
            tempKP.Value = listValues[index];
            return tempKP;
        }
        set
        {   listKeys[index] = value.Key;
            listValues[index] = value.Value;
        }
        //set { dic[ key ] = value; }
    }
    
    internal l2 GetByKey(l1 key)
    {   int index = listKeys.IndexOf(key);
        if (index == -1)
        {   index = listKeys.Count;
            Add(key, default(l2));
        }
        return listValues[index];
    }
    internal void SetByKey(l1 key, l2 value)
    {   int index = listKeys.IndexOf(key);
        if (index == -1)
        {   index = listKeys.Count;
            Add(key, default(l2));
        }
        listValues[index] = value;
    }
    
    
    
    internal l2 this[l1 key]
    {   get
        {   int index = listKeys.FindIndex(l => l.Equals(key));
            return listValues[index];
        }
        set
        {   int index = listKeys.FindIndex(l => l.Equals(key));
            listValues[index] = value;
        }
        //set { dic[ key ] = value; }
    }
    
    
}
}
