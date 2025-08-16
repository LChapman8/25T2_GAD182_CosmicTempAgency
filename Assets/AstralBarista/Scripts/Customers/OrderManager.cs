using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance { get; private set; }

    [SerializeField] private bool ordersEnabled = true;
    private bool _stoppedOnce;

    [Header("Aliens & Placement")]
    [SerializeField] private List<GameObject> alienPrefabs = new();  // assign your 8+ alien prefabs
    [SerializeField] private Transform alienSpawnPoint;               // where the alien appears
    [SerializeField] private Vector3 bubbleLocalOffset = new(0f, 1.5f, 0f);

    [Header("Bubble UI")]
    [SerializeField] private TMP_FontAsset alienFont;                 // your custom TMP font
    [SerializeField] private OrderBubbleUI bubbleUIPrefab;                // simple prefab w/ TextMeshPro
    [SerializeField] private bool createTextIfNoPrefab = true;        // fallback if prefab not set

    [Header("Ingredients / Mapping")]
    [Tooltip("Populate with the 8 IngredientData assets that correspond to Y,U,M,?,E,A,T,!.")]
    [SerializeField] private List<IngredientData> ingredientOptions = new();

    [Header("Flow")]
    [SerializeField] private float nextOrderDelay = 0.75f;            // small pause after success
    //[SerializeField] private int defaultPoints = 10;                  // used if IngredientData.points not set

    [Header("Order Timing")]
    [SerializeField] private float roundLengthSeconds = 60f;   // used to scale difficulty over the round
    [SerializeField] private bool scaleOrderTimeOverRound = true;
    [SerializeField] private float orderTimeAtStart = 3f;      // allowed time early in the round
    [SerializeField] private float orderTimeAtEnd = 0.5f;      // allowed time near the end

    [Header("Customer Reactions")]
    [SerializeField] private float flashPulseSeconds = 0.12f;
    [SerializeField] private int happyPulses = 2;
    [SerializeField] private int angryPulses = 1;
    [SerializeField] private float leaveFadeSeconds = 0.22f;

    private CustomerVisuals _currentVisuals;
    private bool _resolving;
    private float _roundStartTime = -1f;
    private Coroutine _orderTimerCo;
    private GameObject _currentAlien;
    private OrderBubbleUI _currentBubble;
    private IngredientData _currentIngredient;                        // the required ingredient for this order

    public IngredientType CurrentIngredientType => _currentIngredient ? _currentIngredient.type : default;
    public bool HasActiveOrder => _currentIngredient != null;

    void Awake()
    {
        if (Instance && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        SpawnNewOrder();
    }

    private float ComputeOrderTimeSeconds()
    {
        if (!scaleOrderTimeOverRound) return orderTimeAtStart;
        if (_roundStartTime < 0f) return orderTimeAtStart;

        float progress = Mathf.Clamp01((Time.time - _roundStartTime) / Mathf.Max(0.01f, roundLengthSeconds));
        return Mathf.Lerp(orderTimeAtStart, orderTimeAtEnd, progress);
    }

    private void StartOrderTimer()
    {
        CancelOrderTimer();
        float seconds = ComputeOrderTimeSeconds();
        _orderTimerCo = StartCoroutine(Co_OrderTimer(seconds));
        Debug.Log($"[Order] Timer started: {seconds:0.00}s");
    }

    private void CancelOrderTimer()
    {
        if (_orderTimerCo != null)
        {
            StopCoroutine(_orderTimerCo);
            _orderTimerCo = null;
        }
    }

    public void StartOrders()
    {
        ordersEnabled = true;
        _stoppedOnce = false;

        // If nothing is active, kick off a fresh order
        if (!HasActiveOrder && _orderTimerCo == null && !_resolving)
            SpawnNewOrder();
    }

    public void StopOrders()
    {
        if (_stoppedOnce) return;
        _stoppedOnce = true;
        ordersEnabled = false;

        // Cancel any drags still held (QoL)
        DragRegistry.CancelAllSilent();

        // Stop timers/effects and remove current customer + bubble
        CancelOrderTimer();
        StopAllCoroutines();
        _resolving = true;   // block Accepts() & further spawns
        ClearCurrent();      // destroys alien + bubble and clears state
    }

    public void SpawnNewOrder()
    {
        if (!ordersEnabled) return;

        ClearCurrent();

        _resolving = false;

        if (_roundStartTime < 0f) _roundStartTime = Time.time;

        // Pick a random alien
        if (alienPrefabs.Count == 0 || ingredientOptions.Count == 0)
        {
            Debug.LogWarning("[OrderManager] Missing alienPrefabs or ingredientOptions.");
            return;
        }

        var alienPrefab = alienPrefabs[Random.Range(0, alienPrefabs.Count)];
        _currentAlien = Instantiate(alienPrefab, alienSpawnPoint.position, Quaternion.identity, alienSpawnPoint.parent);
        _currentVisuals = _currentAlien.GetComponent<CustomerVisuals>();
        if (_currentVisuals == null) _currentVisuals = _currentAlien.AddComponent<CustomerVisuals>();

        // Pick a random ingredient (glyph)
        _currentIngredient = ingredientOptions[Random.Range(0, ingredientOptions.Count)];

        // Make bubble
        if (bubbleUIPrefab)
        {
            _currentBubble = Instantiate(bubbleUIPrefab, _currentAlien.transform);
            _currentBubble.transform.localPosition = bubbleLocalOffset;
            _currentBubble.SetGlyph(_currentIngredient.symbol, alienFont);
        }
        else if (createTextIfNoPrefab)
        {
            // Text-only fallback (no background)
            var go = new GameObject("OrderBubble_Fallback");
            go.transform.SetParent(_currentAlien.transform, false);
            go.transform.localPosition = bubbleLocalOffset;

            var text = go.AddComponent<TextMeshPro>();
            text.font = alienFont;
            text.alignment = TextAlignmentOptions.Center;
            text.fontSize = 6; // tweak to taste (world-space units)
            text.text = _currentIngredient.symbol;

            var r = text.GetComponent<Renderer>();
            if (r) r.sortingOrder = 200;
        }

        Debug.Log($"[Order] New alien spawned. Wants: '{_currentIngredient.symbol}' ({_currentIngredient.type})");

        StartOrderTimer();
    }

    public IngredientData CurrentIngredientData => _currentIngredient;

    public bool Accepts(IngredientData dropped)
    {
        if (_resolving || !HasActiveOrder || dropped == null) return false;

        if (ReferenceEquals(dropped, _currentIngredient)) return true;
        if (!string.IsNullOrEmpty(dropped.symbol) && dropped.symbol == _currentIngredient.symbol) return true;
        return dropped.type == _currentIngredient.type;
    }


    public void OnCorrectDrop(int points)
    {
        if (!ordersEnabled || _resolving) return;
        _resolving = true;
        CancelOrderTimer();

        Debug.Log($"[Order] Correct. (+{points})");
        StartCoroutine(Co_HappyThenNext());
    }

    public void OnWrongDrop()
    {
        if (!ordersEnabled || _resolving) return;
        _resolving = true;
        CancelOrderTimer();

        Debug.Log("[Order] Wrong. (+1 mistake) Customer leaving.");
        StartCoroutine(Co_AngryThenReplace());
    }

    private IEnumerator Co_HappyThenNext()
    {
        if (_currentVisuals)
            yield return _currentVisuals.Flash(Color.green, happyPulses, flashPulseSeconds);

        if (_currentVisuals && leaveFadeSeconds > 0f)
            yield return _currentVisuals.FadeOut(leaveFadeSeconds);

        ClearCurrent();
        yield return new WaitForSeconds(nextOrderDelay);
        if (ordersEnabled) SpawnNewOrder();
        _resolving = false;
    }
    private System.Collections.IEnumerator Co_OrderTimer(float seconds)
    {
        // Wait until the new order is actually active
        while (_resolving) yield return null;

        float deadline = Time.time + seconds;
        while (Time.time < deadline)
        {
            if (!ordersEnabled || _resolving) yield break; // order got resolved by drop or forced clear
            yield return null;
        }

        // Timed out
        _orderTimerCo = null;

        if (!ordersEnabled || _resolving) yield break; // safety

        _resolving = true;

        DragRegistry.CancelAllSilent();

        Debug.Log($"[Order] Timed out after {seconds:0.00}s. (+1 mistake)");
        ScoreManager.Instance.RegisterMistake();

        if (_currentVisuals) yield return _currentVisuals.Flash(Color.red, angryPulses, flashPulseSeconds);
        if (_currentVisuals && leaveFadeSeconds > 0f) yield return _currentVisuals.FadeOut(leaveFadeSeconds);

        ClearCurrent();
        yield return new WaitForSeconds(0.05f);

        if (ordersEnabled) SpawnNewOrder();
        _resolving = false;
    }

    private IEnumerator Co_AngryThenReplace()
    {
        if (_currentVisuals)
            yield return _currentVisuals.Flash(Color.red, angryPulses, flashPulseSeconds);

        if (_currentVisuals && leaveFadeSeconds > 0f)
            yield return _currentVisuals.FadeOut(leaveFadeSeconds);

        ClearCurrent();
        yield return new WaitForSeconds(0.05f);

        if (ordersEnabled) SpawnNewOrder();
        _resolving = false;
    }

    private void ClearCurrent()
    {
        CancelOrderTimer();

        if (_currentBubble) Destroy(_currentBubble.gameObject);
        if (_currentAlien) Destroy(_currentAlien);
        _currentBubble = null;
        _currentAlien = null;
        _currentIngredient = null;
        _currentVisuals = null;
    }

    public string CurrentDebug => _currentIngredient
    ? $"'{_currentIngredient.symbol}' ({_currentIngredient.type})"
    : "<none>";
}
