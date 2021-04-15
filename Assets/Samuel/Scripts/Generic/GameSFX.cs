using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GameSFX : MonoBehaviour
{
    public AudioClip[] hurtSFX;
    public AudioClip[] slashSFX;
    public AudioClip[] deathSFX;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayHurtSFX()
    {
        audioSource.PlayOneShot(hurtSFX[Random.Range(0, hurtSFX.Length)]);
    }
    public void PlaySlashSFX()
    {
        audioSource.PlayOneShot(slashSFX[Random.Range(0, slashSFX.Length)]);
    }
    public void PlayDeathSFX()
    {
        audioSource.PlayOneShot(deathSFX[Random.Range(0, deathSFX.Length)]);
    }
}
