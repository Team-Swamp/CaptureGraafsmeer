using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Framework.SaveLoadSystem;

namespace UI.Canvas.RouteLineSettings
{
    public sealed class RouteLineWidth : MonoBehaviour
    {
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private Slider widthSlider;
        [SerializeField] private TMP_Text sliderText;

        private void Awake() => SetWidth(Saver.Instance.RouteWidth);

        /// <summary>
        /// Changes the width of the Line Renderer's line. Value is divided by 2 for easier control over the line width.
        /// </summary>
        public void ChangeWidth()
        {
            float adjustedValue = widthSlider.value / 2;
            SetWidth(adjustedValue);
            Saver.Instance.RouteWidth = adjustedValue;
        }

        private void SetWidth(float targetWidth)
        {
            sliderText.text = $"{targetWidth}";
            lineRenderer.widthCurve.keys[1].value = targetWidth;
            lineRenderer.endWidth = targetWidth;
        }
    }   
}