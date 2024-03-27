using UnityEngine;
using UnityEngine.UI;

using Framework.PhoneCamera;

namespace UI.Canvas.PhoneCamera
{
    public sealed class PhotoTaker : CameraPermission
    {
        private const string NO_CAMERA_ERROR = "No webcam device found.";
        
        [SerializeField] private RawImage liveCamera;
        [SerializeField] private RawImage photo;
        
        private WebCamTexture _webcamTexture;
        private Texture2D _photo;

        public Texture2D GetPhoto() => _photo;
        
        private void Awake() => FindCamera();

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
        }
        
        /// <summary>
        /// Captures the current frame and applies it to the photo image.
        /// </summary>
        public void TakePhoto()
        {
            _photo = CaptureFrame(_webcamTexture);
            photo.texture = _photo;
            
            OnDisable();
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
        
        private static Texture2D CaptureFrame(WebCamTexture webcamTexture)
        {
            Texture2D texture = new Texture2D(webcamTexture.width, webcamTexture.height);
            Color[] pixels = webcamTexture.GetPixels();
            
            texture.SetPixels(pixels);
            texture.Apply();
            
            return texture;
        }
    }
}