using UnityEngine;

[CreateAssetMenu(fileName = "New Perk", menuName = "ScriptableObject/New Perk")]
public class PerkObject : ScriptableObject
{
    public enum Rarity { Rare, Epic, Legendary}
    public Rarity rarity;

    public string perkName;

    [TextArea] public string description;
    public Sprite icon;
}
