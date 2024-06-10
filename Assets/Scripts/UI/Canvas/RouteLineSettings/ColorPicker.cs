using System.Collections.Generic;
using UnityEngine;

using Framework.Enums;

namespace UI.Canvas.RouteLineSettings
{
    public sealed class ColorPicker : MonoBehaviour
    {
        [SerializeField] private LineRenderer routeLineRenderer;
        
        private List<Color> colorList = new List<Color>()
        {
            Color.red,
            Color.green,
            Color.blue,
            Color.yellow,
            Color.magenta,
            new Color(1f, 0.5f, 0f),
            Color.black
        };
    
        private void Start() => gameObject.SetActive(false);
        
        /// <summary>
        /// Changes the line renderer's line color to a new color
        /// </summary>
        /// <param name="newColor"> The color to set the line to </param>
        public void ChangeColor(ColorChange colorChange)
        {
            Color targetColor = GetColor(colorChange.colorToChange);
            routeLineRenderer.startColor = targetColor;
            routeLineRenderer.endColor = targetColor;
        }
        
        private Color GetColor(RouteColors lineColor)
        {
            int index = (int)lineColor;
            if (index < 0
                || index > colorList.Count)
                return colorList[0];
           
            return colorList[index];
        }
    }
}