using System;
using UnityEngine;

namespace PhotoCamera
{
    public class GalleryHelper : IDisposable
    {
        public readonly string StoragePath;
        private AndroidJavaClass galleryHelper;

        public GalleryHelper()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            galleryHelper = new AndroidJavaClass("photocamera.GalleryHelper");
#endif
            StoragePath = GetExternalStorageDirectory();
        }

        public void Dispose()
        {
            galleryHelper?.Dispose();
        }

        public void RegisterImage(string path)
        {
            galleryHelper?.CallStatic("registerImage", path);
        }

        private string GetExternalStorageDirectory()
        {
            return galleryHelper?.CallStatic<string>("getExternalStorageDirectory");
        }
    }
}