using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerScript : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI timerText; //setting the textmeshprougui variable
    [SerializeField] TextMeshProUGUI timerTextEnd; //setting the textmeshprougui variable
    [SerializeField] TextMeshProUGUI timerTextWin; //setting the textmeshprougui variable
    public float timeLeft;
    public bool timerOn = true;

    // Start is called before the first frame update
    void Start()
    {
        timeLeft = 30f;
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

            timerText.text = timeInMinuteFormat;
            timerTextEnd.text = timeInMinuteFormat;
            timerTextWin.text = timeInMinuteFormat;

            if (timeLeft <= 0f)
            {
                timeLeft = 0f;
                TurnTimerOff();
            
            }
        }


    }

    public void TurnTimerOff()
    {
        timerOn = false;
    }

}