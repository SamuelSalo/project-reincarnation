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
        GameObject player = Instantiate(Resources.Load("Characters/Player/PlayerSpider"), Vector2.zero, Quaternion.identity) as GameObject;
        gameManager.SetPlayer(player.GetComponent<Character>());
        gameUI.SetActive(true);
        gameObject.SetActive(false);
        Destroy(gameObject);
    }

    public void StartAsHuman()
    {
        Time.timeScale = 1f;
        var player = Instantiate(Resources.Load("Characters/Player/PlayerHuman"), Vector2.zero, Quaternion.identity) as GameObject;
        gameManager.SetPlayer(player.GetComponent<Character>());
        gameUI.SetActive(true);
        Destroy(gameObject);
    }
}
