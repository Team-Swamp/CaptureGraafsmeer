using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using FrameWork.Structs;

namespace UI.Canvas.Introduction
{
    [RequireComponent(typeof(RectTransform))]
    public sealed class IntroductionPopup : PageHolder, IScalable
    {
        private readonly Vector3 _openScale = new(0.8f, 0.8f, 1);
        private readonly List<Image> _dots = new();

        [Header("References")]
        [SerializeField] private TMP_Text infoText;
        [SerializeField] private RawImage image;

        [Header("Dots")]
        [SerializeField] private Transform dotsParent;
        [SerializeField] private GameObject dot;
        [SerializeField] private Color unselectedDot = new(0.6313726f, 0.6313726f, 0.6313726f);
        [SerializeField] private Color selectedDot = new(0.8901961f, 0.02352941f, 0.07450981f);

        [Header("Attributes")]
        [SerializeField, Range(0.1f, 10)] private float activeDelay;
        [SerializeField, Range(0.1f, 1)] private float animationDuration;
        [SerializeField] private IntroductionPage[] pages;

        [Header("Events")]
        [SerializeField] private UnityEvent onFirstShow = new();
        [SerializeField] private UnityEvent onReadIntroduction = new();

        private RectTransform _rect;

        private void Awake() => _rect = GetComponent<RectTransform>();

        private void Start()
        {
            CheckButtonsUsability(pages.Length);
            InitPaginationDots();
            SetCurrentItem(null);
            p_closeButton.SetActive(false);
            Invoke(nameof(Activate), activeDelay);
            
            _rect.localScale = Vector3.zero;
        }

        /// <summary>
        /// Scales the pop-up form nothing to almost full display, or otherwise
        /// </summary>
        /// <param name="targetScale">The target scale to be sized as</param>
        IEnumerator IScalable.AnimateScale(Vector3 targetScale)
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

        /// <summary>
        /// Opens the information panel with a small growing animation
        /// </summary>
        public void Open()
        {
            StopAllCoroutines();
            StartCoroutine(((IScalable) this).AnimateScale(_openScale));
        }

        /// <summary>
        /// Closes the information panel with a small shrinking animation
        /// </summary>
        public void Close()
        {
            StopAllCoroutines();
            StartCoroutine(((IScalable) this).AnimateScale(Vector3.zero));
        }

        /// <summary>
        /// Get the next info for displacement.
        /// </summary>
        public void GetNextInfo()
        {
            SetNextItem(pages.Length);
            p_currentIndex++;
            SetCurrentItem(true);

            if (p_currentIndex == pages.Length - 1)
            {
                p_closeButton.SetActive(true);
                onReadIntroduction?.Invoke();
            }
        }

        /// <summary>
        /// Get the previous info for displacement.
        /// </summary>
        public void GetBackInfo()
        {
            SetPreviousItem();
            SetCurrentItem(false);
        }

        /// <summary>
        /// Set the current item to display. Plus changes pagination dots colors.
        /// </summary>
        /// <param name="isIncreasing">When true it will change the previous dot as unselected and otherwise.
        /// If null it does nothing with dots color.</param>
        protected override void SetCurrentItem(bool? isIncreasing)
        {
            infoText.text = pages[p_currentIndex].Info();
            image.texture = pages[p_currentIndex].Image();
            _dots[p_currentIndex].color = selectedDot;

            switch (isIncreasing)
            {
                case null:
                    return;
                case true:
                    _dots[p_currentIndex - 1].color = unselectedDot;
                    break;
                default:
                    _dots[p_currentIndex + 1].color = unselectedDot;
                    break;
            }
        }

        private void InitPaginationDots()
        {
            int lenght = pages.Length;

            for (int i = 0; i < lenght; i++)
            {
                GameObject currentDot = Instantiate(dot, dotsParent, false);
                _dots.Add(currentDot.GetComponent<Image>());
                _dots[i].color = unselectedDot;
            }

            _dots[0].color = selectedDot;
        }
        
        private void Activate()
        {
            gameObject.SetActive(true);
            Open();
            onFirstShow?.Invoke();
        }
    }
}