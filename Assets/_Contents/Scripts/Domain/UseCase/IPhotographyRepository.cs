using System.Threading;
using Cysharp.Threading.Tasks;

namespace PhotoCamera.UseCase
{
    public interface IPhotographyRepository
    {
        bool IsWritable();
        UniTask TakePhotoAsync(CameraMonitor capturedImage, CancellationToken ct);
    }
}