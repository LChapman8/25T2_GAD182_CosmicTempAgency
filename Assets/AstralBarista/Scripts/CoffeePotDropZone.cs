using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeePotDropZone : MonoBehaviour, IScoreTrigger
{
    [Header("Legacy/Debug")]
    public bool treatAllDropsAsCorrect = false; // now off; we use OrderManager instead

    [Header("SFX")]
    public AudioSource sfxSource;
    public AudioClip successSfx;
    public AudioClip failSfx;

    [SerializeField] private CoffeePotHighlight highlight;

    private void Awake()
    {
        if (!highlight) highlight = GetComponent<CoffeePotHighlight>();
    }

    // --- Called by world draggable path ---
    public void ReceiveIngredient(DraggableIngredient ingredient, IngredientData data)
    {
        HandleDrop(data);
        Destroy(ingredient.gameObject);
    }

    // --- Called by the UI-ghost path ---
    public void ReceiveIngredient(IngredientData data)
    {
        HandleDrop(data);
    }

    private void HandleDrop(IngredientData data)
    {
        bool isCorrect;

        if (treatAllDropsAsCorrect)
        {
            isCorrect = true;
        }
        else if (OrderManager.Instance == null || data == null)
        {
            Debug.LogWarning("[Pot] Missing OrderManager or data; treating as wrong.");
            isCorrect = false;
        }
        else
        {
            isCorrect = OrderManager.Instance.Accepts(data);
        }

        // Debug visibility
        string need = OrderManager.Instance ? OrderManager.Instance.CurrentDebug : "<no order>";
        string got = data ? $"'{data.symbol}' ({data.type})" : "<null>";
        Debug.Log($"[Pot] Drop: need {need} | got {got} => {(isCorrect ? "CORRECT" : "WRONG")}");

        if (isCorrect)
        {
            int pts = data != null ? data.points : 10;
            TriggerScore(pts);
            OrderManager.Instance?.OnCorrectDrop(pts);
            if (sfxSource) sfxSource.PlayOneShot(successSfx);
        }
        else
        {
            TriggerMistake();
            OrderManager.Instance?.OnWrongDrop();
            if (sfxSource) sfxSource.PlayOneShot(failSfx);
        }

        highlight?.KickFlash();
    }

    // --- IScoreTrigger hooks ---
    public void TriggerScore() => TriggerScore(10);

    public void TriggerMistake()
    {
        ScoreManager.Instance.RegisterMistake();
    }

    public void TriggerScore(int points)
    {
        ScoreManager.Instance.AddScore(points);
    }
}
