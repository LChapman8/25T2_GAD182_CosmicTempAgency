using UnityEngine;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenuPanel;
    public Button quitButton;

    private bool isPaused = false;

    void Start()
    {
        pauseMenuPanel.SetActive(false); 
        quitButton.onClick.AddListener(QuitGame);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    void TogglePauseMenu()
    {
        isPaused = !isPaused;
        pauseMenuPanel.SetActive(isPaused);

        // Optional: freeze game while paused
        Time.timeScale = isPaused ? 0f : 1f;
    }

    void QuitGame()
    {
        Debug.Log("Quit button pressed");
        Application.Quit();

        
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
