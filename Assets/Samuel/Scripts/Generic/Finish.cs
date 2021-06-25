using UnityEngine;
using Faction = Character.Faction;

public class Finish : Interactable
{
    public Faction faction;
    [Space]
    public GameObject gameUI;
    public GameObject finishUI;

    private Room room;

    private void Start()
    {
        room = transform.parent.GetComponent<Room>();
    }

    public override void Interact()
    {
        base.Interact();

        if (interactable)
        {
            if (gameManager.playerFaction != faction && room.cleared)
                FinishGame();
            else if(gameManager.playerFaction == faction)
                GameObject.FindWithTag("Tooltip").GetComponent<Tooltip>().ShowTooltip("Wrong direction! Go to the other end...", 2f);
            else if(gameManager.playerFaction != faction && !room.cleared)
                GameObject.FindWithTag("Tooltip").GetComponent<Tooltip>().ShowTooltip("Clear the room of enemies first!", 2f);
        }
    }

    private void FinishGame()
    {
        gameUI.SetActive(false);
        finishUI.SetActive(true);
        Time.timeScale = 0f;
        gameManager.canPause = false;
    }
}
