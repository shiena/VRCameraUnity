using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PhotoCamera.Presenter
{
    public interface IPhotoCameraView
    {
        UniTask OnTriggerPulledAsync();
        RenderTexture CapturedImage { get; }
        MeshRenderer CameraMonitor { get; }
        float FlashDuration { get; }
        void InvokeTakePhotoEvent();
    }
}