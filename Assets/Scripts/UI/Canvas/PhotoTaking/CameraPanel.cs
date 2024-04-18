using UnityEngine;
using TMPro;

using Framework.ScriptableObjects;
using UnityEngine.Serialization;

namespace UI.Canvas.PhotoTaking
{
    public sealed class CameraPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text pointOfInterestName;
        [SerializeField] private Texture2D pointOfInterestRender;

        /// <summary>
        /// Fills the camera panel with data from the given scriptable object
        /// </summary>
        /// <param name="photoData"> The scriptable object used to get data </param>
        public void SetPanelData(PhotoData photoData)
        {
            pointOfInterestName.text = photoData.Title;
            //to do: set image from scriptable object
        }
    }   
}
