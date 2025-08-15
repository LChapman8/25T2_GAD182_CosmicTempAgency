using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FruitTimerScript : MonoBehaviour
{
    public FruitSpawner spawner;

    public bool firstPhase = true;
    public bool secondPhase = true;
    public bool thirdPhase = true;

    public TextMeshProUGUI fruitTimerText; //setting the textmeshprougui variable
    [SerializeField] TextMeshProUGUI fruitTimerTextEnd; //setting the textmeshprougui variable
    [SerializeField] TextMeshProUGUI fruitTimerTextWin; //setting the textmeshprougui variable
    public float fruitTimeLeft;
    public bool fruitTimerOn = true;

    // Start is called before the first frame update
    void Start()
    {
        fruitTimeLeft = 21f;
        firstPhase = true;
        secondPhase = true;
        thirdPhase = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (fruitTimerOn)
        {
            fruitTimeLeft -= Time.deltaTime;

            float restrainedTime = Mathf.Max(fruitTimeLeft, 0f);
            int minutes = Mathf.FloorToInt(restrainedTime / 60); // sets a variable for the time to be calculated into minutes
            int seconds = Mathf.FloorToInt(restrainedTime % 60); // sets a variable for the time to be calculated into seconds
            string timeInMinuteFormat = string.Format("{0:00}:{1:00}", minutes, seconds); // using propper string formatting, this formats the time into minutes with seconds for the TextMeshPro UI

            fruitTimerText.text = "Timer : " + timeInMinuteFormat;

            if (fruitTimerTextEnd != null)
            {
                fruitTimerTextEnd.text = "End Time: " + timeInMinuteFormat;
            }

            if (fruitTimerTextWin != null)
            {
                fruitTimerTextWin.text = "Winning Time : " + timeInMinuteFormat;

            }

            if (fruitTimeLeft <= 0f)
            {
                fruitTimeLeft = 0f;
                TurnFruitTimerOff();
            
            }
        }
        //progressive spawn rate
        if (fruitTimeLeft <= 15 && firstPhase == true)
        {
            spawner.secondsBetweenSpawn -= 0.2f;
            firstPhase = false;
            Debug.Log("Spawn rate should be increased");
        }

        if (fruitTimeLeft <= 10 && secondPhase == true)
        {
            spawner.secondsBetweenSpawn -= 0.2f;
            secondPhase = false;
            Debug.Log("Spawn rate should be increased");
        }
        if (fruitTimeLeft <= 5 && thirdPhase == true)
        {
            spawner.secondsBetweenSpawn -= 0.2f;
            thirdPhase = false;
            Debug.Log("Spawn rate should be increased");
        }

    }

    public void TurnFruitTimerOff()
    {
        fruitTimerOn = false;
    }

}