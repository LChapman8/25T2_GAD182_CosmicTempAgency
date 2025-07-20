using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSpawner : MonoBehaviour
{

    [SerializeField] GameObject[] fruitPrefab;
    Coroutine fruitSpawnerCoroutine;
    [SerializeField] float secondsBetweenSpawn = 1.2f, minSpawnArea, maxSpawnArea;//setting the variables for seconds between fruit spawning and the area range.
    // Start is called before the first frame update
    void Start()
    {
        fruitSpawnerCoroutine = StartCoroutine(FruitSpawn());
        
    }

    public IEnumerator FruitSpawn()
    {
        while (true)//an infinite loop unless broken
        {
            var range = Random.Range(minSpawnArea, maxSpawnArea);//chooses a random number between 2 variables
            var position = new Vector2(range, transform.position.y);//selects a position using the random variable and the y axis of the object the script is attached to.
            GameObject gameObject = Instantiate(fruitPrefab[Random.Range(0, fruitPrefab.Length)], position, Quaternion.identity);//Instantiate a fruit from the list at the randomly created position.
            yield return new WaitForSeconds(secondsBetweenSpawn);//wait for the secondsBetweenSpawn variable before restarting the loop
            Destroy(gameObject, 5f);//destroys the t object after 5 seconds.
            
        
        }
    
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void StopSpawningFruit()
    { 
    
        StopCoroutine(fruitSpawnerCoroutine);
    
    }
}
