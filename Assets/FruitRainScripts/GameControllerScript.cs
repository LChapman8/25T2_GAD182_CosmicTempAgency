using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameControllerScript : MonoBehaviour
{
    public FruitCaughtTracker fruitCaughtTrackerScript;
    public GameObject fruitTrackerObject;

    public FruitSpawner fruitSpawnerScript;
    public GameObject fruitSpawnerObject;


    public TimerScript timerScript;
    public GameObject timerObject;

    public SoundByteScript soundByteScript;
    public GameObject soundByteObject;

    public FloorHitbox floorHitbox;
    public GameObject floorHitboxObject;

    public GameObject winScreen;
    public GameObject endScreen;

    public TextMeshProUGUI endTimeText;
    public TextMeshProUGUI caughtFruitText;

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
        endScreen.SetActive(true);
        StopGameFunctions();
        caughtFruitText.text = "Caught Fruit: " + fruitCaughtTrackerScript.fruitCaught;
    }

    public void FruitRainWinScreen()
    {
        StopGameFunctions();
        winScreen.SetActive(true);
        caughtFruitText.text = "Caught Fruit: " + fruitCaughtTrackerScript.fruitCaught;
    }

    public void StopGameFunctions()
    {

        fruitSpawnerScript.StopSpawningFruit();  

        floorHitbox.floorGameEnd = true;

        timerScript.timerOn = false;

        fruitCaughtTrackerScript.fruitCaughtGameEnd = false;



    }
}
