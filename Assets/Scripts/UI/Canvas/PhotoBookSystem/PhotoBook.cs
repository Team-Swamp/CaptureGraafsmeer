﻿using UnityEngine;

using Framework.PhoneCamera;
using Framework.SaveLoadSystem;
using UI.Canvas.PhotoTaking;

namespace UI.Canvas.PhotoBookSystem
{
    public sealed class PhotoBook : PageHolder
    {
        [SerializeField] private PhotoTaker photoTaker;
        [SerializeField, Tooltip("This list should be the page child objects, but in reverse order because they are UI elements.")]
        private Page[] pages;

        private void Awake()
        {
            int l = pages.Length;
            
            for (int i = 0; i < l; i++)
            {
                if (Saver.Instance.PhotoAmountMade >= i)
                    pages[i].GetPhotoInteractable.IsVisited = true;
            }
            
            gameObject.SetActive(false);
        }

        private void OnEnable() => SetupPhotoBook();

        /// <summary>
        /// Opens the next page with animation
        /// </summary>
        public void OpenNextPage()
        {
            SetNextItem(pages.Length);
            
            pages[p_currentIndex].AnimatePage(true);
            p_currentIndex++;
            pages[p_currentIndex].ForceOpen();
        }

        /// <summary>
        /// Close the current page with animation
        /// </summary>
        public void CloseCurrentPage()
        {
            SetPreviousItem();
            pages[p_currentIndex].AnimatePage(false);
        }

        /// <summary>
        /// Finds the correct page and set its properties
        /// </summary>
        public void SetCurrentPageProperties() => SetCurrentItem(null);

        /// <summary>
        /// Set the current item to display.
        /// </summary>
        /// <param name="isIncreasing">Not used here, should be NULL</param>
        protected override void SetCurrentItem(bool? isIncreasing)
        {
            PhotoInteractable photoTakerInteractable = photoTaker.CurrentPhotoInteractable;
            
            foreach (var page in pages)
            {
                if(page.GetPhotoInteractable == photoTakerInteractable)
                    page.GetPhotoInteractable.ParentPage.SetProperties();
            }
        }
        
        private void SetupPhotoBook()
        {
            CheckButtonsUsability(pages.Length);
            ViewPages();
        }

        private void ViewPages()
        {
            foreach (var page in pages)
            {
                Debug.Log(page.name);
                page.GetComponent<RectTransform>().localScale = new Vector3(0, 1, 1);
                // page.ForceClose();
            }
            
            pages[p_currentIndex].ForceOpen();
        }
    }
}