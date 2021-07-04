using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements;
using Rarity = PerkObject.Rarity;

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
    public GameObject perkHolder;

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
        bloodTokens += _amt;
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

        var perkBehaviour = perkHolder.AddComponent<PerkBehaviour>();
        perkBehaviour.perkObject = _perk;
        perkBehaviour.Activate();
        StatsManager.instance.UpdateCharacterStats();

        var obj = Instantiate(perkListObjectPrefab, perkList.transform);
        var txt = obj.GetComponent<TMP_Text>();
        txt.text = _perk.name;
        switch (_perk.rarity)
        {
            case Rarity.Rare:
                txt.color = Color.blue;
                break;
            case Rarity.Epic:
                txt.color = new Color(238, 130, 238);
                break;
            case Rarity.Legendary:
                txt.color = new Color(255, 165, 0);
                break;
        }

        perkAmountText.text = "Activated Perks: " + perkInventory.Count;
    }
}