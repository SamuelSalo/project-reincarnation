using UnityEngine;
using Faction = Character.Faction;

public class Remains : MonoBehaviour
{
    [HideInInspector] public Faction faction;
    [Range(0,100)] public float restoreAmount;
    private Character character;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            character = collision.GetComponent<Character>();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            character = null;
        }
    }

    private void Update()
    {
        if (character && Input.GetKeyDown(KeyCode.E))
            if (character.faction == Faction.Red && faction == Faction.Blue)
            {
                character.RestoreHealth(restoreAmount);
                Destroy(gameObject);
            }
            else if (character.faction == Faction.Blue)
                return;
            else if (character.faction == Faction.Red && faction == Faction.Red)
                GameObject.FindWithTag("Tooltip").GetComponent<Tooltip>().ShowTooltip("You can only devour human remains!", 2f);
    }
}
