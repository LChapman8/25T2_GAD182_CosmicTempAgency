using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PestHealthScript : MonoBehaviour
{
    public Pest2AnimationScript pest2AnimationScript;

    public float health = 5;
    public bool dead = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            if (!dead)
            {
                pest2AnimationScript.PlayDeathPest2Animation();
                dead = true;

            }
        }
    }
}
