using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Player
{
    public sealed class LocationUpdater : MonoBehaviour
    {
        private const string INPUT_ACTION_NAME= "<Device>/location";
        
        [FormerlySerializedAs("rend")] [SerializeField] private SpriteRenderer sprite;
        [FormerlySerializedAs("mats")] [SerializeField] private Color[] colors;
        
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
            sprite.color = Input.location.status switch
            {
                LocationServiceStatus.Running => colors[0],
                LocationServiceStatus.Failed => colors[1],
                LocationServiceStatus.Initializing => colors[2],
                LocationServiceStatus.Stopped => colors[3],
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        /// <summary>
        /// Get the live location of the player.
        /// </summary>
        /// <returns>Returns a Vector2 with latitude & longitude</returns>
        public Vector2 GetLiveLocation()
        {
            if (!Input.location.isEnabledByUser)
                return Vector2.zero;
            
            LocationInfo location = Input.location.lastData;
            return new Vector2(location.latitude, location.longitude);
        }
    }
}