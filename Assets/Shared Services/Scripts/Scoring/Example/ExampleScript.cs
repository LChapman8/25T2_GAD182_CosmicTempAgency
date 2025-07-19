using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SExampleScript : MonoBehaviour, IScoreTrigger
{
    // The number of points awarded for catching a scoring item
    public int scoreValue = 100;

    /// <summary>
    /// This method is called when the player earns points in the minigame.
    /// It adds score to the shared ScoreManager.
    /// </summary>
    public void TriggerScore()
    {
        ScoreManager.Instance.AddScore(scoreValue);
    }

    /// <summary>
    /// This method is called when the player makes a mistake in the minigame.
    /// It registers a mistake in the shared ScoreManager.
    /// </summary>
    public void TriggerMistake()
    {
        ScoreManager.Instance.RegisterMistake();
    }

    /// <summary>
    /// Unity trigger callback for when an object enters the catcher's trigger zone.
    /// This checks the object's tag to determine if it's a good or rotten apple,
    /// and calls the appropriate score/mistake method.
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GoodApple"))
        {
            // Player caught a good apple – award points
            TriggerScore();
        }
        else if (other.CompareTag("RottenApple"))
        {
            // Player caught a bad apple – count as a mistake
            TriggerMistake();
        }
    }
}
