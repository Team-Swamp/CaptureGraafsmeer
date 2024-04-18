using UnityEngine;
using TMPro;

using Framework.ScriptableObjects;

namespace UI.Canvas.PhotoTaking
{
    public sealed class CameraPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text pointOfInterestName;
        [SerializeField] private Texture2D pointOfInterestRender;

        /// <summary>
        /// Fills the camera panel with data from the given scriptable object
        /// </summary>
        /// <param name="targetPhotoData"> The scriptable object used to get data </param>
        public void SetPanelData(PhotoData targetPhotoData)
        {
            pointOfInterestName.text = targetPhotoData.Title;
            //to do: set image from scriptable object
        }
    }   
}
