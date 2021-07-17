using UnityEngine;
using UnityEngine.Audio;

public class MenuSFX : MonoBehaviour
{
    #region Singleton

    public static MenuSFX instance;

    private void Awake()
    {
        if (instance)
        {
            Debug.LogWarning("Multiple instances of " + name + "found!");
            return;
        }
        instance = this;
    }

    #endregion

    public AudioClip select;
    public AudioClip click;
    public AudioMixerGroup sfxGroup;

    public void PlaySelectSound()
    {
        var audioSource = NewAudioInstance();
        audioSource.clip = select;
        audioSource.Play();
        Destroy(audioSource.gameObject, audioSource.clip.length);
    }

    public void PlayClickSound()
    {
        var audioSource = NewAudioInstance();
        audioSource.clip = click;
        audioSource.Play();
        Destroy(audioSource.gameObject, audioSource.clip.length);
    }
    private AudioSource NewAudioInstance()
    {
        var audioObject = new GameObject("AudioSource Instance");
        audioObject.transform.parent = transform;
        var audioSource = audioObject.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = sfxGroup;
        return audioSource;
    }
}