using UnityEngine;

[CreateAssetMenu(fileName = "New Perk", menuName = "ScriptableObject/New Perk")]
public partial class PerkObject : ScriptableObject
{
    public Rarity rarity;

    public string perkName;

    [TextArea] public string description;
    public Sprite icon;
}
public partial class PerkObject : ScriptableObject
{
    public enum Rarity { Negative, Rare, Epic, Legendary }
}
