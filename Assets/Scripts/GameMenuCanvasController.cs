using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class GameMenuCanvasController : MonoBehaviour
    {
        [SerializeField]
        private RectTransform pauseMenu;
        
        [SerializeField]
        private RectTransform optionsMenu;

        [SerializeField]
        private RectTransform gameOverMenu;

        private bool isPaused;
        private static void Show(Component component)
        {
            component.gameObject.SetActive(true);
        }

        private static void Hide(Component component)
        {
            component.gameObject.SetActive(false);
        }

        public void HidePauseMenu()
        {
            Hide(pauseMenu);
            Hide(optionsMenu);
            Time.timeScale = 1;
            isPaused = false;
        }

        public void RestartGame()
        {
            Scenes.RestartScene();
        }

        public void ExitGame()
        {
            Scenes.ExitGame();
        }
        
        public void ShowPauseMenu()
        {
            if (!isPaused)
            {
                Time.timeScale = 0;
                Show(pauseMenu);
                Hide(optionsMenu);
                isPaused = true;
            }
            else
            {
                Time.timeScale = 1;
                HidePauseMenu();
                isPaused = false;
            }
        }
        
        public void ShowOptionsMenu()
        {
            Show(optionsMenu);
            Hide(pauseMenu);
            Hide(gameOverMenu);
        }
    }
}