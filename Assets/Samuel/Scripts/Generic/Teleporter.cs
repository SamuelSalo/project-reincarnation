using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public Transform targetLocation;
    public Transform npcs;

    private bool reach;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            reach = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            reach = false;
    }

    private void Update()
    {
        if (reach && Input.GetKeyDown(KeyCode.E) && npcs.childCount == 0)
            GameObject.FindWithTag("Player").transform.position = targetLocation.position;
    }
}
