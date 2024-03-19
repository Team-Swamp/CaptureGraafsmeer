using System;
using FrameWork.Extensions;
using UnityEngine;
using UnityEngine.Serialization;

namespace Framework
{
    public class LanLon : MonoBehaviour
    {
        private const float SCALE_FACTOR = 100;

        [SerializeField] private Vector2 coordinates;
        [SerializeField] private bool isStatic;

        private void Start()
        {
            if (isStatic)
                UpdateLocation();
        }

        private void Update()
        {
            if(isStatic)
                return;
            
            UpdateLocation();
        }

        private void UpdateLocation()
        {
            //todo: haal 53, 4.9 er van af.
            
            Vector2 a = new Vector2(coordinates.x, coordinates.y);
            // a.Multiply(SCALE_FACTOR);
            transform.position = a;
        }
    }
}