using System.Threading;
using Cysharp.Threading.Tasks;
using PhotoCamera.Presenter;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using VContainer;

namespace PhotoCamera.View
{
    public class PhotoCameraView : MonoBehaviour, IPhotoCameraView
    {
        [Header("Input")]
        [SerializeField] private XRGrabInteractable xrGrabInteractable;
        [SerializeField] private RenderTexture capturedImage;
        [Header("Output")]
        [SerializeField] private MeshRenderer cameraMonitor;
        [SerializeField] [Range(0, 1)] private float flashDuration;
        [SerializeField] private UnityEvent onTakePhotoEvent;

        private AsyncUnityEventHandler<ActivateEventArgs> triggerPulledEventHandler;

        [Inject]
        public void Construct(CancellationToken token)
        {
            triggerPulledEventHandler = xrGrabInteractable.activated.GetAsyncEventHandler(token);
        }

        public UniTask OnTriggerPulledAsync()
        {
            return triggerPulledEventHandler.OnInvokeAsync();
        }

        public MeshRenderer CameraMonitor => cameraMonitor;
        public float FlashDuration => flashDuration;

        public void InvokeTakePhotoEvent()
        {
            onTakePhotoEvent.Invoke();
        }

        public RenderTexture CapturedImage => capturedImage;
    }
}