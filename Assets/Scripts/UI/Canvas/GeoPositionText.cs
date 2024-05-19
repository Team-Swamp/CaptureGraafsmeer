using System;
using TMPro;
using UnityEngine;

namespace UI.Canvas
{
    public sealed class GeoPositionText : MonoBehaviour
    {
        private const string NO_LOCATION_ERROR = "Location services not enabled on device";
        
        [SerializeField] private Transform player;
        [SerializeField] private TMP_Text locationText;

        private void Start()
        {
            // if (!Input.location.isEnabledByUser)
            // {
            //     Destroy(locationText);
            //     locationText = null;
            //     throw new Exception(NO_LOCATION_ERROR);
            // }

            SetText(0, 0);
        }

        private void Update()
        {
            if (player)
                // && Input.location.status == LocationServiceStatus.Running)
                SetText(player.position.x, player.position.z);
        }

        private void OnEnable()
        {
            Input.location.Start();
            Input.location.Start(1f, 1f);
        }

        private void OnDisable() => Input.location.Stop();

        private void SetText(float xPos, float yPos) => locationText.text = $"Position: {xPos}, {yPos}";
    }
}