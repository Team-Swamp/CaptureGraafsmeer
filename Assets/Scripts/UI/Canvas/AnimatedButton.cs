using System.Collections;
using UnityEngine;

using FrameWork.Extensions;

namespace UI.Canvas
{
    [RequireComponent(typeof(RectTransform))]
    public sealed class AnimatedButton : MonoBehaviour
    {
        private readonly Vector3 resetPosition = new (0, 100, 0);
        
        [SerializeField] private Vector2 direction;
        [SerializeField] private float animationDuration;
        [SerializeField] private AnimationCurve animationCurve;

        private RectTransform _rect;
        private bool _isAtBeginPosition = true;

        private void Awake() => _rect = GetComponent<RectTransform>();

        /// <summary>
        /// Resets the button the to beginning state, without questions
        /// </summary>
        public void ForceReset()
        {
            _rect.localPosition = resetPosition;
            _isAtBeginPosition = true;
        }
        
        /// <summary>
        /// If the button is at the begin position it will move to the target position, otherwise in reverse.
        /// </summary>
        public void TogglePlacement()
        {
            Vector3 dir = direction;
            StartCoroutine(AnimateScale(_isAtBeginPosition ? dir : dir.Invert()));
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
