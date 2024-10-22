using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

using Framework.PhoneCamera;
using Framework.ScriptableObjects;

namespace UI.Canvas.PhotoTaking
{
    public sealed class CameraPanel : CameraPermission
    {
        [Header("References")]
        [SerializeField] private PhotoTaker taker;
        [SerializeField] private RenderTexture renderTexture;
        
        [Header("Child objects")]
        [SerializeField] private TMP_Text pointOfInterestName;
        [SerializeField] private RawImage pointOfInterestRender;

        [Header("Event")]
        [SerializeField] private UnityEvent onShow = new();

        private bool _isActive;

        private new void Start()
        {
            base.Start();
            gameObject.SetActive(false);
        }
        
        /// <summary>
        /// Fills the camera panel with data from the given scriptable object
        /// </summary>
        /// <param name="targetPhotoData"> The scriptable object used to get data </param>
        public void SetPanelData(PhotoData targetPhotoData)
        {
            pointOfInterestName.text = targetPhotoData.Title;
            pointOfInterestRender.texture = targetPhotoData.Render;
            taker.StartCamera();
            
            onShow?.Invoke();
        }
        
        private void Update()
        {
            WebCamTexture currentCameraTexture = taker.CameraTexture;
            
            if(_isActive 
               && currentCameraTexture 
               && currentCameraTexture.isPlaying)
                Graphics.Blit(taker.CameraTexture, renderTexture);
        }

        private void OnEnable() => _isActive = true;

        private void OnDisable() => _isActive = false;
    }   
}
