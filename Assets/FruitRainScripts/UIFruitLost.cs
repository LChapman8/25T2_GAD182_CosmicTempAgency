using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIFruitLost : MonoBehaviour
{
    public FloorHitbox FruitLostTrackerScript;
    public TextMeshProUGUI fruitLostText;
    public int fruitLostValue;
    // Start is called before the first frame update
    void Start()
    {
        FruitLostTrackerScript = GameObject.Find("FloorHitbox").GetComponent<FloorHitbox>();
        fruitLostText = GameObject.Find("FruitLost").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        fruitLostText.text = " Fruit Lost : " + fruitLostValue;
        fruitLostValue = FruitLostTrackerScript.fruitLost;
    }
}
