using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwatterSprayHitboxcript : MonoBehaviour
{
    public PestHealthScript pestHealthScript;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnTriggerEnter(Collider collider)
    {

        if (collider.gameObject.CompareTag("Pest"))
        { 
        
            pestHealthScript = collider.gameObject.GetComponent<PestHealthScript>();
            pestHealthScript.health -= 1;
        

            Debug.Log("Should deal damage to pest.");
            //play dealt damage sound effect

        }


    }




}
