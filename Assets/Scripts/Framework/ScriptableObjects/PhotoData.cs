using UnityEngine;

namespace Framework.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewPhotoData", menuName = "Photo", order = 0)]
    public sealed class PhotoData : ScriptableObject
    {
        private const string TAKE_PHOTO = "Take a photo of this to learn about it.";
        
        [SerializeField] private string title = "Object title";
        [SerializeField, TextArea(3, 6)] private string info ;

        public (string, string) Info => (info, TAKE_PHOTO);
        public string Title => title;
    }
}