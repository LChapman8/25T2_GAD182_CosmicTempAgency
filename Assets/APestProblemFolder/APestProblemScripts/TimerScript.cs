using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerScript : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip timerSound;

    public GameStatisticsControllerScript gameStatisticsControllerScript;
    public TextMeshProUGUI timerText;

    public float timeAtStart = 60f;
    public float elapsedTime;
    public bool timerOn = false;

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

    void Update()
    {
        if (timerOn)
        {
            elapsedTime -= Time.deltaTime;

            int minutes = Mathf.FloorToInt(elapsedTime / 60);
            int seconds = Mathf.FloorToInt(elapsedTime % 60);
            timerText.text = $"{minutes:0}:{seconds:00}";
        }

        if (elapsedTime <= 0)
        {
            TurnTimerOff();
            timerText.text = "0:00";
            elapsedTime = 0;
        }

        gameStatisticsControllerScript.timeLeft = elapsedTime;
    }

    public void TurnTimerOff()
    {
        timerOn = false;
        audioSource.Stop();
    }
}
