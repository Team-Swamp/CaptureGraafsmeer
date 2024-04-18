using UnityEngine;
using TMPro;

using Framework.ScriptableObjects;

namespace UI.Canvas.PhotoTaking
{
    public sealed class CameraPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text buildingName;
        [SerializeField] private Texture2D buildingRender;

        /// <summary>
        /// Fills the camera panel with data from the given scriptable object
        /// </summary>
        /// <param name="photoData"> The scriptable object used to get data </param>
        public void FillPanelData(PhotoData photoData)
        {
            buildingName.text = photoData.Title;
            //to do: set image from scriptable object
        }
    }   
}
