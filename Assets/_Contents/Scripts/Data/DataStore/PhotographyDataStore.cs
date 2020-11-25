using System;
using System.IO;
using PhotoCamera.Repository;
using UnityEngine;
using VContainer;
#if UNITY_ANDROID
using UnityEngine.Android;
#endif

namespace PhotoCamera.DataStore
{
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
            return Path.Combine("Pictures", Application.productName);
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

        public void Save(string ext, sbyte[] data)
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            if (!Directory.Exists(screenshotFolder))
            {
                Directory.CreateDirectory(screenshotFolder);
            }
#endif
            var screenshotFile = Path.Combine(screenshotFolder, $"{Application.installerName}-{DateTime.Now:yyyyMMdd-HHmmssfff}.{ext}");
            galleryDataStore.SaveImage(screenshotFile, data);
        }
    }
}