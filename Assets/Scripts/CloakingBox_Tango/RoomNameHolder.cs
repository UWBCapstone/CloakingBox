using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CloakingBox
{
    public class RoomNameHolder : MonoBehaviour
    {
        private static bool created = false;
        private static string roomName_m;

        public string roomName_Debug;

        public void Awake()
        {
            roomName_m = "";
            DontDestroyOnLoad(this.gameObject);
            created = true;
        }

        public void Update()
        {
            if(RoomName != null)
            {
                roomName_Debug = RoomName;
            }
        }

        public void SetRoomName(string roomName)
        {
            RoomName = roomName;
            WorkflowDebugger.Log("Room Name Holder set room name as " + roomName);
        }

        public static string RoomName
        {
            get
            {
                return roomName_m;
            }
            set
            {
                roomName_m = value;
            }
        }
    }
}