using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public float startTime = 30f;
    private float currentTime;
    public TextMeshProUGUI timerText;
    private bool isRunning = true;

    void Start()
    {
        currentTime = startTime;
        UpdateTimerDisplay();
    }

    void Update()
    {
        if (!isRunning) return;

        currentTime -= Time.deltaTime;
        if (currentTime <= 0)
        {
            currentTime = 0;
            isRunning = false;

            
            FindObjectOfType<GameManager>().GameOver(false);
        }

        UpdateTimerDisplay();
    }

    void UpdateTimerDisplay()
    {
        int seconds = Mathf.CeilToInt(currentTime);
        timerText.text = seconds.ToString();
    }
}
