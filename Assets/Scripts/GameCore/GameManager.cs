using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    [SerializeField] private FruitFactory fruitFactory;
    [SerializeField] private string startSceneName = "Main";
    [SerializeField] private float restartDelay = 1f;
    [SerializeField] private AudioSource backgroundMusic;
    [SerializeField] private GameObject confettiPrefab;
    [SerializeField] private Transform confettiPoint;

    private bool isGameOver = false;
    private SpawnFruit cachedSpawner;
    private PlayerIdleBob cachedIdleBob;

    private void Awake()
    {
        // Single authority required to avoid conflicting game state decisions
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        // Cached to avoid repeated scene-wide searches during pauses and resumes
        if (cachedSpawner == null)
            cachedSpawner = FindObjectOfType<SpawnFruit>();

        if (cachedIdleBob == null)
            cachedIdleBob = FindObjectOfType<PlayerIdleBob>();

        isGameOver = false;

        // Difficulty progression always restarts with a new session
        FallSpeedManager.instance.ResetFallSpeed();
        FallSpeedManager.instance.Resume();

        // Explicit reset keeps spawner state predictable after restarts
        var spawner = FindObjectOfType<SpawnFruit>();
        if (spawner != null)
            spawner.ResetSpawner();

        fruitFactory = FindObjectOfType<FruitFactory>();

        // Decoupled start signal allows UI and systems to react independently
        GameEvents.OnGameStart?.Invoke();

        if (backgroundMusic != null)
        {
            // User preference has priority over game flow
            if (!AudioManager.Instance.IsSoundOn)
            {
                backgroundMusic.Pause();
                return;
            }

            backgroundMusic.Stop();
            backgroundMusic.time = 0f;
            backgroundMusic.Play();
        }
    }

    public void GameOver()
    {
        // Guard prevents double-trigger from overlapping fail conditions
        if (isGameOver)
            return;

        isGameOver = true;

        // Progression frozen to preserve end-of-run difficulty state
        FallSpeedManager.instance.Pause();

        if (GameStateMachine.Instance != null)
            GameStateMachine.Instance.SetState(GameState.GameOver);
    }

    public void RestartGame()
    {
        StartCoroutine(RestartAfterDelay());
    }

    private IEnumerator RestartAfterDelay()
    {
        // Small delay gives audio and visuals time to settle before reload
        yield return new WaitForSeconds(restartDelay);

        SceneManager.LoadScene(startSceneName);

        // One frame wait ensures scene objects are ready before reinitialization
        yield return null;

        StartGame();
    }

    public void ReturnAllFallingObjectsToPool()
    {
        // Interface-based sweep avoids hard dependency on concrete fruit types
        var fruits = FindObjectsOfType<MonoBehaviour>(true)
            .OfType<IFruit>()
            .ToArray();

        foreach (var fruit in fruits)
            fruit.ForceReturnToPool();
    }

    public void PauseMusicTemporarily(float delay)
    {
        if (backgroundMusic != null)
            backgroundMusic.Pause();

        var spawnFruit = FindObjectOfType<SpawnFruit>();
        if (spawnFruit != null)
            spawnFruit.enabled = false;

        var idle = FindObjectOfType<PlayerIdleBob>();
        if (idle != null)
            idle.PauseIdle();

        // Screen cleared to avoid stale objects during temporary interruptions
        ReturnAllFallingObjectsToPool();

        StartCoroutine(ResumeGameAfterDelay(delay));
    }

    private IEnumerator ResumeGameAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Resume only if gameplay state was not changed meanwhile
        if (!IsPlaying())
            yield break;

        if (backgroundMusic != null && AudioManager.Instance.IsSoundOn)
            backgroundMusic.Play();

        var spawnFruit = FindObjectOfType<SpawnFruit>();
        if (spawnFruit != null)
            spawnFruit.enabled = true;

        var idle = FindObjectOfType<PlayerIdleBob>();
        if (idle != null)
            idle.ResumeIdle();
    }

    public bool IsPlaying()
    {
        return GameStateMachine.Instance.CurrentState == GameState.Playing;
    }

    public void PauseGame()
    {
        if (!IsPlaying())
            return;

        // State change centralized to keep pause logic consistent
        GameStateMachine.Instance.SetState(GameState.Paused);

        FallSpeedManager.instance.Pause();

        if (cachedSpawner != null)
            cachedSpawner.enabled = false;

        if (cachedIdleBob != null)
            cachedIdleBob.PauseIdle();

        if (backgroundMusic != null)
            backgroundMusic.Pause();

        // Render disabled instead of pooling to preserve positions
        foreach (var fo in FindObjectsOfType<FallingObject>())
            fo.Hide();
    }

    public void ResumeGame()
    {
        if (GameStateMachine.Instance.CurrentState != GameState.Paused)
            return;

        GameStateMachine.Instance.SetState(GameState.Playing);

        FallSpeedManager.instance.Resume();

        if (cachedSpawner != null)
            cachedSpawner.enabled = true;

        if (cachedIdleBob != null)
            cachedIdleBob.ResumeIdle();

        if (backgroundMusic != null && AudioManager.Instance != null)
        {
            if (AudioManager.Instance.IsSoundOn)
            {
                if (!backgroundMusic.isPlaying)
                    backgroundMusic.Play();
            }
            else
            {
                backgroundMusic.Pause();
            }
        }

        foreach (var fo in FindObjectsOfType<FallingObject>())
            fo.Show();
    }

    public void PlayConfetti()
    {
        // Fire-and-forget effect kept isolated from game state
        if (confettiPoint == null || confettiPrefab == null)
            return;

        var confetti = Instantiate(
            confettiPrefab,
            confettiPoint.position,
            Quaternion.identity);

        Destroy(confetti, 2f);
    }
}



















