using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public sealed class LocationUpdater : MonoBehaviour
    {
        private const string INPUT_ACTION_NAME= "<Device>/location";
        
        [SerializeField] private MeshRenderer rend;
        [SerializeField] private Material[] mats;
        
        private InputAction _locationAction;

        private void OnEnable()
        {
            _locationAction = new InputAction(binding: INPUT_ACTION_NAME);
            
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

        /// <summary>
        /// A test version of the status for the location. This should be removed when we have player model.
        /// </summary>
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

        /// <summary>
        /// Get the live location of the player.
        /// </summary>
        /// <returns>Returns a Vector2 with latitude & longitude</returns>
        public Vector2 GetLiveLocation()
        {
            LocationInfo location = Input.location.lastData;
            return new Vector2(location.latitude, location.longitude);
        }
    }
}