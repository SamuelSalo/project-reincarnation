using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GameSFX : MonoBehaviour
{
    public AudioClip[] hurtSFX;
    public AudioClip[] slashSFX;
    public AudioClip[] deathSFX;
    public AudioClip[] dashSFX;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayHurtSFX()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(hurtSFX[Random.Range(0, hurtSFX.Length)]);
    }
    public void PlaySlashSFX()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(slashSFX[Random.Range(0, slashSFX.Length)]);
    }
    public void PlayDeathSFX()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(deathSFX[Random.Range(0, deathSFX.Length)]);
    }
    public void PlayDashSFX()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(dashSFX[Random.Range(0, dashSFX.Length)]);
    }
}
