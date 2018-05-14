using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Tango;

public class AreaLearningStartup_Standalone : MonoBehaviour, ITangoLifecycle
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
            Invoke("StartTangoApplication", 0.5f);
            //StartTangoApplication();
            WorkflowDebugger.Log("Tango application permissions accepted. Starting tango application...");
        }
    }

    /// <summary>
    /// Start the Tango application to scan a new room.
    /// </summary>
    public void StartTangoApplication()
    {
        //string roomNameToLoad = RoomNameHolder.RoomName;
        //AreaDescription room = RoomManager.GetRoom(roomNameToLoad);

        //TangoManager.Startup(room);

        TangoManager.Startup(null);
    }

    public void OnTangoServiceConnected()
    {
    }

    public void OnTangoServiceDisconnected()
    {
    }
}
