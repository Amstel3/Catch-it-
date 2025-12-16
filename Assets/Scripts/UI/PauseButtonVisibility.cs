using UnityEngine;

public class PauseButtonVisibility : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        // Cached once to avoid repeated component lookups during state changes
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        // Subscribed on enable to stay in sync with scene and UI lifecycle
        GameStateMachine.Instance.OnStateChanged += OnStateChanged;

        // Immediate sync prevents incorrect visibility after scene load
        OnStateChanged(GameStateMachine.Instance.CurrentState);
    }

    private void OnDisable()
    {
        // Defensive unsubscribe to avoid dangling listeners
        if (GameStateMachine.Instance != null)
            GameStateMachine.Instance.OnStateChanged -= OnStateChanged;
    }

    private void OnStateChanged(GameState state)
    {
        // CanvasGroup used instead of SetActive to preserve layout and animations
        bool visible = state == GameState.Playing;

        canvasGroup.alpha = visible ? 1f : 0f;
        canvasGroup.interactable = visible;
        canvasGroup.blocksRaycasts = visible;
    }
}