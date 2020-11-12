using System.Threading;
using Cysharp.Threading.Tasks;
using PhotoCamera.DataStore;
using UnityEngine;
using VContainer;

namespace PhotoCamera.Repository
{
    public interface IPhotographyRepository
    {
        UniTask TakePhotoAsync(RenderTexture capturedImage, CancellationToken ct);
    }

    public class PhotographyRepository : IPhotographyRepository
    {
        private readonly IPhotographyDataStore photographyDataStore;

        [Inject]
        public PhotographyRepository(IPhotographyDataStore photographyDataStore)
        {
            this.photographyDataStore = photographyDataStore;
        }

        public async UniTask TakePhotoAsync(RenderTexture capturedImage, CancellationToken ct)
        {
            var tex = new Texture2D(capturedImage.width, capturedImage.height, TextureFormat.RGB24, false);
            await UniTask.WaitForEndOfFrame(ct);

            var currentRt = RenderTexture.active;
            RenderTexture.active = capturedImage;
            tex.ReadPixels(new Rect(0, 0, capturedImage.width, capturedImage.height), 0, 0);
            if (QualitySettings.activeColorSpace == ColorSpace.Linear && !capturedImage.sRGB)
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

            var bytes = tex.EncodeToPNG();
            Object.Destroy(tex);
            photographyDataStore.Save(bytes);
        }
    }
}