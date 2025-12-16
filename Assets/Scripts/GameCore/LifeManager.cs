using UnityEngine;
using UnityEngine.UI;

public class LifeManager : MonoBehaviour
{
    public static LifeManager instance;

    [SerializeField] private Image[] hearts;

    private int currentLives = 3;

    private void Awake()
    {
        // Single instance used to keep life state globally consistent
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        ResetLives();
    }

    public void LoseLife()
    {
        // Guard prevents negative states and repeated game over calls
        if (currentLives <= 0)
            return;

        currentLives--;
        UpdateHeartsDisplay();

        // Game over delegated to GameManager to avoid circular rules
        if (currentLives <= 0)
            GameManager.instance.GameOver();
    }

    private void UpdateHeartsDisplay()
    {
        // Direct UI update chosen for simplicity over event indirection
        for (int i = 0; i < hearts.Length; i++)
            hearts[i].enabled = i < currentLives;
    }

    public void ResetLives()
    {
        // Explicit reset keeps retries visually and logically clean
        currentLives = 3;
        UpdateHeartsDisplay();
    }
}