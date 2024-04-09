using System;
using System.Collections;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace Framework.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewPhotoData", menuName = "Photo", order = 0)]
    public sealed class PhotoData : ScriptableObject
    {
        private const string NO_TEXTURE_TO_SAVE = "No texture to save.";
        private const string NO_TEXTURE_TO_LOAD = "No texture data to load.";
        private const string SAVED_TEXTURE = "savedTexture.png";
        private const string TAKE_PHOTO = "Take a photo of this to learn about it.";
        
        [SerializeField] private string title = "Object title";
        [SerializeField, TextArea(3, 6)] private string info ;

        public Texture2D a;
        private byte[] _textureBytes;

        public (string, string) Info => (info, TAKE_PHOTO);
        public string Title => title;

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
            
            _textureBytes = targetTexture.EncodeToPNG();
            string filePath = Path.Combine(Application.persistentDataPath, name + "_" +  SAVED_TEXTURE);
            
            // File.WriteAllBytes(filePath, _textureBytes);
            File.WriteAllBytesAsync(filePath, _textureBytes);

            return true;
        }

        /// <summary>
        /// Load the texture from the saved byte array
        /// </summary>
        /// <returns>The photo that is saved in memory</returns>
        public void LoadTexture()
        {
            string filePath = Path.Combine(Application.persistentDataPath, name + "_" + SAVED_TEXTURE);

            if (!File.Exists(filePath))
            {
                filePath = Path.Combine(Application.persistentDataPath, SAVED_TEXTURE);
                
                if (!File.Exists(filePath))
                    throw new Exception(NO_TEXTURE_TO_LOAD);
            }
            
            Task<byte[]> loadingBytes = File.ReadAllBytesAsync(filePath);
            _textureBytes = loadingBytes.Result;
            Texture2D loadedTexture = new Texture2D(2, 2);
            loadedTexture.LoadImage(_textureBytes);
            a = loadedTexture;
        }
        
        public async Task LoadTextureAsync()
        {
            string filePath = Path.Combine(Application.persistentDataPath, name + "_" + SAVED_TEXTURE);

            if (!File.Exists(filePath))
            {
                filePath = Path.Combine(Application.persistentDataPath, SAVED_TEXTURE);

                if (!File.Exists(filePath))
                    throw new Exception(NO_TEXTURE_TO_LOAD);
            }

            byte[] textureBytes = await File.ReadAllBytesAsync(filePath);
            Texture2D loadedTexture = new Texture2D(2, 2);
            loadedTexture.LoadImage(textureBytes);
            a = loadedTexture;
        }
    }
}