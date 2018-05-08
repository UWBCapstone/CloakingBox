using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

namespace CloakingBox.BoxScene
{
    [CustomEditor(typeof(CloakingBoxCreator))]
    public class CloakingBoxCreatorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            CloakingBoxCreator creator = (CloakingBoxCreator)target;
            if(GUILayout.Button("Create Box using Raycast w/ Room"))
            {
                creator.GenerateCloakingBox();
            }
            if(GUILayout.Button("Create Box 3m in Front of Main Camera"))
            {
                creator.GenerateCloakingBox(Camera.main.transform.position + Camera.main.transform.forward * 3.0f);
            }
        }
    }
}
#endif