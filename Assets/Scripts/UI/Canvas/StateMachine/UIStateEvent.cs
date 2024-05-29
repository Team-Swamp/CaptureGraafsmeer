using UnityEngine.Events;

namespace UI.Canvas.StateMachine
{
    [System.Serializable]
    public sealed class UIStateEvent : UnityEvent<StateChange> { }
}
