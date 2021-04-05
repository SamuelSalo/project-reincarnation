using UnityEngine;

public class Healthorb : MonoBehaviour
{
    [Range(0,100)] public float restoreAmount;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.GetComponent<Character>().RestoreHealth(restoreAmount);
            Destroy(gameObject);
        }
    }
}
