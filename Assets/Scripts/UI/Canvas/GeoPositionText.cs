using System;
using TMPro;
using UnityEngine;

namespace UI.Canvas
{
    public sealed class GeoPositionText : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private TMP_Text locationText;

        private void Start()
        {
            #if !UNITY_EDITOR
            if (!Input.location.isEnabledByUser)
            {
                Destroy(locationText);
                locationText = null;
                throw new Exception("Location services not enabled on device");
            }
            #endif

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