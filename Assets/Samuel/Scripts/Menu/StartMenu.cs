using UnityEngine;

public class StartMenu : MonoBehaviour
{
    public GameObject gameUI;
    public GameManager gameManager;

    private void OnEnable()
    {
        Time.timeScale = 0f;
    }
    public void StartAsMonster()
    {
        Time.timeScale = 1f;
        GameObject player;
        var rng = Random.Range(1, 3);
        if(rng == 1)
            player = Instantiate(Resources.Load("Characters/Player/Monster/PlayerSpider"), Vector2.zero, Quaternion.identity) as GameObject;
        else if (rng == 2)
            player = Instantiate(Resources.Load("Characters/Player/Monster/PlayerRoach"), Vector2.zero, Quaternion.identity) as GameObject;
        else if (rng == 3)
            player = Instantiate(Resources.Load("Characters/Player/Monster/PlayerAnt"), Vector2.zero, Quaternion.identity) as GameObject;

        else player = Instantiate(Resources.Load("Characters/Player/Monster/PlayerSpider"), Vector2.zero, Quaternion.identity) as GameObject;

        gameManager.SetPlayer(player.GetComponent<Character>());
        gameUI.SetActive(true);
        gameObject.SetActive(false);
        Destroy(gameObject);
    }

    public void StartAsHuman()
    {
        Time.timeScale = 1f;
        var player = Instantiate(Resources.Load("Characters/Player/Human/PlayerHuman"), Vector2.zero, Quaternion.identity) as GameObject;
        gameManager.SetPlayer(player.GetComponent<Character>());
        gameUI.SetActive(true);
        Destroy(gameObject);
    }
}
