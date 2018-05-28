using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CloakingBox
{
    public class CameraManager : MonoBehaviour
    {
        public GameObject MainCamera;
        public GameObject RenderTextureCamera;
        public GameObject RoomCamera;

        #region Layers To Ignore
        public CloakLayers MainCamIgnoreLayers1 = CloakLayers.Room;
        public CloakLayers MainCamIgnoreLayers2 = CloakLayers.Debug;
        public CloakLayers MainCamIgnoreLayers3 = CloakLayers.RoomImage;

        public CloakLayers RenderCamIgnoreLayers1 = CloakLayers.Box;
        public CloakLayers RenderCamIgnoreLayers2 = CloakLayers.Debug;
        public CloakLayers RenderCamIgnoreLayers3 = CloakLayers.Room;

        public CloakLayers RoomCameraLayers1 = CloakLayers.Room;
        public CloakLayers RoomCameraLayers2 = CloakLayers.Debug;
        public CloakLayers RoomCameraLayers3 = CloakLayers.NULL;
        #endregion

        public void Awake()
        {
            //DebugLogMainCameraAspectRatio();
            SetCameraCullingMasks();
            SetCameraSettings();
            //SetRenderTexture();
        }

        private void DebugLogMainCameraAspectRatio()
        {
            Debug.Log("Main camera aspect ratio = " + MainCamera.GetComponent<Camera>().aspect);
        }

        public void SetCameraCullingMasks()
        {
            WorkflowDebugger.Log("Setting camera culling masks...");

            // Make the render texture camera show the right stuff
            // Make the normal camera show the right stuff
            if (MainCamera != null)
            {
                Camera mainCam = MainCamera.GetComponent<Camera>();
                if (mainCam != null)
                {
                    // Make room invisible to the main camera
                    int everythingLayerMask = ~0;
                    //int roomLayerMask = LayerManager.GetLayerMask(CloakLayers.Room);
                    //int debugLayerMask = LayerManager.GetLayerMask(CloakLayers.Debug);
                    //int roomImageLayerMask = LayerManager.GetLayerMask(CloakLayers.RoomImage);
                    ////mainCam.cullingMask = everythingLayerMask & ~(1 << roomLayerMask); // Show everything except for the room layer
                    //mainCam.cullingMask = everythingLayerMask & ~(1 << roomLayerMask) & ~(1 << debugLayerMask) & ~(1 << roomImageLayerMask); // Show everything except for the room layer, debug lines layer, and room image layer

                    int mask1Con = ((MainCamIgnoreLayers1 != CloakLayers.NULL) ? ~(1 << LayerManager.GetLayerMask(MainCamIgnoreLayers1)) : ~(0));
                    int mask2Con = ((MainCamIgnoreLayers2 != CloakLayers.NULL) ? ~(1 << LayerManager.GetLayerMask(MainCamIgnoreLayers2)) : ~(0));
                    int mask3Con = ((MainCamIgnoreLayers3 != CloakLayers.NULL) ? ~(1 << LayerManager.GetLayerMask(MainCamIgnoreLayers3)) : ~(0));
                    mainCam.cullingMask = everythingLayerMask & mask1Con & mask2Con & mask3Con;
                }
            }
            if (RenderTextureCamera != null)
            {
                Camera renderCam = RenderTextureCamera.GetComponent<Camera>();
                if (renderCam != null)
                {
                    // Make the box invisible to the render camera (should still have blackness shown for background since it is a depth clear flag on the render camera
                    int everythingLayerMask = ~0;
                    //int boxLayerMask = LayerManager.GetLayerMask(CloakLayers.Box);
                    //int debugLayerMask = LayerManager.GetLayerMask(CloakLayers.Debug);
                    //int roomLayerMask = LayerManager.GetLayerMask(CloakLayers.Room);
                    ////renderCam.cullingMask = everythingLayerMask & ~(1 << boxLayerMask);
                    //renderCam.cullingMask = everythingLayerMask & ~(1 << boxLayerMask) & ~(1 << debugLayerMask) & ~(1 << roomLayerMask);

                    int mask1Con = ((RenderCamIgnoreLayers1 != CloakLayers.NULL) ? ~(1 << LayerManager.GetLayerMask(RenderCamIgnoreLayers1)) : ~(0));
                    int mask2Con = ((RenderCamIgnoreLayers2 != CloakLayers.NULL) ? ~(1 << LayerManager.GetLayerMask(RenderCamIgnoreLayers2)) : ~(0));
                    int mask3Con = ((RenderCamIgnoreLayers3 != CloakLayers.NULL) ? ~(1 << LayerManager.GetLayerMask(RenderCamIgnoreLayers3)) : ~(0));
                    renderCam.cullingMask = everythingLayerMask & mask1Con & mask2Con & mask3Con;
                }
            }
            if(RoomCamera != null)
            {
                Camera roomCam = RoomCamera.GetComponent<Camera>();
                if(roomCam != null)
                {
                    // Make everything but the room and the debug invisible to the render camera
                    int roomLayerMask = LayerManager.GetLayerMask(CloakLayers.Room);
                    int debuglayerMask = LayerManager.GetLayerMask(CloakLayers.Debug);
                    roomCam.cullingMask = (1 << roomLayerMask) | (1 << debuglayerMask);

                    int mask1 = ((RoomCameraLayers1 != CloakLayers.NULL) ? (1 << LayerManager.GetLayerMask(RoomCameraLayers1)) : 0);
                    int mask2 = ((RoomCameraLayers2 != CloakLayers.NULL) ? (1 << LayerManager.GetLayerMask(RoomCameraLayers2)) : 0);
                    int mask3 = ((RoomCameraLayers3 != CloakLayers.NULL) ? (1 << LayerManager.GetLayerMask(RoomCameraLayers3)) : 0);
                    roomCam.cullingMask = mask1 | mask2 | mask3;
                }
            }
        }

        public void SetCameraSettings()
        {
            if (MainCamera != null)
            {
                Camera mainCam = MainCamera.GetComponent<Camera>();
                if (RenderTextureCamera != null)
                {
                    Camera renderCam = RenderTextureCamera.GetComponent<Camera>();
                    if (renderCam != null)
                    {
                        WorkflowDebugger.Log("Setting Render camera's field of view to match main camera's field of view...");

                        //renderCam.fieldOfView = 120.0f;
                        renderCam.fieldOfView = mainCam.fieldOfView;

                        WorkflowDebugger.Log("Setting Render camera's aspect ratio to match main camera's aspect ratio...");
                        renderCam.aspect = mainCam.aspect;

                        WorkflowDebugger.Log("Setting Render Texture to correct size to match aspect ratio...");
                        renderCam.targetTexture.width = (int)(renderCam.targetTexture.height * renderCam.aspect);

                        GUIDebug.Log("FOV = " + renderCam.fieldOfView + "; Aspect = " + renderCam.aspect + "; targetTextureWidth = " + renderCam.targetTexture.width);
                    }
                }
            }
        }

        public RenderTexture SetRenderTexture()
        {
            if(RenderTextureCamera != null)
            {
                Camera renderCam = RenderTextureCamera.GetComponent<Camera>();
                if(renderCam != null)
                {
                    WorkflowDebugger.Log("Setting Render camera's render texture dynamically...");

                    RenderTexture rt = renderCam.targetTexture;
                    if(rt == null)
                    {
                        // render texture not set
                        rt = new RenderTexture(1024, 1024, 0);
                        rt.dimension = UnityEngine.Rendering.TextureDimension.Tex2D;
                        rt.antiAliasing = 8; // might be '3' for 8 samples (None, 2, 4, 8 samples)
                    }
                    return rt;
                }
            }

            return null;
        }
    }
}