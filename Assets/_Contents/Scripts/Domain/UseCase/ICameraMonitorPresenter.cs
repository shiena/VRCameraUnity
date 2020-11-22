using System.Threading;
using Cysharp.Threading.Tasks;

namespace PhotoCamera.UseCase
{
    public interface ICameraMonitorPresenter
    {
        UniTask FlashAsync(CancellationToken ct);
        void InvokeTakePhotoEvent();
    }
}