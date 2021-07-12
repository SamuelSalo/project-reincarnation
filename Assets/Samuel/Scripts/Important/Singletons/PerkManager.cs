using UnityEngine;

public class PerkManager : MonoBehaviour
{
    #region Singleton

    public static PerkManager instance;

    private void Awake()
    {
        if (instance)
        {
            Debug.LogWarning("Multiple instances of " + name + "found!");
            return;
        }
        instance = this;
    }

    #endregion

    public bool DeathMark(Character _target)
    {
        if(_target == deathMarkTarget)
        {
            deathMarkCount++;

            if (deathMarkCount >= 3)
                return true;

            else return false;
        }
        else
        {
            deathMarkTarget = _target;
            deathMarkCount = 1;

            return false;
        }
    }

    private int deathMarkCount;
    private Character deathMarkTarget;

    [Header("Legendary")]
    public int gravelordsBlessing;
    public int wiseMansJournal;
    public bool deathMark;
    public int thornmailArmor;

    [Header("Epic")]
    public int vampiricBlade;
    public int frostRelic;
    public int tearstonePendant;

    [Header("Rare")]
    public int whetstone;
    public int luckyCharm;
    public int serratedBlade;

    [Header("Negative")]
    public int gravelordsCurse;
    public int bleedingTendencies;

    public void Activate(PerkObject _perkObject)
    {
        if (!_perkObject)
        {
            Debug.LogWarning("Tried to activate perk without perk object!");
            return;
        }

        Debug.Log(_perkObject.perkName);
        switch (_perkObject.perkName)
        {
            //Legendary
            case "Gravelord's Blessing":
                gravelordsBlessing++;
                break;
            case "Wise Man's Journal":
                wiseMansJournal++;
                break;
            case "Thornmail Armor":
               thornmailArmor++;
                break;
            case "Death Mark":
                deathMark = true;
                break;

            //Epic
            case "Frost Relic":
               frostRelic++;
                break;
            case "Vampiric Blade":
                vampiricBlade++;
                break;
            case "Tearstone Pendant":
                tearstonePendant++;
                break;

            //Rare
            case "Whetstone":
                whetstone++;
                break;
            case "Lucky Charm":
                luckyCharm++;
                break;
            case "Serrated Blade":
                serratedBlade++;
                break;

            //Negative
            case "Gravelord's Curse":
                gravelordsCurse++;
                break;
            case "Bleeding Tendencies":
                bleedingTendencies++;
                break;

            //Default
            default:
                Debug.LogWarning("Perk switch defaulted! Possible misspelling of: " + _perkObject.perkName);
                break;
        }
    }
}
