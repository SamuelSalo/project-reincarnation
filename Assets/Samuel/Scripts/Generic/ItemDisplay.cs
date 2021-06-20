using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Rarity = Item.Rarity;

public class ItemDisplay : MonoBehaviour
{
    public Item item;

    [Space]

    public TMP_Text nameLabel;
    public TMP_Text descriptionLabel;
    public TMP_Text priceLabel;
    public Image iconImage;

    public void UpdateDisplay()
    {
        if(!item)
        {
            nameLabel.text = "None";
            descriptionLabel.text = "No item equipped.";
            iconImage.sprite = null;
            nameLabel.color = Color.white;
            return;
        }

        nameLabel.text = item.name;
        descriptionLabel.text = item.description;
        if (priceLabel) priceLabel.text = item.price.ToString() + "G";
        iconImage.sprite = item.icon;

        switch (item.rarity)
        {
            case Rarity.Rare:
                nameLabel.color = Color.blue;
                break;
            case Rarity.Epic:
                nameLabel.color = new Color(238, 130, 238);
                break;
            case Rarity.Legendary:
                nameLabel.color = new Color(255, 165, 0);
                break;
        }
    }
}
