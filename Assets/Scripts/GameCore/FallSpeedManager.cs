using UnityEngine;

public class FallSpeedManager : MonoBehaviour
{
    public static FallSpeedManager instance { get; private set; }

    [Header("Speed settings")]
    [SerializeField] private float fallSpeedMultiplier = 1f;
    [SerializeField] private float maxMultiplier = 5f;
    [SerializeField]
    private AnimationCurve fallSpeedCurve =
        AnimationCurve.Linear(0f, 1f, 60f, 3f);

    private bool isActive = true;
    private float elapsedTime = 0f;

    private void Awake()
    {
        // Global access used to keep difficulty progression consistent across scenes
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // Progression paused instead of reset to preserve pacing on temporary stops
        if (!isActive)
            return;

        // Synced with gameplay state to avoid difficulty drift in menus or pauses
        if (!GameManager.instance.IsPlaying())
            return;

        elapsedTime += Time.deltaTime;

        // Curve used to allow non-linear difficulty tuning without code changes
        float value = fallSpeedCurve.Evaluate(elapsedTime);

        // Hard cap prevents unplayable speeds during long sessions
        fallSpeedMultiplier = Mathf.Clamp(value, 1f, maxMultiplier);
    }

    public void ResetFallSpeed()
    {
        // Explicit reset used for restarts to ensure predictable early-game feel
        elapsedTime = 0f;
        fallSpeedMultiplier = 1f;
    }

    public void Pause()
    {
        // Separate flag allows pausing progression without touching game state
        isActive = false;
    }

    public void Resume()
    {
        isActive = true;
    }

    public float GetFallSpeedMultiplier()
    {
        return fallSpeedMultiplier;
    }
}







