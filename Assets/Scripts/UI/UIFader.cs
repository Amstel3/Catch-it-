using System.Collections;
using UnityEngine;

public class UIFader : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 0.3f;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        // CanvasGroup used to fade UI without breaking layout or raycasts
        canvasGroup = GetComponent<CanvasGroup>();

        // Initial alpha forced to avoid one-frame flashes on enable
        canvasGroup.alpha = 0f;
    }

    private void OnEnable()
    {
        // Triggered on enable to support reuse across different UI states
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = time / fadeDuration;
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }
}
