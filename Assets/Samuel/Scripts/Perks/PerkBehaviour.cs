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
        switch (perkObject.perkName)
        {
            //Legendary
            case "Gravelord's Blessing":
                PerkManager.instance.gravelordsBlessing++;
                break;
            case "Wise Man's Journal":
                PerkManager.instance.wiseMansJournal++;
                break;

            //Epic
            case "Lucky Charm":
                PerkManager.instance.luckyCharm++;
                break;

            //Rare
            case "Whetstone":
                PerkManager.instance.whetstone++;
                break;

            //Negative
            case "Gravelord's Curse":
                PerkManager.instance.gravelordsCurse++;
                break;

            //Default
            default:
                Debug.LogWarning("Perk switch defaulted! Possible misspelling of: " + perkObject.perkName);
                break;
        }
    }
}