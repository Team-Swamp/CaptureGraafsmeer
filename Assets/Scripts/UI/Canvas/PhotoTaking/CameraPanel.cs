using UnityEngine;
using TMPro;

using Framework.PhoneCamera;
using Framework.ScriptableObjects;

namespace UI.Canvas.PhotoTaking
{
    public sealed class CameraPanel : CameraPermission
    {
        [SerializeField] private PhotoTaker taker;
        [SerializeField] private TMP_Text pointOfInterestName;
        [SerializeField] private Texture2D pointOfInterestRender;
        [SerializeField] private RenderTexture renderTexture;

        private bool _isActive;

        /// <summary>
        /// Fills the camera panel with data from the given scriptable object
        /// </summary>
        /// <param name="targetPhotoData"> The scriptable object used to get data </param>
        public void SetPanelData(PhotoData targetPhotoData)
        {
            pointOfInterestName.text = targetPhotoData.Title;
            taker.StartCamera();
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
