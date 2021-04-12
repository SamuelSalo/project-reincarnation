using System.Collections;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    private FaderOverlay fader;
    private GameManager gameManager;

    public Transform targetLocation;
    public Transform npcs;

    private bool reach;
    private bool teleporting;

    private void Start()
    {
        fader = GameObject.FindWithTag("FaderOverlay").GetComponent<FaderOverlay>();
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
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
        if (reach && Input.GetKeyDown(KeyCode.E) && npcs.childCount == 0 && !teleporting)
            StartCoroutine(TeleportPlayer());
    }

    private IEnumerator TeleportPlayer()
    {
        teleporting = true;
        gameManager.currentCharacter.playerMovement.teleporting = teleporting;
        fader.FadeOut();

        yield return new WaitForSeconds(1f);
        gameManager.currentCharacter.transform.position = targetLocation.position;

        
        teleporting = false;
        gameManager.currentCharacter.playerMovement.teleporting = teleporting;
        fader.FadeIn();
    }
}
