#if UNITY_ANDROID
using System;
using UnityEngine;

namespace PhotoCamera.DataStore
{
    public class GalleryDataStore : IGalleryDataStore, IDisposable
    {
        private readonly AndroidJavaClass galleryHelper;

        public GalleryDataStore()
        {
            galleryHelper = new AndroidJavaClass("photocamera.GalleryHelper");
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
#endif