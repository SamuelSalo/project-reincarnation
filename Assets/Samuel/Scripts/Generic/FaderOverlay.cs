using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class FaderOverlay : MonoBehaviour
{

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void FadeIn()
    {
        animator.SetTrigger("FadeIn");
    }

    public void FadeOut()
    {
        animator.SetTrigger("FadeOut");
    }
}