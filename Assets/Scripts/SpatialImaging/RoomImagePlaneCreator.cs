using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CloakingBox
{
    public class RoomImagePlaneCreator : MonoBehaviour
    {
        public bool AutomaticGeneration = true;
        public float farClipPlaneRatio = 0.9f;
        public GameObject planePrefab;
        public Camera MainCamera;

        public void Awake()
        {
            if (AutomaticGeneration)
            {
                GenerateImagePlane();
            }
        }

        public GameObject GenerateImagePlane()
        {
            // Attach to Main Camera as child
            var plane = Object.Instantiate(planePrefab);
            plane.transform.parent = MainCamera.transform;
            // Adjust its scale (take into account camera ratio)
            var bounds = plane.GetComponent<MeshFilter>().mesh.bounds;
            float currentWidth = bounds.size.x;
            float currentHeight = bounds.size.z;
            plane.transform.localScale = calculatePlaneLocalScale(currentWidth, currentHeight);
            // Push it out from the camera appropriately
            plane.transform.localPosition = new Vector3(0, 0, MainCamera.farClipPlane * farClipPlaneRatio);
            // Set to the correct layer
            plane.layer = LayerManager.GetLayerMask(CloakLayers.RoomImage);

            // Attach this to the RoomImageUpdater & CloakingBoxVisibilityManager
            GameObject.FindObjectOfType<RoomImageUpdater>().roomImagePlane = plane;
            GameObject.FindObjectOfType<CloakingBoxVisibilityManager>().SpatialImagingPlane = plane;

            return plane;
        }

        private Vector3 calculatePlaneLocalScale(float currentWidth, float currentHeight)
        {
            float dis = MainCamera.farClipPlane * farClipPlaneRatio;
            float aspect = MainCamera.aspect;
            float verticalAngle = MainCamera.fieldOfView;
            float horizontalAngle = verticalAngle * aspect;
            float height = dis * Mathf.Tan(Mathf.Deg2Rad * verticalAngle / 2) * 2;
            float width = dis * Mathf.Tan(Mathf.Deg2Rad * horizontalAngle / 2) * 2;

            // x = plane width; z = plane height
            return new Vector3(width / currentWidth, 1, height / currentHeight);
        }
    }
}