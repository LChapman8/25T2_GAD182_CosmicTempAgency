using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerScript : MonoBehaviour
{
    public FruitSpawner spawner;

    public bool firstPhase = true;
    public bool secondPhase = true;
    public bool thirdPhase = true;

    public TextMeshProUGUI timerText; //setting the textmeshprougui variable
    [SerializeField] TextMeshProUGUI timerTextEnd; //setting the textmeshprougui variable
    [SerializeField] TextMeshProUGUI timerTextWin; //setting the textmeshprougui variable
    public float timeLeft;
    public bool timerOn = true;

    // Start is called before the first frame update
    void Start()
    {
        timeLeft = 21f;
        firstPhase = true;
        secondPhase = true;
        thirdPhase = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (timerOn)
        {
            timeLeft -= Time.deltaTime;

            float restrainedTime = Mathf.Max(timeLeft, 0f);
            int minutes = Mathf.FloorToInt(restrainedTime / 60); // sets a variable for the time to be calculated into minutes
            int seconds = Mathf.FloorToInt(restrainedTime % 60); // sets a variable for the time to be calculated into seconds
            string timeInMinuteFormat = string.Format("{0:00}:{1:00}", minutes, seconds); // using propper string formatting, this formats the time into minutes with seconds for the TextMeshPro UI

            timerText.text = "Timer : " + timeInMinuteFormat;

            if (timerTextEnd != null)
            {
                timerTextEnd.text = "End Time: " + timeInMinuteFormat;
            }

            if (timerTextWin != null)
            {
                timerTextWin.text = "Winning Time : " + timeInMinuteFormat;

            }

            if (timeLeft <= 0f)
            {
                timeLeft = 0f;
                TurnTimerOff();
            
            }
        }
        //progressive spawn rate
        if (timeLeft <= 15 && firstPhase == true)
        {
            spawner.secondsBetweenSpawn -= 0.2f;
            firstPhase = false;
            Debug.Log("Spawn rate should be increased");
        }

        if (timeLeft <= 10 && secondPhase == true)
        {
            spawner.secondsBetweenSpawn -= 0.2f;
            secondPhase = false;
            Debug.Log("Spawn rate should be increased");
        }
        if (timeLeft <= 5 && thirdPhase == true)
        {
            spawner.secondsBetweenSpawn -= 0.2f;
            thirdPhase = false;
            Debug.Log("Spawn rate should be increased");
        }

    }

    public void TurnTimerOff()
    {
        timerOn = false;
    }

}