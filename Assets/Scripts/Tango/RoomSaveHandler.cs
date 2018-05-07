using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Threading;

#if UNITY_ANDROID || UNITY_EDITOR
using Tango;

namespace CloakingBox
{
    public class RoomSaveHandler : MonoBehaviour
    {
        public string FileDirectory;
        public string RoomName = "Room";
        public string FileExtension = ".tngrm";
        public static readonly string FileFolder = "TangoRooms";

        #region Methods
        #region Setters
        public void SetFileDirectory(string directory)
        {
            FileDirectory = directory;
        }

        public void SetFileDirectory()
        {
            FileDirectory = Path.Combine(Application.dataPath, FileFolder);

            Directory.CreateDirectory(FileDirectory);
            Debug.Log("Room File (Area Learning Descriptions) directory set to " + FileDirectory);
        }
        #endregion

        #region Getters
        public string GetFileName(string roomName)
        {
            return roomName + FileExtension;
        }

        public string GetFilePath(string directory, string roomName)
        {
            string fileName = GetFileName(roomName);
            string filepath = Path.Combine(directory, fileName);
            return filepath;
        }
        #endregion

        #region Saving / Loading
        public void SetFileSettings(string directory, string roomName)
        {
            FileDirectory = directory;
            RoomName = roomName;
        }

        public void SaveRoom(string directory, string roomName)
        {
            SetFileSettings(directory, roomName);
            Thread saveThread = new Thread(SaveThread);
            saveThread.Start();
        }

        #region Saving Thread Logic
        public void SaveThread()
        {
            try
            {
                ThreadManager.Register(Thread.CurrentThread);

                AreaDescription room = AreaDescription.SaveCurrent();
                room.ExportToFile(GetFilePath(FileDirectory, RoomName));
                AreaDescription.Metadata metadata = room.GetMetadata();
                metadata.m_name = RoomName;
                room.SaveMetadata(metadata);
            }
            catch (ThreadAbortException)
            {
            }
        }
        #endregion

        //public void LoadRoom(string roomName)
        //{
        //    Thread loadThread = new Thread(LoadThread);
        //    loadThread.Start();
        //}

        //#region Loading Thread Logic
        //public void LoadThread()
        //{
        //    try
        //    {
        //        ThreadManager.Register(Thread.CurrentThread);
                
        //        AreaDescription.ImportFromFile(GetFilePath(FileDirectory, RoomName));
        //    }
        //    catch (ThreadAbortException)
        //    {
        //    }
        //}
        //#endregion

        public AreaDescription LoadRoom(string roomName) // AreaDescription
        {
            AreaDescription room = AreaDescription.ForUUID("");

            string filepath = GetFilePath(FileDirectory, roomName);
            if (File.Exists(filepath))
            {
                // Grab the area descriptor

                AreaDescription.ImportFromFile(GetFilePath(FileDirectory, roomName));
                //return AreaDescription.ImportFromFile(GetFilePath(FileDirectory, roomName));
            }

            AreaDescription[] areaDescriptionList = AreaDescription.GetList();
            foreach(var roomCandidate in areaDescriptionList)
            {
                string metaName = roomCandidate.GetMetadata().m_name;
                
                if (metaName.Equals(roomName))
                {
                    string roomUUID = roomCandidate.m_uuid;
                    room = AreaDescription.ForUUID(roomUUID);
                    break;
                }
            }

            return room;
        }
        #endregion
        #endregion
    }
}
#endif