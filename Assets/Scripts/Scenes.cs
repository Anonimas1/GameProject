using UnityEngine;
using UnityEngine.SceneManagement;

public static class Scenes
{
    public static void RestartScene()
    {
        LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public static void LoadNextScene()
    {
        LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public static void LoadPreviousScene()
    {
        LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    private static void LoadScene(int buildindex)
    {
        SceneManager.LoadScene(buildindex);
    }
    
    public static void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}