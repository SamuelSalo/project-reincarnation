using UnityEngine;
using Rarity = PerkObject.Rarity;

public class Chest : Interactable
{
    private PerkObject[] rares, epics, legendaries, negatives;
    private SpriteTint spriteTint;
    private bool opened;
    private Animator animator;

    protected override void Start()
    {
        base.Start();
        PerkLoader.LoadPerks(ref negatives,ref  rares, ref epics, ref legendaries);
        spriteTint = GetComponent<SpriteTint>();
        animator = GetComponent<Animator>();
    }

    public override void Interact()
    {
        base.Interact();
        if (!interactable || opened) return;

        opened = true;
        animator.SetTrigger("openChest");
    }

    public void OpenChest()
    {
        Color tintColor = Color.white;
        bool item = Random.Range(0, 101) > 50;
        var rng = Random.Range(0, 101);
        Rarity rarityRNG;

        if (rng > 60)
        {
            if (rng > 90)
                rarityRNG = Rarity.Legendary;
            else rarityRNG = Rarity.Epic;
        }
        else rarityRNG = Rarity.Rare;
        
        if (item)
        {
            switch (rarityRNG)
            {
                case Rarity.Rare:
                    InventoryManager.instance.AddPerk(rares[Random.Range(0, rares.Length)]);
                    tintColor = Color.blue;
                    break;
                case Rarity.Epic:
                    InventoryManager.instance.AddPerk(epics[Random.Range(0, epics.Length)]);
                    tintColor = Color.magenta;
                    break;
                case Rarity.Legendary:
                    InventoryManager.instance.AddPerk(legendaries[Random.Range(0, legendaries.Length)]);
                    tintColor = Color.yellow;
                    break;
            }
        }
        else
        {
            tintColor = Color.red;
            switch (rarityRNG)
            {
                case Rarity.Rare:
                    InventoryManager.instance.GiveTokens(50);
                    break;
                case Rarity.Epic:
                    InventoryManager.instance.GiveTokens(100);
                    break;
                case Rarity.Legendary:
                    InventoryManager.instance.GiveTokens(200);
                    break;
            }
        }
        spriteTint.FlashColor(tintColor);
    }
}
