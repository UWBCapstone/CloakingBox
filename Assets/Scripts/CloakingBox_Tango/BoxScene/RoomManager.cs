using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_ANDROID || UNITY_EDITOR
using Tango;

namespace CloakingBox.BoxScene
{
    public class RoomManager : MonoBehaviour, ITangoLifecycle
    {
        public string[] roomNames;
        private static AreaDescription[] roomArr_m;

        // Grab all the rooms, make them available
        public void Awake()
        {
            HandleTangoStuff();

            //roomArr_m = AreaDescription.GetList();
            //roomNames = new string[roomArr_m.Length];
            //List<string> roomNamesList = GetAvailableRoomNames();
            //for(int i = 0; i < roomNames.Length; i++)
            //{
            //    roomNames[i] = roomNamesList[i];
            //}

            WorkflowDebugger.Log("Room Manager started. Area Descriptions loaded...");
        }

        private void HandleTangoStuff()
        {
            var tangoManager = FindObjectOfType<TangoApplication>();
            if (tangoManager != null)
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
                roomArr_m = AreaDescription.GetList();
                roomNames = new string[roomArr_m.Length];
                List<string> roomNamesList = GetAvailableRoomNames();
                for (int i = 0; i < roomNames.Length; i++)
                {
                    roomNames[i] = roomNamesList[i];
                }
            }
            else
            {
                AndroidHelper.ShowAndroidToastMessage("Motion Tracking and Area Learning Permissions Needed to Load Tango...");
                AndroidHelper.AndroidQuit();
            }
        }

        public static List<string> GetAvailableRoomNames()
        {
            List<string> availableRoomNames = new List<string>();
            if (roomArr_m != null)
            {
                foreach (var room in roomArr_m)
                {
                    AreaDescription.Metadata metadata = room.GetMetadata();
                    availableRoomNames.Add(metadata.m_name);
                }
            }

            return availableRoomNames;
        }

        public static AreaDescription.Metadata GetMetadata(string roomName)
        {
            if (roomArr_m != null)
            {
                foreach (var room in roomArr_m)
                {
                    AreaDescription.Metadata metadata = room.GetMetadata();
                    string name = metadata.m_name;
                    if (name.Equals(roomName))
                    {
                        return metadata;
                    }
                }
            }

            return null;
        }

        public static AreaDescription GetRoom(string roomName)
        {
            if (roomArr_m != null)
            {
                foreach (var room in roomArr_m)
                {
                    AreaDescription.Metadata metadata = room.GetMetadata();
                    if (metadata.m_name.Equals(roomName))
                    {
                        return room;
                    }
                }
            }

            return null;
        }

        public void OnTangoServiceConnected() { }

        public void OnTangoServiceDisconnected() { }
    }
}
#endif