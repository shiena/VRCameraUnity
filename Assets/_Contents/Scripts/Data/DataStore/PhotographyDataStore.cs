using System;
using System.IO;
using UnityEngine;
#if UNITY_ANDROID
using UnityEngine.Android;
#endif
using VContainer;

namespace PhotoCamera.DataStore
{
    public interface IPhotographyDataStore
    {
        bool IsWritable();
        void Save(byte[] bytes);
    }

    public class PhotographyDataStore : IPhotographyDataStore
    {
        private readonly IGalleryDataStore galleryDataStore;
        private readonly string screenshotFolder;

        [Inject]
        public PhotographyDataStore(IGalleryDataStore galleryDataStore)
        {
            this.galleryDataStore = galleryDataStore;
            screenshotFolder = GetScreenshotFolder();
        }

        private string GetScreenshotFolder()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            return Path.Combine(galleryDataStore.StoragePath, "Oculus", "Screenshots");
#elif UNITY_STANDALONE && !UNITY_EDITOR
            return Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyPictures), Application.productName);
#else
            return Path.Combine(Application.temporaryCachePath, Application.productName);
#endif
        }

        public bool IsWritable()
        {
#if UNITY_ANDROID
            if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
            {
                Permission.RequestUserPermission(Permission.ExternalStorageWrite);
                if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
                {
                    return false;
                }
            }
#endif
            return true;
        }

        public void Save(byte[] bytes)
        {
            if (!Directory.Exists(screenshotFolder))
            {
                Directory.CreateDirectory(screenshotFolder);
            }
            var screenshotFile = Path.Combine(screenshotFolder, $"{Application.productName}_{DateTime.Now:yyyy-MM-dd_HH-mm-ss-fff}.png");
            File.WriteAllBytes(screenshotFile, bytes);
            galleryDataStore.RegisterImage(screenshotFile);
        }
    }
}