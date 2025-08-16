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

    // NEW: parent CanvasGroup that contains ALL 8 buttons (e.g., a parent of Left/Right columns)
    [Header("Input Lock")]
    [SerializeField] private CanvasGroup ingredientButtonsGroup;

    private bool _roundEnded;
    private Coroutine _roundCo;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    void Start()
    {
        ScoreManager.Instance.currentMinigame = MinigameID.Barista;
        ScoreManager.Instance.ResetScore();
        ui.InitUI(roundSeconds, limitMistakes, maxMistakes);

        // ensure buttons are enabled at start
        SetIngredientButtonsInteractable(true);
        IngredientButtonUI.SetGlobalInputEnabled(true);

        _roundCo = StartCoroutine(RunRoundClock());
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.F1)) // Press F1 to force end game
        {
            Debug.Log("[DEBUG] Forcing game end via F1 key.");
            EndRound();
        }
#endif
    }

    private System.Collections.IEnumerator RunRoundClock()
    {
        yield return new WaitForSeconds(roundSeconds);
        EndRound();
    }

    public void EndRound()
    {
        if (_roundEnded) return;
        _roundEnded = true;

        // 1) Stop customers immediately (cancels timers, clears current alien)
        OrderManager.Instance?.StopOrders();

        // 2) Disable ingredient input
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
