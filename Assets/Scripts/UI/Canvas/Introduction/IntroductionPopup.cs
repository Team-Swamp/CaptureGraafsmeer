using System.Collections;
using UnityEngine;

namespace UI.Canvas.Introduction
{
    [RequireComponent(typeof(RectTransform))]
    public sealed class IntroductionPopup : MonoBehaviour
    {
        private readonly Vector3 _openScale = new (0.8f, 0.8f, 1);
        
        [SerializeField, Range(0.1f, 1)] private float animationDuration;
        
        private RectTransform _rect;

        private void Awake() => _rect = GetComponent<RectTransform>();
        
        /// <summary>
        /// Opens the information panel with a small growing animation
        /// </summary>
        public void Open()
        {
            StopAllCoroutines();
            StartCoroutine(AnimateScale(_openScale));
        }

        /// <summary>
        /// Closes the information panel with a small shrinking animation
        /// </summary>
        public void Close()
        {
            StopAllCoroutines();
            StartCoroutine(AnimateScale(Vector3.zero));
        }

        private IEnumerator AnimateScale(Vector3 targetScale)
        {
            Vector3 initialScale = _rect.localScale;
            float elapsedTime = 0f;

            while (elapsedTime < animationDuration)
            {
                elapsedTime += Time.deltaTime;
                float currentTime = Mathf.Clamp01(elapsedTime / animationDuration);
                _rect.localScale = Vector3.Lerp(initialScale, targetScale, currentTime);

                yield return null;
            }

            _rect.localScale = targetScale;
        }
    }
}