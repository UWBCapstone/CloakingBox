using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_ANDROID || UNITY_EDITOR
using Tango;

namespace CloakingBox
{
    public class RelocalizingHandler : MonoBehaviour, ITangoPose, ITangoLifecycle
    {
        #region Fields
        private TangoApplication tangoManager_m;
        public List<IRelocalizingBehavior> RelocalizingBehaviorList;

        private static readonly string gameObjectName_m = "RelocalizingHandler";
        #endregion

        #region Methods
        public void Start()
        {
            name = GameObjectName;
            RelocalizingBehaviorList = new List<IRelocalizingBehavior>();
            SetTangoManager();
            RegisterScriptToTangoManager();
        }

        public void OnDestroy()
        {
            UnregisterScriptToTangoManager();
        }

        #region Tango Interfacing
        public void RegisterScriptToTangoManager()
        {
            if(tangoManager_m != null)
            {
                tangoManager_m.Register(this);
            }
        }

        public void UnregisterScriptToTangoManager()
        {
            if(tangoManager_m != null)
            {
                tangoManager_m.Unregister(this);
            }
        }

        public void OnTangoPoseAvailable(TangoPoseData pose)
        {
            if(pose.framePair.baseFrame == TangoEnums.TangoCoordinateFrameType.TANGO_COORDINATE_FRAME_AREA_DESCRIPTION
                && pose.framePair.targetFrame == TangoEnums.TangoCoordinateFrameType.TANGO_COORDINATE_FRAME_DEVICE)
            {
                if(pose.status_code == TangoEnums.TangoPoseStatusType.TANGO_POSE_VALID)
                {
                    OnPoseValid();
                }
                else
                {
                    OnPoseInvalid();
                }
            }
        }

        public void OnTangoServiceConnected()
        {
            OnPoseInvalid();
        }

        public void OnTangoPermissions(bool permissionsGranted)
        {

        }

        public void OnTangoServiceDisconnected()
        {

        }
        #endregion

        #region Init
        public void SetTangoManager()
        {
            if(tangoManager_m == null)
            {
                var tangoManagerObj = GameObject.Find("Tango Manager");
                if(tangoManagerObj != null)
                {
                    tangoManager_m = tangoManagerObj.GetComponent<TangoApplication>();
                }
                else
                {
                    Debug.LogError("Tango Manager not found for Relocalizing Handler. Once pose is found upon starting application, logic will not trigger.");
                }
            }
        }
        #endregion

        #region Relocalizing Behavior
        public void OnPoseValid()
        {
            foreach(var behaviorScript in RelocalizingBehaviorList)
            {
                behaviorScript.OnPoseValid();
            }
        }

        public void OnPoseInvalid()
        {
            foreach(var behaviorScript in RelocalizingBehaviorList)
            {
                behaviorScript.OnPoseInvalid();
            }
        }

        public void RegisterRelocalizingBehavior(IRelocalizingBehavior behavior)
        {
            if (!RelocalizingBehaviorList.Contains(behavior))
            {
                RelocalizingBehaviorList.Add(behavior);
            }
        }

        public void UnregisterLocalizingBehavior(IRelocalizingBehavior behavior)
        {
            if (RelocalizingBehaviorList.Contains(behavior))
            {
                RelocalizingBehaviorList.Remove(behavior);
            }
        }
        #endregion

        #endregion

        #region Properties
        public static string GameObjectName
        {
            get
            {
                return gameObjectName_m;
            }
        }
        #endregion
    }
}
#endif