using System;
using UI.Canvas.Introduction;
using UI.Canvas.PhotoBookSystem;
using UnityEngine;

namespace UI.Canvas.StateMachine
{
    public class UIStateManager : MonoBehaviour
    {
        private IntroductionPopup _introductionPopup;
        private PhotoBook _photoBook;
        private void Awake()
        {
            _introductionPopup = FindObjectOfType<IntroductionPopup>();
            _photoBook = FindObjectOfType<PhotoBook>();
        }

        public void OnStateChange(UIStates newState)
        {
            switch (newState)
            {
                case UIStates.MAIN:
                    break;
                case UIStates.INTRODUCTION:
                    _introductionPopup.Open();
                    break;
                case UIStates.PHOTOBOOK:
                    _photoBook.gameObject.SetActive(_photoBook.gameObject);
                    break;
                case UIStates.CAMERA_PANEL:
                    break;
                case UIStates.TAKING_PICTURE:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
            }
        }
    }
}
