using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public GameObject gameOverPanel;
    public TextMeshProUGUI resultText;

    void Start()
    {
        UpdateScore(0);
        gameOverPanel.SetActive(false);
    }

    public void UpdateScore(int newScore)
    {
        scoreText.text = "Score: " + newScore;
    }

    public void GameOver(bool win)
    {
        gameOverPanel.SetActive(true);
        resultText.text = win ? "You Win!" : "You Injured Yourself!";
    }
}
