using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Canvas.Introduction
{
    [RequireComponent(typeof(RectTransform))]
    public sealed class IntroductionPopup : PageHolder
    {
        private const string NOT_SAME_LENGHT_ERROR = "infos and images need to be the same ";

        private readonly Vector3 _openScale = new(0.8f, 0.8f, 1);
        private readonly List<Image> _dots = new();

        [Header("Refrences")]
        [SerializeField] private TMP_Text infoText;
        [SerializeField] private RawImage image;

        [Header("Dots")]
        [SerializeField] private Transform dotsParent;
        [SerializeField] private GameObject dot;
        [SerializeField] private Color unselectedDot = new(0.6313726f, 0.6313726f, 0.6313726f);
        [SerializeField] private Color selectedDot = new(0.8901961f, 0.02352941f, 0.07450981f);

        [Header("Infos")]
        [SerializeField, Range(0.1f, 1)] private float animationDuration;
        [SerializeField, TextArea(3, 6)] private string[] infos;
        [SerializeField] private Texture2D[] images;

        private RectTransform _rect;

        private void Awake() => _rect = GetComponent<RectTransform>();

        private void Start()
        {
            if (images.Length != infos.Length)
                throw new Exception(NOT_SAME_LENGHT_ERROR);

            CheckButtonsUsability(images.Length);

            InitPaginationDots();
            SetCurrentItem(null);
            closeButton.SetActive(false);
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

        public void GetNextInfo()
        {
            SetNextItem(images.Length);
            p_currentIndex++;
            SetCurrentItem(true);
        }

        public void GetBackInfo()
        {
            SetPreviousItem();
            SetCurrentItem(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isIncreasing"></param>
        protected override void SetCurrentItem(bool? isIncreasing)
        {
            infoText.text = infos[p_currentIndex];
            image.texture = images[p_currentIndex];
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
            int lenght = images.Length;

            for (int i = 0; i < lenght; i++)
            {
                GameObject currentDot = Instantiate(dot, dotsParent, true);
                _dots.Add(currentDot.GetComponent<Image>());
                _dots[i].color = unselectedDot;
            }

            _dots[0].color = selectedDot;
        }
    }
}