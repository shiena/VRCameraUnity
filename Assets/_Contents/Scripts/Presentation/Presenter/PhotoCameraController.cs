using Cysharp.Threading.Tasks;
using PhotoCamera.View;
using UnityEngine;
using VContainer;

namespace PhotoCamera.Presenter
{
    public interface IPhotoCameraController
    {
        UniTask OnTriggerPulledAsync();
        RenderTexture CapturedImage { get; }
    }

    public class PhotoCameraController : IPhotoCameraController
    {
        private readonly IPhotoCameraView photoCameraView;

        [Inject]
        public PhotoCameraController(IPhotoCameraView photoCameraView)
        {
            this.photoCameraView = photoCameraView;
        }

        public UniTask OnTriggerPulledAsync()
        {
            return photoCameraView.OnTriggerPulledAsync();
        }

        public RenderTexture CapturedImage => photoCameraView.CapturedImage;
    }
}