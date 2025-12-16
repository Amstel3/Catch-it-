using UnityEngine;
using TMPro;
using System.Collections;

public class ScoreView : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreValueText;
    [SerializeField] private TMP_Text scoreLabelText;
    [SerializeField] private float punchScale = 1.15f;
    [SerializeField] private float duration = 0.12f;

    private Vector3 originalScale;
    private Coroutine punchRoutine;

    private void Awake()
    {
        // Cached to ensure animation always returns to a known baseline
        originalScale = scoreValueText.transform.localScale;
    }

    public void UpdateScoreText(int score)
    {
        if (scoreValueText == null)
            return;

        scoreValueText.text = score.ToString();

        // Previous animation stopped to keep punch responsive on rapid score changes
        if (punchRoutine != null)
            StopCoroutine(punchRoutine);

        punchRoutine = StartCoroutine(Punch());
    }

    private IEnumerator Punch()
    {
        // Immediate overshoot used to make score changes feel tactile
        scoreValueText.transform.localScale = originalScale * punchScale;

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;

            // Lerp chosen for predictable easing without animation curves
            scoreValueText.transform.localScale = Vector3.Lerp(
                scoreValueText.transform.localScale,
                originalScale,
                t / duration
            );

            yield return null;
        }

        scoreValueText.transform.localScale = originalScale;
    }
}
