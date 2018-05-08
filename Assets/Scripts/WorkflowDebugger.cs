using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WorkflowDebugger
{
    public static bool Active = false;

    public static void Log(string message)
    {
        if (Active)
        {
            Debug.Log(message);
        }
    }
}
