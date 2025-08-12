using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaristaMinigameManager : MonoBehaviour
{
    [SerializeField] private UIController ui;
    [SerializeField] private float roundSeconds = 60f;

    [Header("Mistakes UI Mode")]
    [SerializeField] private bool limitMistakes = false;
    [SerializeField] private int maxMistakes = 3;

    private void Start()
    {
        // Identify this minigame for your score system
        ScoreManager.Instance.currentMinigame = MinigameID.Barista;

        // Reset score/mistakes visually and start timer UI
        ScoreManager.Instance.ResetScore();
        ui.InitUI(roundSeconds, limitMistakes, maxMistakes);
    }
}
