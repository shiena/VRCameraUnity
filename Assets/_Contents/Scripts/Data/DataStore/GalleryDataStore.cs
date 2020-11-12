using System;
using UnityEngine;

namespace PhotoCamera.DataStore
{
    public interface IGalleryDataStore
    {
        string StoragePath { get; }
        void RegisterImage(string path);
    }

    public class GalleryDataStore : IGalleryDataStore, IDisposable
    {
        public string StoragePath { get; }
        private AndroidJavaClass galleryHelper;

        public GalleryDataStore()
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