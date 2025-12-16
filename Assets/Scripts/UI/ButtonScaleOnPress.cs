using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonScaleOnPress : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private float pressedScale = 0.9f;
    [SerializeField] private float returnSpeed = 10f;

    private Vector3 originalScale;
    private bool isPressed = false;

    private void Awake()
    {
        // Cached to ensure consistent return target across layout rebuilds
        originalScale = transform.localScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;

        // Immediate scale change used to give instant tactile feedback
        transform.localScale = originalScale * pressedScale;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // State flag used instead of animation callbacks to keep logic minimal
        isPressed = false;
    }

    private void Update()
    {
        // Manual lerp chosen over Animator to avoid overhead for simple UI feedback
        if (!isPressed && transform.localScale != originalScale)
        {
            transform.localScale = Vector3.Lerp(
                transform.localScale,
                originalScale,
                Time.deltaTime * returnSpeed
            );
        }
    }
}
