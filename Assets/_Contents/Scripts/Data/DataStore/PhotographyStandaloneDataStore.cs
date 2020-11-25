using System;
using System.IO;
using PhotoCamera.Repository;
using UnityEngine;

namespace PhotoCamera.DataStore
{
    public class PhotographyStandaloneDataStore : IPhotographyDataStore
    {
        private readonly DirectoryInfo screenshotFolderInfo;

        public PhotographyStandaloneDataStore() : this(
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), Application.productName))
        {
        }

        protected PhotographyStandaloneDataStore(string path)
        {
            screenshotFolderInfo = new DirectoryInfo(path);
        }

        public bool IsWritable()
        {
            return true;
        }

        public void Save(string ext, byte[] data)
        {
            if (!screenshotFolderInfo.Exists)
            {
                screenshotFolderInfo.Create();
            }

            var screenshotFile = Path.Combine(screenshotFolderInfo.FullName, $"{MakeFileName()}.{ext}");
            File.WriteAllBytes(screenshotFile, data);
        }

        private string MakeFileName()
        {
            return $"{Application.productName}-{DateTime.Now:yyyyMMdd-HHmmssfff}";
        }
    }
}