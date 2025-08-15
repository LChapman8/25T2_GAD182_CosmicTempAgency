using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartScript : MonoBehaviour
{
    public TimerSoundScript timerSoundScript;
    public GameObject StartPanel;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartButton()
    { 
    
        StartPanel.SetActive(false);
        Time.timeScale = 1f;
        timerSoundScript.PlayTimerSoundClip();
    }
}
