using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class IngredientButtonUI : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private IngredientData data;
    [SerializeField] private TextMeshProUGUI symbolText;
    [SerializeField] private Canvas dragCanvas;
    [SerializeField] private Camera worldCamera;
    [SerializeField] private UIDragIngredient uiDragPrefab;
    [SerializeField] private Sprite dragIconOverride;

    private static bool s_InputEnabled = true;
    public static void SetGlobalInputEnabled(bool enabled) => s_InputEnabled = enabled;

    void Awake()
    {
        if (symbolText && data) symbolText.text = data.symbol;
        if (!worldCamera) worldCamera = Camera.main;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!s_InputEnabled) return;
        if (eventData.button != PointerEventData.InputButton.Left) return;
        if (!data || !uiDragPrefab || !dragCanvas) return;

        var ghost = Instantiate(uiDragPrefab, dragCanvas.transform);

        Sprite iconSprite = data.dragIcon;
        if (!iconSprite && data.draggablePrefab)
        {
            var sr = data.draggablePrefab.GetComponent<SpriteRenderer>();
            if (sr) iconSprite = sr.sprite;
        }
        if (!iconSprite && dragIconOverride) iconSprite = dragIconOverride;

        ghost.Init(data, dragCanvas, worldCamera, iconSprite);
    }
}