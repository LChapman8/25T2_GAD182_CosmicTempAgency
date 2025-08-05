using UnityEngine;

public class VegetableManager : MonoBehaviour
{
    public VegetableData[] vegetables;
    public Transform spawnPoint; 
    public Transform[] chopSpawnPoints; 

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
