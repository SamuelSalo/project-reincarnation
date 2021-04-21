using System.Collections;
using UnityEngine;
using Faction = Character.Faction;

public class Teleporter : MonoBehaviour
{
    public Room destinationRoom;
    private Room currentRoom;

    private FaderOverlay fader;
    private GameManager gameManager;
    private Vector2 destination;
    private bool reach;
    private bool teleporting;

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

    private void Update()
    {
        //Allow player to backtrack freely and roam across cleared/own faction rooms, but require room to be cleared to advance
        if (reach && Input.GetKeyDown(KeyCode.E) &&
            (currentRoom.cleared || destinationRoom.cleared || destinationRoom.faction == gameManager.currentFaction || currentRoom.faction == gameManager.currentFaction)
            && !teleporting)

            StartCoroutine(TeleportPlayer());
    }

    /// <summary>
    /// Simple teleportation logic, fade the screen out to black, teleport the player to destination and fade the screen back in.
    /// Also locks player movement and inputs during teleport;
    /// </summary>
    private IEnumerator TeleportPlayer()
    {
        teleporting = true;
        gameManager.currentCharacter.playerMovement.teleporting = teleporting;
        fader.FadeOut();

        yield return new WaitForSeconds(1f);
        gameManager.currentCharacter.transform.position = destination;

        
        teleporting = false;
        gameManager.currentCharacter.playerMovement.teleporting = teleporting;
        fader.FadeIn();
    }
}
