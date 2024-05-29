using System;
using Framework.PhoneCamera;
using UnityEngine;

using UI.Canvas.Introduction;
using UI.Canvas.PhotoBookSystem;
using UI.Canvas.PhotoTaking;

namespace UI.Canvas.StateMachine
{
    public sealed class UIStateManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private IntroductionPopup introductionPopup;
        [SerializeField] private PhotoBook photoBook;
        [SerializeField] private GameObject cameraPanel;
        [SerializeField] private GameObject takingPicture;
        
        [Header("Current UI State")]
        [SerializeField] private UIState currentState;

        /// <summary>
        /// This switch case checks whether or not you're in the correct state,
        /// and whether or not you're allowed to move from a certain state to another one.
        /// </summary>
        /// <param name="newState">This is the state that is passed in.</param>
        /// <exception cref="ArgumentOutOfRangeException">This error will be called when the state is unable</exception>
        public void SwitchState(UIState newState)
        {
            switch (newState)
            {
                case UIState.MAIN:
                    cameraPanel.SetActive(false);
                    takingPicture.SetActive(false);
                    break;
                case UIState.INTRODUCTION when currentState == UIState.MAIN:
                    introductionPopup.Open();
                    break;
                case UIState.PHOTOBOOK when currentState == UIState.MAIN:
                    photoBook.gameObject.SetActive(photoBook.gameObject);
                    break;
                case UIState.CAMERA_PANEL when currentState == UIState.MAIN:
                    cameraPanel.SetActive(true);
                    /*takingPicture.SetActive(false);*/
                    break;
                case UIState.TAKING_PICTURE when currentState == UIState.CAMERA_PANEL:
                    takingPicture.gameObject.SetActive(true);
                    cameraPanel.SetActive(false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
            }
            currentState = newState;
        }
    }
}
