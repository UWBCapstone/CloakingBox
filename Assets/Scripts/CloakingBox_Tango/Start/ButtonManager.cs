using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CloakingBox;

namespace CloakingBox.Start
{
    public class ButtonManager : MonoBehaviour
    {
        // There is a yes and no button
        public void StartNewRoom()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Scenes/CloakingBox_Tango/ScanNewRoom");
        }

        public void StartOldRoom()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Scenes/CloakingBox_Tango/ChooseOldRoom");
        }
    }
}