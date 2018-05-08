﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CloakingBox
{
    public class CameraManager : MonoBehaviour
    {
        public GameObject MainCamera;
        public GameObject RenderTextureCamera;
        public GameObject RoomCamera;

        public void Awake()
        {
            SetCameraCullingMasks();
            SetCameraSettings();
            //SetRenderTexture();
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
                    int roomLayerMask = LayerManager.GetLayerMask(CloakLayers.Room);
                    int debugLayerMask = LayerManager.GetLayerMask(CloakLayers.Debug);
                    //mainCam.cullingMask = everythingLayerMask & ~(1 << roomLayerMask); // Show everything except for the room layer
                    mainCam.cullingMask = everythingLayerMask & ~(1 << roomLayerMask) & ~(1 << debugLayerMask); // Show everything except for the room layer
                }
            }
            if (RenderTextureCamera != null)
            {
                Camera renderCam = RenderTextureCamera.GetComponent<Camera>();
                if (renderCam != null)
                {
                    // Make the box invisible to the render camera (should still have blackness shown for background since it is a depth clear flag on the render camera
                    int everythingLayerMask = ~0;
                    int boxLayerMask = LayerManager.GetLayerMask(CloakLayers.Box);
                    int debugLayerMask = LayerManager.GetLayerMask(CloakLayers.Debug);
                    //renderCam.cullingMask = everythingLayerMask & ~(1 << boxLayerMask);
                    renderCam.cullingMask = everythingLayerMask & ~(1 << boxLayerMask) & ~(1 << debugLayerMask);
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
                    roomCam.cullingMask = (1 << roomLayerMask) & (1 << debuglayerMask);
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