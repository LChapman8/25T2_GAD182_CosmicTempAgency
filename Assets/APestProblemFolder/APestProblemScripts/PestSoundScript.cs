using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PestSoundScript : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip takeDamageSoundClip;
    public AudioClip deathSoundClip;



    public void PlayTakeDamageSound()
    {
        audioSource.clip = takeDamageSoundClip;
        audioSource.Play();

    }

    public void PlayDeathSound()
    {
        audioSource.clip = deathSoundClip;
        audioSource.Play();

    }

}
