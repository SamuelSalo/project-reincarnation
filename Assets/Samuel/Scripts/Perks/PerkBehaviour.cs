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
            case "Thornmail Armor":
                PerkManager.instance.thornmailArmor++;
                break;
            case "Death Mark":
                PerkManager.instance.deathMark = true;
                break;

            //Epic
            case "Frost Relic":
                PerkManager.instance.frostRelic++;
                break;
            case "Vampiric Blade":
                PerkManager.instance.vampiricBlade++;
                break;
            case "Tearstone Pendant":
                PerkManager.instance.tearstonePendant++;
                break;

            //Rare
            case "Whetstone":
                PerkManager.instance.whetstone++;
                break;
            case "Lucky Charm":
                PerkManager.instance.luckyCharm++;
                break;
            case "Serrated Blade":
                PerkManager.instance.serratedBlade++;
                break;

            //Negative
            case "Gravelord's Curse":
                PerkManager.instance.gravelordsCurse++;
                break;
            case "Bleeding Tendencies":
                PerkManager.instance.bleedingTendencies++;
                break;

            //Default
            default:
                Debug.LogWarning("Perk switch defaulted! Possible misspelling of: " + perkObject.perkName);
                break;
        }
    }
}