using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    private bool reach, active;
    private Animator animator;
    public float damage;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player") && !collision.isTrigger && !active)
        {
            reach = true;
            Activate();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player") && !collision.isTrigger && active)
        {
            reach = false;
        }
    }

    private void Activate()
    {
        active = true;
        animator.SetTrigger("activate");
    }

    public void SpikeTrapAnimatorCallback()
    {
        if(reach)
        {
            Character target = GameManager.instance.playerCharacter;

            //TODO character trap dmg handlers
            target.TakeDamage(damage);
        }

        active = false;
        reach = false;
    }
}
