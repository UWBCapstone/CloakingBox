using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CloakingBox
{
    public class CloakingBoxVisibilityManager : MonoBehaviour
    {
        public GameObject CloakingBox;
        public GameObject SpatialImagingPlane;

        public void Update()
        {
            if(CloakingBox == null)
            {
                CloakingBox = GameObject.Find("CloakingBox");
            }

            if(CloakingBox != null
                && SpatialImagingPlane != null)
            {
                CloakingBox.SetActive(SpatialImagingPlane.activeInHierarchy);
            }
        }
    }
}