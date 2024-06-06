using System.Collections;
using UnityEngine;

namespace UI.Canvas.Buttons
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
        private Vector3 _activePosition;

        private void Awake() => _rect = GetComponent<RectTransform>();

        private void Start()
        {
            _rect.localPosition = resetPosition;
            _activePosition = _rect.localPosition + (Vector3)direction;
        }

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
            StopAllCoroutines();
            StartCoroutine(AnimateScale(_isAtBeginPosition ? _activePosition : resetPosition));
            _isAtBeginPosition = !_isAtBeginPosition;
        }
        
        private IEnumerator AnimateScale(Vector3 endPosition)
        {
            Vector3 initialPosition = _rect.localPosition;
            float elapsedTime = 0f;

            while (elapsedTime < animationDuration)
            {
                elapsedTime += Time.deltaTime;
                float timeLeft = Mathf.Clamp01(elapsedTime / animationDuration);
                float curveValue = animationCurve.Evaluate(timeLeft);
        
                _rect.localPosition = Vector3.Lerp(initialPosition, endPosition, curveValue);

                yield return null;
            }
            
            _rect.localPosition = endPosition;
        }
    }   
}
