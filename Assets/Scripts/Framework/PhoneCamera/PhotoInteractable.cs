using System.Collections;
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

        private bool _isFirstTime = true;
        
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
        
        public async IAsyncEnumerable<Texture2D> GetTextureAsync()
        {
            if (!IsVisited)
            {
                yield return parent.DefaultTex;
                yield break;
            }

            while (true)
            {
                await data.LoadTextureAsync();

                if (data.a != parent.DefaultTex 
                    && parent.Data == data)
                {
                    yield return data.a;
                    yield break;
                }

                // Add a small delay before checking again to avoid tight loop
                await Task.Delay(100); // Adjust delay as needed
            }
        }
        
        public Texture2D GetTexture()
        {
            if (!IsVisited)
                return parent.DefaultTex;
            
            while (true)
            {
                data.LoadTexture();
                
                if(data.a != parent.DefaultTex
                   && parent.Data == data)
                    break;
            }

            if (_isFirstTime)
                return GetTexture();
            
            _isFirstTime = false;
            return data.a;
        }
    }
}