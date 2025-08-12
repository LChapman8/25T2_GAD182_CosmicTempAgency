using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Astral Barista/Ingredient Data")]
public class IngredientData : ScriptableObject
{
    [Header("Identity")]
    public IngredientType type;

    [Header("UI")]
    [TextArea] public string symbol;

    [Header("UI Drag Icon")]
    public Sprite dragIcon;

    [Header("Drag Prefab")]
    public DraggableIngredient draggablePrefab;

    [Header("Scoring")]
    public int points = 10; // default points for a correct drop
}
