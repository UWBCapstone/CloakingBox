using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_ANDROID || UNITY_EDITOR
using Tango;

namespace CloakingBox
{
    public class IMyTangoPermissionsManager : MonoBehaviour
    {
        /// <summary>
        /// Interface that manages permissions requests and responses for a tango application.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules",
            "SA1600:ElementsMustBeDocumented", Justification = "Interface for testing; methods documented on implementation.")]
        public interface ITangoPermissionsManager
        {
            HashSet<TangoApplication.PermissionsTypes> PendingRequiredPermissions { get; }

            MyPermissionRequestState PermissionRequestState { get; }

            bool IsPermissionsRequestPending { get; }

            void RequestPermissions();

            void OnPermissionResult(TangoApplication.PermissionsTypes permissionType, bool isGranted);

            void Reset();
        }
    }
}
#endif