using UnityEngine;
/// <summary>
/// Main menu UI functionality
/// </summary>
public class MainMenu : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject titlePanel;
    public GameObject settingsPanel;
    public GameObject tutorialPanel;

    

    public void TogglePanel()
    {
        titlePanel.SetActive(!titlePanel.activeSelf);
        settingsPanel.SetActive(!settingsPanel.activeSelf);
    }

    public void StartGame()
    {
        if(PlayerPrefs.HasKey("PlayTutorial"))
        {
            if(PlayerPrefs.GetInt("PlayTutorial") == 1)
            {
                TutorialPanel(true);
            }
            else if(PlayerPrefs.GetInt("PlayTutorial") == 0)
            {
                GameObject.FindWithTag("SceneChanger").GetComponent<SceneChanger>().FadeToScene(1);
            }
        }
        else
        {
            PlayerPrefs.SetInt("PlayTutorial", 1);
            StartGame();
        }
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartTutorial()
    {
        GameObject.FindWithTag("SceneChanger").GetComponent<SceneChanger>().FadeToScene(2);
        PlayerPrefs.SetInt("PlayTutorial", 0);
    }

    public void TutorialPanel(bool _active)
    {
        tutorialPanel.SetActive(_active);
    }

}