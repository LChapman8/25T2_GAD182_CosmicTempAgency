using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameControllerScript : MonoBehaviour
{

    public ScoreManager scoreManager;

    public FruitCaughtTracker fruitCaughtTrackerScript;
    public GameObject fruitTrackerObject;

    public FruitSpawner fruitSpawnerScript;
    public GameObject fruitSpawnerObject;


    public TimerScript timerScript;
    public GameObject timerObject;
    public TimerSoundScript timerSoundScript;

    public SoundByteScript soundByteScript;
    public GameObject soundByteObject;

    public FloorHitbox floorHitbox;
    public GameObject floorHitboxObject;

    public GameObject winScreen;
    public GameObject endScreen;


    public TextMeshProUGUI caughtFruitText;
    public TextMeshProUGUI winningCaughtFruitText;
    public TextMeshProUGUI winningLostFruitText;
    public TextMeshProUGUI endLostFruitText;

    public float totalScore;

    public TextMeshProUGUI winTotalScoreText;
    public TextMeshProUGUI endTotalScoreText;

    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI finalGradeText;
    public TextMeshProUGUI endFinalScoreText;
    public TextMeshProUGUI endFinalGradeText;

    // Start is called before the first frame update
    void Start()
    {
        fruitCaughtTrackerScript = fruitTrackerObject.GetComponent<FruitCaughtTracker>();
        fruitSpawnerScript = fruitSpawnerObject.GetComponent<FruitSpawner>();
        timerScript = timerObject.GetComponent<TimerScript>();
        soundByteScript = soundByteObject.GetComponent<SoundByteScript>();
        floorHitbox = floorHitboxObject.GetComponent<FloorHitbox>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timerScript.timeLeft <= 0)
        {
            FruitRainWinScreen();
        
        }

        if (floorHitbox.fruitLost >= 5)
        {

            FruitRainEndScreen();
        
        }

        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    FruitRainEndScreen();

        //}
    }

    public void FruitRainEndScreen()
    {
        totalScore = 0f;
        endScreen.SetActive(true);
        StopGameFunctions();
        caughtFruitText.text = "Caught Fruit: " + fruitCaughtTrackerScript.fruitCaught;
        endLostFruitText.text = "Lost Fruit: " + floorHitbox.fruitLost;
        endTotalScoreText.text = "Total Score: " + totalScore;
        endFinalScoreText.text = "Final Score : 0 " ;
        endFinalGradeText.text = "Final Grade : F " ;

    }

    public void FruitRainWinScreen()
    {
        TotalScoreCalculator();
        StopGameFunctions();
        winScreen.SetActive(true);
        winningCaughtFruitText.text = "Caught Fruit: " + fruitCaughtTrackerScript.fruitCaught;
        winningLostFruitText.text = "Lost Fruit: " + floorHitbox.fruitLost;
        winTotalScoreText.text = "Total Score: " + totalScore;

    }

    public void StopGameFunctions()
    {

        fruitSpawnerScript.StopSpawningFruit();  

        floorHitbox.floorGameEnd = true;

        timerScript.timerOn = false;

        fruitCaughtTrackerScript.fruitCaughtGameEnd = true;

        timerSoundScript.audioSource.Stop();



    }

    public void TotalScoreCalculator()
    {
        totalScore = (fruitCaughtTrackerScript.fruitCaught * 2.5f) - (floorHitbox.fruitLost * 5f);

        scoreManager.AddScore((int)totalScore);

        for (int i = 0; i < floorHitbox.fruitLost; i++)
        {
            scoreManager.RegisterMistake();
        }

        scoreManager.GetFinalGrade();

        var result = scoreManager.EndGame();

        finalScoreText.text = "Final Score : " + result.finalScore;
        finalGradeText.text = "Final Grade : " + result.finalGrade;
        



    }

}
