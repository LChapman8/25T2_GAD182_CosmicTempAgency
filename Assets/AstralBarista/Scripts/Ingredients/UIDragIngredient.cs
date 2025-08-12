using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIDragIngredient : MonoBehaviour
{
    [SerializeField] private Image icon;
    private IngredientData data;
    private RectTransform rt;
    private Canvas dragCanvas;
    private Camera worldCam;

    public void Init(IngredientData ingredient, Canvas dragCanvasRef, Camera worldCamera, Sprite iconSprite)
    {
        data = ingredient;
        dragCanvas = dragCanvasRef;
        worldCam = worldCamera;
        rt = GetComponent<RectTransform>();
        if (icon == null) icon = GetComponent<Image>();
        if (iconSprite) icon.sprite = iconSprite;
        FollowMouse();
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            FollowMouse();
        }
        else
        {
            // Drop
            Vector3 screenPos = Input.mousePosition;
            Vector3 worldPos = worldCam.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, Mathf.Abs(worldCam.transform.position.z)));
            var hits = Physics2D.OverlapPointAll(new Vector2(worldPos.x, worldPos.y));

            CoffeePotDropZone pot = null;
            foreach (var h in hits)
            {
                pot = h.GetComponent<CoffeePotDropZone>();
                if (pot) break;
            }

            if (pot != null)
            {
                // Overload on your pot so UI can hand it off without a world object
                pot.ReceiveIngredient(data);
            }
            else
            {
                // Optional: count a mistake here
                // ScoreManager.Instance.RegisterMistake();
            }

            Destroy(gameObject);
        }
    }

    private void FollowMouse()
    {
        Vector2 localPoint;
        RectTransform canvasRT = (RectTransform)dragCanvas.transform;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRT, Input.mousePosition, dragCanvas.worldCamera, out localPoint);
        rt.anchoredPosition = localPoint;
    }
}
