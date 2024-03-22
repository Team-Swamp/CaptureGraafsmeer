using System;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.InputSystem;

namespace Player
{
    public class LocationUpdater : MonoBehaviour
    {
        private const float SCALE_FACTOR = 100;
        
        [SerializeField] private MeshRenderer rend;
        [SerializeField] private Material[] mats;
        
        private InputAction _locationAction;

        private void OnEnable()
        {
            if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
            {
                Permission.RequestUserPermission(Permission.FineLocation);
                Permission.RequestUserPermission(Permission.CoarseLocation);
            }

            _locationAction = new InputAction(binding: "<Device>/location");
            
            _locationAction.started += _ => Input.location.Start();
            _locationAction.canceled += _ => Input.location.Stop();
            _locationAction.Enable();
        }

        private void OnDisable()
        {
            _locationAction.started -= _ => Input.location.Start();
            _locationAction.canceled -= _ => Input.location.Stop();
            _locationAction.Disable();
        }

        private void Update()
        {
            rend.material = Input.location.status switch
            {
                LocationServiceStatus.Running => mats[0],
                LocationServiceStatus.Failed => mats[1],
                LocationServiceStatus.Initializing => mats[2],
                LocationServiceStatus.Stopped => mats[3],
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public Vector2 GetLiveLocation()
        {
            LocationInfo location = Input.location.lastData;
            return new Vector2(location.latitude, location.longitude);
        }
    }
}