using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    [Header("Live Game UI")]
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI mistakeText;

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

    public void InitUI(float minigameTime, bool hasLimit, int limit)
    {
        remainingTime = minigameTime;
        hasMistakeLimit = hasLimit;
        mistakeLimit = limit;
        isGameActive = true;

        scoreCardPanel.SetActive(false);
        lostStamp.SetActive(false);

        UpdateMistakeDisplay(0);
        UpdateScoreDisplay(0);

        ScoreManager.Instance.OnScoreChanged += UpdateScoreDisplay;
        ScoreManager.Instance.OnMistakeMade += UpdateMistakeDisplay;

        StartCoroutine(UpdateTimer());
    }

    private void UpdateScoreDisplay(int newScore)
    {
        scoreText.text = $"Score: {newScore}";
    }

    private void UpdateMistakeDisplay(int mistakes)
    {
        if (hasMistakeLimit)
        {
            mistakeText.text = $"Strikes Left: {mistakeLimit - mistakes}";
        }
        else
        {
            mistakeText.text = $"Mistakes: {mistakes}";
        }
    }

    private IEnumerator UpdateTimer()
    {
        while (isGameActive && remainingTime > 0)
        {
            timeText.text = $"Time Left: {Mathf.CeilToInt(remainingTime)}s";
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
