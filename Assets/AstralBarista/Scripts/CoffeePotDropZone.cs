using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeePotDropZone : MonoBehaviour, IScoreTrigger
{
    [Header("Debug / Placeholder")]
    [Tooltip("If true, all drops are treated as correct until an OrderManager is wired up.")]
    public bool treatAllDropsAsCorrect = true;

    [Tooltip("Optional: simple sfx or particle on success/fail.")]
    public AudioSource sfxSource;
    public AudioClip successSfx;
    public AudioClip failSfx;

    public void ReceiveIngredient(DraggableIngredient ingredient, IngredientData data)
    {
        bool isCorrect = treatAllDropsAsCorrect;

        // In the next phase, replace this with: isCorrect = OrderManager.Instance.Accepts(data.type);
        if (isCorrect)
        {
            TriggerScore(data != null ? data.points : 10);
        }
        else
        {
            TriggerMistake();
        }

        if (sfxSource != null)
            sfxSource.PlayOneShot(isCorrect ? successSfx : failSfx);

        // Visual swallow / disappear
        Destroy(ingredient.gameObject);
    }

    public void ReceiveIngredient(IngredientData data)
    {
        bool isCorrect = treatAllDropsAsCorrect; // replace with your OrderManager check
        if (isCorrect) TriggerScore(data != null ? data.points : 10);
        else TriggerMistake();

        if (sfxSource != null)
            sfxSource.PlayOneShot(isCorrect ? successSfx : failSfx);
    }

    // --- IScoreTrigger (hooks into your Score system) ---
    public void TriggerScore()
    {
        TriggerScore(10);
    }

    public void TriggerMistake()
    {
        // ScoreManager.Instance.RegisterMistake();  // ready to wire
        ScoreManager.Instance.RegisterMistake();
    }

    public void TriggerScore(int points)
    {
        // ScoreManager.Instance.AddScore(points);    // ready to wire
        ScoreManager.Instance.AddScore(points);
    }
}
