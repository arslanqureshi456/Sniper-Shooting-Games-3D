using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;
#if PROJECT
    using EModules.Project;
#endif


namespace EModules.EModulesInternal

{
internal partial class Adapter {
    [InitializeOnLoad]
    internal class MultyThead {
        //static MultyThead _get;
        /*  internal static MultyThead get {
              get { return _get ?? (_get = GameObject.FindObjectOfType<MultyThead>()); }
          }
          void Awake()
          {
              _get = this;
        
        
          }*/
        
        static MultyThead()
        {   EditorApplication.update -= updater;
            EditorApplication.update += updater;
            if (myJob != null)
                foreach (var threadedJob in myJob)
                {   threadedJob.Abort();
                }
            myJob.Clear();
        }
        
        static void updater()
        {   if (myJob.Count == 0) return;
            myJob.RemoveAll(j => j.Update());
        }
        
        static List<ThreadedJob> myJob = new List<ThreadedJob>();
        
        static internal Job CreateJob(Action action, Action onFinished, Action invokeInLock)
        {   var j = new Job() { action = action, onFinish = onFinished, invokeInLock = invokeInLock };
            myJob.Add(j);
            
            return j;
        }
        
        
        
        static internal void Remove(ThreadedJob j)
        {   myJob.Remove(j);
        }
        static internal bool Contains(ThreadedJob j)
        {   return myJob.Contains(j);
        }
    }
    
    
    internal class Job : ThreadedJob {
        internal Action action;
        internal Action onFinish;
        internal Action invokeInLock;
        
        protected override void ThreadFunction()
        {   action();
        }
        protected override void OnFinished()
        {   onFinish();
        }
        protected override void InvokeInLock(Action ac)
        {   onFinish();
        }
    }
    
    
    
    internal class ThreadedJob {
        internal bool RequestStop = false;
        private bool m_IsDone = false;
        private object m_Handle = new object();
        private System.Threading.Thread m_Thread = null;
        public bool IsDone
        {   get
            {   bool tmp;
                lock (m_Handle)
                {   tmp = m_IsDone;
                }
                return tmp;
            }
            set
            {   lock (m_Handle)
                {   m_IsDone = value;
                }
            }
        }
        
        protected virtual void InvokeInLock(Action ac)
        {   lock (m_Handle)
            {   ac();
            }
        }
        
        public virtual void Start()
        {   m_Thread = new System.Threading.Thread(Run);
        
            m_Thread.Start();
        }
        public virtual void Abort()
        {   lock (m_Handle)
            {   RequestStop = true;
            }
            
        }
        public bool IsAlive()
        {   return !IsDone && MultyThead.Contains(this);
        }
        
        protected virtual void ThreadFunction() { }
        
        protected virtual void OnFinished() { }
        
        public virtual bool Update()
        {   if (IsDone)
            {   MultyThead.Remove(this);
                OnFinished();
                return true;
            }
            return false;
        }
        public IEnumerator WaitFor()
        {   while (!Update())
            {   yield return null;
            }
        }
        private void Run()
        {   ThreadFunction();
            IsDone = true;
        }
    }
}
}



/*[CustomEditor(typeof(GameObject))]
[CanEditMultipleObjects]
public class funBoy : Editor
{
    public void OnSceneGUI()
    {
        Handles.BeginGUI();

        if (GUILayout.Button("Press Me"))
            Debug.Log("Got it to work.");

        Handles.EndGUI();
    }
}*/
