using UnityEngine;

namespace DefaultNamespace
{
    public class MainMenuCanvasController : MonoBehaviour
    {
        [SerializeField]
        private RectTransform mainMenu;
        
        [SerializeField]
        private RectTransform optionsMenu;

        private static void Show(Component component)
        {
            component.gameObject.SetActive(true);
        }

        private static void Hide(Component component)
        {
            component.gameObject.SetActive(false);
        }

        public void Start()
        {
            ShowMainMenu();
        }
        
        public void StartGame()
        {
            Scenes.LoadNextScene();
        }

        public void ExitGame()
        {
            Scenes.ExitGame();
        }
        
        public void ShowMainMenu()
        {
            Show(mainMenu);
            Hide(optionsMenu);
        }
        
        public void ShowOptionsMenu()
        {
            Show(optionsMenu);
            Hide(mainMenu);
        }
    }
}