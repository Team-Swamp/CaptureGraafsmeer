using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

using FrameWork;
using Framework.ScriptableObjects;
using UI.Canvas.PhoneBook;

namespace Framework.PhoneCamera
{
    public sealed class PhotoInteractable : InteractableObject
    {
        [SerializeField] private PhotoTaker parent;
        [SerializeReference] private PhotoData data;
        
        public PhotoData Data => data;
        public PhotoTaker Parent => parent;
        public bool IsVisited { get; set; }

        public void SetPhotoData()
        {
            parent.SetCurrentPhotoInteractable(this);
            
            if(parent.Data != data)
                parent.Data = data;
            
            print($"Set {name} as current");
        }

        public void SetThisAsCurrentInteractable()
        {
            
        }

        public string GetInfo()
        {
            (string info, string takePhoto) = data.Info;
            return IsVisited ? info : takePhoto;
        }

        public bool SaveTexture(Texture2D target) => data.SaveTexture(target);

        public async Task<bool> SaveTextureAsync(Texture2D target) => await data.SaveTextureAsync(target);
        
        public async Task<Texture2D> GetTextureAsync() => !IsVisited 
            ? parent.DefaultTex 
            : await data.LoadTextureAsync();
    }
}

