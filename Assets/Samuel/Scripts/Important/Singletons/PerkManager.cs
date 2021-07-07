using System.Collections;
using System.Collections.Generic;
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

    [Header("Legendary")]
    public int gravelordsBlessing;
    public int wiseMansJournal;

    [Header("Epic")]
    public int luckyCharm;

    [Header("Rare")]
    public int whetstone;

    [Header("Negative")]
    public int gravelordsCurse;
}
