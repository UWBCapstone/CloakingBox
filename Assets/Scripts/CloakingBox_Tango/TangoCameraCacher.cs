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
        public WebCamTexture camTex;
        private Texture2D cachedImg;
        public static Semaphore _lock;
        public static byte[] texArr = new byte[1];

        public void Awake()
        {
            _lock = new Semaphore(1, 1);
            camTex = new WebCamTexture(); // Should automatically grab the Android camera webtex
            camTex.Play();
            cachedImg = new Texture2D(camTex.width, camTex.height, TextureFormat.RGBA32, false);

            InvokeRepeating("Cache", 0.3f, 1.0f);
        }

        public void Cache()
        {
            cacheImage();
            convertImgToPNG();
        }

        private void convertImgToPNG()
        {
            _lock.WaitOne();
            texArr = cachedImg.EncodeToPNG();
            _lock.Release();
        }

        private void cacheImage()
        {
            cachedImg.SetPixels(camTex.GetPixels());
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