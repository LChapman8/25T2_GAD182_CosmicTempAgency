using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using TMPro;

public class UIFruitCaught : MonoBehaviour
{
    public FruitCaughtTracker FruitCaughtTrackerScript;
    public TextMeshProUGUI fruitCaughtText;
    public int fruitCaughtValue;
    // Start is called before the first frame update
    void Start()
    {
        FruitCaughtTrackerScript = GameObject.Find("BasketCaughtHitBox").GetComponent<FruitCaughtTracker>();
        fruitCaughtText = GameObject.Find("FruitCaught").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        fruitCaughtText.text = " Fruit Caught : " + fruitCaughtValue;
        fruitCaughtValue = FruitCaughtTrackerScript.fruitCaught;

        
    }
}
