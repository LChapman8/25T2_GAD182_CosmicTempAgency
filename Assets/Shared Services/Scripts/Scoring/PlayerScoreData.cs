using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScoreData
{
    public Dictionary<MinigameID, int> currentScores = new();
    public Dictionary<MinigameID, string> currentGrades = new();

    public void SetScore(MinigameID id, int score, string grade)
    {
        currentScores[id] = score;
        currentGrades[id] = grade;
    }

    public int GetScore(MinigameID id) => currentScores.ContainsKey(id) ? currentScores[id] : 0;
    public string GetGrade(MinigameID id) => currentGrades.ContainsKey(id) ? currentGrades[id] : "-";
}
