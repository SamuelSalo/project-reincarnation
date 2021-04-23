using UnityEngine;
using UnityEngine.Audio;

public class MenuSFX : MonoBehaviour
{
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