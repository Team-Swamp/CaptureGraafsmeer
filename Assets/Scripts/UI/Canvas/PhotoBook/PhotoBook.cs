using UnityEngine;

using Framework.PhoneCamera;

namespace UI.Canvas.PhoneBook
{
    public sealed class PhotoBook : PageHolder
    {
        [SerializeField] private PhotoTaker photoTaker;
        [SerializeField] private Page[] pages;

        private void Awake() => gameObject.SetActive(false);

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
        /// 
        /// </summary>
        /// <param name="isIncreasing"></param>
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
                page.ForceClose();
            }
            
            pages[p_currentIndex].ForceOpen();
        }
    }
}