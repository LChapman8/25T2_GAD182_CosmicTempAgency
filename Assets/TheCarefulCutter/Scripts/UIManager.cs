using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject startPanel;
    public GameObject gamePanel;
    public GameObject gameOverPanel;
    public ChopManager chopManager;

    void Start()
    {
        ShowStartMenu();
        Time.timeScale = 0f; 
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
    }

    public void StartGame()
    {
        startPanel.SetActive(false);
        gamePanel.SetActive(true);
        gameOverPanel.SetActive(false);

        Time.timeScale = 1f;
        chopManager.enabled = true;
    }

    public void ShowStartMenu()
    {
        startPanel.SetActive(true);
        gamePanel.SetActive(false);
        gameOverPanel.SetActive(false);

        chopManager.enabled = false;
    }
}
