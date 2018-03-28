using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CloakingBox
{
    public enum CloakLayers
    {
        Box,
        Visible,
        Invisible
    };

    public class LayerManager : MonoBehaviour
    {
        public string CloakingBoxLayer = "CloakingBox";
        public string VisibleLayer = "Visible";
        public string InvisibleLayer = "Invisible";

        public static string CloakingBoxLayer_s = "CloakingBox";
        public static string VisibleLayer_s = "Visible";
        public static string InvisibleLayer_s = "Invisible";

        // Update is called once per frame
        void Update()
        {
            if (!CloakingBoxLayer.Equals(CloakingBoxLayer))
            {
                // add the layer

                // set the static layer to be the correct one
                CloakingBoxLayer_s = CloakingBoxLayer;
            }
        }

        public static LayerMask GetLayerMask(CloakLayers layer)
        {
            LayerMask mask = LayerMask.NameToLayer("Default");
            switch (layer)
            {
                case CloakLayers.Box:
                    mask = LayerMask.NameToLayer(CloakingBoxLayer_s);
                    break;
                case CloakLayers.Visible:
                    mask = LayerMask.NameToLayer(VisibleLayer_s);
                    break;
                case CloakLayers.Invisible:
                    mask = LayerMask.NameToLayer(InvisibleLayer_s);
                    break;
            }
            return mask;
        }
    }
}
