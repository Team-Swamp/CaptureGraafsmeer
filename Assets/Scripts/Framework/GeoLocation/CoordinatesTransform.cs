using System;
using System.Collections;
using UnityEngine;
using TMPro;

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
        [SerializeField] private Vector2 scaleFactor = Vector2.one;
        [SerializeField] private CoordinatesTransformType type;
        [SerializeField, Range(1, 25)] private float lerpTime = 2.5f;
        [SerializeField, Range(1, 60)] private float updateTime = 2.5f;
        [SerializeField] private CoordinatesTransform[] others;
        [SerializeField] private TMP_Text locationText;

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

            if (type is CoordinatesTransformType.PLAYER or CoordinatesTransformType.PLAYER_DEBUG
                && others.Length is 0 or <= 2)
                throw new Exception(PLAYER_NO_OTHERS_ERROR);
        }

        private void Update()
        {
            if (type == CoordinatesTransformType.STATIC_DEBUG)
                UpdateLocation(null);
            
            if(type is CoordinatesTransformType.STATIC or CoordinatesTransformType.STATIC_DEBUG
               || !_isReactive)
                return;

            switch (type)
            {
                case CoordinatesTransformType.PLAYER:
                    UpdateLocation(_player.GetLiveLocation());
                    break;
                case CoordinatesTransformType.PLAYER_DEBUG:
                    UpdateLocation(coordinates);
                    break;
                case CoordinatesTransformType.STATIC:
                case CoordinatesTransformType.STATIC_DEBUG:
                default:
                    UpdateLocation(null);
                    break;
            }
        }

        /// <summary>
        /// Set the coordinates to a new value.
        /// </summary>
        /// <param name="targetCords">The target coordinates</param>
        /// <param name="targetScale">The target scale</param>>
        public void SetCordsWithScale(Vector2 targetCords, Vector2 targetScale)
        {
            coordinates = targetCords;
            scaleFactor = targetScale;
        }

        private void UpdateLocation(Vector2 ?pos)
        {
            Vector2 targetPosition = pos ?? new Vector2(coordinates.x, coordinates.y);
            targetPosition.Subtract(origin);
            (double latitude, double longitude) = ConvertToMeters(targetPosition.x, -targetPosition.y);
            Vector3 finalTargetPosition = type is CoordinatesTransformType.PLAYER or CoordinatesTransformType.PLAYER_DEBUG
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
            (CoordinatesTransform closest1, CoordinatesTransform closest2, float weight) = FindTwoClosestGameObjects();
            Vector2 currentScaleFactor = Vector2.Lerp(closest1.scaleFactor, closest2.scaleFactor, weight);

            locationText.text = $"Current scale: {scaleFactor}\n" +
                                $"Closest object: {closest1.name}\n" +
                                $"Second closest object: {closest2.name}\n" +
                                $"Weight: {weight}";

            Vector3 finalPosition = new Vector3((float)latitude, 0, (float)longitude);
            finalPosition.x *= currentScaleFactor.x;
            finalPosition.z *= currentScaleFactor.y;

            return finalPosition;
        }
        
        private (CoordinatesTransform, CoordinatesTransform, float) FindTwoClosestGameObjects()
        {
            CoordinatesTransform closest1 = null;
            CoordinatesTransform closest2 = null;
            float shortestDistance = float.MaxValue;
            float secondShortestDistance = float.MaxValue;

            foreach (var currentOtherTransform in others)
            {
                float distance = Vector2.Distance(currentOtherTransform.coordinates, coordinates);

                if (distance < shortestDistance)
                {
                    closest2 = closest1;
                    secondShortestDistance = shortestDistance;
                    closest1 = currentOtherTransform;
                    shortestDistance = distance;
                }
                else if (distance < secondShortestDistance)
                {
                    closest2 = currentOtherTransform;
                    secondShortestDistance = distance;
                }
            }
            
            float weight = 100f * shortestDistance / (shortestDistance + secondShortestDistance);
            weight = Mathf.Clamp(weight, 0, 100);

            return (closest1, closest2, weight);
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