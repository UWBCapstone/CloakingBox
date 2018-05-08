using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Threading;

#if UNITY_ANDROID || UNITY_EDITOR
using Tango;

namespace CloakingBox
{
    public class TangoCameraCacher : MonoBehaviour
    {
        //public WebCamTexture camTex;
        public bool Debugging = false;
        public Camera ThumbnailCamera;
        public Texture2D cachedImg;
        public static Semaphore _lock;
        public static byte[] texArr = new byte[1];

        public UnityEngine.UI.Image debugUIImage;

        public void Awake()
        {
            if (GameObject.FindObjectOfType<CloakingBox.ScanNewRoom.InputManager>().CreateThumbnails == false)
            {
                gameObject.SetActive(false);
            }
            else
            {
                _lock = new Semaphore(1, 1);
                //camTex = new WebCamTexture(); // Should automatically grab the Android camera webtex
                //camTex.Play();

                cachedImg = new Texture2D(MyTangoFileManager.TangoCameraPixelWidth, MyTangoFileManager.TangoCameraPixelHeight, TextureFormat.RGBA32, false);
                assignThumbnailRenderTexture();
                turnOnDebugUI();
                InvokeRepeating("Cache", 0.3f, 1.0f);

                WorkflowDebugger.Log("Awakening TangoCameraCacher class...");
            }
        }

        public void Cache()
        {
            cacheImage();
            convertImgToPNG();

            WorkflowDebugger.Log("TangoCameraCacher: Image from webcam cached...");

            if (Debugging)
            {
                updateDebugUIImage();
                WorkflowDebugger.Log("Updating debug UI Image...");
            }
        }

        private void turnOnDebugUI()
        {
            if (Debugging)
            {
                debugUIImage.gameObject.SetActive(true);
            }
            else
            {
                debugUIImage.gameObject.SetActive(false);
            }
        }

        private void updateDebugUIImage()
        {
            if (Debugging)
            {
                Sprite s = Sprite.Create(cachedImg, new Rect(Vector2.zero, new Vector2(1000, 1000)), Vector2.zero);
                debugUIImage.sprite = s;
            }
        }

        private RenderTexture createThumbnailRenderTexture()
        {
            Resolution screenRes = Screen.currentResolution;
            RenderTexture rt = new RenderTexture(screenRes.width, screenRes.height, (int)(Camera.main.depth - 1));
            cachedImg = new Texture2D(rt.width, rt.height, TextureFormat.RGBA32, false);
            return rt;
        }

        private void assignThumbnailRenderTexture()
        {
            RenderTexture rt = createThumbnailRenderTexture();
            ThumbnailCamera.targetTexture = rt;
        }

        private void convertImgToPNG()
        {
            _lock.WaitOne();
            texArr = cachedImg.EncodeToPNG();
            _lock.Release();
        }

        private void cacheImage()
        {
            //cachedImg.SetPixels(camTex.GetPixels());
            Graphics.CopyTexture(ThumbnailCamera.targetTexture, cachedImg);
        }

        public static byte[] GetPNGOfCachedImage()
        {
            byte[] pngBytes = new byte[texArr.Length];
            _lock.WaitOne();
            texArr.CopyTo(pngBytes, 0);
            _lock.Release();
            return pngBytes;
        }
    }
}
#endif