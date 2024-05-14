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
        private const string PLAYER_NO_OTHERS_ERROR = "The player needs to refrence other CoordinatesTransform! At least 2 are needed.";
        
        private static readonly Vector2 origin = new (52.356531f, 4.9308f);
        
        [SerializeField] private Vector2 coordinates;
        [SerializeField] private Vector2 scaleFactor = Vector2.one;
        [SerializeField] private bool isStatic;
        [SerializeField] private bool isPlayer;
        [SerializeField] private bool isDebugTesting;
        [SerializeField, Range(1, 25)] private float lerpTime = 2.5f;
        [SerializeField] private CoordinatesTransform[] others;

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

            if (isPlayer
                && others.Length == 0
                || others.Length <= 2)
                throw new Exception(PLAYER_NO_OTHERS_ERROR);
        }

        private void Update()
        {
            if (isDebugTesting)
                UpdateLocation(null);
            
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
            Vector3 finalTargetPosition = isPlayer
                ? BlendPlayerPosition(latitude, longitude)
                : new Vector3((float)latitude, 0, (float)longitude);

            if (_isReactive)
                return;
            
            StartCoroutine(LerpPosition(finalTargetPosition));
        }

        private (double, double) ConvertToMeters(double latitude, double longitude)
        {
            double latitudeInMeters = latitude * Math.PI / HALF_CIRCLE * EARTH_RADIUS * scaleFactor.x;
            double longitudeInMeters = longitude * Math.PI / HALF_CIRCLE * EARTH_RADIUS * scaleFactor.y;

            return (latitudeInMeters, longitudeInMeters);
        }

        private Vector3 BlendPlayerPosition(double latitude, double longitude)
        {
            (CoordinatesTransform closeted, CoordinatesTransform secondCloseted) = FindTwoClosestGameObjects();

            scaleFactor = scaleFactor.Multiply(closeted.scaleFactor)
                .Midpoint(secondCloseted.scaleFactor);
            Vector3 finalPosition = new ((float) latitude, 0, (float) longitude);
            finalPosition.Multiply(scaleFactor);
            
            return finalPosition;
        }
        
        private (CoordinatesTransform, CoordinatesTransform) FindTwoClosestGameObjects()
        {
            CoordinatesTransform closest1 = null;
            CoordinatesTransform closest2 = null;
            float shortestDistance = float.MaxValue;

            foreach (var currentOtherTransform in others)
            {
                float distance = Vector3.Distance(currentOtherTransform.gameObject.transform.position, transform.position);
                
                if (distance < shortestDistance)
                {
                    closest2 = closest1;
                    closest1 = currentOtherTransform;
                    shortestDistance = distance;
                }
                else if (closest2 == null
                         || distance < Vector3.Distance(closest2.transform.position, transform.position))
                {
                    closest2 = currentOtherTransform;
                }
            }

            return (closest1, closest2);
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