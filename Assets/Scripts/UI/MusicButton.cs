using UnityEngine;

public class MusicButton : MonoBehaviour
{
    public void OnMusicButtonClicked()
    {
        // UI delegates preference change without owning audio logic
        AudioManager.Instance.ToggleSound();
    }
}
