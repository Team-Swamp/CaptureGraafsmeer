using UnityEngine;

namespace UI.Canvas.PhoneBook
{
    public sealed class PhotoBook : MonoBehaviour
    {
        [SerializeField] private Page[] pages;
        [SerializeField] private GameObject nextButton;
        [SerializeField] private GameObject previousButton;

        private int _currentPage;

        private void Awake() => gameObject.SetActive(false);

        private void OnEnable() => SetupPhotoBook();

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

        public void CloseCurrentPage()
        {
            if (_currentPage - 2 == -1)
                previousButton.SetActive(false);
            
            if(!nextButton.activeSelf)
                nextButton.SetActive(true);
            
            _currentPage--;
            pages[_currentPage].AnimatePage(false);
        }

        public void SetCurrentPageProperties()
        {
            print($"The page of {pages[_currentPage].yes()} has been updated.");
            pages[_currentPage].SetProperties();
        }

        private void SetupPhotoBook()
        {
            CheckButtonsUsability();
            ViewPages();
        }

        private void CheckButtonsUsability()
        {
            if (_currentPage== 0)
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