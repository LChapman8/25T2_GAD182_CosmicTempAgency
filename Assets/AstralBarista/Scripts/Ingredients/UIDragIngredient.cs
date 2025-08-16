using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIDragIngredient : MonoBehaviour, IActiveDrag
{
    [SerializeField] private Image icon;
    private IngredientData data;
    private RectTransform rt;
    private Canvas dragCanvas;
    private Camera worldCam;
    private CoffeePotDropZone _hoverPot;
    private bool _consumed;

    public void Init(IngredientData ingredient, Canvas dragCanvasRef, Camera worldCamera, Sprite iconSprite)
    {
        data = ingredient;
        dragCanvas = dragCanvasRef;
        worldCam = worldCamera ? worldCamera : Camera.main;
        rt = GetComponent<RectTransform>();
        if (!icon) icon = GetComponent<Image>();
        if (iconSprite) icon.sprite = iconSprite;

        // QoL: make the ghost play nice with UI
        if (icon)
        {
            icon.preserveAspect = true;
            icon.raycastTarget = false;
        }

        FollowMouse();
        DragRegistry.Register(this);
    }

    private void OnDestroy()
    {
        // Make sure any hover highlight is cleared
        if (_hoverPot) _hoverPot.GetComponent<CoffeePotHighlight>()?.SetHighlight(false);
        _hoverPot = null;

        DragRegistry.Unregister(this);
    }

    public void CancelDragSilent()
    {
        if (_hoverPot) _hoverPot.GetComponent<CoffeePotHighlight>()?.SetHighlight(false);
        _hoverPot = null;
        Destroy(gameObject);
    }

    void Update()
    {
        {
            if (_consumed) return;

            // While holding LMB: follow mouse; if over pot -> auto-drop
            if (Input.GetMouseButton(0))
            {
                FollowMouse();

                var pot = DetectPotUnderCursor();
                if (pot != null)
                {
                    DropOnPot(pot);
                    return;
                }
                return;
            }

            // LMB released not over pot: just cancel (or count a mistake if desired)
            CancelDragSilent();
        }
    }

    private void DropOnPot(CoffeePotDropZone pot)
    {
        if (_consumed) return;
        _consumed = true;

        // Let the pot handle correctness, scoring, and flash
        pot.ReceiveIngredient(data);

        Destroy(gameObject);
    }

    private CoffeePotDropZone DetectPotUnderCursor()
    {
        Vector3 sp = Input.mousePosition;
        float z = Mathf.Abs(worldCam.transform.position.z);
        Vector3 wp = worldCam.ScreenToWorldPoint(new Vector3(sp.x, sp.y, z));
        var hits = Physics2D.OverlapPointAll(new Vector2(wp.x, wp.y));
        foreach (var h in hits)
        {
            var pot = h.GetComponent<CoffeePotDropZone>();
            if (pot) return pot;
        }
        return null;
    }

    private void FollowMouse()
    {
        RectTransform canvasRT = (RectTransform)dragCanvas.transform;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRT, Input.mousePosition, dragCanvas.worldCamera, out var localPoint);
        rt.anchoredPosition = localPoint;
    }
}
