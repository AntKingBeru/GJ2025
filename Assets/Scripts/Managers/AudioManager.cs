using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private List<Sound> sounds;
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

        foreach (Sound s in sounds)
        {
            s.audioSource = gameObject.AddComponent<AudioSource>();
            s.audioSource.clip = s.clip;
            
            s.audioSource.volume = s.volume;
            s.audioSource.pitch = s.pitch;
            s.audioSource.loop = s.loop;
        }
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySound(string soundName, float duration = 0f)
    {
        foreach (Sound s in sounds)
        {
            if (s.name == soundName)
            {
                if (duration > 0f)
                    s.audioSource.time = s.audioSource.clip.length - duration;
                s.audioSource.Play();
            }
        }
    }
    
    public void StopSound(string soundName)
    {
        foreach (Sound s in sounds)
        {
            if (s.name == soundName)
                s.audioSource.Stop();
        }
    }
}