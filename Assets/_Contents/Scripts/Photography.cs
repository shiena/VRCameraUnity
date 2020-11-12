using System;
using System.IO;
using UnityEngine;

namespace PhotoCamera
{
    public class Photography
    {
        private readonly GalleryHelper galleryHelper;
        private readonly string screenshotFolder;

        public Photography(GalleryHelper galleryHelper)
        {
            this.galleryHelper = galleryHelper;
            screenshotFolder = GetScreenshotFolder();
        }

        private string GetScreenshotFolder()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            return Path.Combine(galleryHelper.StoragePath, "Oculus", "Screenshots");
#elif UNITY_STANDALONE && !UNITY_EDITOR
            return Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyPictures), Application.productName);
#else
            return Path.Combine(Application.temporaryCachePath, Application.productName);
#endif
        }

        public void Save(byte[] bytes)
        {
            if (!Directory.Exists(screenshotFolder))
            {
                Directory.CreateDirectory(screenshotFolder);
            }
            var screenshotFile = Path.Combine(screenshotFolder, $"{Application.productName}_{DateTime.Now:yyyy-MM-dd_HH-mm-ss-fff}.png");
            File.WriteAllBytes(screenshotFile, bytes);
            galleryHelper.RegisterImage(screenshotFile);
        }
    }
}