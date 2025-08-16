using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PestHealthScript : MonoBehaviour
{
    public Pest2AnimationScript pest2AnimationScript;
    public PestHealthbarScript pestHealthbarScript;
    public PestSoundScript pestSoundScript;
    public GameStatisticsControllerScript gameStatisticsControllerScript;

    public int maxHealth = 5;
    public bool dead = false;
    public int currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        pestHealthbarScript.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            if (!dead && gameStatisticsControllerScript.gameEnded == false)
            {
                pest2AnimationScript.PlayDeathPest2Animation();
                dead = true;

                //pest death sound.
                pestSoundScript.PlayDeathSound();

                gameStatisticsControllerScript.RegisterPestRemoved();
            }
        }

        
    }

    public void TakeDamage(int damage)
    {
        if (!dead && gameStatisticsControllerScript.gameEnded == false)
        {

            currentHealth -= damage;

            pestHealthbarScript.SetCurrentHealth(currentHealth);

            //pest take damage sound.
            pestSoundScript.PlayTakeDamageSound();
        }
    }
}
