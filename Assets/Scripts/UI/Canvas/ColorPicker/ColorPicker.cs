using Framework.Enums;
using UnityEngine;

namespace UI.Canvas.ColorPicker
{
    public class ColorPicker : MonoBehaviour
    {
        [SerializeField] private LineRenderer routeLineRenderer;
    
        private void Start()
        {
            gameObject.SetActive(false);
        }

        public Color SetColor(RouteColors lineColor)
        {
            Color newColor = new Color();
            
            switch (lineColor)
            {
                case RouteColors.RED:
                    newColor = new Color(1f,0f,0f);
                    break;
                case RouteColors.GREEN:
                    newColor = new Color(0f, 1f, 0f);
                    break;
                case RouteColors.BLUE:
                    newColor = new Color(0f, 0f, 1f);
                    break;
                case RouteColors.YELLOW:
                    newColor = new Color(1f, 1f, 0f);
                    break;
                case RouteColors.MAGENTA:
                    newColor = new Color(1f, 0f, 1f);
                    break;
                case RouteColors.ORANGE:
                    newColor = new Color(1f, 0.5f, 0f);
                    break;
            }
            
            return newColor;
        }
        
        /// <summary>
        /// Changes the line renderer's line color to a new color
        /// </summary>
        /// <param name="newColor"> The color to set the line to </param>
        public void ChangeColor()
        {
            routeLineRenderer.startColor = SetColor();
            routeLineRenderer.endColor = SetColor();
        }

        /// <summary>
        /// Changes the line renderer's line width to a new width
        /// </summary>
        /// <param name="newWidth"> The width to set the line to </param>
        public void ChangeWidth(float newWidth)
        {
            routeLineRenderer.startWidth = newWidth;
            routeLineRenderer.endWidth = newWidth;
        }
    }
}