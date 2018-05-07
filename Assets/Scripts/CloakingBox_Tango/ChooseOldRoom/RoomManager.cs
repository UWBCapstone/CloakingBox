using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_ANDROID || UNITY_EDITOR
using Tango;

namespace CloakingBox.ChooseOldRoom
{
    public class RoomManager : MonoBehaviour
    {
        public AreaDescription[] roomArr_m;

        // Grab all the rooms, make them available
        public void Awake()
        {
            roomArr_m = AreaDescription.GetList();
        }

        public AreaDescription.Metadata GetMetadata(string roomName)
        {
            foreach(var room in roomArr_m)
            {
                AreaDescription.Metadata metadata = room.GetMetadata();
                string name = metadata.m_name;
                if (name.Equals(roomName))
                {
                    return metadata;
                }
            }

            return null;
        }

        public AreaDescription GetRoom(string roomName)
        {
            foreach(var room in roomArr_m)
            {
                AreaDescription.Metadata metadata = room.GetMetadata();
                if (metadata.m_name.Equals(roomName))
                {
                    return room;
                }
            }

            return null;
        }
    }
}
#endif