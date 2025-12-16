using UnityEngine;
using UnityEngine.SceneManagement;

public class GameBootstrapper : MonoBehaviour
{
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    private void Awake()
    {
        // Forced to keep gameplay timing consistent across different devices
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;

        // Persisted to guarantee a single entry point for the game's lifecycle
        DontDestroyOnLoad(gameObject);

        // Initial scene load kept here to avoid relying on build order
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
