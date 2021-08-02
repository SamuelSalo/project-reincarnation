using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Rarity = PerkObject.Rarity;

public class PerkDisplay : MonoBehaviour
{
    public TMP_Text perkText;
    public TMP_Text perkDescription;
    public Image perkIcon;
    public GameObject panel;

    public void SetPerkToDisplay(PerkObject _perk)
    {
        perkIcon.sprite = _perk.icon;
        perkText.text = _perk.perkName;
        perkDescription.text = _perk.description;

        perkText.color = _perk.rarity switch
        {
            Rarity.Negative => Color.red,
            Rarity.Rare => Color.blue,
            Rarity.Epic => Color.magenta,
            Rarity.Legendary => Color.yellow,
            _ => Color.black,
        };
    }
    public void TogglePanel()
    {
        panel.SetActive(!panel.activeSelf);
    }
}
