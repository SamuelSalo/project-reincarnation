using UnityEngine;

public class StartMenu : MonoBehaviour
{
    public GameObject gameUI;

    private void OnEnable()
    {
        Time.timeScale = 0f;
    }
    public void StartAsMonster()
    {
        Time.timeScale = 1f;
        GameObject player = Instantiate(Resources.Load("Characters/Player/Pyramidhead"), Vector2.zero, Quaternion.identity) as GameObject;
        GameManager.instance.SetPlayer(player.GetComponent<Character>());
        gameUI.SetActive(true);
        gameObject.SetActive(false);
        Destroy(gameObject);
        GameManager.instance.canPause = true;
    }

    public void StartAsHuman()
    {
        Time.timeScale = 1f;
        var player = Instantiate(Resources.Load("Characters/Player/Knight"), Vector2.zero, Quaternion.identity) as GameObject;
        GameManager.instance.SetPlayer(player.GetComponent<Character>());
        gameUI.SetActive(true);
        Destroy(gameObject);
        GameManager.instance.canPause = true;
    }
}
