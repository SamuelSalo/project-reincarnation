using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AIMovement))]
[RequireComponent(typeof(Character))]
public class AICombat : MonoBehaviour
{
    private AIMovement aiMovement;
    private Character character;

    private void Start()
    {
        character = GetComponent<Character>();
        aiMovement = GetComponent<AIMovement>();
    }

    public void Attack()
    {
        Debug.Log("Attack was called!");
        aiMovement.attacking = true;
        StartCoroutine(ResetAttackTest());
    }

    IEnumerator ResetAttackTest()
    {
        yield return new WaitForSeconds(1f);
        aiMovement.attacking = false;
        Debug.Log("Attack reset!");
    }
}
