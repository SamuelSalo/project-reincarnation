using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "ScriptableObject/New Item")]
public class Item : ScriptableObject
{
    public enum Rarity { Rare, Epic, Legendary}
    public Rarity rarity;

    public new string name;

    [TextArea] public string description;

    public int price;

    public Sprite icon;
}
