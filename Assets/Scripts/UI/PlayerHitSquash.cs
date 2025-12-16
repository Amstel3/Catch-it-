using UnityEngine;
using System.Collections;

public class PlayerHitSquash : MonoBehaviour
{
    [SerializeField] private float squashFactor = 0.75f;
    [SerializeField] private float stretchFactor = 1.25f;
    [SerializeField] private float duration = 0.18f;

    private Vector3 originalScale;
    private Coroutine squashRoutine;

    private void Start()
    {
        // Cached once to guarantee consistent return scale after repeated hits
        originalScale = transform.localScale;
    }

    public void PlaySquash()
    {
        // Previous animation stopped to keep impact response snappy
        if (squashRoutine != null)
            StopCoroutine(squashRoutine);

        squashRoutine = StartCoroutine(SquashEffect());
    }

    private IEnumerator SquashEffect()
    {
        // Instant deformation used to sell impact before any game state changes
        transform.localScale = new Vector3(
            originalScale.x * squashFactor,
            originalScale.y * stretchFactor,
            originalScale.z
        );

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;

            // Smooth return preferred over spring to keep motion readable
            transform.localScale = Vector3.Lerp(
                transform.localScale,
                originalScale,
                t / duration
            );

            yield return null;
        }

        // Explicit reset prevents scale drift on chained hits
        transform.localScale = originalScale;
    }
}