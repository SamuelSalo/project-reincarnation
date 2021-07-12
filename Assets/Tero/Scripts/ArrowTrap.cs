using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTrap : MonoBehaviour
{
    public Transform ArrowSpawn;
    public GameObject ArrowPrefab;


    void Start()
    {

    }


    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if(other.gameObject.tag == "Player")
        {
            Instantiate(ArrowPrefab, ArrowSpawn.position, Quaternion.identity);

        }

    }
}
