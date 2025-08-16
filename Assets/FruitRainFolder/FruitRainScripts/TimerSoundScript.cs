using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerSoundScript : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip timerSoundClip;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
 
    }


    public void PlayTimerSoundClip()
    {

        Debug.Log("Play Timer Sound Clip");
        audioSource.clip = timerSoundClip;
        audioSource.Play();

    }


}
