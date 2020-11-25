using System;
using UnityEngine;

namespace PhotoCamera.DataStore
{
    public class GalleryDataStore : IGalleryDataStore, IDisposable
    {
        private AndroidJavaClass galleryHelper;

        public GalleryDataStore()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            galleryHelper = new AndroidJavaClass("photocamera.GalleryHelper");
#endif
        }

        public void Dispose()
        {
            galleryHelper?.Dispose();
        }

        public void SaveImage(string path, sbyte[] data)
        {
            galleryHelper?.CallStatic("saveImage", path, data);
        }
    }
}