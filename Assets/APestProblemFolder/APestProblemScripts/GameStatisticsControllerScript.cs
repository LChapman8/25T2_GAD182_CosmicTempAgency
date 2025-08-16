using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameStatisticsControllerScript : MonoBehaviour
{
    public ScoreManager scoreManager;
    public GameOverScreenScript gameOverScreenScript;
    public TimerScript timerScript;

    public int pestsRemoved;
    public int pestsAlive = 5;
    public float timeLeft;
    public float totalScore;

    public float roundedTotalScore;
    public float roundedTimeLeft;

    public bool gameEnded = false;

    //endscreen texts

    public TextMeshProUGUI totalScoreText;
    public TextMeshProUGUI timeLeftText;
    public TextMeshProUGUI pestsRemovedText;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI finalGradeText;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (timeLeft <= 0 && gameEnded == false)
        {
            timerScript.TurnTimerOff();
            FinalScores();
            gameOverScreenScript.ActivateEndScreen();
            gameEnded = true;
            Debug.Log("Game end due to timeout");

        }

        if (timeLeft >= 0.1 && pestsRemoved == 5 && gameEnded == false)
        {
            timerScript.TurnTimerOff();
            FinalScores();
            gameOverScreenScript.ActivateEndScreen();
            gameEnded = true;
            Debug.Log("Game end due to pest removal");
        }
    }

    public void FinalScores()
    { 
        roundedTimeLeft = Mathf.RoundToInt(timeLeft);

        totalScore = (pestsRemoved * 8) + timeLeft ;

        roundedTotalScore = Mathf.RoundToInt(totalScore);

        pestsRemovedText.text = "Pests Removed : " + pestsRemoved;
        timeLeftText.text = "Time Left : " + roundedTimeLeft;
        totalScoreText.text = "Total Score : " + roundedTotalScore;


        //Notify ben's score controller of mistakes and score
        scoreManager.AddScore((int)totalScore);

        Debug.Log((int)totalScore);

        for (int i = 0; i < pestsAlive; i++)
        {
            scoreManager.RegisterMistake();
        }

        scoreManager.GetFinalGrade();

        var result = scoreManager.EndGame();

        finalScoreText.text = "Final Score : " + result.finalScore;
        finalGradeText.text = "Final Grade : " + result.finalGrade;




    }
}
