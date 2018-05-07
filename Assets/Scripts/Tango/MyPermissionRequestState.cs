using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_ANDROID || UNITY_EDITOR
using Tango;

namespace CloakingBox
{
    /// <summary>
    /// State of the permission request process.
    /// </summary>
    public enum MyPermissionRequestState
    {
        NONE = 0,
        PERMISSION_REQUEST_INIT = 1,
        REQUEST_ANDROID_PERMISSIONS = 2,
        BIND_TO_SERVICE = 3,
        ALL_PERMISSIONS_GRANTED = 4,
        SOME_PERMISSIONS_DENIED = 5,
    }
}
#endif