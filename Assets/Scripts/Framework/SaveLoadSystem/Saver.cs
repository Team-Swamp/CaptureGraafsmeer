using System;
using UnityEngine;

namespace Framework.SaveLoadSystem
{
    public sealed class Saver : Singleton<Saver>
    {
        private const string PHOTO_KEY = "PhotoProgress";
        private const string ROUTE_KEY = "RouteProgress";
        private const string ROUTE_COLOR = "RouteColor";
        private const string ROUTE_WIDTH = "RouteWidth";
        private const float ROUTE_WIDTH_MARGIN = 0.5f;
        
        private int _photoAmountMade;
        private int _checkpointsPassed;
        private int _routeColorIndex;
        private float _routeWidth;

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
        
        public int RouteColorIndex
        {
            get => _routeColorIndex;
            
            set
            {
                if (value == _routeColorIndex)
                    return;
                
                _routeColorIndex = value;
                PlayerPrefs.SetInt(ROUTE_COLOR, _routeColorIndex);
                PlayerPrefs.Save();
            }
        }
        
        public float RouteWidth
        {
            get => _routeWidth;
            
            set
            {
                if (Math.Abs(value - _routeWidth) < ROUTE_WIDTH_MARGIN)
                    return;
                
                _routeWidth = value;
                PlayerPrefs.SetFloat(ROUTE_WIDTH, _routeWidth);
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
            
            if (PlayerPrefs.HasKey(ROUTE_COLOR))
                _routeColorIndex = PlayerPrefs.GetInt(ROUTE_COLOR);

            if (PlayerPrefs.HasKey(ROUTE_WIDTH))
            {
                _routeWidth = PlayerPrefs.GetFloat(ROUTE_WIDTH);
                Debug.Log(_routeWidth);
                if (_routeWidth == 0)
                    _routeWidth = 1;
            }
        }

        /// <summary>
        /// This will reset the progress of the route and photos made
        /// </summary>
        public void ResetData()
        {
            PhotoAmountMade = 0;
            CheckpointsPassed = 0;
            RouteColorIndex = 0;
            RouteWidth = 1f;
        }
    }
}