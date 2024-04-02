using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace Framework.GeoLocation
{
    public sealed class RouteHighlighter : MonoBehaviour
    {   
        [SerializeField] private List<Transform> routePoints;
        [SerializeField] private LineRenderer lineRenderer;

        private int _routeIndex;

        private void Start()
        {
            Invoke("InitLines", 0.1f);
        }

        private void InitLines()
        {
            try
            {
                foreach (var point in routePoints)
                {
                    lineRenderer.positionCount++;
                    _routeIndex = lineRenderer.positionCount - 1;
                    lineRenderer.SetPosition(_routeIndex, point.position);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}