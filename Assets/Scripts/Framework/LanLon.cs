using System;
using UnityEngine;

using FrameWork.Extensions;
using Player;

namespace Framework
{
    public sealed class LanLon : MonoBehaviour
    {
        private const float SCALE_FACTOR = 100000;
        private const double EARTH_RADIUS = 6378137;
        
        private static readonly Vector2 origin = new (52.356531f, 4.930800f);

        [SerializeField] private Vector2 coordinates;
        [SerializeField] private bool isStatic;
        [SerializeField] private bool isPlayer;

        private LocationUpdater _player;


        private void Awake()
        {
            if (isPlayer)
                _player = GetComponent<LocationUpdater>();
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
            Vector2 targetPosition;
            
            if(pos == null)
                targetPosition = new Vector2(coordinates.x, coordinates.y);
            else
                targetPosition = (Vector2) pos;
            
            targetPosition.Subtract(origin);
            double[] a = tada(targetPosition.x, -targetPosition.y);
            
            transform.position = new Vector3(
                (float) a[0], 
                0,
                (float) a[1]);
            
            Debug.Log($"{this.name}: {transform.position}");
        }

        private double[] tada(double latitude, double longitude)
        {
            double latInRadians = latitude * Math.PI / 180;
            double lonInRadians = longitude * Math.PI / 180;
            
            double metersPerDegreeLat = EARTH_RADIUS * Math.PI / 180;
            double metersPerDegreeLon = EARTH_RADIUS * Math.PI / 180 * Math.Cos(latInRadians);
            
            double latitudeInMeters = latInRadians * EARTH_RADIUS;
            double longitudeInMeters = lonInRadians * EARTH_RADIUS;
            
            return new [] { latitudeInMeters, longitudeInMeters };
        }
    }
}