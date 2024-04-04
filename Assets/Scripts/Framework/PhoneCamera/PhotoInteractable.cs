using UnityEngine;

using FrameWork;
using Framework.ScriptableObjects;
using UI.Canvas.PhoneCamera;

namespace Framework.PhoneCamera
{
    public sealed class PhotoInteractable : InteractableObject
    {
        [SerializeField] private PhotoTaker parent;
        [SerializeField] private PhotoData data;
        
        public PhotoData Data => data;

        public void SetPhotoData()
        {
            if(parent.Data != data)
                parent.Data = data;
        }
    }
}