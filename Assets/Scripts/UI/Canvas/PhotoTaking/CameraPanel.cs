using UnityEngine;
using TMPro;

using Framework.ScriptableObjects;

namespace UI.Canvas.PhotoTaking
{
    public class CameraPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI buildingName;
        [SerializeField] private Texture2D buildingRender;

        public void FillPanelData(PhotoData photoData)
        {
            buildingName.text = photoData.Title;
            //to do: set image from scriptable object
        }
    }   
}
