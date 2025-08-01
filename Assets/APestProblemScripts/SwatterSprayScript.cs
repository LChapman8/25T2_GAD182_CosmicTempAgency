using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwatterSprayScript : MonoBehaviour
{
    public GameObject sprayHitBox;
    public Transform sprayHitBoxLocation;
    public Transform sprayExitLocation;

    public GameObject sprayEffect;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {

            SprayHitboxSpawn();


        }
    }

    public void SprayHitboxSpawn()
    {

        var currentSprayHitbox = Instantiate(sprayHitBox, sprayHitBoxLocation.position, Quaternion.identity);
        Destroy(currentSprayHitbox, 0.5f);

        //play spray sound effect


        //instantiate particle effect

        var currentSprayEffect = Instantiate(sprayEffect, sprayExitLocation.position, sprayExitLocation.rotation);
        Destroy(currentSprayEffect, 1f);
    }

}
