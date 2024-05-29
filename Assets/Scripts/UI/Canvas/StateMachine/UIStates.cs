using UnityEngine;

namespace UI.Canvas.StateMachine
{
    public class StateChange : MonoBehaviour
    {
        public UIStates states;
    }
    public enum UIStates
    {
        MAIN,
        INTRODUCTION,
        PHOTOBOOK,
        CAMERA_PANEL,
        TAKING_PICTURE
    }
}
