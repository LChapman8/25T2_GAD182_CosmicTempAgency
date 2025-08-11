using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    [Header("Timer UI")]
    public TextMeshProUGUI timeText;

    [Header("Score UI")]
    public TextMeshProUGUI scoreLabel;
    public TextMeshProUGUI scoreValue;

    [Header("Mistakes Made UI")]
    public GameObject mistakesMadeGroup; // parent group to toggle
    public TextMeshProUGUI mistakesMadeLabel;
    public TextMeshProUGUI mistakesMadeValue;

    [Header("Mistakes Left UI")]
    public GameObject mistakesLeftGroup; // parent group to toggle
    public TextMeshProUGUI mistakesLeftLabel;
    public TextMeshProUGUI mistakesLeftValue;

    [Header("Final Scorecard UI")]
    public GameObject scoreCardPanel;
    public TextMeshProUGUI baseScoreText;
    public TextMeshProUGUI gradeText;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI highScoreText;
    public GameObject lostStamp;

    private bool hasMistakeLimit = false;
    private int mistakeLimit = 0;
    private float remainingTime;
    private bool isGameActive = false;

    public void InitUI(float minigameTime, bool limitMistakes, int maxMistakes)
    {
        remainingTime = minigameTime;
        hasMistakeLimit = limitMistakes;
        mistakeLimit = maxMistakes;
        isGameActive = true;

        scoreCardPanel.SetActive(false);
        lostStamp.SetActive(false);

        // Toggle mistake display type
        mistakesMadeGroup.SetActive(!hasMistakeLimit);
        mistakesLeftGroup.SetActive(hasMistakeLimit);

        // Initialize values
        UpdateMistakeDisplay(0);
        UpdateScoreDisplay(0);

        ScoreManager.Instance.OnScoreChanged += UpdateScoreDisplay;
        ScoreManager.Instance.OnMistakeMade += UpdateMistakeDisplay;

        StartCoroutine(UpdateTimer());
    }

    private void UpdateScoreDisplay(int newScore)
    {
        scoreLabel.text = "Score:";
        scoreValue.text = newScore.ToString();
    }

    private void UpdateMistakeDisplay(int currentMistakes)
    {
        if (hasMistakeLimit)
        {
            mistakesLeftLabel.text = "Strikes Left:";
            mistakesLeftValue.text = Mathf.Max(0, mistakeLimit - currentMistakes).ToString();
        }
        else
        {
            mistakesMadeLabel.text = "Mistakes:";
            mistakesMadeValue.text = currentMistakes.ToString();
        }
    }

    private IEnumerator UpdateTimer()
    {
        while (isGameActive && remainingTime > 0f)
        {
            int minutes = Mathf.FloorToInt(remainingTime / 60f);
            int seconds = Mathf.FloorToInt(remainingTime % 60f);
            timeText.text = $"{minutes}:{seconds:00}";

            yield return null;
            remainingTime -= Time.deltaTime;
        }

        if (isGameActive)
        {
            EndGameUI();
        }
    }

    public void EndGameUI()
    {
        isGameActive = false;
        StopAllCoroutines();

        string grade = ScoreManager.Instance.GetFinalGrade();
        int baseScore = ScoreManager.Instance.GetBaseScore();
        float multiplier = ScoreManager.Instance.gradingProfile.GetMultiplier(grade);
        int finalScore = grade == "F" ? 0 : Mathf.RoundToInt(baseScore * multiplier);

        MinigameID id = ScoreManager.Instance.currentMinigame;
        int best = ScoreSaveManager.GetBestScore(id);
        string bestGrade = ScoreSaveManager.GetBestGrade(id);

        scoreCardPanel.SetActive(true);
        StartCoroutine(AnimateScoreCard(baseScore, grade, finalScore, best, bestGrade));
    }

    private IEnumerator AnimateScoreCard(int baseScore, string grade, int finalScore, int bestScore, string bestGrade)
    {
        baseScoreText.text = $"Base Score: {baseScore}";
        yield return new WaitForSeconds(1f);

        gradeText.text = $"Grade: {grade}";
        yield return new WaitForSeconds(1f);

        finalScoreText.text = $"Final Score: {finalScore}";
        yield return new WaitForSeconds(1f);

        highScoreText.text = $"High Score: {bestScore} ({bestGrade})";
        yield return new WaitForSeconds(1f);

        if (grade == "F")
        {
            lostStamp.SetActive(true);
        }
    }

    private void OnDisable()
    {
        ScoreManager.Instance.OnScoreChanged -= UpdateScoreDisplay;
        ScoreManager.Instance.OnMistakeMade -= UpdateMistakeDisplay;
    }
}
