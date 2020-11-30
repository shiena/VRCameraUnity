using Cysharp.Threading.Tasks;
using PhotoCamera.DataStore;
using PhotoCamera.View;
using PhotoCamera.Presenter;
using PhotoCamera.Repository;
using PhotoCamera.UseCase;
using VContainer;
using VContainer.Unity;

namespace PhotoCamera
{
    public class PhotoCameraLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<PhotographyRepository>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<CameraMonitorPresenter>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<PhotoCameraController>(Lifetime.Singleton).AsImplementedInterfaces();

#if UNITY_ANDROID && !UNITY_EDITOR
            builder.Register<PhotographyAndroidDataStore>(Lifetime.Singleton).AsImplementedInterfaces();
#elif UNITY_STANDALONE && !UNITY_EDITOR
            builder.Register<PhotographyStandaloneDataStore>(Lifetime.Singleton).AsImplementedInterfaces();
#else
            builder.Register<PhotographyEditorDataStore>(Lifetime.Singleton).AsImplementedInterfaces();
#endif

            builder.UseEntryPoints(Lifetime.Singleton, pointsBuilder =>
            {
#if UNITY_ANDROID && !UNITY_EDITOR
                pointsBuilder.Add<GalleryDataStore>();
#endif
                pointsBuilder.Add<PhotoCameraUseCase>();
            });

            builder.RegisterComponentInGameObject<PhotoCameraView>().AsImplementedInterfaces();
            builder.RegisterInstance(gameObject.GetCancellationTokenOnDestroy());
        }
    }
}