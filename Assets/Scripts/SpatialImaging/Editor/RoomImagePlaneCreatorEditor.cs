using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

namespace CloakingBox
{
    [CustomEditor(typeof(RoomImagePlaneCreator))]
    public class RoomImagePlaneCreatorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            RoomImagePlaneCreator creator = (RoomImagePlaneCreator)target;
            if(GUILayout.Button("Generate Plane and Assign to Camera"))
            {
                creator.GenerateImagePlane();
            }
        }
    }
}