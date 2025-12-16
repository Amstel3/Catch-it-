using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        // State set before scene load to keep global systems aligned from frame one
        GameStateMachine.Instance.SetState(GameState.Playing);
        SceneManager.LoadScene("Main");
    }

    public void ExitGame()
    {
        // Explicit quit to avoid relying on platform-specific menu behavior
        Application.Quit();

#if UNITY_EDITOR
        // Editor fallback to mirror build behavior during testing
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}


