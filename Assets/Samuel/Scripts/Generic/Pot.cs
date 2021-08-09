using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void BreakPot()
    {
        animator.SetTrigger("Break");

        var rng = Random.Range(0, 101);

        if (rng >= 90) InventoryManager.instance.GiveTokens(200);
        else if (rng < 90 && rng >= 60) InventoryManager.instance.GiveTokens(100);
        else InventoryManager.instance.GiveTokens(50);
    }

    /// <summary>
    /// Called by animation to delete object
    /// </summary>
    public void PotBroken()
    {
        Destroy(gameObject);
    }
}
