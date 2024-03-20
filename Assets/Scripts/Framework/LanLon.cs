using System;
using FrameWork.Extensions;
using UnityEngine;
using UnityEngine.Serialization;

namespace Framework
{
    public class LanLon : MonoBehaviour
    {
        private const float SCALE_FACTOR = 100000;
        
        private static readonly Vector2 minus = new Vector2(52.356531f, 4.930800f);

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
            Vector2 a = new Vector2(coordinates.x, coordinates.y);
            a.Subtract(minus);
            a.Multiply(SCALE_FACTOR);
            transform.position = new Vector3(a.x, 0, a.y);
        }
    }
}