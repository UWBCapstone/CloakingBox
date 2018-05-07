using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tango
{
    public partial class TangoApplication : MonoBehaviour, ITangoApplication
    {
        public void MimicAutoConnectToService()
        {
            if(m_permissionsManager.PermissionRequestState == PermissionRequestState.NONE)
            {
                m_permissionsManager.RequestPermissions();
            }

            if(m_permissionsManager.PermissionRequestState == PermissionRequestState.ALL_PERMISSIONS_GRANTED)
            {
                Startup(null);
            }
        }

        public void MimicAutoConnectToService(AreaDescription room)
        {
            if (m_permissionsManager.PermissionRequestState == PermissionRequestState.NONE)
            {
                m_permissionsManager.RequestPermissions();
            }

            if (m_permissionsManager.PermissionRequestState == PermissionRequestState.ALL_PERMISSIONS_GRANTED)
            {
                Startup(room);
            }
        }

        public CloakingBox.MyPermissionRequestState GetAndroidPermissionsState()
        {
            switch (m_permissionsManager.PermissionRequestState)
            {
                case PermissionRequestState.ALL_PERMISSIONS_GRANTED:
                    return CloakingBox.MyPermissionRequestState.ALL_PERMISSIONS_GRANTED;
                case PermissionRequestState.BIND_TO_SERVICE:
                    return CloakingBox.MyPermissionRequestState.BIND_TO_SERVICE;
                case PermissionRequestState.PERMISSION_REQUEST_INIT:
                    return CloakingBox.MyPermissionRequestState.PERMISSION_REQUEST_INIT;
                case PermissionRequestState.REQUEST_ANDROID_PERMISSIONS:
                    return CloakingBox.MyPermissionRequestState.REQUEST_ANDROID_PERMISSIONS;
                case PermissionRequestState.SOME_PERMISSIONS_DENIED:
                    return CloakingBox.MyPermissionRequestState.SOME_PERMISSIONS_DENIED;
                case PermissionRequestState.NONE:
                    return CloakingBox.MyPermissionRequestState.NONE;
            }

            return CloakingBox.MyPermissionRequestState.NONE;
        }
    }
}