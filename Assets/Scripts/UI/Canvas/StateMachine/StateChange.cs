using UnityEngine;

namespace UI.Canvas.StateMachine
{
    public sealed class StateChange : MonoBehaviour
    {
        [field: SerializeField] public UIState states { get; private set; }
    }
}

