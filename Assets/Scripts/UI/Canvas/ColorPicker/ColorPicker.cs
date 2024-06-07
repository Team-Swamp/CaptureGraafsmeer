using System;
using System.Collections.Generic;
using UnityEngine;

using Framework.Enums;

namespace UI.Canvas.ColorPicker
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
            switch (lineColor)
            {
                case RouteColors.RED:
                    return colorList[0];
                case RouteColors.GREEN:
                    return colorList[1];
                case RouteColors.BLUE:
                    return colorList[2];
                case RouteColors.YELLOW:
                    return colorList[3];
                case RouteColors.MAGENTA:
                    return colorList[4];
                case RouteColors.ORANGE:
                    return colorList[5];
                case RouteColors.BLACK:
                    return colorList[6];
                default:
                    throw new ArgumentOutOfRangeException(nameof(lineColor), lineColor, null);
            }
        }
    }
}