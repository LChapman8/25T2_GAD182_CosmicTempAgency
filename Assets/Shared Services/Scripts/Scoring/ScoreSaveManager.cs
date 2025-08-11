using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSaveManager : MonoBehaviour
{
    private const string ScoreKeyPrefix = "BestScore_";
    private const string GradeKeyPrefix = "BestGrade_";

    public static void SaveBestScore(MinigameID id, int score, string grade)
    {
        int prevBest = GetBestScore(id);

        if (score > prevBest)
        {
            PlayerPrefs.SetInt(ScoreKeyPrefix + id, score);
            PlayerPrefs.SetString(GradeKeyPrefix + id, grade);
            PlayerPrefs.Save();
        }
    }

    public static int GetBestScore(MinigameID id)
    {
        return PlayerPrefs.GetInt(ScoreKeyPrefix + id, 0);
    }

    public static string GetBestGrade(MinigameID id)
    {
        return PlayerPrefs.GetString(GradeKeyPrefix + id, "-");
    }

    public static void ClearAllData()
    {
        foreach (MinigameID id in System.Enum.GetValues(typeof(MinigameID)))
        {
            PlayerPrefs.DeleteKey(ScoreKeyPrefix + id);
            PlayerPrefs.DeleteKey(GradeKeyPrefix + id);
        }
    }
}
