using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CloakingBox
{
    public class GUIDebug : MonoBehaviour
    {
        public bool Active = false;
        public Color debugColor = Color.black;
        public static string DebugMsg = "";
        public static string lastHitPosition;

        private static GUIStyle style;
        private static int textSize_m = 14;

        public void Awake()
        {
            setStyle();
        }

        private void setStyle()
        {
            style = new GUIStyle();
            style.fontSize = textSize_m;
            style.wordWrap = true;
        }

        public static void LogHitPosition(string hitpos)
        {
            lastHitPosition = hitpos;
        }

        public static void Log(string msg)
        {
            DebugMsg = msg;
        }

        public void OnGUI()
        {
            if (Active)
            {
                GUI.color = debugColor;

                string str = string.Format("LastHitPosition: {0}", lastHitPosition);
                GUI.Label(new Rect(100, 100, 1000, 100), str, style);

                string debugGUI = string.Format("DEBUG MSG: {0}", DebugMsg);
                GUI.Label(new Rect(200, 200, 1000, 100), debugGUI, style);
            }
        }
    }
}