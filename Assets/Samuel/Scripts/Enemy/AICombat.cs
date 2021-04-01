using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AIMovement))]
[RequireComponent(typeof(Character))]
public class AICombat : MonoBehaviour
{
    /// <summary>
    /// WIP TEST CODE
    /// TODO DELETE & REFACTOR
    /// </summary>
    private AIMovement aiMovement;
    private Character character;

    private void Start()
    {
        character = GetComponent<Character>();
        aiMovement = GetComponent<AIMovement>();
    }

    public void Attack()
    {
        aiMovement.state = AIMovement.State.Attacking;
        StartCoroutine(ResetAttackTest());
    }

    
    IEnumerator ResetAttackTest()
    {
        yield return new WaitForSeconds(1f);
        aiMovement.state = AIMovement.State.Chasing;
    }
}
