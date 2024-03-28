using System;
using UnityEngine;
using UnityEngine.UI;

using Framework.PhoneCamera;
using Framework.ScriptableObjects;

namespace UI.Canvas.PhoneCamera
{
    public sealed class PhotoTaker : CameraPermission
    {
        private const string NO_CAMERA_ERROR = "No webcam device found.";
        
        [SerializeField] private RawImage liveCamera;
        [SerializeField] private RawImage photo;
        [SerializeField] private RawImage lastPhoto;
        [SerializeField] private PhotoData photoData;
        
        private WebCamTexture _webcamTexture;
        private Texture2D texture2D;
        
        private void Awake() => FindCamera();

        private void OnDisable()
        {
            if (_webcamTexture != null
                && _webcamTexture.isPlaying)
                _webcamTexture.Stop();
        }

        private void Start()
        {
            lastPhoto.texture = photoData.LoadTexture();
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
            texture2D = CaptureFrame(_webcamTexture);
            photo.texture = texture2D;
            lastPhoto.texture = texture2D;
            
            OnDisable();
        }

        public void SavePhoto()
        {
            PhotoData newData = photoData;
            photoData.SaveTexture(texture2D);
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