using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartScript : MonoBehaviour
{
    public TimerSoundScript timerSoundScript;
    public GameObject StartPanel;

    public UIController uiController;
    public FruitTimerScript fruitTimerScript;
    [SerializeField] private float minigameTime = 21f; 
    [SerializeField] private int missLimit = 5;
    [SerializeField] private FruitSpawner spawner;

    void Start()
    {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        spawner.StopSpawningFruit();
        foreach (var f in GameObject.FindGameObjectsWithTag("Fruit"))
            Destroy(f);
    }

    public void StartButton()
    {
        StartPanel.SetActive(false);
        Time.timeScale = 1f;
        timerSoundScript.PlayTimerSoundClip();

        ScoreManager.Instance.ResetScore();
        uiController.InitUI(minigameTime, true, missLimit);
        uiController.mistakesLeftLabel.text = "Misses Left:";
        fruitTimerScript.Begin(minigameTime);

        StartCoroutine(FinaliseWhenTimeUp(minigameTime));
        spawner.StartSpawning();
    }

    private IEnumerator FinaliseWhenTimeUp(float t)
    {
        yield return new WaitForSecondsRealtime(t);

        // Halt spawning and the internal ramp timer
        spawner.StopSpawningFruit();
        fruitTimerScript.Stop();

        // Clear any fruit still falling/existing
        foreach (var f in GameObject.FindGameObjectsWithTag("Fruit"))
            Destroy(f);

        ScoreManager.Instance.FinaliseGame();
    }
}
