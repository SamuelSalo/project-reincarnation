using UnityEngine;
using Faction = Character.Faction;

public class Finish : MonoBehaviour
{
    #region InputActions
    private PlayerControls playerControls;
    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();
            playerControls.Gameplay.Interact.performed += context => TryFinish();
        }

        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }
    #endregion

    public Faction faction;
    [Space]
    public GameObject gameUI;
    public GameObject finishUI;

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

    private void TryFinish()
    {
        if (reach)
        {
            if (gameManager.playerFaction != faction && room.cleared)
                FinishGame();
            else if(gameManager.playerFaction == faction)
                GameObject.FindWithTag("Tooltip").GetComponent<Tooltip>().ShowTooltip("Wrong direction! Go to the other end...", 2f);
            else if(gameManager.playerFaction != faction && !room.cleared)
                GameObject.FindWithTag("Tooltip").GetComponent<Tooltip>().ShowTooltip("Clear the room of enemies first!", 2f);
        }
    }

    private void FinishGame()
    {
        gameUI.SetActive(false);
        finishUI.SetActive(true);
        Time.timeScale = 0f;
        gameManager.canPause = false;
    }
}
