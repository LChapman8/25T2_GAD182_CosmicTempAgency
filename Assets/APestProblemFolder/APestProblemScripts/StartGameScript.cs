using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class StartGameScript : MonoBehaviour
{
    public TimerScript timerScript;
    public GameObject StartPanel;
    public UIController uiController;
    public GameStatisticsControllerScript gameStats;
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

        // Fresh session every time
        ScoreManager.Instance.ResetScore();

        // Enable "mistakes left" mode
        uiController.InitUI(timerScript.timeAtStart, true, gameStats.pestsAlive);
        uiController.mistakesLeftLabel.text = "Pests Left:";

        timerScript.timerOn = true;
        timerScript.PlayTimerSound();
    }
}
