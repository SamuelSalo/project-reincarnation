using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class FaderOverlay : MonoBehaviour
{
    #region Singleton

    public static FaderOverlay instance;

    private void Awake()
    {
        if(instance)
        {
            Debug.Log("Multiple instances of " + name + " found!");
            return;
        }

        instance = this;
    }

    #endregion

    public bool fadeIntoScene;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>(); 
     
        if(fadeIntoScene)
            FadeIn();
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