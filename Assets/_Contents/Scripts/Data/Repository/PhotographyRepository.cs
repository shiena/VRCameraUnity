using System.Threading;
using Cysharp.Threading.Tasks;
using PhotoCamera.UseCase;
using UnityEngine;
using UnityEngine.Rendering;
using VContainer;

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
            await UniTask.WaitForEndOfFrame(ct);

            var req = await AsyncGPUReadback.Request(image, 0).WithCancellation(ct);
            var rawByteArray = req.GetData<byte>();
            await UniTask.SwitchToThreadPool();
            using var jpg = ImageConversion.EncodeNativeArrayToJPG(rawByteArray, f, (uint)w, (uint)h);
            await UniTask.SwitchToMainThread(ct);
            var bytes = jpg.ToArray();
            photographyDataStore.Save("jpg", bytes);
        }
    }
}