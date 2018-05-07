using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_ANDROID || UNITY_EDITOR
namespace CloakingBox {

    public class DeterminingPose : MonoBehaviour, IRelocalizingBehavior
    {
        #region Fields
        public bool Active = true;
        public GameObject DefaultRelocalizingUIObject;

        [HideInInspector]
        public RelocalizingHandler handler_m;
        [HideInInspector]
        public bool initialized_m = false;
        #endregion

        #region Initialization
        public void Start()
        {
            if (Active)
            {
                Init();
            }
        }

        private void Init()
        {
            handler_m = FindRelocalizingHandler();
            Register();
            initialized_m = true;
        }

        public RelocalizingHandler FindRelocalizingHandler()
        {
            GameObject handler = GameObject.Find(RelocalizingHandler.GameObjectName);
            RelocalizingHandler handlerScript = handler.GetComponent<RelocalizingHandler>();
            return handlerScript;
        }

        public void Register()
        {
            if (handler_m != null)
            {
                handler_m.RegisterRelocalizingBehavior(this);
            }
        }
        #endregion

        public void Update()
        {
            if (Active
                && !initialized_m)
            {
                Init();
            }
        }

        #region Behavior Handling
        #region OnPoseValid
        public void OnPoseValid()
        {
            if(DefaultRelocalizingUIObject != null)
            {
                DefaultRelocalizingUIObject.SetActive(false);
            }
        }
        #endregion

        #region OnPoseInvalid
        public void OnPoseInvalid()
        {
            if(DefaultRelocalizingUIObject != null)
            {
                DefaultRelocalizingUIObject.SetActive(true);
            }
        }
        #endregion
        #endregion
    }
}
#endif