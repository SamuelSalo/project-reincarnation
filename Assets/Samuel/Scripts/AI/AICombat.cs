using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AIMovement))]
[RequireComponent(typeof(Character))]
public class AICombat : MonoBehaviour
{
    // Test code, to be completely/partially revamped.

    private AIMovement aiMovement;
    private Character character;
    private float timer;

    private void Start()
    {
        character = GetComponent<Character>();
        aiMovement = GetComponent<AIMovement>();
    }

    public void Attack()
    {
        if (Time.time < timer) return;

        timer = Time.time + 1f / character.attackRate;

        var hits = Physics2D.RaycastAll(transform.position, transform.up, 2f);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.transform.CompareTag("Player"))
                character.DealDamage(character.damage, hit.transform.GetComponent<Character>());
        }

        StartCoroutine(ResetAttackTest());
    }

    
    IEnumerator ResetAttackTest()
    {
        yield return new WaitForSeconds(1f/character.attackRate);
        aiMovement.state = AIMovement.State.Chasing;
    }
}
