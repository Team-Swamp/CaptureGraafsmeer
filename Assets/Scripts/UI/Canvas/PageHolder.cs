using UnityEngine;

namespace UI.Canvas
{
    public abstract class PageHolder : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] protected GameObject p_nextButton;
        [SerializeField] protected GameObject p_previousButton;
        [SerializeField] protected GameObject p_closeButton;
        
        protected int p_currentIndex;
        
        /// <summary>
        /// Set the next item to display, so also check the buttons visibility
        /// </summary>
        /// <param name="length">The length of the list that is getting checked for the next button.</param>
        protected void SetNextItem(int length)
        {
            if(length == p_currentIndex)
                return;

            if(p_currentIndex == length - 2)
                p_nextButton.SetActive(false);
            
            if(!p_previousButton.activeSelf)
                p_previousButton.SetActive(true);
        }

        /// <summary>
        /// Set the previous item to display, so also check the buttons visibility. Plus decreases p_currentIndex.
        /// </summary>
        protected void SetPreviousItem()
        {
            if(p_currentIndex == 0)
                return;

            if (p_currentIndex - 2 == -1)
                p_previousButton.SetActive(false);
            
            if(!p_nextButton.activeSelf)
                p_nextButton.SetActive(true);
            
            p_currentIndex--;
        }

        /// <summary>
        /// Check if the buttons should be visibility or not
        /// </summary>
        /// <param name="length">The length of the list that is getting checked for the next button.</param>
        protected void CheckButtonsUsability(int length)
        {
            if (p_currentIndex == 0)
                p_previousButton.SetActive(false);
        
            if(p_currentIndex == length)
                p_nextButton.SetActive(false);
        }

        /// <summary>
        /// Set the current item to display.
        /// </summary>
        /// <param name="isIncreasing">Is the current item the next or previous?</param>
        protected abstract void SetCurrentItem(bool ?isIncreasing);
    }
}