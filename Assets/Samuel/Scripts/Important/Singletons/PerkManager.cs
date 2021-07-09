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
}
