using System.Collections;
using UnityEngine;

public class Teleporter : Interactable
{
    public Room destinationRoom;
    private Room currentRoom;

    private Vector2 destination;
    private bool teleporting;

    protected override void Start()
    {
        base.Start();

        currentRoom = transform.parent.GetComponent<Room>();
        destination = transform.GetChild(0).position;
    }

    public override void Interact()
    {
        base.Interact();

        if (!interactable) return;

        //Allow player to backtrack freely and roam across cleared/own faction rooms, but require room to be cleared to advance
        if (!teleporting)
        {
            if (currentRoom.cleared || destinationRoom.cleared || destinationRoom.faction == GameManager.instance.playerFaction || currentRoom.faction == GameManager.instance.playerFaction)
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
        GameManager.instance.playerCharacter.player.freeze = true;
        GameManager.instance.canPause = false;
        FaderOverlay.instance.FadeOut();

        yield return new WaitForSeconds(1f);
        GameManager.instance.playerCharacter.transform.position = destination;

        
        teleporting = false;
        GameManager.instance.canPause = true;
        GameManager.instance.currentRoom = destinationRoom;
        GameManager.instance.playerCharacter.player.freeze = false;
        FaderOverlay.instance.FadeIn();
    }
}
