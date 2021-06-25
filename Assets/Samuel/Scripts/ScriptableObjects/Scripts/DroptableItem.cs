using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rarity = Item.Rarity;

[CreateAssetMenu(menuName = "ScriptableObject/New DroptableItem", fileName = "New DroptableItem")]
public class DroptableItem : ScriptableObject
{
    public enum Type { Gold, Item }

    public Rarity rarity;
    public Type type;

    public int goldAmount;
    public Item item;
}
