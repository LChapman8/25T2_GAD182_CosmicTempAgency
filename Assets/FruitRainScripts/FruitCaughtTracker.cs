using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitCaughtTracker : MonoBehaviour
{
    public SoundByteScript soundByteScript;
    public GameObject soundByteScriptObject;
    public int fruitCaught;
    // Start is called before the first frame update
    void Start()
    {
       // soundByteScriptObject = GameObject.Find("SoundController");
       // soundByteScript = soundByteScriptObject.GetComponent<SoundByteScript>();
       // soundByteScript.PointSoundClip();
    }

   
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Fruit"))
        {
            fruitCaught += 1;
            collision.gameObject.GetComponent<FruitMechanics>().DestroySelf();
            soundByteScript.PointSoundClip();
        }
    }
}
