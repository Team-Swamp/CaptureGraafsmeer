using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class TargetSelector : MonoBehaviour
    {   
        public List<Transform> targetList;
        public LineRenderer lineRenderer;
        public Transform currentTarget;
        public float closeRange = 5.0f;

        private int currentTargetIndex;

        private void Start()
        {
            print("test");
            currentTargetIndex = 0;
            currentTarget = targetList[1];
            // Initialize the line renderer and the first target
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, currentTarget.position);

        }

        private void Update()
        {
            lineRenderer.SetPosition(0, transform.position);
            if (Vector3.Distance(transform.position, currentTarget.position) < closeRange)
            {
                // Select the next target in the list
                currentTargetIndex = (currentTargetIndex + 1) % targetList.Count;
                currentTarget = targetList[currentTargetIndex];
                lineRenderer.SetPosition(1, currentTarget.position);
            }
        }
    }
}