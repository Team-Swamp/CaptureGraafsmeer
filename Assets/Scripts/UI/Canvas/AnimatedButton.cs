using System.Collections;
using FrameWork.Extensions;
using UnityEngine;

namespace UI.Canvas
{
    [RequireComponent(typeof(RectTransform))]
    public sealed class AnimatedButton : MonoBehaviour
    {
        [SerializeField] private Vector2 dir;
        [SerializeField] private float animationDuration;
        [SerializeField] private AnimationCurve animationCurve;

        private RectTransform _rect;
        private bool _isAtBeginPosition = true;

        private void Awake() => _rect = GetComponent<RectTransform>();

        /// <summary>
        /// Yes
        /// </summary>
        public void TogglePlacement()
        {
            Vector3 a = dir;
            StartCoroutine(AnimateScale(_isAtBeginPosition ? a : a.Invert()));

            _isAtBeginPosition = !_isAtBeginPosition;
        }
        
        private IEnumerator AnimateScale(Vector3 addVector)
        {
            Vector3 initialPosition = _rect.localPosition;
            Vector3 targetPosition = initialPosition + addVector;

            float elapsedTime = 0f;

            while (elapsedTime < animationDuration)
            {
                elapsedTime += Time.deltaTime;
                float timeLeft = Mathf.Clamp01(elapsedTime / animationDuration);
                float curveValue = animationCurve.Evaluate(timeLeft);
        
                _rect.localPosition = Vector3.Lerp(initialPosition, targetPosition, curveValue);

                yield return null;
            }
            
            _rect.localPosition = targetPosition;
        }
    }   
}
