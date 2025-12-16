using System.Collections;
using UnityEngine;

public class FlashEffect : MonoBehaviour
{
    [SerializeField] private float duration = 0.12f;
    [SerializeField] private float maxScale = 1.1f;

    private SpriteRenderer sr;

    private void Awake()
    {
        // Cached to avoid repeated component access during animation
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        // Started on enable to support fire-and-forget instantiation
        StartCoroutine(Animate());
    }

    IEnumerator Animate()
    {
        float t = 0f;
        Color startColor = sr.color;

        while (t < duration)
        {
            t += Time.deltaTime;
            float p = t / duration;

            // Manual animation chosen over Animator for short-lived effect
            float scale = Mathf.Lerp(0f, maxScale, p);
            transform.localScale = new Vector3(scale, scale, 1f);

            // Fade-out tied to progress to guarantee full disappearance
            sr.color = new Color(
                startColor.r,
                startColor.g,
                startColor.b,
                1f - p
            );

            yield return null;
        }

        // Destroyed intentionally to avoid pooling overhead for rare effects
        Destroy(gameObject);
    }
}