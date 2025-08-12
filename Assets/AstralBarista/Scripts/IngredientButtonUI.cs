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
    [SerializeField] private Canvas dragCanvas;      // DragCanvas
    [SerializeField] private Camera worldCamera;     // Main Camera
    [SerializeField] private UIDragIngredient uiDragPrefab;
    [SerializeField] private Sprite dragIconOverride; // optional; defaults to prefab/Image sprite

    private void Awake()
    {
        if (symbolText && data) symbolText.text = data.symbol;
        if (!worldCamera) worldCamera = Camera.main;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        if (!data || !uiDragPrefab || !dragCanvas) return;

        var ghost = Instantiate(uiDragPrefab, dragCanvas.transform);

        // Choose the icon for the UI ghost:
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