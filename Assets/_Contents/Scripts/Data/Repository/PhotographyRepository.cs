using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using PhotoCamera.UseCase;
using UnityEngine;
using VContainer;
using Object = UnityEngine.Object;

namespace PhotoCamera.Repository
{
    public class PhotographyRepository : IPhotographyRepository
    {
        private readonly IPhotographyDataStore photographyDataStore;

        [Inject]
        public PhotographyRepository(IPhotographyDataStore photographyDataStore)
        {
            this.photographyDataStore = photographyDataStore;
        }

        public bool IsWritable()
        {
            return photographyDataStore.IsWritable();
        }

        public async UniTask TakePhotoAsync(CameraMonitor capturedImage, CancellationToken ct)
        {
            var image = capturedImage.capturedImage;
            var tex = new Texture2D(image.width, image.height, TextureFormat.RGB24, false);
            await UniTask.WaitForEndOfFrame(ct);

            var currentRt = RenderTexture.active;
            RenderTexture.active = image;
            tex.ReadPixels(new Rect(0, 0, image.width, image.height), 0, 0);
            if (QualitySettings.activeColorSpace == ColorSpace.Linear && !image.sRGB)
            {
                var color = tex.GetPixels();
                for (var i = 0; i < color.Length; i++)
                {
                    color[i].r = Mathf.LinearToGammaSpace(color[i].r);
                    color[i].g = Mathf.LinearToGammaSpace(color[i].g);
                    color[i].b = Mathf.LinearToGammaSpace(color[i].b);
                }

                tex.SetPixels(color);
            }

            tex.Apply();
            RenderTexture.active = currentRt;

            var bytes = tex.EncodeToJPG();
            Object.Destroy(tex);
            var data = new sbyte[bytes.Length];
            photographyDataStore.Save("jpg", bytes);
        }
    }
}