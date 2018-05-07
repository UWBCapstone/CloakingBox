using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_ANDROID || UNITY_EDITOR
using Tango;

namespace CloakingBox
{
    public class TangoMetaImage : MonoBehaviour
    {
        // Take the image when appropriate
        // Navigate to the correct directory / create it when necessary
        // Save the image as a file
        // Load the image when requested

        public static MyTangoFileManager fileManager_m;
        public string RoomName = "DefaultRoomName";
        public Texture2D Texture;

        public void Start()
        {
            setMyTangoFileManager();
            Texture = new Texture2D(fileManager_m.TangoCameraPixelWidth, fileManager_m.TangoCameraPixelHeight, TextureFormat.RGBA32, false);
        }

        private void setMyTangoFileManager()
        {
            if (fileManager_m == null)
            {
                var fileManagerGO = GameObject.Find("CustomTangoFileManager");
                if (fileManagerGO != null)
                {
                    fileManager_m = fileManagerGO.GetComponent<MyTangoFileManager>();
                }
            }
        }

        public void Save(string roomName)
        {
            fileManager_m.SaveThumbnail(roomName);
        }

        /// <summary>
        /// Version meant to be called from the Editor Inspector.
        /// </summary>
        public void Save()
        {
            Save(RoomName);
        }
        
        public void Load(string roomName)
        {
            byte[] pngBytes = fileManager_m.LoadThumbnail(roomName);
            Texture.SetPixels(MyTangoFileManager.GetPixelsFromBytes(pngBytes));
            Texture.Apply();
        }

        /// <summary>
        /// Version meant to be called from the Editor Inspector.
        /// </summary>
        public void Load()
        {
            Load(RoomName);
        }
    }
}
#endif