using System.Collections;
using UnityEngine;

public class Teleporter : Interactable
{
    public Room destinationRoom;
    private Room currentRoom;

    private FaderOverlay fader;
    private Vector2 destination;
    private bool teleporting;

    protected override void Start()
    {
        base.Start();

        currentRoom = transform.parent.GetComponent<Room>();
        fader = GameObject.FindWithTag("FaderOverlay").GetComponent<FaderOverlay>();
        destination = transform.GetChild(0).position;
    }

    public override void Interact()
    {
        base.Interact();

        if (!interactable) return;

        //Allow player to backtrack freely and roam across cleared/own faction rooms, but require room to be cleared to advance
        if (!teleporting)
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
