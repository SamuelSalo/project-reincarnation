using System.Collections;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    private PlayerControls playerControls;

    public Room destinationRoom;
    private Room currentRoom;

    private FaderOverlay fader;
    private GameManager gameManager;
    private Vector2 destination;
    private bool reach;
    private bool teleporting;

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.Gameplay.Interact.performed += context => Teleport();
        }

        playerControls.Enable();
    }

    private void Start()
    {
        currentRoom = transform.parent.GetComponent<Room>();
        fader = GameObject.FindWithTag("FaderOverlay").GetComponent<FaderOverlay>();
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        destination = transform.GetChild(0).position;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            reach = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            reach = false;
    }


    private void Teleport()
    {
        //Allow player to backtrack freely and roam across cleared/own faction rooms, but require room to be cleared to advance
        if (reach && !teleporting)
        {
            if (currentRoom.cleared || destinationRoom.cleared || destinationRoom.faction == gameManager.playerFaction || currentRoom.faction == gameManager.playerFaction)
                StartCoroutine(TeleportPlayer());
            else if (!currentRoom.cleared)
                GameObject.FindWithTag("Tooltip").GetComponent<Tooltip>().ShowTooltip("Clear the room of enemies first!", 2f);
        }
    }

    /// <summary>
    /// Simple teleportation logic, fade the screen out to black, teleport the player to destination and fade the screen back in.
    /// Also locks player movement and inputs during teleport;
    /// </summary>
    private IEnumerator TeleportPlayer()
    {
        teleporting = true;
        gameManager.playerCharacter.player.freeze = teleporting;
        fader.FadeOut();

        yield return new WaitForSeconds(1f);
        gameManager.playerCharacter.transform.position = destination;

        
        teleporting = false;
        gameManager.playerCharacter.player.freeze = teleporting;
        fader.FadeIn();
    }
}
