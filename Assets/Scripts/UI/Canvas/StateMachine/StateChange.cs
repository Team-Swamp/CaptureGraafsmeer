using UnityEngine;

using Framework.Enums;

namespace UI.Canvas.StateMachine
{
    public sealed class StateChange : MonoBehaviour
    {
        [field: SerializeField] public UIState States { get; private set; }
    }
}

