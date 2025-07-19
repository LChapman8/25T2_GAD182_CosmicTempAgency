using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public ScoreGradingProfile gradingProfile;

    private int baseScore = 0;
    private int mistakeCount = 0;
    private bool gameEnded = false;

    public Action<int> OnScoreChanged;
    public Action<int> OnMistakeMade;

    private void Awake()
    {
        // Singleton setup for easy access
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    public void ResetScore()
    {
        baseScore = 0;
        mistakeCount = 0;
        gameEnded = false;
    }

    /// <summary>
    /// Call this to add score from an in-game event.
    /// </summary>
    public void AddScore(int points)
    {
        if (gameEnded) return;

        baseScore += points;
        OnScoreChanged?.Invoke(baseScore);
    }

    /// <summary>
    /// Call this when the player makes a mistake.
    /// </summary>
    public void RegisterMistake()
    {
        if (gameEnded) return;

        mistakeCount++;
        OnMistakeMade?.Invoke(mistakeCount);
    }

    /// <summary>
    /// Returns the player's final grade based on current base score and mistakes.
    /// </summary>
    public string GetFinalGrade()
    {
        return gradingProfile.GetGrade(baseScore, mistakeCount);
    }

    /// <summary>
    /// Ends the game and returns final score and grade.
    /// </summary>
    public (int finalScore, string finalGrade) EndGame()
    {
        gameEnded = true;

        if (GetFinalGrade() == "F")
            baseScore = 0; // If player failed, set score to 0

        return (baseScore, GetFinalGrade());
    }
}
