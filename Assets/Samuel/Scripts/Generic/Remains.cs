using UnityEngine;
using System.Collections;
using Faction = Character.Faction;

public class Remains : MonoBehaviour
{
    private PlayerControls playerControls;

    [HideInInspector] public Faction faction;
    [Range(0,100)] public float restoreAmount;
    private Character character;
    private bool running = false;
    private float timer;

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.Gameplay.HoldInteract.started += context => StartConsume();
            playerControls.Gameplay.HoldInteract.canceled += context => CancelConsume();
        }
        playerControls.Enable();
            
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !collision.isTrigger)
        {
            character = collision.GetComponent<Character>();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            character = null;
        }
    }

    private void StartConsume()
    {
        if (character && !running)
            if (character.faction == Faction.Red)
            {
                running = true;
                character.player.freeze = true;
                GameObject.FindWithTag("ProgressBar").GetComponent<InteractProgressBar>().StartProgress(2f);
            }
    }

    private void CancelConsume()
    {
        if (character && running)
        {
            running = false;
            timer = 0f;
            character.player.freeze = false;
            GameObject.FindWithTag("ProgressBar").GetComponent<InteractProgressBar>().StopProgress();
        }
    }

    private void Update()
    {
        if(running && character)
        {
            timer += Time.deltaTime;

            if (timer >= 2f)
                FinishConsume();
        }
    }

    private void FinishConsume()
    {
        running = false;
        playerControls.Disable();
        character.player.freeze = false;
        character.RestoreHealth(restoreAmount);
        Destroy(gameObject);
    }
}