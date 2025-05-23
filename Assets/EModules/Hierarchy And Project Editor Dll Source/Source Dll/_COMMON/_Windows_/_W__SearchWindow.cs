﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor.IMGUI.Controls;
#if PROJECT
    using EModules.Project;
#endif
using UnityEditor;
//namespace EModules


namespace EModules.EModulesInternal

{
/* internal static class E<T>
{
 internal static T Invoke( object o , string method )
 {
   var callType = o.GetType();
   return (T)callType.InvokeMember( method , (System.Reflection.BindingFlags)int.MaxValue , null , o , null );
 }
 /* internal static bool InvokeBool(this object o , string method)
  {
    var callType = o.GetType();
    return (bool)callType.InvokeMember( method , (System.Reflection.BindingFlags)int.MaxValue , null , o , null );
  } internal static float InvokeFloat(this object o , string method)
  {
    var callType = o.GetType();
    return (float)callType.InvokeMember( method , (System.Reflection.BindingFlags)int.MaxValue , null , o , null );
  }#1#
}*/



public partial class _W__SearchWindow : _W___IWindow {
    bool ONEFRAMEPIN = false;
    static bool lastPin;
    internal override bool PIN
    {   get { return m_PIN || ONEFRAMEPIN || __inputWindow.ContainsKey( typeof( _W__InputWindow ) ) && __inputWindow[typeof( _W__InputWindow )]; }
        set
        {   m_PIN = value;
            lastPin = value;
            /* if (!value && op)
             {   this.CloseThis();
                 Init(
                     ___guiMousePos,
                     ___title,
                     ___searchFilter,
                     ___IN,
                     ___callBackModule,
                     ___adapter,
                     ___extension, false);
             }*/
        }
    }
    
    void Pin()
    {   this.CloseThis();
        Init(
            lastMousePos,
            ___title,
            ___searchFilter,
            ___IN,
            ___callBackModule,
            ___adapter,
            ___o,
            ___extension, true, false, __calledWIthControl, __activeObject );
    }
    void UnPin()
    {   this.CloseThis();
        Init(
            lastMousePos,
            ___title,
            ___searchFilter,
            ___IN,
            ___callBackModule,
            ___adapter,
            ___o,
            ___extension, false, false, __calledWIthControl, __activeObject );
    }
    
    internal class FillterData_Inputs {
        internal FillterData_Inputs( FillterData_Inputs oldin )
        {   if ( oldin != null )
            {   this.analizedObjects = oldin.analizedObjects;
                this.analizeEnumerator = oldin.analizeEnumerator;
            }
            else
            {   this.analizedObjects = null;
                this.analizeEnumerator = Utilities.AllSceneObjectsInterator( Adapter ).GetEnumerator();
            }
            
            
        }
        
        internal void Registrate_FillterData_Inputs( FillterData_Inputs oldin )
        {   this.analizedObjects = oldin.analizedObjects;
            this.analizeEnumerator = oldin.analizeEnumerator;
        }
        
        internal Action<_W__SearchWindow.FillterData_Inputs> UpdateCache;
        internal Func<Adapter.HierarchyObject, bool> Valudator;
        internal Func<Adapter.HierarchyObject, int, string> SelectCompareString;
        internal Func<Adapter.HierarchyObject, int, int> SelectCompareCostInt;
        
        //   internal Func<float, float> topGUI;
        
        internal List<Adapter.HierarchyObject> analizedObjects = new List<Adapter.HierarchyObject>();
        // internal List<int> analizedOrderBeforeCost = new List<int>();
        //internal List<int> analizedCost = new List<int>();
        internal IEnumerator<Adapter.HierarchyObject> analizeEnumerator;
        internal Type typeFilter = null;
        
        internal bool SKANNING;
#pragma warning disable
        internal bool SKIP_SKANNING;
#pragma warning restore
    }
    
    FillterData_Inputs IN = null;
    Type m_typeFilter = null;
    /* internal List<GameObject> objects;
     internal List<int> contentCost;*/
    internal string FiltersOf = null;
    internal static Adapter Adapter = null;
    
    
    static Adapter.HierarchyObject ___o;
    static string ___title;
    static string ___searchFilter;
    static FillterData_Inputs ___IN;
    static Adapter.Module ___callBackModule;
    static Adapter ___adapter;
    static string ___extension;
    static   bool __callFromExternal;
    static   bool __calledWIthControl;
    static   UnityEngine.Object __activeObject;
    // static   bool ___utils;
    
    
    int id = 0;
    
    internal static _W___IWindow Init( MousePos guiMousePos, string title, string searchFilter, FillterData_Inputs IN, Adapter.Module callBackModule,
                                       Adapter adapter, Adapter.HierarchyObject _o, string extension = null, bool utils = false, bool useAnim = true, bool? calledWithControlOld = null,
                                       UnityEngine.Object activeObject = null )
    {
    
        if ( guiMousePos.type != MousePos.Type.Search_356_0 )
        {   Debug.LogWarning( "Mismatch type" );
            guiMousePos.SetType( MousePos.Type.Search_356_0 );
        }
        /* var pinover = callFromExternal && ___utils;
         utils = utils || pinover;*/
        
        if ( callBackModule.callFromExternal() && lastPin )
        {   utils = true;
            useAnim = false;
        }
        
        if ( calledWithControlOld.HasValue )
        {   __calledWIthControl = calledWithControlOld.Value
                                  ;
        }
        else
        {   if ( !callBackModule.callFromExternal() )
                __calledWIthControl = Event.current != null && Event.current.control;
        }
        
        
        
        //Debug.Log(AssetDatabase.LoadAssetAtPath<Hierarchy_GUI>("ProjectSettings/Hierarchy (MAYA).asset"));
        
        /*  foreach (var asdads in AssetDatabase.FindAssets( "t:" + typeof( Hierarchy_GUI ).Name ))
          {   var l = AssetDatabase.LoadAssetAtPath<Hierarchy_GUI>( AssetDatabase.GUIDToAssetPath( asdads ) );
              if (!l || l.pluginID != adapter.pluginID) continue;
              Debug.Log(AssetDatabase.GUIDToAssetPath( asdads ));
        
              break;
          }*/
        if ( !callBackModule.callFromExternal() )
            __activeObject = activeObject ?? Selection.activeObject;
            
        ___o = _o;
        ___title = title;
        ___searchFilter = searchFilter;
        ___IN = IN;
        ___callBackModule = callBackModule;
        ___adapter = adapter;
        ___extension = extension;
        // ___utils = utils;
#pragma warning disable
        if ( Adapter.LITE )     // var pos = InputData.WidnwoRect(guiMousePos, 148, 68, adapter);
        {   //  InputData.Init(pos, "", adapter, null, null, "Search Box (Pro Only)", 0.83f);
            return null;
        }
#pragma warning restore
        var _inputWindow = __inputWindow.Values.FirstOrDefault(w => w is _W__SearchWindow) as _W__SearchWindow;
        
        
        __callFromExternal = callBackModule.callFromExternal();
        
        if ( __callFromExternal && _inputWindow && ((_W__SearchWindow)_inputWindow).IN != null && ((_W__SearchWindow)_inputWindow).IN.SKANNING ) return null;
        
        // if (!callFromExternal) guiMousePos.x += 20;
        //  MonoBehaviour.print(hierWin.position.height);
        
        // var savePosition = callFromExternal;
        // var rect = InputData.WidnwoRect(!callFromExternal && clamp, guiMousePos, 356, 0, adapter, savePosition ? (Vector2 ? )new Vector2(_inputWindow.position.x, _inputWindow.position.y - 40) : null );
        
        
        
        //if (IN.topGUI == null) IN.topGUI = (w) => 0;
        
        
        m_columnOffset = callBackModule.GetInputWidth();
        m_callBackType = callBackModule;
        // m_topGUI = IN.topGUI;
        m_title = title;
        m_searchFilter = searchFilter;
        Adapter = adapter;
        utils |= Adapter.par.PIN_FILLTERWIN_BYDEFAULT;
        /* m_tobjects_sortHierarchy = objects.ToList();
         m_tobjects_sortHierarchyDescending = m_tobjects_sortHierarchy.ToArray().Reverse().ToList();
         m_tobjects_sortName = objects.OrderBy(o => o.name).ToList();
         m_tobjects_sortNameDescending = m_tobjects_sortName.ToArray().Reverse().ToList();
        
         var enumerable = from gameObject in objects.Select((o, i) => new { i, o })
                          join cost in contentCost.Select((o, i) => new { i, o }) on gameObject.i equals cost.i
                          let union = new { gameObject, cost }
                          orderby union.cost.o
                          select union.gameObject.o;
        
         m_tobjects_sortContent = enumerable.ToList();
         m_tobjects_sortContentDescendting = m_tobjects_sortContent.ToArray().Reverse().ToList();*/
        // up = IN.topGUI( rect.width );
        
        
        // rect.height =  yPosSum;
        
        // rect = InputData.WidnwoRect( !callFromExternal, guiMousePos, 356, rect.height, adapter, savePosition ? (Vector2 ? )new Vector2( _inputWindow.position.x, _inputWindow.position.y - 40 ) : null );
        
        //  MonoBehaviour.print(rect);
        
        //   if (adapter._S_searchBintToLeft) rect.x = GUIUtility.GUIToScreenPoint(Vector2.zero).x;
        
        guiMousePos.Clamp = !utils;
        
        if ( adapter._S_searchBintToLeft && guiMousePos.Clamp ) guiMousePos.ScreenMousePosition = new Vector2( adapter.window().position.x + 20, guiMousePos.ScreenMousePosition.y );
        guiMousePos.Width = adapter._S_searchWidthMulty;
        guiMousePos.Height = yPosSum;
        
        
        var newwin = false;
        if ( !_inputWindow ) //new
        {   _inputWindow = _W___IWindow.private_Init( guiMousePos, typeof( _W__SearchWindow ), adapter, "Search " + searchFilter + " " + adapter.pluginname,
                           new Vector2( 356, yPosSum ),
                           new Vector2( 656, Adapter.MAX_WINDOW_HEIGHT.y ),
                           // new Vector2( 656,  Screen.currentResolution.height * 0.95f ),
                           utils: utils, useAnim: useAnim ) as _W__SearchWindow;
            _inputWindow.SET_NEW_HEIGHT( Adapter, guiMousePos.Height );
            _inputWindow.SIZIBLE_Y = true;
            newwin = true;
        }
        else   //old
        {   _inputWindow.lastMousePos = guiMousePos;
            _inputWindow.titleContent = new GUIContent( "Search " + searchFilter + " " + adapter.pluginname );
            /* _inputWindow.minSize = new Vector2( 356, yPosSum );
             _inputWindow.maxSize = new Vector2( 656, Resources.FindObjectsOfTypeAll<EditorWindow>().Select( w => w.position.height ).Max() );*/
            _inputWindow.StoptBroadcasting();
        }
        _inputWindow.id++;
        
        if ( newwin || guiMousePos.Clamp )
        {   if ( __callFromExternal || !useAnim )     // _inputWindow.lastTime = EditorApplication.timeSinceStartup - 100;
            {   _inputWindow.AniimationUpdateR( 1 );
                _inputWindow.wasAnim = true;
            }
        }
        
        //_inputWindow.minSize = _inputWindow.minSize;
        /*  var ts = _inputWindow.maxSize;
        //  Debug.Log( "123" );
          ts.y = Resources.FindObjectsOfTypeAll<EditorWindow>().Select(w=>w.position.height).Max();
          _inputWindow.maxSize = ts;*/
        // _inputWindow.maxSize = Vector2.zero;
        
        var f = (_W__SearchWindow)_inputWindow;
        // f.RawRect = rect;
        f.m_typeFilter = IN.typeFilter;
        f.IN = IN;
        f.FiltersOf = null;
        f.PIN = utils /*|| pinover*/ ;
        
        /* if (redT == null)
         {   redT = Adapter.GET_TEXTURE( 0x001, new Color( 0.6f, 0.3f, 0.1f, 1 ) );
         */
        /* redT = new  Texture2D(1, 1, TextureFormat.ARGB32, false, true);
         redT.hideFlags = HideFlags.DontSave;
         redT.SetPixel(0, 0, new Color(0.6f, 0.3f, 0.1f, 1));
         redT.Apply();*/
        /* }*/
        
        if ( __callFromExternal && f.IN != null ) IN = f.IN;
        
        
        
        
        f.ClearSortedLists_AndStartBroadCast();
        f.MoveToSeledction();
        //if (IN.UpdateCache != null)  IN.UpdateCache(IN);
        
        
        
        //  _inputWindow.add = true;
        return _inputWindow;
    }
    
    
    
    
    
    float min_height
    {   get
        {   /* var rect = _inputWindow.position;
                 up = m_topGUI( rect.width );*/
            return /*up +*/ yPosSum;
        }
    }
    void UpdateSize()
    {
    
        if ( !m_PIN || !__callFromExternal )
        {   var rect = _inputWindow.position;
            rect.height = ItemH() * s__HierarchyPos.Count + min_height;
            SET_NEW_HEIGHT( Adapter, rect.height );
        }
        Repaint();
        /*    rect = InputData.WidnwoRect(Vector2.one, 356, rect.height, (Vector2?)new Vector2(_inputWindow.position.x, _inputWindow.position.y - 40), HEIGH_CLAMPER: 0.75f);
           // _inputWindow.position = rect;
        
            targetRect.height = rect.height;*/
    }
    
    
    
    // internal Rect RawRect;
    // static Color redT = new Color( 0.6f, 0.3f, 0.1f, 1 );
    
    static Adapter.Module m_callBackType;
    static Func<float, float> m_topGUI;
    static string m_title = "";
    static string m_searchFilter = "";
    internal string SearchFilter { get { return m_searchFilter; } }
    
    
    IList<Adapter.HierarchyObject[]> objectsList
    {   get
        {   var sort = Adapter.GetSortMode(m_callBackType.GetType());
            switch ( sort.Key )
            {   case SortType.Hierarchy:
                    if ( sort.Value ) return s__HierarchyPos_Desc.Values;
                    return s__HierarchyPos.Values;
                case SortType.Name:
                    if ( sort.Value ) return s__Name_Desc.Values;
                    return s__Name.Values;
                case SortType.Content:
                    if ( sort.Value ) return s__Content_Desc.Values;
                    return s__Content.Values;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            
            /*if (cacheList != null && order == sort.Value && cahceType == sort.Key)
            {
                return cacheList;
            }
            order = sort.Value;
            cahceType = sort.Key;
            
            switch (sort.Key)
            {
            
            default:
                throw new ArgumentOutOfRangeException();
            }
            
            return cacheList;*/
            
        }
    }
    readonly Dictionary<int, int> drawIndex = new Dictionary<int, int>();
    
    readonly SortedList<int, Adapter.HierarchyObject[]> s__HierarchyPos = new SortedList<int, Adapter.HierarchyObject[]>(new DescComparer<int>());
    readonly SortedList<string, Adapter.HierarchyObject[]> s__Name = new SortedList<string, Adapter.HierarchyObject[]>();
    readonly SortedList<string, Adapter.HierarchyObject[]> s__SelectCompareString = new SortedList<string, Adapter.HierarchyObject[]>();
    readonly SortedList<int, Adapter.HierarchyObject[]> s__Content = new SortedList<int, Adapter.HierarchyObject[]>();
    
    readonly SortedList<int, Adapter.HierarchyObject[]> s__HierarchyPos_Desc = new SortedList<int, Adapter.HierarchyObject[]>();
    readonly SortedList<string, Adapter.HierarchyObject[]> s__Name_Desc = new SortedList<string, Adapter.HierarchyObject[]>(new DescComparer<string>());
    readonly SortedList<int, Adapter.HierarchyObject[]> s__Content_Desc = new SortedList<int, Adapter.HierarchyObject[]>(new DescComparer<int>());
    
    
    class DescComparer<T> : IComparer<T> {
        public int Compare( T x, T y )
        {   return Comparer<T>.Default.Compare( y, x );
        }
    }
    
    
    void ClearSortedLists_AndStartBroadCast( bool skipIfSkip = false )
    {   s__HierarchyPos.Clear();
        s__Name.Clear();
        s__Content.Clear();
        s__HierarchyPos_Desc.Clear();
        s__Name_Desc.Clear();
        s__Content_Desc.Clear();
        s__SelectCompareString.Clear();
        
        parentsCount.Clear();
        LastDirection = -1;
        drawIndex.Clear();
        
        if ( IN.UpdateCache != null ) IN.UpdateCache( IN );
        OnUpdateCache( skipIfSkip );
    }
    
    void OnUpdateCache( bool skipIfSkip = false )     //  if (IN.analizedObjects == null)
    {
    
        var skip = IN.SKIP_SKANNING || __callFromExternal && IN.analizedObjects != null;
        if ( skipIfSkip ) skip = false;
        if ( !skip )
        {   IN.analizedObjects = new List<Adapter.HierarchyObject>();
            var active = __activeObject;
            //if (Event.current == null || !Event.current.control) active = null;
            if ( !__calledWIthControl ) active = adapter.SEARCH_USE_ROOT_ONLY ? ___o != null ? ___o.root(adapter).GetHardLoadObject() : null : null;
            
            IN.analizeEnumerator = Utilities.AllSceneObjectsInterator( adapter, activeParent: active, extension: ___extension ).GetEnumerator();
        }
        var ans = IN.analizedObjects.ToList();
        IN.analizedObjects.Clear();
        for ( int i = 0 ; i < ans.Count ; i++ )
        {   if ( IN.Valudator( ans[i] ) )
                RegistrateObject( ans[i] );
        }
        UpdateCostList();
        if ( skip )
            UpdateSize();
            
        StoptBroadcasting();
        if ( !skip )
            if ( IN.analizeEnumerator != null )
            {   StartBroadcasting();
                //  CalcBroadCast();
            }
    }
    
    
    
    
    void UpdateCostList()
    {   s__Content.Clear();
        s__Content_Desc.Clear();
        
        var i = -1;
        foreach ( var gameObjectse in s__SelectCompareString )
        {   for ( int j = 0 ; j < gameObjectse.Value.Length ; j++ )
            {   var ob = gameObjectse.Value[j];
                i++;
                if ( !ob.Validate() ) continue;
                var obCost = IN.SelectCompareCostInt(ob, i);
                
                if ( s__Content.ContainsKey( obCost ) )
                {   var arr = s__Content[obCost];
                    Array.Resize( ref arr, s__Content[obCost].Length + 1 );
                    s__Content[obCost] = arr;
                    s__Content[obCost][s__Content[obCost].Length - 1] = ob;
                    
                    s__Content_Desc[obCost] = arr;
                }
                else
                {   s__Content.Add( obCost, new Adapter.HierarchyObject[] { ob } );
                    s__Content_Desc.Add( obCost, new Adapter.HierarchyObject[] { ob } );
                }
            }
            
        }
        
    }
    
    
    void RegistrateObject( Adapter.HierarchyObject ob )
    {   if ( !ob.Validate() ) return;
    
    
        IN.analizedObjects.Add( ob );
        
        if ( !drawIndex.ContainsKey( ob.id ) )
            drawIndex.Add( ob.id, -1 );
            
        s__HierarchyPos.Add( s__HierarchyPos.Count, new Adapter.HierarchyObject[] { ob } );
        s__HierarchyPos_Desc.Add( s__HierarchyPos_Desc.Count, new Adapter.HierarchyObject[] { ob } );
        
        var nameInt = ob.ToString();
        if ( s__Name.ContainsKey( nameInt ) )
        {   var arr = s__Name[nameInt];
            Array.Resize( ref arr, s__Name[nameInt].Length + 1 );
            s__Name[nameInt] = arr;
            s__Name[nameInt][s__Name[nameInt].Length - 1] = ob;
            
            s__Name_Desc[nameInt] = arr;
        }
        else
        {   s__Name.Add( nameInt, new Adapter.HierarchyObject[] { ob } );
            s__Name_Desc.Add( nameInt, new Adapter.HierarchyObject[] { ob } );
        }
        
        
        var obString = IN.SelectCompareString(ob, s__HierarchyPos.Count);
        if ( s__SelectCompareString.ContainsKey( obString ) )
        {   var arr = s__SelectCompareString[obString];
            Array.Resize( ref arr, s__SelectCompareString[obString].Length + 1 );
            s__SelectCompareString[obString] = arr;
            s__SelectCompareString[obString][s__SelectCompareString[obString].Length - 1] = ob;
        }
        else
        {   s__SelectCompareString.Add( obString, new Adapter.HierarchyObject[] { ob } );
        }
        
        
        
        
    }
    
    // static Action<string> comformAction;
    // static Action<string> closeAction;
    
    protected internal override void CloseThis()     // if (closeAction != null) closeAction(textInput);
    {   base.CloseThis();
        if ( adapter != null && adapter.window() ) adapter.window().Focus();
    }
    
    
    static Dictionary<int, int> selectedO = new Dictionary<int, int>();
    internal void MoveToSeledction()
    {   selectedO.Clear();
        if ( Adapter.activeGameObject() != null )
        {   foreach ( var gameObject in Adapter.SELECTED_GAMEOBJECTS() )
            {   if ( !selectedO.ContainsKey( gameObject.id ) ) selectedO.Add( gameObject.id, 0 );
            }
            var o = Adapter.activeGameObject();
            // var i = objectsList.IndexOf(Hierarchy.activeGameObject());
            var i = -1;
            if ( drawIndex.ContainsKey( o.id ) ) i = drawIndex[o.id];
            if ( i != -1 )
            {   if ( (i - 5) * ItemH() < scrollPos.y ) scrollPos.y = (i - 5) * ItemH();
                else if ( (i + 5) * ItemH() > scrollPos.y + scrollHeight ) scrollPos.y = (i + 5) * ItemH() - scrollHeight;
            }
        }
    }
    
    internal override void OnSelectionChange()
    {   if ( PIN ) MoveToSeledction();
        base.OnSelectionChange();
    }
    
    static float bottomBut = EditorGUIUtility.singleLineHeight + 4;
    private static int yPosSum
    {   get
        {   return Mathf.RoundToInt( bottomBut * 5
                                     + LINEAFTERSPACE
                                     + LINE1 + LINE2 + LINE3
                                     + LINESPACE + EditorGUIUtility.singleLineHeight * 0.2f );
        }
    }
    private static int ItemH()
    {   return Mathf.RoundToInt( /*Adapter.HALFFACTOR_8() * */Adapter.parLINE_HEIGHT );
    }
    /* float scrollHeight { get { return Math.Min(RawRect.height - up - yPosSum, s__HierarchyPos.Count * ItemH()); } }
     bool scrollHeightOver { get { return animcomplete && (RawRect.height - up - yPosSum < s__HierarchyPos.Count * ItemH()); } }*/
    float scrollHeight { get { return Math.Min( /*MAX_HEIGHT( adapter )*/ position.height /*- up*/ - yPosSum, s__HierarchyPos.Count * ItemH() ); } }
    bool scrollHeightOver { get { return animcomplete && ((MAX_HEIGHT( adapter ) /*- up */ - yPosSum < s__HierarchyPos.Count * ItemH()) || (position.height /*- up*/ - yPosSum) < s__HierarchyPos.Count * ItemH()); } }
    
    internal static float MAX_HEIGHT( Adapter a )     //if (! a.window()) Debug.Log("ASD");
    {   return a.window().position.height + 20;
        //Screen.currentResolution.height * (HEIGH_CLAMPER)
    }
    
    //static float HEIGH_CLAMPER = 0.75f;
    private static Vector2 scrollPos = Vector2.zero;
    private static float m_columnOffset;
    //private static float up;
    static bool USEDOUBLECLICKTOCLOSE = true;
    double DoubleClickTime = -10000;
    static    GUIStyle shadow = null;
    
    void SELECT( Adapter.HierarchyObject[] list, bool useDOuble)
    {   UnityEngine.Object[] target = null;
        if ( adapter.pluginID == Initializator.HIERARCHY_ID ) target = list.Where( o => o.go ).Select( o => o.go ).ToArray();
        if ( adapter.pluginID == Initializator.PROJECT_ID ) target = list.Select( o => AssetDatabase.LoadMainAssetAtPath( o.project.assetPath ) ).Where( a => a ).ToArray();
        
        if ( useDOuble && USEDOUBLECLICKTOCLOSE )
        {   var tid = target.Select(t => t.GetInstanceID()).ToDictionary(k => k);
            var same = tid.Count == Selection.objects.Length && Selection.objects.Select(o => o.GetInstanceID()).All(i => tid.ContainsKey(i));
            if ( same && Math.Abs( DoubleClickTime - EditorApplication.timeSinceStartup ) < 0.4f )
            {   Adapter.EventUseFast();
                CloseThis();
            }
            DoubleClickTime = EditorApplication.timeSinceStartup;
        }
        
        
        
        Selection.objects = target;
        
    }
    
    static Dictionary<int, int> parentsCount = new Dictionary<int, int>();
    
    Rect? dragRect = null;
    UnityEngine.Object[] dragObjects = null;
    
    static  float LINESPACE = 5;
    static float LINE1 { get { return 42 * Adapter.HALFFACTOR_8(); } }
    static float LINE2 { get { return EditorGUIUtility.singleLineHeight * 2; } }
    static float LINE3 { get { return EditorGUIUtility.singleLineHeight * 1.5f; } }
    static float LINEAFTERSPACE { get { return EditorGUIUtility.singleLineHeight * 0.2f; } }
    static Adapter.TempColorClass tempColorEmpty = new Adapter.TempColorClass().AddIcon(null);
    
    GUIStyle __button_left;
    GUIStyle button_left
    {   get
        {   if ( __button == null )
            {   __button = new GUIStyle( adapter.button );
                __button.alignment = TextAnchor.MiddleLeft;
                __button.padding.top = 3;
                __button.padding.bottom = 0;
            }
            __button.fontSize = Adapter.WINDOW_FONT_10();
            return __button;
        }
    }
    
    GUIStyle __button;
    GUIStyle button
    {   get
        {   if ( __button == null )
            {   __button = new GUIStyle( adapter.button );
            
            }
            __button.fontSize = Adapter.WINDOW_FONT_12();
            return __button;
        }
    }
    GUIStyle __label;
    GUIStyle label
    {   get
        {   if ( __label == null )
            {   __label = new GUIStyle( adapter.label );
                __label.alignment = TextAnchor.MiddleLeft;
            }
            __label.fontSize = Adapter.WINDOW_FONT_10() - 2;
            return __label;
        }
    }
    GUIStyle __textArea;
    GUIStyle textArea
    {   get
        {   if ( __textArea == null )
            {   __textArea = new GUIStyle( Adapter.GET_SKIN().textArea );
                __textArea.alignment = TextAnchor.MiddleCenter;
            }
            __textArea.fontSize = Adapter.WINDOW_FONT_12();
            return __textArea;
        }
    }
    
    void rawOnUp()
    {   dragRect = null;
    }
    
    protected override void OnGUI()
    {   if ( _inputWindow == null || Adapter.LITE ) return;
    
        if ( Event.current.type == EventType.Repaint )
            ONEFRAMEPIN = false;
        if ( Adapter == null )
        {
        
            // if (!m_PIN)
            {   CloseThis();
                return;
            }
            
            
            /*var c = titleContent;
            var adapterName = c.text.Substring(c.text.LastIndexOf(' ') + 1);
            if (!Initializator.AdaptersByName.ContainsKey( adapterName ))
            {   adapterName = c.text.Remove( c.text.IndexOf( ' ' ) );
                if (!Initializator.AdaptersByName.ContainsKey( adapterName ))
                    return;
            }
            adapter = Initializator.AdaptersByName[adapterName];
            
            */
            
            
        }
        
        
        base.OnGUI();
        
        if ( Event.current.rawType == EventType.MouseUp ) rawOnUp();
        if ( Event.current.type == EventType.MouseDrag && dragRect.HasValue )
        {   var mp = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
            if ( !dragRect.Value.Contains( mp ) )
            {   dragRect = null;
                adapter.InternalClearDrag();
                // DragAndDrop.PrepareStartDrag();// reset data
                DragAndDrop.objectReferences = dragObjects;
                DragAndDrop.StartDrag( "Dragging Assets" );
                DragAndDrop.SetGenericData( adapter.pluginname, null );
                Adapter.EventUseFast();
                GUIUtility.hotControl = 0;
            }
        }
        
        #region INIT
        
        if ( !Event.current.isKey )
        {
        
            if ( !Event.current.isMouse && Event.current.type != EventType.Repaint && Event.current.type != EventType.Ignore && Event.current.type != EventType.Used && Event.current.type != EventType.ScrollWheel
                    ||
                    Event.current.type == EventType.MouseMove/* || Event.current.type == EventType.mouseDrag*/) return;
                    
        }
        /*   var ctrl = Event.current.control;
              var shift = Event.current.shift;
              var alt = Event.current.alt;
              var keyCode = Event.current.keyCode;
              if (ctrl && !shift && !alt && keyCode == KeyCode.A)
              {
        
                  EventUse();
              }*/
        /*  Debug.Log(Event.current.isKey);
          Debug.Log(Event.current.control);
          Debug.Log(Event.current.keyCode == KeyCode.A);*/
        
        Adapter.ChangeGUI( false );
        
        //up = m_topGUI( _inputWindow.position.width );
        // var r = new Rect(0, up, _inputWindow.position.width, _inputWindow.position.height - up);
        var r = new Rect(0, 0, _inputWindow.position.width, _inputWindow.position.height);
        var sort = Adapter.GetSortMode(m_callBackType.GetType());
        
        GUI.BeginGroup( r ); //BEGIN
        /*    var s_labelFS = Adapter.GET_SKIN().label.fontSize;
            var s_labelAL = Adapter.GET_SKIN().label.alignment;
            Adapter.GET_SKIN().label.fontSize = Adapter.WINDOW_FONT_10() - 2;
            Adapter.GET_SKIN().label.alignment = TextAnchor.MiddleLeft;
            */
        //var bF = Adapter.GET_SKIN().button.fontSize;
        // Adapter.GET_SKIN().button.fontSize = (Adapter.WINDOW_FONT_8() - 8) / 2 + 6;
        // Adapter.GET_SKIN().button.fontSize = Adapter.WINDOW_FONT_10();
        
        r.width -= 10;
        float yPos = 0;
        var rect = new Rect(0, yPos, r.width - LINE1, LINE1);
        //  GUI.TextArea( rect, m_title, textArea);
        if ( Event.current.type == EventType.Repaint ) textArea.Draw( rect, m_title, false, false, false, false );
        
        // var rect = new Rect(r.width - 32, yPos + 2, 25 * Adapter.HALFFACTOR_8(), 25 * Adapter.HALFFACTOR_8());
        var IS = 30;
        rect.x = r.width - LINE1 + (LINE1 - IS) / 2;
        rect.width = IS;
        // if (PIN) GUI.DrawTexture(rect, Adapter.GetIcon("BUTBLUE"));
        if ( m_PIN ) Adapter.DrawRect( rect, new Color( 0.6f, 0.3f, 0.1f, 0.2f ) );
        GUI.DrawTexture( new Rect( rect.x, (rect.height - IS) / 2, IS, IS ), Adapter.GetIcon( m_PIN ? "AUTOHIDEPIN" : "AUTOHIDEFREE" ) );
        if ( GUI.Button( rect, new GUIContent() { tooltip = "Auto Hide Window " + (m_PIN ? "(disable)" : "(enable)") }, button ) )
        {   PIN = !PIN;
            if ( !PIN ) UnPin();
            else Pin();
        }
        yPos += LINE1;
        yPos += +LINESPACE;
        
        if ( m_searchFilter != "All" )     // GUI.DrawTexture( new Rect( 0, yPos + 8, r.width, 19 ), redT );
        {
        }
        var lf = label.fontSize;
        var lfs = label.fontStyle;
        label.fontSize = Adapter.WINDOW_FONT_10();
        label.fontStyle = FontStyle.Bold;
        GUI.Label( new Rect( EditorGUIUtility.singleLineHeight, yPos, r.width, LINE2 ), "Filter: " + m_searchFilter + (FiltersOf != null ? " of " + FiltersOf : ""), label );
        label.fontSize = lf;
        label.fontStyle = lfs;
        yPos += LINE2;
        
        
        var SORTBY_W = EditorGUIUtility.singleLineHeight * 5;
        //rect = new Rect( 0, yPos, SORTBY_W, 47 );
        rect = new Rect( 0, yPos, SORTBY_W, LINE3 );
        var SH = rect.height;
        var ORDER_BUT = rect.height;
        var S1 = EditorStyles.miniButtonLeft;
        var S2 = EditorStyles.miniButton;
        var S3 = EditorStyles.miniButtonRight;
        S1 = S2 = S3 = EditorStyles.toolbarButton;
        GUI.Label( new Rect( rect.x + 16, rect.y, rect.width, rect.height ), "Sort by: " /*+ sort.Key.ToString()*/, label );
        
        var WWW = r.width - SORTBY_W - ORDER_BUT;
        
        //rect = new Rect(0, yPos + 2, r.width / 3, 22);
        rect.y += 2;
        rect.height -= 4;
        rect.x += rect.width;
        rect.width = WWW / 3;
        //  if (sort.Key == SortType.Hierarchy) Adapter. INTERNAL_BOX(rect, "");
        //GUI.DrawTexture(rect, Adapter.GetIcon("BUT"));
        var ss = new GUIContent() { text = SortType.Hierarchy.ToString(), tooltip = "Sort by " + SortType.Hierarchy };
        var sortSymb = !sort.Value ? "▲" : "▼";
        
        var style = S1;
        var tt = style.border.top;
        var bb = style.border.bottom;
        if ( !EditorGUIUtility.isProSkin )
        {   style.border.top = 5;
            style.border.bottom = 5;
        }
        if ( GUI.Button( rect, ss, S1 ) )
        {   if ( sort.Key == SortType.Hierarchy ) sort = Adapter.SetSortMode( m_callBackType.GetType(), sort.Key, !sort.Value );
            else sort = Adapter.SetSortMode( m_callBackType.GetType(), SortType.Hierarchy, sort.Value );
        }
        var casd = GUI.color;
        if ( !EditorGUIUtility.isProSkin )
            GUI.color *= new Color( 1f, 0.7f, 0.6f );
        ss.text = ss.text + sortSymb;
        if ( sort.Key == SortType.Hierarchy && Event.current.type == EventType.Repaint ) S1.Draw( rect, ss, true, true, false, true );
        GUI.color = casd;
        rect.x += rect.width;
        style.border.top = tt;
        style.border.bottom = bb;
        
        // if (sort.Key == SortType.Name) Adapter. INTERNAL_BOX(rect, "");
        //GUI.DrawTexture(rect, Adapter.GetIcon("BUT"));
        style = S2;
        tt = style.border.top;
        bb = style.border.bottom;
        if ( !EditorGUIUtility.isProSkin )
        {   style.border.top = 5;
            style.border.bottom = 5;
        }
        ss = new GUIContent() { text = SortType.Name.ToString(), tooltip = "Sort by " + SortType.Name };
        if ( GUI.Button( rect, ss, style ) )
        {   if ( sort.Key == SortType.Name ) sort = Adapter.SetSortMode( m_callBackType.GetType(), sort.Key, !sort.Value );
            else sort = Adapter.SetSortMode( m_callBackType.GetType(), SortType.Name, sort.Value );
        }
        casd = GUI.color;
        if ( !EditorGUIUtility.isProSkin )
            GUI.color *= new Color( 1f, 0.7f, 0.6f );
        ss.text = ss.text + sortSymb;
        if ( sort.Key == SortType.Name && Event.current.type == EventType.Repaint ) style.Draw( rect, ss, true, true, false, true );
        GUI.color = casd;
        rect.x += rect.width;
        style.border.top = tt;
        style.border.bottom = bb;
        
        //if (sort.Key == SortType.Content) Adapter. INTERNAL_BOX(rect, "");
        //GUI.DrawTexture(rect, Adapter.GetIcon("BUT"));
        style = S3;
        tt = style.border.top;
        bb = style.border.bottom;
        if ( !EditorGUIUtility.isProSkin )
        {   style.border.top = 5;
            style.border.bottom = 5;
        }
        ss = new GUIContent() { text = SortType.Content.ToString(), tooltip = "Sort by " + SortType.Content };
        if ( GUI.Button( rect, ss, style ) )
        {   if ( sort.Key == SortType.Content ) sort = Adapter.SetSortMode( m_callBackType.GetType(), sort.Key, !sort.Value );
            else sort = Adapter.SetSortMode( m_callBackType.GetType(), SortType.Content, sort.Value );
        }
        casd = GUI.color;
        if ( !EditorGUIUtility.isProSkin )
            GUI.color *= new Color( 1f, 0.7f, 0.6f );
        ss.text =  ss.text + sortSymb;
        if ( sort.Key == SortType.Content && Event.current.type == EventType.Repaint ) style.Draw( rect, ss, true, true, false, true );
        GUI.color = casd;
        rect.x += rect.width;
        style.border.top = tt;
        style.border.bottom = bb;
        
        
        rect = new Rect( r.width - ORDER_BUT, yPos + 2, ORDER_BUT, rect.height );
        //  rect.y += 7;
        //  rect.height -= 14;
        /* var ASD = rect.width * 0.2f;
         GUI.DrawTexture( new Rect(rect.x + ASD, rect.y, rect.width - ASD * 2, rect.height), Adapter.GetIcon( !sort.Value ? "SORT" : "SORTDES" ), ScaleMode.ScaleToFit );
         if (GUI.Button( rect, new GUIContent() { tooltip = ("Sorting Order"  ) } )) sort = Adapter.SetSortMode( m_callBackType.GetType(),
                 sort.Key, !sort.Value );*/
        
        
        yPos += SH; ;
        yPos += LINEAFTERSPACE;
        
        var H = scrollHeight ;
        var SCE = 10;
        rect = new Rect( SCE, yPos, r.width + 15 - SCE, H );
        if ( scrollHeightOver ) rect.width = r.width;
        
        /*   var s_butAl = Adapter.GET_SKIN().button.alignment;
           Adapter.GET_SKIN().button.alignment = TextAnchor.MiddleLeft;*/
        var scrrect = rect;
        scrrect.x -= SCE;
        scrrect.width += SCE;
        #endregion
        
        var itemH = ItemH();
        
        var contentRect = new Rect(0, 0, r.width - (scrollHeightOver ? GUI.skin.verticalScrollbar.fixedWidth : 0) - 10, itemH * s__HierarchyPos.Count);
        rect.x += 5;
        var bgc = Adapter.EditorBGColor;
        Adapter.DrawRect( rect, bgc );
        rect.width -= 8;
        Adapter.INTERNAL_BOX( rect, "" );
        if ( Event.current.isKey && Event.current.control && !Event.current.alt && !Event.current.shift && Event.current.keyCode == KeyCode.A && EditorWindow.focusedWindow == this)     //var oldPIN = PIN;
        {   PIN = PIN;
        
            if ( !PIN )
            {   Adapter.EventUseFast();
                var list = objectsList.SelectMany( o => o ).ToArray();
                adapter.GUI_ONESHOTPUSH( () =>
                {   SELECT( list, false );
                    CloseThis();
                } );
            }
            else
            {   Adapter.EventUse();
                SELECT( objectsList.SelectMany( o => o ).ToArray(), false );
            }
            
            //  PIN = oldPIN;
        }
        
        
        
        adapter.RestoreGUI();
        adapter.ChangeGUI();
        
        //    bool nores = false;
        if ( rect.height > 0 )     // MonoBehaviour.print(objectsList);
        {   // if (scrollHeightOver) contentRect.width -= 15;
            Adapter.currentClipRect = rect;
            //  MonoBehaviour.print("ASD");
            if ( !scrollHeightOver || !animcomplete ) contentRect.height = rect.height;
            var brr = rect;
            // bgc.a /= 2;
            
            
            GUI.BeginClip( rect );
            scrollPos = GUI.BeginScrollView( new Rect( 0, 0, rect.width, rect.height ), scrollPos, contentRect, false, scrollHeightOver && animcomplete );
            // var list = objectsList;
            int i = -1;
            var needPad = sort.Key == SortType.Hierarchy;
            var oldID = id;
            foreach ( var gameObjectse in objectsList )
            {   for ( int j = 0 ; j < gameObjectse.Length ; j++ )
                {   i++;
                
                    if ( (i + 4) * itemH < scrollPos.y ) continue;
                    if ( (i - 4) * itemH > scrollPos.y + H )
                    {   goto stop;
                    }
                    
                    var OBJECT = gameObjectse[j];
                    
                    if ( adapter.pluginID == Initializator.HIERARCHY_ID && !OBJECT.go ) continue;
                    
                    drawIndex[OBJECT.id] = i;
                    
                    if ( m_columnOffset == -1 ) rect.Set( 0, itemH * i, contentRect.width * 0.65f, itemH - 1 );
                    else rect.Set( 0, itemH * i, contentRect.width * m_columnOffset, itemH - 1 );
                    
                    
                    
                    //  if (selectedO.ContainsKey( OBJECT.id ))
                    if ( adapter.IsSelected( OBJECT.id ) )
                    {   r = rect;
                        //rect.Set(0, ItemH * i, contentRect.width, ItemH);
                        r.width = contentRect.width;
                        Adapter.SelectRect( r, overrideSelect: EditorWindow.focusedWindow == this );
                    }
                    Color? labelColor = null;
                    /* if (Event.current.type == EventType.Repaint) {
                       var col = adapter.MOI.M_Colors.needdrawGetColor(OBJECT);
                       if (col != null) {
                         var oc = GUI.color;
                         var c = col[0];
                         GUI.color *= c;
                         r = rect;
                         r.width = contentRect.width;
                         GUI.DrawTexture( r, Texture2D.whiteTexture );
                         GUI.color = oc;
                    
                         labelColor = col[1];
                       }
                     }*/
                    
                    
                    //  var oldl = Adapter.GET_SKIN().button.padding.left;
                    // var oldT = Adapter.GET_SKIN().button.padding.top;
                    //  var oldB = Adapter.GET_SKIN().button.padding.bottom;
                    float PAD_LEFT = 0;
                    // Adapter.GET_SKIN().button.padding.top = 3;
                    //  Adapter.GET_SKIN().button.padding.bottom = 0;
                    if ( needPad && !OBJECT.ParentIsNull() )
                    {   if ( !parentsCount.ContainsKey( OBJECT.id ) ) parentsCount.Add( OBJECT.id, OBJECT.parentCount() );
                        var count =  parentsCount[OBJECT.id] ;
                        if ( count > 4 ) count = 4;
                        PAD_LEFT = count * 9;
                        /*  rect.x += 10;
                          rect.width -= 10;*/
                    }
                    else
                    {   PAD_LEFT = 9;
                    }
                    
                    // rect.y -= 1;
                    // rect.height -= 1;
                    
                    var c222 = button_left.normal.textColor;
                    if ( labelColor.HasValue )
                    {   var c = labelColor.Value;
                        if ( c.r != 0 || c.g != 0 || c.b != 0 || c.a != 0 ) /////////
                        {   //   c.a = Math.Max((byte)10, c.a); /////////
                            button_left.normal.textColor = labelColor.Value;
                        }/////////
                    }
                    
                    
                    if ( Event.current.type == EventType.MouseDown && Event.current.button == 0 && rect.Contains( Event.current.mousePosition ) )
                    {   var wp = GUIUtility.GUIToScreenPoint(new Vector2(rect.x, rect.y));
                    
                        if (!dragRect.HasValue )
                        {   PUSH_ONMOUSEUP( rawOnUp, this );
                        }
                        dragRect = new Rect( wp.x, wp.y, rect.width, rect.height );
                        var selection = Adapter.SELECTED_GAMEOBJECTS();
                        if ( selection.Any( s => s == OBJECT ) )
                        {   dragObjects = selection.Select( h => h.GetHardLoadObject() ).ToArray();
                        }
                        else
                        {   dragObjects = new[] { OBJECT.GetHardLoadObject() };
                        }
                    }
                    
                    
                    //DRAW
                    {   // drawContent.text = OBJECT.ToString();
                    
                        var label_rect = rect;
                        label_rect.x = PAD_LEFT + EditorGUIUtility.singleLineHeight;
                        label_rect.width -= PAD_LEFT + EditorGUIUtility.singleLineHeight;
                        
                        
                        var icon_rect = label_rect;
                        // icon_rect.x = Adapter.GET_SKIN().button.padding.left;
                        var size = (icon_rect.height - EditorGUIUtility.singleLineHeight) / 2;
                        //icon_rect.x += size;
                        icon_rect.y += size;
                        icon_rect.width = icon_rect.height = EditorGUIUtility.singleLineHeight;
                        
                        
                        //** var icon = Event.current.type == EventType.Repaint ? adapter.bottomInterface.GetContent( OBJECT ) : tempColorEmpty;
                        var icon = tempColorEmpty;
                        
                        
                        Adapter.TempColorClass tempColor = null;
                        if ( adapter.ENABLE_LEFTDOCK_PROPERTY && !adapter.DISABLE_DESCRIPTION( OBJECT ) )
                        {   /* var ir = rect;
                                 ir.width  = contentRect.width;*/
                            
                            tempDynamicRect.Set( new Rect( label_rect.x, label_rect.y, contentRect.width + contentRect.x - label_rect.x, label_rect.height ),
                                                 new Rect( rect.x + rect.width, rect.y, contentRect.width - (rect.x + rect.width), rect.height ),
                                                 false, OBJECT, true, 0 );
                                                 
                            //** tempColor = adapter.ColorModule.DrawBackground(null, null, tempDynamicRect, OBJECT, 1, resetFonts: false );
                        }
                        
                        label_rect.x += EditorGUIUtility.singleLineHeight;
                        label_rect.width -= EditorGUIUtility.singleLineHeight;
                        
                        
                        {
                        
                        
                            //                                 var al = Adapter.GET_SKIN().label.alignment;
                            //                                 Adapter.GET_SKIN().label.alignment = TextAnchor.MiddleLeft;
                            var oc = label.normal.textColor;
                            
                            /*  var lc = adapter.MOI.M_Colors.GetLabelColor( OBJECT );
                             if (lc != null)
                             {   Adapter.GET_SKIN().label.normal.textColor = lc.Value;
                             }*/
                            if ( tempColor != null && tempColor.HAS_LABEL_COLOR )
                            {   label.normal.textColor = tempColor.LABELCOLOR;
                            }
                            else
                            {   label.normal.textColor = adapter.labelStyle.normal.textColor;
                            }
                            
                            
                            // GUI.Label( ir_Ref, OBJECT.ToString() );
                            
                            
                            var c = GUI.color;
                            var c3 = c;
                            if ( !OBJECT.Active() ) c3.a /= 2f;
                            GUI.color = c3;
                            
                            if ( tempColor != null && tempColor.LABEL_SHADOW == true && tempColor.HAS_LABEL_COLOR )
                            {   var _oc2 =  label.normal.textColor;
                                //var c2 = oc;
                                var c2 = Color.black;
                                c2.a = _oc2.a;
                                label.normal.textColor = c2;
                                label_rect.y -= 0.5f;
                                label_rect.x -= 1f;
                                GUI.Label( label_rect, OBJECT.ToString(), label );
                                label.normal.textColor = _oc2;
                                label_rect.y += 0.5f;
                                label_rect.x += 1f;
                            }
                            
                            
                            GUI.Label( label_rect, OBJECT.ToString(), label );
                            GUI.color = c;
                            
                            
                            label.normal.textColor = oc;
                            // Adapter.GET_SKIN().label.alignment = al;
                            
                            // adapter.MOI.M_Colors.DrawBackground( ir_Ref, OBJECT, 2 );
                            
                            
                            
                        }
                        
                        
                        // adapter.MOI.M_Colors.DrawBackground( rect, OBJECT );
                        
                        
                        if ( icon.add_icon )
                        {   /* ir_Ref.x += ir.width;
                                 ir_Ref.width -= ir.width;*/
                            // var iconColor = adapter.MOI.M_Colors.GetColorForObject(OBJECT);
                            
                            var iconColor = icon.add_hasiconcolor ? icon.add_iconcolor : Color.white;
                            
                            
                            if ( !OBJECT.Active() ) iconColor.a /= 2;
                            var c = GUI.color;
                            GUI.color *= iconColor;
                            GUI.DrawTexture( icon_rect, icon.add_icon, ScaleMode.ScaleToFit );
                            GUI.color = c;
                        }
                        
                    }
                    //DRAW
                    
                    
                    var BUT = GUI.Button(rect, drawContent, button_left);
                    button_left.normal.textColor = c222;
                    
                    //   Adapter.GET_SKIN().button.padding.left = oldl;
                    // Adapter.GET_SKIN().button.padding.top = oldT;
                    //Adapter.GET_SKIN().button.padding.bottom = oldB;
                    if ( BUT )
                    {
                    
                        if ( Event.current.control )
                        {   var oldSel = (Adapter.SELECTED_GAMEOBJECTS() ?? new Adapter.HierarchyObject[0]).Where(o => drawIndex.ContainsKey(o.id)).ToList();
                            int findInd = oldSel.FindIndex(os => os.id == OBJECT.id);
                            if ( findInd != -1 ) oldSel.RemoveAt( findInd );
                            else oldSel.Add( OBJECT );
                            SELECT( oldSel.ToArray(), false );
                            //Selection.objects = oldSel.ToArray();
                            ONEFRAMEPIN = true;
                            
                        }
                        else if ( Event.current.shift )
                        {   var oldSel = (Adapter.SELECTED_GAMEOBJECTS() ?? new Adapter.HierarchyObject[0]).Where(o => drawIndex.ContainsKey(o.id)).ToList();
                            if ( oldSel.Count == 0 )
                                SELECT( new[] { OBJECT }, false );
                            //Selection.objects = new[] { OBJECT };
                            else
                            {   var min = oldSel.Min(o => drawIndex[o.id]);
                                var max = oldSel.Max(o => drawIndex[o.id]);
                                int findInd = i;
                                if ( findInd < min )     //Selection.objects = objectsList.SelectMany( o => o ).ToList().GetRange( findInd , max - findInd + 1 ).ToArray();
                                {   SELECT( objectsList.SelectMany( o => o ).ToList().GetRange( findInd, max - findInd + 1 ).ToArray(), false );
                                    LastDirection = 1;
                                }
                                else if ( findInd > max )       //Selection.objects = objectsList.SelectMany( o => o ).ToList().GetRange( min , findInd - min + 1 ).ToArray();
                                {   SELECT( objectsList.SelectMany( o => o ).ToList().GetRange( min, findInd - min + 1 ).ToArray(), false );
                                    LastDirection = 2;
                                }
                                else       //Selection.objects = objectsList.SelectMany( o => o ).ToList().GetRange( min , findInd - min + 1 ).ToArray();
                                {   if ( LastDirection == 2 || LastDirection == -1 )
                                    {   SELECT( objectsList.SelectMany( o => o ).ToList().GetRange( min, findInd - min + 1 ).ToArray(), false );
                                    }
                                    else
                                    {   SELECT( objectsList.SelectMany( o => o ).ToList().GetRange( findInd, max - findInd + 1 ).ToArray(), false );
                                    }
                                }
                                
                            }
                            
                            ONEFRAMEPIN = true;
                            
                        }
                        else
                        {   if ( USEDOUBLECLICKTOCLOSE ) ONEFRAMEPIN = true;
                            //Selection.objects = new[] { OBJECT };
                            SELECT( new[] { OBJECT }, true );
                        }
                        
                    }
                    
                    // if (!OBJECT.Active()) Adapter.FadeRect( rect );
                    
                    if ( m_columnOffset == -1 ) rect.Set( rect.x + rect.width, itemH * i, contentRect.width * 0.35f, itemH - 1 );
                    else rect.Set( rect.x + rect.width, itemH * i, contentRect.width * (1 - m_columnOffset), itemH - 1 );
                    GUI.BeginGroup( rect );
                    rect.y = rect.x = 0;
                    Adapter.INTERNAL_BOX( rect, "" );
                    // Adapter.GET_SKIN().label.alignment = s_labelAL;
                    m_callBackType.typeFillter = m_typeFilter;
                    m_callBackType.callFromExternal_objects = IN;
                    // MonoBehaviour.print(m_callBackType.typeFillter);
                    var oldEv = Event.current.type;
                    m_callBackType.Draw( rect, OBJECT );
                    if ( oldEv != Event.current.type ) ONEFRAMEPIN = true;
                    m_callBackType.callFromExternal_objects = null;
                    //  Adapter.GET_SKIN().button.alignment = TextAnchor.MiddleLeft;
                    GUI.EndGroup();
                }
                
                
                if ( oldID != id ) break;
            }
            
            
stop:;

            GUI.EndScrollView();
            GUI.EndClip();
            
            if ( Event.current.type == EventType.Repaint )
            {   if ( shadow == null ) shadow = adapter.InitializeStyle( "SHADOW", 0.25f, 0.25f, 0.25f, 0.25f );
                if ( shadow != null )
                {   var shRect = brr;
                    // shRect.height = Math.Max( rect.height, shadow.border.bottom * 2 );
                    if ( brr.height < shadow.border.bottom * 2 )
                    {   var ob = shadow.border.bottom;
                        shadow.border.bottom = shadow.border.top = (int)(brr.height / 2);
                        shadow.Draw( shRect, false, false, false, false );
                        shadow.border.bottom = ob;
                    }
                    else
                    {   shadow.Draw( shRect, false, false, false, false );
                    }
                }
            }
            
        }
        else       // nores = true;
        {
        }
        
        
        
        //             Adapter.GET_SKIN().button.alignment = s_butAl;
        //             Adapter.GET_SKIN().label.alignment = s_labelAL;
        //             Adapter.GET_SKIN().label.fontSize = s_labelFS;
        
        
        rect = scrrect;
        rect.y = position.height - bottomBut * 5;
        //  rect.y += rect.height + 4;
        rect.height = bottomBut;
        
        /* Adapter.GET_SKIN().button.fontSize = bF;
         var fs = Adapter.GET_SKIN().button.fontSize;
         Adapter.GET_SKIN().button.fontSize = Adapter.WINDOW_FONT_12();*/
        
        if ( IN.SKANNING )
        {   rect.height *= 3;
            GUI.DrawTexture( rect, Adapter.GetIcon( "BUTBLUE" ) );
            GUI.Label( rect, "Scanning...", adapter.STYLE_LABEL_10_middle );
            rect.width = rect.height;
            GUI.DrawTexture( rect, Adapter.LOADING_TEXTURE() );
        }
        else
        {   /* if (nores)
                 {   rect.height = EditorGUIUtility.singleLineHeight;
                     GUI.Label(rect, "There aren't matched objects found");
                 }
                 else*/
            {   if ( GUI.Button( rect, "Rescan for all Objects"/*.ToUpper()*/) )
                {
                
                    // Debug.Log(IN.analizeEnumerator == null);
                    //  if (IN.analizeEnumerator != null)
                    {   StoptBroadcasting();
                        ClearSortedLists_AndStartBroadCast( true );
                        CalcBroadCast( false );
                        // IN.analizeEnumerator = Utilities.AllSceneObjectsInterator( adapter ).GetEnumerator();
                        // IN.analizedObjects = new List<Adapter.HierarchyObject>();
                        // StartBroadcasting();
                    }
                    
                }
            }
            
            rect.y += rect.height;
            var n1 = EditorGUI.ToggleLeft(rect, "Include disabled objects", adapter.SEARCH_SHOW_DISABLED_OBJECT);
            rect.y += rect.height;
            var n2 = EditorGUI.ToggleLeft(rect, "Search uses only the root object of the clicked object", adapter.SEARCH_USE_ROOT_ONLY);
            if (n1 != adapter.SEARCH_SHOW_DISABLED_OBJECT || n2 != adapter.SEARCH_USE_ROOT_ONLY )
            {   adapter.SEARCH_USE_ROOT_ONLY = n2;
                adapter.SEARCH_SHOW_DISABLED_OBJECT = n1;
                adapter.SavePrefs();
                
                StoptBroadcasting();
                ClearSortedLists_AndStartBroadCast( true );
                CalcBroadCast( false );
            }
            
            rect.y += rect.height;
            EditorGUI.HelpBox( rect, "ctrl/shift to multiselection; ctrl+A to select all; doubleclick to close", MessageType.None );
            rect.y += rect.height;
            EditorGUI.HelpBox( rect, "leftclick to drag, ctrl+rightclick search in children", MessageType.None );
        }
        
        
        
        /* if (GUI.Button(rect, "Open Settings"))
         {
             Hierarchy.EventUse();
             SETUPROOT.showWindow();
         }*/
        //Adapter.GET_SKIN().button.fontSize = fs;
        
        GUI.EndGroup(); //END
        
        
        /* GUILayout.Label(m_title);
         GUILayout.Label("Filter: " + m_searchFilter);
        
         GUI.SetNextControlName("MyTextField");
         //textInput = EditorGUILayout.TextField(textInput);
        
         var oldS = Adapter.GET_SKIN().button.fontSize;
         Adapter.GET_SKIN().button.fontSize = 12;
        /*  if (GUILayout.Button("Ok"))
         {
             comformAction(textInput);
             CloseThis();
         }#1#
         Adapter.GET_SKIN().button.fontSize = oldS;*/
        
        
        Adapter.RestoreGUI();
        
        // if (!wasFocus)
        if (/*!m_PIN &&*/ Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.KeypadEnter || Event.current.keyCode == KeyCode.Escape) )
        {   Adapter.EventUseFast();
            CloseThis();
            adapter.SKIP_PREFAB_ESCAPE = true;
            
            /*EditorGUIUtility.ExitGUI();
            EditorGUIUtility.hotControl = 0;*/
        }
    }
    
    GUIContent drawContent = new GUIContent();
    
    protected override void Update()
    {
    
    
        CalcBroadCast();
        
        base.Update();
    }
    
    
    
    int LastDirection = -1;
    
    
    
    Adapter. DynamicRect __tempDynamicRect;
    Adapter.DynamicRect tempDynamicRect { get { return __tempDynamicRect ?? (__tempDynamicRect = new Adapter.DynamicRect() { adapter = adapter }); } }
    
    
    
    
    private void StoptBroadcasting()     // Debug.Log("StoptBroadcasting");
    {
    
        m_ERROR = false;
        m_WASSCAN = false;
        if ( IN == null ) return;
        // m_currentList = null;
        IN.SKANNING = false;
    }
    
    private void StartBroadcasting()
    {   if ( IN.SKANNING ) return;
    
        if ( IN.analizeEnumerator == null ) return;
        
        
        m_WASSCAN = true;
        IN.SKANNING = true;
        // m_currentList = Utilities.AllSceneObjectsInterator().GetEnumerator();
        CountProgress = Utilities.AllSceneObjectsInteratorCount( adapter );
        interator = currentIndex = 0;
        
        
        /* if (updateCache && IN.UpdateCache != null)
         {   IN.UpdateCache(IN);
             OnUpdateCache();
         }*/
        
    }
    
    //static int fdsHotControl = 0;
#pragma warning disable
    static double time;
    private int CountProgress;
    private int interator;
    private bool m_WASSCAN;
    private bool m_ERROR;
    private int currentIndex;
#pragma warning restore
    System.Diagnostics.Stopwatch WATCH_CLONE = System.Diagnostics.Stopwatch.StartNew();
    
    
    internal void CalcBroadCast( bool skipsize = false )
    {   if ( IN == null || !IN.SKANNING || IN.analizeEnumerator == null ) return;
    
    
        interator = 0;
        // fdsHotControl = 0;
        var stopped = false;
        
        WATCH_CLONE.Stop();
        WATCH_CLONE.Reset();
        bool start = false;
        double counter = 0;
        var dsbuse = !adapter.SEARCH_SHOW_DISABLED_OBJECT;
        var limit = adapter.IS_HIERARCHY() ?  0.04f : 0.06f;
        while ( true )
        {   if ( !start )
            {   WATCH_CLONE.Start();
                start = true;
            }
            
            
            
            var result = IN.analizeEnumerator.MoveNext();
            
            if ( !result ) break;
            var current = IN.analizeEnumerator.Current;
            
            // Debug.Log( "ASD" );
            
            if ( current != null )
            {   if ( !current.Validate() )
                {   currentIndex = Utilities.AllSceneObjectsInteratorProgress( adapter );
                    continue;
                }
                if ( adapter.pluginID == Initializator.HIERARCHY_ID && (current.go.hideFlags & Utilities.SearchFlags) == Utilities.SearchFlags )
                {   currentIndex = Utilities.AllSceneObjectsInteratorProgress( adapter );
                    continue;
                }
                
                
                if ( dsbuse && !current.Active() ) continue;
                
                
                currentIndex = Utilities.AllSceneObjectsInteratorProgress( adapter );
                // if (last != currentIndex) Repaint();
                /* if (Math.Abs(time - EditorApplication.timeSinceStartup) > 0.5f)
                 {
                     time = EditorApplication.timeSinceStartup;
                     Repaint();
                 }*/
                //currentIndex++;
                interator++;
                if ( IN.Valudator( current ) )
                {
                
                    RegistrateObject( current );
                }
            }
            
            
            
            start = false;
            //start = true;
            WATCH_CLONE.Stop();
            counter += WATCH_CLONE.ElapsedTicks / (double)System.Diagnostics.Stopwatch.Frequency;
            WATCH_CLONE.Reset();
            
            
            /*   if (par.HiperGraphParams.SCANPERFOMANCE != 1 &&
                   interator > (par.HiperGraphParams.SCANPERFOMANCE - 0.2f) * 1600 + 0.2f * 150)*/
            if ( counter > limit )     // MonoBehaviour.print("ASD");
            {   // counter = 0;
                if ( !skipsize ) UpdateSize();
                stopped = true;
                break;
            }
        }
        
        UpdateCostList();
        
        //   MonoBehaviour.print(dsc.TEXTUREobjects.Count + " " + dsc.OBJECTtexture.Count);
        if ( !stopped )     // MonoBehaviour.print("ASD");
        {   IN.SKANNING = false;
            m_ERROR = false;
            if ( !skipsize ) UpdateSize();
        }
        else { }
    }
    
}
}


