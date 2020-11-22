using Cysharp.Threading.Tasks;
using PhotoCamera.UseCase;
using VContainer;

namespace PhotoCamera.Presenter
{
    public class PhotoCameraController : IPhotoCameraController
    {
        private readonly IPhotoCameraView photoCameraView;

        [Inject]
        public PhotoCameraController(IPhotoCameraView photoCameraView)
        {
            this.photoCameraView = photoCameraView;
            CameraMonitor = new CameraMonitor
            {
                capturedImage = photoCameraView.CapturedImage
            };
        }

        public UniTask OnTriggerPulledAsync()
        {
            return photoCameraView.OnTriggerPulledAsync();
        }

        public CameraMonitor CameraMonitor { get; }
    }
}