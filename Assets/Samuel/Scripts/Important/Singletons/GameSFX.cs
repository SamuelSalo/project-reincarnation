using UnityEngine;
using UnityEngine.Audio;

public class GameSFX : MonoBehaviour
{
    public enum SFXType { Hit, Miss, Death, Dash, Heal, Crit};

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
    public AudioClip[] hitSFX;
    public AudioClip[] missSFX;
    public AudioClip[] deathSFX;
    public AudioClip[] dashSFX;
    public AudioClip[] healSFX;

    public AudioMixerGroup sfxGroup;

    public void PlaySFX(SFXType sfxType)
    {
        var audioSource = NewAudioInstance();
        audioSource.clip = sfxType switch
        {
            SFXType.Hit => GetRandomClip(hitSFX),
            SFXType.Miss => GetRandomClip(missSFX),
            SFXType.Death => GetRandomClip(deathSFX),
            SFXType.Dash => GetRandomClip(dashSFX),
            SFXType.Heal => GetRandomClip(healSFX),
            _ => throw new System.NotImplementedException()
        };

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
    private AudioClip GetRandomClip(AudioClip[] clips)
    {
        return clips[Random.Range(0, clips.Length)];
    }
}