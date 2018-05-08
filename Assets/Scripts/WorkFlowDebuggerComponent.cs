using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkFlowDebuggerComponent : MonoBehaviour {
    public bool Debug = false;

    public void Update()
    {
        WorkflowDebugger.Active = Debug;
    }
}
