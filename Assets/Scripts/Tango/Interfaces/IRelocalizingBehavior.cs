using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_ANDROID || UNITY_EDITOR
using Tango;

namespace CloakingBox
{
    public interface IRelocalizingBehavior
    {
        /// <summary>
        /// Triggers when the Tango realizes its pose
        /// </summary>
        void OnPoseValid();

        /// <summary>
        /// Triggers on startup and when the Tango has lost its pose tracking
        /// </summary>
        void OnPoseInvalid();
    }
}
#endif