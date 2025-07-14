using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject startPanel;
    public GameObject gamePanel;

    void Start()
    {
        ShowStartMenu();
    }

    public void StartGame()
    {
        startPanel.SetActive(false);
        gamePanel.SetActive(true);
    }

    public void ShowStartMenu()
    {
        startPanel.SetActive(true);
        gamePanel.SetActive(false);
    }
}
