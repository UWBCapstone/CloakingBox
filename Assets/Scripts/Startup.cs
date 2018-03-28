﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CloakingBox
{
#if UNITY_EDITOR
    [InitializeOnLoad]
    public class Startup
    {
        static Startup()
        {
            Debug.Log("Project Booting...");

            AddTags();
            AddLayers();
        }

        private static void AddTags()
        {
            string[] tags =
            {
                // Add tags as appropriate / needed here
            };

            foreach(string tag in tags)
            {
                TagsAndLayers.AddTag(tag);
            }
        }

        private static void AddLayers()
        {
            string[] layers =
            {
                // Add layers as appropriate / needed here
            };
            
            foreach(string layer in layers)
            {
                TagsAndLayers.AddLayer(layer);
            }

            List<string> layerNameList = LayerManager.GetLayerNameList();
            foreach (string layerName in layerNameList)
            {
                TagsAndLayers.AddLayer(layerName);
            }
        }
    }
#endif
}