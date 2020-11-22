using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
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
            await DOTween.To(() => material.GetColor(EmissionColor), color => material.SetColor(EmissionColor, color),
                    Color.black, photoCameraView.FlashDuration)
                .SetEase(Ease.InSine)
                .WithCancellation(ct);
            material.SetColor(EmissionColor, originalColor);
        }

        public void InvokeTakePhotoEvent()
        {
            photoCameraView.InvokeTakePhotoEvent();
        }
    }
}