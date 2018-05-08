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
        public GameObject RoomList;

        public void LoadRoom()
        {
            // Get the list label
            string label = RoomList.GetComponent<UnityEngine.UI.Dropdown>().captionText.text;
            RoomNameHolder.RoomName = label;
            UnityEngine.SceneManagement.SceneManager.LoadScene("Scenes/CloakingBox_Tango/BoxScene");
            //UnityEngine.SceneManagement.SceneManager.LoadScene("Scenes/CloakingBox_Tango/transitiontest");
        }

        public void ReturnToPreviousScene()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Scenes/CloakingBox_Tango/Start");
        }
    }
}
#endif