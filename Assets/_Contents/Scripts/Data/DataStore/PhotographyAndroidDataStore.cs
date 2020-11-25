#if UNITY_ANDROID
using System;
using System.IO;
using PhotoCamera.Repository;
using UnityEngine;
using UnityEngine.Android;
using VContainer;

namespace PhotoCamera.DataStore
{
    public class PhotographyAndroidDataStore : IPhotographyDataStore
    {
        private readonly IGalleryDataStore galleryDataStore;
        private readonly string screenshotFolder;

        [Inject]
        public PhotographyAndroidDataStore(IGalleryDataStore galleryDataStore)
        {
            this.galleryDataStore = galleryDataStore;
            screenshotFolder = Application.productName;
        }

        public bool IsWritable()
        {
            if (Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
            {
                return true;
            }
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
            return Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite);
        }

        public void Save(string ext, byte[] data)
        {
            var screenshotFile = Path.Combine(screenshotFolder, $"{MakeFileName()}.{ext}");
            var sbytes = new sbyte[data.Length];
            Buffer.BlockCopy(data, 0, sbytes, 0, data.Length);
            galleryDataStore.SaveImage(screenshotFile, sbytes);
        }

        private string MakeFileName()
        {
            return $"{Application.identifier}-{DateTime.Now:yyyyMMdd-HHmmssfff}";
        }
    }
}
#endif