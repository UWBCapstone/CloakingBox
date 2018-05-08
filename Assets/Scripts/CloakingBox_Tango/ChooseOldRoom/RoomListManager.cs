using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_ANDROID || UNITY_EDITOR
using Tango;

namespace CloakingBox.ChooseOldRoom
{
    public class RoomListManager : MonoBehaviour, ITangoLifecycle
    {
        public GameObject roomListObject;

        private UnityEngine.UI.Dropdown roomListDropdown_m;

        public void Awake()
        {
            WorkflowDebugger.Log("Awakening Room List Manager. Initializing room list dropdown...");

            HandleTangoStuff();

            roomListDropdown_m = roomListObject.GetComponent<UnityEngine.UI.Dropdown>();
            //initRoomListDropdown();
        }

        private void initRoomListDropdown()
        {
            // Set the label
            // Set the options

            roomListDropdown_m.ClearOptions();

            var roomNameList = RoomManager.GetAvailableRoomNames();
            roomListDropdown_m.AddOptions(roomNameList);
        }

        private void HandleTangoStuff()
        {
            var tangoManager = FindObjectOfType<TangoApplication>();
            if(tangoManager != null)
            {
                tangoManager.Register(this);
                if (AndroidHelper.IsTangoCorePresent())
                {
                    tangoManager.RequestPermissions();
                }
            }
            else
            {
                Debug.Log("No Tango Manager found in scene.");
            }
        }

        public void OnTangoPermissions(bool permissionsGranted)
        {
            if (permissionsGranted)
            {
                Invoke("initRoomListDropdown", 0.5f);
                //initRoomListDropdown();
            }
            else
            {
                AndroidHelper.ShowAndroidToastMessage("Motion Tracking and Area Learning Permissions Needed to Load Tango...");
                AndroidHelper.AndroidQuit();
            }
        }

        public void OnTangoServiceConnected() { }

        public void OnTangoServiceDisconnected() { }
    }
}

#endif