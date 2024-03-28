using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class TargetSelector : MonoBehaviour
    {   
        [SerializeField] private List<Transform> targetList;
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private Transform currentTarget;
        [SerializeField] private float closeRange = 5.0f;

        private int currentTargetIndex;

        private void Start()
        {
            currentTargetIndex = 0;
            currentTarget = targetList[1];
            
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, currentTarget.position);

        }

        private void Update()
        {
            lineRenderer.SetPosition(0, transform.position);
            if (Vector3.Distance(transform.position, currentTarget.position) < closeRange)
            {
                currentTargetIndex = (currentTargetIndex + 1) % targetList.Count;
                currentTarget = targetList[currentTargetIndex];
                lineRenderer.SetPosition(1, currentTarget.position);
            }
        }
    }
}