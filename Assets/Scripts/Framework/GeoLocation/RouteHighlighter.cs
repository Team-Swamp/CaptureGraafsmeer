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
        
        [SerializeField] private List<Transform> routePoints;
        [SerializeField] private LineRenderer route;
        [SerializeField, Range(1, 100)] private float closeRange = 5f;
        
        private Transform _nextPoint;
        
        [SerializeField] private UnityEvent onWithinRange = new UnityEvent();
        [SerializeField] private UnityEvent onRouteDone = new UnityEvent();

        private void Awake()
        {
            if (route == null)
                route.GetComponent<LineRenderer>();
        }

        private void Start()
        {
            Invoke(nameof(UpdateLine), INVOKE_DELAY);
            _nextPoint = routePoints[1];
        }

        private void Update()
        {
            route.SetPosition(0, transform.position);
            
            if (Vector3.Distance(transform.position, _nextPoint.position) < closeRange)
            {
                onWithinRange?.Invoke();
                RemoveLine();
            }
        }

        private void RemoveLine()
        {
            if (routePoints.Count <= 2)
            {
                onRouteDone?.Invoke();
                route.enabled = false;
                routePoints.Clear();
                return;
            }
            
            routePoints.RemoveAt(1);
            _nextPoint = routePoints[1];
            route.positionCount = 0;
            UpdateLine();
        }
        
        private void UpdateLine()
        {
            try
            {
                foreach (var point in routePoints)
                {
                    route.positionCount++;
                    int index = route.positionCount - 1;
                    route.SetPosition(index, point.position);
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