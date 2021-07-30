using UnityEngine;
using System.Collections.Generic;

public class SpikeTrap : MonoBehaviour
{
    private bool active;
    private Animator animator;
    public float damage;
    public List<Collider2D> contactList;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.transform.CompareTag("Player") || collision.transform.CompareTag("AI"))&& !collision.isTrigger && !active)
        {
            Activate();
            contactList.Add(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((collision.transform.CompareTag("Player") || collision.transform.CompareTag("AI")) && contactList.Contains(collision))
            contactList.Remove(collision);
    }

    private void Activate()
    {
        active = true;
        animator.SetTrigger("activate");
    }

    public void SpikeTrapAnimatorCallback()
    {
        foreach(Collider2D contact in contactList)
        {
            //TODO trap damage reducing perk
            contact.GetComponent<Character>().TakeDamage(damage);
        }
        active = false;
    }
}
