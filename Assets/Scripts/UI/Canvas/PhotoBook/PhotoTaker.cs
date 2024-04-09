using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using Framework.PhoneCamera;
using Framework.ScriptableObjects;

namespace UI.Canvas.PhoneBook
{
    public sealed class PhotoTaker : CameraPermission
    {
        private const string NO_CAMERA_ERROR = "No camera device found.";
        private const string CAMERA_NOT_ACTIVE_ERROR = "The camera is not active at this moment.";
        
        [SerializeField] private RawImage liveCamera;
        [SerializeField] private RawImage lastPhoto;
        [SerializeField] private PhotoData photoData;
        [SerializeField] private Texture2D defaultTex;
        
        private WebCamTexture _webcamTexture;
        private Texture2D _currentPhoto;
        private PhotoInteractable _currentInteractable;
        
        public PhotoData Data { get; set; }
        public Texture2D DefaultTex => defaultTex;

        [SerializeField] private UnityEvent onOpenCamera= new();
        [SerializeField] private UnityEvent onPhotoTaken = new();
        
        private void Awake() => FindCamera();

        // private void Start() => lastPhoto.texture = photoData.LoadTexture();

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
            // StartCoroutine(Wajow());
            // return;
            
            if (!_webcamTexture.isPlaying)
                throw new Exception(CAMERA_NOT_ACTIVE_ERROR);
            
            if (_currentInteractable == null)
            {
                Debug.LogWarning("Current interactable not set. Cannot take photo.");
                return;
            }
            
            _currentPhoto = CaptureFrame(_webcamTexture);
            
            if (!_currentInteractable.SaveTexture(_currentPhoto))
                return;

            // StartCoroutine(Wajow());
            // return;
            
            Debug.Log(_currentInteractable.name);
            Yes();
            lastPhoto.texture = _currentInteractable.GetTexture();
            
            onPhotoTaken?.Invoke();
            OnDisable();
        }

        private IEnumerator Wajow()
        {
            yield return new WaitForSeconds(3);
                
            Debug.Log(_currentInteractable.name);
            Yes();
            lastPhoto.texture = _currentInteractable.GetTexture();
            
            onPhotoTaken?.Invoke();
            OnDisable();
        }

        public void Yes()
        {
            _currentInteractable.IsVisited = true;
        }

        public void SetCurrentPhotoInteractable(PhotoInteractable target) => _currentInteractable = target;
        
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