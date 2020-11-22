using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PhotoCamera.UseCase
{
    public class CameraMonitor
    {
        public RenderTexture capturedImage;
    }

    public interface IPhotoCameraController
    {
        UniTask OnTriggerPulledAsync();
        CameraMonitor CameraMonitor { get; }
    }
}