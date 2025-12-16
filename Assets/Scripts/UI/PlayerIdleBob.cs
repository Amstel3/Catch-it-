using UnityEngine;

public class PlayerIdleBob : MonoBehaviour
{
    [SerializeField] private float amplitude = 0.08f;
    [SerializeField] private float frequency = 2f;

    private float time;

    // World
    private bool isWorld;
    private float startY;

    // UI
    private RectTransform rect;
    private Vector2 startAnchoredPos;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();

        if (rect != null)
        {
            // UI mode detected to reuse the same logic without duplicating scripts
            isWorld = false;
            startAnchoredPos = rect.anchoredPosition;
        }
        else
        {
            // World mode fallback keeps behaviour identical for in-game objects
            isWorld = true;
            startY = transform.position.y;
        }
    }

    private void Update()
    {
        // Idle motion paused with gameplay to avoid visual noise in menus and pauses
        if (GameManager.instance != null && !GameManager.instance.IsPlaying())
            return;

        time += Time.deltaTime;
        float offset = Mathf.Sin(time * frequency) * amplitude;

        if (isWorld)
        {
            Vector3 pos = transform.position;
            pos.y = startY + offset;
            transform.position = pos;
        }
        else
        {
            // UI uses amplified offset to compensate for canvas scaling
            Vector2 pos = startAnchoredPos;
            pos.y += offset * 100f;
            rect.anchoredPosition = pos;
        }
    }

    public void ResetIdle()
    {
        // Explicit reset keeps animation deterministic after interruptions
        time = 0f;

        if (isWorld)
        {
            Vector3 pos = transform.position;
            pos.y = startY;
            transform.position = pos;
        }
        else
        {
            rect.anchoredPosition = startAnchoredPos;
        }
    }

    public void PauseIdle()
    {
        // Component disabled instead of flag to fully stop Update execution
        enabled = false;
    }

    public void ResumeIdle()
    {
        // Time reset prevents visible jump on resume
        time = 0f;
        enabled = true;
    }
}