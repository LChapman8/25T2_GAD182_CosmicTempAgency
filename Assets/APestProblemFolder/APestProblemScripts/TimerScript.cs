using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerScript : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip timerSound;


    public GameStatisticsControllerScript gameStatisticsControllerScript;

    [SerializeField] TextMeshProUGUI timerText; //setting the textmeshprougui variable
    [SerializeField] TextMeshProUGUI timerTextEnd; //setting the textmeshprougui variable
    [SerializeField] TextMeshProUGUI timerTextWin; //setting the textmeshprougui variable
    public float timeAtStart = 21;
    public float elapsedTime;
    public bool timerOn = false;

    // Start is called before the first frame update
    void Start()
    {
        
        elapsedTime = timeAtStart;
        gameStatisticsControllerScript.timeLeft = elapsedTime;
    }

    public void PlayTimerSound()
    {
        audioSource.clip = timerSound;
        audioSource.Play();


    }

    // Update is called once per frame
    void Update()
    {
        if (timerOn)
        {
            elapsedTime -= Time.deltaTime;
            int minutes = Mathf.FloorToInt(elapsedTime / 60); // sets a variable for the time to be calculated into minutes
            int seconds = Mathf.FloorToInt(elapsedTime % 60); // sets a variable for the time to be calculated into seconds
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds); // using propper string formatting, this formats the time into minutes with seconds for the TextMeshPro UI
            timerTextEnd.text = string.Format("{0:00}:{1:00}", minutes, seconds); // using propper string formatting, this formats the time into minutes with seconds for the TextMeshPro UI
            timerTextWin.text = string.Format("{0:00}:{1:00}", minutes, seconds); // using propper string formatting, this formats the time into minutes with seconds for the TextMeshPro UI
        }

        if (elapsedTime <= 0)
        { 
            TurnTimerOff();
            timerText.text = "00:00";
            elapsedTime = 0;
            audioSource.Stop();
        }

        gameStatisticsControllerScript.timeLeft = elapsedTime;

    }

    public void TurnTimerOff()
    {
        timerOn = false;
        gameStatisticsControllerScript.timeLeft = elapsedTime;
        audioSource.Stop();

    }
}
