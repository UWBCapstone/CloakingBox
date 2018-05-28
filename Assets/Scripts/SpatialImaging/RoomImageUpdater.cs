using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CloakingBox
{
    /// <summary>
    /// The purpose of this class is to update the room image plane every frame by grabbing the current Pose of the camera, checking if there's an image, and pushing that to the image plane if appropriate.
    /// </summary>
    public class RoomImageUpdater : MonoBehaviour
    {
        public bool DeactivatePlaneOnBadImage = true;
        public GameObject roomImagePlane;
        public PoseVoxelSet poseVoxelSet;
        public PoseImageDictionary poseImageDictionary;

        public void Update()
        {
            Texture2D tex = GetTexture();
            UpdateTexture(tex);
        }

        public Texture2D GetTexture()
        {
            // Get current camera pose
            Pose camPose = new Pose(Camera.main);
            PoseVoxel voxel = poseVoxelSet.ToPoseVoxel(camPose);
            if (poseVoxelSet.Contains(voxel))
            {
                // Grab the associated texture from the dictionary
                var tex = poseImageDictionary[voxel];
                return tex;
            }

            return null;
        }

        public void UpdateTexture(Texture2D tex)
        {
            if (tex != null)
            {
                roomImagePlane.SetActive(true);

                Material mat = roomImagePlane.GetComponent<MeshRenderer>().material;

                Texture2D oldTex = (Texture2D)mat.GetTexture("_MainTex");
                mat.SetTexture("_MainTex", tex);

                if (oldTex != null)
                {
                    ReleaseTexture(oldTex);
                }
            }
            else
            {
                if (DeactivatePlaneOnBadImage)
                {
                    roomImagePlane.SetActive(false);
                }
            }
        }

        public void ReleaseTexture(Texture2D tex)
        {
            GameObject.Destroy(tex);
        }
    }
}