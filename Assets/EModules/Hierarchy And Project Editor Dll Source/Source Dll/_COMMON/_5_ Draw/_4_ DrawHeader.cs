
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



namespace EModules.EModulesInternal

{
internal partial class Adapter {

    // Dictionary<int, bool> skipCache =  new Dictionary<int, bool>();
    bool SKIPMODULE( Module drawModule, float FULLWIDTH, bool writeMode = false )
    {   // if ( writeMode || !drawModule.wasAnyStart )
        // {
        drawModule.write_enable();
        drawModule.write_sibbildPos();
        drawModule.write_width();
        drawModule.wasAnyStart = true;
        
        
        /*  Debug.Log( drawModule.enable );
          Debug.Log( drawModule.DRAW_AS_COLUMN() );
          Debug.Log( par.RIGHTDOCK_TEMPHIDE );
          Debug.Log( FULLWIDTH <= par.RIGHTDOCK_TEMPHIDEMINWIDTH );
          Debug.Log( par.COMPONENTS_NEXT_TO_NAME && drawModule is IModuleOnnector_M_CustomIcons );
          Debug.Log( drawModule.SKIP() );
          Debug.Log( drawModule .GetType().Name);*/
        // if ( !skipCache.ContainsKey( drawModule.identifire ) ) skipCache.Add( drawModule.identifire, true );
        var result =/* IS_PREFAB_MOD_OPENED() && !(drawModule is Hierarchy.M_Vertices) ||*/ !drawModule.enable || !drawModule.DRAW_AS_COLUMN() || (par.RIGHTDOCK_TEMPHIDE
                && FULLWIDTH <= par.RIGHTDOCK_TEMPHIDEMINWIDTH/* && !(drawModule is M_Freeze)*/)
                || par.COMPONENTS_NEXT_TO_NAME && drawModule is IModuleOnnector_M_CustomIcons || drawModule.SKIP();
        // skipCache[drawModule.identifire] = result;
        // }
        // if ( !skipCache.ContainsKey( drawModule.identifire ) ) return true;
        // return skipCache[drawModule.identifire];
        return result;
        
        /*
                    if (!drawModule.enable || !drawModule.enableOverride() || (par.RIGHTDOCK_TEMPHIDE && width <= Hierarchy.par.RIGHTDOCK_TEMPHIDEMINWIDTH/* && !(drawModule is M_Freeze)#1#)
                         || par.COMPONENTS_NEXT_TO_NAME && drawModule is M_CustomIcons || drawModule.SKIP()) continue;
        
                    if (!drawModule.enable
                                //  || (par.RIGHTDOCK_TEMPHIDE && window().position.width <= Hierarchy.par.RIGHTDOCK_TEMPHIDEMINWIDTH/* && !(drawModule is M_Freeze)#1#)
                                || (par.RIGHTDOCK_TEMPHIDE && width <= Hierarchy.par.RIGHTDOCK_TEMPHIDEMINWIDTH/* && !(drawModule is M_Freeze)#1#)
                             || par.COMPONENTS_NEXT_TO_NAME && drawModule is M_CustomIcons || drawModule.SKIP()) continue;*/
    }
    
    //internal Action CACHER_ACTION;
    
    void CalcHeaderRects( Rect selectionRect )
    {   var currentRect = selectionRect;
        currentRect.x = currentRect.x + currentRect.width - padding_right - PREFAB_BUTTON_SIZE;
        var width = selectionRect.x + selectionRect.width;
        var needReapaint = false;
        var w = window();
        // if (w == null) return;
        
        
        
        
        
        
        
        //if (CACHER_ACTION != null) CACHER_ACTION();
        
        
        
        
        
        
        
        
        // int i = 0;
        
        foreach ( var drawModule in modulesOrdered )
        {   /* if (!drawModule.enable || (par.RIGHTDOCK_TEMPHIDE && width <= Hierarchy.par.RIGHTDOCK_TEMPHIDEMINWIDTH/* && !(drawModule is M_Freeze)#1#)
                     || par.COMPONENTS_NEXT_TO_NAME && drawModule is M_CustomIcons || drawModule.SKIP()) continue;*/
            /*  if ( !drawModule.SKIP_BAKED.HasValue )*/ drawModule.SKIP_BAKED = SKIPMODULE( drawModule, width, true );
            if ( drawModule.SKIP_BAKED.Value  ) continue;
            
            
            currentRect.width = Math.Max( drawModule.width, defWDTH );
            currentRect.x -= currentRect.width;
            
            if ( currentRect.width < 1 ) continue;
            
            
            currentRect = ClipMINSizeRect( currentRect, width );
            
            
            
            
            if ( mouseEvent != null || mouseEventDrag != null )
            {   CurrentRectInit( w, drawModule, currentRect );
                // if (!CurrentRectContainsKey(w,drawModule)) CurrentRect.Add(drawModule, currentRect);
                
                if ( Event.current.type == EventType.Repaint )     //  if (lastEditTime == 0)
                {   //var delts = EditorApplication.timeSinceStartup - lastEditTime;
                    var v = CurrentRect(w, drawModule);
                    // v.x = Mathf.Lerp(CurrentRect[drawModule].x, currentRect.x, (float)delts * 3.6f);
                    v.x = width - v.x;
                    currentRect.x = width - currentRect.x;
                    v.x = Mathf.MoveTowards( v.x, currentRect.x, deltaTime * 1300.6f );
                    currentRect.x = width - currentRect.x;
                    v.x = width - v.x;
                    CurrentRectSet( w, drawModule, v );
                    
                    if ( CurrentRect( w, drawModule ).x != currentRect.x )
                    {   needReapaint = true;
                    }
                }
            }
            else       // MonoBehaviour.print("ASD");
            {   CurrentRectSet( w, drawModule, currentRect );
                //  if (++i == 1) EditorGUI.DrawRect(currentRect, Color.white);
                if ( currentRect.x < Event.current.mousePosition.x && currentRect.x + currentRect.width > Event.current.mousePosition.x )
                {   ENABLE_HOVER_ITEMS = false;
                    // ENABLE_HOVER_ITEMS_WIN = w;
                }
                
            }
            
        }
        
        if ( needReapaint )
        {   RepaintWindow(true);
        }
    }
    
    static Color colCache;
    internal static Color grayFreezee = new Color( 0.2f, 0.2f, 0.2f, 1 );
#pragma warning disable
    bool ENABLE_HOVER_ITEMS = true;
#pragma warning restore
    // EditorWindow ENABLE_HOVER_ITEMS_WIN = null;
    
    void DrawHeader( Rect selectionRect )     // GUI.depth = 51;
    {   // selectionRect.height = EditorGUIUtility.singleLineHeight;
        /* if (parLINE_HEIGHT != EditorGUIUtility.singleLineHeight)
         {
             var h = selectionRect;
             h.y += EditorGUIUtility.singleLineHeight;
             h.height -= EditorGUIUtility.singleLineHeight;
             if (Event.current.type == EventType.repaint) EditorStyles.foldout.Draw(h, false, false, false, false);
             EditorGUIUtility.FindTexture(EditorResourcesUtility.folderIconName));
         }*/
        
        var currentRect = selectionRect;
        currentRect.x = currentRect.x + currentRect.width - padding_right;
        bool needReapaint = false;
        
        var oldG1 = GUI.color;
        var oldG2 = GUI.contentColor;
        
        var width = selectionRect.x + selectionRect.width ;
        var w = window();
        
        /*
                    if (par.DataKeeperParams.ENABLE)
                    {
                        modules[12].Draw(currentRect, o);
                        currentRect.x -= currentRect.width;
                    }*/
        
        
        foreach ( var drawModule in
                  modulesOrdered )     //  if (!drawModule.enable || (par.RIGHTDOCK_TEMPHIDE && window().position.width <= Hierarchy.par.RIGHTDOCK_TEMPHIDEMINWIDTH/* && !(drawModule is M_Freeze)*/)
        {   //if (drawModule.sibbildPos == -1) MonoBehaviour.print("ASD");
            // if ( SKIPMODULE( drawModule, width ) ) continue;
            if ( !drawModule.SKIP_BAKED.HasValue ) drawModule.SKIP_BAKED = SKIPMODULE( drawModule, width, true );
            if ( drawModule.SKIP_BAKED.Value ) continue;
            /*   if (!drawModule.enable || !drawModule.enableOverride() || (par.RIGHTDOCK_TEMPHIDE && width <= Hierarchy.par.RIGHTDOCK_TEMPHIDEMINWIDTH/* && !(drawModule is M_Freeze)#1#)
                   || par.COMPONENTS_NEXT_TO_NAME && drawModule is M_CustomIcons || drawModule.SKIP()) continue;*/
            //  MonoBehaviour.print(drawModule.width);
            // GUI.DrawTexture(currentRect,Texture2D.whiteTexture);
            
            // var last = modules.Max(m => m.enable ? m.sibbildPos : -1);
            /*     var last = modules.Max(m => m.enable ? m.sibbildPos : -1);
                    var lastModule = modules.FirstOrDefault(m => m.sibbildPos == last);
                    if (last == -1) lastModule = null;
                    if (lastModule != null &&CurrentRect.ContainsKey(lastModule) && CurrentRect[lastModule].x>)*/
            
            // currentRect.width = drawModule.sibbildPos == -1 ? drawModule.STATIC_WIDTH() : Math.Max(drawModule.width, defWDTH);
            currentRect.width = Math.Max( drawModule.width, defWDTH );
            currentRect.x -= currentRect.width;
            
            
            
            bool fade = (currentRect.x < par.PADDING_LEFT);
            currentRect = ClipMINSizeRect( currentRect, width );
            
            
            if ( currentRect.width < 1 ) continue;
            
            
            
            FadeSceneRect( currentRect, par.HEADER_OPACITY ?? DefaultBGOpacity );
            
            
            if ( fade )
            {   var c = GUI.color;
                c.a = Mathf.Lerp( 0, 1, currentRect.width / drawModule.width );
                GUI.color = c;
                c = GUI.contentColor;
                c.a = Mathf.Lerp( 0, 1, currentRect.width / drawModule.width );
                GUI.contentColor = c;
            }
            
            // FadeRect(currentRect, 1);
            
            
            var rightPos = currentRect.x + currentRect.width;
            
            
            // if (!CurrentRect.ContainsKey(drawModule)) CurrentRect.Add(drawModule, currentRect);
            
            
            
            /*    if (Event.current.type == EventType.layout) {
                    //  if (lastEditTime == 0)
                    //var delts = EditorApplication.timeSinceStartup - lastEditTime;
                    var v = CurrentRect[drawModule];
                    // v.x = Mathf.Lerp(CurrentRect[drawModule].x, currentRect.x, (float)delts * 3.6f);
                    v.x = Mathf.MoveTowards(CurrentRect[drawModule].x, currentRect.x, deltaTime * 300.6f);
                    CurrentRect[drawModule] = v;
            
                    if (CurrentRect[drawModule].x != currentRect.x) {
                        needReapaint = true;
                    }
                }*/
            
            CurrentRectInit( w, drawModule, currentRect );
            
            var drawRect = currentRect;
            drawRect.x = CurrentRect( w, drawModule ).x;
            
            /* if (drawModule.sibbildPos == last && par.PADDING_LEFT < CurrentRect[drawModule].x)
             {
                 if (drawRect.width - (par.PADDING_LEFT - CurrentRect[drawModule].x) > 0)
                 {
                     //MonoBehaviour.print(par.PADDING_LEFT);
            
                     drawRect.width -= par.PADDING_LEFT - CurrentRect[drawModule].x;
                     drawRect.x = par.PADDING_LEFT;
                 }
             }*/
            
            //if (drawModule.sibbildPos == -1) MonoBehaviour.print( currentRect + " " + drawRect + " " + drawModule.GetType().Name);
            
            
            GUI.BeginClip( drawRect );
            drawRect.x = 0;
            drawRect.y = 0;
            
            if ( drawModule.headOverrideTexture != null )
            {   var oldC = GUI.color;
                var c = GUI.color;
                c.a = 0.4f;
                GUI.color = c;
                Adapter.DrawTexture( drawRect, drawModule.headOverrideTexture.Value );
                GUI.color = oldC;
                
            }
            
            
            if ( drawModule.HeaderTexture2D != null )
            {   content.image = null;
                content.text = "";
                content.tooltip = drawModule.ContextHelper;
                if ( ENABLED_OPTIMIZER ) content.tooltip += "\nEnabled Broadcast Memory Optimizer!\n(This may reduce performance)";
                GUI.  Label( drawRect, content, STYLE_LABEL_10_middle);
                
                
                var oldW = drawRect.width;
                var oldH = drawRect.height;
                drawRect.width = 13;
                drawRect.height = 13;
                drawRect.x += (oldW - drawRect.width) / 2;
                drawRect.y += (oldH - drawRect.width) / 2;
                
                bool needBack = false;
                if ( !EditorGUIUtility.isProSkin && drawModule is IModuleOnnector_M_Freeze )
                {   colCache = GUI.color;
                    GUI.color = grayFreezee * oldG1;
                    needBack = true;
                }
                Adapter.DrawTexture( drawRect, GetIcon( drawModule.HeaderTexture2D ) );
                if ( needBack ) GUI.color = oldG1 * colCache;
                
                
            }
            else
            {   content.image = null;
                content.text = drawModule.HeaderText;
                content.tooltip = drawModule.ContextHelper;
                if ( ENABLED_OPTIMIZER ) content.tooltip += "\nEnabled Broadcast Memory Optimizer!\n(This may reduce performance)";
                var rrr = drawRect;
                rrr.width += 100;
                rrr.x -= 50;
                GUI. Label( rrr, content, STYLE_LABEL_10_middle );
            }
            /*   if (GUI.Button(drawRect, ""))
               {
                   drawModule.CallHeader(selectionRect);
               }*/
            
            
            drawRect = currentRect;
            drawRect.x = 0;
            /* drawRect.x = TOTAL_LEFT_PADDING;
             drawRect.width -= TOTAL_LEFT_PADDING;*/
            drawRect.y = 0;
            // drawRect.x += 4;
            
            /* if ( drawModule is Hierarchy.M_PlayModeKeeper ) {
                 Debug.Log( drawRect + " " + Event.current.mousePosition );
                 //Debug.Log( r.Contains( Event.current.mousePosition ) + " " + (Event.current.type == EventType.MouseDown) + " " + (Event.current.button == 0) );
                 //Event.current.type == EventType.Layout
             }*/
            
            var r = drawRect;
            r.x += 6;
            r.width -= 9;
            if ( !drawModule.disableSib ) EditorGUIUtility.AddCursorRect( r, MouseCursor.MoveArrow );
            
            // GUI.Button(r,"");
            //  if (r.Contains(Event.current.mousePosition) &&Event.current.type == EventType.mouseDown) MonoBehaviour.print("START " + mouseEventDrag + " " + mouseEvent);
            //MonoBehaviour.print(mouseEventDrag);
            
            
            
            if ( r.Contains( Event.current.mousePosition ) && Event.current.type == EventType.MouseDown && Event.current.button == 0 && mouseEventDrag == null && mouseEvent == null )
            {   curMod = drawModule;
                //  curMod.CreateUndo();
                // allow = true;
                EventUse();
                var startRect = currentRect;
                var startMouse = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
                var startX = startRect.x;
                var init = Hierarchy_GUI.Instance(this);
                if ( init != null ) Undo.RegisterCompleteObjectUndo( init, "Change Hierarchy" );
                DragRect = startRect;
                // if (init != null) Undo.RecordObject(init, "Change Hierarchy");
                // Hierarchy_GUI.Undo("Move Hierarchy Header");
                mouseEventW = window();
                mouseEventDrag = ( b, deltaTime ) =>
                {
                
                    /*   var cr = startRect;
                       cr.width = 80;
                       cr.height = 80;
                       cr.x = Event.current.mousePosition.x - 40;
                       cr.y = Event.current.mousePosition.y - 40;
                    
                       EditorGUIUtility.AddCursorRect(cr, MouseCursor.MoveArrow);*/
                    if ( !curMod.disableSib )
                    {
                    
                    
                    
                        DragRect = startRect;
                        DragRect.x = -(int)startMouse.x + (int)GUIUtility.GUIToScreenPoint( Event.current.mousePosition ).x + startX;
                        
                        //  LogProxy.Log(DragRect.x + " " + startRect.x);
                        var max = modules.Max(m => m.sibbildPos);
                        // if (DragRect.x + startRect.width * 0.65f < startRect.x)
                        var upnext = curMod.sibbildPos;
                        var cached_upnext = upnext;
                        while ( cached_upnext < max )
                        {   cached_upnext++;
                            var f = modules.FirstOrDefault(m => m.sibbildPos == cached_upnext);
                            if ( f == null || !f.enable || !f.DRAW_AS_COLUMN() ) continue;
                            upnext = cached_upnext;
                            break;
                        }
                        
                        //  if (DragRect.x > startRect.x + startRect.width * 0.65f)
                        var downnext = curMod.sibbildPos;
                        var cached_downnext = downnext;
                        while ( cached_downnext > 0 )
                        {   cached_downnext--;
                            var f = modules.FirstOrDefault(m => m.sibbildPos == cached_downnext);
                            if ( f == null || !f.enable || !f.DRAW_AS_COLUMN() ) continue;
                            downnext = cached_downnext;
                            // startRect.x += curMod.width;
                            break;
                        }
                        var next = curMod.sibbildPos;
                        if ( upnext != curMod.sibbildPos && DragRect.x - startRect.x < -modules.First( m => m.sibbildPos == upnext ).width * 0.6f ) next = upnext;
                        if ( downnext != curMod.sibbildPos && DragRect.x - startRect.x > +modules.First( m => m.sibbildPos == downnext ).width * 0.6f ) next = downnext;
                        
                        // MonoBehaviour.print((DragRect.x - startRect.x) + " " + modules.First(m => m.sibbildPos == downnext).width * 0.6f);
                        /*   MonoBehaviour.print(
                             (DragRect.x - startRect.x)
                             + " " + DragRect.x
                             + " " + startRect.x
                             + " " + downnext
                             + " " + modules.First(m => m.sibbildPos == downnext).width
                             + " " + modules.First(m => m.sibbildPos == downnext).batchType);*/
                        
                        if ( next != curMod.sibbildPos )     //  LogProxy.Log(next);
                        {   __modulesOrdered = null;
                        
                            // lastEditTime = EditorApplication.timeSinceStartup;
                            var target = modules.First(m => m.sibbildPos == next);
                            
                            if ( next < curMod.sibbildPos ) startRect.x += target.width;
                            else startRect.x -= target.width;
                            
                            target.sibbildPos = curMod.sibbildPos;
                            curMod.sibbildPos = next;
                        }
                        
                        if ( Event.current.delta.x != 0 )
                        {   allow = false;
                        }
                    }
                    
                    if ( b )     //MonoBehaviour.print("ASD");
                    {   if ( startMouse == GUIUtility.GUIToScreenPoint( Event.current.mousePosition ) )
                        {   mouseEventDrag = null;
                            ShowCategoryList( fade, menu =>
                            {   menu.AddItem( new GUIContent( "[ Hide This Module ☓ ]" ), false, () =>
                                {   curMod.CreateUndo();
                                    curMod.enable = false;
                                    curMod.SetDirty();
                                } );
                                menu.AddSeparator( "" );
                            }
                                            );
                            /*var menu = new GenericMenu();
                            menu.AddSeparator("");
                            menu.ShowAsContext();*/
                        }
                        curMod.SetDirty();
                    }
                    
                    
                    
                };
                RepaintWindow(true);
            }
            
            
            GUI.EndClip();
            
            if ( mouseEvent != null && curMod == drawModule && mouseEventW == w ) GUI.DrawTexture( currentRect, button.active.background );
            if ( mouseEventDrag != null && curMod == drawModule && mouseEventW == w ) GUI.DrawTexture( DragRect, button.active.background );
            
            
            drawRect = currentRect;
            drawRect.width = 1;
            
            r = drawRect;
            var oh = r.height;
            r.height /= 1.5f;
            r.y += (oh - r.height) / 2;
            Adapter.DrawTexture( r, colorText11 );
            
            drawRect.x -= 4;
            drawRect.width = 10;
            
            
            //   if (last != drawModule.sibbildPos)
            {   if ( !drawModule.disableSib ) EditorGUIUtility.AddCursorRect( drawRect, MouseCursor.SplitResizeLeftRight );
            
                if ( mouseEvent != null && curMod != null )
                {   var cr = drawRect;
                    cr.width = 200;
                    cr.height = 200;
                    cr.x = Event.current.mousePosition.x - 100;
                    cr.y = Event.current.mousePosition.y - 100;
                    
                    EditorGUIUtility.AddCursorRect( cr, MouseCursor.SplitResizeLeftRight );
                }
                
                if ( drawRect.Contains( Event.current.mousePosition ) && Event.current.type == EventType.MouseDown && Event.current.button == 0 && mouseEventDrag == null && mouseEvent == null )
                    //if (GUI.Button(drawRect, "") && Event.current.button == 0)
                {   allow = true;
                    curMod = drawModule;
                    //  curMod.CreateUndo();
                    EventUse();
                    
                    // if (init != null) Undo.RecordObject(init, "Change Hierarchy");
                    if ( !drawModule.disableSib )
                    {   var startMouse = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
                        var startWidth = (int)currentRect.width;
                        var init = Hierarchy_GUI.Instance(this);
                        if ( init != null ) Undo.RegisterCompleteObjectUndo( init, "Change Hierarchy" );
                        
                        mouseEventW = window();
                        
                        mouseEvent = ( b, deltaTime ) =>
                        {   var dif = (int)startMouse.x - (int)GUIUtility.GUIToScreenPoint(Event.current.mousePosition).x;
                        
                            /* var cr = drawRect;
                             cr.width = 80;
                             cr.height = 80;
                             cr.x = Event.current.mousePosition.x - 40;
                             cr.y = Event.current.mousePosition.y - 40;
                            
                             EditorGUIUtility.AddCursorRect(cr, MouseCursor.SplitResizeLeftRight);*/
                            
                            /*  if (CurrentRect.ContainsKey(curMod) && )
                              {
                                  // MonoBehaviour.print(CurrentRect[curMod].x + " " + GET_PADING);
                                  if (CurrentRect[curMod].x - dif < GET_PADING)
                                  {
                                      par.PADDING_LEFT -= GET_PADING - (CurrentRect[curMod].x - dif);
                                   /*   if (CurrentRect[curMod].x - dif < GET_PADING)
                                          dif = (int)(CurrentRect[curMod].x - GET_PADING);#1#
                                      // curMod.width -= (int)(par.PADDING_LEFT - GET_PADING);
                                  }
                              }*/
                            
                            curMod.width = dif + startWidth;
                            
                            if ( curMod.width > rightPos - 20 )
                            {   curMod.width = (int)rightPos - 20;
                            }
                            if ( curMod.width < defWDTH ) curMod.width = defWDTH;
                            
                            if ( b )
                            {
                            
                            
                                curMod.SetDirty();
                                // SavePrefs();
                                // Hierarchy.RepaintWindow();
                            }
                            else if ( Event.current.delta.x != 0 )
                            {   allow = false;
                                // Hierarchy.RepaintWindow();
                            }
                        };
                    }
                    
                    RepaintWindow(true);
                }
                
            }
            
            
            // Adapter. INTERNAL_BOX(
            drawRect = currentRect;
            drawRect.x = CurrentRect( w, drawModule ).x;
            drawRect.y = 0;
            
            
            //  if (Event.current.type != EventType.mouseDown) GUI.Button(currentRect, "");
            
            // if(currentRect.Contains(Event.current.mousePosition) && Event.current.type == EventType.mouseDown) MonoBehaviour.print(Event.current.type);
            if ( currentRect.Contains( Event.current.mousePosition ) && Event.current.type == EventType.MouseDown && Event.current.button == 1 && mouseEventDrag == null && mouseEvent == null )
            {   EventUse();
            
                var captureRect = currentRect;
                var captureModule = drawModule;
                if ( !controlIDsAndOnMouseUp.ContainsKey( captureModule.sibbildPos ) )
                {
                
                    PUSH_EVENT_HELPER_RAW();
                    controlIDsAndOnMouseUp.Add( captureModule.sibbildPos, () =>
                    {   if ( captureRect.Contains( Event.current.mousePosition ) )
                        {
                        
                            controlIDsAndOnMouseUp.Remove( captureModule.sibbildPos );
                            RepaintWindowInUpdate();
                            
                            EventUse();
                            /*  int[] contentCost = new int[0];
                              GameObject[] obs = new GameObject[0];*/
                            bool error = false;
                            // if (EditorSceneManager.GetActiveScene().rootCount != 0)
                            /*  {
                                  error = !captureModule.CallHeader();
                              }*/
                            
                            if ( !error )
                            {
                            
                            
                            
                            
                                var capturedModule = captureModule;
                                // FocusRoot win = null;
                                /*   Func<float, float> topGui = (topWidth) => {
                                       float Y = 10;
                                       var oldFont = Adapter.GET_SKIN().button.fontSize;
                                       Adapter.GET_SKIN().button.fontSize = WINDOW_FONT_12();
                                
                                
                                       GUI.DrawTexture(new Rect(0, Y, topWidth, 25), REDT);
                                
                                       if (GUI.Button(new Rect(0, Y, topWidth, 25), "Hide This Category"))
                                       {
                                           capturedModule.CreateUndo();
                                           capturedModule.enable = false;
                                           capturedModule.SetDirty();
                                           if (win != null) win.CloseThis();
                                       }
                                       Y += 30;
                                       GUI.DrawTexture(new Rect(10, Y, topWidth - 20, 1), colorStatic);
                                       Y += 5;
                                       if (GUI.Button(new Rect(0, Y, topWidth, 25), "Show Category List"))
                                       {
                                           ShowCategoryList();
                                       }
                                       Adapter.GET_SKIN().button.fontSize = oldFont;
                                       Y += 35;
                                       return Y;
                                   };*/
                                
                                /*win =*/
                                captureModule.callFromExternal_objects = null;
                                var result = captureModule.CallHeader();
                                // result.topGUI = null;
                                //MonoBehaviour.print((result != null) + " " + captureModule.GetType());
                                // EventUse();
                                if ( result != null )
                                {
                                
                                    var mp = new MousePos( Event.current.mousePosition, MousePos.Type.Search_356_0, true, this);
                                    _W__SearchWindow.Init(
                                        mp, capturedModule.SearchHelper, "All",  /*topGui*/result, capturedModule, this, null );
                                }
                            }
                        }
                    } );
                }
                RepaintWindow(true);
            }
            //  if (GUIUtility.hotControl != 0)
            
            if ( Event.current.type == EventType.Repaint && controlIDsAndOnMouseUp.ContainsKey( drawModule.sibbildPos ) )
            {   button.Draw( currentRect, "", true, true, false, true );
            }
            // if (controlIDsAndOnMouseUp.ContainsKey(drawModule.sibbildPos)) GUIUtility.hotControl = 0;
            
            
            
            
            /*    if (GUI.Button(drawRect, "")) {
                    // MonoBehaviour.print("ASD");
                    if (Event.current.button == 1) {
                        // GUIUtility.hotControl = 0;
            
            
                    }
                }*/
            
            GUI.color = oldG1;
            GUI.contentColor = oldG2;
        }
        GUI.color = oldG1;
        GUI.contentColor = oldG2;
        
        
        // currentRect = DragRect;
        currentRect.width = EditorGUIUtility.singleLineHeight;
        currentRect.x -= currentRect.width;
        
        /*    if (mouseEvent != null)
            {
                //currentRect.x = par.PADDING_LEFT - currentRect.width;
            }*/
        
        // FadeRectWhite(currentRect, 1);
        EditorGUIUtility.AddCursorRect( currentRect, MouseCursor.Link );
        FadeSceneRect( currentRect, par.HEADER_OPACITY ?? DefaultBGOpacity );
        
        
        var proprect = currentRect;
        if ( proprect.height > EditorGUIUtility.singleLineHeight )
        {   proprect.y += (proprect.height - EditorGUIUtility.singleLineHeight) / 2;
            proprect.height = EditorGUIUtility.singleLineHeight;
        }
        Adapter.DrawTexture( proprect, GetIcon( "PROP" ) );
        //  GUI.DrawTexture(currentRect, Adapter.GET_SKIN().box.normal.background, "");
        /**/
        
        if ( currentRect.Contains( Event.current.mousePosition ) && Event.current.type == EventType.MouseDown && Event.current.button == 0 && mouseEventDrag == null && mouseEvent == null )
            //if (GUI.Button(drawRect, "") && Event.current.button == 0)
        {   // MonoBehaviour.print(currentRect);
            curMod = null;
            //  curMod = drawModule;
            //  curMod.CreateUndo();
            allow = true;
            var startMouse = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
            var startWidth = GET_PADING(selectionRect.x + selectionRect.width);
            var init = Hierarchy_GUI.Instance(this);
            if ( init != null ) Undo.RegisterCompleteObjectUndo( init, "Change Hierarchy" );
            // if (init != null) Undo.RecordObject(init, "Change Hierarchy");
            
            // MonoBehaviour.print("ASD");
            var captureWidth = selectionRect.x + selectionRect.width;
            mouseEventW = window();
            
            mouseEvent = ( b, deltaTime ) =>
            {   if ( !b )
                {   if ( Event.current.delta.x != 0 )
                    {   allow = false;
                        RepaintWindow(true);
                        // Hierarchy.RepaintWindow();
                        // Hierarchy.RepaintWindow();
                    }
                }
                
                
                if ( !allow )
                {   par.PADDING_LEFT = startWidth - ((int)startMouse.x - (int)GUIUtility.GUIToScreenPoint( Event.current.mousePosition ).x);
                    if ( par.PADDING_LEFT > captureWidth - RIGHTPAD ) par.PADDING_LEFT = captureWidth - RIGHTPAD;
                }
                
                
                if ( b )
                {   SavePrefs();
                
                    if ( startMouse == GUIUtility.GUIToScreenPoint( Event.current.mousePosition ) )
                    {   mouseEventDrag = null;
                        ShowCategoryList( true );
                    }
                    //  Hierarchy.RepaintWindow();
                }
                
            };
            RepaintWindow(true);
        }
        
        
        if ( mouseEvent != null && !allow )
        {   var dr = currentRect;
            dr.y -= window().position.height;
            dr.width = selectionRect.x + selectionRect.width;
            // dr.width = window().position.width;
            dr.height = window().position.height * 2;
            dr.x += currentRect.width;
            
            if ( curMod == null )
            {   SelectRect( dr, 0.05f );
                //  GUI.DrawTexture(currentRect, Adapter.GET_SKIN().button.active.background);
            }
            
            var oldC = GUI.color;
            // GUI.color = Color.red;
            GUI.color *= new Color32( 255, 90, 80, 255 );
            dr.x = par.PADDING_LEFT - currentRect.width;
            dr.width = 1;
            GUI.DrawTexture( dr, Texture2D.whiteTexture );
            GUI.color = oldC;
            // GUI.DrawTexture(dr, );
        }
        if ( mouseEvent != null && curMod == null && mouseEventW == w )
        {   GUI.DrawTexture( currentRect, button.active.background );
        }
        
        
        //  Debug.Log(GUI.depth);
        LastHeaderRect = currentRect;
        LastHeaderRect.width = selectionRect.width + selectionRect.x - LastHeaderRect.x;
        
        
        if ( currentRect.Contains( Event.current.mousePosition ) && Event.current.type == EventType.MouseDown && Event.current.button == 1 && mouseEventDrag == null && mouseEvent == null )
        {   var captureRect = currentRect;
            if ( !controlIDsAndOnMouseUp.ContainsKey( 999 ) ) controlIDsAndOnMouseUp.Add( 999, () =>
            {   if ( captureRect.Contains( Event.current.mousePosition ) )
                {   ShowCategoryList( false );
                }
            } );
        }
        Button( currentRect, PropContent );
        if ( Event.current.type == EventType.Repaint ) button.Draw( currentRect, PropContent, false, controlIDsAndOnMouseUp.ContainsKey( 999 ), false, false );
        
        /* if (GUI.Button(currentRect, )) {
             if (allow) ShowCategoryList(true);
         }*/
        if ( needReapaint )
        {   RepaintWindow(true);
        }
    }
    GUIContent PropContent = new GUIContent() { tooltip = "Show Category List\nDrag the icon to change width" };
    
    Rect LastHeaderRect;
    
    
    
}
}
