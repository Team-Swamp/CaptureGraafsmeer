using Framework.Enums;
using UnityEngine;

namespace UI.Canvas.ColorPicker
{
    public sealed class ColorChange : MonoBehaviour
    {
        [field: SerializeField] public RouteColors States { get; private set; }
    }
}
