using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CloakingBox
{
    public class RoomCacheManager : MonoBehaviour
    {
        public GameObject RoomGO;
        public RoomCreator roomCreationManager;
        public float refreshTime = 0.5f;

        public void Awake()
        {
            RoomGO = RoomCreator.CreateBlankRoom();
            InvokeRepeating("UpdateRoom", 0.0f, refreshTime);
        }

        public void Update()
        {
            Resources.UnloadUnusedAssets();
        }

        public void UpdateRoom()
        {
            Mesh mesh = roomCreationManager.GetUpdatedMesh();
            UpdateRoom(RoomGO, mesh);
        }

        public void UpdateRoom(GameObject room, Mesh m)
        {
            room.name = retrieveRoomName();
            var mf = room.GetComponent<MeshFilter>();
            mf.mesh = m;

            var mc = room.GetComponent<MeshCollider>();

            mc.sharedMesh = m;
        }
        
        private static string retrieveRoomName()
        {
            GameObject RoomNameInputField = GameObject.Find("RoomNameInputField");
            if (RoomNameInputField != null)
            {
                var text = RoomNameInputField.GetComponent<UnityEngine.UI.InputField>().text;
                return text;
            }
            else
            {
                return RoomNameHolder.RoomName;
            }
            //return GameObject.Find("RoomNameInputField").GetComponent<UnityEngine.UI.InputField>().text;
        }
    }
}