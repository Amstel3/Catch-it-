using UnityEngine;

public abstract class FallingObject : MonoBehaviour, IFruit
{
    [SerializeField] protected float baseFallSpeed = 1f;
    [SerializeField] private float speedIncreaseRate = 0.1f;
    [SerializeField] private float maxFallSpeed = 10f;

    protected ScoreController scoreController;

    private ObjectPool pool;
    private float initialFallSpeed;
    private bool isReturned = false;
    private SpriteRenderer sr;

    // Injected to keep scoring logic outside of the object itself
    public void SetScoreController(ScoreController controller)
    {
        scoreController = controller;
    }

    protected virtual void Awake()
    {
        // Cached to guarantee clean resets when reused from pool
        initialFallSpeed = baseFallSpeed;

        // Cached to avoid repeated GetComponent calls during gameplay
        sr = GetComponent<SpriteRenderer>();
    }

    protected virtual void Update()
    {
        // Movement is paused instead of disabling objects to preserve pool state
        if (!GameManager.instance.IsPlaying())
            return;

        float fallSpeed = baseFallSpeed * FallSpeedManager.instance.GetFallSpeedMultiplier();
        fallSpeed *= GetSpeedMultiplier();
        fallSpeed = Mathf.Min(fallSpeed, maxFallSpeed);

        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);
        IncreaseFallSpeedOverTime();

        // Extra guard to prevent double-return when crossing bounds in one frame
        if (transform.position.y < -6f && !isReturned)
        {
            isReturned = true;
            OnGroundHit();
            ReturnToPool();
        }
    }

    // Gradual acceleration used instead of curves to keep tuning simple
    protected void IncreaseFallSpeedOverTime()
    {
        baseFallSpeed += speedIncreaseRate * Time.deltaTime;
    }

    protected void ReturnToPool()
    {
        if (pool != null)
        {
            // Reset here to avoid relying on pool implementation details
            isReturned = false;
            pool.ReturnObject(gameObject);
        }
        else
        {
            Debug.LogError("ObjectPool is null! Make sure the FallingObject is a child of an ObjectPool.");
        }
    }

    public virtual void OnSpawn()
    {
        // Full reset required because pooled objects may have arbitrary history
        isReturned = false;
        baseFallSpeed = initialFallSpeed;
    }

    // Assigned explicitly to avoid scene-wide searches
    public void SetPool(ObjectPool assignedPool)
    {
        pool = assignedPool;
    }

    // Overridable hook instead of branching logic per fruit type
    protected virtual float GetSpeedMultiplier()
    {
        return 1f;
    }

    public abstract void OnGroundHit();
    public abstract void OnPlayerCollision();

    public void ForceReturnToPool()
    {
        // Used for hard resets where gameplay rules no longer apply
        baseFallSpeed = initialFallSpeed;
        transform.position = new Vector3(0, 10, 0);

        if (pool != null)
            pool.ReturnObject(gameObject);
    }

    public void Hide()
    {
        // Renderer toggled instead of GameObject to keep Update running if needed
        if (sr != null)
            sr.enabled = false;
    }

    public void Show()
    {
        if (sr != null)
            sr.enabled = true;
    }
}












