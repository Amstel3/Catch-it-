using UnityEngine;

public class GameStateListener : MonoBehaviour
{
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject scoreRoot;

    private void OnEnable()
    {
        // Subscribed on enable to stay in sync with dynamic scene lifetimes
        GameStateMachine.Instance.OnStateChanged += HandleStateChanged;

        // Immediate sync prevents incorrect UI state on scene load
        HandleStateChanged(GameStateMachine.Instance.CurrentState);
    }

    private void OnDisable()
    {
        // Unsubscribed defensively to avoid dangling listeners on scene unload
        if (GameStateMachine.Instance != null)
            GameStateMachine.Instance.OnStateChanged -= HandleStateChanged;
    }

    private void HandleStateChanged(GameState state)
    {
        if (gameOverUI != null)
            gameOverUI.SetActive(state == GameState.GameOver);

        if (scoreRoot != null)
            scoreRoot.SetActive(state == GameState.Playing);
    }
}