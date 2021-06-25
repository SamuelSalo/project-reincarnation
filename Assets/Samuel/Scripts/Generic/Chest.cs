using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Interactable
{
    private bool opened;

    public override void Interact()
    {
        base.Interact();
        if (!interactable || opened) return;

        var rng = Random.Range(0, 100);

        if(rng > 75)
        {
            gameManager.invManager.GiveGold(100);
        }
        else
        {
            gameManager.invManager.GiveGold(50);
        }

        OpenChest();
    }

    private void OpenChest()
    {
        opened = true;
        GetComponent<SpriteRenderer>().color = Color.red;
    }
}
