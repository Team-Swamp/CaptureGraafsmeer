using System;
using UnityEngine;
using TMPro;

using Framework.ScriptableObjects;
using UnityEngine.UI;

namespace UI.Canvas.PhotoTaking
{
    public sealed class CameraPanel : MonoBehaviour
    {
        [SerializeField] private PhotoTaker taker;
        [SerializeField] private RawImage background;
        [SerializeField] private TMP_Text pointOfInterestName;
        [SerializeField] private Texture2D pointOfInterestRender;

        /// <summary>
        /// Fills the camera panel with data from the given scriptable object
        /// </summary>
        /// <param name="targetPhotoData"> The scriptable object used to get data </param>
        public void SetPanelData(PhotoData targetPhotoData)
        {
            pointOfInterestName.text = targetPhotoData.Title;
            // to do: set render from scriptable object
            taker.StartCamera(background);
        }
    }   
}
