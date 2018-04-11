using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CloakingBox
{
    public class CameraManager : MonoBehaviour
    {
        public GameObject MainCamera;
        public GameObject RenderTextureCamera;

        public void Awake()
        {
            SetCameraCullingMasks();
            SetCameraSettings();
            //SetRenderTexture();
        }

        public void SetCameraCullingMasks()
        {
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
                    mainCam.cullingMask = everythingLayerMask & ~(1 << roomLayerMask); // Show everything except for the room layer
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
                    renderCam.cullingMask = everythingLayerMask & ~(1 << boxLayerMask);
                }
            }
        }

        public void SetCameraSettings()
        {
            if(RenderTextureCamera != null)
            {
                Camera renderCam = RenderTextureCamera.GetComponent<Camera>();
                if(renderCam != null)
                {
                    renderCam.fieldOfView = 120.0f;
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