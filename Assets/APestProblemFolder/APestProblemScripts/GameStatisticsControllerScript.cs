using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameStatisticsControllerScript : MonoBehaviour
{
    public TimerScript timerScript;
    public UIController uiController;
    public ScoreManager scoreManager;

    public int pestsRemoved = 0;
    public int pestsAlive = 5;

    public float timeLeft;
    public bool gameEnded = false;

    [Header("Scoring Settings")]
    public int pointsPerPest = 10;
    public bool failOnRemainingPests = true;

    void Update()
    {
        // Time runs out
        if (timeLeft <= 0 && !gameEnded)
        {
            EndGame();
            Debug.Log("Game ended: time out");
        }

        // Player wins
        if (timeLeft >= 0.1f && pestsRemoved == 5 && !gameEnded)
        {
            EndGame();
            Debug.Log("Game ended: all pests removed");
        }
    }

    public void RegisterPestRemoved()
    {
        if (gameEnded) return;

        pestsRemoved++;
        pestsAlive = Mathf.Max(0, pestsAlive - 1);

        scoreManager.AddScore(pointsPerPest);
        ScoreManager.Instance.OnMistakeMade?.Invoke(pestsRemoved);
    }

    public void EndGame()
    {
        if (gameEnded) return;
        gameEnded = true;
        timerScript.TurnTimerOff();

        if (failOnRemainingPests)
        {
            for (int i = 0; i < pestsAlive; i++)
                ScoreManager.Instance.RegisterMistake();
        }

        ScoreManager.Instance.FinaliseGame();
        uiController.EndGameUI();
    }
}
