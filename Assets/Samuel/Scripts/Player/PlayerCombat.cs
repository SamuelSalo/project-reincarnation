using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class PlayerCombat : MonoBehaviour
{
    // Test code, to be completely or partially revamped

    private Character character;
    private float timer;

    private void Start()
    {
        character = GetComponent<Character>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F) && Time.time >= timer)
        {
            Debug.Log("Player attacked");
            timer = Time.time + 1f / character.attackRate;

            var hits = Physics2D.RaycastAll(transform.position, transform.up, 2f);
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.transform.CompareTag("Enemy"))
                    character.DealDamage(character.damage, hit.transform.GetComponent<Character>());
            }
        }
    }
}
