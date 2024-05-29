using UnityEngine;

namespace UI.Canvas.StateMachine
{

    public class StateChange : MonoBehaviour
    {
        public UIStates States;
    }
    public enum UIStates
    {
        MAIN_BUTTON,
        INTRODUCTION,
        PHOTOBOOK,
        CAMERA_PANEL,
        TAKING_PICTURE
    }
}
