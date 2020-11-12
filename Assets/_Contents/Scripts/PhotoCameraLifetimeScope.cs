using Cysharp.Threading.Tasks;
using PhotoCamera.DataStore;
using PhotoCamera.View;
using PhotoCamera.Presenter;
using PhotoCamera.Repository;
using PhotoCamera.UseCase;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace PhotoCamera
{
    public class PhotoCameraLifetimeScope : LifetimeScope
    {
        [SerializeField] private PhotoCameraView photoCameraView;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<PhotographyRepository>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<PhotographyDataStore>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<CameraMonitorPresenter>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<PhotoCameraController>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.UseEntryPoints(Lifetime.Singleton, pointsBuilder =>
            {
                pointsBuilder.Add<GalleryDataStore>();
                pointsBuilder.Add<PhotoCameraUseCase>();
            });

            builder.RegisterInstance(photoCameraView).AsImplementedInterfaces();
            builder.RegisterInstance(gameObject.GetCancellationTokenOnDestroy());
        }
    }
}