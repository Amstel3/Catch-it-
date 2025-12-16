using UnityEngine;

public class MusicButtonUI : MonoBehaviour
{
    [SerializeField] private GameObject soundOnIcon;
    [SerializeField] private GameObject soundOffIcon;

    private void OnEnable()
    {
        // Synced on enable to stay correct after scene loads and UI reactivations
        UpdateIcons();
    }

    public void UpdateIcons()
    {
        bool isOn = AudioManager.Instance.IsSoundOn;

        soundOnIcon.SetActive(isOn);
        soundOffIcon.SetActive(!isOn);
    }
}

