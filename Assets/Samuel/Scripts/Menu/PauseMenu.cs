using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PauseMenu : MonoBehaviour
{
    private bool paused;

    [Header("Canvases")]
    public GameObject pauseUi;
    public GameObject gameUi;

    [Space] [Header("UI Panels")]
    public GameObject mainPanel;
    public GameObject settingsPanel;
    
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (!paused) Pause();
        }
    }

    public void Resume()
    {
        paused = false;
        Time.timeScale = 1f;
        pauseUi.SetActive(false);
        gameUi.SetActive(true);
    }

    private void Pause()
    {
        paused = true;
        Time.timeScale = 0f;
        pauseUi.SetActive(true);
        gameUi.SetActive(false);
    }

    public void QuitToMenu()
    {
        StartCoroutine(QuitCoroutine());
    }

    private IEnumerator QuitCoroutine()
    {
        GameObject.FindWithTag("FaderOverlay").GetComponent<FaderOverlay>().FadeOut();
        yield return new WaitForSecondsRealtime(1f);
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }

    public void TogglePanels()
    {
        mainPanel.SetActive(!mainPanel.activeSelf);
        settingsPanel.SetActive(!settingsPanel.activeSelf);
    }
}
