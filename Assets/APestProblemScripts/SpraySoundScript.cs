using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpraySoundScript : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip spraySoundClip;
   

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySpraySound()
    {
        audioSource.clip = spraySoundClip;
        audioSource.Play();

    }

}
