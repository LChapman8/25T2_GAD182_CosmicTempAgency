using UnityEngine;

public class VegetableManager : MonoBehaviour
{
    public VegetableData[] vegetables;
    public Transform spawnPoint; // Used for whole veggie spawn
    public Transform[] chopSpawnPoints; // Set in Inspector — 8 locations for chopped veggies

    private GameObject currentVegetableObj;
    private VegetableData currentData;
    private int currentCuts = 0;
    private ChopManager chopManager;

    public void StartGame(ChopManager chopMgr)
    {
        chopManager = chopMgr;
        SpawnNextVegetable();
    }

    public void RegisterChop()
    {
        if (currentData == null) return;

        currentCuts++;
        if (currentCuts >= currentData.cutsRequired)
        {
            FinishVegetable();
        }
    }

    void FinishVegetable()
    {
        Destroy(currentVegetableObj);

        // Pick random spawn point for chopped prefab
        int randomIndex = Random.Range(0, chopSpawnPoints.Length);
        Transform randomSpawn = chopSpawnPoints[randomIndex];

        Instantiate(currentData.cutPrefab, randomSpawn.position, Quaternion.identity);

        AudioManager.Instance.PlayFinishChop();

        chopManager.IncreaseScore(currentData.scoreValue);
        SpawnNextVegetable();
    }

    void SpawnNextVegetable()
    {
        currentData = vegetables[Random.Range(0, vegetables.Length)];
        currentVegetableObj = Instantiate(currentData.wholePrefab, spawnPoint.position, Quaternion.identity);
        currentCuts = 0;
    }
}
