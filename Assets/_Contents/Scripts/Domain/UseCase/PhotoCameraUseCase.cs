﻿using System.Threading;
using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;

namespace PhotoCamera.UseCase
{
    public class PhotoCameraUseCase : IInitializable
    {
        private readonly IPhotoCameraController photoCameraController;
        private readonly ICameraMonitorPresenter cameraMonitorPresenter;
        private readonly IPhotographyRepository photographyRepository;
        private readonly CancellationToken token;

        [Inject]
        public PhotoCameraUseCase(
            IPhotoCameraController photoCameraController,
            ICameraMonitorPresenter cameraMonitorPresenter,
            IPhotographyRepository photographyRepository,
            CancellationToken token
        )
        {
            this.photoCameraController = photoCameraController;
            this.cameraMonitorPresenter = cameraMonitorPresenter;
            this.photographyRepository = photographyRepository;
            this.token = token;
        }

        public void Initialize()
        {
            TriggerPulledEventAsync().Forget();
        }

        private async UniTaskVoid TriggerPulledEventAsync()
        {
            while (!token.IsCancellationRequested)
            {
                await photoCameraController.OnTriggerPulledAsync();
                if (!photographyRepository.IsWritable())
                {
                    continue;
                }
                cameraMonitorPresenter.InvokeTakePhotoEvent();
                await (cameraMonitorPresenter.FlashAsync(token),
                    photographyRepository.TakePhotoAsync(photoCameraController.CameraMonitor, token));
            }
        }
    }
}