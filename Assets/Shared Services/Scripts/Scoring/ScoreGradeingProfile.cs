using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Score System/Score Grading Profile")]
public class ScoreGradingProfile : ScriptableObject
{
    [System.Serializable]
    public class GradeThreshold
    {
        public string grade;          // e.g., "A+", "A", "B", etc.
        public int minScore;          // Minimum base score required
        public int maxMistakes;       // Max mistakes allowed for this grade
    }

    public List<GradeThreshold> gradeThresholds = new List<GradeThreshold>();

    /// <summary>
    /// Returns the appropriate grade for the given score and mistake count.
    /// </summary>
    public string GetGrade(int score, int mistakes)
    {
        foreach (var threshold in gradeThresholds)
        {
            if (score >= threshold.minScore && mistakes <= threshold.maxMistakes)
                return threshold.grade;
        }

        return "F"; // Default to F if no thresholds are met
    }
}
