﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CloakingBox;

#if UNITY_ANDROID || UNITY_EDITOR
using Tango;

namespace CloakingBox.ScanNewRoom
{
    public class AreaLearningStartup : MonoBehaviour, ITangoLifecycle
    {
        public TangoApplication TangoManager;

        public void Start()
        {
            Init();
            WorkflowDebugger.Log("Tango Area Learning Startup started...");
        }

        public void Init()
        {
            TangoManager = FindObjectOfType<TangoApplication>();
            if (TangoManager != null)
            {
                TangoManager.Register(this);
                TangoManager.RequestPermissions();
            }
        }

        public void OnTangoPermissions(bool permissionsGranted)
        {
            if (permissionsGranted)
            {
                StartTangoApplication();
                WorkflowDebugger.Log("Tango application permissions accepted. Starting tango application...");
            }
        }

        /// <summary>
        /// Start the Tango application to scan a new room.
        /// </summary>
        public void StartTangoApplication()
        {
            TangoManager.Startup(null);
        }

        public void OnTangoServiceConnected()
        {
        }

        public void OnTangoServiceDisconnected()
        {
        }
    }
}
#endif