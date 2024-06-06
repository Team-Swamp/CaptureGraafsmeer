using System;
using UnityEngine;

using Framework.Enums;
using UI.Canvas.Introduction;
using UI.Canvas.PhotoBookSystem;

namespace UI.Canvas.StateMachine
{
    public sealed class UIStateManager : MonoBehaviour
    {
        [SerializeField] private UIState currentState;
        
        [Header("References")]
        [SerializeField] private IntroductionPopup introductionPopup;
        [SerializeField] private PhotoBook photoBook;
        [SerializeField] private GameObject cameraPanel;
        [SerializeField] private GameObject takingPicture;
        [SerializeField] private GameObject colorPicker;

        /// <summary>
        /// This switch case checks whether or not you're in the correct state,
        /// and whether or not you're allowed to move from a certain state to another one.
        /// </summary>
        /// <param name="targetState">This is the state that is passed in.</param>
        /// <exception cref="ArgumentOutOfRangeException">This error will be called when the state is unusable</exception>
        public void SwitchState(UIState targetState)
        {
            switch (targetState)
            {
                case UIState.MAIN:
                    cameraPanel.SetActive(false);
                        takingPicture.SetActive(false);
                    break;
                case UIState.INTRODUCTION 
                    when currentState == UIState.MAIN:
                        introductionPopup.Open();
                    break;
                case UIState.PHOTOBOOK 
                    when currentState == UIState.MAIN:
                        photoBook.gameObject.SetActive(photoBook.gameObject);
                    break;
                case UIState.CAMERA_PANEL 
                    when currentState == UIState.MAIN:
                        cameraPanel.SetActive(true);
                        takingPicture.SetActive(false);
                    break;
                case UIState.TAKING_PICTURE 
                    when currentState == UIState.CAMERA_PANEL:
                        takingPicture.gameObject.SetActive(true);
                        cameraPanel.SetActive(false);
                    break;
                case UIState.COLOR_PICKER
                    when currentState == UIState.MAIN:
                        colorPicker.gameObject.SetActive(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(targetState), targetState, null);
            }
            
            currentState = targetState;
        }
    }
}
