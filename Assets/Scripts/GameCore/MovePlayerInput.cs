using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class MovePlayerInput : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float followSpeed = 15f;

    private bool isDragging;
    private Camera cam;

    private void Awake()
    {
        // Cached once to avoid repeated Camera.main lookups during gameplay
        cam = Camera.main;
    }

    private void Update()
    {
        // Input ignored outside active gameplay to avoid state desync
        if (!GameManager.instance.IsPlaying())
            return;

        if (player == null)
            return;

        // Reacquired defensively in case camera was recreated on scene load
        if (cam == null)
            cam = Camera.main;

        if (cam == null)
            return;

        // Platform split kept explicit to avoid mixed input edge cases
        if (Application.isMobilePlatform)
            HandleTouch();
        else
            HandleMouse();
    }

    private void HandleMouse()
    {
        if (Mouse.current == null)
            return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
            TryStartDrag(Mouse.current.position.ReadValue());

        if (Mouse.current.leftButton.wasReleasedThisFrame)
            isDragging = false;

        if (!isDragging)
            return;

        MovePlayer(Mouse.current.position.ReadValue());
    }

    private void HandleTouch()
    {
        if (Touchscreen.current == null)
            return;

        var touch = Touchscreen.current.primaryTouch;

        if (touch.press.wasPressedThisFrame)
            TryStartDrag(touch.position.ReadValue());

        if (touch.press.wasReleasedThisFrame)
            isDragging = false;

        // Continuous check prevents stale drag when touch is interrupted
        if (!isDragging || !touch.press.isPressed)
            return;

        MovePlayer(touch.position.ReadValue());
    }

    private void TryStartDrag(Vector2 screenPos)
    {
        if (!GameManager.instance.IsPlaying())
            return;

        // UI blocking applied only on mobile to avoid breaking desktop input
        if (Application.isMobilePlatform &&
            EventSystem.current != null &&
            Touchscreen.current != null &&
            EventSystem.current.IsPointerOverGameObject(
                Touchscreen.current.primaryTouch.touchId.ReadValue()))
            return;

        Vector3 worldPos = cam.ScreenToWorldPoint(screenPos);
        worldPos.z = 0f;

        // Collider check used instead of distance to require explicit grab
        Collider2D hit = Physics2D.OverlapPoint(worldPos);

        if (hit != null && hit.transform == player)
            isDragging = true;
    }

    private void MovePlayer(Vector2 screenPos)
    {
        Vector3 worldPos = cam.ScreenToWorldPoint(screenPos);
        worldPos.z = 0f;

        // Hard clamp prevents off-screen movement across different aspect ratios
        float clampedX = Mathf.Clamp(worldPos.x, -2.8f, 2.8f);
        Vector3 target = new Vector3(clampedX, player.position.y, player.position.z);

        // Lerp chosen to soften input noise instead of snapping to cursor
        player.position = Vector3.Lerp(
            player.position,
            target,
            followSpeed * Time.deltaTime
        );
    }

    public void ForceStop()
    {
        // External reset used for pauses and state transitions
        isDragging = false;
    }
}