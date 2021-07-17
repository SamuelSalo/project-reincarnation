using UnityEngine;
using System.Collections.Generic;

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

    public Dictionary<string, int> perkDictionary;
    private PerkObject[] negatives, rares, epics, legendaries;

    private void Start()
    {
        perkDictionary = new Dictionary<string, int>();
        PerkLoader.LoadPerks(ref negatives, ref rares, ref epics, ref legendaries);

        foreach (PerkObject p in negatives) { perkDictionary.Add(p.perkName, 0); }
        foreach (PerkObject p in rares) { perkDictionary.Add(p.perkName, 0); }
        foreach (PerkObject p in epics) { perkDictionary.Add(p.perkName, 0); }
        foreach (PerkObject p in legendaries) { perkDictionary.Add(p.perkName, 0); }
    }

    public void Activate(PerkObject _perk)
    {
        Debug.Log(_perk.perkName);

        if (perkDictionary.ContainsKey(_perk.name))
        {
            perkDictionary[_perk.perkName]++;
        }
        else
        {
            Debug.LogWarning("PerkDictionary does not contain " + _perk.perkName);
        }
    }
}
