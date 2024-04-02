using System;
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

        private LocationUpdater _player;

        private void Awake()
        {
            if (isPlayer)
                _player = GetComponent<LocationUpdater>();
            
            if(isStatic
               && isPlayer)
                Debug.LogError(PLAYER_STATIC_ERROR);
        }

        private void Start()
        {
            if (isStatic)
                UpdateLocation(null);
        }

        private void Update()
        {
            if(isStatic)
                return;
            
            UpdateLocation(isPlayer ? _player.GetLiveLocation() : null);
        }

        private void UpdateLocation(Vector2 ?pos)
        {
            Vector2 targetPosition = pos ?? new Vector2(coordinates.x, coordinates.y);
            targetPosition.Subtract(origin);
            (double latitude, double longitude) = ConvertToMeters(targetPosition.x, -targetPosition.y);
            
            transform.position = new Vector3(
                (float) latitude, 
                0,
                (float) longitude);
        }

        private (double, double) ConvertToMeters(double latitude, double longitude)
        {
            double latInRadians = latitude * Math.PI / HALF_CIRCLE;
            double lonInRadians = longitude * Math.PI / HALF_CIRCLE;
            
            double latitudeInMeters = latInRadians * EARTH_RADIUS;
            double longitudeInMeters = lonInRadians * EARTH_RADIUS;
            
            return (latitudeInMeters, longitudeInMeters);
        }
    }
}