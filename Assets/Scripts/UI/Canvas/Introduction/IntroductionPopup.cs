using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Canvas.Introduction
{
    [RequireComponent(typeof(RectTransform))]
    public sealed class IntroductionPopup : MonoBehaviour
    {
        private const string NOT_SAME_LENGHT_ERROR = "infos and images need to be the same ";
        private readonly Vector3 _openScale = new (0.8f, 0.8f, 1);

        [Header("Refrences")]
        [SerializeField] private TMP_Text infoText;
        [SerializeField] private RawImage image;
        [SerializeField] private GameObject nextButton;
        [SerializeField] private GameObject previousButton;
        [SerializeField] private GameObject closeButton;

        [Header("Dots")]
        [SerializeField] private Transform dotsParent;
        [SerializeField] private GameObject dot;
        [SerializeField] private Color unselectedDot = new (0.6313726f, 0.6313726f, 0.6313726f);
        [SerializeField] private Color selectedDot = new (0.8901961f, 0.02352941f, 0.07450981f);

        [Header("Infos")]
        [SerializeField, Range(0.1f, 1)] private float animationDuration;
        [SerializeField, TextArea(3, 6)] private string[] infos;
        [SerializeField] private Texture2D[] images;
        
        private List<Image> _dots =  new();
        private RectTransform _rect;
        private int _currentInfo;

        private void Awake() => _rect = GetComponent<RectTransform>();

        private void Start()
        {
            if (images.Length != infos.Length)
                throw new Exception(NOT_SAME_LENGHT_ERROR);
            
            if (_currentInfo == 0)
                previousButton.SetActive(false);
            
            if(_currentInfo == infos.Length)
                nextButton.SetActive(false);
            
            InitPaginationDots();
            SetInfo(null);
            closeButton.SetActive(false);
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
            if(images.Length == _currentInfo)
                return;

            if(_currentInfo == images.Length - 2)
                nextButton.SetActive(false);
            
            if(!previousButton.activeSelf)
                previousButton.SetActive(true);
            
            _currentInfo++;
            
            if(_currentInfo == images.Length - 1)
                closeButton.SetActive(true);
            
            SetInfo(true);
        }
        
        public void GetBackInfo()
        {
            if(_currentInfo == 0)
                return;

            if (_currentInfo - 2 == -1)
                previousButton.SetActive(false);
            
            if(!nextButton.activeSelf)
                nextButton.SetActive(true);
            
            closeButton.SetActive(false);
            _currentInfo--;
            SetInfo(false);
        }

        private void SetInfo(bool ?isIncreasing)
        {
            infoText.text = infos[_currentInfo];
            image.texture = images[_currentInfo];
            _dots[_currentInfo].color = selectedDot;
            
            switch (isIncreasing)
            {
                case null:
                    return;
                case true:
                    _dots[_currentInfo - 1].color = unselectedDot;
                    break;
                default:
                    _dots[_currentInfo + 1].color = unselectedDot;
                    break;
            }
        }

        private void InitPaginationDots()
        {
            int lenght = images.Length;
            
            for (int i = 0; i < lenght; i++)
            {
                GameObject currentDot = Instantiate(dot);
                currentDot.transform.parent = dotsParent;
                _dots.Add(currentDot.GetComponent<Image>());
                _dots[i].color = unselectedDot;
            }

            _dots[0].color = selectedDot;
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