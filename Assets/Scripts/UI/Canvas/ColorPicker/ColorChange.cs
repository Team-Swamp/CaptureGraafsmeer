using UnityEngine;

using Framework.Enums;

namespace UI.Canvas.ColorPicker
{
    public class ColorChange : MonoBehaviour
    {
        [field: SerializeField] public RouteColors ColorToChange { get; private set; }
    }
}
