using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Threading;

#if UNITY_ANDROID || UNITY_EDITOR
using Tango;

namespace CloakingBox.ChooseOldRoom
{
    public class InputManager : MonoBehaviour
    {
        // When i hit the button, do the thing
        public void SaveRoom()
        {
            // Show the "Saving..." text
            GameObject SavingText = GameObject.Find("SavingText");
            SavingText.GetComponent<UnityEngine.UI.Text>().enabled = true;

            // Register the thread and register that it will stop showing the "Saving..." text when it is done
            Thread saveThread = new Thread(SaveThread);
            ThreadManager.Register(saveThread);
            ThreadManager.RegisterOnJoin(saveThread, TurnOffSavingMessage);

            saveThread.Start();

            //            GetRoomNameHolder().RoomName = getRoomName();
            RoomNameHolder.RoomName = getRoomName();
        }

        public void SaveThread()
        {
            //ThreadManager.Register(Thread.CurrentThread);

            AreaDescription room = AreaDescription.SaveCurrent();

            AreaDescription.Metadata metadata = room.GetMetadata();
            metadata.m_name = getRoomName();

            room.SaveMetadata(metadata);

            //GameObject.Find("SavingText").SetActive(false);
        }

        public void ExitScanMode()
        {
            LevelManager.LoadLevel("Scenes/CloakingBox_Tango/BoxScene");
        }

        private string getRoomName()
        {
            // Get the room name from the field
            // if it's empty, return "DefaultRoomName"

            string roomName = GameObject.Find("RoomNameInputField").GetComponent<UnityEngine.UI.InputField>().text;

            if (string.IsNullOrEmpty(roomName))
            {
                return "DefaultRoomName";
            }
            else
            {
                return roomName;
            }
        }

        public static void TurnOffSavingMessage()
        {
            GameObject SavingText = GameObject.Find("SavingText");
            SavingText.GetComponent<UnityEngine.UI.Text>().enabled = false;
        }
    }
}
#endif