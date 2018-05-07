using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CloakingBox
{
    public enum CloakLayers
    {
        Room,
        Box,
        Debug
    };

    public class LayerManager : MonoBehaviour
    {
        public string RoomLayer = "Room";
        public string BoxLayer = "Box";
        public string DebugLayer = "Debug";
        
        public static string RoomLayer_s = "Room";
        public static string BoxLayer_s = "Box";
        public static string DebugLayer_s = "Debug";

        // Update is called once per frame
        void Update()
        {
            if (!RoomLayer_s.Equals(RoomLayer))
            {
                // set the static layer to be the correct one
                RoomLayer_s = RoomLayer;
            }
            if (!BoxLayer_s.Equals(BoxLayer))
            {
                // set the static layer to be the correct one
                BoxLayer_s = BoxLayer;
            }
            if (!DebugLayer_s.Equals(DebugLayer))
            {
                DebugLayer_s = DebugLayer;
            }
        }

        public static LayerMask GetLayerMask(CloakLayers layer)
        {
            LayerMask mask = LayerMask.NameToLayer("Default");
            switch (layer)
            {
                case CloakLayers.Room:
                    mask = LayerMask.NameToLayer(RoomLayer_s);
                    break;
                case CloakLayers.Box:
                    mask = LayerMask.NameToLayer(BoxLayer_s);
                    break;
                case CloakLayers.Debug:
                    mask = LayerMask.NameToLayer(DebugLayer_s);
                    break;
            }
            return mask;
        }

        public static List<string> GetLayerNameList()
        {
            List<string> layerNameList = new List<string>();
            layerNameList.Add(RoomLayer_s);
            layerNameList.Add(BoxLayer_s);
            layerNameList.Add(DebugLayer_s);

            return layerNameList;
        }

        public static LayerMask GetCullingMaskWithout(List<CloakLayers> layersToIgnore)
        {
            int everythingCullingMask = ~0;
            int mask = everythingCullingMask;
            foreach(CloakLayers layer in layersToIgnore)
            {
                mask = mask & ~GetLayerMask(layer);
            }

            return mask;
        }
    }
}
