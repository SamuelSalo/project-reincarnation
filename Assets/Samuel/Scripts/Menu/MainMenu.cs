using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        StartCoroutine(StartGameRoutine());
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator StartGameRoutine()
    {
        GameObject.FindWithTag("FaderOverlay").GetComponent<FaderOverlay>().FadeOut();
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(1);
    }
}