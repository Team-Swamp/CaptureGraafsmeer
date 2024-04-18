using System;
using System.Collections;
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

        [SerializeField] private TMP_Text infoText;
        [SerializeField] private TMP_Text pageNumber;
        [SerializeField] private RawImage image;
        [SerializeField] private GameObject nextButton;
        [SerializeField] private GameObject previousButton;
        [SerializeField] private GameObject closeButton;
        
        [SerializeField, Range(0.1f, 1)] private float animationDuration;
        [SerializeField, TextArea(3, 6)] private string[] infos;
        [SerializeField] private Texture2D[] images;
        
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
            
            SetInfo();
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
            
            SetInfo();
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
            SetInfo();
        }

        private void SetInfo()
        {
            infoText.text = infos[_currentInfo];
            image.texture = images[_currentInfo];
            
            pageNumber.text = _currentInfo + 1 + "/" + images.Length;
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