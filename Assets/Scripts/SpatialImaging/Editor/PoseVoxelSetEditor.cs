using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

namespace CloakingBox
{
    [CustomEditor(typeof(PoseVoxelSet))]
    public class PoseVoxelSetEditor : Editor
    {
        public float xWidth = 0.025f;
        public float yWidth = 0.025f;
        public float zWidth = 0.025f;
        public float xDegree = 15f;
        public float yDegree = 15f;
        public float zDegree = 15f;

        public override void OnInspectorGUI()
        {
            PoseVoxelSet set = (PoseVoxelSet)target;
            set.Debugging = GUILayout.Toggle(set.Debugging, "Debugging");

            // Draw Boundary limits
            DisplayBoundaryLimits();
            set.xWidth = xWidth;
            set.yWidth = yWidth;
            set.zWidth = zWidth;
            set.xDegree = xDegree;
            set.yDegree = yDegree;
            set.zDegree = zDegree;

            //DrawDefaultInspector();
            DisplayDebuggingInfo(set);
        }

        private void DisplayBoundaryLimits()
        {
            EditorGUILayout.LabelField("Boundary Limits");
            EditorGUI.indentLevel = 2;

            Vector3 widths = new Vector3(xWidth, yWidth, zWidth);
            widths = EditorGUILayout.Vector3Field("Dimensions", widths);
            xWidth = widths.x;
            yWidth = widths.y;
            zWidth = widths.z;
            Vector3 degrees = new Vector3(xDegree, yDegree, zDegree);
            degrees = EditorGUILayout.Vector3Field("Rotation Dimensions", degrees);
            xDegree = degrees.x;
            yDegree = degrees.y;
            zDegree = degrees.z;
            EditorGUILayout.Space();
        }

        private void DisplayDebuggingInfo(PoseVoxelSet set)
        {
            //using (new EditorGUI.DisabledScope(map.Debugging == false))
            if (set.Debugging)
            {
                foreach(var voxel in set.Items)
                {
                    DisplayVoxel(voxel);
                    EditorGUILayout.Space();
                }
            }
        }

        private void DisplayVoxel(PoseVoxel voxel)
        {
            int oldIndentLevel = EditorGUI.indentLevel;

            // Display each voxel
            EditorGUI.indentLevel = 2;
            EditorGUILayout.LabelField("Voxel:");
            EditorGUILayout.Vector3Field("Min Pos", voxel.Min.Position);
            EditorGUILayout.Vector3Field("Max Pos", voxel.Max.Position);
            EditorGUILayout.Vector3Field("Min Euler", voxel.Min.Rotation.eulerAngles);
            EditorGUILayout.Vector3Field("Max Euler", voxel.Max.Rotation.eulerAngles);

            EditorGUI.indentLevel = oldIndentLevel;
        }
    }
}