using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwatterSprayScript : MonoBehaviour
{
    public SpraySoundScript spraySoundScript;

    public SprayAnimationScript sprayAnimationScript;

    public SprayCooldownUIScript sprayCooldownUIScript;

    public GameObject sprayHitBox;
    public Transform sprayHitBoxLocation;
    public Transform sprayExitLocation;

    public GameStatisticsControllerScript gameStatisticsControllerScript;

    public float nextSprayTime = 0f;
    public float sprayCooldown = 0.4f;


    public GameObject sprayEffect;
    // Start is called before the first frame update
    void Start()
    {
        sprayCooldownUIScript.SetMaxCooldown(sprayCooldown);
    }

    // Update is called once per frame
    void Update()
    {
        sprayCooldownUIScript.SetCurrentCooldown(nextSprayTime - Time.time);
            
        if (Input.GetKeyDown(KeyCode.Mouse0) && Time.time >= nextSprayTime)   
        {

                SprayHitboxSpawn();
                nextSprayTime = Time.time + sprayCooldown;

                
            
        }
       
    }

    public void SprayHitboxSpawn()
    {
        if (gameStatisticsControllerScript.gameEnded == false)
        {
            //instantiate spray hitbox

            var currentSprayHitbox = Instantiate(sprayHitBox, sprayHitBoxLocation.position, Quaternion.identity);
            Destroy(currentSprayHitbox, 0.2f);

            //play spray sound effect
            spraySoundScript.PlaySpraySound();

            //instantiate particle effect

            var currentSprayEffect = Instantiate(sprayEffect, sprayExitLocation.position, sprayExitLocation.rotation);
            Destroy(currentSprayEffect, 1f);

            //play spray animation

            sprayAnimationScript.PlaySprayHeadAnimation();
        }
    }

}
