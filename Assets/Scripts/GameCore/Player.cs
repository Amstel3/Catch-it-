using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject flashPrefab;
    [SerializeField] private AudioSource hitSound;
    [SerializeField] private AudioSource backgroundMusic;

    private PlayerHitSquash cachedSquash;
    private ScoreController scoreController;

    private void Awake()
    {
        // Cached once to avoid component lookups during collisions
        cachedSquash = GetComponent<PlayerHitSquash>();
    }

    private void Start()
    {
        // Scene lookup kept here to decouple player from score initialization order
        scoreController = FindObjectOfType<ScoreController>();

        // Registered explicitly to respect global sound settings
        AudioManager.Instance.RegisterSFX(hitSound);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ignored outside gameplay to prevent side effects during transitions
        if (!GameManager.instance.IsPlaying())
            return;

        // Interface check keeps player logic independent of fruit implementations
        if (collision.gameObject.TryGetComponent<IFruit>(out IFruit fruit))
        {
            hitSound.Play();

            CameraShake.Instance?.Shake();

            // Visual feedback played before any state changes to ensure it is seen
            cachedSquash?.PlaySquash();

            SpawnFlash(collision.transform.position);

            // Temporary pause used to emphasize impact without ending the run immediately
            GameManager.instance.PauseMusicTemporarily(2f);

            // Fruit resolved before life loss to keep rules deterministic
            fruit.OnPlayerCollision();
            LifeManager.instance.LoseLife();

            // Screen cleared to avoid lingering objects during impact pause
            scoreController.DestroyAllFallingObjects();
        }
    }

    private void SpawnFlash(Vector3 pos)
    {
        // Fire-and-forget effect intentionally not pooled due to low frequency
        if (flashPrefab != null)
            Instantiate(flashPrefab, pos, Quaternion.identity);
    }
}





