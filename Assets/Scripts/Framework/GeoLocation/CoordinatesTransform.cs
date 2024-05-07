using System;
using System.Collections;
using UnityEngine;

using FrameWork.Extensions;
using Player;

namespace Framework.GeoLocation
{
    public sealed class CoordinatesTransform : LocationPermission
    {
        private const int HALF_CIRCLE = 180;
        private const int EARTH_RADIUS = 6378137;
        private const string PLAYER_STATIC_ERROR = "The player is not a static CoordinatesTransform!";
        
        private static readonly Vector2 origin = new (52.356531f, 4.930800f);

        [SerializeField] private Vector2 coordinates;
        [SerializeField] private bool isStatic;
        [SerializeField] private bool isPlayer;
        [SerializeField, Range(1, 25)] private float lerpTime;

        private LocationUpdater _player;
        private bool _isReactive;

        private void Awake()
        {
            if (isPlayer)
                _player = GetComponent<LocationUpdater>();
            
            if(isStatic
               && isPlayer)
                Debug.LogError(PLAYER_STATIC_ERROR);
        }

        /// <summary>
        /// Ask for the location of the device form its inheritance
        /// and will place the CoordinatesTransform once to the correct location
        /// </summary>
        protected override void Start()
        {
            base.Start();
            
            if (isStatic)
                UpdateLocation(null);
        }

        private void Update()
        {
            if(isStatic)
                return;
            
            UpdateLocation(isPlayer ? _player.GetLiveLocation() : null);
        }

        /// <summary>
        /// Set the coordinates to a new value.
        /// </summary>
        /// <param name="targetCords">The target coordinates</param>
        public void SetCords(Vector2 targetCords) => coordinates = targetCords;
        
        private void UpdateLocation(Vector2 ?pos)
        {
            Vector2 targetPosition = pos ?? new Vector2(coordinates.x, coordinates.y);
            targetPosition.Subtract(origin);
            (double latitude, double longitude) = ConvertToMeters(targetPosition.x, -targetPosition.y);
            
            Vector3 finalTargetPosition = new Vector3((float)latitude, 0, (float)longitude);

            if (_isReactive)
                return;
            
            StartCoroutine(LerpPosition(finalTargetPosition));
        }

        private (double, double) ConvertToMeters(double latitude, double longitude)
        {
            double latInRadians = latitude * Math.PI / HALF_CIRCLE;
            double lonInRadians = longitude * Math.PI / HALF_CIRCLE;
            
            double latitudeInMeters = latInRadians * EARTH_RADIUS;
            double longitudeInMeters = lonInRadians * EARTH_RADIUS;
            
            return (latitudeInMeters, longitudeInMeters);
        }
        
        private IEnumerator LerpPosition(Vector3 targetPosition)
        {
            _isReactive = true;
            float timeElapsed = 0f;
            Vector3 startPosition = transform.position;

            while (timeElapsed < lerpTime)
            {
                transform.position = Vector3.Lerp(startPosition, targetPosition, timeElapsed / lerpTime);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            transform.position = targetPosition;
            _isReactive = false;
        }
    }
}