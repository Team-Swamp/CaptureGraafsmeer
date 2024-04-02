using UnityEngine;
using UnityEngine.UI;

using Framework.PhoneCamera;
using Framework.ScriptableObjects;
using UnityEngine.Events;

namespace UI.Canvas.PhoneCamera
{
    public sealed class PhotoTaker : CameraPermission
    {
        private const string NO_CAMERA_ERROR = "No webcam device found.";
        
        [SerializeField] private RawImage liveCamera;
        [SerializeField] private RawImage lastPhoto;
        [SerializeField] private PhotoData photoData;
        
        private WebCamTexture _webcamTexture;
        private Texture2D _currentPhoto;
        
        public PhotoData Data { get; set; }

        [SerializeField] private UnityEvent onOpenCamera= new();
        [SerializeField] private UnityEvent onPhotoTaken = new();
        
        private void Awake() => FindCamera();

        private void Start() => lastPhoto.texture = photoData.LoadTexture();

        private void OnDisable()
        {
            if (_webcamTexture != null
                && _webcamTexture.isPlaying)
                _webcamTexture.Stop();
        }
        
        /// <summary>
        /// Will start the assign camera to play and will apply it to the live camera image.
        /// </summary>
        public void StartCamera()
        {
            _webcamTexture.Play();
            liveCamera.texture = _webcamTexture;
            onOpenCamera?.Invoke();
        }
        
        /// <summary>
        /// Captures the current frame and applies it to the photo image.
        /// </summary>
        public void TakePhoto()
        {
            _currentPhoto = CaptureFrame(_webcamTexture);
            photoData.SaveTexture(_currentPhoto);
            lastPhoto.texture = photoData.LoadTexture();
            
            OnDisable();
            onPhotoTaken?.Invoke();
        }
        
        private void FindCamera()
        {
            WebCamDevice[] devices = WebCamTexture.devices;
            
            if (devices.Length > 0)
            {
                _webcamTexture = new WebCamTexture(devices[0].name);
                return;
            }
            
            Debug.LogError(NO_CAMERA_ERROR);
        }
        
        private static Texture2D CaptureFrame(WebCamTexture liveTexture)
        {
            Texture2D currentTexture = new Texture2D(liveTexture.width, liveTexture.height);
            Color[] pixels = liveTexture.GetPixels();
            
            currentTexture.SetPixels(pixels);
            currentTexture.Apply();
            
            return currentTexture;
        }
    }
}