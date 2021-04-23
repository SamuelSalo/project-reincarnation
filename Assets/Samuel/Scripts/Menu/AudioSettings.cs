using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    public AudioMixer audioMixer;

    [Space] [Header("Volume Sliders")]
    public Slider musicVolSlider;
    public Slider sfxVolSlider;

    public void Start()
    {
        if (PlayerPrefs.HasKey("musicVolume"))
            audioMixer.SetFloat("musicVolume", PlayerPrefs.GetFloat("musicVolume"));

        if (PlayerPrefs.HasKey("sfxVolume"))
            audioMixer.SetFloat("sfxVolume", PlayerPrefs.GetFloat("sfxVolume"));

        if (audioMixer.GetFloat("musicVolume", out var musicVol))
            musicVolSlider.value = musicVol;

        if (audioMixer.GetFloat("sfxVolume", out var sfxVol))
            sfxVolSlider.value = sfxVol;
    }

    public void SetMusicVolume(float _volume)
    {
        audioMixer.SetFloat("musicVolume", _volume);
        PlayerPrefs.SetFloat("musicVolume", _volume);
        PlayerPrefs.Save();
    }
    public void SetSFXVolume(float _volume)
    {
        audioMixer.SetFloat("sfxVolume", _volume);
        PlayerPrefs.SetFloat("sfxVolume", _volume);
        PlayerPrefs.Save();
    }
}
