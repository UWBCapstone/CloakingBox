using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Threading;

#if UNITY_ANDROID || UNITY_EDITOR
using Tango;

namespace CloakingBox
{
    public class MyTangoFileManager : MonoBehaviour
    {
        public string FileDirectory;
        public string RoomName;
        private Semaphore _lock;
        //public const string FileExtension = ".tngrm";
        public static readonly string FileFolder = "TangoRooms";
        private static readonly int nTangoCamPixelWidth = 1;
        private static readonly int nTangoCamPixelHeight = 1;

        public void Start()
        {
            _lock = new Semaphore(1, 1);
            //DontDestroyOnLoad(gameObject);
#if UNITY_ANDROID
            SetFileDirectory(Application.persistentDataPath);
#else
            SetFileDirectory(Application.dataPath);
#endif
            SetRoomName("DefaultRoomName");

            WorkflowDebugger.Log("Custom Tango File Manager started...");
        }

#region Methods
#region Setters
        public void SetFileDirectory(string directory)
        {
            _lock.WaitOne();
            FileDirectory = directory;
            _lock.Release();
        }

        public void SetFileDirectory()
        {
            _lock.WaitOne();
#if UNITY_ANDROID
            FileDirectory = Path.Combine(Application.persistentDataPath, FileFolder);
#else
            FileDirectory = Path.Combine(Application.dataPath, FileFolder);
#endif

            _lock.Release();

            Directory.CreateDirectory(FileDirectory);
            //Debug.Log("Room File (Area Learning Descriptions) directory set to " + FileDirectory);
            Debug.Log("Custom Tango File directory set to " + FileDirectory);
        }

        public void SetRoomName(string roomName)
        {
            _lock.WaitOne();
            RoomName = roomName;
            _lock.Release();
        }
#endregion

#region Getters
        public string GetFileName(string roomName, MyTangoFileTypes filetype)
        {
            string fileName = "";

            switch (filetype)
            {
                case MyTangoFileTypes.Thumbnail:
                    fileName = roomName + "_thumbnail" + GetExtension(filetype);
                    break;
                case MyTangoFileTypes.TangoRoom:
                    fileName = roomName + GetExtension(filetype);
                    break;
                default:
                    Debug.LogError("Custom Tango file type not encountered. Please create the expected file type in MyTangoFileTypes.");
                    break;
            }

            return fileName;
        }

        public string GetFilePath(string directory, string roomName, MyTangoFileTypes filetype)
        {
            string fileName = GetFileName(roomName, filetype);
            string filepath = Path.Combine(directory, fileName);
            return filepath;
        }

        public string GetExtension(MyTangoFileTypes filetype)
        {
            string extension = "";

            switch (filetype)
            {
                case MyTangoFileTypes.Thumbnail:
                    extension = ".tngthmb";
                    break;
            }

            return extension;
        }

        public string GetFileDirectory()
        {
            _lock.WaitOne();
            string dir = FileDirectory;
            _lock.Release();

            return dir;
        }

        public string GetRoomName()
        {
            _lock.WaitOne();
            string name = RoomName;
            _lock.Release();

            return name;
        }
#endregion

#region Saving / Loading
        //public void SetFileSettings(string directory, string roomName)
        //{
        //    FileDirectory = directory;
        //    RoomName = roomName;
        //}

        public void SaveThumbnail(string roomName)
        {
            // use preset directory and MyTangoFileTypes
            SetFileDirectory();
            SetRoomName(roomName);
            Thread saveThumbnailThread_t = new Thread(saveThumbnailThread);
            ThreadManager.Register(saveThumbnailThread_t);
            ThreadManager.RegisterOnJoin(saveThumbnailThread_t, finishSavingThumbnail);
            saveThumbnailThread_t.Start();

            WorkflowDebugger.Log("Tango room thumbnail save process main thread completed...");
        }

        //public void SaveRoom(string directory, string roomName)
        //{
        //    SetFileSettings(directory, roomName);
        //    Thread saveThread = new Thread(SaveThread);
        //    saveThread.Start();
        //}

#region Saving Thread Logic
        public void saveThumbnailThread()
        {
            // Take the picture from the Tango camera - this should be the same as the Android camera
            // Save the picture as a png using .NET
            // Save the png as a tango thumbnail type

            try
            {
                // Redundant call
                ThreadManager.Register(Thread.CurrentThread);

                byte[] pngBytesOfCachedImage = TangoCameraCacher.GetPNGOfCachedImage();
                FileStream fs = new FileStream(GetFilePath(GetFileDirectory(), GetRoomName(), MyTangoFileTypes.Thumbnail), FileMode.Create);
                fs.Write(pngBytesOfCachedImage, 0, pngBytesOfCachedImage.Length);
            }
            catch (ThreadAbortException)
            {
            }
        }

        public void finishSavingThumbnail()
        {
            // To be used by the main thread after the saveThumbnailThread finishes
            Debug.Log("Thumbnail saved at " + GetFilePath(GetFileDirectory(), GetRoomName(), MyTangoFileTypes.Thumbnail));
        }

        public void SaveThread()
        {
            try
            {
                ThreadManager.Register(Thread.CurrentThread);

                AreaDescription room = AreaDescription.SaveCurrent();
                room.ExportToFile(GetFilePath(GetFileDirectory(), GetRoomName(), MyTangoFileTypes.TangoRoom));
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

            string filepath = GetFilePath(GetFileDirectory(), roomName, MyTangoFileTypes.TangoRoom);
            if (File.Exists(filepath))
            {
                // Grab the area descriptor

                AreaDescription.ImportFromFile(GetFilePath(GetFileDirectory(), roomName, MyTangoFileTypes.TangoRoom));
                //return AreaDescription.ImportFromFile(GetFilePath(FileDirectory, roomName));
            }

            AreaDescription[] areaDescriptionList = AreaDescription.GetList();
            foreach (var roomCandidate in areaDescriptionList)
            {
                string metaName = roomCandidate.GetMetadata().m_name;

                if (metaName.Equals(roomName))
                {
                    string roomUUID = roomCandidate.m_uuid;
                    room = AreaDescription.ForUUID(roomUUID);
                    break;
                }
            }

            WorkflowDebugger.Log("Tango room loaded (custom save path)...");

            return room;
        }

        /// <summary>
        /// Loads the png that represents the thumbnail associated with this room.
        /// </summary>
        /// <returns></returns>
        public byte[] LoadThumbnail(string roomName)
        {
            string filepath = GetFilePath(GetFileDirectory(), roomName, MyTangoFileTypes.Thumbnail);
            if (File.Exists(filepath))
            {
                using (FileStream fs = new FileStream(filepath, FileMode.Open))
                {
                    FileInfo fileInfo = new FileInfo(filepath);
                    byte[] pngBytes = new byte[(int)(fileInfo.Length)];
                    int bytesRead = fs.Read(pngBytes, 0, pngBytes.Length);

                    WorkflowDebugger.Log("Tango room thumbnail loaded...");

                    return pngBytes;
                }
            }
            else
            {
                return null;
            }
        }

        public static Color[] GetPixelsFromBytes(byte[] pngBytes)
        {
            if(pngBytes == null)
            {
                return null;
            }

            Texture2D tex = new Texture2D(nTangoCamPixelWidth, nTangoCamPixelHeight, TextureFormat.RGBA32, false);
            tex.LoadImage(pngBytes);
            tex.Apply();
            Color[] pixels = tex.GetPixels();
#if UNITY_EDITOR
            GameObject.DestroyImmediate(tex);
#else
            GameObject.Destroy(tex);
#endif
            return pixels;
        }
#endregion
#endregion

#region Properties
        public static int TangoCameraPixelWidth
        {
            get
            {
                return nTangoCamPixelWidth;
            }
        }
        public static int TangoCameraPixelHeight
        {
            get
            {
                return nTangoCamPixelHeight;
            }
        }
#endregion
    }
}
#endif