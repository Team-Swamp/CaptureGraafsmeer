using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Framework.ScriptableObjects;
using Framework.PhoneCamera;

namespace UI.Canvas.PhoneBook
{
    [RequireComponent(typeof(RectTransform))]
    public sealed class Page : MonoBehaviour
    {
        private readonly Vector3 _closed = new (0, 1, 1);
        
        [Header("References")]
        [SerializeField] private RawImage photo;
        [SerializeField] private TMP_Text title;
        [SerializeField] private TMP_Text info;
        [SerializeField] private PhotoInteractable interactable;

        [Header("Data")]
        [SerializeReference] private PhotoData data;
        [SerializeField] private float animationDuration;
        [SerializeField] private AnimationCurve animationCurve;

        private RectTransform _rect;

        private void Awake()
        {
            _rect = GetComponent<RectTransform>();
            SetProperties();
        }
        
        public void SetProperties()
        {
            title.text = data.Title;
            photo.texture = interactable.GetTextureAsync().Result;
            info.text = interactable.GetInfo();
        }
        
        public void ForceClose() => _rect.localScale = _closed;
        
        public void ForceOpen() => _rect.localScale = Vector3.one;

        public void AnimatePage(bool isOpening)
            => StartCoroutine(AnimateScale(isOpening ? _closed : Vector3.one));

        private IEnumerator AnimateScale(Vector3 targetScale)
        {
            Vector3 initialScale = _rect.localScale;
            float elapsedTime = 0f;

            while (elapsedTime < animationDuration)
            {
                elapsedTime += Time.deltaTime;
                float timeLeft = Mathf.Clamp01(elapsedTime / animationDuration);
                float curveValue = animationCurve.Evaluate(timeLeft);
                _rect.localScale = Vector3.Lerp(initialScale, targetScale, curveValue);
                yield return null;
            }

            _rect.localScale = targetScale;

            if (targetScale == Vector3.zero)
                gameObject.SetActive(false);
        }
    }
}