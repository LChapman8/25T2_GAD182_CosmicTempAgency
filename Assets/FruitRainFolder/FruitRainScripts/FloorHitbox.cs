using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorHitbox : MonoBehaviour
{
    [SerializeField] private int missLimit = 5;

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
        if (collision.gameObject.CompareTag("Fruit") && !floorGameEnd)
        {
            fruitLost += 1;
            ScoreManager.Instance.RegisterMistake();
            soundByteScript.LossPointSoundClip();
            collision.gameObject.GetComponent<FruitMechanics>().DestroySelf();

            if (fruitLost >= missLimit)
            {
                floorGameEnd = true;
                ScoreManager.Instance.FinaliseGame();
                FindObjectOfType<UIController>().EndGameUI();
            }
        }
    }
}
