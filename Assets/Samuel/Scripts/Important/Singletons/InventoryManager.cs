using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    #region Singleton

    public static InventoryManager instance;

    private void Awake()
    {
        if (instance)
        {
            Debug.LogWarning("Multiple instances of " + name + "found!");
            return;
        }
        instance = this;
    }

    #endregion

    public int bloodTokens;
    public TMP_Text tokensText;
    public List<PerkObject> perkInventory;

    [Space]
    [Header("UI")]
    public GameObject perkList;
    public GameObject perkListObjectPrefab;
    public TMP_Text perkAmountText;

    private void Start()
    {
        perkInventory = new List<PerkObject>();
    }

    public void GiveTokens(int _amt)
    {
        int adjAmount = _amt;

        if(PerkManager.instance.perkDictionary["Gravelord's Blessing"] > 0)
        {
            adjAmount += PerkManager.instance.perkDictionary["Gravelord's Blessing"] * 10;
        }

        bloodTokens += adjAmount;
        tokensText.text = bloodTokens + " Blood Tokens";
    }

    public void SpendTokens(int _amt)
    {
        bloodTokens -= _amt;
        tokensText.text = bloodTokens + " Blood Tokens";
    }

    public void AddPerk(PerkObject _perk)
    {
        perkInventory.Add(_perk);

        PerkManager.instance.Activate(_perk);
        StatsManager.instance.UpdateCharacterStats();

        var obj = Instantiate(perkListObjectPrefab, perkList.transform);
        obj.GetComponent<PerkDisplay>().SetPerkToDisplay(_perk);

        perkAmountText.text = "Activated Perks: " + perkInventory.Count;
    }
}