using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Canvas.RouteLineSettings
{
    public class RouteLineWidth : MonoBehaviour
    {
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private Slider widthSlider;
        [SerializeField] private TMP_Text sliderText;
    
        /// <summary>
        /// Changes the width of the Line Renderer's line. Value is divided by 2 for easier control over the line width.
        /// </summary>
        public void ChangeWidth()
        {
            float adjustedValue = widthSlider.value / 2;
            sliderText.text = adjustedValue.ToString();
            lineRenderer.startWidth = adjustedValue;
            lineRenderer.endWidth = adjustedValue;
        }
    }   
}