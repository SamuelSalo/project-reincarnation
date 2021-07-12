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

        switch (_perk.rarity)
        {
            case Rarity.Rare:
                perkText.color = Color.blue;
                break;
            case Rarity.Epic:
                perkText.color = new Color32(238, 130, 238, 255); //purple
                break;
            case Rarity.Legendary:
                perkText.color = new Color32(255, 165, 0, 255); //orange
                break;
            case Rarity.Negative:
                perkText.color = Color.red;
                break;
        }
    }
    public void TogglePanel()
    {
        panel.SetActive(!panel.activeSelf);
    }
}
