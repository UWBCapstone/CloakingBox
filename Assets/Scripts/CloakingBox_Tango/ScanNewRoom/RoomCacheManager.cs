using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CloakingBox
{
    public class RoomCacheManager : MonoBehaviour
    {
        public GameObject StopReadingRoomButton;
        //public GameObject RoomGO;
        public List<GameObject> RoomGOs;
        public RoomCreator roomCreationManager;
        public float refreshTime = 0.5f;

        public void Awake()
        {
            //RoomGO = RoomCreator.CreateBlankRoom();
            RoomGOs = new List<GameObject>();
            //InvokeRepeating("UpdateRoom", 0.0f, refreshTime);
            //Invoke("UpdateRoom", 6.0f);
        }

        public void Update()
        {
            Resources.UnloadUnusedAssets();
        }

        public void UpdateRoom()
        {
            //Mesh mesh = roomCreationManager.GetUpdatedMesh();
            //UpdateRoom(RoomGO, mesh);

            List<Mesh> meshes = roomCreationManager.GetUpdatedMeshes();
            for(int i = 0; i < meshes.Count; i++)
            {
                GameObject room = RoomCreator.CreateBlankRoom();
                room.name += "_" + i;

                UpdateRoom(room, meshes[i]);
                RoomGOs.Add(room);
            }

            //var stopReadingRoomButton = GameObject.Find("StopReadingRoomButton");
            if(StopReadingRoomButton != null)
            {
                StopReadingRoomButton.SetActive(false);
            }
            else
            {
                GUIDebug.Log("Couldn't find StopReadingRoomButton");
            }
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