using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

namespace CloakingBox
{
    public static class LevelManager
    {
        // Load up levels, given a scene name
        // Transition info between levels

        public static void LoadLevel(string sceneName)
        {
            //SceneManager.LoadScene(sceneName);
            SceneManager.LoadSceneAsync(sceneName);
        }
    }
}