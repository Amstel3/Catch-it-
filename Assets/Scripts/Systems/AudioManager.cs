using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("AudioSources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource[] sfxSources;

    public bool IsSoundOn { get; private set; }

    private void Awake()
    {
        // Single instance required to keep audio preferences global
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Loaded once to preserve user preference across sessions
        IsSoundOn = PlayerPrefs.GetInt("SoundEnabled", 1) == 1;
    }

    public void ToggleSound()
    {
        IsSoundOn = !IsSoundOn;

        // Persisted immediately to avoid desync after app restart
        PlayerPrefs.SetInt("SoundEnabled", IsSoundOn ? 1 : 0);
        PlayerPrefs.Save();

        ApplySoundState();
    }

    public void ApplySoundState()
    {
        if (musicSource != null)
        {
            if (IsSoundOn)
            {
                // Music playback controlled by GameManager, not here
                musicSource.mute = false;
            }
            else
            {
                // Paused explicitly to prevent resume on scene reload
                musicSource.Pause();
                musicSource.mute = true;
            }
        }

        // SFX muted globally to respect user preference instantly
        foreach (var sfx in sfxSources)
        {
            if (sfx == null)
                continue;

            sfx.mute = !IsSoundOn;
        }
    }

    public void RegisterSFX(AudioSource source)
    {
        // Dynamic registration used for runtime-created sound sources
        if (source == null)
            return;

        var list = new List<AudioSource>(sfxSources);
        if (!list.Contains(source))
        {
            list.Add(source);
            sfxSources = list.ToArray();
        }

        // Applied immediately to stay consistent with current sound state
        source.mute = !IsSoundOn;
    }
}




