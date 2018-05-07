using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_ANDROID || UNITY_EDITOR
using Tango;

namespace CloakingBox
{
    public class TangoRoomLearningBackendManager : MonoBehaviour
    {
        public TangoApplication tangoManager_m;
        public bool Learning = true;
        public string RoomName = "Room";

        [HideInInspector]
        public TangoManagerState fallbackState_m;
        private string saveHandlerString_m = "RoomSaveHandler";
        [HideInInspector]
        public RoomSaveHandler saveHandler_m;

        #region Methods
        #region Public Access
        /// <summary>
        /// To be called in a start before the program starts up. Otherwise, you'll encounter errors (?)
        /// </summary>
        /// <returns></returns>
        public void Learn()
        {
            Learning = true;
            startLearning();
        }

        /// <summary>
        /// To be called manually by pressing a button on the Tango touchscreen. Stops the learning process and returns a copy of the scanned room. Resets the Tango Manager's settings.
        /// </summary>
        /// <returns></returns>
        public GameObject StopLearning()
        {
            Learning = false;
            GameObject room = finishLearning();

            return room;
        }

        public void ClearLearnedReconstruction()
        {
            tangoManager_m.Tango3DRClear();
        }

        /// <summary>
        /// To be called in a start before the program starts up. Otherwise, you'll encounter errors (? Tango)
        /// </summary>
        /// <param name="roomName"></param>
        /// <returns></returns>
        public GameObject UseLearnedRoom(string roomName)
        {
            manualStartup(roomName);
            GameObject room = generateRoom();

            return room;
        }
        #endregion

        #region Init
        public void Start()
        {
            setTangoManager();
            tangoManager_m.m_autoConnectToService = false;
            setRoomSaveHandler();
            recordFallbackState();
        }
        
        private void setTangoManager()
        {
#if UNITY_ANDROID
            if (tangoManager_m == null)
            {
                var tangoManagerObj = GameObject.Find("Tango Manager");
                if(tangoManagerObj != null)
                {
                    tangoManager_m = tangoManagerObj.GetComponent<TangoApplication>();
                }
                else
                {
                    Debug.LogError("Tango Manager not found! Please set the Tango Manager");
                }
            }
#endif
        }

        private void setRoomSaveHandler()
        {
            if (saveHandler_m == null)
            {
                var saveHandlerObj = GameObject.Find(saveHandlerString_m);
                if (saveHandlerObj != null)
                {
                    saveHandler_m = saveHandlerObj.GetComponent<RoomSaveHandler>();
                }
            }
        }
        #endregion

        //public void Update()
        //{
        //    if (Learning)
        //    {
        //        // Must be placed in Update to ensure that it will run properly after all Starts and Awakes finish
        //        learningManualStartup();
        //        // LearningManualStartup(RoomName);
        //    }
        //}

        #region Learning Area Description
        private void learningManualStartup()
        {
            tangoManager_m.MimicAutoConnectToService();
        }

        private void manualStartup(string roomName)
        {
            //tangoManager_m.MimicAutoConnectToService();
            AreaDescription room = saveHandler_m.LoadRoom(roomName);
            tangoManager_m.MimicAutoConnectToService(room);
        }
        
        private void startLearning()
        {
            tangoManager_m.m_autoConnectToService = false;

            tangoManager_m.AreaDescriptionLearningMode = true;
            tangoManager_m.m_3drUseAreaDescriptionPose = true;

            Learning = true;
        }

        private GameObject finishLearning()
        {
            string directory = saveHandler_m.FileDirectory;

            saveHandler_m.SaveRoom(directory, RoomName);

            GameObject room = generateRoom();
            
            ResetManager();
            Learning = false;

            return room;
        }

        private GameObject generateRoom()
        {
            List<Vector3> vertices = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<Color32> colors = new List<Color32>();
            List<int> triangles = new List<int>();

            Tango3DReconstruction.Status meshExtractionStatus = tangoManager_m.Tango3DRExtractWholeMesh(vertices, normals, colors, triangles);
            if (meshExtractionStatus == Tango3DReconstruction.Status.SUCCESS)
            {
                GameObject room = TangoRoomObject.GenerateRoom(RoomName, vertices, normals, colors, triangles);
                Debug.Log("Created Room[" + RoomName + "]");

                return room;
            }
            else
            {
                Debug.LogError("Failed to reconstruct room on Tango device after finishing learning.");

                return null;
            }
        }
        #endregion

        #region Resetting Tango Manager
        private void recordFallbackState()
        {
            fallbackState_m = new TangoManagerState(tangoManager_m);
        }

        public void ResetManager()
        {
            fallbackState_m.Apply(tangoManager_m);
        }
        #endregion
        #endregion
    }
}
#endif