using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Canvas.Buttons
{
    public sealed class FlashingButton : MonoBehaviour
    {
        private const string ERROR = "Game object should be on when calling this function.";
        
        [SerializeField] private Image buttonImage;
        [SerializeField] private float flashingSpeed = 0.75f;
        [SerializeField] private Color startColor = Color.white;
        [SerializeField] private Color flashedColor = Color.gray;

        private Coroutine _flashingCoroutine;

        /// <summary>
        /// 
        /// </summary>
        public void ActivateFlashing()
        {
            if (!gameObject.activeSelf)
            {
                Debug.LogError(ERROR);
                return;
            }
            
            _flashingCoroutine ??= StartCoroutine(Flashing());
        }

        /// <summary>
        /// 
        /// </summary>
        public void StopFlashing()
        {
            if (_flashingCoroutine == null)
                return;
            
            StopCoroutine(_flashingCoroutine);
            _flashingCoroutine = null;
            buttonImage.color = startColor;
        }
        
        private IEnumerator Flashing()
        {
            while (true)
            {
                float elapsedTime = 0f;
                while (elapsedTime < flashingSpeed)
                {
                    buttonImage.color = Color.Lerp(startColor, flashedColor, elapsedTime / flashingSpeed);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
                
                elapsedTime = 0f;
                while (elapsedTime < flashingSpeed)
                {
                    buttonImage.color = Color.Lerp(flashedColor, startColor, elapsedTime / flashingSpeed);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
            }
        }
    }
}