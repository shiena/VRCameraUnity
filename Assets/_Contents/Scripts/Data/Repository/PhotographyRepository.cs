using System.Threading;
using Cysharp.Threading.Tasks;
using PhotoCamera.UseCase;
using UnityEngine;
using UnityEngine.Rendering;
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
            var (f, w, h) = (image.graphicsFormat, image.width, image.height);
            var ff = image.format;
            var tex = new Texture2D(w, h, TextureFormat.RGB24, false);
            await UniTask.NextFrame(ct);

            var req = await AsyncGPUReadback.Request(image, 0).WithCancellation(ct);
            var rawByteArray = req.GetData<byte>();
            await UniTask.SwitchToThreadPool();
            using var jpg = ImageConversion.EncodeNativeArrayToJPG(rawByteArray, f, (uint)w, (uint)h);
            await UniTask.SwitchToMainThread(ct);
            var bytes = jpg.ToArray();
            Object.Destroy(tex);
            photographyDataStore.Save("jpg", bytes);
        }
    }
}