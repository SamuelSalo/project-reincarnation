using UnityEngine;

/// <summary>
/// Pause UI functionality
/// </summary>
public class PauseMenu : MonoBehaviour
{
    private PlayerControls playerControls;
    private bool paused;

    [Header("Canvases")]
    public GameObject pauseUi;
    public GameObject gameUi;

    [Space] [Header("UI Panels")]
    public GameObject mainPanel;
    public GameObject settingsPanel;

    private void OnEnable()
    {
        if (playerControls == null)
            playerControls = new PlayerControls();

        playerControls.Gameplay.Pause.performed += context => TryPause();
    }

    private void TryPause()
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
        GameObject.FindWithTag("SceneChanger").GetComponent<SceneChanger>().FadeToScene(0);
    }

    public void TogglePanels()
    {
        mainPanel.SetActive(!mainPanel.activeSelf);
        settingsPanel.SetActive(!settingsPanel.activeSelf);
    }
}
