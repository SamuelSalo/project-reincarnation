using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

/// <summary>
/// Controls settings & scene transitions in the menu
/// </summary>
public class MainMenu : MonoBehaviour
{
    public GameObject titlePanel;
    public GameObject settingsPanel;
    public AudioMixer audioMixer;

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
    public void TogglePanel()
    {
        titlePanel.SetActive(!titlePanel.activeSelf);
        settingsPanel.SetActive(!settingsPanel.activeSelf);
        PlayerPrefs.Save();
    }

    public void SetMusicVolume(float _volume)
    {
        audioMixer.SetFloat("musicVolume", _volume);
        PlayerPrefs.SetFloat("musicVolume", _volume);
    }
    public void SetSFXVolume(float _volume)
    {
        audioMixer.SetFloat("sfxVolume", _volume);
        PlayerPrefs.SetFloat("sfxVolume", _volume);
    }

    public void StartGame()
    {
        StartCoroutine(StartGameRoutine());
    }
    IEnumerator StartGameRoutine()
    {
        GameObject.FindWithTag("FaderOverlay").GetComponent<FaderOverlay>().FadeOut();
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(1);
    }
}