using UnityEngine;

public class Interactable : MonoBehaviour
{
    #region InputActions
    private PlayerControls playerControls;
    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();
            playerControls.Gameplay.Interact.performed += context => Interact();
        }

        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }
    #endregion

    [HideInInspector] public bool interactable;

    protected virtual void Start()
    {
        
    }

    public virtual void Interact()
    {
        if (!interactable) return;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !collision.isTrigger)
        {
            interactable = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            interactable = false;
        }
    }
}
