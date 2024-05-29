using UnityEngine;

namespace UI.Canvas.StateMachine
{
    public sealed class UIStateCaller : MonoBehaviour
    {
        [SerializeField] private UIStateManager stateManager;

        private UIStateEvent onStateChange;

        /// <summary>
        /// Calls the custom unity event that takes allows enums to be used as parameter.
        /// </summary>
        /// <param name="stateChange">The class with an visible enum.</param>
        public void ChangeState(StateChange stateChange)
        {
            onStateChange?.Invoke(stateChange);
            stateManager.SwitchState(stateChange.states);
        }
    }
}
