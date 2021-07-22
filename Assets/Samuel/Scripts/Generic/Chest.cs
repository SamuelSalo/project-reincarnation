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
        Debug.Log("xd");
        bool item = Random.Range(0, 101) > 50;

        var rng = Random.Range(0, 101);

        Rarity rarityRNG;
        if (rng < 60) rarityRNG = Rarity.Rare; //60%
        else if (rng < 90) rarityRNG = Rarity.Epic; //30%
        else rarityRNG = Rarity.Legendary; // 10%

        if (item)
        {
            switch (rarityRNG)
            {
                case Rarity.Rare:
                    InventoryManager.instance.AddPerk(rares[Random.Range(0, rares.Length)]);
                    spriteTint.FlashColor(Color.blue);
                    break;
                case Rarity.Epic:
                    InventoryManager.instance.AddPerk(epics[Random.Range(0, epics.Length)]);
                    spriteTint.FlashColor(new Color32(238, 130, 238, 255));
                    break;
                case Rarity.Legendary:
                    InventoryManager.instance.AddPerk(legendaries[Random.Range(0, legendaries.Length)]);
                    spriteTint.FlashColor(new Color32(255, 165, 0, 255));
                    break;
            }
        }
        else
        {
            switch (rarityRNG)
            {
                case Rarity.Rare:
                    InventoryManager.instance.GiveTokens(50);
                    spriteTint.FlashColor(Color.red);
                    break;
                case Rarity.Epic:
                    InventoryManager.instance.GiveTokens(100);
                    spriteTint.FlashColor(Color.red);
                    break;
                case Rarity.Legendary:
                    InventoryManager.instance.GiveTokens(200);
                    spriteTint.FlashColor(Color.red);
                    break;
            }
        }
    }
}
