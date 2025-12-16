
using UnityEngine;
using System.Collections;

public class TitlePulse : MonoBehaviour
{
    [Header("Pulse")]
    [SerializeField] private float scaleAmplitude = 0.03f;
    [SerializeField] private float pulseSpeed = 1.5f;

    [Header("Intro")]
    [SerializeField] private float introDuration = 0.25f;
    [SerializeField] private float introStartScale = 0.9f;

    private Vector3 startScale;
    private float time;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        // Cached once to keep all animations relative to a stable baseline
        startScale = transform.localScale;

        // CanvasGroup used to fade without affecting layout or raycasts
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        // Initial state prepared before any frame is rendered
        canvasGroup.alpha = 0f;
        transform.localScale = startScale * introStartScale;
    }

    private void Start()
    {
        // Intro separated from pulse to avoid overlapping responsibilities
        StartCoroutine(Intro());
    }

    private IEnumerator Intro()
    {
        float t = 0f;

        while (t < introDuration)
        {
            t += Time.deltaTime;
            float p = t / introDuration;

            // Linear interpolation chosen to keep intro subtle and readable
            float scale = Mathf.Lerp(introStartScale, 1f, p);
            transform.localScale = startScale * scale;

            canvasGroup.alpha = p;

            yield return null;
        }

        transform.localScale = startScale;
        canvasGroup.alpha = 1f;
    }

    private void Update()
    {
        // Continuous pulse kept lightweight to avoid Animator overhead
        time += Time.deltaTime;

        float pulse = 1f + Mathf.Sin(time * pulseSpeed) * scaleAmplitude;
        transform.localScale = startScale * pulse;
    }
}