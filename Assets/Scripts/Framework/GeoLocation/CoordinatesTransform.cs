using System;
using System.Collections;
using UnityEngine;

using FrameWork.Extensions;
using Framework.Enums;
using Player;

namespace Framework.GeoLocation
{
    public sealed class CoordinatesTransform : LocationPermission
    {
        private const int HALF_CIRCLE = 180;
        private const int EARTH_RADIUS = 6378137;
        private const string PLAYER_NO_OTHERS_ERROR = "The player needs to refrence other CoordinatesTransform! At least 2 are needed.";
        
        private static readonly Vector2 origin = new (52.356531f, 4.9308f);
        
        [SerializeField] private Vector2 coordinates;
        [SerializeField] private CoordinatesTransformType type;
        [SerializeField, Range(1, 25)] private float lerpTime = 2.5f;
        [SerializeField, Range(1, 60)] private float updateTime = 2.5f;

        private LocationUpdater _player;
        private bool _isReactive;
        
        private void Awake()
        {
            if (type is CoordinatesTransformType.PLAYER or CoordinatesTransformType.PLAYER_DEBUG)
                _player = GetComponent<LocationUpdater>();
        }

        /// <summary>
        /// Ask for the location of the device form its inheritance
        /// and will place the CoordinatesTransform once to the correct location
        /// </summary>
        protected override void Start()
        {
            base.Start();
            
            if (type == CoordinatesTransformType.STATIC)
                UpdateLocation(null);
        }

        private void Update()
        {
            if(_isReactive
               || type is CoordinatesTransformType.STATIC)
                return;

            switch (type)
            {
                case CoordinatesTransformType.PLAYER:
                    UpdateLocation(_player.GetLiveLocation());
                    break;
                case CoordinatesTransformType.PLAYER_DEBUG:
                case CoordinatesTransformType.STATIC_DEBUG:
                    UpdateLocation(null);
                    break;
                default:
                    UpdateLocation(null);
                    Debug.LogWarning($"Default was triggered in {gameObject.name}.");
                    break;
            }
        }

        /// <summary>
        /// Set the coordinates to a new value.
        /// </summary>
        /// <param name="targetCords">The target coordinates</param>
        public void SetCords(Vector2 targetCords) => coordinates = targetCords;

        private void UpdateLocation(Vector2 ?pos)
        {
            Vector2 targetPosition = pos ?? coordinates;
            targetPosition.Subtract(origin);
            (double latitude, double longitude) = ConvertToMeters(targetPosition.x, -targetPosition.y);
            Vector3 finalTargetPosition = new Vector3((float) latitude, 0, (float) longitude);
            
            if (_isReactive)
                return;
            
            StartCoroutine(LerpPosition(finalTargetPosition));
        }

        private (double, double) ConvertToMeters(double latitude, double longitude)
        {
            double latitudeInMeters = latitude * Math.PI / HALF_CIRCLE * EARTH_RADIUS;
            double longitudeInMeters = longitude * Math.PI / HALF_CIRCLE * EARTH_RADIUS;
            
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
            Invoke(nameof(ShouldUpdateLocation), updateTime);
        }

        private void ShouldUpdateLocation() => _isReactive = false;
    }
}