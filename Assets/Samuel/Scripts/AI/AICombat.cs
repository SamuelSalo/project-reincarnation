using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AIMovement))]
[RequireComponent(typeof(Character))]
public class AICombat : MonoBehaviour
{
    private AIMovement aiMovement;
    private Character character;

    private float timer;

    private void Start()
    {
        character = GetComponent<Character>();
        aiMovement = GetComponent<AIMovement>();
    }

    /// <summary>
    /// AIMovement calls this to attack when in range.
    /// </summary>
    public void Attack()
    {
        if (Time.time < timer) return;

        timer = Time.time + 1f / character.attackRate;
        character.animator.SetTrigger("Attack");
        StartCoroutine(ResetAttack());
    }

    /// <summary>
    /// Reset AI after attack.
    /// </summary>
    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(1f/character.attackRate);
        aiMovement.state = AIMovement.State.Chasing;
    }

    /// <summary>
    /// Activates a temporary hurtbox and deals damage to all hostile characters inside it.
    /// </summary>
    public void ActivateAttackHurtbox()
    {
        var hits = Physics2D.OverlapBoxAll(transform.position + transform.up, new Vector2(1f, 1f), 0f);
        if (hits.Length != 0)
        {
            foreach (Collider2D hit in hits)
            {
                if (hit.transform.CompareTag("Player") && hit.transform.GetComponent<Character>().faction != character.faction)
                    character.DealDamage(character.damage, hit.transform.GetComponent<Character>());
            }
        }
    }
}