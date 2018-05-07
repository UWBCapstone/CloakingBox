using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

namespace CloakingBox.ScanNewRoom
{
    [CustomEditor(typeof(RoomCameraManager))]
    public class RoomCameraManagerEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            RoomCameraManager rcm = (RoomCameraManager)target;
            if(GUILayout.Button("Apply Viewport Settings"))
            {
                rcm.ApplyViewportSettings();
            }
            if(GUILayout.Button("Apply Background Color"))
            {
                rcm.ApplyBackgroundColor();
            }
        }
    }
}
#endif