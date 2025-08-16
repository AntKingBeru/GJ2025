using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private List<Sound> _sounds;
    public static AudioManager instance;
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        UpdateVolume();
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void UpdateVolume()
    {
        foreach (Sound s in _sounds)
        {
            s.audioSource = gameObject.AddComponent<AudioSource>();
            s.audioSource.clip = s.clip;
            
            float multiplier = 0;
            if (s.name == "BackgroundMusic")
                multiplier = SettingsManager.musicVolume;
            else
                multiplier = SettingsManager.sfxVolume;
            
            s.audioSource.volume = s.volume * SettingsManager.masterVolume * multiplier;
            s.audioSource.pitch = s.pitch;
            s.audioSource.loop = s.loop;

            if (s.audioSource.isPlaying)
            {
                s.audioSource.Stop();
                PlaySound(s.name);
            }
        }
    }

    public void PlaySound(string soundName, float duration = 0f)
    {
        foreach (Sound s in _sounds)
        {
            if (s.name == soundName)
            {
                if (duration > 0f)
                    s.audioSource.time = s.audioSource.clip.length - duration;
                s.audioSource.Play();
                break;
            }
        }
    }

    public void StopSound(string soundName)
    {
        foreach (Sound s in _sounds)
        {
            if (s.name == soundName)
            {
                s.audioSource.Stop();
                break;
            }
        }
    }
}