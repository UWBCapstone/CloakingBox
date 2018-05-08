using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CloakingBox.ChooseOldRoom
{
    public class BackgroundImageManager : MonoBehaviour
    {
        public bool UseThumbnails = false;

        public GameObject BackgroundImageObject;
        public Sprite DefaultBG;
        private bool defaultBGLoaded = false;

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
            //if(metaImage.RoomName != RoomNameHolder.RoomName)
            //{
            //    metaImage.RoomName = RoomNameHolder.RoomName;
            //    metaImage.Load(RoomNameHolder.RoomName);
            //    SetBackgroundImage(metaImage.Texture);

            //    WorkflowDebugger.Log("Background Image Manager set background image with updated meta image thumbnail...");
            //}

            string targetRoomName = getTargetRoomName();
            if(metaImage.RoomName != targetRoomName
                && !string.IsNullOrEmpty(metaImage.RoomName))
            {
                if (UseThumbnails)
                {
                    metaImage.RoomName = targetRoomName;
                    metaImage.Load(RoomNameHolder.RoomName);
                    SetBackgroundImage(metaImage.Texture);
                }
            }
            else if (!defaultBGLoaded
                && (metaImage.RoomName.Equals(TangoMetaImage.DefaultRoomName)
                    || string.IsNullOrEmpty(metaImage.RoomName)))
            {
                SetDefaultBackgroundImage();
            }
        }

        private string getTargetRoomName()
        {
            string label = GameObject.FindObjectOfType<UnityEngine.UI.Dropdown>().captionText.text;
            return label;
        }

        private void createCustomTangoFileManager()
        {
            GameObject customTangoFileManagerObj = new GameObject();
            customTangoFileManager = customTangoFileManagerObj.AddComponent<MyTangoFileManager>();
            customTangoFileManager.name = "CustomTangoFileManager";
        }

        private void createMetaImageChild()
        {
            GameObject metaImageChild = new GameObject();
            metaImage = metaImageChild.AddComponent<TangoMetaImage>();
            metaImageChild.transform.parent = customTangoFileManager.gameObject.transform;
            metaImageChild.name = "MetaImage";
        }

        public void SetDefaultBackgroundImage()
        {
            var img = BackgroundImageObject.GetComponent<UnityEngine.UI.Image>();
            //img.sprite.texture.SetPixels(DefaultBG.GetPixels());
            //img.sprite.texture.Apply();
            img.sprite = DefaultBG;
            defaultBGLoaded = true;

            WorkflowDebugger.Log("Background image set as default...");
        }

        public void SetBackgroundImage(Texture2D tex)
        {
            var img = BackgroundImageObject.GetComponent<UnityEngine.UI.Image>();
            img.sprite.texture.SetPixels(tex.GetPixels());
            img.sprite.texture.Apply();

            WorkflowDebugger.Log("Background image set...");
        }
    }
}