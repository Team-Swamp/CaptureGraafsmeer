using UnityEngine;

namespace UI.Canvas.StateMachine
{
    public sealed class UIStateCaller : MonoBehaviour
    {
        [SerializeField] private UIStateManager stateManager;
        
        /// <summary>
        /// Allows a unity event to change the state of the UI
        /// </summary>
        /// <param name="stateChange">The class with an visible enum.</param>
        public void ChangeState(StateChange stateChange)
        {
            stateManager.SwitchState(stateChange.States);
        }
    }
}
