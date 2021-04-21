using UnityEngine;
/// <summary>
/// Main menu UI functionality
/// </summary>
public class MainMenu : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject titlePanel;
    public GameObject settingsPanel;

    public void TogglePanel()
    {
        titlePanel.SetActive(!titlePanel.activeSelf);
        settingsPanel.SetActive(!settingsPanel.activeSelf);
    }

    public void StartGame()
    {
        GameObject.FindWithTag("SceneChanger").GetComponent<SceneChanger>().FadeToScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}