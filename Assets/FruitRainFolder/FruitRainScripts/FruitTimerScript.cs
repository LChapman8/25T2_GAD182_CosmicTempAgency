using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Stripped timer used ONLY to ramp FruitSpawner difficulty over time.
/// UI and end-of-round are handled elsewhere (UIController/ScoreManager).
/// </summary>
public class FruitTimerScript : MonoBehaviour
{
    [Header("Spawner to ramp")]
    public FruitSpawner spawner;

    [Header("Timing")]
    [SerializeField] private float durationSeconds = 21f; // default;
    [SerializeField] private bool isRunning = false;
    private float timeLeft;

    // Phase gates so each ramp applies once
    private bool phase15Applied = false;
    private bool phase10Applied = false;
    private bool phase05Applied = false;

    /// <summary>
    /// Start the internal countdown used for ramp thresholds.
    /// </summary>
    public void Begin(float totalSeconds)
    {
        durationSeconds = totalSeconds;
        timeLeft = durationSeconds;
        isRunning = true;

        phase15Applied = phase10Applied = phase05Applied = false;
    }

    /// <summary>Stops the internal countdown (no UI side-effects).</summary>
    public void Stop()
    {
        isRunning = false;
    }

    // Backwards-compatible alias if something still calls TurnFruitTimerOff()
    public void TurnFruitTimerOff() => Stop();

    private void Update()
    {
        if (!isRunning) return;

        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0f)
        {
            timeLeft = 0f;
            isRunning = false;
            return;
        }

        // Progressive spawn rate (applied once at the thresholds)
        if (timeLeft <= 15f && !phase15Applied)
        {
            spawner.secondsBetweenSpawn = Mathf.Max(0.05f, spawner.secondsBetweenSpawn - 0.2f);
            phase15Applied = true;
        }

        if (timeLeft <= 10f && !phase10Applied)
        {
            spawner.secondsBetweenSpawn = Mathf.Max(0.05f, spawner.secondsBetweenSpawn - 0.2f);
            phase10Applied = true;
        }

        if (timeLeft <= 5f && !phase05Applied)
        {
            spawner.secondsBetweenSpawn = Mathf.Max(0.05f, spawner.secondsBetweenSpawn - 0.2f);
            phase05Applied = true;
        }
    }
}