using System.Threading;
using AnimeTask;
using Cysharp.Threading.Tasks;
using PhotoCamera.UseCase;
using UnityEngine;
using VContainer;

namespace PhotoCamera.Presenter
{
    public class CameraMonitorPresenter : ICameraMonitorPresenter
    {
        private readonly IPhotoCameraView photoCameraView;
        private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

        [Inject]
        public CameraMonitorPresenter(IPhotoCameraView photoCameraView)
        {
            this.photoCameraView = photoCameraView;
        }

        public async UniTask FlashAsync(CancellationToken ct)
        {
            var material = photoCameraView.CameraMonitor.material;
            var originalColor = material.GetColor(EmissionColor);
            await Easing.Create<InSine>(originalColor, Color.black, photoCameraView.FlashDuration)
                .ToAction(color => material.SetColor(EmissionColor, color), ct);
            material.SetColor(EmissionColor, originalColor);
        }

        public void InvokeTakePhotoEvent()
        {
            photoCameraView.InvokeTakePhotoEvent();
        }
    }
}