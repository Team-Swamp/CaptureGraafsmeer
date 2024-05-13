using UnityEngine;

namespace Framework.SaveLoadSystem
{
    public sealed class Saver : Singleton<Saver>
    {
        private const string PHOTO_KEY = "PhotoProgress";
        private const string ROUTE_KEY = "RouteProgress";
        
        private int _photoAmountMade;
        private int _checkpointsPassed;

        public int PhotoAmountMade
        {
            get => _photoAmountMade;
            
            set
            {
                if (value == _photoAmountMade) 
                    return;
                
                _photoAmountMade = value;
                PlayerPrefs.SetInt(PHOTO_KEY, _photoAmountMade);
                PlayerPrefs.Save();
            }
        }
        
        public int CheckpointsPassed
        {
            get => _checkpointsPassed;
            
            set
            {
                if (value == _checkpointsPassed)
                    return;
                
                _checkpointsPassed = value;
                PlayerPrefs.SetInt(ROUTE_KEY, _checkpointsPassed);
                PlayerPrefs.Save();
            }
        }
        
        protected override void Awake()
        {
            base.Awake();
            
            if (PlayerPrefs.HasKey(PHOTO_KEY))
                _photoAmountMade = PlayerPrefs.GetInt(PHOTO_KEY);
            
            if (PlayerPrefs.HasKey(ROUTE_KEY))
                _checkpointsPassed = PlayerPrefs.GetInt(ROUTE_KEY);
        }
    }
}