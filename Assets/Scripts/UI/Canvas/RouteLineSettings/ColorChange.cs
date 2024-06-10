using UnityEngine;

using Framework.Enums;

namespace UI.Canvas.RouteLineSettings
{
    public sealed class ColorChange : MonoBehaviour
    {
        [field: SerializeField] public RouteColors colorToChange { get; private set; }
    }
}
