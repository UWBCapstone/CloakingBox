using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CloakingBox.BoxScene
{
    public class TouchScreenManager : MonoBehaviour
    {
        public CameraManager camManager;
        public static CloakingBoxCreator cloakingBoxCreator;
        public static Vector3 lastHitPosition = new Vector3();
        public static string DebugMsg = "";
        
        public void Awake()
        {
            cloakingBoxCreator = GameObject.Find("CloakingBoxCreatorManager").GetComponent<CloakingBoxCreator>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.touchSupported)
            {
                bool touched = false;

                if(Input.touches.Length > 0)
                {
                    foreach(Touch touch in Input.touches)
                    {
                        if(touch.phase == TouchPhase.Began)
                        {
                            touched = true;
                            break;
                        }
                    }
                }

                if (touched)
                {
                    OnTap();
                }
            }
        }

        public void OnTap()
        {
            cloakingBoxCreator.GenerateCloakingBox();
        }
    }
}