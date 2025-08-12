using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance { get; private set; }

    [Header("Aliens & Placement")]
    [SerializeField] private List<GameObject> alienPrefabs = new();  // assign your 8+ alien prefabs
    [SerializeField] private Transform alienSpawnPoint;               // where the alien appears
    [SerializeField] private Vector3 bubbleLocalOffset = new(0f, 1.5f, 0f);

    [Header("Bubble UI")]
    [SerializeField] private TMP_FontAsset alienFont;                 // your custom TMP font
    [SerializeField] private OrderBubble bubblePrefab;                // simple prefab w/ TextMeshPro
    [SerializeField] private bool createTextIfNoPrefab = true;        // fallback if prefab not set

    [Header("Ingredients / Mapping")]
    [Tooltip("Populate with the 8 IngredientData assets that correspond to Y,U,M,?,E,A,T,!.")]
    [SerializeField] private List<IngredientData> ingredientOptions = new();

    [Header("Flow")]
    [SerializeField] private float nextOrderDelay = 0.75f;            // small pause after success
    [SerializeField] private int defaultPoints = 10;                  // used if IngredientData.points not set

    // Runtime state
    private GameObject _currentAlien;
    private OrderBubble _currentBubble;
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

    public void SpawnNewOrder()
    {
        ClearCurrent();

        // Pick a random alien
        if (alienPrefabs.Count == 0 || ingredientOptions.Count == 0)
        {
            Debug.LogWarning("[OrderManager] Missing alienPrefabs or ingredientOptions.");
            return;
        }

        var alienPrefab = alienPrefabs[Random.Range(0, alienPrefabs.Count)];
        _currentAlien = Instantiate(alienPrefab, alienSpawnPoint.position, Quaternion.identity, alienSpawnPoint.parent);

        // Pick a random ingredient (glyph)
        _currentIngredient = ingredientOptions[Random.Range(0, ingredientOptions.Count)];

        // Make bubble
        if (bubblePrefab)
        {
            _currentBubble = Instantiate(bubblePrefab, _currentAlien.transform);
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
    }

    public IngredientData CurrentIngredientData => _currentIngredient;

    public bool Accepts(IngredientData dropped)
    {
        if (!HasActiveOrder || dropped == null) return false;

        // 1) Exact asset match (best)
        if (ReferenceEquals(dropped, _currentIngredient)) return true;

        // 2) Symbol match
        if (!string.IsNullOrEmpty(dropped.symbol) && !string.IsNullOrEmpty(_currentIngredient.symbol))
            if (dropped.symbol == _currentIngredient.symbol) return true;

        // 3) Enum fallback
        return dropped.type == _currentIngredient.type;
    }

    public void OnCorrectDrop(int points)
    {
        int p = points > 0 ? points : defaultPoints;
        Debug.Log($"[Order] Correct! +{p} points.");
        ScoreManager.Instance.AddScore(p);

        // Advance to next order
        Invoke(nameof(SpawnNewOrder), nextOrderDelay);
        // You might play a happy animation here later
    }

    public void OnWrongDrop()
    {
        Debug.Log("[Order] Wrong ingredient. +1 mistake.");
        ScoreManager.Instance.RegisterMistake();
        // Keep the same order until correct (or you can time out later)
    }

    private void ClearCurrent()
    {
        if (_currentBubble) Destroy(_currentBubble.gameObject);
        if (_currentAlien) Destroy(_currentAlien);
        _currentBubble = null;
        _currentAlien = null;
        _currentIngredient = null;
    }

    public string CurrentDebug => _currentIngredient
    ? $"'{_currentIngredient.symbol}' ({_currentIngredient.type})"
    : "<none>";
}
