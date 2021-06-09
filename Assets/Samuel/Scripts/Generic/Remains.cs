using UnityEngine;
using Faction = Character.Faction;

public class Remains : MonoBehaviour
{
    private PlayerControls playerControls;

    [HideInInspector] public Faction faction;
    [Range(0,100)] public float restoreAmount;
    private Character character;

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.Gameplay.Interact.performed += context => Activate();
        }
        playerControls.Enable();
            
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            character = collision.GetComponent<Character>();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            character = null;
        }
    }

    private void Update()
    {
        if (character && Input.GetKeyDown(KeyCode.E))
            if (character.faction == Faction.Red)
            {
                character.RestoreHealth(restoreAmount);
                Destroy(gameObject);
            }
    }

    private void Activate()
    {
        if (character)
            if (character.faction == Faction.Red)
            {
                character.RestoreHealth(restoreAmount);
                Destroy(gameObject);
            }
    }
}
