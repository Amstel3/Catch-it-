using UnityEngine;

public class PauseButton : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;

    private void Start()
    {
        // Hidden explicitly to avoid inheriting state from previous scene loads
        if (pausePanel != null)
            pausePanel.SetActive(false);
    }

    public void OnPauseButtonClicked()
    {
        if (GameStateMachine.Instance == null)
            return;

        var state = GameStateMachine.Instance.CurrentState;

        if (state == GameState.Playing)
        {
            // Pause delegated to GameManager to keep UI free from gameplay rules
            GameManager.instance.PauseGame();

            if (pausePanel != null)
                pausePanel.SetActive(true);
        }
        else if (state == GameState.Paused)
        {
            if (pausePanel != null)
                pausePanel.SetActive(false);

            // Resume routed through GameManager to restore full game state
            GameManager.instance.ResumeGame();
        }
    }
}
