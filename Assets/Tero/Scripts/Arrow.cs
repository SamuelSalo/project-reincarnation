using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float ArrowMoveSpeed;
    public float ArrowDamage;

    public Transform Target;

    void Start()
    {
        Target = GameObject.FindWithTag("Player").transform;

    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, Target.position, ArrowMoveSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<Character>().health -= ArrowDamage;
            Destroy(gameObject);
        }
    }
}
