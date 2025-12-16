using UnityEngine;

public class SpawnFruit : MonoBehaviour
{
    [SerializeField] private float spawnHeight = 6f;
    [SerializeField] private FruitFactory fruitFactory;
    [SerializeField] private ScoreController scoreController;
    [SerializeField] private float horizontalPadding = 0.3f;
    [SerializeField]
    private AnimationCurve spawnIntervalCurve =
        AnimationCurve.Linear(0f, 1.2f, 60f, 0.5f);

    private Camera cachedCamera;
    private float minX;
    private float maxX;
    private float timer;
    private float elapsedTime = 0f;

    private readonly string[] fruitTypes = { "Apple", "Pear" };

    private void Start()
    {
        // Cached once to avoid repeated Camera.main lookups
        cachedCamera = Camera.main;
        CalculateCameraBounds();

        // Initial interval pulled from curve to keep timing consistent from frame one
        timer = spawnIntervalCurve.Evaluate(0f);
        elapsedTime = 0f;
    }

    private void CalculateCameraBounds()
    {
        if (cachedCamera == null)
        {
            Debug.LogError("Main Camera not found!");
            return;
        }

        // Viewport-based bounds used to stay resolution-independent
        Vector3 left = cachedCamera.ViewportToWorldPoint(new Vector3(0, 0.5f, 0));
        Vector3 right = cachedCamera.ViewportToWorldPoint(new Vector3(1, 0.5f, 0));

        // Padding applied to avoid edge spawns on narrow screens
        minX = left.x + horizontalPadding;
        maxX = right.x - horizontalPadding;
    }

    private void Update()
    {
        // Spawning tied strictly to gameplay state to avoid hidden difficulty drift
        if (!GameManager.instance.IsPlaying())
            return;

        elapsedTime += Time.deltaTime;

        // Curve-driven interval allows pacing changes without code edits
        float dynamicInterval = spawnIntervalCurve.Evaluate(elapsedTime);

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            SpawnRandomFruit();
            timer = dynamicInterval;
        }
    }

    private void SpawnRandomFruit()
    {
        float x = Random.Range(minX, maxX);
        Vector2 spawnPos = new Vector2(x, spawnHeight);

        // Type selection kept simple to avoid coupling with factory internals
        string type = fruitTypes[Random.Range(0, fruitTypes.Length)];
        GameObject fruit = fruitFactory.CreateFruit(type);

        if (fruit != null)
        {
            fruit.transform.position = spawnPos;

            // Injected per spawn to keep pooled objects stateless
            var falling = fruit.GetComponent<FallingObject>();
            if (falling != null)
                falling.SetScoreController(scoreController);

            // Explicit spawn hook ensures pooled objects fully reset
            fruit.GetComponent<IFruit>()?.OnSpawn();
        }
    }

    public void ResetSpawner()
    {
        // Reset required to keep early-game rhythm predictable on restart
        elapsedTime = 0f;
        timer = spawnIntervalCurve.Evaluate(0f);
    }
}





















