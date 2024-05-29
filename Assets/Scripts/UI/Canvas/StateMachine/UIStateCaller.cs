using UnityEngine;

namespace UI.Canvas.StateMachine
{
    public class UIStateCaller : MonoBehaviour
    {
        private UIStateEvent onStateChange;
        [SerializeField] private UIStateManager stateManager;

        private void Start() => stateManager = GetComponent<UIStateManager>();

        public void ChangeState(StateChange stateChange)
        {
            onStateChange?.Invoke(stateChange);
            stateManager.OnStateChange(stateChange.states);
        }
    }
}
