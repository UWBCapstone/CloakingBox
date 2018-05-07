using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CloakingBox.ChooseOldRoom
{
    public class BackgroundImageManager : MonoBehaviour
    {
        public GameObject BackgroundImageObject;
        public Texture2D DefaultBG;

        private MyTangoFileManager customTangoFileManager;
        private TangoMetaImage metaImage;

        public void Awake()
        {
            // Create harness for loading up texture files
            // Create the MyTangoFileManager for the scene
            // Create the meta image to grab the texture from the thumbnail file
            createCustomTangoFileManager();
            createMetaImageChild();
        }

        public void Update()
        {
            if(metaImage.RoomName != RoomNameHolder.RoomName)
            {
                metaImage.RoomName = RoomNameHolder.RoomName;
                metaImage.Load(RoomNameHolder.RoomName);
                SetBackgroundImage(metaImage.Texture);
            }
        }

        private void createCustomTangoFileManager()
        {
            GameObject customTangoFileManagerObj = new GameObject();
            customTangoFileManager = customTangoFileManagerObj.AddComponent<MyTangoFileManager>();
        }

        private void createMetaImageChild()
        {
            GameObject metaImageChild = new GameObject();
            metaImage = metaImageChild.AddComponent<TangoMetaImage>();
            metaImageChild.transform.parent = customTangoFileManager.gameObject.transform;
        }

        public void SetDefaultBackgroundImage()
        {
            var img = BackgroundImageObject.GetComponent<UnityEngine.UI.Image>();
            img.sprite.texture.SetPixels(DefaultBG.GetPixels());
            img.sprite.texture.Apply();
        }

        public void SetBackgroundImage(Texture2D tex)
        {
            var img = BackgroundImageObject.GetComponent<UnityEngine.UI.Image>();
            img.sprite.texture.SetPixels(tex.GetPixels());
            img.sprite.texture.Apply();
        }
    }
}