using System.Collections.Generic;
using UnityEngine;

using Framework.Enums;
using Framework.SaveLoadSystem;

namespace UI.Canvas.RouteLineSettings
{
    public sealed class ColorPicker : MonoBehaviour
    {
        [SerializeField] private LineRenderer routeLineRenderer;

        private readonly List<Color> _colors = new()
        {
            Color.red,
            Color.green,
            Color.blue,
            Color.yellow,
            Color.magenta,
            new Color(1f, 0.5f, 0f),
            Color.black,
            new Color(0.5f, 0f, 1f),
            new Color(0, 1f, 1)
        };

        private void Awake() => ChangeColor(Saver.Instance.RouteColorIndex);

        private void Start() => gameObject.SetActive(false);

        /// <summary>
        /// Changes the line renderer's line color to a new color
        /// </summary>
        /// <param name="colorChange"> The color to set the line to </param>
        public void ChangeColor(ColorChange colorChange) => SetRouteColor(GetColor(colorChange.colorToChange));

        private void ChangeColor(int targetColor) => GetColor((RouteColors) targetColor);

        private void SetRouteColor(Color targetColor)
        {
            routeLineRenderer.startColor = targetColor;
            routeLineRenderer.endColor = targetColor;
        }

        private Color GetColor(RouteColors lineColor)
        {
            int index = (int)lineColor;

            if (index < 0
                || index > _colors.Count)
                index = 0;

            Saver.Instance.RouteColorIndex = index;
            return _colors[index];
        }
    }
}