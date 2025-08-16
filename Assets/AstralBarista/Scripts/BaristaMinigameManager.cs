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

    [Header("Input Lock (parent CanvasGroup for all 8 buttons)")]
    [SerializeField] private CanvasGroup ingredientButtonsGroup;

    [Header("Optional: Root object that contains all gameplay UI/objects (not the background)")]
    [SerializeField] private GameObject gameplayRoot;

    private bool _roundRunning;
    private Coroutine _roundCo;

    void Awake()
    {
        // Ensure mouse works in menus
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    void Start()
    {
        // --- Scene loads into MENU state ---
        // Stop any eager OrderManager that might auto-spawn in Start
        OrderManager.Instance?.StopOrders();

        // Hide/lock gameplay until Play is pressed
        if (gameplayRoot) gameplayRoot.SetActive(false);
        SetIngredientButtonsInteractable(false);
        IngredientButtonUI.SetGlobalInputEnabled(false);
    }

    // Called by MenuUIController when Play is clicked
    public void BeginRound()
    {
        if (_roundRunning) return;
        _roundRunning = true;

        if (gameplayRoot) gameplayRoot.SetActive(true);

        // Reset and init UI/timer
        ScoreManager.Instance.currentMinigame = MinigameID.Barista;
        ScoreManager.Instance.ResetScore();
        ui.InitUI(roundSeconds, limitMistakes, maxMistakes);

        // Unlock player input on buttons
        SetIngredientButtonsInteractable(true);
        IngredientButtonUI.SetGlobalInputEnabled(true);

        // Start orders
        OrderManager.Instance?.StartOrders();

        // Start end-of-round clock
        if (_roundCo != null) StopCoroutine(_roundCo);
        _roundCo = StartCoroutine(RunRoundClock());
    }

    private System.Collections.IEnumerator RunRoundClock()
    {
        yield return new WaitForSeconds(roundSeconds);
        EndRound();
    }

    // Call when the round ends
    public void EndRound()
    {
        if (!_roundRunning) return;
        _roundRunning = false;

        OrderManager.Instance?.StopOrders();

        SetIngredientButtonsInteractable(false);
        IngredientButtonUI.SetGlobalInputEnabled(false);
    }

    private void SetIngredientButtonsInteractable(bool on)
    {
        if (!ingredientButtonsGroup) return;
        ingredientButtonsGroup.interactable = on;
        ingredientButtonsGroup.blocksRaycasts = on;
    }
}
