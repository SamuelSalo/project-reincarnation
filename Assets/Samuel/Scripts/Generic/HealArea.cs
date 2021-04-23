using UnityEngine;

public class HealArea : MonoBehaviour
{
    private Character target;

    [Header("Health Restoration Settings")]
    public float restoration;
    public float restoreSpeed;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            target = collision.GetComponent<Character>();
            InvokeRepeating(nameof(Restore), 0f, restoreSpeed);
        }
            
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            target = null;
            if (IsInvoking(nameof(Restore)))
                CancelInvoke(nameof(Restore));
        }
            
    }

    private void Restore()
    {
        if (!target) return;

        if(target.health != target.maxHealth && target.player.moveDirection == Vector2.zero && target.faction == Character.Faction.Blue)
            target.RestoreHealth(restoration);
    }
}
