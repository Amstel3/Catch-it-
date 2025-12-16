using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOverController : MonoBehaviour
{
    public void RestartGame()
    {
        StartCoroutine(RestartDelay());
    }

    private IEnumerator RestartDelay()
    {
        // Small delay allows UI feedback to register before scene reload
        yield return new WaitForSeconds(0.2f);

        // State set early to keep global systems in sync during reload
        GameStateMachine.Instance.SetState(GameState.Playing);
        SceneManager.LoadScene("Main");
    }

    public void BackToMenu()
    {
        StartCoroutine(MenuDelay());
    }

    private IEnumerator MenuDelay()
    {
        // Same delay used to keep navigation feel consistent
        yield return new WaitForSeconds(0.2f);

        // Explicit state change avoids relying on scene-specific initialization
        GameStateMachine.Instance.SetState(GameState.MainMenu);
        SceneManager.LoadScene("MainMenu");
    }
}



