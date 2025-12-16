using UnityEngine;

public class GameStateMachine : MonoBehaviour
{
    public static GameStateMachine Instance { get; private set; }
    public GameState CurrentState { get; private set; } = GameState.MainMenu;
    public event System.Action<GameState> OnStateChanged;

    private void Awake()
    {
        // Single instance required to keep global state transitions deterministic
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Persisted to allow state continuity across scene reloads
        DontDestroyOnLoad(gameObject);
    }

    public void SetState(GameState newState)
    {
        // Guard prevents redundant transitions and unnecessary notifications
        if (newState == CurrentState)
            return;

        CurrentState = newState;

        // Event-based propagation keeps systems loosely coupled
        OnStateChanged?.Invoke(CurrentState);
    }
}