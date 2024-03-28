using System;
using System.IO;
using UnityEngine;

namespace Framework.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewPhotoData", menuName = "Photo", order = 0)]
    public sealed class PhotoData : ScriptableObject
    {
        private const string NO_TEXTURE_TO_SAVE = "No texture to save.";
        private const string NO_TEXTURE_TO_LOAD = "No texture data to load.";
        private const string SAVED_TEXTURE = "savedTexture.png";
        
        private byte[] _textureBytes;

        /// <summary>
        /// Save the texture to a byte array
        /// </summary>
        /// <param name="targetTexture">The target photo to save in memory</param>
        public void SaveTexture(Texture2D targetTexture)
        {
            if (!targetTexture)
                throw new Exception(NO_TEXTURE_TO_SAVE);

            _textureBytes = targetTexture.EncodeToPNG();
            string filePath = Path.Combine(Application.persistentDataPath, SAVED_TEXTURE);
            
            File.WriteAllBytes(filePath, _textureBytes);
        }

        /// <summary>
        /// Load the texture from the saved byte array
        /// </summary>
        /// <returns>The photo that is saved in memory</returns>
        public Texture2D LoadTexture()
        {
            string filePath = Path.Combine(Application.persistentDataPath, SAVED_TEXTURE);
            
            if (!File.Exists(filePath))
                throw new Exception(NO_TEXTURE_TO_LOAD);

            _textureBytes = File.ReadAllBytes(filePath);
            Texture2D loadedTexture = new Texture2D(2, 2);
            loadedTexture.LoadImage(_textureBytes);
            return loadedTexture;
        }
    }
}