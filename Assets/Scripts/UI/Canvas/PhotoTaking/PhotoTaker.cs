﻿using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using Framework.PhoneCamera;
using Framework.SaveLoadSystem;
using Framework.ScriptableObjects;

namespace UI.Canvas.PhotoTaking
{
    public sealed class PhotoTaker : CameraPermission
    {
        private const string NO_CAMERA_ERROR = "No camera device found.";
        private const string NO_PHOTO_INTERACTABLE_ERROR = "Current interactable not set. Cannot take photo.";
        private const string CAMERA_NOT_ACTIVE_ERROR = "The camera is not active at this moment.";
        private const string UNABLE_TO_SAVE_PHOTO_ERROR = "Unable to save the photo of PhotoInteractable: ";
        
        [SerializeField] private RawImage liveCamera;
        [SerializeField] private RawImage lastPhoto;
        [SerializeField] private Texture2D defaultTex;

        private Texture2D _currentPhoto;
        private PhotoInteractable _currentInteractable;
        
        public WebCamTexture CameraTexture { get; private set; }
        
        public PhotoData Data { get; set; }
        
        public Texture2D DefaultTex => defaultTex;
        
        public PhotoInteractable CurrentPhotoInteractable => _currentInteractable;

        [SerializeField] private UnityEvent onOpenCamera= new();
        [SerializeField] private UnityEvent onPhotoTaken = new();
        
        private void Awake() => FindCamera();

        private void OnEnable() => FindCamera();

        private void OnDisable()
        {
            if (CameraTexture != null
                && CameraTexture.isPlaying)
                CameraTexture.Stop();
        }
        
        /// <summary>
        /// Will start the assign camera to play and will apply it to the live camera image.
        /// </summary>
        public void StartCamera() => ApplyCamera(liveCamera);

        /// <summary>
        /// Will start the assign camera to play and will apply it to the target image.
        /// </summary>
        /// <param name="targetImage">Target image to be the live camera</param>
        public void StartCamera(RawImage targetImage) => ApplyCamera(targetImage);

        private void ApplyCamera(RawImage targetImage)
        {
            if (CameraTexture == null)
                FindCamera();
            CameraTexture.Play();
            targetImage.texture = CameraTexture;
            onOpenCamera?.Invoke();
        }
        
        /// <summary>
        /// Captures the current frame and applies it to the photo image.
        /// </summary>
        public void TakePhoto()
        {
            if (!CameraTexture.isPlaying)
                throw new Exception(CAMERA_NOT_ACTIVE_ERROR);
            
            if (_currentInteractable == null)
                throw new Exception(NO_PHOTO_INTERACTABLE_ERROR);
            
            _currentPhoto = CaptureFrame(CameraTexture);

            if (!_currentInteractable.SaveTexture(_currentPhoto))
                throw new Exception(UNABLE_TO_SAVE_PHOTO_ERROR + _currentInteractable.name);
            
            _currentInteractable.IsVisited = true;
            Saver.Instance.PhotoAmountMade++;
            lastPhoto.texture = _currentInteractable.GetTexture();
            
            onPhotoTaken?.Invoke();
            OnDisable();
        }

        /// <summary>
        /// Set the target interactable as the current
        /// </summary>
        /// <param name="target">The target to set as current</param>
        public void SetCurrentPhotoInteractable(PhotoInteractable target) => _currentInteractable = target;
        
        private void FindCamera()
        {
            WebCamDevice[] devices = WebCamTexture.devices;

            if (devices.Length <= 0)
                throw new Exception(NO_CAMERA_ERROR);
            
            CameraTexture = new WebCamTexture(devices[0].name);
        }
        
        private Texture2D CaptureFrame(WebCamTexture liveTexture)
        {
            Texture2D currentTexture = new Texture2D(liveTexture.width, liveTexture.height);
            Color[] pixels = liveTexture.GetPixels();
            
            currentTexture.SetPixels(pixels);
            currentTexture.Apply();
            
            return currentTexture;
        }
    }
}