using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{

    public float damage;
    private float damageTimer;
    public SpriteRenderer SpriteRenderer;
    //public Sprite SpriteSpikesUp;

    void Start()
    {

    }

    void Update()
    {
        if(damageTimer > 0){
          damageTimer -= Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
      if(other.gameObject.tag == "Player")
      {
          //SpriteRenderer.sprite = SpriteSpikesUp;
          if(damageTimer <= 0){
              other.gameObject.GetComponent<Character>().health -= damage;
              damageTimer = 1;
          }
      }
    }
}
