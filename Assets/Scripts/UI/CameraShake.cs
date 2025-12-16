using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    [SerializeField] private float duration = 0.08f;
    [SerializeField] private float magnitude = 0.1f;

    private Vector3 originalPos;

    private void Awake()
    {
        // Global access kept for one-shot effects without tight coupling
        Instance = this;
    }

    public void Shake()
    {
        // Fire-and-forget trigger to keep callers free from timing logic
        StartCoroutine(ShakeRoutine());
    }

    private IEnumerator ShakeRoutine()
    {
        // Cached to guarantee exact return even if camera moved earlier
        originalPos = transform.localPosition;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            // Random offset used instead of animation to keep shake unpredictable
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = originalPos + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Explicit reset prevents drift from accumulated offsets
        transform.localPosition = originalPos;
    }
}
