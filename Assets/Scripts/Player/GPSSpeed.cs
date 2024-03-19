using System;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class GPSSpeed : MonoBehaviour
    {
        public Transform player;
        public Text speedText;

        private void Start()
        {
            if (!Input.location.isEnabledByUser)
                throw new Exception("Location services not enabled on device");
        }

        private void Update()
        {
            if (player
                && Input.location.status == LocationServiceStatus.Running)
                speedText.text = $"Position: {player.position.x}, {player.position.z}";
        }

        private void OnEnable()
        {
            Input.location.Start();
            Input.location.Start(1f, 1f);
        }

        private void OnDisable() => Input.location.Stop();
    }
}