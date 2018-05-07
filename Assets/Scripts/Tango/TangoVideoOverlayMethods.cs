using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_ANDROID || UNITY_EDITOR
namespace CloakingBox
{
    public enum TangoVideoOverlayMethods
    {
        NULL,
        Texture_ITangoCameraTexture,
        YUV_Texture_ExperimentalTangoVideoOverlay,
        Raw_Bytes_ITangoVideoOverlay,
        Texture_and_Raw_Bytes,
        YUV_Texture_and_Raw_Bytes,
        Texture_and_YUV_Texture,
        All
    }
}
#endif