using UnityEngine;
/// <summary>
/// Death/Game loss UI functionality
/// </summary>
public class DeathMenu : MonoBehaviour
{
    public void QuitToMenu()
    {
        GameObject.FindWithTag("SceneChanger").GetComponent<SceneChanger>().FadeToScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Retry()
    {
        GameObject.FindWithTag("SceneChanger").GetComponent<SceneChanger>().FadeToScene(1);
    }
}
