using UnityEngine;

/// <summary>
/// Pause UI functionality
/// </summary>
public class PauseMenu : MonoBehaviour
{
    #region InputActions
    private PlayerControls playerControls;
    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();
            playerControls.Gameplay.Pause.performed += context => TogglePause();
        }

        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }
    #endregion

    private bool paused;

    [Header("Canvases")]
    public GameObject pauseUi;
    public GameObject gameUi;

    [Space] [Header("UI Panels")]
    public GameObject mainPanel;
    public GameObject settingsPanel;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = GetComponent<GameManager>();
    }

    public void TogglePause()
    {
        if (!paused && !gameManager.canPause) return;

        paused = !paused;
        gameManager.canPause = !gameManager.canPause;
        Time.timeScale = paused ? 0f : 1f;
        pauseUi.SetActive(!pauseUi.activeSelf);
        gameUi.SetActive(!gameUi.activeSelf);
    }

    public void QuitToMenu()
    {
        GameObject.FindWithTag("SceneChanger").GetComponent<SceneChanger>().FadeToScene(0);
    }

    public void TogglePanels()
    {
        mainPanel.SetActive(!mainPanel.activeSelf);
    }
}
