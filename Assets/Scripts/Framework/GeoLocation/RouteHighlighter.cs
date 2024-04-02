using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Framework.GeoLocation
{
    [RequireComponent(typeof(LineRenderer))]
    public sealed class RouteHighlighter : MonoBehaviour
    {
        private const float INVOKE_DELAY = 0.1f;

        [SerializeField] private UnityEvent withinRange = new UnityEvent();
        
        [SerializeField] private List<Transform> routePoints;
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private float closeRange = 5f;
        
        [SerializeField] private Transform _nextPoint;

        private void Start()
        {
            Invoke(nameof(InitLine), INVOKE_DELAY);
            _nextPoint = routePoints[1];
            
        }

        private void Update()
        {
            if (Vector3.Distance(transform.position, _nextPoint.position) < closeRange)
            {
                withinRange?.Invoke();
            }
        }

        public void RemoveLine()
        {
            if (routePoints.Count <= 2)
            {
                lineRenderer.enabled = false;
                routePoints.Clear();
            }
            else
            {
                _nextPoint = routePoints[2];
                routePoints.RemoveAt(0);
                lineRenderer.positionCount = 0;
                InitLine();
            }

        }
        private void InitLine()
        {
            try
            {
                foreach (var point in routePoints)
                {
                    lineRenderer.positionCount++;
                    int index = lineRenderer.positionCount - 1;
                    lineRenderer.SetPosition(index, point.position);
                    print(point);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
        }
    }
}