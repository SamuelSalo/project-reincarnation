using UnityEngine;

public class PerkBehaviour : MonoBehaviour
{
    public PerkObject perkObject;

    /// <summary>
    /// Definitely not best practice, but I'm out of ideas.
    /// </summary>
    public void Activate()
    {
        if(!perkObject)
        {
            Debug.LogWarning("Tried to activate perk without perk object!");
            return;
        }

        Debug.Log(perkObject.perkName);
        switch(perkObject.perkName)
        {
            
            case "Max HP Perk":
                StatsManager.instance.itemMaxHpBonus += 5;
                break;
        }
    }
}