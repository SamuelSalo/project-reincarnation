using UnityEngine;
using Rarity = PerkObject.Rarity;

[CreateAssetMenu(menuName = "ScriptableObject/New DroptableItem", fileName = "New DroptableItem")]
public class DroptableItem : ScriptableObject
{
    public enum Type { BloodTokens, Perk }

    public Rarity rarity;
    public Type type;

    public int tokenAmount;
    public PerkObject perk;
}
