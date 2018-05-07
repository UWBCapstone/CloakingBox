using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_ANDROID || UNITY_EDITOR
using Tango;

namespace CloakingBox
{
    public enum TangoPoseModes
    {
        MotionTracking,
        MotionTracking_withDriftCorrection,
        LocalAreaDescription_LoadExisting,
        LocalAreaDescription_Learning,
        CloudAreaDescription
    }
}
#endif