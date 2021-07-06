using UnityEngine;
using Faction = Character.Faction;

public class Finish : Interactable
{
    public Faction faction;
    [Space]
    public GameObject gameUI;
    public GameObject finishUI;

    private Room room;

    protected override void Start()
    {
        base.Start();
        room = transform.parent.GetComponent<Room>();
    }

    public override void Interact()
    {
        base.Interact();

        if (interactable)
        {
            if (GameManager.instance.playerFaction != faction && room.cleared)
                FinishGame();
            else if(GameManager.instance.playerFaction == faction)
                Tooltip.instance.ShowTooltip("Wrong direction! Go to the other end...", 2f);
            else if(GameManager.instance.playerFaction != faction && !room.cleared)
                Tooltip.instance.ShowTooltip("Clear the room of enemies first!", 2f);
        }
    }

    private void FinishGame()
    {
        gameUI.SetActive(false);
        finishUI.SetActive(true);
        Time.timeScale = 0f;
        GameManager.instance.canPause = false;
    }
}
