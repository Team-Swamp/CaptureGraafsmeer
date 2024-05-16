using UnityEngine;

using Framework.SaveLoadSystem;

namespace UI.Canvas
{
    public class ResetProgressButton : MonoBehaviour
    {
        private const float MAKE_SURE_DATA_IS_SAVED_DELAY = 0.1f;
        
        /// <summary>
        /// This will reset the progress of the route and photos made. After that it will quit the game.
        /// </summary>
        public void ResetProgress()
        {
            Saver.Instance.ResetData();
            Invoke(nameof(DelayedQuit), MAKE_SURE_DATA_IS_SAVED_DELAY);
        }
        
        private void DelayedQuit() => Application.Quit();
    }
}
