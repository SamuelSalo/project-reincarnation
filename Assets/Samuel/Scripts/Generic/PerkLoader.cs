using UnityEngine;

public static class PerkLoader
{
    public static void LoadPerks(ref PerkObject[] negatives, ref PerkObject[] rares, ref PerkObject[] epics, ref PerkObject[] legendaries)
    {
        rares = Resources.LoadAll<PerkObject>("Perks/Rare/");
        epics = Resources.LoadAll<PerkObject>("Perks/Epic/");
        legendaries = Resources.LoadAll<PerkObject>("Perks/Legendary/");
        negatives = Resources.LoadAll<PerkObject>("Perks/Negative/");
    }
}
