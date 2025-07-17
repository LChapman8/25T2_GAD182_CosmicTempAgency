using UnityEngine;

[CreateAssetMenu(fileName = "NewVegetable", menuName = "CuttingGame/Vegetable")]
public class VegetableData : ScriptableObject
{
    public string vegetableName;
    public GameObject wholePrefab;
    public GameObject cutPrefab;
    public int cutsRequired;
    public int scoreValue;
}
