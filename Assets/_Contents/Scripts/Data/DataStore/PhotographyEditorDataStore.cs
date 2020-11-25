using System.IO;
using UnityEngine;

namespace PhotoCamera.DataStore
{
    public class PhotographyEditorDataStore : PhotographyStandaloneDataStore
    {
        public PhotographyEditorDataStore() : base(
            Path.Combine(Application.temporaryCachePath, Application.productName))
        {
        }
    }
}