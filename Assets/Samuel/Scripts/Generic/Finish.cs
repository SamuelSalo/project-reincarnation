using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Faction = Character.Faction;

public class Finish : MonoBehaviour
{
    public Faction faction;
    private Room room;
    private GameManager gameManager;
    private bool reach;

    private void Start()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        room = transform.parent.GetComponent<Room>();
    }
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
        //Allow player to backtrack freely and roam across cleared/own faction rooms, but require room to be cleared to advance
        if (reach && Input.GetKeyDown(KeyCode.E) && gameManager.currentFaction != faction && room.cleared)
            FinishGame();
    }

    private void FinishGame()
    {
        //todo
        print("finished game, gg");
    }
}
