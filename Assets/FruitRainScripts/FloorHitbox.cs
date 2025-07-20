using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorHitbox : MonoBehaviour
{
    public int fruitLost;
    public SoundByteScript soundByteScript;
    public GameObject soundByteScriptObject;
    public bool floorGameEnd = false;
    // Start is called before the first frame update
    void Start()
    {
        soundByteScriptObject = GameObject.Find("SoundController");
        soundByteScript = soundByteScriptObject.GetComponent<SoundByteScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Fruit") && floorGameEnd == false)
        {
            fruitLost += 1;
            soundByteScript.LossPointSoundClip();
            collision.gameObject.GetComponent<FruitMechanics>().DestroySelf();
        }
    }
}
