using System;
using System.IO;
using UnityEngine;

using FrameWork;
using Framework.ScriptableObjects;
using UI.Canvas.PhotoBookSystem;
using UI.Canvas.PhotoTaking;

namespace Framework.PhoneCamera
{
    public sealed class PhotoInteractable : InteractableObject
    {
        private const string NO_TEXTURE_TO_SAVE = "No texture to save.";
        private const string NO_TEXTURE_TO_LOAD = "No texture data to load.";
        private const string SAVED_TEXTURE = "savedTexture.png";
        
        [SerializeField] private PhotoTaker parent;
        [SerializeField] private CameraPanel panel;
        [SerializeReference] private PhotoData data;
        
        private byte[] _textureBytes;
        
        public bool IsVisited { get; set; }
        
        public Page ParentPage { get; set; }

        /// <summary>
        /// Will show the camera panel and will set the data correctly
        /// </summary>
        public void ActiveInteract()
        {
            panel.SetPanelData(data);
            panel.gameObject.SetActive(true);
            SetPhotoData();
        }
        
        /// <summary>
        /// Set this PhotoInteractable as the current in the PhotoTaker
        /// </summary>
        public void SetPhotoData()
        {
            parent.SetCurrentPhotoInteractable(this);
            
            if(parent.Data != data)
                parent.Data = data;
        }

        /// <summary>
        /// Get the text to show in the page, if visited will show the text for it, otherwise the default text
        /// </summary>
        /// <returns></returns>
        public string GetInfo()
        {
            (string info, string takePhoto) = data.Info;
            return IsVisited ? info : takePhoto;
        }
        
        /// <summary>
        /// Save the texture to a byte array
        /// </summary>
        /// <param name="targetTexture">The target photo to save in memory</param>
        public bool SaveTexture(Texture2D targetTexture)
        {
            if (!targetTexture)
            {
                Debug.LogError(NO_TEXTURE_TO_SAVE);
                return false;
            }

            _textureBytes = targetTexture.EncodeToJPG();
            string filePath = Path.Combine(Application.persistentDataPath, name + "_" + SAVED_TEXTURE);
            File.WriteAllBytes(filePath, _textureBytes);
            
            return true;
        }
        
        /// <summary>
        /// Get the texture to show, if this is visited it will show the made photo, otherwise the default texture
        /// </summary>
        /// <returns>Returns text for text block depending if the photo has been taken or not</returns>
        public Texture2D GetTexture() => !IsVisited 
            ? parent.DefaultTex 
            : LoadTexture();

        /// <summary>
        /// Set the PhotoData to a new data set.
        /// </summary>
        /// <param name="targetData">The target data</param>
        public void SetPhotoData(PhotoData targetData) => data = targetData;
        
        /// <summary>
        /// Set the parent to a PhotoTaker.
        /// </summary>
        /// <param name="photoTaker">The target PhotoTaker</param>
        public void SetPhotoTaker(PhotoTaker photoTaker) => parent = photoTaker;
        
        /// <summary>
        /// Set the CameraPanel reference to a CameraPanel.
        /// </summary>
        /// <param name="target">The target CameraPanel</param>
        public void SetPanel(CameraPanel target) => panel = target;
        
        private Texture2D LoadTexture()
        {
            string filePath = Path.Combine(Application.persistentDataPath, name + "_" + SAVED_TEXTURE);

            if (!File.Exists(filePath))
            {
                filePath = Path.Combine(Application.persistentDataPath, SAVED_TEXTURE);
                
                if (!File.Exists(filePath))
                    throw new Exception(NO_TEXTURE_TO_LOAD);
            }
            
            byte[] loadingBytes = File.ReadAllBytes(filePath);
            _textureBytes = loadingBytes;
            Texture2D loadedTexture = new Texture2D(2, 2);
            loadedTexture.LoadImage(_textureBytes);
            
            return loadedTexture;
        }
    }
}

