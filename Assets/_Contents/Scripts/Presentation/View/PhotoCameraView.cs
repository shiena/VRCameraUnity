using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using VContainer;

namespace PhotoCamera.View
{
    public interface IPhotoCameraView
    {
        UniTask OnTriggerPulledAsync();
        RenderTexture CapturedImage { get; }
        MeshRenderer CameraMonitor { get; }
        float FlashDuration { get; }
        void InvokeTakePhotoEvent();
    }

    [Serializable]
    public class PhotoCameraView : IPhotoCameraView
    {
        [Header("Input")]
        [SerializeField] private XRGrabInteractable xrGrabInteractable;
        [SerializeField] private RenderTexture capturedImage;
        [Header("Output")]
        [SerializeField] private MeshRenderer cameraMonitor;
        [SerializeField] [Range(0, 1)] private float flashDuration;
        [SerializeField] private UnityEvent onTakePhotoEvent;

        private AsyncUnityEventHandler<XRBaseInteractor> triggerPulledEventHandler;

        [Inject]
        public void Construct(CancellationToken token)
        {
            triggerPulledEventHandler = xrGrabInteractable.onActivate.GetAsyncEventHandler(token);
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