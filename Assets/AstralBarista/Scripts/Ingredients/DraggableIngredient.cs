using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class DraggableIngredient : MonoBehaviour, IActiveDrag
{
    [Header("Tuning")]
    [SerializeField] private float zDepth = 0f;
    [SerializeField] private Vector3 cursorOffset = Vector3.zero;

    private IngredientData data;
    private Camera worldCam;
    private bool isDragging;
    private bool hasDropped;
    private bool _consumed;


    private Rigidbody2D rb;
    private Collider2D col;

    private CoffeePotDropZone currentDropZone;

    public IngredientType Type => data != null ? data.type : default;

    public void Init(IngredientData ingredientData, Camera cam)
    {
        data = ingredientData;
        worldCam = cam;
        BeginDrag();
        DragRegistry.Register(this);
    }

    private void OnDestroy()
    {
        DragRegistry.Unregister(this);
    }

    public void CancelDragSilent()
    {
        Destroy(gameObject);
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        rb.isKinematic = true;
        rb.gravityScale = 0f;
    }

    private void BeginDrag()
    {
        isDragging = true;
        FollowMouseImmediate();
    }

    private void Update()
    {
        if (hasDropped) return;

        if (isDragging && Input.GetMouseButton(0))
        {
            FollowMouseImmediate();
            return;
        }

        // Left mouse released — perform drop
        if (isDragging && !Input.GetMouseButton(0))
        {
            isDragging = false;
            Drop();
        }
    }

    private void FollowMouseImmediate()
    {
        Vector3 screenPos = Input.mousePosition;
        screenPos.z = Mathf.Abs(worldCam.transform.position.z) + zDepth;
        Vector3 worldPos = worldCam.ScreenToWorldPoint(screenPos);
        transform.position = worldPos + cursorOffset;
    }

    private void Drop()
    {
        hasDropped = true;

        if (currentDropZone != null)
        {
            currentDropZone.ReceiveIngredient(this, data);
            // Pot destroys this object
        }
        else
        {
            // Not dropped in pot, simply remove it
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_consumed) return;

        var pot = other.GetComponent<CoffeePotDropZone>();
        if (pot == null) return;

        _consumed = true;

        // Hand off to pot; it will score/flash
        pot.ReceiveIngredient(this, data);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var pot = other.GetComponent<CoffeePotDropZone>();
        if (pot != null && currentDropZone == pot)
        {
            pot.GetComponent<CoffeePotHighlight>()?.SetHighlight(false);
            currentDropZone = null;
        }
    }
}
