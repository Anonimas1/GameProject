using StarterAssets;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class GameMenuCanvasController : MonoBehaviour
    {
        [SerializeField]
        private RectTransform mainMenu;
        
        [SerializeField]
        private RectTransform pauseMenu;
        
        [SerializeField]
        private RectTransform optionsMenu;

        [SerializeField]
        private RectTransform gameOverMenu;

        public Slider musicVolumeSlider;
        
        public Slider effectsVolumeSlider;

        public MixerController mixerController;

        public Damageable damageableToTrack;

        private bool isPaused;

        private void Start()
        {
            //masterVolumeSlider.SetValueWithoutNotify(mixerController.MasterVolume);
            musicVolumeSlider.SetValueWithoutNotify(mixerController.MusicVolume);
            effectsVolumeSlider.SetValueWithoutNotify(mixerController.EffectsVolume);

            if (damageableToTrack != null)
            {
                damageableToTrack.DamageableHealthBelowZeroHandler += (_, _) => ShowGameOverScreen();   
            }
        }

        private void ShowGameOverScreen()
        {
            PauseGame();
            Show(gameOverMenu);
        }

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
            UnpauseGame();
        }

        public void RestartGame()
        {
            Scenes.RestartScene();
            UnpauseGame();
        }

        public void ExitGame()
        {
            Scenes.ExitGame();
        }
        
        public void ShowPauseMenu()
        {
            if (!isPaused)
            {
                PauseGame();
                Show(pauseMenu);
                Hide(optionsMenu);
            }
            else
            {
                UnpauseGame();
                HidePauseMenu();
            }
        }
        
        public void ShowOptionsMenu()
        {
            Show(optionsMenu);
            Hide(pauseMenu);
            Hide(gameOverMenu);
            Hide(mainMenu);
        }

        private void PauseGame()
        {
            Time.timeScale = 0;
            damageableToTrack.gameObject.GetComponent<ThirdPersonController>().enabled = false;
            isPaused = true;
        }

        private void UnpauseGame()
        {
            Time.timeScale = 1;
            isPaused = false;
            damageableToTrack.gameObject.GetComponent<ThirdPersonController>().enabled = true;
        }

        public void StartGame()
        {
            Scenes.LoadNextScene();
        }

        public void ShowMainMenu()
        {
            Show(mainMenu);
            Hide(optionsMenu);
        }
    }
}