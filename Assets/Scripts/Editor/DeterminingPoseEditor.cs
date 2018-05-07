using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

namespace CloakingBox
{
    [CustomEditor(typeof(DeterminingPose))]
    public class DeterminingPoseEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("Displays default splash screen when active and the Tango is searching for its pose / orientation.", MessageType.Info);
            DrawDefaultInspector();
        }
    }
}
#endif