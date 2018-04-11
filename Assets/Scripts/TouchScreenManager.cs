using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CloakingBox
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
            // Shoot a ray from the Tango camera to the room reconstruction
            GameObject room = RoomManager.GetRoom();
            if(room == null)
            {
                DebugMsg = "Room was not retrieved and reconstructed properly.";
            }

            GameObject RenderTextureCamera = camManager.RenderTextureCamera;
            Ray r = new Ray(RenderTextureCamera.transform.position, RenderTextureCamera.transform.forward);
            RaycastHit hitInfo;
            Physics.Raycast(r, out hitInfo, float.MaxValue, LayerManager.GetLayerMask(CloakLayers.Box));

            // For debugging purposes
            lastHitPosition = hitInfo.point;

            GameObject portal = cloakingBoxCreator.GenerateCloakingBox(hitInfo.point);
            //GameObject portal = cloakingBoxCreator.GenerateCloakingBox(new Vector3());
            if (portal.activeInHierarchy)
            {
                DebugMsg = "Portal constructed and should be visible.";
            }
        }

        public void OnGUI()
        {
            GUI.color = Color.black;
            string str = string.Format("LastHitPosition: {0}", lastHitPosition);
            GUI.Label(new Rect(100, 100, 1000, 100), str);

            string debugGUI = string.Format("DEBUG MSG: {0}", DebugMsg);
            GUI.Label(new Rect(200, 200, 1000, 100), debugGUI);
        }
    }
}