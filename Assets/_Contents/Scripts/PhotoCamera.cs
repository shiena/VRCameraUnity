using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace PhotoCamera
{
    [RequireComponent(typeof(XRGrabInteractable))]
    public class PhotoCamera : MonoBehaviour
    {
        [SerializeField] private RenderTexture renderTexture;
        [SerializeField] private MeshRenderer cameraDisplay;
        [SerializeField] [Range(0, 1)] private float flashDuration = 0.3f;
        [SerializeField] private UnityEvent onTakePhotoEvent;

        private Photography photography;
        private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

        private async UniTaskVoid Start()
        {
            var token = this.GetCancellationTokenOnDestroy();
            var xrGrabInteractable = GetComponent<XRGrabInteractable>();
            using (var galleryHelper = new GalleryHelper())
            {
                photography = new Photography(galleryHelper);
                var triggerPulledEvent = xrGrabInteractable.onActivate.GetAsyncEventHandler(token);
                TriggerPulledEventAsync(triggerPulledEvent, token).Forget();
                await this.OnDestroyAsync();
            }
            photography = null;
        }

        private async UniTaskVoid TriggerPulledEventAsync(AsyncUnityEventHandler<XRBaseInteractor> e, CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                await e.OnInvokeAsync();
                onTakePhotoEvent.Invoke();
                await (FlashAsync(ct), TakePhotoAsync(ct));
            }
        }

        private async UniTask FlashAsync(CancellationToken ct)
        {
            var material = cameraDisplay.material;
            var originalColor = material.GetColor(EmissionColor);
            await DOTween.To(() => material.GetColor(EmissionColor), color => material.SetColor(EmissionColor, color), Color.black, flashDuration)
                .SetEase(Ease.InSine)
                .WithCancellation(ct);
            material.SetColor(EmissionColor, originalColor);
        }

        private async UniTask TakePhotoAsync(CancellationToken ct)
        {
            var tex = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
            await UniTask.WaitForEndOfFrame(ct);

            var currentRt = RenderTexture.active;
            RenderTexture.active = renderTexture;
            tex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            if (QualitySettings.activeColorSpace == ColorSpace.Linear && !renderTexture.sRGB)
            {
                var color = tex.GetPixels();
                for (var i = 0; i < color.Length; i++)
                {
                    color[i].r = Mathf.LinearToGammaSpace(color[i].r);
                    color[i].g = Mathf.LinearToGammaSpace(color[i].g);
                    color[i].b = Mathf.LinearToGammaSpace(color[i].b);
                }

                tex.SetPixels(color);
            }

            tex.Apply();
            RenderTexture.active = currentRt;

            var bytes = tex.EncodeToPNG();
            Destroy(tex);
            photography.Save(bytes);
        }
    }
}