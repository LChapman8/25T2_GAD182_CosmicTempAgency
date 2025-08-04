using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameStatisticsControllerScript : MonoBehaviour
{
    public GameOverScreenScript gameOverScreenScript;
    public TimerScript timerScript;

    public int pestsRemoved;
    public float timeLeft;
    public float totalScore;

    public float roundedTotalScore;
    public float roundedTimeLeft;

    public bool gameEnded = false;

    //endscreen texts

    public TextMeshProUGUI totalScoreText;
    public TextMeshProUGUI timeLeftText;
    public TextMeshProUGUI pestsRemovedText;

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

        if (timeLeft >= 0.1 && pestsRemoved == 4 && gameEnded == false)
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

        totalScore = (pestsRemoved * 3) + timeLeft;

        roundedTotalScore = Mathf.RoundToInt(totalScore);

        pestsRemovedText.text = "Pests Removed : " + pestsRemoved;
        timeLeftText.text = "Time Left : " + roundedTimeLeft;
        totalScoreText.text = "Total Score : " + roundedTotalScore;

    }
}
