using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameScript : MonoBehaviour
{
    public TimerScript timerScript;
    public GameObject StartPanel;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void StartGame()
    {

        StartPanel.SetActive(false);
        Time.timeScale = 1f;
        timerScript.timerOn = true;
        timerScript.PlayTimerSound();

    }
}
