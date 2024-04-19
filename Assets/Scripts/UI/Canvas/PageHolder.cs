using UnityEngine;

namespace UI.Canvas
{
    public abstract class PageHolder : MonoBehaviour
    {
        [SerializeField] protected GameObject nextButton;
        [SerializeField] protected GameObject previousButton;
        [SerializeField] protected GameObject closeButton;
        
        protected int p_currentIndex;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="length"></param>
        protected void SetNextItem(int length)
        {
            if(length == p_currentIndex)
                return;

            if(p_currentIndex == length - 2)
                nextButton.SetActive(false);
            
            if(!previousButton.activeSelf)
                previousButton.SetActive(true);
        }

        /// <summary>
        /// 
        /// </summary>
        protected void SetPreviousItem()
        {
            if(p_currentIndex == 0)
                return;

            if (p_currentIndex - 2 == -1)
                previousButton.SetActive(false);
            
            if(!nextButton.activeSelf)
                nextButton.SetActive(true);
            
            p_currentIndex--;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="length"></param>
        protected void CheckButtonsUsability(int length)
        {
            if (p_currentIndex == 0)
                previousButton.SetActive(false);
        
            if(p_currentIndex == length)
                nextButton.SetActive(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isIncreasing"></param>
        protected abstract void SetCurrentItem(bool ?isIncreasing);
    }
}