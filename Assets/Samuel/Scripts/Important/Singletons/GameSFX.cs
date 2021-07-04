using UnityEngine;
using UnityEngine.Audio;

public class GameSFX : MonoBehaviour
{

    #region Singleton

    public static GameSFX instance;

    private void Awake()
    {
        if(instance)
        {
            Debug.LogWarning("Multiple instances of " + name + "found!");
            return;
        }
        instance = this;
    }

    #endregion

    public AudioClip[] hurtSFX;
    public AudioClip[] slashSFX;
    public AudioClip[] deathSFX;
    public AudioClip[] dashSFX;
    public AudioClip[] healSFX;
    public AudioMixerGroup sfxGroup;

    public void PlayHurtSFX()
    {
        var audioSource = NewAudioInstance();
        audioSource.clip = hurtSFX[Random.Range(0, hurtSFX.Length)];
        audioSource.Play();
        Destroy(audioSource.gameObject, audioSource.clip.length);
    }
    public void PlaySlashSFX()
    {
        var audioSource = NewAudioInstance();
        audioSource.clip = slashSFX[Random.Range(0, slashSFX.Length)];
        audioSource.Play();
        Destroy(audioSource.gameObject, audioSource.clip.length);
    }
    public void PlayDeathSFX()
    {
        var audioSource = NewAudioInstance();
        audioSource.clip = deathSFX[Random.Range(0, deathSFX.Length)];
        audioSource.Play();
        Destroy(audioSource.gameObject, audioSource.clip.length);
    }
    public void PlayDashSFX()
    {
        var audioSource = NewAudioInstance();
        audioSource.clip = dashSFX[Random.Range(0, dashSFX.Length)];
        audioSource.Play();
        Destroy(audioSource.gameObject, audioSource.clip.length);
    }
    public void PlayHealSFX()
    {
        var audioSource = NewAudioInstance();
        audioSource.clip = healSFX[Random.Range(0, healSFX.Length)];
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