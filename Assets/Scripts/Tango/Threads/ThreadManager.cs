using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Threading;

//#if UNITY_ANDROID || UNITY_EDITOR
//using Tango;

namespace CloakingBox
{
    public delegate void OnJoinAction();

    public class ThreadManager : MonoBehaviour
    {
        public int ActiveSideThreads = 0;

        public static List<Thread> ThreadList = new List<Thread>();
        public static Dictionary<Thread, OnJoinAction> OnJoinActions = new Dictionary<Thread, OnJoinAction>();

        public void Update()
        {
            ActiveSideThreads = ThreadList.Count;

            List<Thread> inactiveThreads = new List<Thread>();
            foreach(Thread t in ThreadList)
            {
                if (!t.IsAlive)
                {
                    if (OnJoinActions.ContainsKey(t))
                    {
                        OnJoinAction action = OnJoinActions[t];
                        action();
                    }
                    inactiveThreads.Add(t);
                }
            }

            foreach(var it in inactiveThreads)
            {
                ThreadList.Remove(it);
                OnJoinActions.Remove(it);
            }
        }

        public static void Register(Thread t)
        {
            if (ThreadList != null)
            {
                if (!ThreadList.Contains(t))
                {
                    ThreadList.Add(t);
                }
            }
        }

        public static void UnRegister(Thread t)
        {
            if (ThreadList != null)
            {
                if (ThreadList.Contains(t))
                {
                    if (t.ThreadState == ThreadState.Stopped)
                    {
                        t.Join();
                    }
                    else
                    {
                        t.Abort();
                    }
                    ThreadList.Remove(t);
                }
            }
        }
        
        public static void RegisterOnJoin(Thread t, OnJoinAction action)
        {
            if (!OnJoinActions.ContainsKey(t))
            {
                OnJoinActions.Add(t, action);
            }
        }

        public static void UnregisterOnJoin(Thread t)
        {
            if (OnJoinActions.ContainsKey(t))
            {
                OnJoinActions.Remove(t);
            }
        }

        public static void AbortAll()
        {
            foreach (var thread in ThreadList)
            {
                if (thread.ThreadState == ThreadState.Stopped)
                {
                    thread.Join();
                    if (OnJoinActions.ContainsKey(thread))
                    {
                        OnJoinAction action = OnJoinActions[thread];
                        action();
                    }
                }
                else
                {
                    thread.Abort();
                }
            }

            ThreadList.Clear();
        }

        public void OnApplicationQuit()
        {
            AbortAll();
        }

    }
}
//#endif