using UnityEngine;

namespace Framework.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewPhotoData", menuName = "Photo", order = 0)]
    public sealed class PhotoData : ScriptableObject
    {
        private const string TAKE_PHOTO = "Maak een foto om hier over te leren.";

        [SerializeField] private Texture render;
        [SerializeField] private string title = "Object title";
        [SerializeField, TextArea(3, 6)] private string info ;

        public Texture Render => render;
        
        public (string, string) Info => (info, TAKE_PHOTO);
        
        public string Title => title;
    }
}