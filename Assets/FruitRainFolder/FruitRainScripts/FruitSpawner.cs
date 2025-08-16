using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSpawner : MonoBehaviour
{

    [SerializeField] GameObject[] fruitPrefab;
    Coroutine fruitSpawnerCoroutine;
    public float secondsBetweenSpawn = 1.2f, minSpawnArea, maxSpawnArea;//setting the variables for seconds between fruit spawning and the area range.

    public void StartSpawning()
    {
        if (fruitSpawnerCoroutine == null)
            fruitSpawnerCoroutine = StartCoroutine(FruitSpawn());
    }

    public IEnumerator FruitSpawn()
    {
        while (true)
        {
            // Wait first so nothing spawns during the start screen (respects Time.timeScale)
            yield return new WaitForSeconds(secondsBetweenSpawn);

            float range = Random.Range(minSpawnArea, maxSpawnArea);
            Vector2 position = new(range, transform.position.y);
            GameObject go = Instantiate(fruitPrefab[Random.Range(0, fruitPrefab.Length)], position, Quaternion.identity);
            Destroy(go, 5f); // use scaled time
        }
    }

    public void StopSpawningFruit()
    {
        if (fruitSpawnerCoroutine != null)
        {
            StopCoroutine(fruitSpawnerCoroutine);
            fruitSpawnerCoroutine = null;
        }
    }
}
