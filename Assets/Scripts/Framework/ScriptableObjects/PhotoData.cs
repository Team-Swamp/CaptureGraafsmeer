using System.IO;
using UnityEngine;

namespace Framework.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewPhotoData", menuName = "Photo", order = 0)]
    [System.Serializable]
    public sealed class PhotoData : ScriptableObject
    {
        public byte[] textureBytes;

        // Save the texture to a byte array
        public void SaveTexture(Texture2D texture)
        {
            if (texture == null)
            {
                Debug.LogWarning("No texture to save.");
                return;
            }

            textureBytes = texture.EncodeToPNG();
            string filePath = Path.Combine(Application.persistentDataPath, "savedTexture.png");
            File.WriteAllBytes(filePath, textureBytes);
        }

        // Load the texture from the saved byte array
        public Texture2D LoadTexture()
        {
            string filePath = Path.Combine(Application.persistentDataPath, "savedTexture.png");
            if (!File.Exists(filePath))
            {
                Debug.LogWarning("No texture data to load.");
                return null;
            }

            textureBytes = File.ReadAllBytes(filePath);

            Texture2D loadedTexture = new Texture2D(2, 2); // Initialize with dummy values
            loadedTexture.LoadImage(textureBytes);
            return loadedTexture;
        }
    }
}