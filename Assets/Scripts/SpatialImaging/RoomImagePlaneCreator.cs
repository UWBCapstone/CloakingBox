using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CloakingBox
{
    public class RoomImagePlaneCreator : MonoBehaviour
    {
        public GameObject planePrefab;
        public Camera MainCamera;

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
            plane.transform.localPosition = new Vector3(0, 0, MainCamera.farClipPlane);
            // Set to the correct layer
            plane.layer = LayerManager.GetLayerMask(CloakLayers.RoomImage);

            // Attach this to the RoomImageUpdater
            GameObject.FindObjectOfType<RoomImageUpdater>().roomImagePlane = plane;

            return plane;
        }

        private Vector3 calculatePlaneLocalScale(float currentWidth, float currentHeight)
        {
            float dis = MainCamera.farClipPlane;
            float aspect = MainCamera.aspect;
            float verticalAngle = MainCamera.fieldOfView;
            float horizontalAngle = verticalAngle * aspect;
            float height = dis * Mathf.Tan(Mathf.Deg2Rad * verticalAngle);
            float width = dis * Mathf.Tan(Mathf.Deg2Rad * horizontalAngle);

            // x = plane width; z = plane height
            return new Vector3(width / currentWidth, 1, height / currentHeight);
        }
    }
}