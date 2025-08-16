using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    [SerializeField] public static float masterVolume = 0.5f;
    [SerializeField] public static float musicVolume = 0.5f;
    [SerializeField] public static float sfxVolume = 0.5f;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    
}
