using UI.Canvas.PhotoTaking;
using UnityEngine;

namespace UI.Canvas.PhotoBookSystem
{
    public sealed class PhotoBook : MonoBehaviour
    {
        [SerializeField] private Page[] pages;
        [SerializeField] private GameObject nextButton;
        [SerializeField] private GameObject previousButton;

        [SerializeField] private PhotoTaker photoTaker;

        private int _currentPage;

        private void Awake() => gameObject.SetActive(false);

        private void OnEnable() => SetupPhotoBook();

        /// <summary>
        /// Opens the next page with animation
        /// </summary>
        public void OpenNextPage()
        {
            if(_currentPage == pages.Length - 2)
                nextButton.SetActive(false);
            
            if(!previousButton.activeSelf)
                previousButton.SetActive(true);
            
            pages[_currentPage].AnimatePage(true);
            _currentPage++;
            pages[_currentPage].ForceOpen();
        }

        /// <summary>
        /// Close the current page with animation
        /// </summary>
        public void CloseCurrentPage()
        {
            if (_currentPage - 2 == -1)
                previousButton.SetActive(false);
            
            if(!nextButton.activeSelf)
                nextButton.SetActive(true);
            
            _currentPage--;
            pages[_currentPage].AnimatePage(false);
        }

        /// <summary>
        /// Finds the correct page and set its properties
        /// </summary>
        public void SetCurrentPageProperties()
        {
            var photoTakerInteractable = photoTaker.CurrentPhotoInteractable;
            
            foreach (var page in pages)
            {
                if(page.GetPhotoInteractable == photoTakerInteractable)
                    page.GetPhotoInteractable.ParentPage.SetProperties();
            }
        }

        private void SetupPhotoBook()
        {
            CheckButtonsUsability();
            ViewPages();
        }

        private void CheckButtonsUsability()
        {
            if (_currentPage == 0)
                previousButton.SetActive(false);
            
            if(_currentPage == pages.Length)
                nextButton.SetActive(false);
        }

        private void ViewPages()
        {
            foreach (var page in pages)
            {
                page.ForceClose();
            }
            
            pages[_currentPage].ForceOpen();
        }
    }
}