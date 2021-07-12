using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonTrap : MonoBehaviour
{
    public float speedDecrease;
    public float damage;
    public float tick;
    private float timer;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
      if(other.gameObject.tag == "Player")
      {
          other.gameObject.GetComponent<Character>().movementSpeed -= speedDecrease;

          while(tick >= 0 && timer >= 0){
              other.gameObject.GetComponent<Character>().health -= damage;
              tick--;
              timer = 0.5f;
          }
      }
    }
}
