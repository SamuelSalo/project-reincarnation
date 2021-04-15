using UnityEngine;

public class MenuSFX : MonoBehaviour
{
    public AudioClip select;
    public AudioClip click;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = Camera.main.GetComponent<AudioSource>();
    }

    public void PlaySelectSound()
    {
        audioSource.PlayOneShot(select);
    }

    public void PlayClickSound()
    {
        audioSource.PlayOneShot(click);
    }
}
