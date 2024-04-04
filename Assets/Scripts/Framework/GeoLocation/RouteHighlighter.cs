using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Framework.GeoLocation
{
    [RequireComponent(typeof(LineRenderer))]
    public sealed class RouteHighlighter : MonoBehaviour
    {
        private const string NO_ROUTE_POINTS_ERROR = "There are no route points assaigned.";
        private const string MORE_THEN_ONE_POINT_ERROR = "Need to assaign more then 1 route point.";
        private const float INVOKE_DELAY = 0.1f;
        
        [SerializeField] private List<Transform> routePoints;
        [SerializeField] private LineRenderer route;
        [SerializeField, Range(1, 100)] private float closeRange = 5f;
        
        private Transform _nextPoint;
        private bool _canUpdate = true;
        
        [SerializeField] private UnityEvent onWithinRange = new ();
        [SerializeField] private UnityEvent onRouteDone = new ();

        private void Awake()
        {
            if (routePoints.Count == 0)
                throw new Exception(NO_ROUTE_POINTS_ERROR);
            
            if (route == null)
                route.GetComponent<LineRenderer>();
        }

        private void Start()
        {
            if (routePoints.Count == 1)
            {
                _canUpdate = false;
                throw new Exception(MORE_THEN_ONE_POINT_ERROR);
            }
            
            // routePoints.RemoveAt(0); // Tried to fix it
            
            _nextPoint = routePoints[1];
            Invoke(nameof(UpdateLine), INVOKE_DELAY);
        }

        private void Update()
        {
            if(!_canUpdate)
                return;
            
            route.SetPosition(0, routePoints[0].position);

            if (!(Vector3.Distance(transform.position, _nextPoint.position) < closeRange))
                return;
            
            onWithinRange?.Invoke();
            RemoveLine();
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