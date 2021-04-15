using UnityEngine;

[RequireComponent(typeof(Character))]
public class PlayerCombat : MonoBehaviour
{
    private Character character;
    private GameSFX gameSFX;

    private float timer;

    private void Start()
    {
        character = GetComponent<Character>();
        gameSFX = Camera.main.GetComponent<GameSFX>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F) && Time.time >= timer)
        {
            timer = Time.time + 1f / character.attackRate;

            character.animator.SetTrigger("Attack");
        }
    }

    /// <summary>
    /// Activates a temporary hurtbox and deals damage to all hostile characters inside it.
    /// </summary>
    public void ActivateAttackHurtbox()
    {
        gameSFX.PlaySlashSFX();
        var hits = Physics2D.OverlapBoxAll(transform.position + transform.up, new Vector2(1f, 1f), 0f);
        if(hits.Length != 0)
        {
            foreach (Collider2D hit in hits)
            {
                if (hit.transform.CompareTag("AI") && hit.transform.GetComponent<Character>().faction != character.faction)
                    character.DealDamage(character.damage, hit.transform.GetComponent<Character>());
            }
        }
    }
}