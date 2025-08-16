using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Slider _masterVolumeSlider;
    [SerializeField] private Slider _musicVolumeSlider;
    [SerializeField] private Slider _sfxVolumeSlider;
    void Start()
    {
        _masterVolumeSlider.value = SettingsManager.masterVolume * 100;
        _musicVolumeSlider.value = SettingsManager.musicVolume * 100;
        _sfxVolumeSlider.value = SettingsManager.sfxVolume * 100;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MasterVolumeChanged()
    {
        SettingsManager.masterVolume = _masterVolumeSlider.value / 100f;
    }
    
    public void MusicVolumeChanged()
    {
        SettingsManager.musicVolume = _musicVolumeSlider.value / 100f;
    }
    
    public void SfxVolumeChanged()
    {
        SettingsManager.sfxVolume = _sfxVolumeSlider.value / 100f;
    }
    
    public void UpdateSounds()
    {
        AudioManager.instance.UpdateVolume();
    }
}
