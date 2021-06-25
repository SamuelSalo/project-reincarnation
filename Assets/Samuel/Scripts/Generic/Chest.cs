using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rarity = Item.Rarity;
using Type = DroptableItem.Type;

public class Chest : Interactable
{
    private bool opened;

    public DroptableItem[] rares, epics, legendaries;
    
    public override void Interact()
    {
        base.Interact();
        if (!interactable || opened) return;

        var rng = Random.Range(0, 101);

        Rarity rarityRNG;
        if (rng < 60) rarityRNG = Rarity.Rare; //60%
        else if (rng < 90) rarityRNG = Rarity.Epic; //30%
        else rarityRNG = Rarity.Legendary; // 10%

        DroptableItem item = null;

        switch (rarityRNG)
        {
            case Rarity.Rare:
                item = rares[Random.Range(0, rares.Length)];

                break;

            case Rarity.Epic:
                item = epics[Random.Range(0, epics.Length)];
                break;

            case Rarity.Legendary:
                item = legendaries[Random.Range(0, legendaries.Length)];
                break;
            
        }

        switch (item.type)
        {
            case Type.Item:
                gameManager.invManager.AddItem(item.item);
                break;

            case Type.Gold:
                gameManager.invManager.GiveGold(item.goldAmount);
                break;
        }
        OpenChest();
        //TODO droptable
    }

    private void OpenChest()
    {
        //TODO open chest sprite
        opened = true;
        GetComponent<SpriteRenderer>().color = Color.red;
    }
}
