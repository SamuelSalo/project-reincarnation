using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using OptionData = TMPro.TMP_Dropdown.OptionData;

public class InventoryManager : MonoBehaviour
{
    public StatsManager statsManager;
    public int gold;

    [Space]

    public Item equippedItem;
    public ItemDisplay equippedItemDisplay;

    [Space]

    public Item TEST_startingItem;
    public TMP_Dropdown inventoryDropdown;
    public TMP_Text goldText;
    public List<Item> itemInventory;

    private void Start()
    {
        itemInventory = new List<Item>();
        //UpdateInventory();
        AddItem(TEST_startingItem);
        UpdateEquipment(0);
    }

    public void GiveGold(int _amt)
    {
        gold += _amt;
        goldText.text = gold + " Gold";
    }
    public void BuyItem(Item _item)
    {
        gold -= _item.price;
        goldText.text = gold + " Gold";
        AddItem(_item);
    }

    private void AddItem(Item _item)
    {
        itemInventory.Add(_item);
        UpdateInventory();
    }

    private void UpdateInventory()
    {
        if (itemInventory.Count == 0)
        {
            inventoryDropdown.ClearOptions();
            inventoryDropdown.AddOptions(new List<OptionData> { new OptionData("None") });
            return;
        }

        inventoryDropdown.interactable = true;

        var optionList = new List<OptionData> { new OptionData("None") };

        foreach (Item i in itemInventory)
        {
            optionList.Add(new OptionData(i.name));
        }

        inventoryDropdown.ClearOptions();
        inventoryDropdown.AddOptions(optionList);
    }

    public void UpdateEquipment(int _index)
    {
        if (_index == 0)
            equippedItem = null;
        else
            equippedItem = itemInventory[_index - 1];

        equippedItemDisplay.item = equippedItem;
        equippedItemDisplay.UpdateDisplay();
        UpdateStatEffects();
    }

    public void UpdateStatEffects()
    {
        //TODO


        //test code here
        if (equippedItem == null)
            return;
        statsManager.itemMaxHpBonus = equippedItem.name == "TestHPItem1" ? 5 : 0;
        statsManager.UpdateCharacterStats();
    }
}